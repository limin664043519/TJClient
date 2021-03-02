using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using Microsoft.VisualBasic.PowerPacks;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using TJClient;
using TJClient.sys;
using LIS;
using TJClient.Common;
using TJClient.sys.Bll;
using System.Logger;
using System.IO;
namespace FBYClient
{
    public partial class LnrDataUp : sysCommonForm
    {
        #region 成员变量

        /// <summary>
        /// X
        /// </summary>
        private int mCurrentX;

        /// <summary>
        /// Y
        /// </summary>
        private int mCurrentY;

        /// <summary>
        /// 是否移动窗体
        /// </summary>
        private bool mBeginMove = false;

        /// <summary>
        /// 用户名
        /// </summary>
        private static string userId = "";

        /// <summary>
        /// 分组
        /// </summary>
        private static string yhfz = "";

        /// <summary>
        /// 医疗机构
        /// </summary>
        private static string yljg = "";

        /// <summary>
        /// label控件的前缀
        /// </summary>
        private static string controlType_lable = "label_";

        /// <summary>
        /// line控件的前缀
        /// </summary>
        private static string controlType_line = "line_";

        /// <summary>
        /// panel控件的前缀
        /// </summary>
        private static string controlType_panel = "panel_";

        /// <summary>
        ///  checkBox控件的前缀
        /// </summary>
        private static string controlType_checkBox = "checkBox_";

        /// <summary>
        /// textBox_控件的前缀
        /// </summary>
        private static string controlType_textBox = "textBox_";

        /// <summary>
        /// radioButton控件的前缀
        /// </summary>
        private static string controlType_radioButton = "radioButton_";

        /// <summary>
        /// dataGridView控件的前缀
        /// </summary>
        private static string controlType_dataGridView = "dataGridView_";

        /// <summary>
        /// LinkLabel控件的前缀
        /// </summary>
        private static string controlType_LinkLabel = "LinkLabel_";

        /// <summary>
        /// 控件信息保存
        /// </summary>
        private DataTable dtResult = new DataTable();

        private DataTable dtControl = null;

        private DataTable dt_TJXMGLKZ = new DataTable();

        //private int TabIndex = 100;

        private int TabIndex_p = 200;

        ///// <summary>
        ///// 村庄编码
        ///// </summary>
        //private string Czbm = "";

        /// <summary>
        /// 1：没有接收  2正在接收
        /// </summary>
        private int lis_Recevier = 1;

        //private string serverIp = "";

        /// <summary>
        /// groupbox的标题
        /// </summary>
        private int groupbox_title = 0;

        /// <summary>
        /// 保存控件的原有的颜色
        /// </summary>
        public Color color_tem = new Color();

        /// <summary>
        /// 中医体质辨识的问题列表
        /// </summary>
        public DataTable dt_wt_zytz = new DataTable();
        /// <summary>
        /// 中医体质辨识的答案列表
        /// </summary>
        public DataTable dt_da_zytz = new DataTable();
        /// <summary>
        /// 中医体质得分
        /// </summary>
        public DataTable dt_df_zytz = new DataTable();

        /// <summary>
        /// 仪器
        /// </summary>
        public static IInterface yqDemo = null;

        /// <summary>
        /// 上次体检结果
        /// </summary>
        public DataTable dt_T_JK_JKTJ_TMP = null;

        /// <summary>
        /// 下拉框绑定时的标志
        /// </summary>
        public bool ComboBoxFlag = false;
        public SimpleLogger logger = SimpleLogger.GetInstance();

        /// <summary>
        /// 在页面初始化时，控件的事件不触发
        /// </summary>
        public bool initStatue_left = false;

        public bool initStatue_right = true;

        public bool initStatue_top = false;

        /// <summary>
        /// 体检人员信息
        /// </summary>
        public DataTable dt_list_tjryxx = null; 

        ///// <summary>
        ///// 保存当前辅助录入的textbox
        ///// </summary>
        //public TextBox TextBox_tem_fzlr = null;

        #endregion

      
        #region 窗体事件
        public LnrDataUp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载健康体检项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jktj_Load(object sender, EventArgs e)
        {
            //中医体质
           // tabPage2.Parent = null;
            //自理能力评估
            //tabPage3.Parent = null;

            //progressBar_sc.Visible = false;
            //取得前以页面传递的数据
            DataTable dt = (DataTable)((Login)this.Owner).Tag;
            //用户id
            userId = dt.Rows[0]["userId"].ToString();
            //工作组
            yhfz = dt.Rows[0]["gzz"].ToString();
            //医疗机构
            yljg = dt.Rows[0]["yljg"].ToString();
            DBAccess dBAccess = new DBAccess();

            //label_userinfo.Text =string .Format ("[{0}] [{1}]",userId,UserInfo.Username) ;


            //获取该工作组对应的体检项目
            string sql = @"SELECT T_JK_GZZ_XM.YLJGBM, T_JK_GZZ_XM.GZZBM, T_JK_TJXM.XMFLBM, T_JK_TJXM.XMBM, T_JK_TJXM.XMMC, T_JK_TJXM.KJXSMC, T_JK_TJXM.KJLX, T_JK_TJXM.SJZDBM, T_JK_TJXM.KJID, T_JK_TJXM.KJKD, T_JK_TJXM.KJGD, T_JK_TJXM.KJMRZ, T_JK_TJXM.JKDA_DB, T_JK_TJXM.HIS_DB, T_JK_TJXM.SL, T_JK_TJXM.DJ, T_JK_TJXM.parentxm, T_JK_TJXM.parentxmvalue, T_JK_TJXM.maxcount, T_JK_TJXM.dw, T_JK_XMFL.XMFLMC,T_JK_TJXM.rowNo,T_JK_TJXM.jj,T_JK_TJXM.valueHeigh,T_JK_TJXM.valueLower,T_JK_TJXM.isNotNull,T_JK_TJXM.fzlritem
                           FROM (T_JK_GZZ_XM INNER JOIN T_JK_TJXM ON T_JK_GZZ_XM.XMBM = T_JK_TJXM.XMBM) INNER JOIN T_JK_XMFL ON T_JK_TJXM.XMFLBM = T_JK_XMFL.XMFLBM
                           WHERE (((T_JK_GZZ_XM.YLJGBM)='{YLJGBM}') AND ((T_JK_GZZ_XM.GZZBM)='{GZZBM}')) order by T_JK_TJXM.XMFLBM, T_JK_TJXM.rowNo,T_JK_TJXM.ORDERBY ";

            //从数据库中取值
            dtResult = dBAccess.ExecuteQueryBySql(sql.Replace("{YLJGBM}", yljg).Replace("{GZZBM}", yhfz));
            //if (dtResult != null && dtResult.Rows.Count > 0)
            //{
            //    //initStatue_right = false;

            //    ////加载tab
            //    //dtResult = loadtab(dtResult);
            //    ////加载tab之外的控件
            //    //loadControls(dtResult);

            //    ////控制控件的状态
            //    //get_T_JK_TJXMGLKZ();
            //    //init_TJXMGLKZ("","");
            //    //initStatue_right = true;
            //}
            //else
            //{
            //    MessageBox.Show("没有取到对应的体检项目，请确认！");
            //}

            ////体检状态
            //DataTable dt_TJZT = new DataTable();
            //dt_TJZT.Columns.Add("text");
            //dt_TJZT.Columns.Add("value");
            //dt_TJZT.Rows.Add();
            //dt_TJZT.Rows[0]["text"] = "未体检";
            //dt_TJZT.Rows[0]["value"] = "2";
            //dt_TJZT.Rows.Add();
            //dt_TJZT.Rows[1]["text"] = "已体检";
            //dt_TJZT.Rows[1]["value"] = "1";
            //comboBox_head_TJZT.DataSource = dt_TJZT;
            //comboBox_head_TJZT.DisplayMember = "text";
            //comboBox_head_TJZT.ValueMember = "value";
            //comboBox_head_TJZT.SelectedValue = "2";

            ////体检医生
            //DataTable dt_thfzr = new DataTable();
            ////dt_thfzr = dBAccess.ExecuteQueryBySql("SELECT Xt_gg_czy.bm, Xt_gg_czy.xm FROM Xt_gg_czy ");
            //jktjBll jktjbll = new jktjBll();
            //dt_thfzr=jktjbll.GetMoHuList("", "sql084");

            //comboBox_head_TJFZR.DataSource = dt_thfzr;
            //comboBox_head_TJFZR.DisplayMember = "xm";
            //comboBox_head_TJFZR.ValueMember = "bm";



            ////性别
            //comboBox_head_XB.DataSource = getSjzdList("xb_xingbie");
            //comboBox_head_XB.DisplayMember = "ZDMC";
            //comboBox_head_XB.ValueMember = "ZDBM";


//            //取得村庄
//            string sqlCunzhaung = @"SELECT T_BS_CUNZHUANG.B_RGID, T_BS_CUNZHUANG.B_NAME
//                                    FROM T_TJ_YLJG_XIANGZHEN INNER JOIN T_BS_CUNZHUANG ON    T_TJ_YLJG_XIANGZHEN.XZBM = left(T_BS_CUNZHUANG.B_RGID,len(T_TJ_YLJG_XIANGZHEN.XZBM))
//
//                                    where  T_TJ_YLJG_XIANGZHEN.YLJGBM='{YLJGBM}'
//                                     order by T_BS_CUNZHUANG.B_RGID;";
//            DataTable dtCunzhuang = dBAccess.ExecuteQueryBySql(sqlCunzhaung.Replace("{YLJGBM}", yljg));
//            //绑定医疗机构
//            ComboBoxFlag = false;
//            DataRow dtRow = dtCunzhuang.NewRow();
//            dtRow["B_RGID"] = "";
//            dtRow["B_NAME"] = "--请选择--";
//            dtCunzhuang.Rows.InsertAt(dtRow, 0);
//            comboBox_cunzhuang.DataSource = dtCunzhuang;
//            comboBox_cunzhuang.DisplayMember = "B_NAME";
//            comboBox_cunzhuang.ValueMember = "B_RGID";
//            ComboBoxFlag = true;

            ////中医体质辨识的问题
            //getWtList();

            //initWt();

            ////下次随访日期
            //dateTimePicker_XCSFRQ_lkzp.Value = DateTime.Now.AddYears(1);


            ////设定控件事件可用
            //initStatue_left = true;

            //comboBox_cunzhuang.Focus();

            //textBox_TJBH.Focus();

        }
        #endregion


        #region 数据上传

        ///// <summary>
        ///// 上传
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void button_upload_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        ////体检号为空
        //        //if (textBox_head_TJBH.Text.Length <= 0)
        //        //{
        //        //    MessageBox.Show("请选择要上传的对象！");
        //        //    return;
        //        //}

        //        //progressBar_sc.Visible = true;
        //        //progressBar_sc.Maximum = 6;
        //        //progressBar_sc.Minimum = 0;
        //        //progressBar_sc.Value = 0;

        //        DBAccess access = new DBAccess();
        //        //bool result = false;
        //        ArrayList sqlList = new ArrayList();

        //        ////取得页面上的信息
        //        //getTjResultDtFromPage();
        //        ////页面信息保存到数据库中
        //        //setTjResultToDb();

        //        //取得当前要上传的人的信息
        //        DataRowView drv = listBox_ryxx.SelectedItem as DataRowView;

        //        int index = listBox_ryxx.SelectedIndex;

        //        string strWhere = "";
        //        if(dt_list_tjryxx!=null && dt_list_tjryxx.Rows .Count >0){

        //            strWhere = string.Format(" and  jkdah='{0}' and xm='{1}' and ( tjsj='{2}' or tjsj_zytz='{3}'  or tjsj_jkzp='{4}')"
        //                , dt_list_tjryxx.Rows[index]["jkdah"] != null ? dt_list_tjryxx.Rows[index]["jkdah"].ToString() : ""
        //                , dt_list_tjryxx.Rows[index]["xm"] != null ? dt_list_tjryxx.Rows[index]["xm"].ToString() : ""
        //                , dt_list_tjryxx.Rows[index]["tjsj"] != null ? dt_list_tjryxx.Rows[index]["tjsj"].ToString() : ""
        //                , dt_list_tjryxx.Rows[index]["tjsj_zytz"] != null ? dt_list_tjryxx.Rows[index]["tjsj_zytz"].ToString() : ""
        //                , dt_list_tjryxx.Rows[index]["tjsj_jkzp"] != null ? dt_list_tjryxx.Rows[index]["tjsj_jkzp"].ToString() : "");
        //        }







        //        //DataTable dt_ryxx = getList_ryxx( string.Format (" and SFH like '%{0}%' ", drv[listBox_ryxx.ValueMember]) );
        //        DataTable dt_ryxx = getList_ryxx(strWhere);
        //        if (dt_ryxx.Rows.Count > 0)
        //        {
        //            //体检人员信息只上传新建档案的人员
        //            //体检人员信息表(T_JK_TJRYXX）
        //            string sql_tem = upLoad_T_JK_TJRYXX(dt_ryxx.Rows[0]["YLJGBM"].ToString(), dt_ryxx.Rows[0]["TJPCH"].ToString(), dt_ryxx.Rows[0]["SFH"].ToString());
        //            if (sql_tem.Length > 0)
        //            {
        //                sqlList.Add(sql_tem);
        //            }

        //            //progressBar_sc.Value++;

        //            //健康体检信息表(T_JK_JKTJ）
        //            ArrayList sqlList_T_JK_JKTJ = upLoad_T_JK_JKTJ(dt_ryxx.Rows[0]);
        //            if (sqlList_T_JK_JKTJ != null)
        //            {
        //                for (int i = 0; i < sqlList_T_JK_JKTJ.Count; i++)
        //                {
        //                    sqlList.Add(sqlList_T_JK_JKTJ[i]);
        //                }
        //            }
        //            //progressBar_sc.Value++;
        //            //检验申请主表

        //            if (dt_ryxx.Rows[0]["FL"].ToString().Equals("2"))
        //            {
        //                ArrayList sqlList_Lis = upLoad_lis_reqmain(dt_ryxx.Rows[0]);
        //                if (sqlList_Lis != null)
        //                {
        //                    for (int i = 0; i < sqlList_Lis.Count; i++)
        //                    {
        //                        sqlList.Add(sqlList_Lis[i]);
        //                    }
        //                }
        //            }

        //            //progressBar_sc.Value++;

        //            //化验结果上传
        //            ArrayList sqlList_lis_reqresult = upLoad_lis_reqresult(dt_ryxx.Rows[0]);
        //            if (sqlList_lis_reqresult != null)
        //            {
        //                for (int i = 0; i < sqlList_lis_reqresult.Count; i++)
        //                {
        //                    sqlList.Add(sqlList_lis_reqresult[i]);
        //                }
        //            }

        //            //心电结果上传
        //            ArrayList sqlList_T_JK_xdResult = upLoad_T_JK_xdResult(dt_ryxx.Rows[0]);
        //            if (sqlList_T_JK_xdResult != null)
        //            {
        //                for (int i = 0; i < sqlList_T_JK_xdResult.Count; i++)
        //                {
        //                    sqlList.Add(sqlList_T_JK_xdResult[i]);
        //                }
        //            }



        //            //健康档案人口学资料（T_DA_JKDA_RKXZL）
        //            ArrayList sqlList_T_DA_JKDA_RKXZL = upLoad_T_DA_JKDA_RKXZL(dt_ryxx.Rows[0]);
        //            if (sqlList_T_DA_JKDA_RKXZL != null)
        //            {
        //                for (int i = 0; i < sqlList_T_DA_JKDA_RKXZL.Count; i++)
        //                {
        //                    sqlList.Add(sqlList_T_DA_JKDA_RKXZL[i]);
        //                }
        //            }

        //            //老年人中医药健康管理表（T_LNR_ZYYTZGL）
        //            ArrayList sqlList_T_LNR_ZYYTZGL = upLoad_T_LNR_ZYYTZGL(dt_ryxx.Rows[0]);
        //            if (sqlList_T_LNR_ZYYTZGL != null)
        //            {
        //                for (int i = 0; i < sqlList_T_LNR_ZYYTZGL.Count; i++)
        //                {
        //                    sqlList.Add(sqlList_T_LNR_ZYYTZGL[i]);
        //                }
        //            }

        //            //健康随访（自理评估）（T_JG_LNRSF）
        //            ArrayList sqlList_T_JG_LNRSF = upLoad_T_JG_LNRSF(dt_ryxx.Rows[0]);
        //            if (sqlList_T_JG_LNRSF != null)
        //            {
        //                for (int i = 0; i < sqlList_T_JG_LNRSF.Count; i++)
        //                {
        //                    sqlList.Add(sqlList_T_JG_LNRSF[i]);
        //                }
        //            }

        //            //progressBar_sc.Value++;
        //            //上传
        //            up_WebService(sqlList);

        //            //调用webservice处理上传后的数据
        //            dealSave_WebService(yljg, userId);

        //            //上传后的数据状态变更
        //            updateDataStatus(dt_ryxx.Rows[0]);
        //            //progressBar_sc.Value++;
        //            //progressBar_sc.Visible = false;
        //            MessageBox.Show("上传成功！");
        //        }
        //        else
        //        {
        //            //progressBar_sc.Visible = false;
        //            MessageBox.Show("没有需要上传的数据！");
        //        }

        //        //setPageClear();
        //        textBox_TJBH.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        //progressBar_sc.Visible = false;
        //        logger.Error("jktj:" + "button_upload_Click:" + ex.Message );
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 批量上传
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void button_uploads_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DBAccess access = new DBAccess();
        //        bool result = false;
        //        ArrayList sqlList = new ArrayList();

        //        //progressBar_sc.Visible = true;
        //        //progressBar_sc.Maximum = listBox_ryxx.Items.Count + 1;
        //        //progressBar_sc.Minimum = 0;
        //        //用于更新的dataTable
        //        DataTable dt_update_statue = null;

        //        for (int listBoxIndex = 0; listBoxIndex < listBox_ryxx.Items.Count; listBoxIndex++)
        //        {
        //            //progressBar_sc.Value = listBoxIndex;
        //            //取得当前要上传的人的信息
        //            DataRowView drv = listBox_ryxx.Items[listBoxIndex] as DataRowView;

        //            int index = listBoxIndex;
        //            string strWhere = "";
        //            if (dt_list_tjryxx != null && dt_list_tjryxx.Rows.Count > 0)
        //            {

        //                strWhere = string.Format(" and  jkdah='{0}' and xm='{1}' and ( tjsj='{2}' or tjsj_zytz='{3}'  or tjsj_jkzp='{4}')"
        //                    , dt_list_tjryxx.Rows[index]["jkdah"] != null ? dt_list_tjryxx.Rows[index]["jkdah"].ToString() : ""
        //                    , dt_list_tjryxx.Rows[index]["xm"] != null ? dt_list_tjryxx.Rows[index]["xm"].ToString() : ""
        //                    , dt_list_tjryxx.Rows[index]["tjsj"] != null ? dt_list_tjryxx.Rows[index]["tjsj"].ToString() : ""
        //                    , dt_list_tjryxx.Rows[index]["tjsj_zytz"] != null ? dt_list_tjryxx.Rows[index]["tjsj_zytz"].ToString() : ""
        //                    , dt_list_tjryxx.Rows[index]["tjsj_jkzp"] != null ? dt_list_tjryxx.Rows[index]["tjsj_jkzp"].ToString() : "");
        //            }

        //            //DataTable dt_ryxx = getList_ryxx(" and SFH like '" + drv[listBox_ryxx.ValueMember] + "' and  (TJZT='1' or TJZT_zytz ='1' or TJZT_jkzp='1') ");
        //            DataTable dt_ryxx = getList_ryxx(strWhere);


        //            if (dt_update_statue == null)
        //            {
        //                dt_update_statue = dt_ryxx.Clone();
        //            }

        //            if (dt_ryxx!=null && dt_ryxx.Rows.Count > 0)
        //            {
        //                //体检人员信息表(T_JK_TJRYXX）
        //                string sql_tem = upLoad_T_JK_TJRYXX(dt_ryxx.Rows[0]["YLJGBM"].ToString(), dt_ryxx.Rows[0]["TJPCH"].ToString(), dt_ryxx.Rows[0]["SFH"].ToString());
        //                if (sql_tem.Length > 0)
        //                {
        //                    sqlList.Add(sql_tem);
        //                }

        //                //健康体检信息表(T_JK_JKTJ）
        //                ArrayList sqlList_T_JK_JKTJ = upLoad_T_JK_JKTJ(dt_ryxx.Rows[0]);
        //                if (sqlList_T_JK_JKTJ != null)
        //                {
        //                    for (int i = 0; i < sqlList_T_JK_JKTJ.Count; i++)
        //                    {
        //                        sqlList.Add(sqlList_T_JK_JKTJ[i]);
        //                    }
        //                }

        //                //检验申请主表
        //                if (dt_ryxx.Rows[0]["FL"].ToString().Equals("2"))
        //                {
        //                    ArrayList sqlList_Lis = upLoad_lis_reqmain(dt_ryxx.Rows[0]);
        //                    if (sqlList_Lis != null)
        //                    {
        //                        for (int i = 0; i < sqlList_Lis.Count; i++)
        //                        {
        //                            sqlList.Add(sqlList_Lis[i]);
        //                        }
        //                    }
        //                }

        //                //化验结果上传
        //                ArrayList sqlList_lis_reqresult = upLoad_lis_reqresult(dt_ryxx.Rows[0]);
        //                if (sqlList_lis_reqresult != null)
        //                {
        //                    for (int i = 0; i < sqlList_lis_reqresult.Count; i++)
        //                    {
        //                        sqlList.Add(sqlList_lis_reqresult[i]);
        //                    }
        //                }

        //                //心电结果上传
        //                ArrayList sqlList_T_JK_xdResult = upLoad_T_JK_xdResult(dt_ryxx.Rows[0]);
        //                if (sqlList_T_JK_xdResult != null)
        //                {
        //                    for (int i = 0; i < sqlList_T_JK_xdResult.Count; i++)
        //                    {
        //                        sqlList.Add(sqlList_T_JK_xdResult[i]);
        //                    }
        //                }

        //                //健康档案人口学资料（T_DA_JKDA_RKXZL）
        //                ArrayList sqlList_T_DA_JKDA_RKXZL = upLoad_T_DA_JKDA_RKXZL(dt_ryxx.Rows[0]);
        //                if (sqlList_T_DA_JKDA_RKXZL != null)
        //                {
        //                    for (int i = 0; i < sqlList_T_DA_JKDA_RKXZL.Count; i++)
        //                    {
        //                        sqlList.Add(sqlList_T_DA_JKDA_RKXZL[i]);
        //                    }
        //                }


        //                //老年人中医药健康管理表（T_LNR_ZYYTZGL）
        //                ArrayList sqlList_T_LNR_ZYYTZGL = upLoad_T_LNR_ZYYTZGL(dt_ryxx.Rows[0]);
        //                if (sqlList_T_LNR_ZYYTZGL != null)
        //                {
        //                    for (int i = 0; i < sqlList_T_LNR_ZYYTZGL.Count; i++)
        //                    {
        //                        sqlList.Add(sqlList_T_LNR_ZYYTZGL[i]);
        //                    }
        //                }

        //                //健康随访（自理评估）（T_JG_LNRSF）
        //                ArrayList sqlList_T_JG_LNRSF = upLoad_T_JG_LNRSF(dt_ryxx.Rows[0]);
        //                if (sqlList_T_JG_LNRSF != null)
        //                {
        //                    for (int i = 0; i < sqlList_T_JG_LNRSF.Count; i++)
        //                    {
        //                        sqlList.Add(sqlList_T_JG_LNRSF[i]);
        //                    }
        //                }

                      
        //                dt_update_statue.Rows.Add(dt_ryxx.Rows[0].ItemArray);
                       
        //            }

                    
        //        }
        //        //上传
        //        result = up_WebService(sqlList);

        //        //调用webService处理上传的数据
        //        result = dealSave_WebService(yljg, userId);

        //        //上传结束后的数据状态变更
        //        updateDataStatus(dt_update_statue);

        //        //progressBar_sc.Value++;
        //        //progressBar_sc.Visible = false;
        //        MessageBox.Show("上传成功！");

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        //progressBar_sc.Visible = false;
        //    }

        //}

        /// <summary>
        /// 调用webservice上传数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private bool up_WebService(ArrayList sql)
        {
            string result = "";
            return true;
            //TJClient.WebReference.JktjServiceService webService = new TJClient.WebReference.JktjServiceService();
            ////上传
            //string[] sqlList = new string[sql.Count];
            //if (sqlList.Length == 0)
            //{
            //    return true;
            //}
            //for (int i = 0; i < sql.Count; i++)
            //{
            //    logger.Info(sql[i].ToString());
            //    sqlList[i] = sql[i].ToString();
            //}
            ////调用webService上传
            //result = webService.uploadTjjg(sqlList);
            //if (result.Length > 0)
            //{
            //    string[] resultList = result.Split(new char[] { '-' });
            //    //上传成功
            //    if (resultList.Length > 0 && "s".Equals(resultList[0]))
            //    {
            //        //调用webservice处理上传后的数据
            //        //dealSave_WebService(yljg, userId);

            //        //string url = System.Configuration.ConfigurationManager.AppSettings["HisUrl"].ToString();
            //        //byte[] imgbyte=getImg(dtRow);
            //        //if (imgbyte != null)
            //        //{
            //        //    webService.getdealXdt(dtRow["JKDAH"].ToString(), dtRow["TJSJ"].ToString(), dtRow["XM"].ToString(), imgbyte, url);
            //        //}

                    
            //        return true;
            //    }
            //    else
            //    {
            //        logger.Error("jktj:" + "dealSave_WebService:" + result);
            //        throw new Exception("上传失败！" + result);
            //    }
            //}
            //else
            //{
            //    logger.Error("jktj:" + "dealSave_WebService:" + result);
            //    throw new Exception("上传失败！没有得到任何返回值");
            //}
        }


        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        public byte[] getImg(DataRow dtRow)
        {
            //图片
            DataTable dtImg = new DataTable();
            jktjBll jktjbll = new jktjBll();
            dtImg = jktjbll.GetMoHuList(string.Format(" and D_GRDABH='{0}' and czy='{1}' and HAPPENTIME like '%{2}%'", dtRow["JKDAH"].ToString(),UserInfo .userId ,DateTime .Now .Year .ToString()), "sql089");
            if (dtImg != null && dtImg.Rows.Count > 0)
            {
                string imgUrl = dtImg.Rows[0]["XDTURL"]!=null? dtImg.Rows[0]["XDTURL"].ToString():"";

                if (imgUrl.Length > 0)
                {
                    //判断文件的存在
                    if (File.Exists(imgUrl) == true)
                    {
                        FileStream fs = File.OpenRead(imgUrl); //OpenRead
                        int filelength = 0;
                        filelength = (int)fs.Length; //获得文件长度 
                        Byte[] image = new Byte[filelength]; //建立一个字节数组 
                        fs.Read(image, 0, filelength); //按字节流读取 
                        fs.Close();
                        return image;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 更新数据的状态
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        private bool updateDataStatus(DataRow dtRow)
        {
            jktjBll jktjbll = new jktjBll();
            //更新体检人员信息表状态
            jktjbll.UpdateBysql(string.Format(" and   yljgbm='{0}' and tjpch='{1}' and sfh={2} and SCZT ='2' ", UserInfo.Yybm, dtRow["TJPCH"].ToString(), dtRow["SFH"].ToString()), "sql081");
            //更新健康档案状态
            jktjbll.UpdateBysql(string.Format(" and   D_GRDABH='{0}' ", dtRow["JKDAH"].ToString()), "sql082");

            //更新lis申请单的状态
            jktjbll.UpdateBysql(string.Format(" and   brdh='{0}' ", dtRow["JKDAH"].ToString()), "sql101");

            return true;
        }

        /// <summary>
        /// 更新数据的状态
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        private bool updateDataStatus(DataTable  dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    updateDataStatus(dt.Rows[i]);
                }
            }
            return true;
        }

        /// <summary>
        /// 调用webservice处理上传后的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private bool dealSave_WebService(string yybm, string czy)
        {
            string result = "";
            return true;
            //TJClient.WebReference.JktjServiceService webService = new TJClient.WebReference.JktjServiceService();
            ////调用webservice处理上传后的数据
            //result = webService.dealSaveTjjg(yybm, czy);
            //if (result.Length > 0)
            //{
            //    string[] resultList = result.Split(new char[] { '-' });
            //    //上传成功
            //    if (resultList.Length > 0 && "s".Equals(resultList[0]))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        logger.Error("jktj:" + "dealSave_WebService:" + result);
            //        throw new Exception("处理失败！" + result);
            //    }
            //}
            //else
            //{
            //    throw new Exception("处理失败！没有得到任何返回值");
            //}
        }


        /// <summary>
        /// 体检人员信息表(T_JK_TJRYXX）
        /// </summary>
        /// <param name="yljgbm"></param>
        /// <param name="tjpch"></param>
        /// <param name="sfh"></param>
        /// <returns></returns>
        private string upLoad_T_JK_TJRYXX(string yljgbm, string tjpch, string sfh)
        {
            string sql_select = @" select YLJGBM,
                                            TJJHBM,
                                            TJPCH,
                                            sfh,
                                            SXHM,
                                            TJBM,
                                            JKDAH,
                                            GRBM,
                                            XM,
                                            XB,
                                            SFZH,
                                            LXDH,
                                            CSRQ,
                                            CZBM,
                                            TJZT,
                                            TJSJ,
                                            TJFZR,
                                            FL,
                                            BZ,
                                            TJBH_TEM,
                                            CREATETIME,
                                            CREATEUSER,
                                            UPDATETIME,
                                            UPDATEUSER,
                                            SCZT,
                                            LISZT,
                                            TJZT_zytz,
                                            TJSJ_zytz,
                                            TJZT_jkzp,
                                            TJSJ_jkzp,nd,PRID,zlbz
                                             from T_JK_TJRYXX where yljgbm='{yljgbm}' and tjpch='{tjpch}' and sfh={sfh} ";
            sql_select = sql_select.Replace("{yljgbm}", yljgbm).Replace("{tjpch}", tjpch).Replace("{sfh}", sfh);
            DBAccess acess = new DBAccess();

            string sqlColumns = "";
            string sqlvalues = "";
            string sqlResult = "";
            DataTable dt = acess.ExecuteQueryBySql(sql_select);
            if (dt.Rows.Count > 0)
            {
                //新添加人员
                if (dt.Rows[0]["FL"] != null && dt.Rows[0]["FL"].ToString().Equals("2") && dt.Rows[0]["sczt"] != null && dt.Rows[0]["sczt"].ToString().Equals("2") && dt.Rows[0]["zlbz"] != null && !dt.Rows[0]["zlbz"].ToString().Equals("4"))
                {
                    //在添加时顺番号自动生成
                    if (dt.Columns.Contains("sfh"))
                    {
                        dt.Columns.Remove("sfh");
                    }
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sqlColumns = sqlColumns + dt.Columns[i].ColumnName + ",";
                        if (dt.Columns[i].DataType.Equals(Type.GetType("System.String")))
                        {
                            sqlvalues = sqlvalues + "'" + dt.Rows[0][i].ToString() + "' ,";
                        }
                        else
                        {
                            sqlvalues = sqlvalues + dt.Rows[0][i].ToString() + ",";
                        }
                    }
                    sqlResult = "insert into T_JK_TJRYXX(" + sqlColumns.Substring(0, sqlColumns.Length - 1) + ") values (" + sqlvalues.Substring(0, sqlvalues.Length - 1) + ")";
                
                }
                else
                {
                    //更新
                    //sqlResult = save_T_JK_TJRYXX(dt.Rows[0], dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd"), "TJZT", "TJSJ");
                }
                
            }
            return sqlResult;
        }

        /// <summary>
        /// 健康体检信息表(T_JK_JKTJ）
        /// </summary>
        /// <returns></returns>
        private ArrayList upLoad_T_JK_JKTJ(DataRow dtRow)
        {

            string sql_select = " select * from T_JK_JKTJ where D_GRDABH='{D_GRDABH}' and HAPPENTIME='{HAPPENTIME}' and czy='{czy}' ";
            sql_select = sql_select.Replace("{D_GRDABH}", dtRow["JKDAH"].ToString()).Replace("{HAPPENTIME}", dtRow["TJSJ"].ToString()).Replace("{czy}",UserInfo .userId);
            DBAccess acess = new DBAccess();
            ArrayList sqlList = new ArrayList();

            DataTable dt = acess.ExecuteQueryBySql(sql_select);
            if (dt.Rows.Count > 0)
            {
                //删除
                string sql_delete = "delete from T_JK_LKTJXX_TEM where ID='" + dt.Rows[0]["guid"].ToString() + "'";
                sqlList.Add(sql_delete);

                //更新用的sql       数据库表名称&项目对应的数据库字段1：值1 | 项目对应的数据库字段2：值2
                string Result = "";

                DataView dtdtResult_view_tem = dtResult.AsDataView();
                dtdtResult_view_tem.Sort = "HIS_DB";
                DataTable dtdtResult_tem = dtdtResult_view_tem.ToTable();
                DataTable dtResult_tem = dtdtResult_tem.Clone();
                string his_db_tem = "";
                for (int i = 0; i < dtdtResult_tem.Rows.Count; i++)
                {
                    string his_db = dtdtResult_tem.Rows[i]["HIS_DB"].ToString().ToLower();
                    if (his_db_tem != his_db)
                    {
                        dtResult_tem.Rows.Add();
                        for (int jj = 0; jj < dtResult.Columns.Count; jj++)
                        {
                            dtResult_tem.Rows[dtResult_tem.Rows.Count - 1][jj] = dtdtResult_tem.Rows[i][jj];
                        }
                        his_db_tem = dtdtResult_tem.Rows[i]["HIS_DB"].ToString().ToLower();
                    }
                }

                for (int i = 0; i < dtResult_tem.Rows.Count; i++)
                {
                    //textbox
                    if (controlType_textBox.ToLower().IndexOf(dtResult_tem.Rows[i]["KJLX"].ToString().ToLower()) > -1)
                    {
                        string[] columList = dtResult_tem.Rows[i]["HIS_DB"].ToString().Split(new char[] { ',' });
                        for (int ii = 0; ii < columList.Length; ii++)
                        {
                            Result = Result + columList[ii].ToString() + "='" + dt.Rows[0][columList[ii].ToString()].ToString() + "',";
                        }
                    }

                    else if (controlType_dataGridView.ToLower().IndexOf(dtResult_tem.Rows[i]["KJLX"].ToString().ToLower()) == -1)
                    {
                        //dt.Columns[i].DataType.Equals(Type.GetType("System.String"))
                        if (dt.Columns[dtResult_tem.Rows[i]["HIS_DB"].ToString()].DataType.Equals(Type.GetType("System.String")))
                        {

                            Result = Result + dtResult_tem.Rows[i]["HIS_DB"].ToString() + "='" + dt.Rows[0][dtResult_tem.Rows[i]["HIS_DB"].ToString()].ToString() + "',";
                        }
                        else
                        {
                            Result = Result + dtResult_tem.Rows[i]["HIS_DB"].ToString() + "=" + dt.Rows[0][dtResult_tem.Rows[i]["HIS_DB"].ToString()].ToString() + ",";
                        }
                    }
                }

                Result = "Update T_JK_JKTJ set " + Result.Substring(0, Result.Length - 1) + " {0} Where D_GRDABH='" + dtRow["JKDAH"].ToString() + "' and HAPPENTIME='" + dtRow["TJSJ"].ToString() + "'";
                //其他项
                if (dt.Rows[0]["FIELD2"] != null && dt.Rows[0]["FIELD2"].ToString().Length > 0)
                {
                    Result = Result.Replace("{0}", string.Format(",  FIELD2='{0}',P_RGID='{1}' ", dt.Rows[0]["FIELD2"] != null ? dt.Rows[0]["FIELD2"].ToString() : "", dt.Rows[0]["P_RGID"] != null ? dt.Rows[0]["P_RGID"].ToString() : ""));
                }
                else
                {
                    Result = Result.Replace("{0}", string.Format(", P_RGID='{0}' ", dt.Rows[0]["P_RGID"] != null ? dt.Rows[0]["P_RGID"].ToString() : ""));
                }


                string sql_insert = @"insert into T_JK_LKTJXX_TEM
                                            (YLJGBM,
                                            ID,
                                            GZZBM,
                                            CZY,
                                            TJBH,
                                            TJBH_TEM,
                                            JKDAH,
                                            GRBM,
                                            CZBM,
                                            TJSJ,
                                            RESULT,
                                            ZT,
                                            CREATETIME,
                                            CREATEUSER,
                                            UPDATETIME,
                                            UPDATEUSER)
                                            values(
                                            '{YLJGBM}',
                                            '{ID}',
                                            '{GZZBM}',
                                            '{CZY}',
                                            '{TJBH}',
                                            '{TJBH_TEM}',
                                            '{JKDAH}',
                                            '{GRBM}',
                                            '{CZBM}',
                                            '{TJSJ}',
                                            '{RESULT}',
                                            '{ZT}',
                                            '{CREATETIME}',
                                            '{CREATEUSER}',
                                            '{UPDATETIME}',
                                            '{UPDATEUSER}'
                                            )";


                //医疗机构编码
                sql_insert = sql_insert.Replace("{YLJGBM}", yljg);
                //guid
                sql_insert = sql_insert.Replace("{ID}", dt.Rows[0]["guid"].ToString());
                //工作组编码
                sql_insert = sql_insert.Replace("{GZZBM}", yhfz);
                //操作员
                sql_insert = sql_insert.Replace("{CZY}", userId);
                //个人体检编号
                sql_insert = sql_insert.Replace("{TJBH}", dtRow["TJBM"].ToString());
                //个人体检编号临时
                sql_insert = sql_insert.Replace("{TJBH_TEM}", dtRow["TJBH_TEM"].ToString());
                //个人健康档案号
                sql_insert = sql_insert.Replace("{JKDAH}", dtRow["JKDAH"].ToString());
                sql_insert = sql_insert.Replace("{GRBM}", dtRow["GRBM"].ToString());
                //村庄编码
                sql_insert = sql_insert.Replace("{CZBM}", dtRow["CZBM"].ToString());
                //体检时间
                sql_insert = sql_insert.Replace("{TJSJ}", dtRow["TJSJ"].ToString());
                //体检结果
                sql_insert = sql_insert.Replace("{RESULT}", Result.Replace("'", "''"));
                //状态  0未处理
                sql_insert = sql_insert.Replace("{ZT}", "0");
                //创建时间
                sql_insert = sql_insert.Replace("{CREATETIME}", DateTime.Now.ToString());
                //创建者
                sql_insert = sql_insert.Replace("{CREATEUSER}", userId);
                //更新时间
                sql_insert = sql_insert.Replace("{UPDATETIME}", DateTime.Now.ToString());
                //更新者
                sql_insert = sql_insert.Replace("{UPDATEUSER}", userId);


                //插入
                sqlList.Add(sql_insert);

                //dataGridView
                DBAccess access = new DBAccess();
                for (int i = 0; i < dtResult_tem.Rows.Count; i++)
                {
                    if (controlType_dataGridView.ToLower().IndexOf(dtResult_tem.Rows[i]["KJLX"].ToString().ToLower()) > -1)
                    {
                        string sql_GridViewSelect = "select * from  " + dtResult_tem.Rows[i]["KJID"].ToString() + " where D_GRDABH='" + dtRow["JKDAH"].ToString() + "' ";

                        DataTable dt_GridView = access.ExecuteQueryBySql(sql_GridViewSelect);

                        if (dt_GridView != null && dt_GridView.Rows.Count > 0)
                        {
                            string sqldelete = "";
                            sqldelete = "delete from " + dtResult_tem.Rows[i]["KJID"].ToString() + " where D_GRDABH='" + dtRow["JKDAH"].ToString() + "' and  " + dt_GridView.Columns[dt_GridView.Columns.Count - 1].ToString() + "='" + dtRow["TJSJ"].ToString() + "' ";
                            //sqlList.Add(sqldelete);
                            ArrayList sql_GridViewList = new ArrayList();
                            for (int j = 0; j < dt_GridView.Rows.Count; j++)
                            {

                                string happentime = DateTime.Parse(dt_GridView.Rows[j][dt_GridView.Columns.Count - 1].ToString()).ToString("yyyy-MM-dd");
                                dt_GridView.Rows[j][dt_GridView.Columns.Count - 1] = happentime;

                                if (!happentime.Equals(dtRow["TJSJ"].ToString()))
                                {
                                    continue;
                                }

                                string sqlcolumn = "";
                                string sqlvalue = "";

                                for (int k = 1; k < dt_GridView.Columns.Count; k++)
                                {
                                    sqlcolumn = sqlcolumn + dt_GridView.Columns[k].ColumnName + ",";
                                    sqlvalue = sqlvalue + "'" + dt_GridView.Rows[j][k].ToString() + "',";
                                }
                                string sql_GridView = "insert into " + dtResult_tem.Rows[i]["KJID"].ToString() + "(" + sqlcolumn.Substring(0, sqlcolumn.Length - 1) + ") values (" + sqlvalue.Substring(0, sqlvalue.Length - 1) + ") ";

                                sql_GridViewList.Add(sql_GridView);
                            }

                            //只有在有体检数据时才上传
                            if (sql_GridViewList.Count > 0)
                            {
                                sqlList.Add(sqldelete);
                                for (int ii = 0; ii < sql_GridViewList.Count; ii++)
                                {
                                    sqlList.Add(sql_GridViewList[ii]);
                                }
                            }
                        }
                    }
                }
                return sqlList;
            }
            return null;
        }

        /// <summary>
        /// 申请单最终报告结果表(lis_reqresult）
        /// </summary>
        /// <returns></returns>
        private ArrayList upLoad_lis_reqresult(DataRow dtRow)
        {
            string sql_select = " select * from lis_reqresult where brdh ='{brdh}' ";
            sql_select = sql_select.Replace("{brdh}", dtRow["JKDAH"].ToString());
            DBAccess acess = new DBAccess();
            ArrayList sqlList = new ArrayList();
            string sqlColumns = "";
            string sqlvalues = "";
            string sqlResult = "";
            DataTable dt = acess.ExecuteQueryBySql(sql_select);
            if (dt.Rows.Count > 0)
            {
                sqlList.Add(string.Format("delete lis_reqresult where TESTNO='{0}'", dt.Rows[0]["TESTNO"].ToString ()));





                for (int j = 0; j < dt.Rows.Count; j++)
                {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlColumns = sqlColumns + dt.Columns[i].ColumnName + ",";
                            if (dt.Columns[i].DataType.Equals(Type.GetType("System.String")))
                            {
                                sqlvalues = sqlvalues + "'" + dt.Rows[j][i].ToString() + "' ,";
                            }
                            else if (dt.Columns[i].DataType.Equals(Type.GetType("System.DateTime")))
                            {
                                if (dt.Rows[j][i] != null)
                                {
                                    sqlvalues = sqlvalues + "'" + Convert.ToDateTime( dt.Rows[j][i].ToString()).ToString ("yyyy-MM-dd") + "' ,";
                                }
                                else
                                {
                                    sqlvalues = sqlvalues + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ,";
                                }
                            }
                            else
                            {
                                sqlvalues = sqlvalues + dt.Rows[j][i].ToString() + ",";
                            }
                        }
                        sqlResult = "insert into lis_reqresult(" + sqlColumns.Substring(0, sqlColumns.Length - 1) + ") values (" + sqlvalues.Substring(0, sqlvalues.Length - 1) + ")";
                        sqlList.Add(sqlResult);
                        sqlvalues = "";
                        sqlColumns = "";
                }
                return sqlList;
            }

            return null;
        
        }

        /// <summary>
        /// 心电测量结果表(T_JK_xdResult）
        /// </summary>
        /// <returns></returns>
        private ArrayList upLoad_T_JK_xdResult(DataRow dtRow)
        {
            string sql_select = " select YLJGBM, rq, sj, wjlx, jqxh, ajfh, id, xb, nl, xm, sg, tz, xy, yy, zz, bz, ys, xl, rr, pr, qrs, qt, qtc, deg, rv5, rv6, SV1, RS, dj, zddm, mnsdm, tm, nd, jkdabh from T_JK_xdResult where jkdabh ='{0}' and nd='{1}' and YLJGBM='{2}' ";
            sql_select = string.Format(sql_select, dtRow["JKDAH"].ToString(),DateTime .Now .Year .ToString (),UserInfo.Yybm);

            DBAccess acess = new DBAccess();
            ArrayList sqlList = new ArrayList();
            string sqlColumns = "";
            string sqlvalues = "";
            string sqlResult = "";
            DataTable dt = acess.ExecuteQueryBySql(sql_select);
            if (dt.Rows.Count > 0)
            {
                sqlList.Add(string.Format("delete T_JK_xdResult where jkdabh ='{0}' and nd='{1}' and YLJGBM='{2}'", dtRow["JKDAH"].ToString(), DateTime.Now.Year.ToString(), UserInfo.Yybm));
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sqlColumns = sqlColumns + dt.Columns[i].ColumnName + ",";
                        if (dt.Columns[i].DataType.Equals(Type.GetType("System.String")))
                        {
                            sqlvalues = sqlvalues + "'" + dt.Rows[j][i].ToString() + "' ,";
                        }
                        else if (dt.Columns[i].DataType.Equals(Type.GetType("System.DateTime")))
                        {
                            if (dt.Rows[j][i] != null)
                            {
                                sqlvalues = sqlvalues + "'" + Convert.ToDateTime(dt.Rows[j][i].ToString()).ToString("yyyy-MM-dd") + "' ,";
                            }
                            else
                            {
                                sqlvalues = sqlvalues + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ,";
                            }
                        }
                        else
                        {
                            sqlvalues = sqlvalues + dt.Rows[j][i].ToString() + ",";
                        }
                    }
                    sqlResult = "insert into T_JK_xdResult(" + sqlColumns.Substring(0, sqlColumns.Length - 1) + ") values (" + sqlvalues.Substring(0, sqlvalues.Length - 1) + ")";
                    sqlList.Add(sqlResult);
                    sqlvalues = "";
                    sqlColumns = "";
                }
                return sqlList;
            }

            return null;

        }
        /// <summary>
        /// 健康档案人口学资料（T_DA_JKDA_RKXZL）
        /// </summary>
        /// <returns></returns>
        private ArrayList upLoad_T_DA_JKDA_RKXZL(DataRow dtRow)
        {

            string sql_select = " select * from T_DA_JKDA_RKXZL where D_GRDABH='{D_GRDABH}' and (zt='2'or zt='3') ";
            sql_select = sql_select.Replace("{D_GRDABH}", dtRow["JKDAH"].ToString());
            DBAccess acess = new DBAccess();
            ArrayList sqlList = new ArrayList();

            DataTable dt = acess.ExecuteQueryBySql(sql_select);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["zt"] != null && dt.Rows[0]["zt"].ToString().Equals("2"))
                {
                    string sqlUpdate = "";

                    for (int i = 1; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].DataType.Equals(Type.GetType("System.String")))
                        {
                            sqlUpdate = sqlUpdate + dt.Columns[i].ColumnName + "='" + dt.Rows[0][i].ToString() + "',";
                        }
                        else
                        {
                            sqlUpdate = sqlUpdate + dt.Columns[i].ColumnName + "=" + dt.Rows[0][i].ToString() + ",";
                        }
                    }

                    sqlUpdate = " update T_DA_JKDA_RKXZL set " + sqlUpdate.Substring(0, sqlUpdate.Length - 1) + " where D_GRDABH=" + dt.Rows[0]["D_GRDABH"].ToString();

                    //更新
                    sqlList.Add(sqlUpdate);
                }
                else
                {
                    string sqlColumns = "";
                    string sqlvalues = "";
                    string sqlResult = "";
                    if (dt.Rows.Count > 0)
                    {
                        //去掉自增列
                        if (dt.Columns.Contains("id"))
                        {
                            dt.Columns.Remove("id");
                        }

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                sqlColumns = sqlColumns + dt.Columns[i].ColumnName + ",";
                                if (dt.Columns[i].DataType.Equals(Type.GetType("System.String")))
                                {
                                    sqlvalues = sqlvalues + "'" + dt.Rows[j][i].ToString() + "' ,";
                                }
                                else
                                {
                                    sqlvalues = sqlvalues + dt.Rows[j][i].ToString() + ",";
                                }
                            }
                            sqlResult = "insert into T_DA_JKDA_RKXZL(" + sqlColumns.Substring(0, sqlColumns.Length - 1) + ") values (" + sqlvalues.Substring(0, sqlvalues.Length - 1) + ")";
                            sqlList.Add(sqlResult);
                        }

                    }
                }
                return sqlList;
            }
            return null;
        }

        /// <summary>
        /// 老年人中医药健康管理表（T_LNR_ZYYTZGL）
        /// </summary>
        /// <returns></returns>
        private ArrayList upLoad_T_LNR_ZYYTZGL(DataRow dtRow)
        {

            string sql_select = " select * from T_LNR_ZYYTZGL where D_GRDABH='{D_GRDABH}' and HAPPENTIME='{HAPPENTIME}' ";
            string deleteSql = "delete from T_LNR_ZYYTZGL where  D_GRDABH='{D_GRDABH}' and HAPPENTIME='{HAPPENTIME}'";
            sql_select = sql_select.Replace("{D_GRDABH}", dtRow["JKDAH"].ToString()).Replace("{HAPPENTIME}", dtRow["TJSJ_zytz"].ToString());
            deleteSql = deleteSql.Replace("{D_GRDABH}", dtRow["JKDAH"].ToString()).Replace("{HAPPENTIME}", dtRow["TJSJ_zytz"].ToString());
            DBAccess acess = new DBAccess();
            ArrayList sqlList = new ArrayList();

            string insertItem = "";
            string insertvalue = "";
            string insertSql = "";
            DataTable dt = acess.ExecuteQueryBySql(sql_select);
            if (dt.Rows.Count > 0)
            {
                sqlList.Add(deleteSql);
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    insertItem = insertItem + dt.Columns[i].ColumnName + ",";
                    if (dt.Columns[i].DataType.Equals(Type.GetType("System.String")))
                    {
                        insertvalue = insertvalue + "'" + dt.Rows[0][i].ToString() + "',";
                    }
                    else
                    {
                        insertvalue = insertvalue+ dt.Rows[0][i].ToString() + ",";
                    }
                }

                insertSql = " insert into  T_LNR_ZYYTZGL( " + insertItem.Substring(0, insertItem.Length - 1) + ") values (" + insertvalue.Substring(0, insertvalue.Length - 1)+")";

                //更新
                sqlList.Add(insertSql);
                return sqlList;
            }
            return null;
        }

        /// <summary>
        /// 健康随访（自理评估）（T_JG_LNRSF）
        /// </summary>
        /// <returns></returns>
        private ArrayList upLoad_T_JG_LNRSF(DataRow dtRow)
        {

            string sql_select = " select  D_GRDABH,G_XCSFMB,G_XCSFRQ,G_SFYS,CREATREGION,CREATEUSER,P_RGID,CREATETIME,UPDATETIME,HAPPENTIME,G_LRRQ,QDQXZ,JCPF,SXPF,RCPF,HDPF,ZPF,CYPF,guid,czy,gzz from T_JG_LNRSF where D_GRDABH='{D_GRDABH}' and HAPPENTIME='{HAPPENTIME}' ";
            string deleteSql = "delete from T_JG_LNRSF where  D_GRDABH='{D_GRDABH}' and HAPPENTIME='{HAPPENTIME}'";
            sql_select = sql_select.Replace("{D_GRDABH}", dtRow["JKDAH"].ToString()).Replace("{HAPPENTIME}", dtRow["TJSJ_jkzp"].ToString());
            deleteSql = deleteSql.Replace("{D_GRDABH}", dtRow["JKDAH"].ToString()).Replace("{HAPPENTIME}", dtRow["TJSJ_jkzp"].ToString());
            DBAccess acess = new DBAccess();
            ArrayList sqlList = new ArrayList();

            string insertItem = "";
            string insertvalue = "";
            string insertSql = "";
            DataTable dt = acess.ExecuteQueryBySql(sql_select);
            if (dt.Rows.Count > 0)
            {
                sqlList.Add(deleteSql);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    insertItem = insertItem + dt.Columns[i].ColumnName + ",";
                    if (dt.Columns[i].DataType.Equals(Type.GetType("System.String")))
                    {
                        insertvalue = insertvalue + "'" + dt.Rows[0][i].ToString() + "',";
                    }
                    else
                    {
                        insertvalue = insertvalue + dt.Rows[0][i].ToString() + ",";
                    }
                }

                insertSql = " insert into  T_JG_LNRSF( " + insertItem.Substring(0, insertItem.Length - 1) + ") values (" + insertvalue.Substring(0, insertvalue.Length - 1) + ")";

                //更新
                sqlList.Add(insertSql);
                return sqlList;
            }
            return null;
        }

        /// <summary>
        /// 检验申请主表
        /// </summary>
        /// <returns></returns>
        private ArrayList upLoad_lis_reqmain(DataRow dtRow)
        {
            //lis_reqmain
            string sql_select = " select * from lis_reqmain where brdh='{D_GRDABH}' and jzbz='0'";
            sql_select = sql_select.Replace("{D_GRDABH}", dtRow["TJBM"].ToString());
            DBAccess acess = new DBAccess();
            ArrayList sqlList = new ArrayList();
            string sqlColumns = "";
            string sqlValue = "";
            DataTable dt = acess.ExecuteQueryBySql(sql_select);
            if (dt.Rows.Count > 0)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].DataType.Equals(Type.GetType("System.String")))
                        {
                            sqlColumns = sqlColumns + dt.Columns[i].ColumnName + ",";
                            sqlValue = sqlValue + "'" + dt.Rows[j][i].ToString() + "',";
                        }
                        else if (dt.Columns[i].DataType.Equals(Type.GetType("System.DateTime")))
                        {
                            if (dt.Rows[j][i].ToString() != null && dt.Rows[j][i].ToString().Length > 0)
                            {
                                sqlColumns = sqlColumns + dt.Columns[i].ColumnName + ",";
                                sqlValue = sqlValue + "'" +  Convert .ToDateTime( dt.Rows[j][i].ToString()).ToString ("yyyy-MM-dd HH:mm:ss") + "',";
                            }
                        }
                        else
                        {
                            if (dt.Rows[j][i] != null && dt.Rows[j][i].ToString().Trim().Length > 0)
                            {
                                sqlColumns = sqlColumns + dt.Columns[i].ColumnName + ",";
                                sqlValue = sqlValue + dt.Rows[j][i].ToString() + ",";
                            }

                        }
                    }

                    //删除
                    sqlList.Add(" delete from lis_reqmain where sqh='{sqh}'".Replace("{sqh}", dt.Rows[j]["sqh"].ToString()));
                    //插入
                    sqlList.Add("insert into lis_reqmain(" + sqlColumns.Substring(0, sqlColumns.Length - 1) + ") values (" + sqlValue.Substring(0, sqlValue.Length - 1) + ")");

                    //lis_reqdetail
                    string sql_select_lis_reqdetail = " select SQH,xh, SQXMDH, SQXMMC, SL, DJ, ZT, JJZT, BZ1, BZ2, COSTS, NUMBK1, NUMBK2, DTBK1, DTBK2, yybm, datafrom, zlbz from lis_reqdetail where sqh='{sqh}'";
                    sql_select_lis_reqdetail = sql_select_lis_reqdetail.Replace("{sqh}", dt.Rows[j]["sqh"].ToString());

                    sqlColumns = "";
                    sqlValue = "";
                    DataTable dt_lis_reqdetail = acess.ExecuteQueryBySql(sql_select_lis_reqdetail);
                    if (dt_lis_reqdetail.Rows.Count > 0)
                    {
                        DataTable dt_lis_reqdetail_tem = dt_lis_reqdetail.Copy();
                        dt_lis_reqdetail_tem.Columns.Remove("xh");
                        for (int jj = 0; jj < dt_lis_reqdetail_tem.Rows.Count; jj++)
                        {

                            for (int ii = 0; ii < dt_lis_reqdetail_tem.Columns.Count; ii++)
                            {
                                if (dt_lis_reqdetail_tem.Columns[ii].DataType.Equals(Type.GetType("System.String")))
                                {
                                    sqlColumns = sqlColumns + dt_lis_reqdetail_tem.Columns[ii].ColumnName + ",";
                                    sqlValue = sqlValue + "'" + dt_lis_reqdetail_tem.Rows[jj][ii].ToString() + "',";
                                }
                                else
                                {
                                    sqlColumns = sqlColumns + dt_lis_reqdetail_tem.Columns[ii].ColumnName + ",";
                                    sqlValue = sqlValue + dt_lis_reqdetail_tem.Rows[jj][ii].ToString() + ",";
                                }
                            }

                            //删除
                            sqlList.Add(" delete from lis_reqdetail where sqh='{sqh}'".Replace("{sqh}", dt_lis_reqdetail.Rows[jj]["sqh"].ToString()).Replace("{xh}", dt_lis_reqdetail.Rows[jj]["xh"].ToString()));
                            //插入
                            sqlList.Add("insert into LIS_REQDETAIL(" + sqlColumns.Substring(0, sqlColumns.Length - 1) + ") values (" + sqlValue.Substring(0, sqlValue.Length - 1) + ")");
                            sqlColumns = "";
                            sqlValue = "";
                        }

                    }
                }
                return sqlList;
            }
            return null;
        }

        #endregion


        /// <summary>
        /// 取得数据字典中的项目
        /// </summary>
        /// <param name="Sjzdbm"></param>
        /// <returns></returns>
        private DataTable getSjzdList(string Sjzdbm)
        {
            //获取该工作组对应的体检项目
            string sql = @"select * from  T_JK_SJZD where ZDLXBM='{ZDLXBM}' order by id ";

            DataTable checkList = new DataTable();
            DBAccess dBAccess = new DBAccess();
            checkList = dBAccess.ExecuteQueryBySql(sql.Replace("{ZDLXBM}", Sjzdbm));
            if (checkList != null && checkList.Rows.Count > 0)
            {
                return checkList;
            }
            return null;
        }


        /// <summary>
        /// 时间类型转换
        /// </summary>
        /// <param name="paraDate"></param>
        /// <returns></returns>
        private DateTime getDateFromString(string paraDate)
        {
            try
            {
                return DateTime.Parse(paraDate);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }

        }

        
    }
}