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
    public partial class Form_daxq : sysCommonForm
    {
        /// <summary>
        /// 存放原始的结果集
        /// </summary>
        private static  DataTable dt_tem = new DataTable();

        private bool drpflag = false;

        public string czList = "";

        /// <summary>
        /// 健康档案人口学资料（T_DA_JKDA_RKXZL）
        /// </summary>
        public DataTable dt_T_DA_JKDA_RKXZL = null;
        /// <summary>
        /// 家庭档案表（T_DA_JTDA）
        /// </summary>
        public DataTable dt_T_DA_JTDA = null;
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

        public Form_daxq()
        {
            InitializeComponent();
        }

        #region 控件事件
        private void Form_downLoad_Load(object sender, EventArgs e)
        {

            form_readsfzh.Show();
            form_readsfzh.Owner = this;
            form_readsfzh.Visible = false;

            ////隐藏既往史标签
            //tab_jws.Parent = null;
            ////隐藏其他标签
            //tab_qt.Parent = null;
            initPage();
            //初始化疾病
            init_jb();

            //显示档案信息
            if (str_jkdah .Length>0 || str_sfzh .Length >0 || str_xm .Length >0 )
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
                //医疗费用支付方式
                //setDrp(comboBox_D_SHENG, "", true);
                setListData(checkedListBox_D_YLFZFLX, "ylfzflx");
                //过敏
                setListData(checkedListBox_D_YGMS, "gms");

                //所属机构
                setDrp(comboBox_ssjg, "", "sql085", true);


                //录入时间
                TextBox_lrsj.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //录入人
               // TextBox_lrr.Text = UserInfo .Username;

                //修改时间
               // TextBox_xgsj.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //修改人
               // TextBox_xgr.Text = UserInfo.Username;

                //初始化时隐藏户主信息
                labts.Visible = false;
                labhzsfzh.Visible = false;
                hzxm.Visible = false;

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
        public DataTable  initTable( string sqlcode)
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
                return ;
            }

            if ( comboBox_D_YHZGX.SelectedValue.ToString() != "1")
            {
                if (hzxm.Text.Trim() == "")
                {
                    MessageBox.Show("请填写户主身份证号！");
                    return;
                }
            }
            

            //if (comboBox_ssjg.SelectedValue == null || comboBox_ssjg.SelectedValue.ToString().Length == 0)
            //{
            //    MessageBox.Show("[所属机构] 必须选择！");
            //    return;
            //}

            //if (comboBox_lrr.SelectedValue == null || comboBox_lrr.SelectedValue.ToString().Length == 0)
            //{
            //    MessageBox.Show("[录入人] 必须选择！");
            //    return;
            //}
            string message = "";
            if (checksfz_nl(out message) == false)
            {
                MessageBox.Show(message);
            }

            #region 人口学资料
            //初始化表结构
            dt_T_DA_JKDA_RKXZL=  initTable("sql056");
            dt_T_DA_JKDA_RKXZL.Rows.Add();
          

            //健康档案号  健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_GRDABH"] = lTextBox_D_GRDABH.Text.PadLeft(12,'0');
            
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

            //姓名 健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_XM"] = lTextBox_D_XM.Text;

            //性别 健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_XB.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_XB"] = comboBox_D_XB.SelectedValue.ToString();
            }

            //证件编号 身份证号 健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_zjhqt"] = lTextBox_D_SFZH.Text;

            //出生日期    健康档案人口学资料（T_DA_JKDA_RKXZL）
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_CSRQ"] = dateTimePicker_D_CSRQ.Value .ToString ("yyyy-MM-dd");

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
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_YLFZFLX"] = getSelectList(checkedListBox_D_YLFZFLX, "zdbm");

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
            //所属片区
            // comboBox_D_SSPQ.SelectedIndex = 0;
            //档案类别   健康档案人口学资料（T_DA_JKDA_RKXZL）
            if (comboBox_D_DALB.SelectedValue != null)
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_DALB"] = comboBox_D_DALB.SelectedValue.ToString();
            }

            //增加户主身份证号
            if (comboBox_D_YHZGX.SelectedValue.ToString() != "1")
            {
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_HZSFZH"] = hzxm.Text.Trim();
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_SFZH"] = hzxm.Text.Trim();
            }
            else
            { 
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_HZSFZH"] = lTextBox_D_SFZH.Text.Trim();
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_SFZH"] = hzxm.Text.Trim();
            }

            //证件类型
            dt_T_DA_JKDA_RKXZL.Rows[0]["D_ZJLX"] = cmb_zjlx2.SelectedValue.ToString();

            //创建地区
            dt_T_DA_JKDA_RKXZL.Rows[0]["CREATREGION"] = comboBox_ssjg.SelectedValue !=null?comboBox_ssjg.SelectedValue.ToString ():"";

            //区域编号
            dt_T_DA_JKDA_RKXZL.Rows[0]["P_RGID"] = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";

            //创建者姓名
            dt_T_DA_JKDA_RKXZL.Rows[0]["CREATEUSER"] =  comboBox_lrr.SelectedValue !=null?comboBox_lrr.SelectedValue.ToString ():"";

            //修改者姓名
            dt_T_DA_JKDA_RKXZL.Rows[0]["UPDATEUSER"] = comboBox_lrr.SelectedValue != null ? comboBox_lrr.SelectedValue.ToString() : "";

            //创建时间
            dt_T_DA_JKDA_RKXZL.Rows[0]["CREATETIME"] = TextBox_lrsj.Text;

            //修改时间
            dt_T_DA_JKDA_RKXZL.Rows[0]["UPDATETIME"] = TextBox_lrsj.Text;

            //年龄
           string nl = (DateTime.Now.Year - dateTimePicker_D_CSRQ.Value.Year ).ToString();
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

            #region 体检人员信息

            //初始化表结构
            DataTable  dt_T_jk_ryxx = new DataTable();
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
            dt_T_jk_ryxx.Rows[0]["XB"] = comboBox_D_XB.SelectedValue!=null? comboBox_D_XB.SelectedValue.ToString():"";
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
            dt_T_jk_ryxx.Rows[0]["UPDATETIME"] = DateTime.Now .ToString ("yyyy-MM-dd");
            //UPDATEUSER	更新者
            dt_T_jk_ryxx.Columns.Add("UPDATEUSER");
            dt_T_jk_ryxx.Rows[0]["UPDATEUSER"] = UserInfo .userId ;
            //nd	年度
            dt_T_jk_ryxx.Columns.Add("nd");
            dt_T_jk_ryxx.Rows[0]["nd"] = DateTime.Now.Year .ToString();

            //nd	分类
            dt_T_jk_ryxx.Columns.Add("fl");
            dt_T_jk_ryxx.Rows[0]["fl"] = "2";

            //nd	增量标志
            dt_T_jk_ryxx.Columns.Add("zlbz");
            dt_T_jk_ryxx.Rows[0]["zlbz"] ="1";

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
                comboBox_D_YHZGX.SelectedIndex=0;
                hzxm.Text = "";
                //档案状态
                radioButton_D_DAZT_1.Checked = true;
                radioButton_D_DAZT_2.Checked = false;
                //姓名
                lTextBox_D_XM.Text = "";
                //性别
                comboBox_D_XB.SelectedIndex=-1;
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
                comboBox_czlx.SelectedIndex=-1;
                //民族
                comboBox_D_MZ.SelectedIndex=-1;
                //血型
                comboBox_D_XX.SelectedIndex=-1;
                //RH
                comboBox_D_SFRHYX.SelectedIndex=-1;
                //职业
                comboBox_D_ZY.SelectedIndex=-1;
                //文化程度
                comboBox_D_WHCD.SelectedIndex=-1;
                //劳动强度
                comboBox_D_LDQD.SelectedIndex=-1;

                //婚姻状况
                comboBox_D_HYZK.SelectedIndex=-1;
                //医疗费用支付方式
                checkedListBox_D_YLFZFLX.ClearSelected();
                //医疗费用支付方式
                lTextBox_D_YLFZFLXQT.Text = "";

                //医疗保险号
                lTextBox_D_YLBXH.Text = "";
                //新农合号
                lTextBox_D_XNHH.Text = "";
                //省
                drpflag = false;
                comboBox_D_SHENG.SelectedIndex=-1;
                //市
                comboBox_D_SHI.SelectedIndex=-1;
                //县
                comboBox_D_QU.SelectedIndex=-1;
                //镇
                comboBox_D_JD.SelectedIndex=-1;
                //村
                comboBox_D_JWH.SelectedIndex=-1;
                drpflag = true;
                //详细地址
                lTextBox_D_XXDZ.Text = "";
                //所属片区
                // comboBox_D_SSPQ.SelectedIndex = 0;
                //档案类别
                comboBox_D_DALB.SelectedIndex=-1;
                //药物过敏史
                comboBox_D_GMS.SelectedIndex=-1;
                //药物过敏史
                checkedListBox_D_YGMS.ClearSelected();
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
                if (dt.Rows[0]["D_YHZGX"]!=null && dt.Rows[0]["D_YHZGX"].ToString().Equals("1"))
                {
                    radioButton_D_DAZT_1.Checked = true;
                }
                else
                {
                    radioButton_D_DAZT_2.Checked = true;
                }

                //姓名
                lTextBox_D_XM.Text = dt.Rows[0]["D_XM"]!=null? dt.Rows[0]["D_XM"].ToString():""; 
                //性别
                comboBox_D_XB.SelectedValue = dt.Rows[0]["D_XB"] != null ? dt.Rows[0]["D_XB"].ToString() : ""; 
                //证件编号
                lTextBox_D_SFZH.Text = dt.Rows[0]["D_zjhqt"] != null ? dt.Rows[0]["D_zjhqt"].ToString() : "";
                //
                comboBox_D_YHZGX.SelectedValue = dt.Rows[0]["D_YHZGX"] != null ? dt.Rows[0]["D_YHZGX"].ToString() : "";
                //出生日期
                dateTimePicker_D_CSRQ.Value = dt.Rows[0]["D_csrq"] != null && dt.Rows[0]["D_csrq"] .ToString ().Length >0? Convert.ToDateTime(dt.Rows[0]["D_csrq"].ToString()) : DateTime.Now; 
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
                setItemSelect(checkedListBox_D_YLFZFLX, dt.Rows[0]["D_YLFZFLX"] != null ? dt.Rows[0]["D_YLFZFLX"].ToString() : "", "ZDBM");

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
                //所属片区
                // comboBox_D_SSPQ.SelectedIndex = 0;
                //档案类别
                comboBox_D_DALB.SelectedValue = dt.Rows[0]["D_DALB"] != null ? dt.Rows[0]["D_DALB"].ToString() : ""; 
               
                hzxm.Text = dt.Rows[0]["D_DALB"].ToString();
                hzxm.Text = dt.Rows[0]["D_HZSFZH"].ToString();

                //所属单位
                comboBox_ssjg.SelectedValue = dt.Rows[0]["P_RGID"].ToString();
                //录入人
                comboBox_lrr.SelectedValue = dt.Rows[0]["CREATEUSER"].ToString();

                //编号	id
                //个人档案编号	D_GRDABH
                //家庭档案编号	D_JTDABH
                //姓名	D_XM
                //身份证号	D_SFZH
                //与户主关系	D_YHZGX
                //工作单位	D_GZDW
                //联系电话	D_LXDH
                //E_MAIL	D_EMAIL
                //省	D_SHENG
                //市	D_SHI
                //区（县）	D_QU
                //街道（乡）	D_JD
                //居委会	D_JWH
                //详细地址	D_XXDZ
                //所属片区	D_SSPQ
                //居住状况	D_JZZK
                //性别	D_XB
                //出生日期	D_CSRQ
                //民族	D_MZ
                //文化程度	D_WHCD
                //职业	D_ZY
                //婚姻状况	D_HYZK
                //医疗费支付类型	D_YLFZFLX
                //医疗保险号	D_YLBXH
                //新农合号	D_XNHH
                //拼音简码	D_PYJM
                //区域编号	P_RGID
                //创建时间	CREATETIME
                //修改时间	UPDATETIME
                //修改创建时间	HAPPENTIME
                //创建者姓名	CREATEUSER
                //修改者姓名	UPDATEUSER
                //创建地区	CREATREGION
                //联系人姓名	D_LXRXM
                //联系人电话	D_LXRDH
                //医疗支付类型其他	D_YLFZFLXQT
                //怀孕情况	L_HYQK
                //孕次	L_YC
                //未知	L_YCSC
                //产次	L_CC
                //未知	L_CCSC
                //缺项值	QDQXZ
                //厨房排气设备	cfpqsb
                //新家庭档案编号	D_JTDABH_NEW 
                //个人档案编号	D_GRDABH_NEW 
                //档案类别	D_DALB 
                //档案状态	D_DAZT 

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
            //编号id
            //个人档案编号D_GRDABH
            //家族成员D_JZCY
            //家族病史D_JZBS
            //疾病编码D_JBBM
            //其他疾病D_QTJB
            //其他疾病编码D_QTJBBM
            //区域编号P_RGID
            //创建时间CREATETIME
            //修改时间UPDATETIME
            //修改创建时间HAPPENTIME
            //创建者姓名CREATEUSER
            //修改者姓名UPDATEUSER
            //创建地区CREATREGION
            #endregion

            #region 健康档案健康状况既往病史表（T_DA_JKDA_JKZK_JWBS）
            //编号id
            //个人档案编号D_GRDABH
            //疾病名称D_JBMC
            //疾病编码D_JBBM
            //诊断日期D_ZDRQ
            //诊断单位D_ZDDW
            //处理情况D_CLQK
            //转归情况D_ZGQK
            //区域编号P_RGID
            //创建时间CREATETIME
            //修改时间UPDATETIME
            //修改创建时间HAPPENTIME
            //创建者姓名CREATEUSER
            //修改者姓名UPDATEUSER
            //创建地区CREATREGION
            //疾病类型D_JBLX
            //恶性肿瘤EXZL
            //疾病其他JBQT
            //职业病其他zybqt
            //其他D_JBMCQT
            #endregion

            #region 健康档案健康状况表（T_DA_JKDA_JKZK）
            //编号id
            //个人档案编号D_GRDABH
            //血型D_XX
            //comboBox_D_XX.SelectedValue = dt.Rows[0]["D_XX"] != null ? dt.Rows[0]["D_XX"].ToString() : ""; 
            //是否为RH阴性D_SFRHYX
            //comboBox_D_SFRHYX.SelectedValue = dt.Rows[0]["D_SFRHYX"] != null ? dt.Rows[0]["D_SFRHYX"].ToString() : ""; 
            
            //有过敏史D_YGMS
            //comboBox_D_GMS.SelectedValue = dt.Rows[0]["D_GMS"] != null ? dt.Rows[0]["D_GMS"].ToString() : ""; 
            //是否过敏史D_GMS
            //checkedListBox_D_YGMS.ClearSelected();
            //过敏史其他D_GMSQT
            // lTextBox_D_GMSQT.Text = dt.Rows[0]["D_GMSQT"] != null ? dt.Rows[0]["D_GMSQT"].ToString() : ""; 
            //暴露史D_BLS
            //有暴露史D_YBLS
            //暴露史化学品D_BLSHXP
            //暴露史毒物D_BLSDW
            //暴露史射线D_BLSSX
            //遗传病史D_YCBS
            //遗传史疾病D_YCBSJB
            //有无残疾D_YWCJ
            //有无疾病D_YWJB
            //有无既往史D_YWJWS
            //残疾名字D_CJMZ
            //区域编号P_RGID
            //创建时间CREATETIME
            //修改时间UPDATETIME
            //修改创建时间HAPPENTIME
            //创建者姓名CREATEUSER
            //修改者姓名UPDATEUSER
            //创建地区CREATREGION
            //其他D_CJQT
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

                string message = "";
                if (checksfz_nl(out message) == false)
                {
                    DialogResult result;
                    result = MessageBox.Show(message+ " 是否仍要加入体检？ \r\n是：加入体检 \r\n否：不加入体检 \r\n取消：取消本次操作", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
                        string resultStr = form_peopleadd.Add_jktj(lTextBox_D_GRDABH.Text, lTextBox_D_SFZH.Text, lTextBox_D_XM.Text, comboBox_D_XB.Text, dateTimePicker_D_CSRQ.Value .ToString ("yyyy-MM-dd"),nl,"","","", ref errMsg);
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
            ////市
            //if (comboBox_D_SHENG.SelectedValue != null && comboBox_D_SHENG.SelectedValue.ToString().Length >0)
            //{
            //    setDrp(comboBox_D_SHI, string.Format(" and B_RGID like '%{0}%'", comboBox_D_SHENG.SelectedValue.ToString()), "sql052", true);
            //}
            //else
            //{
            //    comboBox_D_SHI.Items.Clear();
            //}
            ////县
            //comboBox_D_QU.DataSource = null;
            ////镇
            //comboBox_D_JD.DataSource = null;
            ////村
            //comboBox_D_JWH.DataSource = null;
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
            string str_d_grdabh="";

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
            else {
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
        public void setItemSelect(CheckedListBox chkBoxList, string selectvalue,string itemName)
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

        #region 样式
        
        /// <summary>
        /// tab的样式设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlForm_DrawItem(object sender, DrawItemEventArgs e)
        {
            //获取TabControl主控件的工作区域
            Rectangle rec = tabControl1.ClientRectangle;
            //获取背景图片，我的背景图片在项目资源文件中。
            //Image backImage = FBYClient.Properties.Resources.tjtab_bg;
            //新建一个StringFormat对象，用于对标签文字的布局设置
            StringFormat StrFormat = new StringFormat();
            StrFormat.LineAlignment = StringAlignment.Center;// 设置文字垂直方向居中
            StrFormat.Alignment = StringAlignment.Center;// 设置文字水平方向居中          
            // 标签背景填充颜色，也可以是图片
            SolidBrush bru = new SolidBrush(Color.FromArgb(215, 239, 236));
            SolidBrush bru_noSelect = new SolidBrush(Color.FromArgb(226, 226, 226));
            //SolidBrush bruFont = new SolidBrush(Color.FromArgb(111, 111, 111));// 标签字体颜色
            SolidBrush bruFont = new SolidBrush(Color.Black);// 标签字体颜色
            Font font = new System.Drawing.Font("微软雅黑", 11F);//设置标签字体样式

            //绘制主控件的背景
            // tabControl.SelectedTab.Name
            //e.Graphics.DrawImage(backImage, 0, 0, tabControlForm.Width, tabControlForm.Height);

            //绘制标签样式
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                //获取标签头的工作区域
                Rectangle recChild = tabControl1.GetTabRect(i);
                if (tabControl1.SelectedIndex == i)
                {
                    //绘制标签头背景颜色
                    e.Graphics.FillRectangle(bru, recChild);
                }
                else
                {
                    //绘制标签头背景颜色
                    e.Graphics.FillRectangle(bru_noSelect, recChild);
                }
                //绘制标签头的文字
                e.Graphics.DrawString(tabControl1.TabPages[i].Text, font, bruFont, recChild, StrFormat);
            }
        }

        /// <summary>
        /// 绘制边线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {

            GroupBox groupBox1 = (GroupBox)sender;
            if (groupbox_title == 0)
            {

                Font font = new System.Drawing.Font("微软雅黑", 11F, FontStyle.Bold);
                // 117,117,117
                //Brush Brush_tem = new Brush();
                e.Graphics.DrawString(groupBox1.Tag.ToString(), font, Brushes.Black, 10, -1);
            }
            Pen pen = new Pen(Color.FromArgb(210, 220, 255));
            e.Graphics.DrawLine(pen, 1, 7, 8, 7);

            e.Graphics.DrawLine(pen, e.Graphics.MeasureString(groupBox1.Text, groupBox1.Font).Width + 8, 7, groupBox1.Width - 2, 7);


            e.Graphics.DrawLine(pen, 1, 7, 1, groupBox1.Height - 2);


            e.Graphics.DrawLine(pen, 1, groupBox1.Height - 2, groupBox1.Width - 2, groupBox1.Height - 2);


            e.Graphics.DrawLine(pen, groupBox1.Width - 2, 7, groupBox1.Width - 2, groupBox1.Height - 2);

        }
        #endregion

        #region 疾病
        
        //项目列表
        DataTable dtList_jb = new DataTable();
        static int RowCount_jb = 0;

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            RowCount_jb = RowCount_jb + 1;
            this.tableLayoutPanel3.RowCount = RowCount_jb;
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle());

            this.tableLayoutPanel3.Controls.Add(addJb(RowCount_jb, dtList_jb), 0, RowCount_jb - 1);

            Button button_add = new Button();
            button_add.Click += buttonDel_Click;
            button_add.Text = "删除";
            button_add.Tag = (RowCount_jb - 1).ToString();
            this.tableLayoutPanel3.Controls.Add(button_add, 1, RowCount_jb - 1);
        }
        /// <summary>
        /// 生成初始行
        /// </summary>
        /// <returns></returns>
        public bool init_jb()
        {

            dtList_jb = Common.getsjzd("jb_gren", "sql_select_sjzd");


            int index = this.tableLayoutPanel3.RowCount++;
            RowCount_jb = RowCount_jb + 1;
            RowStyle style = new RowStyle(SizeType.AutoSize);
            tableLayoutPanel3.RowStyles.Add(style);
            //return index;

            //int rowIndex = AddTableRow();
            tableLayoutPanel3.Controls.Add(addJb(RowCount_jb, dtList_jb), 0, RowCount_jb - 1);
            Button button_add = new Button();
            button_add.Click += buttonAdd_Click;
            button_add.Text = "添加";
            tableLayoutPanel3.Controls.Add(button_add, 1, RowCount_jb - 1);


            return true;
        }


        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDel_Click(object sender, EventArgs e)
        {
            int iRowId = int.Parse(((Button)sender).Tag.ToString());

            this.tableLayoutPanel3.SuspendLayout();

            Control c = null;

            //delete current row controls
            for (int j = 0; j < this.tableLayoutPanel3.ColumnCount; j++)
            {
                c = this.tableLayoutPanel3.GetControlFromPosition(j, iRowId);
                if (c != null)
                {
                    this.tableLayoutPanel3.Controls.Remove(c);
                }
            }

            //need to shift all controls up one row from delete point
            TableLayoutPanelCellPosition controlPosition;
            for (int i = iRowId; i < this.tableLayoutPanel3.RowCount; i++)
            {
                for (int j = 0; j < this.tableLayoutPanel3.ColumnCount; j++)
                {
                    c = this.tableLayoutPanel3.GetControlFromPosition(j, i + 1);
                    if (c == null)
                        break;
                    c.Tag = i;
                    controlPosition = new TableLayoutPanelCellPosition(j, i);
                    this.tableLayoutPanel3.SetCellPosition(c, controlPosition);
                }
            }

            //need to remove the row
            this.tableLayoutPanel3.RowCount--;

            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();


        }
        /// <summary>
        /// 添加疾病
        /// </summary>
        /// <param name="rowNo"></param>
        /// <param name="dtList"></param>
        /// <returns></returns>
        public Panel addJb(int rowNo, DataTable dtList)
        {
            Panel panelDemo = new Panel();
            panelDemo.Name = "jb" + rowNo.ToString();
            panelDemo.Location = new Point(0, 0);
            //panelDemo.Dock = DockStyle.Fill;
            panelDemo.AutoSize = true;
            CheckBox check_tem = new CheckBox();
            check_tem.Width = 0;
            int x = 5, y = 0;
            for (int i = 0; i < dtList.Rows.Count; i++)
            {
                x = x + check_tem.Width + 5;
                if (i == 5)
                {
                    LTextBox ltextbox = new LTextBox();
                    ltextbox.Name = "text_" + rowNo.ToString() + "_" + "EXZL";
                    ltextbox.Location = new Point(x, y);
                    ltextbox.LineType = LTextBox.BorderType.Bottom;
                    ltextbox.LineColor = Color.Black;
                    ltextbox.Width = 150;
                    panelDemo.Controls.Add(ltextbox);

                    // 增加 录入框  换行
                    x = 5;
                    y = y + 30;
                }
                else if (i == 11)
                {
                    LTextBox ltextbox = new LTextBox();
                    ltextbox.Name = "text_" + rowNo.ToString() + "_" + "zybqt";
                    ltextbox.Location = new Point(x, y);
                    ltextbox.LineColor = Color.Black;
                    ltextbox.LineType = LTextBox.BorderType.Bottom;
                    ltextbox.Width = 150;
                    panelDemo.Controls.Add(ltextbox);

                    // 增加 录入框  换行
                    x = 5;
                    y = y + 30;
                }
                CheckBox check = new CheckBox();
                check.AutoSize = true;
                check.Name = "chk_" + rowNo.ToString() + "_" + dtList.Rows[i]["zdbm"].ToString();
                check.Text = dtList.Rows[i]["zdmc"].ToString();
                check.Tag = dtList.Rows[i]["zdbm"].ToString();
                //check.BackColor = Color.Red;
                check.Location = new Point(x, y);
                check_tem = check;

                panelDemo.Controls.Add(check);
            }
            x = x + check_tem.Width + 5;
            LTextBox ltextbox1 = new LTextBox();
            ltextbox1.Name = "text_" + rowNo.ToString() + "_" + "JBQT";
            ltextbox1.Location = new Point(x, y);
            ltextbox1.LineColor = Color.Black;
            ltextbox1.LineType = LTextBox.BorderType.Bottom;
            ltextbox1.Width = 150;
            panelDemo.Controls.Add(ltextbox1);


            //确诊时间
            x = 5;
            y = y + 30;
            Label label = new Label();
            label.Text = "确诊时间";
            label.Location = new Point(x, y);
            label.AutoSize = true;
            panelDemo.Controls.Add(label);

            x = x + label.Width + 5;
            DateTimePicker datetimepicker = new DateTimePicker();
            datetimepicker.Location = new Point(x, y);
            datetimepicker.Name = "dtp_" + rowNo.ToString() + "_" + "D_ZDRQ";
            datetimepicker.Width = 150;
            panelDemo.Controls.Add(datetimepicker);
            return panelDemo;
        }

        /// <summary>
        /// 遍历疾病
        /// </summary>
        /// <param name="dtList"></param>
        /// <returns></returns>
        public DataTable search_jb(DataTable dtList)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("D_GRDABH");
            dt.Columns.Add("D_JBMC");
            dt.Columns.Add("D_JBBM");
            dt.Columns.Add("D_ZDRQ");
            dt.Columns.Add("D_ZDDW");
            dt.Columns.Add("D_CLQK");
            dt.Columns.Add("EXZL");
            dt.Columns.Add("JBQT");
            dt.Columns.Add("zybqt");
            dt.Columns.Add("zt");
            dt.Columns.Add("zlbz");
            dt.Columns.Add("nd");

            for (int i = 1; i <= tableLayoutPanel3.RowCount; i++)
            {
                dt.Rows.Add();
                string strD_JBMC = "";//疾病名称
                string D_JBBM = "";//疾病编码
                string D_ZDRQ = "";//诊断日期
                string EXZL = "";//恶性肿瘤
                string JBQT = "";//疾病其他
                string zybqt = "";//职业病其他

                for (int j = 0; j < dtList.Rows.Count; j++)
                {
                    // Panel panelInCell = tableLayoutPanel1.GetControlFromPosition(j, i) as Panel;
                    string D_JBBM_tem = GetControlValueByName("chk_" + i.ToString() + "_" + dtList.Rows[j]["value"].ToString());
                    if (D_JBBM_tem.Length > 0)
                    {
                        D_JBBM = D_JBBM + D_JBBM_tem + ",";
                    }
                    string strD_JBMC_tem = GetControlTextByName("chk_" + i.ToString() + "_" + dtList.Rows[j]["value"].ToString());
                    if (strD_JBMC_tem.Length > 0)
                    {
                        strD_JBMC = strD_JBMC + strD_JBMC_tem + ",";
                    }
                }
                //恶性肿瘤
                EXZL = GetControlValueByName("text_" + i.ToString() + "_" + "EXZL");
                //疾病其他
                JBQT = GetControlValueByName("text_" + i.ToString() + "_" + "JBQT");
                //职业病其他
                zybqt = GetControlValueByName("text_" + i.ToString() + "_" + "zybqt");
                //诊断日期
                D_ZDRQ = GetControlValueByName("dtp_" + i.ToString() + "_" + "D_ZDRQ");

                dt.Rows[dt.Rows.Count - 1]["D_JBBM"] = D_JBBM;
                dt.Rows[dt.Rows.Count - 1]["D_JBMC"] = strD_JBMC;
                dt.Rows[dt.Rows.Count - 1]["EXZL"] = EXZL;
                dt.Rows[dt.Rows.Count - 1]["JBQT"] = JBQT;
                dt.Rows[dt.Rows.Count - 1]["zybqt"] = zybqt;
                dt.Rows[dt.Rows.Count - 1]["D_ZDRQ"] = D_ZDRQ;
                dt.Rows[dt.Rows.Count - 1]["zt"] = "2";
                dt.Rows[dt.Rows.Count - 1]["zlbz"] = "1";
                dt.Rows[dt.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString();
            }
            return dt;
        }

        /// <summary>
        /// 按照控件名称获取控件的值
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private string GetControlValueByName(string ControlName)
        {
            Control control = Controls.Find(ControlName, true)[0];
            string value = "";
            //text
            if (ControlName.IndexOf("text_") > -1)
            {
                TextBox TextBox_tem = (TextBox)control;
                value = TextBox_tem.Text;
            }
            //checkBox
            else if (ControlName.IndexOf("chk_") > -1)
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
            else if (ControlName.IndexOf("dtp_") > -1)
            {
                DateTimePicker checkBox_tem = (DateTimePicker)control;
                if (checkBox_tem.Checked == true)
                {
                    value = checkBox_tem.Value.ToString("yyyy-MM");
                }
                else
                {
                    value = "";
                }
            }
            return value;
        }

        /// <summary>
        /// 按照控件名称获取控件的名称
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private string GetControlTextByName(string ControlName)
        {
            Control control = Controls.Find(ControlName, true)[0];
            string text = "";
            //checkBox
            if (ControlName.IndexOf("chk_") > -1)
            {
                CheckBox checkBox_tem = (CheckBox)control;
                if (checkBox_tem.Checked == true)
                {
                    text = checkBox_tem.Text.ToString();
                }
                else
                {
                    text = "";
                }
            }
            return text;
        }
        /// <summary>
        /// 给控件赋值
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private void setValueToControl(string ControlId, string ControlValue)
        {
            try
            {
                Control control = Controls.Find(ControlId, true)[0];
                //text
                if (ControlId.IndexOf("text_") > -1)
                {
                    TextBox TextBox_tem = (TextBox)control;
                    TextBox_tem.Text = ControlValue;
                }
                //checkBox
                else if (ControlId.IndexOf("chk_") > -1)
                {
                    CheckBox checkBox_tem = (CheckBox)control;
                    if (ControlValue.Trim().Length > 0)
                    {
                        checkBox_tem.Checked = true;
                    }
                    else
                    {
                        checkBox_tem.Checked = false;
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        #endregion

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
            if (e.KeyData == Keys.Enter) {

                if (lTextBox_D_GRDABH.Text.Length > 0)
                {
                    lTextBox_D_GRDABH.Text = lTextBox_D_GRDABH.Text.PadLeft(12, '0');
                }
            }
        }
        /// <summary>
        /// 户主关系发生变化时业务处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_YHZGX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_D_YHZGX.SelectedValue == null) {
                return;
            }
            if (comboBox_D_YHZGX.SelectedValue.ToString() == "1")
            {
                //labts.Visible = false;
                //labhzsfzh.Visible = false;
                //hzxm.Visible = false;

                //text_hzsfzh.Text = lTextBox_D_SFZH.Text;



            }
            else {
                //labts.Visible = true;
                //labhzsfzh.Visible = true;
                //hzxm.Visible = true;
            }
        }

        private void hzxm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (hzxm.Text.Trim().Length > 0)
                {
                    //MousePosition.X
                    //Point textLocation = PointToScreen(mc.Location);
                    //textLocation.X = textLocation.X - 10;
                    //textLocation.Y = textLocation.Y -50;
                    Point lco = new Point();
                    lco.X = hzxm.Location.X + 10;
                    lco.Y = hzxm.Location.Y + 50;
                    DropDownGrid form = new DropDownGrid();
                    form.Location = lco;
                    form.Owner = this;

                    form.setInfo(hzxm.Text.Trim(), this);
                    form.ShowDialog();
                    if (hzxm.Text != "")
                        hzxm.Focus();
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
                hzxm.Text = dr["D_SFZH"].ToString();


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
           DataTable dtsfzh=form_readsfzh.readSfzh();
           if (dtsfzh != null && dtsfzh.Rows.Count > 0)
           {
               lTextBox_D_SFZH.Text = dtsfzh.Rows[0]["身份证号"].ToString();
               lTextBox_D_XM.Text = dtsfzh.Rows[0]["姓名"].ToString();
               comboBox_D_XB.SelectedValue = dtsfzh.Rows[0]["性别"].ToString();
               dateTimePicker_D_CSRQ.Value = Convert.ToDateTime(dtsfzh.Rows[0]["出生日期"].ToString());
               string message = "";
               if (checksfz_nl(out message) == false)
               {
                   MessageBox.Show(message);
               }
           }
        }

        /// <summary>
        /// 自动读取身份证号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_auto_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_auto.Enabled == false)
            {
                form_readsfzh.autoReadShzh_stop();
            }
            else if(checkBox_auto.Enabled == true)
            {
                 form_readsfzh.autoReadShzh_start();
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
                    dateTimePicker_D_CSRQ.Value = Convert.ToDateTime (dtSfzh.Rows[0]["出生日期"].ToString());
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
            if (Status.Equals("1"))
            {
                checkBox_auto.Checked = false;
            }
            else
            {
                checkBox_auto.Checked = true;
            }
            return true;
        }


        /// <summary>
        /// 验证身份证是否符合年龄
        /// </summary>
        /// <returns></returns>
        public bool checksfz_nl( out string message)
        {
            message = "";
            if (lTextBox_D_SFZH.Text.Length > 0)
            {
                //当年度年底
                DateTime  now_year =Convert .ToDateTime(string.Format ("{0}-12-31", DateTime .Now .Year.ToString ())).AddYears (-65) ;
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
        public bool initXzjgBycz( string czbm)
        {
            if (czbm.Length != 11)
            {
                return false;
            }

            //乡镇  37160211 013
            string xiangzhen = czbm.Substring (0,8);
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


    }
}
