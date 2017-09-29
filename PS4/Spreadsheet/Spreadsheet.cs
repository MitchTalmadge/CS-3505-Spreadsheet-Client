using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Maps cell names to their Cell object. Only contains non-empty cells. 
        /// </summary>
        private Dictionary<string, Cell> cells;

        /// <summary>
        /// Dependency Graph mapping each cell to its dependents and dependees. 
        /// </summary>
        private DependencyGraph dependencyGraph;

        /// <summary>
        /// Constructor
        /// </summary>
        public Spreadsheet()
        {
            cells =  new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            if (name == null || !ValidVariable(name))
            {
                throw new InvalidNameException();
            }
            if (!cells.TryGetValue(name, out var cell))
            {
                return "";
            }
            return cell.Contents;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return new List<string>(cells.Keys);
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            return SetCellNumOrText(name, number);
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("A cell can't have a null value!);");
            }
            return SetCellNumOrText(name, text);
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException("A cell can't have a null value!);");
            }
            if (name == null || !ValidVariable(name))
            {
                throw new InvalidNameException();
            }

            //saving old dependees and contents in case a circular dependency is found
            List<string> oldDependees = new List<string>(dependencyGraph.GetDependees(name));
            cells.TryGetValue(name, out var oldContents);

            //dependees are replaced with dependees (variables) of new formula
            dependencyGraph.ReplaceDependees(name, formula.GetVariables());
            cells[name] =  new Cell(formula);

            //a circular dependency is checked for, old dependees and content are kept if one is found
            try
            {
                return new HashSet<string>(GetCellsToRecalculate(name));
            }
            catch (CircularException)
            {
                if (oldContents != null)
                {
                    cells[name] = new Cell(oldContents.Contents);
                }
                else //if the cell was empty before setting to this invalid formula, leave it empty
                {
                    cells.Remove(name);
                }
                dependencyGraph.ReplaceDependees(name, oldDependees);
                throw;
            }
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (!ValidVariable(name))
            {
                throw new InvalidNameException();
            }
            //dependency graph's get dependents enumerates all unique dependents 
            //and returns an empty list if the cell does not have dependents
            return dependencyGraph.GetDependents(name);
        }

        /// <summary>
        /// Helper method for SetCellContent methods where content of Cell is double or string. 
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// Otherwise, the contents of the named cell becomes object parameter which can
        /// be a double, Formula, or string. The method returns a set consisting of name 
        /// plus the names of all other cells whose value depends, directly or indirectly, 
        /// on the named cell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        private ISet<string> SetCellNumOrText(string name, object contents)
        {
            if (name == null || !ValidVariable(name))
            {
                throw new InvalidNameException();
            }
            if (cells.TryGetValue(name, out var oldContents))
            {
                //dependencies must be removed if the old contents are a formula with variables
                if (oldContents.Contents is Formula)
                {
                    Formula oldFormula = (Formula)oldContents.Contents;

                    foreach (var oldCell in oldFormula.GetVariables())
                    {
                        dependencyGraph.RemoveDependency(oldCell, name);
                    }
                }
            }
            //don't add an empty cell 
            if (contents is string && (string)contents == "")
            {
                if (oldContents != null)
                {
                    cells.Remove(name);
                }
                return new HashSet<string>(GetCellsToRecalculate(name));
            }
            cells[name] = new Cell(contents);
            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Helper method to determining if the token is a valid variable by the base rules:
        /// 
        /// A string is a valid cell name if and only if:
        ///   (1) its first character is an underscore or a letter.
        ///   (2) its remaining characters (if any) are underscores and/or letters and/or digits.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool ValidVariable(String name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z_](?:[a-zA-Z_]|\d)*$");
        }
    }
}
