using SpaceEngineers.Game.ModAPI;

namespace SpaceEngineers
{
    /// <summary> 
    /// Reprak 
    /// </summary> 
    class Sound : Block
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block"></param>
        public Sound(string block) : base(block) { }

        /// <summary> 
        /// Zapnuti 
        /// <returns>Objekt svetla</returns> 
        /// </summary> 
        public Sound Play()
        {
            Action("PlaySound");
            return this;
        }

        /// <summary> 
        /// Vypnuti 
        /// <returns>Objekt svetla</returns> 
        /// </summary> 
        public Sound Pause()
        {
            Action("StopSound");
            return this;
        }
    }
}