using System;
using VRageMath;
using SpaceEngineers.Game.ModAPI;

namespace SpaceEngineers
{
    /// <summary> 
    /// Svetla 
    /// </summary> 
    class Light
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static Block Instance = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Light(Block block) {
            Instance = block;
        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Light Create(Block block)
        {
            return new Light(block);
        }

        /// <summary> 
        /// Zapnuti 
        /// <returns>Objekt svetla</returns> 
        /// </summary> 
        public Light On()
        {
            Instance.Action<IMyInteriorLight>("OnOff_On");
            return this;
        }

        /// <summary> 
        /// Vypnuti 
        /// <returns>Objekt svetla</returns> 
        /// </summary> 
        public Light Off()
        {
            Instance.Action<IMyInteriorLight>("OnOff_On");
            return this;
        }

        /// <summary>
        /// Bila
        /// </summary>
        /// <returns>Light</returns> 
        public Light White()
        {
            Instance.SetValue<IMyInteriorLight, Color>("Color", Color.White);
            return this;
        }

        /// <summary>
        /// Cervena
        /// </summary>
        /// <returns>Light</returns> 
        public Light Red()
        {
            Instance.SetValue<IMyInteriorLight, Color>("Color", Color.Red);
            return this;
        }

        /// <summary>
        /// Zelena
        /// </summary>
        /// <returns>Light</returns> 
        public Light Green()
        {
            Instance.SetValue<IMyInteriorLight, Color>("Color", Color.Green);
            return this;
        }

        /// <summary>
        /// Modra
        /// </summary>
        /// <returns>Light</returns> 
        public Light Blue()
        {
            Instance.SetValue<IMyInteriorLight, Color>("Color", Color.Blue);
            return this;
        }

        /// <summary>
        /// Zluta
        /// </summary>
        /// <returns>Light</returns> 
        public Light Yellow()
        {
            Instance.SetValue<IMyInteriorLight, Color>("Color", Color.Yellow);
            return this;
        }

        /// <summary> 
        /// Nastaveni intervalu blikani 
        /// </summary> 
        /// <param name="interval">Interval</param> 
        /// <returns>Objekt svetla</returns> 
        public Light Blink(float interval = 1)
        {
            // overeni intervalu
            if (interval < 1)
            {
                throw new Exception("E-LI-01: Blick interval can't be less then one");
            }
            // nastaveni
            Instance.SetValue<IMyInteriorLight, float>("Blink Interval", interval);
            // fluent
            return this;
        }

        /// <summary> 
        /// Nastaveni intenzity 
        /// </summary> 
        /// <param name="intensity">Intenzita</param> 
        /// <returns>Objekt svetla</returns> 
        public Light Intensity(float intensity)
        {
            // overeni intenzity
            if (intensity < 0)
            {
                throw new Exception("E-LI-02: Intensity must be greater then zero");
            }
            // nastaveni
            Instance.SetValue<IMyInteriorLight, float>("Intensity", intensity);
            // fluent
            return this;
        }

        /// <summary> 
        /// Nastaveni polomeru zareni 
        /// </summary> 
        /// <param name="radius">Polomer</param> 
        /// <returns>Objekt svetla</returns> 
        public Light Radius(float radius)
        {
            // overeni polomeru
            if (radius < 0)
            {
                throw new Exception("E-LI-03: Radius must be greater then zero");
            }
            // nastaveni
            Instance.SetValue<IMyInteriorLight, float>("Radius", radius);
            // fluent
            return this;
        }
    }
}