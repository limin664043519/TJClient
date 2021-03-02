using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TJClient.Dal;
using TJClient.model;

namespace TJClient.Bll
{
    public class TjryTxmBll
    {
        public static List<TjryTxm> GetAll(string grdabh)
        {
            List<TjryTxm> result=new List<TjryTxm>();
            DataTable dt = TjryTxmDal.GetAll(grdabh);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    TjryTxm info = new TjryTxm()
                    {
                        Yljgbm=row["yljgbm"].ToString(),
                        Txmbh=row["txmbh"].ToString(),
                        Tmbm=row["tmbm"].ToString(),
                        Jkdah=row["jkdah"].ToString(),
                        Sfzh=row["sfzh"].ToString(),
                        Nd=row["nd"].ToString()
                    };
                    result.Add(info);
                }
            }

            return result;
        }

        public static TjryTxm GetLisTjryTxm(string tmbm,string grdabh)
        {
            List<TjryTxm> list = GetAll(grdabh);
            TjryTxm result = new TjryTxm();
            if (list.Count > 0)
            {
                result = list.SingleOrDefault(x => x.Tmbm == tmbm);
            }

            return result;
        }

    }
}
