using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;
using Aspose.Words;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using System.Data;
using System.IO;
using System.Xml;
using System.Collections;
using TJClient.Common;
//using YCYL.Core.Common;
//using XpsPrint;

namespace YCYL.Client.Common
{
    class PrintHelper_excel
    {
        //保存打印文件
        public string printFilePath = "";
        public string defaultPrintName = "";

        //保存打印文件的列表
        public ArrayList printFilePathList = new ArrayList();

        /// <summary>
        /// 打印文档（不带明细）
        /// </summary>
        /// <param name="printName"></param>
        /// <param name="list"></param>
        /// <param name="documentName"></param>
        /// <returns></returns>
        public bool createdPrintDocument(System.Data.DataTable list, string filePathPara, string documentName, string printDocPath_save)
        {
            if (list == null || list.Rows.Count == 0)
            {
                return false;
            }

            try
            {

                //if (File.Exists(filePathPara) == false)
                //{
                //    File.Create(filePathPara);
                //    //存在
                //    return true;
                //}
                if (!Directory.Exists(printDocPath_save)) Directory.CreateDirectory(printDocPath_save);
                //产生要打印的文档
                //使用特殊字符串替换
                Aspose.Words.Document doc1 = new Aspose.Words.Document((filePathPara + documentName));

                for (int i = 0; i < list.Columns.Count; i++)
                {
                    if (list.Columns[i].ColumnName.IndexOf("img") == -1)
                    {
                        var repStr = string.Format("${0}$", list.Columns[i].ColumnName);
                        string printValue = list.Rows[0][list.Columns[i].ColumnName] == null ? "" : list.Rows[0][list.Columns[i].ColumnName].ToString();
                        doc1.Range.Replace(repStr, printValue, false, false);
                    }
                    else
                    {
                        var repStr = string.Format("${0}$", list.Columns[i].ColumnName);
                        Regex reg = new Regex(repStr);
                        string printValue = list.Rows[0][list.Columns[i].ColumnName] == null ? "" : list.Rows[0][list.Columns[i].ColumnName].ToString();
                        doc1.Range.Replace(reg, new ReplaceAndInsertImage(printValue), false);
                    }
                }

                //doc1.Range.Replace("$a2$", UserInfo.Jgmc, false, false);
                string filePath = string.Format("{0}{1}.doc", printDocPath_save, DateTime.Now.ToString("yyyyMMddhhmmssfff"));
                doc1.Save(filePath);//也可以保存为1.doc 兼容03-07
                printFilePathList.Add(filePath);
            }
            catch (Exception ex)
            {
                //return false;
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// 打印文档（图片）
        /// </summary>
        /// <param name="printName"></param>
        /// <param name="list"></param>
        /// <param name="documentName"></param>
        /// <returns></returns>
        public bool createdPrintDocument_img(string imgUrl, string filePathPara, string documentName, string printDocPath_save)
        {

            try
            {

                //if (!Directory.Exists(printDocPath_save)) Directory.CreateDirectory(printDocPath_save);
                ////产生要打印的文档
                ////使用特殊字符串替换
                //Aspose.Words.Document doc1 = new Aspose.Words.Document((filePathPara + documentName));
                //////var repStr = string.Format("&{0}&", "img");
                //////Regex reg = new Regex(repStr);
                //////string printValue = imgUrl;
                //////doc1.Range.Replace(reg, new ReplaceAndInsertImage(printValue), false);
                //Regex reg = new Regex("&头像&");
                //doc1.Range.Replace(reg, new ReplaceAndInsertImage(".//1.jpg"), false);

                ////doc1.Range.Replace("$a2$", UserInfo.Jgmc, false, false);
                //string filePath = string.Format("{0}{1}.doc", printDocPath_save, DateTime.Now.ToString("yyyyMMddhhmmssfff"));
                //doc1.Save(filePath);//也可以保存为1.doc 兼容03-07
                //printFilePathList.Add(filePath);



            var dic = new Dictionary<string, string>();
            dic.Add("img", ".//1.jpg");
            //使用特殊字符串替换
            Aspose.Words.Document doc = new Aspose.Words.Document((filePathPara + documentName));
            foreach (var key in dic.Keys)
            {
                if (key != "img")
                {
                    var repStr = string.Format("&{0}&", key);
                    doc.Range.Replace(repStr, dic[key], false, false);
                }
                else
                {
                    Regex reg = new Regex("&img&");
                    doc.Range.Replace(reg, new ReplaceAndInsertImage(imgUrl), false);
                }
            }
            string filePath = string.Format("{0}{1}.doc", printDocPath_save, DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            doc.Save(filePath);//也可以保存为1.doc 兼容03-07
            printFilePathList.Add(filePath);


            }
            catch (Exception ex)
            {
                //return false;
                throw ex;
            }

            return true;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool existsFile(string filePath)
        {
            try
            {
                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("WINWORD"))
                {
                    p.Kill();
                }
                if (Directory.Exists(filePath) == true)
                {
                   //File.Delete(filePath);
                    DirectoryInfo DirectoryArray = new DirectoryInfo(filePath);
                    FileInfo[] Files = DirectoryArray.GetFiles();//获取该文件夹下的文件列表
                    DirectoryInfo[] Directorys = DirectoryArray.GetDirectories();//获取该文件夹下的文件夹列表 
                    foreach (FileInfo theFile in Files)//逐个删除文件     
                    {
                        //如果临时文件夹下存在与应用程序所在目录下的文件同名的文件，则删除应用程序目录下的文件   
                        if (File.Exists(theFile.FullName))
                        {
                            try
                            {
                                File.Delete(theFile.FullName);
                            }
                            catch (Exception ex)
                            {
                                //Log.Error("打印文件删除失败！" + ex.Message);
                                
                            }
                        }
                    }
                  
                    //存在
                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }

        }

        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="printName"></param>
        /// <returns></returns>
        public bool printList(string printName)
        {
            try
            {
                if (printFilePathList != null && printFilePathList.Count > 0)
                {
                    for (int i = 0; i < printFilePathList.Count; i++)
                    {
                        printFilePath = printFilePathList[i].ToString();

                        //0：调用采用office组件打印 1：不调用office组件打印
                        //if (Common.GetConfigValue("013").Equals("0"))
                        //{
                            printByOffice(printName);
                        //}
                        //else
                        //{
                        //    printByAspos(printName);
                        //}
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }

        }


        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printName"></param>
        /// <returns></returns>
        public bool printByOffice(string printName)
        {
            if (printFilePathList.Count == 1)
            {
                //foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("WINWORD"))
                //{
                //    p.Kill();
                //}
            }
            //打印
            Microsoft.Office.Interop.Word.Application app = null;
            Microsoft.Office.Interop.Word.Document doc = null;
            object missing = System.Reflection.Missing.Value;
            object templateFile = System.IO.Path.GetFullPath(printFilePath);
            //System.IO.Path.GetFullPath("TextFile1.txt")
            try
            {
                app = new Microsoft.Office.Interop.Word.ApplicationClass();
                doc = app.Documents.Add(ref templateFile, ref missing, ref missing, ref missing);

                //保存默认打印机
                defaultPrintName = DefaultPrinter();
                app.ActivePrinter = printName;

                //打印  
                doc.PrintOut(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

            }
            catch (Exception exp)
            {
                //object saveChange = Microsoft.Office.Interop.Word.WdSaveOptions.wdPromptToSaveChanges;// .wdDoNotSaveChanges;                 
                //if (doc != null)
                //    doc.Close(ref saveChange, ref missing, ref missing);
                //if (app != null)
                //    app.Quit(ref missing, ref missing, ref missing);

                //foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("WINWORD"))
                //{
                //    p.Kill();
                //}
                throw exp;
            }
            finally
            {
                //删除文件
                //existsFile(templateFile.ToString());
            }
            return true;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printName"></param>
        /// <returns></returns>
        public bool printByAspos(string printName)
        {
            //生成打印对象
            PrintHelper_excel printDemo = new PrintHelper_excel();

            //保存默认打印机
            defaultPrintName = DefaultPrinter();

            //设定使用的打印机
            printDemo.setPrint(printName);

            Aspose.Words.Document document = new Aspose.Words.Document(printFilePath);



            //foreach (Aspose.Words.Section section in document)
            //{
            //    section.PageSetup.Orientation = Orientation.Landscape;
            //    section.PageSetup.TextColumns.SetCount(2);
            //    section.PageSetup.TextColumns.EvenlySpaced = true;
            //    section.PageSetup.TextColumns.LineBetween = true;

            //}  
            document.Print();
            //删除文件
            existsFile(printFilePath.ToString());
            return true;
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="printName"></param>
        /// <returns></returns>
        public bool printView(string printName)
        {
            //打印
            Microsoft.Office.Interop.Word.Application app = null;
            Microsoft.Office.Interop.Word.Document doc = null;
            object missing = System.Reflection.Missing.Value;
            object templateFile = printFilePath;
            try
            {
                app = new Microsoft.Office.Interop.Word.ApplicationClass();
                doc = app.Documents.Add(ref templateFile, ref missing, ref missing, ref missing);

                //保存默认打印机
                string defaultPrint = app.ActivePrinter;
                app.ActivePrinter = printName;
                //预览
                app.Visible = true;
                doc.PrintPreview();
            }
            catch (Exception exp)
            {
                object saveChange = Microsoft.Office.Interop.Word.WdSaveOptions.wdPromptToSaveChanges;// .wdDoNotSaveChanges;                 
                if (doc != null)
                    doc.Close(ref saveChange, ref missing, ref missing);
                if (app != null)
                    app.Quit(ref missing, ref missing, ref missing);

                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("WINWORD"))
                {
                    p.Kill();
                }

                return false;
            }
            return true;
        }


        /// <summary>
        /// 设定打印机
        /// </summary>
        /// <returns></returns>
        public bool setPrint(string printName)
        {
            try
            {
                if (Externs.SetDefaultPrinter(printName)) //设置默认打印机
                {

                    return true;
                }
                else
                {
                    throw new Exception("设置默认打印机失败!请查看系统设置中默认打印机的名称设定是否正确");
                    // return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得打印机列表
        /// </summary>
        /// <returns></returns>
        public List<String> GetLocalPrinters()
        {
            return LocalPrinter.GetLocalPrinters();
        }

        /// <summary>
        /// 取得默认打印机的名称
        /// </summary>
        /// <returns></returns>
        public string DefaultPrinter()
        {
            return LocalPrinter.DefaultPrinter();
        }
    }

    /// <summary>
    /// 取得打印机列表
    /// </summary>
    class LocalPrinter
    {
        private static PrintDocument fPrintDocument = new PrintDocument();

        //获取本机默认打印机名称
        public static String DefaultPrinter()
        {
            return fPrintDocument.PrinterSettings.PrinterName;
        }
        public static List<String> GetLocalPrinters()
        {
            List<String> fPrinters = new List<String>();
            fPrinters.Add(DefaultPrinter()); //默认打印机始终出现在列表的第一项
            foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                }
            }
            return fPrinters;
        }
    }

    //设定默认打印机
    class Externs
    {
        //调用win api将指定名称的打印机设置为默认打印机
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name);
    }

    /// <summary>
    /// 替换图片
    /// </summary>
    class ReplaceAndInsertImage : IReplacingCallback
    {
        /// <summary>
        /// 需要插入的图片路径
        /// </summary>
        public string url { get; set; }

        public ReplaceAndInsertImage(string url)
        {
            this.url = url;
        }

        public ReplaceAction Replacing(ReplacingArgs e)
        {
            //获取当前节点
            var node = e.MatchNode;
            //获取当前文档
            Aspose.Words.Document doc = node.Document as Aspose.Words.Document;
            DocumentBuilder builder = new DocumentBuilder(doc);
            //将光标移动到指定节点
            builder.MoveTo(node);
            //插入图片
            builder.InsertImage(url);
            return ReplaceAction.Replace;
        }

    }
}
