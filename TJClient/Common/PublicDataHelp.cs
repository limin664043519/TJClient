using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FBYClient
{
    public class PublicDataHelp
    {
        //数据库连接字符串
        private static String connstr = "";
        //数据库连接
        private static SqlConnection conn = null;

        private static SqlConnection getConn()
        {
            if (conn == null)
            {
                if (connstr.Equals(""))
                {
                    connstr = ConfigurationSettings.AppSettings["SQLString"].ToString();
                }
                conn = new SqlConnection(connstr);
                conn.Open();
                return conn;
            }
            else
            {
                return conn;
            }

        }

        public static void CloseConn()
        {
            if (conn != null)
            {
                conn.Close();
            }

        }

        /// <summary>
        /// 根据sql语句查询数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>List</returns>
        public static DataTable GetDataSuoceLis(String sql, string tableId)
        {
            SqlDataAdapter adap = null;
            if (sql.Equals(""))
            {
                return null;
            }
            try
            {
                //创建填充器
                if (conn == null)
                {
                    conn = getConn();
                }

                adap = new SqlDataAdapter(sql, conn);
                adap.SelectCommand.CommandTimeout = 3600;
                //实例化临时储存数据的DataTable对象
                DataTable dt = new DataTable();
                //填充数据
                adap.Fill(dt);
                dt.TableName = tableId;
                //返回数据();
                return dt;
            }
            finally
            {
                //关闭连接
                adap.Dispose();
                CloseConn();
            }
        }


        /// <summary>
        /// 根据sql语句更新 删除 插入数据
        /// </summary>
        /// <param name="SQLStringList"></param>
        public void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }
        /// <summary>
        /// 根据sql语句更新 删除 插入数据
        /// </summary>
        /// <param name="SQLStringList"></param>
        public int ExecuteSqlTran(string SQLString)
        {
            int i = 0;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                try
                {
                    cmd.CommandText = SQLString;
                    i = cmd.ExecuteNonQuery();
                    tx.Commit();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                    i = 0;
                }
            }
            return i;
        }
    }
}
