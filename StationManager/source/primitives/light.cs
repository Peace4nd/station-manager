using SpaceEngineers.Game.ModAPI;
using System;
using VRageMath;

namespace SpaceEngineers
{
    /// <summary> 
    /// Svetlo 
    /// </summary> 
    class Light : Group
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block"></param>
        public Light(string block) : base(block) { }

        /// <summary> 
        /// Zapnuti 
        /// <returns>Objekt svetla</returns> 
        /// </summary> 
        public Light On()
        {
            Action("OnOff_On");
            return this;
        }

        /// <summary> 
        /// Vypnuti 
        /// <returns>Objekt svetla</returns> 
        /// </summary> 
        public Light Off()
        {
            Action("OnOff_Off");
            return this;
        }

        /// <summary>
        /// Bila
        /// </summary>
        /// <returns>Light</returns> 
        public Light White()
        {
            SetValue<Color>("Color", Color.White);
            return this;
        }

        /// <summary>
        /// Cervena
        /// </summary>
        /// <returns>Light</returns> 
        public Light Red()
        {
            SetValue<Color>("Color", Color.Red);
            return this;
        }

        /// <summary>
        /// Zelena
        /// </summary>
        /// <returns>Light</returns> 
        public Light Green()
        {
            SetValue<Color>("Color", Color.Green);
            return this;
        }

        /// <summary>
        /// Modra
        /// </summary>
        /// <returns>Light</returns> 
        public Light Blue()
        {
            SetValue<Color>("Color", Color.Blue);
            return this;
        }

        /// <summary>
        /// Zluta
        /// </summary>
        /// <returns>Light</returns> 
        public Light Yellow()
        {
            SetValue<Color>("Color", Color.Yellow);
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
            SetValue<float>("Blink Interval", interval);
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
            SetValue<float>("Intensity", intensity);
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
            SetValue<float>("Radius", radius);
            // fluent
            return this;
        }
    }
}