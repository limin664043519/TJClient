using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using clientDoWebService.common;
using Oracle.DataAccess.Client;

namespace clientDoWebService.Upload
{
    public class ExcelToDb
    {
        /// <summary>
        /// 将Excel中的内容转换为数据集合DataSet，调用数据库处理
        /// </summary>
        /// <param name="ExcelFilePathAll"></param>
        /// <returns></returns>
        public static bool DoExcelTextToDb(string ExcelFilePathAll, string GuidStr)
        {
            try
            {
                TxtLogger.Error(string.Format("{0}:[{1}]:[{2}]", "doExcelTextToDb", ExcelFilePathAll, GuidStr));
                //mq 2017-11-24上面操作语句注释，加入以下语句。
                commonExcel commonexcel = new commonExcel();
                DataSet ds = commonexcel.ExcelFileToDataSet(ExcelFilePathAll, GuidStr);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        dataToDbFromDt(dt, GuidStr);
                    }
                }
            }
            catch (Exception ex)
            {
                TxtLogger.Error(string.Format("{0}:[{1}]:[{2}]:[{3}]", "doExcelTextToDb", ExcelFilePathAll, "数据保存异常", ex.Message + ex.StackTrace));
                DBLogger.Insert(DBLogger.GetLoggerInfo(ExcelFilePathAll, ex.Message + Environment.NewLine + ex.StackTrace, GuidStr, 0));
                //throw ex;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 数据写到数据库中
        /// </summary>
        /// <param name="dtPara"></param>
        private static void dataToDbFromDt(DataTable dtPara, string GuidStr)
        {

            TxtLogger.Error(string.Format("{0}:[{1}]", "dataToDbFromDt", GuidStr));
            try
            {
                if (dtPara != null && dtPara.Rows.Count > 0)
                {
                    //每次处理的数据条数
                    int pagecount = 2000;

                    //将数据按照处理条数分次处理
                    for (int i = 0; i <= dtPara.Rows.Count / pagecount; i++)
                    {
                        DataTable dt = dtPara.Clone();
                        for (int j = i * pagecount; (j < (i + 1) * pagecount && j < dtPara.Rows.Count); j++)
                        {
                            dt.ImportRow(dtPara.Rows[j]);
                        }

                        DataColumn dtColumn = new DataColumn();
                        dtColumn.ColumnName = "DeleteGuid";
                        dtColumn.DefaultValue = GuidStr;
                        dt.Columns.Add(dtColumn);

                        TxtLogger.Debug("TableName:" + dtPara.TableName.Split(new char[] { '-' })[0]);
                        //数据库处理
                        MultiInsertData(dt, dtPara.TableName.Split(new char[] { '-' })[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数据导入到数据库中
        /// </summary>
        private static void MultiInsertData(DataTable dt, string tableName)
        {
            try
            {
                string dbFilds = "";

                if (dt == null || dt.Rows.Count == 0) return;

                // 从名称可以直接看出每个参数的含义,不在每个解释了 
                OracleParameter[] OracleParameters = new OracleParameter[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dbFilds = dbFilds + string.Format(",:{0}", dt.Columns[i].ColumnName);
                    string[] A = new string[dt.Rows.Count];
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        A[j] = dt.Rows[j][i].ToString();
                    }
                    OracleParameter Param = new OracleParameter(dt.Columns[i].ColumnName, OracleDbType.Varchar2);
                    Param.Direction = ParameterDirection.Input;
                    Param.Value = A;
                    OracleParameters[i] = Param;
                }
                DBOracle dboracle = new DBOracle();
                string sql = string.Format("insert into {0}({1}) values({2})", tableName, dbFilds.Substring(1).Replace(":", ""), dbFilds.Substring(1));
                dboracle.MultiInsertData(dt.Rows.Count, sql, OracleParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 调用存储过程将临时表中的数据添加到正式表中
        /// </summary>
        /// <param name="clientUser"></param>
        /// <param name="GuidStr"></param>
        /// <param name="yljgbm"></param>
        /// <returns></returns>
        public static bool ExecuteProCreateData(string clientUser, string GuidStr, string yljgbm)
        {
            try
            {
                // 从名称可以直接看出每个参数的含义,不在每个解释了 
                OracleParameter[] OracleParameters = new OracleParameter[2];

                OracleParameters[0] = new OracleParameter("GuidCode", OracleDbType.Varchar2);   //数据的标志
                OracleParameters[1] = new OracleParameter("v_yljgbm", OracleDbType.Varchar2); //医疗机构编码

                //指明参数是输入还是输出型
                OracleParameters[0].Direction = ParameterDirection.Input;
                OracleParameters[1].Direction = ParameterDirection.Input;

                //给参数赋值
                OracleParameters[0].Value = GuidStr;
                OracleParameters[1].Value = yljgbm;
                DBOracle dboracle = new DBOracle();
                dboracle.ExecuteProNonQuery_oracle("UploadGwtjData", OracleParameters);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("调用存储过程将临时表中的数据添加到正是表中发生异常！"+ex.Message);
                //throw ex;
            }
        }
    }
}