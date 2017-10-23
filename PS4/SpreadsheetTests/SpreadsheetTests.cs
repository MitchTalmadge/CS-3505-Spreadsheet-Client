using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System.Threading;
using System.Xml;
using System.IO;

///
/// Jiahui Chen
/// u0980890
/// CS 3500 PS5
///
namespace SpreadsheetTests
{
    /// <summary>
    /// Tests for the Spreadsheet class.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        [TestMethod]
        public void TestChanged()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            Assert.IsFalse(spreadsheet.Changed);
            spreadsheet.SetContentsOfCell("a1", "change it!");
            Assert.IsTrue(spreadsheet.Changed);
            spreadsheet.Save("TestSpreadsheets/ChangedTest.xml");
            Assert.IsFalse(spreadsheet.Changed);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadCircularDependency()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet("TestSpreadsheets/Circular.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadInvalidCellName()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet("TestSpreadsheets/InvalidCellName.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadInvalidFormula()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet("TestSpreadsheets/BadFormula.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadUnclosedCell()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet("TestSpreadsheets/UnclosedCell.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadNoCellName()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet("TestSpreadsheets/NoCellName.xml", s => true, s => s, "2.008");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadNoCell()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet("TestSpreadsheets/NoCell.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadNoCellContents()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet("TestSpreadsheets/NoCellContents.xml", s => true, s => s, "2.008");
        }

        [TestMethod]
        public void TestGetCellValueInvalidName()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            //null name
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellValue((string)null));

            //syntactically invalid name
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellValue("Ui8.7"));
        }

        [TestMethod]
        public void Test3ParamConstructor()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet(s => true, s => s.ToLower(), "trial");
            Assert.AreEqual("", spreadsheet.GetCellContents("nonexistent4"));

            //ensure the passed in normalizer is being used
            spreadsheet.SetContentsOfCell("HEY01", "food is good");
            Assert.AreEqual("food is good", spreadsheet.GetCellContents("hey01"));
        }

        [TestMethod]
        public void TestValidSave()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.SetContentsOfCell("a1", " 10.01 ");
            spreadsheet.SetContentsOfCell("a2", "yoooooooooooo");
            spreadsheet.SetContentsOfCell("a3", "= 8 + 9 ");
            Assert.AreEqual(10.01, spreadsheet.GetCellValue("a1"));
            Assert.AreEqual("yoooooooooooo", spreadsheet.GetCellValue("a2"));
            Assert.AreEqual((double)17, spreadsheet.GetCellValue("a3"));

            //saving this spreadsheet to a file, then building a spreadsheet out of that file to check for same
            //contents as spreadhseet
            spreadsheet.Save("TestSpreadsheets/SameAsValidThreeTypes.xml");

            AbstractSpreadsheet spreadsheetCopy = new Spreadsheet("TestSpreadsheets/SameAsValidThreeTypes.xml", s => true, s => s, "default");
            Assert.AreEqual(10.01, spreadsheetCopy.GetCellContents("a1"));
            Assert.AreEqual(10.01, spreadsheetCopy.GetCellValue("a1"));

            Assert.AreEqual("yoooooooooooo", spreadsheetCopy.GetCellContents("a2"));
            Assert.AreEqual("yoooooooooooo", spreadsheetCopy.GetCellValue("a2"));

            Assert.AreEqual(new Formula("8 + 9"), spreadsheetCopy.GetCellContents("a3"));
            Assert.AreEqual((double)17, spreadsheetCopy.GetCellValue("a3"));
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadWrongVersion()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet("TestSpreadsheets/ValidThreeTypes.xml", s => true, s => s, "2.008");
        }

        [TestMethod]
        public void TestValidLoad()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet("TestSpreadsheets/ValidThreeTypes.xml", s => true, s => s, "default");

            Assert.AreEqual(10.01, spreadsheet.GetCellContents("a1"));
            Assert.AreEqual(10.01, spreadsheet.GetCellValue("a1"));

            Assert.AreEqual("yoooooooooooo", spreadsheet.GetCellContents("a2"));
            Assert.AreEqual("yoooooooooooo", spreadsheet.GetCellValue("a2"));

            Assert.AreEqual(new Formula("8 + 9"), spreadsheet.GetCellContents("a3"));
            Assert.AreEqual((double)17, spreadsheet.GetCellValue("a3"));
        }

        [TestMethod]
        public void TestGetSaveVersion()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            Assert.ThrowsException<SpreadsheetReadWriteException>(() => spreadsheet.GetSavedVersion("TestSpreadsheets/NotSpreadsheet.XML"));

            Assert.ThrowsException<SpreadsheetReadWriteException>(() => spreadsheet.GetSavedVersion("TestSpreadsheets/NoVersion.xml"));

            Assert.AreEqual("default", spreadsheet.GetSavedVersion("TestSpreadsheets/ValidThreeTypes.xml"));
        }

        [TestMethod]
        public void TestFormulaErrorValue()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            //a1 depends on no0 but no0 hasn't been set yet, so value of a1 should be FormulaError
            spreadsheet.SetContentsOfCell("a1", "= no0 + 1.0");
            Assert.IsTrue(spreadsheet.GetCellValue("a1") is FormulaError);

            //cell contains formula referencing string value cell
            spreadsheet.SetContentsOfCell("string69", "ayyyeelmao");
            Assert.AreEqual("ayyyeelmao", spreadsheet.GetCellValue("string69"));
            spreadsheet.SetContentsOfCell("badForm01", "= string69 * 7.2");
            Assert.IsTrue(spreadsheet.GetCellValue("badForm01") is FormulaError);

            spreadsheet.SetContentsOfCell("string69", " = 8 + 9");
            Assert.IsTrue(spreadsheet.GetCellValue("badForm01") is FormulaError);
        }

        [TestMethod]
        public void TestRecalculateValueInvalidFormula()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            //a1 depends on no0 but no0 hasn't been set yet, so value of a1 should be FormulaError
            spreadsheet.SetContentsOfCell("a1", "= no0 + 1.0");
            Assert.IsTrue(spreadsheet.GetCellValue("a1") is FormulaError);

            //no0 gets set, so a1 should be a value now
            spreadsheet.SetContentsOfCell("no0", "3.5");
            Assert.AreEqual(3.5, spreadsheet.GetCellValue("no0"));
            Assert.AreEqual(4.5, spreadsheet.GetCellValue("a1"));
        }

        [TestMethod]
        public void TestSetAndGetCellValueEmpty()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            //empty/never set cell
            Assert.AreEqual("", spreadsheet.GetCellValue("a1"));

            //cell is set than emptied
            spreadsheet.SetContentsOfCell("A2", " plz8");
            Assert.AreEqual(" plz8", spreadsheet.GetCellValue("A2"));
            spreadsheet.SetContentsOfCell("A2", "");
            Assert.AreEqual("", spreadsheet.GetCellValue("A2"));
        }

        [TestMethod]
        public void TestSetAndGetCellValueFormula()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.SetContentsOfCell("a1", "= 90 + 1");
            Assert.AreEqual(new Formula("90 + 1"), spreadsheet.GetCellContents("a1"));
            Assert.AreEqual((double)91, spreadsheet.GetCellValue("a1"));

            //due to the space before the '=' this should not be a formula (but a string) 
            spreadsheet.SetContentsOfCell("A2", " = 9 * 8");
            Assert.AreEqual(" = 9 * 8", spreadsheet.GetCellValue("A2"));
        }

        [TestMethod]
        public void TestRecalculateValueFormula()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.SetContentsOfCell("a1", "= 1 * 0.5");
            Assert.AreEqual(0.5, spreadsheet.GetCellValue("a1"));

            //a2 is dependent on a1
            spreadsheet.SetContentsOfCell("a2", "= a1 * 2");
            Assert.AreEqual((double)1, spreadsheet.GetCellValue("a2"));

            //a3 depends on a1
            spreadsheet.SetContentsOfCell("a3", "= 2 + a1");
            Assert.AreEqual(2.5, spreadsheet.GetCellValue("a3"));

            //a4 depends on a3 and a2
            spreadsheet.SetContentsOfCell("a4", "=  a3 + a2");
            Assert.AreEqual(3.5, spreadsheet.GetCellValue("a4"));

            //setting a1 to something new, and checking if a2 - a4 update accordingly
            spreadsheet.SetContentsOfCell("a1", "1.0  ");
            Assert.AreEqual(1.0, spreadsheet.GetCellValue("a1"));
            Assert.AreEqual(2.0, spreadsheet.GetCellValue("a2"));
            Assert.AreEqual(3.0, spreadsheet.GetCellValue("a3"));
            Assert.AreEqual(5.0, spreadsheet.GetCellValue("a4"));
        }

        [TestMethod]
        public void TestSetAndGetCellValueString()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.SetContentsOfCell("a1", "yo !");
            Assert.AreEqual("yo !", spreadsheet.GetCellContents("a1"));
            Assert.AreEqual("yo !", spreadsheet.GetCellValue("a1"));

            spreadsheet.SetContentsOfCell("a2", "  uioi   ");
            Assert.AreEqual("  uioi   ", spreadsheet.GetCellContents("a2"));

            spreadsheet.SetContentsOfCell("A2", "n90");
            Assert.AreEqual("n90", spreadsheet.GetCellValue("A2"));

            spreadsheet.SetContentsOfCell("A32", "i = 908 + 89");
            Assert.AreEqual("i = 908 + 89", spreadsheet.GetCellValue("A32"));
        }

        [TestMethod]
        public void TestRecalculateValueString()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.SetContentsOfCell("a1", "yo !");
            Assert.AreEqual("yo !", spreadsheet.GetCellContents("a1"));
            Assert.AreEqual("yo !", spreadsheet.GetCellValue("a1"));

            //replace and recalculate cell's value
            spreadsheet.SetContentsOfCell("a1", "  uioi   ");
            Assert.AreEqual("  uioi   ", spreadsheet.GetCellContents("a1"));

            spreadsheet.SetContentsOfCell("A2", "? = 90");
            Assert.AreEqual("? = 90", spreadsheet.GetCellValue("A2"));

            spreadsheet.SetContentsOfCell("A2", "  + -9.8 ");
            Assert.AreEqual("  + -9.8 ", spreadsheet.GetCellValue("A2"));
        }

        [TestMethod]
        public void TestSetAndGetCellValueDouble()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.SetContentsOfCell("a1", "18.9");
            Assert.AreEqual(18.9, spreadsheet.GetCellContents("a1"));
            Assert.AreEqual(18.9, spreadsheet.GetCellValue("a1"));

            spreadsheet.SetContentsOfCell("a2", "  5.8   ");
            Assert.AreEqual(5.8, spreadsheet.GetCellContents("a2"));

            spreadsheet.SetContentsOfCell("A2", "4.6");
            Assert.AreEqual(4.6, spreadsheet.GetCellValue("A2"));

            spreadsheet.SetContentsOfCell("A32", "-4.6");
            Assert.AreEqual(-4.6, spreadsheet.GetCellValue("A32"));
        }

        [TestMethod]
        public void TestRecalculateValueDouble()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.SetContentsOfCell("a1", "18.9");
            Assert.AreEqual(18.9, spreadsheet.GetCellValue("a1"));

            //setting new value
            spreadsheet.SetContentsOfCell("a1", "5.8");
            Assert.AreEqual(5.8, spreadsheet.GetCellValue("a1"));

            spreadsheet.SetContentsOfCell("A2", "4.6");
            Assert.AreEqual(4.6, spreadsheet.GetCellValue("A2"));

            spreadsheet.SetContentsOfCell("A2", "  -9.8 ");
            Assert.AreEqual(-9.8, spreadsheet.GetCellValue("A2"));
        }

        /// <summary>
        /// Tests setting and getting doubles cell contents
        /// </summary>
        [TestMethod]
        public void TestSetAndGetCellContentsDouble()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // Set new cell
            spreadsheet.SetContentsOfCell("a1", "18.9");
            Assert.AreEqual(18.9, spreadsheet.GetCellContents("a1"));

            // Replace
            spreadsheet.SetContentsOfCell("a1", "5.8");
            Assert.AreEqual(5.8, spreadsheet.GetCellContents("a1"));

            // Set another new cell
            spreadsheet.SetContentsOfCell("A2", "4.6");
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
            spreadsheet.SetContentsOfCell("a1", "moo");
            Assert.AreEqual("moo", spreadsheet.GetCellContents("a1"));

            // Replace
            spreadsheet.SetContentsOfCell("a1", "ayyyelmao");
            Assert.AreEqual("ayyyelmao", spreadsheet.GetCellContents("a1"));

            // Set another new cell
            spreadsheet.SetContentsOfCell("A2", "piggeh");
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
            spreadsheet.SetContentsOfCell("a1", "moo");
            Assert.AreEqual("moo", spreadsheet.GetCellContents("a1"));

            // Replace 
            spreadsheet.SetContentsOfCell("a1", "=a3 + b4");
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
            spreadsheet.SetContentsOfCell("a1", "= 1 + 5");
            Assert.AreEqual(new Formula("1 + 5"), spreadsheet.GetCellContents("a1"));

            // Replace
            spreadsheet.SetContentsOfCell("a1", "= .5 * (4.4 + 5)");
            Assert.AreEqual(new Formula(".5 * (4.4 + 5)"), spreadsheet.GetCellContents("a1"));

            // Set another new cell
            spreadsheet.SetContentsOfCell("A2", "= 18.1 - Bee5 + (0.9 * Cat3)");
            Assert.AreEqual(new Formula("18.1 - Bee5 + (0.9 * Cat3)"), spreadsheet.GetCellContents("A2"));

            // Make sure new cell didn't interfere with old cell
            Assert.AreEqual(new Formula(".5 * (4.4 + 5)"), spreadsheet.GetCellContents("a1"));
        }

        [TestMethod]
        public void TestGetDirectDependents()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.SetContentsOfCell("a1", "10.78");

            // These cells depend on "a1"
            spreadsheet.SetContentsOfCell("b1", "= a1 + independent1");
            spreadsheet.SetContentsOfCell("c1", "=1.10 + 8 * a1");

            // These cells do not depend on a1
            spreadsheet.SetContentsOfCell("independent1", "= 5 + 10");
            spreadsheet.SetContentsOfCell("dannyboii2", "= 4.0 * 3500");

            // Check direct dependents of a1
            PrivateObject sheetAccessor = new PrivateObject(spreadsheet);
            IEnumerable<string> directDependents = (IEnumerable<string>)sheetAccessor.Invoke("GetDirectDependents", new String[1] { "a1" });

            Assert.AreEqual(2, directDependents.Count());
            Assert.IsTrue(directDependents.Contains("b1"));
            Assert.IsTrue(directDependents.Contains("c1"));
            Assert.IsFalse(directDependents.Contains("independent1"));
            Assert.IsFalse(directDependents.Contains("dannyboii2"));
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
            spreadsheet.SetContentsOfCell("a1", "10.78");
            Assert.IsTrue(spreadsheet.GetNamesOfAllNonemptyCells().Contains("a1"));

            //setting it to an empty string
            spreadsheet.SetContentsOfCell("a1", "");

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
            spreadsheet.SetContentsOfCell("a1", "");
            Assert.IsFalse(spreadsheet.GetNamesOfAllNonemptyCells().Contains("a1"));
        }

        /// <summary>
        /// Tests creating a circular dependency.
        /// </summary>
        [TestMethod]
        public void TestCircularDependency()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("a1", "= a3 * d3");
            spreadsheet.SetContentsOfCell("a3", "= a2 - 5");
            spreadsheet.SetContentsOfCell("a2", "2.3");
            Assert.AreEqual(2.3, spreadsheet.GetCellContents("a2"));

            // Adding this cell will cause the circular dependency.
            Assert.ThrowsException<CircularException>(() => spreadsheet.SetContentsOfCell("a2", "= a1 + d3"));

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
            spreadsheet.SetContentsOfCell("a1", "=  a3 * d3");
            spreadsheet.SetContentsOfCell("a3", "= a2 - 5");

            Assert.ThrowsException<CircularException>(() => spreadsheet.SetContentsOfCell("a2", "= a1 + d3"));

            // Make sure the cell wasn't added since a circular dependency was found
            Assert.IsFalse(spreadsheet.GetNamesOfAllNonemptyCells().Contains("a2"));
        }

        [TestMethod]
        public void TestGetNonEmptyCells()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("a1", "=a3 * d3");
            spreadsheet.SetContentsOfCell("a3", "=a2 - 5");
            spreadsheet.SetContentsOfCell("hy1", "= 3 + 4");
            spreadsheet.SetContentsOfCell("h78", "= a1 + 7");
            spreadsheet.SetContentsOfCell("a2", "= t3 + 4.5");

            Assert.AreEqual(5, spreadsheet.GetNamesOfAllNonemptyCells().Count());

            spreadsheet.SetContentsOfCell("empty1", "");
            Assert.AreEqual(5, spreadsheet.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetNullString()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            string nullString = null;

            spreadsheet.SetContentsOfCell("a1", nullString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceWithNull()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            string nullString = null;

            spreadsheet.SetContentsOfCell("a1", "yay");
            spreadsheet.SetContentsOfCell("a1", nullString);
        }

        [TestMethod]
        public void TestReplaceFormula()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            PrivateObject accessSheet = new PrivateObject(spreadsheet);
            DependencyGraph deps = (DependencyGraph)accessSheet.GetField("dependencyGraph");

            //setting cell to formula with dependees
            spreadsheet.SetContentsOfCell("a1", "=9.8 + u1 + dannyboi9 + 8.7");
            Assert.IsTrue(deps.HasDependees("a1"));

            //replacing cell with double contents with no dependees
            spreadsheet.SetContentsOfCell("a1", "9.43");
            Assert.IsFalse(deps.HasDependees("a1"));
        }

        [TestMethod]
        public void TestSetInvalidNames()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // invalid variable name
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetContentsOfCell("AB!", "yikes"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetContentsOfCell("yeet 89", "test"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetContentsOfCell("  ", "no"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetContentsOfCell("89u", "2.3"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetContentsOfCell("UI.9", "2.56"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetContentsOfCell("d-85", "hi"));

            // null name
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetContentsOfCell(null, "daenyris"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetContentsOfCell(null, "9.8"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.SetContentsOfCell(null, " = 6.8 * 8 + 2.32"));
        }

        /// <summary>
        /// Stress test to ensure replacement/get value is efficient. 
        /// </summary>
        [TestMethod]
        public void StressTestDouble()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            for (int i = 0; i < 10000; i++)
            {
                spreadsheet.SetContentsOfCell("a1", i.ToString());
                spreadsheet.GetCellValue("a1");
            }
        }

        /// <summary>
        /// Stress test to ensure replacement/get value is efficient. 
        /// </summary>
        [TestMethod]
        public void StressTestFormula()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            for (int i = 0; i < 10000; i++)
            {
                spreadsheet.SetContentsOfCell("a1", "= 8 + 4 -" + i.ToString());
                spreadsheet.GetCellValue("a1");
            }
        }

        /// <summary>
        /// Tests the set returned when cell is set is correct. 
        /// </summary>
        [TestMethod]
        public void TestSetReturnsDependencies()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            //no dependencies expected
            CollectionAssert.AreEqual(new[] { "a1" }, spreadsheet.SetContentsOfCell("a1", "3.48").ToArray());

            //direct dependency
            spreadsheet.SetContentsOfCell("a2", "= a1 + 7.8");
            CollectionAssert.AreEqual(new[] { "a1", "a2" }, spreadsheet.SetContentsOfCell("a1", "yo").ToArray());

            //indirect dependency
            spreadsheet.SetContentsOfCell("a3", "= a2 + 7.8");
            spreadsheet.SetContentsOfCell("a4", "= a3 + 7.8");
            CollectionAssert.AreEquivalent(new[] { "a1", "a2", "a3", "a4" }, spreadsheet.SetContentsOfCell("a1", "78").ToArray());
        }

        [TestMethod]
        public void TestGetInvalidNames()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            //trying to get a null name cell
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellContents((string)null));

            //trying to get an invalid named cell
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellContents("8.9i"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellContents("AB!"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellContents("yeet 89"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellContents("  "));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellContents("89u"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellContents("UI.9"));
            Assert.ThrowsException<InvalidNameException>(() => spreadsheet.GetCellContents("d-85"));
        }

        [TestMethod]
        public void TestGetDependentsNullName()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            PrivateObject sheetAccessor = new PrivateObject(spreadsheet);

            try
            {
                sheetAccessor.Invoke("GetDirectDependents", new String[1] { (string)null });
            }
            catch (TargetInvocationException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void TestGetDependentsInalidName()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            PrivateObject sheetAccessor = new PrivateObject(spreadsheet);

            try
            {
                sheetAccessor.Invoke("GetDirectDependents", new String[1] { "87ei" });
            }
            catch (TargetInvocationException e)
            {
                Assert.IsInstanceOfType(e.InnerException, typeof(InvalidNameException));
            }
        }


        //////////////GRADING TESTS////////////








        /// <summary>
        ///This is a test class for SpreadsheetTest and is intended
        ///to contain all SpreadsheetTest Unit Tests
        ///</summary>
        [TestClass()]
        public class GradingTests
        {


            private TestContext testContextInstance;

            /// <summary>
            ///Gets or sets the test context which provides
            ///information about and functionality for the current test run.
            ///</summary>
            public TestContext TestContext
            {
                get
                {
                    return testContextInstance;
                }
                set
                {
                    testContextInstance = value;
                }
            }

            #region Additional test attributes
            // 
            //You can use the following additional attributes as you write your tests:
            //
            //Use ClassInitialize to run code before running the first test in the class
            //[ClassInitialize()]
            //public static void MyClassInitialize(TestContext testContext)
            //{
            //}
            //
            //Use ClassCleanup to run code after all tests in a class have run
            //[ClassCleanup()]
            //public static void MyClassCleanup()
            //{
            //}
            //
            //Use TestInitialize to run code before running each test
            //[TestInitialize()]
            //public void MyTestInitialize()
            //{
            //}
            //
            //Use TestCleanup to run code after each test has run
            //[TestCleanup()]
            //public void MyTestCleanup()
            //{
            //}
            //
            #endregion

            // Verifies cells and their values, which must alternate.
            public void VV(AbstractSpreadsheet sheet, params object[] constraints)
            {
                for (int i = 0; i < constraints.Length; i += 2)
                {
                    if (constraints[i + 1] is double)
                    {
                        Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
                    }
                    else
                    {
                        Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
                    }
                }
            }


            // For setting a spreadsheet cell.
            public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
            {
                List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
                return result;
            }

            // Tests IsValid
            [TestMethod()]
            public void IsValidTest1()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "x");
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void IsValidTest2()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
                ss.SetContentsOfCell("A1", "x");
            }

            [TestMethod()]
            public void IsValidTest3()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "= A1 + C1");
            }

            [TestMethod()]
            [ExpectedException(typeof(FormulaFormatException))]
            public void IsValidTest4()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
                ss.SetContentsOfCell("B1", "= A1 + C1");
            }

            // Tests Normalize
            [TestMethod()]
            public void NormalizeTest1()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "hello");
                Assert.AreEqual("", s.GetCellContents("b1"));
            }

            [TestMethod()]
            public void NormalizeTest2()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
                ss.SetContentsOfCell("B1", "hello");
                Assert.AreEqual("hello", ss.GetCellContents("b1"));
            }

            [TestMethod()]
            public void NormalizeTest3()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("a1", "5");
                s.SetContentsOfCell("A1", "6");
                s.SetContentsOfCell("B1", "= a1");
                Assert.AreEqual(5.0, (double)s.GetCellValue("B1"), 1e-9);
            }

            [TestMethod()]
            public void NormalizeTest4()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
                ss.SetContentsOfCell("a1", "5");
                ss.SetContentsOfCell("A1", "6");
                ss.SetContentsOfCell("B1", "= a1");
                Assert.AreEqual(6.0, (double)ss.GetCellValue("B1"), 1e-9);
            }

            // Simple tests
            [TestMethod()]
            public void EmptySheet()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                VV(ss, "A1", "");
            }


            [TestMethod()]
            public void OneString()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                OneString(ss);
            }

            public void OneString(AbstractSpreadsheet ss)
            {
                Set(ss, "B1", "hello");
                VV(ss, "B1", "hello");
            }


            [TestMethod()]
            public void OneNumber()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                OneNumber(ss);
            }

            public void OneNumber(AbstractSpreadsheet ss)
            {
                Set(ss, "C1", "17.5");
                VV(ss, "C1", 17.5);
            }


            [TestMethod()]
            public void OneFormula()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                OneFormula(ss);
            }

            public void OneFormula(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "5.2");
                Set(ss, "C1", "= A1+B1");
                VV(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
            }


            [TestMethod()]
            public void Changed()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Assert.IsFalse(ss.Changed);
                Set(ss, "C1", "17.5");
                Assert.IsTrue(ss.Changed);
            }


            [TestMethod()]
            public void DivisionByZero1()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                DivisionByZero1(ss);
            }

            public void DivisionByZero1(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "0.0");
                Set(ss, "C1", "= A1 / B1");
                Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
            }

            [TestMethod()]
            public void DivisionByZero2()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                DivisionByZero2(ss);
            }

            public void DivisionByZero2(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "5.0");
                Set(ss, "A3", "= A1 / 0.0");
                Assert.IsInstanceOfType(ss.GetCellValue("A3"), typeof(FormulaError));
            }



            [TestMethod()]
            public void EmptyArgument()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                EmptyArgument(ss);
            }

            public void EmptyArgument(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "C1", "= A1 + B1");
                Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
            }


            [TestMethod()]
            public void StringArgument()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                StringArgument(ss);
            }

            public void StringArgument(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "hello");
                Set(ss, "C1", "= A1 + B1");
                Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
            }


            [TestMethod()]
            public void ErrorArgument()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                ErrorArgument(ss);
            }

            public void ErrorArgument(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "B1", "");
                Set(ss, "C1", "= A1 + B1");
                Set(ss, "D1", "= C1");
                Assert.IsInstanceOfType(ss.GetCellValue("D1"), typeof(FormulaError));
            }


            [TestMethod()]
            public void NumberFormula1()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                NumberFormula1(ss);
            }

            public void NumberFormula1(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.1");
                Set(ss, "C1", "= A1 + 4.2");
                VV(ss, "C1", 8.3);
            }


            [TestMethod()]
            public void NumberFormula2()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                NumberFormula2(ss);
            }

            public void NumberFormula2(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "= 4.6");
                VV(ss, "A1", 4.6);
            }


            // Repeats the simple tests all together
            [TestMethod()]
            public void RepeatSimpleTests()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Set(ss, "A1", "17.32");
                Set(ss, "B1", "This is a test");
                Set(ss, "C1", "= A1+B1");
                OneString(ss);
                OneNumber(ss);
                OneFormula(ss);
                DivisionByZero1(ss);
                DivisionByZero2(ss);
                StringArgument(ss);
                ErrorArgument(ss);
                NumberFormula1(ss);
                NumberFormula2(ss);
            }

            // Four kinds of formulas
            [TestMethod()]
            public void Formulas()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Formulas(ss);
            }

            public void Formulas(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "4.4");
                Set(ss, "B1", "2.2");
                Set(ss, "C1", "= A1 + B1");
                Set(ss, "D1", "= A1 - B1");
                Set(ss, "E1", "= A1 * B1");
                Set(ss, "F1", "= A1 / B1");
                VV(ss, "C1", 6.6, "D1", 2.2, "E1", 4.4 * 2.2, "F1", 2.0);
            }

            [TestMethod()]
            public void Formulasa()
            {
                Formulas();
            }

            [TestMethod()]
            public void Formulasb()
            {
                Formulas();
            }


            // Are multiple spreadsheets supported?
            [TestMethod()]
            public void Multiple()
            {
                AbstractSpreadsheet s1 = new Spreadsheet();
                AbstractSpreadsheet s2 = new Spreadsheet();
                Set(s1, "X1", "hello");
                Set(s2, "X1", "goodbye");
                VV(s1, "X1", "hello");
                VV(s2, "X1", "goodbye");
            }

            [TestMethod()]
            public void Multiplea()
            {
                Multiple();
            }

            [TestMethod()]
            public void Multipleb()
            {
                Multiple();
            }

            [TestMethod()]
            public void Multiplec()
            {
                Multiple();
            }

            // Reading/writing spreadsheets
            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest1()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                ss.Save("q:\\missing\\save.txt");
            }

            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest2()
            {
                AbstractSpreadsheet ss = new Spreadsheet("q:\\missing\\save.txt", s => true, s => s, "");
            }

            [TestMethod()]
            public void SaveTest3()
            {
                AbstractSpreadsheet s1 = new Spreadsheet();
                Set(s1, "A1", "hello");
                s1.Save("save1.txt");
                s1 = new Spreadsheet("save1.txt", s => true, s => s, "default");
                Assert.AreEqual("hello", s1.GetCellContents("A1"));
            }

            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest4()
            {
                using (StreamWriter writer = new StreamWriter("save2.txt"))
                {
                    writer.WriteLine("This");
                    writer.WriteLine("is");
                    writer.WriteLine("a");
                    writer.WriteLine("test!");
                }
                AbstractSpreadsheet ss = new Spreadsheet("save2.txt", s => true, s => s, "");
            }

            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadWriteException))]
            public void SaveTest5()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                ss.Save("save3.txt");
                ss = new Spreadsheet("save3.txt", s => true, s => s, "version");
            }

            [TestMethod()]
            public void SaveTest6()
            {
                AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "hello");
                ss.Save("save4.txt");
                Assert.AreEqual("hello", new Spreadsheet().GetSavedVersion("save4.txt"));
            }

            [TestMethod()]
            public void SaveTest7()
            {
                using (XmlWriter writer = XmlWriter.Create("save5.txt"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", "");

                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", "A1");
                    writer.WriteElementString("contents", "hello");
                    writer.WriteEndElement();

                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", "A2");
                    writer.WriteElementString("contents", "5.0");
                    writer.WriteEndElement();

                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", "A3");
                    writer.WriteElementString("contents", "4.0");
                    writer.WriteEndElement();

                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", "A4");
                    writer.WriteElementString("contents", "= A2 + A3");
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                AbstractSpreadsheet ss = new Spreadsheet("save5.txt", s => true, s => s, "");
                VV(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
            }

            [TestMethod()]
            public void SaveTest8()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Set(ss, "A1", "hello");
                Set(ss, "A2", "5.0");
                Set(ss, "A3", "4.0");
                Set(ss, "A4", "= A2 + A3");
                ss.Save("save6.txt");
                using (XmlReader reader = XmlReader.Create("save6.txt"))
                {
                    int spreadsheetCount = 0;
                    int cellCount = 0;
                    bool A1 = false;
                    bool A2 = false;
                    bool A3 = false;
                    bool A4 = false;
                    string name = null;
                    string contents = null;

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    Assert.AreEqual("default", reader["version"]);
                                    spreadsheetCount++;
                                    break;

                                case "cell":
                                    cellCount++;
                                    break;

                                case "name":
                                    reader.Read();
                                    name = reader.Value;
                                    break;

                                case "contents":
                                    reader.Read();
                                    contents = reader.Value;
                                    break;
                            }
                        }
                        else
                        {
                            switch (reader.Name)
                            {
                                case "cell":
                                    if (name.Equals("A1")) { Assert.AreEqual("hello", contents); A1 = true; }
                                    else if (name.Equals("A2")) { Assert.AreEqual(5.0, Double.Parse(contents), 1e-9); A2 = true; }
                                    else if (name.Equals("A3")) { Assert.AreEqual(4.0, Double.Parse(contents), 1e-9); A3 = true; }
                                    else if (name.Equals("A4")) { contents = contents.Replace(" ", ""); Assert.AreEqual("=A2+A3", contents); A4 = true; }
                                    else Assert.Fail();
                                    break;
                            }
                        }
                    }
                    Assert.AreEqual(1, spreadsheetCount);
                    Assert.AreEqual(4, cellCount);
                    Assert.IsTrue(A1);
                    Assert.IsTrue(A2);
                    Assert.IsTrue(A3);
                    Assert.IsTrue(A4);
                }
            }


            // Fun with formulas
            [TestMethod()]
            public void Formula1()
            {
                Formula1(new Spreadsheet());
            }
            public void Formula1(AbstractSpreadsheet ss)
            {
                Set(ss, "a1", "= a2 + a3");
                Set(ss, "a2", "= b1 + b2");
                Assert.IsInstanceOfType(ss.GetCellValue("a1"), typeof(FormulaError));
                Assert.IsInstanceOfType(ss.GetCellValue("a2"), typeof(FormulaError));
                Set(ss, "a3", "5.0");
                Set(ss, "b1", "2.0");
                Set(ss, "b2", "3.0");
                VV(ss, "a1", 10.0, "a2", 5.0);
                Set(ss, "b2", "4.0");
                VV(ss, "a1", 11.0, "a2", 6.0);
            }

            [TestMethod()]
            public void Formula2()
            {
                Formula2(new Spreadsheet());
            }
            public void Formula2(AbstractSpreadsheet ss)
            {
                Set(ss, "a1", "= a2 + a3");
                Set(ss, "a2", "= a3");
                Set(ss, "a3", "6.0");
                VV(ss, "a1", 12.0, "a2", 6.0, "a3", 6.0);
                Set(ss, "a3", "5.0");
                VV(ss, "a1", 10.0, "a2", 5.0, "a3", 5.0);
            }

            [TestMethod()]
            public void Formula3()
            {
                Formula3(new Spreadsheet());
            }
            public void Formula3(AbstractSpreadsheet ss)
            {
                Set(ss, "a1", "= a3 + a5");
                Set(ss, "a2", "= a5 + a4");
                Set(ss, "a3", "= a5");
                Set(ss, "a4", "= a5");
                Set(ss, "a5", "9.0");
                VV(ss, "a1", 18.0);
                VV(ss, "a2", 18.0);
                Set(ss, "a5", "8.0");
                VV(ss, "a1", 16.0);
                VV(ss, "a2", 16.0);
            }

            [TestMethod()]
            public void Formula4()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                Formula1(ss);
                Formula2(ss);
                Formula3(ss);
            }

            [TestMethod()]
            public void Formula4a()
            {
                Formula4();
            }


            [TestMethod()]
            public void MediumSheet()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                MediumSheet(ss);
            }

            public void MediumSheet(AbstractSpreadsheet ss)
            {
                Set(ss, "A1", "1.0");
                Set(ss, "A2", "2.0");
                Set(ss, "A3", "3.0");
                Set(ss, "A4", "4.0");
                Set(ss, "B1", "= A1 + A2");
                Set(ss, "B2", "= A3 * A4");
                Set(ss, "C1", "= B1 + B2");
                VV(ss, "A1", 1.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 3.0, "B2", 12.0, "C1", 15.0);
                Set(ss, "A1", "2.0");
                VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 4.0, "B2", 12.0, "C1", 16.0);
                Set(ss, "B1", "= A1 / A2");
                VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
            }

            [TestMethod()]
            public void MediumSheeta()
            {
                MediumSheet();
            }


            [TestMethod()]
            public void MediumSave()
            {
                AbstractSpreadsheet ss = new Spreadsheet();
                MediumSheet(ss);
                ss.Save("save7.txt");
                ss = new Spreadsheet("save7.txt", s => true, s => s, "default");
                VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
            }

            [TestMethod()]
            public void MediumSavea()
            {
                MediumSave();
            }


            // A long chained formula.  If this doesn't finish within 60 seconds, it fails.
            [TestMethod()]
            public void LongFormulaTest()
            {
                object result = "";
                Thread t = new Thread(() => LongFormulaHelper(out result));
                t.Start();
                t.Join(60 * 1000);
                if (t.IsAlive)
                {
                    t.Abort();
                    Assert.Fail("Computation took longer than 60 seconds");
                }
                Assert.AreEqual("ok", result);
            }

            public void LongFormulaHelper(out object result)
            {
                try
                {
                    AbstractSpreadsheet s = new Spreadsheet();
                    s.SetContentsOfCell("sum1", "= a1 + a2");
                    int i;
                    int depth = 100;
                    for (i = 1; i <= depth * 2; i += 2)
                    {
                        s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
                        s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
                    }
                    s.SetContentsOfCell("a" + i, "1");
                    s.SetContentsOfCell("a" + (i + 1), "1");
                    Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("sum1"), 1.0);
                    s.SetContentsOfCell("a" + i, "0");
                    Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("sum1"), 1.0);
                    s.SetContentsOfCell("a" + (i + 1), "0");
                    Assert.AreEqual(0.0, (double)s.GetCellValue("sum1"), 0.1);
                    result = "ok";
                }
                catch (Exception e)
                {
                    result = e;
                }
            }
        }
    }
}
