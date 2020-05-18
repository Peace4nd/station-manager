using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary>
    /// Debugger
    /// </summary>
    class Debugger
    {
        /// <summary>
        /// Echo
        /// </summary>
        private static Action<string> Echo;

        /// <summary>
        /// Povoleni / zakazani
        /// </summary>
        private static bool Enabled = false;

        /// <summary>
        /// Nastaveni echovace
        /// </summary>
        /// <param name="echo"></param>
        public static void SetEcho(Action<string> echo)
        {
            Echo = echo;
        }

        /// <summary>
        /// Povoleni
        /// </summary>
        public static void Enable()
        {
            Enabled = true;
        }

        /// <summary>
        /// Chyba
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            Log(">>> ERROR: " + message);
        }

        /// <summary>
        /// Varovani
        /// </summary>
        /// <param name="message"></param>
        public static void Warning(string message)
        {
            Log(">>> WARNING: " + message);
        }

        /// <summary>
        /// Zapis logu
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            if (Enabled)
            {
                Echo(message + "\n");
            }
        }

        /// <summary>
        /// Zapis logu
        /// </summary>
        /// <param name="message"></param>
        /// <param name="blocks"></param>
        public static void Log(string message, List<IMyTerminalBlock> blocks)
        {
            // definice
            List<string> names = new List<string>();
            // premapovani bloku
            foreach (IMyTerminalBlock block in blocks)
            {
                names.Add(block.CustomName);
            }
            // zapis
            Log(message, names);
        }

        /// <summary>
        /// Zapis logu
        /// </summary>
        /// <param name="message"></param>
        /// <param name="blocks"></param>
        public static void Log(string message, SortedDictionary<string, IMyTerminalBlock> blocks)
        {
            Log(message, new List<string>(blocks.Keys));
        }

        /// <summary>
        /// Zapis logu
        /// </summary>
        /// <param name="message"></param>
        /// <param name="strings"></param>
        public static void Log(string message, List<string> strings)
        {
            Log(message + "\n----------\n" + string.Join("\n", strings) + "\n");
        }
    }
}
