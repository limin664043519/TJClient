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
    public class RegisterDal
    {
        //实例化SQLHelper对象
        private DBAccess db = new DBAccess();

        //log对象
        public SimpleLogger logger = SimpleLogger.GetInstance();


        /// <summary>
        /// 是否开启验证
        /// </summary>
        /// <param name="yljgbm"></param>
        /// <returns>false: 不验证  true:验证 </returns>
        public string isUserREGISTER(string yljgbm)
        {
            DataTable  return_dt = db.ExecuteQueryBySql(string.Format("select svalue from  BASE_SYSCONFIG where codes='Register' and yybm='{0}'", yljgbm));

            if (return_dt != null && return_dt.Rows.Count >0)
            {


                return return_dt.Rows[0]["svalue"].ToString();
                //if (return_dt.Rows[0]["svalue"].ToString().Equals("0"))
                //{
                //    return false;
                //}
            }
            return "";
        }

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
                throw new Exception("GetMoHuList错误！" + e.Message);
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
            ArrayList sqlList = new ArrayList();
            try
            {
                
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
    }
}
