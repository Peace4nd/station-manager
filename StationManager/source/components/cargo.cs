using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Rudy 
        /// </summary> 
        private IMyCargoContainer Ice = null;

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
                foreach (MyInventoryItem item in items)
                {
                    // polozka
                    string type = item.Type.SubtypeId;
                    float amount = (float)item.Amount;
                    // pouye nenulove poloykz
                    if (amount > 0)
                    {
                        // ziskani referencni hodnoty
                        int reference = 0;
                        if (Constants.AmountReference.ContainsKey(type))
                        {
                            reference = Constants.AmountReference[type];
                        }
                        else
                        {
                            Debugger.Warning("Unknown type '" + type + "'");
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
                foreach (KeyValuePair<string, IMyTerminalBlock> block in Instance.GetAll())
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
                    else if (block.Value is IMyShipDrill)
                    {
                        (block.Value as IMyShipDrill).GetInventory(0).GetItems(items);
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
        /// <param name="simple">Zjednoduseny vypis</param>
        /// <param name="referenceOverride">Externi referencni hodnota</param>
        /// <returns></returns>
        private List<string> BuildItemList(Dictionary<string, float[]> items, bool simple, double referenceOverride = -1)
        {
            // definice
            List<string> itemList = new List<string>();
            // sestaveni
            foreach (KeyValuePair<string, float[]> item in items)
            {
                if (simple)
                {
                    itemList.Add(item.Key + " => " + Formater.Amount(item.Value[0]));
                }
                else
                {
                    itemList.Add(Formater.BarsWithAmount(item.Key, item.Value[0], referenceOverride > -1 ? referenceOverride : item.Value[1]));
                }

            }
            // vraceni
            return itemList;
        }

        /// <summary> 
        /// Presun polozek inventare
        /// </summary> 
        /// <param name="inventory">Zdrojovy inventar</param> 
        private void TransferItems(IMyInventory inventory, bool onlyThrow = false)
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
                string type = items[i].Type.TypeId;
                string subtype = items[i].Type.SubtypeId;
                // pouze vyhozeni sutru
                if (onlyThrow && type == "MyObjectBuilder_Ore" && subtype == "Stone")
                {
                    dest = Thrower.GetInventory(0);
                }
                else
                {
                    // zpracovani dle typu
                    switch (type)
                    {
                        case "MyObjectBuilder_Ingot":
                            dest = Ingot.GetInventory(0);
                            break;
                        case "MyObjectBuilder_Ore":
                            // se sutrem se zachazi specificky
                            if (subtype == "Stone")
                            {
                                if (CargoOre.ContainsKey("Stone"))
                                {
                                    // definice
                                    MyFixedPoint needed = (MyFixedPoint)(CargoOre["Stone"][1] - CargoOre["Stone"][0]);
                                    // kam s nim
                                    if (needed <= 0)
                                    {
                                        dest = Thrower.GetInventory(0);
                                    }
                                    else
                                    {
                                        dest = Ore.GetInventory(0);
                                        amount = needed;
                                    }
                                }
                                else
                                {
                                    dest = Ore.GetInventory(0);
                                    amount = items[i].Amount >= Constants.AmountReference["Stone"] ? Constants.AmountReference["Stone"] : items[i].Amount;
                                }
                            }
                            else if (subtype == "Ice")
                            {
                                dest = Ice.GetInventory(0);
                            }
                            else
                            {
                                if (items[i].Amount > Constants.MaximalOreAmount && subtype != "Ice")
                                {
                                    amount = items[i].Amount - Constants.MaximalOreAmount;
                                    dest = Thrower.GetInventory(0);
                                }
                                else
                                {
                                    dest = Ore.GetInventory(0);
                                }
                            }
                            break;
                        default:
                            if (subtype == "Scrap")
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
            foreach (KeyValuePair<string, T> block in Instance.GetByType<T>())
            {
                // sestaveni mapy
                if (block.Value.CustomData.Length > 0)
                {
                    // priprava zpracovavanych polozek
                    string[] items = block.Value.CustomData.Split('\n').Select(i => i.Trim()).ToArray();
                    // sestaveni
                    foreach (string item in items)
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
        public List<string> GetComponentList(bool simple = false)
        {
            return BuildItemList(CargoComponent, simple);
        }

        /// <summary> 
        /// Seznam materialu 
        /// </summary> 
        /// <returns>Seznam materialu</returns> 
        public List<string> GetIngotList(bool simple = false)
        {
            return BuildItemList(CargoIngot, simple);
        }

        /// <summary> 
        /// Seznam rud 
        /// </summary> 
        /// <returns>Seznam rud</returns> 
        public List<string> GetOreList(bool simple = false)
        {
            return BuildItemList(CargoOre, simple, Constants.MaximalOreAmount);
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
            Ice = Instance.GetListByName<IMyCargoContainer>("_Ice", true).First();
            // aktivace vyhazovace
            Thrower.ThrowOut = true;
            // vycucnuti zasob z konektoru, rafinerii a monteru
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Instance.GetAll())
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
                    //TransferItems((block.Value as IMyRefinery).InputInventory, true);
                }
                // monteri 
                if (block.Value is IMyAssembler)
                {
                    TransferItems((block.Value as IMyAssembler).OutputInventory);
                }
            }
            // serazovaci sklad 
            foreach (IMyCargoContainer block in Sorter)
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
            foreach (MyInventoryItem item in cache)
            {
                cached.Add(item.Type.SubtypeId, item.Amount);
            }
            // presun polozek
            for (int i = components.Count - 1; i >= 0; i--)
            {
                // definice
                string localType = components[i].Type.SubtypeId;
                MyFixedPoint localAmount = components[i].Amount;
                // pokud se nejedna o nezadouci polozku a zaroven u v kesi neexistuje
                if (Constants.ComponentsToCache.Contains(localType))
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
            // definice
            Dictionary<string, List<IMyRefinery>> refineries = PrepareProductionBlocks<IMyRefinery>(true);
            Random random = new Random();
            // nacteni dostupnych rud
            List<MyInventoryItem> items = new List<MyInventoryItem>();
            Ore.GetInventory(0).GetItems(items);
            // nacteni dostupnych rud
            List<string> needed = new List<string>();
            foreach (MyInventoryItem item in items)
            {
                needed.Add(item.Type.SubtypeId);
            }
            // prochazeni typu rafinerii
            foreach (KeyValuePair<string, List<IMyRefinery>> refinery in refineries)
            {
                // jednotlive rafinerie
                foreach (IMyRefinery block in refinery.Value)
                {
                    // definice
                    string ore = refinery.Key == "generic" ? needed[random.Next(needed.Count)] : refinery.Key;
                    int index = items.FindIndex(item => item.Type.SubtypeId == ore);
                    // pokud dana ruda existuje
                    if (index >= 0)
                    {
                        // definice
                        MyFixedPoint amount = items[index].Amount;
                        MyFixedPoint transfer;
                        // urceni prislusneho mnozstvi
                        if (amount > (Constants.RefineryAmount * refinery.Value.Count))
                        {
                            transfer = Constants.RefineryAmount;
                        }
                        else
                        {
                            transfer = (MyFixedPoint)Math.Floor((double)amount / refinery.Value.Count) - 1;
                        }
                        // presmerovani prislusneho mnozstvi
                        if (transfer > 0)
                        {
                            // nacteni existujicich polozek
                            List<MyInventoryItem> assigned = new List<MyInventoryItem>();
                            block.InputInventory.GetItems(assigned);
                            // overeni existence
                            int exist = assigned.FindIndex(item => item.Type.SubtypeId == ore);
                            // presun je-li to potreba
                            if (!block.InputInventory.IsFull && exist == -1)
                            {
                                Ore.GetInventory(0).TransferItemTo(block.InputInventory, index, null, true, transfer);
                            }
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
            foreach (KeyValuePair<string, IMyRefinery> block in Instance.GetByType<IMyRefinery>())
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
            bool stucked;
            return GetAssemblesOverview(out stucked);
        }

        /// <summary>
        /// Prehled o praci assembleru
        /// </summary>
        /// <returns></returns>
        public TableData GetAssemblesOverview(out bool stucked)
        {
            // definice
            List<string[]> ovreview = new List<string[]>();
            int localStuck = 0;
            // nalezeni bloku 
            foreach (KeyValuePair<string, IMyAssembler> block in Instance.GetByType<IMyAssembler>())
            {
                // definice
                List<MyProductionItem> items = new List<MyProductionItem>();
                // nacteni polozek inventare
                block.Value.GetQueue(items);
                // sestaveni prehledu
                if (items.Count > 0)
                {
                    // detekce zaseknuti
                    localStuck = Math.Max(localStuck, items.Count > 0 && !block.Value.IsProducing ? 1 : 0);
                    // prirazeni   
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
            // zaseknuti
            stucked = localStuck == 1 ? true : false;
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
            foreach (string item in Constants.CheckMissingIngot)
            {
                if (CargoIngot.ContainsKey(item))
                {
                    if (CargoIngot[item][0] < CargoIngot[item][1])
                    {
                        missing.Add(new string[] { item, Formater.Amount(CargoIngot[item][1] - CargoIngot[item][0]) });
                    }
                }
                else
                {
                    missing.Add(new string[] { item, Formater.Amount(Constants.AmountReference[item]) });
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
            foreach (KeyValuePair<string, string> comp in Constants.BasicAssembly)
            {
                if (CargoComponent.ContainsKey(comp.Key))
                {
                    float[] exist = CargoComponent[comp.Key];
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
            if (needed.Count > 0)
            {
                foreach (KeyValuePair<string, IMyAssembler> block in Instance.GetByType<IMyAssembler>())
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
                        MyDefinitionId blueprint = MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/" + needed[index]);
                        block.Value.AddQueueItem(blueprint, (MyFixedPoint)100);
                    }
                    // zvyseni indexu
                    index++;
                }
            }
            //vraceni
            return this;
        }
    }
}