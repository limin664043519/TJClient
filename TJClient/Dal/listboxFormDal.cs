using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Logger;
using System.Data.SqlClient;
using FBYClient;
using TJClient.Common;

namespace TJClient.sys.Dal
{
    public class listboxFormDal
    {
        //实例化SQLHelper对象
        private DBAccess db = new DBAccess();

        //log对象
        public SimpleLogger logger = SimpleLogger.GetInstance();



        //获取信息
        public DataTable GetMoHuList(string strWhere,string code )
        {
            
            try
            {
                string sql_Sjzd = Common.Common.getSql(code, strWhere);
               return db.ExecuteQueryBySql(sql_Sjzd);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
                //return null;
            }
        }
    }
}
