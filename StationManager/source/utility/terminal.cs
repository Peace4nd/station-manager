using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;

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
        /// Overeni pristupnosti bloku
        /// </summary>
        /// <param name="block">Blok</param>
        /// <returns></returns>
        public static bool CanAccess(IMyTerminalBlock block)
        {
            // debugger
            Debugger.Log("Checking accessibility of block '" + block.CustomName + "'");
            // nalezeni bloku
            return terminal.CanAccess(block);
        }

        /// <summary>
        /// Nalezeni bloku
        /// </summary>
        /// <param name="name">Nazev</param>
        /// <returns></returns>
        public static IMyTerminalBlock FindBlock(string name)
        {
            // debugger
            Debugger.Log("Looking for block '" + name + "'");
            // nalezeni bloku
            var block = terminal.GetBlockWithName(name);
            // prirazeni do slovniku
            if (block == null)
            {
                throw new Exception("E-TE-01: Block '" + name + "' doesn't exist");
            }
            // vraceni
            return block;
        }

        /// <summary>
        /// Nalezeni skupiny bloku
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<IMyTerminalBlock> FindGroup(string name)
        {
            // debugger
            Debugger.Log("Looking for blocks '" + name + "'");
            // definice
            List<IMyTerminalBlock> result = new List<IMyTerminalBlock>();
            // nalezeni skupiny
            var group = terminal.GetBlockGroupWithName(name);
            // pokud existuje skupina pracujeme s ni, jinak se vyhledaji bloky podle jmena
            if (group != null)
            {
                // ziskani bloku
                group.GetBlocks(result);
                // overeni 
                if (result.Count == 0)
                {
                    throw new Exception("E-TE-02: Group '" + name + "' is empty");
                }
            }
            else
            {
                // nalezeni bloku
                terminal.SearchBlocksOfName(name, result, (block) => { return block.CustomName.StartsWith(name); });
                // prirazeni do slovniku
                if (result.Count == 0)
                {
                    throw new Exception("E-TE-03: No block named '" + name + "' has been found");
                }
            }
            // debugger
            Debugger.Log("Found blocks", result);
            // vraceni
            return result;
        }
    }
}