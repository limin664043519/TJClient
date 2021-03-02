using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Collections;
using FBYClient;
using System.Text.RegularExpressions;

namespace TJClient.Common
{
    class Common
    {
        public enum DBTYPE { access, db2, oracle};

        public enum SQLTYPE  { insert, delete };

        //public 
        /// <summary>
        /// 取得sql
        /// </summary>
        /// <param name="sqlCode"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string getSql(string sqlCode, string para)
        {
            //取得sql文
            string sql = "";

            //数据库类型 1：access  2：db2 3:oracle
            string dbType = ConfigurationSettings.AppSettings["dbType"];

            //access
            if (DBTYPE.access.ToString ().Equals(dbType) == true)
            {
                sql = XmlRW.GetSqlFormXML(getConfigPath("UserSqlConfig.xml"), sqlCode + "_A");
                sql = FormatSql_A(sql, para);
            }
            else if (DBTYPE.db2.ToString().Equals(dbType) == true)
            {
                sql = XmlRW.GetSqlFormXML(getConfigPath("UserSqlConfig.xml"), sqlCode + "_D");
                sql = FormatSql_D(sql, para);
            }

            else if (DBTYPE.oracle.ToString().Equals(dbType) == true)
            {
                sql = XmlRW.GetSqlFormXML(getConfigPath("UserSqlConfig.xml"), sqlCode + "_O");
                sql = FormatSql_O(sql, para);
            }
            return sql;
        }

        /// <summary>
        /// 取得sql
        /// </summary>
        /// <param name="sqlCode"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string getSql(string sqlCode, DataTable para, int rowNo)
        {
            //取得sql文
            string sql = "";

            //数据库类型 access db2
            string dbType = ConfigurationSettings.AppSettings["dbType"];

            //access
            if (DBTYPE.access.ToString().Equals(dbType) == true)
            {
                sql = XmlRW.GetSqlFormXML(getConfigPath("UserSqlConfig.xml"), sqlCode + "_A");
                sql = FormatSql_A(sql.ToLower(), para, rowNo);
            }
            else if (DBTYPE.db2.ToString().Equals(dbType) == true)
            {
                sql = XmlRW.GetSqlFormXML(getConfigPath("UserSqlConfig.xml"), sqlCode + "_D");
                sql = FormatSql_D(sql.ToLower(), para, rowNo);
            }
            else if (DBTYPE.oracle.ToString().Equals(dbType) == true)
            {
                sql = XmlRW.GetSqlFormXML(getConfigPath("UserSqlConfig.xml"), sqlCode + "_O");
                sql = FormatSql_O(sql.ToLower(), para, rowNo);
            }
            return sql;
        }

        /// <summary>
        /// 格式化access sql
        /// </summary>
        /// <returns></returns>
        public static string FormatSql_A(string sql, string para)
        {
            try
            {
                return string.Format(sql, para);
            }
            catch
            {
                return sql;
            }
        }

        /// <summary>
        /// 格式化oracle sql
        /// </summary>
        /// <returns></returns>
        public static string FormatSql_O(string sql, string para)
        {

            try
            {
                return string.Format(sql, para);
            }
            catch
            {
                return sql;
            }
        }

        /// <summary>
        /// 格式化db2 sql
        /// </summary>
        /// <returns></returns>
        public static string FormatSql_D(string sql, string para)
        {
            
            try
            {
                return string.Format(sql, para);
            }
            catch
            {
                return sql;
            }
        }

        /// <summary>
        /// 格式化access sql
        /// </summary>
        /// <returns></returns>
        public static string FormatSql_A(string sql, DataTable para, int rowNo)
        {
            if (para != null && para.Rows.Count > 0 && rowNo < para.Rows.Count)
            {
                for (int i = 0; i < para.Columns.Count; i++)
                {
                    if (para.Rows[rowNo][i] != null)
                    {
                        sql = sql.Replace("{" + para.Columns[i].ColumnName.ToString().ToLower() + "}", para.Rows[rowNo][i].ToString());
                    }
                    else
                    {
                        sql = sql.Replace("{" + para.Columns[i].ColumnName.ToString().ToLower() + "}", "");
                    }
                }
            }
            return sql;
        }

        /// <summary>
        /// 格式化db2 sql
        /// </summary>
        /// <returns></returns>
        public static string FormatSql_D(string sql, DataTable para, int rowNo)
        {
            return string.Format(sql, para);
        }

        /// <summary>
        /// 格式化oracle sql
        /// </summary>
        /// <returns></returns>
        public static string FormatSql_O(string sql, DataTable para, int rowNo)
        {
            return string.Format(sql, para);
        }

        /// <summary>
        /// 获取用户sql文件的路径
        /// </summary>
        /// <returns></returns>
        public static string getConfigPath(string fileName)
        {
            return System.Configuration.ConfigurationSettings.AppSettings["sqlfilePath"].ToString();// "E:\\0SVN\\公共卫生体检系统\\2.0\\程序\\TJClient_2008\\TJClient\\TJClient\\sql\\" + fileName;
        }

        #region 自动生成sql
        /// <summary>
        /// 按照数据库的类型和datatable 生成SQL文
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="sqlType">操作：insert delete</param>
        /// <returns></returns>
        public static ArrayList FormatSql(DataTable dt, string sqlType, string para)
        {

            //取得sql文
            ArrayList sqllist = new ArrayList();

            //数据库类型 access  db2
            string dbType = ConfigurationSettings.AppSettings["dbType"];

            //access
            if (DBTYPE.access.ToString () .Equals(dbType) == true)
            {
                //新增
                if (sqlType.Equals(SQLTYPE.insert.ToString() ))
                {
                   sqllist=createSql_insert_access(dt);
                }
                else if (sqlType.Equals(SQLTYPE.delete.ToString()))
                {
                    //生成删除 sql
                    string deletesql = " delete from  " + dt.TableName;
                    if (dt.Columns.Contains("CZBM"))
                    {
                        deletesql = deletesql + " where CZBM in (" + para + ") ";
                    }
                    if (dt.Columns.Contains("D_JWH"))
                    {
                        deletesql = deletesql + " where D_JWH in (" + para + ") ";
                    }

                    sqllist.Add(deletesql);
                }
            }
            else if (DBTYPE.db2.ToString() .Equals(dbType) == true)
            {
                //db2数据库
            }
            else if (DBTYPE.oracle.ToString().Equals(dbType) == true)
            {
                //db2数据库
            }

            return sqllist;
        }

        /// <summary>
        /// 生成新增数据用的sql
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static ArrayList createSql_insert_access(DataTable dt)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sqlvalueBuilder = new StringBuilder();
            StringBuilder sqlColumnsBuilder = new StringBuilder();
            DBAccess access = new DBAccess();
            ArrayList sqllist = new ArrayList();

            //取得数据库表的结构
            DataTable dbRowName = access.ExecuteQueryBySql(" Select top 1 * from  " + dt.TableName);

            //生成sql模版
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dbRowName.Columns.Contains(dt.Columns[i].ColumnName) && dbRowName.Columns[dt.Columns[i].ColumnName].DataType.Equals(Type.GetType("System.String")))
                {
                    sqlColumnsBuilder.Append("[").Append(dt.Columns[i].ColumnName).Append("],");
                    sqlvalueBuilder.Append("'{" + dt.Columns[i].ColumnName.ToLower() + "}',");

                }
                else if (dbRowName.Columns.Contains(dt.Columns[i].ColumnName))
                {
                    sqlColumnsBuilder.Append("[").Append(dt.Columns[i].ColumnName).Append("],");
                    sqlvalueBuilder.Append("0{" + dt.Columns[i].ColumnName.ToLower() + "},");
                }
            }
            sql.Append(" insert into ").Append(dt.TableName).Append(" ( ").Append(sqlColumnsBuilder.ToString().Substring(0, sqlColumnsBuilder.ToString().Length - 1)).Append(" ) ");
            sql.Append(" values (").Append(sqlvalueBuilder.ToString().Substring(0, sqlvalueBuilder.ToString().Length - 1)).Append(" )");

            //生成sql
            string sqltem = "";
            for (int j = 1; j < dt.Rows.Count; j++)
            {
                sqltem = sql.ToString();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sqltem = sqltem.Replace("{" + dt.Columns[i].ColumnName + "}", dt.Rows[j][i]!=null?dt.Rows[j][i].ToString().Replace ("'","''"):"");
                }
                sqllist.Add(sqltem);
            }
            return sqllist;

        }
        #endregion

        #region

        /// <summary>
        /// 取得数据字典中的值
        /// </summary>
        /// <param name="zdCode"></param>
        /// <param name="sqlCode"></param>
        /// <returns></returns>
        public static DataTable getsjzd(string zdCode, string sqlCode)
        {
            //Form_commBll comm = new Form_commBll();
            //string strWhere = "";
            //if (zdCode.Length > 0)
            //{
            //    strWhere = strWhere + string.Format(" and  zdlxbm ='{0}'  ", zdCode);
            //}
            //DataTable dt = comm.GetMoHuList(strWhere, sqlCode);
            //return dt;
            return null;
        }


        #endregion

        #region 身份证验证
        /// <summary>
        /// 验证身份证号合法性
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool CheckIDCard(string Id)
        {
            if (Id.Trim().Length == 15)
            {
                return CheckIDCard15(Id);
            }
            else if (Id.Trim().Length == 18)
            {
                return CheckIDCard18(Id);
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 验证18位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 验证15位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        /// <summary>
        /// 根据身份证获取生日
        /// </summary>
        /// <param name="cardid">身份证</param>
        /// <param name="res">是否有格式(true1990-01-01,false19900101)</param>
        /// <returns></returns>
        public static string GetBirthdayByIdentityCardId(string cardid, bool res)
        {
            string birthday = string.Empty;
            System.Text.RegularExpressions.Regex regex = null;

            if (cardid.Length == 18)
            {
                regex = new Regex(@"^\d{17}(\d|x)$");
                if (regex.IsMatch(cardid))
                {
                    if (res)
                        birthday = cardid.Substring(6, 8).Insert(4, "-").Insert(7, "-");
                    else
                        birthday = cardid.Substring(6, 8).Insert(4, "-").Insert(7, "-");
                }
                else
                {
                    birthday = cardid.Substring(6, 8).Insert(4, "-").Insert(7, "-");
                }
            }
            else if (cardid.Length == 15)
            {
                regex = new Regex(@"^\d{15}");
                if (regex.IsMatch(cardid))
                {
                    if (res)
                        birthday = cardid.Substring(6, 6).Insert(2, "-").Insert(5, "-");
                    else
                        birthday = cardid.Substring(6, 6);
                }
                else
                {
                    birthday = cardid.Substring(6, 6).Insert(2, "-").Insert(5, "-");
                }
            }
            else
            {
                birthday = "";
            }

            return birthday;
        }


        /// <summary>
        /// 根据身份证获取身份证信息
        /// 18位身份证
        /// 0地区代码(1~6位,其中1、2位数为各省级政府的代码，3、4位数为地、市级政府的代码，5、6位数为县、区级政府代码)
        /// 1出生年月日(7~14位)
        /// 2顺序号(15~17位单数为男性分配码，双数为女性分配码)
        /// 3性别
        /// 
        /// 15位身份证
        /// 0地区代码 
        /// 1出生年份(7~8位年,9~10位为出生月份，11~12位为出生日期 
        /// 2顺序号(13~15位)，并能够判断性别，奇数为男，偶数为女
        /// 3性别
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static string[] GetCardIdInfo(string cardId)
        {
            string[] info = new string[4];
            System.Text.RegularExpressions.Regex regex = null;
            if (cardId.Length == 18)
            {
                regex = new Regex(@"^\d{17}(\d|x)$");
                if (regex.IsMatch(cardId))
                {
                    info.SetValue(cardId.Substring(0, 6), 0);
                    info.SetValue(cardId.Substring(6, 8), 1);
                    info.SetValue(cardId.Substring(14, 3), 2);
                    info.SetValue(Convert.ToInt32(info[2]) % 2 != 0 ? "男" : "女", 3);
                }
            }
            else if (cardId.Length == 15)
            {
                regex = new Regex(@"^\d{15}");
                if (regex.IsMatch(cardId))
                {
                    info.SetValue(cardId.Substring(0, 6), 0);
                    info.SetValue(cardId.Substring(6, 6), 1);
                    info.SetValue(cardId.Substring(12, 3), 2);
                    info.SetValue(Convert.ToInt32(info[2]) % 2 != 0 ? "男" : "女", 3);
                }
            }

            return info;

        }
        #endregion

        #region 体检类型
        public class TJTYPE
        {
            public static string 登记 = "0";
            public static string 健康体检表 = "1";
            public static string 中医体质辨识 = "2";
            public static string 生活自理能力评估 = "3"; 
        }
        #endregion

        #region 体检状态
        public class ZT
        {
            public static string 否定状态 = "0";
            public static string 确定状态 = "1";
        }
        #endregion


    }
}
