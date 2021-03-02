using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TJClient
{
    public partial class Form_datagridview : Form
    {
        public Form_datagridview()
        {
            InitializeComponent();
        }

        //protected override bool ProcessCmdKey(ref　Message msg, Keys keyData)
        //{
        //    //DataGridView dv = (DataGridView)FromHandle(msg.HWnd);
        //    //if (keyData == Keys.Enter)
        //    //{
        //    //    if (dv.IsCurrentCellInEditMode)
        //    //    {
        //    //        if (dv.CurrentCell.RowIndex == dv.Rows.Count - 1)
        //    //        {
        //    //            SendKeys.Send("{Tab}");
        //    //        }
        //    //        else
        //    //        {
        //    //            SendKeys.Send("{Up}");
        //    //            SendKeys.Send("{Tab}");
        //    //        }
        //    //    }
        //    //}
        //    //return base.ProcessCmdKey(ref　msg, keyData);
        //}  
        //private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //if ((e.KeyCode == Keys.Tab))
        //    //{
        //    //   // SendKeys.Send("{Tab}");
        //    //    this.dataGridView1.BeginEdit(true);


        //    //    //this.dataGridView1.CurrentCell = this.dataGridView1.Rows[2].Cells[1];
        //    //    this.dataGridView1.BeginEdit(true);

        //    //    //if ((this.dataGridView1.FirstDisplayedScrollingColumnHiddenWidth > 0))
        //    //    // //&& !xGrid1.Columns[this.dataGridView1.SelectedCells[0].ColumnIndex].Frozen)
        //    //    //{
        //    //    //    //this.dataGridView1.FirstDisplayedScrollingColumnIndex =
        //    //    //    //      this.dataGridView1.SelectedCells[0].ColumnIndex;

        //    //    //    this.dataGridView1.BeginEdit(true);
        //    //    //}
        //    //}  
        //}

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            try
            {
               





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
            }

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.dataGridView1.BeginEdit(true);
                DataGridViewComboBoxColumn comboBoxColumn = dataGridView1.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
                if (comboBoxColumn != null)
                {
                    dataGridView1.BeginEdit(true);
                    DataGridViewComboBoxEditingControl comboBoxEditingControl = dataGridView1.EditingControl as DataGridViewComboBoxEditingControl;
                    if (comboBoxEditingControl != null)
                    {
                        comboBoxEditingControl.DroppedDown = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dataGridView1[e.ColumnIndex, e.RowIndex] != null && !dataGridView1[e.ColumnIndex, e.RowIndex].ReadOnly)
            {
                DataGridViewComboBoxColumn comboBoxColumn = dataGridView1.Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
                if (comboBoxColumn != null)
                {
                    dataGridView1.BeginEdit(true);
                    DataGridViewComboBoxEditingControl comboBoxEditingControl = dataGridView1.EditingControl as DataGridViewComboBoxEditingControl;
                    if (comboBoxEditingControl != null)
                    {
                        comboBoxEditingControl.DroppedDown = true;
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView DataGridView_tem = (DataGridView)sender;

                if (DataGridView_tem.Columns[e.ColumnIndex].Name == "delColumn")
                {
                    int d = e.RowIndex;
                    Object obj_dt = DataGridView_tem.DataSource;
                    DataTable dt = null;
                    if (obj_dt != null)
                    {
                        dt = obj_dt as DataTable;

                        if (d >= 0 && d < dt.Rows.Count)
                        {

                            dt.Rows.RemoveAt(d);
                            dt.AcceptChanges();

                        }
                    }
                }
            }
           
        }

        private void Form_datagridview_Load(object sender, EventArgs e)
        {
            DataTable dt=new DataTable ();
            dt.Columns .Add ("a");
            dt.Columns .Add ("b");
            dt.Columns .Add ("c");
            dataGridView2.DataSource = dt;
        }
    }
}
