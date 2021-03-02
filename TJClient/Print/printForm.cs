using AxSHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TJClient.Common;
//using SHDocVw;

namespace YCYL.Client.AllForms
{
    public partial class printForm : Form
    {
        public printForm()
        {
            InitializeComponent();
        }
   
        private Object oDocument;

        /// <summary>
        /// 文件地址
        /// </summary>
        public string strFileName = "";

        /// <summary>
        /// 打印的数据
        /// </summary>
        public DataTable dt_print = null;

        /// <summary>
        /// 文档模板
        /// </summary>
        public string printDoc="";

        /// <summary>
        ///打印机名称
        /// </summary>
        public string printName = "";

        /// <summary>
        /// 打印的文档地址
        /// </summary>
        public string printDocPath = "./printDocument/";//newdocument

        /// <summary>
        /// 打印的文档地址
        /// </summary>
        public string printDocPath_save = "./printDocument/newdocument/";//newdocument

        /// <summary>
        /// 打印配置文档
        /// </summary>
        public string printSetdoc = "UserConfig.xml";


        private PrintHelper printDemo = null;

        /// <summary>
        /// 页面加载处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printForm_Load(object sender, EventArgs e)
        {
            //生成打印对象
            printDemo = new PrintHelper();

            //生成要打印的word文档
            printDemo.createdPrintDocument(dt_print, printDocPath, printDoc, printDocPath_save);

            if (printDemo.printFilePathList != null && printDemo.printFilePathList.Count > 0)
            {
                strFileName = System.Windows.Forms.Application.StartupPath + printDemo.printFilePathList[0].ToString().Substring(1);
                Object refmissing = System.Reflection.Missing.Value;
                oDocument = null;
                axWebBrowser1.Navigate(strFileName, ref refmissing, ref refmissing, ref refmissing, ref refmissing);
            }


        }

        /// <summary>
        /// 打印处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_print_Click(object sender, EventArgs e)
        {
            //备份打印文件
            copyDirectory(Path.GetDirectoryName(strFileName), 
                          Path.GetFileName(strFileName), 
                          Path.GetDirectoryName(strFileName) + "backup"+"//"+DateTime .Now .ToString ("yyyyMM")+"//",
                          UserInfo.Yybm+DateTime .Now .ToString ("yyyy-MMddHHmmssfff")+"_"+ Path.GetFileName(strFileName));

            //指定打印机打印
            XmlRW xml = new XmlRW();
            printDemo.printList(xml.GetValueFormXML(printSetdoc, printName));

            //设定回默认的打印机
            printDemo.setPrint(printDemo.defaultPrintName);
        }

        /// <summary>
        /// 还要加一个该控件的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axWebBrowser1_NavigateComplete2(object sender, DWebBrowserEvents2_NavigateComplete2Event e)
        {
            Object o = e.pDisp;
            oDocument = o.GetType().InvokeMember("Document", BindingFlags.GetProperty, null, o, null);
            Object oApplication = o.GetType().InvokeMember("Application", BindingFlags.GetProperty, null, oDocument, null);
            Object oName = o.GetType().InvokeMember("Name", BindingFlags.GetProperty, null, oApplication, null);
        }

        /// <summary>
        /// 关闭打印窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                printDemo.existsFile(printDocPath_save);
            }
            catch (Exception ex)
            {
                //printDemo.existsFile(strFileName);
                //Log.Error("打印文件删除失败！"+ex.Message);
            }
        }

        /// <summary>
        /// 复制文件夹中的所有文件到指定文件夹
        /// </summary>
        /// <param name="DirectoryPath">源文件夹路径</param>
        /// <param name="DirAddress">保存路径</param>
        private void copyDirectory(string DirectoryPath,string oldfileName, string DirAddress,string newFileName)//复制文件夹，
        {
            if (!Directory.Exists(DirAddress)) Directory.CreateDirectory(DirAddress);

            DirectoryInfo DirectoryArray = new DirectoryInfo(DirectoryPath);

            FileInfo[] Files = DirectoryArray.GetFiles(oldfileName);//获取该文件夹下的文件列表

            try
            {
                foreach (FileInfo theFile in Files)//逐个复制文件     
                {
                    //如果临时文件夹下存在与应用程序所在目录下的文件同名的文件，则删除应用程序目录下的文件   
                    if (File.Exists(DirAddress + newFileName))
                    {
                        try
                        {
                            File.Delete(DirAddress + "\\" + Path.GetFileName(theFile.FullName));
                            //将临时文件夹的文件移到应用程序所在的目录下   
                            File.Copy(theFile.FullName, DirAddress + "\\" + newFileName);
                        }
                        catch (Exception ex)
                        {
                            //SetControlTextStr(lblMsg, string.Format("文件[{0}]复制失败！", Path.GetFileName(theFile.FullName)));
                        }
                    }
                    else
                    {
                        //将临时文件夹的文件移到应用程序所在的目录下   
                        File.Copy(theFile.FullName, DirAddress  + newFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
            }
        }
    }
}
