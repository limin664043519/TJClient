using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FBYClient;

namespace TJClient
{
    public partial class DropDownGrid : Form
    {
        public DropDownGrid()
        {
            InitializeComponent();
        }
        public string yplb = "xy";
        public static int RowIndex = 0;
        public static int CellIndex = 0;
        public static string Code = "";
        public static string Name1 = "";
        public static Form_daxq bxDemo = new Form_daxq();
        public DataTable dt = null;
        public static string code_old = "";

        private void DdGrid_Load(object sender, EventArgs e)
        {
            //this.textBox_PYM.Focus();
            cbo_update();
            dataGridView1.Focus();
            //listBox_list.DataSource = null;
        }

        private void textBox_PYM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //检索数据
                if (cbo_update() == true)
                {
                    dataGridView1.Focus();
                }
                else
                {
                    textBox_PYM.Focus();
                    textBox_PYM.SelectAll();
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                textBox_PYM.Focus();
                textBox_PYM.SelectAll();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                bool result = bxDemo.setValue(null,code_old);
                //if (result == true)
                //{
                this.Close();
                //}
            }
        }
        /// <summary>
        /// 检索数据
        /// </summary>
        /// <returns></returns>
        private bool cbo_update()
        {
            if (textBox_PYM.Text == "")
                return false;
            try
            {
                TJClient.sys.Bll.Form_daxqBll form_daxqbll = new TJClient.sys.Bll.Form_daxqBll();
                //按照拼音码取得疾病诊断
                DataTable dt = form_daxqbll.GetMoHuList(" and D_XM like '%" + textBox_PYM.Text + "%'", "sql083");
                
                //string txt = cboStyle.Text;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = dt;
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 设定信息
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="cellIndex"></param>
        /// <param name="pym"></param>
        public void setInfo( string pym, Form_daxq demo)
        {
            code_old = pym;
            bxDemo = demo;
            textBox_PYM.Text = pym;

            //textBox_PYM.SelectAll();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( e.RowIndex >= 0)
            {
                //返回选中的结果
                DataGridViewRow dr = dataGridView1.Rows[e.RowIndex];
                string name = dr.Cells["姓名"].Value.ToString();
                DataRow dataRow = (dr.DataBoundItem as DataRowView).Row;
                bool result = bxDemo.setValue(dataRow,  name);
                if (result == true)
                {
                    this.Close();
                }
            }
        }

        private void ddgrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {


                //返回选中的结果
                string code = "";
                string name = "";

                if (dataGridView1.Rows.Count > 0)
                {
                    //返回选中的结果
                    DataGridViewRow dr = dataGridView1.SelectedRows[0];
                    name = dr.Cells["姓名"].Value.ToString();
                    DataRow dataRow = (dr.DataBoundItem as DataRowView).Row;
                    bool result = bxDemo.setValue(dataRow, name);
                    if (result == true)
                    {
                        this.Close();
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                bool result = bxDemo.setValue(null, code_old);
                if (result == true)
                {
                    this.Close();
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                textBox_PYM.Focus();
                textBox_PYM.SelectAll();
            }
        }
    }
}
