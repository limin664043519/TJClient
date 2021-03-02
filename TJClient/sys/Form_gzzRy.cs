using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.sys.Bll;
using TJClient.Common;

namespace TJClient.sys
{
    public partial class Form_gzzRy : sysCommonForm
    {
        private static string strWhere = "";
        public Form_gzzRy()
        {
            InitializeComponent();
        }

        private void para_load(object sender, EventArgs e)
        {
            mc.Focus();
        }
        //查询
        private void button3_Click(object sender, EventArgs e)
        {
            dataSelect();
        }

        /// <summary>
        /// 检索
        /// </summary>
        private void dataSelect()
        {
            Form_gzzRyBll xmfl = new Form_gzzRyBll();
            string strWhere = "";
            if (UserInfo.Yybm != null && UserInfo.Yybm.Trim().Length > 0)
            {
                strWhere = strWhere + string.Format(" and  T_JK_GZZ_CZY.YLJGBM ='{0}' ", UserInfo.Yybm);
            }

            if (mc.Text.Trim().Length > 0)
            {
                strWhere = strWhere + string.Format("  and T_JK_GZZ.GZZMC like '%{0}%' ", mc.Text.Trim());
            }
            DataTable dt = xmfl.GetMoHuList(strWhere);
            showDataGrid(dt);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_del_Click(object sender, EventArgs e)
        {
          int rowIndex = dataGridView1.CurrentRow .Index;
          DialogResult result;
          result = MessageBox.Show("确定要删除数据吗？ \r\n确定：删除数据  \r\n取消：取消本次操作", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
          if (result == DialogResult.Cancel )
          {
              return;
          }
         
          try
          {
              DataTable dtupdate = (DataTable)dataGridView1.DataSource;
              Form_gzzRyBll SjzdBll = new Form_gzzRyBll();

              if (dtupdate != null && dtupdate.Rows.Count > 0 && dtupdate.Rows.Count > rowIndex)
              {
                  SjzdBll.Del(dtupdate, rowIndex);
              }
              dataSelect();
              MessageBox.Show("删除成功！");
          }
          catch (Exception ex)
          {
              MessageBox.Show("删除失败！" + ex.Message);
          }
        }

        //保存
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtupdate = (DataTable)dataGridView1.DataSource;
                Form_gzzRyBll SjzdBll = new Form_gzzRyBll();

                if (dtupdate != null && dtupdate.Rows.Count > 0)
                {
                    SjzdBll.Add(dtupdate);
                    SjzdBll.Upd(dtupdate);
                }
                dataSelect();
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！" + ex.Message);
            }
        }
       
       
        /// <summary>
        /// 显示列表
        /// </summary>
        private void showDataGrid(DataTable dt)
        {
            dataGridView1.DataSource = dt;
            if (!(dt != null && dt.Rows.Count > 0))
            {
                MessageBox.Show("没有查询到数据！");
            }
        }

        

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clear_Click(object sender, EventArgs e)
        {
            pageClear();
            textBox_pym.Focus();
        }

        /// <summary>
        /// 清空页面信息
        /// </summary>
        private void pageClear()
        {
            bm.Text = "";
            mc.Text = "";
            textBox_pym.Text = "";
        }

        /// <summary>
        /// Enter转换为tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Tab_KeyDown(object sender, KeyEventArgs e)
        {
            //Enter转换为tab
            if (sender.GetType().ToString().Equals("System.Windows.Forms.TextBox"))
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
            }
        }

        /// <summary>
        /// KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_pym_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox_pym.Text.Trim().Length > 0)
                {
                    //MousePosition.X
                    Point textLocation = PointToScreen(textBox_pym.Location);
                    textLocation.X = textLocation.X + 1;
                    textLocation.Y = textLocation.Y + textBox_pym.Height;
                    listboxForm form = new listboxForm();
                    form.Location = textLocation;
                    form.Owner = this;
                    form.Show();
                    if (form.setListData(textBox_pym.Text.Trim(), "sql002") == false)
                    {
                        textBox_pym.SelectAll();
                        textBox_pym.Focus();
                        MessageBox.Show("没有对应的信息！");
                    }
                }
                else
                {
                    Enter_Tab_KeyDown(sender, e);
                }
            }
            else
            {
            }

        }

        /// <summary>
        /// 重写父类的方法，设定当前页面的值 strText： 编码|名称
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public override bool setText(string strText)
        {
            if (strText.Length > 0)
            {
                string[] textList = strText.Split(new char[] { '|' });

                bm.Text = textList[0];
                mc.Text = textList[1];
                dataSelect();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
