using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace clientDoWebService.common
{
    public class CheckDownloadDataExpire
    {
        private static List<DownloadInfo> _downloadInfos = new List<DownloadInfo>();

        private static string DownloadDirPath { set; get; }

        private static DownloadInfo FindDownloadInfo(DownloadInfo info)
        {
            return GetDownloadInfos().Find(x => x.Yljgbm == info.Yljgbm && x.OpType == info.OpType);
        }
        

        private static void ClearDownloadInfos()
        {
            _downloadInfos.Clear();
        }

        private static void ClearDownloadInfo(DownloadInfo info)
        {
            GetDownloadInfos().Remove(info);
        }

        private static bool IsExpire(DownloadInfo info, int Interval)
        {
            TimeSpan ts = DateTime.Now - Convert.ToDateTime(info.CreateDate);
            if (ts.TotalMinutes > Interval)
            {
                //在此处可以先清理对应的文件
                ClearDownloadInfo(info);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static List<string> GetDownloadFilePath(DownloadInfo info)
        {
            List<string> result = new List<string>();
            string[] lines=info.DownloadFilePath.Split('|');
            for (int i = 1; i < lines.Count(); i++)
            {
                result.Add(string.Format("{0}{1}",DownloadDirPath,lines[i].Split('$')[0]));
            }
            return result;
        }

        private static bool HaveDownloadInfos()
        {
            return GetDownloadInfos().Count > 0 ? true : false;
        }

        private static bool HaveMatchConditionDownloadInfo(string dataType,string yljgbm)
        {
            return GetMatchConditionDownloadInfo(dataType,yljgbm) != null ? true : false;
        }

        private static DownloadInfo GetMatchConditionDownloadInfo(string dataType,string yljgbm)
        {
            return GetDownloadInfos().Find(x => x.OpType == dataType && x.Yljgbm==yljgbm);
        }

        private static bool HaveDownloadFile(DownloadInfo info)
        {
            bool result = true;
            List<string> files = GetDownloadFilePath(info);

            if (!CommonFile.IsExist(files)) //如果缺少文件
            {
                ClearDownloadInfo(info);
                result = false;
            }
            return result;
        }

        private static List<DownloadInfo> GetDownloadInfos()
        {
            return _downloadInfos;
        }

        

        public static List<DownloadInfo> DownloadInfos
        {
            set { _downloadInfos = value; }
            get { return GetDownloadInfos(); }
        }

        public static void AddDownloadInfo(DownloadInfo info)
        {
            DownloadInfo currInfo = FindDownloadInfo(info);
            if (currInfo == null)
            {
                _downloadInfos.Add(info);
            }
            else
            {
                currInfo.DownloadFilePath = info.DownloadFilePath;
                currInfo.CreateDate = info.CreateDate;
            }

        }

        public static string GetDownloadFilePath(string dataType,string yljgbm)
        {
            return GetMatchConditionDownloadInfo(dataType,yljgbm).DownloadFilePath;
        }

        public static bool CanDirectDownload(string dirPath,string dataType,int interval,string yljgbm)
        {
            DownloadDirPath = dirPath;
            if (HaveDownloadInfos() && HaveMatchConditionDownloadInfo(dataType,yljgbm) &&
                !IsExpire(GetMatchConditionDownloadInfo(dataType,yljgbm), interval) 
                && HaveDownloadFile(GetMatchConditionDownloadInfo(dataType,yljgbm)))
            {
                return true;
            }
            return false;
        }
        

        
        
    }

    public class DownloadInfo
    {
        private static string _createDate = null;
        public DownloadInfo(string downloadFilePath,string opType,string yljgbm)
        {
            DownloadFilePath = downloadFilePath;
            OpType = opType;
            Yljgbm = yljgbm;
            
            CreateDate = DateTime.Now.ToString();
        }
        public string DownloadFilePath { set; get; }
        public string OpType { set; get; }
        public string Yljgbm { set; get; }
        public string CreateDate
        {
            set { _createDate = value; }
            get { return _createDate; }
        }
    }
}