using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TJClient.Dal;
using TJClient.model;

namespace TJClient.Bll
{
    public class TmBll
    {
        private static List<Tm> _list = null;
        public static List<Tm> GetTmList()
        {
            if (_list == null || _list.Count<=0)
            {
                _list = new List<Tm>();
                DataTable dt = TmDal.GetLisTmDataTable();
                if (dt!=null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Tm tm=new Tm()
                        {
                            Iflnr=row["iflnr"].ToString(),
                            Sfdy=row["sfdy"].ToString(),
                            Sqxmdh=row["sqxmdh"].ToString(),
                            Tmbm=row["tmbm"].ToString(),
                            Tmmc=row["tmmc"].ToString(),
                            Yljgbm = row["yljgbm"].ToString()
                        };
                        _list.Add(tm);
                    }
                }
            }

            return _list;
        }
    }
}
