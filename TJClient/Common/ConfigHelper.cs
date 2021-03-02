using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJClient.Common
{
    public class ConfigHelper
    {
        private static string _qianmaiFactory = "qianmai";
        private static string GetLisFactory()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["LisFactory"] != null)
            {
                return System.Configuration.ConfigurationManager.AppSettings["LisFactory"].ToLower();
            }

            return "";
        }

        public static bool DontNeedToCorrespondingLisTm()
        {
            if (GetLisFactory() == "0")
            {
                return true;
            }
            return false;
        }
        public static bool IsQianmai()
        {
            if(GetLisFactory()== _qianmaiFactory)
            {
                return true;
            }

            return false;
        }

        public static string GetConfigDir()
        {
            string dir = Path.Combine(Signname.Common.GetCurrRunExeDir(), "config");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static string GetDoctorConfigFilePath()
        {
            
            return Path.Combine(GetConfigDir(), "doctorconfig.txt");
        }

        public static string GetQianmaiExcelTemplate()
        {
            return Path.Combine(Signname.Common.GetCurrRunExeDir(), "exceltemplate/qianmai.xls");
        }

        public static string GetJinyuExcelTemplate()
        {
            return Path.Combine(Signname.Common.GetCurrRunExeDir(), "exceltemplate/jinyu.xls");
        }
    }
}
