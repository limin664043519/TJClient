using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using clientDoWebService.common;

namespace clientDoWebService.Upload
{
    public class Checker
    {
        //2017-08-09 mq 在判断客户端是否进行上传操作时加锁，只允许一个线程进入
        private static Object upLocker = new Object();
        public static bool DoFileIsExcute(string clientUser)
        {
            try
            {
                lock (upLocker)
                {
                    //获取数据处理的状态
                    DBOracle dboracle = new DBOracle();
                    //CLCXZT 处理程序状态  1停止   2运行中  
                    DataTable dt = dboracle.ExcuteDataTable_oracle("select * from T_JK_CXZT where CLCXZT='2' ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        TxtLogger.Debug(string.Format("{0}:[{1}{2}]", "DoFileIsExcute", clientUser, "程序已经启动"));
                        //程序正在执行中
                        return true;
                    }
                    else
                    {
                        //处理程序没有执行，启动处理程序
                        string sql_Insert = string.Format("insert into T_JK_CXZT(CLCXBM,CLCXSM,CLCXZT,CZY,KSSJ)values('{0}','{1}','{2}','{3}','{4}') ", clientUser, clientUser, "2", clientUser, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                        //从数据库中获取数据状态
                        //DBOracle dboracle = new DBOracle();
                        int insertResult = dboracle.ExecuteNonQuery_oracle(sql_Insert);
                        TxtLogger.Debug(string.Format("{0}:[{1}{2}]", "DoFileIsExcute", clientUser, "程序正常启动"));
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                TxtLogger.Error(string.Format("{0}:[{1}{2}]:[{3}]", "DoFileIsExcute", clientUser, "设定程序启动状态错误", ex.Message));
                throw new Exception(string.Format("DoFileIsExcute:设定程序启动状态错误：{0}", ex.Message));
            }

        }

        public static int ChangeExcuteStatus()
        {
            string sql_update = string.Format("update T_JK_CXZT set CLCXZT='1' ,KSSJ='{0}' where CLCXZT='2'  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
            //更新数据库中数据状态
            DBOracle dboracle = new DBOracle();
            return dboracle.ExecuteNonQuery_oracle(sql_update);
        }
    }
}