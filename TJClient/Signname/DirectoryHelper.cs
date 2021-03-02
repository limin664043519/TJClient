using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TJClient.Signname
{
    public class DirectoryHelper
    {
        /// <summary>
        /// 获取目录，日期格式的目录
        /// </summary>
        /// <returns></returns>
        public static string GetDirectory()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 获取随机目录，日期格式的目录，目录到毫秒
        /// </summary>
        /// <returns></returns>
        public static string GetRandomDirectory()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public static bool CreateDirectory(string path)
        {
            if (!Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return true;
        }
        /// <summary>
        /// 如果目录存在，则删除目录
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public static bool DeleteDirectory(string path)
        {
            if (Exists(path))
            {
                Directory.Delete(path,true);
            }
            return true;
        }
        /// <summary>
        /// 得到目录下的所有文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="filter">文件后缀名</param>
        /// <returns></returns>
        public static List<string> GetAllFiles(string path, string filter = "png")
        {

            List<string> files = Directory.GetFiles(path, string.Format("*.{0}",filter)).ToList();
            return files;
        }

    }
}
