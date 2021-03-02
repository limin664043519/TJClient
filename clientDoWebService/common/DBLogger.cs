using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace clientDoWebService.common
{
    public class DBLogger
    {
        public static void Insert(LoggerInfo info)
        {
            ArrayList sqlList = new ArrayList();
            string sql = string.Format("insert into T_JK_CLIENTUPLOADINFOS(ip,czy,info,createdate,guid,status) " +
                                       "values('{0}','{1}','{2}','{3}','{4}',{5})",info.IP,
                info.Czy,info.Info,info.CreateDate,info.GUID,info.Status);
            sqlList.Add(sql);
            DBOracle db = new DBOracle();
            db.ExecuteNonQuery_oracle(sqlList);
        }

        public static string GetTableName(string tableNameInfo)
        {
            int pos = tableNameInfo.ToLower().IndexOf("upload");
            string result = tableNameInfo;
            if (pos > 0)
            {
                result = tableNameInfo.Substring(pos, tableNameInfo.Length - pos);
            }
            return result;
        }

        public static string GetClientIP4(string clientIpInfo)
        {
            string regex = @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}";
            Match m = Regex.Match(clientIpInfo, regex);
            if (m.Success)
            {
                return m.Value;
            }
            return "";
        }
        public static LoggerInfo GetLoggerInfo(string fileName)
        {
            string[] infos = fileName.Split(new[] { "__" }, StringSplitOptions.None);
            LoggerInfo loggerInfo = new LoggerInfo();
            if (infos.Length >= 3)
            {
                loggerInfo.TableName = GetTableName(infos[0]);
                loggerInfo.Czy = infos[1];
                loggerInfo.IP = GetClientIP4(infos[2]);
            }
            return loggerInfo;
        }

        public static LoggerInfo GetLoggerInfo(string fileName, string info,int status)
        {
            LoggerInfo loggerInfo = GetLoggerInfo(fileName);
            loggerInfo.Info = string.Format("{0} {1}",loggerInfo.TableName,info);
            loggerInfo.Status = status;
            return loggerInfo;
        }
        public static LoggerInfo GetLoggerInfo(string fileName, string info,string guid,int status)
        {
            LoggerInfo loggerInfo = GetLoggerInfo(fileName,info,status);
            loggerInfo.GUID = guid;
            return loggerInfo;
        }

        private static DataTable GetLoggerInfos(string czy)
        {
            string sql = string.Format("select ip,czy,info,createdate,guid,status from T_JK_CLIENTUPLOADINFOS where czy='{0}' " +
                                       "and to_char(to_date(createdate,'yyyy-mm-dd hh24:mi:ss'),'dd')=to_char(sysdate,'dd') " +
                                       "order by to_date(createdate,'yyyy-mm-dd hh24:mi:ss') asc,rowid asc", czy);
            DBOracle db = new DBOracle();
            DataTable dt = db.ExcuteDataTable_oracle(sql);
            return dt;
        }

        public static List<LoggerInfo> Get(string czy)
        {
            DataTable dt = GetLoggerInfos(czy);
            List<LoggerInfo> infos = new List<LoggerInfo>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    infos.Add(new LoggerInfo()
                    {
                        IP=row["ip"].ToString(),
                        CreateDate=row["createdate"].ToString(),
                        Czy=row["czy"].ToString(),
                        GUID=row["guid"].ToString(),
                        Info =row["info"].ToString(),
                        Status = int.Parse(row["status"].ToString())
                    });
                }
            }
            return infos;
        }
    }

    public class LoggerInfo
    {
        public LoggerInfo()
        {
            
        }

        public LoggerInfo(string czy,string info,int status)
        {
            Czy = czy;
            Info = info;
            Status = status;
        }
        private string _currentDateTime = "";
        public string GetCurrentDateTime()
        {
            if (_currentDateTime == "")
            {
                return DateTime.Now.ToString();
            }
            return _currentDateTime;
        }
        public string TableName { set; get; }
        public string IP { set; get; }
        public string Czy { set; get; }
        public string Info { set; get; }
        public string GUID { set; get; }
        public int Status { set; get; }
        public string CreateDate
        {
            set { _currentDateTime = value; }
            get { return GetCurrentDateTime(); }
        }
    }
}