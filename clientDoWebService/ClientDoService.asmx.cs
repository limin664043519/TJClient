using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using clientDoWebService.common;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Diagnostics;
using System.Logger;
using System.Threading;
using clientDoWebService.Signname;
using clientDoWebService.Upload;
using Common = clientDoWebService.Signname.Common;
using FileHelper = clientDoWebService.Signname.FileHelper;

namespace clientDoWebService
{
    /// <summary>
    /// ClientDoService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class ClientDoService : System.Web.Services.WebService
    {
        /// <summary>
        /// 书写日志对象
        /// </summary>
        public SimpleLogger logger = SimpleLogger.GetInstance();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        #region 文件下载

       /// <summary>
        /// 获取下载生成数据间隔时间，在这个间隔时间内，不需要重新生成数据。60代表60分钟,0代表用户下载永远重新生成，必须为整形数字
       /// </summary>
       /// <returns></returns>
        public int GetInterval()
        {
            int interval = 60;
            if (System.Configuration.ConfigurationManager.AppSettings["NoRecreateDataIntervalTimeForDownloadData"] != null)
            {
                interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["NoRecreateDataIntervalTimeForDownloadData"]);
            }
            return interval;
        }

        /// <summary>
        /// 获取生成的文件的暂存地址
        /// </summary>
        /// <returns></returns>
        public string GetDownloadDirPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["FileAddress"];
        }


        private static Object locker = new Object();

        #region 数据生成
        /// <summary>
        /// 启动文件处理线程
        /// </summary>
        [WebMethod]
        public string downLoadInfoByParm(string rndPrefix, string yljgbm, string czList, string dataType)
        {
            try
            {
                startThread_create(rndPrefix, yljgbm, czList, dataType);

            }
            catch (Exception ex)
            {
                return string.Format("0-{0}-{1}", "DoFileThread_create", ex.Message);
            }
            return string.Format("1-{0}-{1}", "DoFileThread", "正常结束");
        }

        /// <summary>
        /// 文件处理
        /// </summary>
        /// <param name="threadid"></param>
        public void DoFile_create(string rndPrefix, string yljgbm, string czList, string dataType)
        {
            downLoadFileCreate(rndPrefix, yljgbm, czList, dataType);
        }

        //数据生成的委托
        protected delegate void delegatetestfile(string rndPrefix, string yljgbm, string czList, string dataType);

        public void startThread_create(string rndPrefix, string yljgbm, string czList, string dataType)
        {
            delegatetestfile currentDaliyCollect = new delegatetestfile(DoFile_create);
            IAsyncResult iADaliyCollect = currentDaliyCollect.BeginInvoke(rndPrefix, yljgbm, czList,dataType, null, null);
            if (iADaliyCollect.IsCompleted)
            {
                currentDaliyCollect.EndInvoke(iADaliyCollect);
            }
        }

        /// <summary>
        /// 客户端下载文件生成处理    
        /// </summary>
        /// <param name="yljgbm"></param>
        /// <param name="czList"></param>
        /// <returns></returns>
        public string downLoadFileCreate(string rndPrefix,string yljgbm ,string czList,string dataType)
        {
            //lock (locker)
            //{
                string functionName = "downLoadInfoByParm";
                try
                {
                    //2017-08-24 mq添加,用于判断时间间隔
                    //if (CheckDownloadDataExpire.CanDirectDownload(GetDownloadDirPath(), dataType, GetInterval(), yljgbm))
                    //{
                    //    return CheckDownloadDataExpire.GetDownloadFilePath(dataType, yljgbm);
                    //}
                    DoServiceNew doservicenew = new DoServiceNew();
                    string resultStr = doservicenew.DoDownLoadInfo(rndPrefix, yljgbm, czList, dataType);

                    ////将生成的文件对应的保存到静态变量中
                    //CheckDownloadDataExpire.AddDownloadInfo(new DownloadInfo(resultStr, dataType, yljgbm));

                    //日志处理
                    TxtLogger.Debug(string.Format("{0}:[{1}{2}]:[{3}]", functionName, yljgbm, "数据下载结束", resultStr));
                    return resultStr;
                }
                catch (Exception ex)
                {
                    TxtLogger.Error(string.Format("{0}:[{1}{2}]:[{3}]", functionName, yljgbm, "数据下载错误", ex.Message));
                    throw new Exception(string.Format("{1}:处理异常：{0}", ex.Message,functionName));
                }
            //}
        }
        #endregion

        #region 数据下载
        /// <summary>
        /// 下载生成的文件
        /// </summary>
        [WebMethod]
        public string downLoadInfoByParm_upload(string rndPrefix, string yljgbm, string czList, string dataType)
        {
            string resultStr = "";
            try
            {
                DoServiceNew doservicenew = new DoServiceNew();
                resultStr = doservicenew.DoDownLoadInfo_create(rndPrefix, yljgbm, czList, dataType);
            }
            catch (Exception ex)
            {
                return string.Format("0-{0}-{1}", "DoFileThread_create", ex.Message);
            }
            return resultStr;
        }


        #endregion

        #endregion

        #region 上传

        #region 以文件的形式上传信息

        /// <summary>
        /// 创建保存文件内容的文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [WebMethod]
        public bool CreateFile(string fileName)
        {
            bool isCreate = true;
            try
            {
                //上传的文件的保存路径
                string filePath = System.Configuration.ConfigurationManager.AppSettings["ExcelUpFile"];
                string filePathAll = string.Format("{0}{1}\\{2}", filePath, "upLoade_execute", fileName);

                //首先设置上传服务器文件的路径  然后发布web服务 发布的时候要自己建一个自己知道的文件夹 "C:\NMGIS_Video\" "C:\NMGIS_Video\"
                FileStream fs = new FileStream(filePathAll, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Close();
                TxtLogger.Debug(string.Format("{0}:[{1} {2}]", "CreateFile", fileName, "数据上传的文件创建完成"));
                DBLogger.Insert(DBLogger.GetLoggerInfo(fileName, "数据上传的文件创建完成",1));
            }
            catch(Exception ex)
            {
                isCreate = false;
                TxtLogger.Error(string.Format("{0}:[{1} {2}]:[{3}]", "CreateFile", fileName, "数据上传错误", ex.Message));
                DBLogger.Insert(DBLogger.GetLoggerInfo(fileName, "数据上传错误," + ex.Message,0));
            }
            return isCreate;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        [WebMethod]
        public bool Append(string fileName, byte[] buffer)
        {
            bool isAppend = true;
            try
            {
                //上传的文件的保存路径
                string filePath = System.Configuration.ConfigurationManager.AppSettings["ExcelUpFile"];
                string filePathAll = string.Format("{0}{1}\\{2}", filePath, "upLoade_execute", fileName);
                //保存上传的内容
                FileStream fs = new FileStream(filePathAll, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Seek(0, SeekOrigin.End);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
                TxtLogger.Debug(string.Format("{0}:[{1} {2}]", "Append", fileName, "数据上传"));
                DBLogger.Insert(DBLogger.GetLoggerInfo(fileName, "数据上传正确",1));
            }
            catch(Exception ex)
            {
                isAppend = false;
                logger.Error(string.Format("{0}:[{1} {2}]:[{3}]", "Append", fileName, "数据上传错误", ex.Message));
                DBLogger.Insert(DBLogger.GetLoggerInfo(fileName, "数据上传错误,"+ex.Message,0));
            }
            return isAppend;
        }

        private bool ZipFileOperation(string fileName,string md5,string filePathAll)
        {
            string newZipFilePath = FileHelper.CopyTo(filePathAll);
            if (md5 == Common.Md5(newZipFilePath))
            {
                if (Operation.UnZipOperation(newZipFilePath))
                {
                    DBLogger.Insert(DBLogger.GetLoggerInfo(fileName, "上传的签名zip文件解压缩成功", 1));
                    return true;
                }
                DBLogger.Insert(DBLogger.GetLoggerInfo(fileName, "上传的签名zip文件解压缩失败", 0));
                return false;
            }
            else
            {
                DBLogger.Insert(DBLogger.GetLoggerInfo(fileName, "上传的签名zip文件验证不正确", 0));
                return false;
            }
        }




        /// <summary>
        /// 文件验证
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        [WebMethod]
        public bool Verify(string fileName, string md5)
        {
            bool isVerify = true;
            try
            {
                //文件保存的路径
                string filePathAll = Upload.Common.GetFilePathAll(fileName);
                string filePathAllTo = Upload.Common.GetFilePathAllTo(fileName);
                if (fileName.EndsWith("xls") == false)
                {
                    //先判断是否是zip签名图片文件，如果是，单独处理
                    //先将zip移动到zip_file目录中，再解压缩，进行复制操作
                    if (fileName.EndsWith("zip"))
                    {
                        return ZipFileOperation(fileName,md5,filePathAll);
                    }
                    filePathAllTo =Upload.Common.GetXdtImgFilePathToAll(fileName);
                }
                string uploadMessage = "上传数据验证结束";
                //判断文件是否正确
                if (md5 != Common.Md5(filePathAll))
                {
                    isVerify = false;
                    uploadMessage = "上传数据验证异常";
                    filePathAllTo = Upload.Common.GetErrorFilePath(fileName);
                }
                Upload.FileHelper.CopyDirectoryOneFile(filePathAll, filePathAllTo);
                TxtLogger.Debug(string.Format("{0}:[{1} {2}]", "Verify", fileName, uploadMessage));
                DBLogger.Insert(DBLogger.GetLoggerInfo(fileName, "上传数据验证结束", 1));
            }
            catch(Exception ex)
            {
                isVerify = false;
                TxtLogger.Error(string.Format("{0}:[{1} {2}]:[{3}]", "Verify", fileName, "上传数据验证异常", ex.Message));
                DBLogger.Insert(DBLogger.GetLoggerInfo(fileName, "上传数据验证异常,"+ex.Message,0));
            }
            return isVerify;
        }
        #endregion



        #region 开启文件处理线程


        /// <summary>
        /// 启动文件处理线程
        /// </summary>
        [WebMethod]
        public string DoFileThread(string clientUser, string GuidStr, string yljgbm)
        {
            try
            {
                //文件处理是否已经启动 true:已经启动  false:未启动
                if (Checker.DoFileIsExcute(clientUser) == false)
                {
                    startThread(clientUser, GuidStr, yljgbm);
                }
                TxtLogger.Debug(string.Format("{0}:[{1} {2}]]", "DoFileThread", clientUser, "上传数据处理程序启动"));
                DBLogger.Insert(new LoggerInfo(clientUser, "DoFileThread上传数据处理程序启动", 1));
            }
            catch (Exception ex)
            {
                TxtLogger.Error(string.Format("{0}:[{1} {2}]:[{3}]", "DoFileThread", clientUser, "上传数据处理程序启动异常", ex.Message));
                DBLogger.Insert(new LoggerInfo(clientUser,
                    string.Format("DoFileThread上传数据处理程序启动异常,{0}",ex.Message),0));
                return string.Format("0-{0}-{1}", "DoFileThread", ex.Message);
            }
            return string.Format("1-{0}-{1}", "DoFileThread", "正常结束");
        }

        /// <summary>
        /// 文件处理
        /// </summary>
        /// <param name="threadid"></param>
        public void DoFile(string clientUser, string GuidStr, string yljgbm)
        {
            Uploader uploader = new Uploader();
            uploader.DoFile(clientUser,GuidStr,yljgbm);
        }

        protected delegate void delegatetest(string clientUser, string GuidStr, string yljgbm);

        [WebMethod]
        public void startThread(string clientUser, string GuidStr, string yljgbm)
        {
            delegatetest currentDaliyCollect = new delegatetest(DoFile);
            IAsyncResult iADaliyCollect = currentDaliyCollect.BeginInvoke(clientUser,GuidStr,yljgbm, null, null);
            if (iADaliyCollect.IsCompleted)
            {
                currentDaliyCollect.EndInvoke(iADaliyCollect);
            }
        }





        #endregion

        #endregion

        #region 获取上传日志
        [WebMethod]
        public List<LoggerInfo> GetUploadLoggerInfos(string czy)
        {
            return DBLogger.Get(czy);
        }
        #endregion
    }

}
