using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TJClient.sys.Dal;
namespace TJClient.sys.Bll
{
    public class Form_commBll
    {
        public Form_commDal model = new Form_commDal();

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
    }
}
