using Sandbox.ModAPI.Ingame;
using System;

namespace SpaceEngineers
{
    /// <summary> 
    /// Majak 
    /// </summary> 
    class Beacon : Block
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block"></param>
        public Beacon(string block) : base(block) { }

        /// <summary> 
        /// Dekomprese mistnosti 
        /// </summary> 
        public Beacon On()
        {
            Action("OnOff_On");
            return this;
        }

        /// <summary> 
        /// Natlakovani mistnosti 
        /// </summary> 
        public Beacon Off()
        {
            Action("OnOff_Off");
            return this;
        }
    }
}