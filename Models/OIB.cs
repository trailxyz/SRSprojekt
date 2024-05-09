using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRSprojekt.Models
{
    public class OIB
    {
        public static bool checkOIB(string oib) 
        {
            if (oib.Length != 11) return false;
            long a;
            if(!long.TryParse(oib, out a)) return false;

            int b = 10;
            for (int i = 0; i < 10; i++) 
            {
                b=b+Convert.ToInt32(oib.Substring(i,1));
                b = b % 10;
                if (b == 0) b = 10;
                b *= 2;
                b = b % 11;
            }
            int kontrola = 11 - b;
            if (kontrola == 10) kontrola=0;
            return kontrola==Convert.ToInt32(oib.Substring(10,1));
        }
    }
}