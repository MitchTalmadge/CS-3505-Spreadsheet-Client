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
    internal class Cell
    {
        /// <summary>
        /// A cell can either hold a string, double, or Formula as its contents.
        /// </summary>
        internal object Contents { get; }

        /// <summary>
        /// The cell's or variable's name, used to map to it in 
        /// the Spreadsheet class. 
        /// </summary>
        private string Name;

        /// <summary>
        /// A cell's value can either be a string, a double, or a 
        /// SpreadsheetUtilities.FormulaError.
        /// </summary>
        internal object Value { get; private set; }

        /// <summary>
        ///Sets cell's contents property to parameter, which
        ///can be a double, string, or Formula. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <param name="formula"></param>
        public Cell(string name, object contents)
        {
            Contents = contents;
            Name = name;
            
            //setting this Cell's initial value based on contents type
            if (Contents is double)
            {
                Value = Contents;
            }
            if (Contents is string)
            {
                Value = Contents;
            }
            else if (Contents is Formula)
            {
                Formula contentFormula = (Formula)Contents;
                ////////// WHAT DO WE USE FOR LOOKUP???? GETCELLCONTENTS DOES NOT MATCH RETURN TYPE!!!!!!////////
                Value = contentFormula.Evaluate(GetCellContents(Name));
            }
        }
    }
}
