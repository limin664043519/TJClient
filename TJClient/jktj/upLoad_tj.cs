using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Logger;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using TJClient.Common;
using TJClient.jktj;
using TJClient.JKTJ;

namespace FBYClient
{
    public partial class upLoad_tj : Form
    {
        /// <summary>
        ///     进度条更新处理
        /// </summary>
        private readonly ValueEventArgsloading e_load = new ValueEventArgsloading();

        /// <summary>
        ///     执行的状态
        /// </summary>
        public string ResultStatus = "0";

        /// <summary>
        ///     信息
        /// </summary>
        public string ResultStr = "";

        /// <summary>
        ///     总条数
        /// </summary>
        public Int32 dataCountAll = 0;

        /// <summary>
        ///     当前数据条数
        /// </summary>
        public Int32 dataCountCrrent = 0;

        /// <summary>
        ///     需要上传的数据条数
        /// </summary>
        public Int32 doDataCount = 0;

        /// <summary>
        ///     前页面的参数
        /// </summary>
        public DataTable dt_Para = null;

        /// <summary>
        ///     文件总的大小
        /// </summary>
        public float fileAllSize = 1;

        /// <summary>
        ///     保存生成后的本地文件地址
        /// </summary>
        public ArrayList localFileAddressList = new ArrayList();

        /// <summary>
        ///     下载文件的本地路径
        /// </summary>
        public string localPath = "";

        /// <summary>
        ///     日志输出对象
        /// </summary>
        public SimpleLogger logger = SimpleLogger.GetInstance();

        /// <summary>
        ///     显示的消息的类型区分 1:百分比  2:数据条数进度
        /// </summary>
        private string msgQf = "1";

        /// <summary>
        ///     每次处理的数据条数
        /// </summary>
        public Int32 pagecount = 100;

        /// <summary>
        ///     前页面的参数(健康档案号)
        /// </summary>
        public string sqlWhere_jkdah = "";

        /// <summary>
        ///     前页面的参数（身份证号）
        /// </summary>
        public string sqlWhere_sfzh = "";

        /// <summary>
        ///     前页面的参数（体检编号）
        /// </summary>
        public string sqlWhere_tjbh = "";

        /// <summary>
        ///     保存心电图的地址
        /// </summary>
        public ArrayList xdtImgList = new ArrayList();

        #region 检测网络状态

        /// <summary>
        ///     检测网络状态
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        #endregion

        #region  初始化操作

        /// <summary>
        ///     构造函数
        /// </summary>
        public upLoad_tj()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_Load(object sender, EventArgs e)
        {
            if (dt_Para == null || dt_Para.Rows.Count == 0)
            {
                MessageBox.Show("请选择要处理的数据！");
                return;
            }

            //upLoadFile();

            //下载数据
            upLoad_File();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upLoad_File()
        {
            // 实例化业务对象
            ValueChanged += workder_ValueChanged;

            // 使用异步方式调用长时间的方法
            Action handler = upLoadFile;

            handler.BeginInvoke(
                AsyncCallback,
                handler
                );
        }

        /// <summary>
        ///     数据下载结束异步操作
        /// </summary>
        /// <param name="ar"></param>
        public void AsyncCallback(IAsyncResult ar)
        {
            // 标准的处理步骤
            var handler = ar.AsyncState as Action;
            handler.EndInvoke(ar);

            MethodInvoker invoker = () =>
            {
                if (ResultStatus.Equals("0"))
                {
                    //progressBar1.Value = 0;
                    ////下载完成后调用数据导入处理
                    //dataImport();

                    MessageBox.Show("上传完成！");
                }
                else if (ResultStatus.Equals("4"))
                {
                    MessageBox.Show(ResultStr);
                }
                else
                {
                    if (ResultStr.Length > 0)
                    {
                        MessageBox.Show(ResultStr);
                    }
                    else
                    {
                        MessageBox.Show("上传结束！");
                    }
                }
            };

            if (InvokeRequired)
            {
                Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }



        /// <summary>
        /// 上传签名图片
        /// </summary>
        private void UploadFileByService_Signname()
        {
            if (TJClient.Signname.Common.ShowSignnameOperation())
            {
                SetControlTextStr(lblMsg, "健康体检签名图片上传......", "INFO");
                //数据文件生成
                SetControlTextStr(label_title, "健康体检签名图片上传", "INFO");
                UploadSignnamePicFiles();
                //将信息设定到页面中
                SetControlTextStr(lblMsg, "健康体检签名图片上传完成", "INFO");
            }
        }

        /// <summary>
        /// 准备数据
        /// </summary>
        /// <returns></returns>
        private bool CreateUploadFiles()
        {
            //创建上传的文件
            //将信息设定到页面中  INFO|Error
            SetControlTextStr(lblMsg, "上传数据准备......", "INFO");
            //数据文件生成
            SetControlTextStr(label_title, "上传数据准备", "INFO");
            createUploadFile(dt_Para);
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "上传数据准备完成", "INFO");
            return true;
        }
        /// <summary>
        /// 调用上传接口，上传服务
        /// </summary>
        /// <returns></returns>
        private bool UploadFilesByService()
        {
            //文件上传
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "数据上传......", "INFO");
            //数据文件生成
            SetControlTextStr(label_title, "数据上传", "INFO");
            upLoadFileByService();
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "数据上传完成", "INFO");
            return true;
        }

        /// <summary>
        /// 调用上传接口，上传心电图文件
        /// </summary>
        /// <returns></returns>
        private bool UploadXdtFilesByService()
        {
            //文件上传
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "心电图图片上传......", "INFO");
            //数据文件生成
            SetControlTextStr(label_title, "心电图图片上传", "INFO");
            upLoadFileByService_xdt();
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "心电图图片上传完成", "INFO");
            return true;
        }
        /// <summary>
        /// 启动服务端处理上传的问题件
        /// </summary>
        /// <returns></returns>
        private bool DoFileExe()
        {
            //启动服务端处理
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "上传数据处理......", "INFO");
            //数据文件生成
            SetControlTextStr(label_title, "上传数据处理", "INFO");
            doFileExe();
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "上传数据处理完成", "INFO");
            return true;
        }

        /// <summary>
        /// 跟新数据状态
        /// </summary>
        /// <returns></returns>
        private bool UpdateDataStatus()
        {
            //更新本地数据状态
            SetControlTextStr(lblMsg, "数据状态更新处理......", "INFO");
            //数据文件生成
            SetControlTextStr(label_title, "本地数据状态处理", "INFO");
            doUpdateDataStatus();
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "数据状态更新处理完成", "INFO");
            return true;
        }

        private bool UploadSuccess()
        {
            SetControlTextStr(label_title, "处理结束", "INFO");

            //处理正常结束
            ResultStatus = "7";
            return true;
        }

        /// <summary>
        ///     上传文件
        /// </summary>
        private void upLoadFile()
        {

            CreateUploadFiles();//创建上传文件
            UploadFilesByService();//调用上传接口，上传服务
            UploadXdtFilesByService();//上传心电图
            UploadFileByService_Signname();//健康体检签名上传
            DoFileExe(); //启动服务端处理上传的问题件
            UpdateDataStatus();//跟新数据状态
            UploadSuccess(); //上传成功更新状态
            
            // 进度条控制
            Thread.Sleep(2000);
            e_load.Value = 2;
            OnValueChanged(e_load);
        }

        #endregion

        #region 生成相应的上传用的文件

        /// <summary>
        ///     生成excel文件，将要上传的数据转换成文件上传
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool OutFileToDisk(DataTable dt_upload, string tableName)
        {
            try
            {
                
                if (dt_upload != null && dt_upload.Rows.Count > 0)
                {
                    string errMsg = "";

                    //本地文件保存的目录
                    string filePath = string.Format("{0}{1}\\{2}\\{3}.xls", ConfigurationManager.AppSettings["filePath"],
                        UserInfo.Yybm, UserInfo.userId, GetExcelFileName(tableName));

                    //保存文件的目录不存在时，创建目录
                    string filePathDirectory = Path.GetDirectoryName(filePath);
                    if (Directory.Exists(filePathDirectory) == false) Directory.CreateDirectory(filePathDirectory);

                    //调用共同处理将datatable转换为excel文件
                    bool result = commonExcel.OutFileToDisk(dt_upload, tableName, filePath, out errMsg);
                    if (result)
                    {
                        //将文件的路径保存起来
                        localFileAddressList.Add(filePath);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                ResultStr = string.Format("    [{0}]生成excel文件异常！[{1}]", tableName, ex.Message);
                SetControlTextStr(lblMsg, ResultStr, "Error");

                //throw new Exception(ResultStr);
            }
            return true;
        }

        /// <summary>
        ///     获取sql文的in条件
        /// </summary>
        /// <param name="dt_result">要组合的数据集合</param>
        /// <param name="start">起始行</param>
        /// <param name="end">结束行</param>
        /// <param name="colunmName">列名称</param>
        /// <returns></returns>
        public string getWhereIn(DataTable dt_result, Int32 start, Int32 end, string colunmName)
        {
            //将指定的参数组合成sql文中in体检使用的格式字符串

            //没有数据时直接退出
            if (dt_result == null || dt_result.Rows.Count == 0) return "";
            string sqlWhereIn = "";
            if (dt_result.Rows.Count > 0)
            {
                //按照参数组合条件
                for (int i = start; i < dt_result.Rows.Count && i < end; i++)
                {
                    sqlWhereIn = string.Format("{0}, '{1}'", sqlWhereIn, dt_result.Rows[i][colunmName]);
                }
            }
            return sqlWhereIn.Substring(1);
        }

        private string GetLocalIP()
        {
            string hostname = Dns.GetHostName();//得到本机名    
            //PHostEntry localhost = Dns.GetHostEntry(hostname);
            IPHostEntry localhost = Dns.GetHostByName(hostname);
            IPAddress localaddr = localhost.AddressList[0];
            return localaddr.ToString();  
        }
        private string GetCzy()
        {
            return UserInfo.userId;
        }
        /// <summary>
        /// 2017-08-14 mq添加，用于将ip,czy放入excel文件名中
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetExcelFileName(string tableName)
        {
            return string.Format("{0}__{1}__{2}", tableName, GetCzy(), GetLocalIP());
        }

        #region

        private bool ShowOperationRowsCount(DataTable dt,DataTable dt_result)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                SetControlTextStr(lblMsg,
                    string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count, dt.Rows.Count), "INFO");
            }
            else
            {
                SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count, "0"), "INFO");
                return true;
            }
            return false;
        }

        private DataTable GetUploadDatatable(DataTable dt_result, DataTable dt, string[] columNameList, string sqlWhere,string tableName)
        {
            DataTable dt_upload = null;
            for (int i = 0; i < dt_result.Rows.Count; i++)
            {
                //组合数据的筛选条件
                string sqlWhere_tem = sqlWhere;
                for (int paCount = 0; paCount < columNameList.Length; paCount++)
                {
                    sqlWhere_tem = sqlWhere_tem.Replace("{" + columNameList[paCount] + "}",
                        dt_result.Rows[i][columNameList[paCount]].ToString());
                }

                //进行数据筛选并处理，筛选出的数据
                DataRow[] dtRow = dt.Select(sqlWhere_tem);
                if (dt != null && dtRow.Length > 0)
                {
                    if (dt_upload == null)
                    {
                        dt_upload = dt.Clone();
                        dt_upload.TableName = tableName;
                    }
                    for (int j = 0; j < dtRow.Length; j++)
                    {
                        //需要上传的数据条数
                        doDataCount++;

                        if (tableName.ToLower().Equals("upload_t_jk_jktj"))
                        {
                            if (dtRow[j]["xdturl"] != null)
                            {
                                dtRow[j]["XDTURL"] = getUrl_xdt(dtRow[j]["xdturl"].ToString());
                            }

                            if (dtRow[j]["bcurl"] != null)
                            {
                                dtRow[j]["BCURL"] = getUrl_xdt(dtRow[j]["bcurl"].ToString());
                            }
                        }
                        if (tableName.ToLower().Equals("upload_t_jk_photo"))
                        {
                            if (dtRow[j]["photourl"] != null)
                            {
                                dtRow[j]["PHOTOURL"] = getUrl_xdt(dtRow[j]["photourl"].ToString());
                            }
                        }

                        dt_upload.ImportRow(dtRow[j]);
                    }
                }

                //当前处理数据条数
                dataCountCrrent = i + 1;
            }
            return dt_upload;
        }
        #endregion

        /// <summary>
        ///     查找要上传的数据的共同处理
        /// </summary>
        /// <param name="dt_result">页面中选中的参数集合</param>
        /// <param name="tableName">要处理的数据表</param>
        /// <param name="sql_select">处理用的sql</param>
        /// <param name="sqlWhere">筛选数据的条件</param>
        /// <param name="columNameList">参数列</param>
        /// <returns></returns>
        private bool upLoad_FileDoCommon(DataTable dt_result, string tableName, string sql_select, string sqlWhere,
            string[] columNameList,bool needSaveToDatatable=false)
        {
            try
            {
                //获取信息
                var acess = new DBAccess();
                

                //按照条件取得要出的数据的集合
                DataTable dt = acess.ExecuteQueryBySql(sql_select);

                //显示处理的行数，如果为空，则返回true，直接退出处理
                if (ShowOperationRowsCount(dt, dt_result))
                {
                    return true;
                }
                

                //页面上的进度条初始化
                //进度条的最大值
                dataCountAll = dt_result.Rows.Count;
                //进度条的当前值
                dataCountCrrent = 0;
                //启动进度条
                e_load.Value = 3;
                OnValueChanged(e_load);
                DataTable dt_upload = GetUploadDatatable(dt_result,dt,columNameList,sqlWhere,tableName);
                if (dt_upload == null || dt_upload.Rows.Count <= 0)
                {
                    return true;
                }
                //导出excel文件
                //因签名需要压缩，所以需要将dt_upload传出。加如下语句：
                if (needSaveToDatatable)
                {
                    TJClient.Signname.UploadOperation.SetUploadSignnameDataTable(dt_upload);
                }
                bool result = OutFileToDisk(dt_upload, tableName);
                return result;
            }
            catch (Exception ex)
            {
                ResultStr = string.Format("数据({0}）生成异常!{1}", tableName, ex.Message);
                SetControlTextStr(lblMsg, ResultStr, "Error");

                return false;
            }
        }

        /// <summary>
        ///     获取心电图的地址
        /// </summary>
        /// <param name="jkdahPara"></param>
        /// <returns></returns>
        public string getUrl_xdt(string jkdahPara)
        {
            //string sql_select = "select * from t_jk_tjry_txm inner join  T_JK_lis_result_re on t_jk_tjry_txm.txmbh= T_JK_lis_result_re.testno where xmdh='xdturl'  and jkdah='{0}'";
            //获取信息
            //DBAccess acess = new DBAccess();
            string xdt_url = "";
            try
            {
                //按照条件取得要出的数据的集合
                // DataTable dt_url = acess.ExecuteQueryBySql(string.Format(sql_select, jkdahPara));
                //if (dt_url != null && dt_url.Rows.Count > 0)
                //{
                //    string xdtServerUrl = System.Configuration.ConfigurationManager.AppSettings["xdtServerUrl"].ToString();

                //    xdtImgList.Add(dt_url.Rows[0]["result"].ToString());
                //    xdt_url = xdtServerUrl + Path.GetFileName(dt_url.Rows[0]["result"].ToString());
                //}

                if (jkdahPara.Length > 0)
                {
                    string xdtServerUrl = ConfigurationManager.AppSettings["xdtServerUrl"];
                    xdtImgList.Add(jkdahPara);
                    xdt_url = xdtServerUrl + DateTime.Now.ToString("yyyy-MM-dd") + "/" + Path.GetFileName(jkdahPara);
                }
            }
            catch (Exception ex)
            {
                return xdt_url;
            }
            return xdt_url;
        }

        /// <summary>
        ///     健康档案人口学资料（T_DA_JKDA_RKXZL）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_DA_JKDA_RKXZL(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_DA_JKDA_RKXZL",
                " select * from T_DA_JKDA_RKXZL where (ISNEWDOC='1' or zt='2'or zt='3')", " D_GRDABH = '{jkdah}' ",
                new[] {"jkdah"});
        }


        /// <summary>
        ///     健康档案健康状况家族病史表（T_DA_JKDA_JKZK_JZBS）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_DA_JKDA_JKZK_JZBS(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_DA_JKDA_JKZK_JZBS",
                " select * from T_DA_JKDA_JKZK_JZBS where 1=1 ", " D_GRDABH = '{jkdah}' ", new[] {"jkdah"});
        }

        /// <summary>
        ///     健康档案健康状况既往病史表 T_DA_JKDA_JKZK_JWBS
        /// </summary>
        /// <returns></returns>
        private bool upload_T_DA_JKDA_JKZK_JWBS(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "UPLOAD_T_DA_JKDA_JKZK_JWBS",
                " select * from T_DA_JKDA_JKZK_JWBS where 1=1  ", "  D_GRDABH='{jkdah}' ", new[] {"jkdah"});
        }


        /// <summary>
        ///     健康档案健康状况表（T_DA_JKDA_JKZK）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_DA_JKDA_JKZK(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_DA_JKDA_JKZK", " select * from T_DA_JKDA_JKZK where 1=1 ",
                " D_GRDABH = '{jkdah}' ", new[] {"jkdah"});
        }


        /// <summary>
        ///     体检人员信息表(T_JK_TJRYXX）
        /// </summary>
        /// <param name="yljgbm"></param>
        /// <param name="tjpch"></param>
        /// <param name="sfh"></param>
        /// <returns></returns>
        private bool upLoad_T_JK_TJRYXX(DataTable dt_result)
        {
            string sql_select = string.Format(" select * from T_JK_TJRYXX where  FL='2' and ZLBZ='1' and YLJGBM='{0}'",
                UserInfo.Yybm);
            return upLoad_FileDoCommon(dt_result, "upLoad_T_JK_TJRYXX", sql_select, " SFZH = '{SFZH}' ", new[] {"SFZH"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = string .Format (" select * from T_JK_TJRYXX where  FL='2' and ZLBZ='1' and YLJGBM='{0}'",UserInfo.Yybm);
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null;

            //    DataTable dt_tem = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_tem != null && dt_tem.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt_tem.Rows .Count ));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }

            //    int countPage = dt_result.Rows.Count % pagecount == 0 ? dt_result.Rows.Count / pagecount : (dt_result.Rows.Count / pagecount) + 1;

            //    //进度条初始化
            //    dataCountAll = countPage;
            //    dataCountCrrent = 0;
            //    e_load.Value = 3;
            //    this.OnValueChanged(e_load);

            //    for (int i = 0; i < countPage; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format("  SFZH in ({0})", getWhereIn(dt_result, i * pagecount, (i + 1) * pagecount, "SFZH"));
            //        DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "T_JK_TJRYXX";
            //            }
            //            for (int j = 0; j < dt.Rows.Count; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dt.Rows[j]);
            //            }
            //        }
            //        //当前处理数据条数
            //        dataCountCrrent = i + 1;
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "T_JK_TJRYXX");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("体检人员信息表(T_JK_TJRYXX）生成异常!" + ex.Message);
            //}

            #endregion
        }

        /// <summary>
        ///     健康体检信息表(T_JK_JKTJ）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_JK_JKTJ(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_JK_JKTJ", " select * from T_JK_JKTJ where 1=1  ",
                "  D_GRDABH='{jkdah}' and HAPPENTIME='{TJSJ_jktj}' ", new[] {"jkdah", "TJSJ_jktj"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = " select * from T_JK_JKTJ where 1=1  ";
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null;
            //    DataTable dt = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_rowCount != null && dt.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt.Rows .Count ));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }
            //    for (int i = 0; i < dt_result.Rows.Count; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format("  D_GRDABH='{0}' and HAPPENTIME='{1}' ", dt_result.Rows[i]["jkdah"].ToString(), dt_result.Rows[i]["TJSJ_jktj"].ToString());
            //        //DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        DataRow[] dtRow = dt.Select(sqlWhere);
            //        if (dtRow != null && dtRow.Length > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "t_jk_jktj_tmp_tmp_tmp";
            //            }
            //            for (int j = 0; j < dtRow.Length; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dtRow[j]);
            //            }
            //        }

            //        //当前处理数据条数
            //        dataCountCrrent = i + 1;
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "t_jk_jktj_tmp_tmp_tmp");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("健康体检信息表(T_JK_JKTJ）生成异常!" + ex.Message);
            //}

            #endregion
        }

        /// <summary>
        /// 登记签名上传
        /// </summary>
        /// <param name="dt_result"></param>
        /// <returns></returns>
        private bool upload_T_JK_JKTJSIGNNAME(DataTable dt_result)
        {
            //return upLoad_FileDoCommon(dt_result, "upLoad_T_JK_JKTJSIGNNAME", " select * from T_JK_JKTJSIGNNAME where 1=1  ",
            //    "  D_GRDABH='{jkdah}' and (TJSJ='{TJSJ_jktj}'  or TJSJ='{TJSJ_dj}') ", new[] { "jkdah", "TJSJ_jktj", "TJSJ_dj" }, true);

            return upLoad_FileDoCommon(dt_result, "upLoad_T_JK_JKTJSIGNNAME", " select * from T_JK_JKTJSIGNNAME where 1=1  ",
               "  D_GRDABH='{jkdah}' ", new[] { "jkdah" }, true);

        }


        /// <summary>
        ///     健康体检信息表，用药情况(T_TJ_YYQKB）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_TJ_YYQKB(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_TJ_YYQKB", " select * from T_TJ_YYQKB where 1=1  ",
                "  D_GRDABH='{jkdah}' and Y_HAPPENTIME='{TJSJ_jktj}' ", new[] {"jkdah", "TJSJ_jktj"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = " select * from T_TJ_YYQKB where 1=1  ";
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null;

            //    DataTable dt = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_rowCount != null && dt.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt.Rows.Count));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }

            //    for (int i = 0; i < dt_result.Rows.Count; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format("  D_GRDABH='{0}' and Y_HAPPENTIME='{1}' ", dt_result.Rows[i]["jkdah"].ToString(), dt_result.Rows[i]["TJSJ_jktj"].ToString());
            //        //DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        DataRow[] dtRow = dt.Select(sqlWhere);
            //        if (dt != null && dtRow.Length  > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "T_TJ_YYQKB";
            //            }
            //            for (int j = 0; j < dtRow.Length; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dtRow[j]);
            //            }
            //        }

            //        //当前处理数据条数
            //        dataCountCrrent = i + 1;
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "T_TJ_YYQKB");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("健康体检信息,用药情况(T_TJ_YYQKB）生成异常!" + ex.Message);
            //}

            #endregion
        }


        /// <summary>
        ///     健康体检信息表，住院治疗情况表(T_TJ_ZYZLQKB）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_TJ_ZYZLQKB(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_TJ_ZYZLQKB", " select * from T_TJ_ZYZLQKB where 1=1  ",
                "  D_GRDABH='{jkdah}' and Z_HAPPENTIME='{TJSJ_jktj}' ", new[] {"jkdah", "TJSJ_jktj"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = " select * from T_TJ_ZYZLQKB where 1=1  ";
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null; 
            //    DataTable dt = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_rowCount != null && dt.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt.Rows.Count));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }

            //    for (int i = 0; i < dt_result.Rows.Count; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format("  D_GRDABH='{0}' and Z_HAPPENTIME='{1}' ", dt_result.Rows[i]["jkdah"].ToString(), dt_result.Rows[i]["TJSJ_jktj"].ToString());
            //        //DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        DataRow[] dtRow = dt.Select(sqlWhere);
            //        if (dt != null && dtRow.Length > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "T_TJ_ZYZLQKB";
            //            }
            //            for (int j = 0; j < dtRow.Length ; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dtRow[j]);
            //            }
            //        }

            //        //当前处理数据条数
            //        dataCountCrrent = i + 1;
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "T_TJ_ZYZLQKB");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("健康体检信息,住院治疗情况表(T_TJ_ZYZLQKB）生成异常!" + ex.Message);
            //}

            #endregion
        }

        /// <summary>
        ///     健康体检信息表，非免疫(T_TJ_FMYGHYFB）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_TJ_FMYGHYFB(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_TJ_FMYGHYFB", " select * from T_TJ_FMYGHYFB where 1=1  ",
                "  D_GRDABH='{jkdah}' and F_HAPPENTIME='{TJSJ_jktj}' ", new[] {"jkdah", "TJSJ_jktj"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = " select * from T_TJ_FMYGHYFB where 1=1  ";
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null;

            //    DataTable dt = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_rowCount != null && dt.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt.Rows.Count));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }

            //    for (int i = 0; i < dt_result.Rows.Count; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format(" and D_GRDABH='{0}' and F_HAPPENTIME='{1}' ", dt_result.Rows[i]["jkdah"].ToString(), dt_result.Rows[i]["TJSJ_jktj"].ToString());
            //        DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "T_TJ_FMYGHYFB";
            //            }
            //            for (int j = 0; j < dt.Rows.Count; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dt.Rows[j]);
            //            }
            //        }

            //        //当前处理数据条数
            //        dataCountCrrent = i + 1;
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "T_TJ_FMYGHYFB");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("健康体检信息,非免疫(T_TJ_FMYGHYFB）生成异常!" + ex.Message);
            //}

            #endregion
        }

        /// <summary>
        ///     申请单最终报告结果表(lis_reqresult）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_lis_reqresult(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_lis_reqresult", " select * from lis_reqresult where 1=1  ",
                "  brdh='{jkdah}' ", new[] {"jkdah"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = " select * from lis_reqresult where 1=1 {0} "; //brdh ='{brdh}'
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null;

            //    DataTable dt = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_rowCount != null && dt.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt.Rows.Count));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }

            //    for (int i = 0; i < dt_result.Rows.Count; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format(" and brdh='{0}' ", dt_result.Rows[i]["TJBM"].ToString());
            //        DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "lis_reqresult";
            //            }
            //            for (int j = 0; j < dt.Rows.Count; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dt.Rows[j]);
            //            }

            //            //当前处理数据条数
            //            dataCountCrrent = i + 1;
            //        }
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "lis_reqresult");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("申请单最终报告结果表(lis_reqresult）生成异常!" + ex.Message);
            //}

            #endregion
        }

        /// <summary>
        ///     心电测量结果表(T_JK_xdResult）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_JK_xdResult(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_JK_xdResult",
                string.Format(" select * from T_JK_xdResult where 1=1 and YLJGBM='{0}' ", UserInfo.Yybm),
                string.Format("   nd='{0}' ", DateTime.Now.Year) + " and jkdabh ='{jkdah}' ", new[] {"jkdah"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = " select * from T_JK_xdResult where 1=1 {0} "; //brdh ='{brdh}'
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null;

            //    DataTable dt = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_rowCount != null && dt.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt.Rows.Count));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }

            //    for (int i = 0; i < dt_result.Rows.Count; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format(" and jkdabh ='{0}' and nd='{1}' and YLJGBM='{2}' ", dt_result.Rows[i]["jkdah"].ToString(), dt_result.Rows[i]["nd"].ToString(), UserInfo.Yybm);
            //        DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "T_JK_xdResult";
            //            }
            //            for (int j = 0; j < dt.Rows.Count; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dt.Rows[j]);
            //            }
            //        }

            //        //当前处理数据条数
            //        dataCountCrrent = i + 1;
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "T_JK_xdResult");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("心电测量结果表(T_JK_xdResult）生成异常!" + ex.Message);
            //}

            #endregion
        }

        /// <summary>
        ///     老年人中医药健康管理表（T_LNR_ZYYTZGL）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_LNR_ZYYTZGL(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_LNR_ZYYTZGL", " select * from T_LNR_ZYYTZGL where 1=1  ",
                "  D_GRDABH ='{jkdah}' and HAPPENTIME='{TJSJ_tzbs}' ", new[] {"jkdah", "TJSJ_tzbs"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = " select * from T_LNR_ZYYTZGL where 1=1 {0} "; //brdh ='{brdh}'
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null;

            //    DataTable dt = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_rowCount != null && dt.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt.Rows.Count));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }

            //    for (int i = 0; i < dt_result.Rows.Count; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format(" and D_GRDABH ='{0}' and HAPPENTIME='{1}' ", dt_result.Rows[i]["jkdah"].ToString(), dt_result.Rows[i]["TJSJ_tzbs"].ToString());
            //        DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "T_LNR_ZYYTZGL";
            //            }
            //            for (int j = 0; j < dt.Rows.Count; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dt.Rows[j]);
            //            }
            //        }

            //        //当前处理数据条数
            //        dataCountCrrent = i + 1;
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "T_LNR_ZYYTZGL");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("老年人中医药健康管理表（T_LNR_ZYYTZGL）生成异常!" + ex.Message);
            //}

            #endregion
        }

        /// <summary>
        ///     健康随访（自理评估）（T_JG_LNRSF）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_JG_LNRSF(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_JG_LNRSF", " select * from T_JG_LNRSF where 1=1  ",
                "  D_GRDABH ='{jkdah}' and HAPPENTIME='{TJSJ_zlpg}' ", new[] {"jkdah", "TJSJ_zlpg"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = " select * from T_JG_LNRSF where 1=1 {0} "; //brdh ='{brdh}'
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null;

            //    DataTable dt = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_rowCount != null && dt.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt.Rows.Count));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }

            //    for (int i = 0; i < dt_result.Rows.Count; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format(" and D_GRDABH ='{0}' and HAPPENTIME='{1}' ", dt_result.Rows[i]["jkdah"].ToString(), dt_result.Rows[i]["TJSJ_zlpg"].ToString());
            //        DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "T_JG_LNRSF";
            //            }
            //            for (int j = 0; j < dt.Rows.Count; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dt.Rows[j]);
            //            }
            //        }

            //        //当前处理数据条数
            //        dataCountCrrent = i + 1;
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "T_JG_LNRSF");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("健康随访（自理评估）（T_JG_LNRSF）生成异常!" + ex.Message);
            //}

            #endregion
        }

        /// <summary>
        ///     条形码信息上传
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_JK_TJRY_TXM(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_JK_TJRY_TXM",
                " select * from T_JK_TJRY_TXM where zlbz='1'  ", "  jkdah ='{jkdah}'", new[] {"jkdah"});
        }

        /// <summary>
        ///     检验申请主表（lis_reqmain）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_lis_reqmain(DataTable dt_result)
        {
            try
            {
                //获取体检人员信息的语句，仅获取客户端新增人员
                string sql_select = " select * from lis_reqmain where zlbz='1' and jzbz='0' "; //brdh ='{brdh}'
                var acess = new DBAccess();
                DataTable dt_upload = null;
                DataTable dt_upload_lis_reqdetail = null;

                DataTable dt = acess.ExecuteQueryBySql(sql_select);
                if (ShowOperationRowsCount(dt, dt_result))
                {
                    return true;
                }

                for (int i = 0; i < dt_result.Rows.Count; i++)
                {
                    //按照身份证号获取人员信息
                    string sqlWhere = string.Format("  jkdah ='{0}'  ", dt_result.Rows[i]["jkdah"]);
                    DataRow[] dtRow = dt.Select(sqlWhere);
                    //DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
                    if (dtRow != null && dtRow.Length > 0)
                    {
                        if (dt_upload == null)
                        {
                            dt_upload = dt.Clone();
                            dt_upload.TableName = "upLoad_lis_reqmain";
                        }
                        for (int j = 0; j < dtRow.Length; j++)
                        {
                            //需要上传的数据条数
                            doDataCount++;
                            dt_upload.ImportRow(dtRow[j]);

                            //lis_reqdetail s申请单明细
                            string sql_select_lis_reqdetail =
                                " select * from lis_reqdetail where zlbz='1' and sqh='{0}'";
                            DataTable dt_reqdetail =
                                acess.ExecuteQueryBySql(string.Format(sql_select_lis_reqdetail, dtRow[j]["sqh"]));
                            if (dt_reqdetail != null && dt_reqdetail.Rows.Count > 0)
                            {
                                if (dt_upload_lis_reqdetail == null)
                                {
                                    dt_upload_lis_reqdetail = dt_reqdetail.Clone();
                                    dt_upload_lis_reqdetail.TableName = "upLoad_lis_reqdetail";
                                }

                                for (int k = 0; k < dt_reqdetail.Rows.Count; k++)
                                {
                                    dt_upload_lis_reqdetail.ImportRow(dt_reqdetail.Rows[k]);
                                }
                            }
                        }
                    }

                    //当前处理数据条数
                    dataCountCrrent = i + 1;
                }
                //导出excel文件
                bool result = OutFileToDisk(dt_upload, "upLoad_lis_reqmain");
                bool result_detail = OutFileToDisk(dt_upload_lis_reqdetail, "upLoad_lis_reqdetail");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("检验申请主表（lis_reqmain）生成异常!" + ex.Message);
            }
        }

        /// <summary>
        ///     体检状态表(T_JK_TJZT）：记录体检的各个阶段的状态
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_JK_TJZT(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_JK_TJZT",
                string.Format(" select * from T_JK_TJZT where 1=1 and YLJGBM='{0}'  ", UserInfo.Yybm),
                "  JKDAH ='{jkdah}' and SFZH='{sfzh}'  ", new[] {"jkdah", "sfzh"});
            //return true;

            #region

            //try
            //{
            //    //获取体检人员信息的语句，仅获取客户端新增人员
            //    string sql_select = " select * from T_JK_TJZT where 1=1 {0} "; //brdh ='{brdh}'
            //    DBAccess acess = new DBAccess();
            //    DataTable dt_upload = null;

            //    DataTable dt = acess.ExecuteQueryBySql(sql_select);
            //    if (dt_rowCount != null && dt.Rows.Count > 0)
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), dt.Rows.Count));
            //    }
            //    else
            //    {
            //        SetControlTextStr(lblMsg, string.Format("    需要处理数据[{0} x {1}]条", dt_result.Rows.Count.ToString(), "0"));
            //        return true;
            //    }

            //    for (int i = 0; i < dt_result.Rows.Count; i++)
            //    {
            //        //按照身份证号获取人员信息
            //        string sqlWhere = string.Format(" and JKDAH ='{0}' and SFZH='{1}' and YLJGBM='{2}' ", dt_result.Rows[i]["jkdah"].ToString(), dt_result.Rows[i]["sfzh"].ToString(), dt_result.Rows[i]["yljgbm"].ToString());
            //        DataTable dt = acess.ExecuteQueryBySql(string.Format(sql_select, sqlWhere));
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            if (dt_upload == null)
            //            {
            //                dt_upload = dt.Clone();
            //                dt_upload.TableName = "T_JK_TJZT";
            //            }
            //            for (int j = 0; j < dt.Rows.Count; j++)
            //            {
            //                //需要上传的数据条数
            //                doDataCount++;
            //                dt_upload.ImportRow(dt.Rows[j]);
            //            }
            //        }

            //        //当前处理数据条数
            //        dataCountCrrent = i + 1;
            //    }
            //    //导出excel文件
            //    bool result = OutFileToDisk(dt_upload, "T_JK_TJZT");
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("体检状态表(T_JK_TJZT）生成异常!" + ex.Message);
            //}

            #endregion
        }

        /// <summary>
        ///     人员拍照信息（T_JK_PHOTO）
        /// </summary>
        /// <returns></returns>
        private bool upLoad_T_JK_PHOTO(DataTable dt_result)
        {
            return upLoad_FileDoCommon(dt_result, "upLoad_T_JK_PHOTO",
                string.Format(" select * from T_JK_PHOTO where 1=1 and YLJGBM='{0}'  ", UserInfo.Yybm),
                "  JKDAH ='{jkdah}' and SFZH='{sfzh}'  ", new[] {"jkdah", "sfzh"});
        }

        #endregion

        #region 生成文件

        private bool PrepareSignnameData(DataTable dt_para)
        {
            if (TJClient.Signname.Common.ShowSignnameOperation())
            {
                SetControlTextStr(lblMsg, "    健康体检签名信息准备中......", "INFO");
                //初始化，需要上传的数据条数的计算值
                doDataCount = 0;
                upload_T_JK_JKTJSIGNNAME(dt_para);
                SetControlTextStr(lblMsg, "    健康体检签名信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            }
            return true;

        }



        /// <summary>
        ///准备健康档案人口学资料
        /// </summary>
        /// <returns></returns>
        private bool PrepareHealthRecords(DataTable dt_para)
        {
            //健康档案人口学资料（T_DA_JKDA_RKXZL）
            SetControlTextStr(lblMsg, "    健康档案人口学资料信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_DA_JKDA_RKXZL(dt_para);
            SetControlTextStr(lblMsg, "    健康档案人口学资料信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备健康档案健康状况既往病史信息
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareDiseaseHistory(DataTable dt_para)
        {
            //慢病信息(T_JK_mbjb）   既往病史
            SetControlTextStr(lblMsg, "    健康档案健康状况既往病史信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upload_T_DA_JKDA_JKZK_JWBS(dt_para);
            SetControlTextStr(lblMsg, "    健康档案健康状况既往病史信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备健康档案健康状况家族病史信息
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareFamilyDisease(DataTable dt_para)
        {
            //健康档案健康状况家族病史表（T_DA_JKDA_JKZK_JZBS）
            SetControlTextStr(lblMsg, "    健康档案健康状况家族病史信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_DA_JKDA_JKZK_JZBS(dt_para);
            SetControlTextStr(lblMsg, "    健康档案健康状况家族病史信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }

        
        /// <summary>
        /// 准备健康档案健康状况
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareHealthStatusInHealthExamine(DataTable dt_para)
        {
            //健康档案健康状况表（T_DA_JKDA_JKZK）
            SetControlTextStr(lblMsg, "    健康档案健康状况信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_DA_JKDA_JKZK(dt_para);
            SetControlTextStr(lblMsg, "    健康档案健康状况信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备体检人员信息
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareHealthExaminePeopleInfos(DataTable dt_para)
        {
            //体检人员信息表(T_JK_TJRYXX）
            SetControlTextStr(lblMsg, "    体检人员信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_JK_TJRYXX(dt_para);
            SetControlTextStr(lblMsg, "    体检人员信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备健康体检信息
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareHealthExamine(DataTable dt_para)
        {
            //健康体检信息表(T_JK_JKTJ）
            SetControlTextStr(lblMsg, "    健康体检信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            xdtImgList = new ArrayList();
            doDataCount = 0;
            upLoad_T_JK_JKTJ(dt_para);
            SetControlTextStr(lblMsg, "    健康体检信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备用药情况信息
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareMedication(DataTable dt_para)
        {
            //健康体检信息表，用药情况(T_TJ_YYQKB）
            SetControlTextStr(lblMsg, "    用药情况信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_TJ_YYQKB(dt_para);
            SetControlTextStr(lblMsg, "    用药情况信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备住院治疗情况信息
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareHospitalCourse(DataTable dt_para)
        {
            //健康体检信息表，住院治疗情况表(T_TJ_ZYZLQKB）
            SetControlTextStr(lblMsg, "    住院治疗情况信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_TJ_ZYZLQKB(dt_para);
            SetControlTextStr(lblMsg, "    住院治疗情况信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备非免疫
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareNoImmune(DataTable dt_para)
        {
            //健康体检信息表，非免疫(T_TJ_FMYGHYFB）
            SetControlTextStr(lblMsg, "    非免疫信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_TJ_FMYGHYFB(dt_para);
            SetControlTextStr(lblMsg, "    非免疫信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备检验结果
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareReqResult(DataTable dt_para)
        {
            //申请单最终报告结果表(lis_reqresult）
            SetControlTextStr(lblMsg, "    检验结果信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_lis_reqresult(dt_para);
            SetControlTextStr(lblMsg, "    检验结果信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备条形码
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareBarCode(DataTable dt_para)
        {
            //条形码(T_JK_TJRY_TXM）
            SetControlTextStr(lblMsg, "   条形码信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_JK_TJRY_TXM(dt_para);
            SetControlTextStr(lblMsg, "    条形码信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备心电检查结果
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareElectrocardiographyResult(DataTable dt_para)
        {
            //心电测量结果表(T_JK_xdResult）
            SetControlTextStr(lblMsg, "    心电测量结果信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_JK_xdResult(dt_para);
            SetControlTextStr(lblMsg, "    心电测量结果信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备老年人中医药健康管理信息
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareZyytzgl(DataTable dt_para)
        {
            //老年人中医药健康管理表（T_LNR_ZYYTZGL）
            SetControlTextStr(lblMsg, "    老年人中医药健康管理信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_LNR_ZYYTZGL(dt_para);
            SetControlTextStr(lblMsg, "    老年人中医药健康管理信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备自理能力评估
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareZlnlpg(DataTable dt_para)
        {
            //健康随访（自理评估）（T_JG_LNRSF）
            SetControlTextStr(lblMsg, "    自理能力评估信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_JG_LNRSF(dt_para);
            SetControlTextStr(lblMsg, "    自理能力评估信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }

        /// <summary>
        /// 准备检验申请信息
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareLisReqMain(DataTable dt_para)
        {
            //检验申请主表（lis_reqmain）
            SetControlTextStr(lblMsg, "    检验申请信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_lis_reqmain(dt_para);
            SetControlTextStr(lblMsg, "    检验申请信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }
        /// <summary>
        /// 准备体检状态
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PrepareHealthExamineStatus(DataTable dt_para)
        {
            //体检状态表(T_JK_TJZT）
            SetControlTextStr(lblMsg, "    体检状态信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_JK_TJZT(dt_para);
            SetControlTextStr(lblMsg, "    体检状态信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }

        /// <summary>
        /// 准备人员拍照信息
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        private bool PreparePhoto(DataTable dt_para)
        {
            //人员拍照表(T_JK_PHOTO）
            SetControlTextStr(lblMsg, "    人员拍照信息准备中......", "INFO");

            //初始化，需要上传的数据条数的计算值
            doDataCount = 0;
            upLoad_T_JK_PHOTO(dt_para);
            SetControlTextStr(lblMsg, "    人员拍照信息准备完成" + string.Format("({0})", doDataCount), "INFO");
            return true;
        }

        /// <summary>
        ///     生成上传用的文件
        /// </summary>
        /// <param name="dt_para"></param>
        /// <returns></returns>
        public bool createUploadFile(DataTable dt_para)
        {
            // 进度条控制
            //进度条类型
            msgQf = "2";

            //进度条初始化
            //dataCountAll = dt_para.Rows .Count ;
            dataCountCrrent = 0;
            e_load.Value = 1;
            OnValueChanged(e_load);

            PrepareHealthRecords(dt_para); //准备健康档案人口学资料
            PrepareDiseaseHistory(dt_para);//准备既往病史
            PrepareFamilyDisease(dt_para);//准备家族病史
            PrepareHealthStatusInHealthExamine(dt_para);//准备健康档案健康状况
            PrepareHealthExaminePeopleInfos(dt_para);//准备健康体检人员信息
            PrepareHealthExamine(dt_para);//准备健康体检
            PrepareSignnameData(dt_para);//健康体检签名(T_JK_JKTJSIGNNAME),如果启动就处理，不启动就不处理。放入单独的方法中
            PrepareMedication(dt_para);//准备用药情况
            PrepareHospitalCourse(dt_para);//准备住院治疗情况
            PrepareNoImmune(dt_para);//准备非免疫
            PrepareReqResult(dt_para);//准备req result
            PrepareBarCode(dt_para);//准备条形码
            PrepareElectrocardiographyResult(dt_para);//准备心电检查结果
            PrepareZyytzgl(dt_para);//准备中医体质管理
            PrepareZlnlpg(dt_para);//准备自理能力评估
            PrepareLisReqMain(dt_para);//准备检验申请信息
            PrepareHealthExamineStatus(dt_para);//准备健康体检状态
            PreparePhoto(dt_para);//准备拍照图片
            return true;
        }

        #endregion

        #region 文件上传

        private bool UploadFileByService(string sourceFile,string serverfileName)
        {
            if (!File.Exists(sourceFile))
            {
                SetControlTextStr(lblMsg, string.Format("{0}文件不存在",sourceFile), "INFO");
            }
            var client = new ClientDoService();
            client.Url = ConfigurationManager.AppSettings["GwtjUrl"];
            SetControlTextStr(lblMsg, client.Url, "INFO");
            
            client.CreateFile(serverfileName);

            string md5 = GetMD5(sourceFile);

            var fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var size = (int)fs.Length;
            int bufferSize = 1024 * 512;
            var count = (int)Math.Ceiling(size / (double)bufferSize);
            for (int i = 0; i < count; i++)
            {
                int readSize = bufferSize;
                if (i == count - 1)
                    readSize = size - bufferSize * i;
                var buffer = new byte[readSize];
                fs.Read(buffer, 0, readSize);
                client.Append(serverfileName, buffer);
            }

            return client.Verify(serverfileName, md5);
        }

        public bool upLoadFileByService()
        {
            if (localFileAddressList != null && localFileAddressList.Count > 0)
            {
                try
                {
                    //进度条初始化
                    dataCountAll = localFileAddressList.Count;
                    dataCountCrrent = 0;
                    e_load.Value = 3;
                    OnValueChanged(e_load);
                    doDataCount = 0;
                    for (int fileIndex = 0; fileIndex < localFileAddressList.Count; fileIndex++)
                    {
                        //上传服务器后的文件名  一般不修改文件名称
                        string sourceFile = localFileAddressList[fileIndex].ToString();
                        string serverfileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                                                Path.GetFileName(sourceFile);
                        SetControlTextStr(lblMsg, sourceFile, "INFO");
                        bool isVerify=UploadFileByService(sourceFile,serverfileName);
                        if (isVerify)
                        {
                            SetControlTextStr(lblMsg, string.Format("    [{0}]上传成功！", Path.GetFileName(sourceFile)),
                                "INFO");
                            doDataCount++;
                        }
                        dataCountCrrent++;
                    }
                }
                catch (Exception ex)
                {
                    SetControlTextStr(lblMsg, "    上传错误" + ex.Message, "INFO");
                }
                SetControlTextStr(lblMsg, "    上传完成" + string.Format("({0})", doDataCount), "INFO");
            }
            return true;
        }

        #region 上传健康体检签名图片
        /// <summary>
        /// 上传健康体检签名图片
        /// </summary>
        /// <returns></returns>
        private bool UploadSignnamePicFiles()
        {
            //先压缩
            string zipSignnamePicsPath = GetZipSignnamePicsPath();
            if (string.IsNullOrEmpty(zipSignnamePicsPath))
            {
                return false;
            }
            //如果打包成功,上传压缩文件。
            UploadZipSignnameFile(zipSignnamePicsPath);
            return true;
        }

        private bool UploadZipSignnameFile(string filePath)
        {
            //var client = new ClientDoService();
            //client.Url = ConfigurationManager.AppSettings["GwtjUrl"];
            //SetControlTextStr(lblMsg, client.Url, "INFO");
            string serverfileName = Path.GetFileName(filePath);
            bool isVerify = UploadFileByService(filePath,serverfileName);
            if (isVerify)
            {
                SetControlTextStr(lblMsg, string.Format("    [{0}]上传成功！", Path.GetFileName(filePath)),
                    "INFO");
                doDataCount++;
            }
            return true;
        }
        private string GetZipSignnamePicsPath()
        {
            DataTable dt = TJClient.Signname.UploadOperation.GetUploadSignnameDataTable();
            if (dt == null || dt.Rows.Count <= 0)
            {
                return "";
            }
            List<string> tabletSignnamePics = TJClient.Signname.Operation.GetTabletSignnamePics(dt);
            if (tabletSignnamePics.Count <= 0)
            {
                return "";
            }
            string zipFilePath = Path.Combine(TJClient.Signname.Common.GetTabletSignnameDirectory(),
                TJClient.Signname.FilenameHelper.ZipFileName(UserInfo.userId));
            if (TJClient.Signname.CompressHelper.Zip(tabletSignnamePics, zipFilePath))
            {
                return zipFilePath;
            }
            return "";
        }
        #endregion
        

        /// <summary>
        ///     上传心电图图片
        /// </summary>
        /// <returns></returns>
        public bool upLoadFileByService_xdt()
        {
            if (xdtImgList != null && xdtImgList.Count > 0)
            {
                try
                {
                    //进度条初始化
                    dataCountAll = xdtImgList.Count;
                    dataCountCrrent = 0;
                    e_load.Value = 3;
                    OnValueChanged(e_load);
                    doDataCount = 0;

                    for (int fileIndex = 0; fileIndex < xdtImgList.Count; fileIndex++)
                    {
                        //上传服务器后的文件名  一般不修改文件名称
                        string sourceFile = xdtImgList[fileIndex].ToString();
                        SetControlTextStr(lblMsg, sourceFile, "INFO");
                        if (File.Exists(sourceFile) == false)
                        {
                            SetControlTextStr(lblMsg, "文件不存在:" + sourceFile, "Error");
                        }
                        else
                        {
                            string serverfileName = Path.GetFileName(sourceFile);
                            bool isVerify = UploadFileByService(sourceFile,serverfileName);
                            if (isVerify)
                            {
                                SetControlTextStr(lblMsg, string.Format("    [{0}]上传成功！", Path.GetFileName(sourceFile)),
                                    "INFO");
                                doDataCount++;
                            }
                        }
                        dataCountCrrent++;
                    }
                }
                catch (Exception ex)
                {
                    SetControlTextStr(lblMsg, "    上传错误" + ex.Message, "INFO");
                }

                SetControlTextStr(lblMsg, "    上传完成" + string.Format("({0})", doDataCount), "INFO");
            }
            return true;
        }

        #endregion

        #region  启动文件处理程序

        public bool doFileExe()
        {
            var client = new ClientDoService();
            client.Url = ConfigurationManager.AppSettings["GwtjUrl"];
            SetControlTextStr(lblMsg, client.Url, "INFO");
            try
            {
                string result = client.DoFileThread(UserInfo.userId, Guid.NewGuid().ToString(), UserInfo.Yybm);
                return true;
            }
            catch (Exception ex)
            {
                ResultStr = string.Format("    启动文件处理程序发生异常！[{0}]", ex.Message);
                SetControlTextStr(lblMsg, ResultStr, "Error");
                return false;
            }
        }

        #endregion

        #region 更新本地数据状态

        public bool doUpdateDataStatus()
        {
            //TJClient.JKTJ.ClientDoService client = new TJClient.JKTJ.ClientDoService();
            //string result = client.DoFileThread("zyb");

            return true;
        }

        #endregion

        #region 进度管理

        #region 获取文件大小

        /// <summary>
        ///     获取文件的大小
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public float getFileSize(string dirPath)
        {
            float fileSize = GetDirectoryLength(dirPath);
            string sizeK = (fileSize/1024).ToString("f2");
            return float.Parse(sizeK);
        }

        /// <summary>
        ///     获取文件的大小
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
            var fileinfo = new FileInfo(dirPath);
            return fileinfo.Length;
        }

        #endregion

        /// <summary>
        ///     获取md5加密
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetMD5(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            var p = new MD5CryptoServiceProvider();
            byte[] md5buffer = p.ComputeHash(fs);
            fs.Close();
            string md5Str = "";
            var strList = new List<string>();
            for (int i = 0; i < md5buffer.Length; i++)
            {
                md5Str += md5buffer[i].ToString("x2");
            }
            return md5Str;
        }

        /// <summary>
        ///     关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ResultStatus.Equals("7") == false)
            {
                DialogResult result = MessageBox.Show("是否要停止数据的下载导入处理？", "提示信息", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    e.Cancel = false; //点击OK
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
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }

        #region 进度条处理

        /// <summary>
        ///     更新进度用线程（档案下载）
        /// </summary>
        public Thread t;

        /// <summary>
        ///     进度发生变化之后的回调方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void workder_ValueChanged(object sender, ValueEventArgsloading e)
        {
            try
            {
                MethodInvoker invoker = () =>
                {
                    // 打开进度条
                    if (e.Value == 1) //开始
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

                if (InvokeRequired)
                {
                    Invoke(invoker);
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
        ///     启用线程更新当前下载的进度
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool setTime(Control control, int text, int maxValue)
        {
            var ProgressBar_tem = (ProgressBar) control;
            ProgressBar_tem.Value = 0;
            ProgressBar_tem.Visible = true;
            ProgressBar_tem.Maximum = maxValue;

            t = new Thread(setText);
            t.SetApartmentState(ApartmentState.STA);
            t.Start(new object[] {ProgressBar_tem, text});
            return true;
        }

        /// <summary>
        ///     设定进度(下载)
        /// </summary>
        /// <param name="control"></param>
        private void setText(object control)
        {
            var parms = (object[]) control;
            var tem = (ProgressBar) parms[0];
            int text = int.Parse(parms[1].ToString());

            //设定要显示的内容
            //显示的消息的类型区分 1:百分比  2:数据条数进度
            if (msgQf.Equals("1"))
            {
                SetControlText(tem, text, text + "%");

                Thread.Sleep(1000);
                int lenght = Convert.ToInt32((getFileSize(localPath)*100.00)/fileAllSize);

                parms[1] = lenght;
                setText(parms);
            }
            else if (msgQf.Equals("2")) //数据条数进度
            {
                SetControlText(tem, dataCountCrrent, string.Format("{0}/{1}", dataCountCrrent, dataCountAll));

                Thread.Sleep(1000);


                parms[1] = dataCountCrrent;
                setText(parms);
            }
        }

        #region  设定页面显示的内容

        private void SetControlText(Control control, int text, string str)
        {
            try
            {
                if (InvokeRequired)
                {
                    SetTextCallback d = SetControlText;
                    Invoke(d, new object[] {control, text, str});
                }
                else
                {
                    var tem = (ProgressBar) control;
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

        private void SetControlTextStr(Control control, string text, string msgLevel)
        {
            try
            {
                if (InvokeRequired)
                {
                    SetTextCallbackStr d = SetControlTextStr;
                    Invoke(d, new object[] {control, text, msgLevel});
                }
                else
                {
                    if (control.Name.ToLower().Equals("lblmsg"))
                    {
                        if (text.Length > 0)
                        {
                            var richTextBox = (RichTextBox) control;
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

        /// <summary>
        ///     设定文本时的委托
        /// </summary>
        /// <param name="control"></param>
        private delegate void SetTextCallback(Control control, int text, string str);

        /// <summary>
        ///     设定文本时的委托
        /// </summary>
        /// <param name="control"></param>
        private delegate void SetTextCallbackStr(Control control, string text, string msgLevel);

        #endregion

        #endregion

        #endregion
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class ValueEventArgsloading : EventArgs
    {
        public int Value { set; get; }
        public string text { set; get; }
    }

    /// <summary>
    /// 定义事件使用的委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ValueChangedEventHandlerloading(object sender, ValueEventArgsloading e);
}