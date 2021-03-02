using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace clientDoWebService.Signname
{
    public static class DirectoryHelper
    {
        public static string GetFiveNumRandom()
        {
            Random ro = new Random();
            int iUp = 99999;
            int iDown = 10000;
            int iResult = ro.Next(iDown, iUp);
            return iResult.ToString().Trim();
        }

        public static string GetRandomDirectionName()
        {
            return string.Format("{0}_{1}",DateTime.Now.ToString("yyyyMMddhhmmss"),GetFiveNumRandom());
        }

        public static bool CreateDirection(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            return true;
        }

        public static List<string> GetAllFiles(string path, string filter = "png")
        {
            List<string> files = Directory.GetFiles(path, "*.png").ToList();
            return files;
        }

        public static bool RemoveDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path,true);
            }
            return true;
        }
    }
}