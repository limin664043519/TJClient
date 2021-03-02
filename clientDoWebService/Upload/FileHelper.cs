using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using clientDoWebService.common;

namespace clientDoWebService.Upload
{
    public class FileHelper
    {
        /// <summary>
        /// 复制文件夹中的所有文件到指定文件夹
        /// </summary>
        /// <param name="DirectoryPath">源文件夹路径</param>
        /// <param name="DirAddress">保存路径</param>
        public static void CopyDirectoryOneFile(string DirectoryPath, string DirAddress)//复制文件夹，
        {
            //保存文件的路径
            string filePathTo = Path.GetDirectoryName(DirAddress);

            //保存文件的文件名称
            string fileName = Path.GetFileName(DirAddress);

            try
            {
                //文件目录的处理
                if (!Directory.Exists(filePathTo)) Directory.CreateDirectory(filePathTo);

                //获取要复制的文件
                if (File.Exists(DirectoryPath))
                {
                    //取得要copy的文件
                    if (File.Exists(DirAddress))
                    {
                        File.Delete(DirAddress);
                    }
                    //将临时文件夹的文件移到应用程序所在的目录下   
                    File.Move(DirectoryPath, DirAddress);
                }

                TxtLogger.Debug(string.Format("{0}:[{1}{2}]]", "copyDirectoryOneFile", DirAddress, "数据复制完成"));
            }
            catch (Exception ex)
            {
                TxtLogger.Error(string.Format("{0}:[{1}{2}]:[{3}]", "DoFileIsExcute", DirectoryPath, "设定程序启动状态错误", ex.Message));
                throw new Exception("copyDirectoryOneFile处理错误：" + fileName + ":" + ex.Message);
            }
        }

        public static bool CopyDirectory(string DirectoryPath, string DirAddress)//复制文件夹，
        {
            //是否存在要处理的文件
            bool isExistsFile = false;
            //要处理的文件数量
            int fileCount = 0;
            try
            {
                if (!Directory.Exists(DirAddress)) Directory.CreateDirectory(DirAddress);

                DirectoryInfo DirectoryArray = new DirectoryInfo(DirectoryPath);
                FileInfo[] Files = DirectoryArray.GetFiles();//获取该文件夹下的文件列表
                DirectoryInfo[] Directorys = DirectoryArray.GetDirectories();//获取该文件夹下的文件夹列表 
                foreach (FileInfo theFile in Files)//逐个复制文件     
                {
                    //如果临时文件夹下存在与应用程序所在目录下的文件同名的文件，则删除应用程序目录下的文件   
                    if (File.Exists(DirAddress + "\\" + Path.GetFileName(theFile.FullName)))
                    {
                        File.Delete(DirAddress + "\\" + Path.GetFileName(theFile.FullName));
                        //将临时文件夹的文件移到应用程序所在的目录下   
                        File.Move(theFile.FullName, DirAddress + "\\" + Path.GetFileName(theFile.FullName));
                        fileCount++;
                    }
                    else
                    {
                        //将临时文件夹的文件移到应用程序所在的目录下   
                        File.Move(theFile.FullName, DirAddress + "\\" + Path.GetFileName(theFile.FullName));
                        fileCount++;
                    }
                }
                ////删除处理完的文件目录
                foreach (DirectoryInfo Dir in Directorys)//逐个获取文件夹名称，并递归调用方法本身     
                {
                    bool result = CopyDirectory(DirectoryPath + "\\" + Dir.Name, DirAddress + "\\" + Dir.Name);
                    if (result == true)
                    {
                        isExistsFile = true;
                    }
                }

                //只要存在要处理的文件就返回true
                if (fileCount > 0)
                {
                    isExistsFile = true;
                }
            }
            catch (Exception ex)
            {
                TxtLogger.Error(string.Format("{0}:[{1}{2}]:[{3}]:[{4}]", "copyDirectory", DirectoryPath,
                    "设定程序启动状态错误", ex.Message, ex.StackTrace));
                return false;
            }
            return isExistsFile;
        }

        public static FileInfo[] GetFilesInDirectory(string dirPath)
        {
            //处理文件内容到数据库中
            DirectoryInfo DirectoryArray = new DirectoryInfo(dirPath);

            //获取该文件夹下的文件列表
            return DirectoryArray.GetFiles();
        }

        public static DirectoryInfo[] GetDirectorysInDirectory(string dirPath)
        {
            //处理文件内容到数据库中
            DirectoryInfo DirectoryArray = new DirectoryInfo(dirPath);
            return DirectoryArray.GetDirectories();
        }
    }
}