using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
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
        /// The regex pattern used for validating cell names.
        /// This pattern only allows cells with columns from A to Z, and rows from 1 to 99.
        /// </summary>
        private static readonly Regex CellValidityPattern = new Regex("^[A-Z][1-9][0-9]?$");

        /// <summary>
        /// No arguments in constructor. Cell validity has same rule for all
        /// spreadsheets, and is determined by IsValid method in this class.
        /// Cell name normalization has same rule (all uppercase letters) for all
        /// spreadsheets, and is determined by Normalize method in this class.
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// Helper method that loads the Cell corresponding to the ame parameter.
        /// Tries to add the Cell to the spreadsheet and throws a SpreadsheetReadWrite
        /// Exception reporting the specific error if it fails to do so.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        private void LoadCell(string name, string contents)
        {
            try
            {
                SetContentsOfCell(name, contents);
            }
            catch (InvalidNameException)
            {
                throw new SpreadsheetReadWriteException("An invalid cell name was in the XML file!");
            }
            catch (FormulaFormatException)
            {
                throw new SpreadsheetReadWriteException("An invalid formula was in the XML file!");
            }
            catch (CircularException)
            {
                throw new SpreadsheetReadWriteException("A Circular Dependency was found within a formula in this XML file!");
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null || !ValidVariable(name))
            {
                throw new InvalidNameException();
            }
            if (!cells.TryGetValue(Normalize(name), out var cell))
            {
                return "";
            }

            return cell.Contents;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if (name == null || !ValidVariable(name))
            {
                throw new InvalidNameException();
            }

            string normalizedName = Normalize(name);
            //an empty or umapped cell should have a value of an empty string
            if (!cells.TryGetValue(normalizedName, out var cell))
            {
                return "";
            }

            return cell.Value;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return new List<string>(cells.Keys);
        }

        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        ///
        /// The returned cells that need to be recalculated are passed into the RecalculateCellValues
        /// helper method, where their values are updated, in the corresponding SetCellContents helper
        /// method.
        ///
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || !ValidVariable(name))
            {
                throw new InvalidNameException();
            }
            string normalizedName = Normalize(name);

            if (Double.TryParse(content, out var num) == true)
            {
                return SetCellContents(normalizedName, num);
            }
            if (content.StartsWith("="))
            {
                return SetCellContents(normalizedName, new Formula(content.Substring(1), Normalize, ValidVariable));
            }
            else
            {
                return SetCellContents(normalizedName, content);
            }
        }

        /// <summary>
        /// Recalculates and stores cell values of all Cells corresponding
        /// to names in parameter string Set, in the order they occur in the parameter.
        ///
        /// Will never find empty cell, since the passed in cells were found to
        /// be dependent on a changed cell.
        /// </summary>
        /// <param name="recalculatedCells"></param>
        private void RecalculateCellValues(IEnumerable<string> recalculatedCells)
        {
            foreach (string cellName in recalculatedCells)
            {
                cells.TryGetValue(cellName, out Cell recalcCell);
                recalcCell.Recalculate();
            }
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
        protected override ISet<string> SetCellContents(string name, double number)
        {
            return SetCellNumOrText(Normalize(name), number);
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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("A cell can't have a null value!);");
            }
            return SetCellNumOrText(Normalize(name), text);
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException("A cell can't have a null value!");
            }
            if (name == null || !ValidVariable(name))
            {
                throw new InvalidNameException();
            }
            string normalizedName = Normalize(name);

            //saving old dependees and contents in case a circular dependency is found
            List<string> oldDependees = new List<string>(dependencyGraph.GetDependees(normalizedName));
            cells.TryGetValue(normalizedName, out var oldContents);

            //dependees are replaced with dependees (variables) of new formula
            dependencyGraph.ReplaceDependees(normalizedName, formula.GetVariables());
            cells[normalizedName] = new Cell(normalizedName, formula, LookupCellValue);

            try
            {
                //recalculating necessary cell values
                List<string> recalculatedCells = new List<string>(GetCellsToRecalculate(normalizedName));
                RecalculateCellValues(recalculatedCells);
                //successful return means spreadsheet is changed
                return new HashSet<string>(recalculatedCells);
            }
            catch (CircularException)
            {
                if (oldContents != null)
                {
                    cells[normalizedName] = new Cell(normalizedName, oldContents.Contents, LookupCellValue);
                }
                else //if the cell was empty before setting to this invalid formula, leave it empty
                {
                    cells.Remove(normalizedName);
                }
                dependencyGraph.ReplaceDependees(normalizedName, oldDependees);
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
            return dependencyGraph.GetDependents(Normalize(name));
        }

        /// <summary>
        /// Helper method for SetCellContent methods where content of Cell is double or string.
        ///
        /// If name is null or invalid, throws an InvalidNameException.
        /// Name is already normalized by the Normalize delegate.
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

            cells[name] = new Cell(name, contents, LookupCellValue);

            //recalculating necessary cell values
            List<string> recalculatedCells = new List<string>(GetCellsToRecalculate(name));
            RecalculateCellValues(recalculatedCells);

            //successful return means spreadsheet is changed
            return new HashSet<string>(recalculatedCells);
        }

        /// <summary>
        /// Helper method to determining if the token is a valid variable by the base syntax rule:
        /// The string starts with one or more letters and is followed by one or more numbers.
        ///
        /// Input name is normalized and checked if it follows base syntax rule,
        /// if syntax check passes, name is also checked if the variable name passes the input IsValid function.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool ValidVariable(String name)
        {
            return Regex.IsMatch(Normalize(name), @"^[A-Za-z]+\d+$") && IsValid(Normalize(name));
        }

        /// <summary>
        /// Looks up a Cell's value, is delegate that's passed into the Evaluate method
        /// of a Formula object. Used and passed into the Cell object.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private double LookupCellValue(String name)
        {
            //an empty or umapped cell should have a value of an empty string
            if (cells.TryGetValue(Normalize(name), out var cell))
            {
                if (cell.Value is double num)
                {
                    return num;
                }
            }
            throw new ArgumentException("Lookup did not find double cell value!");
        }

        /// <summary>
        /// Determines if a cell name is valid (exists within the spreadsheet panel).
        /// A valid cell/variable name is a string that consists of one or more letters
        /// followed by one or more digits.
        /// </summary>
        /// <param name="cellName">The name of the cell to validate.</param>
        /// <returns>True if the cell name is valid, false otherwise.</returns>
        private static bool IsValid(string cellName)
        {
            return CellValidityPattern.IsMatch(cellName);
        }

        /// <summary>
        /// Normalizes the given cell name to maintain consistency.
        /// Lowercase cell names are converted to uppercase.
        /// </summary>
        /// <param name="cellName">The name of the cell to normalize.</param>
        /// <returns>The normalized cell name.</returns>
        private static string Normalize(string cellName)
        {
            return cellName.ToUpper();
        }
    }
}