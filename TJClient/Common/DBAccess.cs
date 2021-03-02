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
using System.Logger;
using System.Data.Odbc;
using System.Data.Common;

namespace FBYClient
{
    class DBAccess : dbCommon
    {
        //private static string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Source};Jet OLEDB:Database Password={Password};";
        private static string connString = "";
        public static OleDbConnection conn = null;
        public static string Source = "";
        public static string Password = "";
        public SimpleLogger logger = SimpleLogger.GetInstance();

        public static string DBtype = "";

        ////db2
        //DbConnection con = null;

        public DBAccess()
        {
            DBtype = ConfigurationSettings.AppSettings["dbType"];//DB2数据库
            if (DBtype.Equals("access") == true)
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
            else if (DBtype.ToLower().Equals("oracle") == true)
            {
                connString = ConfigurationSettings.AppSettings["oracleConnection"];//oracle数据库
            }
            else
            {
                throw new Exception("数据库类型设定错误");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DBAccess(string dbtype)
        {
            DBtype = dbtype;//数据库
            if (DBtype.Equals("access") == true)
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
            else if (DBtype.ToLower().Equals("oracle") == true)
            {
                connString = ConfigurationSettings.AppSettings["oracleConnection"];//oracle数据库
            }
            else
            {
                throw new Exception("数据库类型设定错误");
            }
        }
        #region access
        public override void SetDataSource(string source, string password)
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
                logger.Error("DBAccess:" + "SetDataSource:" + ex.Message);
                MessageBox.Show("access数据库连接失败！" + ex.Message);
            }
        }

        public override void Open()
        {
            if (conn == null)
            {
                conn = new OleDbConnection(connString);
            }
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }

        public override void CloseConn()
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
        public override DataTable ExecuteQueryBySql(string sql)
        {
            DataTable dt = new DataTable();
            if (DBtype.Equals("access") == true)
            {
                #region access
                //access数据库处理
                OleDbDataAdapter da = null;
                try
                {
                    Open();
                    da = new OleDbDataAdapter(sql, conn);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    logger.Error("DBAccess:" + "ExecuteQueryBySql:" + ex.Message + string.Format("[{0}]", sql));
                    throw new Exception("ExecuteQueryBySql 异常：" + sql + ":" + ex.Message);
                }
                finally
                {
                    if (da != null)
                    {
                        da.Dispose();
                    }
                    CloseConn();
                }

                #endregion
            }
            return dt;
        }

        /// <summary>
        /// 插入记录，更新记录，删除记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override int ExecuteNonQueryBySql(string sql)
        {
            int rowcount = 0;
            if (DBtype.Equals("access") == true)
            {
                #region access
                OleDbCommand cmd = null;
                try
                {
                    Open();
                    // 插入一条记录
                    cmd = new OleDbCommand(sql, conn);
                    rowcount = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logger.Error("DBAccess:" + "ExecuteNonQueryBySql:" + ex.Message + string.Format("[{0}]", sql));
                    throw ex; //new Exception(ex.Message);
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    CloseConn();
                }

                #endregion
            }
            return rowcount;
        }

        /// <summary>
        /// 插入记录，更新记录，删除记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override int ExecuteNonQueryBySqlList(ArrayList sql)
        {
            int rowcount = 0;
            if (DBtype.Equals("access") == true)
            {
                #region access
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
                    logger.Error("DBAccess:" + "ExecuteNonQueryBySqlList:" + ex.Message + string.Format("[{0}]", cmd.CommandText));
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
                #endregion
            }
            return rowcount;
        }

        /// <summary>
        /// 插入记录，更新记录，删除记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override int ExecuteNonQueryBySql(ArrayList sql)
        {

            int rowcount = 0;

            //rowcount += ExecuteNonQueryBySqlList(sql);
            ArrayList sqllist_child = new ArrayList();
            string sql_tem = "";
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
                logger.Error("DBAccess:" + "ExecuteNonQueryBySql:" + ex.Message);
                throw new Exception(ex.Message);
            }

            return rowcount;
        }

        #endregion

        
    }
}
