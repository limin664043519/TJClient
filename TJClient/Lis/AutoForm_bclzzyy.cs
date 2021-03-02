using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Logger;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LIS;
using TJClient.Common;
using TJClient.Helper;
using TJClient.Lis;
using TJClient.Lis.Model;
using TJClient.NeedToUseForm;
using TJClient.sys.Bll;

namespace FBYClient
{
    public partial class AutoForm_bclzzyy : sysAutoForm
    {
        public string yqxh = "";
        public lis_new Jktj_lis = null;
        public static IInterface yqDemo = null;
        public SimpleLogger logger = SimpleLogger.GetInstance();
        private int _interval = 60000;
        private bool _needToOperation = false;
        private LogHelper _logHelper = null;
        public AutoForm_bclzzyy()
        {
            InitializeComponent();
        }

        // <summary>
        /// 设定仪器的型号
        /// </summary>
        /// <param name="yqxh"></param>
        /// <returns></returns>
        public override bool setStart(string yqxhPara)
        {

            string YqXmlPath = "";
            string YQ_Interval = "";
            Jktj_lis = new lis_new();
            //xmlpath
            YqXmlPath = Common.getyqPath(yqxhPara);
            //jg数据处理间隔时间
            YQ_Interval = XmlRW.GetValueFormXML(YqXmlPath, "YQ_Interval", "value");


            yqxh = yqxhPara;
            if (yqxh.Length > 0)
            {
                if (YQ_Interval != "0")
                {
                    _interval = int.Parse(YQ_Interval) * 1000;
                }

                _needToOperation = true;
            }
            return true;
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

        private void Operation(OpInfo info)
        {
            Task t = new Task(() =>
            {
                try
                {
                    _logHelper = new LogHelper(string.Format("{0}_recieve", yqxh), true);//在开始之前初始话
                    _logHelper.AppendMessageToTxt(string.Format("B超开始处理,仪器型号为:{0},获取时间间隔为:{1},当前时间为:{2}", yqxh, _interval,
                        DateHelper.CurrDateTime()));
                    //改变按钮文字状态为执行
                    ButtonAndTextboxStatus(false, "B超结果处理中......");
                    while (_needToOperation)
                    {
                        Thread.Sleep(_interval);
                        dataRecive(info);
                    }
                    //改变按钮文字状态为结束
                    ButtonAndTextboxStatus(true, "B超结果处理结束");
                    _logHelper.AppendMessageToTxt(string.Format("{0}:B超结果处理结束", DateHelper.CurrDateTime()));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            t.Start();
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

        private void AutoForm_bclzzyy_Load(object sender, EventArgs e)
        {
            Jktj_lis = (lis_new)this.Owner;
            //初始化签名基础信息
            SignNameGroupInit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
        }

        private void CloseTheForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string GetBeginTime(string date)
        {
            return date + " 00:00:00";
        }

        private string GetEndTime(string date)
        {
            return date + " 23:59:59";
        }
        private string GetParams(OpInfo info)
        {
            string paratime = "";
            return string.Format("{0}@{1}@{2}", GetBeginTime(info.Begin), GetEndTime(info.End), info.OpType);
        }
        private string GetYqType()
        {
            string xmlPath = Common.getyqPath(yqxh);
            XmlRW BC = new XmlRW();
            string YQ_YQtype = BC.GetValueFormXML(xmlPath, "YQ_YQType");
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
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTableSecondaryTreatment(ref dt);
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

        private DataTable GetDataTable(string paratime, out string errMsg)
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
            }
            else
            {
                if (yqxh.Trim().Length == 0)
                {
                    return dt;
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

            return dt;
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

        private DataTable GetDtAccess(DataRow drs)
        {
            DBAccess dbjkdah = new DBAccess();
            string sql = string.Format("select txmbh,jkdah,sfzh from t_jk_tjry_txm where txmbh='{0}' and yljgbm='{1}'",drs["ybh"],drs["yybm"]);
            return
                dbjkdah.ExecuteQueryBySql(sql);

        }

        private bool TjZtOperation(DataTable dtAll, DataRow drs, string jkdah, string sfzh)
        {
            Form_lisBll form_lisbll = new Form_lisBll();
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

        /// <summary>
        /// 其他通用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_sys(DataTable dt_all, string sqlcode)
        {
            string errsql = "";

            try
            {
                if (dt_all.Rows.Count > 0)
                {
                    DataRow[] drs = dt_all.Select("xmdh='PTNNM'");
                    _logHelper.AppendMessageToTxt(string.Format("获取到新数据，共需要处理{0}条数据", drs.Length));
                    for (int i = 0; i < drs.Length; i++)
                    {
                        string testno = "";
                        DataTable dt_access = GetDtAccess(drs[i]);
                        if (dt_access == null || dt_access.Rows.Count <= 0)
                        {
                            //如果未识别到，才进行插入操作，在插入之前，先删除，保证只取一次
                            LisResultReOperation(dt_all.Select(string.Format("ybh='{0}' and jyrq='{1}' and yq='{2}' and yybm='{3}'", drs[i]["ybh"], drs[i]["jyrq"],
                                drs[i]["yq"], drs[i]["yybm"])).CopyToDataTable());
                            _logHelper.AppendMessageToTxt(string.Format("无此样本号:{0},已将此信息插入到T_JK_lis_result_re表中", drs[i]["ybh"]));
                            continue;
                        }

                        TjZtOperation(dt_all, drs[i], dt_access.Rows[0]["jkdah"].ToString(),
                            dt_access.Rows[0]["sfzh"].ToString());

                        //取得化验结果
                        DataTable dt_dyxm = GetHyxm(dt_all, drs[i]);


                        TjjgOperation(dt_dyxm, drs[i], dt_access, sqlcode);

                        //签名
                        SaveJktjSignname(drs[i]["jyrq"].ToString(), dt_access.Rows[0]["jkdah"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "-----" + ex.StackTrace + "------sql:" + errsql);
            }
        }

        private bool DeleteFromLisResultRe(string ybh, string jyrq, string yq, string yybm)
        {
            //todo  这里可能有问题，要根据实际情况改动
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
        

        private void UpdateLisResultRe(string ybh, string jyrq, string testno,ref string errsql)
        {
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
            dt_update.Rows[0]["yq"] = "BCLZZYY";
            dt_update.Rows[0]["ybh"] = ybh;
            dt_update.Rows[0]["jyrq"] = Convert.ToDateTime(jyrq).ToString("yyyy-MM-dd");
            dt_update.Rows[0]["testno"] = testno;//dt_access.Rows[0]["jkdah"].ToString();
            //dt_update.Rows[0]["nd"] = Convert.ToDateTime(jyrq_tem).Year .ToString();
            dt_update.Rows[0]["nd"] = DateTime.Now.Year.ToString();
            Form_lisBll form_lisbll = new Form_lisBll();
            errsql = "sql_lis_result_ryxx_update";
            //标本与人员的对应关系保存
            form_lisbll.Upd(dt_update, "sql_lis_result_ryxx_update");
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

        private DataTable GetHyxm(DataTable dtAll, DataRow drs)
        {

            //todo 这里也要根据实际情况改动
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

        

        private DataTable GetTjjgUpdateDateTable(DataTable dt_dyxm, string jyrq, DataTable dt_access, ref string errsql, ref string Guid, ref bool GuidResult)
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
            errsql = "guid:" + Guid;
            if (dt_tjjgUpdate.Columns.Contains("guid") == false)
            {
                DataColumn dtColumn = new DataColumn("guid");
                dtColumn.DefaultValue = Guid;
                dt_tjjgUpdate.Columns.Add(dtColumn);
            }
            return dt_tjjgUpdate;
        }

        /// <summary>
        /// 取得Guid
        /// </summary>
        /// <returns></returns>
        private bool getNewGuid(string jkdah, string jyrq, out string guid)
        {
            guid = Guid.NewGuid().ToString();
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
