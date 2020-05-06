using System;
using System.Collections.Generic;
using VRageMath;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System.Linq;

namespace SpaceEngineers
{
    /// <summary> 
    /// Prace s displayem 
    /// </summary> 
    class Display
    {
        /// <summary> 
        /// Velikost pisma 
        /// </summary> 
        private float FontSize = 1;

        /// <summary> 
        /// Velikost pisma 
        /// </summary> 
        private Color FontColor = Color.White;

        /// <summary> 
        /// Pocitadlo radku na panelu 
        /// </summary> 
        private int RowCounter = 0;

        /// <summary> 
        /// Index panelu 
        /// </summary> 
        private int PanelCounter = 0;

        /// <summary>
        /// Instance
        /// </summary>
        private static Block Instance = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Display(string block)
        {
            Instance = new Block(block);
        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static Display Create(string block)
        {
            return new Display(block);
        }

        /// <summary>
        /// Prepocet dostupnych sloupcu
        /// </summary>
        /// <returns></returns>
        private double CalculateColumns()
        {
            return Math.Floor(Constants.PanelColumns / FontSize);
        }

        /// <summary>
        /// Prepocet dostupnych radku
        /// </summary>
        /// <returns></returns>
        private double CalculateRows()
        {
            return Math.Floor(Constants.PanelRows / FontSize);
        }

        /// <summary>
        /// Sloupce
        /// </summary>
        /// <param name="line"></param>
        /// <param name="width"></param>
        private void Columns(string[] line, double[] width)
        {
            // definice
            string output = string.Empty;
            // sestaveni
            for (var i = 0; i < line.Length; i++)
            {
                var fill = width[i] - line[i].Length;
                if (i != line.Length - 1)
                {
                    output += line[i] + new string(' ', (int)(fill - 2)) + "│ ";
                }
                else
                {
                    output += line[i] + new string(' ', (int)fill);
                }
            }
            // vypis na display
            Line(output);
        }

        /// <summary> 
        /// Mala velikost pisma
        /// </summary> 
        /// <returns>Display</returns> 
        public Display Small()
        {
            FontSize = 0.9f;
            return this;
        }

        /// <summary> 
        /// Stredni velikost pisma
        /// </summary> 
        /// <returns>Display</returns> 
        public Display Medium()
        {
            FontSize = 1;
            return this;
        }

        /// <summary> 
        /// Velka velikost pisma
        /// </summary> 
        /// <returns>Display</returns> 
        public Display Large()
        {
            FontSize = 1.1f;
            return this;
        }

        /// <summary>
        /// Bila
        /// </summary>
        /// <returns>Display</returns> 
        public Display White()
        {
            FontColor = Color.White;
            return this;
        }

        /// <summary>
        /// Cervena
        /// </summary>
        /// <returns>Display</returns> 
        public Display Red()
        {
            FontColor = Color.Red;
            return this;
        }

        /// <summary>
        /// Zelena
        /// </summary>
        /// <returns>Display</returns> 
        public Display Green()
        {
            FontColor = Color.Green;
            return this;
        }

        /// <summary>
        /// Modra
        /// </summary>
        /// <returns>Display</returns> 
        public Display Blue()
        {
            FontColor = Color.Blue;
            return this;
        }

        /// <summary>
        /// Zluta
        /// </summary>
        /// <returns>Display</returns> 
        public Display Yellow()
        {
            FontColor = Color.Yellow;
            return this;
        }

        /// <summary> 
        /// Vypis radky textu 
        /// </summary> 
        /// <param name="text">Text</param> 
        /// <returns>Display</returns>
        public Display Line(string text)
        {
            // definice panelu 
            if (RowCounter == CalculateRows())
            {
                RowCounter = 0;
                PanelCounter++;
            }
            // definice
            var panels = Instance.GetByType<IMyTextPanel>();
            var names = Instance.Keys;
            // zapis do panelu 
            if (PanelCounter < Instance.Count)
            {
                // zkratka
                var panel = panels[names[PanelCounter]];
                // nastaveni a zapis
                panel.SetValue("FontSize", FontSize);
                panel.SetValue("FontColor", FontColor);
                panel.SetValue("Font", Constants.FontFamily);
                panel.SetValue("Content", Constants.PanelContent);
                panel.WriteText(text + "\n", true);
                // multipanel
                RowCounter++;
            }
            return this;
        }

        /// <summary>
        /// Vypis seznamu radku textu
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns></returns>
        public Display Lines(List<string> text)
        {
            foreach (var line in text)
            {
                Line(line);
            }
            return this;
        }

        /// <summary>
        /// Prazdny radek
        /// </summary>
        /// <returns></returns>
        public Display NewLine()
        {
            Line("");
            return this;
        }

        /// <summary> 
        /// Oddelovace 
        /// </summary> 
        public Display Ruler(bool single = false)
        {
            Line(new string(single ? '─' : '═', (int)CalculateColumns()));
            return this;
        }

        /// <summary> 
        /// Vymazani obsahu textoveho ponelu 
        /// </summary> 
        public Display Clear()
        {
            foreach (var block in Instance.GetByType<IMyTextPanel>())
            {
                block.Value.SetValue("Font", Constants.FontFamily);
                block.Value.SetValue("Content", Constants.PanelContent);
                block.Value.WriteText("", false);
            }
            return this;
        }

        /// <summary>
        /// Tabulka
        /// </summary>
        /// <param name="header"></param>
        /// <param name="lines"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public Display Table(TableData table)
        {
            // overeni celkove sirky
            if (table.Width.Sum() != 100)
            {
                throw new Exception("E-FT-01: Sum of all column width must be exactly 100");
            }
            // definice
            var maximum = CalculateColumns();
            var width = table.Width.Select(w => Math.Round((w / 100) * maximum)).ToArray();
            // hlavicka
            Columns(table.Header, width);
            Ruler(true);
            // tabulka
            foreach (var line in table.Rows)
            {
                Columns(line, width);
            }
            // fluent
            return this;
        }
    }
}