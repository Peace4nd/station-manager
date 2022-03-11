using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary>
    /// Tools
    /// </summary>
    class Tools
    {
        /// <summary>
        /// Rouhozeno seznamu
        /// </summary>
        /// <param name="list"></param>
        public static void ShuffleList(List<string> list)
        {
            // nahodne rozhoreni seznamu
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                string value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Ziskani definice komponenty
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Definition GetComponentDefinition(string name)
        {
            return Constants.Components.GetValueOrDefault(name, Definition.Create(-1, false, false, null, false));
        }

        /// <summary>
        /// Nacteni polozek inventare
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public static List<(string Name, double Amount, int Index)> GetInventoryList(IMyInventory inventory)
        {
            // definice
            List<MyInventoryItem> loaded = new List<MyInventoryItem>();
            List<(string Name, double Amount, int Index)> result = new List<(string Name, double Amount, int Index)>();
            /// nacteni inventare
            inventory.GetItems(loaded);
            // vypocet celkoveho mnozstvi
            for (int i = 0; i < loaded.Count; i++)
            {
                result.Add((loaded[i].Type.SubtypeId, (double)loaded[i].Amount, i));
            }
            // vraceni
            return result;
        }

        /// <summary>
        /// Nacteni polozek inventare
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static List<(string Name, double Amount, int Index)> GetInventoryList(Block block, InventoryType type = InventoryType.Output)
        {
            if (block.Is<IMyCargoContainer>())
            {
                return GetInventoryList(block.As<IMyCargoContainer>().GetInventory(0));
            }
            else if (block.Is<IMyProductionBlock>())
            {
                switch (type)
                {
                    case InventoryType.Input:
                        return GetInventoryList(block.As<IMyProductionBlock>().InputInventory);
                    case InventoryType.Output:
                        return GetInventoryList(block.As<IMyProductionBlock>().OutputInventory);
                }
            }
            throw new Exception("E-TL-01: Block '" + block.Name + "' doesn't containt inventory!");
        }

        /// <summary>
        /// Nacteni polozek inventare
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public static Dictionary<string, (double Amount, int Index)> GetInventoryInfo(IMyInventory inventory)
        {
            // definice
            List<MyInventoryItem> loaded = new List<MyInventoryItem>();
            Dictionary<string, (double Amount, int Index)> result = new Dictionary<string, (double Amount, int Index)>();
            /// nacteni inventare
            inventory.GetItems(loaded);
            // vypocet celkoveho mnozstvi
            for (int i = 0; i < loaded.Count; i++)
            {
                result.Add(loaded[i].Type.SubtypeId, ((double)loaded[i].Amount, i));
            }
            // vraceni
            return result;
        }

        /// <summary>
        /// Nacteni polozek inventare
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Dictionary<string, (double Amount, int Index)> GetInventoryInfo(Block block, InventoryType type = InventoryType.Output)
        {
            if (block.Is<IMyCargoContainer>())
            {
                return GetInventoryInfo(block.As<IMyCargoContainer>().GetInventory(0));
            }
            else if (block.Is<IMyProductionBlock>())
            {
                switch (type)
                {
                    case InventoryType.Input:
                        return GetInventoryInfo(block.As<IMyProductionBlock>().InputInventory);
                    case InventoryType.Output:
                        return GetInventoryInfo(block.As<IMyProductionBlock>().OutputInventory);
                }
            }
            throw new Exception("E-TL-02: Block '" + block.Name + "' doesn't containt inventory!");
        }
    }
}
