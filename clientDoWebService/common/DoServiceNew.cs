using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using clientDoWebService.Download;
using clientDoWebService.Upload;
using System.IO;

namespace clientDoWebService.common
{
    public class DoServiceNew
    {

        private  string Operation(DownloadInfoModel model)
        {
            switch (model.DataType)
            {
                case "1":
                    BaseData basedata = new BaseData(model);
                    return basedata.Init(model).Get();
                case "2":
                    ArchivesData archivesdata = new ArchivesData(model);
                    return archivesdata.Init(model).Get();
                case "3":

                    HealthExamResult healthexamresult = new HealthExamResult(model);
                    return healthexamresult.Get();
            }
            return "";
        }
        public  string DoDownLoadInfo(string rndPrefix, string yljgbm, string czList, string dataType)
        {
            DownloadInfoModel model=new DownloadInfoModel()
            {
                RndPrefix = rndPrefix,
                Yljgbm = yljgbm,
                CzList=czList,
                DataType=dataType
            };
            try
            {
                return string.Format("1-{0}-{1}", clientDoWebService.Download.Common.GetFileDowLoadUrl(model), Operation(model));
            }
            catch (Exception ex)
            {
                return string.Format("0-{0}", ex.Message);
            }
        }


        /// <summary>
        /// 客户端下载数据
        /// </summary>
        /// <param name="rndPrefix"></param>
        /// <param name="yljgbm"></param>
        /// <param name="czList"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public  string DoDownLoadInfo_create(string rndPrefix, string yljgbm, string czList, string dataType)
        {
            DownloadInfoModel model = new DownloadInfoModel()
            {
                RndPrefix = rndPrefix,
                Yljgbm = yljgbm,
                CzList = czList,
                DataType = dataType
            };
            try
            {
                clientDoWebService.Download.Download download = new clientDoWebService.Download.Download();
                download._model = model;

                //删除临时文件夹下的所有文件
                clientDoWebService.Download.Common.dropFileAll(download.GetFilePath_delete(""));

                //将要下载的文件转移到临时文件夹
                FileHelper.CopyDirectory(download.GetFilePath_end(""), download.GetFilePath_delete(""));

                return string.Format("1-{0}-{1}-{2}", clientDoWebService.Download.Common.GetFileDowLoadUrl(model), getfilenameList(download.GetFilePath_delete("")), getCreateFileStatue(model, download.GetFilePath ()));
            }
            catch (Exception ex)
            {
                return string.Format("0-{0}", ex.Message);
            }
        }

        /// <summary>
        /// 获取要下载的文件的地址
        /// </summary>
        /// <returns></returns>
        public  string getfilenameList(string filePath)
        {
            string resultStr="";

            try
            {
                DirectoryInfo DirectoryArray = new DirectoryInfo(filePath);
                FileInfo[] Files = DirectoryArray.GetFiles();//获取该文件夹下的文件列表

                foreach (FileInfo theFile in Files)
                {
                    resultStr = resultStr + "|" + theFile.Name + "$" + clientDoWebService.Download.Common.GetFileSize(theFile.FullName);
                }
  
            }
            catch (Exception ex)
            {
                return ex.Message ;
            }
            return resultStr;

        }


        /// <summary>
        /// 返回文件的生成状态 1：全部生成完成  2：正在生成中
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public  string getCreateFileStatue(DownloadInfoModel model ,string filepath )
        {

            string strfileText = clientDoWebService.Download.Common.RedFileTxt(model, filepath);
            if (strfileText.Equals("end"))
            {
                clientDoWebService.Download.Common.WritFileTxt(model, filepath, "endend");
                return "2";
            } if (strfileText.Equals("endend"))
            {
                return "1";
            }
            else if (strfileText.ToLower().IndexOf ("error")>0)
            {
                return "3";
            }
            else
            {
                return "2";
            }

        }



    }
}