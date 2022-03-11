using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary> 
    /// Prechodove komory
    /// </summary> 
    class Airlock
    {
        /// <summary>
        /// Seznam hlidanych dveri
        /// </summary>
        private readonly List<(Door Entrance, Door Exit)> doors = new List<(Door Entrance, Door Exit)>();

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Airlock() { }

        /// <summary>
        /// Pridani dveri
        /// </summary>
        /// <param name="entrance"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public Airlock AddDoor(string entrance, string exit)
        {
            doors.Add((new Door(entrance), new Door(exit)));
            return this;
        }

        /// <summary>
        /// Hlidani prechodovzch komor
        /// </summary>
        /// <returns></returns>
        public Airlock Watch()
        {
            foreach (var (Entrance, Exit) in doors)
            {
                // blokace vystupu
                if (!Entrance.IsClosed())
                {
                    Exit.Lock();
                }
                else
                {
                    Exit.Unlock();
                }
                // blokace vstupu
                if (!Exit.IsClosed())
                {
                    Entrance.Lock();
                }
                else
                {
                    Entrance.Unlock();
                }
            }
            // fluent
            return this;
        }
    }
}