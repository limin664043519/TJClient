using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using clientDoWebService.common;
using clientDoWebService.Upload;

namespace clientDoWebService.Download
{
    public class ArchivesData:Download
    {
        public ArchivesData(DownloadInfoModel model)
        {
            _model = model;
        }

        public  ArchivesData Init(DownloadInfoModel model)
        {
            return new ArchivesData(model);
        }

        public string GetExecuteSql(string tableName)
        {
            DBOracle dboracle = new DBOracle();
            string str_dt_title = string.Format("select * from {0} where 1=2 ", tableName);
            TxtLogger.Debug(string.Format("{0}:[{1}{2}]", "生成档案数据表结构", "sql文", str_dt_title));
            DataTable dt_title = dboracle.ExcuteDataTable_oracle(str_dt_title);
            string strWhere = "";
            if (dt_title.Columns.Contains("P_RGID") && !string.IsNullOrEmpty(_model.CzList))
            {
                strWhere = string.Format(" and P_RGID in ('{0}')", _model.CzList.Replace(",", "','"));
            }
            else if (dt_title.Columns.Contains("PRGID") && !string.IsNullOrEmpty(_model.CzList))
            {
                strWhere = string.Format(" and PRGID in ('{0}')", _model.CzList.Replace(",", "','"));
            }

            if (dt_title.Columns.Contains("YLJGBM") && !string.IsNullOrEmpty(_model.Yljgbm))
            {
                strWhere = strWhere + string.Format(" and yljgbm ='{0}'", _model.Yljgbm);
            }
            else if (dt_title.Columns.Contains("YYBM") && !string.IsNullOrEmpty(_model.Yljgbm))
            {
                strWhere = strWhere + string.Format(" and yybm ='{0}'", _model.Yljgbm);
            }
            //按机构下载时判断结束
            string str_dt_data = string.Format("select * from {0} where 1=1 {1} ", tableName, strWhere);
            if (tableName.ToLower().Equals("t_jk_tjry_txm"))
            {
                strWhere = string.Format(" and p_rgid in ('{0}')", _model.CzList.Replace(",", "','"));
                str_dt_data = "select * from t_jk_tjry_txm join t_da_jkda_rkxzl on t_jk_tjry_txm.rkxzlid=t_da_jkda_rkxzl.id and t_jk_tjry_txm.nd='{0}' where  1=1 {1}";

                str_dt_data = string.Format(str_dt_data, DateTime.Now.Year, strWhere);
            }

            TxtLogger.Debug(string.Format("{0}:[{1}{2}]", "生成档案数据", "sql文", str_dt_data));
            return str_dt_data;
        }

        public string Get()
        {
            string result = "";
            if (Check())
            {
                try
                {
                    string[] tableList = DownloadTableList(Common.DownloadType.JKDA);
                    if (tableList != null)
                    {

                        //将要生成的文件信息写入到文本文件中
                        Common.WritFileTxt(_model, GetFilePath(), "start");

                        foreach (string table in tableList)
                        {
                            Common.AppendFileTxt(_model, GetFilePath(), table+" start");
                            List<string> fileNames = null;
                            string sql = GetExecuteSql(table);
                            string outErrMsg = "";

                            commonExcel commonexcel = new commonExcel();
                            commonexcel.OutFileToDistCheckExceedingPaginationCountAndOperation(_model.RndPrefix, _model.Yljgbm, sql, table, GetFilePath_start(""),
                                out fileNames, out outErrMsg);
                            foreach (string fileName in fileNames)
                            {
                                result = result + "|" + string.Format("{0}${1}", fileName, Common.GetFileSize(GetFilePath() + fileName));
                            }
                            Common.AppendFileTxt(_model, GetFilePath(), table + " end");
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