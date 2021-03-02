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
using System.Threading;
using Florentis;
using TJClient.jktj;
using TJClient.Signname.Model;
using TJClient.ComForm;

namespace FBYClient
{
    public partial class LnrJktj : sysCommonForm
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

        private int TabIndex_p = 200;

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

        //自动接收数据窗口
        public AutoForm autoform = null;
        public AutoForm_sgtz autoform_sgtz = null;

        /// <summary>
        /// 在页面初始化时，控件的事件不触发
        /// </summary>
        public bool initStatue_left = false;

        //public bool initStatue_right = true;

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
        /// 暂存视力ID
        /// </summary>
        string eyesid = "";

        /// <summary>
        /// 保存父窗体
        /// </summary>
        public Main_Form main_form = null;

        #endregion

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

        public LnrJktj()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 获取签名的图片的路径
        /// </summary>
        /// <returns></returns>
        private string GetSignnamePicPath(out string realname)
        {
            TJClient.Signname.Model.JktjSignname jktjSignname = new TJClient.Signname.Model.JktjSignname()
            {
                Tjsj = Common.FormatDateTime(CommomSysInfo.tjsj),
                D_Grdabh = getValueFromDt(dt_paraFromParent, 0, "JKDAH")
            };

            //文字签名
            //string realname = "";

            string signnamePicPath = TJClient.Signname.Operation.SignnamePicPath(jktjSignname, out realname);
            if (string.IsNullOrEmpty(signnamePicPath))
            {
                signnamePicPath = TJClient.Signname.Operation.SignnamePicPath(
                    TJClient.Signname.ControlOperation.SignnameTitle(cboSignname));
            }
            return signnamePicPath;
        }

        private void ShowSignnameControl(bool visible)
        {
            btnTabletSignname.Visible = visible;
            cboSignname.Visible = visible;
            picSignname1111.Visible = visible;
            if (visible)
            {

                SignnameCboControlInit();
                SignnamePicControlInit();
            }
        }

        private void SignnameCboControlInit()
        {
            TJClient.Signname.ControlOperation.SignnameCboInit(cboSignname,
                    TJClient.Signname.Operation.UserSignnames(UserInfo.userId));
        }


        /// <summary>
        /// 设定签名
        /// </summary>
        private void SignnamePicControlInit()
        {
            string realname = "";
            string SignnamePicPath = GetSignnamePicPath(out realname);
            TJClient.Signname.ControlOperation.SignnamePicInit(picSignname1111, SignnamePicPath, realname, textBox_realname);

            //int index = cboSignname.FindString(realname);
            //cboSignname.SelectedIndex = index;

            //cboSignname.SelectedText = realname;



        }

        /// <summary>
        /// 已经签名的签名展示
        /// </summary>
        private void ChangeSignnamePic()
        {
            if (TJClient.Signname.Common.ShowSignnameOperation())
            {
                SignnamePicControlInit();
                //cboSignname.SelectedIndex=-1;
            }
        }

        private void OnlyHaveOneSignname()
        {
            //cboSignname.SelectedIndex = -1;
            cboSignname.SelectedIndex = 0;
        }

        private void NoSignname()
        {
            TJClient.Signname.ControlOperation.SignnamePicInit(picSignname1111, "", "", textBox_realname);
        }

        private void HaveManySignname(int count)
        {
            cboSignname.SelectedIndex = -1;
            int cboSignnameSelectedIndex = TJClient.Signname.Common.GetRandomInRange(count);
            if (cboSignnameSelectedIndex > -1)
            {
                cboSignname.SelectedIndex = cboSignnameSelectedIndex;
            }
        }

        /// <summary>
        /// 产生随机的签名
        /// </summary>
        private void ChangeSignnamePicRandom()
        {
            if (TJClient.Signname.Common.ShowSignnameOperation())
            {
                int count = cboSignname.Items.Count;
                if (count == 1)
                {
                    OnlyHaveOneSignname();
                }
                else if (count > 1)
                {
                    HaveManySignname(count);
                }
                else
                {
                    NoSignname();
                }
            }
        }
        /// <summary>
        /// 加载健康体检项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jktj_Load(object sender, EventArgs e)
        {
            //是否显示签名控件
            ShowSignnameControl(TJClient.Signname.Common.ShowSignnameOperation());
            //用户id
            userId = dt_para_sys.Rows[0]["userId"].ToString();
            //工作组
            yhfz = dt_para_sys.Rows[0]["gzz"].ToString();
            //医疗机构
            yljg = dt_para_sys.Rows[0]["yljg"].ToString();
            DBAccess dBAccess = new DBAccess();

            //获取该工作组对应的体检项目
            string sql = @"SELECT T_JK_TJXM.SIGNNAMEGROUP,T_JK_GZZ_XM.YLJGBM, T_JK_GZZ_XM.GZZBM, T_JK_TJXM.XMFLBM, T_JK_TJXM.XMBM, T_JK_TJXM.XMMC, T_JK_TJXM.KJXSMC, T_JK_TJXM.KJLX, T_JK_TJXM.SJZDBM, T_JK_TJXM.KJID, T_JK_TJXM.KJKD, T_JK_TJXM.KJGD, T_JK_TJXM.KJMRZ, T_JK_TJXM.JKDA_DB, T_JK_TJXM.HIS_DB, T_JK_TJXM.SL, T_JK_TJXM.DJ, T_JK_TJXM.parentxm, T_JK_TJXM.parentxmvalue, T_JK_TJXM.maxcount, T_JK_TJXM.dw, T_JK_XMFL.XMFLMC,T_JK_TJXM.rowNo,T_JK_TJXM.jj,T_JK_TJXM.valueHeigh,T_JK_TJXM.valueLower,T_JK_TJXM.isNotNull,T_JK_TJXM.fzlritem
                           FROM (T_JK_GZZ_XM INNER JOIN T_JK_TJXM ON T_JK_GZZ_XM.XMBM = T_JK_TJXM.XMBM) INNER JOIN T_JK_XMFL ON T_JK_TJXM.XMFLBM = T_JK_XMFL.XMFLBM
                           WHERE (((T_JK_GZZ_XM.YLJGBM)='{YLJGBM}') AND ((T_JK_GZZ_XM.GZZBM)='{GZZBM}')) order by T_JK_TJXM.XMFLBM, T_JK_TJXM.rowNo,T_JK_TJXM.ORDERBY ";

            //从数据库中取值
            dtResult = dBAccess.ExecuteQueryBySql(sql.Replace("{YLJGBM}", yljg).Replace("{GZZBM}", yhfz));
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                //加载tab之外的控件
                loadControls(dtResult);

                //控制控件的状态
                get_T_JK_TJXMGLKZ();
                init_TJXMGLKZ("", "");
            }
            else
            {
                MessageBox.Show("没有取到对应的体检项目，请确认！");
            }

        }
        #endregion

        #region 健康体检表

        #region 控件加载变量
        //保存最后添加的Panel
        public GroupBox panel_tem = null;

        //分类项目坐标
        public Point location = new Point();

        //分类中的项目坐标
        public Point childPoint = new Point();

        //分类项目坐标X
        public int d_x = 0;

        //分类项目坐标Y
        public int d_y = -10;

        //分类中的项目坐标X
        public int d_x_child = 0;

        //分类中的项目坐标Y
        public int d_y_child = 0;

        public int ControlRowCount = 4;

        public int ControlCount = 0;
        public int textControlCount = 0;

        //在groupbox中第一行与边线的间距
        public int d = 20;
        //控件行号
        public int rowNo = 0;

        public bool datagridview_bool = true;
        #endregion

        #region 控件加载

        /// <summary>
        /// 加载页面上的控件
        /// </summary>
        /// <param name="Controlsdt"></param>
        private void loadControls(DataTable Controlsdt)
        {
            //项目分类编码
            string strXmflbm = "";
            string strXmflbm_tem = "";

            //所有的项目的容器都是 panel_Form
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                //取得当前数据的项目分类编码
                strXmflbm = dtResult.Rows[i]["XMFLBM"].ToString();
                //加载分类名称，容器，line
                if (strXmflbm.Trim().ToLower().Equals(strXmflbm_tem.Trim().ToLower()) == false)
                {
                    if (panel_tem != null)
                    {
                        panel_tem.Height = d_y_child + 40;
                    }
                    if (panel_tem != null && panel_tem.Height > 0)
                    {
                        d_y = d_y + panel_tem.Height;
                    }

                    d_x = 0;
                    d_y = d_y + 10;
                    location.X = d_x;
                    location.Y = d_y;
                    panel_tem = AddControl_GroupBox(panel_body, location, dtResult.Rows[i]["KJID"].ToString(), dtResult.Rows[i]["XMFLMC"].ToString());
                    d_x_child = 0;
                    d_y_child = 0;
                    //保存当前分类
                    strXmflbm_tem = strXmflbm;
                    //textbox的当前行的数量设定为0
                    ControlCount = 0;
                    textControlCount = 0;
                }


                #region 加载label
                //加载checkbox
                if (dtResult.Rows[i]["KJLX"].ToString().ToLower().Equals("label"))
                {
                    Add_Label(dtResult.Rows[i], Color.FromArgb(31, 167, 150));
                }
                #endregion

                #region 加载linklabel
                //加载checkbox
                if (dtResult.Rows[i]["KJLX"].ToString().ToLower().Equals("linklabel"))
                {
                    Add_Linklabel(dtResult.Rows[i], Color.FromArgb(31, 167, 150));
                }
                #endregion

                #region 加载checkbox
                //加载checkbox
                if (dtResult.Rows[i]["KJLX"].ToString().ToLower().Equals("checkbox"))
                {
                    Add_checkBox(dtResult.Rows[i]);
                }
                #endregion

                #region 加载text
                //加载text
                if (dtResult.Rows[i]["KJLX"].ToString().ToLower().Equals("textbox"))
                {
                    Add_Textbox(dtResult.Rows[i]);
                }
                #endregion

                #region 加载radioButton
                //加载radioButton
                if (dtResult.Rows[i]["KJLX"].ToString().ToLower().Equals("radiobutton"))
                {
                    Add_radioButton(dtResult.Rows[i]);
                }
                #endregion

                #region 加载dataGridView
                //加载dataGridView
                if (dtResult.Rows[i]["KJLX"].ToString().ToLower().Equals("datagridview"))
                {
                    Add_dataGridView(dtResult.Rows[i]);
                }
                #endregion
            }

            if (panel_tem != null)
            {
                panel_tem.Height = d_y_child + 40;
            }
        }

        /// <summary>
        /// 取得控件的坐标
        /// </summary>
        /// <param name="dtRow"></param>
        public void getPoint(DataRow dtRow)
        {
            int rowNo_tem = 0;
            int jj = 0;
            //行号   换行
            rowNo_tem = int.Parse(dtRow["rowNo"].ToString());
            if (rowNo != rowNo_tem)
            {
                rowNo = int.Parse(dtRow["rowNo"].ToString());
                d_x_child = 0;
                if (ControlCount > 0)
                {
                    d_y_child = d_y_child + 30;
                }
                else
                {
                    d_y_child = d;
                }
            }

            if (dtRow["jj"] != null && dtRow["jj"].ToString().Length > 0)
            {
                jj = int.Parse(dtRow["jj"].ToString());
                d_x_child = d_x_child + jj;
            }

            #region 加载label
            //加载checkbox
            if (dtRow["KJLX"].ToString().ToLower().Equals("label"))
            {
                childPoint.X = d_x_child;
                childPoint.Y = d_y_child;
            }
            #endregion

            #region 加载linklabel
            //加载text
            if (dtRow["KJLX"].ToString().ToLower().Equals("linklabel"))
            {
                childPoint.X = d_x_child;
                childPoint.Y = d_y_child;
            }
            #endregion

            #region 加载checkbox
            //加载checkbox
            if (dtRow["KJLX"].ToString().ToLower().Equals("checkbox"))
            {
                childPoint.X = d_x_child;
                childPoint.Y = d_y_child;

            }
            #endregion

            #region 加载text
            //加载text
            if (dtRow["KJLX"].ToString().ToLower().Equals("textbox"))
            {
                if (ControlCount == 0)
                {
                    d_y_child = d;
                }

                //是否换行
                ControlCount = ControlCount + 1;
                textControlCount = textControlCount + 1;

                //加载项目对应的标签
                childPoint.X = d_x_child;
                childPoint.Y = d_y_child;
            }
            #endregion

            #region 加载radioButton
            //加载radioButton
            if (dtRow["KJLX"].ToString().ToLower().Equals("radiobutton"))
            {
                childPoint.X = d_x_child;
                childPoint.Y = d_y_child;

            }
            #endregion

            #region 加载dataGridView
            //加载dataGridView
            if (dtRow["KJLX"].ToString().ToLower().Equals("datagridview"))
            {
                d_x_child = d_x_child + 30;
                childPoint.X = d_x_child;
                childPoint.Y = d_y_child;

            }
            #endregion
        }

        /// <summary>
        /// Add_Label
        /// </summary>
        /// <param name="dtRow"></param>
        public void Add_Label(DataRow dtRow, Color color_font)
        {

            getPoint(dtRow);
            Label label_title = AddControl_label(panel_tem, childPoint, dtRow["KJXSMC"].ToString(), dtRow["KJID"].ToString(), 10F, FontStyle.Regular, ContentAlignment.BottomLeft, 0, color_font);
            //标签的最大长度固定100
            d_x_child = d_x_child + label_title.Width;
            ControlCount = ControlCount + 1;
            //textbox的当前行的数量设定为0
            textControlCount = 0;
        }

        /// <summary>
        /// Add_linklabel
        /// </summary>
        /// <param name="dtRow"></param>
        public void Add_Linklabel(DataRow dtRow, Color color_font)
        {
            getPoint(dtRow);
            LinkLabel label_title = AddControl_linklabel(panel_tem, childPoint, dtRow["KJXSMC"].ToString(), dtRow["KJID"].ToString(), 10F, FontStyle.Regular, ContentAlignment.BottomLeft, 0, color_font, dtRow);
            //标签的最大长度固定100
            d_x_child = d_x_child + label_title.Width;
            ControlCount = ControlCount + 1;
            //textbox的当前行的数量设定为0
            textControlCount = 0;
        }

        /// <summary>
        /// 添加checkBox
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="dtRow"></param>
        /// <param name="childPoint"></param>
        /// <param name="d_x_child"></param>
        /// <param name="d_y_child"></param>
        /// <param name="ControlCount"></param>
        /// <param name="textControlCount"></param>
        public void Add_checkBox(DataRow dtRow)
        {

            getPoint(dtRow);
            Label label_title = AddControl_label(panel_tem, childPoint, dtRow["KJXSMC"].ToString(), dtRow["KJID"].ToString(), 10F, FontStyle.Regular, ContentAlignment.BottomLeft, 0, Color.FromArgb(31, 167, 150));
            //标签的最大长度固定100
            d_x_child = d_x_child + label_title.Width;

            childPoint.X = d_x_child;
            childPoint.Y = d_y_child;
            childPoint = AddControl_checkBox(panel_tem, childPoint, dtRow);

            d_y_child = childPoint.Y;
            d_x_child = childPoint.X;
            ControlCount = ControlCount + 1;
            //textbox的当前行的数量设定为0
            textControlCount = 0;

        }

        /// <summary>
        /// 加载text
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="dtRow"></param>
        /// <param name="childPoint"></param>
        /// <param name="d_x_child"></param>
        /// <param name="d_y_child"></param>
        /// <param name="ControlCount"></param>
        /// <param name="textControlCount"></param>
        public void Add_Textbox(DataRow dtRow)
        {

            //加载项目对应的标签
            getPoint(dtRow);
            if (dtRow["KJXSMC"].ToString().Trim().Length > 0)
            {
                Label label_title = AddControl_label(panel_tem, childPoint, dtRow["KJXSMC"].ToString(), dtRow["KJID"].ToString(), 10F, FontStyle.Regular, ContentAlignment.TopRight, 0, Color.FromArgb(31, 167, 150));

                d_x_child = d_x_child + label_title.Width;
            }
            childPoint.X = d_x_child;
            childPoint.Y = d_y_child;
            TextBox textBox_tem = new TextBox();
            textBox_tem = AddControl_textBox(panel_tem, childPoint, dtRow);
            JktjCheckboxDics.Append(string.Format("{0}_{1}", dtRow["XMFLBM"], dtRow["rowNo"]), textBox_tem.Name);
            d_x_child = textBox_tem.Location.X + textBox_tem.Width;

            //加载项目对应的单位
            if (dtRow["dw"].ToString().Length > 0)
            {
                childPoint.X = d_x_child;
                childPoint.Y = d_y_child;

                Label label_dw = AddControl_label(panel_tem, childPoint, dtRow["dw"].ToString(), dtRow["KJID"].ToString() + "dw", 9F, FontStyle.Regular, ContentAlignment.TopLeft, 0, Color.Black);
                d_x_child = d_x_child + label_dw.Width;
            }
        }


        /// <summary>
        /// 加载radioButton
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="dtRow"></param>
        /// <param name="childPoint"></param>
        /// <param name="d_x_child"></param>
        /// <param name="d_y_child"></param>
        /// <param name="ControlCount"></param>
        /// <param name="textControlCount"></param>
        public void Add_radioButton(DataRow dtRow)
        {
            //加载项目对应的标签
            getPoint(dtRow);
            Label label_title = AddControl_label(panel_tem, childPoint, dtRow["KJXSMC"].ToString(), dtRow["KJID"].ToString(), 10F, FontStyle.Regular, ContentAlignment.BottomLeft, 0, Color.FromArgb(31, 167, 150));
            //标签的最大长度固定100
            d_x_child = d_x_child + label_title.Width;

            childPoint.X = d_x_child;
            childPoint.Y = d_y_child;
            childPoint = AddControl_radioButton(panel_tem, childPoint, dtRow);
            d_y_child = childPoint.Y;
            d_x_child = childPoint.X;
            ControlCount = ControlCount + 1;
            //textbox的当前行的数量设定为0
            textControlCount = 0;
        }


        /// <summary>
        /// 加载dataGridView
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="dtRow"></param>
        /// <param name="childPoint"></param>
        /// <param name="d_x_child"></param>
        /// <param name="d_y_child"></param>
        /// <param name="ControlCount"></param>
        /// <param name="textControlCount"></param>
        public void Add_dataGridView(DataRow dtRow)
        {
            //加载项目对应的标签
            getPoint(dtRow);
            DataGridView DataGridView_tem = AddControl_dataGridView(panel_tem, childPoint, dtRow);

            d_y_child = d_y_child + DataGridView_tem.Height;

            ControlCount = ControlCount + 1;
            //textbox的当前行的数量设定为0
            textControlCount = 0;
        }


        /// <summary>
        /// 添加控件label
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        private Label AddControl_label(GroupBox parentPanel, Point Location, string labelText, string ControlId, float font, FontStyle FontStyleBold, ContentAlignment textAlign, int labelWidth, Color color_ForeColor)
        {
            Label label_tem = new System.Windows.Forms.Label();
            label_tem.Name = controlType_lable + ControlId;
            label_tem.Text = labelText;
            Location.Y = Location.Y + 5;
            label_tem.Location = Location;
            label_tem.AutoSize = false;
            label_tem.Font = new System.Drawing.Font("微软雅黑", font, FontStyleBold);
            label_tem.ForeColor = color_ForeColor;

            //TabIndex_p = TabIndex_p + 1;
            label_tem.TabIndex = TabIndex_p;
            label_tem.TabStop = false;

            label_tem.TextAlign = textAlign;
            if (labelWidth == 0)
            {
                label_tem.AutoSize = true;
            }
            else
            {
                label_tem.Width = labelWidth;
            }

            // label_tem.BackColor = Color.Red ;
            parentPanel.Controls.Add(label_tem);
            return label_tem;
        }

        /// <summary>
        /// 添加控件linklabel
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        private LinkLabel AddControl_linklabel(GroupBox parentPanel, Point Location, string labelText, string ControlId, float font, FontStyle FontStyleBold, ContentAlignment textAlign, int labelWidth, Color color_ForeColor, DataRow dtrow)
        {
            LinkLabel label_tem = new System.Windows.Forms.LinkLabel();
            label_tem.Name = controlType_LinkLabel + ControlId;
            label_tem.Text = labelText;
            Location.Y = Location.Y + 5;
            label_tem.Location = Location;
            label_tem.AutoSize = false;
            label_tem.Font = new System.Drawing.Font("微软雅黑", font, FontStyleBold);
            label_tem.ForeColor = color_ForeColor;


            label_tem.TabIndex = TabIndex_p;
            label_tem.TabStop = false;

            label_tem.TextAlign = textAlign;
            if (labelWidth == 0)
            {
                label_tem.AutoSize = true;
            }
            else
            {
                label_tem.Width = labelWidth;
            }

            //绑定事件
            label_tem.LinkClicked += linkLabel_img_LinkClicked;

            // label_tem.BackColor = Color.Red ;
            parentPanel.Controls.Add(label_tem);

            addControlToList(dtrow, label_tem.Name, "", "", dtrow["HIS_DB"].ToString());
            return label_tem;
        }

        /// <summary>
        /// 添加控件label
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        private Label AddControl_label(Panel parentPanel, Point Location, string labelText, string ControlId, float font, FontStyle FontStyleBold, ContentAlignment textAlign)
        {
            Label label_tem = new System.Windows.Forms.Label();
            label_tem.Name = controlType_lable + ControlId;
            label_tem.Text = labelText;
            Location.Y = Location.Y + 5;
            label_tem.Location = Location;
            label_tem.AutoSize = false;
            label_tem.Font = new System.Drawing.Font("宋体", font, FontStyleBold);
            TabIndex_p = TabIndex_p + 1;
            label_tem.TabIndex = TabIndex_p;
            label_tem.TabStop = false;
            label_tem.BackColor = Color.RoyalBlue;
            label_tem.TextAlign = textAlign;
            parentPanel.Controls.Add(label_tem);
            return label_tem;
        }

        /// <summary>
        /// 添加控件line
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        private void AddControl_line(Panel parentPanel, Point Location, string lineType, string ControlId)
        {
            GroupBox groupBox_tem = new GroupBox();
            groupBox_tem.Width = parentPanel.Width - 20;
            groupBox_tem.Height = 5;
            groupBox_tem.Name = controlType_line + ControlId;
            groupBox_tem.Location = Location;
            TabIndex_p = TabIndex_p + 1;
            groupBox_tem.TabIndex = TabIndex_p;
            groupBox_tem.TabStop = false;
            if (lineType.Trim().ToLower().Equals("head"))
            {
                groupBox_tem.BackColor = Color.FromArgb(224, 249, 255);
            }
            parentPanel.Controls.Add(groupBox_tem);
        }
        /// <summary>
        /// 添加控件Panel
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        private Panel AddControl_Panel(GroupBox parentPanel, Point Location, string ControlId)
        {
            Panel panel_tem = new Panel();
            panel_tem.Location = Location;
            panel_tem.Name = controlType_panel + ControlId;
            panel_tem.Width = parentPanel.Width - 20;
            parentPanel.Controls.Add(panel_tem);
            panel_tem.AutoSize = true;
            return panel_tem;
        }
        /// <summary>
        /// 添加控件Panel
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        private GroupBox AddControl_GroupBox(Panel parentPanel, Point Location, string ControlId, string xmflmc)
        {
            //
            GroupBox panel_tem = new GroupBox();
            string xmflmc_tem = "•";
            for (int i = 0; i < xmflmc.Length + 2; i++)
            {

                xmflmc_tem = xmflmc_tem + "　";
            }
            xmflmc_tem = xmflmc_tem + "•";

            panel_tem.Text = xmflmc_tem;
            panel_tem.Font = new System.Drawing.Font("宋体", 11, FontStyle.Regular);
            panel_tem.Location = Location;
            panel_tem.Name = controlType_panel + ControlId;
            panel_tem.Paint += groupBox_Paint;
            parentPanel.Controls.Add(panel_tem);
            panel_tem.Width = parentPanel.Width - 80;
            panel_tem.Tag = "　" + xmflmc;
            groupbox_title = 0;
            //panel_tem.AutoSize = true;
            return panel_tem;
        }

        private Panel AddPanel(GroupBox parentPanel)
        {
            Panel panel = new Panel();
            panel.Width = parentPanel.Width;
            //panel.BackColor = Color.Red;
            return panel;
        }
        /// <summary>
        /// 添加控件checkBox
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="dtrow"></param>
        /// <returns></returns>
        private Point AddControl_checkBox(GroupBox parentPanel, Point Location, DataRow dtrow)
        {
            //获取该工作组对应的体检项目
            DataTable checkList = getSjzdList(dtrow["SJZDBM"].ToString());
            CheckBox checkbox_tem = new CheckBox();
            int d_x_tem = 5;
            int d_y_tem = 0;
            int d_x = 5;
            int d_y = 0;
            int d_weight = 5;
            int d_height = 5;

            d_x_tem = Location.X;
            d_y_tem = Location.Y;
            d_x = d_x_tem;
            d_y = d_y_tem;

            if (checkList != null)
            {
                //添加panel控件，在panel空间中加入checkbox控件
                //Panel panel = AddPanel(parentPanel);
                //parentPanel.Controls.Add(panel);
                //添加CheckBox控件
                for (int i = 0; i < checkList.Rows.Count; i++)
                {
                    Location.X = d_x;
                    Location.Y = d_y;
                    checkbox_tem = createCheckBox(dtrow, checkList.Rows[i]["ZDBM"].ToString(), checkList.Rows[i]["ZDMC"].ToString(), Location);

                    JktjCheckboxDics.Add(string.Format("{0}_{1}", dtrow["XMFLBM"], dtrow["rowNo"]), new List<string>() { checkbox_tem.Name });

                    //checkbox_tem.BackColor = Color.Red;
                    parentPanel.Controls.Add(checkbox_tem);
                    //panel.Controls.Add(checkbox_tem);
                    //控件换行
                    d_x = d_x + checkbox_tem.Width + d_weight;
                    if (d_x > parentPanel.Width)
                    {
                        d_x = d_x_tem;
                        d_y = d_y + checkbox_tem.Height + d_height;
                        Location.X = d_x;
                        Location.Y = d_y;
                        checkbox_tem.Location = Location;
                        d_x = d_x + checkbox_tem.Width + d_weight;
                    }
                }
                checkbox_tem.AutoSize = true;
                Location.X = Location.X + checkbox_tem.Width;

            }
            return Location;
        }
        /// <summary>
        /// 创建一个新的CheckBox控件
        /// </summary>
        /// <param name="dtrow"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private CheckBox createCheckBox(DataRow dtrow, string value, string text, Point location)
        {
            TabIndex = TabIndex + 1;

            CheckBox checkbox_tem = new CheckBox();
            checkbox_tem.Name = controlType_checkBox + dtrow["KJID"].ToString() + "_" + value;
            checkbox_tem.Tag = value;
            checkbox_tem.Text = text;
            location.Y = location.Y + 5;
            checkbox_tem.Location = location;
            //设定默认值
            if (dtrow["kjmrz"] != null)
            {
                if (("," + dtrow["kjmrz"].ToString() + ",").IndexOf("," + value + ",") > -1)
                {
                    checkbox_tem.Checked = true;
                }
            }
            checkbox_tem.CheckStateChanged += checkBox_CheckStateChanged;
            checkbox_tem.KeyDown += Enter_Tab_KeyDown;
            checkbox_tem.Enter += Control_Enter;
            checkbox_tem.Leave += Control_Leave;
            checkbox_tem.Click += JktjCheckboxClick;
            checkbox_tem.TabIndex = TabIndex;
            Font font = new System.Drawing.Font("微软雅黑", 9F, FontStyle.Regular);
            checkbox_tem.Font = font;
            checkbox_tem.ForeColor = Color.Black;



            //控件宽度设定
            if ("".Equals(dtrow["KJKD"].ToString().Trim()) || int.Parse(dtrow["KJKD"].ToString().Trim()) <= 0)
            {
                checkbox_tem.AutoSize = true;
            }
            else
            {
                checkbox_tem.Width = int.Parse(dtrow["KJKD"].ToString().Trim());
            }
            addControlToList(dtrow, checkbox_tem.Name, checkbox_tem.CheckState.ToString(), value, dtrow["HIS_DB"].ToString());
            return checkbox_tem;
        }

        //添加checkbox的点击事件
        private void JktjCheckboxClick(object sender, EventArgs e)
        {
            CheckBox currBox = (CheckBox)sender;
            Control controlParent = currBox.Parent;
            //根据checkbox的name去字典里找
            KeyValuePair<string, List<string>> dics = JktjCheckboxDics.GetAllByValue(currBox.Name);
            if (string.IsNullOrEmpty(dics.Key))  //如果字典为空，退出
            {
                return;
            }
            List<string> defaultValues = new List<string>() { "正常", "未发现", "未见异常", "无症状" };
            string text = currBox.Text.Trim();
            if (defaultValues.Contains(text))
            {
                foreach (var value in dics.Value)
                {
                    Control[] currControls = controlParent.Controls.Find(value, true);
                    foreach (var curr in currControls)
                    {
                        if (curr is CheckBox)
                        {
                            CheckBox chk = (CheckBox)curr;
                            if (!defaultValues.Contains(chk.Text.Trim()))
                            {
                                chk.Checked = false;
                            }
                            continue;
                        }
                        if (curr is TextBox)
                        {
                            TextBox txt = (TextBox)curr;
                            txt.Text = "";
                        }
                    }
                }
                return;
            }

            foreach (var value in dics.Value)
            {
                Control[] currControls = controlParent.Controls.Find(value, true);
                foreach (var curr in currControls)
                {
                    if (curr is CheckBox)
                    {
                        CheckBox chk = (CheckBox)curr;
                        if (defaultValues.Contains(chk.Text.Trim()))
                        {
                            chk.Checked = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加控件textbox
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="dtrow"></param>
        /// <returns></returns>
        private TextBox AddControl_textBox(GroupBox parentPanel, Point Location, DataRow dtrow)
        {
            string textIdList = dtrow["KJID"].ToString();
            string[] textboxIdList = textIdList.Split(new char[] { ',' });
            string[] HIS_DB = dtrow["HIS_DB"].ToString().Split(new char[] { ',' });


            TextBox textbox_tem = null;
            for (int i = 0; i < textboxIdList.Length; i++)
            {
                textbox_tem = new TextBox();
                textbox_tem.Name = controlType_textBox + textboxIdList[i];
                //textbox_tem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                //控件宽度设定
                if ("".Equals(dtrow["KJKD"].ToString().Trim()) || int.Parse(dtrow["KJKD"].ToString().Trim()) <= 0)
                {
                    textbox_tem.Width = 50;

                    //Location.X = Location.X +  50;
                }
                else
                {
                    textbox_tem.Width = int.Parse(dtrow["KJKD"].ToString().Trim());
                    //Location.X = Location.X + textbox_tem.Width;
                }
                //控件高度设定
                if ("".Equals(dtrow["KJGD"].ToString().Trim()) || int.Parse(dtrow["KJGD"].ToString().Trim()) <= 0)
                {
                    textbox_tem.Height = 10;
                }
                else
                {
                    textbox_tem.Height = int.Parse(dtrow["KJGD"].ToString().Trim());
                }

                Location.Y = Location.Y;
                textbox_tem.Location = Location;
                //设定默认值
                if (dtrow["kjmrz"] != null)
                {
                    textbox_tem.Text = dtrow["kjmrz"].ToString();
                }

                //辅助录入信息
                textbox_tem.Tag = dtrow["fzlritem"].ToString();

                //textbox_tem.Leave += textBox_Leave;
                textbox_tem.KeyDown += Enter_Tab_KeyDown;
                textbox_tem.Enter += Control_Enter;
                textbox_tem.Leave += Control_Leave;
                textbox_tem.TextChanged += TextChanged;

                TabIndex = TabIndex + 1;
                textbox_tem.TabIndex = TabIndex;
                //文本框双击事件
                textbox_tem.DoubleClick += textBox_DoubleClick;

                //控件置于顶层
                textbox_tem.TextAlign = HorizontalAlignment.Center;
                textbox_tem.BorderStyle = System.Windows.Forms.BorderStyle.None;

                //线
                Label label1 = new Label();
                label1.BackColor = System.Drawing.Color.Black;
                label1.Location = new System.Drawing.Point(Location.X, Location.Y + textbox_tem.Height - 3);
                label1.Size = new System.Drawing.Size(textbox_tem.Width, 1);
                label1.BringToFront();

                parentPanel.Controls.Add(textbox_tem);
                parentPanel.Controls.Add(label1);

                if (HIS_DB.Length == 1)
                {
                    addControlToList(dtrow, textbox_tem.Name, textbox_tem.Text, "", HIS_DB[0]);
                }
                else
                {
                    addControlToList(dtrow, textbox_tem.Name, textbox_tem.Text, "", HIS_DB[i]);
                }
                //添加间隔符号
                if (i < textboxIdList.Length - 1)
                {
                    Location.X = Location.X + textbox_tem.Width;
                    Label label_tem = new System.Windows.Forms.Label();
                    label_tem.Name = controlType_lable + textboxIdList[i] + i.ToString();
                    label_tem.Text = "/";
                    label_tem.Location = Location;
                    label_tem.Font = new System.Drawing.Font("宋体", 12, FontStyle.Bold);
                    label_tem.AutoSize = true;
                    label_tem.ForeColor = Color.Black;
                    // label_tem.Width = 10; 
                    label_tem.TextAlign = ContentAlignment.BottomLeft;
                    parentPanel.Controls.Add(label_tem);
                    Location.X = Location.X + label_tem.Width;
                }
            }

            return textbox_tem;
        }

        /// <summary>
        /// 添加控件radioButton
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="dtrow"></param>
        /// <returns></returns>
        private Point AddControl_radioButton(GroupBox parentPanel, Point Location, DataRow dtrow)
        {

            //创建一个容器保存单选框按钮
            Panel panel_tem = AddControl_Panel(parentPanel, Location, dtrow["KJID"].ToString());

            panel_tem.Height = 25;
            Point Location_tem = new Point();
            panel_tem.AutoSize = false;
            panel_tem.SendToBack();

            //获取该工作组对应的体检项目
            DataTable checkList = getSjzdList(dtrow["SJZDBM"].ToString());
            RadioButton radioButton_tem = new RadioButton();
            int d_x_tem = 0;
            int d_y_tem = 0;
            int d_x = 5;
            int d_y = 0;
            int d_weight = 5;
            int d_height = 5;
            int panelWidth = 0;

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

                    bool ischecked = false;
                    //if (dtrow["KJMRZ"] != null && dtrow["KJMRZ"].ToString().Equals(checkList.Rows[i]["ZDBM"].ToString()))
                    //{
                    //    ischecked = true;
                    //}
                    radioButton_tem = createRadioButton(dtrow, checkList.Rows[i]["ZDBM"].ToString(), checkList.Rows[i]["ZDMC"].ToString(), Location_tem, ischecked);
                    //radioButton_tem.AutoSize = true;
                    //控件宽度设定
                    if ("".Equals(dtrow["KJKD"].ToString().Trim()) || int.Parse(dtrow["KJKD"].ToString().Trim()) <= 0)
                    {
                        radioButton_tem.AutoSize = true;
                    }
                    else
                    {
                        radioButton_tem.Width = int.Parse(dtrow["KJKD"].ToString().Trim());
                    }
                    panel_tem.Controls.Add(radioButton_tem);

                    //控件换行
                    d_x = d_x + radioButton_tem.Width + d_weight;
                    if (d_x > panel_tem.Width)
                    {
                        panelWidth = d_x;
                        d_x = d_x_tem;
                        d_y = d_y + radioButton_tem.Height + d_height;
                        Location_tem.X = d_x;
                        Location_tem.Y = d_y;
                        radioButton_tem.Location = Location_tem;
                    }
                }
                if (panelWidth == 0)
                {
                    panelWidth = d_x;
                }
                panel_tem.Width = panelWidth;
                Location.X = Location.X + d_x;
            }
            else
            {
                panel_tem.Width = panelWidth;
            }
            Location.Y = Location.Y + Location_tem.Y;
            return Location;

        }
        /// <summary>
        /// 创建单选按钮
        /// </summary>
        /// <param name="dtrow"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private RadioButton createRadioButton(DataRow dtrow, string value, string text, Point location, bool ischecked)
        {
            RadioButton radioButton_tem = new RadioButton();
            try
            {
                TabIndex = TabIndex + 1;

                radioButton_tem.Name = controlType_radioButton + dtrow["KJID"].ToString() + "_" + value;
                radioButton_tem.Tag = value;
                radioButton_tem.Text = text;
                location.Y = location.Y + 5;
                radioButton_tem.Location = location;

                //设定默认值
                if (dtrow["kjmrz"] != null)
                {
                    if (("," + dtrow["kjmrz"].ToString() + ",").IndexOf("," + value + ",") > -1)
                    {
                        radioButton_tem.Checked = true;
                    }
                }


                radioButton_tem.CheckedChanged += radioButton_CheckedChanged;
                radioButton_tem.KeyDown += Enter_Tab_KeyDown;
                radioButton_tem.Enter += Control_Enter;
                radioButton_tem.Leave += Control_Leave;
                radioButton_tem.TabIndex = TabIndex;
                Font font = new System.Drawing.Font("微软雅黑", 9F, FontStyle.Regular);
                radioButton_tem.Font = font;
                radioButton_tem.ForeColor = Color.Black;


                addControlToList(dtrow, radioButton_tem.Name, radioButton_tem.Checked.ToString(), value, dtrow["HIS_DB"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return radioButton_tem;
        }


        /// <summary>
        /// 添加控件dataGridView
        /// </summary>
        /// <param name="parentPanel"></param>
        /// <param name="Location"></param>
        /// <param name="dtrow"></param>
        /// <returns></returns>
        private DataGridView AddControl_dataGridView(GroupBox parentPanel, Point Location, DataRow dtrow)
        {
            DataGridView GridView_tem = createDataGridView(dtrow);

            GridView_tem.Name = controlType_dataGridView + dtrow["KJID"].ToString();
            GridView_tem.Location = Location;
            GridView_tem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            //控件宽度设定
            if ("".Equals(dtrow["KJKD"].ToString().Trim()) || int.Parse(dtrow["KJKD"].ToString().Trim()) <= 0)
            {
                GridView_tem.Width = parentPanel.Width - 200;
            }
            else
            {
                GridView_tem.Width = int.Parse(dtrow["KJKD"].ToString().Trim());
            }
            //控件高度设定
            if ("".Equals(dtrow["KJGD"].ToString().Trim()) || int.Parse(dtrow["KJGD"].ToString().Trim()) <= 0)
            {
                GridView_tem.Height = 150;
            }
            else
            {
                GridView_tem.Height = int.Parse(dtrow["KJGD"].ToString().Trim());
            }
            //GridView_tem.Leave += textBox_Leave;
            GridView_tem.KeyDown += Enter_Tab_KeyDown;
            //GridView_tem.Enter += Control_Enter;
            //GridView_tem.Leave += Control_Leave;
            TabIndex = TabIndex + 1;
            GridView_tem.TabIndex = TabIndex;

            parentPanel.Controls.Add(GridView_tem);

            addControlToList(dtrow, GridView_tem.Name, "", "", dtrow["HIS_DB"].ToString());

            return GridView_tem;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtrow"></param>
        /// <returns></returns>
        private DataGridView createDataGridView(DataRow dtrow)
        {
            DataGridView GridView_tem = new DataGridView();
            //绑定事件         
            GridView_tem.SelectionMode = DataGridViewSelectionMode.CellSelect;
            GridView_tem.DataBindingComplete += GridView_tem_DataBindingComplete;
            GridView_tem.CellEnter += dataGridView_CellEnter;
            //GridView_tem.CellClick += dataGridView_CellClick;
            GridView_tem.CellContentClick += dataGridView_CellContentClick;

            DataTable dtList = new DataTable();
            string[] columnList = null;
            string[] columnValueList = null;
            if (dtrow["HIS_DB"].ToString().Length > 0)
            {
                columnList = dtrow["HIS_DB"].ToString().Split(new char[] { '$' });
            }

            if (columnList.Length > 0)
            {
                for (int i = 0; i < columnList.Length; i++)
                {
                    columnValueList = columnList[i].Split(new char[] { ':' });
                    if (columnValueList.Length == 3)
                    {
                        dtList.Columns.Add(columnValueList[1]);
                        if (columnValueList[2].Length > 0)
                        {
                            DataGridViewComboBoxColumn Column_tem = new DataGridViewComboBoxColumn();
                            DataTable dt = new DataTable();

                            dt.Columns.Add("value");
                            dt.Columns.Add("text");
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["text"] = "--请选择--";
                            string[] ComboBoxlistValue = columnValueList[2].Split(new char[] { '|' });
                            for (int j = 0; j < ComboBoxlistValue.Length; j++)
                            {
                                string[] listValue = ComboBoxlistValue[j].Split(new char[] { ';' });
                                dt.Rows.Add();
                                dt.Rows[dt.Rows.Count - 1]["value"] = listValue[1];
                                dt.Rows[dt.Rows.Count - 1]["text"] = listValue[0];
                            }
                            Column_tem.DataSource = dt;
                            Column_tem.DisplayMember = "text";
                            Column_tem.ValueMember = "value";
                            Column_tem.HeaderText = columnValueList[0];
                            Column_tem.DataPropertyName = columnValueList[1];
                            GridView_tem.Columns.Add(Column_tem);
                        }
                        else
                        {
                            DataGridViewTextBoxColumn TextBox_tem = new DataGridViewTextBoxColumn();
                            TextBox_tem.HeaderText = columnValueList[0];
                            TextBox_tem.DataPropertyName = columnValueList[1];
                            GridView_tem.Columns.Add(TextBox_tem);
                        }
                    }
                }

                DataGridViewLinkColumn LinkColumn_tem = new DataGridViewLinkColumn();
                LinkColumn_tem.Name = "delColumn";
                LinkColumn_tem.HeaderText = "操作";
                LinkColumn_tem.Text = "删除";
                LinkColumn_tem.Width = 60;

                LinkColumn_tem.UseColumnTextForLinkValue = true;
                GridView_tem.Columns.Add(LinkColumn_tem);

                GridView_tem.DataSource = dtList.Copy();
            }

            return GridView_tem;
        }

        private void GridView_tem_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView DataGridView_tem = (DataGridView)sender;
            DataGridView_tem.ClearSelection();
            DataGridView_tem.CurrentCell = null;
            DataGridView_tem.Rows[0].Selected = false;
        }

        /// <summary>
        /// DataGridView获得焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (datagridview_bool == false) { return; }
            try
            {
                DataGridView DataGridView_tem = (DataGridView)sender;
                DataGridView_tem.BeginEdit(true);

                //弹出药物框
                if (e.ColumnIndex == 1)
                {
                    YWDropDownGrid dgv_yw = new YWDropDownGrid();
                    if (dgv_yw.ShowDialog() == DialogResult.OK)
                    {
                        SendKeys.SendWait(dgv_yw.resultStr);
                    }

                    SendKeys.Send("{Tab}");//转换为Tab键 
                }

                DataGridViewComboBoxColumn comboBoxColumn = DataGridView_tem.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
                if (comboBoxColumn != null)
                {
                    DataGridViewComboBoxEditingControl comboBoxEditingControl = DataGridView_tem.EditingControl as DataGridViewComboBoxEditingControl;
                    if (comboBoxEditingControl != null)
                    {
                        comboBoxEditingControl.DroppedDown = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// DataGridView单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        //{            
        //    if (datagridview_bool == false) { return; }
        //    try
        //    {               
        //        DataGridView DataGridView_tem = (DataGridView)sender;
        //        if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && DataGridView_tem[e.ColumnIndex, e.RowIndex] != null && !DataGridView_tem[e.ColumnIndex, e.RowIndex].ReadOnly)
        //        {                    
        //            /*DataGridViewComboBoxColumn comboBoxColumn = DataGridView_tem.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
        //            if (comboBoxColumn != null)
        //            {
        //                DataGridView_tem.BeginEdit(true);
        //                DataGridViewComboBoxEditingControl comboBoxEditingControl = DataGridView_tem.EditingControl as DataGridViewComboBoxEditingControl;
        //                if (comboBoxEditingControl != null)
        //                {
        //                    comboBoxEditingControl.DroppedDown = true;
        //                }
        //            }*/
        //        }

        //        //设定系统参数改项目被编辑过
        //        CommomSysInfo.IsEdited = "1";
        //    }
        //    catch (Exception ex) { }
        //}

        /// <summary>
        /// DataGridView删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView DataGridView_tem = (DataGridView)sender;

                if (DataGridView_tem.Columns[e.ColumnIndex].Name == "delColumn")
                {
                    int d = e.RowIndex;
                    Object obj_dt = DataGridView_tem.DataSource;
                    DataTable dt = null;
                    if (obj_dt != null)
                    {
                        dt = obj_dt as DataTable;

                        if (d >= 0 && d < dt.Rows.Count)
                        {
                            dt.Rows.RemoveAt(d);
                        }
                    }

                    //设定系统参数改项目被编辑过
                    CommomSysInfo.IsEdited = "1";
                }
            }

        }
        #endregion

        #region 窗体移动

        private void Form_Client_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mBeginMove = true;

                // 鼠标的x坐标为当前窗体左上角x坐标 
                mCurrentX = MousePosition.X;

                // 鼠标的y坐标为当前窗体左上角y坐标
                mCurrentY = MousePosition.Y;
            }
        }

        private void Form_Client_MouseMove(object sender, MouseEventArgs e)
        {
            if (mBeginMove)
            {
                //根据鼠标x坐标确定窗体的左边坐标x 
                this.Left += MousePosition.X - mCurrentX;

                //根据鼠标的y坐标窗体的顶部，即Y坐标 
                this.Top += MousePosition.Y - mCurrentY;
                mCurrentX = MousePosition.X;
                mCurrentY = MousePosition.Y;
            }

        }

        private void Form_Client_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //设置初始状态    
                mCurrentX = 0;
                mCurrentY = 0;
                mBeginMove = false;
            }

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
        /// checkBox改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_CheckStateChanged(object sender, EventArgs e)
        {
            ////控件初始化时，该事件不触发
            //if (initStatue_right == false)
            //{
            //    return;
            //}

            //设定系统参数改项目被编辑过
            CommomSysInfo.IsEdited = "1";

            CheckBox demo = new CheckBox();
            demo = (CheckBox)sender;
            //设定控件的控制关系
            get_T_JK_TJXMGLKZ();
            init_TJXMGLKZ(demo.Tag != null ? demo.Tag.ToString() : "", demo.Name);
        }

        /// <summary>
        /// radioButton改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            ////控件初始化时，该事件不触发
            //if (initStatue_right == false)
            //{
            //    return;
            //}

            //设定系统参数改项目被编辑过
            CommomSysInfo.IsEdited = "1";

            RadioButton demo = new RadioButton();
            demo = (RadioButton)sender;
            //控件间的控制关系
            get_T_JK_TJXMGLKZ();
            init_TJXMGLKZ(demo.Tag != null ? demo.Tag.ToString() : "", demo.Name);
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
        /// Enter转换为tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Tab_KeyDown(object sender, KeyEventArgs e)
        {
            ////控件初始化时，该事件不触发
            //if (initStatue_right == false)
            //{
            //    return;
            //}
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
            // TextBox_tem_fzlr = text_tem;
            //辅助录入
            if (!CommomSysInfo.IsFzlr.Equals("1"))
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
                //if ("richTextBox_XCSFMB_lkzp".Equals(textbox.Name))
                //{
                //    RichTextBox 
                //}
                //TextBox text_tem = (TextBox)textbox;
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
        /// 按钮的keyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                SendKeys.Send("{Tab}");
                //e.Handled = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                SendKeys.Send("+{Tab}");
                e.Handled = false;
            }
        }


        #endregion

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
        /// 绘制边线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {

            GroupBox groupBox1 = groupBox1 = (GroupBox)sender;
            if (groupbox_title == 0)
            {

                Font font = new System.Drawing.Font("微软雅黑", 11F, FontStyle.Bold);
                // 117,117,117
                //Brush Brush_tem = new Brush();
                e.Graphics.DrawString(groupBox1.Tag.ToString(), font, Brushes.Black, 10, 1);
            }
            Pen pen = new Pen(Color.FromArgb(210, 220, 255));
            e.Graphics.DrawLine(pen, 1, 7, 8, 7);

            e.Graphics.DrawLine(pen, e.Graphics.MeasureString(groupBox1.Text, groupBox1.Font).Width + 8, 7, groupBox1.Width - 2, 7);


            e.Graphics.DrawLine(pen, 1, 7, 1, groupBox1.Height - 2);


            e.Graphics.DrawLine(pen, 1, groupBox1.Height - 2, groupBox1.Width - 2, groupBox1.Height - 2);


            e.Graphics.DrawLine(pen, groupBox1.Width - 2, 7, groupBox1.Width - 2, groupBox1.Height - 2);

        }


        /// <summary>
        /// 获得焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_Enter(object sender, EventArgs e)
        {
            ////控件初始化时，该事件不触发
            //if (initStatue_right == false)
            //{
            //    return;
            //}
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
            ////控件初始化时，该事件不触发
            //if (initStatue_right == false)
            //{
            //    return;
            //}

            Control control = (Control)sender;
            control.ForeColor = Color.FromArgb(0, 0, 0);
            control.BackColor = color_tem;

            //焦点离开时验证内容
            checkList(control.Name, control);
        }

        /// <summary>
        ///  值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChanged(object sender, EventArgs e)
        {
            //设定系统参数改项目被编辑过
            CommomSysInfo.IsEdited = "1";
        }

        /// <summary>
        /// 控件验证
        /// </summary>
        public void checkList(string controlId, Control control)
        {
            //文本框验证
            if (controlId.IndexOf(controlType_textBox) > -1)
            {
                textBoxCheck(control.Name, control);
            }
        }

        /// <summary>
        /// 文本框内容验证
        /// </summary>
        /// <param name="controlId"></param>
        public void textBoxCheck(string controlId, Control control)
        {
            try
            {
                //身高
                if (controlId.ToLower().IndexOf("_g_sg") > -1 && controlId.Length == 12)
                {
                    TextBox textBox_g_sg_tem = (TextBox)control;
                    //清空上次体检结果
                    //if (dt_T_JK_JKTJ_TMP == null)
                    //{
                    jktjBll jktjbll = new jktjBll();
                    dt_T_JK_JKTJ_TMP = jktjbll.GetMoHuList(string.Format(" and D_GRDABH='{0}'", getValueFromDt(dt_paraFromParent, 0, "JKDAH")), "sql066");
                    //}

                    if (dt_T_JK_JKTJ_TMP != null && dt_T_JK_JKTJ_TMP.Rows.Count > 0)
                    {
                        string g_sg_tem = "";
                        if (dt_T_JK_JKTJ_TMP.Rows[0]["g_sg"] != null && dt_T_JK_JKTJ_TMP.Rows[0]["g_sg"].ToString().Length > 0)
                        {
                            g_sg_tem = dt_T_JK_JKTJ_TMP.Rows[0]["g_sg"].ToString();
                            Decimal sg_Decimal = Convert.ToDecimal(g_sg_tem) - Convert.ToDecimal(textBox_g_sg_tem.Text);
                            string str_sgc = System.Configuration.ConfigurationManager.AppSettings["shengaocha"];
                            Decimal d = 5;
                            if (str_sgc != null && str_sgc.Length > 0)
                            {
                                d = Convert.ToDecimal(str_sgc);
                            }

                            if (Math.Abs(sg_Decimal) > d)
                            {
                                string msg = string.Format("身高：上次体检为[{0}]，本次体检为[{1}]，相差{2}，是否修改身高信息？", g_sg_tem, textBox_g_sg_tem.Text, sg_Decimal.ToString());
                                DialogResult result;
                                result = MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    //修改信息
                                    textBox_g_sg_tem.SelectAll();
                                    textBox_g_sg_tem.Focus();
                                }
                            }
                        }
                    }
                }

                //bmi指数计算
                if ((controlId.ToLower().IndexOf("_g_sg") > -1 && controlId.Length == 12) || controlId.ToLower().IndexOf("_g_tzh") > -1)
                {
                    //身高
                    string str_g_sg = GetControlValueByName(controlType_textBox + "g_sg");
                    //体重
                    string str_g_tzh = GetControlValueByName(controlType_textBox + "g_tzh");

                    if (str_g_sg.Length > 0 && str_g_tzh.Length > 0)
                    {
                        double sg = double.Parse(str_g_tzh) / double.Parse(str_g_sg) / double.Parse(str_g_sg) * 10000;
                        setValueToControl(controlType_textBox + "g_tzhzh", Math.Round(sg, 2).ToString());
                    }
                }

                //腰围计算
                if ((controlId.ToLower().IndexOf("_g_yw") > -1 && controlId.Length == 12) && CommomSysInfo.IsYwhs.Equals("1"))
                {
                    //腰围
                    string str_g_yw = GetControlValueByName(controlType_textBox + "g_yw");

                    if (str_g_yw.Length > 0)
                    {
                        double sg = double.Parse(str_g_yw);
                        if (sg < 10)
                        {
                            sg = sg * 30 + 7;
                        }
                        else if (sg > 10 && sg < 50)
                        {
                            sg = sg * 3 + 7;
                        }
                        setValueToControl(controlType_textBox + "g_yw", Math.Round(sg, 2).ToString());
                    }
                }

                //血压按照一侧自动计算另一侧 0:不计算  1：左侧 2：右侧
                string str_autoXy = System.Configuration.ConfigurationManager.AppSettings["autoXy"];
                string str_autoXy_D_SZ = System.Configuration.ConfigurationManager.AppSettings["autoXy_D_SZ"];
                string str_autoXy_D_SS = System.Configuration.ConfigurationManager.AppSettings["autoXy_D_SS"];

                int int_autoXy_d_SZ = 5;
                int int_autoXy_d_SS = 5;
                if (str_autoXy_D_SZ.Length > 0)
                {
                    int_autoXy_d_SZ = Convert.ToInt32(str_autoXy_D_SZ);
                }

                if (str_autoXy_D_SS.Length > 0)
                {
                    int_autoXy_d_SS = Convert.ToInt32(str_autoXy_D_SS);
                }

                //2：右侧 按照左侧计算右侧
                if (str_autoXy.Equals("2"))
                {
                    //右侧血压设定
                    if ((controlId.ToLower().IndexOf("_g_xyzc1") > -1) || controlId.ToLower().IndexOf("_g_xyzc2") > -1)
                    {
                        //左侧血压收缩压
                        string str_g_xyzc1 = GetControlValueByName(controlType_textBox + "g_xyzc1");
                        //左侧血压舒张压
                        string str_g_xyzc2 = GetControlValueByName(controlType_textBox + "g_xyzc2");

                        if (str_g_xyzc1.Length > 0 && str_g_xyzc2.Length > 0)
                        {
                            int int_G_XYYC1 = Int32.Parse(str_g_xyzc1) > int_autoXy_d_SS ? (Int32.Parse(str_g_xyzc1) + int_autoXy_d_SS) : 0;
                            int int_G_XYYC2 = Int32.Parse(str_g_xyzc2) > int_autoXy_d_SZ ? (Int32.Parse(str_g_xyzc2) + int_autoXy_d_SZ) : 0;
                            setValueToControl(controlType_textBox + "g_xyyc1", int_G_XYYC1 > 0 ? int_G_XYYC1.ToString() : "");
                            setValueToControl(controlType_textBox + "g_xyyc2", int_G_XYYC2 > 0 ? int_G_XYYC2.ToString() : "");
                        }
                    }

                }
                //1：左侧 按照右侧计算左侧
                else if (str_autoXy.Equals("1"))
                {
                    //左侧血压设定
                    if ((controlId.ToLower().IndexOf("_g_xyyc1") > -1) || controlId.ToLower().IndexOf("_g_xyyc2") > -1)
                    {
                        //左侧血压收缩压
                        string str_g_xyyc1 = GetControlValueByName(controlType_textBox + "g_xyyc1");
                        //左侧血压舒张压
                        string str_g_xyyc2 = GetControlValueByName(controlType_textBox + "g_xyyc2");

                        if (str_g_xyyc1.Length > 0 && str_g_xyyc2.Length > 0)
                        {
                            int int_G_XYzC1 = Int32.Parse(str_g_xyyc1) > int_autoXy_d_SS ? (Int32.Parse(str_g_xyyc1) + int_autoXy_d_SS) : 0;
                            int int_G_XYzC2 = Int32.Parse(str_g_xyyc2) > int_autoXy_d_SZ ? (Int32.Parse(str_g_xyyc2) + int_autoXy_d_SZ) : 0;
                            setValueToControl(controlType_textBox + "g_xyzc1", int_G_XYzC1 > 0 ? int_G_XYzC1.ToString() : "");
                            setValueToControl(controlType_textBox + "g_xyzc2", int_G_XYzC2 > 0 ? int_G_XYzC2.ToString() : "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 清空按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认清空？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                setPageClear();

                //设定系统参数改项目被编辑过
                CommomSysInfo.IsEdited = "1";
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
                //关闭按钮
                //Point point = button_exit.Location;
                //point.X = Formpanel2.Width - 50;
                //button_exit.Location = point;
                //最大化按钮
                //button_max
                //最小化按钮
                //button_min
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                //Formpanel2.Width = FormPanle1.Width - 6;
                //Formpanel2.Height = FormPanle1.Height - 10;
                //关闭按钮
                // Point point = button_exit.Location;
                // point.X = Formpanel2.Width - 50;
                //button_exit.Location = point;
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

        #region 保存

        private void SaveJktjSignname()
        {
            //先判断是否启用签名
            if (TJClient.Signname.Common.ShowSignnameOperation())
            {
                if (picSignname1111.Image == null && cboSignname.Items[cboSignname.SelectedIndex].ToString().Equals("")) //判断是否签名
                {
                    return;
                }
                TJClient.Signname.Model.JktjSignname jktjSignname = new TJClient.Signname.Model.JktjSignname()
                {
                    Czy = UserInfo.userId,
                    Yljgbm = UserInfo.Yybm,
                    Tjsj = Common.FormatDateTime(CommomSysInfo.tjsj),
                    D_Grdabh = getValueFromDt(dt_paraFromParent, 0, "JKDAH"),
                    SignnamePicPath = picSignname1111.ImageLocation,
                    Realname = textBox_realname.Text,
                };
                TJClient.Signname.Operation.SaveJktjSignname(dtResult, jktjSignname);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {

            if (getValueFromDt(dt_paraFromParent, 0, "JKDAH").Trim().Length <= 0)
            {
                MessageBox.Show("请选择体检人员！");
                return;
            }

            try
            {
                //取得页面上的信息
                getTjResultDtFromPage();

                //验证必须录入项目是否都已经录入
                string errmsg = "";
                string id = "";
                if (check_dw.Checked && checkNull(out errmsg, out id) == false)
                {
                    return;
                }

                //页面信息保存到数据库中
                setTjResultToDb();

                //保存签名
                SaveJktjSignname();

                MessageBox.Show("保存成功！");

                ////取得当前要上传的人的信息,维护体检状态
                //int index = listBox_ryxx.SelectedIndex;
                //if (dt_list_tjryxx != null && dt_list_tjryxx.Rows.Count > 0)
                //{
                //    dt_list_tjryxx.Rows[index]["tjsj"] = dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd");
                //}

                //清空页面信息
                setPageClear();

                //设定系统参数改项目被编辑过
                CommomSysInfo.IsEdited = "0";
                //调用父页面的方法
                if (main_form != null)
                {
                    main_form.setParentFormDo(null);
                }

                //textBox_TJBH.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 验证必须录入项目是否已经录入
        /// </summary>
        /// <returns></returns>
        public bool checkNull(out string Message, out string id)
        {
            Message = "";
            id = "";

            //当前项目编码
            string controlGroup_tem = "";

            //是否必须录入项
            string ifNotNull = "1";

            bool isCheckNull = true;

            //项目名称
            string xmbm = "";
            string xmmc = "";
            string ControlId = "";
            string dw = "";
            string KJLX = "";

            if (dtControl != null && dtControl.Rows.Count > 0)
            {
                for (int i = 0; i < dtControl.Rows.Count; i++)
                {
                    //同一个项目
                    if (controlGroup_tem.Equals(dtControl.Rows[i]["ControlGroup"].ToString()))
                    {
                        //必须录入验证
                        if (ifNotNull.Equals("1") && isCheckNull == false)
                        {
                            if (dtControl.Rows[i]["value"] != null && dtControl.Rows[i]["value"].ToString().Trim().Length > 0)
                            {
                                isCheckNull = true;
                            }
                        }
                    }
                    else
                    {
                        //有必须录入项没有录入内容
                        if (ifNotNull.Equals("1") && isCheckNull == false)
                        {
                            //如果项目为必须录入项目，并且为空返回true
                            if (SetCheckControlFocus(ControlId, out Message) == true)
                            {
                                MessageBox.Show("请填写【" + xmmc + "】");
                                return false;
                            }
                        }

                        //项目改变时记录项目编码
                        controlGroup_tem = dtControl.Rows[i]["ControlGroup"].ToString();
                        //1：必须录入项
                        ifNotNull = dtControl.Rows[i]["isNotNull"] != null ? dtControl.Rows[i]["isNotNull"].ToString() : "0";
                        //未录入项目
                        isCheckNull = false;
                        //项目名称
                        xmmc = dtControl.Rows[i]["xmmc"] != null ? dtControl.Rows[i]["xmmc"].ToString() : "0";
                        xmbm = dtControl.Rows[i]["xmbm"] != null ? dtControl.Rows[i]["xmbm"].ToString() : "0";
                        KJLX = dtControl.Rows[i]["KJLX"] != null ? dtControl.Rows[i]["KJLX"].ToString() : "";
                        ControlId = dtControl.Rows[i]["ControlId"] != null ? dtControl.Rows[i]["ControlId"].ToString() : "";

                        //必须录入验证
                        if (ifNotNull.Equals("1"))
                        {
                            if (dtControl.Rows[i]["value"] != null && dtControl.Rows[i]["value"].ToString().Trim().Length > 0)
                            {
                                isCheckNull = true;
                            }
                        }
                        else
                        {
                            isCheckNull = true;
                        }

                        //验证带单位项是否未数字
                        dw = dtControl.Rows[i]["DW"] != null ? dtControl.Rows[i]["DW"].ToString() : "";
                        if ((!string.IsNullOrWhiteSpace(dw) && dw != "0") || xmbm == "0048" || xmbm == "0049" || xmbm == "0050" || xmbm == "0051")
                        {
                            double aaa;
                            if (dtControl.Rows[i]["value"] != null && !string.IsNullOrWhiteSpace(dtControl.Rows[i]["value"].ToString()) && !double.TryParse(dtControl.Rows[i]["value"].ToString(), out aaa))
                            {
                                if (SetCheckControlFocus(ControlId, out Message) == true)
                                {
                                    MessageBox.Show("请填写有效的【" + xmmc + "】");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 设定焦点
        /// </summary>
        /// <param name="ControlName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SetCheckControlFocus(string ControlName, out string message)
        {

            message = "";
            if (ControlName.Length == 0)
            {
                return false;
            }

            Control[] control = Controls.Find(ControlName, true);
            if (control == null || control.Length == 0)
            {
                return false;
            }
            if (control[0].Enabled == true)
            {
                control[0].Focus();

                return true;
            }
            else
            {
                return false;
            }
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
            SqlDele = SqlDele.Replace("{TJSJ}", Common.FormatDateTime(getValueFromDt(dt_paraFromParent, 0, "tjsj")));
            //操作员
            //SqlDele = SqlDele.Replace("{CZY}", UserInfo .userId);
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
            SqlAdd = SqlAdd.Replace("{TJSJ}", Common.FormatDateTime(tisj));
            //操作员
            SqlAdd = SqlAdd.Replace("{CZY}", UserInfo.userId);
            //文档类型
            SqlAdd = SqlAdd.Replace("{TJTYPE}", TJTYPE);
            //体检状态
            SqlAdd = SqlAdd.Replace("{ZT}", zt);

            //体检医生
            SqlAdd = SqlAdd.Replace("{Tjys}", CommomSysInfo.TJFZR_BM);
            returnArrayList.Add(SqlAdd);
            return returnArrayList;
        }

        #endregion

        #region 控件制御
        /// <summary>
        /// 取得控件控制关系
        /// </summary>
        private void get_T_JK_TJXMGLKZ()
        {
            string sql = @"SELECT   k.XMBM,k.XMZ,k.XMBM_B,k.XMZ_C,k.ifky,t1.KJID as KJID1,t1.KJLX as KJLX1,t1.SJZDBM as SJZDBM1,t2.KJID as KJID2,t2.KJLX as KJLX2,t2.SJZDBM as SJZDBM2 
                           FROM   (((t_jk_gzz_xm tg INNER JOIN  T_JK_TJXMGLKZ k on tg.xmbm= k.xmbm and  tg.YLJGBM=k.YLJGBM) INNER JOIN T_JK_TJXM t1 ON k.XMBM = t1.XMBM)  INNER JOIN T_JK_TJXM t2 ON k.XMBM_B = t2.XMBM)
                           where tg.yljgbm='{yljgbm}' and tg.gzzbm='{gzzbm}'
                           order by k.orderby,k.XMBM ";

            //DataTable dt_TJXMGLKZ = new DataTable();
            DBAccess access = new DBAccess();
            if (dt_TJXMGLKZ.Rows.Count < 1)
            {
                dt_TJXMGLKZ = access.ExecuteQueryBySql(sql.Replace("{yljgbm}", yljg).Replace("{gzzbm}", yhfz));
            }
        }

        /// <summary>
        ///  设定控件控制关系
        /// </summary>
        private void init_TJXMGLKZ(string xmz, string id)
        {
            try
            {
                if (dt_TJXMGLKZ == null)
                {
                    return;
                }
                if (dt_TJXMGLKZ.Rows.Count < 1)
                {
                    return;
                }

                DataRow[] dtRows = null;
                if (xmz.Length == 0 && id.Length == 0)
                {
                    dtRows = dt_TJXMGLKZ.Select();
                }
                else
                {

                    string id_tem = "";
                    DataRow[] dtRowsid = dtControl.Select(string.Format(" ControlId ='{0}' ", id));
                    if (dtRowsid != null && dtRowsid.Length > 0)
                    {
                        id_tem = dtRowsid[0]["kjid"].ToString();
                    }


                    dtRows = dt_TJXMGLKZ.Select(string.Format(" xmz='{0}' and  kjid1 like '%{1}%'", xmz, id_tem));
                }

                if (dtRows == null || dtRows.Length == 0)
                {
                    return;
                }

                //控件关系
                for (int j = 0; j < dtRows.Length; j++)
                {
                    #region checkbox
                    //控制控件
                    if (dtRows[j]["KJLX1"].ToString().ToLower().Equals("checkbox"))
                    {
                        //控制控件
                        string ControlId1 = controlType_checkBox + dtRows[j]["KJID1"].ToString() + "_" + dtRows[j]["XMZ"].ToString();

                        //被控制控件
                        string ControlId2 = controlType_checkBox + dtRows[j]["KJID2"].ToString();

                        Control control = Controls.Find(ControlId1, true)[0];
                        DataTable dtSJZDBM = new DataTable();
                        CheckBox KJID1 = (CheckBox)control;

                        if (KJID1.Checked == true)
                        {
                            //被控制控件为checkbox
                            if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("checkbox"))
                            {
                                setCheckBox(dtRows[j]["SJZDBM2"].ToString(), ControlId2, dtRows[j]["XMZ_C"].ToString(), dtRows[j]["ifky"].ToString(), true);
                            }

                            //加载text
                            //被控制控件
                            if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("textbox"))
                            {
                                setText(dtRows[j]["KJID2"].ToString(), dtRows[j]["ifky"].ToString(), true);
                            }
                            else if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("radiobutton"))
                            {
                                setCheckBox(dtRows[j]["SJZDBM2"].ToString(), ControlId2, dtRows[j]["XMZ_C"].ToString(), dtRows[j]["ifky"].ToString(), true);

                            }
                        }
                        else
                        {
                            //被控制控件为checkbox
                            if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("checkbox"))
                            {
                                setCheckBox(dtRows[j]["SJZDBM2"].ToString(), ControlId2, dtRows[j]["XMZ_C"].ToString(), dtRows[j]["ifky"].ToString(), false);
                            }

                            //加载text
                            //被控制控件
                            if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("textbox"))
                            {
                                setText(dtRows[j]["KJID2"].ToString(), dtRows[j]["ifky"].ToString(), false);
                            }
                            else if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("radiobutton"))
                            {
                                setCheckBox(dtRows[j]["SJZDBM2"].ToString(), ControlId2, dtRows[j]["XMZ_C"].ToString(), dtRows[j]["ifky"].ToString(), false);

                            }

                        }
                    }
                    #endregion

                    #region 加载radioButton
                    //加载radioButton
                    //控制控件
                    // MessageBox.Show(dtRows[j]["KJLX1"].ToString());
                    if (dtRows[j]["KJLX1"].ToString().ToLower().Equals("radiobutton"))
                    {
                        // MessageBox.Show(dtRows[j]["KJID1"].ToString());

                        //控制控件
                        string ControlId1 = controlType_radioButton + dtRows[j]["KJID1"].ToString() + "_" + dtRows[j]["XMZ"].ToString();

                        //被控制控件
                        string ControlId2 = controlType_checkBox + dtRows[j]["KJID2"].ToString();

                        Control control = Controls.Find(ControlId1, true)[0];
                        DataTable dtSJZDBM = new DataTable();
                        RadioButton KJID1 = (RadioButton)control;

                        if (KJID1.Checked == true)
                        {
                            //被控制控件为checkbox
                            if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("checkbox"))
                            {
                                setCheckBox(dtRows[j]["SJZDBM2"].ToString(), ControlId2, dtRows[j]["XMZ_C"].ToString(), dtRows[j]["ifky"].ToString(), true);
                            }

                            //加载text
                            //被控制控件
                            if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("textbox"))
                            {
                                setText(dtRows[j]["KJID2"].ToString(), dtRows[j]["ifky"].ToString(), true);
                            }
                            else if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("radiobutton"))
                            {
                                setCheckBox(dtRows[j]["SJZDBM2"].ToString(), ControlId2, dtRows[j]["XMZ_C"].ToString(), dtRows[j]["ifky"].ToString(), true);

                            }
                        }
                        else
                        {
                            //被控制控件为checkbox
                            if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("checkbox"))
                            {
                                setCheckBox(dtRows[j]["SJZDBM2"].ToString(), ControlId2, dtRows[j]["XMZ_C"].ToString(), dtRows[j]["ifky"].ToString(), false);

                            }

                            //加载text
                            //被控制控件
                            else if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("textbox"))
                            {
                                setText(dtRows[j]["KJID2"].ToString(), dtRows[j]["ifky"].ToString(), false);
                            }
                            else if (dtRows[j]["KJLX2"].ToString().ToLower().Equals("radiobutton"))
                            {
                                setCheckBox(dtRows[j]["SJZDBM2"].ToString(), ControlId2, dtRows[j]["XMZ_C"].ToString(), dtRows[j]["ifky"].ToString(), false);

                            }
                        }

                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设定CheckBox状态
        /// </summary>
        private void setCheckBox(string SJZDBM, string ControlId, string XMZ_C, string ifky, Boolean checkstaue)
        {
            try
            {
                DataTable dtValueList = new DataTable();
                dtValueList = getSjzdList(SJZDBM);

                //处理checkbox的项目
                for (int k = 0; k < dtValueList.Rows.Count; k++)
                {
                    Control control2_tem = Controls.Find(ControlId + "_" + dtValueList.Rows[k]["ZDBM"].ToString(), true)[0];
                    CheckBox CheckBox2_tem = (CheckBox)control2_tem;
                    //除外的的项目
                    if (("," + XMZ_C + ",").IndexOf("," + dtValueList.Rows[k]["ZDBM"].ToString() + ",") > -1)
                    {
                    }
                    //需要设定的项目
                    else
                    {
                        //控制元控件未被选中时
                        if (checkstaue == false)
                        {
                            //控制元控件选中时，被控制控件的状态设定。 1：设定为可用  0：设定为不可用
                            if (ifky.Equals("0"))
                            {
                                CheckBox2_tem.Enabled = true;
                            }
                            else
                            {
                                CheckBox2_tem.Checked = false;
                                CheckBox2_tem.Enabled = false;
                            }
                        }
                        else
                        {
                            //控制元控件选中时，被控制控件的状态设定。 1：设定为可用  0：设定为不可用
                            if (ifky.Equals("0"))
                            {
                                CheckBox2_tem.Checked = false;
                                CheckBox2_tem.Enabled = false;

                            }
                            else
                            {
                                CheckBox2_tem.Enabled = true;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("控制关系设定错误");
            }

        }

        /// <summary>
        /// 设定TextBox
        /// </summary>
        private void setText(string ControlId, string ifky, Boolean checkstaue)
        {
            try
            {
                string[] control2List = ControlId.Split(new char[] { ',' });
                for (int i = 0; i < control2List.Length; i++)
                {
                    Control control2_tem = Controls.Find(controlType_textBox + control2List[i], true)[0];
                    TextBox CheckBox2_tem = (TextBox)control2_tem;

                    //控制元控件未被选中时
                    if (checkstaue == false)
                    {
                        //控制元控件选中时，被控制控件的状态设定。 1：设定为可用  0：设定为不可用
                        if (ifky.Equals("0"))
                        {
                            CheckBox2_tem.Enabled = true;
                        }
                        else
                        {
                            CheckBox2_tem.Text = "";
                            CheckBox2_tem.Enabled = false;
                        }
                    }
                    else
                    {
                        //控制元控件选中时，被控制控件的状态设定。 1：设定为可用  0：设定为不可用
                        if (ifky.Equals("0"))
                        {
                            CheckBox2_tem.Text = "";
                            CheckBox2_tem.Enabled = false;
                        }
                        else
                        {
                            CheckBox2_tem.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("控制关系设定错误");
            }
        }

        /// <summary>
        /// 设定CheckBox状态
        /// </summary>
        private void setradiobutton(string SJZDBM, string ControlId, string XMZ_C, string ifky, Boolean checkstaue)
        {
            try
            {
                DataTable dtValueList = new DataTable();
                dtValueList = getSjzdList(SJZDBM);

                //处理checkbox的项目
                for (int k = 0; k < dtValueList.Rows.Count; k++)
                {
                    Control control2_tem = Controls.Find(ControlId + "_" + dtValueList.Rows[k]["ZDBM"].ToString(), true)[0];
                    RadioButton RadioButton_tem = (RadioButton)control2_tem;
                    //除外的的项目
                    if (("," + XMZ_C + ",").IndexOf("," + dtValueList.Rows[k]["ZDBM"].ToString() + ",") > -1)
                    {
                    }
                    //需要设定的项目
                    else
                    {
                        //控制元控件未被选中时
                        if (checkstaue == false)
                        {
                            //控制元控件选中时，被控制控件的状态设定。 1：设定为可用  0：设定为不可用
                            if (ifky.Equals("0"))
                            {
                                RadioButton_tem.Enabled = true;
                            }
                            else
                            {
                                RadioButton_tem.Checked = false;
                                RadioButton_tem.Enabled = false;
                            }
                        }
                        else
                        {
                            //控制元控件选中时，被控制控件的状态设定。 1：设定为可用  0：设定为不可用
                            if (ifky.Equals("0"))
                            {
                                RadioButton_tem.Checked = false;
                                RadioButton_tem.Enabled = false;

                            }
                            else
                            {
                                RadioButton_tem.Enabled = true;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("控制关系设定错误");
            }

        }
        #endregion
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

                    if (ControlValue.Trim().Length > 0 && checkBox_tem.Tag != null && ("," + ControlValue.Trim().ToString() + ",").IndexOf("," + checkBox_tem.Tag.ToString().ToLower() + ",") > -1)
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
                else if (ControlId.IndexOf(controlType_dataGridView) > -1)
                {
                    datagridview_bool = false;
                    DataGridView DataGridView_tem = (DataGridView)control;
                    if (DataGridView_tem.DataSource != null)
                    {
                        DataTable dt = (DataTable)DataGridView_tem.DataSource;
                        DataGridView_tem.DataSource = dt.Clone();
                        DataGridView_tem.ClearSelection();
                        DataGridView_tem.CurrentCell = null;
                        DataGridView_tem.Rows[0].Cells[2].Selected = true;
                        Thread t = new Thread(Run) { IsBackground = true };
                        t.Start();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        void Run()
        {
            Thread.Sleep(200);
            datagridview_bool = true;
        }

        /// <summary>
        /// 创建一个保存控件信息的容器
        /// </summary>
        /// <param name="dtControls"></param> 
        /// <returns></returns>
        private DataTable createControlList()
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add("ControlGroup");//控件
            dtList.Columns.Add("ControlId");//控件id
            dtList.Columns.Add("value");//控件值
            dtList.Columns.Add("HIS_DB");//His编码
            dtList.Columns.Add("KJLX");//控件类型
            dtList.Columns.Add("XMBM");//项目编码
            dtList.Columns.Add("parentxm");//父项目
            dtList.Columns.Add("parentxmvalue");//关联项目编码
            dtList.Columns.Add("maxcount");//可以选择的最多项目数
            dtList.Columns.Add("isNotNull");//是否必须录入
            dtList.Columns.Add("ifnumber");//是否数值类型
            dtList.Columns.Add("tag");//tag
            dtList.Columns.Add("kjid");
            dtList.Columns.Add("mrz");//默认值
            dtList.Columns.Add("sctjjg");//上次体检结果

            dtList.Columns.Add("valueHeigh");//项目参考值最高值

            dtList.Columns.Add("valueLower");//项目参考值最低值

            dtList.Columns.Add("fzlritem");//辅助录入项目
            dtList.Columns.Add("XMMC");//项目名称
            dtList.Columns.Add("DW");//His编码
            return dtList;
        }

        /// <summary>
        /// 添加控件到容器控件中
        /// </summary>
        /// <param name="dtRow"></param>
        /// <param name="ControlId"></param>
        private void addControlToList(DataRow dtRow, string ControlId, string value, string tag, string his_db)
        {
            if (dtControl == null)
            {
                dtControl = createControlList();
            }
            dtControl.Rows.Add();
            //dtControl.Rows[dtControl.Rows.Count - 1]["ControlGroup"] = dtRow["KJID"].ToString();
            dtControl.Rows[dtControl.Rows.Count - 1]["ControlGroup"] = his_db;
            dtControl.Rows[dtControl.Rows.Count - 1]["ControlId"] = ControlId;
            //dtControl.Rows[dtControl.Rows.Count - 1]["HIS_DB"] = dtRow["HIS_DB"].ToString();
            dtControl.Rows[dtControl.Rows.Count - 1]["HIS_DB"] = his_db;
            dtControl.Rows[dtControl.Rows.Count - 1]["KJLX"] = dtRow["KJLX"].ToString();
            dtControl.Rows[dtControl.Rows.Count - 1]["XMBM"] = dtRow["XMBM"].ToString();
            dtControl.Rows[dtControl.Rows.Count - 1]["parentxm"] = dtRow["parentxm"].ToString();
            dtControl.Rows[dtControl.Rows.Count - 1]["parentxmvalue"] = dtRow["parentxmvalue"].ToString();
            dtControl.Rows[dtControl.Rows.Count - 1]["maxcount"] = dtRow["maxcount"].ToString();
            dtControl.Rows[dtControl.Rows.Count - 1]["tag"] = tag;
            dtControl.Rows[dtControl.Rows.Count - 1]["kjid"] = dtRow["KJID"];//KJMRZ
            //return dtControlList;
            dtControl.Rows[dtControl.Rows.Count - 1]["mrz"] = dtRow["KJMRZ"];
            //是否必须录入项目
            dtControl.Rows[dtControl.Rows.Count - 1]["isNotNull"] = dtRow["isNotNull"];
            dtControl.Rows[dtControl.Rows.Count - 1]["valueHeigh"] = dtRow["valueHeigh"];
            dtControl.Rows[dtControl.Rows.Count - 1]["valueLower"] = dtRow["valueLower"];
            dtControl.Rows[dtControl.Rows.Count - 1]["fzlritem"] = dtRow["fzlritem"];
            dtControl.Rows[dtControl.Rows.Count - 1]["XMMC"] = dtRow["XMMC"];//项目名称
            dtControl.Rows[dtControl.Rows.Count - 1]["DW"] = dtRow["DW"];//项目名称
        }


        /// <summary>
        /// 给控件赋值dataGridView
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private void setValueToControl_dataGridView_sc(string ControlId)
        {
            Control control = Controls.Find(ControlId, true)[0];
            DBAccess access = new DBAccess();
            //dataGridView
            if (ControlId.IndexOf(controlType_dataGridView) > -1)
            {
                DataGridView DataGridView_tem = (DataGridView)control;
                DataTable dt = (DataTable)DataGridView_tem.DataSource;

                string sql = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sql = sql + dt.Columns[i].ColumnName + ",";
                }
                sql = "select " + sql.Substring(0, sql.Length - 1) + " from " + ControlId.Replace(controlType_dataGridView, "") + "_TMP ";

                sql = sql + " where D_GRDABH like '%" + getValueFromDt(dt_paraFromParent, 0, "JKDAH") + "%'";

                DataTable dtResult_tem = access.ExecuteQueryBySql(sql);

                if (dtResult_tem != null && dtResult_tem.Rows.Count > 0)
                {
                    if (dtResult_tem.Columns.Contains("y_fyycx") == true)
                    {
                        for (int i = 0; i < dtResult_tem.Rows.Count; i++)
                        {
                            if (dtResult_tem.Rows[i]["y_fyycx"] != null && dtResult_tem.Rows[i]["y_fyycx"] != DBNull.Value)
                            {
                                if (dtResult_tem.Rows[i]["y_fyycx"].ToString().Equals("规律") == true)
                                {
                                    dtResult_tem.Rows[i]["y_fyycx"] = "1";
                                }
                                else if (dtResult_tem.Rows[i]["y_fyycx"].ToString().Equals("不服药") == true)
                                {
                                    dtResult_tem.Rows[i]["y_fyycx"] = "3";
                                }
                                else if (dtResult_tem.Rows[i]["y_fyycx"].ToString().Equals("间断") == true)
                                {
                                    dtResult_tem.Rows[i]["y_fyycx"] = "2";
                                }
                            }
                        }
                    }
                    DataGridView_tem.ForeColor = Color.Blue;
                }
                else
                {
                    DataGridView_tem.ForeColor = Color.Black;
                }

                DataGridView_tem.DataSource = dtResult_tem;
            }

        }

        /// <summary>
        /// 给控件赋值dataGridView
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private void setValueToControl_dataGridView(string ControlId)
        {
            Control control = Controls.Find(ControlId, true)[0];
            DBAccess access = new DBAccess();
            //dataGridView
            if (ControlId.IndexOf(controlType_dataGridView) > -1)
            {
                DataGridView DataGridView_tem = (DataGridView)control;
                DataTable dt = (DataTable)DataGridView_tem.DataSource;

                string sql = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sql = sql + dt.Columns[i].ColumnName + ",";
                }
                sql = "select " + sql.Substring(0, sql.Length - 1) + " from " + ControlId.Replace(controlType_dataGridView, "");

                sql = sql + " where D_GRDABH like '%" + getValueFromDt(dt_paraFromParent, 0, "JKDAH") + "%'";

                DataTable dtResult_tem = access.ExecuteQueryBySql(sql);
                DataGridView_tem.DataSource = dtResult_tem;
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
            if (sql.Trim().Length == 0)
            {
                return null;
            }
            sql = "select " + sql.Substring(0, sql.Length - 1) + " from  view_RYXX_JKTJ where 1=1  " + sqlWhere;

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
                setValueToControl_dataGridView(dtControl.Rows[i]["ControlId"].ToString());
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
        /// 将页面上的数据保存到数据库中
        /// </summary>
        private void setTjResultToDb()
        {
            string sql = "";
            string key = "";
            string value = "";
            string kjlx = "";
            string his_db = "";

            ArrayList sqlList = new ArrayList();
            //sql = "delete from T_JK_JKTJ where czy='{czy}' and gzz='{gzz}' and d_grdabh='{d_grdabh}' and  happentime ='{happentime}'";
            sql = "delete from T_JK_JKTJ where  gzz='{gzz}' and d_grdabh='{d_grdabh}' and  happentime ='{happentime}'";
            ////操作员
            //sql = sql.Replace("{czy}", userId);
            //工作组
            sql = sql.Replace("{gzz}", yhfz);
            //健康档案编号
            sql = sql.Replace("{d_grdabh}", getValueFromDt(dt_paraFromParent, 0, "JKDAH"));
            //体检日期
            sql = sql.Replace("{happentime}", Common.FormatDateTime(getValueFromDt(dt_paraFromParent, 0, "tjsj")));
            sqlList.Add(sql);
            sql = "";

            DataView dtResult_view = dtResult.AsDataView();
            dtResult_view.Sort = "HIS_DB";
            DataTable dtResult_tem = dtResult_view.ToTable();

            string dtResult_HIS_DB_tem = "";

            for (int i = 0; i < dtResult_tem.Rows.Count; i++)
            {
                if (controlType_dataGridView.ToLower().IndexOf(dtResult_tem.Rows[i]["KJLX"].ToString().ToLower()) == -1 && !dtResult_HIS_DB_tem.Equals(dtResult_tem.Rows[i]["HIS_DB"].ToString().ToLower()))
                {
                    dtResult_HIS_DB_tem = dtResult_tem.Rows[i]["HIS_DB"].ToString().ToLower();
                    if (controlType_textBox.ToLower().IndexOf(dtResult_tem.Rows[i]["KJLX"].ToString().ToLower()) > -1)
                    {
                        string[] HIS_DBList = dtResult_tem.Rows[i]["HIS_DB"].ToString().ToLower().Split(new char[] { ',' });
                        for (int ii = 0; ii < HIS_DBList.Length; ii++)
                        {
                            sql = sql + ",'{" + HIS_DBList[ii].ToLower() + "}'";
                        }
                    }
                    else if (controlType_LinkLabel.ToLower().IndexOf(dtResult_tem.Rows[i]["KJLX"].ToString().ToLower()) > -1)
                    {
                        try
                        {
                            sql = sql + ",'{" + dtResult_tem.Rows[i]["HIS_DB"].ToString().ToLower() + "}'";
                        }
                        catch (Exception ex) { }

                    }
                    else
                    {
                        sql = sql + ",'{" + dtResult_tem.Rows[i]["HIS_DB"].ToString().ToLower() + "}'";
                    }
                }
            }

            sql = " insert into T_JK_JKTJ ([guid],czy,gzz,d_grdabh,creatregion,happentime,createtime,createuser,field1,field2,field3,P_RGID" + sql.Replace("'{", "").Replace("}'", "") + ") values ('{guid}','{czy}','{gzz}','{d_grdabh}','{creatregion}','{happentime}','{createtime}','{createuser}','{field1}','{field2}','{field3}','{P_RGID}'" + sql + ");";

            //DataTable dt = dBAccess.ExecuteQueryBySql(sql);

            if (dtControl.Rows.Count > 0)
            {
                DataView dtControl_view = dtControl.AsDataView();
                dtControl_view.Sort = "HIS_DB,ControlGroup";
                dtControl = dtControl_view.ToTable();

                //控件类型
                his_db = dtControl.Rows[0]["HIS_DB"].ToString().ToLower();

                for (int i = 0; i < dtControl.Rows.Count; i++)
                {
                    //textbox
                    if (controlType_textBox.ToLower().IndexOf(dtControl.Rows[i]["KJLX"].ToString().ToLower()) > -1)
                    {
                        //控件类型发生改变时的处理
                        if (his_db.Equals(dtControl.Rows[i]["HIS_DB"].ToString().ToLower()) == false)
                        {
                            if (value.Length > 0)
                            {
                                sql = sql.Replace("{" + key.ToLower() + "}", value.Substring(1));
                            }
                            else
                            {
                                sql = sql.Replace("{" + key.ToLower() + "}", value);
                            }
                            his_db = dtControl.Rows[i]["HIS_DB"].ToString().ToLower();
                            key = "";
                            value = "";
                        }

                        key = dtControl.Rows[i]["HIS_DB"].ToString().ToLower();
                        if (dtControl.Rows[i]["value"].ToString().Length >= 0)
                        {
                            value = value + "," + dtControl.Rows[i]["value"].ToString();
                        }
                    }

                    //checkBox
                    if (controlType_checkBox.ToLower().IndexOf(dtControl.Rows[i]["KJLX"].ToString().ToLower()) > -1)
                    {

                        if (his_db.Equals(dtControl.Rows[i]["HIS_DB"].ToString().ToLower()) == false)
                        {
                            if (value.Length > 0)
                            {
                                sql = sql.Replace("{" + key.ToLower() + "}", value.Substring(1));
                            }
                            else
                            {
                                sql = sql.Replace("{" + key.ToLower() + "}", value);
                            }
                            his_db = dtControl.Rows[i]["HIS_DB"].ToString().ToLower();
                            key = "";
                            value = "";
                        }

                        key = dtControl.Rows[i]["HIS_DB"].ToString().ToLower();
                        if (dtControl.Rows[i]["value"].ToString().Length > 0)
                        {
                            value = value + "," + dtControl.Rows[i]["value"].ToString();
                        }
                    }

                    //radioButton
                    if (controlType_radioButton.ToLower().IndexOf(dtControl.Rows[i]["KJLX"].ToString().ToLower()) > -1)
                    {
                        //控件类型发生改变时的处理
                        if (his_db.Equals(dtControl.Rows[i]["HIS_DB"].ToString().ToLower()) == false)
                        {
                            //sql = sql.Replace("{" + key.ToLower() + "}", value);
                            if (value.Length > 0)
                            {
                                sql = sql.Replace("{" + key.ToLower() + "}", value.Substring(1));
                                //sql = sql.Replace("{" + key.ToLower() + "}", value.Replace(",", ""));
                            }
                            else
                            {
                                sql = sql.Replace("{" + key.ToLower() + "}", value);
                            }
                            his_db = dtControl.Rows[i]["HIS_DB"].ToString().ToLower();
                            key = "";
                            value = "";
                        }

                        key = dtControl.Rows[i]["HIS_DB"].ToString().ToLower();
                        if (dtControl.Rows[i]["value"].ToString().Length > 0)
                        {
                            value = value + "," + dtControl.Rows[i]["value"].ToString();
                        }
                    }



                    if (controlType_LinkLabel.ToLower().IndexOf(dtControl.Rows[i]["KJLX"].ToString().ToLower()) > -1)
                    {

                        //控件类型发生改变时的处理
                        if (his_db.Equals(dtControl.Rows[i]["HIS_DB"].ToString().ToLower()) == false)
                        {

                            if (value.Length > 0)
                            {
                                sql = sql.Replace("{" + key.ToLower() + "}", value.Substring(1));

                            }
                            else
                            {
                                sql = sql.Replace("{" + key.ToLower() + "}", value);
                            }
                            his_db = dtControl.Rows[i]["HIS_DB"].ToString().ToLower();
                            key = "";
                            value = "";
                        }

                        key = dtControl.Rows[i]["HIS_DB"].ToString().ToLower();

                        string controlId = controlType_LinkLabel + key;
                        //if (dtControl.Rows[i]["value"].ToString().Length > 0)
                        //{
                        value = value + "," + GetControlValueByName(controlId); ;
                        //}

                    }
                }

                //最后一个控件的处理
                if (dtControl.Rows.Count > 0 && controlType_radioButton.ToLower().IndexOf(dtControl.Rows[dtControl.Rows.Count - 1]["KJLX"].ToString().ToLower()) > -1)
                {
                    if (value.Length > 0)
                    {
                        //sql = sql.Replace("{" + key.ToLower() + "}", value.Replace(",", ""));
                        sql = sql.Replace("{" + key.ToLower() + "}", value.Substring(1));
                    }
                    else
                    {
                        sql = sql.Replace("{" + key.ToLower() + "}", value);
                    }

                }
                //else if (controlType_LinkLabel.ToLower().IndexOf(dtControl.Rows[dtControl.Rows.Count - 1]["KJLX"].ToString().ToLower()) > -1)
                //{
                //    string controlId = controlType_LinkLabel + key.ToLower();
                //    value = value + "," + GetControlValueByName(controlId);


                //    if (value.Length > 0)
                //    {
                //        sql = sql.Replace("{" + key.ToLower() + "}", value.Substring(1));
                //    }
                //    else
                //    {
                //        sql = sql.Replace("{" + key.ToLower() + "}", value);
                //    }

                //}
                else
                {

                    if (value.Length > 0)
                    {
                        sql = sql.Replace("{" + key.ToLower() + "}", value.Substring(1));
                    }
                    else
                    {
                        sql = sql.Replace("{" + key.ToLower() + "}", value);
                    }
                }

                //guid
                string guid = getGuid();
                sql = sql.Replace("{guid}", guid);
                //操作员
                sql = sql.Replace("{czy}", userId);
                //工作组
                sql = sql.Replace("{gzz}", yhfz);
                //健康档案编号
                sql = sql.Replace("{d_grdabh}", getValueFromDt(dt_paraFromParent, 0, "JKDAH"));
                //体检日期
                sql = sql.Replace("{happentime}", Common.FormatDateTime(CommomSysInfo.tjsj));
                //CREATREGION	//创建机构
                sql = sql.Replace("{creatregion}", yljg);
                //CREATETIME	//创建时间
                sql = sql.Replace("'{createtime}'", "date()");
                //CREATEUSER	//创建人
                sql = sql.Replace("{createuser}", userId);
                //FIELD1	//体检机构
                sql = sql.Replace("{field1}", yljg);
                //FIELD2	//责任医生编码
                sql = sql.Replace("{field2}", CommomSysInfo.TJFZR_BM);
                //FIELD3	//联系电话
                sql = sql.Replace("{field3}", getValueFromDt(dt_paraFromParent, 0, "JKDAH"));
                //P_RGID	//所属机构
                sql = sql.Replace("{P_RGID}", getValueFromDt(dt_paraFromParent, 0, "prgid"));
                sqlList.Add(sql);
                //更新到数据库中
                DBAccess dBAccess = new DBAccess();

                //更新体检人员信息表(T_JK_TJRYXX）
                //sqlList.Add(save_T_JK_TJRYXX(null, dateTimePicker_head_TJSJ.Value.ToString("yyyy-MM-dd"), "TJZT", "TJSJ"));
                ArrayList TJRYXXList = save_T_JK_TJRYXX(dt_paraFromParent, CommomSysInfo.tjsj, Common.TJTYPE.健康体检表, Common.ZT.确定状态);
                if (TJRYXXList != null && TJRYXXList.Count > 0)
                {
                    for (int i = 0; i < TJRYXXList.Count; i++)
                    {
                        sqlList.Add(TJRYXXList[i]);
                    }
                }

                //dataGridView 
                if (dtResult.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        if (controlType_dataGridView.ToLower().IndexOf(dtResult.Rows[i]["KJLX"].ToString().ToLower()) > -1)
                        {
                            string ControlId = controlType_dataGridView + dtResult.Rows[i]["KJID"].ToString();
                            Control control = Controls.Find(ControlId, true)[0];

                            DataGridView DataGridView_tem = (DataGridView)control;
                            DataTable dt = (DataTable)DataGridView_tem.DataSource;

                            //数据库表名称
                            string tablename = ControlId.Replace(controlType_dataGridView, "").ToLower();
                            //在此处加入表明，和数量。用于对非免疫，用药情况表，住院治疗情况的签名操作
                            TJClient.Signname.Operation.AddDataGridViewTableToDict(tablename, dt.Rows.Count);

                            string happentime = "";
                            if (tablename.Equals("T_TJ_FMYGHYFB".ToLower()))
                            {
                                happentime = "F_HAPPENTIME";
                            }
                            else if (tablename.Equals("T_TJ_YYQKB".ToLower()))
                            {
                                happentime = "Y_HAPPENTIME";
                            }
                            else if (tablename.Equals("T_TJ_ZYZLQKB".ToLower()))
                            {
                                happentime = "Z_HAPPENTIME";
                            }
                            else
                            {
                                happentime = "HAPPENTIME";
                            }

                            //sqlList.Add(string.Format("delete from {0} where D_GRDABH like '%{1}%' and {2}='{3}' ", ControlId.Replace(controlType_dataGridView, ""), getValueFromDt(dt_paraFromParent, 0, "JKDAH"), happentime, Common.FormatDateTime(CommomSysInfo.tjsj)));
                            sqlList.Add(string.Format("delete from {0} where D_GRDABH like '%{1}%'  ", ControlId.Replace(controlType_dataGridView, ""), getValueFromDt(dt_paraFromParent, 0, "JKDAH")));


                            if (dt.Rows.Count > 0)
                            {
                                // sqlList.Add("delete from " + ControlId.Replace(controlType_dataGridView, "") + " where D_GRDABH like '%" + textBox_head_JKDAH.Text + "%' ");
                                for (int tdindex = 0; tdindex < dt.Rows.Count; tdindex++)
                                {
                                    string sql_tem = "";
                                    string sql_column = "";

                                    for (int k = 0; k < dt.Columns.Count; k++)
                                    {
                                        sql_column = sql_column + dt.Columns[k].ColumnName + ",";

                                        sql_tem = sql_tem + "'" + dt.Rows[tdindex][k].ToString() + "',";
                                    }

                                    string sqlInserter = "insert into " + ControlId.Replace(controlType_dataGridView, "") + " (" + sql_column + happentime + ", D_GRDABH) values (" + sql_tem + "'" + Common.FormatDateTime(CommomSysInfo.tjsj) + "'," + "'" + getValueFromDt(dt_paraFromParent, 0, "JKDAH") + "')";
                                    sqlList.Add(sqlInserter);
                                }
                            }
                        }
                    }

                }

                dBAccess.ExecuteNonQueryBySql(sqlList);
            }
        }



        /// <summary>
        /// 取得Guid
        /// </summary>
        /// <returns></returns>
        private string getGuid()
        {
            string guid = System.Guid.NewGuid().ToString();
            DBAccess dBAccess = new DBAccess();
            string sql = "";
            ArrayList sqlList = new ArrayList();
            sql = "select guid from T_JK_JKTJ where czy='{czy}' and gzz='{gzz}' and d_grdabh='{d_grdabh}' and  happentime ='{happentime}'";
            //操作员
            sql = sql.Replace("{czy}", userId);
            //工作组
            sql = sql.Replace("{gzz}", yhfz);
            //健康档案编号
            sql = sql.Replace("{d_grdabh}", getValueFromDt(dt_paraFromParent, 0, "JKDAH"));
            //体检日期
            sql = sql.Replace("{happentime}", CommomSysInfo.tjsj);
            DataTable dt = dBAccess.ExecuteQueryBySql(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                guid = dt.Rows[0]["guid"].ToString();
            }
            return guid;
        }

        /// <summary>
        /// 检索健康体检
        /// </summary>
        private bool selectFromDb(bool isClear)
        {
            try
            {
                string sqlWhere = "";
                sqlWhere = sqlWhere + " and d_grdabh like '" + getValueFromDt(dt_paraFromParent, 0, "JKDAH") + "'";
                sqlWhere = sqlWhere + " and happentime like '" + getValueFromDt(dt_paraFromParent, 0, "tjsj") + "' and GZZBM='" + yhfz + "'";

                //取得数据库中的信息
                DataTable result_selectFromDb = getTjResultDtFromDb(sqlWhere);

                //将信息设定到页面上
                if (result_selectFromDb != null && result_selectFromDb.Rows.Count > 0)
                {
                    setTjResultToPage();
                    return true;
                }
                else
                {
                    //取得页面上的信息
                    setPageClear();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void setPageClear()
        {
            try
            {
                if (dtControl == null)
                {
                    return;
                }
                for (int i = 0; i < dtControl.Rows.Count; i++)
                {
                    dtControl.Rows[i]["value"] = "";   //dtControl.Rows[dtControl.Rows.Count - 1]["mrz"] 
                    setValueToControl(dtControl.Rows[i]["ControlId"].ToString(), dtControl.Rows[i]["mrz"].ToString());
                }

                //签名清空
                //picSignname1111.Image = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        /// <summary>
        /// 项目类型
        /// </summary>
        public string YQ_itemType = "";

        /// <summary>
        /// 项目
        /// </summary>
        public string YQ_items = "";

        /// <summary>
        /// 启用仪器开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_yq_CheckedChanged(object sender, EventArgs e)
        {
            //仪器标签
            if (checkBox_yq.Checked == true)
            {
                //显示仪器操作处理
                //仪器列表
                comboBox_yq.Visible = true;
                //开始
                button_start.Visible = true;
                //停止
                //button_stop.Visible = true;
                //结果
                //button_result.Visible = true;
                //绑定仪器列表
                setDrp(comboBox_yq, "hyyq", true);
                comboBox_type.Visible = true;
            }
            else
            {
                //隐藏仪器操作处理
                //仪器列表
                comboBox_yq.Visible = false;
                //开始
                button_start.Visible = false;
                //停止
                button_stop.Visible = false;
                //结果
                button_result.Visible = false;
                comboBox_type.Visible = false;
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private void WriteAction(bool begin, DataTable dt)
        {
            JktjStartButtonStatusInvoke(begin);
            setdataTopage(dt);
        }
        private void JktjStartButtonStatus(bool begin)
        {
            if (begin)
            {
                button_start.Text = "停止";
            }
            else
            {

                button_start.Text = "开始";
            }
        }
        private void JktjStartButtonStatusInvoke(bool begin)
        {
            if (button_start.InvokeRequired)
            {
                button_start.Invoke(new Action(() =>
                {
                    JktjStartButtonStatus(begin);
                }));
            }
            else
            {
                JktjStartButtonStatus(begin);
            }

        }
        private void MaiboboOperation()
        {
            string text = button_start.Text;
            if (text == "开始")
            {

                JktjStartButtonStatus(true);
                TJClient.Devices.Com.Maibobo.SetWriteAction(WriteAction);
                TJClient.Devices.Com.Maibobo.Start();
            }
            else
            {
                TJClient.Devices.Com.Maibobo.End();
                JktjStartButtonStatus(false);
            }

        }

        private void OmronHbp1100UOperation()
        {
            try
            {
                string text = button_start.Text;
                if (text == "开始")
                {

                    JktjStartButtonStatus(true);
                    if (!TJClient.Devices.Usb.Omronhbp1100U.HadSetWriteAction())
                    {
                        TJClient.Devices.Usb.Omronhbp1100U.SetWriteAction(WriteAction);
                    }

                    TJClient.Devices.Usb.Omronhbp1100U.SetCanWrite(true);
                    if (!TJClient.Devices.Usb.Omronhbp1100U.Start())
                    {
                        TJClient.Devices.Usb.Omronhbp1100U.SetCanWrite(false);
                        MessageBox.Show(TJClient.Devices.Usb.Omronhbp1100U.GetError());
                    }
                }
                else
                {
                    TJClient.Devices.Usb.Omronhbp1100U.SetCanWrite(false);
                    //TJClient.Devices.Com.Maibobo.End();
                    JktjStartButtonStatus(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        /// <summary>
        /// 开始测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_start_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_yq.SelectedValue == null || comboBox_yq.SelectedValue.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("请选择仪器！");
                    return;
                }

                //验证注册
                //string YQ_RegisterCode = XmlRW.GetValueFormXML(Common.getyqPath(comboBox_yq.SelectedValue.ToString()), "YQ_RegisterCode", "value");
                //RegisterBll RegisterBllDom = new RegisterBll();
                //string outMsg = "";
                //bool RegisterResult = RegisterBllDom.YqRegister(UserInfo.Yybm, UserInfo.Yymc, comboBox_yq.SelectedValue.ToString(), YQ_RegisterCode, out outMsg);
                //bool RegisterResult = true;
                //if (!RegisterResult)
                //{
                //    MessageBox.Show(outMsg);
                //    return;
                //}

                ////显示提示信息
                //if (outMsg.Length > 0)
                //{
                //    MessageBox.Show(outMsg);
                //}

                //maibobo血压计单独处理，以前的com对接太乱。去掉相关的设置配置文件，因为实施会增加工作量
                if (comboBox_yq.SelectedValue.ToString().ToUpper() == "MAIBOBO")
                {
                    MaiboboOperation();
                    return;
                }

                if (comboBox_yq.SelectedValue.ToString().ToUpper() == "OMRONHBP1100U")
                {
                    OmronHbp1100UOperation();
                    return;
                }

                try
                {
                    if (YQ_Interval.Length > 0 && int.Parse(YQ_Interval) > 0)
                    {
                        //设定自动接收
                        if (timer_yq.Enabled == false)
                        {
                            //间隔时间
                            timer_yq.Interval = int.Parse(YQ_Interval) * 1000;
                            timer_yq.Enabled = true;
                            button_start.Text = "停止";
                        }
                        else
                        {
                            //停止自动接收
                            timer_yq.Enabled = false;
                            button_start.Text = "开始";
                        }
                    }
                    else
                    {
                        button_start.Text = "停止";
                        dt_yq_result = dataRecive();
                        setdataTopage(dt_yq_result);
                        button_start.Text = "开始";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 停止测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_stop_Click(object sender, EventArgs e)
        {
            try
            {
                string errMsg = "";
                if (comboBox_yq.SelectedValue == null || comboBox_yq.SelectedValue.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("请选择仪器！");
                    return;
                }

                if (yqDemo == null)
                {
                    string yqVersion = XmlRW.GetValueFormXML(Common.getyqPath(comboBox_yq.SelectedValue.ToString()), "YQ_Version", "value");
                    yqDemo = LisFactory.LisCreate(yqVersion);
                }

                yqDemo.stop(out errMsg);


                if (errMsg.Length > 0)
                {
                    MessageBox.Show(errMsg);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_result_Click(object sender, EventArgs e)
        {


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
                dt_yq_result = dataRecive();

                //设定结果到页面
                setdataTopage(dt_yq_result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                timer_yq.Enabled = false;
                button_result.Text = "结果(停止)";
                button_start.Text = "开始";
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

            if (comboBox_yq.SelectedValue.ToString().Trim() == "EYETEST")
            {
                if (dt_paraFromParent == null || dt_paraFromParent.Rows.Count == 0)
                {
                    {
                        //停止自动接收
                        timer_yq.Enabled = false;
                        button_start.Text = "开始";

                        MessageBox.Show("已停止获取操作。请先选择一个病人");
                        return null;
                    }
                }
            }
            if (yqDemo == null)
            {
                if (comboBox_yq.SelectedValue == null || comboBox_yq.SelectedValue.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("请选择仪器！");
                    return null;
                }

                string yqVersion = XmlRW.GetValueFormXML(Common.getyqPath(comboBox_yq.SelectedValue.ToString()), "YQ_Version", "value");
                yqDemo = LisFactory.LisCreate(yqVersion);
                yqDemo.open(out errMsg);
            }

            if (yqDemo.IsOpen(out errMsg) == false)
            {
                yqDemo.open(out errMsg);
            }

            // 视力仪器特殊处理
            if (comboBox_yq.SelectedValue.ToString().Trim() == "EYETEST")
            {
                if (string.IsNullOrEmpty(eyesid))
                {
                    //发送个人信息到仪器
                    string strRadCode = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    string strAge = (Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dt_paraFromParent.Rows[0]["SFZH"].ToString().Substring(6, 4))).ToString();
                    eyesid = dt_paraFromParent.Rows[0]["JKDAH"].ToString() + DateTime.Now.ToString("mmssffff");
                    string eyeresult = yqDemo.YQDataReceived(@"/snmpManager.cgi?cgimodule=set&id=" + eyesid + "&name=" + dt_paraFromParent.Rows[0]["XM"].ToString() + "&age=" + strAge + "&sex=" + dt_paraFromParent.Rows[0]["XB"].ToString() + "&temp=" + strRadCode, out errMsg);
                    if (eyeresult != "1")
                    {
                        eyesid = "";
                        MessageBox.Show(eyeresult);
                    }
                }
                else
                {
                    //获取仪器返回的视力信息
                    string strRadCode2 = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    string strAge2 = (Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dt_paraFromParent.Rows[0]["SFZH"].ToString().Substring(6, 4))).ToString();
                    dt = yqDemo.YQDataReturn(@"/snmpManager.cgi?cgimodule=get&id=" + eyesid + "&name=" + dt_paraFromParent.Rows[0]["XM"].ToString() + "&age=" + strAge2 + "&sex=" + dt_paraFromParent.Rows[0]["XB"].ToString() + "&temp=" + strRadCode2, out errMsg);
                    if (dt != null)
                    {
                        //停止自动接收
                        timer_yq.Enabled = false;
                        button_start.Text = "开始";
                        eyesid = "";
                    }
                }
            }
            else if (comboBox_yq.SelectedValue.ToString().Trim() == "F03D")
            {
                timer_yq.Enabled = false;
                errMsg = "";
                DataTable dtResult = null;
                string resultTest = "";
                bool flag = true;
                double tw = 0;
                int maxcnt = 6;
                int nowcnt = 0;

                //接收数据
                if (yqDemo.start(out errMsg))
                {
                    while (flag)
                    {
                        nowcnt++;
                        if (nowcnt == maxcnt)
                        {
                            flag = false;
                            errMsg = "请等额温仪上出现三角连接标志后进行体温扫描，并在测量时靠近主机设备。";
                            MessageBox.Show(errMsg);
                            return null;
                        }
                        Thread.Sleep(4000);

                        //DialogResult result2 = MessageBox.Show("准备就绪，请测量体温后点确定 或者退出测量", "提示", MessageBoxButtons.OKCancel);
                        //if (result2 == DialogResult.OK)
                        //{
                        //接收数据
                        resultTest = yqDemo.getDataReceived(out errMsg);
                        //int twindex = resultTest.IndexOf("ERR");
                        //int zcstate = resultTest.IndexOf("MODE: TTM");
                        int twindex = resultTest.IndexOf("C5C5");
                        if (resultTest != "" && twindex >= 0)
                        {
                            string temp = resultTest.Substring(twindex + 20, 4);
                            tw = Convert.ToInt32(temp.Substring(2, 2) + temp.Substring(0, 2), 16) / 10.0;
                            flag = false;
                        }
                        else if (resultTest == "")
                        {
                            MessageBox.Show("请重新测量。");

                            continue;
                        }
                        else if (twindex < 0)
                        {
                            yqDemo.start(out errMsg);
                        }
                        //}
                        //else flag = false;
                    }
                    if (tw > 0)
                    {
                        dt = yqDemo.YQDataReturn(tw.ToString(), out errMsg);
                        if (dt != null)
                        {
                            //停止自动接收

                            button_start.Text = "开始";
                        }
                        else
                        {
                            MessageBox.Show(errMsg);
                            button_start.Text = "开始";
                        }
                    }
                }
                else
                {
                    MessageBox.Show(errMsg);
                    timer_yq.Enabled = false;
                    button_start.Text = "开始";
                }

            }
            else
            {
                // MessageBox.Show("begin");
                dt = yqDemo.YQDataReturn("", out errMsg);
                //  if (errMsg != "")
                //MessageBox.Show("血压测量完毕。值：" + errMsg);

                if (dt != null && dt.Rows.Count > 0)
                {
                    //停止自动接收
                    timer_yq.Enabled = false;
                    button_start.Text = "开始";
                }
            }
            // string msg="";
            // yqDemo.close(out msg);
            return dt;
        }
        /// <summary>
        /// 设定项目到页面上
        /// </summary>
        /// <param name="dt_yqjg"></param>
        public void setdataTopage(DataTable dt_yqjg)
        {
            jktjBll jktjbll = new jktjBll();
            DataTable dt_dy = jktjbll.GetMoHuList(string.Format(" and  T_JK_LIS_XM.YLJGBM='{0}' ", UserInfo.Yybm), "sql065");
            string itemExcept = ",";

            if (dt_yqjg == null || dt_yqjg.Rows.Count == 0 || dt_dy == null || dt_dy.Rows.Count == 0)
            {
                return;
            }
            else
            {
                //将不是本次测定的项目去掉
                if (comboBox_type.Visible == true)
                {
                    string selectItem = comboBox_type.SelectedValue.ToString();
                    DataTable comboBox_typeList = (DataTable)comboBox_type.DataSource;
                    for (int i = 0; i < comboBox_typeList.Rows.Count; i++)
                    {
                        if (comboBox_typeList.Rows[i]["value"].ToString().Equals(selectItem))
                        {
                            continue;
                        }
                        else
                        {
                            itemExcept = itemExcept + comboBox_typeList.Rows[i]["value"].ToString() + ",";
                        }
                    }
                }
            }

            for (int i = 0; i < dt_dy.Rows.Count; i++)
            {
                if (itemExcept.IndexOf("," + dt_dy.Rows[i]["xmbm"].ToString() + ",") > -1)
                {
                    continue;
                }
                string[] strListT = dt_dy.Rows[i]["KJID"].ToString().Split(new char[] { ',' });
                string[] strListV = dt_dy.Rows[i]["XMBM_LIS"].ToString().Split(new char[] { ',' });
                for (int k = 0; k < strListV.Length; k++)
                {
                    for (int j = 0; j < dt_yqjg.Rows.Count; j++)
                    {
                        //if (dt_yqjg.Rows[j]["xmdh"].ToString() == "PRO")
                        //{
                        //    string s = "im in";
                        //}
                        if (strListV[k].ToLower().Equals(dt_yqjg.Rows[j]["xmdh"].ToString().ToLower()))
                        {
                            string id = controlType_textBox + strListT[k];
                            string value = dt_yqjg.Rows[j]["result"].ToString();

                            setValueToControl(id, value);

                            //设定焦点
                            Control[] control = Controls.Find(id, true);
                            if (control != null && control.Length > 0)
                            {
                                control[0].Focus();
                            }
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 化验数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_hyjg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lis form_sys = new lis();
            form_sys.ShowDialog();
        }

        /// <summary>
        /// 显示上次体检结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_fuzhuluru_CheckedChanged(object sender, EventArgs e)
        {
            ////显示上次体检结果
            //if (checkBox_sctjjg.Checked == true)
            //{
            //    setDataToPage();
            //}
        }

        #region 上次体检结果设定
        /// <summary>
        /// 设定体检结果到页面上  上次体检结果
        /// </summary>
        /// <returns></returns>
        public bool setDataToPage()
        {
            //清空上次体检结果
            if (dt_T_JK_JKTJ_TMP == null || (dt_T_JK_JKTJ_TMP != null && dt_T_JK_JKTJ_TMP.Rows.Count == 0))
            {

                jktjBll jktjbll = new jktjBll();
                dt_T_JK_JKTJ_TMP = jktjbll.GetMoHuList(string.Format(" and D_GRDABH='{0}'", getValueFromDt(dt_paraFromParent, 0, "JKDAH")), "sql066");

                if (dt_T_JK_JKTJ_TMP == null || (dt_T_JK_JKTJ_TMP != null && dt_T_JK_JKTJ_TMP.Rows.Count == 0))
                {
                    setPageClear();

                    for (int i = 0; i < dtControl.Rows.Count; i++)
                    {
                        if (dt_T_JK_JKTJ_TMP.Columns.Contains(dtControl.Rows[i]["kjid"].ToString()))
                        {
                            dtControl.Rows[i]["sctjjg"] = "";
                        }
                    }

                    //没有取得上次体检结果直接退出
                    return true;
                }
            }

            try
            {

                if (dtControl != null && dtControl.Rows.Count > 0 && dt_T_JK_JKTJ_TMP != null && dt_T_JK_JKTJ_TMP.Rows.Count > 0)
                {
                    for (int i = 0; i < dtControl.Rows.Count; i++)
                    {
                        if (dt_T_JK_JKTJ_TMP.Columns.Contains(dtControl.Rows[i]["his_db"].ToString()))
                        {
                            if (dt_T_JK_JKTJ_TMP.Rows[0][dtControl.Rows[i]["his_db"].ToString()] != null && dt_T_JK_JKTJ_TMP.Rows[0][dtControl.Rows[i]["his_db"].ToString()].ToString().IndexOf('@') > -1)
                            {
                                //齿列显示上次体检结果
                                string[] clList = dt_T_JK_JKTJ_TMP.Rows[0][dtControl.Rows[i]["his_db"].ToString()].ToString().Split(new char[] { '@' });
                                if (clList.Length == 4)
                                {
                                    //Controlid
                                    string kjid = dtControl.Rows[i]["Controlid"].ToString();
                                    if (kjid.IndexOf("1") > 0)
                                    {
                                        dtControl.Rows[i]["sctjjg"] = clList[0].Equals("0") == true ? "" : clList[0];
                                    }
                                    else if (kjid.IndexOf("2") > 0)
                                    {
                                        dtControl.Rows[i]["sctjjg"] = clList[1].Equals("0") == true ? "" : clList[1];
                                    }
                                    else if (kjid.IndexOf("3") > 0)
                                    {
                                        dtControl.Rows[i]["sctjjg"] = clList[2].Equals("0") == true ? "" : clList[2];
                                    }
                                    else if (kjid.IndexOf("4") > 0)
                                    {
                                        dtControl.Rows[i]["sctjjg"] = clList[3].Equals("0") == true ? "" : clList[3];
                                    }
                                }
                            }
                            else
                            {
                                dtControl.Rows[i]["sctjjg"] = dt_T_JK_JKTJ_TMP.Rows[0][dtControl.Rows[i]["his_db"].ToString()] != null ? dt_T_JK_JKTJ_TMP.Rows[0][dtControl.Rows[i]["his_db"].ToString()].ToString() : "";
                            }

                        }
                    }

                    //for (int i = 0; i < dtControl.Rows.Count; i++)
                    //{
                    //    if (dt_T_JK_JKTJ_TMP.Columns.Contains(dtControl.Rows[i]["kjid"].ToString()))
                    //    {
                    //        dtControl.Rows[i]["sctjjg"] = dt_T_JK_JKTJ_TMP.Rows[0][dtControl.Rows[i]["kjid"].ToString()]!=null ? dt_T_JK_JKTJ_TMP.Rows[0][dtControl.Rows[i]["kjid"].ToString()].ToString(): "" ;
                    //    }
                    //}
                }

                //将信息设定到页面上
                setTjResultToPage_Sc();

                dt_T_JK_JKTJ_TMP.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }

        /// <summary>
        /// 将数据库中检索到的数据设定到页面中
        /// </summary>
        private void setTjResultToPage_Sc()
        {
            if (dtControl == null)
            {
                return;
            }

            //取得控件的值
            for (int i = 0; i < dtControl.Rows.Count; i++)
            {
                //上次体检结果
                if (CommomSysInfo.IsSctjjg.Equals("1"))
                {
                    if (dtControl.Rows[i]["sctjjg"] != null && dtControl.Rows[i]["sctjjg"].ToString().Length > 0 && (dtControl.Rows[i]["value"] == null || (dtControl.Rows[i]["value"] != null && dtControl.Rows[i]["value"].ToString().Length == 0)))
                    {
                        setValueToControl_Sc(dtControl.Rows[i]["ControlId"].ToString(), dtControl.Rows[i]["sctjjg"].ToString());
                    }
                }

                //dataGridView 用药情况
                if (CommomSysInfo.IsSctjjg_yyqk.Equals("1"))
                {
                    setValueToControl_dataGridView_sc(dtControl.Rows[i]["ControlId"].ToString());
                }
            }
        }

        /// <summary>
        /// 给控件赋值
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private void setValueToControl_Sc(string ControlId, string ControlValue)
        {
            try
            {
                Control control = Controls.Find(ControlId, true)[0];
                //text
                if (ControlId.IndexOf(controlType_textBox) > -1)
                {
                    TextBox TextBox_tem = (TextBox)control;
                    TextBox_tem.Text = ControlValue;
                    TextBox_tem.ForeColor = Color.Blue;
                }
                //checkBox
                else if (ControlId.IndexOf(controlType_checkBox) > -1)
                {
                    CheckBox checkBox_tem = (CheckBox)control;

                    if (ControlValue.Trim().Length > 0 && checkBox_tem.Tag != null && ("," + ControlValue.Trim().ToString() + ",").IndexOf("," + checkBox_tem.Tag.ToString().ToLower() + ",") > -1)
                    {
                        checkBox_tem.Checked = true;
                        checkBox_tem.ForeColor = Color.Blue;
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
                        radioButton_tem.ForeColor = Color.Blue;
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
                    LinkLabel_tem.Text = ControlValue;
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        //#endregion


        /// <summary>
        /// 页面关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jktj_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                TJClient.Devices.Usb.Omronhbp1100U.End(); //关闭血压计欧姆龙设备
                System.Environment.Exit(System.Environment.ExitCode);
                this.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        /// <summary>
        /// imag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_img_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            LinkLabel linklabel = (LinkLabel)sender;
            string xdturl = linklabel.Tag != null ? linklabel.Tag.ToString() : "";
            if (xdturl.Length > 0 && File.Exists(xdturl))
            {
                img IMG = new img();
                Image imgmodel = Image.FromFile(xdturl);
                IMG.setImg(imgmodel);
                IMG.Show();
            }
            else
            {
                MessageBox.Show("没有发现对应的图片，请确认！");
            }


        }

        /// <summary>
        /// 前一页面传过来的人员信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public override bool setParaToChild(DataTable para)
        {
            dt_paraFromParent = para;

            //健康体检
            if (selectFromDb(true) == false)
            {
                //显示上次体检结果
                lastDataTopage();

                // 随机选择设定的签名信息
                ChangeSignnamePicRandom();
            }
            else
            {

                //清空已选定的签名
                //cboSignname.SelectedIndex = -1;
                //显示已经签过名的签名信息
                ChangeSignnamePic();
            }


            //初始化后设定页面的编辑状态
            CommomSysInfo.IsEdited = "0";
            return true;
        }

        /// <summary>
        /// 显示上次体检信息
        /// </summary>
        /// <returns></returns>
        public bool lastDataTopage()
        {
            //健康体检
            if (selectFromDb(false) == false)
            {
                //显示上次体检结果
                if (CommomSysInfo.IsSctjjg.Equals("1"))
                {
                    setDataToPage();
                }

                else if (CommomSysInfo.IsSctjjg.Equals("0") && CommomSysInfo.IsSctjjg_yyqk.Equals("1"))
                {
                    setDataToPage();
                }
            }
            return true;
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

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_delete_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 同步检验结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_lis_Click(object sender, EventArgs e)
        {
            try
            {
                string JKDAH = getValueFromDt(dt_paraFromParent, 0, "JKDAH");
                if (JKDAH.Length > 0)
                {
                    Common common = new Common();
                    DataTable dt_jktj = new DataTable();
                    dt_jktj.Rows.Add();
                    dt_jktj.Columns.Add("JKDAH");

                    dt_jktj.Rows[0]["JKDAH"] = JKDAH;
                    string msg = common.updateLis_jktj(dt_jktj);
                    if (msg.Length > 0)
                    {
                        MessageBox.Show(msg);
                    }
                    else
                    {
                        //健康体检
                        if (selectFromDb(true) == false)
                        {
                            //显示上次体检结果
                            lastDataTopage();
                        }
                        //初始化后设定页面的编辑状态
                        CommomSysInfo.IsEdited = "0";
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


        private void ComboBoxTypeDataBind(string yqItems)
        {
            //panel_LR.Visible = true;
            comboBox_type.Visible = true;

            DataTable dt = new DataTable();
            dt.Columns.Add("text");
            dt.Columns.Add("value");

            string[] itemsList = yqItems.Split(new char[] { '|' });
            for (int i = 0; i < itemsList.Length; i++)
            {
                dt.Rows.Add();
                string[] item = itemsList[i].Split(new char[] { ',' });
                dt.Rows[dt.Rows.Count - 1]["text"] = item[0];
                dt.Rows[dt.Rows.Count - 1]["value"] = item[1];
            }
            comboBox_type.DataSource = dt;
            comboBox_type.DisplayMember = "text";
            comboBox_type.ValueMember = "value";
        }

        private void comboBox_yq_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboBox_yq.SelectedValue.ToString().ToUpper() == "OMRONHBP1100U") //不需要配置文件
            //{
            //    return;
            //}
            if (comboBox_yq.SelectedValue.ToString().ToUpper() == "MAIBOBO")
            {
                if (!TJClient.Devices.Com.Maibobo.XmlFileExisted())
                {
                    MessageBox.Show("仪器配置文件不存在!");
                }
                else
                {
                    ComboBoxTypeDataBind(TJClient.Devices.Com.Maibobo.GetYqItems());
                }

                return;
            }
            string YqXmlPath = "";
            if (yqDemo != null)
            {
                string msg = "";
                yqDemo.close(out msg);
            }
            yqDemo = null;


            if (drpFlag == true && comboBox_yq.SelectedValue != null && comboBox_yq.SelectedValue.ToString().Length > 0)
            {
                //xmlpath
                if (Common.getyqPath(comboBox_yq.SelectedValue.ToString()).Length <= 0)
                {
                    MessageBox.Show("仪器配置文件不存在!");
                    return;
                }
                YqXmlPath = Common.getyqPath(comboBox_yq.SelectedValue.ToString());

                //jg数据处理间隔时间
                YQ_Interval = XmlRW.GetValueFormXML(YqXmlPath, "YQ_Interval", "value");

                //项目类型 单项目还是多个项目 比如血压  分为左侧血压和右侧血压   0:单项目 1:多项目
                YQ_itemType = XmlRW.GetValueFormXML(YqXmlPath, "YQ_itemType", "value");

                //项目  项目1名称,体检项目编码1|项目2名称,体检项目编码2
                YQ_items = XmlRW.GetValueFormXML(YqXmlPath, "YQ_items", "value");

                //多个项目
                if (YQ_itemType.Equals("1"))
                {
                    ComboBoxTypeDataBind(YQ_items);
                }
                else
                {
                    //panel_LR.Visible = false;
                    comboBox_type.Visible = false;
                }
            }
            //停止自动接收
            timer_yq.Enabled = false;
            button_start.Text = "开始";
        }

        //写字板签名
        private void btnTabletSignname_Click(object sender, EventArgs e)
        {
            string way = string.Format("姓名：{0}  性别：{1}  身份证号：{2}", getValueFromDt(dt_paraFromParent, 0, "XM"),
                getValueFromDt(dt_paraFromParent, 0, "XB"), getValueFromDt(dt_paraFromParent, 0, "SFZH"));
            string saveSignnamePicPath = TJClient.Signname.Operation.GetTabletSignnamePicPath();
            string msg = TJClient.Signname.TabletHelper.TabletSignname(way, picSignname1111, saveSignnamePicPath);
            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
            }
        }

        private void cboSignname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSignname.SelectedIndex < 0)
            {
                return;
            }
            TJClient.Signname.ControlOperation.SignnamePicInit(picSignname1111, TJClient.Signname.Operation.SignnamePicPath(
                    TJClient.Signname.ControlOperation.SignnameTitle(cboSignname)), ((UserSignname)cboSignname.SelectedItem).Realname, textBox_realname);
        }
    }
}
