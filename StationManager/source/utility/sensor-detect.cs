using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary> 
    /// Sensor range
    /// </summary> 
    class SensorDetect
    {
        /// <summary>
        /// Definice rozsahu
        /// </summary>
        private readonly Dictionary<string, bool> Objects = new Dictionary<string, bool>() {
            { "DetectAsteroids", false },
            { "DetectEnemy", false },
            { "DetectFloatingObjects", false },
            { "DetectFriendly", false },
            { "DetectLargeShips", false },
            { "DetectNeutral", false },
            { "DetectOwner", false },
            { "DetectPlayers", false },
            { "DetectSmallShips", false },
            { "DetectStations", false },
            { "DetectSubgrids", false }
        };

        /// <summary>
        /// Konstruktor
        /// </summary>
        public SensorDetect()
        {

        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <returns></returns>
        public static SensorDetect Create()
        {
            return new SensorDetect();
        }

        /// <summary>
        /// Pridani noveho rozsahu
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="detect"></param>
        /// <returns></returns>
        public SensorDetect Add(SensorDetectEnum obj, bool detect)
        {
            Objects["Detect" + obj] = detect;
            return this;
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, bool>.Enumerator GetEnumerator()
        {
            return Objects.GetEnumerator();
        }
    }
}

