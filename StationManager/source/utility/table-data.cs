using System.Collections.Generic;

namespace SpaceEngineers
{
    /// <summary>
    /// Tabulka
    /// </summary>
    class TableData
    {
        /// <summary>
        /// Hlavicka
        /// </summary>
        public string[] Header;

        /// <summary>
        /// Radky
        /// </summary>
        public List<string[]> Rows;

        /// <summary>
        /// Proporce sloupcu
        /// </summary>
        public double[] Width;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="header"></param>
        /// <param name="rows"></param>
        /// <param name="width"></param>
        public TableData(string[] header, List<string[]> rows, double[] width)
        {
            Header = header;
            Rows = rows;
            Width = width;
        }

        /// <summary>
        /// Staticky konstruktor
        /// </summary>
        /// <param name="header"></param>
        /// <param name="rows"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static TableData Create(string[] header, List<string[]> rows, double[] width)
        {
            return new TableData(header, rows, width);
        }
    }
}