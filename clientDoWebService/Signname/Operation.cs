using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace clientDoWebService.Signname
{
    public class Operation
    {
        private static string GetZipFileToDirPath()
        {
            return Path.Combine(Common.UnzipFilesPath(), DirectoryHelper.GetRandomDirectionName());
        }
        /// <summary>
        /// 上传使用。先解压缩，然后copy到指定目录中，然后删除对应的解压缩文件夹
        /// </summary>
        /// <param name="zipfilePath">压缩文件目录</param>
        /// <returns></returns>
        public static bool UnZipOperation(string zipfilePath)
        {
            string unzipFilePath = GetZipFileToDirPath();
            CompressHelper.UnZip(zipfilePath,unzipFilePath);
            List<string> files = DirectoryHelper.GetAllFiles(unzipFilePath);
            foreach (string file in files)
            {
                FileHelper.CopyTo(file, Common.SignnameImgUpFile());
            }
            DirectoryHelper.RemoveDirectory(unzipFilePath);
            return true;
        }
        /// <summary>
        /// 客户端下载使用。压缩文件，并返回压缩文件的路径
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool ZipOperation(DataTable dt,string filePath)
        {
            if (dt == null || dt.Rows.Count <= 0 || !dt.Columns.Contains("signnamepicpath"))
            {
                return false;
            }
            List<string> defaultSignnamePics = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string defaultSignnamePicDir = Common.DefaultSignnameImgDownload();
                string relativePath = row["SIGNNAMEPICPATH"].ToString();
                if (relativePath.IndexOf('/') == 0)
                {
                    relativePath = relativePath.Substring(1, relativePath.Length - 1);
                }
                string signnamePicPath = Path.Combine(defaultSignnamePicDir,relativePath);
                defaultSignnamePics.Add(signnamePicPath);
            }
            CompressHelper.Zip(defaultSignnamePics, filePath);
            return true;
        }
        
    }
}