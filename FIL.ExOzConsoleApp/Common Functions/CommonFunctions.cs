using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.ExOzConsoleApp
{
    public class CommonFunctions
    {
        public CommonFunctions()
        {

        }
        public static string replaceSingleQuotes(string str)
        {
            return str = string.IsNullOrWhiteSpace(str) ? "" : str.Replace("'", "''");
        }
    }
}
