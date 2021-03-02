using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace TJClient.Signname
{
    public class CompressHelper
    {
        
        private static bool ZipFileWithStream(string fileToZip, ZipOutputStream zipStream)
        {
            //如果文件没有找到，则返回false
            if (!File.Exists(fileToZip))
            {
                //throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
                return false;
            }
            FileStream zipFile = null;
            ZipEntry zipEntry = null;
            bool res = true;
            try
            {
                zipFile = File.OpenRead(fileToZip);
                byte[] buffer = new byte[zipFile.Length];
                zipFile.Read(buffer, 0, buffer.Length);
                zipFile.Close();
                zipEntry = new ZipEntry(Path.GetFileName(fileToZip));
                zipStream.PutNextEntry(zipEntry);
                zipStream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (zipEntry != null)
                {
                }

                if (zipFile != null)
                {
                    zipFile.Close();
                }
                GC.Collect();
                GC.Collect(1);
            }
            return res;

        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileList">需要压缩的文件列表，文件的绝对路径</param>
        /// <param name="zipedFile">zip文件的保存路径，绝对路径</param>
        /// <param name="password">可选，是否使用密码</param>
        /// <returns></returns>
        public static bool Zip(IEnumerable<string> fileList, string zipedFile, string password = "")
        {
            using (var s = new ZipOutputStream(File.Create(zipedFile)))
            {
                s.SetLevel(6);
                if (!string.IsNullOrEmpty(password))
                {
                    s.Password = password;
                }
                foreach (string file in fileList)
                {
                    ZipFileWithStream(file, s);
                }
                s.Finish();
                s.Close();
                return true;
            }
        }
        /// <summary>
        /// 解压缩zip文件
        /// </summary>
        /// <param name="zipFilePath">压缩文件的路径</param>
        /// <param name="unZipDir">解压缩有图片保存的路径</param>
        public static void UnZip(string zipFilePath, string unZipDir)
        {
            if (zipFilePath == string.Empty)
            {
                throw new Exception("压缩文件不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("压缩文件不存在！");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("/"))
                unZipDir += "/";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            using (var s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(unZipDir + directoryName);
                    }
                    if (directoryName != null && !directoryName.EndsWith("/"))
                    {
                    }
                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                        {

                            int size;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }



    }
}
