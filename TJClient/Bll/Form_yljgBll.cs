using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TJClient.sys.Dal;
namespace TJClient.sys.Bll
{
    public class Form_yljgBll
    {
        public Form_yljgDal model = new Form_yljgDal();

        //获取信息
        public DataTable GetMoHuList(string strWhere)
        {
            return model.GetMoHuList(strWhere);
        }

        //获取信息
        public DataTable GetMoHuList(string strWhere,string sqlCode)
        {
            return model.GetMoHuList(strWhere, sqlCode);
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

        public bool Del(DataTable dt,int rowIndex)
        {
            return model.Del(dt,rowIndex);
        }
    }
}
