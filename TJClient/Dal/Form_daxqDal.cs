using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Logger;
using System.Data.SqlClient;
using FBYClient;
using TJClient.Common;
using System.Collections;

namespace TJClient.sys.Dal
{
    public class Form_daxqDal
    {
        //实例化SQLHelper对象
        private DBAccess db = new DBAccess();

        //log对象
        public SimpleLogger logger = SimpleLogger.GetInstance();

        //获取信息
        public DataTable GetMoHuList(string strWhere ,string sqlcode)
        {
            string sql_Sjzd = Common.Common.getSql(sqlcode, strWhere);
            try
            {
                return db.ExecuteQueryBySql(sql_Sjzd);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool Update(DataTable dt,string sqlcode)
        {
            try
            {
                ArrayList sqlList = new ArrayList();
                string sql_Sjzd = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i].RowState == DataRowState.Modified)
                        {
                            sql_Sjzd = Common.Common.getSql(sqlcode, dt, i);
                            sqlList.Add(sql_Sjzd);
                        }
                    }
                }
                db.ExecuteNonQueryBySqlList(sqlList);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return true;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool Add(DataTable dt, string sqlcode)
        {
            try
            {
                ArrayList sqlList = new ArrayList();
                string sql_Sjzd = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i].RowState == DataRowState.Added)
                        {
                            sql_Sjzd = Common.Common.getSql(sqlcode, dt, i);
                            sqlList.Add(sql_Sjzd);
                        }
                    }
                }
                db.ExecuteNonQueryBySqlList(sqlList);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return true;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool AddAll(DataTable dt, string sqlcode)
        {
            try
            {
                ArrayList sqlList = new ArrayList();
                string sql_Sjzd = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                         sql_Sjzd = Common.Common.getSql(sqlcode, dt, i);
                         sqlList.Add(sql_Sjzd);
                        
                    }
                }
                db.ExecuteNonQueryBySqlList(sqlList);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool Del(DataTable dt, int rowIndex)
        {
            try
            {
                string sql_Sjzd = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    sql_Sjzd = Common.Common.getSql("sql021", dt, rowIndex);
                    db.ExecuteNonQueryBySql(sql_Sjzd);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool Del(DataTable dt, int rowIndex, string sqlcode)
        {
            try
            {
                string sql_Sjzd = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    sql_Sjzd = Common.Common.getSql(sqlcode, dt, rowIndex);
                    db.ExecuteNonQueryBySql(sql_Sjzd);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return true;
        }
    }
}
