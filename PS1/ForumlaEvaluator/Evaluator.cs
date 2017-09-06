using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ForumlaEvaluator
{
    /// <summary>
    /// 
    /// </summary>
    public static class Evaluator
    {
        //regex checking if string is one or more letters followed by one or more digits
        public static Regex isVariable = new Regex("^[a-zA-Z]+[0-9]+");
        public delegate int Lookup(String v);

        public static int Evaluate(String expression, Lookup lookup)
        {
            Stack<String> operatorStack = new Stack<String>();
            Stack<int> valueStack = new Stack<int>();

            expression.Trim();
            string[] tokens = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            for (int tokenIndex = 0; tokenIndex < tokens.Length; tokenIndex++)
            {
                String token = tokens[tokenIndex];

                //ignore empty strings
                if (token == " " || token == "") 
                {
                    continue;
                }

                //handling int token
                else if (Int32.TryParse(token, out int t))
                {
                    if (operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        if (valueStack.Count() == 0)
                        {
                            throw new ArgumentException("The multiply and divide operations must be done on two values!");
                        }
                        valueStack.Push(Operation(operatorStack.Pop(), valueStack.Pop(), t));
                    }
                    else
                    {
                        valueStack.Push(t);
                    }
                }

                //handling variable token
                else if (isVariable.IsMatch(token.Trim()))
                {
                    int value = lookup(token.Trim());
                    if (operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        if (valueStack.Count() == 0)
                        {
                            throw new ArgumentException("The multiply and divide operations must be done on two values!");
                        }
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
                        if (valueStack.Count() < 2)
                        {
                            throw new ArgumentException("Operations in input string must be between two values!");
                        }
                        else
                        {
                            int right = valueStack.Pop();
                            valueStack.Push(Operation(operatorStack.Pop(), valueStack.Pop(), right));
                        }
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
                        if (valueStack.Count() < 2)
                        {
                            throw new ArgumentException("Operations in input string must be between two values!");
                        }
                        else
                        {
                            int right = valueStack.Pop();
                            valueStack.Push(Operation(operatorStack.Pop(), valueStack.Pop(), right));
                        }
                    }
                    if (operatorStack.Count != 0 && operatorStack.Peek() != "(")
                    {
                        throw new ArgumentException("Unclosed set of parenthesis!");
                    }
                    operatorStack.Pop();
                    if (operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        if (valueStack.Count() < 2)
                        {
                            throw new ArgumentException("The multiply and divide operations must be done on two values!");
                        }
                        else
                        {
                            int right = valueStack.Pop(); //order matters for division
                            valueStack.Push(Operation(operatorStack.Pop(), valueStack.Pop(), right));
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid token in the input expression!");
                }
            }
            if (operatorStack.Count() == 0)
            {
                if (valueStack.Count != 1)
                {
                    throw new ArgumentException("Read all operators and there's no single, final value of expression!");
                }
                return valueStack.Pop();
            }
            else
            {
                if (operatorStack.Count != 1 && valueStack.Count != 2 && (operatorStack.Pop() != "+" || operatorStack.Pop() != "-"))
                {
                    throw new ArgumentException("Input expression has been read, invalid format!");
                }
                int right = valueStack.Pop();
                 return Operation(operatorStack.Pop(), valueStack.Pop(), right);
            }
        }

        public static int Operation(String operation, int left, int right)
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
            if (operation == "/")
            {
                try
                {
                    return left / right;
                }
                catch
                {
                    throw new ArgumentException("Can't divide by 0!");
                }
            }
            else
            {
                throw new ArgumentException("The only valid operations are: + - * and /");
            }
        }
    }
}
