using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FBYClient;
using TJClient.Common;
using TJClient.model;

namespace TJClient.Dal
{
    public class TjryTxmDal
    {
        public static DataTable GetAll(string grdabh)
        {
        //实例化SQLHelper对象
            DBAccess db = new DBAccess();
            string sql = string.Format("select * from t_jk_tjry_txm where jkdah='{0}' and yljgbm='{1}' and nd='{2}'",grdabh,UserInfo.Yybm,DateHelper.Year());
            return db.ExecuteQueryBySql(sql);
        }
    }
}
