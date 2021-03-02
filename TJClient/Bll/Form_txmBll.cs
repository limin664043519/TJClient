using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TJClient.sys.Dal;
namespace TJClient.sys.Bll
{
    public class Form_txmBll
    {
        public Form_txmDal model = new Form_txmDal();

        //获取信息
        public DataTable GetMoHuList(string strWhere)
        {
            return model.GetMoHuList(strWhere);
        }

        //获取信息
        public DataTable GetMoHuList(string strWhere,string sqlcode)
        {
            return model.GetMoHuList(strWhere, sqlcode);
        }
        //更新
        public bool Upd(DataTable dt)
        {
            return model.Update(dt);
        }

        public bool Add(DataTable dt)
        {
            return model.Add(dt);
        }

        public bool Add(DataTable dt,string sqlcode)
        {
            return model.Add(dt, sqlcode);
        }
        public bool Del(DataTable dt,int rowIndex)
        {
            return model.Del(dt,rowIndex);
        }

        public bool Del(DataTable dt, int rowIndex,string sqlcode)
        {
            return model.Del(dt, rowIndex, sqlcode);
        }
    }
}
