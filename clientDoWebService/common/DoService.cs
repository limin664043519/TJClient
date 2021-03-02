using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.IO;
using System.Logger;
using clientDoWebService.Download;
using Oracle.DataAccess.Client;

namespace clientDoWebService.common
{
    public class DoService
    {
        /// <summary>
        /// 书写日志对象
        /// </summary>
        public SimpleLogger logger = SimpleLogger.GetInstance();

        /// <summary>
        /// 文件本地保存路径
        /// </summary>
        public string fileDirAddress = "";

        #region 下载数据
        //客户端下载
        /// <summary>
        /// 客户端下载处理
        /// </summary>
        /// <param name="yljgbm">医疗机构编码</param>
        /// <param name="czList">村庄编码列表 多个村庄用【，】分割</param>
        /// <param name="dataType">数据类型 1：基础数据 2:档案数据 3:体检数据  用【，】分割</param>
        /// <returns></returns>
       public string DoDownLoadInfo(string rndPrefix,string yljgbm, string czList, string dataType)
        {
            #region 变量声明
            //信息记录
            string errMsg = "";

            //是否中断处理 false:不中断处理  true:中断处理
            bool isBreakExe = false;

            //基础数据表列表
            string[] jcsj_tabList = null;

            //档案数据表列表
            string[] da_tabList = null;

            //上次体检结果数据表列表
            string[] tjjg_tabList = null;

            //服务端的下载地址
            string FileDowLoadUrl = ConfigurationManager.AppSettings["FileDowLoadUrl"];

           //服务端文件保存地址
            string FileAddress = ConfigurationManager.AppSettings["FileAddress"];

            //要下载的文件列表
            string fileNameList = "";

            
            //返回的结果 1：正常结束  0:异常结束
            string resultstr = "";
            #endregion
            try
            {

                #region 参数验证

                //医疗机构编码
                if (yljgbm!=null && yljgbm.Trim().Length == 0)
                {
                    errMsg = errMsg + "|" + "医疗机构编码不能为空 ";
                    isBreakExe = true;
                }

                //数据类型验证
                if (isBreakExe == false && dataType !=null && dataType.Trim().Length == 0)
                {
                    errMsg = errMsg + "|" + "没有选择要下载的数据类型 ";
                    isBreakExe = true;
                }
                else
                {
                    //数据类型处理
                    string[] dataTypeList = dataType.Split(new char[] { ',' });
                    if (dataTypeList.Length > 0)
                    {
                        for (int i = 0; i < dataTypeList.Length; i++)
                        {
                            //基础数据
                            if (dataTypeList[i].Equals("1"))
                            {
                                string jcsjStr = ConfigurationManager.AppSettings["JCSJ"].ToString();
                                jcsj_tabList = getLIstFromStr(jcsjStr, ',');
                            }
                            else if (dataTypeList[i].Equals("2"))//档案数据
                            {
                                string jkdaStr = ConfigurationManager.AppSettings["JKDA"].ToString();
                                da_tabList = getLIstFromStr(jkdaStr, ',');
                            }
                            else if (dataTypeList[i].Equals("3"))//体检数据
                            {
                                string jcsjStr = ConfigurationManager.AppSettings["TJJG"].ToString();
                                tjjg_tabList = getLIstFromStr(jcsjStr, ',');
                            }
                        }
                    }
                }


                if (isBreakExe)
                {
                    return string.Format("0-{0}", errMsg);
                }
                #endregion

                #region 基础数据
                //基础数据在修改按机构下载时未动，因为strWhere未使用
                //DataSet ds_jcsj = new DataSet();
                if (jcsj_tabList != null && jcsj_tabList.Length > 0)
                {
                    //取得基础数据
                    DBOracle dboracle = new DBOracle();
                    string outErrMsg = "";

                    for (int i = 0; i < jcsj_tabList.Length; i++)
                    {
                        string str_dt_title = string.Format("select * from {0} where 1=2 ", jcsj_tabList[i]);
                        logger.Debug(string.Format("{0}:[{1}{2}]", "生成基础数据表结构", "sql文", str_dt_title));
                        DataTable dt_title = dboracle.ExcuteDataTable_oracle(str_dt_title);
                        string strWhere = "";  //strWhere并未使用
                        //注释的为以村进行下载进行筛选
                        //if (dt_title.Columns.Contains("D_JWH") == true && czList != null && czList.Length > 0)
                        //{
                        //    strWhere = string.Format(" and D_JWH in ('{0}')", czList.Replace(",", "','"));
                        //}
                        //else if (dt_title.Columns.Contains("CZBM") == true && czList !=null && czList.Length > 0)
                        //{
                        //    strWhere = string.Format(" and CZBM in ('{0}')", czList.Replace(",", "','"));
                        //}
                        //注释结束
                        ////下面为以机构进行下载
                        //if (dt_title.Columns.Contains("P_RGID") == true && czList != null && czList.Length > 0)
                        //{
                        //    strWhere = string.Format(" and P_RGID in ('{0}')", czList.Replace(",", "','"));
                        //}
                        ////以机构下载结束

                        //if (dt_title.Columns.Contains("YLJGBM") == true && yljgbm.Length > 0)
                        //{
                        //    strWhere = strWhere + string.Format(" and yljgbm ='{0}'", yljgbm);
                        //}
                        //else if (dt_title.Columns.Contains("YYBM") == true && yljgbm.Length > 0)
                        //{
                        //    strWhere = strWhere + string.Format(" and yybm ='{0}'", yljgbm);
                        //}
                        //用户签名表，只取出没有删除的
                        if (jcsj_tabList[i].ToLower() == "t_jk_usersignname")
                        {
                            strWhere = strWhere + string.Format(" and isdelete=0 and yljgbm='{0}'",yljgbm);
                        }
                        DataTable dt = dboracle.ExcuteDataTable_oracle(string.Format("select * from {0} where 1=1 {1}", 
                            jcsj_tabList[i],strWhere));
                        dt.TableName = jcsj_tabList[i];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //ds_jcsj.Tables.Add(dt.Copy());
                            //基础数据转换为excel
                            string fileName = rndPrefix + "_" + yljgbm + "_" + jcsj_tabList[i] + ".xlsx";
                            commonExcel commonexcel = new commonExcel();
                            bool result = commonexcel.OutFileToDisk(dt.Copy(), jcsj_tabList[i], FileAddress + fileName, out outErrMsg);

                            fileNameList = fileNameList + "|" + string.Format("{0}${1}", fileName, getFileSize(FileAddress + fileName));
                            //在此处，如果是签名表，还要对应着打包图片
                            if (jcsj_tabList[i].ToLower() == "t_jk_usersignname")
                            {
                                fileName = rndPrefix + "_" + yljgbm + "_" + jcsj_tabList[i] + ".zip";
                                string zipfilePath = Path.Combine(FileAddress,fileName);
                                if (Signname.Operation.ZipOperation(dt, zipfilePath))
                                {
                                    fileNameList = fileNameList + "|" + string.Format("{0}${1}",fileName,getFileSize(zipfilePath));
                                };
                            }
                        }
                    }
                }

                #endregion

                #region 档案数据

                if (da_tabList != null && da_tabList.Length > 0)
                {
                    //取得基础数据
                    DBOracle dboracle = new DBOracle();
                    string outErrMsg = "";

                    for (int i = 0; i < da_tabList.Length; i++)
                    {
                        string str_dt_title = string.Format("select * from {0} where 1=2 ", da_tabList[i]);
                        logger.Debug(string.Format("{0}:[{1}{2}]", "生成档案数据表结构", "sql文", str_dt_title));
                        DataTable dt_title = dboracle.ExcuteDataTable_oracle(str_dt_title);
                        string strWhere = "";
                        //按村下载时判断
                        //if (dt_title.Columns.Contains("D_JWH") == true && czList != null && czList.Length > 0)
                        //{
                        //    strWhere = string.Format(" and D_JWH in ('{0}')", czList.Replace (",","','"));
                        //}
                        //else if (dt_title.Columns.Contains("CZBM") == true && czList != null && czList.Length > 0)
                        //{
                        //    strWhere = string.Format(" and CZBM in ('{0}')", czList.Replace(",", "','"));
                        //}
                        //按村下载时判断结束
                        //按机构下载时判断,受限判断是否包含P_RGID
                        if (dt_title.Columns.Contains("P_RGID") == true && czList != null && czList.Length > 0)
                        {
                            strWhere = string.Format(" and P_RGID in ('{0}')", czList.Replace(",", "','"));
                        }else if(dt_title.Columns.Contains("PRGID") == true && czList != null && czList.Length > 0)
                        {
                            strWhere = string.Format(" and PRGID in ('{0}')", czList.Replace(",", "','"));
                        }

                        if(dt_title.Columns.Contains("YLJGBM") == true && yljgbm.Length > 0)
                        {
                            strWhere = strWhere+string.Format(" and yljgbm ='{0}'", yljgbm);
                        }
                        else if (dt_title.Columns.Contains("YYBM") == true && yljgbm.Length > 0)
                        {
                            strWhere = strWhere + string.Format(" and yybm ='{0}'", yljgbm);
                        }
                        //按机构下载时判断结束
                        string str_dt_data = string.Format("select * from {0} where 1=1 {1} ", da_tabList[i], strWhere);
                        if (da_tabList[i].ToLower().Equals("t_jk_tjry_txm") == true)
                        {
                            
                            //strWhere = string.Format(" and D_JWH in ('{0}')", czList.Replace(",", "','"));
                            strWhere = string.Format(" and p_rgid in ('{0}')", czList.Replace(",", "','"));
                            str_dt_data = "select * from t_jk_tjry_txm join t_da_jkda_rkxzl on t_jk_tjry_txm.rkxzlid=t_da_jkda_rkxzl.id and t_jk_tjry_txm.nd='{0}' where  1=1 {1}";

                            str_dt_data = string.Format(str_dt_data, DateTime.Now.Year.ToString(), strWhere);
                        }

                        logger.Debug(string.Format("{0}:[{1}{2}]", "生成档案数据", "sql文", str_dt_data));
                        //在这里判断数量，如果数量大于web.config中的指定数量，就分页取数据。
                        //string fileName =rndPrefix+"_" + yljgbm + "_" + da_tabList[i] + ".xls";
                        //bool result = commonExcel.OutFileToDistCheckExceedingPaginationCountAndOperation(str_dt_data, da_tabList[i],fileName, FileAddress + fileName, out outErrMsg);
                        //fileNameList = fileNameList + "|" + string.Format("{0}${1}", fileName, getFileSize(FileAddress + fileName));
                        
                        List<string> fileNames = null;
                        commonExcel commonexcel = new commonExcel();
                        bool result = commonexcel.OutFileToDistCheckExceedingPaginationCountAndOperation(rndPrefix, yljgbm, str_dt_data, da_tabList[i], FileAddress,
                            out fileNames, out outErrMsg);
                        foreach (string fileName in fileNames)
                        {
                            fileNameList = fileNameList + "|" + string.Format("{0}${1}", fileName, getFileSize(FileAddress + fileName));
                        }
                       
                        #region 以下方法是原方法，先读入数据再使用分页的数量保存数据到sheet中
                        //以下方法是原方法，先读入数据再使用分页的数量保存数据到sheet中
                        //DataTable dt = dboracle.ExcuteDataTable_oracle(str_dt_data);
                        //dt.TableName = da_tabList[i].ToString();
                        //if (dt != null && dt.Rows.Count > 0)
                        //{
                        //    //基础数据转换为excel


                        //    bool result = commonExcel.OutFileToDisk(dt.Copy(),da_tabList[i].ToString(), FileAddress + fileName, out outErrMsg);

                        //    fileNameList = fileNameList + "|" + string.Format("{0}${1}", fileName, getFileSize(FileAddress + fileName));
                        //}
                        #endregion
                        
                    }
                }
                #endregion

                #region 体检结果数据
                if (tjjg_tabList != null && tjjg_tabList.Length > 0)
                {
                    //取得基础数据
                    DBOracle dboracle = new DBOracle();
                    string outErrMsg = "";

                    for (int i = 0; i < tjjg_tabList.Length; i++)
                    {
                        DataTable dt = null;
                        string sql_tjjg = "";
                        
                        if (czList != null && czList.Length > 0)
                        {
                            //如果指定机构不为空，修改此处，将czbm修改为p_rgid
                            //sql_tjjg = string.Format("SELECT {0}.* FROM T_JK_TJRYXX INNER JOIN {0} ON T_JK_TJRYXX.JKDAH = {0}.D_GRDABH where T_JK_TJRYXX.yljgbm='{1}' and  T_JK_TJRYXX.czbm in ('{2}')", tjjg_tabList[i], yljgbm, czList.Replace(",", "','"));
                            //txm表没有个人档案编号，只有健康档案号
                            if (tjjg_tabList[i] == "T_JK_TJRY_TXM")
                            {
                                sql_tjjg = string.Format("SELECT {0}.* FROM T_JK_TJRYXX INNER JOIN {0} ON T_JK_TJRYXX.JKDAH = {0}.JKDAH " +
                                                     "where T_JK_TJRYXX.yljgbm='{1}' and  T_JK_TJRYXX.prgid in ('{2}')", 
                                                     tjjg_tabList[i], yljgbm, czList.Replace(",", "','"));
                            }
                            else
                            {
                                sql_tjjg = string.Format("SELECT {0}.* FROM T_JK_TJRYXX INNER JOIN {0} ON T_JK_TJRYXX.JKDAH = {0}.D_GRDABH " +
                                                     "where T_JK_TJRYXX.yljgbm='{1}' and  T_JK_TJRYXX.prgid in ('{2}')", 
                                                     tjjg_tabList[i], yljgbm, czList.Replace(",", "','"));
                            }
                            
                        }
                        else
                        {
                            if (tjjg_tabList[i] == "T_JK_TJRY_TXM")
                            {
                                sql_tjjg = string.Format("SELECT {0}.* FROM T_JK_TJRYXX INNER JOIN {0} ON T_JK_TJRYXX.JKDAH = {0}.JKDAH where T_JK_TJRYXX.yljgbm='{1}' ",
                                    tjjg_tabList[i], yljgbm);
                            }
                            else
                            {
                                sql_tjjg = string.Format("SELECT {0}.* FROM T_JK_TJRYXX INNER JOIN {0} ON T_JK_TJRYXX.JKDAH = {0}.D_GRDABH where T_JK_TJRYXX.yljgbm='{1}' ", tjjg_tabList[i], yljgbm);
                            }
                            
                        }
                        logger.Debug(string.Format("{0}:[{1}{2}]", "上次体检结果", "sql文", sql_tjjg));

                        //在这里判断数量，如果数量大于web.config中的指定数量，就分页取数据。
                        //string fileName = rndPrefix + "_" + yljgbm + "_" + tjjg_tabList[i] + ".xls";
                        //bool result = commonExcel.OutFileToDistCheckExceedingPaginationCountAndOperation(sql_tjjg, tjjg_tabList[i], fileName, 
                        //    FileAddress + fileName, out outErrMsg);
                        //fileNameList = fileNameList + "|" + string.Format("{0}${1}", fileName, getFileSize(FileAddress + fileName));

                        List<string> fileNames = null;
                        commonExcel commonexcel = new commonExcel();
                        bool result = commonexcel.OutFileToDistCheckExceedingPaginationCountAndOperation(rndPrefix, yljgbm, sql_tjjg, tjjg_tabList[i],
                            FileAddress, out fileNames, out outErrMsg);
                        foreach (string fileName in fileNames)
                        {
                            fileNameList = fileNameList + "|" + string.Format("{0}${1}", fileName, getFileSize(FileAddress + fileName));
                        }

                        #region 过去使用的方法，分页出现问题
                        //dt = dboracle.ExcuteDataTable_oracle(sql_tjjg);

                        //dt.TableName = tjjg_tabList[i];
                        //if (dt != null && dt.Rows.Count > 0)
                        //{
                        //    //基础数据转换为excel
                        //    string fileName = rndPrefix + "_" + yljgbm + "_" + tjjg_tabList[i] + ".xls";

                        //    bool result = commonExcel.OutFileToDisk(dt.Copy(), tjjg_tabList[i], FileAddress + fileName, out outErrMsg);

                        //    fileNameList = fileNameList + "|" + string.Format("{0}${1}", fileName, getFileSize(FileAddress + fileName));
                        //    //ArrayList fileListArray= commonExcel.OutFileToDiskAll(dt.Copy(), tjjg_tabList[i].ToString(), FileAddress, out outErrMsg);
                        //    //if (fileListArray != null && fileListArray.Count > 0)
                        //    //{
                        //    //    for (int fileIndex = 0; fileIndex < fileListArray.Count; fileIndex++)
                        //    //    {
                        //    //        fileNameList = fileNameList + "|" + string.Format("{0}${1}", Path.GetFileName(fileListArray[fileIndex].ToString()), getFileSize(fileListArray[fileIndex].ToString()));
                        //    //    }
                        //    //}
                        //}
                        #endregion
                        
                    }
                }
                #endregion

                resultstr = string.Format("1-{0}-{1}", FileDowLoadUrl, fileNameList);

            }
            catch (Exception ex)
            {
                resultstr = string.Format("0-{0}", ex.Message);
            }

            return resultstr;

        }
        
        #region  共同处理
        /// <summary>
        /// 按照格式字符串获取list数组
        /// </summary>
        /// <param name="paraStr">需要分割的字符串</param>
        /// <param name="splitChar">字符串分隔符</param>
        /// <returns></returns>
        public string[]  getLIstFromStr(string paraStr, char splitChar)
        {

            if (paraStr.Trim().Length == 0 || splitChar.ToString().Length == 0)
            {
                return null;
            }

            //分割字符串
            string[] paraStrList = paraStr.Split(new char[] { splitChar });
            if (paraStrList.Length > 0)
            {
                return paraStrList;
            }
            return null;
        }

        /// <summary>
        /// 获取文件的大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string getFileSize(string filePath)
        {
            if (File.Exists(filePath) == true)
            {
                FileInfo fileinfo = new FileInfo(filePath);
                return (fileinfo.Length/1024).ToString();
            }
            return "0";
        }



        #endregion
        #endregion

        #region 客户端上传后的处理
        //2017-08-09 mq 在判断客户端是否进行上传操作时加锁，只允许一个线程进入
        private static Object upLocker = new Object();
        /// <summary>
        /// 是否启动文件执行的处理
        /// </summary>
        /// <param name="threadid"></param>
        public bool DoFileIsExcute(string clientUser)
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
                        logger.Debug(string.Format("{0}:[{1}{2}]", "DoFileIsExcute", clientUser, "程序已经启动"));
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

                        logger.Debug(string.Format("{0}:[{1}{2}]", "DoFileIsExcute", clientUser, "程序正常启动"));
                        return false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}:[{1}{2}]:[{3}]", "DoFileIsExcute",clientUser, "设定程序启动状态错误", ex.Message ));
                throw new Exception(string.Format("DoFileIsExcute:设定程序启动状态错误：{0}", ex.Message));
            }

        }

        #region 将文件内容处理到数据库中

        /// <summary>
        /// 执行文件的处理
        /// </summary>
        /// <param name="threadid"></param>
        public void DoFile(string clientUser, string GuidStr, string yljgbm)
        {
            TxtLogger.Debug(string.Format("{0}:[{1}]:[{2}]", "DoFile", clientUser, "文件处理开始"));
            //取得本地的保存文件地址
            fileDirAddress = ConfigurationManager.AppSettings["ExcelUpFile"];
            string filePathAll = string.Format("{0}{1}", fileDirAddress, "upLoade");
            string filePathAllTo = string.Format("{0}{1}", fileDirAddress, "excute");
            try
            {
                //文件处理
                if (copyDirectory(filePathAll, filePathAllTo) == true)
                {
                    //将文件内容保存到数据库中
                    DoFileToDb(filePathAllTo, GuidStr, yljgbm, clientUser);

                    //处理完当前数据后再次扫描是否存在需要处理的文件
                    DoFile(clientUser, GuidStr, yljgbm);
                }
                else
                {
                    //调用存储过程将临时表中的数据处理到正式表中
                    //ExecuteProCreateData(clientUser, GuidStr, yljgbm);

                    //ExecuteProCreateData(clientUser, GuidStr, yljgbm);

                    ////处理程序执行结束，关闭处理程序
                    //string sql_update = string.Format("update T_JK_CXZT set CLCXZT='1' ,KSSJ='{0}' where CLCXZT='2'  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                    ////更新数据库中数据状态
                    //DBOracle dboracle = new DBOracle();
                    //int insertResult = dboracle.ExecuteNonQuery_oracle(sql_update);
                    //return;
                }
                logger.Debug(string.Format("{0}:[{1}]:[{2}]", "DoFile", clientUser, "文件处理结束"));
                DBLogger.Insert(new LoggerInfo(clientUser, "DoFile 文件处理结束", 1));
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}:[{1}]:[{2}]:[{3}]", "DoFile", clientUser, "文件处理异常", ex.Message));

                DBLogger.Insert(new LoggerInfo(clientUser, string.Format("DoFile文件处理异常,{0}", ex.Message), 0));
                throw ex;
                //出错时增加错误处理

                ////处理程序执行结束，关闭处理程序
                //string sql_update = string.Format("insert into   ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                ////更新数据库中数据状态
                //DBOracle dboracle = new DBOracle();
                //int insertResult = dboracle.ExecuteNonQuery_oracle(sql_update);

                //throw new Exception(string.Format("DoFile文件处理异常：{0}", ex.Message));
            }
            finally
            {
                //处理程序执行结束，关闭处理程序
                string sql_update = string.Format("update T_JK_CXZT set CLCXZT='1' ,KSSJ='{0}' where CLCXZT='2'  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                //更新数据库中数据状态
                DBOracle dboracle = new DBOracle();
                int insertResult = dboracle.ExecuteNonQuery_oracle(sql_update);
            }
            return;
               
        }

        /// <summary>
        /// 调用存储过程将临时表中的数据添加到正式表中
        /// </summary>
        /// <param name="clientUser"></param>
        /// <param name="GuidStr"></param>
        /// <param name="yljgbm"></param>
        /// <returns></returns>
        public bool ExecuteProCreateData(string clientUser, string GuidStr, string yljgbm)
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
                //throw new Exception("调用存储过程将临时表中的数据添加到正是表中发生异常！"+ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 2017-08-14 mq 修改，方法比原方法加入yljgbm
        /// 遍历目录下的所有文件，将每个文件内容保存到数据库的临时表中，然后立即导入正式表中后删除
        /// </summary>
        /// <param name="DirectoryPath">文件夹路径</param>
        private bool DoFileToDb(string DirectoryPath, string GuidStr,string yljgbm,string clientUser)
        {

            bool isExistsFile = false;
            int fileCount = 0;
            string errorFile = "";
            try
            {
                //备份已处理的文件的路径
                string filePathBack = ConfigurationManager.AppSettings["ExcelUpFile"] + "backup\\";
                string filePathBackError = ConfigurationManager.AppSettings["ExcelUpFile"] + "Error\\";

                //处理文件内容到数据库中
                DirectoryInfo DirectoryArray = new DirectoryInfo(DirectoryPath);

                //获取该文件夹下的文件列表
                FileInfo[] Files = DirectoryArray.GetFiles();

                //获取该文件夹下的文件夹列表 
                DirectoryInfo[] Directorys = DirectoryArray.GetDirectories();

                //逐个处理文件 
                foreach (FileInfo theFile in Files)
                {
                    errorFile = theFile.Name;
                    //将数据处理到数据库中
                    bool result = doExcelTextToDb(theFile.FullName, GuidStr);
                    
                    if (result == true)  //为true时，导入临时表成功
                    {

                        DBLogger.Insert(DBLogger.GetLoggerInfo(theFile.Name, "导入临时表成功", GuidStr,1));
                        
                        //如果目录不存在创建目录
                        if (!Directory.Exists(filePathBack)) Directory.CreateDirectory(filePathBack);

                        //将处理的文件进行备份
                        if (File.Exists(filePathBack + Path.GetFileName(theFile.FullName)) == true)
                        {
                            File.Delete(filePathBack + Path.GetFileName(theFile.FullName));
                        }
                        if (File.Exists(theFile.FullName) == true)
                        {
                            File.Move(theFile.FullName, filePathBack + Path.GetFileName(theFile.FullName));
                        }
                    }
                    else
                    {
                        DBLogger.Insert(DBLogger.GetLoggerInfo(theFile.Name, "导入临时表失败", GuidStr,0));
                        //如果目录不存在创建目录
                        if (!Directory.Exists(filePathBackError)) Directory.CreateDirectory(filePathBackError);
                        //将处理的文件进行备份
                        if (File.Exists(filePathBackError + Path.GetFileName(theFile.FullName)) == true)
                        {
                            File.Delete(filePathBackError + Path.GetFileName(theFile.FullName));
                        }

                        if (File.Exists(theFile.FullName) == true)
                        {
                            File.Move(theFile.FullName, filePathBackError + Path.GetFileName(theFile.FullName));
                        }
                    }
                    //在这里记录导入临时表的信息

                    if (result == true)
                    {
                        //如果文件导入成功，立即执行临时表到正式表操作
                        ExecuteProCreateData(clientUser, GuidStr, yljgbm);
                        //记录到正式表的信息
                        DBLogger.Insert(DBLogger.GetLoggerInfo(theFile.Name, "导入正式表成功", GuidStr,1));
                    }
                }

                //逐个获取文件夹名称，并递归调用方法本身  
                foreach (DirectoryInfo Dir in Directorys)
                {
                    bool result = copyDirectory(DirectoryPath + "\\" + Dir.Name, filePathBack + "\\" + Dir.Name);
                    if (result == true)
                    {
                        isExistsFile = true;
                    }
                }

                //删除处理完的文件目录
                //Directory.Delete(DirectoryPath, false);

                //只要存在要处理的文件就返回true
                if (fileCount > 0)
                {
                    isExistsFile = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}:[{1}]:[{2}]", "DoFileToDb", "数据保存异常", ex.Message));
                DBLogger.Insert(DBLogger.GetLoggerInfo(errorFile, ex.Message, GuidStr, 0));
                return false;
            }
            return isExistsFile;

        }

        /// <summary>
        /// 遍历目录下的所有文件，将文件内容保存到数据库中
        /// </summary>
        /// <param name="DirectoryPath">文件夹路径</param>
        private bool DoFileToDb(string DirectoryPath, string GuidStr)
        {

            bool isExistsFile = false;
            int fileCount = 0;
            try
            {
                //备份已处理的文件的路径
                string filePathBack = ConfigurationManager.AppSettings["ExcelUpFile"] + "backup\\";
                string filePathBackError = ConfigurationManager.AppSettings["ExcelUpFile"] + "Error\\";

                //处理文件内容到数据库中
                DirectoryInfo DirectoryArray = new DirectoryInfo(DirectoryPath);

                //获取该文件夹下的文件列表
                FileInfo[] Files = DirectoryArray.GetFiles();

                //获取该文件夹下的文件夹列表 
                DirectoryInfo[] Directorys = DirectoryArray.GetDirectories();

                //逐个处理文件 
                foreach (FileInfo theFile in Files)
                {
                    //将数据处理到数据库中
                    bool result = doExcelTextToDb(theFile.FullName, GuidStr);
                    if (result == true)
                    {
                        //这里加入对单个文件处理
                        //ExecuteProCreateData(clientUser, GuidStr, yljgbm);
                        //如果目录不存在创建目录
                        if (!Directory.Exists(filePathBack)) Directory.CreateDirectory(filePathBack);

                        //将处理的文件进行备份
                        if (File.Exists(filePathBack + Path.GetFileName(theFile.FullName)) == true)
                        {
                            File.Delete(filePathBack + Path.GetFileName(theFile.FullName));
                        }
                        if (File.Exists(theFile.FullName) == true)
                        {
                            File.Move(theFile.FullName, filePathBack + Path.GetFileName(theFile.FullName));
                        }
                    }
                    else
                    {
                        //如果目录不存在创建目录
                        if (!Directory.Exists(filePathBackError)) Directory.CreateDirectory(filePathBackError);
                        //将处理的文件进行备份
                        if (File.Exists(filePathBackError + Path.GetFileName(theFile.FullName)) == true)
                        {
                            File.Delete(filePathBackError + Path.GetFileName(theFile.FullName));
                        }

                        if (File.Exists(theFile.FullName) == true )
                        {
                            File.Move(theFile.FullName, filePathBackError + Path.GetFileName(theFile.FullName));
                        }
                    }
                }

                //逐个获取文件夹名称，并递归调用方法本身  
                foreach (DirectoryInfo Dir in Directorys)
                {
                    bool result = copyDirectory(DirectoryPath + "\\" + Dir.Name, filePathBack + "\\" + Dir.Name);
                    if (result == true)
                    {
                        isExistsFile = true;
                    }
                }

                //删除处理完的文件目录
                //Directory.Delete(DirectoryPath, false);

                //只要存在要处理的文件就返回true
                if (fileCount > 0)
                {
                    isExistsFile = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}:[{1}]:[{2}]", "DoFileToDb", "数据保存异常", ex.Message));
                return false;
            }
                return isExistsFile;
            
        }

        /// <summary>
        /// 将Excel中的内容转换为数据集合DataSet，调用数据库处理
        /// </summary>
        /// <param name="ExcelFilePathAll"></param>
        /// <returns></returns>
        public bool doExcelTextToDb(string ExcelFilePathAll, string GuidStr)
        {
            //处理Excel文件
            //DataSet ds_result=new DataSet();
            //DataSet ds_result=null;
            //string ErrMsg = "";
            try
            {
                logger.Error(string.Format("{0}:[{1}]:[{2}]", "doExcelTextToDb", ExcelFilePathAll, GuidStr));
                //excel转换为dataset
                //bool result = commonExcel.ExcelFileToDataSet(ExcelFilePathAll,GuidStr,out ds_result, out ErrMsg);
                //if (result == true)
                //{
                //    if (ds_result != null && ds_result.Tables.Count > 0)
                //    {
                //        //存在数据的情况下将数据保存到数据库中
                //        for (int i = 0; i < ds_result.Tables.Count; i++)
                //        {
                //            dataToDbFromDt(ds_result.Tables[i], GuidStr);
                //        }
                //    }
                //}
                //mq 2017-11-24上面操作语句注释，加入以下语句。
                commonExcel commonexcel = new commonExcel();
                DataSet ds = commonexcel.ExcelFileToDataSet(ExcelFilePathAll, GuidStr);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        dataToDbFromDt(dt,GuidStr);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}:[{1}]:[{2}]:[{3}]", "doExcelTextToDb", ExcelFilePathAll, "数据保存异常", ex.Message+ex.StackTrace));
                DBLogger.Insert(DBLogger.GetLoggerInfo(ExcelFilePathAll, ex.Message+Environment.NewLine+ex.StackTrace, GuidStr, 0));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 数据写到数据库中
        /// </summary>
        /// <param name="dtPara"></param>
        private void dataToDbFromDt(DataTable dtPara, string GuidStr)
        {

            logger.Error(string.Format("{0}:[{1}]", "dataToDbFromDt", GuidStr));
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

                        logger.Debug("TableName:" + dtPara.TableName.Split(new char[] { '-' })[0]);
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
        private void MultiInsertData(DataTable dt, string tableName)
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
        #endregion

        #region 文件复制
        /// <summary>
        /// 复制文件夹中的所有文件到指定文件夹
        /// </summary>
        /// <param name="DirectoryPath">源文件夹路径</param>
        /// <param name="DirAddress">保存路径</param>
        private bool copyDirectory(string DirectoryPath, string DirAddress)//复制文件夹，
        {
            //是否存在要处理的文件
            bool isExistsFile = false;

            //要处理的文件数量
            int fileCount = 0;
            try
            {
                if (!Directory.Exists(DirAddress)) Directory.CreateDirectory(DirAddress);

                DirectoryInfo DirectoryArray = new DirectoryInfo(DirectoryPath);
                FileInfo[] Files = DirectoryArray.GetFiles();//获取该文件夹下的文件列表
                DirectoryInfo[] Directorys = DirectoryArray.GetDirectories();//获取该文件夹下的文件夹列表 
                foreach (FileInfo theFile in Files)//逐个复制文件     
                {
                    //如果临时文件夹下存在与应用程序所在目录下的文件同名的文件，则删除应用程序目录下的文件   
                    if (File.Exists(DirAddress + "\\" + Path.GetFileName(theFile.FullName)))
                    {
                        //try
                        //{
                            File.Delete(DirAddress + "\\" + Path.GetFileName(theFile.FullName));
                            //将临时文件夹的文件移到应用程序所在的目录下   
                            File.Move(theFile.FullName, DirAddress + "\\" + Path.GetFileName(theFile.FullName));
                            fileCount++;
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                    }
                    else
                    {
                        //将临时文件夹的文件移到应用程序所在的目录下   
                        File.Move(theFile.FullName, DirAddress + "\\" + Path.GetFileName(theFile.FullName));
                        fileCount++;
                    }
                }
                ////删除处理完的文件目录
                //Directory.Delete(DirectoryPath, false);

                foreach (DirectoryInfo Dir in Directorys)//逐个获取文件夹名称，并递归调用方法本身     
                {
                    bool result = copyDirectory(DirectoryPath + "\\" + Dir.Name, DirAddress + "\\" + Dir.Name);
                    if (result == true)
                    {
                        isExistsFile = true;
                    }
                }

                //只要存在要处理的文件就返回true
                if (fileCount > 0)
                {
                    isExistsFile = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}:[{1}{2}]:[{3}]:[{4}]", "copyDirectory", DirectoryPath, 
                    "设定程序启动状态错误", ex.Message,ex.StackTrace));
                //throw new Exception("copyDirectory",ex);
                return false;
            }
            return isExistsFile;
        }


        /// <summary>
        /// 复制文件夹中的所有文件到指定文件夹
        /// </summary>
        /// <param name="DirectoryPath">源文件夹路径</param>
        /// <param name="DirAddress">保存路径</param>
        public void copyDirectoryOneFile(string DirectoryPath, string DirAddress)//复制文件夹，
        {
            //保存文件的路径
            string filePathTo = Path.GetDirectoryName(DirAddress);

            //保存文件的文件名称
            string fileName = Path.GetFileName(DirAddress);

            try
            {
                //文件目录的处理
                if (!Directory.Exists(filePathTo)) Directory.CreateDirectory(filePathTo);

                //获取要复制的文件
                if (File.Exists(DirectoryPath) == true)
                {
                    //取得要copy的文件
                    FileInfo fileinfo = new FileInfo(DirectoryPath);
                    if (File.Exists(DirAddress) == true)
                    {
                        File.Delete(DirAddress);
                    }

                    //将临时文件夹的文件移到应用程序所在的目录下   
                    File.Move(DirectoryPath, DirAddress);
                }

                logger.Debug(string.Format("{0}:[{1}{2}]]", "copyDirectoryOneFile", DirAddress, "数据复制完成"));
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("{0}:[{1}{2}]:[{3}]", "DoFileIsExcute", DirectoryPath, "设定程序启动状态错误", ex.Message));
                throw new Exception("copyDirectoryOneFile处理错误：" + fileName + ":" + ex.Message);
            }
        }
        #endregion
        #endregion

    }
}
