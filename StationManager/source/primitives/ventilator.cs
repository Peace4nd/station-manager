using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary> 
    /// Ventilator 
    /// </summary> 
    class Ventilator
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static Block Instance = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Ventilator(Block block)
        {
            Instance = block;
        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Ventilator Create(Block block)
        {
            return new Ventilator(block);
        }

        /// <summary> 
        /// Dekomprese mistnosti 
        /// </summary> 
        public void Depressurize()
        {
            Instance.Action<IMyAirVent>("Depressurize_On");
        }

        /// <summary> 
        /// Natlakovani mistnosti 
        /// </summary> 
        public void Pressurize()
        {
            Instance.Action<IMyAirVent>("Depressurize_Off");
        }

        /// <summary> 
        /// Overeni ze je mistnost natlakovana 
        /// </summary> 
        /// <returns>Natlakovani mistnosti</returns> 
        public bool IsPressurized()
        {
            // definice
            bool pressurized = true;
            // prochazeni bloku
            foreach (KeyValuePair<string, IMyAirVent> block in Instance.GetByType<IMyAirVent>())
            {
                if (block.Value.GetOxygenLevel() < 1)
                {
                    pressurized = false;
                }
            }
            // vraceni
            return pressurized;
        }
    }
}