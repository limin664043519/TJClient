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
    public partial class LnrXwnlpg : sysCommonForm
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
            lis_Recevier = 1;
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
        public LnrXwnlpg()
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
            //用户id
            userId = dt_para_sys.Rows[0]["userId"].ToString();
            //工作组
            yhfz = dt_para_sys.Rows[0]["gzz"].ToString();
            //医疗机构
            yljg = dt_para_sys.Rows[0]["yljg"].ToString();
            DBAccess dBAccess = new DBAccess();

            //下次随访日期
            dateTimePicker_XCSFRQ_lkzp.Value = DateTime.Now.AddYears(1);

        }
        #endregion

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
                    // datagridview_bool = false;
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

        #endregion

        #region 健康自评
        /// <summary>
        /// 答案事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton RadioButton_tem = (RadioButton)sender;
            string controlId = controlType_textBox + RadioButton_tem.Name.Substring(12, 3) + "lkzp";
            setValueToControl(controlId, RadioButton_tem.Tag.ToString());
            //计算总分
            calculate_xlzp();
        }

        /// <summary>
        /// 计算评分
        /// </summary>
        private void calculate_xlzp()
        {
            int pf = 0;
            for (int i = 0; i < 5; i++)
            {
                string controlId = controlType_textBox + (i + 1).ToString().PadLeft(2, '0') + "_lkzp";
                Control control = Controls.Find(controlId, true)[0];

                TextBox TextBox_tem = (TextBox)control;
                if (TextBox_tem.Text.Length > 0)
                {
                    pf = pf + int.Parse(TextBox_tem.Text);
                }
            }
            //设定总分
            setValueToControl(controlType_textBox + "06_lkzp", pf.ToString());
        }

        /// <summary>
        /// 取得Guid
        /// </summary>
        /// <returns></returns>
        private string getGuid_xlzp()
        {
            string guid = System.Guid.NewGuid().ToString();
            DBAccess dBAccess = new DBAccess();
            string sql = "";
            ArrayList sqlList = new ArrayList();
            sql = "select guid from T_JG_LNRSF where czy='{czy}' and gzz='{gzz}' and D_GRDABH='{d_grdabh}' and  HAPPENTIME ='{happentime}'";
            //操作员
            sql = sql.Replace("{czy}", userId);
            //工作组
            sql = sql.Replace("{gzz}", yhfz);
            //健康档案编号
            sql = sql.Replace("{d_grdabh}", textBox_head_JKDAH.Text);
            //体检日期
            sql = sql.Replace("{happentime}", dateTimePicker_SFRQ_lkzp.Value.ToString("yyyy-MM-dd"));
            DataTable dt = dBAccess.ExecuteQueryBySql(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                guid = dt.Rows[0]["guid"].ToString();
            }
            return guid;
        }


        #region 心里自评sql
        public string insert_xlzp = @"insert into T_JG_LNRSF(
                                                D_GRDABH,
                                                G_XCSFMB,
                                                G_XCSFRQ,
                                                G_SFYS,
                                                CREATREGION,
                                                CREATEUSER,
                                                P_RGID,
                                                CREATETIME,
                                                UPDATETIME,
                                                HAPPENTIME,
                                                G_LRRQ,
                                                QDQXZ,
                                                JCPF,
                                                SXPF,
                                                RCPF,
                                                HDPF,
                                                ZPF,
                                                CYPF,
                                                [guid],
                                                czy,
                                                gzz)
                                                values(
        
                                                '{D_GRDABH}',
                                                '{G_XCSFMB}',
                                                '{G_XCSFRQ}',
                                                '{G_SFYS}',
                                                '{CREATREGION}',
                                                '{CREATEUSER}',
                                                '{P_RGID}',
                                                '{CREATETIME}',
                                                '{UPDATETIME}',
                                                '{HAPPENTIME}',
                                                '{G_LRRQ}',
                                                '{QDQXZ}',
                                                '{JCPF}',
                                                '{SXPF}',
                                                '{RCPF}',
                                                '{HDPF}',
                                                '{ZPF}',
                                                '{CYPF}',
                                                '{guid}',
                                                '{czy}',
                                                '{gzz}')";
        #endregion

        /// <summary>
        /// 健康自评保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            // string guid = System.Guid.NewGuid().ToString();
            DBAccess dBAccess = new DBAccess();

            string sql = "";
            //string key = "";
            //string value = "";
            //string kjlx = "";
            //string his_db = "";
            try
            {
                ArrayList sqlList = new ArrayList();
                sql = "delete from T_JG_LNRSF where czy='{czy}' and gzz='{gzz}' and d_grdabh='{d_grdabh}' and  happentime ='{happentime}'";
                //操作员
                sql = sql.Replace("{czy}", userId);
                //工作组
                sql = sql.Replace("{gzz}", yhfz);
                //健康档案编号
                sql = sql.Replace("{d_grdabh}", textBox_head_JKDAH.Text);
                //体检日期
                sql = sql.Replace("{happentime}", getValueFromDt(dt_paraFromParent, 0, "TJSJ"));
                sqlList.Add(sql);

                //插入
                sql = insert_xlzp;

                sql = sql.Replace("{D_GRDABH}", textBox_head_JKDAH.Text);       //编号
                sql = sql.Replace("{G_XCSFMB}", richTextBox_XCSFMB_lkzp.Text);		  //下次随访目标
                sql = sql.Replace("{G_XCSFRQ}", dateTimePicker_XCSFRQ_lkzp.Value.ToString("yyyy-MM-dd"));		  //下次随访日期
                sql = sql.Replace("{G_SFYS}", lTextBox_SFYS_lkzp.Text);			  //随访医生签名
                //sql = sql.Replace("{G_SFYS}", CommomSysInfo.TJFZR_BM);			  //随访医生签名
                sql = sql.Replace("{CREATREGION}", yljg);	  //创建机构
                sql = sql.Replace("{CREATEUSER}", userId);		  //创建人
                sql = sql.Replace("{P_RGID}", yljg);			  //当前所属机构
                sql = sql.Replace("{CREATETIME}", DateTime.Now.ToString("yyy-MM-dd"));		  //创建时间
                sql = sql.Replace("{UPDATETIME}", DateTime.Now.ToString("yyy-MM-dd"));		  //更新时间
                //sql = sql.Replace("{HAPPENTIME}", dateTimePicker_SFRQ_lkzp.Value.ToString("yyyy-MM-dd"));		  //随访日期
                //sql = sql.Replace("{G_LRRQ}", dateTimePicker_SFRQ_lkzp.Value.ToString("yyyy-MM-dd"));			  //录入日期
                sql = sql.Replace("{HAPPENTIME}", CommomSysInfo.tjsj );		  //随访日期
                sql = sql.Replace("{G_LRRQ}", CommomSysInfo.tjsj);			  //录入日期
                sql = sql.Replace("{QDQXZ}", "");			  //缺省值
                sql = sql.Replace("{JCPF}", textBox_01_lkzp.Text);			  //进餐
                sql = sql.Replace("{SXPF}", textBox_02_lkzp.Text);			  //梳洗
                sql = sql.Replace("{CYPF}", textBox_03_lkzp.Text);			  //穿衣
                sql = sql.Replace("{RCPF}", textBox_04_lkzp.Text);			  //如厕
                sql = sql.Replace("{HDPF}", textBox_05_lkzp.Text);			  //活动
                sql = sql.Replace("{ZPF}", textBox_06_lkzp.Text);			  //总评分
                sql = sql.Replace("{guid}", getGuid_xlzp());//guid
                sql = sql.Replace("{czy}", userId);//czy
                sql = sql.Replace("{gzz}", yhfz);//gzz

                sqlList.Add(sql);

                //更新体检人员信息表(T_JK_TJRYXX）
                //sqlList.Add(save_T_JK_TJRYXX(null, dateTimePicker_SFRQ_lkzp.Value.ToString("yyyy-MM-dd"), "TJZT_jkzp", "TJSJ_jkzp"));
                ArrayList TJRYXXList = save_T_JK_TJRYXX(dt_paraFromParent, CommomSysInfo.tjsj, Common.TJTYPE.生活自理能力评估, Common.ZT.确定状态);
                if (TJRYXXList != null && TJRYXXList.Count > 0)
                {
                    for (int i = 0; i < TJRYXXList.Count; i++)
                    {
                        sqlList.Add(TJRYXXList[i]);
                    }
                }

                dBAccess.ExecuteNonQueryBySql(sqlList);
                MessageBox.Show("保存成功！");

                //调用父页面的方法
                if (main_form != null)
                {
                    main_form.setParentFormDo(null);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！" + ex.Message );
            }
        }


        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            clear_xlzp();
        }

        /// <summary>
        /// 心里自评清空页面上的内容
        /// </summary>
        private void clear_xlzp()
        {
            //rdo_01_99.Checked = true;
            //rdo_02_99.Checked = true;
            //rdo_03_99.Checked = true;
            //rdo_04_99.Checked = true;
            //rdo_05_99.Checked = true;
            radioButton_01_0.Checked = true;
            radioButton_02_0.Checked = true;
            radioButton_03_0.Checked = true;
            radioButton_04_0.Checked = true;
            radioButton_05_0.Checked = true;
            textBox_01_lkzp.Text = "0";
            textBox_02_lkzp.Text = "0";
            textBox_03_lkzp.Text = "0";
            textBox_04_lkzp.Text = "0";
            textBox_05_lkzp.Text = "0";
            textBox_06_lkzp.Text = "0";
            richTextBox_XCSFMB_lkzp.Text = "";

            lTextBox_SFYS_lkzp.Text ="";

        }


        /// <summary>
        /// 检索当前数据数据
        /// </summary>
        private void select_xlzp()
        {
            string select_sql =@"select D_GRDABH,
                                    G_XCSFMB,
                                    G_XCSFRQ,
                                    G_SFYS,
                                    CREATREGION,
                                    CREATEUSER,
                                    P_RGID,
                                    CREATETIME,
                                    UPDATETIME,
                                    HAPPENTIME,
                                    G_LRRQ,
                                    QDQXZ,
                                    JCPF,
                                    SXPF,
                                    RCPF,
                                    HDPF,
                                    ZPF,
                                    CYPF
                                    from T_JG_LNRSF
                                    where 
                                    D_GRDABH='{D_GRDABH}'
                                    and 
                                    HAPPENTIME='{HAPPENTIME}'";


            select_sql = select_sql.Replace("{D_GRDABH}", textBox_head_JKDAH.Text).Replace("{HAPPENTIME}", getValueFromDt(dt_paraFromParent, 0, "TJSJ"));

            DBAccess access = new DBAccess();
            DataTable dt_xlzp = new DataTable();
            dt_xlzp = access.ExecuteQueryBySql(select_sql);

            if (dt_xlzp != null && dt_xlzp.Rows.Count > 0)
            {
                //进餐
                setValueToControl(controlType_radioButton + "01_" + dt_xlzp.Rows[0]["JCPF"].ToString(), dt_xlzp.Rows[0]["JCPF"].ToString());
                //梳洗
                setValueToControl(controlType_radioButton + "02_" + dt_xlzp.Rows[0]["SXPF"].ToString(), dt_xlzp.Rows[0]["SXPF"].ToString());
                //穿衣
                setValueToControl(controlType_radioButton + "03_" + dt_xlzp.Rows[0]["CYPF"].ToString(), dt_xlzp.Rows[0]["CYPF"].ToString());
                //如厕
                setValueToControl(controlType_radioButton + "04_" + dt_xlzp.Rows[0]["RCPF"].ToString(), dt_xlzp.Rows[0]["RCPF"].ToString());
                //活动
                setValueToControl(controlType_radioButton + "05_" + dt_xlzp.Rows[0]["HDPF"].ToString(), dt_xlzp.Rows[0]["HDPF"].ToString());

                //下次随访目标
                richTextBox_XCSFMB_lkzp.Text = dt_xlzp.Rows[0]["G_XCSFMB"].ToString();

                //下次随访日期
                dateTimePicker_XCSFRQ_lkzp.Value = getDateFromString(dt_xlzp.Rows[0]["G_XCSFRQ"].ToString());

                // 随访医生签名
                if (dt_xlzp.Rows[0]["G_SFYS"] != null)
                {
                    lTextBox_SFYS_lkzp.Text = dt_xlzp.Rows[0]["G_SFYS"].ToString();
                }
                else
                {
                    lTextBox_SFYS_lkzp.Text = CommomSysInfo.TJFZR_MC;
                }
                //随访日期
                dateTimePicker_SFRQ_lkzp.Value = getDateFromString(dt_xlzp.Rows[0]["HAPPENTIME"].ToString());
            }
            else
            {
                clear_xlzp();
                lTextBox_SFYS_lkzp.Text = CommomSysInfo.TJFZR_MC;
            }


        }

        /// <summary>
        /// 下次随访目标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox_XCSFMB_lkzp_DoubleClick(object sender, EventArgs e)
        {
        //    //TextBox_tem_fzlr = text_tem;
        //    if (checkBox_fzlr.Checked == false)
        //    {
        //        return ;
        //    }

            RichTextBox richtextbox = (RichTextBox)sender;

            if (richtextbox.Tag != null && richtextbox.Tag.ToString().Length > 0)
            {
                Formfzlr formfzlr = new Formfzlr();
                formfzlr.Owner = this;
                formfzlr.setListData(string.Format(" and ZDLXBM='{0}'", richtextbox.Tag.ToString()), "sql_select_sjzd", richtextbox);
                formfzlr.ShowDialog();
            }
            else
            {
                MessageBox.Show("没有设定辅助录入内容！");
            }
        }

        /// <summary>
        /// 设定返回的结果
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public override  bool setTextToText(Control text_tem, string strText)
        {
            string[] textList = strText.Split(new char[]{'|'});
            if (textList.Length == 2)
            {
                text_tem.Text = text_tem.Text + " " + textList[1];
            }
            else
            {
                text_tem.Text = text_tem.Text + " " + strText;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 前一页面传过来的人员信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public override bool setParaToChild(DataTable para)
        {
            dt_paraFromParent = para;
            //健康档案号
            textBox_head_JKDAH.Text = getValueFromDt(dt_paraFromParent, 0, "JKDAH");
            setpageInit();
            //心里自评
            select_xlzp();
            
            return true;
        }

        /// <summary>
        /// 体检医生及体检时间 改变是的处理
        /// </summary>
        public void setpageInit()
        {
            //体检日期设定
            dateTimePicker_SFRQ_lkzp.Value = Convert.ToDateTime(CommomSysInfo .tjsj);

            //随访医生
            lTextBox_SFYS_lkzp.Text = CommomSysInfo.TJFZR_MC;
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
