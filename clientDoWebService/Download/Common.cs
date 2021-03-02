using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace clientDoWebService.Download
{
    public class Common
    {
        public static string GetFileDowLoadUrl(DownloadInfoModel model)
        {
            return ConfigurationManager.AppSettings["FileDowLoadUrl"] + model.Yljgbm + "/createfile/" + model.RndPrefix + "/delete/";
        }

        /// <summary>
        /// 创建一个文件记录数据的相关信息
        /// </summary>
        public static void WritFileTxt(DownloadInfoModel model,string filepath, string text)
        {
            FileStream fs = new FileStream( filepath+"\\"+model.RndPrefix+ ".txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(text);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
   
        }


        /// <summary>
        /// 创建一个文件记录数据的相关信息
        /// </summary>
        public static void AppendFileTxt(DownloadInfoModel model, string filepath, string text)
        {
            FileStream fs = new FileStream(filepath + "\\" + model.RndPrefix + ".txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.WriteLine(text);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();

        }


        /// <summary>
        /// 读写文件记录数据的相关信息
        /// </summary>
        public static string  RedFileTxt(DownloadInfoModel model, string filepath)
        {
            StreamReader sr = new StreamReader(filepath + "\\" + model.RndPrefix + ".txt", Encoding.Default);
            String line = sr.ReadToEnd();
            sr.Close();
            return line;
        }

        /// <summary>
        /// 删除生成的文件  
        /// </summary>
        public static void dropFileAll(string path)
        {
            //文件夹路径 
            DirectoryInfo dyInfo = new DirectoryInfo(path);
            //获取文件夹下所有的文件 
            foreach (FileInfo feInfo in dyInfo.GetFiles())
            {
                    feInfo.Delete();
            }
        }


        public static string GetFileAddress()
        {
            return ConfigurationManager.AppSettings["FileAddress"];
        }
        public static string[] getLIstFromStr(string paraStr, char splitChar=',')
        {
            //分割字符串
            string[] paraStrList = paraStr.Split(new char[] { splitChar });
            if (paraStrList.Length > 0)
            {
                return paraStrList;
            }
            return null;
        }

        public static string GetFileSize(string filePath)
        {
            if (File.Exists(filePath) == true)
            {
                FileInfo fileinfo = new FileInfo(filePath);
                return (fileinfo.Length / 1024).ToString();
            }
            return "0";
        }

        public static string[] GetJcsjTableList()
        {
            string jcsjStr = ConfigurationManager.AppSettings["JCSJ"];
            return getLIstFromStr(jcsjStr);
        }

        public static string[] GetJkdaTableList()
        {
            string jkdaStr = ConfigurationManager.AppSettings["JKDA"];
            return getLIstFromStr(jkdaStr);
        }

        public static string[] GetTjjgTableList()
        {
            string tjjgStr = ConfigurationManager.AppSettings["TJJG"];
            return getLIstFromStr(tjjgStr);
        }

        public enum DownloadType
        {
            JCSJ=1,
            JKDA=2,
            TJJG=3
        }
    }
}