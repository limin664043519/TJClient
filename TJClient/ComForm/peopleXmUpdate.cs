using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Common;
using TJClient.sys.Bll;
using TJClient.sys;
using FBYClient;

namespace TJClient.jktj
{
    public partial class peopleXmUpdate : Form
    {
        public peopleXmUpdate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 健康档案号
        /// </summary>
        public string jkdah = "";

        /// <summary>
        /// 身份证号
        /// </summary>
        public string sfzh = "";

        /// <summary>
        /// 姓名
        /// </summary>
        public string xm_da = "";

        /// <summary>
        /// 姓名
        /// </summary>
        public string xm_sfz = "";

        /// <summary>
        /// 联系电话
        /// </summary>
        public string lxdh = "";

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string lxrdh = "";

        /// <summary>
        /// 性别
        /// </summary>
        public string xb_sfz = "";

        /// <summary>
        /// 出生日期
        /// </summary>
        public string csrq_sfz = "";

        public int rowindex = 0;
        /// <summary>
        /// 前页面传过来的档案信息（身份证号相同）
        /// </summary>
        public DataTable dt_info = null;

        public string id = "";


        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_PeopleAdd_Load(object sender, EventArgs e)
        {
            comboBox_da_xb.DataSource = getSjzdList("xb_xingbie");
            comboBox_da_xb.DisplayMember = "ZDMC";
            comboBox_da_xb.ValueMember = "ZDBM";

            comboBox_sfz_xb.DataSource = getSjzdList("xb_xingbie");
            comboBox_sfz_xb.DisplayMember = "ZDMC";
            comboBox_sfz_xb.ValueMember = "ZDBM";

            //健康档案号
            textBox_jkdah.Text = this.jkdah ;

            //身份证号
            textBox_sfzh.Text = this.sfzh;

            //身份证的姓名
            textBox_xm_sfz.Text = xm_sfz;
            if (xb_sfz != null)
            {
                comboBox_sfz_xb.SelectedValue = xb_sfz;
            }

            if (csrq_sfz != null)
            {
                comboBox_sfz_rq.Text = csrq_sfz;
            }

            //
            if(dt_info!=null){
                dataGridView_list.DataSource = dt_info;
                //dr = (DataRowView)dataGridView_list.Rows[0].DataBoundItem;

                rowindex = 0;
                id = dt_info.Rows[0]["id"].ToString();
                string str_xm = dt_info.Rows[0]["D_XM"].ToString();
                string str_lxdh = dt_info.Rows[0]["D_LXDH"].ToString();
                string str_lxrdh = dt_info.Rows[0]["D_LXrDH"].ToString();
                string str_csrq = dt_info.Rows[0]["D_CSRQ"].ToString();
                string str_xb = dt_info.Rows[0]["d_xb"].ToString();
                string str_jkdah = dt_info.Rows[0]["D_GRDABH"].ToString();

                //档案中的姓名
                textBox_xm_da.Text = str_xm;

                //联系电话
                textBox_lxdh.Text = str_lxdh;
                textBox_lxrdh.Text = str_lxrdh;

                //联系电话
                textBox_new_lxdh.Text = str_lxdh; 
                textBox_new_lxrdh.Text = str_lxrdh;

                //性别
                if (str_xb != null)
                {
                    comboBox_da_xb.SelectedValue = str_xb;
                }

                //出生日期
                if (str_csrq != null)
                {
                    comboBox_da_rq.Text = str_csrq;
                }

                //健康档案号
                textBox_jkdah.Text = str_jkdah;

            }
        }

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
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string message="";
           bool result= false;
           
           result = peopleUpdate(out message);
         
           if (result == true)
           {
               dt_info.Rows[rowindex]["D_SFZH"] = textBox_sfzh.Text;
               dt_info.Rows[rowindex]["D_XM"] = textBox_xm_sfz.Text;
               dt_info.Rows[rowindex]["D_LXDH"] = textBox_new_lxdh.Text;
               dt_info.Rows[rowindex]["D_CSRQ"] = comboBox_sfz_rq.Text;
               dt_info.Rows[rowindex]["d_xb"] = comboBox_sfz_xb.SelectedValue.ToString();
               dt_info.Rows[rowindex]["XBName"] = comboBox_sfz_xb.Text.ToString();

               MessageBox.Show("保存成功！" );
               //sysCommonForm syscommonform = (sysCommonForm)this.Owner;
               //syscommonform.setParentFormDo("");
               //this.Close();
           }
           else
           {
               MessageBox.Show("保存失败！" + message);
           }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            sysCommonForm syscommonform = (sysCommonForm)this.Owner;
            syscommonform.setParentFormDo("");
            this.Close();
        }


        #region 创建建议档案处理

        public bool peopleUpdate(out string msg)
        {
            msg = "";
            try
            {
                jktjBll jktjbll = new jktjBll();

                DataTable dt_xm_Update = new DataTable();
                dt_xm_Update.Rows.Add();
                //个人档案编号
                dt_xm_Update.Columns.Add("D_GRDABH");
                dt_xm_Update.Rows[0]["D_GRDABH"] = textBox_jkdah.Text;

                //姓名
                dt_xm_Update.Columns.Add("D_XM");
                dt_xm_Update.Rows[0]["D_XM"] = textBox_xm_sfz.Text;

                //身份证号
                dt_xm_Update.Columns.Add("D_ZJHQT");
                dt_xm_Update.Rows[0]["D_ZJHQT"] = textBox_sfzh.Text;

                //id
                dt_xm_Update.Columns.Add("id");
                dt_xm_Update.Rows[0]["id"] = id;

                //出生日期
                dt_xm_Update.Columns.Add("D_CSRQ");
                dt_xm_Update.Rows[0]["D_CSRQ"] = comboBox_sfz_rq.Text.ToString();

                //性别
                dt_xm_Update.Columns.Add("D_XB");
                dt_xm_Update.Rows[0]["D_XB"] = comboBox_sfz_xb.SelectedValue.ToString();

                //联系人电话
                dt_xm_Update.Columns.Add("D_LXrDH");
                dt_xm_Update.Rows[0]["D_LXrDH"] = textBox_new_lxrdh.Text;

                //联系电话
                dt_xm_Update.Columns.Add("D_LXDH");

                dt_xm_Update.AcceptChanges();

                dt_xm_Update.Rows[0]["D_LXDH"] = textBox_new_lxdh.Text;

                //人口学资料表表
                bool result = jktjbll.Upd(dt_xm_Update, "sql_T_DA_JKDA_RKXZL_update_xm");
                //体检人员信息表
                bool result_ = jktjbll.Upd(dt_xm_Update, "sql_update_TJRYXX1");
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        public bool peopleDelete(out string msg)
        {
            msg = "";
            try
            {
                jktjBll jktjbll = new jktjBll();

                DataTable dt_xm_Update = new DataTable();
                dt_xm_Update.Rows.Add();

                //id
                dt_xm_Update.Columns.Add("id");
                dt_xm_Update.Rows[0]["id"] = id;

                //联系电话
                dt_xm_Update.Columns.Add("deleteFlag");

                dt_xm_Update.AcceptChanges();

                dt_xm_Update.Rows[0]["deleteFlag"] = "1";

                bool result = jktjbll.Upd(dt_xm_Update, "sql_T_DA_JKDA_RKXZL_delete");
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 姓名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_xm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_xm_sfz.Text = textBox_xm_da.Text;
        }

        /// <summary>
        /// 电话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_dh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_new_lxdh.Text = textBox_lxdh.Text;
        }

        /// <summary>
        /// 日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_rq_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            comboBox_sfz_rq.Text = comboBox_da_rq.Text;
        }

        /// <summary>
        /// 性别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_xb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            comboBox_sfz_xb.SelectedValue = comboBox_da_xb.SelectedValue;
        }

        private void dataGridView_list_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                rowindex = e.RowIndex;
                DataRowView dr = (DataRowView)dataGridView_list.Rows[rowindex].DataBoundItem;

                //id
                id = dr["id"].ToString();
                string str_xm = dr["D_XM"].ToString();
                string str_lxdh = dr["D_LXDH"].ToString();
                string str_csrq = dr["D_CSRQ"].ToString();
                string str_xb = dr["d_xb"].ToString();
                string str_jkdah = dr["D_GRDABH"].ToString();
                //档案中的姓名
                textBox_xm_da.Text = str_xm;

                //联系电话
                textBox_lxdh.Text = str_lxdh;

                //性别
                comboBox_da_xb.SelectedValue = str_xb;

                //出生日期
                comboBox_da_rq.Text = str_csrq;
                //健康档案号
                textBox_jkdah.Text = str_jkdah;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_del_Click(object sender, EventArgs e)
        {
            string message = "";
            bool result = false;

            result = peopleDelete(out message);

            if (result == true)
            {
               
                dt_info.Rows.RemoveAt(rowindex);
                dt_info.AcceptChanges();
                MessageBox.Show("删除成功！");
                //sysCommonForm syscommonform = (sysCommonForm)this.Owner;
                //syscommonform.setParentFormDo("");
                //this.Close();
            }
            else
            {
                MessageBox.Show("删除失败！" + message);
            }
        }

        /// <summary>
        /// 联系人电话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_lxrdh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_new_lxrdh.Text = textBox_lxrdh.Text;
        }
    }
}
