///
/// Jiahui Chen
/// u0980890
/// CS 3500 PS#
///
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTester
{
    /// <summary>
    /// Units tests for the Formula object/class. 
    /// </summary>
    [TestClass]
    public class FormulaTester
    {
        ///////////////////////////////////////////////  S Y N T A X     T E S T S    /////////////////////////////////////////////////
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidTokens1()
        {
            Formula invalid1 = new Formula("^");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidTokens2()
        {
            Formula invalid2 = new Formula("45 #");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidTokens3()
        {
            Formula invalid2 = new Formula("45 + (7 - 3) - 90) % 3");
        }

        [TestMethod]
        public void TestValidVariableNames()
        {
            Formula underscoreAfter = new Formula("U_67");
            Assert.IsNotNull(underscoreAfter);

            Formula underscoreFirst = new Formula(" _89");
            Assert.IsNotNull(underscoreFirst);
            Formula underscoreFirst1 = new Formula("__u70");
            Assert.IsNotNull(underscoreFirst1);

            Formula onlyLetter = new Formula("i");
            Assert.IsNotNull(onlyLetter);

            Formula onlyUnderscore = new Formula("________");
            Assert.IsNotNull(onlyUnderscore);
        }

        [TestMethod]
        public void TestScientificNotation()
        {
            Formula simpleSci = new Formula("2.378E-09");
            Assert.IsNotNull(simpleSci);

            Formula simpleSci1 = new Formula("2.378E30");
            Assert.IsNotNull(simpleSci1);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidDecimal()
        {
            Formula invalid1 = new Formula("9.08.0");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOneTokens()
        {
            Formula invalid1 = new Formula("      ");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestClosingParenthesis()
        {
            Formula invalid1 = new Formula("(1 + 3) + 5)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParenthesisMatch()
        {
            Formula invalid1 = new Formula("(1 + (3 + 5)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestStartingToken()
        {
            Formula invalid1 = new Formula(") 89");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEndingToken()
        {
            //Formula invalid1 = new Formula("7 * 9 -");
            Formula invalid = new Formula("9 -");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOneTokenInvalid()
        {
            Formula invalid1 = new Formula("+");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestTwoTokenInvalid()
        {
            Formula invalid1 = new Formula("( )");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOpParenFollow()
        {
            Formula inavlid = new Formula("78 + 9 * ( ( + 7 ))");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOperatorFollow()
        {
            Formula inavlid = new Formula("7.8 + 9 * 0.0 + (");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestCloseParenFollow()
        {
            Formula inavlid = new Formula("7.8 + (9 * 0.0) 89");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNumFollow()
        {
            Formula inavlid = new Formula("4.20 + 9.67 89");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestVarFollow()
        {
            Formula inavlid = new Formula("4.20 + 9.67 E78 E_98");
        }

        /// <summary>
        /// Testing to see if a variable validator returning false will be caught.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestFalseValidator()
        {
            Formula inavlid = new Formula("4.20 + E90", s => s, s => false);
        }

        /// <summary>
        /// Testing to see if an invalid variable normalizer will be caught.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidNormalizer()
        {
            Formula inavlid = new Formula("4.20 + E90", s => "^E", s => true);
        }

        /// <summary>
        /// Tests the get variables method.
        /// </summary>
        [TestMethod]
        public void TestGetVariables()
        {
            string none = String.Join(",", new Formula("8.90 + 8").GetVariables());
            Assert.AreEqual("", none);

            string one = String.Join(",", new Formula("_U89 + 0").GetVariables());
            Assert.AreEqual("_U89", one);

            string two = String.Join(" ", new Formula("_U89 + 0 * Ab78").GetVariables());
            Assert.AreEqual("_U89 Ab78", two);
        }
        //[TestMethod]
        //[ExpectedException(typeof(FormulaFormatException))]
        //public void TestParenthesis()
        //{
        //    Formula invalid2 = new Formula("8 + (9)");
        //}
    }
}
