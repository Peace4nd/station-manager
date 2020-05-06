using Sandbox.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary> 
    /// Dvere 
    /// </summary> 
    class Door
    {
        /// <summary>
        /// Instance
        /// </summary>
        private static Block Instance = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Door(Block block)
        {
            Instance = block;
        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Door Create(Block block)
        {
            return new Door(block);
        }

        /// <summary> 
        /// Otevreni dveri 
        /// </summary> 
        public Door Open()
        {
            Instance.Action<IMyDoor>("Open_On");
            return this;
        }

        /// <summary> 
        /// Zavreni dveri 
        /// </summary> 
        public Door Close()
        {
            Instance.Action<IMyDoor>("Open_Off");
            return this;
        }

        /// <summary> 
        /// Zamceni dveri 
        /// </summary> 
        public Door Lock()
        {
            Instance.Action<IMyDoor>("OnOff_Off");
            return this;
        }

        /// <summary> 
        /// Odemceni dveri 
        /// </summary> 
        public Door Unlock()
        {
            Instance.Action<IMyDoor>("OnOff_On");
            return this;
        }

        /// <summary> 
        /// Overeni ze jsou dvere zavrene 
        /// </summary> 
        /// <returns>Zavrene dvere</returns> 
        public bool IsClosed()
        {
            // definice
            int closed = 0;
            // krokovani bloku
            foreach (var block in Instance.GetByType<IMyDoor>())
            {
                if (block.Value.OpenRatio == 0)
                {
                    closed++;
                }
            }
            // rozhodnuti o uzavreni
            if (closed == Instance.Count)
            {
                return true;
            }
            return false;
        }
    }
}