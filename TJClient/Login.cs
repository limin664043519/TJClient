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
using TJClient;
using TJClient.Common;
namespace FBYClient
{
    public partial class Login : Form
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

        private Point mPoint;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }

        #endregion

        #region 窗体控件、事件
        /// <summary>
        /// 窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_Load(object sender, EventArgs e)
        {
            DBAccess dBAccess = new DBAccess();
            //取得医疗机构编码
            DataTable dt = dBAccess.ExecuteQueryBySql(" SELECT Xt_gg_sydw.bm, Xt_gg_sydw.mc FROM Xt_gg_sydw ");
            //绑定医疗机构
            comFz.DataSource = dt;
            comFz.DisplayMember = "mc";
            comFz.ValueMember = "bm";

            string yljgbm = ConfigurationSettings.AppSettings["YLJGBM"].ToString();
            if (yljgbm.Length > 0)
            {
                comFz.SelectedValue = yljgbm;
                comFz.Enabled = false;
            }

            //设定默认的用户名
            string defaut_username = System.Configuration.ConfigurationManager.AppSettings["defaut_username"];
            txtCzy.Text = defaut_username;

            txtCzy.Focus();
        }

        /// <summary>
        /// 登录处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //是否初次登陆
                string ifinitLogin = ConfigurationSettings.AppSettings["ifinitLogin"].ToString();
                if (ifinitLogin.Equals("1") || (UserInfo.logInMode!=null && UserInfo.logInMode.Equals("1")))
                {
                    string yljgInit = comFz.Text;
                    if (txtCzy.Text.Equals("admin"))
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("userId");
                        dt.Columns.Add("jkda_czy");
                        dt.Columns.Add("gzz");
                        dt.Columns.Add("yljg");
                        dt.Rows.Add();

                        dt.Rows[0]["userId"] = "admin";
                        dt.Rows[0]["jkda_czy"] = "admin";
                        dt.Rows[0]["yljg"] = yljgInit;
                        dt.Rows[0]["gzz"] = "admin";

                        this.Tag = dt;


                        //登录信息
                        UserInfo.Yybm = yljgInit;
                        UserInfo.Username = "admin";
                        UserInfo.userId = "admin";
                        UserInfo.gzz = "";

                        Main_Form formJktj = new Main_Form();
                        formJktj.Owner = this;
                        this.Hide();
                        formJktj.Show();

                        return;
                    }
                    else
                    {
                        MessageBox.Show("登录信息不正确，请确认");
                        return;
                    }
                }

                DBAccess dBAccess = new DBAccess();
                //个人信息
                string sql = "SELECT Xt_gg_czy.bm, Xt_gg_czy.pym, Xt_gg_czy.xm, Xt_gg_czy.yybh FROM Xt_gg_czy where bm='{bm}' and yybh='{yybm}' and (kl='{kl}' or kl is null )";
                DataTable dtResult = dBAccess.ExecuteQueryBySql(sql.Replace("{bm}", txtCzy.Text).Replace("{yybm}", comFz.SelectedValue.ToString()).Replace("{kl}", txtMm.Text));
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    //工作组
                    string sqlGzz = "SELECT YLJGBM, GZZBM, CZY, JKDA_CZY FROM T_JK_GZZ_CZY where CZY='{CZY}' and YLJGBM='{YLJGBM}' ";
                    DataTable dtGzz = dBAccess.ExecuteQueryBySql(sqlGzz.Replace("{CZY}", txtCzy.Text).Replace("{YLJGBM}", comFz.SelectedValue.ToString()));
                    DataTable dt = new DataTable();
                    if (dtGzz != null && dtGzz.Rows.Count > 0)
                    {
                        //DataTable dt = new DataTable();
                        dt.Columns.Add("userId");
                        dt.Columns.Add("jkda_czy");
                        dt.Columns.Add("gzz");
                        dt.Columns.Add("yljg");
                        dt.Rows.Add();

                        dt.Rows[0]["userId"] = txtCzy.Text;
                        dt.Rows[0]["jkda_czy"] = dtGzz.Rows[0]["JKDA_CZY"].ToString();
                        dt.Rows[0]["yljg"] = comFz.SelectedValue.ToString();
                        dt.Rows[0]["gzz"] = dtGzz.Rows[0]["GZZBM"].ToString();

                        //登录信息
                        UserInfo.Yybm = comFz.SelectedValue.ToString();
                        UserInfo.Yymc = comFz.Text.ToString();
                        UserInfo.Username = dtResult.Rows[0]["xm"].ToString();
                        UserInfo.userId = txtCzy.Text;
                        UserInfo.gzz = dtGzz.Rows[0]["GZZBM"].ToString();
                    }
                    else
                    {
                        //用户没有分配工作组
                        dt.Columns.Add("userId");
                        dt.Columns.Add("jkda_czy");
                        dt.Columns.Add("gzz");
                        dt.Columns.Add("yljg");
                        dt.Rows.Add();

                        dt.Rows[0]["userId"] = txtCzy.Text;
                        dt.Rows[0]["jkda_czy"] = "";
                        dt.Rows[0]["yljg"] = comFz.SelectedValue.ToString();
                        dt.Rows[0]["gzz"] = "";

                        //登录信息
                        UserInfo.Yybm = comFz.SelectedValue.ToString();
                        UserInfo.Yymc = comFz.Text.ToString();
                        UserInfo.Username = dtResult.Rows[0]["xm"].ToString();
                        UserInfo.userId = txtCzy.Text;
                        UserInfo.gzz ="";

                        MessageBox.Show("该用户没有分配工作组！");
                    }

                   

                    this.Tag = dt;
                    Main_Form formJktj = new Main_Form();
                    formJktj.Owner = this;
                    this.Hide();
                    formJktj.Show();

                    ////测试lis
                    //jktj formJktj = new jktj();
                    //formJktj.Owner = this;
                    //this.Hide();
                    //formJktj.Show();

                    //formJktj.Focus();
                   
                }
                else
                {
                    MessageBox.Show("登录信息不正确，请确认");
                    txtCzy.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录信息不正确，请确认！"+ex.Message );
            }

        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 分组回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comFz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtCzy.Focus();
            }
        }
        /// <summary>
        /// 用户名回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCzy_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                txtMm.Focus();
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.CapsLock) 
            {
                DialogResult dr = MessageBox.Show("是否启用无用户登陆模式？", "登陆系统模式", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    UserInfo.logInMode = "1";
                    label_mod.Text = "无用户登陆模式";

                    comFz.Enabled = true;
                }
                else
                {
                    UserInfo.logInMode = "0";
                    label_mod.Text = "";
                }
            }
        }
        /// <summary>
        /// 密码框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonLogin.Focus();
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult dr = MessageBox.Show("确定退出系统吗？", "退出系统", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
            
        }
       
        #endregion

        #region 窗体移动事件

        /// <summary>
        /// MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_MouseDown(object sender, MouseEventArgs e)
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

        /// <summary>
        /// MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_MouseMove(object sender, MouseEventArgs e)
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

        /// <summary>
        /// MouseUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_MouseUp(object sender, MouseEventArgs e)
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

        /// <summary>
        /// 登陆按钮鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_MouseLeave(object sender, EventArgs e)
        {
            this.buttonLogin.BackgroundImage = global ::TJClient.Properties.Resources.login_btn1_2;
        }
        /// <summary>
        /// 登陆按钮鼠标移上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_MouseMove(object sender, MouseEventArgs e)
        {
            this.buttonLogin.BackgroundImage = global::TJClient.Properties.Resources.login_btn1_1;
        }
        /// <summary>
        /// 退出按钮鼠标移上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_MouseLeave(object sender, EventArgs e)
        {
            //this.buttonClose.BackgroundImage = global::TJClient.Properties.Resources.login_btn2_1;
        }
        /// <summary>
        /// 退出按钮鼠标移上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_MouseMove(object sender, MouseEventArgs e)
        {
            //this.buttonClose.BackgroundImage = global::TJClient.Properties.Resources.login_btn2_2;
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }



    }
}
