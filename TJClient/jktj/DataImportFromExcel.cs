using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.Configuration;
using TJClient;
using TJClient.sys;
using TJClient.Common;
using TJClient.sys.Bll;
namespace FBYClient
{
    public partial class DataImportFromExcel : sysCommonForm
    {
        /// <summary>
        /// 存放原始的结果集
        /// </summary>
        private static DataTable dt_tem = new DataTable();

        private DataTable dt_fileType = null;

        ///// <summary>
        ///// 前页面传过来的参数
        ///// </summary>
        //DataTable dt_para_sys = null;

        //public bool setPara(DataTable dtpara)
        //{
        //    try
        //    {
        //        dt_para_sys = dtpara;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        public DataImportFromExcel()
        {
            InitializeComponent();
        }

        #region 控件事件
        private void Form_downLoad_Load(object sender, EventArgs e)
        {
            ////progressBar_xz.Visible = false;
            ////取得前以页面传递的数据

            ////DataTable dtPara = (DataTable)((jktj)this.Owner).Tag;
            ////用户id
            //userId = dt_para_sys.Rows[0]["userId"].ToString();

            ////工作组
            //yhfz = dt_para_sys.Rows[0]["gzz"].ToString();

            ////医疗机构
            //yljg = dt_para_sys.Rows[0]["yljg"].ToString();

            DBAccess dBAccess = new DBAccess();

            //取得导入数据的设定信息
            Form_yljgBll Form_yljg = new Form_yljgBll();
            DataTable importDataSet = Form_yljg.GetMoHuList(string.Format ( " and YLJGBM='{0}' ",UserInfo.Yybm), "sql_importDataSet_select");

            //绑定导入数据的设定信息
            comboBox_importDataSet.DataSource = importDataSet;
            comboBox_importDataSet.DisplayMember = "title";
            comboBox_importDataSet.ValueMember = "IID";
            dt_fileType = importDataSet.Copy();

        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_import_Click(object sender, EventArgs e)
        {
            try
            {
                //对话框初始目录
                openFileDialog1.InitialDirectory = ConfigurationSettings.AppSettings["filePath"].ToString();

                //文件类型
                //openFileDialog1.Filter = "excel文件(*.xls)|*.xlsx";

                //弹出文件选择框
                openFileDialog1.ShowDialog();
                //打开的文件的全限定名
                String strOpenFileName = openFileDialog1.FileName;
                //导入数据
                bool importResult = importFileTo(strOpenFileName);
                if (importResult)
                {
                    MessageBox.Show("导入完成！");
                }
                else
                {
                    MessageBox.Show("导入失败！" );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 数据导入处理
        /// </summary>
        /// <param name="filePathAll">文件路径</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool importFileTo(string filePathAll)
        {
            try
            {
                ArrayList fileList = new ArrayList();
                fileList.Add(filePathAll);
                ImportDataFromExcel importdatafromexcel = new ImportDataFromExcel();
                importdatafromexcel.localFileAddressList = fileList;
                importdatafromexcel.dtRow = dt_fileType.Select(string.Format("iid={0}", comboBox_importDataSet.SelectedValue .ToString ()))[0];
                
                importdatafromexcel.Show();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("importFileTo 数据导入处理失败！"+ex.Message);
            }
        }

        /// <summary>
        /// 检索处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click(object sender, EventArgs e)
        {
            //string sqlWhere = " and  (D_GRDABH like '%{para}%' or D_XM like '%{para}%'  or D_SFZH like '%{para}%' or D_LXDH like '%{para}%'  or CREATEUSER like '%{para}%') ".Replace("{para}", textBox_Select.Text.Trim());
            string sqlWhere = string.Format(" and importDate >='{0}' and importDate <='{1}' ", dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"));
           DataRow dt_row= dt_fileType.Select(string.Format("iid={0}", comboBox_importDataSet.SelectedValue.ToString()))[0];
           string tableName = dt_row["dbTableName_from"].ToString();
           DBAccess dBAccess = new DBAccess();
          

            //Form_downLoadBll form_download = new Form_downLoadBll();
           DataTable dt = dBAccess.ExecuteQueryBySql(string.Format("select * from {0} where 1=1 {1}", tableName, sqlWhere)); // form_download.GetMoHuList(sqlWhere, "sql040");
            dataGridView_list.DataSource = dt;
        }
        #endregion

        /// <summary>
        /// Enter转换为tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Tab_KeyDown(object sender, KeyEventArgs e)
        {
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
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }

            }

            if (sender.GetType().ToString().Equals("System.Windows.Forms.ComboBox"))
            {
                if (e.KeyCode == Keys.Left)
                {
                    //shift+Tab
                    SendKeys.Send("+{Tab}");
                    e.Handled = false;
                }
                if (e.KeyCode == Keys.Right)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
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
                if (e.KeyCode == Keys.Down)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                e.Handled = false;
            }

        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            //czList = "";
            //this.Owner.Visible = true;
            this.Close();
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
                string[] selectList = strText.Split(new char[] { '|' });
                //czList = selectList[0];

                //link_all_cz.Text = string.Format("选择了[{0}]个村庄", selectList[1]);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 选择村庄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void link_all_cz_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listbox_cz form = new listbox_cz();
            form.Owner = this;
            form.Show();
            //if (form.setListData(czList, "sql038") == false)
            //{
            //    MessageBox.Show("没有对应的村庄信息！");
            //}
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_list_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dataGridView_list.Columns[e.ColumnIndex].Name == "详情")
                {
                    DataRowView dr = (DataRowView)dataGridView_list.Rows[e.RowIndex].DataBoundItem;
                    Form_daxq form = new Form_daxq();
                    form.Owner = this;
                    form.str_id = dr["id"].ToString();
                    form.ShowDialog();
                    //if (form.dataSelect(string.Format(" and id ={0}", dr["id"].ToString()), "sql041") == false)
                    //{
                    //    MessageBox.Show("没有取得详情信息！");
                    //}
                }
            }

        }
        /// <summary>
        /// 行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_list_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_list.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView_list.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView_list.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
