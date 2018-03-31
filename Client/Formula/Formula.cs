// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

///
/// Jiahui Chen
/// u0980890
/// CS 3500 PS3
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Array storing the tokens of this Formula object.
        /// </summary>
        private string[] tokens;

        /// <summary>
        /// Array storing the normalized version of variables of this Formula object,
        /// in order in which they occur in the formula.
        /// 
        /// Storing variables here makes GetVariables() much more efficient.
        /// </summary>
        private string[] variables;

        /// <summary>
        /// Normalizer delegate function passed in and set in the constructor.
        /// </summary>
        private Func<string, string> normalizer;

        /// <summary>
        /// Normalizer delegate function passed in and set in the constructor.
        /// </summary>
        private Func<string, bool> validator;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// /******************************************* REMOVED AS PART OF 3505 FINAL PROJECT***********************/
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// /******************************************* REMOVED AS PART OF 3505 FINAL PROJECT***********************/
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            if (formula == null)
            {
                throw new FormulaFormatException("Input formula can't be null!");
            }
            tokens = GetTokens(formula).ToArray();
            normalizer = normalize;
            validator = isValid;

            /******************************************* REMOVED AS PART OF 3505 FINAL PROJECT***********************/
            //All syntax and delegate normalizing/validating is done in this method
            //ValidateSyntax(tokens);
            /******************************************* REMOVED AS PART OF 3505 FINAL PROJECT***********************/
        }


        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<String> operatorStack = new Stack<String>();
            Stack<double> valueStack = new Stack<double>();

            try
            {
                for (int tokenIndex = 0; tokenIndex < tokens.Length; tokenIndex++)
                {
                    String token = tokens[tokenIndex];

                    //handling int token
                    if (Double.TryParse(token, out double t))
                    {
                        if (operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                        {
                            valueStack.Push(Operation(operatorStack.Pop(), valueStack.Pop(), t));
                        }
                        else
                        {
                            valueStack.Push(t);
                        }
                    }

                    //handling variable token
                    else if (ValidVariable(token))
                    {
                        double value;
                        try
                        {
                            value = lookup(token.Trim());
                        }
                        catch
                        {
                            throw new ArgumentException("This variable does not exist!");
                        }
                        if (operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                        {
                            valueStack.Push(Operation(operatorStack.Pop(), valueStack.Pop(), value));
                        }
                        else
                        {
                            valueStack.Push(value);
                        }
                    }

                    //handling plus or minus token
                    else if (token == "+" || token == "-")
                    {
                        if (operatorStack.Count != 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                        {
                            double right = valueStack.Pop();
                            valueStack.Push(Operation(operatorStack.Pop(), valueStack.Pop(), right));
                        }
                        operatorStack.Push(token);
                    }

                    //handling multiplication or division token and left parenthesis
                    else if (token == "/" || token == "*" || token == "(")
                    {
                        operatorStack.Push(token);
                    }

                    //handling right parenthesis
                    else if (token == ")")
                    {
                        if (operatorStack.Count != 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                        {
                            double right = valueStack.Pop();
                            valueStack.Push(Operation(operatorStack.Pop(), valueStack.Pop(), right));
                        }
                        operatorStack.Pop();
                        if (operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                        {
                            double right = valueStack.Pop(); //order matters for division
                            valueStack.Push(Operation(operatorStack.Pop(), valueStack.Pop(), right));
                        }
                    }
                }
                if (operatorStack.Count() == 0)
                {
                    return valueStack.Pop();
                }
                else
                {
                    double right = valueStack.Pop();
                    return Operation(operatorStack.Pop(), valueStack.Pop(), right);
                }
            }
            catch (DivideByZeroException)
            {
                return new FormulaError("Can't divide by zero!");
            }
            catch (ArgumentException)
            {
                return new FormulaError("Lookup method did not find a value for a variable in the formula!");
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return this.variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string stringFormula = "";

            foreach (string token in tokens)
            {
                if (ValidVariable(token))
                {
                    stringFormula += normalizer(token);
                }
                else if (Double.TryParse(token, out double num))
                {
                    stringFormula += num;
                }
                else
                {
                    stringFormula += token;
                }
            }
            return stringFormula;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is Formula))
            {
                return false;
            }
            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null))
            {
                if (ReferenceEquals(f2, null))
                {
                    return true;
                }
                return false;
            }
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null))
            {
                if (ReferenceEquals(f2, null))
                {
                    return false;
                }
                return true;
            }
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /******************************************* REMOVED AS PART OF 3505 FINAL PROJECT***********************/
        /// <summary>
        /// Method that ensures the overall syntax of the input formula,
        /// as well if the tokens are valid. Also adds valid variables to the
        /// variables field array. 
        /// 
        /// The allowed symbols are non-negative numbers written using double-precision 
        /// floating-point syntax; variables that consist of a letter or underscore followed by 
        /// zero or more letters, underscores, or digits; parentheses; and the four operator 
        /// symbols +, -, *, and /.  
        /// </summary>
        /// <param name="tokens"></param>
        //private void ValidateSyntax(IEnumerable<string> tokens)
        //{
        //    if (tokens.Count() < 1)
        //    {
        //        throw new FormulaFormatException("Formula must have at least one token!");
        //    }

        //    //counter, pointer, and boolean flag variables to check syntax
        //    int leftParenthesisCount = 0, rightParenthesisCount = 0;
        //    string firstToken = null, lastToken = null;
        //    bool followingOpenParen = false, followingCloseParen = false;

        //    //Set to ensure no repeat variables are added in constant lookup time
        //    HashSet<string> varSet = new HashSet<string>();
        //    //List to hold valid variables in this formula
        //    List<string> varList = new List<string>();

        //    for (int i = 0; i < tokens.Count(); i++)
        //    {
        //        string token = tokens.ElementAt(i);
        //        if (firstToken == null)
        //        {
        //            firstToken = token;
        //            //the first token of an expression must be a number, a variable, or an opening parenthesis.
        //            if (!FollowingRule(firstToken, "(", normalizer, validator))
        //            {
        //                throw new FormulaFormatException("The first token of the formula must be a number, variable, or openeing parenthesis!");
        //            }
        //        }
        //        //token after an opening parenthesis or operator must be a number, variable, or opening parenthesis.
        //        if (followingOpenParen == true)
        //        {
        //            if (!FollowingRule(token, "(", normalizer, validator))
        //            {
        //                throw new FormulaFormatException("Any token after an opening parenthesis or an operator" +
        //                    " must be either a number, a variable, or an opening parenthesis!");
        //            }
        //            followingOpenParen = false;
        //        }
        //        //token after a number, variable, or closing parenthesis must be either an operator or a closing parenthesis.
        //        if (followingCloseParen == true)
        //        {
        //            if (!IsOperator(token) && token != ")")
        //            {
        //                throw new FormulaFormatException("Any token aftera number, variable, " +
        //                    "or closing parenthesis must be either an operator or a closing parenthesis.");
        //            }
        //            followingCloseParen = false;
        //        }
        //        if (token == "(")
        //        {
        //            leftParenthesisCount++;
        //            followingOpenParen = true;
        //        }
        //        if (token == ")")
        //        {
        //            rightParenthesisCount++;
        //            followingCloseParen = true;
        //        }
        //        if (IsOperator(token))
        //        {
        //            followingOpenParen = true;
        //        }
        //        if (IsNum(token) || ValidVariable(token))
        //        {
        //            //if the token is a valid variable, handle it so it's added to the variables field array
        //            if (ValidVariable(token))
        //            {
        //                string var = normalizer(token);
        //                if (!varSet.Contains(var))
        //                {
        //                    varSet.Add(var);
        //                    varList.Add(var);
        //                }
        //            }
        //            followingCloseParen = true;
        //        }
        //        if (rightParenthesisCount > leftParenthesisCount)
        //        {
        //            throw new FormulaFormatException("Number of closing parenthesis is greater than opening parenthesis!");
        //        }
        //        if (i == tokens.Count() - 1)
        //        {
        //            lastToken = token;
        //            //the last token of an expression must be a number, a variable, or a closing parenthesis.
        //            if (!FollowingRule(lastToken, ")", normalizer, validator))
        //            {
        //                throw new FormulaFormatException("The last token of the formula must be a number, variable, or closing parenthesis!");
        //            }
        //        }
        //    }

        //    //ensuring parenthesis match
        //    if (rightParenthesisCount != leftParenthesisCount)
        //    {
        //        throw new FormulaFormatException("Unmatched parenthesis in input formula!");
        //    }

        //    this.variables = varList.ToArray();
        //}*
        /******************************************* REMOVED AS PART OF 3505 FINAL PROJECT*************************/

        /// <summary>
        /// Determines if a token adheres to the syntax rules of following an opening 
        /// parenthesis or operator or following a number, variable, or closing parenthesis. 
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="paren"></param>
        /// <returns></returns>
        private bool FollowingRule(string token, string paren, Func<string, string> normalizer, Func<string, bool> validator)
        {
            return Double.TryParse(token, out double _) || token == paren || ValidVariable(token);
        }

        /// <summary>
        /// Determines if the token is a valid variable by the base rules,
        /// not by the Validator delegate's rules passed into the Formula's constructor.
        /// 
        /// Any letter or underscore followed by any number of letters, digits, 
        /// and/or underscores would form a valid variable name.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool ValidVariable(String token)
        {
            return Regex.IsMatch(token, @"^[a-zA-Z_](?:[a-zA-Z_]|\d)*$") && validator(this.normalizer(token));
        }

        /// <summary>
        /// Determines if the token is an operator that can be handled
        /// by the formula evaluator. Can be a +, -, *, or /
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool IsOperator(String token)
        {
            return Regex.IsMatch(token, @"^[\+\-*/]$");
        }

        /// <summary>
        /// Determines if the token is a valid variable or double.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool IsNum(String token)
        {
            return Double.TryParse(token, out _);
        }

        private static double Operation(String operation, double left, double right)
        {
            if (operation == "+")
            {
                return left + right;
            }
            if (operation == "-")
            {
                return left - right;
            }
            if (operation == "*")
            {
                return left * right;
            }
            else //the only other operator is a / and now the check for dividing by 0 occurs
            {
                if (right == 0)
                {
                    throw new DivideByZeroException("Can't divide by 0!");
                }
                return left / right;
            }
        }
    }

    /******************************************* REMOVED AS PART OF 3505 FINAL PROJECT*************************/
    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    //public class FormulaFormatException : Exception
    //{
    //    /// <summary>
    //    /// Constructs a FormulaFormatException containing the explanatory message.
    //    /// </summary>
    //    public FormulaFormatException(String message)
    //        : base(message)
    //    {
    //    }
    //}
    /******************************************* REMOVED AS PART OF 3505 FINAL PROJECT*************************/

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}
