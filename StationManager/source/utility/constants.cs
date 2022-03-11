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
        /// Definice komponent
        /// </summary> 
        /// 
        public static readonly Dictionary<string, Definition> Components = new Dictionary<string, Definition>()
        {
            // komponenty
            {"Construction",  Definition.Create(1500, true, true, "ConstructionComponent", false)},
            {"Girder", Definition.Create(1500, true, true, "GirderComponent", false)},
            {"InteriorPlate", Definition.Create(1500, true, true, "InteriorPlate", false)},
            {"LargeTube", Definition.Create(500, true, true, "LargeTube", false)},
            {"MetalGrid", Definition.Create(500, true, true, "MetalGrid", false)},
            {"SmallTube", Definition.Create(1500, true, true, "SmallTube", false)},
            {"SteelPlate", Definition.Create(2500, true, true, "SteelPlate", false)},
            {"BulletproofGlass", Definition.Create(500, true, true, "BulletproofGlass", false)},
            {"Computer", Definition.Create(200, true, true, "ComputerComponent", false)},
            {"Display", Definition.Create(200, true, true, "Display", false)},
            {"Motor", Definition.Create(200, true, true, "MotorComponent", false)},
            {"PowerCell", Definition.Create(200, true, false, "PowerCell", false)},
            {"Detector", Definition.Create(0, false, false, null, false)},
            {"GravityGenerator", Definition.Create(0, false, false, null, false)},
            {"Medical", Definition.Create(0, false, false, null, false)},
            {"RadioCommunication", Definition.Create(0, false, false, null, false)},
            {"Reactor", Definition.Create(0, false, false, null, false)},
            {"SolarCell", Definition.Create(0, false, false, null, false)},
            {"Thrust", Definition.Create(0, false, false, null, false)},
            {"Superconductor", Definition.Create(0, false, false, null, false)},

            // ingoty
            {"Stone", Definition.Create(7500, false, false, null, false)},
            {"Iron", Definition.Create(15000, false, false, null, true)},
            {"Nickel",Definition.Create(7500, false, false, null, true)},
            {"Cobalt", Definition.Create(7500, false, false, null, true)},
            {"Silicon", Definition.Create(7500, false, false, null, true)},
            {"Silver", Definition.Create(7500, false, false, null, true)},
            {"Gold", Definition.Create(7500, false, false, null, true)},
            {"Magnesium", Definition.Create(7500, false, false, null, true)},
            {"Platinum", Definition.Create(7500, false, false, null, true)},
            {"Uranium", Definition.Create(2500, false, false, null, true)},
            {"Ice", Definition.Create(7500, false, false, null, true)},

            // naboje
            {"NATO_25x184mm", Definition.Create(250, false, false, "NATO_25x184mmMagazine", false)},

            // shits
            {"NATO_5p56x45mm", Definition.Create(-1, false, false, null, false)},
            {"Missile200mm", Definition.Create(-1, false, false, null, false)},
            {"AngleGrinderItem", Definition.Create(-1, false, false, null, false)},
            {"AngleGrinder2Item", Definition.Create(-1, false, false, null, false)},
            {"AngleGrinder3Item", Definition.Create(-1, false, false, null, false)},
            {"HandDrillItem", Definition.Create(-1, false, false, null, false)},
            {"HandDrill2Item", Definition.Create(-1, false, false, null, false)},
            {"HandDrill3Item", Definition.Create(-1, false, false, null, false)},
            {"WelderItem", Definition.Create(-1, false, false, null, false)},
            {"Welder2Item", Definition.Create(-1, false, false, null, false)},
            {"Welder3Item", Definition.Create(-1, false, false, null, false)},
            {"AutomaticRifleItem", Definition.Create(-1, false, false, null, false)},
            {"Scrap", Definition.Create(-1, false, false, null, false)},
            {"OxygenBottle", Definition.Create(-1, false, false, null, false)},
            {"HydrogenBottle", Definition.Create(-1, false, false, null, false)},
            {"Explosives", Definition.Create(-1, false, false, null, false)},
            {"Parachute", Definition.Create(-1, false, false, null, false)},
            {"Canvas", Definition.Create(-1, false, false, null, false)},
            {"UltimateAutomaticRifleItem", Definition.Create(-1, false, false, null, false)},
            {"SpaceCredit", Definition.Create(-1, false, false, null, false)}
        };

        /// <summary>
        /// Sorter cache
        /// </summary>
        public const int SorterCache = 200;

        /// <summary>
        /// Zakladni mnozstvi rafinovane rudy
        /// </summary>
        public const int RefineryAmount = 1000;
    }

    /// <summary>
    /// Definice komponent
    /// </summary>
    internal class Definition
    {
        /// <summary>
        /// Reference
        /// </summary>
        public int Reference;

        /// <summary>
        /// Assembly
        /// </summary>
        public bool Assembly;

        /// <summary>
        /// Cache
        /// </summary>
        public bool Cache;

        /// <summary>
        /// Missing
        /// </summary>
        public bool Missing;

        /// <summary>
        /// Blueprint
        /// </summary>
        public string Blueprint;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="assembly"></param>
        /// <param name="cache"></param>
        /// <param name="blueprint"></param>
        public Definition(int reference, bool assembly, bool cache, string blueprint, bool missing)
        {
            Reference = reference;
            Assembly = assembly;
            Cache = cache;
            Blueprint = blueprint;
            Missing = missing;
        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="assembly"></param>
        /// <param name="cache"></param>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        public static Definition Create(int reference, bool assembly, bool cache, string blueprint, bool missing)
        {
            return new Definition(reference, assembly, cache, blueprint, missing);
        }
    }
}