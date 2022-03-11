using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary> 
    /// Ventilator 
    /// </summary> 
    class Ventilator : Block
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block"></param>
        public Ventilator(string block) : base(block) { }

        /// <summary> 
        /// Dekomprese mistnosti 
        /// </summary> 
        public void Depressurize()
        {
            Action("Depressurize_On");
        }

        /// <summary> 
        /// Natlakovani mistnosti 
        /// </summary> 
        public void Pressurize()
        {
            Action("Depressurize_Off");
        }

        /// <summary> 
        /// Overeni ze je mistnost natlakovana 
        /// </summary> 
        /// <returns>Natlakovani mistnosti</returns> 
        public bool IsPressurized()
        {
            return As<IMyAirVent>().GetOxygenLevel() == 1;
        }
    }
}