using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotMajor
{
    [Serializable]
    public class Verification
    {
        public bool FirstUser(string chatID)
        {
            return false;
        }
        public bool IsDigitsOnly(string number)
        {
            bool result = false;
            foreach (char c in number)
            {
                if (c < '0' || c > '9')
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
        public bool NumberValidation(string number)
        {
            bool result = false;

            if (number.Length == 11 && IsDigitsOnly(number))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}