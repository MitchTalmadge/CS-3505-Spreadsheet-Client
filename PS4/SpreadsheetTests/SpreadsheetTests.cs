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
            spreadsheet.SetCellContents("c1", new Formula("1.10 + 8 * a1)"));

            // These cells do not depend on a1
            spreadsheet.SetCellContents("independent1", new Formula("5 + 10"));
            spreadsheet.SetCellContents("dannyboii", new Formula("4.0 * 3500"));

            // Check direct dependents.
            PrivateObject sheetAccessor = new PrivateObject(spreadsheet);
            object directDependents = sheetAccessor.Invoke("GetDirectDependents", new String[1] { "a1" });

            Assert.AreEqual(2, dependents.Length, "There were too many direct dependents.");
            CollectionAssert.Contains(dependents, "b1", "The direct dependents does not include b1.");
            CollectionAssert.Contains(dependents, "d1", "The direct dependents does not include d1.");
            CollectionAssert.DoesNotContain(dependents, "c2", "The direct dependents includes c2.");
            CollectionAssert.DoesNotContain(dependents, "daniel_kopta", "The direct dependents includes daniel_kopta.");
        }

        /// <summary>
        /// When a cell is set to an empty string, it should be removed
        /// from the dictionary keeping track of non-empty cells, and as
        /// well removed from the dependency graph.
        /// </summary>
        [TestMethod]
        public void TestSetCellEmptyContents()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

        }
    }
}
