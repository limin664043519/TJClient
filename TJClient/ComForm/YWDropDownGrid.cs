using FBYClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TJClient.ComForm
{
    public partial class YWDropDownGrid : Form
    {
        private DataTable dtResult_yw;

        public string resultStr;

        public YWDropDownGrid()
        {
            InitializeComponent();
        }

        private void YWDropDownGrid_Load(object sender, EventArgs e)
        {
            //获取药物字典
            DBAccess dBAccess = new DBAccess();
            string sql = @"SELECT YWMC, (YWMC + '|' + YWPYM) AS YWPYM FROM SYS_YW_DICS ORDER BY YWMC";
            dtResult_yw = dBAccess.ExecuteQueryBySql(sql);
            if (dtResult_yw.Rows.Count > 0)
            {
                dgv_yw.DataSource = dtResult_yw;
            }            
        }

        private void dgv_yw_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //DataGridView dgv_tem = ((DataGridView)(this.Controls.Find("dataGridView_T_TJ_YYQKB", true)[0]));
            //dgv_tem.SelectedCells[0].Value = dgv_yw.Rows[e.RowIndex].Cells["YWMC"].Value.ToString();

            resultStr = dgv_yw.Rows[e.RowIndex].Cells["YWMC"].Value.ToString();
            this.DialogResult = DialogResult.OK;
        }

        private void txt_input_TextChanged(object sender, EventArgs e)
        {
            DataRow[] drs = dtResult_yw.Select("YWPYM like '%" + txt_input.Text + "%'");
            if (drs.Length > 0)
            {
                dgv_yw.DataSource = drs.CopyToDataTable();
            }
            else
            {
                dgv_yw.DataSource = null;
            }
        }

        private void YWDropDownGrid_Shown(object sender, EventArgs e)
        {
            txt_input.Focus();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            YWAdd ywadd = new YWAdd();
            ywadd.ShowDialog();
            YWDropDownGrid_Load(null, null);
        }
    }
}
