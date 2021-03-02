using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using clientDoWebService.common;

namespace clientDoWebService.Upload
{
    public class Common
    {
        public static string GetExcelUpFile()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ExcelUpFile"];
        }

        public static string GetFilePathAll(string fileName)
        {
            return string.Format("{0}{1}\\{2}", GetExcelUpFile(), "upLoade_execute", fileName);
        }

        public static string GetUploadeFilePath()
        {
            return string.Format("{0}upLoade",GetExcelUpFile());
        }

        public static string GetExcuteFilePath()
        {
            return string.Format("{0}excute", GetExcelUpFile());
        }

        public static string GetBackupFilePath()
        {
            return string.Format("{0}backup", GetExcelUpFile());
        }
        public static string GetErrorFilePath()
        {
            return string.Format("{0}Error", GetExcelUpFile());
        }

        public static string GetFilePathAllTo(string fileName)
        {
            return string.Format("{0}\\{1}", GetUploadeFilePath(), fileName);
        }

        public static string GetErrorFilePath(string fileName)
        {
            return string.Format("{0}{1}\\{2}", GetExcelUpFile(), "error", fileName);
        }

        public static string GetXdtImgUpFile()
        {
            return System.Configuration.ConfigurationManager.AppSettings["XdtImgUpFile"];

            //string path = System.AppDomain.CurrentDomain.BaseDirectory;
            //string filepath = string.Format("{0}\\img\\", path);

            //if (Directory.Exists(filepath) == false)//如果不存在就创建file文件夹
            //{
            //    Directory.CreateDirectory(filepath);
            //}
            //return filepath;

        }

        public static string GetXdtImgFilePathToAll(string fileName)
        {
            return string.Format("{0}{1}\\{2}", GetXdtImgUpFile(), DateTime.Now.ToString("yyyy-MM-dd"), fileName);
        }

        
    }
}