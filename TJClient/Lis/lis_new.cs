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
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using TJClient;
using TJClient.sys;
using TJClient.Common;
using LIS;
using TJClient.sys.Bll;
using TJClient.jktj;
using System.Reflection;
namespace FBYClient
{
    public partial class lis_new : sysCommonForm
    {
        #region 成员变量

        /// <summary>
        /// 保存控件的原有的颜色
        /// </summary>
        public Color color_tem = new Color();

        /// <summary>
        /// 仪器
        /// </summary>
        public static IInterface yqDemo = null;

        //自动接收数据
        public sysAutoForm autoform = null;
        //public AutoForm_sgtz autoform_sgtz = null;

        //心电设备型号
        public string xdtype = null;
        /// <summary>
        /// 体检人员信息
        /// </summary>
        public DataTable dt_list_tjryxx = null;

        /// <summary>
        /// 申请号
        /// </summary>
        public string sqh = "";

        public string yq_tem = "";

        public string ybh_tem = "";

        public string jyrq_tem = "";

        /// <summary>
        /// 是否进行初始化的操作
        /// </summary>
        public bool isDoInit = false;

        /// <summary>
        /// 前页面传过来的参数
        /// </summary>
        DataTable dt_para_sys = null;

        /// <summary>
        /// 是否是开始的处理
        /// </summary>
        public bool isStart = true;

        public bool setPara(DataTable dtpara)
        {
            try
            {
                dt_para_sys = dtpara;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 取得选择的仪器型号
        /// </summary>
        /// <returns></returns>
        public string getYqlx()
        {
            string yqlx = "";
            if (comboBox_yq.SelectedValue != null)
            {
                yqlx = comboBox_yq.SelectedValue.ToString();
            }
            return yqlx;
        }


        public lis_new()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载健康体检项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lis_Load(object sender, EventArgs e)
        {
            dataGridView_hybb.AutoGenerateColumns = false;
            dataGridView_item.AutoGenerateColumns = false;

            //性别
            comboBox_head_XB.DataSource = getSjzdList("xb_xingbie");
            comboBox_head_XB.DisplayMember = "ZDMC";
            comboBox_head_XB.ValueMember = "ZDBM";

            listboxFormBll listbox = new listboxFormBll();
            DataTable dtCunzhuang = listbox.GetMoHuList(string.Format("and YLJGBM = '{0}'", UserInfo.Yybm), "sql038");
            //绑定医疗机构
            if (dtCunzhuang != null)
            {
                DataRow dtRow = dtCunzhuang.NewRow();
                dtRow["czbm"] = "";
                dtRow["czmc"] = "--请选择--";
                dtCunzhuang.Rows.InsertAt(dtRow, 0);
                comboBox_cunzhuang.DataSource = dtCunzhuang;
                comboBox_cunzhuang.DisplayMember = "czmc";
                comboBox_cunzhuang.ValueMember = "czbm";
            }

            //绑定仪器列表
            setDrp(comboBox_yq, "hyyq", true);


            //绑定检验类型
            comboBox_lisType.DataSource = getLisType();
            comboBox_lisType.DisplayMember = "tmmc";
            comboBox_lisType.ValueMember = "tmbm";

            //初始化签名基础信息
            SignNameGroupInit();

            isDoInit = true;
            comboBox_cunzhuang.Focus();
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
            string sql = @"select * from  T_JK_SJZD where ZDLXBM='{ZDLXBM}' order by zdbm ";

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
        /// 取得数据字典中的项目
        /// </summary>
        /// <param name="Sjzdbm"></param>
        /// <returns></returns>
        private DataTable getLisType()
        {
            //获取该工作组对应的体检项目
            string sql = string.Format("select * from T_JK_TM where  yljgbm='{0}' ", UserInfo.Yybm);

            DataTable checkList = new DataTable();
            DBAccess dBAccess = new DBAccess();
            checkList = dBAccess.ExecuteQueryBySql(sql);
            if (checkList != null && checkList.Rows.Count > 0)
            {
                DataRow dtRow = checkList.NewRow();
                checkList.Rows.InsertAt(dtRow, 0);
                return checkList;
            }
            return null;
        }

        /// <summary>
        /// 初始化个人信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        private void initHead(string sqlWhere)
        {
            if (dt_list_tjryxx != null && dt_list_tjryxx.Rows.Count > 0)
            {
                int i = listBox_ryxx.SelectedIndex;
                DataRow dt_ryxx = dt_list_tjryxx.Rows[i];

                if (dt_ryxx != null)
                {
                    //村庄编码
                    label_czbm.Text = dt_ryxx["CZBM"].ToString();
                    textBox_head_XM.Text = dt_ryxx["XM"].ToString();
                    comboBox_head_XB.SelectedValue = dt_ryxx["XB"].ToString();
                    textBox_head_SFZH.Text = dt_ryxx["SFZH"].ToString();
                    textBox_head_LXDH.Text = dt_ryxx["LXDH"].ToString();
                    textBox_head_TJPCH.Text = dt_ryxx["TJPCH"].ToString();
                    textBox_head_JKDAH.Text = dt_ryxx["JKDAH"].ToString();
                    label_czbm.Text = dt_ryxx["CZBM"].ToString();
                    //出生日期
                    dateTimePicker_head_CSRQ.Value = getDateFromString(dt_ryxx["CSRQ"].ToString());
                    //体检时间
                    //dateTimePicker_head_TJSJ.Value = getDateFromString(dt_ryxx["resulttime"].ToString());
                    label_sfh.Text = dt_ryxx["SFH"].ToString();

                    //条形码编号
                    //textBox_TJBH.Text = dt_ryxx["tjbm"].ToString() + comboBox_lisType.SelectedValue .ToString();
                    textBox_tmh.Text = dt_ryxx["tjbm"].ToString() + comboBox_lisType.SelectedValue.ToString();
                    sqh = textBox_tmh.Text;
                }
            }
        }

        /// <summary>
        /// 获取条形码
        /// </summary>
        private void getTxmList()
        {
            if (dt_list_tjryxx != null && dt_list_tjryxx.Rows.Count > 0)
            {
                int i = listBox_ryxx.SelectedIndex;
                DataRow dt_ryxx = dt_list_tjryxx.Rows[i];

                if (dt_ryxx != null)
                {

                    textBox_head_SFZH.Text = dt_ryxx["SFZH"].ToString();

                    textBox_head_JKDAH.Text = dt_ryxx["JKDAH"].ToString();

                }
            }
        }

        /// <summary>
        /// 按照条件获取条形码
        /// </summary>
        /// <returns></returns>
        private DataTable getTxmList(string strWhere)
        {
            //获取该工作组对应的体检项目
            string sql = string.Format("select * from  view_tjry_txm where 1=1 {0} order by tmbm ", strWhere);

            DataTable checkList = new DataTable();
            DBAccess dBAccess = new DBAccess();
            checkList = dBAccess.ExecuteQueryBySql(sql);
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

        /// <summary>
        /// 设定获取人员信息的条件
        /// </summary>
        private bool selectRyxx(bool isTxm)
        {
            //检验仪器
            if (comboBox_yq.SelectedValue == null || (comboBox_yq.SelectedValue != null && comboBox_yq.SelectedValue.ToString().Length == 0))
            {
                timer_yq.Enabled = false;
                MessageBox.Show("请选择仪器！");
                comboBox_yq.Focus();
                return false;
            }

            //检验类型
            if (comboBox_lisType.SelectedValue == null || (comboBox_lisType.SelectedValue != null && comboBox_lisType.SelectedValue.ToString().Length == 0))
            {
                timer_yq.Enabled = false;
                MessageBox.Show("请设定检验类型！");
                comboBox_lisType.Focus();
                return false;
            }

            string sqlWhere = "";

            //体检号
            if (textBox_TJBH.Text.Trim().Length > 0)
            {
                // string jkdah_tem = getJkdahByTxm();
                string jkdah_tem = textBox_TJBH.Text.Length > 12 ? textBox_TJBH.Text.Substring(0, 12) : textBox_TJBH.Text;
                sqlWhere = string.Format(" {0} and  (t.jkdah ='{1}' or t.tjbm='{2}')  ", sqlWhere, textBox_TJBH.Text, jkdah_tem);
            }

            //非扫码查询时才考虑一下查询条件
            if (isTxm == false)
            {
                //村庄
                if (comboBox_cunzhuang.SelectedValue != null && comboBox_cunzhuang.SelectedValue.ToString().Length > 0)
                {
                    //sqlWhere = string.Format(" {0} and t.CZBM = '{1}' ", sqlWhere, comboBox_cunzhuang.SelectedValue.ToString());
                    sqlWhere = string.Format(" {0} and PRgID = '{1}' ", sqlWhere, comboBox_cunzhuang.SelectedValue.ToString());

                }

                //体检时间
                if (dateTimePicker_tjsj.Checked == true)
                {
                    sqlWhere = string.Format(" {0} and (tjzt.TJSJ ='{1}' or tjzt.TJSJ ='{2}' ) ", sqlWhere, dateTimePicker_tjsj.Value.ToString("yyy-MM-dd"), dateTimePicker_tjsj.Value.ToString("yyy/M/d"));
                }

                //姓名
                if (textBox_xm.Text.Trim().Length > 0)
                {
                    sqlWhere = string.Format(" {0} and  t.XM like '%{1}%'  ", sqlWhere, textBox_xm.Text.Trim());
                }

                //体检状态
                if (!checkBox_wtj.Checked == checkBox_ytj.Checked)
                {
                    //未体检
                    if (checkBox_wtj.Checked == true)
                    {
                        sqlWhere = string.Format(" {0} and  (tjzt.ZT is null or tjzt.ZT='')  ", sqlWhere);
                    }
                    else //已体检
                    {
                        sqlWhere = string.Format(" {0} and  (tjzt.ZT ='1' )  ", sqlWhere);
                    }
                }
            }
            //检验类型
            //sqlWhere = string.Format(" {0} and  tmbm='{1}'  ", sqlWhere, comboBox_lisType.SelectedValue.ToString());

            DataTable dt = initList_ryxx(sqlWhere);

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
                if (drv != null)
                {
                    //initHead(" and SFH like '" + drv[listBox_ryxx.ValueMember] + "' ");
                }
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
                    else
                    {
                        jkdah = textBox_TJBH.Text.Trim();
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
        /// 初始化人员信息
        /// </summary>
        private DataTable initList_ryxx(string strWhere)
        {
            DataTable dt_ryxx = getList_ryxx(strWhere);
            if (dt_ryxx != null && dt_ryxx.Rows.Count > 0)
            {
                isStart = false;
                dt_list_tjryxx = null;
                listBox_ryxx.DataSource = null;
                //listBox_ryxx.Items.Clear();
                for (int i = 0; i < dt_ryxx.Rows.Count; i++)
                {
                    dt_ryxx.Rows[i]["DisplayMember"] = (i + 1).ToString().PadRight(5, ' ') + dt_ryxx.Rows[i]["XM"].ToString();
                    dt_ryxx.Rows[i]["ValueMember"] = dt_ryxx.Rows[i]["SFH"].ToString();
                }

                listBox_ryxx.DataSource = dt_ryxx.Copy();
                listBox_ryxx.DisplayMember = "DisplayMember";
                listBox_ryxx.ValueMember = "ValueMember";
                dt_list_tjryxx = dt_ryxx.Copy();

                isStart = true;
                return dt_ryxx;
            }
            else
            {
                dt_list_tjryxx = null;
                listBox_ryxx.DataSource = null;
                //MessageBox.Show("没有取到对应的体检人员信息，请确认！");
            }
            return null;
        }

        /// <summary>
        /// 取得人员信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        private DataTable getList_ryxx(string strWhere)
        {

            jktjBll jktjbll = new jktjBll();

            DataTable dt_ryxx = jktjbll.GetMoHuList(string.Format(" and t.YLJGBM='{0}' {1}", UserInfo.Yybm, strWhere), "sql_LIS_list");

            if (dt_ryxx != null && dt_ryxx.Rows.Count > 0)
            {

                return dt_ryxx;
            }
            return null;
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void setPageClear()
        {
            //try
            //{
            //    if (dtControl == null)
            //    {
            //        return;
            //    }
            //    for (int i = 0; i < dtControl.Rows.Count; i++)
            //    {
            //        dtControl.Rows[i]["value"] = "";
            //        setValueToControl(dtControl.Rows[i]["ControlId"].ToString(), dtControl.Rows[i]["value"].ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        #endregion

        /// <summary>
        /// 系统设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_sys_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form_sysEdit form_sys = new Form_sysEdit();
            form_sys.ShowDialog();
        }

        /// <summary>
        /// 获得焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_Enter(object sender, EventArgs e)
        {
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
            Control control = (Control)sender;
            control.ForeColor = Color.FromArgb(0, 0, 0);
            control.BackColor = color_tem;

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
        /// 村庄事件KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_cunzhuang_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
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
            if (e.KeyCode == Keys.Enter)
            {
                TextBox text = (TextBox)sender;

                //申请号
                sqh = text.Text;
                //if (text.Text.Length > 12)
                //{
                //    text.Text = text.Text.Substring(0, 12);
                //}
                if (selectRyxx(true) == true)
                {
                    initHead("");
                    init_dataGridView_hybb();
                    text.SelectAll();
                }
                else
                {
                    if (text.Text.Trim().Length > 0)
                    {

                        Form_PeopleAdd form_peopleadd = new Form_PeopleAdd();
                        form_peopleadd.tjbh = textBox_TJBH.Text;
                        textBox_tmh.Text = textBox_TJBH.Text;
                        form_peopleadd.czbm = comboBox_cunzhuang.SelectedValue != null ? comboBox_cunzhuang.SelectedValue.ToString() : "";
                        form_peopleadd.Owner = this;
                        form_peopleadd.ShowDialog();
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

        }

        /// <summary>
        /// 设定返回的结果
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public override bool setParentFormDo(object para)
        {
            if (selectRyxx(false) == true)
            {
                initHead("");

            }
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
            if (e.KeyCode == Keys.Enter)
            {
                if (selectRyxx(false))
                {
                    initHead("");
                    TextBox text = (TextBox)sender;
                    text.SelectAll();
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
        /// 身份证号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_SFZH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (selectRyxx(false))
                {
                    TextBox text = (TextBox)sender;
                    text.SelectAll();
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
            if (e.KeyCode == Keys.Enter)
            {
                if (selectRyxx(false))
                {
                    initHead("");
                    TextBox text = (TextBox)sender;
                    text.SelectAll();
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
        /// cheeckbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_tj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckBox check_tem = (CheckBox)sender;
                //if()
                check_tem.Checked = !check_tem.Checked;
                if (selectRyxx(false))
                {
                    // SendKeys.Send("{Tab}");
                    //TextBox text = (TextBox)sender;
                    //text.SelectAll();
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
        /// 人员列表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_ryxx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox_head_XM.Focus();
                e.Handled = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                //SendKeys.Send("{Tab}");
                //textBox_head_XM.Focus();
                //if (dtControl != null && dtControl.Rows.Count > 0)
                //{
                //    string ControlId = dtControl.Rows[0]["ControlId"].ToString();
                //    Control control = Controls.Find(ControlId, true)[0];
                //    control.Focus();
                //}
                e.Handled = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                SendKeys.Send("+{Tab}");
                e.Handled = false;
            }
        }

        /// <summary>
        /// 人员列表中选择人员后的处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_ryxx_SelectedIndexChanged(object sender, EventArgs e)
        {
            //setPageClear();
            if (isStart == true)
            {
                DataRowView drv = listBox_ryxx.SelectedItem as DataRowView;
                initHead(" and SFH like '" + listBox_ryxx.SelectedValue + "' ");
                if (checkBox_ytj.Checked == true || checkBox_tmh.Checked ==true)
                {
                    init_dataGridView_hybb();
                }
            }
        }


        #region 仪器

        /// <summary>
        /// 保存接收到的结果
        /// </summary>
        public DataTable dt_yq_result = null;

        /// <summary>
        /// 间隔 0：手动   0以外：间隔时间
        /// </summary>
        public string YQ_Interval = "";

        /// <summary>
        /// 标本日期类型 0：化验仪器日期   1：系统日期  
        /// </summary>
        public string YQ_DateType = "";

        public bool drpFlag = false;
        public string YQ_Type = "";
        public string YQ_RegisterCode = "";

        /// <summary>
        /// 设定间隔时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_autoYqItem_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_autoYqItem.Checked == true)
            {
                label_yqauto.Visible = true;
                textBox_yqAuto.Visible = true;
                timer_yq.Enabled = true;
                if (YQ_Interval.Equals("0"))
                {
                    textBox_yqAuto.Text = "2";
                }
                else
                {
                    textBox_yqAuto.Text = YQ_Interval;
                }

            }
            else
            {
                label_yqauto.Visible = false;
                textBox_yqAuto.Visible = false;
                timer_yq.Enabled = false;
            }
        }

        /// <summary>
        /// 选择仪器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_yq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string YqXmlPath = "";

            try
            {
                if (drpFlag == true && comboBox_yq.SelectedValue != null && comboBox_yq.SelectedValue.ToString().Length > 0)
                {
                    string cmbyq = comboBox_yq.SelectedValue.ToString();
                    if (cmbyq == "MECG200" || cmbyq == "ECGNETV260" || cmbyq== "ECGNETMS")
                    {
                        btn_medex.Visible = true;
                        xdtype = cmbyq;
                    }
                    else
                    {
                        btn_medex.Visible = false;
                    }
                    //xmlpath
                    //YqXmlPath = System.Configuration.ConfigurationSettings.AppSettings[comboBox_yq.SelectedValue.ToString()].ToString();
                    YqXmlPath = Common.getyqPath(comboBox_yq.SelectedValue.ToString());
                    if (YqXmlPath.Length <= 0)
                    {
                        MessageBox.Show("配置文件不存在!");
                        return ;
                    }
                    
                    //jg数据处理间隔时间
                    YQ_Interval = XmlRW.GetValueFormXML(YqXmlPath, "YQ_Interval", "value");

                    //rqlx	标本日期类型
                    YQ_DateType = XmlRW.GetValueFormXML(YqXmlPath, "YQ_DateType", "value");

                    //仪器数据处理类型
                    YQ_Type = XmlRW.GetValueFormXML(YqXmlPath, "YQ_YQType", "value");
                    
                    //仪器验证码
                    YQ_RegisterCode = XmlRW.GetValueFormXML(YqXmlPath, "YQ_RegisterCode", "value");

                    string formName = "AutoForm";
                    DataTable dtdrp_tem = (DataTable)comboBox_yq.DataSource;
                    if (dtdrp_tem != null && dtdrp_tem.Rows.Count > 0)
                    {
                        formName = dtdrp_tem.Rows[comboBox_yq.SelectedIndex]["bz"] != null ? dtdrp_tem.Rows[comboBox_yq.SelectedIndex]["bz"].ToString() : "AutoForm";
                    }

                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name.Equals(formName))
                        {
                            frm.Close();
                            break;
                        }
                    }


                    //YqRegister
                    //验证
                    RegisterBll RegisterBllDom = new RegisterBll();
                    string outMsg = "";
                    //bool RegisterResult = RegisterBllDom.YqRegister(UserInfo.Yybm, UserInfo.Yymc, comboBox_yq.SelectedValue.ToString(), YQ_RegisterCode, out outMsg);
                    bool RegisterResult = true;
                    if (RegisterResult)
                    {
                        //显示提示信息
                        if (outMsg.Length > 0)
                        {
                            MessageBox.Show(outMsg);
                        }
                        //启动自动获取数据
                        //autoform = new AutoForm_sgtz();
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        autoform = assembly.CreateInstance("FBYClient." + formName) as sysAutoForm;
                        if (autoform != null)
                        {
                            autoform.Owner = this;
                            autoform.Show();
                            autoform.Visible = false;
                            autoform.setStart(comboBox_yq.SelectedValue.ToString());
                        }
                        else
                        {
                            MessageBox.Show("请确认仪器的配置参数是否正确！");
                        }
                    }
                    else
                    {
                        MessageBox.Show(outMsg);
                    }

                    //检验结果初始化（右侧列表）
                    //init_dataGridView_hybb();
                    //}
                    //else //if (comboBox_yq.SelectedValue.ToString() == "HT2000")
                    //{
                    //    foreach (Form frm in Application.OpenForms)
                    //    {
                    //        if (frm.Name.Equals("AutoForm"))
                    //        {
                    //            frm.Close();
                    //            break;
                    //        }
                    //    }

                    //    //启动自动获取数据
                    //    autoform = new AutoForm();
                    //    autoform.Owner = this;
                    //    autoform.Show();
                    //    autoform.Visible = false;
                    //    autoform.setStart(comboBox_yq.SelectedValue.ToString());

                    //    //检验结果初始化（右侧列表）
                    //    //init_dataGridView_hybb();
                    //}                      
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                drpFlag = false;
                //获取结果集
                DataTable dt_yq = Common.getsjzd(zdCode, "sql_select_sjzd");
                if (ifkh == true)
                {
                    DataRow dtRow = dt_yq.NewRow();
                    dtRow["ZDMC"] = "";
                    dtRow["ZDBM"] = "";
                    dt_yq.Rows.InsertAt(dtRow, 0);                    
                }
                drp.DisplayMember = "ZDMC";
                drp.ValueMember = "ZDBM";
                drp.DataSource = dt_yq;
                drpFlag = true;

                DataTable sdsd = (DataTable)drp.DataSource;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //获取结果
                //dt_yq_result = dataRecive();
                init_dataGridView_hybb();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 刷新
        /// </summary>
        /// <returns></returns>
        public bool init_dataGridView_hybb()
        {
            string strWhere = "";

            if (comboBox_yq.SelectedValue == null || (comboBox_yq.SelectedValue != null && comboBox_yq.SelectedValue.ToString().Length == 0))
            {
                timer_yq.Enabled = false;
                MessageBox.Show("请选择仪器！");
                return false;
            }

            //仪器
            if (comboBox_yq.SelectedValue != null && comboBox_yq.SelectedValue.ToString().Length > 0)
            {
                strWhere = strWhere + string.Format(" and yq='{0}' and  yq <> null ", comboBox_yq.SelectedValue.ToString());
            }
            else
            {
                // MessageBox.Show("");
                return false;
            }

            //检测只针对已经对应过的数据
            if (checkBox_jc.Checked == true)
            {
                strWhere = strWhere + string.Format(" and  T.testno<>' ' and T.testno is not null  and t_jk_tjzt.jkdah is null  ");
            }
            else
            {

                //是否已经对应
                if (!checkBox_All_ydy.Checked == checkBox_All_wdy.Checked)
                {
                    if (checkBox_All_ydy.Checked == true)
                    {
                        strWhere = strWhere + " and yq is not null and T.testno is not null";
                        //TJBM
                        if (textBox_tmh.Text.Trim().Length > 0)
                        {
                            strWhere = strWhere + string.Format(" and (testno = '{0}' or testno='{1}' )", textBox_tmh.Text.Trim(), textBox_head_JKDAH.Text);
                        }

                        //XM
                        if (textBox_xm.Text.Trim().Length > 0)
                        {
                            strWhere = strWhere + string.Format(" and XM like '%{0}%' ", textBox_xm.Text.Trim());
                        }

                    } 
                    
                    if (checkBox_All_ydy.Checked == false && checkBox_All_wdy.Checked == true)
                    {
                        strWhere = strWhere + string.Format(" and (testno is  null or T.testno=' ') ");
                    }


                    //检测时不考虑条形码号
                    if (checkBox_jc.Checked == false)
                    {
                        //条形码号
                        if (textBox_tmh.Text.Trim().Length > 0)
                        {
                            strWhere = strWhere + string.Format(" and (T.testno = '{0}' or T.testno = '{1}' or  T.testno is null or T.testno=' ' )", textBox_tmh.Text.Trim(), textBox_head_JKDAH.Text);
                        }
                    }
                }
            }

            //按照申请号检索
            if (checkBox_tmh.Checked == true)
            {
                strWhere = strWhere + string.Format(" and ybh = '{0}' ", textBox_tmh.Text);
            }

            if (dateTimePicker_head_TJSJ.Checked == true)
            {
                strWhere = strWhere + string.Format(" and (jyrq='{0}'or jyrq='{1}') ", dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd"), dateTimePicker_head_TJSJ.Value.ToString("MM-dd-yyyy"));
            }
            //样本号
            if (textBox_ybh.Text.Trim().Length > 0)
            {
                strWhere = strWhere + string.Format(" and T.ybh = '{0}' ", textBox_ybh.Text.Trim());
            }

           

           // Form_lisBll form_lisbll = new Form_lisBll();

            //DataTable dt_T_JK_lis_result_re = form_lisbll.GetMoHuList(strWhere, "sql_lis_result_ryxx_select_new");
            DBAccess dBAccess = new DBAccess();
            string sql_Sjzd ="";

            if (checkBox_jc.Checked == false)
            {
                sql_Sjzd = Common.getSql("sql_lis_result_ryxx_select_new", "");
            
            }
            else
            {
                sql_Sjzd = Common.getSql("sql_lis_result_ryxx_select_check", "");
            }

            sql_Sjzd = string.Format(sql_Sjzd, comboBox_lisType.SelectedValue.ToString(), strWhere);

            DataTable dt_T_JK_lis_result_re  = dBAccess.ExecuteQueryBySql(sql_Sjzd);
            dataGridView_hybb.DataSource = dt_T_JK_lis_result_re;

            return true;
        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_yq_Tick(object sender, EventArgs e)
        {
            try
            {
                // dt_yq_result = dataRecive();
                init_dataGridView_hybb();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                timer_yq.Enabled = false;
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
                    dt = yqDemo.YQDataReturn(dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd"), out errMsg);
                }
                else
                {
                    if (comboBox_yq.SelectedValue == null || comboBox_yq.SelectedValue.ToString().Trim().Length == 0)
                    {
                        timer_yq.Enabled = false;
                        MessageBox.Show("请选择仪器！");

                        return null;
                    }

                    if (yqDemo == null)
                    {

                        string yqVersion = XmlRW.GetValueFormXML(Common.getyqPath(comboBox_yq.SelectedValue.ToString()), "YQ_Version", "value");
                        yqDemo = LisFactory.LisCreate(yqVersion);
                    }

                    if (yqDemo.IsOpen(out errMsg) == false)
                    {
                        yqDemo.open(out errMsg);
                    }
                    dt = yqDemo.YQDataReturn(dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd"), out errMsg);
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
                        dtcolumn.DefaultValue = comboBox_yq.SelectedValue.ToString();
                        dt.Columns.Add(dtcolumn);
                    }

                    Form_lisBll form_lisbll = new Form_lisBll();
                    form_lisbll.Add(dt, "sql042");

                    //刷新
                    init_dataGridView_hybb();

                }

            }
            catch (Exception ex)
            {
                timer_yq.Enabled = false;
                MessageBox.Show("dataRecive:" + ex.Message);
            }
            return dt;
        }


        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            //体检结果
            //sqh 申请号
            if (sqh.Length == 0)
            {
                MessageBox.Show("请输入检验申请号或者扫描检验条码！");
                return;
            }

            if (comboBox_yq.SelectedValue == null)
            {
                MessageBox.Show("请选择仪器！");
                return;
            }

            try
            {
                //LIS
                if (YQ_Type.Equals("1"))
                {
                    DataRowView dr = (DataRowView)dataGridView_hybb.CurrentRow.DataBoundItem;
                    if (dr != null)
                    {
                        if (dr["testno"] != null && dr["testno"].ToString().Length > 0)
                        {
                            //MessageBox.Show("数据已经对应，请清除对应后再对应新的人员！");
                            //return;

                            if (clearDygx(dr)==false)
                            {
                                return;
                            }

                        }
                    }

                    #region  lis
                    //更新化验结果表(T_JK_lis_result_re），确定化验结果与人员关系
                    DataTable dt_update = new DataTable();
                    //体检批次号
                    //dt_update.Columns.Add("TJPCH");
                    ////顺番号
                    //dt_update.Columns.Add("SFH");
                    //医院编码
                    dt_update.Columns.Add("yybm");
                    //仪器编码
                    dt_update.Columns.Add("yq");
                    //样本号
                    dt_update.Columns.Add("ybh");
                    //体检日期
                    dt_update.Columns.Add("jyrq");
                    //申请号
                    dt_update.Columns.Add("testno");
                    //年度
                    dt_update.Columns.Add("nd");

                    dt_update.Rows.Add();
                    dt_update.AcceptChanges();

                    //dt_update.Rows[0]["TJPCH"] = textBox_head_TJPCH.Text;
                    //dt_update.Rows[0]["SFH"] = label_sfh.Text;
                    dt_update.Rows[0]["yybm"] = UserInfo.Yybm;
                    dt_update.Rows[0]["yq"] = yq_tem;
                    dt_update.Rows[0]["ybh"] = ybh_tem;
                    dt_update.Rows[0]["jyrq"] = jyrq_tem;// Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd");
                    dt_update.Rows[0]["testno"] = textBox_tmh.Text;
                    //dt_update.Rows[0]["nd"] = Convert.ToDateTime(jyrq_tem).Year .ToString();
                    dt_update.Rows[0]["nd"] = DateTime.Now.Year.ToString();
                    Form_lisBll form_lisbll = new Form_lisBll();

                    //标本与人员的对应关系保存
                    form_lisbll.Upd(dt_update, "sql_lis_result_ryxx_update");

                    //更新体检人员状态表
                    //form_lisbll.Upd(dt_update, "sql077");
                    //体检状态信息
                    ArrayList TJRYXXList = save_T_JK_TJZT(dt_update.Rows[0]["jyrq"].ToString(), Common.TJTYPE.健康体检表, Common.ZT.确定状态);
                    if (TJRYXXList != null && TJRYXXList.Count > 0)
                    {
                        DBAccess dbaccess = new DBAccess();
                        dbaccess.ExecuteNonQueryBySql(TJRYXXList);
                    }

                    //保存体检结果到体检结果表
                    //标本与人员的对应关系保存
                    string sqlWhere = "";
                    //医院编码
                    if (UserInfo.Yybm.Length > 0)
                    {
                        sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.yybm='{0}'", UserInfo.Yybm);
                    }
                    //仪器
                    if (UserInfo.Yybm.Length > 0)
                    {
                        sqlWhere = sqlWhere + string.Format(" and  T_JK_lis_result_re.yq='{0}'", yq_tem);
                    }
                    //检验日期
                    if (UserInfo.Yybm.Length > 0)
                    {
                        sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.jyrq='{0}'", jyrq_tem);
                    }
                    //样本号
                    if (UserInfo.Yybm.Length > 0)
                    {
                        sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.ybh='{0}'", ybh_tem);
                    }

                    //取得化验结果
                    DataTable dt_dyxm = form_lisbll.GetMoHuList(sqlWhere, "sql045");

                    //更新健康体检表
                    #region 更新健康体检表
                    DataTable dt_tjjgUpdate = new DataTable();
                    dt_tjjgUpdate.Rows.Add();

                    for (int i = 0; i < dt_dyxm.Rows.Count; i++)
                    {
                        if (dt_tjjgUpdate.Columns.Contains(dt_dyxm.Rows[i]["HIS_DB"].ToString()) == false)
                        {
                            DataColumn dtColumn = new DataColumn(dt_dyxm.Rows[i]["HIS_DB"].ToString());
                            dtColumn.DefaultValue = dt_dyxm.Rows[i]["result"].ToString();
                            dt_tjjgUpdate.Columns.Add(dtColumn);
                        }
                    }

                    if (dt_tjjgUpdate.Columns.Contains("D_GRDABH") == false)
                    {
                        DataColumn dtColumn = new DataColumn("D_GRDABH");
                        dtColumn.DefaultValue = textBox_head_JKDAH.Text.ToString();
                        dt_tjjgUpdate.Columns.Add(dtColumn);
                    }
                    if (dt_tjjgUpdate.Columns.Contains("HAPPENTIME") == false)
                    {
                        DataColumn dtColumn = new DataColumn("HAPPENTIME");
                        dtColumn.DefaultValue = Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd");
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
                    GuidResult = getNewGuid(out Guid);

                    if (dt_tjjgUpdate.Columns.Contains("guid") == false)
                    {
                        DataColumn dtColumn = new DataColumn("guid");
                        dtColumn.DefaultValue = Guid;
                        dt_tjjgUpdate.Columns.Add(dtColumn);
                    }

                    //体检结果
                    if (GuidResult == true)
                    {
                        //体检结果插入
                        form_lisbll.Add(dt_tjjgUpdate, "sql047");
                    }
                    else
                    {
                        //体检结果更新
                        dt_tjjgUpdate.AcceptChanges();
                        dt_tjjgUpdate.Rows[0]["guid"] = Guid;
                        form_lisbll.Upd(dt_tjjgUpdate, "sql048");
                    }
                    #endregion

                    //体检结果保存 到与lis交互的中间表中
                    string sqlWhere1 = "";
                    //医院编码
                    if (UserInfo.Yybm.Length > 0)
                    {
                        sqlWhere1 = sqlWhere1 + string.Format(" and yybm='{0}'", UserInfo.Yybm);
                    }
                    //仪器
                    if (UserInfo.Yybm.Length > 0)
                    {
                        sqlWhere1 = sqlWhere1 + string.Format(" and  yq='{0}'", yq_tem);
                    }
                    //检验日期
                    if (UserInfo.Yybm.Length > 0)
                    {
                        sqlWhere1 = sqlWhere1 + string.Format(" and jyrq='{0}'", jyrq_tem);
                    }
                    //样本号
                    if (UserInfo.Yybm.Length > 0)
                    {
                        sqlWhere1 = sqlWhere1 + string.Format(" and ybh='{0}'", ybh_tem);
                    }

                    //取得化验结果
                    DataTable dt_dyxm_re = form_lisbll.GetMoHuList(sqlWhere, "sql046");
                    form_lisbll.DelBySql(string.Format(" and testno='{0}' and yybm='{1}'", sqh, UserInfo.Yybm), "sql079");

                    DataTable dt_re = new DataTable();
                    //申请号
                    DataColumn testno = new DataColumn();
                    testno.DefaultValue = sqh;
                    testno.ColumnName = "testno";
                    dt_re.Columns.Add(testno);

                    ////样本号
                    //DataColumn sampleNo = new DataColumn();
                    //sampleNo.DefaultValue = ybh_tem;
                    //sampleNo.ColumnName = "sampleno";
                    //dt_re.Columns.Add(sampleNo);

                    //序号
                    dt_re.Columns.Add("seqno");
                    //报告项目代码
                    dt_re.Columns.Add("itemno");
                    //报告项目名称
                    dt_re.Columns.Add("itemname");
                    //检验结果
                    dt_re.Columns.Add("testresult");
                    //检验结果
                    dt_re.Columns.Add("result1");
                    //检验结果
                    dt_re.Columns.Add("result2");
                    //单位
                    dt_re.Columns.Add("units");
                    //参考范围
                    dt_re.Columns.Add("ranges");
                    //结果时间
                    dt_re.Columns.Add("resulttime");
                    //最后修改时间
                    dt_re.Columns.Add("lastmodify");

                    //检验医生
                    DataColumn testman = new DataColumn();
                    testman.DefaultValue = ""; //UserInfo.userId;
                    testman.ColumnName = "testman";
                    dt_re.Columns.Add(testman);

                    //核对医生
                    dt_re.Columns.Add("checkman");

                    //检测仪器
                    DataColumn instrument = new DataColumn();
                    instrument.DefaultValue = comboBox_yq.SelectedValue;
                    instrument.ColumnName = "instrument";
                    dt_re.Columns.Add(instrument);

                    //样本号
                    DataColumn sampleno = new DataColumn();
                    sampleno.DefaultValue = ybh_tem;
                    sampleno.ColumnName = "sampleno";
                    dt_re.Columns.Add(sampleno);

                    //病人姓名
                    DataColumn brxm = new DataColumn();
                    brxm.DefaultValue = textBox_head_XM.Text;
                    brxm.ColumnName = "brxm";
                    dt_re.Columns.Add(brxm);

                    //医院编码
                    DataColumn yybm = new DataColumn();
                    yybm.DefaultValue = UserInfo.Yybm;
                    yybm.ColumnName = "yybm";
                    dt_re.Columns.Add(yybm);
                    //数据来源
                    DataColumn dataFrom = new DataColumn();
                    dataFrom.DefaultValue = "1";
                    dataFrom.ColumnName = "dataFrom";
                    dt_re.Columns.Add(dataFrom);

                    //病历号
                    DataColumn brdh = new DataColumn();
                    brdh.DefaultValue = textBox_head_JKDAH.Text;
                    brdh.ColumnName = "brdh";
                    dt_re.Columns.Add(brdh);

                    //身份证号
                    DataColumn SFZH = new DataColumn();
                    SFZH.DefaultValue = textBox_head_SFZH.Text;
                    SFZH.ColumnName = "SFZH";
                    dt_re.Columns.Add(SFZH);

                    //健康档案号
                    DataColumn JKDAH = new DataColumn();
                    JKDAH.DefaultValue = textBox_head_JKDAH.Text;
                    JKDAH.ColumnName = "JKDAH";
                    dt_re.Columns.Add(JKDAH);

                    //村庄编码
                    DataColumn CZBM = new DataColumn();
                    CZBM.DefaultValue = label_czbm.Text;
                    CZBM.ColumnName = "CZBM";
                    dt_re.Columns.Add(CZBM);

                    //
                    for (int i = 0; i < dt_dyxm_re.Rows.Count; i++)
                    {
                        dt_re.Rows.Add();

                        //序号
                        dt_re.Rows[dt_re.Rows.Count - 1]["seqno"] = (i + 1).ToString();

                        //报告项目代码
                        dt_re.Rows[dt_re.Rows.Count - 1]["itemno"] = dt_dyxm_re.Rows[i]["xmdh"].ToString();
                        //报告项目名称
                        dt_re.Rows[dt_re.Rows.Count - 1]["itemname"] = dt_dyxm_re.Rows[i]["xmmc"].ToString();
                        //检验结果
                        dt_re.Rows[dt_re.Rows.Count - 1]["testresult"] = dt_dyxm_re.Rows[i]["result"].ToString();
                        //检验结果
                        dt_re.Rows[dt_re.Rows.Count - 1]["result1"] = dt_dyxm_re.Rows[i]["result1"].ToString();
                        //检验结果
                        dt_re.Rows[dt_re.Rows.Count - 1]["result2"] = "";
                        //单位
                        dt_re.Rows[dt_re.Rows.Count - 1]["units"] = dt_dyxm_re.Rows[i]["dw"].ToString();

                        ////参考范围
                        //dt.Rows[dt.Rows.Count - 1]["ranges"] = dt_dyxm_re.Rows[i][""].ToString(); 

                        //结果时间
                        dt_re.Rows[dt_re.Rows.Count - 1]["resulttime"] = Convert.ToDateTime(dt_dyxm_re.Rows[i]["jyrq"].ToString()).ToString("yyyy-MM-dd");
                        //最后修改时间
                        dt_re.Rows[dt_re.Rows.Count - 1]["lastmodify"] = Convert.ToDateTime(dt_dyxm_re.Rows[i]["jyrq"].ToString()).ToString("yyyy-MM-dd");
                    }

                    //添加结果 lis_reqResult
                    form_lisbll.Add(dt_re, "sql080");
                   
                    //刷新右侧列表
                    init_dataGridView_hybb();
                    //MessageBox.Show(DateTime.Now.ToString("HH:mm:ss fff"));
                    //刷新左侧人员列表
                    if (selectRyxx(false) == true)
                    {
                        initHead("");
                    }

                    MessageBox.Show("保存成功！");

                    if (textBox_ybh.Text.Length > 0)
                    {
                        textBox_ybh.Text = (int.Parse(textBox_ybh.Text) + 1).ToString();
                    }

                    #endregion
                }
                //心电
                else if (YQ_Type.Equals("0"))
                {
                    button_save_Xd();
                }
                else
                {
                    button_save_sys(YQ_Type);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// 清除对应关系
        /// </summary>
        public bool clearDygx(DataRowView dr)
        {
            try
            {
                #region 清除对应
                DialogResult result;
                result = MessageBox.Show("清除体检人员与化验结果对应关系？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    return false;
                }
                else
                {
                    int selectRowIndex = 0;

                    selectRowIndex = dataGridView_hybb.CurrentRow.Index;

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
                    //检验号
                    dt_update.Columns.Add("testno");
                    //年度
                    dt_update.Columns.Add("nd");

                    dt_update.Rows.Add();
                    dt_update.AcceptChanges();

                    dt_update.Rows[0]["testno"] = "";
                    dt_update.Rows[0]["nd"] = "";
                    dt_update.Rows[0]["yybm"] = UserInfo.Yybm;
                    dt_update.Rows[0]["yq"] = yq_tem;
                    dt_update.Rows[0]["ybh"] = ybh_tem;
                    dt_update.Rows[0]["jyrq"] = dr["jyrq"].ToString();

                    Form_lisBll form_lisbll = new Form_lisBll();

                    //标本与人员的对应关系保存
                    form_lisbll.Upd(dt_update, "sql_lis_result_clear");

                    //更新体检人员状态表
                    ArrayList TJRYXXList = delete_T_JK_TJZT(dr["jyrq"].ToString(), Common.TJTYPE.健康体检表, Common.ZT.确定状态, dr["jkdah"].ToString());
                    if (TJRYXXList != null && TJRYXXList.Count > 0)
                    {
                        DBAccess dbaccess = new DBAccess();
                        dbaccess.ExecuteNonQueryBySql(TJRYXXList);
                    }

                    //删除lis_reqresult表中的数据
                    //form_lisbll.DelBySql(string.Format(" and  brdh='{0}'", dr["jkdah"].ToString()), "sql096");
                    //LIS
                    if (YQ_Type.Equals("1"))
                    {
                        //删除lis_reqresult表中的数据
                        form_lisbll.DelBySql(string.Format(" and  brdh='{0}'", dr["jkdah"].ToString()), "sql096");
                    }
                    else if (YQ_Type.Equals("0")) //心电
                    {
                        //删除心电图的数据
                        form_lisbll.DelBySql(string.Format(" and  yljgbm='{0}' and jkdabh='{1}'", UserInfo.Yybm, dr["jkdah"].ToString()), "sql087");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex .Message);
                return false;
            }
            return true;
        }





        /// <summary>
        /// 体检状态表(T_JK_TJZT）保存
        /// </summary>
        public ArrayList save_T_JK_TJZT(string tjsj, string TJTYPE, string zt)
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
            SqlDele = SqlDele.Replace("{JKDAH}", textBox_head_JKDAH.Text);
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
            SqlAdd = SqlAdd.Replace("{JKDAH}", textBox_head_JKDAH.Text);
            //姓名
            SqlAdd = SqlAdd.Replace("{XM}", textBox_head_XM.Text);
            //身份证号
            SqlAdd = SqlAdd.Replace("{SFZH}", textBox_head_SFZH.Text);
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

        /// <summary>
        /// 体检状态表(T_JK_TJZT）清除
        /// </summary>
        public ArrayList delete_T_JK_TJZT(string tjsj, string TJTYPE, string zt, string JKDAH)
        {
            ArrayList returnArrayList = new ArrayList();

            //删除用sql
            string SqlDele = Common.getSql("sql801", " ");

            //删除用sql
            //医疗机构编码
            SqlDele = SqlDele.Replace("{YLJGBM}", UserInfo.Yybm);
            //健康档案号
            SqlDele = SqlDele.Replace("{JKDAH}", JKDAH);
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

            return returnArrayList;
        }

        /// <summary>
        /// 心电保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Xd()
        {
            DataRowView dr = (DataRowView)dataGridView_hybb.CurrentRow.DataBoundItem;
            if (dr != null)
            {
                if (dr["testno"] != null && dr["testno"].ToString().Length > 0)
                {
                    //MessageBox.Show("数据已经对应，请清除对应后再对应新的人员！");
                    //return;
                    if (clearDygx(dr) == false)
                    {
                        return;
                    }
                }
            }

            //体检结果
            //sqh 申请号
            if (sqh.Length == 0)
            {
                MessageBox.Show("请输入检验申请号或者扫描检验条码！");
                return;
            }

            try
            {
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
                //申请号
                dt_update.Columns.Add("testno");
                //年度
                dt_update.Columns.Add("nd");

                dt_update.Rows.Add();
                dt_update.AcceptChanges();

                //dt_update.Rows[0]["TJPCH"] = textBox_head_TJPCH.Text;
                //dt_update.Rows[0]["SFH"] = label_sfh.Text;
                dt_update.Rows[0]["yybm"] = UserInfo.Yybm;
                dt_update.Rows[0]["yq"] = yq_tem;
                dt_update.Rows[0]["ybh"] = ybh_tem;
                dt_update.Rows[0]["jyrq"] = Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd");
                dt_update.Rows[0]["testno"] = textBox_tmh.Text;
                //dt_update.Rows[0]["nd"] = Convert.ToDateTime(jyrq_tem).Year .ToString();
                dt_update.Rows[0]["nd"] = DateTime.Now.Year.ToString();
                Form_lisBll form_lisbll = new Form_lisBll();

                //标本与人员的对应关系保存
                form_lisbll.Upd(dt_update, "sql_lis_result_ryxx_update");

                //更新体检人员状态表
                //form_lisbll.Upd(dt_update, "sql077");
                //体检状态信息
                ArrayList TJRYXXList = save_T_JK_TJZT(dt_update.Rows[0]["jyrq"].ToString(), Common.TJTYPE.健康体检表, Common.ZT.确定状态);
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
                    sqlWhere = sqlWhere + string.Format(" and  T_JK_lis_result_re.yq='{0}'", yq_tem);
                }
                //检验日期
                if (UserInfo.Yybm.Length > 0)
                {
                    sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.jyrq='{0}'", jyrq_tem);
                }
                //样本号
                if (UserInfo.Yybm.Length > 0)
                {
                    sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.ybh='{0}'", ybh_tem);
                }

                //取得化验结果
                DataTable dt_dyxm = form_lisbll.GetMoHuList(sqlWhere, "sql045");

                //更新健康体检表
                DataTable dt_tjjgUpdate = new DataTable();
                dt_tjjgUpdate.Rows.Add();

                for (int i = 0; i < dt_dyxm.Rows.Count; i++)
                {

                    if (dt_tjjgUpdate.Columns.Contains(dt_dyxm.Rows[i]["HIS_DB"].ToString()) == false)
                    {
                        DataColumn dtColumn = new DataColumn(dt_dyxm.Rows[i]["HIS_DB"].ToString());
                        dtColumn.DefaultValue = dt_dyxm.Rows[i]["result"].ToString().Replace("正常范围;", "");
                        dt_tjjgUpdate.Columns.Add(dtColumn);
                    }

                }

                if (dt_tjjgUpdate.Columns.Contains("D_GRDABH") == false)
                {
                    DataColumn dtColumn = new DataColumn("D_GRDABH");
                    dtColumn.DefaultValue = textBox_head_JKDAH.Text.ToString();
                    dt_tjjgUpdate.Columns.Add(dtColumn);
                }

                if (dt_tjjgUpdate.Columns.Contains("HAPPENTIME") == false)
                {
                    DataColumn dtColumn = new DataColumn("HAPPENTIME");
                    dtColumn.DefaultValue = Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd");
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
                GuidResult = getNewGuid(out Guid);

                if (dt_tjjgUpdate.Columns.Contains("guid") == false)
                {
                    DataColumn dtColumn = new DataColumn("guid");
                    dtColumn.DefaultValue = Guid;
                    dt_tjjgUpdate.Columns.Add(dtColumn);
                }

                //体检结果
                if (GuidResult == true)
                {
                    //体检结果插入
                    form_lisbll.Add(dt_tjjgUpdate, "sql090");
                }
                else
                {
                    //体检结果更新
                    dt_tjjgUpdate.AcceptChanges();
                    dt_tjjgUpdate.Rows[0]["guid"] = Guid;
                    form_lisbll.Upd(dt_tjjgUpdate, "sql091");
                }

                //签名
                SaveJktjSignname(Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd"), textBox_head_JKDAH.Text.ToString());


                //体检结果保存 到   心电结果(T_JK_xdResult）
                string sqlWhere1 = "";

                //医院编码
                if (UserInfo.Yybm.Length > 0)
                {
                    sqlWhere1 = sqlWhere1 + string.Format(" and yybm='{0}'", UserInfo.Yybm);
                }
                //仪器
                if (UserInfo.Yybm.Length > 0)
                {
                    sqlWhere1 = sqlWhere1 + string.Format(" and  yq='{0}'", yq_tem);
                }
                //检验日期
                if (UserInfo.Yybm.Length > 0)
                {
                    sqlWhere1 = sqlWhere1 + string.Format(" and jyrq='{0}'", jyrq_tem);
                }
                //样本号
                if (UserInfo.Yybm.Length > 0)
                {
                    sqlWhere1 = sqlWhere1 + string.Format(" and ybh='{0}'", ybh_tem);
                }

                //取得测量结果
                DataTable dt_dyxm_re = form_lisbll.GetMoHuList(sqlWhere, "sql046");

                //删除已经保存过的数据
                form_lisbll.DelBySql(string.Format(" and nd='{0}' and YLJGBM='{1}' and jkdabh='{2}'", DateTime.Now.Year.ToString(), UserInfo.Yybm, textBox_head_JKDAH.Text.ToString()), "sql087");

                //心电图测量数据
                DataTable dt_re = new DataTable();

                //医疗机构
                DataColumn YLJGBMColumn = new DataColumn();
                YLJGBMColumn.DefaultValue = UserInfo.Yybm;
                YLJGBMColumn.ColumnName = "YLJGBM";
                dt_re.Columns.Add(YLJGBMColumn);

                //年度
                DataColumn ndColumn = new DataColumn();
                ndColumn.DefaultValue = DateTime.Now.Year.ToString();
                ndColumn.ColumnName = "nd";
                dt_re.Columns.Add(ndColumn);

                //医疗机构
                DataColumn jkdabhColumn = new DataColumn();
                jkdabhColumn.DefaultValue = textBox_head_JKDAH.Text;
                jkdabhColumn.ColumnName = "jkdabh";
                dt_re.Columns.Add(jkdabhColumn);

                dt_re.Columns.Add("rq"); //日期
                dt_re.Columns.Add("sj"); //时间
                dt_re.Columns.Add("wjlx"); //文件类型
                dt_re.Columns.Add("jqxh"); //机器型号
                dt_re.Columns.Add("ajfh"); //安静负荷
                dt_re.Columns.Add("id"); //ID
                dt_re.Columns.Add("xb"); //性别
                dt_re.Columns.Add("nl"); //年龄
                dt_re.Columns.Add("xm"); //姓名
                dt_re.Columns.Add("sg"); //身高
                dt_re.Columns.Add("tz"); //体重
                dt_re.Columns.Add("xy"); //血压
                dt_re.Columns.Add("yy"); //用药
                dt_re.Columns.Add("zz"); //症状
                dt_re.Columns.Add("bz"); //备注
                dt_re.Columns.Add("ys"); //医生
                dt_re.Columns.Add("xl"); //心率(bpm)
                dt_re.Columns.Add("rr"); //R-R(ms)
                dt_re.Columns.Add("pr"); //P-R(ms)
                dt_re.Columns.Add("qrs");//QRS(ms)
                dt_re.Columns.Add("qt"); //QT(ms)
                dt_re.Columns.Add("qtc");//QTC
                dt_re.Columns.Add("deg");//轴(deg)
                dt_re.Columns.Add("rv5");//RV5(mV)
                dt_re.Columns.Add("rv6");//RV6(mV)
                dt_re.Columns.Add("SV1");//SV1(mV)
                dt_re.Columns.Add("RS"); //R+S(mV)
                dt_re.Columns.Add("dj"); //等级
                dt_re.Columns.Add("zddm"); //诊断代码
                dt_re.Columns.Add("zdbm"); //诊断编码（1：正常2：异常）
                dt_re.Columns.Add("mnsdm");//明尼苏达码
                dt_re.Columns.Add("tm"); //条码号

                dt_re.Rows.Add();

                //
                for (int i = 0; i < dt_dyxm_re.Rows.Count; i++)
                {
                    //列名称
                    string ColumnsName = dt_dyxm_re.Rows[i]["xmdh"].ToString().ToLower().Replace(comboBox_yq.SelectedValue.ToString().ToLower() + "_", "");

                    //如果存在 设定对应的值
                    if (dt_re.Columns.Contains(ColumnsName) == true)
                    {
                        dt_re.Rows[dt_re.Rows.Count - 1][ColumnsName] = dt_dyxm_re.Rows[i]["result"].ToString();
                    }
                }

                //添加结果 T_JK_xdResult
                form_lisbll.Add(dt_re, "sql088");

                //刷新列表
                //MessageBox.Show("刷新列表"+DateTime.Now.ToString("HH:mm:ss fff"));


                init_dataGridView_hybb();

                //MessageBox.Show(DateTime.Now.ToString("HH:mm:ss fff"));
                MessageBox.Show("保存成功！");

                if (textBox_ybh.Text.Length > 0)
                {
                    textBox_ybh.Text = (int.Parse(textBox_ybh.Text) + 1).ToString();
                }

                if (selectRyxx(false) == true)
                {
                    initHead("");
                }
                //MessageBox.Show(DateTime.Now.ToString("HH:mm:ss fff"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        /// <summary>
        /// 其他通用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_sys(string  sqlcode)
        {
            DataRowView dr = (DataRowView)dataGridView_hybb.CurrentRow.DataBoundItem;
            if (dr != null)
            {
                if (dr["testno"] != null && dr["testno"].ToString().Length > 0)
                {
                    if (clearDygx(dr) == false)
                    {
                        return;
                    }
                }
            }

            //体检结果
            //sqh 申请号
            if (sqh.Length == 0)
            {
                MessageBox.Show("请输入检验申请号或者扫描检验条码！");
                return;
            }

            try
            {
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
                //申请号
                dt_update.Columns.Add("testno");
                //年度
                dt_update.Columns.Add("nd");

                dt_update.Rows.Add();
                dt_update.AcceptChanges();

                //dt_update.Rows[0]["TJPCH"] = textBox_head_TJPCH.Text;
                //dt_update.Rows[0]["SFH"] = label_sfh.Text;
                dt_update.Rows[0]["yybm"] = UserInfo.Yybm;
                dt_update.Rows[0]["yq"] = yq_tem;
                dt_update.Rows[0]["ybh"] = ybh_tem;
                dt_update.Rows[0]["jyrq"] = Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd");
                dt_update.Rows[0]["testno"] = textBox_tmh.Text;
                //dt_update.Rows[0]["nd"] = Convert.ToDateTime(jyrq_tem).Year .ToString();
                dt_update.Rows[0]["nd"] = DateTime.Now.Year.ToString();
                Form_lisBll form_lisbll = new Form_lisBll();

                //标本与人员的对应关系保存
                form_lisbll.Upd(dt_update, "sql_lis_result_ryxx_update");

                //更新体检人员状态表
                //form_lisbll.Upd(dt_update, "sql077");
                //体检状态信息
                ArrayList TJRYXXList = save_T_JK_TJZT(dt_update.Rows[0]["jyrq"].ToString(), Common.TJTYPE.健康体检表, Common.ZT.确定状态);
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
                    sqlWhere = sqlWhere + string.Format(" and  T_JK_lis_result_re.yq='{0}'", yq_tem);
                }
                //检验日期
                if (UserInfo.Yybm.Length > 0)
                {
                    sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.jyrq='{0}'", jyrq_tem);
                }
                //样本号
                if (UserInfo.Yybm.Length > 0)
                {
                    sqlWhere = sqlWhere + string.Format(" and T_JK_lis_result_re.ybh='{0}'", ybh_tem);
                }

                //取得化验结果
                DataTable dt_dyxm = form_lisbll.GetMoHuList(sqlWhere, "sql045");
                if (dt_dyxm == null || dt_dyxm.Rows.Count == 0) throw new Exception("缺少项目对应关系！请联系系统管理员");

                //更新健康体检表
                DataTable dt_tjjgUpdate = new DataTable();
                dt_tjjgUpdate.Rows.Add();

                for (int i = 0; i < dt_dyxm.Rows.Count; i++)
                {

                    if (dt_tjjgUpdate.Columns.Contains(dt_dyxm.Rows[i]["HIS_DB"].ToString()) == false)
                    {
                        DataColumn dtColumn = new DataColumn(dt_dyxm.Rows[i]["HIS_DB"].ToString());
                        dtColumn.DefaultValue = dt_dyxm.Rows[i]["result"].ToString();
                        dt_tjjgUpdate.Columns.Add(dtColumn);
                    }

                }

                if (dt_tjjgUpdate.Columns.Contains("D_GRDABH") == false)
                {
                    DataColumn dtColumn = new DataColumn("D_GRDABH");
                    dtColumn.DefaultValue = textBox_head_JKDAH.Text.ToString();
                    dt_tjjgUpdate.Columns.Add(dtColumn);
                }

                if (dt_tjjgUpdate.Columns.Contains("HAPPENTIME") == false)
                {
                    DataColumn dtColumn = new DataColumn("HAPPENTIME");
                    dtColumn.DefaultValue = Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd");
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
                GuidResult = getNewGuid(out Guid);

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
                       return ;
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
                SaveJktjSignname(Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd"), textBox_head_JKDAH.Text.ToString());

                init_dataGridView_hybb();

                MessageBox.Show("保存成功！");

                if (textBox_ybh.Text.Length > 0)
                {
                    textBox_ybh.Text = (int.Parse(textBox_ybh.Text) + 1).ToString();
                }

                if (selectRyxx(false) == true)
                {
                    initHead("");
                }
                //MessageBox.Show(DateTime.Now.ToString("HH:mm:ss fff"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// 保存体检结果
        /// </summary>
        /// <returns></returns>
        public bool update_tjjg()
        {
            //化验结果
            DataTable dt_hyjg = new DataTable();

            //对应关系
            DataTable dt_lisdy = new DataTable();

            Form_lisBll form_lisbll = new Form_lisBll();

            //lis项目与体检项目的对应关系(T_JK_LIS_XM）
            dt_lisdy = form_lisbll.GetMoHuList(string.Format(" and YLJGBM='{0}'", UserInfo.Yybm), "sql044");

            return true;
        }

        /// <summary>
        /// 取得Guid
        /// </summary>
        /// <returns></returns>
        private bool getNewGuid(out string guid)
        {
            guid = System.Guid.NewGuid().ToString();
            DBAccess dBAccess = new DBAccess();
            string sql = "";
            ArrayList sqlList = new ArrayList();
            sql = "select guid from T_JK_JKTJ where  d_grdabh='{d_grdabh}' and happentime='{happentime}'";

            //健康档案编号
            sql = sql.Replace("{d_grdabh}", textBox_head_JKDAH.Text);

            //体检日期
            sql = sql.Replace("{happentime}", Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd"));

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
        /// 标本号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_ybh_TextChanged(object sender, EventArgs e)
        {
            try
            {

                //init_hyxm(comboBox_yq.SelectedValue!=null? comboBox_yq.SelectedValue.ToString():"", textBox_ybh.Text, dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd"));
                init_dataGridView_hybb();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 选择标本号后显示标本项目
        /// </summary>
        /// <returns></returns>
        public bool init_hyxm(string yq, string ybh, string jyrq)
        {
            //检索条件
            string strWhere = "";
            strWhere = strWhere + string.Format(" and yq='{0}' ", yq);
            strWhere = strWhere + string.Format(" and ybh='{0}' ", ybh);
            strWhere = strWhere + string.Format(" and jyrq='{0}' ", jyrq);

            //检索
            Form_lisBll form_lisbll = new Form_lisBll();
            DataTable dt_T_JK_lis_result_re = form_lisbll.GetMoHuList(strWhere, "sql046");
            dataGridView_item.DataSource = dt_T_JK_lis_result_re;

            return true;
        }

        /// <summary>
        /// 选择行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_hybb_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                yq_tem = dataGridView_hybb.CurrentRow.Cells["yq"].Value.ToString();
                ybh_tem = dataGridView_hybb.CurrentRow.Cells["ybh"].Value.ToString();
                jyrq_tem = dataGridView_hybb.CurrentRow.Cells["jyrq"].Value.ToString();

                init_hyxm(dataGridView_hybb.CurrentRow.Cells["yq"].Value.ToString(), dataGridView_hybb.CurrentRow.Cells["ybh"].Value.ToString(), dataGridView_hybb.CurrentRow.Cells["jyrq"].Value.ToString());

            }
            catch (Exception ex) { }

        }

        /// <summary>
        /// 鼠标移到控件上时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_hybb_MouseEnter(object sender, EventArgs e)
        {
            if (checkBox_autoYqItem.Checked == true)
            {
                timer_yq.Enabled = false;
            }

        }
        /// <summary>
        /// 鼠标离开控件上时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_hybb_MouseLeave(object sender, EventArgs e)
        {
            if (checkBox_autoYqItem.Checked == true)
            {
                timer_yq.Enabled = true;
            }
        }

        /// <summary>
        /// 修改间隔时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_yqAuto_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    timer_yq.Interval = int.Parse(textBox_yqAuto.Text) * 1000;
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        public void msgShow(string msg)
        {
            MessageBox.Show(msg);
        }

        #region  增加不存在的人员进行体检

        //public bool peopleAdd(out string message)
        //{
        //    message = "";
        //    message = "";
        //    try
        //    {
        //        jktjBll jktjbll = new jktjBll();
        //        DataTable dt_tjry_add = new DataTable();
        //        dt_tjry_add.Columns.Add("YLJGBM");//医疗机构编码
        //        dt_tjry_add.Columns.Add("TJJHBM");//体检计划编码
        //        dt_tjry_add.Columns.Add("TJPCH");//体检批次号
        //        dt_tjry_add.Columns.Add("SFH");//顺番号
        //        dt_tjry_add.Columns.Add("SXHM");//顺序号码
        //        dt_tjry_add.Columns.Add("TJBM");//个人体检编号
        //        dt_tjry_add.Columns.Add("JKDAH");//个人健康档案号
        //        dt_tjry_add.Columns.Add("XM");//姓名
        //        dt_tjry_add.Columns.Add("XB");//性别
        //        dt_tjry_add.Columns.Add("SFZH");//身份证号
        //        dt_tjry_add.Columns.Add("LXDH");//联系电话
        //        dt_tjry_add.Columns.Add("CSRQ");//出生日期
        //        dt_tjry_add.Columns.Add("CZBM");//村庄编码
        //        dt_tjry_add.Columns.Add("FL");//体检人员分类
        //        dt_tjry_add.Columns.Add("BZ");//备注
        //        dt_tjry_add.Columns.Add("TJBH_TEM");//临时个人体检编号
        //        dt_tjry_add.Columns.Add("CREATETIME");//创建时间
        //        dt_tjry_add.Columns.Add("CREATEUSER");//创建者
        //        dt_tjry_add.Columns.Add("UPDATETIME");//更新时间
        //        dt_tjry_add.Columns.Add("UPDATEUSER");//更新者
        //        dt_tjry_add.Columns.Add("SCZT");//数据上传状态
        //        dt_tjry_add.Columns.Add("ZLBZ");//增量标志
        //        dt_tjry_add.Columns.Add("nd");//年度
        //        dt_tjry_add.Columns.Add("ISSH");//是否审核
        //        dt_tjry_add.Columns.Add("ISNEWDOC");//是否新建档案
        //        dt_tjry_add.Rows.Add();

        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJJHBM"] = DateTime.Now.ToString("yyyyMMdd");//体检计划编码
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJPCH"] = DateTime.Now.ToString("HHmmss");//体检批次号

        //        //取得顺番号
        //        DataTable dt_SFH = jktjbll.GetMoHuList("", "sql076");
        //        if (dt_SFH != null && dt_SFH.Rows.Count > 0 && dt_SFH.Rows[0]["SFH"] != null && dt_SFH.Rows[0]["SFH"].ToString().Length > 0)
        //        {
        //            dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SFH"] = int.Parse(dt_SFH.Rows[0]["SFH"].ToString()) + 1;//顺番号
        //        }
        //        else
        //        {
        //            dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SFH"] = "0";//顺番号
        //        }

        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SXHM"] = "0";//顺序号码
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJBM"] = textBox_tmh.Text.Trim();//个人体检编号
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["JKDAH"] = textBox_tmh.Text.Trim();//个人健康档案号
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["XM"] = textBox_tmh.Text.Trim();// DateTime.Now.ToString("HHmmss");//姓名
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["XB"] = "";//性别
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SFZH"] = "**";//身份证号
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["LXDH"] = "**";//联系电话
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CSRQ"] = "";//出生日期
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CZBM"] = "**";//村庄编码
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["FL"] = "2";//体检人员分类
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["BZ"] = "";//备注
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJBH_TEM"] = textBox_tmh.Text.Trim();//临时个人体检编号
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CREATETIME"] = DateTime.Now.ToString();//创建时间
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CREATEUSER"] = UserInfo.userId;//创建者
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["UPDATETIME"] = DateTime.Now.ToString();//更新时间
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["UPDATEUSER"] = UserInfo.userId;//更新者
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SCZT"] = "2";//数据上传状态
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["ZLBZ"] = "1";//增量标志
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString();//年度
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["ISSH"] = "0";//登记
        //        dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["ISNEWDOC"] = "0";//登记

        //        //增加体检人员信息
        //        jktjbll.Add(dt_tjry_add, "sql_add_people");


        //        //体检人员条形码对应表(T_JK_TJRY_TXM）
        //        DataTable t_jk_tjry_txm = new DataTable();
        //        t_jk_tjry_txm.Columns.Add("YLJGBM");//医疗机构编码
        //        t_jk_tjry_txm.Columns.Add("TXMBH");//条形码号码
        //        t_jk_tjry_txm.Columns.Add("TMBM");//条码分类
        //        t_jk_tjry_txm.Columns.Add("JKDAH");//个人健康档案号
        //        t_jk_tjry_txm.Columns.Add("SFZH");//身份证号
        //        t_jk_tjry_txm.Columns.Add("BZ");//备注
        //        t_jk_tjry_txm.Columns.Add("ZLBZ");//增量标志
        //        t_jk_tjry_txm.Columns.Add("nd");//年度
        //        t_jk_tjry_txm.Rows.Add();

        //        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
        //        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["TXMBH"] = textBox_tmh.Text.Trim();//条形码号码
        //        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["TMBM"] = textBox_tmh.Text.Length > 2 ? textBox_tmh.Text.Substring(textBox_tmh.Text.Length - 2, 2) : "";//条码分类
        //        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["JKDAH"] = textBox_tmh.Text.Trim();//个人健康档案号
        //        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["SFZH"] = "**";//身份证号
        //        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["BZ"] = "**";//备注
        //        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["ZLBZ"] = "1";//增量标志
        //        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString();//年度

        //        //增加体检人员条形码对应关系
        //        jktjbll.Add(t_jk_tjry_txm, "sql_add_people_txm");
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.Message;
        //        return false;
        //    }
        //    return true;
        //}


        #endregion


        private bool DeleteJktjSignname(string grdabh)
        {
            if (SignnameGroupList != null && SignnameGroupList.Count > 0)
            {
                TJClient.Signname.Operation.DeleteJktjSignname(grdabh, SignnameGroupList);
            }
            return true;
        }

        private bool DeleteJktjInfo(string grdabh, string tjsj)
        {
            Form_lisBll form_lisbll = new Form_lisBll();
            form_lisbll.DelBySql(string.Format(" and d_grdabh='{0}' and happentime='{1}'", grdabh, tjsj), "sql089_B");
            return true;
        }

        /// <summary>
        /// 清除结果  删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_hybb_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;

            if (grid != null && e.RowIndex >= 0)
            {
                try
                {
                    DataTable dt_tem_re = (DataTable)grid.DataSource;
                    if (grid.Columns[e.ColumnIndex].Name == "clearData")
                    {
                        DialogResult result;
                        result = MessageBox.Show("清除体检人员与化验结果对应关系？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }
                        //更新化验结果表(T_JK_lis_result_re），确定化验结果与人员关系
                        DataTable dt_update = new DataTable();
                        ////体检批次号
                        //dt_update.Columns.Add("TJPCH");
                        ////顺番号
                        //dt_update.Columns.Add("SFH");
                        //医院编码
                        dt_update.Columns.Add("yybm");
                        //仪器编码
                        dt_update.Columns.Add("yq");
                        //样本号
                        dt_update.Columns.Add("ybh");
                        //体检日期
                        dt_update.Columns.Add("jyrq");
                        //检验号
                        dt_update.Columns.Add("testno");
                        //年度
                        dt_update.Columns.Add("nd");

                        dt_update.Rows.Add();
                        dt_update.AcceptChanges();

                        dt_update.Rows[0]["testno"] = "";
                        dt_update.Rows[0]["nd"] = "";
                        dt_update.Rows[0]["yybm"] = UserInfo.Yybm;
                        dt_update.Rows[0]["yq"] = yq_tem;
                        dt_update.Rows[0]["ybh"] = ybh_tem;
                        dt_update.Rows[0]["jyrq"] = dt_tem_re.Rows[e.RowIndex]["jyrq"].ToString();

                        Form_lisBll form_lisbll = new Form_lisBll();

                        //标本与人员的对应关系保存
                        form_lisbll.Upd(dt_update, "sql_lis_result_clear");

                        //更新体检人员状态表
                        //form_lisbll.Upd(dt_update, "sql093");

                        //更新体检人员状态表
                        ArrayList TJRYXXList = delete_T_JK_TJZT(dt_tem_re.Rows[e.RowIndex]["jyrq"].ToString(), Common.TJTYPE.健康体检表, Common.ZT.确定状态, dt_tem_re.Rows[e.RowIndex]["jkdah"].ToString());
                        if (TJRYXXList != null && TJRYXXList.Count > 0)
                        {
                            DBAccess dbaccess = new DBAccess();
                            dbaccess.ExecuteNonQueryBySql(TJRYXXList);
                        }
                        //LIS
                        if (YQ_Type.Equals("1"))
                        {
                            //删除lis_reqresult表中的数据
                            form_lisbll.DelBySql(string.Format(" and  brdh='{0}'", dt_tem_re.Rows[e.RowIndex]["jkdah"].ToString()), "sql096");
                        }
                        else if (YQ_Type.Equals("0")) //心电
                        {
                            //删除心电图的数据
                            form_lisbll.DelBySql(string.Format(" and  yljgbm='{0}' and jkdabh='{1}'", UserInfo.Yybm, dt_tem_re.Rows[e.RowIndex]["jkdah"].ToString()), "sql087");
                        }


                        //根据健康档案号，体检时间删除健康档案中的数据
                        DeleteJktjInfo(dt_tem_re.Rows[e.RowIndex]["jkdah"].ToString(),dt_tem_re.Rows[e.RowIndex]["jyrq"].ToString());

                        //删除已经对应的签名信息
                        DeleteJktjSignname(dt_tem_re.Rows[e.RowIndex]["jkdah"].ToString());

                        MessageBox.Show("成功！");
                        //检索
                        init_dataGridView_hybb();

                    }
                    else if (grid.Columns[e.ColumnIndex].Name == "deleteData")
                    {
                        DialogResult result;
                        result = MessageBox.Show("删除化验结果？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }

                        //更新化验结果表(T_JK_lis_result_re），确定化验结果与人员关系
                        DataTable dt_update = new DataTable();
                        ////体检批次号
                        //dt_update.Columns.Add("TJPCH");
                        ////顺番号
                        //dt_update.Columns.Add("SFH");
                        //医院编码
                        dt_update.Columns.Add("yybm");
                        //仪器编码
                        dt_update.Columns.Add("yq");
                        //样本号
                        dt_update.Columns.Add("ybh");
                        //体检日期
                        dt_update.Columns.Add("jyrq");
                        //检验号
                        dt_update.Columns.Add("testno");
                        //年度
                        dt_update.Columns.Add("nd");
                        dt_update.Rows.Add();
                        dt_update.AcceptChanges();

                        dt_update.Rows[0]["testno"] = "";
                        dt_update.Rows[0]["nd"] = "";
                        dt_update.Rows[0]["yybm"] = UserInfo.Yybm;
                        dt_update.Rows[0]["yq"] = yq_tem;
                        dt_update.Rows[0]["ybh"] = ybh_tem;
                        dt_update.Rows[0]["jyrq"] = dt_tem_re.Rows[e.RowIndex]["jyrq"].ToString();

                        Form_lisBll form_lisbll = new Form_lisBll();

                        //删除记录
                        form_lisbll.Upd(dt_update, "sql094");


                        //更新体检人员状态表
                        //form_lisbll.Upd(dt_update, "sql093");

                        //更新体检人员状态表
                        ArrayList TJRYXXList = delete_T_JK_TJZT(dt_tem_re.Rows[e.RowIndex]["jyrq"].ToString(), Common.TJTYPE.健康体检表, Common.ZT.确定状态, dt_tem_re.Rows[e.RowIndex]["jkdah"].ToString());
                        if (TJRYXXList != null && TJRYXXList.Count > 0)
                        {
                            DBAccess dbaccess = new DBAccess();
                            dbaccess.ExecuteNonQueryBySql(TJRYXXList);
                        }


                        //删除lis_reqresult表中的数据
                        form_lisbll.DelBySql(string.Format(" and  brdh='{0}'", dt_tem_re.Rows[e.RowIndex]["jkdah"].ToString()), "sql096");

                        //根据健康档案号，体检时间删除健康档案中的数据
                        DeleteJktjInfo(dt_tem_re.Rows[e.RowIndex]["jkdah"].ToString(), dt_tem_re.Rows[e.RowIndex]["jyrq"].ToString());
                        
                        //删除已经对应的签名信息
                        DeleteJktjSignname(dt_tem_re.Rows[e.RowIndex]["jkdah"].ToString());

                        MessageBox.Show("成功！");
                        //检索
                        init_dataGridView_hybb();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("失败！" + ex.Message);
                }
            }
        }
        /// <summary>
        /// 行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_hybb_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_hybb.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView_hybb.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView_hybb.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private GraphicsPath GetGraphicsPath(Rectangle rc, int r)
        {
            int x = rc.X, y = rc.Y, w = rc.Width, h = rc.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, r, r, 180, 90);                //
            path.AddArc(x + w - r, y, r, r, 270, 90);            //
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90);        //
            path.AddArc(x, y + h - r, r, r, 90, 90);            //
            path.CloseFigure();
            return path;
        }

        private void panelContaioner_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rc = new Rectangle(0 + 2, 0 + 2, this.Width - 10, this.Height - 5);
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush, 1);
            g.DrawPath(pen, this.GetGraphicsPath(rc, 20));
        }

        /// <summary>
        /// 行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_item_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_item.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView_item.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView_item.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 检索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click_1(object sender, EventArgs e)
        {
            if (selectRyxx(false) == true)
            {
                initHead("");
            }
        }

        /// <summary>
        /// 申请号，点击是显示对应的条形码信息可以进行选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_sqh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        /// <summary>
        /// 标本号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_ybh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    //init_hyxm(comboBox_yq.SelectedValue!=null? comboBox_yq.SelectedValue.ToString():"", textBox_ybh.Text, dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd"));
                    init_dataGridView_hybb();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
            if (checkBox_ytj.Checked == true)
            {
                checkBox_wtj.Checked = false;
            }
        }

        /// <summary>
        /// 已对应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_All_ydy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_All_ydy.Checked == true)
            {
                checkBox_All_wdy.Checked = false;
            }
        }

        /// <summary>
        /// 未对应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_All_wdy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_All_wdy.Checked == true)
            {
                checkBox_All_ydy.Checked = false;
            }
        }

        /// <summary>
        /// 检验类型改变时，人员类表初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_lisType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //页面出事化时不进行检索操作
            if (isDoInit == true)
            {
                //if (selectRyxx() == true)
                //{
                //    initHead("");
                //}
            }
        }

        /// <summary>
        /// 左侧人员列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_rylb_Click(object sender, EventArgs e)
        {
            if (panel_body_left.Width == 20)
            {
                panel_body_left.Width = 138;
                button_rylb.Visible = true;
                listBox_ryxx.Visible = true;
                button_right.BackgroundImage = global::TJClient.Properties.Resources.btn_right;
            }
            else
            {
                panel_body_left.Width = 20;
                button_rylb.Visible = false;
                listBox_ryxx.Visible = false;
                button_right.BackgroundImage = global::TJClient.Properties.Resources.btn_left;
            }
        }

        /// <summary>
        /// 检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_jc_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_jc.Checked == true)
            {
                checkBox_All_wdy.Checked = false;
                checkBox_All_wdy.Enabled = false;
                checkBox_All_ydy.Checked = true;

            }
            else
            {
                checkBox_All_wdy.Enabled = true ;
            }
        }

        /// <summary>
        /// 发送申请到心电设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_medex_Click(object sender, EventArgs e)
        {
            MedexList list = new MedexList();
            list.xdtype = xdtype;
            list.ShowDialog();
        }

    }
}
