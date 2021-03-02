using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TJClient.Signname
{
    public class FileHelper
    {
        /// <summary>
        /// 将文件从一个位置复制到另一个位置。如果文件存在，就覆盖。
        /// </summary>
        /// <param name="path">原文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns></returns>
        public static bool CopyTo(string path, string targetPath)
        {
            if (!FileExists(path))
            {
                return false;
            }

            string fileName = Path.GetFileName(path);
            targetPath = Path.Combine(targetPath, fileName);
            File.Copy(path, targetPath, true);
            return true;
        }
        /// <summary>
        /// 如果文件存在，就删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            if (FileExists(path))
            {
                File.Delete(path);
            }
            return true;
        }
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            return false;
        }
    }
}
