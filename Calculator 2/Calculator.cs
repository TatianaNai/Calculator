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
        public static string OpenBrackets(string input)
        {
            int counterOpenBracket = 0;
            int indexOfOpenBracket = 0;
            int levelOfBrackets = 0;
            string output = string.Empty;
            Stack<string> stackOfString = new Stack<string>();

            // раскрываем скобки, пока не останется выражение без скобок
            while (input.Contains('('))
            {
                levelOfBrackets = FindLevelOfBrackets(input);
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '(')
                    {
                        counterOpenBracket++;
                        indexOfOpenBracket = i;
                        if (counterOpenBracket != levelOfBrackets)
                            stackOfString.Push("(");
                    }
                    else if (input[i] == ')')
                    {
                        if (counterOpenBracket != levelOfBrackets)
                        {
                            stackOfString.Push(")");
                            counterOpenBracket--;
                        }
                        else
                        {
                            stackOfString.Push(Calculate(input.Substring(indexOfOpenBracket + 1, i - (indexOfOpenBracket + 1))));
                            counterOpenBracket--;
                        }
                    }
                    else if (IsOperator(input[i]) && input[i - 1] == ')' && input[i + 1] == '(')
                    { stackOfString.Push(Convert.ToString(input[i])); }
                    else if (counterOpenBracket != levelOfBrackets)
                    {
                        if (Char.IsDigit(input[i]))
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
                        else
                        { stackOfString.Push(Convert.ToString(input[i])); }
                    }
                }
                input = string.Empty;

                string[] stringArray = new string[stackOfString.Count];
                for (int i = stackOfString.Count - 1; i >= 0; i--)
                {
                    stringArray[i] = stackOfString.Pop();
                }
                for (int i = 0; i < stringArray.Length; i++)
                {
                    input += stringArray[i];
                }
            }
            return output = Calculate(input);
        }

        private static string Calculate(string input)
        {
            string output = string.Empty;
            Stack<char> stackOfChar = new Stack<char>();

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
                    if (stackOfChar.Count() > 0)
                    {
                        while (stackOfChar.Count() != 0 && FindPpriority(input[i]) < FindPpriority(stackOfChar.Peek()))
                        {
                            output += stackOfChar.Pop() + " ";
                        }
                            stackOfChar.Push(input[i]);
                    }
                    else
                    {
                        stackOfChar.Push(input[i]);
                    }
                }
            }

            // переносим оставшиеся операторы из стека в строку
            while (stackOfChar.Count() > 0)
            { output += stackOfChar.Pop() + " "; }

            /* переносим числа в стек
               когда встречается оператор, производим вычисление над двумя последними числами в стеке
               результат добавляем в стек
            */
            string finalOutput = string.Empty;
            Stack<string> stackOfString = new Stack<string>();
            for (int i = 0; i < output.Length; i++)
            {
                if (Char.IsDigit(output[i]))
                {
                    string number = string.Empty;
                    while (Char.IsDigit(output[i]))
                    {
                        number += output[i];
                        i++;
                    }
                    stackOfString.Push(number);
                    i--;
                }
                else if (output[i] == ' ')
                    continue;
                else if (IsOperator(output[i]))
                {
                    double number2 = Convert.ToInt32(stackOfString.Pop());
                    double number1 = Convert.ToInt32(stackOfString.Pop());
                    stackOfString.Push(Convert.ToString(GetResult(output[i], number1, number2)));
                }
            }
            return finalOutput += stackOfString.Pop();
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

        private static int FindLevelOfBrackets (string input)
        {
            int result = 0;
            
            for (int i = 0; i < input.Length; i++) 
            {
                int amountOfBrackets = 0;
                while (input[i] == '(')
                {
                    amountOfBrackets++;
                    i++;
                }
                
                if (amountOfBrackets > result)
                    result = amountOfBrackets;
            }
            return result;
        }
    }
}
