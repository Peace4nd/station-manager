using System;

namespace SpaceEngineers
{
    /// <summary>
    /// Formatovaci trida pro zobrazovani informaci na LCD panelech
    /// </summary>
    class Formater
    {
        /// <summary>
        /// Graficke zobrazeni procentniho vyjadreni hodnoty
        /// </summary>
        /// <param name="amount">Mnozstvi</param>
        /// <param name="reference">Reference</param>
        /// <returns></returns>
        private static string CreatePercentageBars(double amount, double reference)
        {
            // definice
            string item = "";
            // vypocet poctu carek
            int bars = Math.Min(Constants.ColumnBars, (int)Math.Round((amount / reference) * Constants.ColumnBars, 0));
            // sestaveni
            item += "│";
            item += new String('█', bars);
            item += new String(' ', Constants.ColumnBars - bars);
            item += "│";
            // vraceni
            return item;
        }

        /// <summary>
        /// Naformatovani hodnoty
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        private static string FormatAmount(double amount, bool percent = false)
        {
            // definice
            string formatted;
            // naformatovani
            if (percent)
            {
                formatted = Math.Round(amount, 0).ToString() + "%";
            }
            else
            {
                formatted = Amount(amount);
            }
            // sestaveni
            return " [" + new string(' ', Constants.ColumnAmount - formatted.Length) + formatted + "]  ";
        }

        /// <summary> 
        /// Zaokrouhleni cisla 
        /// </summary> 
        /// <param name="value"></param> 
        /// <returns></returns> 
        public static string Amount(double value)
        {
            if (value >= 1000000)
            {
                return Math.Round(value / 1000000, 2).ToString() + "M";
            }
            else if (value >= 10000)
            {
                return Math.Round(value / 1000, 0).ToString() + "k";
            }
            else if (value >= 1000)
            {
                return Math.Round(value / 1000, 1).ToString() + "k";
            }
            else
            {
                return Math.Round(value, 0).ToString();
            }
        }

        /// <summary> 
        /// Vykresleni procetniho zobrazeni 
        /// </summary> 
        /// <param name="name">Nazev</param> 
        /// <param name="amount">Aktualni hodnota</param> 
        /// <param name="reference">Referencni hodnota</param> 
        public static string Bars(string name, double amount, double reference)
        {
            // kontrola referencni hodnoty 
            if (reference <= 0)
            {
                reference = 1;
            }
            // definice
            string item = "";
            // sestaveni
            item += CreatePercentageBars(amount, reference);
            item += " ";
            item += name;
            // vraceni 
            return item;
        }

        /// <summary> 
        /// Vykresleni procetniho zobrazeni se zobrazenim mnozstvi 
        /// </summary> 
        /// <param name="name">Nazev</param> 
        /// <param name="amount">Aktualni hodnota</param> 
        /// <param name="reference">Referencni hodnota</param> 
        public static string BarsWithAmount(string name, double amount, double reference)
        {
            // kontrola referencni hodnoty 
            if (reference <= 0)
            {
                reference = 1;
            }
            //vraceni 
            return CreatePercentageBars(amount, reference) + FormatAmount(amount) + name;
        }

        /// <summary> 
        /// Vykresleni procetniho zobrazeni se zobrazenim procent 
        /// </summary> 
        /// <param name="name">Nazev</param> 
        /// <param name="amount">Aktualni hodnota</param> 
        /// <param name="reference">Referencni hodnota</param> 
        public static string BarsWithPercent(string name, double amount, double reference)
        {
            // kontrola referencni hodnoty 
            if (reference <= 0)
            {
                reference = 1;
            }
            //vraceni 
            return CreatePercentageBars(amount, reference) + FormatAmount(Math.Min(100, (amount / reference) * 100), true) + name;
        }
    }
}