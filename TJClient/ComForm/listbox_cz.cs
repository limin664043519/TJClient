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
    public partial class listbox_cz : Form
    {
        public listbox_cz()
        {
            InitializeComponent();
        }

        public string codepoara = "";

        /// <summary>
        /// 设定
        /// </summary>
        /// <param name="pym"></param>
        public bool setListData(string str, string code)
        {
            codepoara = code;
            //按照拼音码取得疾病诊断
            listboxFormBll listbox = new listboxFormBll();
            DataTable dt = listbox.GetMoHuList(string.Format("and YLJGBM = '{0}'", UserInfo.Yybm), code);
            if (dt != null && dt.Rows.Count > 0)
            {
                int length = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if(length<dt.Rows[i]["czmc"].ToString ().Length){
                        length = dt.Rows[i]["czmc"].ToString().Length;
                    }
                }
                checkedListBox_cz.ColumnWidth = length * 18 < 100 ? 100 : length * 18;

                checkedListBox_cz.DataSource = dt;
                checkedListBox_cz.DisplayMember = "czmc";
                checkedListBox_cz.ValueMember = "czbm";

                //设定选中项目
                setItemSelect(str);
                return true;
            }
            else
            {
                this.Close();
                return false;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click(object sender, EventArgs e)
        {

            //按照拼音码取得疾病诊断
            listboxFormBll listbox = new listboxFormBll();

            string strwhere = "";

            if (textBox_mc.Text.Trim().Length > 0)
            {
                strwhere = string.Format(" and T_BS_CUNZHUANG.B_NAME like '%{0}%'", textBox_mc.Text.Trim());
            }


            DataTable dt = listbox.GetMoHuList(string.Format("and YLJGBM = '{0}' {1}", UserInfo.Yybm, strwhere), codepoara);
            if (dt != null && dt.Rows.Count > 0)
            {
                checkedListBox_cz.DataSource = null;
                int length = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (length < dt.Rows[i]["czmc"].ToString().Length)
                    {
                        length = dt.Rows[i]["czmc"].ToString().Length;
                    }
                }
                checkedListBox_cz.ColumnWidth = length * 18 < 100 ? 100 : length * 18;

                checkedListBox_cz.DataSource = dt;
                checkedListBox_cz.DisplayMember = "czmc";
                checkedListBox_cz.ValueMember = "czbm";
            }
            else
            {
                MessageBox.Show("没有查询到数据！");
            }


        }

        /// <summary>
        /// 全选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_allSelect_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < checkedListBox_cz.Items.Count; j++)
                checkedListBox_cz.SetItemChecked(j, true);

        }
        /// <summary>
        /// 全不选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_notSelect_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < checkedListBox_cz.Items.Count; j++)
                checkedListBox_cz.SetItemChecked(j, false);
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_true_Click(object sender, EventArgs e)
        {
            sysCommonForm owerForm = (sysCommonForm)this.Owner;
            owerForm.setText(getSelectList());
            this.Close();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_return_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 组合选中的村庄编码
        /// </summary>
        /// <returns></returns>
        public string getSelectList()
        {
            int listCount = 0;
            DataTable dt = (DataTable)checkedListBox_cz.DataSource;
            string strCollected = string.Empty;
            for (int i = 0; i < checkedListBox_cz.Items.Count; i++)
            {
                if (checkedListBox_cz.GetItemChecked(i)==true)
                {
                    listCount++;
                    if (strCollected == string.Empty)
                    {
                        //strCollected = checkedListBox_cz.GetItemText(checkedListBox_cz.Items[i]);
                        strCollected = dt.Rows[i]["czbm"].ToString();
                    }
                    else
                    {
                        //strCollected = strCollected + "|" + checkedListBox_cz.GetItemText(checkedListBox_cz.Items[i]);
                        strCollected = strCollected + "," + dt.Rows[i]["czbm"].ToString();
                    }
                }
            }
            return strCollected + "|" + listCount.ToString() ;
        }

        /// <summary>
        /// 设定选中项目
        /// </summary>
        /// <param name="selectvalue"></param>
        public void setItemSelect(string selectvalue)
        {

            DataTable dt = (DataTable)checkedListBox_cz.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            string[] selectList = selectvalue.Split(new char[] { ',' });
            for (int i = 0; i < selectList.Length; i++)
            {
                for (int j = 0; j < dt.Rows .Count; j++)
                    if (selectList[i].Equals(dt.Rows[j]["czbm"].ToString()))
                    {
                        checkedListBox_cz.SetItemChecked(j, true);
                        break;
                    }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

       

    }
}
