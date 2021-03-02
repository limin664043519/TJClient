using clientDoWebService.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace clientDoWebService.Download
{
    public class HealthExamResult:Download
    {
        public HealthExamResult(DownloadInfoModel model)
        {
            _model = model;
        }

        public static HealthExamResult Init(DownloadInfoModel model)
        {
            return new HealthExamResult(model);
        }

        public string GetExecuteSql(string tableName)
        {
            string sql_tjjg = "";
            if (!string.IsNullOrEmpty(_model.CzList.Trim()))
            {
                //如果指定机构不为空，修改此处，将czbm修改为p_rgid
                //txm表没有个人档案编号，只有健康档案号
                if (tableName.ToUpper() == "T_JK_TJRY_TXM")
                {
                    sql_tjjg = string.Format("SELECT {0}.* FROM T_JK_TJRYXX INNER JOIN {0} ON T_JK_TJRYXX.JKDAH = {0}.JKDAH " +
                                         "where T_JK_TJRYXX.yljgbm='{1}' and  T_JK_TJRYXX.prgid in ('{2}')",
                                         tableName, _model.Yljgbm, _model.CzList.Replace(",", "','"));
                }
                else
                {
                    sql_tjjg = string.Format("SELECT {0}.* FROM T_JK_TJRYXX INNER JOIN {0} ON T_JK_TJRYXX.JKDAH = {0}.D_GRDABH " +
                                         "where T_JK_TJRYXX.yljgbm='{1}' and  T_JK_TJRYXX.prgid in ('{2}')",
                                         tableName, _model.Yljgbm, _model.CzList.Replace(",", "','"));
                }

            }
            else
            {
                if (tableName.ToUpper() == "T_JK_TJRY_TXM")
                {
                    sql_tjjg = string.Format("SELECT {0}.* FROM T_JK_TJRYXX INNER JOIN {0} ON T_JK_TJRYXX.JKDAH = {0}.JKDAH where T_JK_TJRYXX.yljgbm='{1}' ",
                        tableName, _model.Yljgbm);
                }
                else
                {
                    sql_tjjg = string.Format("SELECT {0}.* FROM T_JK_TJRYXX INNER JOIN {0} ON T_JK_TJRYXX.JKDAH = {0}.D_GRDABH where T_JK_TJRYXX.yljgbm='{1}' ", 
                        tableName, _model.Yljgbm);
                }

            }
            return sql_tjjg;
        }
        public string Get()
        {
            string result = "";
            if (Check())
            {
                try
                {
                    string[] tableList = DownloadTableList(Common.DownloadType.TJJG);
                    if (tableList != null)
                    {


                        //将要生成的文件信息写入到文本文件中
                        Common.WritFileTxt(_model, GetFilePath(), "start");

                        foreach (string table in tableList)
                        {
                            List<string> fileNames = null;
                            string sql = GetExecuteSql(table);
                            string outErrMsg = "";
                            commonExcel commonexcel = new commonExcel();
                            commonexcel.OutFileToDistCheckExceedingPaginationCountAndOperation(_model.RndPrefix, _model.Yljgbm, sql, table, GetFilePath_start(""),
                                out fileNames, out outErrMsg);
                            foreach (string fileName in fileNames)
                            {
                                result = result + "|" + string.Format("{0}${1}", fileName, Common.GetFileSize(Common.GetFileAddress() + fileName));
                            }
                            //文件转移到end文件夹中
                            FileHelper.CopyDirectory(GetFilePath_start(""), GetFilePath_end(""));
                        }

                        //将要生成的文件信息写入到文本文件中
                        Common.WritFileTxt(_model, GetFilePath(), "end");
                    }
                }
                catch (Exception ex)
                {
                    //将要生成的文件信息写入到文本文件中
                    Common.WritFileTxt(_model, GetFilePath(), "error :" + ex.Message);
                    throw ex;
                }
            }
            return result;
        }
    }
}