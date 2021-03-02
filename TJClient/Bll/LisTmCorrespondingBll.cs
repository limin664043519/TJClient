using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TJClient.Dal;
using TJClient.model;
namespace TJClient.Bll
{
    public class LisTmCorrespondingBll
    {
        public static DataTable GetLisTmCorrespondingByStatus(int status, string beginDate, string endDate)
        {
            return LisTmCorrespondingDal.GetLisTmCorrespondingByStatus(status, beginDate, endDate);
        }

        public static bool Operation(LisTmCorresponding model)
        {
            if (HadExisted(model.LisTm))
            {
                return Modify(model);
            }

            return Insert(model);
        }

        public static bool TmHadUsed(string wbTm)
        {
            return LisTmCorrespondingDal.TmHadUsed(wbTm);
        }

        public static bool HadExisted(string lisTm)
        {
            return LisTmCorrespondingDal.HadExisted(lisTm);
        }

        public static bool Insert(LisTmCorresponding model)
        {
            return LisTmCorrespondingDal.Insert(model);
        }

        public static bool Modify(LisTmCorresponding model)
        {
            return LisTmCorrespondingDal.Modify(model);
        }

        public static bool UpdateStatus(DataTable dt)
        {
            return LisTmCorrespondingDal.UpdateStatus(dt);
        }
    }
}
