using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Jiahui Chen
/// u0980890
/// CS 3500 PS5
///
namespace SS
{
    /// <summary>
    /// Cell object is what the Spreadsheet object stores.
    /// Each cell contains its contents, value, and the lookup
    /// function used to find values of other cells in the Spreadsheet. 
    /// </summary>
    internal class Cell
    {
        /// <summary>
        /// A cell can either hold a string, double, or Formula as its contents.
        /// </summary>
        internal object Contents { get; }

        /// <summary>
        /// A cell's value can either be a string, a double, or a 
        /// SpreadsheetUtilities.FormulaError.
        /// </summary>
        internal object Value { get; private set; }

        /// <summary>
        /// Finds values of other cells in Spreadsheet, is implemented
        /// in Spreadsheet. 
        /// </summary>
        private Func <string, double> Lookup;

        /// <summary>
        ///Sets cell's contents property to parameter, which
        ///can be a double, string, or Formula. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <param name="formula"></param>
        public Cell(string name, object contents, Func<string, double> lookup)
        {
            Contents = contents;
            Lookup = lookup;
            Recalculate();
        }

        /// <summary>
        /// Recalculates cell's value based on changed contents, (method is 
        /// called when contents of this or a dependee are changed) and sets
        /// Value property to new value. 
        /// </summary>
        internal void Recalculate()
        {
            if (Contents is Formula formula)
            {
                Value = formula.Evaluate(Lookup);
            }
            else 
            {
                Value = Contents;
            }
        }
    }
}
