using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {        
        /// <summary>
        /// Tests setting and getting doubles cell contents
        /// </summary>
        [TestMethod]
        public void TestSetAndGetCellContentsDouble()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // Set new cell
            spreadsheet.SetCellContents("a1", 18.9);
            Assert.AreEqual(18.9, spreadsheet.GetCellContents("a1"));

            // Replace
            spreadsheet.SetCellContents("a1", 5.8);
            Assert.AreEqual(5.8, spreadsheet.GetCellContents("a1"));

            // Set another new cell
            spreadsheet.SetCellContents("A2", 4.6);
            Assert.AreEqual(4.6, spreadsheet.GetCellContents("A2"));

            // Make sure new cell didn't interfere with old cell
            Assert.AreEqual(5.8, spreadsheet.GetCellContents("a1"));
        }

        /// <summary>
        /// Tests setting and getting string cell contents.
        /// </summary>
        [TestMethod]
        public void TestSetAndGetCellContentsString()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // Set new cell
            spreadsheet.SetCellContents("a1", "moo");
            Assert.AreEqual("moo", spreadsheet.GetCellContents("a1"));

            // Replace
            spreadsheet.SetCellContents("a1", "ayyyelmao");
            Assert.AreEqual("ayyyelmao", spreadsheet.GetCellContents("a1"));

            // Set another new cell
            spreadsheet.SetCellContents("A2", "piggeh");
            Assert.AreEqual("piggeh", spreadsheet.GetCellContents("A2"));

            // Make sure new cell didn't interfere with old cell
            Assert.AreEqual("ayyyelmao", spreadsheet.GetCellContents("a1"));
        }
        
        [TestMethod]
        public void TestSetAndGetCellContentsEmpty()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // Get an "empty" or never made cell
            Assert.AreEqual("", spreadsheet.GetCellContents("nothing00"));
        }

        /// <summary>
        /// Tests replacing formula contents with string.
        /// </summary>
        [TestMethod]
        public void TestReplaceFormulaWithString()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // Set new cell
            spreadsheet.SetCellContents("a1", "moo");
            Assert.AreEqual("moo", spreadsheet.GetCellContents("a1"));

            // Replace 
            spreadsheet.SetCellContents("a1", new Formula("a3 + b4"));
            Assert.AreEqual(new Formula("a3 + b4"), spreadsheet.GetCellContents("a1"));
        }

        /// <summary>
        /// Tests setting and getting Formula cell contents.
        /// </summary>
        [TestMethod]
        public void TestSetAndGetCellContentsFormula()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // Set once
            spreadsheet.SetCellContents("a1", new Formula("1 + 5"));
            Assert.AreEqual(new Formula("1 + 5"), spreadsheet.GetCellContents("a1"));

            // Replace
            spreadsheet.SetCellContents("a1", new Formula(".5 * (4.4 + 5)"));
            Assert.AreEqual(new Formula(".5 * (4.4 + 5)"), spreadsheet.GetCellContents("a1"));

            // Set another new cell
            spreadsheet.SetCellContents("A2", new Formula("18.1 - Bee5 + (0.9 * Cat3)"));
            Assert.AreEqual(new Formula("18.1 - Bee5 + (0.9 * Cat3)"), spreadsheet.GetCellContents("A2"));

            // Make sure new cell didn't interfere with old cell
            Assert.AreEqual(new Formula(".5 * (4.4 + 5)"), spreadsheet.GetCellContents("a1"));
        }
        
        [TestMethod]
        public void TestGetDirectDependents()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.SetCellContents("a1", 10.78);

            // These cells depend on "a1"
            spreadsheet.SetCellContents("b1", new Formula("a1 + independent1"));
            spreadsheet.SetCellContents("c1", new Formula("1.10 + 8 * a1"));

            // These cells do not depend on a1
            spreadsheet.SetCellContents("independent1", new Formula("5 + 10"));
            spreadsheet.SetCellContents("dannyboii", new Formula("4.0 * 3500"));

            // Check direct dependents of a1
            PrivateObject sheetAccessor = new PrivateObject(spreadsheet);
            IEnumerable<string> directDependents = (IEnumerable < string >)sheetAccessor.Invoke("GetDirectDependents", new String[1] {"a1"});

            Assert.AreEqual(2, directDependents.Count());
            Assert.IsTrue(directDependents.Contains("b1"));
            Assert.IsTrue(directDependents.Contains("c1"));
            Assert.IsFalse(directDependents.Contains("independent1"));
            Assert.IsFalse(directDependents.Contains("dannyboii"));
        }

        /// <summary>
        /// When a cell is set to an empty string, it should be removed
        /// from the dictionary keeping track of non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestSetCellEmptyContents()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            //adding a new cell with contents
            spreadsheet.SetCellContents("a1", 10.78);
            Assert.IsTrue(spreadsheet.GetNamesOfAllNonemptyCells().Contains("a1"));

            //setting it to an empty string
            spreadsheet.SetCellContents("a1", "");

            //ensuring cell gets removed from inner dictionary
            Assert.IsFalse(spreadsheet.GetNamesOfAllNonemptyCells().Contains("a1"));
        }

        /// <summary>
        /// When a previously empty cell is set to an empty string, it should 
        /// not be added to the dictionary keeping track of non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestSetNewCellEmptyContents()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            //adding a new cell with contents
            spreadsheet.SetCellContents("a1", "");
            Assert.IsFalse(spreadsheet.GetNamesOfAllNonemptyCells().Contains("a1"));
        }

        /// <summary>
        /// Tests creating a circular dependency.
        /// </summary>
        [TestMethod]
        public void TestCircularDependency()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("a1", new Formula("a3 * d3"));
            spreadsheet.SetCellContents("a3", new Formula("a2 - 5"));
            spreadsheet.SetCellContents("a2", 2.3);
            Assert.AreEqual(2.3, spreadsheet.GetCellContents("a2"));

            // Adding this cell will cause the circular dependency.
            Assert.ThrowsException<CircularException>(() => spreadsheet.SetCellContents("a2", new Formula("a1 + d3")));

            // Make sure nothing was changed since circular dependency was found
            Assert.AreEqual(2.3, spreadsheet.GetCellContents("a2"));
        }

        /// <summary>
        /// Tests creating a circular dependency with a previously
        /// empty cell's new contents.
        /// </summary>
        [TestMethod]
        public void TestCircularDependencyNewCell()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("a1", new Formula("a3 * d3"));
            spreadsheet.SetCellContents("a3", new Formula("a2 - 5"));

            Assert.ThrowsException<CircularException>(() => spreadsheet.SetCellContents("a2", new Formula("a1 + d3")));

            // Make sure the cell wasn't added since a circular dependency was found
            Assert.IsFalse(spreadsheet.GetNamesOfAllNonemptyCells().Contains("a2"));
        }

        [TestMethod]
        public void TestGetNonEmptyCells()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("a1", new Formula("a3 * d3"));
            spreadsheet.SetCellContents("a3", new Formula("a2 - 5"));
            spreadsheet.SetCellContents("hy1", new Formula("3 + 4"));
            spreadsheet.SetCellContents("h78", new Formula("a1 + 7"));
            spreadsheet.SetCellContents("a2", new Formula("t3 + 4.5"));

            Assert.AreEqual(5, spreadsheet.GetNamesOfAllNonemptyCells().Count());

            spreadsheet.SetCellContents("empty1", "");
            Assert.AreEqual(5, spreadsheet.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetNullFormula()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            Formula nullFormula = null;

           spreadsheet.SetCellContents("a1", nullFormula);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetNullString()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            string nullString = null;

            spreadsheet.SetCellContents("a1", nullString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceWithNull()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            string nullString = null;

            spreadsheet.SetCellContents("a1", "yay");
            spreadsheet.SetCellContents("a1", nullString);
        }

        [TestMethod]
        public void TestReplaceFormula()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            PrivateObject accessSheet = new PrivateObject(spreadsheet);
            DependencyGraph deps =  (DependencyGraph) accessSheet.GetField("dependencyGraph");

            //setting cell to formula with dependees
            spreadsheet.SetCellContents("a1", new Formula("9.8 + u1 + dannyboi__9 + 8.7"));
            Assert.IsTrue(deps.HasDependees("a1"));

            //replacing cell with double contents with no dependees
            spreadsheet.SetCellContents("a1", 9.43);
            Assert.IsFalse(deps.HasDependees("a1"));
        }

        /// <summary>
        /// Tests setting cells with null or invalid names.
        /// </summary>
        [TestMethod]
        public void TestSetInvalidNames()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // invalid variable name
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetCellContents("AB!", "yikes"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetCellContents("yeet 89", "test"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetCellContents("  ", "no"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetCellContents("89u", 2.3));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetCellContents("UI.9", 2.56));
            Assert.ThrowsException<InvalidNameException>(
                () => spreadsheet.SetCellContents("d-85", "hi"));

            // null name
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetCellContents(null, "daenyris"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetCellContents(null, 9.8));
            Assert.ThrowsException<InvalidNameException>(() =>
                spreadsheet.SetCellContents(null, new Formula("6.8 * 8 + 2.32")));
        }

    }
}
