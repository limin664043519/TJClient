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
    public partial class LnrZytz : sysCommonForm
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

        /// <summary>
        /// 前页面传过来的参数
        /// </summary>
        DataTable dt_para_sys = null;

        /// <summary>
        /// 前一页面传过来的信息
        /// </summary>
        DataTable dt_paraFromParent = null;

        /// <summary>
        /// 保存父窗体
        /// </summary>
        public Main_Form main_form = null;
        #endregion

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
            
        }

        #region 窗体事件

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

        public LnrZytz()
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
           // DataTable dt = (DataTable)((Login)this.Owner).Tag;
            //用户id
            userId = dt_para_sys.Rows[0]["userId"].ToString();
            //工作组
            yhfz = dt_para_sys.Rows[0]["gzz"].ToString();
            //医疗机构
            yljg = dt_para_sys.Rows[0]["yljg"].ToString();
            DBAccess dBAccess = new DBAccess();

            //中医体质辨识的问题
            getWtList();

            initWt();

        }
        #endregion

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
                   // text_fzlr(textbox_tem);
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

            //焦点离开时验证内容
            //checkList(control.Name, control);
        }

        /// <summary>
        /// 体检人员信息表(T_JK_TJRYXX）保存
        /// </summary>
        private ArrayList save_T_JK_TJRYXX(DataTable dt_para, string tisj, string TJTYPE, string zt)
        {
            ArrayList returnArrayList = new ArrayList();

            //删除用sql
            string SqlDele = Common.getSql("sql801", " ");

            //追加记录用sql
            string SqlAdd = Common.getSql("sql802", "");

            //删除用sql
            //医疗机构编码
            SqlDele = SqlDele.Replace("{YLJGBM}", getValueFromDt(dt_para, 0, "YLJGBM"));
            //健康档案号
            SqlDele = SqlDele.Replace("{JKDAH}", getValueFromDt(dt_para, 0, "JKDAH"));
            //年度
            SqlDele = SqlDele.Replace("{ND}", getValueFromDt(dt_para, 0, "ND"));
            //工作组编码
            SqlDele = SqlDele.Replace("{GZZBM}", getValueFromDt(dt_para, 0, "gzz"));
            //体检时间
            SqlDele = SqlDele.Replace("{TJSJ}", getValueFromDt(dt_para, 0, "TJSJ"));
            //操作员
            SqlDele = SqlDele.Replace("{CZY}", UserInfo.userId);
            //文档类型
            SqlDele = SqlDele.Replace("{TJTYPE}", TJTYPE);

            returnArrayList.Add(SqlDele);

            //追加用sql  ( '{YLJGBM}','{JKDAH}','{XM}','{SFZH}','{ND}','{GZZBM}','{TJSJ}','{CZY}','{TJTYPE}','{ZT}')
            //医疗机构编码
            SqlAdd = SqlAdd.Replace("{YLJGBM}", getValueFromDt(dt_para, 0, "YLJGBM"));
            //健康档案号
            SqlAdd = SqlAdd.Replace("{JKDAH}", getValueFromDt(dt_para, 0, "JKDAH"));
            //姓名
            SqlAdd = SqlAdd.Replace("{XM}", getValueFromDt(dt_para, 0, "XM"));
            //身份证号
            SqlAdd = SqlAdd.Replace("{SFZH}", getValueFromDt(dt_para, 0, "SFZH"));
            //年度
            SqlAdd = SqlAdd.Replace("{ND}", getValueFromDt(dt_para, 0, "ND"));
            //工作组编码
            SqlAdd = SqlAdd.Replace("{GZZBM}", getValueFromDt(dt_para, 0, "gzz"));
            //体检时间
            SqlAdd = SqlAdd.Replace("{TJSJ}", tisj);
            //操作员
            SqlAdd = SqlAdd.Replace("{CZY}", UserInfo.userId);
            //文档类型
            SqlAdd = SqlAdd.Replace("{TJTYPE}", TJTYPE);
            //体检状态
            SqlAdd = SqlAdd.Replace("{ZT}", zt);

            //体检医生
            SqlAdd = SqlAdd.Replace("{Tjys}", CommomSysInfo .TJFZR_BM );



            returnArrayList.Add(SqlAdd);

            return returnArrayList;

        }

        #region 公用方法
        /// <summary>
        /// 取得数据字典中的项目
        /// </summary>
        /// <param name="Sjzdbm"></param>
        /// <returns></returns>
        private DataTable getSjzdList(string Sjzdbm)
        {
            //获取该工作组对应的体检项目
            string sql = @"select * from  T_JK_SJZD where ZDLXBM='{ZDLXBM}' order by orderby ";

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
                control.ForeColor = Color.Black;
                //text
                if (ControlId.IndexOf(controlType_textBox) > -1)
                {
                    TextBox TextBox_tem = (TextBox)control;
                    TextBox_tem.Text = ControlValue;
                }
                //checkBox
                else if (ControlId.IndexOf(controlType_checkBox) > -1)
                {
                    CheckBox checkBox_tem = (CheckBox)control;

                    if (ControlValue.Trim().Length > 0 && checkBox_tem.Tag != null && (","+ControlValue.Trim().ToString()+",").IndexOf ( ","+checkBox_tem.Tag .ToString ().ToLower ()+",")>-1)
                    {
                        checkBox_tem.Checked = true;
                    }
                    else
                    {
                        checkBox_tem.Checked = false;
                    }
                }
                //radioButton
                else if (ControlId.IndexOf(controlType_radioButton) > -1)
                {
                    RadioButton radioButton_tem = (RadioButton)control;
                    if (ControlValue.Trim().Length > 0 && radioButton_tem.Tag != null && radioButton_tem.Tag.ToString().ToLower().Equals(ControlValue.Trim().ToLower()))
                    {
                        radioButton_tem.Checked = true;
                    }
                    else
                    {
                        radioButton_tem.Checked = false;
                    }
                }
                //lable
                else if (ControlId.IndexOf(controlType_lable) > -1)
                {
                    Label Label_tem = (Label)control;
                    Label_tem.Text = ControlValue;
                }
                //linklable
                else if (ControlId.IndexOf(controlType_LinkLabel) > -1)
                {
                    LinkLabel LinkLabel_tem = (LinkLabel)control;
                    LinkLabel_tem.Tag = ControlValue;
                    //LinkLabel_tem.Text = ControlValue;
                }
                else if (ControlId.IndexOf(  controlType_dataGridView ) > -1)
                {
                    //datagridview_bool = false;
                    DataGridView DataGridView_tem = (DataGridView)control;
                    if (DataGridView_tem.DataSource != null)
                    {
                        DataTable dt = (DataTable)DataGridView_tem.DataSource;
                        DataGridView_tem.DataSource = dt.Clone();
                    }
                    //datagridview_bool = true;
                }
            }
            catch (Exception ex)
            {
            }
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
        /// 按条件从数据库中取得体检项目的结果取得体检项目的结果
        /// </summary>
        /// <returns></returns>
        private DataTable getTjResultDtFromDb(string sqlWhere)
        {
            string sql = "";

            //没有取得项目
            if (dtResult == null || dtResult.Rows.Count <= 0)
            {
                return null;
            }
            //生成获取当前项目的sql
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                if (!(dtResult.Rows[i]["KJLX"].ToString().Trim().ToLower().Equals("datagridview") || dtResult.Rows[i]["KJLX"].ToString().Trim().ToLower().Equals("tab")))
                {
                    sql = sql + dtResult.Rows[i]["HIS_DB"].ToString() + ",";
                }

            }

            sql = "select " + sql.Substring(0, sql.Length - 1) + " from  T_JK_JKTJ where 1=1  " + sqlWhere;

            //获取数据
            DBAccess dBAccess = new DBAccess();
            DataTable dt = dBAccess.ExecuteQueryBySql(sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dtControl.Rows.Count; i++)
                {
                    //textbox
                    if (dtControl.Rows[i]["ControlId"].ToString().ToLower().IndexOf(controlType_textBox.ToLower()) > -1)
                    {
                        //ControlId
                        string[] textvalue = null;
                        string[] textcontrol = null;
                        textcontrol = dtControl.Rows[i]["kjid"].ToString().Split(new char[] { ',' });
                        if (textcontrol.Length > 1)
                        {
                            if (dt.Rows[0][dtControl.Rows[i]["HIS_DB"].ToString()].ToString().IndexOf(",") > -1)
                            {
                                textvalue = dt.Rows[0][dtControl.Rows[i]["HIS_DB"].ToString()].ToString().Split(new char[] { ',' });

                                for (int ii = 0; ii < textcontrol.Length; ii++)
                                {
                                    //找到对应的值
                                    if ((controlType_textBox + textcontrol[ii]).Equals(dtControl.Rows[i]["ControlId"].ToString()))
                                    {
                                        if (ii < textvalue.Length)
                                        {
                                            dtControl.Rows[i]["value"] = textvalue[ii];
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dtControl.Rows[i]["value"] = dt.Rows[0][dtControl.Rows[i]["HIS_DB"].ToString()].ToString();
                            }

                        }
                        else
                        {
                            dtControl.Rows[i]["value"] = dt.Rows[0][dtControl.Rows[i]["HIS_DB"].ToString()].ToString();
                        }

                        continue;
                    }

                    //checkBox
                    else if (dtControl.Rows[i]["ControlId"].ToString().ToLower().IndexOf(controlType_checkBox.ToLower()) > -1)
                    {
                        if (("," + dt.Rows[0][dtControl.Rows[i]["HIS_DB"].ToString()].ToString() + ",").IndexOf("," + dtControl.Rows[i]["tag"].ToString() + ",") > -1)
                        {
                            dtControl.Rows[i]["value"] = dtControl.Rows[i]["tag"].ToString();
                            continue;
                        }
                        else
                        {
                            dtControl.Rows[i]["value"] = "";
                        }

                    }
                    //radioButton
                    else if (dtControl.Rows[i]["ControlId"].ToString().ToLower().IndexOf(controlType_radioButton.ToLower()) > -1)
                    {
                        if (dt.Rows[0][dtControl.Rows[i]["HIS_DB"].ToString()].ToString().IndexOf(dtControl.Rows[i]["tag"].ToString()) > -1)
                        {
                            dtControl.Rows[i]["value"] = dtControl.Rows[i]["tag"].ToString();
                            continue;
                        }
                        else
                        {
                            dtControl.Rows[i]["value"] = ""; 
                        }

                    }
                    //linklabel
                    else if (dtControl.Rows[i]["ControlId"].ToString().ToLower().IndexOf(controlType_LinkLabel.ToLower()) > -1)
                    {

                        dtControl.Rows[i]["value"] = dt.Rows[0][dtControl.Rows[i]["HIS_DB"].ToString()].ToString();
                    }
                    else
                    {
                        dtControl.Rows[i]["value"] = "";
                    }
                }
            }
            else
            {
                //setPageClear();
            }
            return dt;
        }

        /// <summary>
        /// 将数据库中检索到的数据设定到页面中
        /// </summary>
        private void setTjResultToPage()
        {
            if (dtControl == null)
            {
                return;
            }

            //取得控件的值
            for (int i = 0; i < dtControl.Rows.Count; i++)
            {
                setValueToControl(dtControl.Rows[i]["ControlId"].ToString(), dtControl.Rows[i]["value"].ToString());

                //dataGridView
                //setValueToControl_dataGridView(dtControl.Rows[i]["ControlId"].ToString());
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

        #endregion

        #region 中医体质辨识

        #region  问题区的处理
        //点击链接时会触发单选按钮的CheckedChanged事件，但是此时不应该出发，
        //该标志为1时CheckedChanged事件中的处理执行，0时CheckedChanged事件中的处理不执行
        public int linkLabeFlag = 1;

        /// <summary>
        /// 取得中医体质辨识的问题列表
        /// </summary>
        public void getWtList()
        {
            //中医体质辨识的问题
            DBAccess dBAccess = new DBAccess();
            string zytzbs_wt = @"select * from T_JK_TJXM where [type]='3' order by rowNo,ORDERBY ";
            dt_wt_zytz = dBAccess.ExecuteQueryBySql(zytzbs_wt);
        }

        /// <summary>
        /// 初始化页面问题
        /// </summary>
        public void initWt()
        {
            //设定页面问题初始化
            if (dt_wt_zytz.Rows.Count > 0)
            {
                dt_da_zytz = new DataTable();
                //初始化存放答案的容器
                dt_da_zytz.Columns.Add("da");
                dt_da_zytz.Columns.Add("da1");
                for (int i = 0; i < 33; i++)
                {
                    dt_da_zytz.Rows.Add();
                    dt_da_zytz.Rows[i]["da"] = 1;
                    dt_da_zytz.Rows[i]["da1"] = 1;
                    //label_01_zytz
                    setValueToControl((controlType_lable + (i + 1).ToString().PadLeft(2, '0') + "_zytz"), "1");
                }
                Wt_set_zytz(1, "");
            }
        }


        /// <summary>
        /// 添加控件radioButton
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="dtrow"></param>
        /// <returns></returns>
        private void AddControl_radioButton_zytz(Panel parentPanel, DataRow dtrow, string value)
        {
            panel_da_zytz.Controls.Clear();
            //获取该工作组对应的体检项目
            Point Location_tem = new Point(0, 10);
            DataTable checkList = getSjzdList(dtrow["SJZDBM"].ToString());
            RadioButton radioButton_tem = new RadioButton();
            //Boolean ischecked = false;
            int d_x_tem = 0;
            int d_y_tem = 0;
            int d_x = 5;
            int d_y = 0;
            //int d_weight = 5;
            //int d_height = 5;
            //int panelWidth = 0;

            d_x_tem = Location_tem.X;
            d_y_tem = Location_tem.Y;
            d_x = d_x_tem;
            d_y = d_y_tem;

            if (checkList != null)
            {
                //添加radioButton控件
                for (int i = 0; i < checkList.Rows.Count; i++)
                {
                    Location_tem.X = d_x;
                    Location_tem.Y = d_y;

                    radioButton_tem = createRadioButton_zytz(dtrow, checkList.Rows[i]["ZDBM"].ToString(), checkList.Rows[i]["ZDMC"].ToString(), Location_tem, value);
                    panel_da_zytz.Controls.Add(radioButton_tem);
                    d_x = d_x + radioButton_tem.Width + 20;
                }

            }
        }


        /// <summary>
        /// 创建单选按钮
        /// </summary>
        /// <param name="dtrow"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private RadioButton createRadioButton_zytz(DataRow dtrow, string value, string text, Point location, string value_S)
        {
            TabIndex = TabIndex + 1;
            RadioButton radioButton_tem = new RadioButton();
            radioButton_tem.Name = controlType_radioButton + dtrow["KJID"].ToString() + "_" + value;
            radioButton_tem.Tag = value;
            radioButton_tem.Text = text;
            radioButton_tem.Location = location;
            radioButton_tem.CheckedChanged += radioButton_CheckedChanged_zytz;
            radioButton_tem.KeyDown += Enter_Tab_KeyDown;
            radioButton_tem.Enter += Control_Enter;
            radioButton_tem.Leave += Control_Leave;
            radioButton_tem.TabIndex = TabIndex;
            if (value_S == value)
            {
                radioButton_tem.Checked = true;
            }
            else
            {
                radioButton_tem.Checked = false;
            }
            radioButton_tem.AutoSize = true;
            return radioButton_tem;
        }

        /// <summary>
        /// 跳转到指定的问题
        /// </summary>
        /// <param name="index">行号</param>
        private void Wt_set_zytz(int index, string value)
        {
            //设定页面问题初始化
            if (dt_wt_zytz.Rows.Count > 0 && index - 1 < dt_wt_zytz.Rows.Count)
            {
                //设定问题
                label_wt_zytz.Text = dt_wt_zytz.Rows[index - 1]["KJXSMC"].ToString();
                label_wt_zytz.Tag = index.ToString();
                //设定答案
                AddControl_radioButton_zytz(panel_da_zytz, dt_wt_zytz.Rows[index - 1], value);
            }
        }

        /// <summary>
        /// radioButton改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_CheckedChanged_zytz(object sender, EventArgs e)
        {
            if (linkLabeFlag == 0)
            {
                linkLabeFlag = 1;
                //return;
            }
            else
            {
                RadioButton RadioButton_tem = (RadioButton)sender;
                if (RadioButton_tem.Checked)
                {
                    //设定选项的分数
                    setValueToControl((controlType_lable + label_wt_zytz.Tag.ToString().PadLeft(2, '0') + "_zytz"), RadioButton_tem.Tag.ToString());
                    dt_da_zytz.Rows[int.Parse(label_wt_zytz.Tag.ToString()) - 1]["da"] = RadioButton_tem.Tag.ToString();
                    dt_da_zytz.Rows[int.Parse(label_wt_zytz.Tag.ToString()) - 1]["da1"] = (6 - int.Parse(RadioButton_tem.Tag.ToString())).ToString();
                    //跳转到下一题
                    int index = int.Parse(label_wt_zytz.Tag.ToString()) + 1;
                    Wt_set_zytz(index, "");
                }
            }
            calculate_zytz();
        }

        /// <summary>
        /// 答案区域的链接事件linkLabel事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_Click(object sender, EventArgs e)
        {

            int index = int.Parse(((LinkLabel)sender).Tag.ToString());
            string value = GetControlValueByName(controlType_lable + index.ToString().PadLeft(2, '0') + "_zytz");

            if (value.Length > 0)
            {
                linkLabeFlag = 0;
            }
            else
            {
                linkLabeFlag = 1;
            }
            Wt_set_zytz(index, value);
        }

        /// <summary>
        /// 中医体质中计算分数
        /// </summary>
        private void calculate_zytz()
        {
            //气虚质得分
            string L_QX_DF = "{2}+{3}+{4}+{14}";

            //阳虚质得分	
            string L_YANG_DF = "{11}+{12}+{13}+{29}";
            //阴虚质得分	
            string L_YIN_DF = "{10}+{21}+{26}+{31}";
            //痰湿质得分
            string L_TS_DF = "{9}+{16}+{28}+{32}";
            //湿热质得分	
            string L_SR_DF = "{23}+{25}+{27}+{30}";
            //血瘀质得分	
            string L_XY_DF = "{19}+{22}+{24}+{33}";
            //气郁质得分	
            string L_QY_DF = "{5}+{6}+{7}+{8}";
            //特禀质得分	
            string L_TB_DF = "{15}+{17}+{18}+{20}";
            //平和质得分	
            string L_PH_DF = "{1}+{2a}+{4a}+{5a}+{13a}";


            //生成存放得分的容器
            if (dt_df_zytz.Columns.Count == 0)
            {
                //气虚质得分
                dt_df_zytz.Columns.Add("L_QX_DF");
                //阳虚质得分
                dt_df_zytz.Columns.Add("L_YANG_DF");
                //阴虚质得分
                dt_df_zytz.Columns.Add("L_YIN_DF");
                //痰湿质得分
                dt_df_zytz.Columns.Add("L_TS_DF");
                //湿热质得分
                dt_df_zytz.Columns.Add("L_SR_DF");
                //血瘀质得分
                dt_df_zytz.Columns.Add("L_XY_DF");
                //气郁质得分
                dt_df_zytz.Columns.Add("L_QY_DF");
                //特禀质得分
                dt_df_zytz.Columns.Add("L_TB_DF");
                //平和质得分
                dt_df_zytz.Columns.Add("L_PH_DF");
                dt_df_zytz.Rows.Add();
                dt_df_zytz.Rows.Add();

            }

            //计算得分
            //MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
            //sc.Language = "JavaScript";

            ////气虚质得分
            //dt_df_zytz.Rows[0]["L_QX_DF"] = sc.Eval(getcalculate_zytz(L_QX_DF)).ToString();
            ////阳虚质得分	
            //dt_df_zytz.Rows[0]["L_YANG_DF"] = sc.Eval(getcalculate_zytz(L_YANG_DF)).ToString();
            ////阴虚质得分	
            //dt_df_zytz.Rows[0]["L_YIN_DF"] = sc.Eval(getcalculate_zytz(L_YIN_DF)).ToString();
            ////痰湿质得分
            //dt_df_zytz.Rows[0]["L_TS_DF"] = sc.Eval(getcalculate_zytz(L_TS_DF)).ToString();
            ////湿热质得分	
            //dt_df_zytz.Rows[0]["L_SR_DF"] = sc.Eval(getcalculate_zytz(L_SR_DF)).ToString();
            ////血瘀质得分	
            //dt_df_zytz.Rows[0]["L_XY_DF"] = sc.Eval(getcalculate_zytz(L_XY_DF)).ToString();
            ////气郁质得分	
            //dt_df_zytz.Rows[0]["L_QY_DF"] = sc.Eval(getcalculate_zytz(L_QY_DF)).ToString();
            ////特禀质得分	
            //dt_df_zytz.Rows[0]["L_TB_DF"] = sc.Eval(getcalculate_zytz(L_TB_DF)).ToString();
            ////平和质得分	
            //dt_df_zytz.Rows[0]["L_PH_DF"] = sc.Eval(getcalculate_zytz(L_PH_DF)).ToString();

            //气虚质得分
            dt_df_zytz.Rows[0]["L_QX_DF"] = Eval(getcalculate_zytz(L_QX_DF)).ToString();
            //阳虚质得分	
            dt_df_zytz.Rows[0]["L_YANG_DF"] = Eval(getcalculate_zytz(L_YANG_DF)).ToString();
            //阴虚质得分	
            dt_df_zytz.Rows[0]["L_YIN_DF"] = Eval(getcalculate_zytz(L_YIN_DF)).ToString();
            //痰湿质得分
            dt_df_zytz.Rows[0]["L_TS_DF"] = Eval(getcalculate_zytz(L_TS_DF)).ToString();
            //湿热质得分	
            dt_df_zytz.Rows[0]["L_SR_DF"] = Eval(getcalculate_zytz(L_SR_DF)).ToString();
            //血瘀质得分	
            dt_df_zytz.Rows[0]["L_XY_DF"] = Eval(getcalculate_zytz(L_XY_DF)).ToString();
            //气郁质得分	
            dt_df_zytz.Rows[0]["L_QY_DF"] = Eval(getcalculate_zytz(L_QY_DF)).ToString();
            //特禀质得分	
            dt_df_zytz.Rows[0]["L_TB_DF"] = Eval(getcalculate_zytz(L_TB_DF)).ToString();
            //平和质得分	
            dt_df_zytz.Rows[0]["L_PH_DF"] = Eval(getcalculate_zytz(L_PH_DF)).ToString();

            //气虚质得分~特禀质得分
            string tizhi_pd = "if({0}>=11){ 1 }else if({0}>=9 && {0}<=10){ 2 }else if({0}<=8){ 3 }";
            //平和质得分
            string HPZ_pd = "if({0}>=17 && ({1}<=8 && {2}<=8 &&{3}<=8 &&{4}<=8 &&{5}<=8 &&{6}<=8 &&{7}<=8 &&{8}<=8 )){ 1 }else if({0}>=17 && ({1}<=10 && {2}<=10 &&{3}<=10 &&{4}<=10 &&{5}<=10 &&{6}<=10 &&{7}<=10 &&{8}<=10 )){ 2 }else{ 3 }";

            //页面上的表示的设定
            for (int i = 0; i < dt_df_zytz.Columns.Count; i++)
            {
                //和平质
                if (dt_df_zytz.Columns[i].ColumnName.Equals("L_PH_DF"))
                {
                    setTZtoPage(dt_df_zytz.Columns[i].ColumnName, dt_df_zytz.Rows[0][i].ToString());
                }
                else
                {
                    setTZtoPage(dt_df_zytz.Columns[i].ColumnName, dt_df_zytz.Rows[0][i].ToString());
                }
            }

        }

        /// <summary>
        /// 按照公示计算分数
        /// </summary>
        /// <param name="calculate"></param>
        /// <returns></returns>
        public string Eval(string calculate)
        {
            int score = 0;
            if (calculate.Length > 0)
            {
                string[] calculateList = calculate.Split(new char[] { '+' });
                if (calculateList.Length > 0)
                {

                    for (int i = 0; i < calculateList.Length; i++)
                    {
                        score = score + Convert.ToInt32(calculateList[i].Trim());
                    }
                }
            }
            return score.ToString();
        }

        /// <summary>
        /// 得到计算分数的公式
        /// </summary>
        /// <param name="calculate"></param>
        /// <returns></returns>
        private string getcalculate_zytz(string calculate)
        {
            for (int i = 0; i < dt_da_zytz.Rows.Count; i++)
            {
                if (dt_da_zytz.Rows[i]["da"] != null && dt_da_zytz.Rows[i]["da"].ToString().Length > 0)
                {
                    calculate = calculate.Replace("{" + (i + 1).ToString() + "}", dt_da_zytz.Rows[i]["da"].ToString()).Replace("{" + (i + 1).ToString() + "a" + "}", dt_da_zytz.Rows[i]["da1"].ToString());
                }
                else
                {
                    calculate = calculate.Replace("{" + (i + 1).ToString() + "}", "0").Replace("{" + (i + 1).ToString() + "a" + "}", "0");
                }
            }
            return calculate;
        }

        /// <summary>
        /// 判定体质并对页面赋值
        /// </summary>
        /// <returns></returns>
        private void setTZtoPage(string tz, string TZ_DF)
        {
            try
            {
                ////气虚质得分~特禀质得分
                //string tizhi_pd = "if({0}>=11){ 1 }else if({0}>=9 && {0}<=10){ 2 }else if({0}<=8){ 3 }";

                //体质状态
                string flag = "0";

                //分数
                int score = 0;
                score = Convert.ToInt32(TZ_DF);

                //和平质
                if (tz.Equals("L_PH_DF"))
                {
                    if (Convert.ToInt32(dt_df_zytz.Rows[0]["L_PH_DF"].ToString()) >= 17    //平和质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_QX_DF"].ToString()) < 8   //气虚质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_YANG_DF"].ToString()) < 8 //阳虚质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_YIN_DF"].ToString()) < 8  //阴虚质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_TS_DF"].ToString()) < 8   //痰湿质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_SR_DF"].ToString()) < 8   //湿热质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_XY_DF"].ToString()) < 8   //血瘀质得分	
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_QY_DF"].ToString()) < 8   //气郁质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_TB_DF"].ToString()) < 8   //特禀质得分
                        )
                    {
                        flag = "1";
                    }
                    else if (Convert.ToInt32(dt_df_zytz.Rows[0]["L_PH_DF"].ToString()) >= 17    //平和质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_QX_DF"].ToString()) < 10   //气虚质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_YANG_DF"].ToString()) < 10 //阳虚质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_YIN_DF"].ToString()) < 10  //阴虚质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_TS_DF"].ToString()) < 10   //痰湿质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_SR_DF"].ToString()) < 10   //湿热质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_XY_DF"].ToString()) < 10   //血瘀质得分	
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_QY_DF"].ToString()) < 10   //气郁质得分
                        && Convert.ToInt32(dt_df_zytz.Rows[0]["L_TB_DF"].ToString()) < 10   //特禀质得分
                        )
                    {
                        flag = "2";
                    }
                    else
                    {
                        flag = "3";
                    }
                }
                else
                {
                    if (score >= 11)
                    {
                        flag = "1";
                    }
                    else if (score <= 10 && score >= 9)
                    {
                        flag = "2";
                    }
                    else if (score <= 8)
                    {
                        flag = "3";
                    }

                }


                ////计算
                //MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
                //sc.Language = "JavaScript";
                //int j = 1;
                //for (int i = 0; i < dt_df_zytz.Columns.Count; i++)
                //{
                //    if (dt_df_zytz.Columns[i].ColumnName.Equals(tz))
                //    {
                //        TZ_DF_gs = TZ_DF_gs.Replace("{0}", dt_df_zytz.Rows[0][i].ToString());
                //    }
                //    else
                //    {
                //        TZ_DF_gs = TZ_DF_gs.Replace("{" + j.ToString() + "}", dt_df_zytz.Rows[0][i].ToString());
                //        j++;
                //    }
                //}

                //计算
                //string flag = sc.Eval(TZ_DF_gs).ToString();
                dt_df_zytz.Rows[1][tz] = flag;

                Control control = Controls.Find(controlType_lable + tz + "_title", true)[0];

                //得分设定
                setValueToControl((controlType_lable + tz + "_zytz"), TZ_DF);

                //是 //倾向是
                setValueToControl((controlType_radioButton + tz + flag.ToString() + "_zytz"), flag);
                if (!flag.Equals("3"))
                {
                    setValueToControl((controlType_radioButton + tz.Replace("DF", "BS") + "_" + flag.ToString() + "_zytz"), flag);
                    control.ForeColor = Color.Red;
                }
                else
                {
                    setValueToControl((controlType_radioButton + tz.Replace("DF", "BS") + "_" + flag.ToString() + "_zytz"), flag);
                    control.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 中医体质保存
        /// </summary>
        private void save_zytz()
        {
            // string guid = System.Guid.NewGuid().ToString();
            DBAccess dBAccess = new DBAccess();

            string sql = "";
            //string key = "";
            //string value = "";
            //string kjlx = "";
            //string his_db = "";

            ArrayList sqlList = new ArrayList();
            sql = "delete from T_LNR_ZYYTZGL where czy='{czy}' and gzz='{gzz}' and d_grdabh='{d_grdabh}' and  happentime ='{happentime}'";
            //操作员
            sql = sql.Replace("{czy}", userId);
            //工作组
            sql = sql.Replace("{gzz}", yhfz);
            //健康档案编号
            sql = sql.Replace("{d_grdabh}", textBox_head_JKDAH.Text);
            //体检日期
            sql = sql.Replace("{happentime}", getValueFromDt(dt_paraFromParent, 0, "tjsj"));
            sqlList.Add(sql);

            //插入
            sql = sql_insert_zytz;

            sql = sql.Replace("{D_GRDABH}", textBox_head_JKDAH.Text);//个人档案编号
            sql = sql.Replace("{CREATEREGION}", yljg);//创建机构
            sql = sql.Replace("{CREATEUSER}", userId);////创建者
            sql = sql.Replace("{UPDATEUSER}", userId);//更新人
            sql = sql.Replace("{P_RGID}", yljg);//当前所属机构
            sql = sql.Replace("{CREATETIME}", DateTime.Now.ToString("yyy-MM-dd"));//创建时间
            sql = sql.Replace("{UPDATETIME}", DateTime.Now.ToString("yyy-MM-dd"));//更新时间
            sql = sql.Replace("{L_JL}", dt_da_zytz.Rows[0]["da"].ToString());//精力
            sql = sql.Replace("{L_TL}", dt_da_zytz.Rows[1]["da"].ToString());//体力
            sql = sql.Replace("{L_HX}", dt_da_zytz.Rows[2]["da"].ToString());//呼吸短促
            sql = sql.Replace("{L_SH}", dt_da_zytz.Rows[3]["da"].ToString());//说话
            sql = sql.Replace("{L_XQ}", dt_da_zytz.Rows[4]["da"].ToString());//心情
            sql = sql.Replace("{L_JZJL}", dt_da_zytz.Rows[5]["da"].ToString());//精神紧张、焦虑不安
            sql = sql.Replace("{L_SHZTGB}", dt_da_zytz.Rows[6]["da"].ToString());//生活状态改变
            sql = sql.Replace("{L_HPJX}", dt_da_zytz.Rows[7]["da"].ToString());//害怕或惊吓
            sql = sql.Replace("{L_STCZ}", dt_da_zytz.Rows[8]["da"].ToString());//身体超重
            sql = sql.Replace("{L_YJGS}", dt_da_zytz.Rows[9]["da"].ToString());//眼睛干涩
            sql = sql.Replace("{L_SJFL}", dt_da_zytz.Rows[10]["da"].ToString());//手脚发凉
            sql = sql.Replace("{L_WWBYX}", dt_da_zytz.Rows[11]["da"].ToString());//胃脘部、背部或腰膝部怕冷
            sql = sql.Replace("{L_SBLHL}", dt_da_zytz.Rows[12]["da"].ToString());//受不了寒冷
            sql = sql.Replace("{L_RYGM}", dt_da_zytz.Rows[13]["da"].ToString());//容易患感冒
            sql = sql.Replace("{L_BSLBT}", dt_da_zytz.Rows[14]["da"].ToString());//没有感冒鼻塞、流鼻涕
            sql = sql.Replace("{L_KNNDH}", dt_da_zytz.Rows[15]["da"].ToString());//口粘口腻或睡眠打鼾
            sql = sql.Replace("{L_GM}", dt_da_zytz.Rows[16]["da"].ToString());//容易过敏
            sql = sql.Replace("{L_XMZ}", dt_da_zytz.Rows[17]["da"].ToString());//皮肤容易起荨麻疹
            sql = sql.Replace("{L_PFQZCX}", dt_da_zytz.Rows[18]["da"].ToString());//皮肤在不知不觉中会出现青紫瘀斑、皮下出血
            sql = sql.Replace("{L_PFHHFY}", dt_da_zytz.Rows[19]["da"].ToString());//皮肤一抓就红，并出现抓痕
            sql = sql.Replace("{L_PFKCG}", dt_da_zytz.Rows[20]["da"].ToString());//皮肤或口唇干
            sql = sql.Replace("{L_ZTMMTT}", dt_da_zytz.Rows[21]["da"].ToString());//肢体麻木或固定部位疼痛
            sql = sql.Replace("{L_YNYL}", dt_da_zytz.Rows[22]["da"].ToString());//面部或鼻部有油腻感或者油亮发光
            sql = sql.Replace("{L_MSMK}", dt_da_zytz.Rows[23]["da"].ToString());//面色或目眶晦黯，或出现褐色斑块
            sql = sql.Replace("{L_PFSZCJ}", dt_da_zytz.Rows[24]["da"].ToString());//皮肤湿疹、疮疖
            sql = sql.Replace("{L_KGYZHS}", dt_da_zytz.Rows[25]["da"].ToString());//口干咽燥、总想喝水
            sql = sql.Replace("{L_KKKC}", dt_da_zytz.Rows[26]["da"].ToString());//口苦或嘴里有异味
            sql = sql.Replace("{L_FBFD}", dt_da_zytz.Rows[27]["da"].ToString());//腹部肥大
            sql = sql.Replace("{L_BXHLS}", dt_da_zytz.Rows[28]["da"].ToString());//吃(喝)凉的东西会感到不舒服或者怕吃(喝)凉的东西
            sql = sql.Replace("{L_DBNZ}", dt_da_zytz.Rows[29]["da"].ToString());//大便黏滞
            sql = sql.Replace("{L_DBGZ}", dt_da_zytz.Rows[30]["da"].ToString());//大便干燥
            sql = sql.Replace("{L_STHN}", dt_da_zytz.Rows[31]["da"].ToString());//舌苔厚腻
            sql = sql.Replace("{L_SXJMYZ}", dt_da_zytz.Rows[32]["da"].ToString());//舌下静脉瘀紫或增粗
            sql = sql.Replace("{L_QX_DF}", dt_df_zytz.Rows[0]["L_QX_DF"].ToString());//气虚质得分
            sql = sql.Replace("{L_QX_BS}", dt_df_zytz.Rows[1]["L_QX_DF"].ToString());//气虚质辨识
            sql = sql.Replace("{L_QX_ZD}", getL_ZD_zytz("L_QX_ZD"));//气虚质中医药保健指导
            sql = sql.Replace("{L_QX_QT}", textBox_L_QX_QT_zytz.Text);//气虚质其他
            sql = sql.Replace("{L_YANG_DF}", dt_df_zytz.Rows[0]["L_YANG_DF"].ToString());//阳虚质得分
            sql = sql.Replace("{L_YANG_BS}", dt_df_zytz.Rows[1]["L_YANG_DF"].ToString());//阳虚质辨识
            sql = sql.Replace("{L_YANG_ZD}", getL_ZD_zytz("L_YANG_ZD"));//阳虚质中医药保健指导
            sql = sql.Replace("{L_YANG_QT}", textBox_L_YANG_QT_zytz.Text);//阳虚质其他
            sql = sql.Replace("{L_YIN_DF}", dt_df_zytz.Rows[0]["L_YIN_DF"].ToString());//阴虚质得分
            sql = sql.Replace("{L_YIN_BS}", dt_df_zytz.Rows[1]["L_YIN_DF"].ToString());//阴虚质辨识
            sql = sql.Replace("{L_YIN_ZD}", getL_ZD_zytz("L_YIN_ZD"));//阴虚质中医药保健指导
            sql = sql.Replace("{L_YIN_QT}", textBox_L_YIN_QT_zytz.Text);//阴虚质其他
            sql = sql.Replace("{L_TS_DF}", dt_df_zytz.Rows[0]["L_TS_DF"].ToString());//痰湿质得分
            sql = sql.Replace("{L_TS_BS}", dt_df_zytz.Rows[1]["L_TS_DF"].ToString());//痰湿质辨识
            sql = sql.Replace("{L_TS_ZD}", getL_ZD_zytz("L_TS_ZD"));//痰湿质中医药保健指导
            sql = sql.Replace("{L_TS_QT}", textBox_L_TS_QT_zytz.Text);//痰湿质其他
            sql = sql.Replace("{L_SR_DF}", dt_df_zytz.Rows[0]["L_SR_DF"].ToString());//湿热质得分
            sql = sql.Replace("{L_SR_BS}", dt_df_zytz.Rows[1]["L_SR_DF"].ToString());//湿热质辨识
            sql = sql.Replace("{L_SR_ZD}", getL_ZD_zytz("L_SR_ZD"));//湿热质中医药保健指导
            sql = sql.Replace("{L_SR_QT}", textBox_L_SR_QT_zytz.Text);//湿湿质其他
            sql = sql.Replace("{L_XY_DF}", dt_df_zytz.Rows[0]["L_XY_DF"].ToString());//血瘀质得分
            sql = sql.Replace("{L_XY_BS}", dt_df_zytz.Rows[1]["L_XY_DF"].ToString());//血瘀质辨识
            sql = sql.Replace("{L_XY_ZD}", getL_ZD_zytz("L_XY_ZD"));//血瘀质中医药保健指导
            sql = sql.Replace("{L_XY_QT}", textBox_L_XY_QT_zytz.Text);//血瘀质其他
            sql = sql.Replace("{L_QY_DF}", dt_df_zytz.Rows[0]["L_QY_DF"].ToString());//气郁质得分
            sql = sql.Replace("{L_QY_BS}", dt_df_zytz.Rows[1]["L_QY_DF"].ToString());//气郁质辨识
            sql = sql.Replace("{L_QY_ZD}", getL_ZD_zytz("L_QY_ZD"));//气郁质中医药保健指导
            sql = sql.Replace("{L_QY_QT}", textBox_L_QY_QT_zytz.Text);//气郁质其他
            sql = sql.Replace("{L_TB_DF}", dt_df_zytz.Rows[0]["L_TB_DF"].ToString());//特禀质得分
            sql = sql.Replace("{L_TB_BS}", dt_df_zytz.Rows[1]["L_TB_DF"].ToString());//特禀质辨识
            sql = sql.Replace("{L_TB_ZD}", getL_ZD_zytz("L_TB_ZD"));//特禀质中医药保健指导
            sql = sql.Replace("{L_TB_QT}", textBox_L_TB_QT_zytz.Text);//特禀质其他
            sql = sql.Replace("{L_PH_DF}", dt_df_zytz.Rows[0]["L_PH_DF"].ToString());//平和质得分
            sql = sql.Replace("{L_PH_BS}", dt_df_zytz.Rows[1]["L_PH_DF"].ToString());//平和质辨识
            sql = sql.Replace("{L_PH_ZD}", getL_ZD_zytz("L_PH_ZD"));//平和质中医药保健指导
            sql = sql.Replace("{L_PH_QT}", textBox_L_PH_QT_zytz.Text);//平和质其他
            sql = sql.Replace("{HAPPENTIME}", dateTimePicker_HAPPENTIME.Value.ToString("yyyy-MM-dd"));//填表日期    
            sql = sql.Replace("{YSQM}", lTextBox_YSQM.Text);//医生签名
            sql = sql.Replace("{guid}", getGuid_zytz());//guid
            sql = sql.Replace("{czy}", userId);//czy
            sql = sql.Replace("{gzz}", yhfz);//gzz

            sqlList.Add(sql);

            //更新体检人员信息表(T_JK_TJRYXX）
            //sqlList.Add(save_T_JK_TJRYXX(null, dateTimePicker_HAPPENTIME.Value.ToString("yyyy-MM-dd"), "TJZT_zytz", "TJSJ_zytz"));
            ArrayList TJRYXXList = save_T_JK_TJRYXX(dt_paraFromParent, dateTimePicker_zytz.Value.ToString("yyyy-MM-dd"), Common.TJTYPE.中医体质辨识, Common.ZT.确定状态);
            if (TJRYXXList != null && TJRYXXList.Count > 0)
            {
                for (int i = 0; i < TJRYXXList.Count; i++)
                {
                    sqlList.Add(TJRYXXList[i]);
                }
            }
            dBAccess.ExecuteNonQueryBySql(sqlList);
        }

        /// <summary>
        /// 取得Guid
        /// </summary>
        /// <returns></returns>
        private string getGuid_zytz()
        {
            string guid = System.Guid.NewGuid().ToString();
            DBAccess dBAccess = new DBAccess();
            string sql = "";
            ArrayList sqlList = new ArrayList();
            sql = "select guid from T_LNR_ZYYTZGL where czy='{czy}' and gzz='{gzz}' and D_GRDABH='{d_grdabh}' and  HAPPENTIME ='{happentime}'";
            //操作员
            sql = sql.Replace("{czy}", userId);
            //工作组
            sql = sql.Replace("{gzz}", yhfz);
            //健康档案编号
            sql = sql.Replace("{d_grdabh}", textBox_head_JKDAH.Text);
            //体检日期
            sql = sql.Replace("{happentime}", dateTimePicker_HAPPENTIME.Value.ToString("yyyy-MM-dd"));
            DataTable dt = dBAccess.ExecuteQueryBySql(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                guid = dt.Rows[0]["guid"].ToString();
            }
            return guid;
        }

        /// <summary>
        /// 取得保健指导的结果
        /// </summary>
        /// <param name="name_zd"></param>
        /// <returns></returns>
        private string getL_ZD_zytz(string name_zd)
        {
            //checkBox_L_QX_ZD_1_zytz
            string CheckedResult = "  ";
            try
            {
                for (int i = 1; 1 <= 6; i++)
                {
                    Control control = Controls.Find("checkBox_" + name_zd + "_" + i.ToString() + "_zytz", true)[0];
                    CheckBox CheckBox_tem = (CheckBox)control;
                    if (CheckBox_tem.Checked == true)
                    {
                        CheckedResult = CheckedResult.Trim() + CheckBox_tem.Tag.ToString() + ",";
                    }
                }
            }
            catch { }
            return CheckedResult.Substring(0, CheckedResult.Length - 1);
        }

        /// <summary>
        /// 中医体质辨识保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //如果没有答完题不能保存
                if (dt_da_zytz.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_da_zytz.Rows.Count; i++)
                    {
                        if (dt_da_zytz.Rows[i]["da"].ToString().Length == 0 || dt_da_zytz.Rows[i]["da"].ToString().Equals ("0"))
                        {
                            MessageBox.Show("请回答完全部问题后再提交！");
                            return;
                        }
                    }
                }
                save_zytz();
                MessageBox.Show("保存成功！");

                clear_zytz();

                //调用父页面的方法
                if (main_form != null)
                {
                    main_form.setParentFormDo(null);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！" + ex.Message);
            }
        }
        /// <summary>
        /// 中医体质清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            clear_zytz();
        }

        /// <summary>
        /// 中医体质清空页面上的内容
        /// </summary>
        private void clear_zytz()
        {
            // 中医体质辨识的答案列表
            dt_da_zytz = new DataTable();
            // 中医体质得分
            dt_df_zytz = new DataTable();

            //初始化答题区
            initWt();

            //计算
            calculate_zytz();

            //清除复选框
            clearCheck();

            //清空信息
            label_msg_xx.Text = "";
        }

        //清除复选框
        private void clearCheck()
        {
            //气虚质中医药保健指导
            checkBox_L_QX_ZD_1_zytz.Checked = false;
            checkBox_L_QX_ZD_2_zytz.Checked = false;
            checkBox_L_QX_ZD_3_zytz.Checked = false;
            checkBox_L_QX_ZD_4_zytz.Checked = false;
            checkBox_L_QX_ZD_5_zytz.Checked = false;
            checkBox_L_QX_ZD_6_zytz.Checked = false;
            textBox_L_QX_QT_zytz.Text = "";

            //阳虚质中医药保健指导
            checkBox_L_YANG_ZD_1_zytz.Checked = false;
            checkBox_L_YANG_ZD_2_zytz.Checked = false;
            checkBox_L_YANG_ZD_3_zytz.Checked = false;
            checkBox_L_YANG_ZD_4_zytz.Checked = false;
            checkBox_L_YANG_ZD_5_zytz.Checked = false;
            checkBox_L_YANG_ZD_6_zytz.Checked = false;
            textBox_L_YANG_QT_zytz.Text = "";

            //阴虚质中医药保健指导
            checkBox_L_YIN_ZD_1_zytz.Checked = false;
            checkBox_L_YIN_ZD_2_zytz.Checked = false;
            checkBox_L_YIN_ZD_3_zytz.Checked = false;
            checkBox_L_YIN_ZD_4_zytz.Checked = false;
            checkBox_L_YIN_ZD_5_zytz.Checked = false;
            checkBox_L_YIN_ZD_6_zytz.Checked = false;
            textBox_L_YIN_QT_zytz.Text = "";

            //痰湿质中医药保健指导
            checkBox_L_TS_ZD_1_zytz.Checked = false;
            checkBox_L_TS_ZD_2_zytz.Checked = false;
            checkBox_L_TS_ZD_3_zytz.Checked = false;
            checkBox_L_TS_ZD_4_zytz.Checked = false;
            checkBox_L_TS_ZD_5_zytz.Checked = false;
            checkBox_L_TS_ZD_6_zytz.Checked = false;
            textBox_L_TS_QT_zytz.Text = "";
            //湿热质中医药保健指导
            checkBox_L_SR_ZD_1_zytz.Checked = false;
            checkBox_L_SR_ZD_2_zytz.Checked = false;
            checkBox_L_SR_ZD_3_zytz.Checked = false;
            checkBox_L_SR_ZD_4_zytz.Checked = false;
            checkBox_L_SR_ZD_5_zytz.Checked = false;
            checkBox_L_SR_ZD_6_zytz.Checked = false;
            textBox_L_SR_QT_zytz.Text = "";
            //血瘀质中医药保健指导
            checkBox_L_XY_ZD_1_zytz.Checked = false;
            checkBox_L_XY_ZD_2_zytz.Checked = false;
            checkBox_L_XY_ZD_3_zytz.Checked = false;
            checkBox_L_XY_ZD_4_zytz.Checked = false;
            checkBox_L_XY_ZD_5_zytz.Checked = false;
            checkBox_L_XY_ZD_6_zytz.Checked = false;
            textBox_L_XY_QT_zytz.Text = "";
            //气郁质中医药保健指导
            checkBox_L_QY_ZD_1_zytz.Checked = false;
            checkBox_L_QY_ZD_2_zytz.Checked = false;
            checkBox_L_QY_ZD_3_zytz.Checked = false;
            checkBox_L_QY_ZD_4_zytz.Checked = false;
            checkBox_L_QY_ZD_5_zytz.Checked = false;
            checkBox_L_QY_ZD_6_zytz.Checked = false;
            textBox_L_QY_QT_zytz.Text = "";

            //特禀质中医药保健指导
            checkBox_L_TB_ZD_1_zytz.Checked = false;
            checkBox_L_TB_ZD_2_zytz.Checked = false;
            checkBox_L_TB_ZD_3_zytz.Checked = false;
            checkBox_L_TB_ZD_4_zytz.Checked = false;
            checkBox_L_TB_ZD_5_zytz.Checked = false;
            checkBox_L_TB_ZD_6_zytz.Checked = false;
            textBox_L_TB_QT_zytz.Text = "";

            //平和质中医药保健指导
            checkBox_L_PH_ZD_1_zytz.Checked = false;
            checkBox_L_PH_ZD_2_zytz.Checked = false;
            checkBox_L_PH_ZD_3_zytz.Checked = false;
            checkBox_L_PH_ZD_4_zytz.Checked = false;
            checkBox_L_PH_ZD_5_zytz.Checked = false;
            checkBox_L_PH_ZD_6_zytz.Checked = false;
            textBox_L_PH_QT_zytz.Text = "";
        }

        /// <summary>
        /// 检索中医体质的数据
        /// </summary>
        private void select_zytz()
        {
            clearCheck();
            //中医体质辨识的问题
            getWtList();
            initWt();
            DataTable dt_zytz = get_zytz();
            if (dt_zytz != null && dt_zytz.Rows.Count > 0)
            {
                //对应项目在结果集中的列的位置
                int j = 8;
                //对答题区进行赋值
                //dt_da_zytz = new DataTable();
                for (int i = 0; i < 33; i++)
                {
                    if (!dt_zytz.Rows[0][j].ToString().Equals("0"))
                    {
                        dt_da_zytz.Rows[i]["da"] = dt_zytz.Rows[0][j].ToString();
                        dt_da_zytz.Rows[i]["da1"] = 6 - int.Parse(dt_zytz.Rows[0][j].ToString());
                        setValueToControl(controlType_lable + (i + 1).ToString().PadLeft(2, '0') + "_zytz", dt_zytz.Rows[0][j].ToString());
                    }

                    j++;
                }

                //计算结果
                calculate_zytz();

                //中医保健指导
                //气虚质中医药保健指导
                set_L_ZD("L_QX_ZD", dt_zytz.Rows[0]["L_QX_ZD"].ToString(), dt_zytz.Rows[0]["L_QX_QT"].ToString());

                //阳虚质中医药保健指导
                set_L_ZD("L_YANG_ZD", dt_zytz.Rows[0]["L_YANG_ZD"].ToString(), dt_zytz.Rows[0]["L_YANG_QT"].ToString());

                //阴虚质中医药保健指导
                set_L_ZD("L_YIN_ZD", dt_zytz.Rows[0]["L_YIN_ZD"].ToString(), dt_zytz.Rows[0]["L_YIN_QT"].ToString());

                //痰湿质中医药保健指导
                set_L_ZD("L_TS_ZD", dt_zytz.Rows[0]["L_TS_ZD"].ToString(), dt_zytz.Rows[0]["L_TS_QT"].ToString());

                //湿热质中医药保健指导
                set_L_ZD("L_SR_ZD", dt_zytz.Rows[0]["L_SR_ZD"].ToString(), dt_zytz.Rows[0]["L_SR_QT"].ToString());

                //血瘀质中医药保健指导
                set_L_ZD("L_XY_ZD", dt_zytz.Rows[0]["L_XY_ZD"].ToString(), dt_zytz.Rows[0]["L_XY_QT"].ToString());

                //气郁质中医药保健指导
                set_L_ZD("L_QY_ZD", dt_zytz.Rows[0]["L_QY_ZD"].ToString(), dt_zytz.Rows[0]["L_QY_QT"].ToString());
                //特禀质中医药保健指导

                set_L_ZD("L_TB_ZD", dt_zytz.Rows[0]["L_TB_ZD"].ToString(), dt_zytz.Rows[0]["L_TB_QT"].ToString());

                //平和质中医药保健指导
                set_L_ZD("L_PH_ZD", dt_zytz.Rows[0]["L_PH_ZD"].ToString(), dt_zytz.Rows[0]["L_PH_QT"].ToString());

                //填表日期
                dateTimePicker_HAPPENTIME.Value = getDateFromString(dt_zytz.Rows[0]["HAPPENTIME"].ToString());
                //签名医生
                lTextBox_YSQM.Text = dt_zytz.Rows[0]["YSQM"].ToString();
            }
            else
            {
                //计算结果
                calculate_zytz();
            }


        }
        /// <summary>
        /// 取得当前选中的居民的检查信息
        /// </summary>
        /// <returns></returns>
        private DataTable get_zytz() {

            string select_sql = "select * from T_LNR_ZYYTZGL where D_GRDABH='{D_GRDABH}' and HAPPENTIME='{HAPPENTIME}' ";

            select_sql = select_sql.Replace("{D_GRDABH}", textBox_head_JKDAH.Text).Replace("{HAPPENTIME}", dateTimePicker_zytz.Value .ToString ("yyyy-MM-dd"));

            DBAccess access = new DBAccess();
            DataTable dt_zytz = access.ExecuteQueryBySql(select_sql);
            return dt_zytz;
        }

        /// <summary>
        /// 将保健指导的数据设定到页面上
        /// </summary>
        /// <param name="name_zd">字段名称</param>
        /// <param name="valueList">选中的值得字符串</param>
        /// <param name="qt">其他</param>
        private void set_L_ZD(string name_zd,string valueList,string qt)
        {
            
            for(int i=1;i<=6;i++){
                string controlId=controlType_checkBox +name_zd+"_"+i.ToString ()+"_zytz";
                Control control = Controls.Find(controlId, true)[0];
                CheckBox CheckBox_tem = (CheckBox)control;
                if (valueList.IndexOf(CheckBox_tem.Tag.ToString()) > -1)
                {
                    
                    CheckBox_tem.Checked = true;
                    //setValueToControl(controlId, i.ToString());
                }
                else
                {
                    CheckBox_tem.Checked = false;
                }
            }
           // setValueToControl(controlType_checkBox + name_zd + "_" + 6.ToString() + "_zytz", "");
            string[] namelist = name_zd.Split(new char[]{'_'});
            setValueToControl(controlType_textBox + "L_{0}_QT_zytz".Replace("{0}", namelist[1]), qt);
           
        }



        /// <summary>
        /// 中医保健
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_zybj_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                zybj();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 设定中医保健指导
        /// </summary>
        /// <returns></returns>
        public bool zybj()
        {
            
            //获取维护好的中医保健指导
            jktjBll jktjbll=new jktjBll ();
            DataTable dt_bjzd = jktjbll.GetMoHuList(string .Format (" and  YYBM='{0}'",UserInfo .Yybm ), "sql064");
            if (dt_bjzd != null && dt_bjzd.Rows.Count > 0)
            {
                for (int i = 0; i < dt_bjzd.Rows.Count; i++)
                {
                    //中医保健指导
                    //气虚质中医药保健指导 radioButton_L_QX_BS_1_zytz 
                    if (dt_bjzd.Rows[i]["ZYTZ"].ToString().Equals("1"))
                    {
                        set_L_ZD_tem("L_QX_ZD", dt_bjzd.Rows[i]["zd"].ToString(), dt_bjzd.Rows[i]["QTZD"].ToString(), "L_QX", dt_bjzd.Rows[i]["TZBZ"].ToString());
                    }

                    //阳虚质中医药保健指导
                    if (dt_bjzd.Rows[i]["ZYTZ"].ToString().Equals("2"))
                    {
                        set_L_ZD_tem("L_YANG_ZD", dt_bjzd.Rows[i]["zd"].ToString(), dt_bjzd.Rows[i]["QTZD"].ToString(), "L_YANG", dt_bjzd.Rows[i]["TZBZ"].ToString());
                    }

                    //阴虚质中医药保健指导
                        if (dt_bjzd.Rows[i]["ZYTZ"].ToString().Equals("3"))
                    {
                        set_L_ZD_tem("L_YIN_ZD", dt_bjzd.Rows[i]["zd"].ToString(), dt_bjzd.Rows[i]["QTZD"].ToString(), "L_YIN", dt_bjzd.Rows[i]["TZBZ"].ToString());
                    }

                    //痰湿质中医药保健指导
                            if (dt_bjzd.Rows[i]["ZYTZ"].ToString().Equals("4"))
                    {
                        set_L_ZD_tem("L_TS_ZD", dt_bjzd.Rows[i]["zd"].ToString(), dt_bjzd.Rows[i]["QTZD"].ToString(), "L_TS", dt_bjzd.Rows[i]["TZBZ"].ToString());
                    }

                    //湿热质中医药保健指导
                                if (dt_bjzd.Rows[i]["ZYTZ"].ToString().Equals("5"))
                    {
                        set_L_ZD_tem("L_SR_ZD", dt_bjzd.Rows[i]["zd"].ToString(), dt_bjzd.Rows[i]["QTZD"].ToString(), "L_SR", dt_bjzd.Rows[i]["TZBZ"].ToString());
                    }

                    //血瘀质中医药保健指导
                                    if (dt_bjzd.Rows[i]["ZYTZ"].ToString().Equals("6"))
                    {
                        set_L_ZD_tem("L_XY_ZD", dt_bjzd.Rows[i]["zd"].ToString(), dt_bjzd.Rows[i]["QTZD"].ToString(), "L_XY", dt_bjzd.Rows[i]["TZBZ"].ToString());
                    }

                    //气郁质中医药保健指导
                                        if (dt_bjzd.Rows[i]["ZYTZ"].ToString().Equals("7"))
                    {
                        set_L_ZD_tem("L_QY_ZD", dt_bjzd.Rows[i]["zd"].ToString(), dt_bjzd.Rows[i]["QTZD"].ToString(), "L_QY", dt_bjzd.Rows[i]["TZBZ"].ToString());
                    }
                    //特禀质中医药保健指导
                if (dt_bjzd.Rows[i]["ZYTZ"].ToString().Equals("8"))
                    {
                        set_L_ZD_tem("L_TB_ZD", dt_bjzd.Rows[i]["zd"].ToString(), dt_bjzd.Rows[i]["QTZD"].ToString(), "L_TB", dt_bjzd.Rows[i]["TZBZ"].ToString());
                    }

                    //平和质中医药保健指导
                if (dt_bjzd.Rows[i]["ZYTZ"].ToString().Equals("9"))
                    {
                        set_L_ZD_tem("L_PH_ZD", dt_bjzd.Rows[i]["zd"].ToString(), dt_bjzd.Rows[i]["QTZD"].ToString(), "L_PH", dt_bjzd.Rows[i]["TZBZ"].ToString());
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 将维护好的保健指导的数据设定到页面上
        /// </summary>
        /// <param name="name_zd">字段名称</param>
        /// <param name="valueList">选中的值得字符串</param>
        /// <param name="qt">其他</param>
        public void set_L_ZD_tem(string name_zd, string valueList, string qt, string rdoFlag, string TZBZ)
        {
            string controlId = string.Format("radioButton_{0}_BS_{1}_zytz", rdoFlag, TZBZ);
            Control control = Controls.Find(controlId, true)[0];

            RadioButton CheckBox_tem = (RadioButton)control;
            if (CheckBox_tem.Checked == true)
            {
                set_L_ZD(name_zd, valueList, qt);
            }
        }


        #endregion
        #endregion


        #region 中医体质sql
        public string sql_insert_zytz = @"insert into T_LNR_ZYYTZGL(
                                            [D_GRDABH],
                                            [CREATEREGION],
                                            [CREATEUSER],
                                            [UPDATEUSER],
                                            [P_RGID],
                                            [CREATETIME],
                                            [UPDATETIME],
                                            [L_JL],
                                            [L_TL],
                                            [L_HX],
                                            [L_SH],
                                            [L_XQ],
                                            [L_JZJL],
                                            [L_SHZTGB],
                                            [L_HPJX],
                                            [L_STCZ],
                                            [L_YJGS],
                                            [L_SJFL],
                                            [L_WWBYX],
                                            [L_SBLHL],
                                            [L_RYGM],
                                            [L_BSLBT],
                                            [L_KNNDH],
                                            [L_GM],
                                            [L_XMZ],
                                            [L_PFQZCX],
                                            [L_PFHHFY],
                                            [L_PFKCG],
                                            [L_ZTMMTT],
                                            [L_YNYL],
                                            [L_MSMK],
                                            [L_PFSZCJ],
                                            [L_KGYZHS],
                                            [L_KKKC],
                                            [L_FBFD],
                                            [L_BXHLS],
                                            [L_DBNZ],
                                            [L_DBGZ],
                                            [L_STHN],
                                            [L_SXJMYZ],
                                            [L_QX_DF],
                                            [L_QX_BS],
                                            [L_QX_ZD],
                                            [L_QX_QT],
                                            [L_YANG_DF],
                                            [L_YANG_BS],
                                            [L_YANG_ZD],
                                            [L_YANG_QT],
                                            [L_YIN_DF],
                                            [L_YIN_BS],
                                            [L_YIN_ZD],
                                            [L_YIN_QT],
                                            [L_TS_DF],
                                            [L_TS_BS],
                                            [L_TS_ZD],
                                            [L_TS_QT],
                                            [L_SR_DF],
                                            [L_SR_BS],
                                            [L_SR_ZD],
                                            [L_SR_QT],
                                            [L_XY_DF],
                                            [L_XY_BS],
                                            [L_XY_ZD],
                                            [L_XY_QT],
                                            [L_QY_DF],
                                            [L_QY_BS],
                                            [L_QY_ZD],
                                            [L_QY_QT],
                                            [L_TB_DF],
                                            [L_TB_BS],
                                            [L_TB_ZD],
                                            [L_TB_QT],
                                            [L_PH_DF],
                                            [L_PH_BS],
                                            [L_PH_ZD],
                                            [L_PH_QT],
                                            [HAPPENTIME],
                                            [YSQM],
                                            [CREATEFROM],
                                            [EDIT],
                                            [guid],
                                            [czy],
                                            [gzz]
                                            )
                                            values(
                                            '{D_GRDABH}',
                                            '{CREATEREGION}',
                                            '{CREATEUSER}',
                                            '{UPDATEUSER}',
                                            '{P_RGID}',
                                            '{CREATETIME}',
                                            '{UPDATETIME}',
                                            {L_JL},
                                            {L_TL},
                                            {L_HX},
                                            {L_SH},
                                            {L_XQ},
                                            {L_JZJL},
                                            {L_SHZTGB},
                                            {L_HPJX},
                                            {L_STCZ},
                                            {L_YJGS},
                                            {L_SJFL},
                                            {L_WWBYX},
                                            {L_SBLHL},
                                            {L_RYGM},
                                            {L_BSLBT},
                                            {L_KNNDH},
                                            {L_GM},
                                            {L_XMZ},
                                            {L_PFQZCX},
                                            {L_PFHHFY},
                                            {L_PFKCG},
                                            {L_ZTMMTT},
                                            {L_YNYL},
                                            {L_MSMK},
                                            {L_PFSZCJ},
                                            {L_KGYZHS},
                                            {L_KKKC},
                                            {L_FBFD},
                                            {L_BXHLS},
                                            {L_DBNZ},
                                            {L_DBGZ},
                                            {L_STHN},
                                            {L_SXJMYZ},
                                            {L_QX_DF},
                                            '{L_QX_BS}',
                                            '{L_QX_ZD}',
                                            '{L_QX_QT}',
                                            {L_YANG_DF},
                                            '{L_YANG_BS}',
                                            '{L_YANG_ZD}',
                                            '{L_YANG_QT}',
                                            {L_YIN_DF},
                                            '{L_YIN_BS}',
                                            '{L_YIN_ZD}',
                                            '{L_YIN_QT}',
                                            {L_TS_DF},
                                            '{L_TS_BS}',
                                            '{L_TS_ZD}',
                                            '{L_TS_QT}',
                                            {L_SR_DF},
                                            '{L_SR_BS}',
                                            '{L_SR_ZD}',
                                            '{L_SR_QT}',
                                            {L_XY_DF},
                                            '{L_XY_BS}',
                                            '{L_XY_ZD}',
                                            '{L_XY_QT}',
                                            {L_QY_DF},
                                            '{L_QY_BS}',
                                            '{L_QY_ZD}',
                                            '{L_QY_QT}',
                                            {L_TB_DF},
                                            '{L_TB_BS}',
                                            '{L_TB_ZD}',
                                            '{L_TB_QT}',
                                            {L_PH_DF},
                                            '{L_PH_BS}',
                                            '{L_PH_ZD}',
                                            '{L_PH_QT}',
                                            '{HAPPENTIME}',
                                            '{YSQM}',
                                            '',
                                            1,
                                            '{guid}',
                                            '{czy}',
                                            '{gzz}'
                                            )
                                            ";
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
        /// 前一页面传过来的人员信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public override bool setParaToChild(DataTable para)
        {
            dt_paraFromParent = para;

            ////体检日期设定
            //if (getValueFromDt(dt_paraFromParent, 0, "tjsj").Length > 0)
            //{
            //    dateTimePicker_zytz.Value = Convert.ToDateTime(getValueFromDt(dt_paraFromParent, 0, "tjsj"));
            //}
            //else
            //{
            //    dateTimePicker_zytz.Value = DateTime.Now;
            //}

           
            //健康档案号
            textBox_head_JKDAH.Text = getValueFromDt(dt_paraFromParent, 0, "JKDAH");
            // lTextBox_YSQM.Text = CommomSysInfo.TJFZR_MC;
            setpageInit();
            select_zytz();
            
            return true;
        }

        /// <summary>
        /// 体检医生及体检时间 改变是的处理
        /// </summary>
        public void setpageInit()
        {
            //体检日期设定
            //if (getValueFromDt(dt_paraFromParent, 0, "tjsj").Length > 0)
            //{
            //    dateTimePicker_zytz.Value = Convert.ToDateTime(getValueFromDt(dt_paraFromParent, 0, "tjsj"));
            //}
            //else
            //{
            //    dateTimePicker_zytz.Value = DateTime.Now;
            //}

            dateTimePicker_zytz.Value =  Convert.ToDateTime(CommomSysInfo.tjsj);
            dateTimePicker_HAPPENTIME.Value = Convert.ToDateTime(CommomSysInfo.tjsj);
            //健康档案号
            lTextBox_YSQM.Text = CommomSysInfo.TJFZR_MC;

            //获取体质指数 腰围信息
            DBAccess dBAccess = new DBAccess();
            string jktj_sql = string.Format("select * from T_JK_jktj where d_grdabh='{0}' order by happentime desc ", getValueFromDt(dt_paraFromParent, 0, "JKDAH"));
            DataTable dt_jktj = dBAccess.ExecuteQueryBySql(jktj_sql);

            if (dt_jktj != null && dt_jktj.Rows.Count > 0)
            {
                label_msg_xx.Text = "";

                if (dt_jktj.Rows[0]["G_yw"] != null && dt_jktj.Rows[0]["G_yw"] != DBNull.Value)
                {
                    label_msg_xx.Text =string .Format ("{0} 腰围：[{1}]",label_msg_xx.Text,dt_jktj.Rows[0]["G_yw"].ToString());
                }

                if (dt_jktj.Rows[0]["G_tzhzh"] != null && dt_jktj.Rows[0]["G_tzhzh"] != DBNull.Value)
                {
                    label_msg_xx.Text = string.Format("{0} 体质指数：[{1}]", label_msg_xx.Text, dt_jktj.Rows[0]["G_tzhzh"].ToString());
                }
            }
        }

        /// <summary>
        /// 从datatable中取值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getValueFromDt(DataTable dt, int rowId, string key)
        {

            if (dt == null)
            {
                return "";
            }
            if (dt.Rows.Count == 0)
            {
                return "";
            }
            if (dt.Rows[rowId][key] != null)
            {
                return dt.Rows[rowId][key].ToString();
            }
            return "";
        }
    }
}
