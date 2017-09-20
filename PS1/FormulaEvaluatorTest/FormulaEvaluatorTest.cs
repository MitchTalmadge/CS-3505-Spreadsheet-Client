using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormulaEvaluatorTest
{
    /// <summary>
    /// Tests for the Evaluator in the FormulaEvaluator class. 
    /// Evaluates mathematical expressions. 
    /// </summary>
    [TestClass]
    public class FormulaEvaluatorTest
    {
        /// <summary>
        /// Testing one integer or one variable input, and odd spacing
        /// edge cases for these one value cases.
        /// </summary>
        [TestMethod]
        public void OneValue()
        {
            Assert.AreEqual(4, ForumlaEvaluator.Evaluator.Evaluate("4", Lookup));
            Assert.AreEqual(6, ForumlaEvaluator.Evaluator.Evaluate("UAB890", Lookup));
            Assert.AreEqual(6, ForumlaEvaluator.Evaluator.Evaluate("UAB890    ", Lookup));
            Assert.AreEqual(6, ForumlaEvaluator.Evaluator.Evaluate("       UAB890    ", Lookup));
            Assert.AreEqual(8, ForumlaEvaluator.Evaluator.Evaluate("    8   ", Lookup));
        }

        /// <summary>
        /// Testing with addition and subtraction expressions, including weird 
        /// spacing cases.
        /// </summary>
        [TestMethod]
        public void PlusMinusOperator()
        {
            Assert.AreEqual(10, ForumlaEvaluator.Evaluator.Evaluate("4 + 6", Lookup));
            Assert.AreEqual(7, ForumlaEvaluator.Evaluator.Evaluate("UAB89 + 2", Lookup));
            Assert.AreEqual(15, ForumlaEvaluator.Evaluator.Evaluate("UAB89    + 2 +   8", Lookup));
            Assert.AreEqual(3, ForumlaEvaluator.Evaluator.Evaluate("UAB89 - 2", Lookup));
            Assert.AreEqual(-2, ForumlaEvaluator.Evaluator.Evaluate("UAB89 - 5    - 2", Lookup));
        }

        /// <summary>
        /// Testing with multiplication and division expressions, including weird 
        /// spacing cases.
        /// </summary>
        [TestMethod]
        public void MultiplyDivideOperator()
        {
            Assert.AreEqual(10, ForumlaEvaluator.Evaluator.Evaluate("5   * 2", Lookup));
            Assert.AreEqual(24, ForumlaEvaluator.Evaluator.Evaluate("UB9 * 2 * 4", Lookup));
            Assert.AreEqual(7, ForumlaEvaluator.Evaluator.Evaluate("21 /   S33", Lookup));
            Assert.AreEqual(2, ForumlaEvaluator.Evaluator.Evaluate("28 / SEVEN77 / 2 ", Lookup));
        }

        /// <summary>
        /// Testing expressions with parenthesis, including weird 
        /// spacing cases.
        /// </summary>
        [TestMethod]
        public void Parenthesis()
        {
            Assert.AreEqual(5, ForumlaEvaluator.Evaluator.Evaluate("(5   * 2) / a8", Lookup));
            Assert.AreEqual(1, ForumlaEvaluator.Evaluator.Evaluate(" (   5 + 12) / 17", Lookup));
            Assert.AreEqual(2, ForumlaEvaluator.Evaluator.Evaluate(" ((5 + 1) * 2) / 6", Lookup));
        }

        /// <summary>
        /// Tests to ensure that order of operations is followed
        /// when expressions are evaluated. 
        /// </summary>
        [TestMethod]
        public void PEMDAS()
        {
            Assert.AreEqual(17, ForumlaEvaluator.Evaluator.Evaluate("6 + 5 * 2 + 1", Lookup));
            Assert.AreEqual(-6, ForumlaEvaluator.Evaluator.Evaluate(" 5 + 12 / 2 - 17", Lookup));
            Assert.AreEqual(2, ForumlaEvaluator.Evaluator.Evaluate(" ((5 + 1 * 2 - 3) * 2) / 4", Lookup));
        }

        /// <summary>
        /// Various invalid input tests which should all throw ArgumentExceptions. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void invalidInput()
        {
            ForumlaEvaluator.Evaluator.Evaluate("6 + 53ri * 2 + 1", Lookup);
            ForumlaEvaluator.Evaluator.Evaluate("i", Lookup);
            ForumlaEvaluator.Evaluator.Evaluate("6 + -1", Lookup);
            ForumlaEvaluator.Evaluator.Evaluate("6 + 1 * 8 )", Lookup);
            ForumlaEvaluator.Evaluator.Evaluate("9/0", Lookup);
            ForumlaEvaluator.Evaluator.Evaluate("", Lookup);
            ForumlaEvaluator.Evaluator.Evaluate("56 8 - 2", Lookup);
        }

        public static int Lookup(String variable)
        {
            return variable.Length;
        }
        

          //////////////////////////////// G R A D I N G     T E S T S    ////////////////////////////

         [TestMethod()]
    public void TestSingleNumber()
    {
      Assert.AreEqual(5, ForumlaEvaluator.Evaluator.Evaluate("5", s => 0));
    }

    [TestMethod()]
    public void TestSingleVariable()
    {
      Assert.AreEqual(13, ForumlaEvaluator.Evaluator.Evaluate("X5", s => 13));
    }

    [TestMethod()]
    public void TestAddition()
    {
      Assert.AreEqual(8, ForumlaEvaluator.Evaluator.Evaluate("5+3", s => 0));
    }

    [TestMethod()]
    public void TestSubtraction()
    {
      Assert.AreEqual(8, ForumlaEvaluator.Evaluator.Evaluate("18-10", s => 0));
    }

    [TestMethod()]
    public void TestMultiplication()
    {
      Assert.AreEqual(8, ForumlaEvaluator.Evaluator.Evaluate("2*4", s => 0));
    }

    [TestMethod()]
    public void TestDivision()
    {
      Assert.AreEqual(8, ForumlaEvaluator.Evaluator.Evaluate("16/2", s => 0));
    }

    [TestMethod()]
    public void TestArithmeticWithVariable()
    {
      Assert.AreEqual(6, ForumlaEvaluator.Evaluator.Evaluate("2+X1", s => 4));
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void TestUnknownVariable()
    {
            ForumlaEvaluator.Evaluator.Evaluate("2+X1", s => { throw new ArgumentException("Unknown variable"); });
    }

    [TestMethod()]
    public void TestLeftToRight()
    {
      Assert.AreEqual(15, ForumlaEvaluator.Evaluator.Evaluate("2*6+3", s => 0));
    }

    [TestMethod()]
    public void TestOrderOperations()
    {
      Assert.AreEqual(20, ForumlaEvaluator.Evaluator.Evaluate("2+6*3", s => 0));
    }

    [TestMethod()]
    public void TestParenthesesTimes()
    {
      Assert.AreEqual(24, ForumlaEvaluator.Evaluator.Evaluate("(2+6)*3", s => 0));
    }

    [TestMethod()]
    public void TestTimesParentheses()
    {
      Assert.AreEqual(16, ForumlaEvaluator.Evaluator.Evaluate("2*(3+5)", s => 0));
    }

    [TestMethod()]
    public void TestPlusParentheses()
    {
      Assert.AreEqual(10, ForumlaEvaluator.Evaluator.Evaluate("2+(3+5)", s => 0));
    }

    [TestMethod()]
    public void TestPlusComplex()
    {
      Assert.AreEqual(50, ForumlaEvaluator.Evaluator.Evaluate("2+(3+5*9)", s => 0));
    }

    [TestMethod()]
    public void TestComplexTimesParentheses()
    {
      Assert.AreEqual(26, ForumlaEvaluator.Evaluator.Evaluate("2+3*(3+5)", s => 0));
    }

    [TestMethod()]
    public void TestComplexAndParentheses()
    {
      Assert.AreEqual(194, ForumlaEvaluator.Evaluator.Evaluate("2+3*5+(3+4*8)*5+2", s => 0));
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void TestDivideByZero()
    {
      ForumlaEvaluator.Evaluator.Evaluate("5/0", s => 0);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void TestSingleOperator()
    {
            ForumlaEvaluator.Evaluator.Evaluate("+", s => 0);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void TestExtraOperator()
    {
            ForumlaEvaluator.Evaluator.Evaluate("2+5+", s => 0);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void TestExtraParentheses()
    {
            ForumlaEvaluator.Evaluator.Evaluate("2+5*7)", s => 0);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void TestInvalidVariable()
    {
            ForumlaEvaluator.Evaluator.Evaluate("xx", s => 0);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void TestPlusInvalidVariable()
    {
            ForumlaEvaluator.Evaluator.Evaluate("5+xx", s => 0);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void TestParensNoOperator()
    {
            ForumlaEvaluator.Evaluator.Evaluate("5+7+(5)8", s => 0);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEmpty()
    {
            ForumlaEvaluator.Evaluator.Evaluate("", s => 0);
    }

    [TestMethod()]
    public void TestComplexMultiVar()
    {
      Assert.AreEqual(6, ForumlaEvaluator.Evaluator.Evaluate("y1*3-8/2+4*(8-9*2)/14*x7", s => (s == "x7") ? 1 : 4));
    }

    [TestMethod()]
    public void TestComplexNestedParensRight()
    {
      Assert.AreEqual(6, ForumlaEvaluator.Evaluator.Evaluate("x1+(x2+(x3+(x4+(x5+x6))))", s => 1));
    }

    [TestMethod()]
    public void TestComplexNestedParensLeft()
    {
      Assert.AreEqual(12, ForumlaEvaluator.Evaluator.Evaluate("((((x1+x2)+x3)+x4)+x5)+x6", s => 2));
    }

    [TestMethod()]
    public void TestRepeatedVar()
    {
      Assert.AreEqual(0, ForumlaEvaluator.Evaluator.Evaluate("a4-a4*a4/a4", s => 3));
    }
    }
   
}


