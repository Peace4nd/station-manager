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
        /// Nasatveni obsahu panelu
        /// </summary>
        public const int PanelColumnsWide = 51;
        public const int PanelColumnsSmall = 25;
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
            {"Computer", 1000},
            {"Display", 1000},
            {"Motor", 1000},
            {"Detector", 0},
            {"GravityGenerator", 0},
            {"Medical", 0},
            {"PowerCell", 0},
            {"RadioCommunication", 0},
            {"Reactor", 0},
            {"SolarCell", 0},
            {"Thrust", 0},
            {"Superconductor", 0},
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
            {"Parachute", -1},
            {"Canvas", -1},
            {"UltimateAutomaticRifleItem", -1}
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
        /// Komponenty na cachovani
        /// </summary>
        public static readonly List<string> ComponentsToCache = new List<string> {
            "Construction",
            "Girder",
            "InteriorPlate",
            "LargeTube",
            "MetalGrid",
            "SmallTube",
            "SteelPlate",
            "BulletproofGlass",
            "Computer",
            "Display",
            "Motor"
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
        public const int MaximalOreAmount = 250000;
    }
}