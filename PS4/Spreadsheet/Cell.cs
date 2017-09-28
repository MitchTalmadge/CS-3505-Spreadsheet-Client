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
        ///Sets cell's contents property to whichever parameter. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <param name="formula"></param>
        public Cell(object contents)
        {
            this.contents = contents;
        }
    }
}
