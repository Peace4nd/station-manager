using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;

namespace SpaceEngineers
{
    /// <summary> 
    /// Obecny objekt pro praci s bloky 
    /// </summary> 
    class Block
    {
        /// <summary>
        /// Skupina
        /// </summary>
        protected IMyTerminalBlock current = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block"></param>
        public Block(string block)
        {
            current = Terminal.FindBlock(block);
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block"></param>
        public Block(IMyTerminalBlock block)
        {
            current = block;
        }

        /// <summary> 
        /// Akce 
        /// </summary> 
        /// <param name="action">Akce</param> 
        public void Action(string action)
        {
            current.ApplyAction(action);
        }

        /// <summary> 
        /// Nastaveni hodnoty vlastnosti 
        /// </summary> 
        /// <param name="property">Vlastnost</param> 
        /// <param name="value">Hodnota</param> 
        public void SetValue<V>(string property, V value)
        {
            current.SetValue(property, value);
        }

        /// <summary> 
        /// Ziskani hodnoty vlastnosti 
        /// </summary> 
        /// <param name="property">Vlastnost</param>  
        public V GetValue<V>(string property)
        {
            return current.GetValue<V>(property);
        }

        /// <summary>
        /// Nazev bloku
        /// </summary>
        public string Name
        {
            get {
                if (current.CustomName.Contains("_"))
                {
                    return current.CustomName.Substring(current.CustomName.IndexOf("_") + 1);
                }
                else
                {
                    return current.CustomName;
                }
            }
        }

        /// <summary>
        /// Blok je funkcni a pracuje
        /// </summary>
        public bool IsWorking
        {
            get
            {
                if (current.IsFunctional && current.IsWorking)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Pretypovani
        /// </summary>
        /// <returns></returns>
        public T As<T>()
        {
            if (current is T)
            {
                return (T)current;
            }
            throw new Exception("E-BL-01: Block '" + current.CustomName + "' cannot be converted");
        }

        /// <summary>
        /// Overeni typu
        /// </summary>
        /// <returns></returns>
        public bool Is<T>()
        {
            return current is T;
        }
    }
}