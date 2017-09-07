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
        }

        public static int Lookup(String variable)
        {
            return variable.Length;
        }
    }
}
