using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary>
    /// Systemove konstanty
    /// </summary>
    class Constants
    {
        /// <summary>
        /// Proporce sloupcu delka statusu
        /// </summary>
        public const int ColumnBars = 17;
        public const int ColumnStatus = 12;
        public const int ColumnAmount = 5;

        /// <summary>
        /// Rodina pisma
        /// </summary>
        public const long FontFamily = 1147350002;

        /// <summary>
        /// Nasatveni obsahu panelu
        /// </summary>
        public const long PanelContent = 2;
        public const int PanelColumns = 51;
        public const int PanelRows = 17;

        /// <summary> 
        /// Minimalni mnoztvi oceli 
        /// </summary> 
        public static readonly Dictionary<string, int> AmountReference = new Dictionary<string, int>()
        {
            // ocel
            {"Construction", 2000},
            {"Girder", 2000},
            {"InteriorPlate", 2000},
            {"LargeTube", 500},
            {"MetalGrid", 2000},
            {"SmallTube", 2000},
            {"SteelPlate", 5000},
            // komponenty
            {"BulletproofGlass", 500},
            {"Computer", 500},
            {"Display", 500},
            {"Motor", 500},
            {"Detector", 100},
            {"GravityGenerator", 100},
            {"Medical", 100},
            {"PowerCell", 100},
            {"RadioCommunication", 100},
            {"Reactor", 100},
            {"SolarCell", 100},
            {"Thrust", 1000},
            {"Superconductor", 1000},
            // ingoty
            {"Stone", 7500},
            {"Iron", 15000},
            {"Nickel", 7500},
            {"Cobalt", 7500},
            {"Silicon", 7500},
            {"Silver", 7500},
            {"Gold", 7500},
            {"Magnesium", 7500},
            {"Platinum", 7500},
            {"Uranium", 2500},
            {"Ice", 50000},
            // naboje
            {"NATO_25x184mm", 100},
            // shits
            {"NATO_5p56x45mm", -1},
            {"Missile200mm", -1},
            {"AngleGrinderItem", -1},
            {"AngleGrinder2Item", -1},
            {"AngleGrinder3Item", -1},
            {"HandDrillItem", -1},
            {"HandDrill2Item", -1},
            {"HandDrill3Item", -1},
            {"WelderItem", -1},
            {"Welder2Item", -1},
            {"Welder3Item", -1},
            {"AutomaticRifleItem", -1},
            {"Scrap", -1},
            {"OxygenBottle", -1},
            {"HydrogenBottle", -1},
            {"Explosives", -1},
            {"Parachute", -1}
        };

        /// <summary>
        /// Rudy zpracovatelne v obloukove peci
        /// </summary>
        public static readonly List<string> FurnanceOres = new List<string> {
            "Scrap",
            "Iron",
            "Nickel",
            "Cobalt"
        };

        /// <summary>
        /// Rudy zpracovatelne v obloukove peci
        /// </summary>
        public static readonly List<string> RefineryOres = new List<string> {
            "Silicon",
            "Silver",
            "Gold",
            "Magnesium",
            "Platinum",
            "Uranium"
        };

        /// <summary>
        /// Materialy ke kontrole (ktere chybi)
        /// </summary>
        public static readonly List<string> CheckMissingIngot = new List<string> {
            "Iron",
            "Nickel",
            "Cobalt",
            "Silicon",
            "Silver",
            "Gold",
            "Magnesium",
            "Platinum",
            "Uranium"
        };

        /// <summary>
        /// Zakladni vyrobni fronta
        /// </summary>
        public static readonly Dictionary<string, string> BasicAssembly = new Dictionary<string, string> {
            {"Construction", "ConstructionComponent"},
            {"Girder", "GirderComponent"},
            {"InteriorPlate", "InteriorPlate" },
            {"LargeTube","LargeTube"},
            {"MetalGrid", "MetalGrid"},
            {"SmallTube", "SmallTube"},
            {"SteelPlate", "SteelPlate"},
            {"Computer", "ComputerComponent"},
            {"Display", "Display"},
            {"Motor", "MotorComponent"},
            {"BulletproofGlass", "BulletproofGlass"},
            {"Thrust", "ThrustComponent"}
        };

        /// <summary>
        /// Sorter cache
        /// </summary>
        public const int SorterCache = 100;

        /// <summary>
        /// Zakladni mnozstvi rafinovane rudy
        /// </summary>
        public const int RefineryAmount = 1000;

        /// <summary>
        /// Maximalni limit rud
        /// </summary>
        public const int MaximalOreAmount = 200000;
    }
}