using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Calculator_2
{
    public class Calculator
    {
        public static string FindResult(string input)
        {
            List<string> expressionList = new List<string>();
            Stack<string> stackOfString = OpenBrackets(input);

            if (stackOfString.Count > 1)
            {
                while (stackOfString.Count > 0)
                {
                    expressionList.Insert(0, stackOfString.Pop());
                }
                string expression = string.Join("", expressionList);
                stackOfString.Push(Calculate(expression));
                expressionList.Clear();
                return stackOfString.Pop();
            }
            else { return stackOfString.Pop(); }
        }

        private static Stack<string> OpenBrackets(string input)
        {
            Stack<string> stackOfString = new Stack<string>();
            List<string> expressionList = new List<string>();
            string expression = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(' || IsOperator(input[i]))
                {
                    stackOfString.Push(Convert.ToString(input[i]));
                }
                else if (input[i] == ')')
                {
                    while (stackOfString.Peek() != "(")
                    {
                        expressionList.Insert(0, stackOfString.Pop());
                    }
                    stackOfString.Pop();
                    expression = string.Join("", expressionList);
                    stackOfString.Push(Calculate(expression));
                    expressionList.Clear();
                }
                else if (Char.IsDigit(input[i]))
                {
                    string number = string.Empty;
                    while (Char.IsDigit(input[i]))
                    {
                        number += input[i];
                        i++;
                        if (i == input.Length)
                            break;
                    }
                    stackOfString.Push(number);
                    i--;
                }
            }
            return stackOfString;
        }

        private static string Calculate(string input)
        {
            string output = string.Empty;
            Stack<string> stackOfString = new Stack<string>();

            // переносим число в строку, оператор в стек
            // оператор переносим в строку к числам в соответствии с приоритетом
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    while (Char.IsDigit(input[i]))
                    {
                        output += input[i];
                        i++;
                        if (i == input.Length)
                            break;
                    }
                    output += " ";
                    i--;
                }
                else if (IsOperator(input[i]))
                {
                    string negativeNumber = string.Empty;
                    if (input[i] == '-' && i == 0)
                    {
                        negativeNumber += input[i];
                        i++;
                        while (Char.IsDigit(input[i]))
                        {
                            negativeNumber += input[i];
                            i++;
                        }
                        output += negativeNumber + " ";
                        i--;
                    }
                    else if (input[i] == '-' && IsOperator(input[i - 1]))
                    {
                        negativeNumber += input[i];
                        i++;
                        while (Char.IsDigit(input[i]))
                        {
                            negativeNumber += input[i];
                            i++;
                            if (i == input.Length)
                                break;
                        }
                        output += negativeNumber + " ";
                        i--;
                    }
                    else
                    {
                        if (stackOfString.Count() > 0)
                        {
                            while (stackOfString.Count() != 0 && FindPpriority(input[i]) < FindPpriority(Convert.ToChar(stackOfString.Peek())))
                            {
                                output += stackOfString.Pop() + " ";
                            }
                            stackOfString.Push(Convert.ToString(input[i]));
                        }
                        else
                        {
                            stackOfString.Push(Convert.ToString(input[i]));
                        }
                    }
                }
            }

            // переносим оставшиеся операторы из стека в строку
            while (stackOfString.Count() > 0)
            { output += stackOfString.Pop() + " "; }

            /* переносим числа в стек
               когда встречается оператор, производим вычисление над двумя последними числами в стеке
               результат добавляем в стек
            */
            Stack<double> stackOfDouble = new Stack<double>();
            List<string> expressionList = output.Split(" ").ToList();
            expressionList.RemoveAt(expressionList.Count - 1);

            foreach (string item in expressionList)
            {
                if (double.TryParse(item, out double number))
                {
                    stackOfDouble.Push(number);
                }
                else
                {
                    double number2 = stackOfDouble.Pop();
                    double number1 = stackOfDouble.Pop();
                    stackOfDouble.Push(GetResult(Convert.ToChar(item), number1, number2));
                }
            }
            return Convert.ToString(stackOfDouble.Pop());
        }

        private static int FindPpriority(char operator1)
        {
            int priority = 0;
            switch (operator1)
            {
                case '(':
                    priority = 0;
                    break;
                case '+':
                case '-':
                    priority = 1;
                    break;
                case '*':
                case '/':
                    priority = 2;
                    break;
            }
            return priority;
        }

        private static double GetResult(char operator1, double number1, double number2)
        {
            double result = 0;
            switch (operator1)
            {
                case '+':
                    result = Math.Ceiling(number1 + number2);
                    break;
                case '-':
                    result = Math.Ceiling(number1 - number2);
                    break;
                case '*':
                    result = Math.Ceiling(number1 * number2);
                    break;
                case '/':
                    result = Math.Ceiling(number1 / number2);
                    break;
            }
            return result;
        }

        private static bool IsOperator(char item)
        {
            if (item == '+' || item == '-' || item == '*' || item == '/')
                return true;
            else { return false; }
        }
    }
}
