using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YCYL.Client;
using TJClient.Common;
using FBYClient;
namespace YCYL.Client.AllForms
{
    public partial class printSet : Form
    {
       // private string path = "..\\..\\Config\\UserConfig.xml"; 
        public printSet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printSet_load(object sender, EventArgs e)
        {
            //从字典表中取得打印机类型列表
            DBAccess dBAccess = new DBAccess();
            DataTable dtPrintList = TJClient.Common.Common.getsjzd("printFileType", "sql_select_sjzd");
            if (dtPrintList != null && dtPrintList.Rows.Count > 0)
            {
                comboBox_printType.DataSource = dtPrintList;
                comboBox_printType.DisplayMember = "ZDMC";
                comboBox_printType.ValueMember = "ZDBM";
            }

           

            //已设定的打印机
            XmlRW xmlWrite = new XmlRW();
            string printName = xmlWrite.GetValueFormXML("UserConfig.xml", comboBox_printType.SelectedValue.ToString());
            string print_view = xmlWrite.GetValueFormXML("UserConfig.xml", comboBox_printType.SelectedValue.ToString() + "_view");
            if (print_view.ToLower().Equals("true"))
            {
                checkBox_view.Checked = true;
            }
            else
            {
                checkBox_view.Checked = false;
            }

            //获得系统中的打印机列表
            List<String> list = LocalPrinter.GetLocalPrinters(); 
            foreach (String s in list)
            {
                //将打印机名称添加到下拉框中
                comboBox_sysPrint.Items.Add(s);
                if (s.Equals(printName))
                {
                    comboBox_sysPrint.Text = printName;
                }
            }

            comboBox_printType.Focus();

        }

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox_printType.SelectedValue == null || comboBox_printType.SelectedValue.ToString().Length == 0)
            {
                MessageBox.Show("请选择打印类型！");
                return;
            }

            try
            {
                //XmlRW xmlWrite = new XmlRW();
                XmlRW.WriteXML("UserConfig.xml", UserInfo.Yybm, comboBox_sysPrint.Text, comboBox_printType.SelectedValue.ToString());
                XmlRW.WriteXML("UserConfig.xml", UserInfo.Yybm, checkBox_view.Checked.ToString(), comboBox_printType.SelectedValue.ToString() + "_view");
                MessageBox.Show("设定成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("设定失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_printType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                XmlRW xmlWrite = new XmlRW();
                comboBox_sysPrint.Text = xmlWrite.GetValueFormXML("UserConfig.xml", comboBox_printType.SelectedValue.ToString());
                string print_view = xmlWrite.GetValueFormXML("UserConfig.xml", comboBox_printType.SelectedValue.ToString() + "_view");
                if (print_view.ToLower().Equals("true"))
                {
                    checkBox_view.Checked = true;
                }
                else
                {
                    checkBox_view.Checked = false;
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Enter转换为tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Tab_KeyDown(object sender, KeyEventArgs e)
        {
            //Enter转换为tab
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
        }
    }
}
