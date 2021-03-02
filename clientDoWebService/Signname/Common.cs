using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace clientDoWebService.Signname
{
    public static class Common
    {
        public static string SignnameImgUpFile()
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["SignnameImgUpFile"];
            return Path.Combine(filePath, @"signnamepics\tabletsignname\");
        }

        public static string DefaultSignnameImgDownload()
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["SignnameImgUpFile"];
            return filePath;
        }
        /// <summary>
        /// 下载使用的路径
        /// </summary>
        /// <returns></returns>
        public static string ZipFilesPath()
        {
            string path = HttpContext.Current.Server.MapPath("~/zipFiles");
            DirectoryHelper.CreateDirection(path);
            return path;
        }
        /// <summary>
        /// 上传使用的路径
        /// </summary>
        /// <returns></returns>
        public static string UnzipFilesPath()
        {
            string path = HttpContext.Current.Server.MapPath("~/UnzipFiles");
            DirectoryHelper.CreateDirection(path);
            return path;
        }
        public static string Md5(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            MD5CryptoServiceProvider p = new MD5CryptoServiceProvider();
            byte[] md5buffer = p.ComputeHash(fs);
            fs.Close();
            string md5Str = "";
            List<string> strList = new List<string>();
            for (int i = 0; i < md5buffer.Length; i++)
            {
                md5Str += md5buffer[i].ToString("x2");
            }
            return md5Str;
        }
    }
}