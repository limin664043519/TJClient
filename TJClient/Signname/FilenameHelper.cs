using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace TJClient.Signname
{
    public class FilenameHelper
    {
        /// <summary>
        /// 获取随机的三位数字
        /// </summary>
        /// <returns></returns>
        public static string GetThreeNumRandom()
        {
            Random ro = new Random();
            int iUp = 999;
            int iDown = 100;
            int iResult = ro.Next(iDown, iUp);
            return iResult.ToString().Trim();
        }
        /// <summary>
        /// 获取日期的文件名，到秒
        /// </summary>
        /// <returns></returns>
        public static string GenerateNewLongDateTime()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmss");
        }
        /// <summary>
        /// 获取签名图片的文件名
        /// </summary>
        /// <returns></returns>
        public static string SignnamePicFileName()
        {
            return string.Format("{0}_{1}.png",GenerateNewLongDateTime(),GetThreeNumRandom());
        }
        /// <summary>
        /// 获取压缩文件的文件名
        /// </summary>
        /// <param name="czy">操作员</param>
        /// <returns></returns>
        public static string ZipFileName(string czy)
        {
            return string.Format("{0}_signname.zip",czy);
        }
    }
}
