using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TJClient.sys.Dal;
namespace TJClient.sys.Bll
{
    public class listboxFormBll
    {
        public listboxFormDal model = new listboxFormDal();

        //获取信息
        public DataTable GetMoHuList(string strWhere,string code )
        {
            return model.GetMoHuList(strWhere,code);
        }
    }
}
