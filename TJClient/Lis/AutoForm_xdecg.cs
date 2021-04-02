using ECGNETV260;
using FBYClient;
using LIS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Common;
using TJClient.sys.Bll;
using System.Logger;
using System.Threading;
using System.Threading.Tasks;
using TJClient.Helper;
using TJClient.Lis.Model;
using TJClient.NeedToUseForm;

namespace FBYClient
{
    public partial class AutoForm_xdecg : sysAutoForm
    {
        public lis_new Jktj_lis = null;
        //log对象
        public SimpleLogger logger = SimpleLogger.GetInstance();
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

        private int _interval = 5000;

        private bool _needToOperation = false;

        private LogHelper _logHelper = null;
        public AutoForm_xdecg()
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
            YQ_Interval = ECGNETV260.XmlRW.GetValueFormXML(YqXmlPath, "YQ_Interval");

            //rqlx	标本日期类型
            YQ_DateType = ECGNETV260.XmlRW.GetValueFormXML(YqXmlPath, "YQ_DateType");

            yqxh = yqxhPara;
            if (yqxh.Length > 0)
            {
                if (YQ_Interval != "0")
                {
                    _interval = int.Parse(YQ_Interval) * 1000;
                }

                _needToOperation = true;
            }
            Operation(new OpInfo(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), "0"));
            return true;
        }

        private void Operation(OpInfo info)
        {
            Task t = new Task(() =>
            {
                try
                {
                    _logHelper = new LogHelper(string.Format("{0}_recieve", yqxh), true);//在开始之前初始话
                    _logHelper.AppendMessageToTxt(string.Format("心电开始处理,仪器型号为:{0},获取时间间隔为:{1},当前时间为:{2}", yqxh, _interval,
                        DateHelper.CurrDateTime()));
                    //改变按钮文字状态为执行
                    ButtonAndTextboxStatus(false, "心电结果处理中......");
                    while (_needToOperation)
                    {
                        Thread.Sleep(_interval);
                        dataRecive(info);
                    }
                    //改变按钮文字状态为结束
                    ButtonAndTextboxStatus(true, "心电结果处理结束");
                    _logHelper.AppendMessageToTxt(string.Format("{0}:心电结果处理结束", DateHelper.CurrDateTime()));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            t.Start();
        }

        private bool ButtonAndTextboxStatus(bool enabled, string message)
        {
            if (btn_bcstart.InvokeRequired)
            {
                btn_bcstart.Invoke(new Action(() => { btn_bcstart.Enabled = enabled; }));
                button_again.Invoke(new Action(() => { button_again.Enabled = enabled; }));
            }
            else
            {
                btn_bcstart.Enabled = enabled;
                button_again.Enabled = enabled;
            }

            if (btn_bcstop.InvokeRequired)
            {
                btn_bcstop.Invoke(new Action(() => { btn_bcstop.Enabled = !enabled; }));
            }
            else
            {
                btn_bcstart.Enabled = !enabled;
            }

            if (label_title.InvokeRequired)
            {
                label_title.Invoke(new Action(() => { label_title.Text = message; }));
            }
            else
            {
                label_title.Text = message;
            }
            return true;
        }

        private string GetBeginTime(string date)
        {
            return date + " 00:00:00";
        }

        private string GetEndTime(string date)
        {
            return date+" 23:59:59";
        }

        private string GetParams(OpInfo info)
        {
            return string.Format("{0}|{1}|{2}", GetBeginTime(info.Begin), GetEndTime(info.End), info.OpType);
        }

        private DataTable GetDataTable(string paratime,out string errMsg)
        {
            DataTable dt = null;
            errMsg = "";
            if (yqDemo != null)
            {
                if (yqDemo.IsOpen(out errMsg) == false)
                {
                    yqDemo.open(out errMsg);
                }
                dt = yqDemo.YQDataReturn(paratime, out errMsg);
                return dt;
            }
            else
            {
                if (yqxh.Trim().Length == 0)
                {
                    return dt;
                }
                if (yqDemo == null)
                {
                    string yqVersion = ECGNETV260.XmlRW.GetValueFormXML(Common.getyqPath(yqxh), "YQ_Version");
                    yqDemo = LisFactory.LisCreate(yqVersion);
                }

                if (yqDemo.IsOpen(out errMsg) == false)
                {
                    yqDemo.open(out errMsg);
                }
                dt = yqDemo.YQDataReturn(paratime, out errMsg);
                return dt;
            }
        }

        private void DataTableSecondaryTreatment(ref DataTable dt)
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
                dtcolumn.DefaultValue = yqxh;
                dt.Columns.Add(dtcolumn);
            }
        }

        private string GetYqType()
        {
            string xmlPath = TJClient.Common.Common.getyqPath(yqxh);
            TJClient.Common.XmlRW rw = new TJClient.Common.XmlRW();
            string YQ_YQtype = rw.GetValueFormXML(xmlPath, "YQ_YQType");
            return YQ_YQtype;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        public DataTable dataRecive(OpInfo info)
        {
            DataTable dt = null;
            string errMsg = "";
            try
            {
                string paratime = GetParams(info);
                _logHelper.AppendMessageToTxt(string.Format("时间范围为:{0}", paratime));
                dt = GetDataTable(paratime, out errMsg);

                //将取得的结果保存到数据库中
                if (dt != null && dt.Rows .Count >0)
                {
                    DataTableSecondaryTreatment(ref dt);

                    //LisResultReOperation(dt);
                    dt_pp(dt);
                    //数据保存到T_JK_lis_result_re
                    //Upd_all(dt);
                    button_save_sys(dt, GetYqType());
                }
                else
                {
                    _logHelper.AppendMessageToTxt(string.Format("{0}:数据不存在", DateHelper.CurrDateTime()));
                }
                if (info.OpType != "0")
                {
                    _needToOperation = false;
                }

            }
            catch (Exception ex)
            {
                Jktj_lis.msgShow(ex.Message);
            }
            return dt;
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

            DataRow[] dtRows = dt.Select(" xmdh='XDT-RESULT-INFO'");
            if (dtRows.Length > 0)
            {

                DBAccess dbaccess = new DBAccess();
                DataTable dt_pp_base = dbaccess.ExecuteQueryBySql(string.Format("select * from t_jk_xdtjgppb where  yljgbm is null or yljgbm='' or yljgbm ='{0}' order by value ", UserInfo.Yybm));

                for (int i = 0; i < dtRows.Length; i++)
                {

                    DataRow[] dtrow_qt = dt.Select(string.Format(" xmdh='XDT-ZT' and ybh='{0}' and jyrq='{1}' ", dtRows[i]["ybh"].ToString(), dtRows[i]["jyrq"].ToString()));
                    if (dtrow_qt != null && dtrow_qt.Length > 0)
                    {
                        if (dt_pp_base != null && dt_pp_base.Rows.Count > 0)
                        {
                            string CONCLUSION_QT = dtRows[i]["result"].ToString().Replace("1.","").Replace("2.", "").Replace("3.", "").Replace("4.", "").Replace("5.", "").Replace("6.", "").Replace("7.", "").Replace("8.", "").Replace("9.", "").Replace("10.", "").Replace("11.", "").Replace("12.", "").Replace("13.", "").Replace("14.", "").Replace("15.", "");
                            string tjztqt = "";
                            for (int j = 0; j < dt_pp_base.Rows.Count; j++)
                            {
                                if (CONCLUSION_QT.IndexOf(dt_pp_base.Rows[j]["text"].ToString()) > -1)
                                {
                                    CONCLUSION_QT = CONCLUSION_QT.Replace(dt_pp_base.Rows[j]["text"].ToString() + ".", "").Replace("." + dt_pp_base.Rows[j]["text"].ToString(), "");
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
                            else {
                                tjztqt = ",1";
                            }

                            dtRows[i]["result"] = CONCLUSION_QT;
                            dtrow_qt[0]["result"] = tjztqt.Length > 1 ? tjztqt.Substring(1) : tjztqt;

                        }
                    }
                }
            }
            return dt;
        }

        private DataTable GetDtAccess(string ybh, string yljgbm, string sblx)
        {
            DBAccess dbjkdah = new DBAccess();
            if (sblx == "1") //条码
            {
                //获取以对应的人员的信息

                return dbjkdah.ExecuteQueryBySql(@"select jkdah,sfzh from T_JK_TJRY_TXM where txmbh='" + ybh + "' and yljgbm='" + yljgbm + "'");
            }
            else
            {
                return
                    dbjkdah.ExecuteQueryBySql(@"select jkdah,sfzh from t_jk_tjryxx where sfzh='" + ybh + "' and yljgbm='" + yljgbm + "'");
            }

        }

        private DataTable GetHyxm(DataTable dtAll, DataRow drs)
        {
            string sql = string.Format("select b.HIS_DB,b.JKDA_DB,b.KJID,b.XMMC,a.xmbm_lis from T_JK_LIS_XM a,t_jk_tjxm b where a.xmbm=b.xmbm and yljgbm='{0}'", UserInfo.Yybm);
            DBAccess access = new DBAccess();
            DataTable dt = access.ExecuteQueryBySql(sql);
            string where = string.Format("ybh='{0}' and jyrq='{1}' and yq='{2}' and yybm='{3}'", drs["ybh"],
                drs["jyrq"], drs["yq"], drs["yybm"]);
            DataTable reDt = dtAll.Select(where).CopyToDataTable();


            var results = from a in reDt.AsEnumerable()
                join b in dt.AsEnumerable() on a.Field<string>("xmdh") equals b.Field<String>("XMBM_LIS")
                select new
                {
                    yybm = a["yybm"],
                    yq = a["yq"],
                    jyrq = a["jyrq"],
                    ybh = a["ybh"],
                    xmdh = a["xmdh"],
                    xmmc = a["xmmc"],
                    result = a["result"],
                    //result1 = a["result1"],
                    lowerValue = a["lowerValue"],
                    heightValue = a["heightValue"],
                    HIS_DB = b["HIS_DB"],
                    JKDA_DB = b["JKDA_DB"],
                    KJID = b["KJID"],
                    XMMC = b["XMMC"]
                };
            return LinQHelper.LINQResultToDataTable(results);
        }


        private DataTable GetTjjgUpdateDateTable(DataTable dt_dyxm, string jyrq, DataTable dt_access, ref string Guid, ref bool GuidResult)
        {
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

            if (dt_tjjgUpdate.Columns.Contains("G_MB") == false)
            {
                DataColumn dtColumn = new DataColumn("G_MB");
                dtColumn.DefaultValue = "";
                dt_tjjgUpdate.Columns.Add(dtColumn);
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
                dtColumn.DefaultValue = Convert.ToDateTime(jyrq).ToString("yyyy-MM-dd");
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
            //true:新的Guid  false:已经存在的Guid
            GuidResult = getNewGuid(dt_access.Rows[0]["jkdah"].ToString(), jyrq, out Guid);
            if (dt_tjjgUpdate.Columns.Contains("guid") == false)
            {
                DataColumn dtColumn = new DataColumn("guid");
                dtColumn.DefaultValue = Guid;
                dt_tjjgUpdate.Columns.Add(dtColumn);
            }
            return dt_tjjgUpdate;
        }

        private bool DeleteFromLisResultRe(string ybh, string jyrq, string yq, string yybm)
        {
            DBAccess db = new DBAccess();
            string sql =
                string.Format(
                    "delete from T_JK_lis_result_re where ybh='{0}' and jyrq='{1}' and yq='{2}' and yybm='{3}'", ybh, jyrq, yq, yybm);
            db.ExecuteNonQueryBySql(sql);
            return true;
        }

        private void LisResultReOperation(DataTable dt) //用来判断进行插入修改操作
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                //先删除，后插入
                DeleteFromLisResultRe(dt.Rows[0]["ybh"].ToString(), dt.Rows[0]["jyrq"].ToString(),
                    dt.Rows[0]["yq"].ToString(), dt.Rows[0]["yybm"].ToString());

                Form_lisBll form_lisbll = new Form_lisBll();
                form_lisbll.Add(dt, "sql042", true);

            }
        }

        private bool TjZtOperation(DataTable dtAll, DataRow drs, string jkdah, string sfzh)
        {
            //查询体检状态
            string tjzt = "1";
            DataRow[] dr_zt = dtAll.Select("xmdh='TJZT' and ybh='" + drs["ybh"].ToString() + "'");
            if (dr_zt != null && dr_zt.Length > 0)
                tjzt = dr_zt[0]["result"].ToString();
            //体检状态信息
            //errsql = "save_T_JK_TJZT " + tjzt.ToString();
            ArrayList TJRYXXList = save_T_JK_TJZT(jkdah, sfzh, drs["result"].ToString(),
                drs["jyrq"].ToString(), TJClient.Common.Common.TJTYPE.健康体检表, TJClient.Common.Common.ZT.确定状态);
            if (TJRYXXList != null && TJRYXXList.Count > 0)
            {
                DBAccess dbaccess = new DBAccess();
                dbaccess.ExecuteNonQueryBySql(TJRYXXList);
            }
            return true;
        }

        private bool TjjgOperation(DataTable dtDyxm, DataRow drs, DataTable dtAccess, string sqlCode)
        {
            Form_lisBll form_lisbll = new Form_lisBll();
            //更新健康体检表
            string Guid = "";
            //true:新的Guid  false:已经存在的Guid
            bool GuidResult = true;
            DataTable dt_tjjgUpdate = GetTjjgUpdateDateTable(dtDyxm, drs["jyrq"].ToString(), dtAccess, ref Guid, ref GuidResult);



            string[] sqllist = sqlCode.Split(new char[] { '|' });

            if (sqlCode.Length == 0 || sqllist.Length < 2)
            {
                MessageBox.Show("仪器sql设定错误！");
                return false;
            }
            //体检结果
            if (GuidResult)
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

            return true;
        }

        private void ChangeStatus(DataTable dtAll,DataRow drs)
        {
            //查询体检状态和检验单号
            string jydh = "";
            DataRow[] dr_jydh = dtAll.Select("xmdh='NOTE_NO' and ybh='" + drs["ybh"].ToString() + "'");

            if (dr_jydh != null && dr_jydh.Length > 0)
                jydh = dr_jydh[0]["result"].ToString();

            //获取成功后，更改xd数据库的mstate表
            InsertMState(jydh);
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
                    _logHelper.AppendMessageToTxt(string.Format("获取到新数据，共需要处理{0}条数据", drs.Length));
                    for (int i = 0; i < drs.Length; i++)
                    {
                        DataTable dt_access = GetDtAccess(drs[i]["ybh"].ToString(), drs[i]["yybm"].ToString(), "1");//1为条码
                        if (dt_access == null || dt_access.Rows.Count <= 0)
                        {
                            //如果未识别到，才进行插入操作，在插入之前，先删除，保证只取一次
                            LisResultReOperation(dt_all.Select(string.Format("ybh='{0}' and jyrq='{1}' and yq='{2}' and yybm='{3}'", drs[i]["ybh"], drs[i]["jyrq"],
                                drs[i]["yq"], drs[i]["yybm"])).CopyToDataTable());
                            ChangeStatus(dt_all, drs[i]);
                            _logHelper.AppendMessageToTxt(string.Format("无此样本号:{0},已将此信息插入到T_JK_lis_result_re表中", drs[i]["ybh"]));
                            continue;
                        }
                        TjZtOperation(dt_all, drs[i], dt_access.Rows[0]["jkdah"].ToString(),
                            dt_access.Rows[0]["sfzh"].ToString());


                        DataTable dtHyxm = GetHyxm(dt_all, drs[i]);
                        TjjgOperation(dtHyxm, drs[i], dt_access, sqlcode);


                        ChangeStatus(dt_all, drs[i]);

                        //签名
                        SaveJktjSignname(drs[i]["jyrq"].ToString(), dt_access.Rows[0]["jkdah"].ToString());
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "-----" + ex.StackTrace);
            }

        }

        private void InsertMState(string jydh)
        {
            if (string.IsNullOrEmpty(jydh.Trim()))
            {
                return;
            }
            XDAccess DBF = new XDAccess();
            var dtstate = DBF.ExcuteDataTable("select count(1) from mstate where m_id='" + jydh + "'");

            if (dtstate != null && dtstate.Rows.Count > 0)
                if (Convert.ToInt32(dtstate.Rows[0][0]) <= 0)
                    DBF.ExecuteQueryBySql_RETURNROWS("insert into mstate values('" + jydh + "',1)");
        }

        /// <summary>
        /// 取得Guid
        /// </summary>
        /// <returns></returns>
        private bool getNewGuid(string jkdah, string jyrq, out string guid)
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
        public ArrayList save_T_JK_TJZT(string jkdah, string sfzh, string xm, string tjsj, string TJTYPE, string zt)
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
        
        #endregion                    

        private void AutoForm_xdecg_Load(object sender, EventArgs e)
        {
            Jktj_lis = (lis_new)this.Owner;
            //初始化签名基础信息
            SignNameGroupInit();
        }

        private void btn_bcstart_Click(object sender, EventArgs e)
        {
            OpInfo info = new OpInfo(dateTimePicker_start.Value.ToString("yyyy-MM-dd"), dateTimePicker_end.Value.ToString("yyyy-MM-dd"), "0");

            _needToOperation = true;
            Operation(info);
        }

        private void btn_bcstop_Click(object sender, EventArgs e)
        {
            label_title.Text = "当前数据处理完毕后，便进入结束操作";
            _needToOperation = false;
        }

        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            this.Show();
            //this.Location = formLocal;

            this.WindowState = FormWindowState.Normal;

            this.TopMost = true;
        }
        
        private void button_again_Click(object sender, EventArgs e)
        {
            OpInfo info = new OpInfo(dateTimePicker_start.Value.ToString("yyyy-MM-dd"), dateTimePicker_end.Value.ToString("yyyy-MM-dd"), "1");
            _needToOperation = true;
            Operation(info);
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            FrmRecieveLog._xdType = yqxh;
            FrmRecieveLog frm = new FrmRecieveLog();
            frm.Show();
        }
    }

}
