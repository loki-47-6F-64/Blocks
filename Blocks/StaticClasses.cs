using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blocks
{
    public static class Function
    {
        public static int StringToInt(string sString)
        {
            int Multiply = 1;
            int Result = 0;
            for (int x = sString.Length - 1; x >= 0; x--)
            {
                if (sString[x] >= 48 && sString[x] <= 57)
                {
                    Result += ((int)sString[x] - 48) * Multiply;
                    Multiply *= 10; //Eerst 1 keer, dan 10 keer, dan 100 keer, enz.
                }
                else
                {
                    return -1; //Er is een non-getal in de string
                }
            }
            return Result;
        }
    }
    static class Constant
    {
        public const char chBlock = (char)9608;
        public const char chArrow = (char)8594;
    }
}
