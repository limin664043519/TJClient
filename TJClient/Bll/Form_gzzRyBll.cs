using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TJClient.sys.Dal;
namespace TJClient.sys.Bll
{
    public class Form_gzzRyBll
    {
        public Form_gzzRyDal model = new Form_gzzRyDal();

        //获取信息
        public DataTable GetMoHuList(string strWhere)
        {
            return model.GetMoHuList(strWhere);
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
