using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitorAPI.Util
{
    public class Helper
    {
        public static bool IsNumeric(string number)
        {
            try
            {
                for (int i = 0; i < number.Length; i++)
                {
                    if (number[i] < '0' || number[i] > '9')
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}