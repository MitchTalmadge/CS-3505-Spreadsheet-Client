using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet
{
    class Cell
    {
        /// <summary>
        /// A cell can either hold a string, double, or Formula as its contents.
        /// </summary>
        internal object contents
        {
            get
            {
                return contents;
            }
            private set { }
        }

        /// <summary>
        ///Master constructor which takes in all 3 content types but isn't 
        ///callable outside of this class. Sets cell's contents to whichever value is
        ///non-null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <param name="formula"></param>
        private Cell(string name, string text, Double ? val, Formula formula)
        {
            if (text != null)
            {
                contents = text;
            }
            else if (val != null)
            {
                contents = val;
            }
            else if (formula != null)
            {
                contents = formula;
            }
            else
            {
                throw new ArgumentException("Cell's contents may not be null!");
            }
        }

        /// <summary>
        /// Creates a cell that holds string contents. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public Cell(string name, string text) : this(name, text, null, null) { }

        /// <summary>
        /// Creates a cell that holds double contents. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public Cell(string name, double val) : this(name, null, val, null){ }

        /// <summary>
        /// Creates a cell that holds Formula contents. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        public Cell(string name, Formula formula) : this(name, null, null, formula){ }
    }
}
