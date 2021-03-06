﻿using System;
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
    public class Form_gzzXmDal
    {
        //实例化SQLHelper对象
        private DBAccess db = new DBAccess();

        //log对象
        public SimpleLogger logger = SimpleLogger.GetInstance();

        //获取信息
        public DataTable GetMoHuList(string strWhere)
        {
            string sql_Sjzd = Common.Common.getSql("sql022", strWhere);
            try
            {
                return db.ExecuteQueryBySql(sql_Sjzd);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool Update(DataTable dt)
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
                            sql_Sjzd = Common.Common.getSql("sql023", dt, i);
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
        public bool Add(DataTable dt)
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
                            sql_Sjzd = Common.Common.getSql("sql024", dt, i);
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
        /// 删除
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool Del(DataTable dt,int rowIndex)
        {
            try
            {
                string sql_Sjzd = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    sql_Sjzd = Common.Common.getSql("sql025", dt, rowIndex);
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
