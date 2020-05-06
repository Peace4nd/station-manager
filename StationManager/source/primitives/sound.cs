using SpaceEngineers.Game.ModAPI;

namespace SpaceEngineers
{
    /// <summary> 
    /// Reprak 
    /// </summary> 
    class Sound
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static Block Instance = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Sound(Block block) {
            Instance = block;
        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Sound Create(Block block)
        {
            return new Sound(block);
        }

        /// <summary> 
        /// Zapnuti 
        /// <returns>Objekt svetla</returns> 
        /// </summary> 
        public Sound Play()
        {
            Instance.Action<IMySoundBlock>("PlaySound");
            return this;
        }

        /// <summary> 
        /// Vypnuti 
        /// <returns>Objekt svetla</returns> 
        /// </summary> 
        public Sound Pause()
        {
            Instance.Action<IMyInteriorLight>("StopSound");
            return this;
        }
    }
}