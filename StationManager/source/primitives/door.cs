using Sandbox.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary> 
    /// Dvere 
    /// </summary> 
    class Door : Block
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block"></param>
        public Door(string block) : base(block) { }

        /// <summary> 
        /// Otevreni dveri 
        /// </summary> 
        public Door Open()
        {
            Action("Open_On");
            return this;
        }

        /// <summary> 
        /// Zavreni dveri 
        /// </summary> 
        public Door Close()
        {
            Action("Open_Off");
            return this;
        }

        /// <summary> 
        /// Zamceni dveri 
        /// </summary> 
        public Door Lock()
        {
            Action("OnOff_Off");
            return this;
        }

        /// <summary> 
        /// Odemceni dveri 
        /// </summary> 
        public Door Unlock()
        {
            Action("OnOff_On");
            return this;
        }

        /// <summary> 
        /// Overeni ze jsou dvere zavrene 
        /// </summary> 
        /// <returns>Zavrene dvere</returns> 
        public bool IsClosed()
        {
            return As<IMyDoor>().OpenRatio == 0;
        }
    }
}