using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary> 
    /// Plyny
    /// </summary> 
    class Gases
    {
        /// <summary> 
        /// Referencni mnozstvi ledu 
        /// </summary> 
        private int IceReference = 0;

        /// <summary>
        /// Instance
        /// </summary>
        private readonly Block Instance = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Gases(string block)
        {
            Instance = new Block(block);
        }

        /// <summary> 
        /// Stav zasobnikuplynu
        /// </summary> 
        /// <returns>Stav zasobniku</returns> 
        public List<string> GetTankStatus()
        {
            // definice
            List<string> status = new List<string>();
            // prochazeni bloku
            foreach (var block in Instance.GetByType<IMyGasTank>())
            {
                status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value), (block.Value as IMyGasTank).FilledRatio, 1));
            }
            return status;

        }

        /// <summary> 
        /// Mnozstvi ledu v generatoru
        /// </summary> 
        /// <returns></returns> 
        public List<string> GetIceStatus()
        {
            // definice
            List<string> status = new List<string>();
            // prochazeni bloku
            foreach (var block in Instance.GetByType<IMyGasGenerator>())
            {
                // definice
                List<MyInventoryItem> items = new List<MyInventoryItem>();
                float total = 0;
                // nacteni polozek v inventari
                block.Value.GetInventory(0).GetItems(items);
                // vypocet celkoveho mnozstvi
                if (items.Count > 0)
                {
                    for (int j = 0; j < items.Count; j++)
                    {
                        if (items[j].Type.SubtypeId == "Ice")
                        {
                            total += (float)items[j].Amount;
                        }
                    }
                }
                // status
                if (block.Value.IsWorking && block.Value.IsFunctional)
                {
                    status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value), total, IceReference));
                }
            }
            // vraceni
            return status;
        }

        /// <summary> 
        /// Nastaveni referencniho mnozstvi ledu 
        /// </summary> 
        /// <param name="reference">Referencni mnozstvi</param> 
        /// <returns></returns> 
        public Gases SetIceReference(int reference)
        {
            if (reference > 0)
            {
                IceReference = reference;
            }
            return this;
        }
    }
}