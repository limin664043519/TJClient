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

namespace FBYClient//TJClient.Lis
{
    public partial class AutoForm_md : sysAutoForm
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


        public AutoForm_md()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设定仪器的型号
        /// </summary>
        /// <param name="yqxh"></param>
        /// <returns></returns>
        public override  bool setStart(string yqxhPara)
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
        private void AutoForm_md_Load(object sender, EventArgs e)     
        {           
            Jktj_lis = (lis_new)this.Owner;
            //初始化签名基础信息
            SignNameGroupInit();
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
                string paratime = "";

                //开始时间
                if (dateTimePicker_start.Checked == true)
                {
                    paratime = paratime + dateTimePicker_start.Value.ToString("yyyy-MM-dd")+" 00:00:00"+"|";
                }

                //结束时间
                if (dateTimePicker_end.Checked == true)
                {
                    if (paratime.Length > 0)
                    {
                        paratime = paratime + dateTimePicker_end.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    }
                    else
                    {
                        paratime = paratime +"|"+ dateTimePicker_end.Value.ToString("yyyy-MM-dd")+ " 23:59:59";
                    }
                }

                if (paratime.Length == 0)
                {
                    paratime = DateTime.Now.ToString("yyyy-MM-dd")+" 00:00:00" + "|";
                }

                //0：获取新数据  1:获取所有数据
                paratime = paratime + "|" + "0";


                if (yqDemo != null)
                {
                    if (yqDemo.IsOpen(out errMsg) == false)
                    {
                        yqDemo.open(out errMsg);
                    }
                    dt = yqDemo.YQDataReturn(paratime, out errMsg);
                }
                else
                {
                    if (yqxh.Trim().Length == 0)
                    {
                        timer_lis.Enabled = false;

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

                    dt = yqDemo.YQDataReturn(paratime, out errMsg);
                }

                //将取得的结果保存到数据库中
                if (dt != null && dt.Rows .Count >0)
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

                    //Form_lisBll form_lisbll = new Form_lisBll();
                    //form_lisbll.Add(dt, "sql042");
                    
                    //string xmlPath = System.Configuration.ConfigurationSettings.AppSettings["MECG200"].ToString();
                    //string YQ_YQtype = XmlRW.GetValueFormXML(xmlPath, "YQ_YQType","value");
                    //button_save_sys(dt, YQ_YQtype);

                    //将获得的结果匹配体检表中的心电图的结果
                    dt = dt_pp(dt);


                    //再次处理
                    DataTable dt_again = dt.Clone();
                    DataRow[] dtrow_again = dt.Select(" qf='1'");
                    if (dtrow_again.Length > 0)
                    {
                        for (int i = 0; i < dtrow_again.Length; i++)
                        {
                            dt_again.ImportRow(dtrow_again[i]);
                        }
                    }

                    //新数据
                    DataTable dt_new = dt.Clone();
                    DataRow[] dtrow_new = dt.Select(" qf='2'");
                    if (dtrow_new.Length > 0)
                    {
                        for (int i = 0; i < dtrow_new.Length; i++)
                        {
                            dt_new.ImportRow(dtrow_new[i]);
                            //置变量，以后不再读取此记录
                            MedExSqlHelper DBF = new MedExSqlHelper();
                            DBF.ExecuteNonQuery("update PACS_APPLY set work_status = 1 where PATIENT_ID = '" + dtrow_new[i]["ybh"].ToString() + "'");
                        }
                    }

                    Form_lisBll form_lisbll = new Form_lisBll();

                    if (dt_new != null && dt_new.Rows.Count > 0)
                    {
                        form_lisbll.Add(dt, "sql042");

                        string xmlPath = Common.getyqPath("MECG200");
                        string YQ_YQtype = XmlRW.GetValueFormXML(xmlPath, "YQ_YQType", "value");
                        button_save_sys(dt, YQ_YQtype);
                    }

                    if (dt_again != null && dt_again.Rows.Count > 0)
                    {
                        //更新数据
                        form_lisbll.Upd_all(dt_again, "sql042_update");
                        string xmlPath = Common.getyqPath("MECG200");
                        string YQ_YQtype = XmlRW.GetValueFormXML(xmlPath, "YQ_YQType", "value");

                        //更新数据
                        button_save_sys_again(dt_again, YQ_YQtype);
                    }

                }

            }
            catch (Exception ex)
            {
                timer_lis.Enabled = false;
                Jktj_lis.msgShow(ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// 其他通用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_sys(DataTable dt_all, string sqlcode)
        {          
            try
            {
                if (dt_all.Rows.Count > 0)
                {
                    DataRow[] drs = dt_all.Select("xmdh='NAME'");
                    for (int i = 0; i < drs.Length; i++)
                    {
                        DBAccess dbjkdah = new DBAccess();
                        DataTable dt_access = dbjkdah.ExecuteQueryBySql(@"select jkdah,sfzh from T_JK_TJRY_TXM where txmbh='" + drs[i]["ybh"].ToString() + "' and yljgbm='" + drs[i]["yybm"].ToString() + "'");
                        //没有对应的人员信息时直接退出
                        if (dt_access == null || dt_access.Rows.Count <= 0) continue;

                        //更新化验结果表(T_JK_lis_result_re），确定化验结果与人员关系
                        DataTable dt_update = new DataTable();
                        //医院编码
                        dt_update.Columns.Add("yybm");
                        //仪器编码
                        dt_update.Columns.Add("yq");
                        //样本号
                        dt_update.Columns.Add("ybh");
                        //体检日期
                        dt_update.Columns.Add("jyrq");
                        //健康档案号
                        dt_update.Columns.Add("testno");
                        //年度
                        dt_update.Columns.Add("nd");

                        dt_update.Rows.Add();
                        dt_update.AcceptChanges();

                        dt_update.Rows[0]["yybm"] = UserInfo.Yybm;
                        dt_update.Rows[0]["yq"] = "MECG200";
                        dt_update.Rows[0]["ybh"] = drs[i]["ybh"].ToString();
                        dt_update.Rows[0]["jyrq"] = Convert.ToDateTime(drs[i]["jyrq"].ToString()).ToString("yyyy-MM-dd");
                        //dt_update.Rows[0]["testno"] = dt_access.Rows[0]["jkdah"].ToString();
                        //对应字段保存样本号
                        dt_update.Rows[0]["testno"] = drs[i]["ybh"].ToString();
                        //dt_update.Rows[0]["nd"] = Convert.ToDateTime(jyrq_tem).Year .ToString();
                        dt_update.Rows[0]["nd"] = DateTime.Now.Year.ToString();
                        Form_lisBll form_lisbll = new Form_lisBll();

                        //标本与人员的对应关系保存
                        form_lisbll.Upd(dt_update, "sql_lis_result_ryxx_update");

                        //更新体检人员状态表
                        //form_lisbll.Upd(dt_update, "sql077");
                        //体检状态信息
                        ArrayList TJRYXXList = save_T_JK_TJZT(dt_access.Rows[0]["jkdah"].ToString(), dt_access.Rows[0]["sfzh"].ToString(), drs[i]["xmmc"].ToString(), dt_update.Rows[0]["jyrq"].ToString(), Common.TJTYPE.健康体检表, Common.ZT.确定状态);
                        if (TJRYXXList != null && TJRYXXList.Count > 0)
                        {
                            DBAccess dbaccess = new DBAccess();
                            dbaccess.ExecuteNonQueryBySql(TJRYXXList);
                        }

                        //保存体检结果到体检结果表
                        string sqlWhere = "";
                        //医院编码
                        if (UserInfo.Yybm.Length > 0)
                        {
                            sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.yybm='{0}'", UserInfo.Yybm);
                        }
                        //仪器
                        if (UserInfo.Yybm.Length > 0)
                        {
                            sqlWhere = sqlWhere + string.Format(" and  T_JK_lis_result_re.yq='{0}'", drs[i]["yq"].ToString());
                        }
                        //检验日期
                        if (UserInfo.Yybm.Length > 0)
                        {
                            sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.jyrq='{0}'", drs[i]["jyrq"].ToString());
                        }
                        //样本号
                        if (UserInfo.Yybm.Length > 0)
                        {
                            sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.ybh='{0}'", drs[i]["ybh"].ToString());
                        }

                        //取得化验结果
                        DataTable dt_dyxm = form_lisbll.GetMoHuList(sqlWhere, "sql045");

                        //更新健康体检表
                        DataTable dt_tjjgUpdate = new DataTable();
                        dt_tjjgUpdate.Rows.Add();

                        for (int j = 0; j < dt_dyxm.Rows.Count; j++)
                        {

                            if (dt_tjjgUpdate.Columns.Contains(dt_dyxm.Rows[j]["HIS_DB"].ToString()) == false)
                            {
                                DataColumn dtColumn = new DataColumn(dt_dyxm.Rows[j]["HIS_DB"].ToString());
                                dtColumn.DefaultValue = dt_dyxm.Rows[j]["result"].ToString();
                                dt_tjjgUpdate.Columns.Add(dtColumn);
                            }

                        }

                        if (dt_tjjgUpdate.Columns.Contains("D_GRDABH") == false)
                        {
                            DataColumn dtColumn = new DataColumn("D_GRDABH");
                            dtColumn.DefaultValue = dt_access.Rows[0]["jkdah"].ToString();
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }

                        if (dt_tjjgUpdate.Columns.Contains("HAPPENTIME") == false)
                        {
                            DataColumn dtColumn = new DataColumn("HAPPENTIME");
                            dtColumn.DefaultValue = Convert.ToDateTime(drs[i]["jyrq"].ToString()).ToString("yyyy-MM-dd");
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }

                        if (dt_tjjgUpdate.Columns.Contains("czy") == false)
                        {
                            DataColumn dtColumn = new DataColumn("czy");
                            dtColumn.DefaultValue = UserInfo.userId;
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }

                        if (dt_tjjgUpdate.Columns.Contains("gzz") == false)
                        {
                            DataColumn dtColumn = new DataColumn("gzz");
                            dtColumn.DefaultValue = UserInfo.gzz;
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }


                        //体检结果是否已经存在
                        string Guid = "";
                        //true:新的Guid  false:已经存在的Guid
                        bool GuidResult = true;
                        GuidResult = getNewGuid(dt_access.Rows[0]["jkdah"].ToString(), drs[i]["jyrq"].ToString(), out Guid);

                        if (dt_tjjgUpdate.Columns.Contains("guid") == false)
                        {
                            DataColumn dtColumn = new DataColumn("guid");
                            dtColumn.DefaultValue = Guid;
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }


                        string[] sqllist = sqlcode.Split(new char[] { '|' });

                        if (sqlcode.Length == 0 || sqllist.Length < 2)
                        {
                            MessageBox.Show("仪器sql设定错误！");
                            return;
                        }
                        //体检结果
                        if (GuidResult == true)
                        {
                            //体检结果插入
                            form_lisbll.Add(dt_tjjgUpdate, sqllist[0]);
                        }
                        else
                        {
                            //体检结果更新
                            dt_tjjgUpdate.AcceptChanges();
                            dt_tjjgUpdate.Rows[0]["guid"] = Guid;
                            form_lisbll.Upd(dt_tjjgUpdate, sqllist[1]);
                        }

                        //签名
                        SaveJktjSignname(drs[i]["jyrq"].ToString(), dt_access.Rows[0]["jkdah"].ToString());

                        //置变量，以后不再读取此记录
                        MedExSqlHelper DBF = new MedExSqlHelper();
                        DBF.ExecuteNonQuery("update PACS_APPLY set work_status = 1 where PATIENT_ID = '" + drs[i]["ybh"].ToString() + "'");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 取得Guid
        /// </summary>
        /// <returns></returns>
        private bool getNewGuid(string jkdah,string jyrq,out string guid)
        {
            guid = System.Guid.NewGuid().ToString();
            DBAccess dBAccess = new DBAccess();
            string sql = "";
            ArrayList sqlList = new ArrayList();
            sql = "select guid from T_JK_JKTJ where  d_grdabh='{d_grdabh}' and happentime='{happentime}'";

            //健康档案编号
            sql = sql.Replace("{d_grdabh}", jkdah);

            //体检日期
            sql = sql.Replace("{happentime}", Convert.ToDateTime(jyrq).ToString("yyyy-MM-dd"));

            DataTable dt = dBAccess.ExecuteQueryBySql(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                guid = dt.Rows[0]["guid"].ToString();
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 体检状态表(T_JK_TJZT）保存
        /// </summary>
        public ArrayList save_T_JK_TJZT(string jkdah,string sfzh,string xm,string tjsj, string TJTYPE, string zt)
        {
            ArrayList returnArrayList = new ArrayList();

            //删除用sql
            string SqlDele = Common.getSql("sql801", " ");

            //追加记录用sql
            string SqlAdd = Common.getSql("sql802", "");

            //删除用sql
            //医疗机构编码
            SqlDele = SqlDele.Replace("{YLJGBM}", UserInfo.Yybm);
            //健康档案号
            SqlDele = SqlDele.Replace("{JKDAH}", jkdah);
            //年度
            SqlDele = SqlDele.Replace("{ND}", Convert.ToDateTime(tjsj).Year.ToString());
            //工作组编码
            SqlDele = SqlDele.Replace("{GZZBM}", UserInfo.gzz);
            //体检时间
            SqlDele = SqlDele.Replace("{TJSJ}", tjsj);
            //操作员
            SqlDele = SqlDele.Replace("{CZY}", UserInfo.userId);
            //文档类型
            SqlDele = SqlDele.Replace("{TJTYPE}", TJTYPE);

            returnArrayList.Add(SqlDele);

            //追加用sql  ( '{YLJGBM}','{JKDAH}','{XM}','{SFZH}','{ND}','{GZZBM}','{TJSJ}','{CZY}','{TJTYPE}','{ZT}')
            //医疗机构编码
            SqlAdd = SqlAdd.Replace("{YLJGBM}", UserInfo.Yybm);
            //健康档案号
            SqlAdd = SqlAdd.Replace("{JKDAH}", jkdah);
            //姓名
            SqlAdd = SqlAdd.Replace("{XM}", xm);
            //身份证号
            SqlAdd = SqlAdd.Replace("{SFZH}", sfzh);
            //年度
            SqlAdd = SqlAdd.Replace("{ND}", Convert.ToDateTime(tjsj).Year.ToString());
            //工作组编码
            SqlAdd = SqlAdd.Replace("{GZZBM}", UserInfo.gzz);
            //体检时间
            SqlAdd = SqlAdd.Replace("{TJSJ}", tjsj);
            //操作员
            SqlAdd = SqlAdd.Replace("{CZY}", UserInfo.userId);
            //文档类型
            SqlAdd = SqlAdd.Replace("{TJTYPE}", TJTYPE);
            //体检状态
            SqlAdd = SqlAdd.Replace("{ZT}", zt);

            //体检医生
            SqlAdd = SqlAdd.Replace("{Tjys}", "");
            returnArrayList.Add(SqlAdd);
            return returnArrayList;
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
                this.notifyIcon1.ShowBalloonTip(30, "提示", "心电（Medex）同步处理", ToolTipIcon.Info);
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

        /// <summary>
        /// 手动开始处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_bcstart_Click(object sender, EventArgs e)
        {

            try
            {
                //时间控件失效
                timer_lis.Enabled = false;

                this.btn_bcstart.Enabled = false;
                this.btn_bcstop.Enabled = true;

                this.label_title.Text = "心电结果处理中。。。。";
                this.label_title.Refresh();

                //数据处理
                dataRecive();

                this.label_title.Text = "心电结果处理结束";
                this.label_title.Refresh();

                this.btn_bcstart.Enabled = true;
                this.btn_bcstop.Enabled = false;

                //时间控件开启
                timer_lis.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 结束处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_bcstop_Click(object sender, EventArgs e)
        {
            try
            {
                //时间控件失效
                timer_lis.Enabled = false;
                this.label_title.Text = "心电结果处理结束";
                this.label_title.Refresh();

                this.btn_bcstart.Enabled = true;
                this.btn_bcstop.Enabled = false;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 重新新处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_again_Click(object sender, EventArgs e)
        {
            try
            {
                //时间控件失效
                timer_lis.Enabled = false;

                this.btn_bcstart.Enabled = false;
                this.btn_bcstop.Enabled = true;

                this.label_title.Text = "心电结果处理中。。。。（再次）";
                this.label_title.Refresh();

                //是否重新处理 1：重新获取  0：不重新获取
               // string again = "1";

                //数据处理
                dataRecive_again();

                this.label_title.Text = "心电结果处理结束（再次）";
                this.label_title.Refresh();

                this.btn_bcstart.Enabled = true;
                this.btn_bcstop.Enabled = false;

                //时间控件开启
                timer_lis.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        public DataTable dataRecive_again()
        {
            DataTable dt = null;
            string errMsg = "";
            try
            {
                string paratime = "";

                //开始时间
                if (dateTimePicker_start.Checked == true)
                {
                    paratime = paratime + dateTimePicker_start.Value.ToString("yyyy-MM-dd") + " 00:00:00" + "|";
                }

                //结束时间
                if (dateTimePicker_end.Checked == true)
                {
                    if (paratime.Length > 0)
                    {
                        paratime = paratime + dateTimePicker_end.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    }
                    else
                    {
                        paratime = paratime + "|" + dateTimePicker_end.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                    }
                }

                if (paratime.Length == 0)
                {
                    paratime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "|";
                }

                //0：获取新数据  1:获取所有数据
                paratime = paratime + "|" + "1";

                if (yqDemo != null)
                {
                    if (yqDemo.IsOpen(out errMsg) == false)
                    {
                        yqDemo.open(out errMsg);
                    }
                    dt = yqDemo.YQDataReturn(paratime, out errMsg);
                }
                else
                {
                    if (yqxh.Trim().Length == 0)
                    {
                        timer_lis.Enabled = false;

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

                    dt = yqDemo.YQDataReturn(paratime, out errMsg);
                }

                //将取得的结果保存到数据库中
                if (dt != null && dt.Rows .Count >0)
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

                    //再次处理

                   //将获得的结果匹配体检表中的心电图的结果
                   dt=dt_pp(dt);

                    DataTable dt_again = dt.Clone();

                    DataRow[] dtrow_again = dt.Select(" qf='1'");
                    if (dtrow_again.Length > 0)
                    {
                        for (int i = 0; i < dtrow_again.Length; i++)
                        {
                            dt_again.ImportRow(dtrow_again[i]);
                        }
                    }

                    //新数据
                    DataTable dt_new = dt.Clone(); 
                    DataRow[] dtrow_new = dt.Select(" qf='2'");
                    if (dtrow_new.Length > 0)
                    {
                        for (int i = 0; i < dtrow_new.Length; i++)
                        {
                            dt_new.ImportRow(dtrow_new[i]);
                            //置变量，以后不再读取此记录
                            MedExSqlHelper DBF = new MedExSqlHelper();
                            DBF.ExecuteNonQuery("update PACS_APPLY set work_status = 1 where PATIENT_ID = '" + dtrow_new[i]["ybh"].ToString() + "'");
                        }
                    }

                    Form_lisBll form_lisbll = new Form_lisBll();

                    if (dt_new != null && dt_new.Rows.Count > 0)
                    {
                        form_lisbll.Add(dt, "sql042");
                        string xmlPath = Common.getyqPath("MECG200");
                        string YQ_YQtype = XmlRW.GetValueFormXML(xmlPath, "YQ_YQType", "value");
                        button_save_sys(dt, YQ_YQtype);
                    }

                    if (dt_again != null && dt_again.Rows.Count > 0)
                    {
                        //更新数据
                        form_lisbll.Upd_all(dt_again, "sql042_update");
                        string xmlPath = Common.getyqPath("MECG200");
                        string YQ_YQtype = XmlRW.GetValueFormXML(xmlPath, "YQ_YQType", "value");
                        
                        //更新数据
                        button_save_sys_again(dt_again, YQ_YQtype);
                    }
                }
            }
            catch (Exception ex)
            {
                timer_lis.Enabled = false;
                Jktj_lis.msgShow(ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// 再次处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_sys_again(DataTable dt_all, string sqlcode)
        {
            try
            {
                if (dt_all.Rows.Count > 0)
                {
                    DataRow[] drs = dt_all.Select("xmdh='NAME'");
                    for (int i = 0; i < drs.Length; i++)
                    {
                        DBAccess dbjkdah = new DBAccess();

                        //获取以对应的人的信息   不一定与心电工作站中的条码号对应
                        DataTable dt_access_no = dbjkdah.ExecuteQueryBySql(@"select testno from T_JK_lis_result_re where ybh='" + drs[i]["ybh"].ToString() + "'");
                        if (dt_access_no == null || dt_access_no.Rows.Count <= 0) continue;

                        //获取以对应的人员的信息
                        DataTable dt_access = dbjkdah.ExecuteQueryBySql(@"select jkdah,sfzh from T_JK_TJRY_TXM where (txmbh='" + dt_access_no.Rows[0]["testno"].ToString() 
                            + "' or jkdah ='" + dt_access_no.Rows[0]["testno"].ToString() + "') and yljgbm='" + drs[i]["yybm"].ToString() + "'");
                        //没有对应的人员信息时直接退出
                        if (dt_access == null || dt_access.Rows.Count <= 0) continue;
                        ////更新化验结果表(T_JK_lis_result_re），确定化验结果与人员关系
                        //DataTable dt_update = new DataTable();
                        ////医院编码
                        //dt_update.Columns.Add("yybm");
                        ////仪器编码
                        //dt_update.Columns.Add("yq");
                        ////样本号
                        //dt_update.Columns.Add("ybh");
                        ////体检日期
                        //dt_update.Columns.Add("jyrq");
                        ////健康档案号
                        //dt_update.Columns.Add("testno");
                        ////年度
                        //dt_update.Columns.Add("nd");

                        //dt_update.Rows.Add();
                        //dt_update.AcceptChanges();

                        //dt_update.Rows[0]["yybm"] = UserInfo.Yybm;
                        //dt_update.Rows[0]["yq"] = "MECG200";
                        //dt_update.Rows[0]["ybh"] = drs[i]["ybh"].ToString();
                        //dt_update.Rows[0]["jyrq"] = Convert.ToDateTime(drs[i]["jyrq"].ToString()).ToString("yyyy-MM-dd");
                        //dt_update.Rows[0]["testno"] = dt_access.Rows[0]["jkdah"].ToString();
                        ////dt_update.Rows[0]["nd"] = Convert.ToDateTime(jyrq_tem).Year .ToString();
                        //dt_update.Rows[0]["nd"] = DateTime.Now.Year.ToString();

                        Form_lisBll form_lisbll = new Form_lisBll();

                        ////标本与人员的对应关系保存
                        //form_lisbll.Upd(dt_update, "sql_lis_result_ryxx_update");

                        //更新体检人员状态表
                        //form_lisbll.Upd(dt_update, "sql077");
                        //体检状态信息
                        ArrayList TJRYXXList = save_T_JK_TJZT(dt_access.Rows[0]["jkdah"].ToString(), dt_access.Rows[0]["sfzh"].ToString(), drs[i]["xmmc"].ToString(), drs[i]["jyrq"].ToString(), Common.TJTYPE.健康体检表, Common.ZT.确定状态);
                        if (TJRYXXList != null && TJRYXXList.Count > 0)
                        {
                            DBAccess dbaccess = new DBAccess();
                            dbaccess.ExecuteNonQueryBySql(TJRYXXList);
                        }

                        //保存体检结果到体检结果表
                        string sqlWhere = "";
                        //医院编码
                        if (UserInfo.Yybm.Length > 0)
                        {
                            sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.yybm='{0}'", UserInfo.Yybm);
                        }
                        //仪器
                        if (UserInfo.Yybm.Length > 0)
                        {
                            sqlWhere = sqlWhere + string.Format(" and  T_JK_lis_result_re.yq='{0}'", drs[i]["yq"].ToString());
                        }
                        //检验日期
                        if (UserInfo.Yybm.Length > 0)
                        {
                            sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.jyrq='{0}'", drs[i]["jyrq"].ToString());
                        }
                        //样本号
                        if (UserInfo.Yybm.Length > 0)
                        {
                            sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.ybh='{0}'", drs[i]["ybh"].ToString());
                        }

                        //取得化验结果
                        DataTable dt_dyxm = form_lisbll.GetMoHuList(sqlWhere, "sql045");

                        //更新健康体检表
                        DataTable dt_tjjgUpdate = new DataTable();
                        dt_tjjgUpdate.Rows.Add();

                        for (int j = 0; j < dt_dyxm.Rows.Count; j++)
                        {

                            if (dt_tjjgUpdate.Columns.Contains(dt_dyxm.Rows[j]["HIS_DB"].ToString()) == false)
                            {
                                DataColumn dtColumn = new DataColumn(dt_dyxm.Rows[j]["HIS_DB"].ToString());
                                dtColumn.DefaultValue = dt_dyxm.Rows[j]["result"].ToString();
                                dt_tjjgUpdate.Columns.Add(dtColumn);
                            }

                        }

                        if (dt_tjjgUpdate.Columns.Contains("D_GRDABH") == false)
                        {
                            DataColumn dtColumn = new DataColumn("D_GRDABH");
                            dtColumn.DefaultValue = dt_access.Rows[0]["jkdah"].ToString();
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }

                        if (dt_tjjgUpdate.Columns.Contains("HAPPENTIME") == false)
                        {
                            DataColumn dtColumn = new DataColumn("HAPPENTIME");
                            dtColumn.DefaultValue = Convert.ToDateTime(drs[i]["jyrq"].ToString()).ToString("yyyy-MM-dd");
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }

                        if (dt_tjjgUpdate.Columns.Contains("czy") == false)
                        {
                            DataColumn dtColumn = new DataColumn("czy");
                            dtColumn.DefaultValue = UserInfo.userId;
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }

                        if (dt_tjjgUpdate.Columns.Contains("gzz") == false)
                        {
                            DataColumn dtColumn = new DataColumn("gzz");
                            dtColumn.DefaultValue = UserInfo.gzz;
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }


                        //体检结果是否已经存在
                        string Guid = "";
                        //true:新的Guid  false:已经存在的Guid
                        bool GuidResult = true;
                        GuidResult = getNewGuid(dt_access.Rows[0]["jkdah"].ToString(), drs[i]["jyrq"].ToString(), out Guid);

                        if (dt_tjjgUpdate.Columns.Contains("guid") == false)
                        {
                            DataColumn dtColumn = new DataColumn("guid");
                            dtColumn.DefaultValue = Guid;
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }


                        string[] sqllist = sqlcode.Split(new char[] { '|' });

                        if (sqlcode.Length == 0 || sqllist.Length < 2)
                        {
                            MessageBox.Show("仪器sql设定错误！");
                            return;
                        }
                        //体检结果
                        if (GuidResult == true)
                        {
                            //体检结果插入
                            form_lisbll.Add(dt_tjjgUpdate, sqllist[0]);
                        }
                        else
                        {
                            //体检结果更新
                            dt_tjjgUpdate.AcceptChanges();
                            dt_tjjgUpdate.Rows[0]["guid"] = Guid;
                            form_lisbll.Upd(dt_tjjgUpdate, sqllist[1]);
                        }

                        //签名
                        SaveJktjSignname(drs[i]["jyrq"].ToString(), dt_access.Rows[0]["jkdah"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 按照维护的基础数据匹配心电图的结果
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable dt_pp(DataTable dt)
        {
            //将数据进行排序
            //DataView dtview_pp = dt.DefaultView;
            //dtview_pp.Sort = " ybh,xmdh ";
            //dt = dtview_pp.ToTable();

            DataRow[] dtRows = dt.Select(" xmdh='CONCLUSION_QT'");
            if (dtRows.Length > 0)
            {

                DBAccess  dbaccess=new DBAccess ();
                DataTable dt_pp_base = dbaccess.ExecuteQueryBySql(string.Format("select * from t_jk_xdtjgppb where  yljgbm is null or yljgbm='' or yljgbm ='{0}' order by value ", UserInfo.Yybm));

                for (int i = 0; i < dtRows.Length; i++)
                {

                    DataRow[] dtrow_qt = dt.Select(string.Format(" xmdh='TJPPZT' and ybh='{0}' and jyrq='{1}' ", dtRows[i]["ybh"].ToString(), dtRows[i]["jyrq"].ToString()));
                    if (dtrow_qt != null && dtrow_qt.Length > 0)
                    {
                        if (dt_pp_base != null && dt_pp_base.Rows.Count > 0)
                        {
                            string CONCLUSION_QT = dtRows[i]["result"].ToString();
                            string tjztqt = "";
                            for (int j = 0; j < dt_pp_base.Rows.Count; j++)
                            {
                                if (CONCLUSION_QT.IndexOf(dt_pp_base.Rows[j]["text"].ToString()) > -1)
                                {
                                    CONCLUSION_QT = CONCLUSION_QT.Replace(dt_pp_base.Rows[j]["text"].ToString(), "");
                                    string strvalue_tem = dt_pp_base.Rows[j]["value"].ToString();

                                    if (strvalue_tem.IndexOf((tjztqt + ",")) == -1)
                                    {
                                        tjztqt = tjztqt + "," + strvalue_tem;
                                    }
                                    
                                }
                            }

                            if (tjztqt.Replace(",1", "").Length > 0)
                            {
                                tjztqt = tjztqt.Replace(",1", "");
                            }

                            if (CONCLUSION_QT.Trim().Length > 0)
                            {
                                tjztqt = tjztqt + "," + "99";
                            }

                            dtRows[i]["result"] = CONCLUSION_QT;
                            dtrow_qt[0]["result"] = tjztqt.Length > 1 ? tjztqt.Substring(1) : tjztqt;

                        }
                    }
                }
            }
            return dt;
        }
    }
}
