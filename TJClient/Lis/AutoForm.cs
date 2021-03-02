using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using TJClient.sys.Bll;
using TJClient.Common;
using LIS;

namespace FBYClient 
{
    public partial class AutoForm : sysAutoForm
    {
        /// <summary>
        /// 用户名
        /// </summary>
        private static string userId = "";

        ///// <summary>
        ///// 分组
        ///// </summary>
        //private static string yhfz = "";

        ///// <summary>
        ///// 医疗机构
        ///// </summary>
        //private static string yljg = "";

        ///// <summary>
        ///// 村庄编码
        ///// </summary>
        //private static string czbm= "";

        public lis_new Jktj_lis = null;

        /// <summary>
        /// 仪器的型号
        /// </summary>
        public string yqxh = "";

        /// <summary>
        /// 仪器
        /// </summary>
        public static IInterface yqDemo = null;

        /// <summary>
        /// 仪器数据处理类型
        /// </summary>
        public static string YQ_Type = "";


        public AutoForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设定仪器的型号
        /// </summary>
        /// <param name="yqxh"></param>
        /// <returns></returns>
        public override bool setStart(string yqxhPara)
        {
            string YqXmlPath = "";
            string YQ_Interval = "";
            string YQ_DateType = "";
            Jktj_lis = new lis_new();
            //xmlpath
            YqXmlPath = Common.getyqPath(yqxhPara);
            //jg数据处理间隔时间
            YQ_Interval = XmlRW.GetValueFormXML(YqXmlPath, "YQ_Interval", "value");

            //rqlx	标本日期类型
            YQ_DateType = XmlRW.GetValueFormXML(YqXmlPath, "YQ_DateType", "value");

            //仪器数据处理类型
            YQ_Type = XmlRW.GetValueFormXML(YqXmlPath, "YQ_Type", "value");



            timer_lis.Enabled = false;

            yqxh = yqxhPara;
            if (yqxh.Length > 0)
            {
                if (YQ_Interval.Equals("0"))
                {
                    timer_lis.Interval = 5000;
                }
                else
                {
                    timer_lis.Interval = int.Parse(YQ_Interval) * 1000;
                }
                timer_lis.Enabled = true;
            }
            return true;
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoForm_Load(object sender, EventArgs e)
        {
         
            ////取得前以页面传递的数据
            //DataTable dt = (DataTable)((jktj)this.Owner).Tag;
            ////用户id
            //userId = dt.Rows[0]["userId"].ToString();
            ////工作组
            //yhfz = dt.Rows[0]["gzz"].ToString();
            ////医疗机构
            //yljg = dt.Rows[0]["yljg"].ToString();

            ////村庄编码
            //czbm = dt.Rows[0]["czbm"].ToString();

            Jktj_lis = (lis_new)this.Owner;

            //初始化签名基础信息
            //SignNameGroupInit();
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_lis_Tick(object sender, EventArgs e)
        {
            try
            {
                dataRecive();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        public DataTable dataRecive()
        {
            DataTable dt = null;
            string errMsg = "";
            try
            {
                if (yqDemo != null)
                {

                    if (yqDemo.IsOpen(out errMsg) == false)
                    {
                        yqDemo.open(out errMsg);
                    }
                    dt = yqDemo.YQDataReturn(DateTime .Now.ToString("yyyy-MM-dd"), out errMsg);

                }
                else
                {
                    if (yqxh.Trim().Length == 0)
                    {
                        timer_lis.Enabled = false;
                        //MessageBox.Show("请选择仪器！");

                        return null;
                    }

                    if (yqDemo == null)
                    {
                        string yqVersion = XmlRW.GetValueFormXML(Common.getyqPath(yqxh), "YQ_Version", "value");
                        yqDemo = LisFactory.LisCreate(yqVersion);
                    }

                    if (yqDemo.IsOpen(out errMsg) == false)
                    {
                        yqDemo.open(out errMsg);
                    }
                    dt = yqDemo.YQDataReturn(DateTime.Now.ToString("yyyy-MM-dd"), out errMsg);
                }

                //将取得的结果保存到数据库中
                if (dt != null)
                {
                    if (!dt.Columns.Contains("yybm"))
                    {
                        DataColumn dtcolumn = new DataColumn("yybm");
                        dtcolumn.DefaultValue = UserInfo.Yybm;
                        dt.Columns.Add(dtcolumn);
                    }

                    if (!dt.Columns.Contains("yq"))
                    {
                        DataColumn dtcolumn = new DataColumn("yq");
                        dtcolumn.DefaultValue = timer_lis;
                        dt.Columns.Add(dtcolumn);
                    }

                    Form_lisBll form_lisbll = new Form_lisBll();
                    form_lisbll.Add(dt, "sql042");
                }

            }
            catch (Exception ex)
            {
                timer_lis.Enabled = false;
                Jktj_lis.msgShow(ex.Message);
            }
            return dt;
        }

        #region 托盘及右键退出
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            this.Show();
            //this.Location = formLocal;

            this.WindowState = FormWindowState.Normal;

            this.TopMost = true;

        }

        /// <summary>
        /// 托盘中显示时的设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoForm_xd_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.Visible = false;
                this.notifyIcon1.ShowBalloonTip(30, "提示", "心电同步处理", ToolTipIcon.Info);
            }
        }

        /// <summary>
        /// 右键退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //退出系统时清空登陆信息
            this.Close();
            //Application.ExitThread();
        }

        #endregion
    
    }
}
