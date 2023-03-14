using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline.Common
{
    public class ReturnValue
    {

        
        public ReturnValue()
        {

        }

        public static string returnTrueOrFalse(bool status)
        {
            return status is true ? "Có" : "Không";
        }

        public static string StringToDecimal(long status)
        {
            return String.Format("{0:#,###,###.##}", status);
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool validateTime(long Bookingtime)
        {
            DateTime dt = DateTime.Today;
            var ca1 = new TimeSpan(7, 30, 0);
            
            var ca2 = new TimeSpan(9, 30, 0);
            var ca3 = new TimeSpan(13, 30, 0);
            var ca4 = new TimeSpan(15, 30, 0);
            var dtn = DateTime.Now.TimeOfDay;
            if (dtn > ca1)
            {   
                return true; // cho phep dat lich
            }
            else if(dtn > ca2)
            {
                return true;
            }
            else if (dtn > ca3)
            {
                return true;
            }    
            else if(dtn > ca4)
            {
                return true;
            }
            else
            {
                return false;
            }    
        }


    }
}