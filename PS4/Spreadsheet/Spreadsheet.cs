﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using Spreadsheet;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Maps cell names to their Cell object. Only contains non-empty cells. 
        /// </summary>
        private Dictionary<string, Cell> cells = new Dictionary<string, Cell>();

        /// <summary>
        /// Dependency Graph mapping each cell to its dependents and dependees. 
        /// </summary>
        private DependencyGraph dependencyGraph = new DependencyGraph();

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            if (name == null || ValidVariable(name))
            {
                throw new InvalidNameException();
            }
            if (!cells.TryGetValue(name, out var cell))
            {
                return "";
            }
            return cell.contents;
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
            return SetCell(name, number);
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
            return SetCell(name, text);
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
            return SetCell(name, formula);
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
            return dependencyGraph.GetDependents(name);
        }

        /// <summary>
        /// Helper method for all SetCellContent methods. 
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// Otherwise, the contents of the named cell becomes object parameter which can
        /// be a double, Formula, or string. The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        private ISet<string> SetCell(string name, object contents)
        {
            if (name == null || !ValidVariable(name))
            {
                throw new InvalidNameException();
            }
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
