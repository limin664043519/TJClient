using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.sys.Bll;

namespace TJClient.sys
{
    public partial class listboxForm : Form
    {
        public listboxForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设定诊断信息
        /// </summary>
        /// <param name="pym"></param>
        public bool  setListData(string strWhere,string code)
        {
            //按照拼音码取得疾病诊断
            listboxFormBll listbox = new listboxFormBll();
            DataTable dt = listbox.GetMoHuList(string.Format ("and pym like '%{0}%'", strWhere),code);
            if (dt!=null && dt.Rows.Count > 0)
            {
                listBox_com.DataSource = dt;
                listBox_com.DisplayMember = "Name";
                listBox_com.ValueMember = "Code";
                return true;
            }
            else
            {
                this.Close();
                return false;
            }
        }

        /// <summary>
        /// 将结果设定到父页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_jbzd_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //回车选中
                if (e.KeyCode == Keys.Enter)
                {
                    sysCommonForm owerForm = (sysCommonForm)this.Owner;
                    owerForm.setText(listBox_com.SelectedValue.ToString() + "|" + listBox_com.Text);
                    this.Close();
                }

                if (e.KeyCode == Keys.Escape || e.KeyCode ==Keys .Space)
                {
                    this.Close();
                }
               
            }
            catch 
            {
                this.Close();
            }
        }

    }
}
