using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Logger;
using System.Data.Odbc;
using Oracle.DataAccess.Client;

    class DBOracle 
    {
        private static string connString = "";
        public SimpleLogger logger = SimpleLogger.GetInstance();
        public DBOracle()
        {
            connString = ConfigurationSettings.AppSettings["oracleConnection"];//oracle数据库
        }

        #region oracle
        /// <summary>
        /// 检索返回结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataTable ExcuteDataTable_oracle(string sql, params System.Data.SqlClient.SqlParameter[] pms)
        {
            using (OracleDataAdapter sda = new OracleDataAdapter(sql, connString))
            {
                DataTable dt = new DataTable();
                sda.SelectCommand.CommandType = CommandType.Text;
                if (pms != null)
                {
                    sda.SelectCommand.Parameters.AddRange(pms);
                }
                sda.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// 检索返回结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataTable ExcuteProDataTable_oracle(string sql, params System.Data.SqlClient.SqlParameter[] pms)
        {
            //Oracle.DataAccess.Client.OracleDataAdapter 
            using (Oracle.DataAccess.Client.OracleDataAdapter sda = new Oracle.DataAccess.Client.OracleDataAdapter(sql, connString))
            {
                DataTable dt = new DataTable();
                sda.SelectCommand.CommandType = CommandType.Text;
                if (pms != null)
                {
                    sda.SelectCommand.Parameters.AddRange(pms);
                }
                sda.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// 执行插入删除等操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public int ExecuteNonQuery_oracle(string sql, params System.Data.SqlClient.SqlParameter[] pms)
        {
            using (OracleConnection con = new OracleConnection(connString))
            {
                using (OracleCommand cmd = new OracleCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text;

                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行插入删除等操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public int ExecuteNonQuery_oracle(ArrayList sql)
        {
            using (OracleConnection con = new OracleConnection(connString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = con;
                    con.Open();

                    for (int i = 0; i < sql.Count; i++)
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql[i].ToString();
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + ":" + sql[i]);
                        }
                    }
                    return 1;
                }
            }
        }


        /// <summary>
        /// 数据的批量导入
        /// </summary>
        /// <param name="ds"></param>
        public void MultiInsertData(int recc, string sql, OracleParameter[] pms)
        {
            //string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.5.116)(PORT=1521))(CONNECT_DATA=(SID=zljyhis)));User Id=gwtj;Password=gwtj";

            OracleConnection conn = new OracleConnection(connString);
            OracleCommand command = new OracleCommand();
            command.Connection = conn;
            try
            {
                command.Connection = conn;
                //导入的数据条数
                command.ArrayBindCount = recc;
                //用到的是数组,而不是单个的值,这就是它独特的地方 
                command.CommandText = sql;// "insert into testnew values(:A, :B)";
                if (pms != null)
                {
                    for (int i = 0; i < pms.Length; i++)
                    {
                        command.Parameters.Add(pms[i]);
                    }
                }
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="ProcedureName">存储过程名称</param>
        /// <param name="pms">参数</param>
        public void ExecuteProNonQuery_oracle(string ProcedureName, params OracleParameter[] pms)
        {
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(connString);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = ProcedureName;

                
                //传递参数给Oracle命令
                for (int i = 0; i < pms.Length; i++)
                {
                    cmd.Parameters.Add(pms[i]);
                }

                //打开连接
                conn.Open();

                cmd.Connection = conn;
               int  rows = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //关闭连接,释放空间.
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }

            //return rows;
        }

        #endregion
    }

