using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Logger;
using System.Text;
using FBYClient;
using TJClient.Common;

namespace TJClient.Dal
{
    public class TmDal
    {
        private static DBAccess db = new DBAccess();

        //log对象
        public static SimpleLogger logger = SimpleLogger.GetInstance();

        //获取信息
        public static DataTable GetLisTmDataTable()
        {
            string sql = string.Format("select * from t_jk_tm where yljgbm='{0}' and sfdy='1' and sqxmdh<>''",UserInfo.Yybm);
            try
            {
                return db.ExecuteQueryBySql(sql);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new Exception("GetMoHuList:" + e.Message);
                return null;
            }
        }
    }
}
