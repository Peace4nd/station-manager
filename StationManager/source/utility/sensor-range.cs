using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary> 
    /// Sensor range
    /// </summary> 
    class SensorRange
    {
        /// <summary>
        /// Definice rozsahu
        /// </summary>
        private readonly Dictionary<string, float> Ranges = new Dictionary<string, float>() {
            { "BackExtend", 0 },
            { "BottomExtend", 0 },
            { "FrontExtend", 0 },
            { "LeftExtend" , 0},
            { "RightExtend" , 0},
            { "TopExtend", 0 }
        };

        /// <summary>
        /// Konstruktor
        /// </summary>
        public SensorRange()
        {

        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <returns></returns>
        public static SensorRange Create()
        {
            return new SensorRange();
        }

        /// <summary>
        /// Pridani noveho rozsahu
        /// </summary>
        /// <param name="range"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SensorRange Add(SensorRangeEnum range, float value)
        {
            Ranges[range + "Extend"] = value;
            return this;
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, float>.Enumerator GetEnumerator()
        {
            return Ranges.GetEnumerator();
        }
    }
}

