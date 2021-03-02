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
using System.Security.Cryptography;
using System.Logger;
namespace FBYClient
{
    public partial class upLoad_jkda : Form
    {
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
        /// 总条数
        /// </summary>
        public Int32  dataCountAll = 0;

        /// <summary>
        /// 当前数据条数
        /// </summary>
        public Int32  dataCountCrrent = 0;

        /// <summary>
        /// 保存生成后的本地文件地址
        /// </summary>
        public ArrayList localFileAddressList = new ArrayList();

        /// <summary>
        /// 进度条更新处理
        /// </summary>
        ValueEventArgsloading e_load = new ValueEventArgsloading();

        /// <summary>
        /// 显示的消息的类型区分 1:百分比  2:数据条数进度
        /// </summary>
        string msgQf = "2";

        /// <summary>
        /// 前页面的参数
        /// </summary>
       public DataTable dt_Para = null;


       /// <summary>
       /// 需要上传的数据条数
       /// </summary>
       public Int32 doDataCount = 0;

       /// <summary>
       /// 每次处理的数据条数
       /// </summary>
       public Int32 pagecount = 100;

       /// <summary>
       /// 日志输出对象
       /// </summary>
       public SimpleLogger logger = SimpleLogger.GetInstance();

        /// <summary>
        /// 上传的文件类型
        /// </summary>
       public string dataType = "";

        #region 检测网络状态
        
        /// <summary>
        ///检测网络状态
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        #endregion

        #region  初始化操作

        /// <summary>
        /// 构造函数
        /// </summary>
        public upLoad_jkda()
        {
            InitializeComponent();
        }

        /// <summary>
        ///初始化
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

            //上传数据
            upLoad_File();
        }

        /// <summary>
        ///初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upLoad_File()
        {
            // 实例化业务对象
            this.ValueChanged += new ValueChangedEventHandlerloading(this.workder_ValueChanged);

            // 使用异步方式调用长时间的方法
            Action handler = new Action(this.upLoadFile);

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

                    MessageBox.Show("上传完成！");
                }
                else if (ResultStatus.Equals("4"))
                {
                    MessageBox.Show(ResultStr);
                }
                else
                {
                    MessageBox.Show(ResultStr);
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

        /// <summary>
        /// 上传文件
        /// </summary>
        private void upLoadFile()
        {
            //文件上传
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "数据上传......", "INFO");
            //数据文件生成
            SetControlTextStr(label_title, "数据上传......", "INFO");
            upLoadFileByService(dt_Para, dataType);
            //将信息设定到页面中
            SetControlTextStr(lblMsg, "数据上传完成", "INFO");

            SetControlTextStr(label_title, "处理结束", "INFO");

            //处理正常结束
            ResultStatus = "7";
            // 进度条控制
            System.Threading.Thread.Sleep(2000);
            e_load.Value = 2;
            this.OnValueChanged(e_load);

        }

        #endregion

        #region 文件上传

        /// <summary>
        /// 数据上传
        /// </summary>
        /// <param name="dtLIst"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool upLoadFileByService(DataTable dtLIst,string type)
        {
            if (dtLIst != null && dtLIst.Rows.Count > 0)
            {

                //进度条初始化
                dataCountAll = dtLIst.Rows.Count;
                dataCountCrrent = 0;
                e_load.Value = 1;
                this.OnValueChanged(e_load);
                doDataCount = 0;

                int successCount = 0;
                int failCount = 0;
                int failNullCount = 0;

                string mssage = "";

                //string[] typeList

                for (int i = 0; i < dtLIst.Rows.Count; i++)
                {

                    //健康档案信息上传

                    //中医体质辨识信息上传

                    //自理能力评估信息上传

                    //档案信息上传

                    //lis信息上传
                    if (Common.UploadTYPE.lis信息.Equals(type))
                    {
                        string errMsg = "";
                        bool result = lisUpLoadToJkda(dtLIst.Rows[i]["SFZH"].ToString(), dtLIst.Rows[i]["JKDAH"].ToString(), dtLIst.Rows[i]["TJSJ_jktj"].ToString(), "", out errMsg);
                        //bool result = lisUpLoadToJkda("232102196202244020", dtLIst.Rows[i]["JKDAH"].ToString(), dtLIst.Rows[i]["TJSJ_jktj"].ToString(), "", out errMsg);
                        if (result == true)
                        {
                            successCount++;
                            update_T_JK_SJSC(dtLIst.Rows[i], type);
                        }
                        else
                        {
                            failCount++;
                            if (errMsg.Length > 0)
                            {
                                mssage = string.Format("{0} \r\n      {1} ", mssage, errMsg);
                                dataCountCrrent++;
                            }
                            else
                            {
                                failNullCount++;
                            }
                        }
                    }
                    dataCountCrrent = i+1;
                }
                SetControlTextStr(lblMsg, "    上传成功数据条数" + string.Format("({0})", successCount.ToString()), "INFO");
                SetControlTextStr(lblMsg, "    不存在上传数据条数" + string.Format("({0})", failNullCount.ToString()), "INFO");
                if (failCount > 0)
                {
                    SetControlTextStr(lblMsg, "    上传失败数据条数" + string.Format("({0})", failCount.ToString()), "Error");
                    SetControlTextStr(lblMsg, "    错误信息：" + mssage, "Error");
                }
                SetControlTextStr(lblMsg, "    上传完成" + string.Format("({0})", doDataCount.ToString()), "INFO");
            }
            return true;

        }

        /// <summary>
        /// 健康体检中的lis信息直接上传到健康档案系统中
        /// </summary>
        /// <param name="D_SFZH"></param>
        /// <param name="D_GRDABH"></param>
        /// <param name="P_RGID"></param>
        /// <returns></returns>
        public bool lisUpLoadToJkda(string D_SFZH, string D_GRDABH,string HAPPENTIME, string P_RGID,out string errMsg)
        {
            errMsg = "";
            try
            {
                //sql
                string sql = string.Format("select * from T_JK_JKTJ where D_GRDABH='{0}' and HAPPENTIME='{1}'", D_GRDABH, HAPPENTIME);

                //取得数据
                DBAccess dbaccess = new DBAccess();
                DataTable dt_jkdaLis = dbaccess.ExecuteQueryBySql(sql);
                dt_jkdaLis.TableName = gwtjUpload.commSys.tableName.检验信息;
                if (dt_jkdaLis != null && dt_jkdaLis.Rows.Count > 0)
                {
                    gwtjUpload.gwtjUpload gwtjupload = new gwtjUpload.gwtjUpload();
                    string result = gwtjupload.gwtjDoService(dt_jkdaLis, D_SFZH, D_GRDABH, P_RGID,"","");
                    string[] resultList = result.Split(new char[] { '-' });

                    //正常情况下
                    if (resultList.Length > 0 && resultList[0].Equals("0"))
                    {
                        return true;
                    }
                    else
                    {
                        errMsg = result;
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string update_T_JK_SJSC(DataRow dtRow,string type)
        {
            try
            {
                ArrayList returnArrayList = new ArrayList();

                //删除用sql
                string SqlDele = Common.getSql("sql_T_JK_SJSC_delete", " ");

                //追加记录用sql
                string SqlAdd = Common.getSql("sql_T_JK_SJSC_insert", "");

                //删除用sql
                //医疗机构编码
                SqlDele = SqlDele.Replace("{YLJGBM}", dtRow["YLJGBM"].ToString());
                //健康档案号
                SqlDele = SqlDele.Replace("{JKDAH}", dtRow["JKDAH"].ToString());
                //年度
                SqlDele = SqlDele.Replace("{ND}", DateTime.Now.Year.ToString());
                //文档类型
                SqlDele = SqlDele.Replace("{TJTYPE}", type);

                returnArrayList.Add(SqlDele);

                //追加用sql  ( '{YLJGBM}','{JKDAH}','{XM}','{SFZH}','{ND}','{GZZBM}','{TJSJ}','{CZY}','{TJTYPE}','{ZT}')
                //医疗机构编码
                SqlAdd = SqlAdd.Replace("{YLJGBM}", dtRow["YLJGBM"].ToString());
                //健康档案号
                SqlAdd = SqlAdd.Replace("{JKDAH}", dtRow["JKDAH"].ToString());
                //姓名
                SqlAdd = SqlAdd.Replace("{XM}", dtRow["XM"].ToString());
                //身份证号
                SqlAdd = SqlAdd.Replace("{SFZH}", dtRow["SFZH"].ToString());
                //年度
                SqlAdd = SqlAdd.Replace("{ND}", DateTime.Now.Year.ToString());
                //上传时间
                SqlAdd = SqlAdd.Replace("{SCSJ}", DateTime.Now.ToString("yyyy-MM-dd"));
                //操作员
                SqlAdd = SqlAdd.Replace("{CZY}", UserInfo.userId);
                //文档类型
                SqlAdd = SqlAdd.Replace("{TJTYPE}", type);
                //体检状态
                SqlAdd = SqlAdd.Replace("{ZT}", Common.ZT.确定状态);
                returnArrayList.Add(SqlAdd);
                DBAccess dbaccess = new DBAccess();
                dbaccess.ExecuteNonQueryBySqlList(returnArrayList);
            }
            catch (Exception ex)
            {
                return "update_T_JK_SJSC更新错误！" + ex.Message;
            }
            return "";
        }

        #endregion
        #region 进度管理

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
                int lenght = Convert.ToInt32((getFileSize(localPath) * 100.00) / fileAllSize);

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
        private void SetControlTextStr(Control control, string text,string msgLevel)
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
