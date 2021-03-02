using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TJClient.Signname
{
    public static class Common
    {
        /// <summary>
        /// 是否启用了签名功能
        /// </summary>
        /// <returns>true为启用，false为不启用</returns>
        public static bool ShowSignnameOperation()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["SignnameOperation"] ==null || 
                System.Configuration.ConfigurationManager.AppSettings["SignnameOperation"]!="1")
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取签字板的key
        /// </summary>
        /// <returns></returns>
        public static string TabletKey()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["TabletKey"] == null)
            {
                return "AgAZAPZTkH0EBVdhY29tClNESyBTYW1wbGUBAoECA2UA";
            }
            else
            {
                return System.Configuration.ConfigurationManager.AppSettings["TabletKey"];
            }
        }
        /// <summary>
        /// 获取签字图片的相对路径
        /// </summary>
        /// <param name="path">签字图片的完整路径</param>
        /// <returns></returns>
        public static string GetPicRelativePath(string path)
        {
            string result = "";
            if (path == null || path.Equals("")) throw new Exception("没有设定签名！");
            if (path.ToLower().Contains("default"))
            {
                //如果是默认签名路径
                result = path.Replace(GetCurrRunExeDir(), "");
            }
            else
            {
                //如果是手写板签名
                result = path.Replace(GetTabletSignnameDirectory(), "");
            }
            
            while (result.IndexOf('\\') == 0)
            {
                result=result.Remove(0, 1);
            }
            return result;
        }
        /// <summary>
        /// 获取默认签字的路径，服务端设置的签字，下拉框选择,私有方法
        /// </summary>
        /// <returns></returns>
        private static string GetDefaultTabletSignnameDirectory()
        {
            string path = Path.Combine(GetCurrRunExeDir(), "signnamepics\\default");
            DirectoryHelper.CreateDirectory(path);
            return path;
        }
        /// <summary>
        /// 获取签字板签名的默认路径
        /// </summary>
        /// <returns></returns>
        public static string GetTabletSignnameDirectory()
        {
            string tabletSignnamePicPath = Path.Combine(GetCurrRunExeDir(), "signnamepics\\tabletsignname");
            DirectoryHelper.CreateDirectory(tabletSignnamePicPath);
            return tabletSignnamePicPath;
        }
        /// <summary>
        /// 获取上传文件的保存路径
        /// </summary>
        /// <param name="yljgbm">医疗机构编码</param>
        /// <returns></returns>
        public static string GetUploadFileDepositPath(string yljgbm)
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["filePath"];
            return Path.Combine(filePath, yljgbm);
        }
        /// <summary>
        /// 获取当前运行程序的路径
        /// </summary>
        /// <returns></returns>
        public static string GetCurrRunExeDir()
        {
            //return Directory.GetCurrentDirectory();
            return Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
        }
        /// <summary>
        /// 获取默认签字的路径，服务端设置的签字，下拉框选择
        /// </summary>
        /// <returns></returns>
        public static string DefaultSignnamePicsDirPath()
        {
            return GetDefaultTabletSignnameDirectory();
        }

        public static int GetRandomInRange(int maxLength)
        {
            if (maxLength < 0)
            {
                maxLength = 0;
            }
            Random ran = new Random(Guid.NewGuid().GetHashCode());
            int rnd = ran.Next(0, maxLength);
            Debug.Write(rnd);
            return rnd;
        }
    }
}
