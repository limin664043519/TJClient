using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJClient.Common;

namespace TJClient.Lis
{
    public class YqCommon
    {
        private static string GetYqXmlPath(string yqxh)
        {
            if (string.IsNullOrEmpty(yqxh))
            {
                return "";
            }
            return Common.Common.getyqPath(yqxh);
        }
        public static int GetTimerInterval(string yqxh)
        {
            string YqXmlPath = GetYqXmlPath(yqxh);
            if (string.IsNullOrEmpty(YqXmlPath))
            {
                return 5000;
            }
            XmlRW xmlrw = new XmlRW();
            return int.Parse(xmlrw.GetValueFormXML(YqXmlPath, "YQ_Interval")) * 1000;
        }

        public static string GetYqDateType(string yqxh)
        {
            string YqXmlPath = GetYqXmlPath(yqxh);
            if (string.IsNullOrEmpty(YqXmlPath))
            {
                return "";
            }
            XmlRW xmlrw = new XmlRW();
            return xmlrw.GetValueFormXML(YqXmlPath, "YQ_DateType");
        }

        public static string GetYQType(string yqxh)
        {
            string xmlPath = Common.Common.getyqPath(yqxh);
            XmlRW xmlrw = new XmlRW();
            return xmlrw.GetValueFormXML(xmlPath, "YQ_YQType");
        }
    }
}
