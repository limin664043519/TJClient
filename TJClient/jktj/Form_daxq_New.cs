using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.Configuration;
using TJClient;
using TJClient.sys;
using TJClient.Common;
using TJClient.sys.Bll;
using TJClient.ComForm;
using TJClient.jktj;
namespace FBYClient
{
    public partial class Form_daxq_New : sysCommonForm
    {
        /// <summary>
        /// 存放原始的结果集
        /// </summary>
        private static DataTable dt_tem = new DataTable();

        private bool drpflag = false;

        public string czList = "";

        /// <summary>
        /// 健康档案人口学资料（T_DA_JKDA_RKXZL）
        /// </summary>
        public DataTable dt_T_DA_JKDA_RKXZL = null;

        ///// <summary>
        ///// 家庭档案表（T_DA_JTDA）
        ///// </summary>
        //public DataTable dt_T_DA_JTDA = null;

        /// <summary>
        /// 个人健康特征表（T_DA_JKDA_GRJKTZ）
        /// </summary>
        public DataTable dt_T_DA_JKDA_GRJKTZ = null;

        /// <summary>
        /// 健康档案健康状况家族病史表（T_DA_JKDA_JKZK_JZBS）
        /// </summary>
        public DataTable dt_T_DA_JKDA_JKZK_JZBS = null;

        /// <summary>
        /// 健康档案健康状况既往病史表（T_DA_JKDA_JKZK_JWBS）
        /// </summary>
        public DataTable dt_T_DA_JKDA_JKZK_JWBS = null;

        /// <summary>
        /// 健康档案健康状况表（T_DA_JKDA_JKZK）
        /// </summary>
        public DataTable dt_T_DA_JKDA_JKZK = null;

        /// <summary>
        /// 健康档案生活习惯表（T_DA_JKDA_SHXG）
        /// </summary>
        public DataTable dt_T_DA_JKDA_SHXG = null;

        /// <summary>
        /// 疾病
        /// </summary>
        public DataTable dt_jb = null;

        /// <summary>
        /// 手术
        /// </summary>
        public DataTable dt_ss = null;

        /// <summary>
        /// 外伤
        /// </summary>
        public DataTable dt_ws = null;

        /// <summary>
        /// 输血
        /// </summary>
        public DataTable dt_sx = null;

        /// <summary>
        /// 家族病史
        /// </summary>
        public DataTable dt_jzs = null;


        /// <summary>
        /// 健康id
        /// </summary>
        public string str_id = "";

        /// <summary>
        /// 健康档案号
        /// </summary>
        public string str_jkdah = "";

        /// <summary>
        /// 身份证号
        /// </summary>
        public string str_sfzh = "";

        /// <summary>
        /// 姓名
        /// </summary>
        public string str_xm = "";

        /// <summary>
        /// groupbox的标题
        /// </summary>
        private int groupbox_title = 0;

        /// <summary>
        /// 村庄编码
        /// </summary>
        public string czbm = "";


        public Form_readSfzh form_readsfzh = new Form_readSfzh();

        public Form_daxq_New()
        {
            InitializeComponent();
        }

        #region 控件事件
        private void Form_downLoad_Load(object sender, EventArgs e)
        {

            form_readsfzh.Show();
            form_readsfzh.Owner = this;
            form_readsfzh.Visible = false;
            //疾病
            dataGridView_jb.AutoGenerateColumns = false;
            this.dateTime_jb_qzsj.Format = DateTimePickerFormat.Custom;
            this.dateTime_jb_qzsj.ShowUpDown = true;
            this.dateTime_jb_qzsj.CustomFormat = "yyyy-MM";

            //手术
            dataGridView_ss.AutoGenerateColumns = false;
            //外伤
            dataGridView_ws.AutoGenerateColumns = false;
            //输血
            dataGridView_sx.AutoGenerateColumns = false;
            //家族病史
            dataGridView_jzs.AutoGenerateColumns = false;

            //初始化下拉框的值
            initPage();

            //显示档案信息
            if (str_jkdah.Length > 0 || str_sfzh.Length > 0 || str_xm.Length > 0)
            {
                //检索条件
                string sqlWhere = "";
                if (str_jkdah.Length > 0)
                {
                    sqlWhere = string.Format(" {0} and D_GRDABH='{1}' ", sqlWhere, str_jkdah);
                }

                if (str_sfzh.Length > 0)
                {
                    sqlWhere = string.Format(" {0} and D_zjhqt='{1}' ", sqlWhere, str_sfzh);
                }

                if (str_xm.Length > 0)
                {
                    sqlWhere = string.Format(" {0} and D_XM='{1}' ", sqlWhere, str_xm);
                }

                setDataTopage(string.Format(sqlWhere, str_id));
            }
            else
            {
                initXzjgBycz(czbm);
            }

        }

        /// <summary>
        /// 下拉框初始化
        /// </summary>
        /// <returns></returns>
        public bool initPage()
        {
            try
            {
                //证件类型
                setDrp(cmb_zjlx2, "zjlb", true);
                cmb_zjlx2.SelectedValue = "1";
                //与户主关系
                setDrp(comboBox_D_YHZGX, "yhzgx", true);
                //性别
                setDrp(comboBox_D_XB, "xb_xingbie", true);
                //常住类型
                setDrp(comboBox_czlx, "jzzk", true);
                //民族
                setDrp(comboBox_D_MZ, "mz_minzu", true);
                //血型
                setDrp(comboBox_D_XX, "xuexing", true);
                //RH
                setDrp(comboBox_D_SFRHYX, "rhxx", true);
                //职业
                setDrp(comboBox_D_ZY, "zy_zhiye", true);
                //文化程度
                setDrp(comboBox_D_WHCD, "whcd", true);
                //劳动强度
                setDrp(comboBox_D_LDQD, "ldqd", true);
                //婚姻状况
                setDrp(comboBox_D_HYZK, "hyzk", true);
                //省
                setDrp(comboBox_D_SHENG, "", "sql051", true);
                //市
                setDrp(comboBox_D_SHI, "", "sql052", true);
                //档案类别
                setDrp(comboBox_D_DALB, "dalb", true);
                //药物过敏史
                setDrp(comboBox_D_GMS, "yw_youwu", true);
                //暴露史
                setDrp(comboBox_bls, "wy_wuyou", true);
                //遗传病史
                setDrp(comboBox_ycbs, "wy_wuyou", true);
                //残疾情况
                setDrp(comboBox_cjqk, "wy_wuyou", true);

                //所属机构
                setDrp(comboBox_ssjg, "", "sql085", true);
                //所属机构
                setDrp(comboBox_gx, "jzscy", true);

                //数据存贮
                setDrp(comboBox_sjcc, "", "sql_T_JK_systemConfig_select", false);

                //录入时间
                TextBox_lrsj.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //录入人
                // TextBox_lrr.Text = UserInfo .Username;

                //修改时间
                // TextBox_xgsj.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //修改人
                // TextBox_xgr.Text = UserInfo.Username;

                //初始化时隐藏户主信息
                //labts.Visible = false;
                //labhzsfzh.Visible = false;
                //hzxm.Visible = false;
            }
            catch (Exception ex)
            {

            }

            return true;
        }

        /// <summary>
        /// 初始化表结构
        /// </summary>
        /// <param name="dt_table"></param>
        /// <returns></returns>
        public DataTable initTable(string sqlcode)
        {
            DataTable dt = new DataTable();
            Form_daxqBll form_daxq = new Form_daxqBll();
            //获取数据库表结构
            dt = form_daxq.GetMoHuList("and 1=2", sqlcode);
            return dt;
        }

        /// <summary>
        /// 设定诊断信息
        /// </summary>
        /// <param name="pym"></param>
        public bool setListData(CheckedListBox chk, string zdCode)
        {
            //按照拼音码取得疾病诊断
            DataTable dt = Common.getsjzd(zdCode, "sql_select_sjzd");
            if (dt != null && dt.Rows.Count > 0)
            {
                chk.DataSource = dt;
                chk.DisplayMember = "ZDMC";
                chk.ValueMember = "ZDBM";
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 公用方法

        /// <summary>
        /// 绑定数据值
        /// </summary>
        /// <param name="drp"></param>
        /// <param name="initValue"></param>
        /// <returns></returns>
        public bool setDrp(ComboBox drp, string zdCode, bool ifkh)
        {
            try
            {
                drpflag = false;
                //获取结果集
                DataTable dt = Common.getsjzd(zdCode, "sql_select_sjzd");
                if (ifkh == true)
                {
                    DataRow dtRow = dt.NewRow();
                    dt.Rows.InsertAt(dtRow, 0);
                }

                drp.DisplayMember = "ZDMC";
                drp.ValueMember = "ZDBM";
                drp.DataSource = dt;
                drpflag = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Enter转换为tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Tab_KeyDown(object sender, KeyEventArgs e)
        {
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
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }

            }

            if (sender.GetType().ToString().Equals("System.Windows.Forms.ComboBox"))
            {
                if (e.KeyCode == Keys.Left)
                {
                    //shift+Tab
                    SendKeys.Send("+{Tab}");
                    e.Handled = false;
                }
                if (e.KeyCode == Keys.Right)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
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
                if (e.KeyCode == Keys.Down)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                e.Handled = false;
            }

        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            czList = "";
            this.Owner.Visible = true;
            this.Close();
        }

        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            if (lTextBox_D_GRDABH.Text.Trim().Length == 0)
            {
                MessageBox.Show("健康档案号为空，请生成健康档案号！");
                return;
            }

            if (comboBox_D_JWH.SelectedValue == null || comboBox_D_JWH.SelectedValue.ToString().Length == 0)
            {
                MessageBox.Show("[居/村委会] 必须选择！");
                return;
            }

            if (comboBox_D_YHZGX.SelectedValue == null)
            {
                MessageBox.Show("请选择户主关系！");
                comboBox_D_YHZGX.Focus();
                return;
            }

            if (comboBox_D_YHZGX.SelectedValue.ToString() != "1")
            {
                if (text_hzsfzh.Text.Trim() == "")
                {
                    MessageBox.Show("请填写户主身份证号！");
                    return;
                }
            }

            if (comboBox_ssjg.SelectedValue == null || comboBox_ssjg.SelectedValue.ToString().Length == 0)
            {
                MessageBox.Show("[所属机构] 必须选择！");
                return;
            }

            //if (comboBox_lrr.SelectedValue == null || comboBox_lrr.SelectedValue.ToString().Length == 0)
            //{
            //    MessageBox.Show("[录入人] 必须选择！");
            //    return;
            //}

            //string message = "";
            //if (checksfz_nl(out message) == false)
            //{
            //    MessageBox.Show(message);
            //}

            #region 人口学资料

            //初始化表结构
            dt_T_DA_JKDA_RKXZL = initTable("sql056");
            dt_T_DA_JKDA_RKXZL.Rows.Add();

            //与户主关系  健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_YHZGX.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_YHZGX"] = comboBox_D_YHZGX.SelectedValue.ToString();
            }

            //档案状态 健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (radioButton_D_DAZT_1.Checked)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_DAZT"] = "1";
            }
            else
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_DAZT"] = "0";
            }

            //健康档案号  健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_GRDABH"] = lTextBox_D_GRDABH.Text.PadLeft(12, '0');

            //户主身份证号
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_HZSFZH"] = text_hzsfzh.Text.Trim();
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_SFZH"] = text_hzsfzh.Text.Trim();

            //姓名 健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_XM"] = lTextBox_D_XM.Text;

            //性别 健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_XB.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_XB"] = comboBox_D_XB.SelectedValue.ToString();
            }

            //证件类型
            if (cmb_zjlx2.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_ZJLX"] = cmb_zjlx2.SelectedValue.ToString();
            }
            
            //证件编号 身份证号 健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_zjhqt"] = lTextBox_D_SFZH.Text;

            //出生日期    健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_CSRQ"] = dateTimePicker_D_CSRQ.Value.ToString("yyyy-MM-dd");

            //本人电话   健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_LXDH"] = lTextBox_D_LXDH.Text;

            //工作单位   健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_GZDW"] = lTextBox_D_GZDW.Text;

            //联系人电话   健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_LXRDH"] = lTextBox_D_LXRDH.Text;

            //联系人姓名   健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_LXRXM"] = lTextBox_D_LXRXM.Text;

            //常住类型 居住状况   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_czlx.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_JZZK"] = comboBox_czlx.SelectedValue.ToString();
            }

            //民族   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_MZ.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_MZ"] = comboBox_D_MZ.SelectedValue.ToString();
            }

            //职业    健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_ZY.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_ZY"] = comboBox_D_ZY.SelectedValue.ToString();
            }

            //文化程度   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_WHCD.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_WHCD"] = comboBox_D_WHCD.SelectedValue.ToString();
            }


            //婚姻状况   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_HYZK.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_HYZK"] = comboBox_D_HYZK.SelectedValue.ToString();
            }

            //医疗费用支付方式   健康档案人口学资料（T_DA_JKDA_RKXZL）
            //checkedListBox_D_YLFZFLX.ClearSelected();
            string valueList = "";
            string textList = "";
            getChecklist("checkBox_D_YLFZFLX_", 1, 9, out valueList, out textList);
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_YLFZFLX"] = valueList;

            //医疗费用支付方式   健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_YLFZFLXQT"] = lTextBox_D_YLFZFLXQT.Text;

            //医疗保险号   健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_YLBXH"] = lTextBox_D_YLBXH.Text;

            //新农合号   健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_XNHH"] = lTextBox_D_XNHH.Text;

            //省   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_SHENG.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_SHENG"] = comboBox_D_SHENG.SelectedValue.ToString();
            }

            //市   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_SHI.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_SHI"] = comboBox_D_SHI.SelectedValue.ToString();
            }

            //县   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_QU.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_QU"] = comboBox_D_QU.SelectedValue.ToString();
            }
            //镇   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_JD.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_JD"] = comboBox_D_JD.SelectedValue.ToString();
            }
            //村   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_JWH.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_JWH"] = comboBox_D_JWH.SelectedValue.ToString();
            }

            //详细地址   健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_XXDZ"] = lTextBox_D_XXDZ.Text;

            //纸质编号
            dt_T_DA_JKDA_RKXZL.Rows[0]["d_zzbm"] = textBox_zzbh.Text;

            //身份证地址
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_SFZDZ"] = textBox_sfzdz.Text;
            
            //所属片区
            if (comboBox_D_SSPQ.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_SSPQ"] = comboBox_D_SSPQ.SelectedValue.ToString();
            }
            //档案类别   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_DALB.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_DALB"] = comboBox_D_DALB.SelectedValue.ToString();
            }

            //证件类型
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_ZJLX"] = cmb_zjlx2.SelectedValue.ToString();

            //创建地区
            dt_T_DA_JKDA_RKXZL.Rows[0]["CREATREGION"] = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";

            //区域编号
            dt_T_DA_JKDA_RKXZL.Rows[0]["P_RGID"] = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";

            //创建者姓名
            dt_T_DA_JKDA_RKXZL.Rows[0]["CREATEUSER"] = comboBox_lrr.SelectedValue != null ? comboBox_lrr.SelectedValue.ToString() : "";

            //修改者姓名
            dt_T_DA_JKDA_RKXZL.Rows[0]["UPDATEUSER"] = comboBox_lrr.SelectedValue != null ? comboBox_lrr.SelectedValue.ToString() : "";

            //创建时间
            dt_T_DA_JKDA_RKXZL.Rows[0]["CREATETIME"] = TextBox_lrsj.Text;

            //修改时间
            dt_T_DA_JKDA_RKXZL.Rows[0]["UPDATETIME"] = TextBox_lrsj.Text;

            //访问时间
            if (TextBox_lrsj.Text.Length > 0)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["happentime"] = Convert.ToDateTime(TextBox_lrsj.Text).ToString("yyyy-MM-dd");
            }
            else
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["happentime"] = DateTime.Now.ToString("yyyy-MM-dd");
            }

            //d_zzbm	纸质编码
            dt_T_DA_JKDA_RKXZL.Rows[0]["d_zzbm"] = textBox_zzbh.Text;

            //d_sfzdz  身份证地址
            dt_T_DA_JKDA_RKXZL.Rows[0]["d_sfzdz"] = textBox_sfzdz.Text;

            //datafrom  数据存储
            dt_T_DA_JKDA_RKXZL.Rows[0]["DATAFROM"] = comboBox_sjcc.SelectedValue != null ? comboBox_sjcc.SelectedValue.ToString() : "";

            //档案是否上传到公卫系统
            if (checkBox_ifUploade.Checked == true)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["IFUPLOADE"] = "1";
            }
            else
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["IFUPLOADE"] = "0";
            }



            //厨房排气设备	cfpqsb
            string valueList_CFPQSB = "";
            string textList_CFPQSB = "";
            getChecklist("checkBox_cfpfss_", 1, 4, out valueList_CFPQSB, out textList_CFPQSB);
            dt_T_DA_JKDA_RKXZL.Rows[0]["CFPQSB"] = valueList_CFPQSB;


            //燃料类型	rllx
            string valueList_RLLX = "";
            string textList_RLLX = "";
            getChecklist("checkBox_rllx_", 1, 6, out valueList_RLLX, out textList_RLLX);
            dt_T_DA_JKDA_RKXZL.Rows[0]["RLLX"] = valueList_RLLX;


            //饮水	ys
            string valueList_YS = "";
            string textList_YS = "";
            getChecklist("checkBox_ys_", 1, 6, out valueList_YS, out textList_YS);
            dt_T_DA_JKDA_RKXZL.Rows[0]["YS"] = valueList_YS;


            //厕所	cs
            string valueList_CS = "";
            string textList_CS = "";
            getChecklist("checkBox_cs_", 1, 5, out valueList_CS, out textList_CS);
            dt_T_DA_JKDA_RKXZL.Rows[0]["CS"] = valueList_CS;


            //禽畜栏	qxl
            string valueList_qxl = "";
            string textList_qxl = "";
            getChecklist("checkBox_qcl_", 1, 4, out valueList_qxl, out textList_qxl);
            dt_T_DA_JKDA_RKXZL.Rows[0]["QXL"] = valueList_qxl;


            //年龄
            string nl = (DateTime.Now.Year - dateTimePicker_D_CSRQ.Value.Year).ToString();
            dt_T_DA_JKDA_RKXZL.Rows[0]["NL"] = nl.ToString();

            string outMsg = "";
            Form_daxqBll form_daxqbll = new Form_daxqBll();
            //验证该档案是否已经存在
            if (check_lkda(out outMsg) == true)
            {
                //更新
                dt_T_DA_JKDA_RKXZL.AcceptChanges();
                //年度
                dt_T_DA_JKDA_RKXZL.Rows[0]["nd"] = DateTime.Now.Year.ToString();
                ////标识该条数据是否进行过修改。1：未修改  2：已修改  3：新增
                dt_T_DA_JKDA_RKXZL.Rows[0]["zt"] = "iif(zt='1','2',zt)";

                ////增量标
                //dt_T_DA_JKDA_RKXZL.Rows[0]["zlbz"] = "1";
                form_daxqbll.Upd(dt_T_DA_JKDA_RKXZL, "sql068");
            }
            else
            {
                //年度
                dt_T_DA_JKDA_RKXZL.Rows[0]["nd"] = DateTime.Now.Year.ToString();
                //标识该条数据是否进行过修改。1：未修改  2：已修改  3：新增
                dt_T_DA_JKDA_RKXZL.Rows[0]["zt"] = "3";

                //增量标
                dt_T_DA_JKDA_RKXZL.Rows[0]["zlbz"] = "1";
                //新增
                form_daxqbll.Add(dt_T_DA_JKDA_RKXZL, "sql067");
            }

            #endregion

            #region 健康档案健康状况表
            
            //初始化表结构
            dt_T_DA_JKDA_JKZK = initTable("sql_T_DA_JKDA_JKZK_select");
            dt_T_DA_JKDA_JKZK.Rows.Add();

            //健康档案号  
            dt_T_DA_JKDA_JKZK.Rows[0]["D_GRDABH"] = lTextBox_D_GRDABH.Text.PadLeft(12, '0');

            //血型
            if (comboBox_D_XX.SelectedValue != null)
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_XX"] = comboBox_D_XX.SelectedValue.ToString();
            }

            //是否为RH阴性
            if (comboBox_D_SFRHYX.SelectedValue != null)
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_SFRHYX"] = comboBox_D_SFRHYX.SelectedValue.ToString();
            }

            //是否过敏史
            if (comboBox_D_GMS.SelectedValue != null)
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_GMS"] = comboBox_D_GMS.SelectedValue.ToString();
            }

            //有过敏史
            string valueList_gm = "";
            string textList_gm = "";
            getChecklist("checkBox_D_YGMS_", 1, 5, out valueList_gm, out textList_gm);
            dt_T_DA_JKDA_JKZK.Rows[0]["D_YGMS"] = valueList_gm;

            //过敏史其他
            dt_T_DA_JKDA_JKZK.Rows[0]["D_GMSQT"] = lTextBox_D_GMSQT.Text;

            //暴露史 
            if (comboBox_bls.SelectedValue != null)
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_BLS"] = comboBox_bls.SelectedValue.ToString();
            }

        //有暴露史
            //暴露史化学品
            dt_T_DA_JKDA_JKZK.Rows[0]["D_BLSHXP"] = textBox_hxp.Text;
            //暴露史毒物
            dt_T_DA_JKDA_JKZK.Rows[0]["D_BLSDW"] = textBox_dw.Text;
            //暴露史射线
            dt_T_DA_JKDA_JKZK.Rows[0]["D_BLSSX"] = textBox_sx.Text;

            //遗传病史
            if (comboBox_ycbs.SelectedValue != null)
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_YCBS"] = comboBox_ycbs.SelectedValue.ToString();
            }
            //遗传史疾病
            dt_T_DA_JKDA_JKZK.Rows[0]["D_YCBSJB"] = textBox_ycbs_jbmc.Text;

            //有无残疾
            if (comboBox_ycbs.SelectedValue != null)
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_YWCJ"] = comboBox_ycbs.SelectedValue.ToString();
            }

            //残疾名字
            string valueList_cj = "";
            string textList_cj = "";
            getChecklist("checkBox_cjqk_", 1, 7, out valueList_cj, out textList_cj);
            dt_T_DA_JKDA_JKZK.Rows[0]["D_CJMZ"] = valueList_cj;
            //其他残疾
            dt_T_DA_JKDA_JKZK.Rows[0]["D_CJQT"] = textBox_cjqk_qt.Text;

            //有无疾病
            if (radioButton_jb_1.Checked == true)
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_YWJB"] = "1"; //无
            }
            else
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_YWJB"] = "2"; //有
            }

            //有无既往史
            if (radioButton_ss_2.Checked == true || radioButton_ws_2.Checked == true || radioButton_sx_2.Checked == true)
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_YWJWS"] = "2"; //有
            }
            else
            {
                dt_T_DA_JKDA_JKZK.Rows[0]["D_YWJWS"] = "1"; //无
            }

            //创建地区
            dt_T_DA_JKDA_JKZK.Rows[0]["CREATREGION"] = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";
      
            //修改创建时间
            dt_T_DA_JKDA_JKZK.Rows[0]["HAPPENTIME"] = TextBox_lrsj.Text;

            //区域编号
            dt_T_DA_JKDA_JKZK.Rows[0]["P_RGID"] = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";

            //创建者姓名
            dt_T_DA_JKDA_JKZK.Rows[0]["CREATEUSER"] = comboBox_lrr.SelectedValue != null ? comboBox_lrr.SelectedValue.ToString() : "";

            //修改者姓名
            dt_T_DA_JKDA_JKZK.Rows[0]["UPDATEUSER"] = comboBox_lrr.SelectedValue != null ? comboBox_lrr.SelectedValue.ToString() : "";

            //创建时间
            dt_T_DA_JKDA_JKZK.Rows[0]["CREATETIME"] = TextBox_lrsj.Text;

            //修改时间
            dt_T_DA_JKDA_JKZK.Rows[0]["UPDATETIME"] = TextBox_lrsj.Text;

            Form_daxqBll form_daxqbll_JKZK = new Form_daxqBll();
            //验证健康状况是否已经存在
            if (check_jkzk() == true)
            {
                //更新
                dt_T_DA_JKDA_JKZK.AcceptChanges();
                //年度
                dt_T_DA_JKDA_JKZK.Rows[0]["nd"] = DateTime.Now.Year.ToString();
                ////标识该条数据是否进行过修改。1：未修改  2：已修改
                dt_T_DA_JKDA_JKZK.Rows[0]["zt"] = "2";

                ////增量标
                //dt_T_DA_JKDA_RKXZL.Rows[0]["zlbz"] = "1";
                form_daxqbll.Upd(dt_T_DA_JKDA_JKZK, "sql_T_DA_JKDA_JKZK_update");
            }
            else
            {
                //年度
                dt_T_DA_JKDA_JKZK.Rows[0]["nd"] = DateTime.Now.Year.ToString();

                //标识该条数据是否进行过修改。1：未修改  2：已修改  
                dt_T_DA_JKDA_JKZK.Rows[0]["zt"] = "2";

                //增量标
                dt_T_DA_JKDA_JKZK.Rows[0]["zlbz"] = "1";

                //新增
                form_daxqbll.Add(dt_T_DA_JKDA_JKZK, "sql_T_DA_JKDA_JKZK_insert");
            }
            #endregion

            #region 健康档案健康状况既往病史表（T_DA_JKDA_JKZK_JWBS）

            //初始化表结构
            //疾病
            //dt_T_DA_JKDA_JKZK_JWBS = null;
            dt_T_DA_JKDA_JKZK_JWBS = createtable("jwbs_jb");
            DataTable dt_jb = null;
            if (dataGridView_jb.DataSource != null )
            {
                dt_jb = ((DataTable)dataGridView_jb.DataSource).Copy();
                if (dt_jb.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_jb.Rows.Count; i++)
                    {
                        dt_T_DA_JKDA_JKZK_JWBS.ImportRow(dt_jb.Rows[i]);
                    }
                }
            }

            //外伤
            DataTable dt_ws = null;
            if (dataGridView_ws.DataSource != null)
            {
                dt_ws = ((DataTable)dataGridView_ws.DataSource).Copy();

                if (dt_ws.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_ws.Rows.Count; i++)
                    {
                        dt_T_DA_JKDA_JKZK_JWBS.ImportRow(dt_ws.Rows [i]);
                    }
                }
            }

            //手术
            DataTable dt_ss = null;
            if (dataGridView_ss.DataSource != null)
            {
                dt_ss = ((DataTable)dataGridView_ss.DataSource).Copy();

                if (dt_ss.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_ss.Rows.Count; i++)
                    {
                        dt_T_DA_JKDA_JKZK_JWBS.ImportRow(dt_ss.Rows[i]);
                    }
                }
            }
            //输血
            DataTable dt_sx = null;
             if (dataGridView_sx.DataSource != null)
            {
                dt_sx = ((DataTable)dataGridView_sx.DataSource).Copy();

                if (dt_sx.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_sx.Rows.Count; i++)
                    {
                        dt_T_DA_JKDA_JKZK_JWBS.ImportRow(dt_sx.Rows[i]);
                    }
                }
            }

            //个人档案编号
            if (dt_T_DA_JKDA_JKZK_JWBS != null && dt_T_DA_JKDA_JKZK_JWBS.Columns.Contains("D_GRDABH") == true)
            {
                dt_T_DA_JKDA_JKZK_JWBS.Columns.Remove("D_GRDABH");
            }

            if (dt_T_DA_JKDA_JKZK_JWBS != null )
            {
                DataColumn dtColumn = new DataColumn();
                dtColumn.ColumnName = "D_GRDABH";
                dtColumn.DefaultValue = lTextBox_D_GRDABH.Text;
                dt_T_DA_JKDA_JKZK_JWBS.Columns.Add(dtColumn);
            }
           

           
            //数据保存到数据库
            if (dt_T_DA_JKDA_JKZK_JWBS != null && dt_T_DA_JKDA_JKZK_JWBS.Rows.Count > 0)
            {
                Form_daxqBll form_daxqbll_jws = new Form_daxqBll();
                //按照个人档案编号删除数据
                form_daxqbll_jws.Del(dt_T_DA_JKDA_JKZK_JWBS, 1, "sql_T_DA_JKDA_JKZK_JWBS_del");

                form_daxqbll_jws.AddAll(dt_T_DA_JKDA_JKZK_JWBS, "sql_T_DA_JKDA_JKZK_JWBS_insert");
            }

            #endregion

            //健康档案健康状况家族病史表（T_DA_JKDA_JKZK_JZBS）
            #region 健康档案健康状况家族病史表（T_DA_JKDA_JKZK_JZBS）

            //初始化表结构
            //家族史
            dt_T_DA_JKDA_JKZK_JZBS = null;
            //DataTable dt_jb = null;
            if (dataGridView_jzs.DataSource != null && dt_T_DA_JKDA_JKZK_JZBS == null)
            {
                dt_T_DA_JKDA_JKZK_JZBS = ((DataTable)dataGridView_jzs.DataSource).Copy();
            }

            //个人档案编号
            if (dt_T_DA_JKDA_JKZK_JZBS != null && dt_T_DA_JKDA_JKZK_JZBS.Columns.Contains("D_GRDABH") == true)
            {
                dt_T_DA_JKDA_JKZK_JZBS.Columns.Remove("D_GRDABH");
            }
            if (dt_T_DA_JKDA_JKZK_JZBS != null)
            {
                DataColumn dtColumn_jzbs = new DataColumn();
                dtColumn_jzbs.ColumnName = "D_GRDABH";
                dtColumn_jzbs.DefaultValue = lTextBox_D_GRDABH.Text;
                dt_T_DA_JKDA_JKZK_JZBS.Columns.Add(dtColumn_jzbs);
            }

            //数据保存到数据库
            if (dt_T_DA_JKDA_JKZK_JZBS != null && dt_T_DA_JKDA_JKZK_JZBS.Rows.Count > 0)
            {
                Form_daxqBll form_daxqbll_jzbs = new Form_daxqBll();
                //按照个人档案编号删除数据
                form_daxqbll_jzbs.Del(dt_T_DA_JKDA_JKZK_JZBS, 1, "sql_T_DA_JKDA_JKZK_JZBS_del");

                form_daxqbll_jzbs.AddAll(dt_T_DA_JKDA_JKZK_JZBS, "sql_T_DA_JKDA_JKZK_JZBS_insert");
            }

            #endregion

            #region 体检人员信息

            //初始化表结构
            DataTable dt_T_jk_ryxx = new DataTable();
            dt_T_jk_ryxx.Rows.Add();

            //YLJGBM	医疗机构编码
            dt_T_jk_ryxx.Columns.Add("YLJGBM");
            dt_T_jk_ryxx.Rows[0]["YLJGBM"] = UserInfo.Yybm;
            //JKDAH	个人健康档案号
            dt_T_jk_ryxx.Columns.Add("JKDAH");
            dt_T_jk_ryxx.Rows[0]["JKDAH"] = lTextBox_D_GRDABH.Text.PadLeft(12, '0');
            //XM	姓名
            dt_T_jk_ryxx.Columns.Add("XM");
            dt_T_jk_ryxx.Rows[0]["XM"] = lTextBox_D_XM.Text;
            //XB	性别
            dt_T_jk_ryxx.Columns.Add("XB");
            dt_T_jk_ryxx.Rows[0]["XB"] = comboBox_D_XB.SelectedValue != null ? comboBox_D_XB.SelectedValue.ToString() : "";
            //SFZH	身份证号
            dt_T_jk_ryxx.Columns.Add("SFZH");
            dt_T_jk_ryxx.Rows[0]["SFZH"] = lTextBox_D_SFZH.Text;
            //LXDH	联系电话
            dt_T_jk_ryxx.Columns.Add("LXDH");
            dt_T_jk_ryxx.Rows[0]["LXDH"] = lTextBox_D_LXDH.Text;
            //CSRQ	出生日期
            dt_T_jk_ryxx.Columns.Add("CSRQ");
            dt_T_jk_ryxx.Rows[0]["CSRQ"] = dateTimePicker_D_CSRQ.Value.ToString("yyyy-MM-dd");
            //CZBM	村庄编码
            dt_T_jk_ryxx.Columns.Add("CZBM");
            dt_T_jk_ryxx.Rows[0]["CZBM"] = comboBox_D_JWH.SelectedValue != null ? comboBox_D_JWH.SelectedValue.ToString() : "";
            //UPDATETIME	更新时间
            dt_T_jk_ryxx.Columns.Add("UPDATETIME");
            dt_T_jk_ryxx.Rows[0]["UPDATETIME"] = DateTime.Now.ToString("yyyy-MM-dd");
            //UPDATEUSER	更新者
            dt_T_jk_ryxx.Columns.Add("UPDATEUSER");
            dt_T_jk_ryxx.Rows[0]["UPDATEUSER"] = UserInfo.userId;
            //nd	年度
            dt_T_jk_ryxx.Columns.Add("nd");
            dt_T_jk_ryxx.Rows[0]["nd"] = DateTime.Now.Year.ToString();

            //nd	分类
            dt_T_jk_ryxx.Columns.Add("fl");
            dt_T_jk_ryxx.Rows[0]["fl"] = "2";

            //nd	增量标志
            dt_T_jk_ryxx.Columns.Add("zlbz");
            dt_T_jk_ryxx.Rows[0]["zlbz"] = "1";

            //PRGID	所属机构
            dt_T_jk_ryxx.Columns.Add("PRGID");
            dt_T_jk_ryxx.Rows[0]["PRGID"] = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";

            //DATAFROM	数据来源
            dt_T_jk_ryxx.Columns.Add("DATAFROM");
            dt_T_jk_ryxx.Rows[0]["DATAFROM"] = comboBox_sjcc.SelectedValue != null ? comboBox_sjcc.SelectedValue.ToString() : "";

            //更新
            dt_T_jk_ryxx.AcceptChanges();
            //年度
            dt_T_jk_ryxx.Rows[0]["nd"] = DateTime.Now.Year.ToString();

            ////增量标
            //dt_T_DA_JKDA_RKXZL.Rows[0]["zlbz"] = "1";
            form_daxqbll.Upd(dt_T_jk_ryxx, "sql_update_TJRYXX");

            #endregion

            MessageBox.Show("保存成功！");

        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clear_Click(object sender, EventArgs e)
        {
            pageClear();
        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <returns></returns>
        public bool pageClear()
        {
            try
            {
                //清空健康档案号
                lTextBox_D_GRDABH.Text = "";
                //与户主关系
                comboBox_D_YHZGX.SelectedIndex = 0;
                text_hzsfzh.Text = "";
                //档案状态
                radioButton_D_DAZT_1.Checked = true;
                radioButton_D_DAZT_2.Checked = false;
                //姓名
                lTextBox_D_XM.Text = "";
                //性别
                comboBox_D_XB.SelectedIndex = -1;
                //证件编号
                lTextBox_D_SFZH.Text = "";
                //出生日期
                dateTimePicker_D_CSRQ.Text = "";
                //本人电话
                lTextBox_D_LXDH.Text = "";
                //工作单位
                lTextBox_D_GZDW.Text = "";
                //联系人电话
                lTextBox_D_LXRDH.Text = "";
                //联系人姓名
                lTextBox_D_LXRXM.Text = "";
                //常住类型
                comboBox_czlx.SelectedIndex = -1;
                //民族
                comboBox_D_MZ.SelectedIndex = -1;
                //血型
                comboBox_D_XX.SelectedIndex = -1;
                //RH
                comboBox_D_SFRHYX.SelectedIndex = -1;
                //职业
                comboBox_D_ZY.SelectedIndex = -1;
                //文化程度
                comboBox_D_WHCD.SelectedIndex = -1;
                //劳动强度
                comboBox_D_LDQD.SelectedIndex = -1;

                //婚姻状况
                comboBox_D_HYZK.SelectedIndex = -1;
                //医疗费用支付方式
                //checkedListBox_D_YLFZFLX.ClearSelected();
                //医疗费用支付方式
                lTextBox_D_YLFZFLXQT.Text = "";

                //医疗保险号
                lTextBox_D_YLBXH.Text = "";
                //新农合号
                lTextBox_D_XNHH.Text = "";
                //省
                drpflag = false;
                comboBox_D_SHENG.SelectedIndex = -1;
                //市
                comboBox_D_SHI.SelectedIndex = -1;
                //县
                comboBox_D_QU.SelectedIndex = -1;
                //镇
                comboBox_D_JD.SelectedIndex = -1;
                //村
                comboBox_D_JWH.SelectedIndex = -1;
                drpflag = true;
                //详细地址
                lTextBox_D_XXDZ.Text = "";
                //所属片区
                // comboBox_D_SSPQ.SelectedIndex = 0;
                //档案类别
                comboBox_D_DALB.SelectedIndex = -1;
                //药物过敏史
                comboBox_D_GMS.SelectedIndex = -1;
                //药物过敏史
                //checkedListBox_D_YGMS.ClearSelected();
                //药物过敏史
                lTextBox_D_GMSQT.Text = "";
                // //与户主关系
                // comboBox_D_YHZGX.SelectedIndex = 0;
                // //档案状态
                // radioButton_D_DAZT_1.Checked = true;
                // radioButton_D_DAZT_2.Checked = false;
                // //姓名
                // lTextBox_D_XM.Text = "";
                // //性别
                // comboBox_D_XB.SelectedIndex = 0;
                // //证件编号
                // lTextBox_D_SFZH.Text = "";
                // //出生日期
                // comboBox_D_YHZGX.SelectedIndex = 0;
                // //本人电话
                // lTextBox_D_LXDH.Text = "";
                // //工作单位
                // lTextBox_D_GZDW.Text = "";
                // //联系人电话
                // lTextBox_D_LXRDH.Text = "";
                // //联系人姓名
                // lTextBox_D_LXRXM.Text = "";
                // //常住类型
                // comboBox_czlx.SelectedIndex = 0;
                // //民族
                // comboBox_D_MZ.SelectedIndex = 0;
                // //血型
                // comboBox_D_XX.SelectedIndex = 0;
                // //RH
                // comboBox_D_SFRHYX.SelectedIndex = 0;
                // //职业
                // comboBox_D_ZY.SelectedIndex = 0;
                // //文化程度
                // comboBox_D_WHCD.SelectedIndex = 0;
                // //劳动强度
                // comboBox_D_LDQD.SelectedIndex = 0;

                // //婚姻状况
                // comboBox_D_HYZK.SelectedIndex = 0;
                // //医疗费用支付方式
                // checkedListBox_D_YLFZFLX.ClearSelected();
                // //医疗费用支付方式
                // lTextBox_D_YLFZFLXQT.Text = "";

                // //医疗保险号
                // lTextBox_D_YLBXH.Text = "";
                // //新农合号
                // lTextBox_D_XNHH.Text = "";
                // //省
                // comboBox_D_SHENG.SelectedIndex = 0;
                // //市
                // comboBox_D_SHI.SelectedIndex = 0;
                // //县
                // comboBox_D_QU.SelectedIndex = 0;
                // //镇
                // comboBox_D_JD.SelectedIndex = 0;
                // //村
                // comboBox_D_JWH.SelectedIndex = 0;
                // //详细地址
                // lTextBox_D_XXDZ.Text = "";
                // //所属片区
                //// comboBox_D_SSPQ.SelectedIndex = 0;
                // //档案类别
                // comboBox_D_DALB.SelectedIndex = 0;
                // //药物过敏史
                // comboBox_D_GMS.SelectedIndex = 0;
                // //药物过敏史
                // checkedListBox_D_YGMS.ClearSelected();
                // //药物过敏史
                // lTextBox_D_GMSQT.Text = "";
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 检索数据
        /// </summary>
        /// <returns></returns>
        public bool setDataTopage(string strWhere)
        {
            Form_daxqBll form_daxq = new Form_daxqBll();

            //获取结果集
            #region 健康档案人口学资料（T_DA_JKDA_RKXZL）

            DataTable dt = form_daxq.GetMoHuList(strWhere, "sql056");
            if (dt != null && dt.Rows.Count > 0)
            {

                lTextBox_D_GRDABH.Text = dt.Rows[0]["D_GRDABH"] != null ? dt.Rows[0]["D_GRDABH"].ToString() : "";
                //与户主关系
                comboBox_D_YHZGX.SelectedValue = dt.Rows[0]["D_YHZGX"] != null ? dt.Rows[0]["D_YHZGX"].ToString() : "";

                //档案状态
                if (dt.Rows[0]["D_YHZGX"] != null && dt.Rows[0]["D_YHZGX"].ToString().Equals("1"))
                {
                    radioButton_D_DAZT_1.Checked = true;
                }
                else
                {
                    radioButton_D_DAZT_2.Checked = true;
                }

                //姓名
                lTextBox_D_XM.Text = dt.Rows[0]["D_XM"] != null ? dt.Rows[0]["D_XM"].ToString() : "";
                //性别
                comboBox_D_XB.SelectedValue = dt.Rows[0]["D_XB"] != null ? dt.Rows[0]["D_XB"].ToString() : "";
                //证件编号
                lTextBox_D_SFZH.Text = dt.Rows[0]["D_zjhqt"] != null ? dt.Rows[0]["D_zjhqt"].ToString() : "";
                //
                comboBox_D_YHZGX.SelectedValue = dt.Rows[0]["D_YHZGX"] != null ? dt.Rows[0]["D_YHZGX"].ToString() : "";
                //出生日期
                dateTimePicker_D_CSRQ.Value = dt.Rows[0]["D_csrq"] != null && dt.Rows[0]["D_csrq"].ToString().Length > 0 ? Convert.ToDateTime(dt.Rows[0]["D_csrq"].ToString()) : DateTime.Now;
                //本人电话
                lTextBox_D_LXDH.Text = dt.Rows[0]["D_LXDH"] != null ? dt.Rows[0]["D_LXDH"].ToString() : "";
                //工作单位
                lTextBox_D_GZDW.Text = dt.Rows[0]["D_GZDW"] != null ? dt.Rows[0]["D_GZDW"].ToString() : "";
                //联系人电话
                lTextBox_D_LXRDH.Text = dt.Rows[0]["D_LXRDH"] != null ? dt.Rows[0]["D_LXRDH"].ToString() : "";
                //联系人姓名
                lTextBox_D_LXRXM.Text = dt.Rows[0]["D_LXRXM"] != null ? dt.Rows[0]["D_LXRXM"].ToString() : "";
                //常住类型
                comboBox_czlx.SelectedValue = dt.Rows[0]["D_JZZK"] != null ? dt.Rows[0]["D_JZZK"].ToString() : "";
                //民族
                comboBox_D_MZ.SelectedValue = dt.Rows[0]["D_MZ"] != null ? dt.Rows[0]["D_MZ"].ToString() : "";

                //职业
                comboBox_D_ZY.SelectedValue = dt.Rows[0]["D_ZY"] != null ? dt.Rows[0]["D_ZY"].ToString() : "";
                //文化程度
                comboBox_D_WHCD.SelectedValue = dt.Rows[0]["D_WHCD"] != null ? dt.Rows[0]["D_WHCD"].ToString() : "";
                //婚姻状况
                comboBox_D_HYZK.SelectedValue = dt.Rows[0]["D_HYZK"] != null ? dt.Rows[0]["D_HYZK"].ToString() : "";
                //医疗费用支付方式
                //setItemSelect(checkedListBox_D_YLFZFLX, dt.Rows[0]["D_YLFZFLX"] != null ? dt.Rows[0]["D_YLFZFLX"].ToString() : "", "ZDBM");
                setChecklist("checkBox_D_YLFZFLX_", 1, 9, dt.Rows[0]["D_YLFZFLX"] != null ? dt.Rows[0]["D_YLFZFLX"].ToString() : "");

                //医疗费用支付方式
                lTextBox_D_YLFZFLXQT.Text = dt.Rows[0]["D_YLFZFLXQT"] != null ? dt.Rows[0]["D_YLFZFLXQT"].ToString() : "";

                //医疗保险号
                lTextBox_D_YLBXH.Text = dt.Rows[0]["D_YLBXH"] != null ? dt.Rows[0]["D_YLBXH"].ToString() : "";
                //新农合号
                lTextBox_D_XNHH.Text = dt.Rows[0]["D_XNHH"] != null ? dt.Rows[0]["D_XNHH"].ToString() : "";
                //省
                //drpflag = false;
                comboBox_D_SHENG.SelectedValue = dt.Rows[0]["D_SHENG"] != null ? dt.Rows[0]["D_SHENG"].ToString() : "";
                //市
                comboBox_D_SHI.SelectedValue = dt.Rows[0]["D_SHI"] != null ? dt.Rows[0]["D_SHI"].ToString() : "";
                //县
                comboBox_D_QU.SelectedValue = dt.Rows[0]["D_QU"] != null ? dt.Rows[0]["D_QU"].ToString() : "";
                //镇
                comboBox_D_JD.SelectedValue = dt.Rows[0]["D_JD"] != null ? dt.Rows[0]["D_JD"].ToString() : "";
                //村
                comboBox_D_JWH.SelectedValue = dt.Rows[0]["D_JWH"] != null ? dt.Rows[0]["D_JWH"].ToString() : "";
                //drpflag = true;
                //详细地址
                lTextBox_D_XXDZ.Text = dt.Rows[0]["D_XXDZ"] != null ? dt.Rows[0]["D_XXDZ"].ToString() : "";
                //所属片区 D_SSPQ
                if (dt.Rows[0]["D_SSPQ"] != null)
                {
                    comboBox_D_SSPQ.SelectedValue = dt.Rows[0]["D_XXDZ"].ToString();
                }

                //档案类别
                comboBox_D_DALB.SelectedValue = dt.Rows[0]["D_DALB"] != null ? dt.Rows[0]["D_DALB"].ToString() : "";

                text_hzsfzh.Text = dt.Rows[0]["D_DALB"].ToString();
                text_hzsfzh.Text = dt.Rows[0]["D_HZSFZH"].ToString();

                //所属单位
                comboBox_ssjg.SelectedValue = dt.Rows[0]["P_RGID"].ToString();
                //录入人
                comboBox_lrr.SelectedValue = dt.Rows[0]["CREATEUSER"].ToString();

                //厨房排气设备	cfpqsb
                setChecklist("checkBox_cfpfss_", 1, 4, dt.Rows[0]["cfpqsb"] != null ? dt.Rows[0]["cfpqsb"].ToString() : "");
                //燃料类型	rllx
                setChecklist("checkBox_rllx_", 1, 6, dt.Rows[0]["rllx"] != null ? dt.Rows[0]["rllx"].ToString() : "");
                //饮水	ys
                setChecklist("checkBox_ys_", 1, 6, dt.Rows[0]["ys"] != null ? dt.Rows[0]["ys"].ToString() : "");
                //厕所	cs
                setChecklist("checkBox_cs_", 1, 5, dt.Rows[0]["cs"] != null ? dt.Rows[0]["cs"].ToString() : "");
                //禽畜栏	qxl
                setChecklist("checkBox_qcl_", 1, 4, dt.Rows[0]["qxl"] != null ? dt.Rows[0]["qxl"].ToString() : "");

                //纸质编码
                textBox_zzbh.Text = dt.Rows[0]["d_zzbm"] != null ? dt.Rows[0]["d_zzbm"].ToString() : "";

                //身份证地址
                textBox_sfzdz.Text = dt.Rows[0]["D_SFZDZ"] != null ? dt.Rows[0]["D_SFZDZ"].ToString() : "";

                //档案数据是否上传到公卫系统
                if (dt.Rows[0]["IFUPLOADE"] != null && dt.Rows[0]["IFUPLOADE"].ToString().Equals("1") == true)
                {
                    checkBox_ifUploade.Checked = true;
                }
                else if (dt.Rows[0]["IFUPLOADE"] != null && dt.Rows[0]["IFUPLOADE"].ToString().Equals("0") == true)
                {
                    checkBox_ifUploade.Checked = false;
                }

            }
            #endregion

            #region 家庭档案表（T_DA_JTDA）

            //编号id
            //家庭档案编号D_JTDABH
            //住房类型D_ZFLX
            //居住面积D_JZMJ
            //厕所类型D_CSLX
            //厕所类型其他D_CSLXQT
            //人均收入D_RJSR
            //是否低保D_SFDB
            //吃油量D_CYL
            //吃盐量D_CYANL
            //所属机构P_RGID
            //创建时间CREATETIME
            //修改时间UPDATETIME
            //修改创建时间HAPPENTIME
            //创建者姓名CREATEUSER
            //修改者姓名UPDATEUSER
            //创建地区CREATREGION
            //缺省值QDQXZ
            //为用Q_KH
            #endregion

            #region 个人健康特征表（T_DA_JKDA_GRJKTZ）
            //编号ID
            //个人档案号D_GRDABH
            //与户主关系D_YHZGX
            //个人基本信息T_DA_JKDARKXZL
            //家庭档案T_DA_JTDA
            //健康体检T_JK_JKTJ
            //接诊记录T_YF_ZSJL
            //会诊记录T_YF_ZSJLHZ
            //儿童基本信息表T_EB_ETJBXX
            //新生儿访视记录表T_EB_XSEFSJL
            //儿童健康检查记录表(满月)T_EB_JKJC_42
            //儿童健康检查记录表(3月龄)T_EB_JKJC_3Yue
            //儿童健康检查记录表(6月龄)T_EB_JKJC_6Yue
            //儿童健康检查记录表(8月龄)T_EB_JKJC_9Yue
            //儿童健康检查记录表(12月龄)T_EB_JKJC_12Yue
            //儿童健康检查记录表(18月龄)T_EB_JKJC_18Yue
            //儿童健康检查记录表(24月龄)T_EB_JKJC_2Sui
            //儿童健康检查记录表(30月龄)T_EB_JKJC_2SuiB
            //儿童健康检查记录表(3岁)T_EB_JKJC_3Sui
            //儿童健康检查记录表(4岁)*T_EB_JKJC_4Sui
            //儿童健康检查记录表(5岁)*T_EB_JKJC_5Sui
            //儿童健康检查记录表(6岁)*T_EB_JKJC_6Sui
            //儿童入托检查表T_EB_RTCTB
            //妇女保健检查表T_FB_FNBJJC
            //更年期保健检查表T_FB_GNQBJJC
            //孕产妇基本信息表T_FB_YUNINFO
            //孕前首诊记录表T_FB_YQSZJL
            //第2次产前检查表T_FB_CQJCJLERC
            //第3次产前检查表T_FB_CQJCJLSANC
            //第4次产前检查表T_FB_CQJCJLSIC
            //第5次产前检查表T_FB_CQJCJLWUC
            //产后访视情况表T_FB_CHFSQK
            //产后42天健康检查表T_FB_CH42JKJC
            //老年人随访表T_JG_LNRSF
            //高血压管理卡表T_JG_GXYGLK
            //高血压随访表T_JG_GXYSFB
            //糖尿病管理卡表T_JG_TNBGLK
            //糖尿病随访表T_JG_TNBSFB
            //脑卒中管理卡表T_JG_NZZGLK
            //脑卒中随访表T_JG_NZZSFB
            //冠心病管理卡表T_JG_GXBGLK
            //冠心病随访表T_JG_GXBSFB
            //精神疾病信息补充表T_CJR_JSJBXXBCB
            //精神残疾表T_CJR_JINGSHEN
            //听力残疾表T_CJR_TINGLI
            //肢体残疾表T_CJR_ZHITI
            //智力残疾表T_CJR_ZHILI
            //视力残疾表T_CJR_SHILI
            //是否高血压IS_GXY
            //是否糖尿病IS_TNB
            //是否冠心病IS_GXB
            //是否脑卒中IS_NZZ
            //是否空腹血糖受损IS_KFXT
            //是否血脂边缘升高IS_XZ
            //是否糖耐量异常IS_CHXT
            //是否肥胖IS_FP
            //是否重度吸烟IS_ZDXY
            //是否超重中心型肥胖IS_CZFP
            //是否肿瘤IS_ZL
            //是否慢性阻塞型肺病IS_MZ


            #endregion

            #region 健康档案健康状况家族病史表（T_DA_JKDA_JKZK_JZBS）
            //家族病史
            DataTable dt_JZBS = form_daxq.GetMoHuList(string.Format(" and D_GRDABH='{0}' ", lTextBox_D_GRDABH.Text), "sql_T_DA_JKDA_JKZK_JZBS_select");
            if (dt_JZBS != null && dt_JZBS.Rows.Count > 0)
            {
                radioButton_jzs_2.Checked = true;
                dataGridView_jzs.DataSource = dt_JZBS;
            }

            #endregion

            #region 健康档案健康状况既往病史表（T_DA_JKDA_JKZK_JWBS）
            //疾病
            DataTable dt_JWBS = form_daxq.GetMoHuList(string.Format(" and D_GRDABH='{0}' and D_JBLX='疾病' ", lTextBox_D_GRDABH.Text), "sql_T_DA_JKDA_JKZK_JWBS_select");
            if (dt_JWBS != null && dt_JWBS.Rows .Count >0)
            {
                radioButton_jb_2.Checked = true;
                dataGridView_jb.DataSource = dt_JWBS;
            }
            //手术
            DataTable dt_JWBS_ss = form_daxq.GetMoHuList(string.Format(" and D_GRDABH='{0}' and D_JBLX='手术' ", lTextBox_D_GRDABH.Text), "sql_T_DA_JKDA_JKZK_JWBS_select");
            if (dt_JWBS_ss != null && dt_JWBS_ss.Rows.Count > 0)
            {
                radioButton_ss_2.Checked = true;
                dataGridView_ss.DataSource = dt_JWBS_ss;
            }
            //外伤
            DataTable dt_JWBS_ws = form_daxq.GetMoHuList(string.Format(" and D_GRDABH='{0}' and D_JBLX='外伤' ", lTextBox_D_GRDABH.Text), "sql_T_DA_JKDA_JKZK_JWBS_select");
            if (dt_JWBS_ws != null && dt_JWBS_ws.Rows.Count > 0)
            {
                radioButton_ws_2.Checked = true;
                dataGridView_ws.DataSource = dt_JWBS_ws;
            }
            //输血
            DataTable dt_JWBS_sx = form_daxq.GetMoHuList(string.Format(" and D_GRDABH='{0}' and D_JBLX='输血' ", lTextBox_D_GRDABH.Text), "sql_T_DA_JKDA_JKZK_JWBS_select");
            if (dt_JWBS_sx != null && dt_JWBS_sx.Rows.Count > 0)
            {
                radioButton_sx_2.Checked = true;
                dataGridView_sx.DataSource = dt_JWBS_sx;
            }

            #endregion

            #region 健康档案健康状况表（T_DA_JKDA_JKZK）
            DataTable dt_JKZK = form_daxq.GetMoHuList(string.Format (" and D_GRDABH='{0}'",lTextBox_D_GRDABH.Text), "sql_T_DA_JKDA_JKZK_select");
            if(dt_JKZK!=null && dt_JKZK.Rows .Count >0){
            //个人档案编号	D_GRDABH
            //血型	D_XX
            if (dt_JKZK.Rows[0]["D_XX"] != null && dt_JKZK.Rows[0]["D_XX"].ToString().Length > 0)
            {
                comboBox_D_XX.SelectedValue = dt_JKZK.Rows[0]["D_XX"].ToString();
            }
            //是否为RH阴性	D_SFRHYX
            if (dt_JKZK.Rows[0]["D_SFRHYX"] != null && dt_JKZK.Rows[0]["D_SFRHYX"].ToString().Length > 0)
            {
                comboBox_D_SFRHYX.SelectedValue = dt_JKZK.Rows[0]["D_SFRHYX"].ToString();
            }

            //是否过敏史	D_GMS
            if (dt_JKZK.Rows[0]["D_GMS"] != null && dt_JKZK.Rows[0]["D_GMS"].ToString().Length > 0)
            {
                comboBox_D_GMS.SelectedValue = dt_JKZK.Rows[0]["D_GMS"].ToString();
                //if (dt_JKZK.Rows[0]["D_GMS"].ToString().Equals("2"))//有过敏史
                //{
                //    lTextBox_D_GMSQT.Enabled 
                //}
            }
            //有过敏史	D_YGMS
            setChecklist("checkBox_D_YGMS_", 1, 5, dt_JKZK.Rows[0]["D_YGMS"] != null ? dt_JKZK.Rows[0]["D_YGMS"].ToString() : "");

            //过敏史其他	D_GMSQT
            lTextBox_D_GMSQT.Text = dt_JKZK.Rows[0]["D_GMSQT"].ToString();

            //暴露史	D_BLS
            if (dt_JKZK.Rows[0]["D_BLS"] != null && dt_JKZK.Rows[0]["D_BLS"].ToString().Length > 0)
            {
                comboBox_bls.SelectedValue = dt_JKZK.Rows[0]["D_BLS"].ToString();
            }
            //有暴露史	D_YBLS
            //暴露史化学品	D_BLSHXP
            textBox_hxp.Text = dt_JKZK.Rows[0]["D_BLSHXP"].ToString();
            //暴露史毒物	D_BLSDW
            textBox_dw.Text = dt_JKZK.Rows[0]["D_BLSDW"].ToString();
            //暴露史射线	D_BLSSX
            textBox_sx.Text = dt_JKZK.Rows[0]["D_BLSSX"].ToString();

            //遗传病史	D_YCBS
            if (dt_JKZK.Rows[0]["D_YCBS"] != null && dt_JKZK.Rows[0]["D_YCBS"].ToString().Length > 0)
            {
                comboBox_ycbs.SelectedValue = dt_JKZK.Rows[0]["D_YCBS"].ToString();
            }

            //遗传史疾病	D_YCBSJB
            textBox_ycbs_jbmc.Text = dt_JKZK.Rows[0]["D_YCBSJB"].ToString();

            //有无残疾	D_YWCJ
            if (dt_JKZK.Rows[0]["D_YWCJ"] != null && dt_JKZK.Rows[0]["D_YWCJ"].ToString().Length > 0)
            {
                comboBox_cjqk.SelectedValue = dt_JKZK.Rows[0]["D_YWCJ"].ToString();
            }
                
            //残疾名字	D_CJMZ
            setChecklist("checkBox_cjqk_", 1, 7, dt_JKZK.Rows[0]["D_CJMZ"] != null ? dt_JKZK.Rows[0]["D_CJMZ"].ToString() : "");
             
            //残疾其他	D_CJQT
            textBox_cjqk_qt.Text = dt_JKZK.Rows[0]["D_CJQT"].ToString();
           
            }

            #endregion

            #region 健康档案生活习惯表（T_DA_JKDA_SHXG）
            //编号id
            //个人档案编号D_GRDABH
            //是否吸烟D_SFXY
            //是否饮酒D_SFHJ
            //吸烟数量D_XYSL
            //被动吸烟时间D_BDXYSJ
            //劳动强度D_LDQD
            //comboBox_D_LDQD.SelectedValue = dt.Rows[0]["D_LDQD"] != null ? dt.Rows[0]["D_LDQD"].ToString() : ""; 
            //是否体育锻炼D_SFTYDL
            //锻炼类型D_DLLX
            //锻炼类型其他D_DLLX_QT
            //锻炼次数D_DLCS
            //锻炼时间D_DLSJ
            //膳食习惯D_SSXG
            //膳食习惯其他D_SSXGQT
            //心理状况D_SLZK
            //睡眠时间D_SMSJ
            //睡眠情况D_SMQK
            //区域编号P_RGID
            //创建时间CREATETIME
            //修改时间UPDATETIME
            //修改创建时间HAPPENTIME
            //创建者姓名CREATEUSER
            //修改者姓名UPDATEUSER
            //创建地区CREATREGION
            #endregion

            return true;
        }
        #region  增加体检人员

        /// <summary>
        /// 生成体检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_jktj_Click(object sender, EventArgs e)
        {
            Form_daxqBll form_daxqbll = new Form_daxqBll();
            try
            {
                //所属机构
                string comboBox_ssjg_str = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";
                //所属机构
                string comboBox_sjcc_str = comboBox_sjcc.SelectedValue != null ? comboBox_sjcc.SelectedValue.ToString() : "";

                string message = "";
                if (checksfz_nl(out message) == false)
                {
                    DialogResult result;
                    result = MessageBox.Show(message + " 是否仍要加入体检？ \r\n是：加入体检 \r\n否：不加入体检 \r\n取消：取消本次操作", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        Form_PeopleAdd form_peopleadd = new Form_PeopleAdd();
                        //加入体检
                        string errMsg = "";
                        string nl = (DateTime.Now.Year - dateTimePicker_D_CSRQ.Value.Year + 1).ToString();
                        string resultStr = form_peopleadd.Add_jktj(lTextBox_D_GRDABH.Text, lTextBox_D_SFZH.Text, lTextBox_D_XM.Text, comboBox_D_XB.Text, dateTimePicker_D_CSRQ.Value.ToString("yyyy-MM-dd"), nl, comboBox_ssjg_str, comboBox_sjcc_str,lTextBox_D_LXDH.Text,ref errMsg);
                        if (resultStr.Equals("1") == false)
                        {
                            //ErrMsg = errMsg;
                            //return false;
                            MessageBox.Show(errMsg);
                        }
                        else
                        {
                            MessageBox.Show("加入完成！");
                        }

                    }
                    else
                    {
                        return;
                    }
                }

                #region 体检信息
                ////生成体检人员信息
                //DataTable dt_tjryxx = new DataTable();
                //dt_tjryxx = form_daxqbll.GetMoHuList(string.Format(" and YLJGBM='{0}' and JKDAH='{1}' ", UserInfo.Yybm, lTextBox_D_GRDABH.Text), "sql_select_TJRYXX");
                ////已经存在
                //if (dt_tjryxx != null && dt_tjryxx.Rows.Count > 0)
                //{
                //    MessageBox.Show("已经存在体检信息！");
                //    return;
                //}

                //dt_tjryxx.Rows.Add();
                //dt_tjryxx.Rows[0]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
                //dt_tjryxx.Rows[0]["TJJHBM"] = DateTime.Now.ToString("yyyyMMdd");//体检计划编码
                //dt_tjryxx.Rows[0]["TJPCH"] = DateTime.Now.ToString("hhmmss");//体检批次号

                ////取得顺番号
                //DataTable dt_SFH = form_daxqbll.GetMoHuList("", "sql076");
                //if (dt_SFH != null && dt_SFH.Rows.Count > 0 && dt_SFH.Rows[0]["SFH"] != null && dt_SFH.Rows[0]["SFH"].ToString().Length > 0)
                //{
                //    dt_tjryxx.Rows[0]["SFH"] = int.Parse(dt_SFH.Rows[0]["SFH"].ToString()) + 1;//顺番号
                //}
                //else
                //{
                //    dt_tjryxx.Rows[0]["SFH"] = "0";//顺番号
                //}

                ////dt_tjryxx.Rows[0]["SFH"] = dt_tjryxx.Rows[0]["SFH"].ToString();//顺番号
                //////取得顺序号码
                ////DataTable dt_SXHM = form_daxqbll.GetMoHuList(string.Format(" and YLJGBM='{0}' and CZBM='{1}'", UserInfo.Yybm, comboBox_D_JWH.SelectedValue != null ? comboBox_D_JWH.SelectedValue.ToString() : ""), "sql072");
                ////if (dt_SXHM != null && dt_SXHM.Rows.Count > 0)
                ////{
                ////    dt_tjryxx.Rows[0]["SXHM"] = (int.Parse(dt_SXHM.Rows[0]["SXHM"].ToString()) + 1).ToString();//顺序号码
                ////}
                ////else
                ////{
                ////    dt_tjryxx.Rows[0]["SXHM"] = "1";//顺序号码
                ////}

                //dt_tjryxx.Rows[0]["SXHM"] = "0";

                //string strTJBM = lTextBox_D_GRDABH.Text.PadLeft(12, '0');
                //dt_tjryxx.Rows[0]["TJBM"] = strTJBM.Substring(strTJBM.Length -12);//个人体检编号
                //dt_tjryxx.Rows[0]["JKDAH"] = strTJBM;//个人健康档案号
                ////dt_tjryxx.Rows[0]["GRBM"] = "0";//个人编码
                //dt_tjryxx.Rows[0]["XM"] = lTextBox_D_XM.Text;//姓名
                //dt_tjryxx.Rows[0]["XB"] = comboBox_D_XB.SelectedValue != null ? comboBox_D_XB.SelectedValue.ToString() : "";//性别
                //dt_tjryxx.Rows[0]["SFZH"] = lTextBox_D_SFZH.Text;//身份证号
                //dt_tjryxx.Rows[0]["LXDH"] = lTextBox_D_LXDH.Text;//联系电话
                //dt_tjryxx.Rows[0]["CSRQ"] = dateTimePicker_D_CSRQ.Value.ToString("yyyy-MM-dd");//出生日期
                //dt_tjryxx.Rows[0]["CZBM"] = comboBox_D_JWH.SelectedValue != null ? comboBox_D_JWH.SelectedValue.ToString() : "";//村庄编码
                ////dt_tjryxx.Rows[0]["TJZT"] = "2";//体检状态
                ////dt_tjryxx.Rows[0]["TJSJ"] = "";//体检时间
                ////dt_tjryxx.Rows[0]["TJFZR"] = "";//体检负责人
                //dt_tjryxx.Rows[0]["FL"] = "2";//体检人员分类
                //dt_tjryxx.Rows[0]["BZ"] = "";//备注
                //dt_tjryxx.Rows[0]["TJBH_TEM"] = strTJBM;//临时个人体检编号
                //dt_tjryxx.Rows[0]["CREATETIME"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//创建时间
                //dt_tjryxx.Rows[0]["CREATEUSER"] = UserInfo.userId;//创建者
                //dt_tjryxx.Rows[0]["UPDATETIME"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//更新时间
                //dt_tjryxx.Rows[0]["UPDATEUSER"] = UserInfo.userId;//更新者
                //dt_tjryxx.Rows[0]["SCZT"] = "2";//数据上传状态
                ////dt_tjryxx.Rows[0]["LISZT"] = "2";//是否已经同步过LIS数据
                ////dt_tjryxx.Rows[0]["TJZT_zytz"] = "2";//体检状态中医体质
                ////dt_tjryxx.Rows[0]["TJSJ_zytz"] = "";//体检时间
                ////dt_tjryxx.Rows[0]["TJZT_jkzp"] = "2";//体检状态生活自理能力
                ////dt_tjryxx.Rows[0]["TJSJ_jkzp"] = "";//体检时间
                //dt_tjryxx.Rows[0]["ZLBZ"] = "1";//增量标志
                //dt_tjryxx.Rows[0]["nd"] = DateTime.Now.Year.ToString();//年度
                ////dt_tjryxx.Rows[0]["djzt"] = "0";//登记
                //dt_tjryxx.Rows[0]["ISSH"] = "0";//登记
                //dt_tjryxx.Rows[0]["ISNEWDOC"] = "1";//登记

                //dt_tjryxx.Rows[0]["PRID"] =comboBox_ssjg.SelectedValue!=null? comboBox_ssjg.SelectedValue.ToString():"";//所属机构

                ////增加体检人员信息
                //form_daxqbll.Add(dt_tjryxx, "sql_add_people");

                ////生成lis信息
                //DataTable dt_tmList = new DataTable(); //sql030
                //dt_tmList = form_daxqbll.GetMoHuList(string.Format(" and  YLJGBM ='{0}' ", UserInfo.Yybm), "sql030");

                //if (dt_tmList != null && dt_tmList.Rows.Count > 0)
                //{
                //    //主表
                //    DataTable dt_reqmain = new DataTable();
                //    dt_reqmain = form_daxqbll.GetMoHuList(" and 1=2 ", "sql070");
                //    //明细表
                //    DataTable dt_reqdetail = new DataTable();
                //    dt_reqdetail = form_daxqbll.GetMoHuList(" and 1=2 ", "sql071");

                //    for (int i = 0; i < dt_tmList.Rows.Count; i++)
                //    {
                //        string sqh = lTextBox_D_GRDABH.Text.ToString().PadLeft(12, '0') + dt_tmList.Rows[i]["TMBM"].ToString();
                //        sqh = sqh.Substring(sqh.Length - 14, 14);

                //        dt_reqmain.Rows.Add();
                //        //主表
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sqh"] = sqh; //申请号
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ksdh"] = "";//送检科室代码
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sqys"] = "";//申请医生代码
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sqsj"] = DateTime.Now.ToString("yyyy-MM-dd");//申请时间
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jsys"] = "";//接收医生
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jssj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//接收时间
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zt"] = "1";//状态
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jjzt"] = "1";//计价状态
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brly"] = "4";//病人来源
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brdh"] = lTextBox_D_GRDABH.Text.ToString();//病历号
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brxm"] = lTextBox_D_XM.Text;//病人姓名
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brxb"] = comboBox_D_XB.SelectedValue != null ? comboBox_D_XB.SelectedValue.ToString() : "";//病人性别
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brsr"] = dateTimePicker_D_CSRQ.Value.ToString("yyyy-MM-dd");//病人生日
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bz1"] = dt_tmList.Rows[i]["SQXMDH"].ToString(); ;
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bz2"] = "";
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bz3"] = "";
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jzbz"] = "0";//急诊标志
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["txm"] = "";
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ch"] = "";//床号
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["yblx"] = "";//样本类型
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zxys"] = "";//执行医生
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zxsj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//执行时间
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bgddh"] = "";
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["costs"] = 0;
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["nl"] = 0;//年龄
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["nldw"] = "";//年龄单位
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zd"] = "";//临床诊断
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["cysj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//采样时间
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["cksj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ckzj"] = "";
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ckyh"] = "";
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sfzh"] = lTextBox_D_SFZH.Text;//身份证号
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jkdah"] = lTextBox_D_GRDABH.Text.ToString();//健康档案号
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["yybm"] = UserInfo.Yybm;//医院编码
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["dataFrom"] = "1";//数据来源
                //        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zlbz"] = "1";//增量标志
                //        //dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString() ;//年度

                //        //生成明细
                //        string tmfl = dt_tmList.Rows[i]["sqxmdh"] != null ? dt_tmList.Rows[i]["sqxmdh"].ToString() : "";
                //        DataTable dt_tmList_reqdetail = form_daxqbll.GetMoHuList(string.Format(" and  YLJGBM ='{0}' and SQXMDH='{1}' ", UserInfo.Yybm, tmfl), "sql078");
                //        if (dt_tmList_reqdetail != null)
                //        {
                //            for (int j = 0; j < dt_tmList_reqdetail.Rows.Count; j++)
                //            {
                //                dt_reqdetail.Rows.Add();
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sqh"] = sqh; //申请号
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["xh"] = (j + 1).ToString();//序号
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sqxmdh"] = dt_tmList_reqdetail.Rows[j]["ITEM_CODE"].ToString();//申请项目代码
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sqxmmc"] = dt_tmList_reqdetail.Rows[j]["ITEM_NAME"].ToString();//申请项目名称
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sl"] = "1";//数量
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dj"] = "0";//单价
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["zt"] = "1";//状态
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["jjzt"] = "1";//计价状态
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["bz1"] = "";//备注1字段
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["bz2"] = "";//
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["costs"] = 0;//
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["numbk1"] = 0;//
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["numbk2"] = 0;//
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dtbk1"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dtbk2"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["yybm"] = UserInfo.Yybm;//医院编码
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dataFrom"] = "1";//数据来源
                //                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["zlbz"] = "1";//增量标志
                //            }
                //        }
                //    }

                //    //检验主表
                //    form_daxqbll.Add(dt_reqmain, "sql074");

                //    //检验明细表
                //    form_daxqbll.Add(dt_reqdetail, "sql075");
                //    MessageBox.Show("保存成功！");
                //}
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion


        /// <summary>
        /// 绑定数据值
        /// </summary>
        /// <param name="drp"></param>
        /// <param name="initValue"></param>
        /// <returns></returns>
        public bool setDrp(ComboBox drp, string strwhere, string sqlCode, bool ifkh)
        {
            try
            {
                drpflag = false;
                Form_daxqBll form_daxq = new Form_daxqBll();

                //获取结果集
                DataTable dt = form_daxq.GetMoHuList(strwhere, sqlCode);
                if (dt == null)
                {
                    return false;
                }
                if (ifkh == true)
                {
                    DataRow dtRow = dt.NewRow();
                    dt.Rows.InsertAt(dtRow, 0);
                }

                drp.DisplayMember = "B_NAME";
                drp.ValueMember = "B_RGID";
                drp.DataSource = dt;
                drpflag = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 省
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_SHENG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpflag == false)
            {
                return;
            }
        }

        /// <summary>
        /// 市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_SHI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpflag == false)
            {
                return;
            }
            //县
            if (comboBox_D_SHI.SelectedValue != null && comboBox_D_SHI.SelectedValue.ToString().Length > 0)
            {
                setDrp(comboBox_D_QU, string.Format(" and B_RGID like '%{0}%'", comboBox_D_SHI.SelectedValue.ToString()), "sql053", true);
            }
            else
            {
                comboBox_D_QU.DataSource = null;
            }
            //镇
            comboBox_D_JD.DataSource = null;
            //村
            comboBox_D_JWH.DataSource = null;
        }
        /// <summary>
        /// 县
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_QU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpflag == false)
            {
                return;
            }
            //镇
            if (comboBox_D_QU.SelectedValue != null && comboBox_D_QU.SelectedValue.ToString().Length > 0)
            {
                setDrp(comboBox_D_JD, string.Format(" and B_RGID like '%{0}%'", comboBox_D_QU.SelectedValue.ToString()), "sql054", true);
            }
            else
            {
                comboBox_D_JD.DataSource = null;
            }
            //村
            comboBox_D_JWH.DataSource = null;
        }
        /// <summary>
        /// 镇
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_JD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpflag == false)
            {
                return;
            }
            //村
            if (comboBox_D_JD.SelectedValue != null && comboBox_D_JD.SelectedValue.ToString().Length > 0)
            {
                setDrp(comboBox_D_JWH, string.Format(" and B_RGID like '%{0}%'", comboBox_D_JD.SelectedValue.ToString()), "sql055", true);
            }
            else
            {
                comboBox_D_JWH.DataSource = null;
            }

        }
        /// <summary>
        /// 村
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_JWH_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (drpflag == false)
            {
                return;
            }
            czbm = comboBox_D_JWH.SelectedValue != null ? comboBox_D_JWH.SelectedValue.ToString() : "";

        }

        /// <summary>
        /// 档案检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            check_daBysfzh();
            //string str_d_grdabh = "";

            //if (check_lkda(out str_d_grdabh) == true)
            //{

            //    DialogResult result;
            //    result = MessageBox.Show("档案已经存在,是否显示档案信息？ \r\n是：显示档案信息 \r\n否：不显示档案信息 \r\n取消：取消本次操作", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    if (result == DialogResult.Cancel)
            //    {

            //    }
            //    else if (result == DialogResult.Yes)
            //    {
            //        //显示档案信息
            //        setDataTopage(string.Format("  and D_GRDABH='{0}' ", str_d_grdabh));
            //        lTextBox_D_GRDABH.Text = str_d_grdabh;
            //    }
            //    else
            //    {
            //        lTextBox_D_GRDABH.Text = str_d_grdabh;
            //    }



            //    // MessageBox.Show("档案已经存在!");

            //}
            //else
            //{
            //    if (lTextBox_D_GRDABH.Text.Length > 0)
            //    {
            //        MessageBox.Show("临时档案号可以正常使用!");
            //    }
            //    else
            //    {
            //        MessageBox.Show("系统自动产生临时档案号!");
            //    }
            //    lTextBox_D_GRDABH.Text = str_d_grdabh;
            //}

        }


        /// <summary>
        /// 按照身份证号验证档案
        /// </summary>
        public void check_daBysfzh()
        {
            string str_d_grdabh = "";

            if (check_lkda(out str_d_grdabh) == true)
            {

                DialogResult result;
                result = MessageBox.Show("档案已经存在,是否显示档案信息？ \r\n是：显示档案信息 \r\n否：不显示档案信息 \r\n取消：取消本次操作", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {

                }
                else if (result == DialogResult.Yes)
                {
                    //显示档案信息
                    setDataTopage(string.Format("  and D_GRDABH='{0}' ", str_d_grdabh));
                    lTextBox_D_GRDABH.Text = str_d_grdabh;
                }
                else
                {
                    lTextBox_D_GRDABH.Text = str_d_grdabh;
                }
                // MessageBox.Show("档案已经存在!");

            }
            else
            {
                if (lTextBox_D_GRDABH.Text.Length > 0)
                {
                    MessageBox.Show("临时档案号可以正常使用!");
                }
                else
                {
                    MessageBox.Show("系统自动产生临时档案号!");
                }
                lTextBox_D_GRDABH.Text = str_d_grdabh;
            }
        }




        /// <summary>
        /// 档案检测
        /// </summary>
        /// <returns></returns>
        public bool check_lkda(out string str_d_grdabh)
        {
            //健康档案号
            str_d_grdabh = "";
            Form_daxqBll form_daxq = new Form_daxqBll();

            //获取结果集
            DataTable dt = form_daxq.GetMoHuList(string.Format(" and ( D_GRDABH='{0}' or D_zjhqt='{1}') and D_zjhqt <> '' and  D_GRDABH  <> '' ", lTextBox_D_GRDABH.Text.Trim(), lTextBox_D_SFZH.Text.Trim()), "sql056");
            if (dt != null && dt.Rows.Count > 0)
            {
                str_d_grdabh = dt.Rows[0]["D_GRDABH"].ToString();
                return true;
            }
            else
            {
                if (lTextBox_D_GRDABH.Text.Trim().Length == 0)
                {
                    str_d_grdabh = DateTime.Now.ToString("MMddhhmmssff");
                }
                else
                {
                    str_d_grdabh = lTextBox_D_GRDABH.Text.Trim();
                }
                return false;
            }
        }

        /// <summary>
        /// 健康状况检测
        /// </summary>
        /// <returns></returns>
        public bool check_jkzk()
        {
            Form_daxqBll form_daxq = new Form_daxqBll();

            //获取结果集
            DataTable dt = form_daxq.GetMoHuList(string.Format(" and  D_GRDABH='{0}'  ", lTextBox_D_GRDABH.Text.Trim()), "sql_T_DA_JKDA_JKZK_select");
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 组合选中的村庄编码
        /// </summary>
        /// <returns></returns>
        public string getSelectList(CheckedListBox chkBoxList, string itemName)
        {
            int listCount = 0;
            DataTable dt = (DataTable)chkBoxList.DataSource;
            string strCollected = string.Empty;
            for (int i = 0; i < chkBoxList.Items.Count; i++)
            {
                if (chkBoxList.GetItemChecked(i) == true)
                {
                    listCount++;
                    if (strCollected == string.Empty)
                    {
                        strCollected = dt.Rows[i][itemName].ToString();
                    }
                    else
                    {
                        strCollected = strCollected + "," + dt.Rows[i][itemName].ToString();
                    }
                }
            }
            return strCollected;
        }

        /// <summary>
        /// 设定选中项目
        /// </summary>
        /// <param name="selectvalue"></param>
        public void setItemSelect(CheckedListBox chkBoxList, string selectvalue, string itemName)
        {
            DataTable dt = (DataTable)chkBoxList.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            string[] selectList = selectvalue.Split(new char[] { ',' });
            for (int i = 0; i < selectList.Length; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                    if (selectList[i].Equals(dt.Rows[j][itemName].ToString()))
                    {
                        chkBoxList.SetItemChecked(j, true);
                        break;
                    }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 健康档案号keydown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lTextBox_D_GRDABH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {

                if (lTextBox_D_GRDABH.Text.Length > 0)
                {
                    lTextBox_D_GRDABH.Text = lTextBox_D_GRDABH.Text.PadLeft(12, '0');
                }
            }
        }

        private void hzxm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (text_hzsfzh.Text.Trim().Length > 0)
                {
                    //MousePosition.X
                    //Point textLocation = PointToScreen(mc.Location);
                    //textLocation.X = textLocation.X - 10;
                    //textLocation.Y = textLocation.Y -50;
                    Point lco = new Point();
                    lco.X = text_hzsfzh.Location.X + 10;
                    lco.Y = text_hzsfzh.Location.Y + 50;
                    DropDownGrid form = new DropDownGrid();
                    form.Location = lco;
                    form.Owner = this;

                    //form.setInfo(hzxm.Text.Trim(), this);
                    form.ShowDialog();
                    if (text_hzsfzh.Text != "")
                        text_hzsfzh.Focus();
                }
                else
                {
                    Enter_Tab_KeyDown(sender, e);
                }
            }

        }
        /// <summary>
        /// 设定单元格的内容
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="cellIndex"></param>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool setValue(DataRow dr, string code)
        {
            try
            {
                if (code == "") return true;

                //hzxm.Text = code;

                //证件信息
                text_hzsfzh.Text = dr["D_SFZH"].ToString();


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 读身份证号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_sfzh_Click(object sender, EventArgs e)
        {
            //读取身份证号
            DataTable dtsfzh = form_readsfzh.readSfzh();
            if (dtsfzh != null && dtsfzh.Rows.Count > 0)
            {
                lTextBox_D_SFZH.Text = dtsfzh.Rows[0]["身份证号"].ToString().Trim();
                lTextBox_D_XM.Text = dtsfzh.Rows[0]["姓名"].ToString().Trim();
                comboBox_D_XB.SelectedValue = dtsfzh.Rows[0]["性别编码"].ToString().Trim();
                dateTimePicker_D_CSRQ.Value = Convert.ToDateTime(dtsfzh.Rows[0]["出生日期"].ToString().Trim());
                textBox_sfzdz.Text = dtsfzh.Rows[0]["地址"].ToString().Trim();

                //设定个人信息到页面
                setDataTopage(string.Format("  and D_SFZH='{0}' ", lTextBox_D_SFZH.Text));

                //本人是户主的 自动设定户主身份证号
                if (comboBox_D_YHZGX.SelectedValue != null && comboBox_D_YHZGX.SelectedValue.ToString().Equals("1"))
                {
                    text_hzsfzh.Text = lTextBox_D_SFZH.Text;
                }

                string message = "";
                if (checksfz_nl(out message) == false)
                {
                    MessageBox.Show(message);
                }
            }
        }

        /// <summary>
        /// 设定身份证号
        /// </summary>
        /// <param name="dtSfzh"></param>
        /// <returns></returns>
        public override bool setTextFromDb(DataTable dtSfzh)
        {
            //读取身份证号
            if (dtSfzh != null && dtSfzh.Rows.Count > 0)
            {
                if (dtSfzh.Rows[0]["身份证号"].ToString().Equals(lTextBox_D_SFZH.Text) == false)
                {
                    lTextBox_D_SFZH.Text = dtSfzh.Rows[0]["身份证号"].ToString();
                    lTextBox_D_XM.Text = dtSfzh.Rows[0]["姓名"].ToString();
                    comboBox_D_XB.SelectedValue = dtSfzh.Rows[0]["性别"].ToString();
                    dateTimePicker_D_CSRQ.Value = Convert.ToDateTime(dtSfzh.Rows[0]["出生日期"].ToString());
                    string message = "";
                    if (checksfz_nl(out message) == false)
                    {
                        MessageBox.Show(message);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 设定返回的结果
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public override bool setStatus(string Status)
        {

            return true;
        }


        /// <summary>
        /// 验证身份证是否符合年龄
        /// </summary>
        /// <returns></returns>
        public bool checksfz_nl(out string message)
        {
            message = "";
            if (lTextBox_D_SFZH.Text.Length > 0)
            {
                //当年度年底
                DateTime now_year = Convert.ToDateTime(string.Format("{0}-12-31", DateTime.Now.Year.ToString())).AddYears(-65);
                if (dateTimePicker_D_CSRQ.Value > now_year)
                {
                    message = string.Format("出生日期为:{0} 到本年年底:{1} 还不满65岁！", dateTimePicker_D_CSRQ.Value.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 焦点离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lTextBox_D_SFZH_MouseLeave(object sender, EventArgs e)
        {
            if (lTextBox_D_SFZH.Text.Length > 0)
            {
                //如果是合法的身份证取信息
                if (Common.CheckIDCard(lTextBox_D_SFZH.Text.Trim()) == true)
                {
                    try
                    {
                        dateTimePicker_D_CSRQ.Value = Convert.ToDateTime(Common.GetBirthdayByIdentityCardId(lTextBox_D_SFZH.Text.Trim(), true));
                        string message = "";
                        if (checksfz_nl(out message) == false)
                        {
                            MessageBox.Show(message);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }


        /// <summary>
        /// 按照村庄编码初始化行政机构
        /// </summary>
        /// <param name="czbm"></param>
        /// <returns></returns>
        public bool initXzjgBycz(string czbm)
        {
            if (czbm.Length != 11)
            {
                return false;
            }

            //乡镇  37160211 013
            string xiangzhen = czbm.Substring(0, 8);
            //县
            string xian = czbm.Substring(0, 6);
            //市
            string shi = czbm.Substring(0, 4);
            //省
            string sheng = czbm.Substring(0, 2);

            comboBox_D_SHENG.SelectedValue = sheng;
            //市
            comboBox_D_SHI.SelectedValue = shi;
            //县
            comboBox_D_QU.SelectedValue = xian;
            //镇
            comboBox_D_JD.SelectedValue = xiangzhen;
            //村
            comboBox_D_JWH.SelectedValue = czbm;

            //drpflag = false;

            ////省
            //setDrp(comboBox_D_SHENG, "", "sql051", true);
            ////市
            //setDrp(comboBox_D_SHI, string.Format("and B_RGID='{0}'", sheng), "sql052", true);
            ////县
            //setDrp(comboBox_D_QU, string.Format("and B_RGID='{0}'", shi), "sql053", true);
            ////乡镇
            //setDrp(comboBox_D_JD, string.Format("and B_RGID='{0}'", xian), "sql054", true);
            ////村庄
            //setDrp(comboBox_D_JWH, string.Format("and B_RGID='{0}'", xiangzhen), "sql055", true);
            //drpflag = true;

            return true;
        }

        private void comboBox_ssjg_SelectedIndexChanged(object sender, EventArgs e)
        {
            //所属机构
            setDrp(comboBox_lrr, string.Format(" and p_RGid='{0}'", comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : ""), "sql086", true);
        }



        /// <summary>
        /// 查询户主
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_查询户主_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        /// <summary>
        /// 户主关系发生变化时业务处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_YHZGX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_D_YHZGX.SelectedValue == null)
            {
                return;
            }
            if (comboBox_D_YHZGX.SelectedValue.ToString() == "1")
            {
                text_hzsfzh.Text = lTextBox_D_SFZH.Text;
            }
        }

        #region 既往病史处理

        //是否是编辑处理
        public bool isupdate = false;
        public int rowIndex_jb = 0;

        /// <summary>
        /// 疾病无
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_jb_1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_jb_1.Checked == false)
            {
                return;
            }
            setChecklist("checkBox_jb_", 1, 12, "");
            checkBox_jb_1.Enabled = false;
            checkBox_jb_2.Enabled = false;
            checkBox_jb_3.Enabled = false;
            checkBox_jb_4.Enabled = false;
            checkBox_jb_5.Enabled = false;
            checkBox_jb_6.Enabled = false;
            checkBox_jb_7.Enabled = false;
            checkBox_jb_8.Enabled = false;
            checkBox_jb_9.Enabled = false;
            checkBox_jb_10.Enabled = false;
            checkBox_jb_11.Enabled = false;
            checkBox_jb_12.Enabled = false;

            //确诊时间
            dateTime_jb_qzsj.Checked = false;

            //恶性肿瘤
            textbox_EXZL.Text = "";
            textbox_EXZL.Enabled = false;

            //职业病其他
            textBox_zybqt.Text = "";
            textBox_zybqt.Enabled = false;

            //其他
            textBox_D_JBMCQT.Text = "";
            textBox_D_JBMCQT.Enabled = false;
            button_疾病.Enabled = false;
            //状态
            isupdate = false;
            if (dt_jb != null)
            {
                DataTable dt_jb_tem = dt_jb.Clone();
                dt_jb = dt_jb_tem;
                dt_jb.AcceptChanges();
                dataGridView_jb.DataSource = dt_jb;
            }
        }

        /// <summary>
        /// 疾病有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_jb_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_jb_2.Checked == false)
            {
                return;
            }
            checkBox_jb_1.Enabled = true;
            checkBox_jb_2.Enabled = true;
            checkBox_jb_3.Enabled = true;
            checkBox_jb_4.Enabled = true;
            checkBox_jb_5.Enabled = true;
            checkBox_jb_6.Enabled = true;
            checkBox_jb_7.Enabled = true;
            checkBox_jb_8.Enabled = true;
            checkBox_jb_9.Enabled = true;
            checkBox_jb_10.Enabled = true;
            checkBox_jb_11.Enabled = true;
            checkBox_jb_12.Enabled = true;
            //恶性肿瘤
            textbox_EXZL.Enabled = true;

            //职业病其他
            textBox_zybqt.Enabled = true;

            //其他
            textBox_D_JBMCQT.Enabled = true;
            button_疾病.Enabled = true;
            dateTime_jb_qzsj.Enabled = true;
        }

        /// <summary>
        /// 疾病确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_疾病_Click(object sender, EventArgs e)
        {
            string valueList = "";
            string textList = "";
            getChecklist("checkBox_jb_", 1, 12, out valueList, out textList);

            if (valueList.Length == 0)
            {
                MessageBox.Show("请选择疾病种类！");
                return;
            }

            if (dataGridView_jb.DataSource == null)
            {
                dt_jb = createtable("jwbs_jb");
            }
            else
            {
                dt_jb = (DataTable)dataGridView_jb.DataSource;
            }

            //增加新行
            if (isupdate == false)
            {
                //增加新行
                dt_jb.Rows.Add();

                //疾病编码
                dt_jb.Rows[dt_jb.Rows.Count - 1]["D_JBBM"] = textList; 
                //疾病名称
                dt_jb.Rows[dt_jb.Rows.Count - 1]["D_JBMC"] = valueList;

                //确诊时间
                if (dateTime_jb_qzsj.Checked == true)
                {
                    dt_jb.Rows[dt_jb.Rows.Count - 1]["D_ZDRQ"] = dateTime_jb_qzsj.Value.ToString("yyyy-MM");
                }
                //恶性肿瘤
                dt_jb.Rows[dt_jb.Rows.Count - 1]["EXZL"] = textbox_EXZL.Text ;
                //疾病类型
                dt_jb.Rows[dt_jb.Rows.Count - 1]["D_JBLX"] = "疾病";
                //职业病其他
                dt_jb.Rows[dt_jb.Rows.Count - 1]["zybqt"] = textBox_zybqt.Text ;
                //其他
                dt_jb.Rows[dt_jb.Rows.Count - 1]["D_JBMCQT"] = textBox_D_JBMCQT.Text;
            }
            else
            {
                //疾病编码
                dt_jb.Rows[rowIndex_jb]["D_JBBM"] = textList;
                //疾病名称
                dt_jb.Rows[rowIndex_jb]["D_JBMC"] = valueList;
                //确诊时间
                if (dateTime_jb_qzsj.Checked == true)
                {
                    dt_jb.Rows[rowIndex_jb]["D_ZDRQ"] = dateTime_jb_qzsj.Value.ToString("yyyy-MM");
                }
                //恶性肿瘤
                dt_jb.Rows[rowIndex_jb]["EXZL"] = textbox_EXZL.Text;
                //疾病类型
                dt_jb.Rows[rowIndex_jb]["D_JBLX"] = "疾病";
                //职业病其他
                dt_jb.Rows[rowIndex_jb]["zybqt"] = textBox_zybqt.Text;
                //其他
                dt_jb.Rows[rowIndex_jb]["D_JBMCQT"] = textBox_D_JBMCQT.Text;
            }

            setChecklist("checkBox_jb_", 1, 12, "");
            //确诊时间
            dateTime_jb_qzsj.Checked = false;
            //恶性肿瘤
            textbox_EXZL.Text ="";
            //职业病其他
            textBox_zybqt.Text = "";
            //其他
            textBox_D_JBMCQT.Text ="";

            //状态
            isupdate = false;

            dataGridView_jb.DataSource = dt_jb;
        }

        /// <summary>
        /// 疾病行选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_jb_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowindex = e.RowIndex;

                dt_jb = dataGridView_jb.DataSource as DataTable;
                //DataTable dt_tem = dataGridView_jb.DataSource as DataTable;

                //编辑
                if (dataGridView_jb.Columns[e.ColumnIndex].Name == "cz_update")
                {
                    //疾病名称
                    setChecklist("checkBox_jb_", 1, 12, dt_jb.Rows[rowindex]["D_JBMC"].ToString());
                    //确诊时间
                    if (dt_jb.Rows[rowindex]["D_ZDRQ"] != null && dt_jb.Rows[rowindex]["D_ZDRQ"].ToString().Length >0)
                    {
                        dateTime_jb_qzsj.Checked = true;
                        dateTime_jb_qzsj.Value = Convert.ToDateTime(dt_jb.Rows[rowindex]["D_ZDRQ"].ToString()+"-01");
                    }

                    //恶性肿瘤
                    textbox_EXZL.Text = dt_jb.Rows[rowindex]["EXZL"].ToString();

                    //职业病其他
                    textBox_zybqt.Text = dt_jb.Rows[rowindex]["zybqt"].ToString();
                    //其他
                    textBox_D_JBMCQT.Text = dt_jb.Rows[rowindex]["D_JBMCQT"].ToString();

                    //编辑
                    isupdate = true;

                    //当前编辑的行号
                    rowIndex_jb = rowindex;
                }
                else if (dataGridView_jb.Columns[e.ColumnIndex].Name == "cz")
                {
                     DialogResult  dialogresult = MessageBox.Show("确定要删除该数据吗?", "提示",MessageBoxButtons.OKCancel);
                     if (dialogresult == DialogResult.OK)
                     {
                         dt_jb.Rows.RemoveAt(rowindex);
                         dt_jb.AcceptChanges();

                         setChecklist("checkBox_jb_", 1, 12, "");
                         //确诊时间
                         dateTime_jb_qzsj.Checked = false;
                         //恶性肿瘤
                         textbox_EXZL.Text = "";
                         //职业病其他
                         textBox_zybqt.Text = "";
                         //其他
                         textBox_D_JBMCQT.Text = "";
                         //状态
                         isupdate = false;
                         dataGridView_jb.DataSource = dt_jb;
                     }

                }
            }
        }

        /// <summary>
        /// 生成既往病史的表结构
        /// </summary>
        /// <param name="datatableName"></param>
        /// <returns></returns>
        public DataTable createtable(string datatableName)
        {
            DataTable dt_tem = new DataTable();
            dt_tem.TableName = datatableName;

            //个人档案编号
            dt_tem.Columns.Add("D_GRDABH");
            //疾病名称
            dt_tem.Columns.Add("D_JBMC");
            //疾病编码
            dt_tem.Columns.Add("D_JBBM");
            //诊断日期
            dt_tem.Columns.Add("D_ZDRQ");
            //诊断单位
            dt_tem.Columns.Add("D_ZDDW");
            //处理情况
            dt_tem.Columns.Add("D_CLQK");
            //转归情况
            dt_tem.Columns.Add("D_ZGQK");
            //区域编号
            dt_tem.Columns.Add("P_RGID");
            //创建时间
            dt_tem.Columns.Add("CREATETIME");
            //修改时间
            dt_tem.Columns.Add("UPDATETIME");
            //修改创建时间
            dt_tem.Columns.Add("HAPPENTIME");
            //创建者姓名
            dt_tem.Columns.Add("CREATEUSER");
            //修改者姓名
            dt_tem.Columns.Add("UPDATEUSER");
            //创建地区
            dt_tem.Columns.Add("CREATREGION");
            //疾病类型
            dt_tem.Columns.Add("D_JBLX");
            //恶性肿瘤
            dt_tem.Columns.Add("EXZL");
            //疾病其他
            dt_tem.Columns.Add("JBQT");
            //职业病其他
            dt_tem.Columns.Add("zybqt");
            //其他
            dt_tem.Columns.Add("D_JBMCQT");
            dt_tem.Columns.Add("JKBS");

            return dt_tem;
        }
        #endregion

        #region  手术

        //是否是编辑处理
        public bool isupdate_ss = false;
        public int rowIndex_ss = 0;

        /// <summary>
        /// 手术无
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_ss_1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ss_1.Checked == false)
            {
                return;
            }
            //手术名称
            textBox_ss_shmc.Enabled = false;
            textBox_ss_shmc.Text = "";

            //手术时间
            dateTime_ss_sssj.Checked = false;
            dateTime_ss_sssj.Enabled = false;

            //确定
            button_手术.Enabled = false;

            //状态
            isupdate_ss = false;
            if (dt_ss != null)
            {
                DataTable dt_ss_tem = dt_ss.Clone();
                dt_ss = dt_ss_tem;
                dt_ss.AcceptChanges();
                dataGridView_ss.DataSource = dt_ss;
            }
        }
        /// <summary>
        /// 手术有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_ss_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ss_2.Checked == false)
            {
                return;
            }
            //手术名称
            textBox_ss_shmc.Enabled = true;

            //手术时间
            dateTime_ss_sssj.Checked = false ;
            dateTime_ss_sssj.Enabled = true;

            //确定
            button_手术.Enabled = true;

            //状态
            isupdate_ss = false;
        }

        /// <summary>
        /// 手术确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_手术_Click(object sender, EventArgs e)
        {
            if (textBox_ss_shmc.Text.Length == 0)
            {
                MessageBox.Show("请录入手术信息！");
                return;
            }

            if (dataGridView_ss.DataSource == null)
            {
                dt_ss = createtable("jwbs_ss");
            }
            else
            {
                dt_ss = (DataTable)dataGridView_ss.DataSource;
            }

            //增加新行
            if (isupdate_ss == false)
            {
                //增加新行
                dt_ss.Rows.Add();

                //疾病名称
                dt_ss.Rows[dt_ss.Rows.Count - 1]["D_JBMC"] = textBox_ss_shmc.Text;

                //手术时间
                if (dateTime_ss_sssj.Checked == true)
                {
                    dt_ss.Rows[dt_ss.Rows.Count - 1]["D_ZDRQ"] = dateTime_ss_sssj.Value.ToString("yyyy-MM");
                }
                
                //疾病类型
                dt_ss.Rows[dt_ss.Rows.Count - 1]["D_JBLX"] = "手术";
            }
            else
            {
                //手术名称
                dt_ss.Rows[rowIndex_ss]["D_JBMC"] = textBox_ss_shmc.Text;

                //手术时间
                if (dateTime_ss_sssj.Checked == true)
                {
                    dt_ss.Rows[rowIndex_ss]["D_ZDRQ"] = dateTime_ss_sssj.Value.ToString("yyyy-MM");
                }
            }

            //手术时间
            dateTime_ss_sssj.Checked = false;
            //手术名称
           textBox_ss_shmc.Text = "";
           

            //状态
           isupdate_ss = false;
           rowIndex_ss = 0;

           dataGridView_ss.DataSource = dt_ss;
        }

        /// <summary>
        /// 手术行选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_ss_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowindex = e.RowIndex;
                dt_ss = (DataTable)dataGridView_ss.DataSource;

                //编辑
                if (dataGridView_ss.Columns[e.ColumnIndex].Name == "update_ss")
                {

                    //手术时间
                    if (dt_ss.Rows[rowindex]["D_ZDRQ"] != null && dt_ss.Rows[rowindex]["D_ZDRQ"].ToString().Length > 0)
                    {
                        dateTime_ss_sssj.Checked = true;
                        dateTime_ss_sssj.Value = Convert.ToDateTime(dt_ss.Rows[rowindex]["D_ZDRQ"].ToString() + "-01");
                    }

                    //手术名称
                    textBox_ss_shmc.Text = dt_ss.Rows[rowindex]["D_JBMC"].ToString();

                    //编辑
                    isupdate_ss = true;

                    //当前编辑的行号
                    rowIndex_ss = rowindex;
                }
                else if (dataGridView_ss.Columns[e.ColumnIndex].Name == "ss_cz")
                {
                    DialogResult dialogresult = MessageBox.Show("确定要删除该数据吗?", "提示", MessageBoxButtons.OKCancel);
                    if (dialogresult == DialogResult.OK)
                    {
                        dt_ss.Rows.RemoveAt(rowindex);
                        dt_ss.AcceptChanges();

                        //确诊时间
                        dateTime_ss_sssj.Checked = false;
                        //手术名称
                        textBox_ss_shmc.Text = "";
                        
                        //状态
                        isupdate_ss = false;
                        dataGridView_ss.DataSource = dt_ss;
                    }

                }
            }
        }

        #endregion

        #region 外伤

        //是否是编辑处理
        public bool isupdate_ws = false;
        public int rowIndex_ws = 0;

        /// <summary>
        /// 外伤  无
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_ws_1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ws_1.Checked == false)
            {
                return;
            }
            //外伤名称
            textBox_wsmc.Enabled = false;
            textBox_wsmc.Text = "";

            //外伤时间
            dateTime_wssj.Checked = false;
            dateTime_wssj.Enabled = false;

            //确定
            button_外伤.Enabled = false;

            //状态
            isupdate_ws = false;
            rowIndex_ws = 0;

            if (dt_ws != null)
            {
                DataTable dt_ws_tem = dt_ws.Clone();
                dt_ws = dt_ws_tem;
                dt_ws.AcceptChanges();
                dataGridView_ws.DataSource = dt_ws;
            }
        }
        /// <summary>
        /// 外伤  有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_ws_2_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton_ws_2.Checked == false)
            {
                return;
            }
            //外伤名称
            textBox_wsmc.Enabled = true;

            //外伤时间
            dateTime_wssj.Checked = false;
            dateTime_wssj.Enabled = true;

            //确定
            button_外伤.Enabled = true;

            //状态
            isupdate_ws = false;
            rowIndex_ws = 0;
        }

        /// <summary>
        /// 外伤 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_外伤_Click(object sender, EventArgs e)
        {
            if (textBox_wsmc.Text.Length == 0)
            {
                MessageBox.Show("请录入外伤信息！");
                return;
            }

            if (dataGridView_ws.DataSource == null)
            {
                dt_ws = createtable("jwbs_ws");
            }
            else
            {
                dt_ws = (DataTable)dataGridView_ws.DataSource;
            }

            //增加新行
            if (isupdate_ws == false)
            {
                //增加新行
                dt_ws.Rows.Add();

                //外伤名称
                dt_ws.Rows[dt_ws.Rows.Count - 1]["D_JBMC"] = textBox_wsmc.Text;

                //外伤时间
                if (dateTime_wssj.Checked == true)
                {
                    dt_ws.Rows[dt_ws.Rows.Count - 1]["D_ZDRQ"] = dateTime_wssj.Value.ToString("yyyy-MM");
                }

                //疾病类型
                dt_ws.Rows[dt_ws.Rows.Count - 1]["D_JBLX"] = "外伤";
            }
            else
            {
                //外伤名称
                dt_ws.Rows[rowIndex_ws]["D_JBMC"] = textBox_wsmc.Text;

                //外伤时间
                if (dateTime_wssj.Checked == true)
                {
                    dt_ws.Rows[rowIndex_ws]["D_ZDRQ"] = dateTime_wssj.Value.ToString("yyyy-MM");
                }
            }

            //外伤时间
            dateTime_wssj.Checked = false;
            //外伤名称
            textBox_wsmc.Text = "";


            //状态
            isupdate_ws = false;
            rowIndex_ws = 0;

            dataGridView_ws.DataSource = dt_ws;

        }
        /// <summary>
        /// 外伤  行选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_ws_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowindex = e.RowIndex;
                dt_ws = (DataTable)dataGridView_ws.DataSource;
                //编辑
                if (dataGridView_ws.Columns[e.ColumnIndex].Name == "update_ws")
                {

                    //外伤时间
                    if (dt_ws.Rows[rowindex]["D_ZDRQ"] != null && dt_ws.Rows[rowindex]["D_ZDRQ"].ToString().Length > 0)
                    {
                        dateTime_wssj.Checked = true;
                        dateTime_wssj.Value = Convert.ToDateTime(dt_ws.Rows[rowindex]["D_ZDRQ"].ToString() + "-01");
                    }

                    //外伤名称
                    textBox_wsmc.Text = dt_ws.Rows[rowindex]["D_JBMC"].ToString();

                    //编辑
                    isupdate_ws = true;

                    //当前编辑的行号
                    rowIndex_ws = rowindex;
                }
                else if (dataGridView_ws.Columns[e.ColumnIndex].Name == "ws_cz")
                {
                    DialogResult dialogresult = MessageBox.Show("确定要删除该数据吗?", "提示", MessageBoxButtons.OKCancel);
                    if (dialogresult == DialogResult.OK)
                    {
                        dt_ws.Rows.RemoveAt(rowindex);
                        dt_ws.AcceptChanges();

                        //外伤时间
                        dateTime_wssj.Checked = false;
                        //外伤名称
                        textBox_wsmc.Text = "";

                        //状态
                        isupdate_ws = false;
                        dataGridView_ws.DataSource = dt_ws;
                    }

                }
            }
        }

        #endregion 

        #region 输血

        //是否是编辑处理
        public bool isupdate_sx = false;
        public int rowIndex_sx = 0;

        /// <summary>
        /// 输血 无
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_sx_1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_sx_1.Checked == false)
            {
                return;
            }
            //输血名称
            textBox_sxyy.Enabled = false;
            textBox_sxyy.Text = "";

            //输血时间
            dateTime_sxsj.Checked = false;
            dateTime_sxsj.Enabled = false;

            //确定
            button_输血.Enabled = false;

            //状态
            isupdate_sx = false;
            rowIndex_sx = 0;

            if (dt_sx != null)
            {
                DataTable dt_sx_tem = dt_sx.Clone();
                dt_sx = dt_sx_tem;
                dt_sx.AcceptChanges();
                dataGridView_sx.DataSource = dt_sx;
            }
        }
        /// <summary>
        /// 输血 有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_sx_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_sx_2.Checked == false)
            {
                return;
            }
            //输血名称
            textBox_sxyy.Enabled = true;

            //输血时间
            dateTime_sxsj.Checked = false;
            dateTime_sxsj.Enabled = true;

            //确定
            button_输血.Enabled = true;

            //状态
            isupdate_sx = false;
            rowIndex_sx = 0;
        }

        /// <summary>
        /// 输血确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_输血_Click(object sender, EventArgs e)
        {
            if (textBox_sxyy.Text.Length == 0)
            {
                MessageBox.Show("请录入输血信息！");
                return;
            }

            if (dataGridView_sx.DataSource == null)
            {
                dt_sx = createtable("jwbs_sx");
            }
            else
            {
                dt_sx = (DataTable)dataGridView_sx.DataSource;
            }

            //增加新行
            if (isupdate_sx == false)
            {
                //增加新行
                dt_sx.Rows.Add();

                //输血原因
                dt_sx.Rows[dt_sx.Rows.Count - 1]["D_JBMC"] = textBox_sxyy.Text;

                //输血时间
                if (dateTime_sxsj.Checked == true)
                {
                    dt_sx.Rows[dt_sx.Rows.Count - 1]["D_ZDRQ"] = dateTime_sxsj.Value.ToString("yyyy-MM");
                }

                //疾病类型
                dt_sx.Rows[dt_sx.Rows.Count - 1]["D_JBLX"] = "输血";
            }
            else
            {
                //输血原因
                dt_sx.Rows[rowIndex_sx]["D_JBMC"] = textBox_sxyy.Text;

                //输血时间
                if (dateTime_sxsj.Checked == true)
                {
                    dt_sx.Rows[rowIndex_ws]["D_ZDRQ"] = dateTime_sxsj.Value.ToString("yyyy-MM");
                }
            }

            //输血时间
            dateTime_sxsj.Checked = false;
            //输血
            textBox_sxyy.Text = "";


            //状态
            isupdate_sx = false;
            rowIndex_sx = 0;

            dataGridView_sx.DataSource = dt_sx;

        }

        /// <summary>
        /// 输血行选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_sx_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowindex = e.RowIndex;
                dt_sx = (DataTable)dataGridView_sx.DataSource;
                //编辑
                if (dataGridView_sx.Columns[e.ColumnIndex].Name == "update_sx")
                {

                    //输血时间
                    if (dt_sx.Rows[rowindex]["D_ZDRQ"] != null && dt_sx.Rows[rowindex]["D_ZDRQ"].ToString().Length > 0)
                    {
                        dateTime_sxsj.Checked = true;
                        dateTime_sxsj.Value = Convert.ToDateTime(dt_sx.Rows[rowindex]["D_ZDRQ"].ToString() + "-01");
                    }

                    //输血
                    textBox_sxyy.Text = dt_sx.Rows[rowindex]["D_JBMC"].ToString();

                    //编辑
                    isupdate_sx = true;

                    //当前编辑的行号
                    rowIndex_sx = rowindex;
                }
                else if (dataGridView_sx.Columns[e.ColumnIndex].Name == "sx_cz")
                {
                    DialogResult dialogresult = MessageBox.Show("确定要删除该数据吗?", "提示", MessageBoxButtons.OKCancel);
                    if (dialogresult == DialogResult.OK)
                    {
                        dt_sx.Rows.RemoveAt(rowindex);
                        dt_sx.AcceptChanges();

                        //输血时间
                        dateTime_sxsj.Checked = false;
                        //输血名称
                        textBox_sxyy.Text = "";

                        //状态
                        isupdate_sx = false;
                        dataGridView_sx.DataSource = dt_sx;
                    }

                }
            }
        }
        #endregion

        #region 通用处理

        /// <summary>
        /// 获取checkbox的值
        /// </summary>
        /// <param name="checkName"></param>
        /// <param name="strat"></param>
        /// <param name="end"></param>
        /// <param name="valueList"></param>
        /// <param name="textList"></param>
        /// <returns></returns>
        public bool getChecklist(string checkName,int strat,int end ,out string valueList,out string textList)
        {
            valueList = "";
            textList = "";
            string valuestr = "";
            string textstr = "";

            for (int i = strat; i <= end; i++)
            {
                Control[] list = this.Controls.Find(checkName + i.ToString(), true);
                if (list != null && list.Length > 0)
                {
                    CheckBox CheckBox_tem = (CheckBox)list[0];

                    if (CheckBox_tem.Checked == true)
                    {
                        valuestr = valuestr + "," + CheckBox_tem.Tag.ToString();
                        textstr = textstr + "|" + CheckBox_tem.Text.ToString();
                    }
                }
            }
            valueList = valuestr.Length >1? valuestr.Substring(1):"";
            textList = textstr.Length >1? textstr.Substring(1):"";
            return true;
        }

        /// <summary>
        /// 设定checkbox的值
        /// </summary>
        /// <param name="checkName"></param>
        /// <param name="strat"></param>
        /// <param name="end"></param>
        /// <param name="valueList"></param>
        /// <param name="textList"></param>
        /// <returns></returns>
        public bool setChecklist(string checkName, int strat, int end,string valueList)
        {
            string[] value = valueList.Split(new char[] { ',' });
            //清空
             for (int i = strat; i <= end; i++)
            {
                Control[] list = this.Controls.Find(checkName + i.ToString(), true);
                if (list != null && list.Length > 0)
                {
                    CheckBox CheckBox_tem = (CheckBox)list[0];
                    CheckBox_tem.Checked =false ;
                }
            }

            //设定选项
             if (value.Length > 0)
             {
                 for (int j = 0; j < value.Length; j++)
                 {
                     for (int i = strat; i <= end; i++)
                     {
                         Control[] list = this.Controls.Find(checkName + i.ToString(), true);
                         if (list != null && list.Length > 0)
                         {
                             CheckBox CheckBox_tem = (CheckBox)list[0];
                             if (value[j].ToString().Equals(CheckBox_tem.Tag.ToString()))
                             {
                                 CheckBox_tem.Checked = true;
                                 break;
                             }
                         }
                     }
                 }
             }
            return true;
        }


        #endregion

    #region 家族史

        //是否是编辑处理
        public bool isupdate_jzs = false;
        public int rowIndex_jzs = 0;

        /// <summary>
        /// 家族史 无
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_jzs_1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_jzs_1.Checked == false)
            {
                return;
            }

            //名称
            checkBox_jzs_1.Enabled = false;
            checkBox_jzs_1.Checked = false;
            checkBox_jzs_2.Enabled = false;
            checkBox_jzs_2.Checked = false;
            checkBox_jzs_3.Enabled = false;
            checkBox_jzs_3.Checked = false;
            checkBox_jzs_4.Enabled = false;
            checkBox_jzs_4.Checked = false;
            checkBox_jzs_5.Enabled = false;
            checkBox_jzs_5.Checked = false;
            checkBox_jzs_6.Enabled = false;
            checkBox_jzs_6.Checked = false;
            checkBox_jzs_7.Enabled = false;
            checkBox_jzs_7.Checked = false;
            checkBox_jzs_8.Enabled = false;
            checkBox_jzs_8.Checked = false;
            checkBox_jzs_9.Enabled = false;
            checkBox_jzs_9.Checked = false;
            checkBox_jzs_10.Enabled = false;
            checkBox_jzs_10.Checked = false;
            checkBox_jzs_11.Enabled = false;
            checkBox_jzs_11.Checked = false;

            textBox_jzs_qt.Text = "";
            textBox_jzs_qt.Enabled = false;
            comboBox_gx.Enabled = false;
            button_家族史.Enabled = false;

            //状态
            isupdate_jzs = false;
            rowIndex_jzs = 0;

            if (dt_jzs != null)
            {
                DataTable dt_jzbs_tem = dt_jzs.Clone();
                dt_jzs = dt_jzbs_tem;
                dt_jzs.AcceptChanges();
                dataGridView_sx.DataSource = dt_jzs;
            }
        }

        /// <summary>
        /// 家族史  有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_jzs_2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_jzs_2.Checked == false)
            {
                return;
            }

            //名称
            checkBox_jzs_1.Enabled = true;
            checkBox_jzs_1.Checked = false;
            checkBox_jzs_2.Enabled = true;
            checkBox_jzs_2.Checked = false;
            checkBox_jzs_3.Enabled = true;
            checkBox_jzs_3.Checked = false;
            checkBox_jzs_4.Enabled = true;
            checkBox_jzs_4.Checked = false;
            checkBox_jzs_5.Enabled = true;
            checkBox_jzs_5.Checked = false;
            checkBox_jzs_6.Enabled = true;
            checkBox_jzs_6.Checked = false;
            checkBox_jzs_7.Enabled = true;
            checkBox_jzs_7.Checked = false;
            checkBox_jzs_8.Enabled = true;
            checkBox_jzs_8.Checked = false;
            checkBox_jzs_9.Enabled = true;
            checkBox_jzs_9.Checked = false;
            checkBox_jzs_10.Enabled = true;
            checkBox_jzs_10.Checked = false;
            checkBox_jzs_11.Enabled = true;
            checkBox_jzs_11.Checked = false;

            textBox_jzs_qt.Text = "";
            textBox_jzs_qt.Enabled = true;
            comboBox_gx.Enabled = true;
            button_家族史.Enabled = true;

            //状态
            isupdate_jzs = false;
            rowIndex_jzs = 0;
        }

        /// <summary>
        /// 家族史确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_家族史_Click(object sender, EventArgs e)
        {
            string valueList = "";
            string textList = "";
            getChecklist("checkBox_jzs_", 1, 11, out valueList, out textList);

            if (valueList.Length == 0)
            {
                MessageBox.Show("请选择家族病史种类！");
                return;
            }

            if (dataGridView_jzs.DataSource == null)
            {
                dt_jzs = createtable_jzbs("jwbs_jzs");
            }
            else
            {
                dt_jzs = (DataTable)dataGridView_jzs.DataSource;
            }

            //增加新行
            if (isupdate_jzs == false)
            {
                //增加新行
                dt_jzs.Rows.Add();

                //疾病名称
                dt_jzs.Rows[dt_jzs.Rows.Count - 1]["D_JZBS"] =valueList;
                //疾病编码
                dt_jzs.Rows[dt_jzs.Rows.Count - 1]["D_JBBM"] = textList;
          
                //家族病史其他
                dt_jzs.Rows[dt_jzs.Rows.Count - 1]["D_QTJB"] = textBox_jzs_qt.Text;

                //关系
                dt_jzs.Rows[dt_jzs.Rows.Count - 1]["D_JZCY"] = comboBox_gx.SelectedValue;
            }
            else
            {
                //疾病名称
                dt_jzs.Rows[rowIndex_jzs]["D_JZBS"] = valueList;
                //疾病编码
                dt_jzs.Rows[rowIndex_jzs]["D_JBBM"] =textList ;

                //家族病史其他
                dt_jzs.Rows[rowIndex_jzs]["D_QTJB"] = textBox_jzs_qt.Text;

                //关系
                dt_jzs.Rows[rowIndex_jzs]["D_JZCY"] = comboBox_gx.SelectedValue;
            }

            setChecklist("checkBox_jzs_", 1, 11, "");

            //家族病史其他
            textBox_jzs_qt.Text = "";
            //关系
            comboBox_gx.SelectedIndex = 0 ;
            

            //状态
            isupdate_jzs = false;
            rowIndex_jzs = 0;

            dataGridView_jzs.DataSource = dt_jzs;
        }

        /// <summary>
        /// 家族史  行选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_jzs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowindex = e.RowIndex;

                DataTable dt_jzs = dataGridView_jzs.DataSource as DataTable;

                //编辑
                if (dataGridView_jzs.Columns[e.ColumnIndex].Name == "jzs_update")
                {
                    //疾病名称
                    setChecklist("checkBox_jzs_", 1, 11, dt_jzs.Rows[rowindex]["D_JZBS"].ToString());
                    //家族成员
                    if (dt_jzs.Rows[rowindex]["D_JZCY"] != null && dt_jzs.Rows[rowindex]["D_JZCY"].ToString().Length > 0)
                    {
                        comboBox_gx.SelectedValue = dt_jzs.Rows[rowindex]["D_JZCY"].ToString();
                    }

                    //家族病史其他
                    textBox_jzs_qt.Text = dt_jzs.Rows[rowindex]["D_QTJB"].ToString();

                    //编辑
                    isupdate_jzs = true;

                    //当前编辑的行号
                    rowIndex_jzs = rowindex;
                }
                else if (dataGridView_jzs.Columns[e.ColumnIndex].Name == "jzs_cz")
                {
                    DialogResult dialogresult = MessageBox.Show("确定要删除该数据吗?", "提示", MessageBoxButtons.OKCancel);
                    if (dialogresult == DialogResult.OK)
                    {
                        dt_jzs.Rows.RemoveAt(rowindex);
                        dt_jzs.AcceptChanges();

                        setChecklist("checkBox_jzs_", 1, 11, "");
                        textBox_jzs_qt.Text = "";
                        comboBox_gx.SelectedIndex  =0;
                       

                        //状态
                        isupdate_jzs = false;
                        dataGridView_jzs.DataSource = dt_jzs;
                    }

                }
            }



        }

        /// <summary>
        /// 生成家族病史的表结构
        /// </summary>
        /// <param name="datatableName"></param>
        /// <returns></returns>
        public DataTable createtable_jzbs(string datatableName)
        {
            DataTable dt_tem = new DataTable();
            dt_tem.TableName = datatableName;
            //个人档案编号
            dt_tem.Columns.Add("D_GRDABH");
            //家族成员
            dt_tem.Columns.Add("D_JZCY");
            //家族病史
            dt_tem.Columns.Add("D_JZBS");
            //疾病编码
            dt_tem.Columns.Add("D_JBBM");
            //其他疾病
            dt_tem.Columns.Add("D_QTJB");
            //其他疾病编码
            dt_tem.Columns.Add("D_QTJBBM");
            //区域编号
            dt_tem.Columns.Add("P_RGID");
            //创建时间
            dt_tem.Columns.Add("CREATETIME");
            //修改时间
            dt_tem.Columns.Add("UPDATETIME");
            //修改创建时间
            dt_tem.Columns.Add("HAPPENTIME");
            //创建者姓名
            dt_tem.Columns.Add("CREATEUSER");
            //修改者姓名
            dt_tem.Columns.Add("UPDATEUSER");
            //创建地区
            dt_tem.Columns.Add("CREATREGION");
            dt_tem.Columns.Add("D_JKBS");

            return dt_tem;
        }
    #endregion

        /// <summary>
        /// 身份证回车查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lTextBox_D_SFZH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                check_daBysfzh();
            }
        }


    }
}
