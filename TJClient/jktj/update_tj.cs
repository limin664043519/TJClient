using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Configuration;
using TJClient.Common;
using System.Text.RegularExpressions;
using System.Collections;
namespace FBYClient
{
    public partial class update_tj : Form
    {
        private const string applicationFile = "Setup";
        public DataTable dtUpdate = null;

        /// <summary>
        /// 文件总的大小
        /// </summary>
        public float fileAllSize = 1;

        /// <summary>
        /// 下载文件的本地路径
        /// </summary>
        public string localPath = "";

        /// <summary>
        /// 信息
        /// </summary>
        public string ResultStr = "";

        /// <summary>
        /// 执行的状态
        /// </summary>
        public string ResultStatus = "0";

        /// <summary>
        /// 下载的文件名称
        /// </summary>
        public string fileName = "";

        /// <summary>
        /// 导入表的名称
        /// </summary>
        public string tableNameTem = "";

        public update_tj()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public int dataCountAll = 0;

        /// <summary>
        /// 当前数据条数
        /// </summary>
        public int dataCountCrrent = 0;

        /// <summary>
        /// 基础数据文件列表
        /// </summary>
        public string[] fileJcsjList = null;

        /// <summary>
        /// 档案数据
        /// </summary>
        public string[] fileDaList = null;

        /// <summary>
        /// 体检信息
        /// </summary>
        public string[] fileTjxxList = null;

        /// <summary>
        /// 文件下载地址
        /// </summary>
        public string fileDownloadUrl = "";

        /// <summary>
        /// 保存下载后的本地文件地址
        /// </summary>
        public ArrayList localFileAddressList = new ArrayList();

        /// <summary>
        /// 进度条更新处理
        /// </summary>
        ValueEventArgsloading e_load = new ValueEventArgsloading();

        /// <summary>
        /// 显示的消息的类型区分 1:百分比  2:数据条数进度
        /// </summary>
        string msgQf = "1";

        /// <summary>
        /// 用户下载标识
        /// </summary>
        string rndPrefix = "";


        /// <summary>
        /// 文件是否下载完毕
        /// </summary>
        bool isFileDownloadAll = false;

        #region 网络状态检测


        #endregion

        /// <summary>
        ///检测网络状态
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        ///初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_Load(object sender, EventArgs e)
        {
            //下载数据
            update_File();
        }

        /// <summary>
        ///初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_File()
        {
            // 实例化业务对象
            this.ValueChanged += new ValueChangedEventHandlerloading(this.workder_ValueChanged);

            // 使用异步方式调用长时间的方法
            Action handler = new Action(this.updateFile);

            handler.BeginInvoke(
                new AsyncCallback(this.AsyncCallback),
                handler
                );
        }

        /// <summary>
        /// 数据下载结束异步操作
        /// </summary>
        /// <param name="ar"></param>
        public void AsyncCallback(IAsyncResult ar)
        {
            // 标准的处理步骤
            Action handler = ar.AsyncState as Action;
            handler.EndInvoke(ar);

            System.Windows.Forms.MethodInvoker invoker = () =>
            {

                if (ResultStatus.Equals("0"))
                {
                    //progressBar1.Value = 0;
                    ////下载完成后调用数据导入处理
                    //dataImport();

                    MessageBox.Show("下载导入完成！");
                }
                else if (ResultStatus.Equals("4"))
                {
                    MessageBox.Show(ResultStr);
                }
                else
                {
                    MessageBox.Show(ResultStr);
                }

                this.ResultStatus = "7";
            };

            if (this.InvokeRequired)
            {
                this.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }



        /// <summary>
        /// 下载文件
        /// </summary>
        private void updateFile()
        {

            //将信息设定到页面中
            SetControlTextStr(lblMsg, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), "INFO");
            SetControlTextStr(lblMsg, "获取数据下载地址信息......", "INFO");
            //数据文件生成
            SetControlTextStr(label_title, "获取数据下载地址信息......", "INFO");
            rndPrefix = GetRndPrefix();

            //启动文件生成处理
            createFile(rndPrefix, DataDownLoad_TJ_Para.fileType);

            //等待4秒后获取文件地址
            Thread.Sleep(4000);

            while (isFileDownloadAll == false)
            {
                //基础数据
                fileJcsjList = null;
                //档案数据
                fileDaList = null;
                //上次体检信息数据
                fileTjxxList = null;
                //清空下载文件的列表
                localFileAddressList = new ArrayList();
                //获取生成的文件    
                bool result_url = GetFilePathList(DataDownLoad_TJ_Para.fileType, rndPrefix);

                if ((fileJcsjList != null && fileJcsjList.Length > 0) || (fileDaList != null && fileDaList.Length > 0) || (fileTjxxList != null && fileTjxxList.Length > 0))
                {
                    SetControlTextStr(lblMsg, "数据下载地址信息获取完成", "INFO");

                    //数据文件下载
                    SetControlTextStr(lblMsg, "开始下载数据文件......", "INFO");
                    SetControlTextStr(label_title, "下载数据文件......", "INFO");

                    bool result_download = fileDownLoad();

                    SetControlTextStr(lblMsg, "数据文件下载完成", "INFO");

                    //数据文件导入
                    SetControlTextStr(lblMsg, "开始导入数据文件......", "INFO");
                    SetControlTextStr(label_title, "导入数据文件......", "INFO");
                    bool result_import = importFiles();
                    SetControlTextStr(lblMsg, "数据文件导入完成", "INFO");

                    //文件没有获取完
                    if (isFileDownloadAll == false)
                    {
                        SetControlTextStr(lblMsg, ".....开始获取剩余文件.......", "INFO");
                    }
                }
                else
                {
                    SetControlTextStr(lblMsg, ".....文件正在生成中10秒后再获取.......", "INFO");
                    // 进度条控制
                    System.Threading.Thread.Sleep(10000);
                }

                
            }
            SetControlTextStr(lblMsg, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), "INFO");
            SetControlTextStr(label_title, "处理结束", "INFO");
            e_load.Value = 2;
            this.OnValueChanged(e_load);
        }

        #region  获取下载地址

        ///
        private string GetRndPrefix()
        {
            string bytes = "0123456789";
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            int range = bytes.Length;
            for (int i = 0; i < 5; i++)
            {
                sb.Append(bytes.Substring(r.Next(range), 1));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取下载文件的地址
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public bool GetFilePathList(string fileType, string rndPrefix_P)
        {
            try
            {
                //文件类型  1：基础数据  2：档案数据 3:体检信息
                string[] fileTypeList = fileType.Split(new char[] { ',' });
                bool result_bool = true;
                // 进度条控制
                this.msgQf = "2";
                dataCountAll = fileTypeList.Length;
                dataCountCrrent = 0;
                e_load.Value = 1;
                e_load.text = ResultStr;
                this.OnValueChanged(e_load);

                for (int i = 0; i < fileTypeList.Length; i++)
                {
                    dataCountCrrent = i + 1;
                    if (fileTypeList[i].Trim().Length > 0)
                    {
                        //基础数据
                        if (fileTypeList[i].Trim().Equals("1"))
                        {
                            //设定提示信息到页面上
                            SetControlTextStr(lblMsg, "    基础数据下载信息获取......", "INFO");

                            getDownLoadFileList(rndPrefix_P, fileTypeList[i]);

                            //设定提示信息到页面上
                            SetControlTextStr(lblMsg, "    基础数据下载信息获取完成", "INFO");
                        }
                        else if (fileTypeList[i].Trim().Equals("2"))//档案数据
                        {

                            //设定提示信息到页面上
                            SetControlTextStr(lblMsg, "    档案数据下载信息获取......", "INFO");

                            getDownLoadFileList(rndPrefix_P, fileTypeList[i]);

                            //设定提示信息到页面上
                            SetControlTextStr(lblMsg, "    档案数据下载信息获取完成", "INFO");
                        }
                        else if (fileTypeList[i].Trim().Equals("3"))//上次体检信息
                        {

                            //设定提示信息到页面上
                            SetControlTextStr(lblMsg, "    上次体检信息下载信息获取......", "INFO");

                            getDownLoadFileList(rndPrefix_P, fileTypeList[i]);

                            //设定提示信息到页面上
                            SetControlTextStr(lblMsg, "    上次体检信息下载信息获取完成", "INFO");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                SetControlTextStr(lblMsg, "获取下载地址错误！" + ex.Message, "Error");
                //throw new Exception("获取下载地址错误！" + ex.Message);
            }
            return true;
        }

        /// <summary>
        /// 启动生成下载的数据文件处理
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public bool createFile(string rndPrefix, string fileType)
        {
            try
            {
                SetControlTextStr(lblMsg, "createFile", "INFO");

                TJClient.JKTJ.ClientDoService webService = new TJClient.JKTJ.ClientDoService();
                webService.Url = System.Configuration.ConfigurationManager.AppSettings["GwtjUrl"];
                //webService.Timeout = 900000;
                if (ConfigurationManager.AppSettings["WaitTimeSec"] != null &&
                    int.Parse(ConfigurationManager.AppSettings["WaitTimeSec"]) > 0)
                {
                    webService.Timeout = int.Parse(ConfigurationManager.AppSettings["WaitTimeSec"]);
                }
                else
                {
                    webService.Timeout = 900000;
                }

                SetControlTextStr(lblMsg, webService.Url, "INFO");

                //获取下载文件的地址 
                string fileUrl = webService.downLoadInfoByParm(rndPrefix, DataDownLoad_TJ_Para.yljgbm, DataDownLoad_TJ_Para.czList, fileType);

                SetControlTextStr(lblMsg, fileUrl, "INFO");
                //下载返回结果处理
                string[] fileUrlList = fileUrl.Split(new char[] { '-' });

                if (fileUrlList.Length > 0 && fileUrlList[0].Equals("1"))
                {
                    //正常返回结果（文件的下载地址）
                    fileUrl = fileUrlList[1];
                    fileDownloadUrl = fileUrlList[1];

                }
                else
                {

                    SetControlTextStr(lblMsg, "createFile:" + fileUrl, "Error");
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetControlTextStr(lblMsg, "createFile:" + ex.Message, "Error");
                //throw new Exception(string.Format("获取文件地址错误！{0}", ex.Message));
                return false;
            }
            return true;
        }


        /// <summary>
        /// 生成下载的数据文件并获得下载的文件的下载地址
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public bool getDownLoadFileList(string rndPrefix, string fileType)
        {
            try
            {
                SetControlTextStr(lblMsg, "getDownLoadFileList", "INFO");

                TJClient.JKTJ.ClientDoService webService = new TJClient.JKTJ.ClientDoService();
                webService.Url = System.Configuration.ConfigurationManager.AppSettings["GwtjUrl"];
                //webService.Timeout = 900000;
                if (ConfigurationManager.AppSettings["WaitTimeSec"] != null &&
                    int.Parse(ConfigurationManager.AppSettings["WaitTimeSec"]) > 0)
                {
                    webService.Timeout = int.Parse(ConfigurationManager.AppSettings["WaitTimeSec"]);
                }
                else
                {
                    webService.Timeout = 900000;
                }

                SetControlTextStr(lblMsg, webService.Url, "INFO");

                //获取下载文件的地址 
                string fileUrl = webService.downLoadInfoByParm_upload(rndPrefix, DataDownLoad_TJ_Para.yljgbm, DataDownLoad_TJ_Para.czList, fileType);

                SetControlTextStr(lblMsg, fileUrl, "INFO");

                //下载返回结果处理   
                string[] fileUrlList = fileUrl.Split(new char[] { '-' });

                if (fileUrlList.Length > 0 && fileUrlList[0].Equals("1"))
                {
                    //处理文件的生成状态
                    if (fileUrlList.Length == 4)
                    {
                        if (fileUrlList[3].Equals("1"))
                        {
                            //文件全部生成完成，并获取到了下载地址
                            isFileDownloadAll = true;
                        }
                        else if (fileUrlList[3].Equals("2"))
                        {
                            //文件没有全部生成完成，获取到了部分文件的下载地址
                            isFileDownloadAll = false;
                        }
                        else
                        {
                            //停止获取文件 可能出现了异常
                            isFileDownloadAll = true;
                        }

                    }

                    //正常返回结果（文件的下载地址）
                    fileUrl = fileUrlList[1];

                    fileDownloadUrl = fileUrlList[1];

                    //基础数据
                    if (fileUrlList[2].Length > 0)
                    {
                        if (fileType.Equals("1"))
                        {
                            fileJcsjList = fileUrlList[2].Split(new char[] { '|' });
                        }
                        else if (fileType.Equals("2"))//档案数据
                        {
                            fileDaList = fileUrlList[2].Split(new char[] { '|' });
                        }
                        else if (fileType.Equals("3"))//上次体检信息
                        {
                            fileTjxxList = fileUrlList[2].Split(new char[] { '|' });
                        }
                    }

                }
                else
                {
                    //停止获取文件
                    isFileDownloadAll = true;
                }
            }
            catch (Exception ex)
            {
                //发生异常时，停止获取文件
                isFileDownloadAll = true;

                SetControlTextStr(lblMsg, "createFile:" + ex.Message, "Error");
                //throw new Exception(string.Format("获取文件地址错误！{0}", ex.Message));
                return false;
            }
            return true;
        }




        #endregion

        #region  下载数据文件

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        public bool fileDownLoad()
        {
            // 进度条控制
            this.msgQf = "1";
            dataCountAll = 100;
            e_load.Value = 3;
            this.OnValueChanged(e_load);

            //基础数据下载
            if (fileJcsjList != null && fileJcsjList.Length > 0)
            {
                for (int i = 0; i < fileJcsjList.Length; i++)
                {
                    //文件地址信息
                    string[] fileinfo = fileJcsjList[i].Split(new char[] { '$' });
                    if (fileinfo.Length >= 2)
                    {
                        //文件下载地址
                        string URLAddress = fileDownloadUrl + fileinfo[0];

                        //文件大小（K）
                        Int32 fileSize = Int16.Parse(fileinfo[1]);

                        SetControlTextStr(lblMsg, URLAddress, "INFO");
                        //下载
                        startDownload(URLAddress, fileSize);
                    }
                }
            }

            //档案数据下载
            if (fileDaList != null && fileDaList.Length > 0)
            {
                for (int i = 0; i < fileDaList.Length; i++)
                {
                    //文件地址信息
                    string[] fileinfo = fileDaList[i].Split(new char[] { '$' });
                    if (fileinfo.Length >= 2)
                    {
                        //文件下载地址
                        string URLAddress = fileDownloadUrl + fileinfo[0];

                        //文件大小（字节）
                        int fileSize = Int32.Parse(fileinfo[1]);

                        //下载
                        startDownload(URLAddress, fileSize);
                    }
                }
            }

            //上次体检结果下载
            if (fileTjxxList != null && fileTjxxList.Length > 0)
            {
                for (int i = 0; i < fileTjxxList.Length; i++)
                {
                    //文件地址信息
                    string[] fileinfo = fileTjxxList[i].Split(new char[] { '$' });
                    if (fileinfo.Length >= 2)
                    {
                        //文件下载地址
                        string URLAddress = fileDownloadUrl + fileinfo[0];

                        //文件大小（字节）
                        float fileSize = float.Parse(fileinfo[1]);

                        //下载
                        startDownload(URLAddress, fileSize);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        public void startDownload(string URLAddress, float fileSize)
        {
            string receiveFilePath = "";
            try
            {
                //本地保存的文件的地址
                string receivePath = ConfigurationSettings.AppSettings["filePath"].ToString();  //@"C:\";


                receiveFilePath = receivePath + System.IO.Path.GetFileName(URLAddress);

                localPath = receiveFilePath;

                //文件大小
                fileAllSize = fileSize;

                //下载文件
                SetControlTextStr(lblMsg, string.Format("    [{0}]文件下载中......", Path.GetFileName(receiveFilePath)), "INFO");
                downLoadFile(URLAddress, receivePath, receiveFilePath);
                SetControlTextStr(lblMsg, string.Format("    [{0}]文件下载完成", Path.GetFileName(receiveFilePath)), "INFO");

            }
            catch (Exception ex)
            {
                //返回错误状态和错误信息
                //ResultStatus = "5";
                //ResultStr = "下载失败！" + ex.Message;
                //SetControlTextStr(lblMsg, ResultStr);
            }
            //return receiveFilePath;
        }

        /// <summary>
        /// 下载数据文件
        /// </summary>
        /// <param name="URLAddress"></param>
        /// <returns></returns>
        public bool downLoadFile(string URLAddress, string receiveFilePath, string receiveFilePathAll)
        {
            try
            {
                if (!Directory.Exists(receiveFilePath)) Directory.CreateDirectory(receiveFilePath);

                //客户端对象
                WebClient client = new WebClient();

                //本地保存的文件的地址
                client.DownloadFile(URLAddress, receiveFilePathAll);

                //保存本地文件地址
                localFileAddressList.Add(receiveFilePathAll);

                return true;
            }
            catch (Exception ex)
            {
                //返回错误状态和错误信息
                ResultStatus = "5";
                ResultStr = "下载失败！" + ex.Message;
                SetControlTextStr(lblMsg, ResultStr, "Error");
                return false;
            }
        }

        #endregion

        #region 文件导入

        private void ZipFileOperation(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            if (!File.Exists(filePath))
            {
                return;
            }
            if (fileName.ToLower().Contains("t_jk_usersignname"))
            {
                //如果是默认签名压缩文件    
                TJClient.Signname.Operation.DefaultSignnamePicsUnzip(filePath);
            }
        }

        /// <summary>
        /// 将下载的文件导入到数据库中Acess数据库
        /// </summary>
        /// <returns></returns>
        public bool importFiles()
        {
            if (localFileAddressList != null && localFileAddressList.Count > 0)
            {
                for (int i = 0; i < localFileAddressList.Count; i++)
                {
                    //导入数据
                    SetControlTextStr(lblMsg, string.Format("    [{0}]文件导入中......", Path.GetFileName(localFileAddressList[i].ToString())), "INFO");
                    if (localFileAddressList[i].ToString().EndsWith("zip"))
                    {
                        //如果是zip文件，处理zip文件
                        ZipFileOperation(localFileAddressList[i].ToString());
                    }
                    else
                    {
                        importFileTo(localFileAddressList[i].ToString());
                    }

                    SetControlTextStr(lblMsg, string.Format("    [{0}]文件导入完成({1})", Path.GetFileName(localFileAddressList[i].ToString()), dataCountAll.ToString()), "INFO");
                }
            }
            return true;
        }

        /// <summary>
        /// 把指定的文件导入到数据库中
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool importFileTo(string filePath)
        {
            //excel转换的数据集合
            DataSet ds_result = new DataSet();
            string tablename = "";
            try
            {
                //这里没有用文件名
                ds_result = getDsFromExcel(filePath);
                //计算数据总条数
                dataCountAll = 0;
                if (ds_result != null && ds_result.Tables.Count > 0)
                {
                    //遍历数据库表,计算数据总条数
                    for (int tableIndex = 0; tableIndex < ds_result.Tables.Count; tableIndex++)
                    {
                        dataCountAll = dataCountAll + ds_result.Tables[tableIndex].Rows.Count;
                        int lastIndex = ds_result.Tables[tableIndex].TableName.LastIndexOf('-');
                        //string tableName_tem=ds_result.Tables[tableIndex].TableName.Substring(0,ds_result.Tables[tableIndex].TableName.Length -2);
                        string tableName_tem = ds_result.Tables[tableIndex].TableName.Substring(0, lastIndex);
                        if (!ds_result.Tables.Contains(tableName_tem))
                        {
                            ds_result.Tables[tableIndex].TableName = tableName_tem;
                            //ds_result.Tables.Remove(tableName_tem);
                            if (ds_result.Tables[tableIndex].TableName.ToLower().Equals(tableNameTem.ToLower()) == false || tableNameTem.Length == 0)
                            {
                                //删除要导入的数据，按ds中的数据进行删除
                                importFileDelete(ds_result.Tables[tableIndex]);

                                tableNameTem = ds_result.Tables[tableIndex].TableName;
                            }
                        }

                    }

                    // 进度条控制
                    this.msgQf = "2";
                    //dataCountAll = dataCountAll;
                    dataCountCrrent = 0;
                    e_load.Value = 3;
                    this.OnValueChanged(e_load);

                    int commitCount = 1000;

                    DataTable dt_result = ds_result.Tables[0].Clone();
                    //遍历数据库表
                    for (int tableIndex = 0; tableIndex < ds_result.Tables.Count; tableIndex++)
                    {

                        tablename = ds_result.Tables[tableIndex].TableName.Split(new char[] { '-' })[0];
                        dt_result.TableName = tablename;

                        for (int rowIndex = 0; rowIndex < ds_result.Tables[tableIndex].Rows.Count; rowIndex++)
                        {
                            dataCountCrrent++;
                            dt_result.ImportRow(ds_result.Tables[tableIndex].Rows[rowIndex]);
                            if (dataCountCrrent % commitCount == 0)
                            {
                                importFileToDbFromDt(dt_result);
                                dt_result.Clear();
                                dt_result.AcceptChanges();
                            }
                        }
                    }
                    if (dt_result.Rows.Count > 0)
                    {
                        importFileToDbFromDt(dt_result);
                    }
                }

            }
            catch (Exception ex)
            {
                //progressBar_xz.Visible = false;
                //MessageBox.Show(string.Format("表[{0}]{1}", tablename, ex.Message));
                return false;
            }
            return true;
        }


        /// <summary>
        /// 将datatable中的数据导入到数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool importFileDelete(DataTable dt)
        {
            ArrayList sqlList = new ArrayList();
            DBAccess access = new DBAccess();
            string tablename = "";

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {


                    tablename = dt.TableName;
                    if (tablename.ToLower().Equals("t_jk_tjry_txm"))
                    {
                        //删除数据
                        string strWhere = "";
                        if (DataDownLoad_TJ_Para.czList != null && DataDownLoad_TJ_Para.czList.Length > 0)
                        {
                            strWhere = "'" + DataDownLoad_TJ_Para.czList.Replace(",", "','") + "'";
                        }
                        //修改了此处，将村修改为p_rgid
                        string deleteSql = string.Format(@" delete from  t_jk_tjry_txm 
                                                where  exists(
                                                select * FROM    T_DA_JKDA_RKXZL  
                                                where t_jk_tjry_txm.jkdah=T_DA_JKDA_RKXZL.d_grdabh
                                                and T_DA_JKDA_RKXZL.P_RGID in({0})
                                                )", strWhere);

                        SetControlTextStr(lblMsg, deleteSql, "INFO");
                        access.ExecuteNonQueryBySql(deleteSql);

                    }
                    else
                    {
                        //删除数据
                        string strWhere = "";
                        if (DataDownLoad_TJ_Para.czList != null && DataDownLoad_TJ_Para.czList.Length > 0)
                        {
                            strWhere = "'" + DataDownLoad_TJ_Para.czList.Replace(",", "','") + "'";
                        }
                        sqlList = Common.FormatSql(dt, Common.SQLTYPE.delete.ToString(), strWhere);

                        if (sqlList != null)
                        {
                            SetControlTextStr(lblMsg, string.Format("    [{0}]", sqlList[0]), "INFO");
                            access.ExecuteNonQueryBySqlList(sqlList);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                SetControlTextStr(lblMsg, ex.Message, "Error");
                //MessageBox.Show(string.Format("表[{0}]{1}", tablename, ex.Message));
                return false;
            }
            return true;
        }


        /// <summary>
        /// 将datatable中的数据导入到数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool importFileToDbFromDt(DataTable dt)
        {

            ArrayList sqlList = new ArrayList();
            DBAccess access = new DBAccess();
            string tablename = "";

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    tablename = dt.TableName;
                    //新增
                    sqlList = Common.FormatSql(dt, Common.SQLTYPE.insert.ToString(), DataDownLoad_TJ_Para.czList);
                    if (sqlList != null)
                    {
                        access.ExecuteNonQueryBySqlList(sqlList);
                    }
                }

            }
            catch (Exception ex)
            {
                ResultStr = ex.Message;
                SetControlTextStr(lblMsg, ResultStr, "Error");
                //MessageBox.Show(string.Format("表[{0}]{1}", tablename, ex.Message));
                return false;
            }
            return true;
        }

        /// <summary>
        /// excel转化为dataset
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private DataSet getDsFromExcel(string filePath)
        {

            DataSet ds = new DataSet();
            string errMessage = "";

            bool boolResult = commonExcel.ExcelFileToDataSet(filePath, out ds, out errMessage);
            if (boolResult == false)
            {
                return null;
            }
            return ds;
        }
        #endregion

        #region

        #region 获取文件大小
        /// <summary>
        /// 获取文件的大小
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public float getFileSize(string dirPath)
        {
            float fileSize = GetDirectoryLength(dirPath);
            string sizeK = (fileSize / 1024).ToString("f2");
            return float.Parse(sizeK);
        }

        /// <summary>
        /// 获取文件的大小
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public long GetDirectoryLength(string dirPath)
        {
            //判断给定的文件是否存在,如果不存在则退出
            if (!File.Exists(dirPath))
                return 0;
            long len = 0;

            //定义一个FileInfo对象,取得文件大小
            FileInfo fileinfo = new FileInfo(dirPath);
            return fileinfo.Length;
        }

        #endregion

        #region 进度条处理

        /// <summary>
        /// 更新进度用线程（档案下载）
        /// </summary>
        public Thread t;

        /// <summary>
        /// 进度发生变化之后的回调方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void workder_ValueChanged(object sender, ValueEventArgsloading e)
        {
            try
            {
                System.Windows.Forms.MethodInvoker invoker = () =>
                    {
                        // 打开进度条
                        if (e.Value == 1)//开始
                        {
                            setTime(progressBar1, 0, dataCountAll);
                        }
                        else if (e.Value == 2) //结束
                        {
                            //progressBar1.Value = 100;
                            //将信息设定到页面中
                            //SetControlTextStr(label_jd, "100");
                            t.Abort();
                        }
                        else if (e.Value == 3) //初始化
                        {
                            t.Abort();
                            setTime(progressBar1, 0, dataCountAll);
                        }
                    };

                if (this.InvokeRequired)
                {
                    this.Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
        }



        /// <summary>
        /// 启用线程更新当前下载的进度
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool setTime(Control control, int text, int maxValue)
        {
            ProgressBar ProgressBar_tem = (ProgressBar)control;
            ProgressBar_tem.Value = 0;
            ProgressBar_tem.Visible = true;
            ProgressBar_tem.Maximum = maxValue;

            t = new Thread(new ParameterizedThreadStart(setText));
            t.SetApartmentState(ApartmentState.STA);
            t.Start(new object[] { ProgressBar_tem, text });
            return true;
        }

        /// <summary>
        /// 设定进度(下载)
        /// </summary>
        /// <param name="control"></param>
        private void setText(object control)
        {
            object[] parms = (object[])control;
            ProgressBar tem = (ProgressBar)parms[0];
            int text = int.Parse(parms[1].ToString());

            //设定要显示的内容
            //显示的消息的类型区分 1:百分比  2:数据条数进度
            if (msgQf.Equals("1"))
            {
                SetControlText(tem, text, text.ToString() + "%");

                System.Threading.Thread.Sleep(1000);
                int lenght = 0;
                if (fileAllSize > 0)
                {
                    lenght = Convert.ToInt32((getFileSize(localPath) * 100.00) / fileAllSize);
                }
                else
                {
                    lenght = 100;
                }
                parms[1] = lenght;
                setText(parms);

            }
            else if (msgQf.Equals("2")) //数据条数进度
            {
                SetControlText(tem, dataCountCrrent, string.Format("{0}/{1}", dataCountCrrent.ToString(), dataCountAll.ToString()));

                System.Threading.Thread.Sleep(1000);


                parms[1] = dataCountCrrent;
                setText(parms);
            }

        }
        #region  设定页面显示的内容
        /// <summary>
        /// 设定文本时的委托
        /// </summary>
        /// <param name="control"></param>
        delegate void SetTextCallback(Control control, int text, string str);
        private void SetControlText(Control control, int text, string str)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetControlText);
                    this.Invoke(d, new object[] { control, text, str });
                }
                else
                {
                    ProgressBar tem = (ProgressBar)control;
                    if (tem.Maximum == 0)
                    {
                        tem.Maximum = dataCountAll;
                    }
                    if (tem.Maximum >= text)
                    {
                        tem.Value = text;
                    }
                    //将信息设定到页面中
                    SetControlTextStr(label_jd, str, "INFO");
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
        }

        /// <summary>
        /// 设定文本时的委托
        /// </summary>
        /// <param name="control"></param>
        delegate void SetTextCallbackStr(Control control, string text, string msgLevel);
        private void SetControlTextStr(Control control, string text, string msgLevel)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    SetTextCallbackStr d = new SetTextCallbackStr(SetControlTextStr);
                    this.Invoke(d, new object[] { control, text, msgLevel });
                }
                else
                {
                    if (control.Name.ToString().ToLower().Equals("lblmsg"))
                    {
                        //if (text.Length > 0)
                        //{
                        //    control.Text = text + "\r\n" + control.Text;
                        //}
                        if (text.Length > 0)
                        {
                            RichTextBox richTextBox = (RichTextBox)control;
                            int p1 = richTextBox.TextLength;
                            richTextBox.AppendText(text + "\r\n");
                            int p2 = text.Length;
                            if (msgLevel.Equals("Error"))
                            {
                                richTextBox.Select(p1, p2);
                                richTextBox.SelectionColor = Color.Red;
                            }
                        }
                    }
                    else
                    {
                        control.Text = text;
                    }
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
        }

        #endregion


        #endregion

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void update_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ResultStatus.Equals("7") == false)
            {
                DialogResult result = MessageBox.Show("是否要停止数据的下载导入处理？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    e.Cancel = false;  //点击OK
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        // 定义一个事件来提示界面工作的进度
        public event ValueChangedEventHandlerloading ValueChanged;

        // 触发事件的方法
        public void OnValueChanged(ValueEventArgsloading e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        #endregion
    }
}
