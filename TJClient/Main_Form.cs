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
using LIS;
using TJClient.Common;
using TJClient.sys.Bll;
using System.Logger;
using System.IO;
using System.Threading.Tasks;
using TJClient.jktj;
using TJClient.sys;
using TJClient.ComForm;
using BorderStyle = System.Windows.Forms.BorderStyle;

namespace FBYClient
{
    public partial class Main_Form : sysCommonForm
    {
        #region 成员变量
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
        /// LinkLabel控件的前缀
        /// </summary>
        private static string controlType_LinkLabel = "LinkLabel_";

        /// <summary>
        /// 控件信息保存
        /// </summary>
        private DataTable dtResult = new DataTable();

        private DataTable dtControl = null;

        private DataTable dt_TJXMGLKZ = new DataTable();

        /// <summary>
        /// 保存控件的原有的颜色
        /// </summary>
        public Color color_tem = new Color();

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

        /// <summary>
        /// 保存前页面传递的参数
        /// </summary>
        public DataTable dt_para = null;

        /// <summary>
        /// 老年人健康体检
        /// </summary>
        public LnrJktj lnrjktj = null;

        /// <summary>
        /// 老年人生活行为能力评估
        /// </summary>
        public LnrXwnlpg lnrxwnlpg = null;

        /// <summary>
        /// 老年人中医体质辨识
        /// </summary>
        public LnrZytz lnrzytz = null;

        #endregion

        #region 窗体事件

        /// <summary>
        /// 设定标签的内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void setLabelText(string id, string value)
        {
            Control control = Controls.Find(id, true)[0];
            Label label = (Label)control;
            label.Text = value;
            //lis_Recevier = 1;
        }

        public Main_Form()
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
            //取得前以页面传递的数据
            dt_para = (DataTable)((Login)this.Owner).Tag;
            //用户id
            userId = dt_para.Rows[0]["userId"].ToString();
            //工作组
            yhfz = dt_para.Rows[0]["gzz"].ToString();
            //医疗机构
            yljg = dt_para.Rows[0]["yljg"].ToString();
            DBAccess dBAccess = new DBAccess();

            //label_userinfo.Text =string .Format ("[{0}] [{1}]",userId,UserInfo.Username) ;
            this.Text = this.Text + string.Format("[{0}]     [{1}]", userId, UserInfo.Username).PadLeft (100,' ');

            //性别
            comboBox_head_XB.DataSource = getSjzdList("xb_xingbie");
            comboBox_head_XB.DisplayMember = "ZDMC";
            comboBox_head_XB.ValueMember = "ZDBM";
            

            //取得村庄
//            string sqlCunzhaung = @"SELECT T_BS_CUNZHUANG.B_RGID, T_BS_CUNZHUANG.B_NAME
//                                    FROM T_TJ_YLJG_XIANGZHEN INNER JOIN T_BS_CUNZHUANG ON    T_TJ_YLJG_XIANGZHEN.XZBM = left(T_BS_CUNZHUANG.B_RGID,len(T_TJ_YLJG_XIANGZHEN.XZBM))
//
//                                    where  T_TJ_YLJG_XIANGZHEN.YLJGBM='{YLJGBM}'
//                                     order by T_BS_CUNZHUANG.B_RGID;";
//            DataTable dtCunzhuang = dBAccess.ExecuteQueryBySql(sqlCunzhaung.Replace("{YLJGBM}", yljg));


            //jktjBll jktjbll_cz = new jktjBll();
            //DataTable dtCunzhuang = jktjbll_cz.GetMoHuList(string.Format(" and YLJGBM='{0}' ", UserInfo.Yybm), "sql038_cunzhuang");

            listboxFormBll listbox = new listboxFormBll();
            DataTable dtCunzhuang = listbox.GetMoHuList(string.Format("and YLJGBM = '{0}'", UserInfo.Yybm), "sql038");

            
            //绑定医疗机构
            ComboBoxFlag = false;
            DataRow dtRow = dtCunzhuang.NewRow();
            
            //dtRow["B_RGID"] = "";
            //dtRow["B_NAME"] = "--请选择--";
            dtRow["czbm"] = "";
            dtRow["czmc"] = "--请选择--";
            dtCunzhuang.Rows.InsertAt(dtRow, 0);
            comboBox_cunzhuang.DataSource = dtCunzhuang;
            //comboBox_cunzhuang.DisplayMember = "B_NAME";
            //comboBox_cunzhuang.ValueMember = "B_RGID";
            comboBox_cunzhuang.DisplayMember = "czmc";
            comboBox_cunzhuang.ValueMember = "czbm";
            
            ComboBoxFlag = true;
            tabLoad();
            comboBox_cunzhuang.Focus();
            
            //体检状态
            DataTable dt_TJZT = new DataTable();
            dt_TJZT.Columns.Add("text");
            dt_TJZT.Columns.Add("value");
            dt_TJZT.Rows.Add();
            dt_TJZT.Rows[0]["text"] = "未体检";
            dt_TJZT.Rows[0]["value"] = Common.ZT.否定状态;
            dt_TJZT.Rows.Add();
            dt_TJZT.Rows[1]["text"] = "已体检";
            dt_TJZT.Rows[1]["value"] = Common.ZT.确定状态;
            comboBox_head_TJZT.DataSource = dt_TJZT;
            comboBox_head_TJZT.DisplayMember = "text";
            comboBox_head_TJZT.ValueMember = "value";
            comboBox_head_TJZT.SelectedValue = Common.ZT.否定状态;
            
            //体检医生
            DataTable dt_thfzr = new DataTable();
            jktjBll jktjbll = new jktjBll();
            dt_thfzr = jktjbll.GetMoHuList(string.Format(" and yljgbm='{0}'", UserInfo.Yybm), "sql_sqys_All");
            comboBox_head_TJFZR.DataSource = dt_thfzr;
            comboBox_head_TJFZR.DisplayMember = "xm";
            comboBox_head_TJFZR.ValueMember = "bm";
            comboBox_head_TJFZR.SelectedValue = UserInfo.userId!=null?UserInfo.userId:"";
            //参数初始化
            //辅助录入
            CommomSysInfo.IsFzlr = checkBox_fzlr.Checked ==true?"1":"0";
            //腰围换算
            CommomSysInfo.IsYwhs = checkBox_autoYw.Checked == true ? "1" : "0";
            //上次提交结果
            CommomSysInfo.IsSctjjg = checkBox_sctjjg.Checked == true ? "1" : "0";

            //用药情况
            CommomSysInfo.IsSctjjg_yyqk = checkBox_yyqk.Checked == true ? "1" : "0";
            //体检医生
            CommomSysInfo.TJFZR_BM = comboBox_head_TJFZR.SelectedValue != null ? comboBox_head_TJFZR.SelectedValue.ToString() : "";
            CommomSysInfo.TJFZR_MC = comboBox_head_TJFZR.Text != null ? comboBox_head_TJFZR.Text.ToString() : "";
            //体检状态
            CommomSysInfo.tjzt = comboBox_head_TJZT.SelectedValue != null ? comboBox_head_TJZT.SelectedValue.ToString() : "";
            //体检日期
            CommomSysInfo.tjsj = dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd");
            SignNameGroupInit();
        }

        /// <summary>
        /// 设定社区医生
        /// </summary>
        public void setSqys(string czbm)
        {
            DataTable dt_thfzr = new DataTable();
            jktjBll jktjbll = new jktjBll();
            dt_thfzr = jktjbll.GetMoHuList(string.Format(" and czbm='{0}'", czbm), "sql_sqys_default");
            if (dt_thfzr != null && dt_thfzr.Rows.Count > 0)
            {
                if (dt_thfzr.Rows[0]["BM"] != DBNull.Value)
                {
                    comboBox_head_TJFZR.SelectedValue = dt_thfzr.Rows[0]["BM"].ToString();
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void tabLoad()
        {
            //健康体检
            lnrjktj = new LnrJktj();
            lnrjktj.setPara(dt_para);
            lnrjktj.TopLevel = false;
            panel_form_body_tj_body_body_body_jktj.Dock = DockStyle.Fill;
            lnrjktj.Parent = panel_form_body_tj_body_body_body_jktj;
            lnrjktj.Dock = DockStyle.Fill;
            lnrjktj.setPara(dt_para);
            lnrjktj.main_form = this;
            lnrjktj.FormBorderStyle = FormBorderStyle.None;
            //panel_form_body_tj_body_body_body_jktj.Hide();
            //lnrjktj.Show();
         
            //行为能力评估
            lnrxwnlpg = new LnrXwnlpg();
            lnrxwnlpg.setPara(dt_para);
            lnrxwnlpg.TopLevel = false;
            panel_form_body_tj_body_body_body_jkzp.Dock = DockStyle.Fill;
            lnrxwnlpg.Parent = panel_form_body_tj_body_body_body_jkzp;
            
            lnrxwnlpg.Dock = DockStyle.Fill;
            lnrxwnlpg.setPara(dt_para);
            lnrxwnlpg.main_form = this;
            lnrxwnlpg.FormBorderStyle = FormBorderStyle.None;
            //panel_form_body_tj_body_body_body_jkzp.Hide;
            //lnrxwnlpg.Show();

            //中医体质辨识
            lnrzytz = new LnrZytz();
            lnrzytz.setPara(dt_para);
            lnrzytz.TopLevel = false;
            panel_form_body_tj_body_body_body_zytz.Dock = DockStyle.Fill;
            lnrzytz.Parent = panel_form_body_tj_body_body_body_zytz;
            
            lnrzytz.Dock = DockStyle.Fill;
            lnrzytz.setPara(dt_para);
            lnrzytz.main_form = this;
            lnrzytz.FormBorderStyle = FormBorderStyle.None;

            //lnrzytz.Show();

            //设定菜单的样式
            setFocus(pictureBox_查询, label_查询);

            //清空主显示区的内容
            foreach (Control ctrl in panel_form_body.Controls)
            {
                if (ctrl.Name.Equals("panel_form_body_tj") == false)
                {
                    panel_form_body.Controls.Remove(ctrl);
                }
            }
            panel_form_body_tj.Visible = false;
            LnrSelect lnrselect = new LnrSelect();
            lnrselect.TopLevel = false;
            lnrselect.Parent = panel_form_body;
            lnrselect.Dock = DockStyle.Fill;
            lnrselect.FormBorderStyle = FormBorderStyle.None;
            lnrselect.Show();
        }
        #endregion

        #region  增加不存在的人员进行体检

        /// <summary>
        /// 扫码时人员不存在，添加人员的基本信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool peopleAdd(out string message)
        {
            message = "";
            try
            {
                jktjBll jktjbll = new jktjBll();
                DataTable dt_tjry_add = new DataTable();
                dt_tjry_add.Columns.Add("YLJGBM");//医疗机构编码
                dt_tjry_add.Columns.Add("TJJHBM");//体检计划编码
                dt_tjry_add.Columns.Add("TJPCH");//体检批次号
                dt_tjry_add.Columns.Add("SFH");//顺番号
                dt_tjry_add.Columns.Add("SXHM");//顺序号码
                dt_tjry_add.Columns.Add("TJBM");//个人体检编号
                dt_tjry_add.Columns.Add("JKDAH");//个人健康档案号
                dt_tjry_add.Columns.Add("XM");//姓名
                dt_tjry_add.Columns.Add("XB");//性别
                dt_tjry_add.Columns.Add("SFZH");//身份证号
                dt_tjry_add.Columns.Add("LXDH");//联系电话
                dt_tjry_add.Columns.Add("CSRQ");//出生日期
                dt_tjry_add.Columns.Add("CZBM");//村庄编码
                dt_tjry_add.Columns.Add("FL");//体检人员分类
                dt_tjry_add.Columns.Add("BZ");//备注
                dt_tjry_add.Columns.Add("TJBH_TEM");//临时个人体检编号
                dt_tjry_add.Columns.Add("CREATETIME");//创建时间
                dt_tjry_add.Columns.Add("CREATEUSER");//创建者
                dt_tjry_add.Columns.Add("UPDATETIME");//更新时间
                dt_tjry_add.Columns.Add("UPDATEUSER");//更新者
                dt_tjry_add.Columns.Add("SCZT");//数据上传状态
                dt_tjry_add.Columns.Add("ZLBZ");//增量标志
                dt_tjry_add.Columns.Add("nd");//年度
                dt_tjry_add.Columns.Add("ISSH");//是否审核
                dt_tjry_add.Columns.Add("ISNEWDOC");//是否新建档案
                dt_tjry_add.Rows.Add();

                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJJHBM"] = DateTime.Now.ToString("yyyyMMdd");//体检计划编码
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJPCH"] = DateTime.Now.ToString("HHmmss");//体检批次号

                //取得顺番号
                DataTable dt_SFH = jktjbll.GetMoHuList("", "sql076");
                if (dt_SFH != null && dt_SFH.Rows.Count > 0 && dt_SFH.Rows[0]["SFH"] != null && dt_SFH.Rows[0]["SFH"].ToString().Length > 0)
                {
                    dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SFH"] = int.Parse(dt_SFH.Rows[0]["SFH"].ToString()) + 1;//顺番号
                }
                else
                {
                    dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SFH"] = "0";//顺番号
                }
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SXHM"] = "0";//顺序号码
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJBM"] = textBox_TJBH.Text.Trim();//个人体检编号
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["JKDAH"] = textBox_TJBH.Text.Trim();//个人健康档案号
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["XM"] = textBox_TJBH.Text.Trim();// DateTime.Now.ToString("HHmmss");//姓名
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["XB"] = "";//性别
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SFZH"] = "**";//身份证号
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["LXDH"] = "**";//联系电话
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CSRQ"] = "";//出生日期
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CZBM"] = "**";//村庄编码
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["FL"] = "2";//体检人员分类
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["BZ"] = "";//备注
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJBH_TEM"] = textBox_TJBH.Text.Trim();//临时个人体检编号
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CREATETIME"] = DateTime.Now.ToString();//创建时间
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CREATEUSER"] = UserInfo.userId;//创建者
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["UPDATETIME"] = DateTime.Now.ToString();//更新时间
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["UPDATEUSER"] = UserInfo.userId;//更新者
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SCZT"] = "2";//数据上传状态
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["ZLBZ"] = "1";//增量标志
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString();//年度
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["ISSH"] = "0";//登记
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["ISNEWDOC"] = "0";//登记

                //增加体检人员信息
                jktjbll.Add(dt_tjry_add, "sql_add_people");

                //体检人员条形码对应表(T_JK_TJRY_TXM）
                DataTable t_jk_tjry_txm = new DataTable();
                t_jk_tjry_txm.Columns.Add("YLJGBM");//医疗机构编码
                t_jk_tjry_txm.Columns.Add("TXMBH");//条形码号码
                t_jk_tjry_txm.Columns.Add("TMBM");//条码分类
                t_jk_tjry_txm.Columns.Add("JKDAH");//个人健康档案号
                t_jk_tjry_txm.Columns.Add("SFZH");//身份证号
                t_jk_tjry_txm.Columns.Add("BZ");//备注
                t_jk_tjry_txm.Columns.Add("ZLBZ");//增量标志
                t_jk_tjry_txm.Columns.Add("nd");//年度
                t_jk_tjry_txm.Rows.Add();

                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["TXMBH"] = textBox_TJBH.Text.Trim();//条形码号码
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["TMBM"] = textBox_TJBH.Text.Length > 2 ? textBox_TJBH.Text.Substring(textBox_TJBH.Text.Length - 2, 2) : "";//条码分类
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["JKDAH"] = textBox_TJBH.Text.Trim();//个人健康档案号
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["SFZH"] = "**";//身份证号
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["BZ"] = "**";//备注
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["ZLBZ"] = "1";//增量标志
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString();//年度

                //增加体检人员条形码对应关系
                jktjbll.Add(t_jk_tjry_txm, "sql_add_people_txm");

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            return true;
        }


        #endregion

        #region 控件事件

        #region  keydown
        /// <summary>
        /// text焦点离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Leave(object sender, EventArgs e)
        {
            TextBox demo = ((TextBox)sender);
            string id = demo.Name;
            string value = demo.Text;
        }


        /// <summary>
        /// 村庄事件KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_cunzhuang_KeyDown(object sender, KeyEventArgs e)
        {

        }
        /// <summary>
        /// 文本框双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_DoubleClick(object sender, EventArgs e)
        {
            TextBox textbox_tem = (TextBox)sender;
            //辅助录入框
            text_fzlr(textbox_tem);
        }

        /// <summary>
        /// 体检时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePicker_tjsj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
            //向上
            if (e.KeyCode == Keys.Up)
            {
                SendKeys.Send("+{Tab}");
            }
            //向下
            if (e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{Tab}");
            }
        }
        /// <summary>
        /// 体检号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_TJBH_KeyDown(object sender, KeyEventArgs e)
        {

            initStatue_left = false;

            if (e.KeyCode == Keys.Enter)
            {
                if (comboBox_cunzhuang.SelectedValue == null)
                {
                    MessageBox.Show("请选择单位！");
                }

                TextBox text = (TextBox)sender;
                //if (text.Text.Length > 0)
                //{
                //    text.Text = text.Text.PadLeft(12, '0');
                //}
                //if (text.Text.Length > 12)
                //{
                //    text.Text = text.Text.Substring(0, 12);
                //}
                if (selectRyxx(true) == true)
                {
                    initHead("");
                    text.SelectAll();
                }
                else
                {
                    if (textBox_TJBH.Text.Trim().Length > 0)
                    {
                        Form_PeopleAdd form_peopleadd = new Form_PeopleAdd();
                        form_peopleadd.tjbh = textBox_TJBH.Text;
                        form_peopleadd.czbm = comboBox_cunzhuang.SelectedValue != null ? comboBox_cunzhuang.SelectedValue.ToString() : "";
                        form_peopleadd.Owner = this;
                        form_peopleadd.ShowDialog();

                        if (selectRyxx(true) == true)
                        {
                            initHead("");
                            text.SelectAll();
                        }
                        
                    }

                }
            }
            //向上
            if (e.KeyCode == Keys.Up)
            {
                SendKeys.Send("+{Tab}");
            }
            //向下
            if (e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{Tab}");
            }
            initStatue_left = true;
        }

        /// <summary>
        /// 设定返回的结果
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public override bool setParentFormDo(object para)
        {
            if (dt_list_tjryxx != null && dt_list_tjryxx.Rows.Count > 0)
            {
               // DataTable dt_ryxx_current = null;
                int i = listBox_ryxx.SelectedIndex == -1 ? 0 : listBox_ryxx.SelectedIndex;
                DataRow dt_ryxx = dt_list_tjryxx.Rows[i];
               //dt_ryxx["zt_jktj"] = "1";

                //dt_ryxx["zt_jktj"] = "1";

                if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.健康体检表) == true)
                {
                    dt_ryxx["zt_jktj"] = "1";
                    dt_ryxx["tjsj_jktj"] = dateTimePicker_head_TJSJ.Value .ToString("yyyy-MM-dd");

                    if (dt_ryxx["zt_jktj"] != DBNull.Value && dt_ryxx["zt_jktj"].ToString().Equals("1"))
                    {
                        dt_ryxx["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx["XM"].ToString().PadRight(4, '　') + "完";
                    }
                    else
                    {
                        dt_ryxx["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx["XM"].ToString().PadRight(4, '　') + "未";
                    }

                    //dt_ryxx_current = ((DataTable)listBox_ryxx.DataSource);
                }
                else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.生活自理能力评估) == true)
                {
                    dt_ryxx["zt_nlpg"] = "1";
                    dt_ryxx["tjsj_nlpg"] = dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd");
                    if (dt_ryxx["zt_nlpg"] != DBNull.Value && dt_ryxx["zt_nlpg"].ToString().Equals("1"))
                    {
                        dt_ryxx["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx["XM"].ToString().PadRight(4, '　') + "完";
                    }
                    else
                    {
                        dt_ryxx["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx["XM"].ToString().PadRight(4, '　') + "未";
                    }

                    //dt_ryxx_current = ((DataTable)listBox_ryxx.DataSource);

                } if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.中医体质辨识) == true)
                {
                    dt_ryxx["zt_zytz"] = "1";
                    dt_ryxx["tjsj_zytz"] = dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd");
                    if (dt_ryxx["zt_zytz"] != DBNull.Value && dt_ryxx["zt_zytz"].ToString().Equals("1"))
                    {
                        dt_ryxx["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx["XM"].ToString().PadRight(4, '　') + "完";
                    }
                    else
                    {
                        dt_ryxx["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx["XM"].ToString().PadRight(4, '　') + "未";
                    }
                    //dt_ryxx_current = ((DataTable)listBox_ryxx.DataSource);

                }
                //listBox_ryxx.Refresh();
                ////dt_ryxx["ValueMember"] = dt_ryxx["SFH"].ToString();
                ////listBox_ryxx.Items.Clear();
                ////listBox_ryxx.DataSource = null;
                //listBox_ryxx.DataSource = dt_ryxx_current;
                //listBox_ryxx.DisplayMember = "DisplayMember";
                //listBox_ryxx.ValueMember = "ValueMember";
                //dt_ryxx_current.Rows.Remove(dt_ryxx);
                //dt_ryxx_current.ImportRow(dt_ryxx);
            }
            //selectRyxx();

            //if (selectRyxx() == true)
            //{
                //initHead("");
            //}
            textBox_TJBH.SelectAll();
            return true;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_xm_KeyDown(object sender, KeyEventArgs e)
        {
            initStatue_left = false;
            if (e.KeyCode == Keys.Enter)
            {
                if (comboBox_cunzhuang.SelectedValue == null)
                {
                    MessageBox.Show("请选择单位！");
                }

                if (selectRyxx(false) == true)
                {
                    initHead("");
                    TextBox text = (TextBox)sender;
                    text.SelectAll();
                }
                else
                {
                    MessageBox.Show("没有取到对应的人员信息！");
                }
            }
            //向上
            if (e.KeyCode == Keys.Up)
            {
                SendKeys.Send("+{Tab}");
            }
            //向下
            if (e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{Tab}");
            }
            initStatue_left = true;
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_SFZH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (selectRyxx(false) == true)
                {

                    TextBox text = (TextBox)sender;
                    text.SelectAll();
                    initHead("");
                }
                else
                {
                    MessageBox.Show("没有取到对应的人员信息！");
                }
            }
            //向上
            if (e.KeyCode == Keys.Up)
            {
                SendKeys.Send("+{Tab}");
            }
            //向下
            if (e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{Tab}");
            }
        }
        /// <summary>
        /// 体检顺序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_SXHM_KeyDown(object sender, KeyEventArgs e)
        {
            initStatue_left = false;
            if (e.KeyCode == Keys.Enter)
            {
                if (comboBox_cunzhuang.SelectedValue == null)
                {
                    MessageBox.Show("请选择单位！");
                }

                if (selectRyxx(false) == true)
                {
                    initHead("");
                    TextBox text = (TextBox)sender;
                    text.SelectAll();
                }
                else
                {
                    MessageBox.Show("没有取到对应的人员信息！");
                }
            }
            //向上
            if (e.KeyCode == Keys.Up)
            {
                SendKeys.Send("+{Tab}");
            }
            //向下
            if (e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{Tab}");
            }
            initStatue_left = true;
        }
        /// <summary>
        /// Enter转换为tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Tab_KeyDown(object sender, KeyEventArgs e)
        {
            //控件初始化时，该事件不触发
            if (initStatue_right == false)
            {
                return;
            }
            //Enter转换为tab
            if (sender.GetType().ToString().Equals("System.Windows.Forms.CheckBox"))
            {
                if (e.KeyCode == Keys.Enter)
                {
                    CheckBox checkBox = (CheckBox)sender;
                    if (checkBox.Checked == true)
                    {
                        checkBox.Checked = false;
                    }
                    else
                    {
                        checkBox.Checked = true;
                    }
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }

            }
            else if (sender.GetType().ToString().Equals("System.Windows.Forms.ComboBox"))
            {
                if (e.KeyCode == Keys.Left)
                {
                    //shift+Tab
                    SendKeys.Send("+{Tab}");
                    e.Handled = false;
                }
                else if (e.KeyCode == Keys.Right)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
            }
            else if (sender.GetType().ToString().Equals("System.Windows.Forms.TextBox"))
            {
                if (e.KeyCode == Keys.Up)
                {
                    //shift+Tab
                    SendKeys.Send("+{Tab}");
                    e.Handled = false;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
                else if (e.KeyCode == Keys.Space)
                {
                    TextBox textbox_tem = (TextBox)sender;
                    //辅助录入框
                    text_fzlr(textbox_tem);
                }
            }
            else
            {

                if (e.KeyCode == Keys.Up)
                {
                    //shift+Tab
                    SendKeys.Send("+{Tab}");
                    e.Handled = false;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
            }

        }

        /// <summary>
        /// textbox的辅助录入的处理
        /// </summary>
        /// <param name="text_tem"></param>
        /// <returns></returns>
        public bool text_fzlr(TextBox text_tem)
        {
            //TextBox_tem_fzlr = text_tem;
            if (checkBox_fzlr.Checked == false)
            {
                return true;
            }

            if (text_tem.Tag != null && text_tem.Tag.ToString().Length > 0)
            {
                Formfzlr formfzlr = new Formfzlr();
                formfzlr.Owner = this;
                formfzlr.setListData(string.Format(" and ZDLXBM='{0}'", text_tem.Tag.ToString()), "sql_select_sjzd", text_tem);
                formfzlr.ShowDialog();
            }
            else
            {
                MessageBox.Show("没有设定辅助录入内容！");
            }
            return true;
        }

        /// <summary>
        /// 重写父类的方法，设定当前页面的值 strText： 编码|名称
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public override bool setTextToText(Control textbox, string strText)
        {
            if (strText.Length > 0 && textbox != null)
            {
                string[] textList = strText.Split(new char[] { '|' });

                textbox.Text = textbox.Text + " " + textList[1];
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 人员列表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_ryxx_KeyDown(object sender, KeyEventArgs e)
        {

        }
        /// <summary>
        /// 按钮的keyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                SendKeys.Send("{Tab}");
            }
            if (e.KeyCode == Keys.Left)
            {
                SendKeys.Send("+{Tab}");
                e.Handled = false;
            }
        }

        /// <summary>
        /// cheeckbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_tj_KeyDown(object sender, KeyEventArgs e)
        {

        }
        #endregion

        /// <summary>
        /// 人员列表中选择人员后的处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_ryxx_SelectedIndexChanged(object sender, EventArgs e)
        {
            //信息是否保存过
            if (Common.commnoIsSaved()==false)
            {
                return;
            }
                DataRowView drv = listBox_ryxx.SelectedItem as DataRowView;
                initHead("");
            
        }

        /// <summary>
        /// 左侧textBox获得焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        /// <summary>
        /// 左侧text单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Click(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }


        /// <summary>
        /// 获得焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_Enter(object sender, EventArgs e)
        {
            //控件初始化时，该事件不触发
            if (initStatue_right == false)
            {
                return;
            }
            if (sender.GetType().ToString().Equals("System.Windows.Forms.TextBox"))
            {
                ((TextBox)sender).SelectAll();
            }
            Control control = (Control)sender;
            color_tem = control.BackColor;
            control.ForeColor = Color.Red;
            control.BackColor = Color.YellowGreen;
        }

        /// <summary>
        /// 失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_Leave(object sender, EventArgs e)
        {
            //控件初始化时，该事件不触发
            if (initStatue_right == false)
            {
                return;
            }

            Control control = (Control)sender;
            control.ForeColor = Color.FromArgb(0, 0, 0);
            control.BackColor = color_tem;
        }


        /// <summary>
        /// 未体检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_wtj_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_wtj.Checked == true)
            {
                checkBox_ytj.Checked = false;
            }
            
        }
        /// <summary>
        /// 已体检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_ytj_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ytj.Checked==true)
            {
                checkBox_wtj.Checked = false;
            }
        }

        /// <summary>
        /// 退出到登录页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Owner.Visible = true;
            this.Close();

        }

        /// <summary>
        /// 左侧人员列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_rylb_Click(object sender, EventArgs e)
        {
            if (panel_form_body_tj_body_left.Width == 20)
            {
                panel_form_body_tj_body_left.Width = 138;
                button_rylb.Visible = true;
                listBox_ryxx.Visible = true;
                button_right.BackgroundImage = global::TJClient.Properties.Resources.btn_right;
            }
            else
            {
                panel_form_body_tj_body_left.Width = 20;
                button_rylb.Visible = false;
                listBox_ryxx.Visible = false;
                button_right.BackgroundImage = global::TJClient.Properties.Resources.btn_left;
            }
        }

        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                //Formpanel2.Width = FormPanle1.Width - 6;
                //Formpanel2.Height = FormPanle1.Height - 10;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                //Formpanel2.Width = FormPanle1.Width - 6;
                //Formpanel2.Height = FormPanle1.Height - 10;
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

        #region  检索

        /// <summary>
        /// 单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_cunzhuang_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 初始化个人信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        private void initHead(string sqlWhere)
        {
            //没有数据源时直接退出
            if (listBox_ryxx.DataSource == null) return;

            if (dt_list_tjryxx != null && dt_list_tjryxx.Rows.Count > 0)
            {
                int i = listBox_ryxx.SelectedIndex == -1 ? 0 : listBox_ryxx.SelectedIndex;
                DataRow dt_ryxx = dt_list_tjryxx.Rows[i];

                ////取得当前选中行的信息
                //MessageBox.Show(DateTime .Now .ToString ("HH:mm:ss fff"));
                //DataTable dt_ryxx_current = getList_ryxx(string.Format(" and JKDAH='{0}' and sfh={1}", dt_ryxx["JKDAH"].ToString(), dt_ryxx["sfh"].ToString()));
                //MessageBox.Show(DateTime.Now.ToString("HH:mm:ss fff"));
                
                DataTable dt_ryxx_current = ((DataTable)listBox_ryxx.DataSource).Clone();

                dt_ryxx_current.ImportRow(dt_ryxx);

                if (dt_ryxx_current != null && dt_ryxx_current.Rows .Count >0)
                {
                    DataTable dt_para = new DataTable();
                    dt_para.Columns.Add("JKDAH");//健康档案号
                    dt_para.Columns.Add("LXDH");//联系电话
                    dt_para.Columns.Add("tjsj");//体检时间
                    dt_para.Columns.Add("gzz");//工作组
                    dt_para.Columns.Add("prgid");//所属机构
                    dt_para.Columns.Add("SFZH");//身份证号
                    dt_para.Columns.Add("XM");//姓名
                    dt_para.Columns.Add("XB");//性别                    
                    dt_para.Columns.Add("YLJGBM");//医疗机构编码
                    dt_para.Columns.Add("ND");//年度
                    dt_para.Rows.Add();

                    //设定默认医生
                    setSqys(dt_ryxx_current.Rows[0]["CZBM"].ToString());

                    //姓名
                    textBox_head_XM.Text = dt_ryxx_current.Rows[0]["XM"].ToString();
                    //性别
                    comboBox_head_XB.SelectedValue = dt_ryxx_current.Rows[0]["XB"].ToString();

                    //出生日期
                    dateTimePicker_head_CSRQ.Value = getDateFromString(dt_ryxx_current.Rows[0]["CSRQ"].ToString());
                    
                    if (dt_ryxx_current.Rows[0]["SFZH"] != null && dt_ryxx_current.Rows[0]["SFZH"] != DBNull.Value)
                    {
                        string[] sfzxx = null;
                        if (Common.CheckIDCard(dt_ryxx_current.Rows[0]["SFZH"].ToString()) == true)
                        {
                            sfzxx = Common.GetCardIdInfo(dt_ryxx_current.Rows[0]["SFZH"].ToString());

                        }

                        if (sfzxx != null && sfzxx.Length > 0)
                        {
                            dateTimePicker_head_CSRQ.Value = getDateFromString(sfzxx[1]);
                            comboBox_head_XB.SelectedValue = sfzxx[3]!=null ?sfzxx[3] :"";
                        }

                    }
                    

                    //身份证号
                    textBox_head_SFZH.Text = dt_ryxx_current.Rows[0]["SFZH"].ToString();

                    //联系电话
                    textBox_head_LXDH.Text = dt_ryxx_current.Rows[0]["LXDH"].ToString();

                    //健康档案号
                    textBox_head_JKDAH.Text = dt_ryxx_current.Rows[0]["JKDAH"].ToString();

                    //设定既往病史
                    setJwbs(textBox_head_JKDAH.Text);

                    //设定血型
                    setXX(textBox_head_JKDAH.Text);

                    dt_para.Rows[0]["JKDAH"] = textBox_head_JKDAH.Text;//健康档案号
                    dt_para.Rows[0]["LXDH"] = textBox_head_LXDH.Text;//联系电话
                    dt_para.Rows[0]["prgid"] = dt_ryxx_current.Rows[0]["PRgID"].ToString();//所属机构 
                    dt_para.Rows[0]["SFZH"] = textBox_head_SFZH.Text;//身份证号
                    dt_para.Rows[0]["XM"] = textBox_head_XM.Text;//姓名
                    dt_para.Rows[0]["YLJGBM"] = dt_ryxx_current.Rows[0]["YLJGBM"].ToString();//医疗结构编码
                    dt_para.Rows[0]["ND"] = dt_ryxx_current.Rows[0]["nd"].ToString();//年度 
                    dt_para.Rows[0]["gzz"] = UserInfo.gzz;//工作组
                    dt_para.Rows[0]["XB"] = comboBox_head_XB.Text;//性别
                     //体检状态
                    if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.健康体检表) == true)
                    {
                        //体检医生
                        if (dt_ryxx_current.Rows[0]["tjys_jktj"] != DBNull.Value)
                        {
                            comboBox_head_TJFZR.SelectedValue = dt_ryxx_current.Rows[0]["tjys_jktj"].ToString();
                        }

                        //体检日期
                        if (dt_ryxx_current.Rows[0]["TJSJ_jktj"] != DBNull.Value)
                        {
                            dateTimePicker_head_TJSJ.Value = DateTime.Parse(dt_ryxx_current.Rows[0]["TJSJ_jktj"].ToString());
                        }
                        else
                        {
                            dateTimePicker_head_TJSJ.Value = DateTime.Now;
                        }
                        //体检状态
                        if (dt_ryxx_current.Rows[0]["ZT_jktj"] != DBNull.Value)
                        {
                            comboBox_head_TJZT.SelectedValue = dt_ryxx_current.Rows[0]["ZT_jktj"].ToString();
                        }
                        else
                        {
                            comboBox_head_TJZT.SelectedValue = Common.ZT.否定状态;
                        }

                        //体检时间
                        dt_para.Rows[0]["tjsj"] = dt_ryxx_current.Rows[0]["TJSJ_jktj"].ToString();
                        
                    }
                    else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.生活自理能力评估) == true)
                        {
                            //体检医生
                            if (dt_ryxx_current.Rows[0]["tjys_nlpg"] != DBNull.Value)
                            {
                                comboBox_head_TJFZR.SelectedValue = dt_ryxx_current.Rows[0]["tjys_nlpg"].ToString();
                            }
                            //体检日期
                            if (dt_ryxx_current.Rows[0]["TJSJ_nlpg"] != DBNull.Value)
                            {
                                dateTimePicker_head_TJSJ.Value = DateTime.Parse(dt_ryxx_current.Rows[0]["TJSJ_nlpg"].ToString());
                            }
                            else
                            {
                                dateTimePicker_head_TJSJ.Value = DateTime.Now;
                            }
                            //体检状态
                            if (dt_ryxx_current.Rows[0]["ZT_nlpg"] != DBNull.Value)
                            {
                                comboBox_head_TJZT.SelectedValue = dt_ryxx_current.Rows[0]["ZT_nlpg"].ToString();
                            }
                        else
                        {
                            comboBox_head_TJZT.SelectedValue = Common.ZT.否定状态;
                        }
                            //体检时间
                            dt_para.Rows[0]["tjsj"] = dt_ryxx_current.Rows[0]["TJSJ_nlpg"].ToString();
                        }
                    else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.中医体质辨识) == true)
                    {
                        //体检医生
                        if (dt_ryxx_current.Rows[0]["tjys_zytz"] != DBNull.Value)
                        {
                            comboBox_head_TJFZR.SelectedValue = dt_ryxx_current.Rows[0]["tjys_zytz"].ToString();
                        }
                        //体检日期
                        if (dt_ryxx_current.Rows[0]["TJSJ_zytz"] != DBNull.Value)
                        {
                            dateTimePicker_head_TJSJ.Value = DateTime.Parse(dt_ryxx_current.Rows[0]["TJSJ_zytz"].ToString());
                        }
                        else
                        {
                            dateTimePicker_head_TJSJ.Value = DateTime.Now;
                        }

                        //体检状态
                        if (dt_ryxx_current.Rows[0]["ZT_zytz"] != DBNull.Value)
                        {
                            comboBox_head_TJZT.SelectedValue = dt_ryxx_current.Rows[0]["ZT_zytz"].ToString();
                        }
                        else
                        {
                            comboBox_head_TJZT.SelectedValue = Common.ZT.否定状态;
                        }

                        //体检时间
                        dt_para.Rows[0]["tjsj"] = dt_ryxx_current.Rows[0]["TJSJ_zytz"].ToString();
                    }

                    var obj = panel_form_body_tj_body_body_body_jktj.Controls;
                    //将参数传递到下一页面
                     if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.健康体检表) == true)
                    {

                        //obj = panel_form_body_tj_body_body_body_jktj.Controls;

                        CommomSysInfo.TJFZR_BM = comboBox_head_TJFZR.SelectedValue != null ? comboBox_head_TJFZR.SelectedValue.ToString() : "";
                        CommomSysInfo.TJFZR_MC = comboBox_head_TJFZR.Text != null ? comboBox_head_TJFZR.Text.ToString() : "";

                        sysCommonForm syscommonform = (sysCommonForm)lnrjktj;
                        syscommonform.setParaToChild(dt_para);
                     
                     }else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.生活自理能力评估) == true)
                        {

                            //obj = panel_form_body_tj_body_body_body_jkzp.Controls;

                            CommomSysInfo.TJFZR_BM = comboBox_head_TJFZR.SelectedValue != null ? comboBox_head_TJFZR.SelectedValue.ToString() : "";
                            CommomSysInfo.TJFZR_MC = comboBox_head_TJFZR.Text != null ? comboBox_head_TJFZR.Text.ToString() : "";

                            sysCommonForm syscommonform = (sysCommonForm)lnrxwnlpg;
                            syscommonform.setParaToChild(dt_para);
                    }
                     else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.中医体质辨识) == true)
                     {
                         //obj = panel_form_body_tj_body_body_body_zytz.Controls;

                         CommomSysInfo.TJFZR_BM = comboBox_head_TJFZR.SelectedValue != null ? comboBox_head_TJFZR.SelectedValue.ToString() : "";
                         CommomSysInfo.TJFZR_MC = comboBox_head_TJFZR.Text != null ? comboBox_head_TJFZR.Text.ToString() : "";

                         sysCommonForm syscommonform = (sysCommonForm)lnrzytz;
                         syscommonform.setParaToChild(dt_para);
                     
                     }
                    


                    //if (obj != null)
                    //{
                    //    //体检医生
                    //    CommomSysInfo.TJFZR_BM = comboBox_head_TJFZR.SelectedValue != null ? comboBox_head_TJFZR.SelectedValue.ToString() : "";
                    //    CommomSysInfo.TJFZR_MC = comboBox_head_TJFZR.Text != null ? comboBox_head_TJFZR.Text.ToString() : "";

                    //    sysCommonForm syscommonform = (sysCommonForm)lnrjktj;
                    //    syscommonform.setParaToChild(dt_para);
                    //}
                }
            }
        }

        /// <summary>
        /// 设定既往病史
        /// </summary>
        /// <param name="JKDAH"></param>
        /// <returns></returns>
        public string setJwbs(string JKDAH)
        {
            label_jwbs.Text = "";

           jktjBll jktjbll = new jktjBll();
           string strjwbs = "";
           //按照档案号获取既往病史
           DataTable dt_jwbs = jktjbll.GetMoHuList(string.Format(" and d_grdabh='{0}' and d_jblx='疾病' ", JKDAH), "sql060");
           if (dt_jwbs != null && dt_jwbs.Rows.Count > 0)
           {
               DataTable dt_sjzd = jktjbll.GetMoHuList(string.Format(" and zdlxbm='jwsjb' "), "sql_select_sjzd");
               for(int i=0;i<dt_jwbs.Rows .Count ;i++){
                   string jwsbm=dt_jwbs.Rows [i]["d_jbmc"]!=null ? dt_jwbs.Rows [i]["d_jbmc"].ToString ():"";

                   string[] jwsbmlist = jwsbm.Split(new char[] { ',' });
                   if (jwsbmlist != null && jwsbmlist.Length > 0)
                   {
                       for (int j = 0; j < jwsbmlist.Length; j++)
                       {
                           DataRow[] dtrow = dt_sjzd.Select(string.Format(" zdbm='{0}'", jwsbmlist[j]));
                           if (dtrow != null && dtrow.Length > 0)
                           {
                               strjwbs = strjwbs + "  " + (dtrow[0]["zdmc"] != null ? dtrow[0]["zdmc"].ToString() : "");
                           }
                       }
                   }
               }
           }
           label_jwbs.Text = string.Format ( "慢病人群:{0}",strjwbs.Length >0?strjwbs:"无");
           return strjwbs;
        }
        
        /// <summary>
        /// 设定血型
        /// </summary>
        /// <param name="JKDAH"></param>
        /// <returns></returns>
        public void setXX(string JKDAH)
        {
            //label_jwbs.Text = "";

            jktjBll jktjbll = new jktjBll();
            string strjwbs = "";
            //按照档案号获取档案血型
            DataTable dt_xx = jktjbll.GetMoHuList(string.Format(" and d_grdabh='{0}' ", JKDAH), "sql_xx_select");
            if (dt_xx != null && dt_xx.Rows.Count > 0)
            {
                string xx =dt_xx.Rows[0]["d_xx"]!=null ? dt_xx.Rows[0]["d_xx"].ToString():" ";
                string rh = dt_xx.Rows[0]["d_sfrhyx"] != null ? dt_xx.Rows[0]["d_sfrhyx"].ToString() : " ";
                label_jwbs.Text = string.Format("{0} \n\t血型:{1} RH阴性:{2} ", label_jwbs.Text, xx, rh);

            }
            else
            {
                label_jwbs.Text = string.Format("{0} \n\t血型:{1} RH阴性:{2} ", label_jwbs.Text, "  ", "  ");
            }
        }

        /// <summary>
        /// 初始化人员信息
        /// </summary>
        private DataTable initList_ryxx(string strWhere)
        {

            DataTable dt_ryxx = getList_ryxx(strWhere);
            if (dt_ryxx != null && dt_ryxx.Rows.Count > 0)
            {
                listBox_ryxx.DataSource = null;
                for (int i = 0; i < dt_ryxx.Rows.Count; i++)
                {
                    if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.健康体检表) == true)
                    {
                        if (dt_ryxx.Rows[i]["zt_jktj"] != DBNull.Value && dt_ryxx.Rows[i]["zt_jktj"].ToString().Equals("1"))
                        {
                            dt_ryxx.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx.Rows[i]["XM"].ToString().PadRight(4, '　') + "完";
                        }
                        else
                        {
                            dt_ryxx.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx.Rows[i]["XM"].ToString().PadRight(4, '　') + "未";
                        }
                    }
                    else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.生活自理能力评估 ) == true)
                    {
                        if (dt_ryxx.Rows[i]["zt_nlpg"] != DBNull.Value && dt_ryxx.Rows[i]["zt_nlpg"].ToString().Equals("1"))
                        {
                            dt_ryxx.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx.Rows[i]["XM"].ToString().PadRight(4, '　') + "完";
                        }
                        else
                        {
                            dt_ryxx.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx.Rows[i]["XM"].ToString().PadRight(4, '　') + "未";
                        }
                    } if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.中医体质辨识) == true)
                    {
                        if (dt_ryxx.Rows[i]["zt_zytz"] != DBNull.Value && dt_ryxx.Rows[i]["zt_zytz"].ToString().Equals("1"))
                        {
                            dt_ryxx.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx.Rows[i]["XM"].ToString().PadRight(4, '　') + "完";
                        }
                        else
                        {
                            dt_ryxx.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxx.Rows[i]["XM"].ToString().PadRight(4, '　') + "未";
                        }
                    }

                    dt_ryxx.Rows[i]["ValueMember"] = dt_ryxx.Rows[i]["SFH"].ToString();
                }

                listBox_ryxx.DataSource = dt_ryxx;
                listBox_ryxx.DisplayMember = "DisplayMember";
                listBox_ryxx.ValueMember = "ValueMember";

                //保存列表数据
                dt_list_tjryxx = dt_ryxx.Copy();
                return dt_ryxx;
            }
            else
            {
                if (comboBox_cunzhuang.SelectedValue != null && comboBox_cunzhuang.SelectedValue.ToString().Length > 0)
                {

                    // MessageBox.Show("没有取到对应的体检人员信息，请确认！");
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 初始化人员信息
        /// </summary>
        private void setRyxxListZt()
        {
            DataTable dt_ryxxDataSource = (DataTable)listBox_ryxx.DataSource;
            if (dt_ryxxDataSource != null && dt_ryxxDataSource.Rows.Count > 0)
            {
                //listBox_ryxx.DataSource = null;
                for (int i = 0; i < dt_ryxxDataSource.Rows.Count; i++)
                {
                    if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.健康体检表) == true)
                    {
                        if (dt_ryxxDataSource.Rows[i]["zt_jktj"] != DBNull.Value && dt_ryxxDataSource.Rows[i]["zt_jktj"].ToString().Equals("1"))
                        {
                            dt_ryxxDataSource.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxxDataSource.Rows[i]["XM"].ToString().PadRight(4, '　') + "完";
                        }
                        else
                        {
                            dt_ryxxDataSource.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxxDataSource.Rows[i]["XM"].ToString().PadRight(4, '　') + "未";
                        }
                    }
                    else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.生活自理能力评估) == true)
                    {
                        if (dt_ryxxDataSource.Rows[i]["zt_nlpg"] != DBNull.Value && dt_ryxxDataSource.Rows[i]["zt_nlpg"].ToString().Equals("1"))
                        {
                            dt_ryxxDataSource.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxxDataSource.Rows[i]["XM"].ToString().PadRight(4, '　') + "完";
                        }
                        else
                        {
                            dt_ryxxDataSource.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxxDataSource.Rows[i]["XM"].ToString().PadRight(4, '　') + "未";
                        }
                    } if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.中医体质辨识) == true)
                    {
                        if (dt_ryxxDataSource.Rows[i]["zt_zytz"] != DBNull.Value && dt_ryxxDataSource.Rows[i]["zt_zytz"].ToString().Equals("1"))
                        {
                            dt_ryxxDataSource.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxxDataSource.Rows[i]["XM"].ToString().PadRight(4, '　') + "完";
                        }
                        else
                        {
                            dt_ryxxDataSource.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(7, ' ') + dt_ryxxDataSource.Rows[i]["XM"].ToString().PadRight(4, '　') + "未";
                        }
                    }

                    dt_ryxxDataSource.Rows[i]["ValueMember"] = dt_ryxxDataSource.Rows[i]["SFH"].ToString();
                }

                listBox_ryxx.Refresh();
                //listBox_ryxx.DataSource = dt_ryxx;
                //listBox_ryxx.DisplayMember = "DisplayMember";
                //listBox_ryxx.ValueMember = "ValueMember";

                //保存列表数据
                dt_list_tjryxx = dt_ryxxDataSource.Copy();
            }
            
        }

        /// <summary>
        /// 取得人员信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        private DataTable getList_ryxx(string strWhere)
        {

            //DBAccess dBAccess = new DBAccess();
            jktjBll jktjbll = new jktjBll();

            DataTable dt_ryxx = jktjbll.GetMoHuList(string.Format (" and YLJGBM='{0}' {1}",UserInfo.Yybm ,strWhere), "sql_ryxx_main_list");

            if (dt_ryxx != null && dt_ryxx.Rows.Count > 0)
            {

                return dt_ryxx;
            }
            return null;
        }

        /// <summary>
        /// 设定获取人员信息的条件
        /// </summary>
        private bool selectRyxx(bool isTxm)
        {
            string sqlWhere = "";

            //通过条形码检索人员时，只考虑条形码的条件

             //体检号
            if (textBox_TJBH.Text.Trim().Length > 0 )
            {
                //按照条码号取得健康档案号
                string jkdahTem = "";
                if (textBox_TJBH.Text.Trim ().Length ==14)
                {
                    jkdahTem = textBox_TJBH.Text.Trim().Substring(0, 12);

                }
                sqlWhere = sqlWhere + string.Format(" and (jkdah = '{0}' or tjbm='{1}')", textBox_TJBH.Text, jkdahTem);
            }


            if (isTxm == false)
            {
                //村庄
                if (comboBox_cunzhuang.SelectedValue != null && comboBox_cunzhuang.SelectedValue.ToString().Length > 0)
                {
                    sqlWhere = sqlWhere + " and prgid = '" + comboBox_cunzhuang.SelectedValue.ToString() + "'";
                }

                //姓名
                if (textBox_xm.Text.Trim().Length > 0)
                {
                    sqlWhere = sqlWhere + " and XM like '%" + textBox_xm.Text.Trim() + "%'";
                }

                //体检状态
                if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.健康体检表) == true)
                {
                    //体检工作组
                    sqlWhere = sqlWhere + string.Format("  and (GZZBM_jktj='{0}' or GZZBM_jktj is null) ", UserInfo.gzz);
                    //体检时间
                    if (dateTimePicker_tjsj.Checked == true)
                    {
                        sqlWhere = sqlWhere + " and TJSJ_jktj = '" + dateTimePicker_tjsj.Value.ToString("yyy-MM-dd") + "'";
                    }
                    //体检状态
                    string strTJZT = "";
                    if (checkBox_wtj.Checked != checkBox_ytj.Checked)
                    {
                        if (checkBox_wtj.Checked == true)
                        {
                            strTJZT = string.Format(" and (ZT_jktj='{0}' or ZT_jktj is null ) ", Common.ZT.否定状态);
                        }
                        else
                        {
                            strTJZT = string.Format(" and  ZT_jktj='{0}' ", Common.ZT.确定状态);
                        }
                    }
                    sqlWhere = sqlWhere + strTJZT;

                    sqlWhere = sqlWhere + " order by ZT_jktj ";

                }
                else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.生活自理能力评估) == true)
                {
                    //体检工作组
                    sqlWhere = sqlWhere + string.Format("  and (GZZBM_nlpg='{0}' or GZZBM_nlpg is null) ", UserInfo.gzz);
                    //体检时间
                    if (dateTimePicker_tjsj.Checked == true)
                    {
                        sqlWhere = sqlWhere + " and TJSJ_nlpg = '" + dateTimePicker_tjsj.Value.ToString("yyy-MM-dd") + "'";
                    }
                    //体检状态
                    string strTJZT = "";
                    if (checkBox_wtj.Checked != checkBox_ytj.Checked)
                    {
                        if (checkBox_wtj.Checked == true)
                        {
                            strTJZT = string.Format(" and (ZT_nlpg='{0}' or ZT_nlpg is null ) ", Common.ZT.否定状态);
                        }
                        else
                        {
                            strTJZT = string.Format(" and  ZT_nlpg='{0}' ", Common.ZT.确定状态);
                        }
                    }
                    sqlWhere = sqlWhere + strTJZT;
                    sqlWhere = sqlWhere + " order by ZT_nlpg ";
                }
                else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.中医体质辨识) == true)
                {
                    //体检工作组
                    sqlWhere = sqlWhere + string.Format("  and (GZZBM_zytz='{0}' or GZZBM_zytz is null) ", UserInfo.gzz);
                    //体检时间
                    if (dateTimePicker_tjsj.Checked == true)
                    {
                        sqlWhere = sqlWhere + " and TJSJ_zytz = '" + dateTimePicker_tjsj.Value.ToString("yyy-MM-dd") + "'";
                    }
                    //体检状态
                    string strTJZT = "";
                    if (checkBox_wtj.Checked != checkBox_ytj.Checked)
                    {
                        if (checkBox_wtj.Checked == true)
                        {
                            strTJZT = string.Format(" and (ZT_zytz='{0}' or ZT_zytz is null ) ", Common.ZT.否定状态);
                        }
                        else
                        {
                            strTJZT = string.Format(" and  ZT_zytz='{0}' ", Common.ZT.确定状态);
                        }
                    }
                    sqlWhere = sqlWhere + strTJZT;

                    sqlWhere = sqlWhere + " order by ZT_zytz ";
                }
            }

            DataTable dt = initList_ryxx(sqlWhere);
            //保存列表数据
            dt_list_tjryxx = dt;

            if (dt == null)
            {
                button_rylb.Text = "人员列表(0)";
                listBox_ryxx.DataSource = null;

                return false;
            }
            else
            {
                button_rylb.Text = string.Format("人员列表({0})", dt.Rows.Count.ToString());
                DataRowView drv = listBox_ryxx.SelectedItem as DataRowView;
                //if (drv != null)
                //{
                //    //initHead(" and SFH like '" + drv[listBox_ryxx.ValueMember] + "' ");
                //    //selectFromDb();
                //}
                return true;
            }

        }
        /// <summary>
        /// 按照条件取得健康档案号
        /// </summary>
        /// <returns></returns>
        public string getJkdahByTxm()
        {
            string sqlWhere = "";
            string jkdah = "";
            try
            {
                //体检号
                if (textBox_TJBH.Text.Trim().Length > 0)
                {
                    //sql_get_tjry_txm
                    DBAccess dBAccess = new DBAccess();
                    string sql_Sjzd = Common.getSql("sql_get_tjry_txm", "");

                    sql_Sjzd = string.Format(sql_Sjzd, UserInfo.Yybm, textBox_TJBH.Text.Trim());

                    DataTable dt_ryxx = dBAccess.ExecuteQueryBySql(sql_Sjzd);


                    if (dt_ryxx != null && dt_ryxx.Rows.Count > 0)
                    {
                        jkdah = dt_ryxx.Rows[0]["jkdah"].ToString();
                        //return dt_ryxx;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return jkdah;
        }

        /// <summary>
        /// 查询按钮的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click(object sender, EventArgs e)
        {
            if (selectRyxx(false) == true)
            {
                initHead("");

            }

        }
        #endregion

        #region 删除人员

        private bool DeleteJktjSignname(string grdabh)
        {
            if (SignnameGroupList != null && SignnameGroupList.Count > 0)
            {
                TJClient.Signname.Operation.DeleteJktjSignname(grdabh, SignnameGroupList);
            }
            return true;
        }

        /// <summary>
        /// 删除人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string msg = "删除当前选中人员信息？";
            //取得当前要上传的人的信息
            int index = listBox_ryxx.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("请选择要删除的人员信息！");
                return;
            }
            //取得要删除的人员的信息
            if (dt_list_tjryxx != null && dt_list_tjryxx.Rows.Count > 0)
            {
                string xm = dt_list_tjryxx.Rows[index]["xm"].ToString();
                msg = msg + string.Format(" [{0}]", xm);
            }

            DialogResult result;
            result = MessageBox.Show(msg, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                //删除人员信息
                jktjBll jktjbll = new jktjBll();
                bool resultDel = jktjbll.Del(dt_list_tjryxx, index, "sql095");
                string message = "";

                if (resultDel == true)
                {
                    //在此处再删除签名
                    DeleteJktjSignname(dt_list_tjryxx.Rows[index]["jkdah"].ToString());
                    MessageBox.Show("删除成功！");
                    selectRyxx(false);
                    return;
                }
            }
        }
        #endregion

        #region 公用方法
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
        /// 按照控件名称获取控件的值
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private string GetControlValueByName(string ControlName)
        {
            Control[] controls = Controls.Find(ControlName, true);
            Control control = null;
            if (controls == null || controls.Length <= 0)
            {
                return "";
            }
            else
            {
                control = controls[0];
            }
            string value = "";
            //text
            if (ControlName.IndexOf(controlType_textBox) > -1)
            {
                TextBox TextBox_tem = (TextBox)control;
                value = TextBox_tem.Text;
            }
            //checkBox
            else if (ControlName.IndexOf(controlType_checkBox) > -1)
            {
                CheckBox checkBox_tem = (CheckBox)control;
                if (checkBox_tem.Checked == true)
                {
                    value = checkBox_tem.Tag.ToString();
                }
                else
                {
                    value = "";
                }
            }
            //radioButton
            else if (ControlName.IndexOf(controlType_radioButton) > -1)
            {
                RadioButton radioButton_tem = (RadioButton)control;
                if (radioButton_tem.Checked == true)
                {
                    value = radioButton_tem.Tag.ToString();
                }
                else
                {
                    value = "";
                }
            }
            //label
            else if (ControlName.IndexOf(controlType_lable) > -1)
            {
                LinkLabel LinkLabel_tem = (LinkLabel)control;
                value = LinkLabel_tem.Text;
            }
            //LinkLabel
            else if (ControlName.IndexOf(controlType_LinkLabel) > -1)
            {
                LinkLabel LinkLabel_tem = (LinkLabel)control;
                if (LinkLabel_tem.Tag != null)
                {
                    value = LinkLabel_tem.Tag.ToString();
                }
                // value = LinkLabel_tem.Text;
            }

            return value;
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


        /// <summary>
        /// 将页面上的数据保存到dtControl中
        /// </summary>
        private void getTjResultDtFromPage()
        {
            //取得控件的值
            for (int i = 0; i < dtControl.Rows.Count; i++)
            {
                dtControl.Rows[i]["value"] = GetControlValueByName(dtControl.Rows[i]["ControlId"].ToString());
            }
        }

        /// <summary>
        /// 页面关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jktj_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                System.Environment.Exit(System.Environment.ExitCode);
                this.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }
        
        #endregion

        #region 菜单

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_download_Click(object sender, EventArgs e)
        {
            //设定菜单的样式
            setMenuStyle(sender, e);

            //清空主显示区的内容
            foreach (Control ctrl in panel_form_body.Controls)
            {
                if (ctrl.Name.Equals("panel_form_body_tj") == false)
                {
                    panel_form_body.Controls.Remove(ctrl);
                }
            }
            panel_form_body_tj.Visible = false;
            DataDownLoad datadownload = new DataDownLoad();
            datadownload.setPara(dt_para);
            datadownload.TopLevel = false;
            datadownload.Parent = panel_form_body;
            datadownload.Dock = DockStyle.Fill;
            datadownload.FormBorderStyle = FormBorderStyle.None;
            datadownload.Show();

        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_查询_Click(object sender, EventArgs e)
        {
            //设定菜单的样式
            setMenuStyle(sender, e);

            //清空主显示区的内容
            foreach (Control ctrl in panel_form_body.Controls)
            {
                if (ctrl.Name.Equals("panel_form_body_tj") == false)
                {
                    panel_form_body.Controls.Remove(ctrl);
                }
            }
            panel_form_body_tj.Visible = false;
            LnrSelect lnrselect = new LnrSelect();
            lnrselect.TopLevel = false;
            lnrselect.Parent = panel_form_body;
            lnrselect.Dock = DockStyle.Fill;
            lnrselect.FormBorderStyle = FormBorderStyle.None;
            lnrselect.Show();
        }

        /// <summary>
        /// 登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_人员登记_Click(object sender, EventArgs e)
        {
            //设定菜单的样式
            setMenuStyle(sender, e);

            //清空主显示区的内容
            foreach (Control ctrl in panel_form_body.Controls)
            {
                if (ctrl.Name.Equals("panel_form_body_tj") == false)
                {
                    panel_form_body.Controls.Remove(ctrl);
                }
            }

            //设定当前操作
            CommomSysInfo.TJTYPE = Common.TJTYPE.登记;

            panel_form_body_tj.Visible = false;
            LnrDj lnrdj = new LnrDj();
            lnrdj.TopLevel = false;
            lnrdj.Parent = panel_form_body;
            lnrdj.Dock = DockStyle.Fill;
            lnrdj.setPara(dt_para);
            lnrdj.FormBorderStyle = FormBorderStyle.None;
            lnrdj.Show();
        }

        /// <summary>
        /// 健康体检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_健康体检_Click(object sender, EventArgs e)
        {
            //设定菜单的样式
            setMenuStyle(sender, e);

            //设定当前操作
            CommomSysInfo.TJTYPE = Common.TJTYPE.健康体检表;

            //更新数据状态
            //setRyxxListZt();

            //健康体检
            panel_form_body_tj.Visible = true;
            //panel_form_body_tj_body_body_body_jktj.Controls.Clear();
            //lnrjktj.TopLevel = false;
            //panel_form_body_tj_body_body_body_zytz.SendToBack();
            //panel_form_body_tj_body_body_body_jkzp.SendToBack();
            //panel_form_body_tj_body_body_body_zytz.Visible =false ;
            //panel_form_body_tj_body_body_body_jkzp.Visible = false;
            //panel_form_body_tj_body_body_body_jktj.Visible = true;
           // panel_form_body_tj_body_body_body_jktj
            //panel_form_body_tj_body_body_body_jktj.Dock = DockStyle.Fill;
            //lnrjktj.Parent = panel_form_body_tj_body_body_body_jktj;

            //lnrjktj.Dock = DockStyle.Fill;
            //lnrjktj.setPara(dt_para);
            //lnrjktj.main_form = this;
            //lnrjktj.FormBorderStyle = FormBorderStyle.None;
            
            lnrjktj.Show();
            panel_form_body_tj_body_body_body_jktj.BringToFront();
            //显示选中的人员的 信息
            initHead("");
        }
        /// <summary>
        /// 中医体质
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_中医体质_Click(object sender, EventArgs e)
        {
            //设定菜单的样式
            setMenuStyle(sender, e);

            //设定当前操作
            CommomSysInfo.TJTYPE = Common.TJTYPE.中医体质辨识;

            //更新数据状态
            //setRyxxListZt();

            //中医体质辨识
            panel_form_body_tj.Visible = true;
            //panel_form_body_tj_body_body_body_jktj.Controls.Clear();
            //lnrzytz.TopLevel = false;
            //panel_form_body_tj_body_body_body_zytz.Visible = true;
           // panel_form_body_tj_body_body_body_zytz.Dock = DockStyle.Fill;
            //panel_form_body_tj_body_body_body_jkzp.SendToBack();
            //panel_form_body_tj_body_body_body_jktj.SendToBack();
            //panel_form_body_tj_body_body_body_jkzp.Visible = false;
            //panel_form_body_tj_body_body_body_jktj.Visible = false;
            //lnrzytz.Parent = panel_form_body_tj_body_body_body_zytz;
            //lnrzytz.Dock = DockStyle.Fill;
            //lnrzytz.setPara(dt_para);
            //lnrzytz.main_form = this;
            //lnrzytz.FormBorderStyle = FormBorderStyle.None;
         
            lnrzytz.Show();
            panel_form_body_tj_body_body_body_zytz.BringToFront();
            //显示选中的人员的 信息
            initHead("");
        }

        /// <summary>
        /// 健康自评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_健康自评_Click(object sender, EventArgs e)
        {
            //设定菜单的样式
            setMenuStyle(sender, e);

            //设定当前操作
            CommomSysInfo.TJTYPE = Common.TJTYPE.生活自理能力评估;

            //更新数据状态
            //setRyxxListZt();

            //健康自评
            panel_form_body_tj.Visible = true;
            //panel_form_body_tj_body_body_body_jktj.Controls.Clear();
            //lnrxwnlpg.TopLevel = false;
            //panel_form_body_tj_body_body_body_zytz.SendToBack();
            //panel_form_body_tj_body_body_body_jktj.SendToBack();

            //panel_form_body_tj_body_body_body_zytz.Visible = false;
            //panel_form_body_tj_body_body_body_jkzp.Visible = true;
            //panel_form_body_tj_body_body_body_jkzp.Dock = DockStyle.Fill;
            //panel_form_body_tj_body_body_body_jktj.Visible = false;
            //lnrxwnlpg.Parent = panel_form_body_tj_body_body_body_jkzp;
            //lnrxwnlpg.Dock = DockStyle.Fill;
            //lnrxwnlpg.setPara(dt_para);
            //lnrxwnlpg.main_form = this;
            //lnrxwnlpg.FormBorderStyle = FormBorderStyle.None;
            
            lnrxwnlpg.Show();

            panel_form_body_tj_body_body_body_jkzp.BringToFront();

            //显示选中的人员的 信息
            initHead("");
        }

        /// <summary>
        /// 化验数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_检验化验_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("模块暂时停用！");
           // return;
            //设定菜单的样式
            setMenuStyle(sender, e);

            //清空主显示区的内容
            foreach (Control ctrl in panel_form_body.Controls)
            {
                if (ctrl.Name.Equals("panel_form_body_tj") == false)
                {
                    panel_form_body.Controls.Remove(ctrl);
                }
            }
            panel_form_body_tj.Visible = false;
            lis_new lisNew = new lis_new();
            lisNew.TopLevel = false;
            lisNew.Parent = panel_form_body;
            lisNew.Dock = DockStyle.Fill;
            lisNew.FormBorderStyle = FormBorderStyle.None;
            lisNew.Show();
        }
        /// <summary>
        /// 系统设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_系统设置_Click(object sender, EventArgs e)
        {
            //设定菜单的样式
            setMenuStyle(sender, e);
            //Form_sysEdit form_sys = new Form_sysEdit();
            //form_sys.ShowDialog();

            //清空主显示区的内容
            foreach (Control ctrl in panel_form_body.Controls)
            {
                if (ctrl.Name.Equals("panel_form_body_tj") == false)
                {
                    panel_form_body.Controls.Remove(ctrl);
                }
            }
            panel_form_body_tj.Visible = false;
            Form_sysEdit form_sysedit = new Form_sysEdit();
            form_sysedit.TopLevel = false;
            form_sysedit.Parent = panel_form_body;
            form_sysedit.Dock = DockStyle.Fill;
            form_sysedit.FormBorderStyle = FormBorderStyle.None;
            form_sysedit.Show();
        }

        /// <summary>
        /// 检验信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_检验信息_Click(object sender, EventArgs e)
        {
            //设定菜单的样式
            setMenuStyle(sender, e);
            //Form_sysEdit form_sys = new Form_sysEdit();
            //form_sys.ShowDialog();

            //清空主显示区的内容
            foreach (Control ctrl in panel_form_body.Controls)
            {
                if (ctrl.Name.Equals("panel_form_body_tj") == false)
                {
                    panel_form_body.Controls.Remove(ctrl);
                }
            }
            panel_form_body_tj.Visible = false;
            //LisImport lisimport = new LisImport();
            //lisimport.TopLevel = false;
            //lisimport.Parent = panel_form_body;
            //lisimport.Dock = DockStyle.Fill;
            //lisimport.FormBorderStyle = FormBorderStyle.None;
            //lisimport.Show();
        }


        /// <summary>
        /// 数据上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_uploads_Click(object sender, EventArgs e)
        {
            //设定菜单的样式
            setMenuStyle(sender, e);

        }

        /// <summary>
        /// 心电图上传处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_xdt_img_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //Form_xdt_upload form_xdt_upload = new Form_xdt_upload();

            //form_xdt_upload.Show();

        }

        /// <summary>
        /// 数据导入处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_数据上传_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DataImportFromExcel dataimportfromexcel = new DataImportFromExcel();
            dataimportfromexcel.Show();
        }

        #region 菜单样式设定
        /// <summary>
        /// 菜单中被选中的菜单
        /// </summary>
       public Control Control_pictureBox_select = null;
       public Control Control_label_select = null;

        /// <summary>
        /// 设定菜单的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setMenuStyle(object sender, EventArgs e)
        {
            Control  control_tem=(Control)sender;
            label_jwbs.Text = "";

            //button_download
            if (control_tem.Name.Equals("pictureBox_数据下载") == true || control_tem.Name.Equals("label_数据下载") == true)
            {
                setDefut(Control_pictureBox_select, Control_label_select);

                setFocus(pictureBox_数据下载, label_数据下载);
            }
            else if (control_tem.Name.Equals("pictureBox_查询") == true || control_tem.Name.Equals("label_查询") == true)
            {
                setDefut(Control_pictureBox_select, Control_label_select);

                setFocus(pictureBox_查询, label_查询);
            }
            else if (control_tem.Name.Equals("pictureBox_人员登记") == true || control_tem.Name.Equals("label_人员登记") == true)
            {
                setDefut(Control_pictureBox_select, Control_label_select);

                setFocus(pictureBox_人员登记, label_人员登记);
            }
            else if (control_tem.Name.Equals("pictureBox_健康体检") == true || control_tem.Name.Equals("label_健康体检") == true)
            {
                setDefut(Control_pictureBox_select, Control_label_select);

                setFocus(pictureBox_健康体检, label_健康体检);
            }
            else if (control_tem.Name.Equals("pictureBox_中医体质") == true || control_tem.Name.Equals("label_中医体质") == true)
            {
                setDefut(Control_pictureBox_select, Control_label_select);

                setFocus(pictureBox_中医体质, label_中医体质);
            }
            else if (control_tem.Name.Equals("pictureBox_健康自评") == true || control_tem.Name.Equals("label_健康自评") == true)
            {
                setDefut(Control_pictureBox_select, Control_label_select);

                setFocus(pictureBox_健康自评, label_健康自评);
            }
            else if (control_tem.Name.Equals("pictureBox_检验化验") == true || control_tem.Name.Equals("label_检验化验") == true)
            {
                setDefut(Control_pictureBox_select, Control_label_select);

                setFocus(pictureBox_检验化验, label_检验化验);
            }
            else if (control_tem.Name.Equals("button_uploads") == true || control_tem.Name.Equals("pictureBox_数据下载") == true)
            {
            }
            else if (control_tem.Name.Equals("pictureBox_系统设置") == true || control_tem.Name.Equals("label_系统设置") == true)
            {
                setDefut(Control_pictureBox_select, Control_label_select);

                setFocus(pictureBox_系统设置, label_系统设置);
            }
            else if (control_tem.Name.Equals("pictureBox_检验信息") == true || control_tem.Name.Equals("label_检验信息") == true)
            {
                setDefut(Control_pictureBox_select, Control_label_select);

                setFocus(pictureBox_检验信息, label_检验信息);
            }
            else
            {
            }
        }

        private void setDefut(Control Control_pictureBox_select_para, Control Control_label_select_para)
        {
            if (Control_pictureBox_select_para != null)
            {
                ((PictureBox)Control_pictureBox_select_para).BorderStyle = BorderStyle.None;
            }
            if (Control_label_select_para != null)
            {
                Control_label_select_para.ForeColor = Color.White;
                Control_label_select_para.Font = new Font("宋体", 9, FontStyle.Regular);
            }
        }

        private void setFocus(Control Control_pictureBox_select_para, Control Control_label_select_para)
        {
            Control_pictureBox_select = Control_pictureBox_select_para;
            Control_label_select = Control_label_select_para;
            ((PictureBox)Control_pictureBox_select_para).BorderStyle = BorderStyle.FixedSingle;
            //Control_label_select_para.ForeColor = Color.FromArgb(224, 224, 224);
            Control_label_select_para.ForeColor = Color.Red;
            Control_label_select_para.Font = new Font("宋体", 9, FontStyle.Bold);
        }

        #endregion

        /// <summary>
        /// 上次体检结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_sctjjg_CheckedChanged(object sender, EventArgs e)
        {

            //信息是否保存过
            if (Common.commnoIsSaved() == false)
            {
                return;
            }
            //参数初始化
            //上次提交结果
            CommomSysInfo.IsSctjjg = checkBox_sctjjg.Checked == true ? "1" : "0";
            if (lnrjktj != null)
            {
                lnrjktj.lastDataTopage();
            }
        }

        /// <summary>
        /// 用药情况提醒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_yyqk_CheckedChanged(object sender, EventArgs e)
        {
            //信息是否保存过
            if (Common.commnoIsSaved() == false)
            {
                return;
            }
            //参数初始化
            //上次提交结果
            CommomSysInfo.IsSctjjg_yyqk = checkBox_yyqk.Checked == true ? "1" : "0";
            if (lnrjktj != null)
            {
                lnrjktj.lastDataTopage();
            }
        }
        #endregion

        /// <summary>
        /// 辅助录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_fzlr_CheckedChanged(object sender, EventArgs e)
        {
            //参数初始化
            //辅助录入
            CommomSysInfo.IsFzlr = checkBox_fzlr.Checked == true ? "1" : "0";
        }

        /// <summary>
        /// 腰围换算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_autoYw_CheckedChanged(object sender, EventArgs e)
        {
            //参数初始化
            //腰围换算
            CommomSysInfo.IsYwhs = checkBox_autoYw.Checked == true ? "1" : "0";
        }

        /// <summary>
        /// 体检医生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_head_TJFZR_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommomSysInfo.TJFZR_BM = comboBox_head_TJFZR.SelectedValue != null ? comboBox_head_TJFZR.SelectedValue.ToString() : "";
            CommomSysInfo.TJFZR_MC = comboBox_head_TJFZR.Text != null ? comboBox_head_TJFZR.Text.ToString() : "";
            if (CommomSysInfo.TJTYPE!=null && CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.健康体检表))
            { 
            }
            else if (CommomSysInfo.TJTYPE != null && CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.生活自理能力评估))
            {
                lnrxwnlpg.setpageInit();
            }
            else if (CommomSysInfo.TJTYPE!=null && CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.中医体质辨识))
            {
                lnrzytz.setpageInit();
            }
        }

        /// <summary>
        /// 体检状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_head_TJZT_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommomSysInfo.tjzt= comboBox_head_TJZT.SelectedValue != null ? comboBox_head_TJZT.SelectedValue.ToString() : "";
        }

        /// <summary>
        /// 体检日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePicker_head_TJSJ_ValueChanged(object sender, EventArgs e)
        {
            CommomSysInfo.tjsj = dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd");
            if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.健康体检表))
            {
            }
            else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.生活自理能力评估))
            {
                lnrxwnlpg.setpageInit();
            }
            else if (CommomSysInfo.TJTYPE.Equals(Common.TJTYPE.中医体质辨识))
            {
                lnrzytz.setpageInit();
            }
        }

        /// <summary>
        /// 批量同步检验结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_lis_Click(object sender, EventArgs e)
        {
            try
            {
               // string JKDAH = getValueFromDt(dt_paraFromParent, 0, "JKDAH");
                if (dt_list_tjryxx != null && dt_list_tjryxx.Rows.Count> 0)
                {
                    Common common = new Common();
                    //DataTable dt_jktj = new DataTable();
                    //dt_jktj.Rows.Add();
                    //dt_jktj.Columns.Add("JKDAH");

                    //dt_jktj.Rows[0]["JKDAH"] = JKDAH;
                    string msg = common.updateLis_jktj(dt_list_tjryxx);
                    if (msg.Length > 0)
                    {
                        MessageBox.Show(msg);
                    }
                    else
                    {
                        MessageBox.Show("同步完成！");
                    }
                }
                else
                {
                    MessageBox.Show("请选择要同步的对象！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 公卫数据下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void btn_pz_Click(object sender, EventArgs e)
        {
            if (listBox_ryxx.DataSource == null)
            {
                MessageBox.Show("请选中人员再进行拍照。");
                return;
            }

            if (dt_list_tjryxx != null && dt_list_tjryxx.Rows.Count > 0)
            {
                int i = listBox_ryxx.SelectedIndex == -1 ? 0 : listBox_ryxx.SelectedIndex;
                DataRow dt_ryxx = dt_list_tjryxx.Rows[i];
                Form_photo form = new Form_photo();
                string jkdah = dt_ryxx["JKDAH"].ToString();
                string sfzh = dt_ryxx["SFZH"].ToString();
                string xm = dt_ryxx["XM"].ToString();
                string createDate=dateTimePicker_head_TJSJ.Value .ToString ("yyyy-MM-dd");
                form.jkdah = jkdah;
                form.sfzh = sfzh;
                TJClient.Signname.Operation.HealthExaminedUserInfoInit(xm,jkdah,sfzh,createDate);
                form.ShowDialog();
            }
            else MessageBox.Show("请选中人员再进行拍照。");
        }


        
    }
}
