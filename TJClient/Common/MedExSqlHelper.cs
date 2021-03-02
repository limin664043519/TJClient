using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Logger;
using System.Collections;

namespace FBYClient
{
    public class MedExSqlHelper
    {

        public SqlConnection connection = null;
        public SqlTransaction transaction = null;

        public static string conStr = "";

        public MedExSqlHelper()
        {
            //XmlRW xml = new XmlRW();
            //社保
            conStr = System.Configuration.ConfigurationSettings.AppSettings["MedExConnection"].ToString();// xml.GetValueFormXML(Common.getConfigPath("UserConfig.xml"), "sql");
        }


        /// <summary>
        /// 开启事物
        /// </summary>
        /// <returns></returns>
        public Boolean beginTransaction()
        {
            try
            {
                connection = new SqlConnection(conStr);
                connection.Open();
                transaction = connection.BeginTransaction("SampleTransaction");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void close()
        {
            if (transaction != null)
            {
                transaction.Dispose();
            }
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        /// <summary>
        /// 回滚事物
        /// </summary>
        /// <returns></returns>
        public Boolean RollbackTransaction()
        {
            try
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                if (connection != null)
                {
                    connection.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 提交事物
        /// </summary>
        /// <returns></returns>
        public Boolean CommitTransaction()
        {
            try
            {
                if (transaction != null)
                {
                    transaction.Commit();
                }
                if (connection != null)
                {
                    connection.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///  启用事物
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public int ExecuteNonQueryByTransaction(string sql, params SqlParameter[] pms)
        {
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.Transaction = this.transaction;

                cmd.CommandType = CommandType.Text;
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///  启用事物
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public int ExecuteNonQueryByTransaction(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, connection);
            try
            {
                cmd.Transaction = this.transaction;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 1000;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int ExecuteNonQuery(string sql, params SqlParameter[] pms)
        {
            //SimpleLogger logger = SimpleLogger.GetInstance();
            //logger.Error("ExecuteNonQuery:"+sql);
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    // logger.Error("ExecuteNonQuery:CommandText:" + cmd.CommandText);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int ExecuteProNonQuery(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
        /// 插入记录，更新记录，删除记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQueryBySqlList(ArrayList sql)
        {
            int rowcount = 0;
            SqlTransaction tx = null;
            SqlCommand cmd = null;
            try
            {
                connection = new SqlConnection(conStr);
                connection.Open();

                cmd = new SqlCommand();

                string sqlTimeOut = ConfigurationSettings.AppSettings["sqlTimeOut"].ToString();
                if (sqlTimeOut.Length > 0)
                {
                    cmd.CommandTimeout = int.Parse(sqlTimeOut);
                }

                tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                cmd.Connection = connection;
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
                connection.Close();
            }
            return rowcount;
        }

        public int ExecuteScalar(string sql, params SqlParameter[] pms)
        {

            DataTable dt = ExcuteDataTable(sql, pms);
            if (dt != null && dt.Rows.Count > 0)
            {

                return int.Parse(dt.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
            //using (SqlConnection con = new SqlConnection(conStr))
            //{
            //    using (SqlCommand cmd = new SqlCommand(sql, con))
            //    {
            //        cmd.CommandType = CommandType.Text;
            //        if (pms != null)
            //        {
            //            cmd.Parameters.AddRange(pms);
            //        }
            //        con.Open();

            //        object o= cmd.ExecuteScalar();
            //        if (o != null)
            //        {
            //            int i;
            //            int.TryParse(o.ToString(), out i);
            //            return i;
            //        }
            //        else
            //            return 0;
            //    }
            //}
        }
        public int ExecuteProScalar(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    object o = cmd.ExecuteScalar();
                    if (o != null)
                    {
                        int i;
                        int.TryParse(o.ToString(), out i);
                        return i;
                    }
                    else
                        return 0;
                }
            }
        }
        public SqlDataReader ExecuteReader(string sql, params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(conStr);
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text; ;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return reader;
                }
            }
            catch
            {
                con.Dispose();
                throw;
            }
        }
        public SqlDataReader ExecuteProReader(string sql, params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(conStr);
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return reader;
                }
            }
            catch
            {
                con.Dispose();
                throw;
            }
        }
        public DataTable ExcuteDataTable(string sql, params SqlParameter[] pms)
        {
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(sql, conStr))
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
            catch (Exception ex)
            {
                throw new Exception("ExcuteDataTable异常!" + ex.Message);
            }
        }
        public DataTable ExcuteProDataTable(string sql, params SqlParameter[] pms)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, conStr))
            {
                DataTable dt = new DataTable();
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (pms != null)
                {
                    sda.SelectCommand.Parameters.AddRange(pms);
                }
                sda.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// 调用存储过程返回字符串
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public string ExcuteProBySp(string spName, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = spName;

                        if (pms != null)
                        {
                            cmd.Parameters.AddRange(pms);
                        }
                        con.Open();

                        SqlParameter returnValue = new SqlParameter("@returnValue", SqlDbType.Text);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnValue);
                        cmd.ExecuteNonQuery();
                        return returnValue.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        SimpleLogger logger = SimpleLogger.GetInstance();
                        logger.Error("ExcuteProBySp:" + ex.Message);
                        return "";
                    }
                }
            }
        }
        /// <summary>
        /// 调用存储过程返回结果集
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataTable ExcuteProBySpTDatatable(string spName, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                DataTable dt = new DataTable();
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(spName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return dt;
            }

        }

        /// <summary>
        /// 调用存储过程返回结果集
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public DataSet ExcuteProBySpTDataSet(string spName, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(spName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool sqlBulkCopyToServer(DataTable dt_main, DataTable dt_detail)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(conStr, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        if (dt_main != null && dt_main.Rows.Count > 0)
                        {
                            sqlbulkcopy.DestinationTableName = dt_main.TableName;
                            for (int i = 0; i < dt_main.Columns.Count; i++)
                            {
                                sqlbulkcopy.ColumnMappings.Add(dt_main.Columns[i].ColumnName, dt_main.Columns[i].ColumnName);
                            }
                            sqlbulkcopy.WriteToServer(dt_main);
                        }

                        if (dt_detail != null && dt_detail.Rows.Count > 0)
                        {
                            sqlbulkcopy.DestinationTableName = dt_detail.TableName;
                            for (int i = 0; i < dt_detail.Columns.Count; i++)
                            {
                                sqlbulkcopy.ColumnMappings.Add(dt_detail.Columns[i].ColumnName, dt_detail.Columns[i].ColumnName);
                            }
                            sqlbulkcopy.WriteToServer(dt_detail);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return true;
        }
    }
}
