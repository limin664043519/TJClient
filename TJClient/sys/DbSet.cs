using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using System.Data.SqlClient;
using System.IO;
using TJClient.sys.Bll;
using System.Data.OleDb;
using FBYClient;
namespace Yibaoxiao.sysmain
{
    public partial class DbSet : Form
    {
        public DbSet()
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


        }

        /// <summary>
        /// 清空数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_DB_clear_Click(object sender, EventArgs e)
        {
             DialogResult result;
            result = MessageBox.Show("是否要删除数据库中的数据?  删除后数据不能再恢复！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }

            ArrayList deltablelist = new ArrayList();
            ArrayList delSql = new ArrayList();
            try
            {
                deltablelist = GetShemaTableName();
                for (int i = 0; i < deltablelist.Count; i++)
                {
                    delSql.Add(" delete from " + deltablelist[i]);
                }

                //删除数据
                DbSetBll dbsetbll = new DbSetBll();
                dbsetbll.DelByArrayList(delSql);
                MessageBox.Show("清理完成！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 返回Mdb数据库中所有表表名
        /// </summary>
        public ArrayList GetShemaTableName()
        {
            try
            {
                DBAccess dbaccess = new DBAccess();
                DBAccess.conn.Open();

                DataTable shemaTable = DBAccess.conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                int n = shemaTable.Rows.Count;

                ArrayList strTable = new ArrayList();

                int m = shemaTable.Columns.IndexOf("TABLE_NAME");
                for (int i = 0; i < n; i++)
                {
                    DataRow m_DataRow = shemaTable.Rows[i];
                    strTable.Add ( m_DataRow.ItemArray.GetValue(m).ToString());
                }
                return strTable;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("指定的限制集无效:\n" + ex.Message);
                return null;
            }
           
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_exec_Click(object sender, EventArgs e)
        {
            try
            {
                DBAccess dbaccess = new DBAccess();
                DBAccess.conn.Open();

                string sql = richTextBox_sql.Text ;
                dbaccess.ExecuteNonQueryBySql(sql);


                MessageBox.Show("执行完成！");
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("执行错误:\n" + ex.Message);
                
            }
        }

    }
}
