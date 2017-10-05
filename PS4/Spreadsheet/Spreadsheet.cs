using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

///
/// Jiahui Chen
/// u0980890
/// CS 3500 PS5
///
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
        /// 0 argument constructor: has no extra validity conditions, 
        /// normalizes every cell name to itself, and has version "default".
        /// </summary>
        public Spreadsheet() :
            base(v => true, s => s, "default")
        {
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// 3 argument constructor: allows the user to provide a validity delegate
        /// (first parameter), a normalization delegate (second parameter),
        /// and a version (third parameter).
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version): 
            base (isValid, normalize, version)
        {
            cells =  new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// 4 argument constructor: allows the user to provide a string representing 
        /// a path to a file (first parameter), a validity delegate (second parameter), 
        /// a normalization delegate (third parameter), and a version (fourth parameter).
        /// Reads a saved spreadsheet from a file and uses it to construct a new spreadsheet. 
        /// New spreadsheet should use the provided validity delegate, normalization delegate, 
        /// and version.
        /// </summary>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) :
            base(isValid, normalize, version)
        {
            // Make sure version of file matches parameter version
            if (GetSavedVersion(filePath) != version)
            {
                throw new SpreadsheetReadWriteException("The provided version does not match the version of the passed in file!");
            }
            //////TODO: READ FROM FILE AND CONSTRUCT NEW SPREADSHEET FROM IT////
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

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

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            Changed = false;
            throw new NotImplementedException();
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
            /////////////TRIM STRING??? OR DOES FIRST CHAR HAVE TO BE = NOT ANY SPACES?????////////
            if (content.Trim().StartsWith("="))
            {
                return SetCellContents(normalizedName, new Formula(content.Trim().Substring(1), Normalize, ValidVariable));
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
                throw new ArgumentNullException("A cell can't have a null value!);");
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
            cells[normalizedName] =  new Cell(normalizedName, formula, LookupCellValue);

            //a circular dependency is checked for, old dependees and content are kept if one is found
            try
            {
                //recalculating necessary cell values
                List<string> recalculatedCells = new List<string>(GetCellsToRecalculate(normalizedName));
                RecalculateCellValues(recalculatedCells);

                //successful return means spreadsheet is changed
                Changed = true;
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
            Changed = true;
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
            if (cells.TryGetValue(name, out var cell))
            {
                if (cell.Value is double num)
                {
                    return num;
                }
            }
            throw new ArgumentException("Lookup did not find double cell value!");
        }
    }
}
