using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Collections;
using FBYClient;
using TJClient.sys.Bll;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace TJClient.Common
{
    class Common
    {
        public enum DBTYPE { access, db2, oracle };

        public enum SQLTYPE { insert, delete };

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
            if (DBTYPE.access.ToString().Equals(dbType) == true)
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
                        sql = sql.Replace("{" + para.Columns[i].ColumnName.ToString().ToLower() + "}", para.Rows[rowNo][i].ToString()).Replace("{" + para.Columns[i].ColumnName.ToString().ToUpper() + "}", para.Rows[rowNo][i].ToString());
                    }
                    else
                    {
                        sql = sql.Replace("{" + para.Columns[i].ColumnName.ToString().ToLower() + "}", "").Replace("{" + para.Columns[i].ColumnName.ToString().ToUpper() + "}", "");
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
            return getsqlPath();// "E:\\0SVN\\公共卫生体检系统\\2.0\\程序\\TJClient_2008\\TJClient\\TJClient\\sql\\" + fileName;
        }

        #region 更新健康体检表
        /// <summary>
        /// 更新健康体检表
        /// </summary>
        /// <returns></returns>
        public string updateLis_jktj( DataTable dtList_ryxx)
        {
            //没有需要同步的人员
            if (dtList_ryxx == null || dtList_ryxx.Rows.Count == 0)
            {
                return "1";
            }
            try
            {
                Form_lisBll form_lisbll = new Form_lisBll();
                string JKDAH="";
                string tjsj="";
                for (int i = 0; i < dtList_ryxx.Rows.Count; i++)
                {
                    JKDAH=dtList_ryxx.Rows[i]["JKDAH"].ToString();
                    //获取人员的信息（条码号 条码类型）
                    string sqlWhere = string.Format(" and  YLJGBM='{0}' and JKDAH='{1}'  and nd='{2}' ", UserInfo.Yybm,JKDAH , DateTime.Now.Year.ToString());
                    DataTable dt_txmList = form_lisbll.GetMoHuList(sqlWhere, "sql_select_people_txm");
                    if (dt_txmList != null && dt_txmList.Rows.Count > 0)
                    {
                        //按照条码号同步lis信息
                        for (int j = 0; j < dt_txmList.Rows.Count; j++)
                        {
                            //按照条码号获取lis检验信息
                            string sqlWhereLis = string.Format(" and  testno='{0}' ", dt_txmList.Rows[i]["TXMBH"].ToString());
                            DataTable dt_lis_reqresultList = form_lisbll.GetMoHuList(sqlWhereLis, "sql_select_lis_reqresult");
                            if (dt_lis_reqresultList != null && dt_lis_reqresultList.Rows.Count > 0)
                            {
                                //获取检验项目与健康体检表的项目对应关系
                                string yq = dt_lis_reqresultList.Rows[0]["instrument"].ToString();
                                tjsj= dt_lis_reqresultList.Rows[0]["resulttime"].ToString();
                                string sqlWhereLisItems = string.Format(" and (YQLX='' or YQLX is null or YQLX='{0}') and YLJGBM='{1}' ", yq, UserInfo.Yybm);
                                DataTable dt_LisItemsList = form_lisbll.GetMoHuList(sqlWhereLisItems, "sql_select_lis_reqresult");
                                if (dt_LisItemsList != null && dt_LisItemsList.Rows.Count > 0)
                                {
                                    DataTable dt_tjjgUpdate = new DataTable();
                                    dt_tjjgUpdate.Rows.Add();

                                    //获取对应的数据值
                                    for (int k = 0; k < dt_LisItemsList.Rows.Count; k++)
                                    {
                                        //lis结果
                                        for (int m = 0; m < dt_lis_reqresultList.Rows.Count; m++)
                                        {
                                            if (dt_LisItemsList.Rows[k]["XMBM_LIS"].ToString().ToUpper().Equals(dt_lis_reqresultList.Rows[m]["itemno"].ToString().ToUpper()))
                                            {
                                                if (dt_tjjgUpdate.Columns.Contains(dt_LisItemsList.Rows[k]["XMBM"].ToString()) == false)
                                                {
                                                    DataColumn dtColumn = new DataColumn(dt_LisItemsList.Rows[k]["XMBM"].ToString());
                                                    dtColumn.DefaultValue = dt_lis_reqresultList.Rows[m]["testresult"].ToString();
                                                    dt_tjjgUpdate.Columns.Add(dtColumn);
                                                }
                                                else
                                                {
                                                    dt_tjjgUpdate.Rows[dt_tjjgUpdate.Rows.Count - 1][dt_LisItemsList.Rows[k]["XMBM"].ToString()] = dt_lis_reqresultList.Rows[m]["testresult"].ToString();
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    //数据更新到健康体检结果中
                                    Update_jktj(dt_tjjgUpdate, JKDAH, tjsj);
                                }
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 更新健康体检表
        /// </summary>
        /// <param name="dt_tjjgUpdate"></param>
        /// <returns></returns>
        public bool Update_jktj(DataTable dt_tjjgUpdate, string JKDAH, string jyrq_tem)
        {
            try
            {
                if (dt_tjjgUpdate == null || dt_tjjgUpdate.Columns.Count == 0 || dt_tjjgUpdate.Rows.Count == 0)
                {
                    return false;
                }

                if (dt_tjjgUpdate.Columns.Contains("D_GRDABH") == false)
                {
                    DataColumn dtColumn = new DataColumn("D_GRDABH");
                    dtColumn.DefaultValue = JKDAH;
                    dt_tjjgUpdate.Columns.Add(dtColumn);
                }
                if (dt_tjjgUpdate.Columns.Contains("HAPPENTIME") == false)
                {
                    DataColumn dtColumn = new DataColumn("HAPPENTIME");
                    dtColumn.DefaultValue = jyrq_tem;
                    dt_tjjgUpdate.Columns.Add(dtColumn);
                }

                if (dt_tjjgUpdate.Columns.Contains("czy") == false)
                {
                    DataColumn dtColumn = new DataColumn("czy");
                    dtColumn.DefaultValue = UserInfo.userId;
                    dt_tjjgUpdate.Columns.Add(dtColumn);
                }
                if (dt_tjjgUpdate.Columns.Contains("gzz") == false)
                {
                    DataColumn dtColumn = new DataColumn("gzz");
                    dtColumn.DefaultValue = UserInfo.gzz;
                    dt_tjjgUpdate.Columns.Add(dtColumn);
                }

                //体检结果是否已经存在
                string Guid = "";
                //true:新的Guid  false:已经存在的Guid
                bool GuidResult = true;
                GuidResult = getNewGuid(out Guid, JKDAH, jyrq_tem);

                Form_lisBll form_lisbll = new Form_lisBll();
                if (dt_tjjgUpdate.Columns.Contains("guid") == false)
                {
                    DataColumn dtColumn = new DataColumn("guid");
                    dtColumn.DefaultValue = Guid;
                    dt_tjjgUpdate.Columns.Add(dtColumn);
                }

                //体检结果
                if (GuidResult == true)
                {
                    //体检结果插入
                    form_lisbll.Add(dt_tjjgUpdate, "sql047");
                }
                else
                {
                    //体检结果更新
                    dt_tjjgUpdate.AcceptChanges();
                    dt_tjjgUpdate.Rows[0]["guid"] = Guid;
                    form_lisbll.Upd(dt_tjjgUpdate, "sql048");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得Guid
        /// </summary>
        /// <returns></returns>
        private bool getNewGuid(out string guid,string JKDAH, string jyrq_tem )
        {
            guid = System.Guid.NewGuid().ToString();
            DBAccess dBAccess = new DBAccess();
            string sql = "";
            ArrayList sqlList = new ArrayList();
            sql = "select guid from T_JK_JKTJ where  d_grdabh='{d_grdabh}' and happentime='{happentime}'";

            //健康档案编号
            sql = sql.Replace("{d_grdabh}", JKDAH);

            //体检日期
            sql = sql.Replace("{happentime}", Convert.ToDateTime(jyrq_tem).ToString("yyyy-MM-dd"));

            DataTable dt = dBAccess.ExecuteQueryBySql(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                guid = dt.Rows[0]["guid"].ToString();
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region 时间处理
        /// <summary>   
        /// 判断用户输入是否为日期   
        /// </summary>   
        /// <param ></param>   
        /// <returns></returns>   
        /// <remarks>   
        /// 可判断格式如下（其中-可替换为.，不影响验证)   
        /// YYYY | YYYY-MM |YYYY.MM| YYYY-MM-DD|YYYY.MM.DD | YYYY-MM-DD HH:MM:SS | YYYY.MM.DD HH:MM:SS | YYYY-MM-DD HH:MM:SS.FFF | YYYY.MM.DD HH:MM:SS:FF   (年份验证从1000到2999年)
        /// </remarks>   
        public static bool IsDateTime(string strValue)
        {
            if (strValue == null || strValue == "")
            {
                return true;
            }
            string regexDate = @"[1-2]{1}[0-9]{3}((-|[.]){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|[.]){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";
            if (Regex.IsMatch(strValue, regexDate))
            {
                //以下各月份日期验证，保证验证的完整性     
                int _IndexY = -1;
                int _IndexM = -1;
                int _IndexD = -1;
                if (-1 != (_IndexY = strValue.IndexOf("-")))
                {
                    _IndexM = strValue.IndexOf("-", _IndexY + 1);
                    _IndexD = strValue.IndexOf(":");
                }
                else
                {
                    _IndexY = strValue.IndexOf(".");
                    _IndexM = strValue.IndexOf(".", _IndexY + 1);
                    _IndexD = strValue.IndexOf(":");
                }
                //不包含日期部分，直接返回true     
                if (-1 == _IndexM)
                {
                    return true;
                }
                if (-1 == _IndexD)
                {
                    _IndexD = strValue.Length + 3;
                }
                int iYear = Convert.ToInt32(strValue.Substring(0, _IndexY));
                int iMonth = Convert.ToInt32(strValue.Substring(_IndexY + 1, _IndexM - _IndexY - 1));
                int iDate = Convert.ToInt32(strValue.Substring(_IndexM + 1, _IndexD - _IndexM - 4));
                //判断月份日期    
                if ((iMonth < 8 && 1 == iMonth % 2) || (iMonth > 8 && 0 == iMonth % 2))
                {
                    if (iDate < 32)
                    { return true; }
                }
                else
                {
                    if (iMonth != 2)
                    {
                        if (iDate < 31)
                        { return true; }
                    }
                    else
                    {
                        //闰年       
                        if ((0 == iYear % 400) || (0 == iYear % 4 && 0 < iYear % 100))
                        {
                            if (iDate < 30)
                            { return true; }
                        }
                        else
                        {
                            if (iDate < 29)
                            { return true; }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 时间串格式化
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="FormatStr"></param>
        /// <returns></returns>
        public static string FormatDateTime(string dateStr, string FormatStr)
        {
            if (dateStr.Length == 0 || FormatStr.Length == 0) return dateStr;

            if (IsDateTime(dateStr) == false) return dateStr;

            try
            {
                return Convert.ToDateTime(dateStr).ToString(FormatStr);
            }
            catch (Exception ex)
            {
                return dateStr;
            }

            return "";
        }

        /// <summary>
        /// 时间串格式化
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="FormatStr"></param>
        /// <returns></returns>
        public static string FormatDateTime(string dateStr)
        {
            string FormatStr = "yyyy-MM-dd";
            if (dateStr.Length == 0 ) return dateStr;

            if (IsDateTime(dateStr) == false) return dateStr;

            try
            {
                return Convert.ToDateTime(dateStr).ToString(FormatStr);
            }
            catch (Exception ex)
            {
                return dateStr;
            }

            return "";
        }

        /// <summary>
        /// 时间串格式化
        /// </summary>
        /// <param name="date"></param>
        /// <param name="FormatStr"></param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime date, string FormatStr)
        {

            if (date==null ) return "";
            if (FormatStr.Length == 0) return date.ToString();
            try
            {
                return date.ToString(FormatStr);
            }
            catch (Exception ex)
            {
                return date.ToString();
            }
            return "";
        }

        #endregion

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
            if (DBTYPE.access.ToString().Equals(dbType) == true)
            {
                //新增
                if (sqlType.Equals(SQLTYPE.insert.ToString()))
                {
                    sqllist = createSql_insert_access(dt);
                }
                else if (sqlType.Equals(SQLTYPE.delete.ToString()))
                {
                    //生成删除 sql
                    string deletesql = " delete from  " + dt.TableName.Split(new char[] { '-' })[0];
                    //按村删除的地方
                    //if (dt.Columns.Contains("CZBM"))
                    //{
                    //    if (para.Length > 0)
                    //    {
                    //        deletesql = deletesql + " where CZBM in (" + para + ") ";
                    //    }
                    //}
                    //if (dt.Columns.Contains("D_JWH"))
                    //{
                    //    if (para.Length > 0)
                    //    {
                    //        deletesql = deletesql + " where D_JWH in (" + para + ") ";
                    //    }
                    //}
                    //按机构删除
                    if (dt.Columns.Contains("P_RGID"))
                    {
                        if (para.Length > 0)
                        {
                            deletesql = deletesql + " where P_RGID in (" + para + ") ";
                        }
                    }
                    if (dt.Columns.Contains("PRGID"))
                    {
                        if (para.Length > 0)
                        {
                            deletesql = deletesql + " where PRGID in (" + para + ") ";
                        }
                    }

                    sqllist.Add(deletesql);
                }
            }
            else if (DBTYPE.db2.ToString().Equals(dbType) == true)
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
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                sqltem = sql.ToString();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sqltem = sqltem.Replace("{" + dt.Columns[i].ColumnName.ToLower() + "}", dt.Rows[j][i] != null ? dt.Rows[j][i].ToString().Replace("'", "''") : "");
                }
                sqllist.Add(sqltem.Replace("0NULL", "NULL"));
            }
            return sqllist;

        }
        #endregion

        #region 取得数据字典中的值

        /// <summary>
        /// 取得数据字典中的值
        /// </summary>
        /// <param name="zdCode"></param>
        /// <param name="sqlCode"></param>
        /// <returns></returns>
        public static DataTable getsjzd(string zdCode, string sqlCode)
        {
            Form_commBll comm = new Form_commBll();
            string strWhere = "";
            if (zdCode.Length > 0)
            {
                strWhere = strWhere + string.Format(" and  zdlxbm ='{0}'  ", zdCode);
            }
            DataTable dt = comm.GetMoHuList(strWhere, sqlCode);
            return dt;
            //return null;
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
                    info.SetValue(cardId.Substring(6, 8).Insert (6,"-").Insert(4,"-"), 1);
                    info.SetValue(cardId.Substring(14, 3), 2);
                    info.SetValue(Convert.ToInt32(info[2]) % 2 != 0 ? "1" : "2", 3);
                }
            }
            else if (cardId.Length == 15)
            {
                regex = new Regex(@"^\d{15}");
                if (regex.IsMatch(cardId))
                {
                    info.SetValue(cardId.Substring(0, 6), 0);
                    info.SetValue(("19" + cardId.Substring(6, 6)).Insert(6, "-").Insert(4, "-"), 1);
                    info.SetValue(cardId.Substring(12, 3), 2);
                    info.SetValue(Convert.ToInt32(info[2]) % 2 != 0 ? "1" : "2", 3);
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
            public static string 档案信息 = "4";
            public static string 检验化验 = "5";
        }
        #endregion

        #region 上传类型
        public class UploadTYPE
        {
            public static string 健康体检表 = "0";
            public static string 中医体质辨识表 = "1";
            public static string 老年人自理能力评估表 = "2";
            public static string 档案信息 = "3";
            public static string lis信息 = "4";
        }
        #endregion

        #region 体检状态
        public class ZT
        {
            public static string 否定状态 = "0";
            public static string 确定状态 = "1";
        }
        #endregion

        #region 页面迁移前的通用处理

        /// <summary>
        /// 在页面迁移前判断页面的内容是否编辑过，如果编辑未保存过提示信息
        /// </summary>
        /// <returns></returns>
        public static bool commnoIsSaved()
        {
            //编辑过未保存
            if (CommomSysInfo.IsEdited!=null && CommomSysInfo.IsEdited.Equals("1"))
            {
                MessageBoxButtons messButton = MessageBoxButtons.YesNo;

                //是否编辑的处理
                DialogResult dr = MessageBox.Show("信息被编辑过，但没有保存，为避免信息丢失，是否先进行保存处理？", "信息编辑提示", messButton);
                if (dr == DialogResult.Yes)//如果点击“确定”按钮
                {
                    return false;
                }

                else
                {
                    CommomSysInfo.IsEdited = "0";
                    return true;
                }
            }
            return true;
        }


        #endregion



        /// <summary>
        /// 获取仪器的配置文件
        /// </summary>
        /// <param name="yqCode"></param>
        /// <returns></returns>
        public static string getyqPath(string yqCode)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo Dir = new DirectoryInfo(path);
            string filepath = string.Format("{0}\\dll\\{1}.xml", Dir.Parent .FullName, yqCode);

            if (File.Exists(filepath) == false)
            {
                filepath = "";
            }

            return filepath;
        }


        /// <summary>
        /// 获取sql的配置文件
        /// </summary>
        /// <param name="yqCode"></param>
        /// <returns></returns>
        public static string getsqlPath()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo Dir = new DirectoryInfo(path);
            string filepath = string.Format("{0}\\sql\\UserSqlConfig.xml", Dir.Parent.FullName);

            if (File.Exists(filepath) == false)
            {
                filepath = "";
            }

            return filepath;
        }
    }
}
