using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace BotMajor
{
    public static class StringEngine
    {
        public static string ExtractFromBrackets(string str)
        {
            //string pattern = @"\[(.*?)\]";
            //var matches = Regex.Matches(str, pattern);

            //foreach (Match m in matches)
            //{
            //    Console.WriteLine(m.Groups[1]);
            //}

            return Regex.Match(str, @"\(([^)]*)\)").Groups[1].Value;
        }
    }
}