﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FBYClient;
using TJClient.Common;
using TJClient.sys.Bll;

namespace FBYClient
{
    public partial class AutoForm_Imac120 :sysAutoForm
    {
        public AutoForm_Imac120()
        {
            InitializeComponent();
        }

        public override bool setStart(string yqxhPara)
        {
            return true;
        }

        private string _yqmc = "IMAC120";
        private void BtnClear_Click(object sender, EventArgs e)
        {
            TxtResult.Text = "";
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WriteMessageToTxt(string message)
        {
            TxtResult.Invoke(new Action(() =>
            {
                TxtResult.AppendText(message+Environment.NewLine);
            }));
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
                dtcolumn.DefaultValue = _yqmc;
                dt.Columns.Add(dtcolumn);
            }
        }
        private void Operation()
        {
            try
            {
                LISYQ.IMAC120 xd = new LISYQ.IMAC120();
                string errorMessage = "";
                DataTable dt = xd.YQDataReturn("", out errorMessage);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    WriteMessageToTxt(errorMessage);
                    return;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTableSecondaryTreatment(ref dt);
                    dt_pp(dt);
                    Upd_all(dt);
                }
            }
            catch (Exception ex)
            {
                WriteMessageToTxt(ex.Message);
            }
            
        }
        private void BtnGet_Click(object sender, EventArgs e)
        {
            Task t=new Task(() =>
            {
                WriteMessageToTxt("开始获取心电数据");
                Operation();
                WriteMessageToTxt("处理结束");
            });
            t.Start();
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
                FBYClient.DBAccess dbaccess = new FBYClient.DBAccess();
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

        /// <summary>
        /// 处理数据到数据库中
        /// </summary>
        /// <param name="dt_all"></param>
        /// <returns></returns>
        public string Upd_all(DataTable dt_all)
        {
            if (dt_all.Rows.Count > 0)
            {
                //过滤数据 得到数据行  身份证号
                DataRow[] drs = dt_all.Select("xmdh='NAME'");
                for (int i = 0; i < drs.Length; i++)
                {
                    string ybh = drs[i]["ybh"].ToString();
                    FBYClient.DBAccess dbjkdah = new FBYClient.DBAccess();
                    DataTable dt_access = dbjkdah.ExecuteQueryBySql(@"select *  from T_JK_lis_result_re where ybh='" + ybh + "'  ");
                    if (dt_access != null && dt_access.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt_access.Rows.Count; j++)
                        {

                            DataTable dt_tem = new DataTable();
                            dt_tem.Columns.Add("yybm");
                            dt_tem.Columns["yybm"].DefaultValue = dt_access.Rows[j]["yybm"];
                            dt_tem.Columns.Add("yq");
                            dt_tem.Columns["yq"].DefaultValue = dt_access.Rows[j]["yq"];
                            dt_tem.Columns.Add("jyrq");
                            dt_tem.Columns["jyrq"].DefaultValue = dt_access.Rows[j]["jyrq"];
                            dt_tem.Columns.Add("ybh");
                            dt_tem.Columns["ybh"].DefaultValue = dt_access.Rows[j]["ybh"];
                            dt_tem.Columns.Add("xmdh");
                            dt_tem.Columns.Add("result");
                            DataRow[] drss = dt_all.Select("jyrq='" + drs[i]["jyrq"].ToString() + "' and ybh='" + drs[i]["ybh"].ToString() + "'");
                            for (int k = 0; k < drss.Length; k++)
                            {
                                dt_tem.Rows.Add();
                                dt_tem.Rows[k]["xmdh"] = drss[k]["xmdh"];
                                dt_tem.Rows[k]["result"] = drss[k]["result"];
                            }
                            Form_lisBll form_lisbll = new Form_lisBll();
                            form_lisbll.Upd_all(dt_tem, "sql042_update");
                        }
                    }
                    else
                    {
                        DataTable dt_insert_tem = dt_all.Clone();
                        DataRow[] drss = dt_all.Select("jyrq='" + drs[i]["jyrq"].ToString() + "' and ybh='" + drs[i]["ybh"].ToString() + "'");
                        if (drss != null && drss.Length > 0)
                        {

                            for (int k = 0; k < drss.Length; k++)
                            {
                                dt_insert_tem.ImportRow(drss[k]);
                            }

                            Form_lisBll form_lisbll = new Form_lisBll();

                            form_lisbll.Add(dt_insert_tem, "sql042");

                        }

                    }
                }
            }

            return "";
        }

        private DataTable GetDtAccess(string ybh, string yljgbm, ref string testno)
        {
            FBYClient.DBAccess dbjkdah = new FBYClient.DBAccess();
            DataTable dt_access_no = dbjkdah.ExecuteQueryBySql(@"select testno from T_JK_lis_result_re where ybh='" + ybh + "'");
            if (dt_access_no != null && dt_access_no.Rows.Count > 0 && !string.IsNullOrEmpty(dt_access_no.Rows[0]["testno"].ToString().Trim()))
            {
                //获取以对应的人员的信息
                testno = dt_access_no.Rows[0]["testno"].ToString().Trim();
                return
                    dbjkdah.ExecuteQueryBySql(@"select jkdah,sfzh from T_JK_TJRY_TXM where (txmbh='" +
                                              dt_access_no.Rows[0]["testno"]
                                              + "' or jkdah ='" + dt_access_no.Rows[0]["testno"] + "' or sfzh='" + dt_access_no.Rows[0]["testno"] +
                                              "') and yljgbm='" + yljgbm + "'");
            }
            else
            {
                testno = ybh;
                return dbjkdah.ExecuteQueryBySql(@"select jkdah,sfzh from T_JK_TJRY_TXM where (txmbh='" + ybh + "' or sfzh='" + ybh + "') and yljgbm='" + yljgbm + "'");
            }

        }

        private void UpdateLisResultRe(string ybh, string jyrq, string testno, ref string errsql)
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

        private DataTable GetHyxm(string yq, string jyrq, string ybh, ref string errsql)
        {
            string sqlWhere = "";
            //医院编码
            if (UserInfo.Yybm.Length > 0)
            {
                sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.yybm='{0}'", UserInfo.Yybm);
            }
            //仪器
            if (yq.Length > 0)
            {
                sqlWhere = sqlWhere + string.Format(" and  T_JK_lis_result_re.yq='{0}'", yq);
            }
            //检验日期
            if (jyrq.Length > 0)
            {
                sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.jyrq='{0}'", jyrq);
            }
            //样本号
            if (ybh.Length > 0)
            {
                sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.ybh='{0}'", ybh);
            }
            errsql = "sql045+sqlwhere:" + sqlWhere;
            //取得化验结果
            Form_lisBll form_lisbll = new Form_lisBll();
            return form_lisbll.GetMoHuList(sqlWhere, "sql045");
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
            FBYClient.DBAccess dBAccess = new FBYClient.DBAccess();
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
                    //dt_pp(dt_all);
                    ////检查数据保存到T_JK_lis_result_re中
                    Form_lisBll form_lisbll = new Form_lisBll();
                    //form_lisbll.Add(dt_all, "sql042");

                    //
                    DataRow[] drs = dt_all.Select("xmdh='NAME'");
                    for (int i = 0; i < drs.Length; i++)
                    {
                        string testno = "";
                        DataTable dt_access = GetDtAccess(drs[i]["ybh"].ToString(), drs[i]["yybm"].ToString(), ref testno);
                        if (dt_access == null || dt_access.Rows.Count <= 0)
                        {
                            WriteMessageToTxt("无此样本号：" + drs[i]["ybh"].ToString());
                            continue;
                        }
                        //在此处进行插入修改操作
                        //LisResultReOperation(dt_all.Select(string.Format("ybh='{0}' and jyrq='{1}' and yq='{2}' and yybm='{3}'", drs[i]["ybh"], drs[i]["jyrq"],
                        //  drs[i]["yq"], drs[i]["yybm"])).CopyToDataTable());

                        //更新化验结果表(T_JK_lis_result_re），确定化验结果与人员关系
                        UpdateLisResultRe(drs[i]["ybh"].ToString(), drs[i]["jyrq"].ToString(), testno, ref errsql);


                        //Form_lisBll form_lisbll = new Form_lisBll();

                        //查询体检状态
                        string tjzt = "1";
                        DataRow[] dr_zt = dt_all.Select("xmdh='TJZT' and ybh='" + drs[i]["ybh"].ToString() + "'");
                        if (dr_zt != null && dr_zt.Length > 0)
                            tjzt = dr_zt[0]["result"].ToString();
                        //体检状态信息
                        errsql = "save_T_JK_TJZT " + tjzt.ToString();
                        ArrayList TJRYXXList = save_T_JK_TJZT(dt_access.Rows[0]["jkdah"].ToString(), dt_access.Rows[0]["sfzh"].ToString(), drs[i]["result"].ToString(), drs[i]["jyrq"].ToString(), Common.TJTYPE.健康体检表, Common.ZT.确定状态);
                        if (TJRYXXList != null && TJRYXXList.Count > 0)
                        {
                            FBYClient.DBAccess dbaccess = new FBYClient.DBAccess();
                            dbaccess.ExecuteNonQueryBySql(TJRYXXList);
                        }

                        //取得化验结果
                        DataTable dt_dyxm = GetHyxm(drs[i]["yq"].ToString(), drs[i]["jyrq"].ToString(), drs[i]["ybh"].ToString(),
                             ref errsql);

                        //更新健康体检表
                        string Guid = "";
                        //true:新的Guid  false:已经存在的Guid
                        bool GuidResult = true;
                        DataTable dt_tjjgUpdate = GetTjjgUpdateDateTable(dt_dyxm, drs[i]["jyrq"].ToString(), dt_access, ref errsql, ref Guid, ref GuidResult);



                        string[] sqllist = sqlcode.Split(new char[] { '|' });

                        if (sqlcode.Length == 0 || sqllist.Length < 2)
                        {
                            MessageBox.Show("仪器sql设定错误！");
                            return;
                        }
                        //体检结果
                        if (GuidResult)
                        {
                            errsql = "jktj insert;sql:" + sqllist[0];
                            //体检结果插入
                            form_lisbll.Add(dt_tjjgUpdate, sqllist[0]);
                        }
                        else
                        {
                            errsql = "jktj update;sql:" + sqllist[1];
                            //体检结果更新
                            dt_tjjgUpdate.AcceptChanges();
                            dt_tjjgUpdate.Rows[0]["guid"] = Guid;
                            form_lisbll.Upd(dt_tjjgUpdate, sqllist[1]);
                        }
                        errsql = "";

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

        private void AutoForm_Imac120_Load(object sender, EventArgs e)
        {
            TxtResult.Text = "";
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            //this.Location = formLocal;

            this.WindowState = FormWindowState.Normal;

            this.TopMost = true;
        }
    }
}
