using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormulaEvaluatorTest
{
    [TestClass]
    public class FormulaEvaluatorTest
    {
        [TestMethod]
        public void OneValue()
        {
            Assert.AreEqual(4, ForumlaEvaluator.Evaluator.Evaluate("4", Lookup));
            Assert.AreEqual(6, ForumlaEvaluator.Evaluator.Evaluate("UAB890", Lookup));
            Assert.AreEqual(6, ForumlaEvaluator.Evaluator.Evaluate("UAB890    ", Lookup));
            Assert.AreEqual(6, ForumlaEvaluator.Evaluator.Evaluate("       UAB890    ", Lookup));
            Assert.AreEqual(8, ForumlaEvaluator.Evaluator.Evaluate("    8   ", Lookup));
        }

        [TestMethod]
        public void PlusMinusOperator()
        {
            Assert.AreEqual(10, ForumlaEvaluator.Evaluator.Evaluate("4 + 6", Lookup));
            Assert.AreEqual(7, ForumlaEvaluator.Evaluator.Evaluate("UAB89 + 2", Lookup));
            Assert.AreEqual(15, ForumlaEvaluator.Evaluator.Evaluate("UAB89    + 2 +   8", Lookup));
            Assert.AreEqual(3, ForumlaEvaluator.Evaluator.Evaluate("UAB89 - 2", Lookup));
            Assert.AreEqual(-2, ForumlaEvaluator.Evaluator.Evaluate("UAB89 - 5    - 2", Lookup));
        }

        [TestMethod]
        public void MultiplyDivideOperator()
        {
            Assert.AreEqual(10, ForumlaEvaluator.Evaluator.Evaluate("5   * 2", Lookup));
            Assert.AreEqual(24, ForumlaEvaluator.Evaluator.Evaluate("UB9 * 2 * 4", Lookup));
            Assert.AreEqual(7, ForumlaEvaluator.Evaluator.Evaluate("21 /   S33", Lookup));
            Assert.AreEqual(2, ForumlaEvaluator.Evaluator.Evaluate("28 / SEVEN77 / 2 ", Lookup));
        }

        [TestMethod]
        public void Parenthesis()
        {
            Assert.AreEqual(5, ForumlaEvaluator.Evaluator.Evaluate("(5   * 2) / a8", Lookup));
            Assert.AreEqual(1, ForumlaEvaluator.Evaluator.Evaluate(" (5 + 12) / 17", Lookup));
            Assert.AreEqual(2, ForumlaEvaluator.Evaluator.Evaluate(" ((5 + 1) * 2) / 6", Lookup));
        }

        [TestMethod]
        public void PEMDAS()
        {
            Assert.AreEqual(17, ForumlaEvaluator.Evaluator.Evaluate("6 + 5 * 2 + 1", Lookup));
            Assert.AreEqual(-6, ForumlaEvaluator.Evaluator.Evaluate(" 5 + 12 / 2 - 17", Lookup));
            Assert.AreEqual(2, ForumlaEvaluator.Evaluator.Evaluate(" ((5 + 1 * 2 - 3) * 2) / 4", Lookup));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void invalidInput()
        {
            ForumlaEvaluator.Evaluator.Evaluate("6 + 53ri * 2 + 1", Lookup);
            ForumlaEvaluator.Evaluator.Evaluate("6 + -1", Lookup);
            ForumlaEvaluator.Evaluator.Evaluate("6 + 1 * 8 )", Lookup);
            ForumlaEvaluator.Evaluator.Evaluate("9/0", Lookup);
        }

        public static int Lookup(String variable)
        {
            return variable.Length;
        }
    }
}
