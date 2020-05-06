using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using System.Linq;
using VRage;
using Sandbox.ModAPI.Ingame;
using VRage.Game;

namespace SpaceEngineers
{
    /// <summary> 
    /// Kargo 
    /// </summary> 
    class Cargo
    {
        /// <summary> 
        /// Komponenty 
        /// </summary> 
        private readonly Dictionary<string, float[]> CargoComponent = new Dictionary<string, float[]>();

        /// <summary> 
        /// Materialy 
        /// </summary> 
        private readonly Dictionary<string, float[]> CargoIngot = new Dictionary<string, float[]>();

        /// <summary> 
        /// Rudy 
        /// </summary> 
        private readonly Dictionary<string, float[]> CargoOre = new Dictionary<string, float[]>();

        /// <summary> 
        /// Vyhazovac 
        /// </summary> 
        private IMyShipConnector Thrower = null;

        /// <summary> 
        /// Serazovaci sklad 
        /// </summary> 
        private List<IMyCargoContainer> Sorter = new List<IMyCargoContainer>();

        /// <summary> 
        /// Materialy 
        /// </summary> 
        private IMyCargoContainer Ingot = null;

        /// <summary> 
        /// Rudy 
        /// </summary> 
        private IMyCargoContainer Ore = null;

        /// <summary> 
        /// Komponenty 
        /// </summary> 
        private IMyCargoContainer Component = null;

        /// <summary> 
        /// Cache 
        /// </summary> 
        private IMyCargoContainer Cache = null;

        /// <summary>
        /// Instance
        /// </summary>
        private readonly Block Instance = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Cargo(string block)
        {
            Instance = new Block(block);
            ScanCargo();
        }

        /// <summary>
        /// Rozrazeni polozek inventare
        /// </summary>
        /// <param name="items">Polozky inventare</param>
        private void LoadAmount(List<MyInventoryItem> items)
        {
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    // polozka
                    string type = item.Type.SubtypeId;
                    float amount = (float)item.Amount;
                    // pouye nenulove poloykz
                    if (amount > 0)
                    {
                        // ziskani referencni hodnoty
                        int reference;
                        Constants.AmountReference.TryGetValue(type, out reference);
                        // pokud neexistuje prida se nula
                        if (reference == 0 && !Constants.AmountReference.ContainsKey(type))
                        {
                            Constants.AmountReference.Add(type, 0);
                        }
                        // zarazeni do seznamu 
                        if (reference != -1)
                        {
                            switch (item.Type.TypeId)
                            {
                                case "MyObjectBuilder_Ore":
                                    if (CargoOre.ContainsKey(type))
                                    {
                                        CargoOre[type][0] += amount;
                                    }
                                    else
                                    {
                                        CargoOre.Add(type, new float[] { amount, reference });
                                    }
                                    break;
                                case "MyObjectBuilder_Ingot":
                                    if (CargoIngot.ContainsKey(type))
                                    {
                                        CargoIngot[type][0] += amount;
                                    }
                                    else
                                    {
                                        CargoIngot.Add(type, new float[] { amount, reference });
                                    }
                                    break;
                                default:
                                    if (CargoComponent.ContainsKey(type))
                                    {
                                        CargoComponent[type][0] += amount;
                                    }
                                    else
                                    {
                                        CargoComponent.Add(type, new float[] { amount, reference });
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary> 
        /// Ziskani seznamu nakladu 
        /// </summary> 
        /// <returns>Seznam nakladu</returns> 
        private void ScanCargo()
        {
            try
            {
                foreach (var block in Instance.GetAll())
                {
                    List<MyInventoryItem> items = new List<MyInventoryItem>();
                    if (block.Value is IMyCargoContainer)
                    {
                        (block.Value as IMyCargoContainer).GetInventory(0).GetItems(items);
                        LoadAmount(items);
                    }
                    else if (block.Value is IMyAssembler)
                    {
                        (block.Value as IMyAssembler).InputInventory.GetItems(items);
                        LoadAmount(items);
                    }
                    else if (block.Value is IMyRefinery)
                    {
                        (block.Value as IMyRefinery).InputInventory.GetItems(items);
                        LoadAmount(items);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("E-CA-01: " + ex.Message);
            }
        }

        /// <summary>
        ///  Priprava vypisu
        /// </summary>
        /// <param name="items">Polozky</param>
        /// <returns></returns>
        private List<string> BuildItemList(Dictionary<String, float[]> items)
        {
            // definice
            List<string> itemList = new List<string>();
            // sestaveni
            foreach (var item in items)
            {
                itemList.Add(Formater.BarsWithAmount(item.Key, item.Value[0], item.Value[0]));
            }
            // vraceni
            return itemList;
        }

        /// <summary> 
        /// Presun polozek inventare
        /// </summary> 
        /// <param name="inventory">Zdrojovy inventar</param> 
        private void TransferItems(IMyInventory inventory)
        {
            // definice
            List<MyInventoryItem> items = new List<MyInventoryItem>();
            // nacteni polozek
            inventory.GetItems(items);
            // presun polozek
            for (int i = items.Count - 1; i >= 0; i--)
            {
                // misto urceni 
                IMyInventory dest = null;
                MyFixedPoint amount = 0;
                // s sutrem se zachazi specificky
                if (items[i].Type.SubtypeId == "Stone")
                {
                    if (CargoOre.ContainsKey("Stone") && CargoOre["Stone"][0] < CargoOre["Stone"][1])
                    {
                        dest = Ore.GetInventory(0);
                        // omezeni mnozstvi
                        MyFixedPoint maximum = (MyFixedPoint)(CargoOre["Stone"][1] - CargoOre["Stone"][0]);
                        if (items[i].Amount > maximum)
                        {
                            amount = maximum;
                        }
                        else
                        {
                            amount = items[i].Amount;
                        }
                    }
                    else
                    {
                        dest = Thrower.GetInventory(0);
                    }
                }
                else
                {
                    switch (items[i].Type.TypeId)
                    {
                        case "MyObjectBuilder_Ingot":
                            dest = Ingot.GetInventory(0);
                            break;
                        case "MyObjectBuilder_Ore":
                            if (items[i].Amount > Constants.MaximalOreAmount)
                            {
                                amount = items[i].Amount - Constants.MaximalOreAmount;
                                dest = Thrower.GetInventory(0);
                            }
                            else
                            {
                                dest = Ore.GetInventory(0);
                            }
                            break;
                        default:
                            if (items[i].Type.SubtypeId == "Scrap")
                            {
                                dest = Ore.GetInventory(0);
                            }
                            else
                            {
                                dest = Component.GetInventory(0);
                            }
                            break;
                    }
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
        /// Priprava produkcnich bloku
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private Dictionary<string, List<T>> PrepareProductionBlocks<T>(bool disableConveyor = false) where T : IMyProductionBlock
        {
            // definice
            Dictionary<string, List<T>> blocks = new Dictionary<string, List<T>>();
            // nalezeni bloku 
            foreach (var block in Instance.GetByType<T>())
            {
                // sestaveni mapy
                if (block.Value.CustomData.Length > 0)
                {
                    // priprava zpracovavanych polozek
                    var items = block.Value.CustomData.Split('\n').Select(i => i.Trim()).ToArray();
                    // sestaveni
                    foreach (var item in items)
                    {
                        if (blocks.ContainsKey(item))
                        {
                            blocks[item].Add(block.Value);
                        }
                        else
                        {
                            blocks.Add(item, new List<T> { block.Value });
                        }
                    }
                }
                else
                {
                    if (blocks.ContainsKey("generic"))
                    {
                        blocks["generic"].Add(block.Value);
                    }
                    else
                    {
                        blocks.Add("generic", new List<T> { block.Value });
                    }
                }
                // deaktivace cucani
                if (disableConveyor)
                {
                    block.Value.UseConveyorSystem = false;
                }
                else
                {
                    block.Value.UseConveyorSystem = true;
                }
            }
            // vraceni
            return blocks;
        }

        /// <summary> 
        /// Seznam komponentu 
        /// </summary> 
        /// <returns>Seznam komponentu</returns> 
        public List<string> GetComponentList()
        {
            return BuildItemList(CargoComponent);
        }

        /// <summary> 
        /// Seznam materialu 
        /// </summary> 
        /// <returns>Seznam materialu</returns> 
        public List<string> GetIngotList()
        {
            return BuildItemList(CargoIngot);
        }

        /// <summary> 
        /// Seznam rud 
        /// </summary> 
        /// <returns>Seznam rud</returns> 
        public List<string> GetOreList()
        {
            return BuildItemList(CargoOre);
        }

        /// <summary> 
        /// Automaticka kontrola rafinerii
        /// </summary> 
        /// <returns>Cargo</returns> 
        public Cargo EnableSorter()
        {
            // potrebne bloky
            Thrower = Instance.GetListByName<IMyShipConnector>("_Thrower", true).First();
            Sorter = Instance.GetListByName<IMyCargoContainer>("_Sorter", true);
            Component = Instance.GetListByName<IMyCargoContainer>("_Component", true).First();
            Ingot = Instance.GetListByName<IMyCargoContainer>("_Ingot", true).First();
            Ore = Instance.GetListByName<IMyCargoContainer>("_Ore", true).First();
            // aktivace vyhazovace
            Thrower.ThrowOut = true;
            // vycucnuti zasob z konektoru, rafinerii a monteru
            foreach (var block in Instance.GetAll())
            {
                // konektory 
                if (block.Value is IMyShipConnector && !block.Key.Contains("_Thrower"))
                {
                    TransferItems(block.Value.GetInventory(0));
                }
                // vrtaky 
                if (block.Value is IMyShipDrill)
                {
                    TransferItems(block.Value.GetInventory(0));
                }
                // rafinerie a pece
                if (block.Value is IMyRefinery)
                {
                    TransferItems((block.Value as IMyRefinery).OutputInventory);
                }
                // monteri 
                if (block.Value is IMyAssembler)
                {
                    TransferItems((block.Value as IMyAssembler).OutputInventory);
                }
            }
            // serazovaci sklad 
            foreach (var block in Sorter)
            {
                TransferItems(block.GetInventory(0));
            }
            // presortovani kontejneru
            TransferItems(Component.GetInventory(0));
            TransferItems(Ore.GetInventory(0));
            TransferItems(Ingot.GetInventory(0));
            // vraceni 
            return this;
        }

        /// <summary>
        /// Sorter cache
        /// </summary>
        /// <returns></returns>
        public Cargo EnableCache()
        {
            // nutny blok
            Cache = Instance.GetListByName<IMyCargoContainer>("_Cache", true).First();
            // definice
            List<MyInventoryItem> components = new List<MyInventoryItem>();
            List<MyInventoryItem> cache = new List<MyInventoryItem>();
            MyFixedPoint amount = 0;
            Dictionary<string, MyFixedPoint> cached = new Dictionary<string, MyFixedPoint>();
            // nacteni polozek
            Component.GetInventory(0).GetItems(components);
            Cache.GetInventory(0).GetItems(cache);
            // premapovani nakesovanych polozek
            foreach (var item in cache)
            {
                cached.Add(item.Type.SubtypeId, item.Amount);
            }
            // presun polozek
            for (int i = components.Count - 1; i >= 0; i--)
            {
                // definice
                var localType = components[i].Type.SubtypeId;
                var localAmount = components[i].Amount;
                // pokud se nejedna o nezadouci polozku a zaroven u v kesi neexistuje
                if (Constants.AmountReference.ContainsKey(localType))
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
                        Component.GetInventory(0).TransferItemTo(Cache.GetInventory(0), i, null, true, amount);
                    }
                }
            }
            // fluent
            return this;
        }

        /// <summary>
        /// Pokrocile ovladani rafinerii
        /// </summary>
        /// <returns></returns>
        public Cargo EnableRefineryControl()
        {
            // rafinerie
            var refineries = PrepareProductionBlocks<IMyRefinery>(true);
            // nacteni dostupnych rud
            List<MyInventoryItem> items = new List<MyInventoryItem>();
            Ore.GetInventory(0).GetItems(items);
            // nacteni dostupnzch rud
            List<string> needed = new List<string>();
            foreach (var item in items)
            {
                needed.Add(item.Type.SubtypeId);
            }
            // rozhozeni seznamu
            Tools.ShuffleList(needed);
            // rozhozeni do rafinerii
            foreach (var ore in needed)
            {
                // definice
                var index = items.FindIndex(item => item.Type.SubtypeId == ore);
                var amount = items[index].Amount;
                List<IMyRefinery> selected;
                MyFixedPoint transfer;
                // rozhodnuti kam rudu presmerovat
                if (refineries.ContainsKey(ore))
                {
                    selected = refineries[ore];
                }
                else
                {
                    selected = refineries["generic"];
                }
                // urceni prislusneho mnozstvi
                if (amount > (Constants.RefineryAmount * selected.Count))
                {
                    transfer = Constants.RefineryAmount;
                }
                else
                {
                    transfer = (MyFixedPoint)Math.Floor((double)amount / selected.Count) - 1;
                }
                // presmerovani prislusneho mnozstvi
                if (transfer > 0)
                {
                    foreach (var block in selected)
                    {
                        // nacteni existujicich polozek
                        List<MyInventoryItem> assigned = new List<MyInventoryItem>();
                        block.InputInventory.GetItems(assigned);
                        // overeni existence
                        var exist = assigned.FindIndex(item => item.Type.SubtypeId == ore);
                        // presun je-li to potreba
                        if (!block.InputInventory.IsFull && exist == -1)
                        {
                            Ore.GetInventory(0).TransferItemTo(block.InputInventory, index, null, true, transfer);
                        }
                    }
                }
            }
            //vraceni
            return this;
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
            foreach (var block in Instance.GetByType<IMyRefinery>())
            {
                // definice
                List<MyInventoryItem> items = new List<MyInventoryItem>();
                // nacteni polozek inventare
                block.Value.InputInventory.GetItems(items);
                // sestaveni prehledu
                if (items.Count > 0)
                {
                    ovreview.Add(new string[] {
                        Block.GetName(block.Value),
                        Status.Working,
                        items[0].Type.SubtypeId,
                        Formater.Amount((double)items[0].Amount),
                        items.Count >= 2 ? items[1].Type.SubtypeId : ""
                    });
                }
                else
                {
                    ovreview.Add(new string[] {
                        Block.GetName(block.Value),
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
        /// Prehled o praci assembleru
        /// </summary>
        /// <returns></returns>
        public TableData GetAssemblesOverview()
        {
            // definice
            List<string[]> ovreview = new List<string[]>();
            // nalezeni bloku 
            foreach (var block in Instance.GetByType<IMyAssembler>())
            {
                // definice
                List<MyProductionItem> items = new List<MyProductionItem>();
                // nacteni polozek inventare
                block.Value.GetQueue(items);
                // sestaveni prehledu
                if (items.Count > 0)
                {
                    ovreview.Add(new string[] {
                        Block.GetName(block.Value),
                        block.Value.IsProducing ? Status.Working : Status.Stuck,
                        items[0].BlueprintId.SubtypeName.Replace("Component", ""),
                        Formater.Amount((double)items[0].Amount)
                    });
                }
                else
                {
                    ovreview.Add(new string[] {
                        Block.GetName(block.Value),
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
        public List<string> GetMissing()
        {
            //definice
            var missing = new List<string>();
            //nalezeni chybejicich
            foreach (var item in Constants.CheckMissingIngot)
            {
                if (CargoIngot.ContainsKey(item))
                {
                    if (CargoIngot[item][0] < CargoIngot[item][1])
                    {
                        missing.Add(item);
                    }
                }
                else
                {
                    missing.Add(item);
                }
            }
            //vraceni
            return missing;
        }

        /// <summary>
        /// Automaticka kontrola assembleru
        /// </summary>
        /// <returns>Cargo</returns>
        public Cargo EnableAssemblerControl()
        {
            // definice
            var needed = new List<string>();
            var index = 0;
            // vyber komponent ktere chybi nebo jich je malo
            foreach (var comp in Constants.BasicAssembly)
            {
                if (CargoComponent.ContainsKey(comp.Key))
                {
                    var exist = CargoComponent[comp.Key];
                    if (exist[0] < exist[1])
                    {
                        needed.Add(comp.Value);
                    }
                }
                else
                {
                    needed.Add(comp.Value);
                }
            }
            // nahodne rozhoreni seznamu
            Tools.ShuffleList(needed);
            // logovani
            Debugger.Log("Assembler queue", needed);
            // zapinani/vypinani assembleru
            foreach (var block in Instance.GetByType<IMyAssembler>())
            {
                // nastaveni
                block.Value.CooperativeMode = false;
                block.Value.UseConveyorSystem = true;
                // osetreni indexu
                if (index > needed.Count - 1)
                {
                    index = 0;
                }
                // vlozeni blueprintu do fronty (pouze pokud je prazdna
                if (block.Value.IsQueueEmpty)
                {
                    var blueprint = MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/" + needed[index]);
                    block.Value.AddQueueItem(blueprint, (MyFixedPoint)100);
                }
                // zvyseni indexu
                index++;
            }
            //vraceni
            return this;
        }
    }
}