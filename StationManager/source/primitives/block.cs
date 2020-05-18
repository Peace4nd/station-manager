using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary> 
    /// Obecny objekt pro praci s bloky 
    /// </summary> 
    class Block
    {
        /// <summary> 
        /// Akce 
        /// </summary> 
        /// <param name="block">Blok</param> 
        /// <param name="action">Akce</param> 
        public static void Action(IMyTerminalBlock block, string action)
        {
            // block.GetActionWithName(action).Apply(block);
            block.ApplyAction(action);
        }

        /// <summary> 
        /// Nastaveni hodnoty vlastnosti 
        /// </summary> 
        /// <param name="block">Blok</param> 
        /// <param name="property">Vlastnost</param> 
        /// <param name="value">Hodnota</param> 
        public static void SetValue<V>(IMyTerminalBlock block, string property, V value)
        {
            block.SetValue(property, value);
        }

        /// <summary> 
        /// Ziskani hodnoty vlastnosti 
        /// </summary> 
        /// <param name="block">Blok</param> 
        /// <param name="property">Vlastnost</param>  
        public static V GetValue<V>(IMyTerminalBlock block, string property)
        {
            return block.GetValue<V>(property);
        }

        /// <summary> 
        /// Nazev bloku 
        /// </summary> 
        /// <param name="block">Blok</param> 
        /// <returns>Nazev bloku</returns> 
        public static string GetName(IMyTerminalBlock block)
        {
            if (block.CustomName.Contains("_"))
            {
                return block.CustomName.Substring(block.CustomName.IndexOf("_") + 1);
            }
            else
            {
                return block.CustomName;
            }
        }

        /// <summary> 
        /// Status bloku 
        /// </summary> 
        /// <param name="block">Blok</param> 
        /// <param name="status">Stav bloku</param>
        /// <returns>Nazev bloku</returns> 
        public static string GetStatus(IMyTerminalBlock block, string status)
        {
            return status + (new string(' ', (Constants.ColumnStatus - status.Length))) + " " + GetName(block);
        }

        /// <summary> 
        /// Priprava nazvu bloku 
        /// </summary> 
        /// <param name="block">Blok</param> 
        /// <returns>Nazev bloku</returns> 
        public static string GetStatus(IMyTerminalBlock block)
        {
            if (block.IsFunctional && block.IsWorking)
            {
                return GetStatus(block, Status.Working);
            }
            else
            {
                return GetStatus(block, Status.NotWorking);
            }
        }

        /// <summary> 
        /// Pole objektu bloku 
        /// </summary> 
        private readonly SortedDictionary<string, IMyTerminalBlock> Found = new SortedDictionary<string, IMyTerminalBlock>();

        /// <summary>
        /// Nazev bloku
        /// </summary>
        private readonly string Name = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Block(string block)
        {
            Debugger.Log("Loaded from name '" + block + "*'");
            Found = Terminal.Find(block);
            Name = block + "_";
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Block(IMyTerminalBlock block)
        {
            Debugger.Log("Creating from block '" + block.CustomName + "'");
            Found.Add(block.CustomName, block);
        }

        /// <summary> 
        /// Overeni existence bloku 
        /// </summary> 
        /// <param name="name"></param> 
        /// <returns></returns> 
        public bool Exist(string block)
        {
            if (Found.Count > 0)
            {
                return Found.ContainsKey(Name + block);
            }
            return false;
        }

        /// <summary> 
        /// Overeni existence bloku 
        /// </summary> 
        /// <param name="name"></param> 
        /// <returns></returns> 
        public bool Exist<T>()
        {
            if (Found.Count > 0)
            {
                foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
                {
                    if (block.Value is T)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Ziskani bloku dle typu
        /// </summary>
        /// <param name="mustExist"></param>
        /// <returns></returns>
        public SortedDictionary<string, T> GetByType<T>(bool mustExist = false) where T : IMyTerminalBlock
        {
            // definice
            SortedDictionary<string, T> typed = new SortedDictionary<string, T>();
            // vyfiltrovani prislusneho typu
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                if (block.Value is T)
                {
                    typed.Add(block.Key, (T)block.Value);
                }
            }
            // overeni existence
            if (typed.Count == 0 && mustExist)
            {
                throw new Exception("E-BL-FI-01: Block doesn't exists");
            }
            // vraceni
            return typed;
        }

        /// <summary>
        /// Ziskani seznamu bloku dle typu
        /// </summary>
        /// <param name="mustExist"></param>
        /// <returns></returns>
        public List<T> GetListByType<T>(bool mustExist = false) where T : IMyTerminalBlock
        {
            // definice
            List<T> typed = new List<T>();
            // vyfiltrovani prislusneho typu
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                if (block.Value is T)
                {
                    typed.Add((T)block.Value);
                }
            }
            // overeni existence
            if (typed.Count == 0 && mustExist)
            {
                throw new Exception("E-BL-FI-02: Block doesn't exists");
            }
            // vraceni
            return typed;
        }

        /// <summary>
        /// Ziskani bloku dle jmeno
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="mustExist"></param>
        /// <returns></returns>
        public SortedDictionary<string, T> GetByName<T>(string name, bool mustExist = false) where T : IMyTerminalBlock
        {
            // definice
            SortedDictionary<string, T> typed = new SortedDictionary<string, T>();
            // vyfiltrovani podle jmena
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                if (block.Key.Contains(name) && block.Value is T)
                {
                    typed.Add(block.Key, (T)block.Value);
                }
            }
            // overeni existence
            if (typed.Count == 0 && mustExist)
            {
                throw new Exception("E-BL-FI-03: Block doesn't exists");
            }
            // vraceni
            return typed;
        }

        /// <summary>
        /// Ziskani bloku dle jmeno
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="mustExist"></param>
        /// <returns></returns>
        public SortedDictionary<string, IMyTerminalBlock> GetByName(string name, bool mustExist = false)
        {
            return GetByName<IMyTerminalBlock>(name, mustExist);
        }

        /// <summary>
        /// Ziskani seznamu bloku dle jmena
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="mustExist"></param>
        /// <returns></returns>
        public List<T> GetListByName<T>(string name, bool mustExist = false) where T : IMyTerminalBlock
        {
            // definice
            List<T> typed = new List<T>();
            // vyfiltrovani podle jmena
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                if (block.Key.Contains(name) && block.Value is T)
                {
                    typed.Add((T)block.Value);
                }
            }
            // overeni existence
            if (typed.Count == 0 && mustExist)
            {
                throw new Exception("E-BL-FI-04: Block '" + name + "' doesn't exists");
            }
            // vraceni
            return typed;
        }

        /// <summary>
        /// Ziskani seznamu  bloku dle jmena
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="mustExist"></param>
        /// <returns></returns>
        public List<IMyTerminalBlock> GetListByName(string name, bool mustExist = false)
        {
            return GetListByName<IMyTerminalBlock>(name, mustExist);
        }


        /// <summary>
        /// Existujici bloky
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, IMyTerminalBlock> GetAll()
        {
            return Found;
        }

        /// <summary>
        /// Nazvy existujicich bloku
        /// </summary>
        /// <returns></returns>
        public List<string> Keys
        {
            get
            {
                List<string> names = new List<string>(Found.Keys);
                names.Sort();
                return names;
            }
        }

        /// <summary>
        /// Pocet nalezenych bloku
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get { return Found.Count; }
        }

        /// <summary> 
        /// Akce 
        /// </summary> 
        /// <param name="action">Akce</param> 
        public void Action(string action)
        {
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                Block.Action(block.Value, action);
            }
        }

        /// <summary> 
        /// Akce 
        /// </summary> 
        /// <param name="action">Akce</param> 
        public void Action<T>(string action)
        {
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                if (block.Value is T)
                {
                    Block.Action(block.Value, action);
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
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                Block.SetValue<V>(block.Value, property, value);
            }
        }

        /// <summary> 
        /// Nastaveni hodnoty vlastnosti 
        /// </summary> 
        /// <param name="property">Vlastnost</param> 
        /// <param name="value">Hodnota</param> 
        public void SetValue<T, V>(string property, V value)
        {
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                if (block.Value is T)
                {
                    Block.SetValue<V>(block.Value, property, value);
                }
            }
        }

        /// <summary> 
        /// Ziskani hodnoty vlastnosti 
        /// </summary> 
        /// <param name="property">Vlastnost</param>  
        public Dictionary<string, V> GetValue<V>(string property)
        {
            // definice
            Dictionary<string, V> values = new Dictionary<string, V>();
            // naplneni
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                values.Add(block.Key, Block.GetValue<V>(block.Value, property));
            }
            // vraceni
            return values;
        }

        /// <summary> 
        /// Ziskani hodnoty vlastnosti 
        /// </summary> 
        /// <param name="property">Vlastnost</param>  
        public Dictionary<string, V> GetValue<T, V>(string property)
        {
            // definice
            Dictionary<string, V> values = new Dictionary<string, V>();
            // naplneni
            foreach (KeyValuePair<string, IMyTerminalBlock> block in Found)
            {
                if (block.Value is T)
                {
                    values.Add(block.Key, Block.GetValue<V>(block.Value, property));
                }
            }
            // vraceni
            return values;
        }
    }
}