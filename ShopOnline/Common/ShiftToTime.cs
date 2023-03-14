using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline.Common
{
    public static class ShiftToTime
    {
        public static string shiftToTime(long? ca)
        {
            switch (ca)
            {
                case 1:
                    return "7h30";                    
                    case 2:
                    return "9h30";
                    case 3:
                    return "13h30";
                case 4:
                    return "15h30";
                default:
                    return "Lỗi";
                    
            }
            
        }

        public static TimeSpan shiftToTime1(long ca)
        {

            switch (ca)
            {
                case 1:
                    return new TimeSpan(7, 30, 0);
                case 2:
                    return new TimeSpan(9, 30, 0);
                case 3:
                    return new TimeSpan(13, 30, 0);
                case 4:
                    return new TimeSpan(15, 30, 0);
                default:
                    return new TimeSpan(0, 0, 0);

            }

        }
    }
}