using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary> 
    /// Plyny
    /// </summary> 
    class Gases
    {
        /// <summary>
        /// Instance
        /// </summary>
        private readonly Group current = null;

        /// <summary>
        /// Reference mnozstvi uranu
        /// </summary>
        private double reference = 0;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="group">Blok</param>
        public Gases(string group)
        {
            current = new Group(group);
        }


        /// <summary>
        /// Nastaveni referencni hodnoty ledu
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Gases SetIceReference(double amount)
        {
            reference = amount;
            return this;
        }

        /// <summary> 
        /// Stav zasobniku plynu
        /// </summary> 
        /// <returns>Stav zasobniku</returns> 
        public List<string> GetStatus()
        {
            // definice
            List<string> status = new List<string>();
            // prochazeni bloku
            foreach (var block in current)
            {
                if (block.Is<IMyGasGenerator>())
                {
                    var items = Tools.GetInventoryInfo(block.As<IMyGasGenerator>().GetInventory(0));
                    status.Add(Formater.BarsWithPercent(block.Name, items.GetValueOrDefault("Ice", (0, -1)).Amount, reference));
                }
                else if (block.Is<IMyGasTank>())
                {
                    status.Add(Formater.BarsWithPercent(block.Name, block.As<IMyGasTank>().FilledRatio, 1));
                }
                else
                {
                    throw new Exception("E-GA-01: Block '" + block.Name + "' is neither GasTank or GasGenerator");
                }
            }
            return status;
        }
    }
}