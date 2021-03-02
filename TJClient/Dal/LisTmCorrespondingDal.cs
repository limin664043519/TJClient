using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Logger;
using System.Text;
using FBYClient;
using TJClient.model;

namespace TJClient.Dal
{
    public class LisTmCorrespondingDal
    {
        //实例化SQLHelper对象
        private static DBAccess db = new DBAccess();

        //log对象
        public static SimpleLogger logger = SimpleLogger.GetInstance();
        public static DataTable GetLisTmCorrespondingByStatus(int status,string beginDate,string endDate)
        {
            
            try
            {
                //select tm.id,tm.qmlistm,tm.listm,tm.createdate,tm.jkdah,rkxzl.D_XM,rkxzl.D_SFZH,rkxzl.D_XB,rkxzl.D_CSRQ,rkxzl.NL from
                //(SELECT a.id, a.qmlistm, a.listm, a.createdate, b.jkdah from t_jk_listmcorresponding a, t_jk_tjry_txm b where a.listm = b.txmbh) tm,t_da_jkda_rkxzl rkxzl
                //where tm.jkdah = rkxzl.d_grdabh

                string sql = string.Format("SELECT a.id, a.qmlistm, a.listm, a.createdate,a.status,a.includeblood,b.jkdah from t_jk_listmcorresponding a, " +
                                           "t_jk_tjry_txm b where a.listm = b.txmbh and a.createdate>='{0}' and a.createdate<='{1}'", beginDate,endDate);
                if (status >= 0)
                {
                    sql = string.Format("{0} and a.status={1}", sql, status);
                }

                sql = string.Format("select tm.id,tm.qmlistm,tm.listm,tm.createdate,tm.jkdah,tm.status,tm.includeblood,rkxzl.D_XM,rkxzl.D_SFZH,rkxzl.D_XB,rkxzl.D_CSRQ,rkxzl.NL from " +
                                    "({0}) tm,t_da_jkda_rkxzl rkxzl where tm.jkdah = rkxzl.d_grdabh",sql);
                return db.ExecuteQueryBySql(sql);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new Exception("GetMoHuList:" + e.Message);
                return null;
            }
        }

        public static bool HadExisted(string lisTm)
        {
            try
            {
                string sql = string.Format("select * from t_jk_listmcorresponding where listm='{0}'",lisTm);
                DataTable dt= db.ExecuteQueryBySql(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new Exception("GetMoHuList:" + e.Message);
            }
        }
        /// <summary>
        /// 判断外部条码是否已经使用
        /// </summary>
        /// <param name="wbTm">外部条码</param>
        /// <returns></returns>
        public static bool TmHadUsed(string wbTm)
        {
            try
            {
                string sql = string.Format("select * from t_jk_listmcorresponding where qmlistm='{0}'", wbTm);
                DataTable dt = db.ExecuteQueryBySql(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public static bool Insert(LisTmCorresponding model)
        {
            string sql =
                string.Format(
                    "insert into t_jk_listmcorresponding(qmlistm,listm,createdate,status,includeblood) values('{0}','{1}','{2}',{3},{4})"
                    ,model.QmLisTm,model.LisTm,model.CreateDate,model.Status,model.IncludeBlood);
            return db.ExecuteNonQueryBySql(sql) > 0;
        }

        public static bool Modify(LisTmCorresponding model)
        {
            string sql = string.Format("update t_jk_listmcorresponding set qmlistm='{0}',includeblood={2} where listm='{1}'",
                model.QmLisTm,model.LisTm,model.IncludeBlood);
            return db.ExecuteNonQueryBySql(sql) > 0;
        }

        public static bool UpdateStatus(DataTable dt)
        {
            ArrayList sqlList=new ArrayList();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["status"].ToString() != "1")
                    {
                        sqlList.Add(string.Format("update t_jk_listmcorresponding set status=1 where id={0}", row["id"]));
                    }
                }

                db.ExecuteNonQueryBySqlList(sqlList);
            }

            return true;
        }

    }
}
