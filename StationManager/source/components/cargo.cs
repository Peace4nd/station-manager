using Sandbox.ModAPI.Ingame;
using System;
using System.Linq;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary> 
    /// Kargo 
    /// </summary> 
    class Cargo
    {
        /// <summary> 
        /// Materialy 
        /// </summary> 
        private Block ingot = null;

        /// <summary> 
        /// Materialy 
        /// </summary> 
        private Dictionary<string, (double Amount, int Index)> ingotItems = new Dictionary<string, (double Amount, int Index)>();

        /// <summary> 
        /// Rudy 
        /// </summary> 
        private Block ore = null;

        /// <summary> 
        /// Rudy 
        /// </summary> 
        private Dictionary<string, (double Amount, int Index)> oreItems = new Dictionary<string, (double Amount, int Index)>();

        /// <summary> 
        /// Komponenty 
        /// </summary> 
        private Block component = null;

        /// <summary> 
        /// Komponenty 
        /// </summary> 
        private Dictionary<string, (double Amount, int Index)> componentItems = new Dictionary<string, (double Amount, int Index)>();

        /// <summary> 
        /// Led
        /// </summary> 
        private Block ice = null;

        /// <summary> 
        /// Vyhazovac 
        /// </summary> 
        private Block thrower = null;

        /// <summary>
        /// Seznam polozek na vyhazovani
        /// </summary>
        private string[] throwerItems = null;

        /// <summary> 
        /// Serazovaci sklad 
        /// </summary> 
        private Group sorter = null;

        /// <summary> 
        /// Cache 
        /// </summary> 
        private Group cache = null;

        /// <summary> 
        /// Refinerie
        /// </summary> 
        private Group refinery = null;

        /// <summary> 
        /// Kontrola rafinerii
        /// </summary> 
        private bool refineryControl = false;

        /// <summary> 
        /// Monteri 
        /// </summary> 
        private Group assembler = null;

        /// <summary> 
        /// Kontrola monteru
        /// </summary> 
        private bool assemblerControl = false;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Cargo() { }

        /// <summary>
        /// Registrace noveho kontejneru
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Cargo AddStorage(string name, StorageType type)
        {
            switch (type)
            {
                case StorageType.Component:
                    component = new Block(name);
                    componentItems = Tools.GetInventoryInfo(component);
                    break;
                case StorageType.Ice:
                    ice = new Block(name);
                    Tools.GetInventoryInfo(ice).ToList().ForEach((item) => oreItems.Add(item.Key, item.Value));
                    break;
                case StorageType.Ingot:
                    ingot = new Block(name);
                    ingotItems = Tools.GetInventoryInfo(component);
                    break;
                case StorageType.Ore:
                    ore = new Block(name);
                    Tools.GetInventoryInfo(ore).ToList().ForEach((item) => oreItems.Add(item.Key, item.Value));
                    break;
            }
            return this;
        }

        /// <summary>
        /// Serazovani sklad
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Cargo AddSorter(string name)
        {
            sorter = new Group(name);
            return this;
        }

        /// <summary>
        /// Pristupovy bod
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Cargo AddCache(string name)
        {
            cache = new Group(name);
            return this;
        }

        /// <summary>
        /// Pridat vyhazovac
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Cargo AddThrower(string name, string[] throwables)
        {
            thrower = new Block(name);
            thrower.As<IMyShipConnector>().ThrowOut = true;
            throwerItems = throwables;
            return this;
        }

        /// <summary>
        /// Pridat rafinerii
        /// </summary>
        /// <param name="name"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public Cargo AddRefinery(string name, bool control = false)
        {
            // definice
            refinery = new Group(name);
            refineryControl = control;
            // deaktivace cucani
            foreach (var block in refinery)
            {
                block.As<IMyRefinery>().UseConveyorSystem = false;
            }
            // fluent
            return this;
        }

        /// <summary>
        /// Pridat montery
        /// </summary>
        /// <param name="name"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public Cargo AddAssembler(string name, bool control = false)
        {
            // definice
            assembler = new Group(name);
            assemblerControl = control;
            // deaktivace cucani
            foreach (var block in assembler)
            {
                var ass = block.As<IMyAssembler>();
                ass.CooperativeMode = false;
                ass.UseConveyorSystem = true;
            }
            // fluent
            return this;
        }

        /// <summary> 
        /// Ziskani seznamu nakladu 
        /// </summary> 
        /// <returns>Seznam nakladu</returns> 
        public Cargo Watch()
        {
            HandleSorter();
            HandleCache();
            HandleThrower();
            HandleRefinery();
            return this;
        }

        /// <summary> 
        /// Seznam komponentu 
        /// </summary> 
        /// <returns>Seznam komponentu</returns> 
        public List<string> GetComponentOverview(bool simple = false)
        {
            return GetFormattedItemList(componentItems, simple);
        }

        /// <summary> 
        /// Seznam materialu 
        /// </summary> 
        /// <returns>Seznam materialu</returns> 
        public List<string> GetIngotOverview(bool simple = false)
        {
            return GetFormattedItemList(ingotItems, simple);
        }

        /// <summary> 
        /// Seznam rud 
        /// </summary> 
        /// <returns>Seznam rud</returns> 
        public List<string> GetOreOverview(bool simple = false)
        {
            return GetFormattedItemList(oreItems, simple);
        }

        /// <summary>
        /// Prehled o praci rafinerii
        /// </summary>
        /// <returns></returns>
        public TableData GetRefineryOverview()
        {
            // definice
            List<string[]> ovreview = new List<string[]>();
            // nalezeni bloku 
            foreach (var block in refinery)
            {
                // nacteni polozek
                var items = Tools.GetInventoryList(block, InventoryType.Input);
                // sestaveni prehledu
                if (items.Count > 0)
                {
                    ovreview.Add(new string[] {
                        block.Name,
                        Status.Working,
                        items[0].Name,
                        Formater.Amount((float)items[0].Amount),
                        items.Count >= 2 ? items[1].Name : ""
                    });
                }
                else
                {
                    ovreview.Add(new string[] {
                        block.Name,
                        Status.Idle,
                        "",
                        "",
                        "",
                    });
                }
            }
            // sestaveni a vraceni
            return TableData.Create(
                new string[] { "Name", "Status", "Current", "Qty", "Queue" },
                ovreview,
                new double[] { 27, 22, 19, 13, 19 }
            );
        }

        /// <summary> 
        /// Automaticka kontrola rafinerii
        /// </summary> 
        /// <returns>Cargo</returns> 
        private void HandleSorter()
        {
            // sortery
            if (sorter != null)
            {
                foreach (var block in sorter)
                {
                    TransferItems(block.As<IMyCargoContainer>().GetInventory(0));
                }
            }
            // rafinerie
            if (refinery != null)
            {
                foreach (var block in refinery)
                {
                    TransferItems(block.As<IMyRefinery>().OutputInventory);
                }
            }
            // monteri
            if (assembler != null)
            {
                foreach (var block in assembler)
                {
                    TransferItems(block.As<IMyAssembler>().OutputInventory);
                }
            }
        }

        /// <summary>
        /// Sorter cache
        /// </summary>
        /// <returns></returns>
        private void HandleCache()
        {
            if (cache != null && component != null)
            {
                foreach (var block in cache)
                {
                    // definice
                    List<MyInventoryItem> componentItems = new List<MyInventoryItem>();
                    List<MyInventoryItem> cacheItems = new List<MyInventoryItem>();
                    MyFixedPoint amount = 0;
                    Dictionary<string, MyFixedPoint> cached = new Dictionary<string, MyFixedPoint>();
                    // nacteni polozek
                    component.As<IMyCargoContainer>().GetInventory(0).GetItems(componentItems);
                    block.As<IMyCargoContainer>().GetInventory(0).GetItems(cacheItems);
                    // premapovani nakesovanych polozek
                    foreach (MyInventoryItem item in cacheItems)
                    {
                        cached.Add(item.Type.SubtypeId, item.Amount);
                    }
                    // presun polozek
                    for (int i = componentItems.Count - 1; i >= 0; i--)
                    {
                        // definice
                        string localType = componentItems[i].Type.SubtypeId;
                        MyFixedPoint localAmount = componentItems[i].Amount;
                        var definition = Tools.GetComponentDefinition(localType);
                        // pokud se nejedna o nezadouci polozku a zaroven u v kesi neexistuje
                        if (definition.Cache)
                        {
                            if (cached.ContainsKey(localType))
                            {
                                amount = Constants.SorterCache - cached[localType];
                            }
                            else
                            {
                                // vytvoreni polozky
                                cached.Add(localType, 0);
                                // urceni mnozstvi
                                if (localAmount < Constants.SorterCache)
                                {
                                    amount = localAmount;
                                }
                                else
                                {
                                    amount = Constants.SorterCache;
                                }
                            }
                            // presun do serteru
                            if (amount > 0)
                            {
                                cached[localType] += amount;
                                component.As<IMyCargoContainer>().GetInventory(0).TransferItemTo(block.As<IMyCargoContainer>().GetInventory(0), i, null, true, amount);
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Ovladani vyhazovace
        /// </summary>
        private void HandleThrower()
        {
            foreach (var throwable in throwerItems)
            {
                // definice
                var definition = Tools.GetComponentDefinition(throwable);
                IMyInventory source = null;
                double amount = 0;
                int index = 0;
                // urceni zdroje (vyhazovat lze jen komponenty a rudy)
                if (oreItems.ContainsKey(throwable))
                {
                    source = ore.As<IMyCargoContainer>().GetInventory(0);
                    amount = oreItems[throwable].Amount;
                    index = oreItems[throwable].Index;
                }
                else if (componentItems.ContainsKey(throwable))
                {
                    source = component.As<IMyCargoContainer>().GetInventory(0);
                    amount = componentItems[throwable].Amount;
                    index = componentItems[throwable].Index;
                }
                // pokud existuje zdroj a je co vyhazovat
                if (source != null && amount > 0 && amount > definition.Reference)
                {
                    source.TransferItemTo(thrower.As<IMyShipConnector>().GetInventory(0), index, null, true, (MyFixedPoint)(amount - definition.Reference));
                }
            }
        }

        /// <summary>
        /// Pokrocile ovladani rafinerii
        /// </summary>
        /// <returns></returns>
        private void HandleRefinery()
        {
            if (refineryControl == true && refinery.Count > 0)
            {
                // definice
                Random random = new Random();
                var processable = oreItems.Keys.ToList();
                // pokud je co zpracovavat
                if (processable.Count > 0)
                {
                    // definice
                    var todo = processable[random.Next(processable.Count)];
                    double amount = oreItems[todo].Amount >= Constants.RefineryAmount * refinery.Count ? Constants.RefineryAmount : Math.Floor(oreItems[todo].Amount / refinery.Count);
                    // jednotlive rafinerie
                    foreach (var block in refinery)
                    {
                        var local = block.As<IMyRefinery>();
                        if (local.InputInventory.ItemCount < 2)
                        {
                            ore.As<IMyCargoContainer>().GetInventory(0).TransferItemTo(local.InputInventory, oreItems[todo].Index, null, true, (MyFixedPoint)amount);
                        }
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////

        

        /// <summary>
        /// Overeni zaseknuteho assembleru
        /// </summary>
        /// <returns></returns>
        public bool GetAssemblesStucked()
        {
            // definice
            bool stucked = false;
            // vyhodnoceni
            foreach (var block in assembler)
            {
                stucked = stucked || IsAssemblerStucked(block.As<IMyAssembler>(), false);
            }
            //vraceni
            return stucked;
        }

        /// <summary>
        /// Prehled o praci assembleru
        /// </summary>
        /// <returns></returns>
        public TableData GetAssemblesOverview()
        {
            // definice
            List<string[]> ovreview = new List<string[]>();
            // nalezeni bloku 
            foreach (var block in assembler)
            {
                // definice
                List<MyProductionItem> items = new List<MyProductionItem>();
                // nacteni polozek inventare
                block.As<IMyAssembler>().GetQueue(items);
                // sestaveni prehledu
                if (items.Count > 0)
                {
                    ovreview.Add(new string[] {
                        block.Name,
                        block.As<IMyAssembler>().IsProducing ? Status.Working : Status.Stuck,
                        items[0].BlueprintId.SubtypeName.Replace("Component", ""),
                        Formater.Amount((float)items[0].Amount)
                    });
                }
                else
                {
                    ovreview.Add(new string[] {
                        block.Name,
                        Status.Idle,
                        "",
                        "",
                    });
                }
            }
            // sestaveni a vraceni
            return TableData.Create(
                new string[] { "Name", "Status", "Queue", "Qty" },
                ovreview,
                new double[] { 30, 20, 35, 15 }
            );
        }

        /// <summary>
        /// Ziskani chybejicich materialu
        /// </summary>
        /// <returns>List<string></returns>
        public TableData GetMissing()
        {
            //definice
            List<string[]> missing = new List<string[]>();
            //nalezeni chybejicich
            foreach (var item in Constants.Components)
            {
                if (item.Value.Missing)
                {
                    if (ingotItems.ContainsKey(item.Key))
                    {
                        if (ingotItems[item.Key].Amount < item.Value.Reference)
                        {
                            missing.Add(new string[] { item.Key, Formater.Amount(item.Value.Reference - ingotItems[item.Key].Amount) });
                        }
                    }
                    else
                    {
                        missing.Add(new string[] { item.Key, Formater.Amount(item.Value.Reference) });
                    }
                }
            }
            // sestaveni a vraceni
            return TableData.Create(
                null,
                missing,
                new double[] { 65, 35 }
            );
        }

        /// <summary>
        /// Automaticka kontrola assembleru
        /// </summary>
        /// <returns>Cargo</returns>
        public Cargo EnableAssemblerControl()
        {
            // definice
            List<string> needed = new List<string>();
            int index = 0;
            // vyber komponent ktere chybi nebo jich je malo
            foreach (var item in Constants.Components)
            {
                if (item.Value.Assembly)
                {
                    if (componentItems.ContainsKey(item.Key))
                    {
                        if (componentItems[item.Key].Amount < item.Value.Reference)
                        {
                            needed.Add(item.Key);
                        }
                    }
                    else
                    {
                        needed.Add(item.Key);
                    }
                }
            }
            // nahodne rozhozeni seznamu
            Tools.ShuffleList(needed);
            // logovani
            Debugger.Log("Assembler queue", needed);
            // zapinani/vypinani assembleru
            if (needed.Count > 0)
            {
                foreach (var block in assembler)
                {
                    // definice
                    var ass = block.As<IMyAssembler>();
                    // osetreni indexu
                    if (index > needed.Count - 1)
                    {
                        index = 0;
                    }
                    // vlozeni blueprintu do fronty (pouze pokud je prazdna
                    if (ass.IsQueueEmpty)
                    {
                        MyDefinitionId blueprint = MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/" + needed[index]);
                        ass.AddQueueItem(blueprint, (MyFixedPoint)100);
                    }
                    else
                    {
                        IsAssemblerStucked(ass, true);
                    }
                    // zvyseni indexu
                    index++;
                }
            }
            //vraceni
            return this;
        }


        /// <summary>
        /// Kontrola zaseknuti assembleru
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool IsAssemblerStucked(IMyAssembler block, bool clear)
        {
            // definice
            List<MyProductionItem> items = new List<MyProductionItem>();
            // nacteni polozek inventare
            block.GetQueue(items);
            // kontrola fronty
            if (items.Count > 0)
            {
                //vycisteni zaseknuteho assembleru
                if (!block.IsProducing && clear)
                {
                    for (int i = items.Count - 1; i >= 0; i--)
                    {
                        block.RemoveQueueItem(i, items[i].Amount);
                    }
                }
                // vraceni
                return !block.IsProducing;
            }
            // pokud neni zadna polozka ve fronte nema se co zaseknout
            return false;
        }
























        /// <summary> 
        /// Presun polozek inventare
        /// </summary> 
        /// <param name="inventory">Zdrojovy inventar</param> 
        private void TransferItems(IMyInventory inventory)
        {
            // definice
            List<MyInventoryItem> items = new List<MyInventoryItem>();
            var stone = Tools.GetComponentDefinition("Stone");
            // nacteni polozek
            inventory.GetItems(items);
            // presun polozek
            for (int i = items.Count - 1; i >= 0; i--)
            {
                // misto urceni 
                IMyInventory dest = null;
                MyFixedPoint amount = 0;
                string type = items[i].Type.TypeId;
                string subtype = items[i].Type.SubtypeId;
                // zpracovani dle typu
                switch (type)
                {
                    case "MyObjectBuilder_Ingot":
                        dest = ingot.As<IMyCargoContainer>().GetInventory(0);
                        break;
                    case "MyObjectBuilder_Ore":
                        if (subtype == "Ice" && ice != null)
                        {
                            dest = ice.As<IMyCargoContainer>().GetInventory(0);
                        }

                        else
                        {
                            dest = ore.As<IMyCargoContainer>().GetInventory(0);
                        }
                        break;
                    default:
                        if (subtype == "Scrap")
                        {
                            dest = ore.As<IMyCargoContainer>().GetInventory(0);
                        }
                        else
                        {
                            dest = component.As<IMyCargoContainer>().GetInventory(0);
                        }
                        break;
                }
                //presun 
                if (dest != null && !dest.IsFull)
                {
                    if (amount > 0)
                    {
                        inventory.TransferItemTo(dest, i, null, true, amount);
                    }
                    else
                    {
                        inventory.TransferItemTo(dest, i, null, true, null);
                    }
                }
            }
        }

        /// <summary>
        ///  Priprava vypisu
        /// </summary>
        /// <param name="items">Polozky</param>
        /// <param name="simple">Zjednoduseny vypis</param>
        /// <param name="referenceOverride">Externi referencni hodnota</param>
        /// <returns></returns>
        private List<string> GetFormattedItemList(Dictionary<string, (double Amount, int Index)> items, bool simple)
        {
            // definice
            List<string> itemList = new List<string>();
            // sestaveni
            foreach (var item in items)
            {
                // definice
                var definition = Tools.GetComponentDefinition(item.Key);
                // pridat do vypisu pouze pokud existuje nejaka referencni hodnota
                if (definition.Reference != -1)
                {
                    if (simple)
                    {
                        itemList.Add(item.Key + " => " + Formater.Amount(item.Value.Amount));
                    }
                    else
                    {
                        itemList.Add(Formater.BarsWithAmount(item.Key, item.Value.Amount, definition.Reference));
                    }
                }
            }
            // vraceni
            return itemList;
        }
    }
}