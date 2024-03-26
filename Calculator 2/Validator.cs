using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_2
{
    public class Validator
    {
        public static bool IsFirstElementCprrect(char item)
        {
            if (item == ')')
                return false;
            else return true;
        }

        public static bool IsAmountOfBracketsCprrect(string expression)
        {
            int counterbracket1 = 0;
            int counterbracket2 = 0;
            foreach (char item in expression)
            {
                if (item == '(')
                    counterbracket1++;
                if (item == ')')
                    counterbracket2++;
            }
            if (counterbracket1 == counterbracket2)
                return true;
            else return false;
        }

        public static bool IsNotLetter(string expression)
        {
            if (expression.Any(c => char.IsLetter(c)))
                return false;
            else return true;
        }
    }
}
    