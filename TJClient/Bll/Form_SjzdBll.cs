using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TJClient.sys.Dal;
namespace TJClient.sys.Bll
{
    public class Form_SjzdBll
    {
        public Form_SjzdDal model = new Form_SjzdDal();

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

        ////根据code获取value
        //public string GetValueByCode(string code)
        //{
        //    return model.GetValueByCode(code);
        //}
        //public bool Add(configModel mod)
        //{
        //    return model.Add(mod);
        //}

        //public bool Upd(configModel mod)
        //{
        //    return model.Update(mod);
        //}

        //public bool Del(string configBM)
        //{
        //    return model.Delete(configBM);
        //}

        ///// <summary>
        ///// 是否存在该记录
        ///// </summary>
        //public bool Exists(string configBM)
        //{
        //    return model.Exists(configBM);
        //}
    }
}
