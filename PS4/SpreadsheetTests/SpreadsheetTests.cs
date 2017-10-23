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
    }
}
