using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace clientDoWebService.Signname
{
    public class FileHelper
    {
        public static bool CopyTo(string path,string targetPath)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            string fileName = Path.GetFileName(path);
            string fileNameNoExt = Path.GetFileNameWithoutExtension(fileName);
            if (fileNameNoExt.Length < 8) //文件名长度不能小于8
            {
                return false;
            }
            //取文件名的前8位做为目录名
            string dirName = fileNameNoExt.Substring(0, 8);
            targetPath = Path.Combine(targetPath, dirName);
            DirectoryHelper.CreateDirection(targetPath);
            targetPath = Path.Combine(targetPath, fileName);
            File.Copy(path,targetPath,true);
            return true;
        }

        public static string CopyTo(string path)
        {
            string targetPath = path.Replace("upLoade_execute", "zip_file");
            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }
            File.Move(path, targetPath);
            return targetPath;
        }
    }
}