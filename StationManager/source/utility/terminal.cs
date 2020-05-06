using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary>
    /// Terminal
    /// </summary>
    class Terminal
    {
        /// <summary>
        /// Terminal
        /// </summary>
        private static IMyGridTerminalSystem terminal = null;

        /// <summary>
        /// /// Nastaveni terminaloveho systemu
        /// </summary>
        /// <param name="gts">Terminal</param>
        public static void Init(IMyGridTerminalSystem gts)
        {
            terminal = gts;
        }

        /// <summary>
        /// Nalezeni bloku
        /// </summary>
        /// <param name="name">Nazev</param>
        /// <returns></returns>
        public static Dictionary<string, IMyTerminalBlock> Find(string name)
        {
            // debugger
            Debugger.Log("Looking for block '" + name + "'");
            // definice
            List<IMyTerminalBlock> searched = new List<IMyTerminalBlock>();
            Dictionary<string, IMyTerminalBlock> blocks = new Dictionary<string, IMyTerminalBlock>();
            // nalezeni bloku
            terminal.SearchBlocksOfName(name, searched);
            // debugger
            Debugger.Log("Found blocks", searched);
            // prirazeni do slovniku
            if (searched.Count > 0)
            {
                foreach (var search in searched)
                {
                    // kontrola ze block zacina pozadovanym nazvem
                    if (!search.CustomName.StartsWith(name))
                    {
                        continue;
                    }
                    // kontrola unikatnosti
                    if (blocks.ContainsKey(search.CustomName))
                    {
                        throw new Exception("E-TE-01: Block '" + name + "' is not unique");
                    }
                    // kontrola poyadovaneho typu
                    blocks.Add(search.CustomName, search);
                }
                // debugger
                Debugger.Log("Filtered blocks", blocks);
            }
            else
            {
                throw new Exception("E-TE-02: Block '" + name + "' doesn't exist");
            }
            // vraceni
            return blocks;
        }
    }
}