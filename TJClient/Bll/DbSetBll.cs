using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TJClient.sys.Dal;
using System.Collections;
namespace TJClient.sys.Bll
{
    public class DbSetBll
    {
        public DbSetDal model = new DbSetDal();

        //获取信息
        public DataTable GetMoHuList(string strWhere,string sqlcode)
        {
            return model.GetMoHuList(strWhere, sqlcode);
        }

        //更新
        public bool Upd(DataTable dt, string sqlcode)
        {
            return model.Update(dt, sqlcode);
        }

        public bool Add(DataTable dt, string sqlcode)
        {
            return model.Add(dt, sqlcode);
        }

        public bool Del(DataTable dt,int rowIndex)
        {
            return model.Del(dt,rowIndex);
        }

        public bool DelByArrayList(ArrayList sqlList)
        {
            return model.DelByArrayList(sqlList);
        }

        public DataTable isExists(string strWhere, string sqlcode)
        {
            return model.GetMoHuList(strWhere, sqlcode);
        }
    }
}
