using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary> 
    /// Pamet 
    /// </summary> 
    class Memory
    {
        /// <summary> 
        /// Panel 
        /// </summary> 
        static IMyTextPanel Panel = null;

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void Init(string name)
        {
            // naleyeni
            SortedDictionary<string, IMyTerminalBlock> found = Terminal.Find(name);
            // overeni existence
            if (found.Count == 0)
            {
                throw new Exception("E-ME-01: No memory panel found");
            }
            // prirayeni
            Panel = found[name] as IMyTextPanel;
        }


        //private stringify()
        //private parse()


        /// <summary> 
        /// Zapis do pameti 
        /// </summary> 
        /// <param name="name">Nazev promenne</param> 
        /// <param name="value">Hodnota</param> 
        public static void Store(string name, float value)
        {
            //definice 
            string data = name + "=" + value + "\n";
            string[] rows = Panel.GetText().Trim().Split('\n');
            //sestaveni dat 
            for (int i = 0; i < rows.Length; i++)
            {
                if (!rows[i].Contains(name + "="))
                {
                    data += rows[i] + "\n";
                }
            }
            //zapis
            Panel.WriteText(data, false);
        }

        /// <summary> 
        /// Reset promenne 
        /// </summary> 
        /// <param name="name">Nazev promenne</param> 
        public static void Reset(string name)
        {
            Store(name, 0);
        }

        /// <summary> 
        /// Nacteni promenne 
        /// </summary> 
        /// <param name="name">Nazev</param> 
        /// <returns>Hodnota</returns> 
        public static float Load(string name)
        {
            //nacteni 
            string[] rows = Panel.GetText().Split('\n');
            for (int j = 0; j < rows.Length; j++)
            {
                if (rows[j].IndexOf(name + "=") == 0)
                {
                    string[] data = rows[j].Split('=');
                    return Convert.ToSingle(data[1]);
                }
            }
            return 0;
        }
    }
}