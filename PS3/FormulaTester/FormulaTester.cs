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
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestValidTokens1()
        {
            Formula invalid1 = new Formula("^");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestValidTokens2()
        {
            Formula invalid2 = new Formula("45 #");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestValidTokens3()
        {
            Formula invalid2 = new Formula("45 + (7 - 3) - 90) % 3");
        }

        [TestMethod]
        public void PublicTestValidVariableNames()
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
        public void PublicTestScientificNotation()
        {
            Formula simpleSci = new Formula("2.378E-09");
            Assert.IsNotNull(simpleSci);

            Formula simpleSci1 = new Formula("2.378E30");
            Assert.IsNotNull(simpleSci1);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestInvalidDecimal()
        {
            Formula invalid1 = new Formula("9.08.0");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestOneTokens()
        {
            Formula invalid1 = new Formula("      ");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestClosingParenthesis()
        {
            Formula invalid1 = new Formula("(1 + 3) + 5)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestParenthesisMatch()
        {
            Formula invalid1 = new Formula("(1 + (3 + 5)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestStartingToken()
        {
            Formula invalid1 = new Formula(") 89");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestEndingToken()
        {
            //Formula invalid1 = new Formula("7 * 9 -");
            Formula invalid = new Formula("9 -");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestOneTokenInvalid()
        {
            Formula invalid1 = new Formula("+");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestTwoTokenInvalid()
        {
            Formula invalid1 = new Formula("( )");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestOpParenFollow()
        {
            Formula inavlid = new Formula("78 + 9 * ( ( + 7 ))");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestOperatorFollow()
        {
            Formula inavlid = new Formula("7.8 + 9 * 0.0 + (");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestCloseParenFollow()
        {
            Formula inavlid = new Formula("7.8 + (9 * 0.0) 89");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestNumFollow()
        {
            Formula inavlid = new Formula("4.20 + 9.67 89");
        }
        
        [TestMethod]
        public void PublicTestValidParenthesis()
        {
            Formula valid = new Formula("8 + (9)");
            Assert.IsNotNull(valid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestVarFollow()
        {
            Formula inavlid = new Formula("4.20 + 9.67 E78 E_98");
        }

        /// <summary>
        /// Testing to see if a variable validator returning false will be caught.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PublicTestFalseValidator()
        {
            Formula inavlid = new Formula("4.20 + E90", s => s, s => false);
        }

        /// <summary>
        /// Tests the GetVariables method.
        /// </summary>
        [TestMethod]
        public void PublicTestGetVariables()
        {
            string none = String.Join(",", new Formula("8.90 + 8").GetVariables());
            Assert.AreEqual("", none);

            string one = String.Join(",", new Formula("_U89 + 0").GetVariables());
            Assert.AreEqual("_U89", one);

            string two = String.Join(" ", new Formula("_U89 + 0 * Ab78").GetVariables());
            Assert.AreEqual("_U89 Ab78", two);

            string justScientific = String.Join(" ", new Formula("3.89E9").GetVariables());
            Assert.AreEqual("", justScientific);

            string withScientific = String.Join(" ", new Formula("_U89 + 0 + 7.8E35 * Ab78 / xu89 + 7.8E4").GetVariables());
            Assert.AreEqual("_U89 Ab78 xu89", withScientific);
        }

        /// <summary>
        /// Tests the ToString method.
        /// </summary>
        [TestMethod]
        public void PublicTestToString()
        {
            Assert.AreEqual("78", new Formula("78  ").ToString());

            Assert.AreEqual("var+890.79+var", new Formula("x89   +   890.79 + hb89", s => "var", s => true).ToString());

            Assert.AreEqual("4.56E34+89+cs3500", new Formula("4.56E34   +89+  cs2420", s => "cs3500", s => true).ToString());
        }

        /// <summary>
        /// Tests the Equals method.
        /// </summary>
        [TestMethod]
        public void PublicTestEquals()
        {
            string nonFormula = "wow";
            Formula formula = new Formula("78");
            Assert.IsFalse(formula.Equals(nonFormula));

            Formula formula1 = new Formula("U98 + 89 + yu8");
            Formula formula2 = new Formula("u98 + 89 + YU8");
            Assert.IsFalse(formula1.Equals(formula2));

            Formula formula3 = new Formula("78888.90");
            Formula nullFormula = null;
            Assert.IsFalse(formula3.Equals(nullFormula));

            Formula formula4 = new Formula("x78 * 67");
            Formula formula5 = new Formula("x78 * 67");
            Assert.IsTrue(formula4.Equals(formula5));
        }

        /// <summary>
        /// Tests the GetHashCode method.
        /// </summary>
        [TestMethod]
        public void PublicTestHashCode()
        {
            string stringFormula = "7+ui89/9";
            Formula formula = new Formula("7 + ui89 / 9");
            Assert.AreEqual(stringFormula.GetHashCode(), formula.GetHashCode());
        }

        /// <summary>
        /// Tests the == operator override. 
        /// </summary>
        [TestMethod]
        public void PublicTestEqualsOperator()
        {
            Formula formula4 = new Formula("x78 * 67");
            Formula formula5 = new Formula("x78 * 67");
            Assert.IsTrue(formula4 == formula5);

            Formula formula1 = new Formula("U98 + 89 + yu8");
            Formula formula2 = new Formula("u98 + 89 + YU8");
            Assert.IsFalse(formula1 == formula2);

            Formula nullFormula = null;
            Formula nullFormula1 = null;
            Assert.IsTrue(nullFormula == nullFormula1);

            Assert.IsFalse(nullFormula == formula1);

            Assert.IsFalse(formula2 == nullFormula);
        }

        /// <summary>
        /// Tests the != operator override. 
        /// </summary>
        [TestMethod]
        public void PublicTestNotEqualsOperator()
        {
            Formula formula4 = new Formula("x78 * 67");
            Formula formula5 = new Formula("x78 * 67");
            Assert.IsFalse(formula4 != formula5);

            Formula formula1 = new Formula("U98 + 89 + yu8");
            Formula formula2 = new Formula("u98 + 89 + YU8");
            Assert.IsTrue(formula1 != formula2);

            Formula nullFormula = null;
            Formula nullFormula1 = null;
            Assert.IsFalse(nullFormula != nullFormula1);

            Assert.IsTrue(nullFormula != formula1);

            Assert.IsTrue(formula2 != nullFormula);
        }

        ////////////////////////////////////////////////////////////////  E V A L U A T E    T E S T S  ///////////////////////////////////////////////////////
        [TestMethod()]
        public void PublicTestSingleNumber()
        {
            Assert.AreEqual(5.2, new Formula("5.2").Evaluate(s => 0));
        }

        [TestMethod()]
        public void PublicTestSingleVariable()
        {
            Assert.AreEqual(45.90, new Formula("x5").Evaluate(s => 45.90));
        }

        [TestMethod()]
        public void PublicTestAddition()
        {
            Assert.AreEqual(8.5, new Formula("8.0 + .5 + 0").Evaluate(s => 3.45));
        }

        [TestMethod()]
        public void PublicTestSubtraction()
        {
            Assert.AreEqual(8.5, new Formula("10.0 - .5 - 1.0").Evaluate(s => 3.45));
        }

        [TestMethod()]
        public void PublicTestMultiplication()
        {
            Assert.AreEqual(6.8, (double)new Formula("2.125 * 3.2").Evaluate(s => 3.45), 1e-9);
        }

        [TestMethod()]
        public void PublicTestDivision()
        {
            Assert.AreEqual(28.01, (double)new Formula("89.632  / 3.2").Evaluate(s => 3.45), 1e-9);
        }

        [TestMethod()]
        public void PublicTestEvalScientificNotation()
        {
            Assert.AreEqual(28.01, new Formula("2.801E1").Evaluate(s => 3.45));

            Assert.AreEqual(78028.01, new Formula("2.801E1 + 7.8E4").Evaluate(s => 3.45));

            Assert.AreEqual(3.6E-4, new Formula("3.6E-4").Evaluate(s => 3.45));

            Assert.AreEqual(-77971.99, (double)new Formula("2.801E1 - 7.8E4").Evaluate(s => 3.45), 1e-9);

            Assert.AreEqual(2184.78, (double)new Formula("2.801E6 * 7.8E-4").Evaluate(s => 3.45), 1e-9);

            Assert.AreEqual(1.4, new Formula("2.8E1 / 2.0E1").Evaluate(s => 3.45));
        }

        [TestMethod()]
        public void PublicTestArithmeticWithVariable()
        {
            Assert.AreEqual(6.4, (double)new Formula("2 + x1").Evaluate(s => 4.4), 1e-9);
        }

        //    [testmethod()]
        //    [expectedexception(typeof(argumentexception))]
        //    public void testunknownvariable()
        //    {
        //        forumlaevaluator.evaluator.evaluate("2+x1", s => { throw new argumentexception("unknown variable"); });
        //    }

        [TestMethod()]
        public void PublicTestUnknownVar()
        {
            Assert.AreEqual(new FormulaError("Lookup method did not find a value for a variable in the formula!"),
                new Formula("2 + x1").Evaluate(s => throw new ArgumentException()));
        }

        //    [testmethod()]
        //    public void testlefttoright()
        //    {
        //        assert.areequal(15, forumlaevaluator.evaluator.evaluate("2*6+3", s => 0));
        //    }

        //    [testmethod()]
        //    public void testorderoperations()
        //    {
        //        assert.areequal(20, forumlaevaluator.evaluator.evaluate("2+6*3", s => 0));
        //    }

        //    [testmethod()]
        //    public void testparenthesestimes()
        //    {
        //        assert.areequal(24, forumlaevaluator.evaluator.evaluate("(2+6)*3", s => 0));
        //    }

        //    [testmethod()]
        //    public void testtimesparentheses()
        //    {
        //        assert.areequal(16, forumlaevaluator.evaluator.evaluate("2*(3+5)", s => 0));
        //    }

        //    [testmethod()]
        //    public void testplusparentheses()
        //    {
        //        assert.areequal(10, forumlaevaluator.evaluator.evaluate("2+(3+5)", s => 0));
        //    }

        //    [testmethod()]
        //    public void testpluscomplex()
        //    {
        //        assert.areequal(50, forumlaevaluator.evaluator.evaluate("2+(3+5*9)", s => 0));
        //    }

        //    [testmethod()]
        //    public void testcomplextimesparentheses()
        //    {
        //        assert.areequal(26, forumlaevaluator.evaluator.evaluate("2+3*(3+5)", s => 0));
        //    }

        //    [testmethod()]
        //    public void testcomplexandparentheses()
        //    {
        //        assert.areequal(194, forumlaevaluator.evaluator.evaluate("2+3*5+(3+4*8)*5+2", s => 0));
        //    }

        //    [testmethod()]
        //    [expectedexception(typeof(argumentexception))]
        //    public void testdividebyzero()
        //    {
        //        forumlaevaluator.evaluator.evaluate("5/0", s => 0);
        //    }

        //    [testmethod()]
        //    [expectedexception(typeof(argumentexception))]
        //    public void testsingleoperator()
        //    {
        //        forumlaevaluator.evaluator.evaluate("+", s => 0);
        //    }

        //    [testmethod()]
        //    [expectedexception(typeof(argumentexception))]
        //    public void testextraoperator()
        //    {
        //        forumlaevaluator.evaluator.evaluate("2+5+", s => 0);
        //    }

        //    [testmethod()]
        //    [expectedexception(typeof(argumentexception))]
        //    public void testextraparentheses()
        //    {
        //        forumlaevaluator.evaluator.evaluate("2+5*7)", s => 0);
        //    }

        //    [testmethod()]
        //    [expectedexception(typeof(argumentexception))]
        //    public void testinvalidvariable()
        //    {
        //        forumlaevaluator.evaluator.evaluate("xx", s => 0);
        //    }

        //    [testmethod()]
        //    [expectedexception(typeof(argumentexception))]
        //    public void testempty()
        //    {
        //        forumlaevaluator.evaluator.evaluate("", s => 0);
        //    }

        //    [testmethod()]
        //    public void testcomplexmultivar()
        //    {
        //        assert.areequal(6, forumlaevaluator.evaluator.evaluate("y1*3-8/2+4*(8-9*2)/14*x7", s => (s == "x7") ? 1 : 4));
        //    }

        //    [testmethod()]
        //    public void testcomplexnestedparensright()
        //    {
        //        assert.areequal(6, forumlaevaluator.evaluator.evaluate("x1+(x2+(x3+(x4+(x5+x6))))", s => 1));
        //    }

        //    [testmethod()]
        //    public void testcomplexnestedparensleft()
        //    {
        //        assert.areequal(12, forumlaevaluator.evaluator.evaluate("((((x1+x2)+x3)+x4)+x5)+x6", s => 2));
        //    }

        //    [testmethod()]
        //    public void testrepeatedvar()
        //    {
        //        assert.areequal(0, forumlaevaluator.evaluator.evaluate("a4-a4*a4/a4", s => 3));
        //    }
    }
}
