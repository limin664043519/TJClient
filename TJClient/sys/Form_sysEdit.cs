using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Yibaoxiao.sysmain;

namespace TJClient.sys
{
    public partial class Form_sysEdit : Form
    {
        public Form_sysEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 数据字典
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_sjzd_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Form_Sjzd frm = new Form_Sjzd();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_sjzd);
        }



        ///// <summary>
        ///// 数据库信息设定
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void button8_Click_1(object sender, EventArgs e)
        //{
        //    panel3.Controls.Clear();
        //    sysmain.DbSet frm = new sysmain.DbSet();
        //    frm.Dock = DockStyle.Fill;
        //    frm.TopLevel = false;
        //    //
        //    panel3.Controls.Add(frm);
        //    frm.Show();

        //    setPageTitle(button8);
        //}

        //private void SysMain_Load(object sender, EventArgs e)
        //{
        //    //系统名称设定
        //    this.Text = UserInfo.xtmc;
        //    if (UserInfo.qf != "1")
        //    {
        //        button1_Click(sender, e);
        //    }
        //    else
        //    {
        //        setnull(sender, e);
        //    }


        //}


        //private void setnull(object sender, EventArgs e)
        //{
        //    this.button1.Visible = false;
        //    this.button2.Visible = false;
        //    this.button3.Visible = false;
        //    this.button4.Visible = false;
        //    this.button5.Visible = false;
        //    this.button6.Visible = false;
        //    this.button7.Visible = false;
        //    this.button8.Visible = false;
        //    button_printSet.Visible = false;
        //    button8_Click_1(sender, e);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void SysMain_FormClosing(object sender, FormClosingEventArgs e)
        //{

        //    if (UserInfo.Username == null || UserInfo.Username.Trim().Length == 0)
        //    {
        //        DialogResult result;
        //        result = MessageBox.Show("确定退出吗？ \r\n是：退出系统 \r\n否：返回登录页面 \r\n取消：取消本次操作", "退出", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        //        if (result == DialogResult.Cancel)
        //        {
        //            e.Cancel = true;
        //        }
        //        else if (result == DialogResult.Yes)
        //        {
        //            //退出系统时清空登陆信息
        //            Common.clearUserInfo();
        //            Application.ExitThread();

        //        }
        //        else
        //        {
        //            //退出系统时清空登陆信息
        //            Common.clearUserInfo();
        //            this.Owner.Show();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 修改密码
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void button8_Click(object sender, EventArgs e)
        //{
        //    panel3.Controls.Clear();
        //    sysmain.pwd frm = new sysmain.pwd();
        //    frm.Dock = DockStyle.Fill;
        //    frm.TopLevel = false;
        //    //
        //    panel3.Controls.Add(frm);
        //    frm.Show();
        //}


        /// <summary>
        /// 设定页面上按钮的状态
        /// </summary>
        /// <param name="button"></param>
        private void setPageTitle(Button button)
        {
            label1.Text = button.Text;
        }

        /// <summary>
        /// 项目制御
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_kjzy_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Form_xmzy frm = new Form_xmzy();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_kjzy);
        }

        /// <summary>
        /// 项目分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_xmfl_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Form_xmfl frm = new Form_xmfl();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_xmfl);
        }
        /// <summary>
        /// 项目管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_xm_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Form_xm frm = new Form_xm();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_xm);
        }

        /// <summary>
        /// 工作组管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_gzz_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Form_gzz frm = new Form_gzz();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_gzz);
        }

        /// <summary>
        /// 工作组项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_gzzxm_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Form_gzzXm frm = new Form_gzzXm();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_gzzxm);
        }

        /// <summary>
        /// 工作组人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_gzzry_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Form_gzzRy frm = new Form_gzzRy();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_gzzry);
        }

        /// <summary>
        /// 条形码管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_txm_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Form_txm frm = new Form_txm();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_txm);
        }

        /// <summary>
        /// 条形码打印设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_printSet_Click(object sender, EventArgs e)
        {
            //panel3.Controls.Clear();
            //Form_txmPrintSet frm = new Form_txmPrintSet();
            //frm.Dock = DockStyle.Fill;
            //frm.TopLevel = false;
            ////
            //panel3.Controls.Add(frm);
            //frm.Show();
            //setPageTitle(button_printSet);
        }

        /// <summary>
        /// 医疗机构管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_yljg_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Form_yljg frm = new Form_yljg();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_yljg);
        }

        /// <summary>
        /// 仪器管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_yq_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();

            Form_lisSet frm = new Form_lisSet();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_yq);
        }

        /// <summary>
        /// 数据库设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_dbset_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();

            DbSet frm = new DbSet();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            //
            panel3.Controls.Add(frm);
            frm.Show();
            setPageTitle(button_dbset);
        }

       
    }
}
