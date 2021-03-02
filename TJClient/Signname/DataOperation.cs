using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FBYClient;
using TJClient.Signname.Model;

namespace TJClient.Signname
{
    public class DataOperation
    {
       
        public static DataOperation Init()
        {
            return new DataOperation();
        }

        private DBAccess _dbAccess = null;

        private DBAccess GetDBAccess()
        {
            if (_dbAccess==null)
            {
                _dbAccess = new DBAccess();
            }
            return _dbAccess;
        }

        public DataTable GetAllRealname(string yljgbm)
        {
            string sql =
                string.Format(
                    "select distinct realname from t_jk_usersignname where yljgbm='{0}' and realname is not null",yljgbm);
            DBAccess db = GetDBAccess();
            return db.ExecuteQueryBySql(sql);
        }

        public DataTable GetSignname(string czy)
        {
            string sql = string.Format("select  signnametitle,signnamepicpath,realname from t_jk_usersignname where czy='{0}' and isdelete=0", czy);
            DBAccess db = GetDBAccess();
            return db.ExecuteQueryBySql(sql);
        }

        /// <summary>
        /// 页面显示签名时检索
        /// </summary>
        /// <param name="jktjSignname"></param>
        /// <returns></returns>
        public DataTable GetJktjSignnameShow(JktjSignname jktjSignname)
        {
            string sql = string.Format("select  signnamepicpath,realname from t_jk_jktjsignname where d_grdabh='{0}' and tjsj='{1}' and signnamegroup<>'FKQZBR' and signnamegroup<>'FKQZJS'",
                jktjSignname.D_Grdabh, jktjSignname.Tjsj);
            DBAccess db = GetDBAccess();
            return db.ExecuteQueryBySql(sql);
        }

        public DataTable GetFkSignnameShow(JktjSignname jktjSignname)
        {
            string sql = string.Format("select  signnamegroup,signnamepicpath,realname,tjsj from t_jk_jktjsignname where d_grdabh='{0}' and Year(tjsj)={1} and (signnamegroup='FKQZBR' or signnamegroup='FKQZJS')",
                jktjSignname.D_Grdabh, jktjSignname.Tjsj);
            DBAccess db = GetDBAccess();
            return db.ExecuteQueryBySql(sql);
        }



        /// <summary>
        /// 保存时的更新还是插入的检索处理
        /// </summary>
        /// <param name="jktjSignname"></param>
        /// <returns></returns>
        public DataTable GetJktjSignname(JktjSignname jktjSignname)
        {
            string sql = string.Format("select signnamepicpath,realname from t_jk_jktjsignname where d_grdabh='{0}' and tjsj='{1}' and  SIGNNAMEGROUP ='{2}' and YLJGBM='{3}'",
                jktjSignname.D_Grdabh, jktjSignname.oldTjsj, jktjSignname.SignnameGroup, jktjSignname.Yljgbm);
            DBAccess db = GetDBAccess();
            return db.ExecuteQueryBySql(sql);
        }

        private bool HadJktjSignname(JktjSignname jktjSignname)
        {
            DataTable dt = GetJktjSignname(jktjSignname);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        private void InsertJktjSignname(JktjSignname jktjSignname)
        {
            DBAccess db = GetDBAccess();
            string sql = "insert into t_jk_jktjsignname(YLJGBM,D_GRDABH,TJSJ,USERSIGNNAMEID,SIGNNAMEPICPATH,SIGNNAMEGROUP,CZY,CREATEUSER,CREATEDATE,UPDATEUSER,UPDATEDATE,realname) " +
                         "values('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')";
            db.ExecuteNonQueryBySql(string.Format(sql, jktjSignname.Yljgbm, jktjSignname.D_Grdabh,
                    jktjSignname.Tjsj, jktjSignname.UserSignnameId,
                    jktjSignname.SignnamePicPath, jktjSignname.SignnameGroup, jktjSignname.Czy,
                    jktjSignname.CreateUser, jktjSignname.CreateDate, jktjSignname.UpdateUser, jktjSignname.UpdateDate, jktjSignname.Realname));
        }

        private void UpdateJktjSignname(JktjSignname jktjSignname)
        {
            DBAccess db = GetDBAccess();
            string sql = "update t_jk_jktjsignname set SIGNNAMEPICPATH='{0}',UPDATEUSER='{4}',UPDATEDATE='{5}',realname='{6}' ,tjsj='{7}' where " +
                         "d_grdabh='{1}' and tjsj='{2}' and signnamegroup='{3}'";
            db.ExecuteNonQueryBySql(string.Format(sql, jktjSignname.SignnamePicPath,jktjSignname.D_Grdabh,jktjSignname.oldTjsj,jktjSignname.SignnameGroup,
                jktjSignname.UpdateUser, jktjSignname.UpdateDate, jktjSignname.Realname, jktjSignname.Tjsj));
        }
        public void SaveJktjSignname(List<JktjSignname> jktjSignnames)
        {
            if (jktjSignnames.Count <= 0)
            {
                return;
            }
            foreach (JktjSignname jktjSignname in jktjSignnames)
            {
                if (HadJktjSignname(jktjSignname))
                {
                    UpdateJktjSignname(jktjSignname);
                }
                else
                {
                    InsertJktjSignname(jktjSignname);
                }
            }
        }

        public bool DeleteSignname(string grdabh, List<string> signnameGroup)
        {
            if (signnameGroup.Count <= 0)
            {
                return true;
            }
            string sql=string.Format("delete from t_jk_jktjsignname where d_grdabh='{0}' and signnamegroup in ({1})",grdabh,string.Join(",",signnameGroup));
            DBAccess db=GetDBAccess();
            db.ExecuteNonQueryBySql(sql);
            return true;
        }
    }
}
