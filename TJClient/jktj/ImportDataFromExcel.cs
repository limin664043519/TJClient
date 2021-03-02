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
using TJClient.sys.Bll;
namespace FBYClient
{
    public partial class ImportDataFromExcel : Form
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

        /// <summary>
        /// 导入的表的设定信息
        /// </summary>
        public DataRow dtRow = null;


        public ImportDataFromExcel()
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

        DataSet ds_result = null;

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
            //导入数据
            importFile_File();
        }

        /// <summary>
        ///初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importFile_File()
        {
            // 实例化业务对象
            this.ValueChanged += new ValueChangedEventHandlerloading(this.workder_ValueChanged);

            // 使用异步方式调用长时间的方法
            Action handler = new Action(this.importFile);

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

                    //MessageBox.Show("导入完成！");
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
        /// 导入文件
        /// </summary>
        private void importFile()
        {
            //数据文件导入
            SetControlTextStr(lblMsg, "开始导入数据文件......", "INFO");
            SetControlTextStr(label_title, "导入数据文件......", "INFO");
            bool result_import = importFiles();
            SetControlTextStr(lblMsg, "数据文件导入完成", "INFO");

            SetControlTextStr(lblMsg, "开始同步数据......", "INFO");
            SetControlTextStr(label_title, "同步数据......", "INFO");
            updateDataToTable();
            SetControlTextStr(lblMsg, "同步数据完成", "INFO");



            SetControlTextStr(label_title, "处理结束", "INFO");
            // 进度条控制
            System.Threading.Thread.Sleep(2000);
            e_load.Value = 2;
            this.OnValueChanged(e_load);
        }

        #region 文件导入

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
                    importFileTo(localFileAddressList[i].ToString());
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
            ds_result = new DataSet();

            string tablename = "";
            try
            {
                ds_result = getDsFromExcel(filePath);
                if (ds_result != null && ds_result.Tables.Count > 0)
                {

                    //计算数据总条数
                    dataCountAll = 0;

                    //遍历数据库表,计算数据总条数
                    for (int tableIndex = 0; tableIndex < ds_result.Tables.Count; tableIndex++)
                    {
                        dataCountAll = dataCountAll + ds_result.Tables[tableIndex].Rows.Count;
                        if (ds_result.Tables[tableIndex].Rows .Count >0)
                        {
                            ds_result.Tables[tableIndex].Columns.Add("jkdabh");
                            ds_result.Tables[tableIndex].Columns.Add("G_BCHAO");
                            //删除要导入的数据
                            //importFileDelete(ds_result.Tables[tableIndex]);
                         }
                    }

                    // 进度条控制
                    this.msgQf = "2";
                    //dataCountAll = dataCountAll;
                    dataCountCrrent = 0;
                    e_load.Value = 1;
                    this.OnValueChanged(e_load);

                    int commitCount = 1000;

                    DataTable dt_result = ds_result.Tables[0].Clone();

                     //设定信息正确
                    if (dtRow != null && dtRow["dbTableName_from"] != null)
                    {
                        //数据库表名称
                        tablename = dtRow["dbTableName_from"].ToString();
                    }

                    //string columnFormat = "";
                    DataTable dt_column = new DataTable();
                    dt_column.Rows.Add();

                    //特殊字段的处理
                    if (dtRow != null && dtRow["columnFormat"] != null)
                    {
                        //数据库表名称 阳性标记:G_BCHAO $ 阴性:0@阳性:1
                        string[] columnFormatList = dtRow["columnFormat"].ToString().Split(new char[] { '$' });
                           
                        string[] columnNameList = columnFormatList[0].Split(new char[] { ':' });
                        dt_column.Columns.Add(columnNameList[0]);
                        dt_column.Rows[0][columnNameList[0]] = columnNameList[1];

                        //值转换
                        string[] columnvalueList = columnFormatList[1].Split(new char[] { '@' });
                        for (int i = 0; i < columnvalueList.Length; i++)
                        {
                            string[] columnvalueListName = columnvalueList[i].Split(new char[] { ':' });
                            dt_column.Columns.Add(columnvalueListName[0]);
                            dt_column.Rows[0][columnvalueListName[0]] = columnvalueListName[1];
                        } 
                    }

                    //遍历数据库表
                    for (int tableIndex = 0; tableIndex < ds_result.Tables.Count; tableIndex++)
                    {
                        dt_result.TableName = tablename;

                        for (int rowIndex = 0; rowIndex < ds_result.Tables[tableIndex].Rows.Count; rowIndex++)
                        {
                            dataCountCrrent++;
                            ds_result.Tables[tableIndex].Rows[rowIndex]["jkdabh"] = getJkdahByTxmh(ds_result.Tables[tableIndex].Rows[rowIndex][dtRow["updateBycolumn"].ToString()].ToString());



                            ds_result.Tables[tableIndex].Rows[rowIndex]["G_BCHAO"] = dt_column.Rows[0][ds_result.Tables[tableIndex].Rows[rowIndex][dt_column.Columns[0].ColumnName].ToString()].ToString();//  getJkdahByTxmh(ds_result.Tables[tableIndex].Rows[rowIndex][dtRow["updateBycolumn"].ToString()].ToString());

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
                //throw ex;
                ResultStatus = "4";
                ResultStr = ex.Message;
                return false;
            }
            return true;
        }



        /// <summary>
        /// 把指定的文件导入到数据库中
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool updateDataToTable()
        {
            ////excel转换的数据集合
            //ds_result = new DataSet();

            string tablename = "";
            try
            {

                if (ds_result != null && ds_result.Tables.Count > 0)
                {

                    //计算数据总条数
                    dataCountAll = 0;

                    //遍历数据库表,计算数据总条数
                    for (int tableIndex = 0; tableIndex < ds_result.Tables.Count; tableIndex++)
                    {
                        dataCountAll = dataCountAll + ds_result.Tables[tableIndex].Rows.Count;

                    }

                    // 进度条控制
                    this.msgQf = "2";
                    //dataCountAll = dataCountAll;
                    dataCountCrrent = 0;
                    e_load.Value = 1;
                    this.OnValueChanged(e_load);

                    //int commitCount = 1000;

                    DataTable dt_result = ds_result.Tables[0].Clone();

                    //设定信息正确
                    if (dtRow != null && dtRow["dbTableName_to"] != null)
                    {
                        //数据库表名称
                        tablename = dtRow["dbTableName_to"].ToString();
                    }

                    //string columnFormat = "";
                    DataTable dt_column_value = new DataTable();
                    dt_column_value.Rows.Add();

                    //DataTable dt_column_where = new DataTable();
                    //dt_column_where.Rows.Add();


                    string sqlInsert = "";
                    string sqlInsertText = "";
                    string sqlInsertvalue = "";

                    string sqlUpdate = "";


                    //dt_column_value  同步字段对应
                    if (dtRow != null && dtRow["columnSet"] != null)
                    {
                        //值转换
                        string[] columnvalueList = dtRow["columnSet"].ToString().Split(new char[] { '|' });
                        for (int i = 0; i < columnvalueList.Length; i++)
                        {
                            string[] columnvalueListName = columnvalueList[i].Split(new char[] { ':' });
                            dt_column_value.Columns.Add(columnvalueListName[0]);
                            dt_column_value.Rows[0][columnvalueListName[0]] = columnvalueListName[1];

                            sqlInsertText = sqlInsertText + "," + columnvalueListName[1];
                            sqlInsertvalue = sqlInsertvalue + ",'[" + columnvalueListName[0]+"]'";
                            sqlUpdate = string.Format("{0}, {1}='[{2}]' ",sqlUpdate, columnvalueListName[1], columnvalueListName[0]);
                        }
                    }

                    string sqlWhere = "";
                    //dt_column_where  updateToColumn
                    if (dtRow != null && dtRow["updateToColumn"] != null)
                    {
                        //值转换
                        string[] columnvalueList = dtRow["updateToColumn"].ToString().Split(new char[] { '|' });
                        for (int i = 0; i < columnvalueList.Length; i++)
                        {
                            string[] columnvalueListName = columnvalueList[i].Split(new char[] { ':' });
                            dt_column_value.Columns.Add(columnvalueListName[0]);
                            dt_column_value.Rows[0][columnvalueListName[0]] = columnvalueListName[1];

                            sqlWhere = string.Format(" {0} and {1}='[{2}]' ", sqlWhere, columnvalueListName[1], columnvalueListName[0]);
                            sqlInsertText = sqlInsertText + "," + columnvalueListName[1];
                            sqlInsertvalue = sqlInsertvalue + ",'[" + columnvalueListName[0] + "]'";
                        }
                    }

                    //插入语句
                    sqlInsert = string.Format(" insert into {0} ( {1},czy,gzz) values ({2},'{3}','{4}') ", tablename, sqlInsertText.Substring(1), sqlInsertvalue.Substring(1),UserInfo .userId,UserInfo .gzz);
                    
                    //更新语句
                    sqlUpdate = string.Format(" update {0} set {1},czy='{3}',gzz='{4}' where 1=1  {2} ", tablename, sqlUpdate.Substring(1), sqlWhere, UserInfo.userId, UserInfo.gzz);
                    //检索语句
                    string sqlSelect = string.Format(" select * from {0} where 1=1  {1} ", tablename,sqlWhere);

                    //体检状态
                    string delete_tjzt = " delete from T_JK_TJZT where YLJGBM='" + UserInfo.Yybm + "' and JKDAH='[jkdabh]' and TJTYPE='1'";

                    string insert_tjzt = " insert into  T_JK_TJZT (YLJGBM,JKDAH,ND,GZZBM,TJSJ,CZY,TJTYPE,ZT) values ('" + UserInfo.Yybm + "','[jkdabh]','" + DateTime.Now.Year.ToString() + "','" + UserInfo.gzz + "','[检查日期]','" + UserInfo.userId + "','1','1')";


                    ArrayList sqlArrayList_tem = new ArrayList();
                    DBAccess access = new DBAccess();
                    //遍历数据库表
                    for (int tableIndex = 0; tableIndex < ds_result.Tables.Count; tableIndex++)
                    {
                        //dt_result.TableName = tablename;

                        for (int rowIndex = 0; rowIndex < ds_result.Tables[tableIndex].Rows.Count; rowIndex++)
                        {
                            dataCountCrrent++;

                            string sqlInsert_tem = sqlInsert;
                            string sqlUpdate_tem = sqlUpdate;
                            string sqlSelect_tem = sqlSelect;
                            string delete_tjzt_tem = delete_tjzt;
                            string insert_tjzt_tem = insert_tjzt;

                            for (int i = 0; i < dt_column_value.Columns.Count; i++)
                            {
                                if ("检查日期".Equals(dt_column_value.Columns[i].ColumnName) == false)
                                {
                                    string replaceStr = "[" + dt_column_value.Columns[i].ColumnName + "]";
                                    sqlInsert_tem = sqlInsert_tem.Replace(replaceStr, ds_result.Tables[tableIndex].Rows[rowIndex][dt_column_value.Columns[i].ColumnName].ToString());
                                    sqlUpdate_tem = sqlUpdate_tem.Replace(replaceStr, ds_result.Tables[tableIndex].Rows[rowIndex][dt_column_value.Columns[i].ColumnName].ToString());
                                    sqlSelect_tem = sqlSelect_tem.Replace(replaceStr, ds_result.Tables[tableIndex].Rows[rowIndex][dt_column_value.Columns[i].ColumnName].ToString());
                                    delete_tjzt_tem = delete_tjzt_tem.Replace(replaceStr, ds_result.Tables[tableIndex].Rows[rowIndex][dt_column_value.Columns[i].ColumnName].ToString());
                                    insert_tjzt_tem = insert_tjzt_tem.Replace(replaceStr, ds_result.Tables[tableIndex].Rows[rowIndex][dt_column_value.Columns[i].ColumnName].ToString());
                                }
                                else
                                {
                                    string replaceStr = "[" + dt_column_value.Columns[i].ColumnName + "]";
                                    string replaceValue = ds_result.Tables[tableIndex].Rows[rowIndex][dt_column_value.Columns[i].ColumnName].ToString();
                                    if (replaceValue.Length == 0)
                                    {
                                        replaceValue = DateTime.Now.ToString("yyyy-MM-dd");
                                    }
                                    else if (replaceValue.Length == 8)
                                    {
                                        replaceValue = replaceValue.Insert(6, "-").Insert(4, "-");
                                    }
                                    sqlInsert_tem = sqlInsert_tem.Replace(replaceStr, replaceValue);
                                    sqlUpdate_tem = sqlUpdate_tem.Replace(replaceStr, replaceValue);
                                    sqlSelect_tem = sqlSelect_tem.Replace(replaceStr, replaceValue);
                                    delete_tjzt_tem = delete_tjzt_tem.Replace(replaceStr, replaceValue);
                                    insert_tjzt_tem = insert_tjzt_tem.Replace(replaceStr, replaceValue);
                                }
                            
                            }

                           DataTable dt_exist= access.ExecuteQueryBySql(sqlSelect_tem);
                           if (dt_exist != null && dt_exist.Rows.Count > 0)
                           {
                               sqlArrayList_tem.Add(sqlUpdate_tem);
                           }
                           else
                           {
                               sqlArrayList_tem.Add(sqlInsert_tem);
                               sqlArrayList_tem.Add(delete_tjzt_tem);
                               sqlArrayList_tem.Add(insert_tjzt_tem);
                           }
                        }
                    }
                    if (sqlArrayList_tem.Count  > 0)
                    {
                        access.ExecuteNonQueryBySqlList(sqlArrayList_tem);
                    }
                }

            }
            catch (Exception ex)
            {
                //progressBar_xz.Visible = false;
                //MessageBox.Show(string.Format("表[{0}]{1}", tablename, ex.Message));
                //throw ex;
                ResultStatus = "4";
                ResultStr = ex.Message;
                return false;
            }
            return true;
        }


        /// <summary>
        /// 按照条形码号获取健康档案号
        /// </summary>
        /// <param name="Txmh"></param>
        /// <returns></returns>
        public string getJkdahByTxmh(string Txmh)
        {
            string jkdah="";
            try
            {
                Form_yljgBll Form_jkdah = new Form_yljgBll();
                DataTable importDataSet = Form_jkdah.GetMoHuList(string.Format(" and YLJGBM='{0}' and TXMBH='{1}' ", UserInfo.Yybm, Txmh), "sql_select_people_txm");

                if (importDataSet != null && importDataSet.Rows.Count > 0)
                {
                    jkdah = importDataSet.Rows[0]["JKDAH"].ToString();
                }
            
            }
            catch (Exception ex)
            {

            }
            return jkdah;
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
            string deleteWhere = "";
            string[] deletecolumn = null;

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    //设定信息正确
                    if (dtRow != null && dtRow["dbTableName_from"] != null)
                    {
                        //数据库表名称
                        tablename = dtRow["dbTableName_from"].ToString();

                        //删除条件
                        deleteWhere = " ";
                        if (dtRow["deleteBycolumn"] != null && dtRow["deleteBycolumn"].ToString().Length > 0)
                        {
                            deletecolumn = dtRow["deleteBycolumn"].ToString().Split(new char[] { ':' });

                            for (int i = 0; i < deletecolumn.Length; i++)
                            {
                                deleteWhere = deleteWhere + " and  " + deletecolumn[i] + "='{" + deletecolumn[i] + "}' ";
                            }
                        }

                        if (deletecolumn == null)
                        {

                            sqlList.Add(string.Format("delete from {0} where YLJGBM ='{1}' ", tablename, UserInfo.Yybm));

                        }
                        else
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                string strWhereTem = deleteWhere;
                                for (int k = 0; k < deletecolumn.Length; k++)
                                {
                                    strWhereTem = strWhereTem.Replace("{" + deletecolumn[k] + "}", dt.Rows[j][deletecolumn[k]].ToString());
                                }

                                sqlList.Add(string.Format("delete from {0} where 1=1 {1} ", tablename, strWhereTem));
                            }
                        }
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

                    DataTable dtresult_tem = dt.Copy();

                    //增加列
                    //YLJGBM，importDate，importUser，nd
                    DataColumn dc_YLJGBM = new DataColumn("YLJGBM");
                    dc_YLJGBM.DefaultValue = UserInfo.Yybm;
                    dtresult_tem.Columns.Add(dc_YLJGBM);

                    //YLJGBM，importDate，importUser，nd
                    DataColumn dc_importDate = new DataColumn("importDate");
                    dc_importDate.DefaultValue = DateTime.Now.ToString ("yyyy-MM-dd");
                    dtresult_tem.Columns.Add(dc_importDate);

                    //YLJGBM，importDate，importUser，nd
                    DataColumn dc_importUser = new DataColumn("importUser");
                    dc_importUser.DefaultValue = UserInfo.userId;
                    dtresult_tem.Columns.Add(dc_importUser);

                    //YLJGBM，importDate，importUser，nd
                    DataColumn dc_nd = new DataColumn("nd");
                    dc_nd.DefaultValue = DateTime.Now.Year .ToString();
                    dtresult_tem.Columns.Add(dc_nd);

                    //新增
                    sqlList = Common.FormatSql(dtresult_tem, Common.SQLTYPE.insert.ToString(), "");
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

            bool boolResult = commonExcel.ExcelFileToDataSet1(filePath, out ds, out errMessage);
            if (boolResult == false)
            {
                throw new Exception(errMessage);
                //return null;
            }
            return ds;
        }
        #endregion

        #region 数据同步









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
        public Thread t=null;

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
                            if (t != null)
                            {
                                t.Abort();
                            }
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
        delegate void SetTextCallbackStr(Control control, string text,string msgLevel);
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
