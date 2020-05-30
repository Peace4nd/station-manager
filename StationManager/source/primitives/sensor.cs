using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary> 
    /// Sensor 
    /// </summary> 
    class Sensor
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static Block Instance = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Sensor(Block block)
        {
            Instance = block;
        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Sensor Create(Block block)
        {
            return new Sensor(block);
        }

        /// <summary>
        /// Nastaveni rozsahu senzoru
        /// </summary>
        /// <param name="ranges"></param>
        /// <returns></returns>
        public Sensor Range(SensorRange ranges)
        {
            foreach (KeyValuePair<string, float> range in ranges)
            {
                Instance.SetValue<IMySensorBlock, float>(range.Key, range.Value);
            }
            return this;
        }

        /// <summary>
        /// Nastaveni detekovanych objektu
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public Sensor Detect(SensorDetect objects)
        {
            foreach (KeyValuePair<string, bool> obj in objects)
            {
                Instance.SetValue<IMySensorBlock, bool>(obj.Key, obj.Value);
            }
            return this;
        }

        public void GetEntitiews()
        {
            foreach (KeyValuePair<string, IMySensorBlock> block in Instance.GetByType<IMySensorBlock>())
            {
                List<MyDetectedEntityInfo> items = new List<MyDetectedEntityInfo>();
                block.Value.DetectedEntities(items);

                /*
            block.Value.DetectEnemy
            block.Value.DetectFloatingObjects
            block.Value.DetectFriendly
            block.Value.DetectLargeShips
            block.Value.DetectNeutral
            block.Value.DetectOwner
            block.Value.DetectPlayers
            block.Value.DetectSmallShips
            block.Value.DetectStations
            block.Value.DetectSubgrids
                */

            }


        }
    }