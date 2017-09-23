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
        //         .evaluator.evaluate("2+x1", s => { throw new argumentexception("unknown variable"); });
        //    }

        [TestMethod()]
        public void PublicTestUnknownVar()
        {
            Assert.AreEqual(new FormulaError("Lookup method did not find a value for a variable in the formula!"),
                new Formula("2 + x1").Evaluate(s => throw new ArgumentException()));
        }

        [TestMethod()]
        public void PublicTestDivideByZero()
        {
            Assert.AreEqual(new FormulaError("Can't divide by zero!"),
                new Formula("2.89 / 0 ").Evaluate(s => 8));
        }

        /// <summary>
        /// Testing with multiplication and division expressions, including weird 
        /// spacing cases.
        /// </summary>
        [TestMethod]
        public void MultiplyDivideOperator()
        {
            Assert.AreEqual(10.0,  new Formula("5   * 2").Evaluate(s => s.Length));
            Assert.AreEqual(24.0,  new Formula("UB9 * 2 * 4").Evaluate( s => s.Length));
            Assert.AreEqual(7.0, new Formula("21 /   S33").Evaluate( s => s.Length));
            Assert.AreEqual(2.0, new Formula("28 / SEVEN77 / 2 ").Evaluate( s => s.Length));
        }

        /// <summary>
        /// Testing expressions with parenthesis, including weird 
        /// spacing cases.
        /// </summary>
        [TestMethod]
        public void Parenthesis()
        {
            Assert.AreEqual(5.0, new Formula("(5   * 2) / a8").Evaluate( s => s.Length));
            Assert.AreEqual(1.0, new Formula(" (   5 + 12) / 17").Evaluate( s => s.Length));
            Assert.AreEqual(2.0, new Formula(" ((5 + 1) * 2) / 6").Evaluate( s => s.Length));
            Assert.AreEqual(2.0, new Formula("(2)").Evaluate( s => s.Length));
            Assert.AreEqual(2.15, new Formula("(4.3)/ (2)").Evaluate( s => s.Length));
        }

        /// <summary>
        /// Tests to ensure that order of operations is followed
        /// when expressions are evaluated. 
        /// </summary>
        [TestMethod]
        public void PEMDAS()
        {
            Assert.AreEqual(17.0, new Formula("6 + 5 * 2 + 1").Evaluate( s => s.Length));
            Assert.AreEqual(15.4, new Formula("2*6+3.4").Evaluate(s => s.Length));
            Assert.AreEqual(-6.0, new Formula(" 5 + 12 / 2 - 17").Evaluate( s => s.Length));
            Assert.AreEqual(2.0, new Formula(" ((5 + 1 * 2 - 3) * 2) / 4").Evaluate(s => s.Length));
        }

        //    [testmethod()]
        //    public void testpluscomplex()
        //    {
        //        assert.areequal(50,  .evaluator.evaluate("2+(3+5*9)", s => 0));
        //    }

        [TestMethod()]
        public void PublicTestComplexParentheses()
        {
            Assert.AreEqual(26.0, new Formula("2+3*(3+5)").Evaluate(s => 0));
            Assert.AreEqual(194.0, new Formula("2+3*5+(3+4*8)*5+2").Evaluate(s => 0));
        }

        [TestMethod()]
        public void PublicTestComplexMultiVar()
        {
            Assert.AreEqual(-2.25, (double)new Formula("y1*3-8.5/2+4*(8-9*2)/4*x7").Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
            Assert.AreEqual(6.0, new Formula("x1+(x2+(x3+(x4+(x5+x6))))").Evaluate(s => 1));
            Assert.AreEqual(12.0, new Formula("((((x1+x2)+x3)+x4)+x5)+x6").Evaluate(s => 2));
            Assert.AreEqual(0.0, new Formula("a4-a4*a4/a4").Evaluate(s => 3));
        }

        [TestMethod()]
        public void PublicTestEval2Formulas()
        {
            Formula six = new Formula("3 * 2");
            Assert.AreEqual(6.0, six.Evaluate(s => 1));
            Formula seven = new Formula("i9 + 4");
            Assert.AreEqual(7.0, seven.Evaluate(s => 3));
        }
    }
}
