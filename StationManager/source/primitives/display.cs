using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.GUI.TextPanel;
using VRageMath;

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
        /// Siroky panel
        /// </summary>
        private readonly bool Wide = true;

        /// <summary>
        /// Vypocteny pocet radku
        /// </summary>
        private double CalculatedRows = 0;

        /// <summary>
        /// Dostupne panely
        /// </summary>
        private List<IMyTextSurface> Panels;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        /// <param name="small"></param>
        public Display(string block, bool small)
        {
            Instance = new Block(block);
            Wide = !small;
        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="block"></param>
        /// <param name="small"></param>
        /// <returns></returns>
        public static Display Create(string block, bool small = false)
        {
            return new Display(block, small);
        }

        /// <summary>
        /// Prepocet dostupnych sloupcu
        /// </summary>
        /// <returns></returns>
        private double CalculateColumns()
        {
            if (Wide)
            {
                return Math.Floor(Constants.PanelColumnsWide / FontSize);
            }
            else
            {
                return Math.Floor(Constants.PanelColumnsSmall / FontSize);
            }
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
        private void Columns(string[] lines, double[] width)
        {
            // definice
            string output = string.Empty;
            // sestaveni
            for (int i = 0; i < lines.Length; i++)
            {
                double fill = width[i] - lines[i].Length;
                if (i != lines.Length - 1)
                {
                    int corectFill = (int)fill - 2;
                    if (fill - 2 >= 0)
                    {
                        output += lines[i] + new string(' ', corectFill) + "│ ";
                    }
                    else
                    {
                        output += lines[i].Substring(0, lines[i].Length + corectFill) + "│ ";
                    }
                }
                else
                {
                    output += lines[i] + new string(' ', (int)fill);
                }
            }
            // vypis na display
            Line(output);
        }

        /// <summary>
        /// Textovy panel
        /// </summary>
        /// <returns></returns>
        public Display Text()
        {
            CalculatedRows = CalculateRows();
            Panels = Instance.GetByType<IMyTextPanel>().Values.ToList<IMyTextSurface>();
            return this;
        }

        /// <summary>
        /// Panel kokpitu
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public Display Cocpit(int surface = 0)
        {
            CalculatedRows = CalculateRows();
            Panels = new List<IMyTextSurface>() { Instance.GetByType<IMyCockpit>().Values.First().GetSurface(surface) };
            return this;
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
        /// Zobrazeni upozorneni
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Display Alert(bool enabled)
        {
            if (enabled)
            {
                Panels[PanelCounter].AddImageToSelection("Danger");
                Panels[PanelCounter].PreserveAspectRatio = true;
            }
            else
            {
                Panels[PanelCounter].ClearImagesFromSelection();
            }
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
            if (RowCounter == CalculatedRows)
            {
                RowCounter = 0;
                PanelCounter++;
            }
            // zapis do panelu 
            if (PanelCounter < Panels.Count)
            {
                // zkratka
                IMyTextSurface panel = Panels[PanelCounter];
                // nastaveni a zapis
                panel.FontSize = FontSize;
                panel.FontColor = FontColor;
                panel.Font = "Monospace";
                panel.ContentType = ContentType.TEXT_AND_IMAGE;
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
            foreach (string line in text)
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
            foreach (IMyTextSurface panel in Panels)
            {
                panel.Font = "Monospace";
                panel.ContentType = ContentType.TEXT_AND_IMAGE;
                panel.WriteText("", false);
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
            double maximum = CalculateColumns();
            double[] width = table.Width.Select(w => Math.Round((w / 100) * maximum)).ToArray();
            // hlavicka
            if (table.HasHeader())
            {
                Columns(table.Header, width);
                Ruler(true);
            }
            // tabulka
            foreach (string[] line in table.Rows)
            {
                Columns(line, width);
            }
            // fluent
            return this;
        }
    }
}