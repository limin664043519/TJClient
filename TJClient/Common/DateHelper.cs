using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJClient.Common
{
    public class DateHelper
    {
        public static string Year()
        {
            return DateTime.Now.ToString("yyyy");
        }

        public static string CurrDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string ConvertToCurrDate(string dateTime)
        {
            return DateTime.Parse(dateTime).ToString("yyyy-MM-dd");
        }

        public static string ConvertToQmLisDate(string date)
        {
            return DateTime.Parse(date).ToString("yyyy/MM/dd");
        }

        public static string CurrDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
