using System;
using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary>
    /// Tools
    /// </summary>
    class Tools
    {
        /// <summary>
        /// Rouhozeno seznamu
        /// </summary>
        /// <param name="list"></param>
        public static void ShuffleList(List<string> list)
        {
            // nahodne rozhoreni seznamu
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                string value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
