using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    internal class Cell
    {
        /// <summary>
        /// A cell can either hold a string, double, or Formula as its contents.
        /// </summary>
        internal object Contents { get; }

        /// <summary>
        ///Sets cell's contents property to parameter, which
        ///can be a double, string, or Formula. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <param name="formula"></param>
        public Cell(object contents)
        {
            Contents = contents;
        }
    }
}
