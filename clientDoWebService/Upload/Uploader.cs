using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using clientDoWebService.common;
using Microsoft.Win32.SafeHandles;
using Oracle.DataAccess.Client;

namespace clientDoWebService.Upload
{
    public class Uploader
    {
        public void DoFile(string clientUser, string GuidStr, string yljgbm)
        {
            TxtLogger.Debug(string.Format("{0}:[{1}]:[{2}]", "DoFile", clientUser, "文件处理开始"));
            //取得本地的保存文件地址
            string filePathAll = Common.GetUploadeFilePath();
            string filePathAllTo = Common.GetExcuteFilePath();
            try
            {
                //文件处理
                if (FileHelper.CopyDirectory(filePathAll, filePathAllTo))
                {
                    //将文件内容保存到数据库中
                    DoFileToDb(filePathAllTo, GuidStr, yljgbm, clientUser);

                    //处理完当前数据后再次扫描是否存在需要处理的文件
                    DoFile(clientUser, GuidStr, yljgbm);
                }
                TxtLogger.Debug(string.Format("{0}:[{1}]:[{2}]", "DoFile", clientUser, "文件处理结束"));
                DBLogger.Insert(new LoggerInfo(clientUser, "DoFile 文件处理结束", 1));
            }
            catch (Exception ex)
            {
                TxtLogger.Error(string.Format("{0}:[{1}]:[{2}]:[{3}]", "DoFile", clientUser, "文件处理异常", ex.Message));
                DBLogger.Insert(new LoggerInfo(clientUser, string.Format("DoFile文件处理异常,{0}", ex.Message), 0));
                throw ex;
            }
            finally
            {
                //将状态改变回来
                Checker.ChangeExcuteStatus();
            }
            return;
        }


        private bool FileOperation(string filePath,string fullName)
        {
            //如果目录不存在创建目录
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            //将处理的文件进行备份
            if (File.Exists(filePath + Path.GetFileName(fullName)))
            {
                File.Delete(filePath + Path.GetFileName(fullName));
            }
            if (File.Exists(fullName))
            {
                File.Move(fullName, filePath + Path.GetFileName(fullName));
            }
            return true;
        }

        

        

        

        private bool FileInfoOperation(FileInfo theFile,string guidStr,string yljgbm, string clientUser)
        {
            //备份已处理的文件的路径
            string filePathBack = Common.GetBackupFilePath() + "\\";
            string filePathBackError = Common.GetErrorFilePath() + "\\";
            //errorFile = theFile.Name;
            //将数据处理到数据库中
            bool result = ExcelToDb.DoExcelTextToDb(theFile.FullName, guidStr);

            if (result == true)  //为true时，导入临时表成功
            {
                DBLogger.Insert(DBLogger.GetLoggerInfo(theFile.Name, "导入临时表成功", guidStr, 1));
                FileOperation(filePathBack, theFile.FullName);
            }
            else
            {
                DBLogger.Insert(DBLogger.GetLoggerInfo(theFile.Name, "导入临时表失败", guidStr, 0));
                FileOperation(filePathBackError, theFile.FullName);
            }
            //在这里记录导入临时表的信息

            if (result)
            {
                //如果文件导入成功，立即执行临时表到正式表操作
                ExcelToDb.ExecuteProCreateData(clientUser, guidStr,yljgbm);
                //记录到正式表的信息
                DBLogger.Insert(DBLogger.GetLoggerInfo(theFile.Name, "导入正式表成功", guidStr, 1));
            }
            return true;
        }

        private bool DoFileToDb(string DirectoryPath, string GuidStr, string yljgbm, string clientUser)
        {

            bool isExistsFile = false;
            string errorFile = "";
            try
            {
                //获取该文件夹下的文件列表
                FileInfo[] Files = FileHelper.GetFilesInDirectory(DirectoryPath);

                //获取该文件夹下的文件夹列表 
                DirectoryInfo[] Directorys = FileHelper.GetDirectorysInDirectory(DirectoryPath);
                //逐个处理文件 
                foreach (FileInfo theFile in Files)
                {
                    FileInfoOperation(theFile, GuidStr, yljgbm, clientUser);
                }
                //逐个获取文件夹名称，并递归调用方法本身  
                foreach (DirectoryInfo Dir in Directorys)
                {
                    bool result = FileHelper.CopyDirectory(DirectoryPath + "\\" + Dir.Name, Common.GetBackupFilePath() + "\\" + Dir.Name);
                    if (result)
                    {
                        isExistsFile = true;
                    }
                }
                //只要存在要处理的文件就返回true
            }
            catch (Exception ex)
            {
                TxtLogger.Error(string.Format("{0}:[{1}]:[{2}]", "DoFileToDb", "数据保存异常", ex.Message));
                DBLogger.Insert(DBLogger.GetLoggerInfo(errorFile, ex.Message, GuidStr, 0));
                return false;
            }
            return isExistsFile;

        }
    }
}