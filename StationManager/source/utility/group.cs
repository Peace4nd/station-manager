using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary> 
    /// Skupina bloku
    /// </summary> 
    class Group : IEnumerable<Block>
    {
        /// <summary>
        /// Skupina
        /// </summary>
        protected List<Block> current = new List<Block>();

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="group"></param>
        public Group(string group)
        {
            foreach (var block in Terminal.FindGroup(group))
            {
                current.Add(new Block(block));
            }
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Block> GetEnumerator()
        {
            foreach (var block in current)
            {
                yield return block;
            }
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary> 
        /// Akce 
        /// </summary> 
        /// <param name="action">Akce</param> 
        public void Action(string action)
        {
            foreach (var block in current)
            {
                block.Action(action);
            }
        }

        /// <summary> 
        /// Akce 
        /// </summary> 
        /// <param name="action">Akce</param> 
        public void Action<T>(string action)
        {
            foreach (var block in current)
            {
                if (block.Is<T>())
                {
                    block.Action(action);
                }
            }
        }

        /// <summary> 
        /// Nastaveni hodnoty vlastnosti 
        /// </summary> 
        /// <param name="property">Vlastnost</param> 
        /// <param name="value">Hodnota</param> 
        public void SetValue<V>(string property, V value)
        {
            foreach (var block in current)
            {
                block.SetValue(property, value);
            }
        }

        /// <summary> 
        /// Nastaveni hodnoty vlastnosti 
        /// </summary> 
        /// <param name="property">Vlastnost</param> 
        /// <param name="value">Hodnota</param> 
        public void SetValue<T, V>(string property, V value)
        {
            foreach (var block in current)
            {
                if (block.Is<T>())
                {
                    block.SetValue(property, value);
                }
            }
        }

        /// <summary>
        /// Yiskani vsech bloku
        /// </summary>
        /// <returns></returns>
        public List<Block> All()
        {
            return current;
        }

        /// <summary>
        /// Prvni blok
        /// </summary>
        /// <returns></returns>
        public Block First()
        {
            return current[0];
        }

        /// <summary>
        /// Vyber pouze konkretniho typu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<Block> Only<T>()
        {
            // definice
            List<Block> typed = new List<Block>();
            // pretypovani
            foreach (var block in current)
            {
                if (block.Is<T>())
                {
                    typed.Add(block);
                }
            }
            // vraceni
            return typed;
        }

        /// <summary>
        /// Pocet bloku ve skupine
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get { return current.Count; }
        }
    }
}