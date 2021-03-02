using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;

namespace FBYClient
{
    class dbCommon 
    {
        //private static string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Source};Jet OLEDB:Database Password={Password};";
        public static string connString = "";
        public static OleDbConnection conn = null;
        public static string Source = "";
        public static string Password = "";

        public  dbCommon()
        {
            connString = ConfigurationSettings.AppSettings["ACCESSConnection"];//access数据库
            Source = ConfigurationSettings.AppSettings["DataSource"];//access数据库
            Password = ConfigurationSettings.AppSettings["password"];//access数据库密码
            connString = connString.Replace("{Source}", Source).Replace("{Password}", Password);
            if (conn == null)
            {
                conn = new OleDbConnection(connString);
            }
            else
            {
                conn.Close();
                conn = new OleDbConnection(connString);
            }
        }

        public  dbCommon(string dbType)
        {
            connString = ConfigurationSettings.AppSettings["ACCESSConnection"];//access数据库
            Source = ConfigurationSettings.AppSettings["DataSource"];//access数据库
            Password = ConfigurationSettings.AppSettings["password"];//access数据库密码
            connString = connString.Replace("{Source}", Source).Replace("{Password}", Password);
            if (conn == null)
            {
                conn = new OleDbConnection(connString);
            }
            else
            {
                conn.Close();
                conn = new OleDbConnection(connString);
            }
        }

        public virtual void SetDataSource(string source, string password)
        {
            //Source = source;//access数据库
            //Password = password;//access数据库密码
            try
            {
                //connString = connString.Replace("{Source}", Source).Replace("{Password}", password);
                if (conn == null)
                {
                    conn = new OleDbConnection(connString);
                }
                else
                {
                    conn.Close();
                    conn = new OleDbConnection(connString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("access数据库连接失败！" + ex.Message);
            }
        }

        public virtual  void Open()
        {
            if (conn == null)
            {
                conn = new OleDbConnection(connString);
            }

            conn.Open();
        }

        public virtual void CloseConn()
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual DataTable ExecuteQueryBySql(string sql)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = null;
            try
            {
                Open();
                da = new OleDbDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                }
                CloseConn();
            }
            return dt;
        }

        /// <summary>
        /// 插入记录，更新记录，删除记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQueryBySql(string sql)
        {
            int  rowcount=0;
            OleDbCommand cmd = null;
            try
            {
                Open();
                // 插入一条记录
                cmd = new OleDbCommand(sql, conn);
               rowcount=  cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                CloseConn();
            }
            return rowcount;
        }

        /// <summary>
        /// 插入记录，更新记录，删除记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQueryBySqlList(ArrayList sql)
        {
            int rowcount = 0;
            OleDbTransaction tx = null;
            OleDbCommand cmd = null;
            try
            {
                Open();
                
                 cmd = new OleDbCommand();

                string sqlTimeOut = ConfigurationSettings.AppSettings["sqlTimeOut"].ToString();
                if (sqlTimeOut.Length > 0)
                {
                    cmd.CommandTimeout = int.Parse(sqlTimeOut);
                }

                tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                cmd.Connection = conn;
                for (int i = 0; i < sql.Count; i++)
                {
                    cmd.CommandText = sql[i].ToString();
                    rowcount = cmd.ExecuteNonQuery();
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new Exception(ex.Message);    
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                CloseConn();
            }
            return rowcount;
        }



        /// <summary>
        /// 插入记录，更新记录，删除记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQueryBySql(ArrayList sql)
        {
           
            int rowcount = 0;
            //rowcount += ExecuteNonQueryBySqlList(sql);
            ArrayList sqllist_child = new ArrayList();
            try
            {
                for (int i = 0; i < sql.Count; i++)
                {
                    if (i > 0 && i % 10000 == 0)
                    {
                        rowcount += ExecuteNonQueryBySqlList(sqllist_child);
                        sqllist_child.Clear();
                    }
                    else
                    {
                        sqllist_child.Add(sql[i]);
                    }
                }
                if (sqllist_child.Count > 0)
                {
                    rowcount += ExecuteNonQueryBySqlList(sqllist_child);
                    sqllist_child.Clear();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return rowcount;
        }

    }
}
