using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace clientDoWebService.Download
{
    public class Download
    {
        //用于保存数据下载所用的信息
        public DownloadInfoModel _model = null;

        private bool IsNullOrEmpty(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return false;
            }
            return true;
        }

        protected bool CheckYljgbm(string yljgbm)
        {
            return IsNullOrEmpty(yljgbm);
        }

        protected bool CheckDataType(string dataType)
        {
            return IsNullOrEmpty(dataType);
        }

        protected bool Check()
        {
            if (_model != null)
            {
                if (CheckYljgbm(_model.Yljgbm) && CheckDataType(_model.DataType))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetFilePath_start(string fileName)
        {
            string filepath = Common.GetFileAddress() + _model.Yljgbm + "\\createfile\\" + _model.RndPrefix +"\\"+"start"+"\\";
            CreateFilePath(filepath);
            return filepath  + fileName;
        }

        public string GetFilePath_end(string fileName)
        {
            string filepath = Common.GetFileAddress() + _model.Yljgbm + "\\createfile\\" + _model.RndPrefix  + "\\" + "end" + "\\";
            CreateFilePath(filepath);
            return filepath + fileName;
        }

        public string GetFilePath_delete(string fileName)
        {
            string filepath = Common.GetFileAddress() + _model.Yljgbm + "\\createfile\\" + _model.RndPrefix + "\\" + "delete" + "\\";
            CreateFilePath(filepath);
            return filepath + fileName;
        }

        public string GetFilePath(string fileName)
        {
            string filepath = Common.GetFileAddress() + _model.Yljgbm + "\\createfile\\" + _model.RndPrefix ;
            CreateFilePath(filepath);
            return filepath + "\\" + fileName;
        }


        public string GetFilePath()
        {
            string filepath = Common.GetFileAddress() + _model.Yljgbm + "\\createfile\\" + _model.RndPrefix ;
            CreateFilePath(filepath);
            return filepath ;
        }


        protected void CreateFilePath(string FilePath)
        {
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            
        }



        protected string GetFileName(string tableName)
        {
            return _model.RndPrefix + "_" + _model.Yljgbm + "_" + tableName + ".xls";
        }

        protected string[] DownloadTableList(Common.DownloadType downloadType)
        {
            switch (downloadType)
            {
                case Common.DownloadType.JCSJ:
                    return Common.GetJcsjTableList();
                case Common.DownloadType.JKDA:
                    return Common.GetJkdaTableList();
                case Common.DownloadType.TJJG:
                    return Common.GetTjjgTableList();
                default:
                    return null;
            }
        }

    }
}