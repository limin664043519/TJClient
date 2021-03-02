using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TJClient.sys.Dal;
namespace TJClient.sys.Bll
{
    public class Form_lisBll
    {
        public Form_lisDal model = new Form_lisDal();

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

        //更新
        public bool Upd_all(DataTable dt, string sqlcode)
        {
            return model.Update_all(dt, sqlcode);
        }

        public bool AddNoNeedStateCheck(DataTable dt, string sqlcode)
        {
            return model.AddNoNeedStateCheck(dt, sqlcode);
        }

        public bool Add(DataTable dt, string sqlcode,bool dontNeedToCheckStatus=false)
        {
            return model.Add(dt, sqlcode,dontNeedToCheckStatus);
        }

        public bool Del(DataTable dt,int rowIndex)
        {
            return model.Del(dt,rowIndex);
        }
        public int DelBySql(string strWhere, string sqlcode)
        {
            return model.DelBySql(strWhere, sqlcode);
        }

        public DataTable isExists(string strWhere, string sqlcode)
        {
            return model.GetMoHuList(strWhere, sqlcode);
        }
    }
}
