using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary> 
    /// Tezba 
    /// </summary> 
    class Mine
    {
        /// <summary> 
        /// Storage 
        /// </summary> 
        private Block Storage = null;

        /// <summary>
        /// Beacon
        /// </summary>
        private Block Beacon = null;

        /// <summary>
        /// Beacon
        /// </summary>
        private Group Switch = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Mine() { }

        /// <summary>
        /// Pridani uloziste
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Mine AddStorage(string name)
        {
            Storage = new Block(name);
            return this;
        }

        /// <summary>
        /// Pridani majaku
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Mine AddBeacon(string name)
        {
            Beacon = new Block(name);
            return this;
        }

        /// <summary>
        /// Pridani skupiny k vypnuti
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Mine AddStartStop(string name)
        {
            Switch = new Group(name);
            return this;
        }

        /// <summary>
        /// Spusteni sledovani
        /// </summary>
        public void Watch()
        {
            if (Storage.As<IMyCargoContainer>().GetInventory(0).IsFull)
            {
                Switch.Action("OnOff_Off");
                Beacon.Action("OnOff_On");
            }
        }
    }
}