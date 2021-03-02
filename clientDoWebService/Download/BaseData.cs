using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using clientDoWebService.common;
using clientDoWebService.Upload;

namespace clientDoWebService.Download
{
    public class BaseData:Download
    {
        public BaseData(DownloadInfoModel model)
        {
            _model = model;
        }

        public  BaseData Init(DownloadInfoModel model)
        {
            return new BaseData(model);
        }

        private string GetExecuteSql(string tableName)
        {
            string sql = "select * from {0} where 1=1 {1}";
            string strWhere = "";
            if (tableName.ToLower() == "t_jk_usersignname")
            {
                strWhere = strWhere + string.Format(" and isdelete=0 and yljgbm='{0}'", _model.Yljgbm);
            }
            return string.Format(sql,tableName,strWhere);
        }

        private string OutExcelToDiskAndReturnFilePath(DataTable dt)
        {
            string result = "";
            string fileName = GetFileName(dt.TableName);
            string outErrMsg = "";
            commonExcel commonexcel = new commonExcel();
            commonexcel.OutFileToDisk(dt.Copy(), dt.TableName, GetFilePath_start(fileName), out outErrMsg);
            return string.Format("{0}${1}", fileName,
                         Common.GetFileSize(GetFilePath_start(fileName)));
        }

        private string OutZipToDiskAndReturnFilePath(DataTable dt)
        {
            if (dt.TableName.ToLower() == "t_jk_usersignname")
            {
                string fileName = _model.RndPrefix + "_" + _model.Yljgbm + "_" + dt.TableName + ".zip";
                //string zipfilePath = System.IO.Path.Combine(Common.GetFileAddress(), fileName);
                if (Signname.Operation.ZipOperation(dt, GetFilePath_start(fileName)))
                {
                    return string.Format("{0}${1}", fileName, Common.GetFileSize(GetFilePath_start(fileName)));
                }
            }
            return "";
        }
        public string Get()
        {
            string result = "";
            if (Check())
            {
                try
                {
                    string[] tableList = DownloadTableList(Common.DownloadType.JCSJ);
                    if (tableList != null)
                    {
                        //将要生成的文件信息写入到文本文件中
                        Common.WritFileTxt(_model, GetFilePath(), "start");

                        DBOracle dboracle = new DBOracle();
                        foreach (string table in tableList)
                        {
                            DataTable dt = dboracle.ExcuteDataTable_oracle(GetExecuteSql(table));
                            dt.TableName = table;
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                result = result + "|" + OutExcelToDiskAndReturnFilePath(dt);
                                string zipFileResult = OutZipToDiskAndReturnFilePath(dt);
                                if (string.IsNullOrEmpty(zipFileResult))
                                {
                                    result = result + "|" + zipFileResult;
                                }
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
                    Common.WritFileTxt(_model, GetFilePath(), "error :" + ex.Message );
                    throw ex;
                }
            }
            return result;
        }
        
    }
}