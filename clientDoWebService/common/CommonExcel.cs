using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections;
using System.Data;
using System.Logger;
using Aspose;
using Aspose.Cells;
using clientDoWebService.common;

/// <summary>
///commonExcel 的摘要说明
/// </summary>
public class commonExcel
{
    public  SimpleLogger logger = SimpleLogger.GetInstance();

    public  bool DataTableToExcel(DataTable datatable, string filepath, DataTable dtTitle, out string error)
    {
        error = "";
        try
        {
            if (datatable == null)
            {
                error = "DataTableToExcel:datatable 为空";
                return false;
            }

            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            Aspose.Cells.Worksheet sheet = workbook.Worksheets[0];
            Aspose.Cells.Cells cells = sheet.Cells;

            int nRow = 0;
            foreach (DataRow row in datatable.Rows)
            {
                nRow++;
                try
                {
                    for (int i = 0; i < datatable.Columns.Count; i++)
                    {
                        if (row[i].GetType().ToString() == "System.Drawing.Bitmap")
                        {
                            //------插入图片数据-------
                            System.Drawing.Image image = (System.Drawing.Image)row[i];
                            MemoryStream mstream = new MemoryStream();
                            image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            sheet.Pictures.Add(nRow, i, mstream);
                        }
                        else
                        {
                            cells[nRow, i].PutValue(row[i]);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    error = error + " DataTableToExcel: " + e.Message;
                }
            }

            workbook.Save(filepath);
            return true;
        }
        catch (System.Exception e)
        {
            error = error + " DataTableToExcel: " + e.Message;
            return false;
        }
    }

    

    #region mq 2017年6月21日添加,其中所有方法都于数据分页相关，保存到excel sheet中。

    /// <summary>
    /// 返回文件名，但没有扩展名
    /// </summary>
    /// <param name="rndPrefix">前缀</param>
    /// <param name="yljgbm">医疗机构编码</param>
    /// <param name="tableName">表名</param>
    /// <param name="No">序号</param>
    /// <returns></returns>
    private  string GetFileNameNoExt(string rndPrefix, string yljgbm, string tableName, string no)
    {
        return rndPrefix + "_" + yljgbm + "_" + tableName + "_" + no;
    }

    public  bool OutFileToDistCheckExceedingPaginationCountAndOperation(string rndPrefix, string yljgbm, string sql,
        string tableName, string path, out List<string> fileNames, out string error)
    {
        error = "";
        fileNames = new List<string>();
        string getCountSql = GetCountSql(sql, tableName);
        int paginationCountInWebConfig = GetPaginationCountInWebConfig();
        int dbDataCount = GetExecuteSqlDbDataCount(getCountSql);
        if (paginationCountInWebConfig >= dbDataCount)
        {
            string fileName = GetFileNameNoExt(rndPrefix, yljgbm, tableName, "1") + ".xls";
            fileNames.Add(fileName);
            return OutFileToDisk(GetExecuteSqlDbData(sql, tableName), tableName, path + fileName, out error);
        }
        //分页处理
        try
        {
            int beginRow = 0;
            int No = 0;
            DataTable dt = GetExecuteSqlDbData(GetPaginationSql(sql, beginRow, paginationCountInWebConfig), tableName);
            dt.TableName = tableName;
            int dtRowCount = dt.Rows.Count;
            while (dtRowCount > 0)
            {
                Workbook workbook = GetWorkbookWithStyle();
                //大于0说明数据存在，进行excel sheet 分页操作。
                string sheetName = dt.TableName + "-" + No;
                workbook.Worksheets.Add(sheetName);
                Worksheet sheet = workbook.Worksheets[sheetName]; //工作表 
                Cells cells = sheet.Cells; //单元格 

                int Colnum = dt.Columns.Count - 1; //表格列数 
                //生成行1 标题行    
                cells.Merge(0, 0, 1, Colnum); //合并单元格 
                cells[0, 0].PutValue(tableName); //填写内容 
                cells[0, 0].SetStyle(workbook.Styles[0]);
                cells.SetRowHeight(0, 38);
                //生成行2 列名行 
                for (int i = 0; i < Colnum; i++)
                {
                    cells[1, i].PutValue(dt.Columns[i].ColumnName);
                    cells[1, i].SetStyle(workbook.Styles[1]);
                    cells.SetRowHeight(1, 25);
                }
                int rowindex = 0;
                //生成数据行
                foreach (DataRow row in dt.Rows)
                {
                    for (int k = 0; k < Colnum; k++)
                    {
                        cells[2 + rowindex, k].PutValue(row[k].ToString());
                        cells[2 + rowindex, k].SetStyle(workbook.Styles[2]);
                    }
                    cells.SetRowHeight(2 + rowindex, 24);
                    rowindex++;
                }
                //结束excel操作
                No++;
                string fileName = GetFileNameNoExt(rndPrefix, yljgbm, tableName, No.ToString()) + ".xls";
                fileNames.Add(fileName);
                workbook.Save(path + fileName);
                GC.Collect();
                beginRow = beginRow + paginationCountInWebConfig;
                dt.Clear();
                dt = GetExecuteSqlDbData(GetPaginationSql(sql, beginRow, paginationCountInWebConfig), tableName);
                dtRowCount = dt.Rows.Count;
            }


            return true;
        }
        catch (Exception ex)
        {
            error = " DataTableToExcel In OutFileToDistCheckExceedingPaginationCountAndOperation Found Exception: " + ex.Message;
            return false;
        }

    }
    /// <summary>
    /// 用于判断sql语句想要获取的数量是否大于web.config中指定的数量。如果不大于，执行
    /// OutFileToDisk方法，如果大于，分页处理。
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="tableName"></param>
    /// <param name="path"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public  bool OutFileToDistCheckExceedingPaginationCountAndOperation(string sql,string tableName,string fileName, string path, out string error)
    {

        error = "";
        string getCountSql = GetCountSql(sql, tableName);
        int paginationCountInWebConfig = GetPaginationCountInWebConfig();
        int dbDataCount = GetExecuteSqlDbDataCount(getCountSql);
        if (paginationCountInWebConfig >= dbDataCount)
        {
            return OutFileToDisk(GetExecuteSqlDbData(sql, tableName), tableName, path, out error);
        }
        //分页处理
        try
        {
            int beginRow = 0;
            Workbook workbook = GetWorkbookWithStyle();
            int No = 0;
            DataTable dt = GetExecuteSqlDbData(GetPaginationSql(sql, beginRow, paginationCountInWebConfig), tableName);
            dt.TableName = tableName;
            int dtRowCount = dt.Rows.Count;
            while (dtRowCount > 0)
            {
                //大于0说明数据存在，进行excel sheet 分页操作。
                string sheetName = dt.TableName + "-" + No;
                workbook.Worksheets.Add(sheetName);
                Worksheet sheet = workbook.Worksheets[sheetName]; //工作表 
                Cells cells = sheet.Cells; //单元格 
                int Colnum = dt.Columns.Count; //表格列数 
                //生成行1 标题行    
                cells.Merge(0, 0, 1, Colnum); //合并单元格 
                cells[0, 0].PutValue(tableName); //填写内容 
                cells[0, 0].SetStyle(workbook.Styles[0]);
                cells.SetRowHeight(0, 38);
                //生成行2 列名行 
                for (int i = 0; i < Colnum; i++)
                {
                    cells[1, i].PutValue(dt.Columns[i].ColumnName);
                    cells[1, i].SetStyle(workbook.Styles[1]);
                    cells.SetRowHeight(1, 25);
                }
                int rowindex = 0;
                //生成数据行
                foreach (DataRow row in dt.Rows)
                {
                    for (int k = 0; k < Colnum; k++)
                    {
                        cells[2 + rowindex, k].PutValue(row[k].ToString());
                        cells[2 + rowindex, k].SetStyle(workbook.Styles[2]);
                    }
                    cells.SetRowHeight(2 + rowindex, 24);
                    rowindex++;
                }
                //结束excel操作
                No++;
                beginRow = beginRow + paginationCountInWebConfig;
                dt = GetExecuteSqlDbData(GetPaginationSql(sql, beginRow, paginationCountInWebConfig), tableName);
                dtRowCount = dt.Rows.Count;
            }
            workbook.Save(path);
            return true;
        }
        catch (Exception ex)
        {
            error = " DataTableToExcel In OutFileToDistCheckExceedingPaginationCountAndOperation Found Exception: " + ex.Message;
            return false;
        }
        
    }

    
    private  Workbook GetWorkbookWithStyle()
    {
        Workbook workbook = new Workbook();
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        styleTitle.Font.Name = "宋体";//文字字体 
        styleTitle.Font.Size = 18;//文字大小 
        styleTitle.Font.IsBold = true;//粗体 

        //样式2 
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        style2.Font.Name = "宋体";//文字字体 
        style2.Font.Size = 14;//文字大小 
        style2.Font.IsBold = true;//粗体 
        style2.IsTextWrapped = true;//单元格内容自动换行 
        style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式3 
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式 
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
        style3.Font.Name = "宋体";//文字字体 
        style3.Font.Size = 12;//文字大小 
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
        workbook.Worksheets.Clear();
        return workbook;
    }

    private  string GetCountSql(string sql,string tableName)
    {
        if (sql.Contains(".*"))
        {
            return sql.Replace(string.Format("{0}.*", tableName), "count(*)");
        }
        return sql.Replace("*", "count(*)");

    }
    private  string GetPaginationSql(string sql,int beginRow,int paginationCount)
    {
        if (sql.Contains("t_jk_tjry_txm"))
        {
            return string.Format("SELECT * FROM (SELECT a.*,ROWNUM rn FROM ({0} order by txmbh) a WHERE ROWNUM<={1}) where rn>{2}",sql,beginRow+paginationCount,beginRow);
        }
        return string.Format("{0} and rownum<={1} MINUS {0} and rownum<={2}",sql,beginRow+paginationCount,beginRow);

    }
    private  DataTable GetExecuteSqlDbData(string sql,string tableName)
    {
        DBOracle dboracle = new DBOracle();
        DataTable dt=dboracle.ExcuteDataTable_oracle(sql);
        dt.TableName = tableName;
        return dt;
    }
    /// <summary>
    /// 获取执行sql语句的总数量
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    private  int GetExecuteSqlDbDataCount(string sql)
    {
        DBOracle dboracle = new DBOracle();
        DataTable dt = dboracle.ExcuteDataTable_oracle(sql);
        if (dt != null)
        {
            return int.Parse(dt.Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 获取web.config中指定的分页数量
    /// </summary>
    /// <returns></returns>
    public  int GetPaginationCountInWebConfig()
    {
        if (ConfigurationManager.AppSettings["DataCountPerSheet"] == null)
        {
            return 30000;
        }
        return int.Parse(ConfigurationManager.AppSettings["DataCountPerSheet"]);
    }
    #endregion
    
    /// <summary> 
    /// 导出数据到本地 
    /// </summary> 
    /// <param name="dt">要导出的数据</param> 
    /// <param name="tableName">表格标题</param> 
    /// <param name="path">保存路径</param> 
    public  bool OutFileToDisk(DataTable datatable, string tableName, string path, out string error)
    {
        error = "";
        if (datatable == null)
        {
            error = "DataTableToExcel:datatable 为空";
            return false;
        }
        try
        {
            Workbook workbook = new Workbook(); //工作簿 
            #region 样式
            //为标题设置样式     
            Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 
            //styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            //styleTitle.Font.Name = "宋体";//文字字体 
            //styleTitle.Font.Size = 18;//文字大小 
            //styleTitle.Font.IsBold = true;//粗体 

            //样式2 
            Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            //style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            //style2.Font.Name = "宋体";//文字字体 
            //style2.Font.Size = 14;//文字大小 
            //style2.Font.IsBold = true;//粗体 
            //style2.IsTextWrapped = true;//单元格内容自动换行 
            //style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            //style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            //style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            //样式3 
            Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            //style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            //style3.Font.Name = "宋体";//文字字体 
            //style3.Font.Size = 12;//文字大小 
            //style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            //style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            //style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            #endregion
            //数据条数
            int ColumnsCount = datatable.Rows.Count;
            int sheetCount = 60000;
            int countSheet = ColumnsCount % sheetCount == 0 ? ColumnsCount / sheetCount : (ColumnsCount / sheetCount) + 1;
            workbook.Worksheets.Clear();
            //创建存放数据的sheet
            for (int j = 0; j < countSheet; j++)
            {
                //基础数据
                string sheetName = datatable.TableName + "-" + j.ToString();
                workbook.Worksheets.Add(sheetName);

                Worksheet sheet = workbook.Worksheets[sheetName]; //工作表 

                Cells cells = sheet.Cells;//单元格 
                

                int Colnum = datatable.Columns.Count;//表格列数 
                int Rownum = datatable.Rows.Count;//表格行数 

                //生成行1 标题行    
                cells.Merge(0, 0, 1, Colnum);//合并单元格 
                cells[0, 0].PutValue(tableName);//填写内容 
                cells[0, 0].SetStyle(styleTitle);
                cells.SetRowHeight(0, 38);

                //生成行2 列名行 
                for (int i = 0; i < Colnum; i++)
                {
                    cells[1, i].PutValue(datatable.Columns[i].ColumnName);
                    cells[1, i].SetStyle(style2);
                    cells.SetRowHeight(1, 25);
                }

                int rowindex = 0;
                //生成数据行 
                for (int i = j * sheetCount; (i <(j+1) * sheetCount && i<Rownum); i++)
                {
                    for (int k = 0; k < Colnum; k++)
                    {
                        cells[2 + rowindex, k].PutValue(datatable.Rows[i][k].ToString());
                        cells[2 + rowindex, k].SetStyle(style3);
                    }
                    cells.SetRowHeight(2 + rowindex, 24);
                    rowindex++;
                }
            }
            workbook.Save(path);
            return true;
        }
        catch (Exception e)
        {
            error = error + " DataTableToExcel: " + e.Message;
            return false;
        }
    }


    /// <summary> 
    /// 导出数据到本地 
    /// </summary> 
    /// <param name="dt">要导出的数据</param> 
    /// <param name="tableName">表格标题</param> 
    /// <param name="path">保存路径</param> 
    public  ArrayList OutFileToDiskAll(DataTable datatable, string tableName, string path, out string error)
    {
        error = "";
        if (datatable == null)
        {
            error = "DataTableToExcel:datatable 为空";
            return null;
        }

        //保存生成的文件
        ArrayList fileList = new ArrayList();

        try
        {
           

            //数据条数
            int ColumnsCount = datatable.Rows.Count;
            int sheetCount = 10000;
            int countSheet = ColumnsCount % sheetCount == 0 ? ColumnsCount / sheetCount : (ColumnsCount / sheetCount) + 1;
            
            //创建存放数据的sheet
            for (int j = 0; j < countSheet; j++)
            {
                Workbook workbook = new Workbook(); //工作簿 
                #region 样式
                //为标题设置样式     
                Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 
                styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                styleTitle.Font.Name = "宋体";//文字字体 
                styleTitle.Font.Size = 18;//文字大小 
                styleTitle.Font.IsBold = true;//粗体 

                //样式2 
                Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
                style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                style2.Font.Name = "宋体";//文字字体 
                style2.Font.Size = 14;//文字大小 
                style2.Font.IsBold = true;//粗体 
                style2.IsTextWrapped = true;//单元格内容自动换行 
                style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                //样式3 
                Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式 
                style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                style3.Font.Name = "宋体";//文字字体 
                style3.Font.Size = 12;//文字大小 
                style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                #endregion
                workbook.Worksheets.Clear();

                string sheetName = datatable.TableName + "-" + j.ToString();
                workbook.Worksheets.Add(sheetName);

                Worksheet sheet = workbook.Worksheets[sheetName]; //工作表 

                Cells cells = sheet.Cells;//单元格 


                int Colnum = datatable.Columns.Count;//表格列数 
                int Rownum = datatable.Rows.Count;//表格行数 

                //生成行1 标题行    
                cells.Merge(0, 0, 1, Colnum);//合并单元格 
                cells[0, 0].PutValue(tableName);//填写内容 
                cells[0, 0].SetStyle(styleTitle);
                cells.SetRowHeight(0, 38);

                //生成行2 列名行 
                for (int i = 0; i < Colnum; i++)
                {
                    cells[1, i].PutValue(datatable.Columns[i].ColumnName);
                    cells[1, i].SetStyle(style2);
                    cells.SetRowHeight(1, 25);
                }

                int rowindex = 0;
                //生成数据行 
                for (int i = j * sheetCount; (i < (j + 1) * sheetCount && i < Rownum); i++)
                {
                    for (int k = 0; k < Colnum; k++)
                    {
                        cells[2 + rowindex, k].PutValue(datatable.Rows[i][k].ToString());
                        cells[2 + rowindex, k].SetStyle(style3);
                    }
                    cells.SetRowHeight(2 + rowindex, 24);
                    rowindex++;
                }

                string fileName = datatable.TableName + "_" + j.ToString() +".xls";
                //保存
                workbook.Save(path + fileName);
                fileList.Add(path + fileName);
            }

            return fileList;
        }
        catch (Exception e)
        {
            error = error + " DataTableToExcel: " + e.Message;
            return null;
        }
    }


    public  bool DataTableToExcel3(DataTable datatable, string filepath, out string error)
    {
        error = "";
        try
        {
            if (datatable == null)
            {
                error = "DataTableToExcel:datatable 为空";
                return false;
            }
            //excel表格的标题
            DataTable dttitle = new DataTable();
            dttitle.Columns.Add("mc_VillageCode");
            dttitle.Columns.Add("Name");
            dttitle.Columns.Add("CMCode");
            dttitle.Columns.Add("HumanCode");
            dttitle.Columns.Add("IDCard");
            dttitle.Columns.Add("RecordDate_sq");
            dttitle.Columns.Add("mc_CardMakeStatus");
            dttitle.Columns.Add("RecordDateEnd");
            dttitle.Columns.Add("mc_hospitalCode");
            dttitle.Columns.Add("makeOrder");
            dttitle.Columns.Add("code");
            dttitle.Columns.Add("Reason_sq");
            dttitle.Rows.Add();
            dttitle.Rows[0]["mc_VillageCode"] = "家庭住址";
            dttitle.Rows[0]["Name"] = "姓名";
            dttitle.Rows[0]["CMCode"] = "新农合卡号";
            dttitle.Rows[0]["HumanCode"] = "个人编号";
            dttitle.Rows[0]["IDCard"] = "身份证号";
            dttitle.Rows[0]["RecordDate_sq"] = "挂失日期";
            dttitle.Rows[0]["mc_CardMakeStatus"] = "补卡完成状态";
            dttitle.Rows[0]["RecordDateEnd"] = "补卡完成时间";
            dttitle.Rows[0]["mc_hospitalCode"] = "挂失单位";
            dttitle.Rows[0]["makeOrder"] = "所在批次";
            dttitle.Rows[0]["code"] = "补卡卡号";
            dttitle.Rows[0]["Reason_sq"] = "补卡原因";

            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();//工作簿 

            Worksheet sheet = wb.Worksheets[0];//工作表

            Cells cells = sheet.Cells;//单元格
            Style style1 = wb.Styles[wb.Styles.Add()];//新增样式 
            style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            style1.Font.Name = "宋体";//文字字体 
            style1.Font.Size = 14;//文字大小 
            style1.Font.IsBold = true;//粗体 
            style1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
            style1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
            style1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
            style1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线 

            Style style2 = wb.Styles[wb.Styles.Add()];//新增样式 
            style2.HorizontalAlignment = TextAlignmentType.Left;//文字居中 
            style2.Font.Name = "宋体";//文字字体 
            style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
            style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
            style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
            style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线 
            style2.IsTextWrapped = true;//自动换行



            int ColumnsCount = dttitle.Columns.Count;

            int rowIndex = 0;
            cells.Merge(rowIndex, 0, 1, ColumnsCount);//合并单元格 
            for (int i = 0; i < ColumnsCount; i++)
            {
                cells[rowIndex, i].SetStyle(style1); //cells[0, 1].SetStyle(style2);
            }

            cells[rowIndex, 0].PutValue("合 作 医 疗 农 户 档 案 补 卡 登 记 表");//填写内容 
            cells.SetRowHeight(rowIndex, 22);//设置行高 


            //导出日期： 2013-12-04 09:32:04

            rowIndex++;
            cells.Merge(rowIndex, 0, 1, ColumnsCount);//合并单元格 
            for (int i = 0; i < ColumnsCount; i++)
            {
                cells[rowIndex, i].SetStyle(style2); //cells[0, 1].SetStyle(style2);
            }

            cells[rowIndex, 0].PutValue("导出日期： " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//填写内容 
            cells.SetRowHeight(rowIndex, 22);//设置行高 

            rowIndex++;
            //标题
            for (int i = 0; i < dttitle.Columns.Count; i++)
            {
                //DataColumn col = datatable.Columns[i];
                string columnName = dttitle.Rows[0][i].ToString();//col.Caption ?? col.ColumnName;
                wb.Worksheets[0].Cells[rowIndex, i].PutValue(columnName);
                cells[rowIndex, i].SetStyle(style2);
                cells.SetRowHeight(0, 38);
                cells.SetColumnWidth(i, 18);
            }

            rowIndex++;

            foreach (DataRow row in datatable.Rows)
            {
                if (row["chkFlag"].ToString().Equals("0"))
                {
                    continue;
                }
                for (int i = 0; i < dttitle.Columns.Count; i++)
                {
                    wb.Worksheets[0].Cells[rowIndex, i].PutValue(row[dttitle.Columns[i].ColumnName].ToString());
                    cells[rowIndex, i].SetStyle(style2);
                }
                cells.SetRowHeight(rowIndex, 18);//设置行高 
                rowIndex++;
            }

            //for (int k = 0; k < datatable.Columns.Count; k++)
            //{
            //    wb.Worksheets[0].AutoFitColumn(k, 0, 150);
            //}
            // wb.Worksheets[0].FreezePanes(1, 0, 1, datatable.Columns.Count);
            wb.Save(filepath);
            return true;
        }
        catch (Exception e)
        {
            error = error + " DataTableToExcel: " + e.Message;
            return false;
        }

    }

    /// <summary>
    /// Excel文件转换为DataTable.
    /// </summary>
    /// <param name="filepath">Excel文件的全路径</param>
    /// <param name="datatable">DataTable:返回值</param>
    /// <param name="error">错误信息:返回错误信息，没有错误返回""</param>
    /// <returns>true:函数正确执行 false:函数执行错误</returns>
    public  bool ExcelFileToDataTable(string filepath, out DataTable datatable, out string error)
    {
        error = "";
        datatable = null;
        try
        {
            if (File.Exists(filepath) == false)
            {
                error = "文件不存在";
                datatable = null;
                return false;
            }
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            workbook.Open(filepath);
            Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];
            datatable = worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1);
            return true;
        }
        catch (System.Exception e)
        {
            error = e.Message;
            return false;
        }

    }

    /// <summary>
    /// 重写了ExcelFileToDataSet方法，原方法有时会出现问题。
    /// </summary>
    /// <returns></returns>
    public  DataSet ExcelFileToDataSet(string filePath,string guidStr)
    {
        DataSet result =new DataSet();
        try
        {
            if (!File.Exists(filePath)) //如果文件不存在
            {
                return result;
            }
            Workbook workBook = new Workbook(filePath);
            foreach (Worksheet sheet in workBook.Worksheets)
            {
                DataTable dt = GetDatatableFromWorksheet(sheet);
                if (dt.Rows.Count > 0)
                {
                    result.Tables.Add(dt);
                    result.Tables[result.Tables.Count - 1].TableName = dt.TableName;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
       
        return result;
    }

    private  DataTable GetDatatableFromWorksheet(Worksheet sheet)
    {
        DataTable dt=new DataTable();
        if (sheet.Cells.Rows.Count > 1)
        {
            dt=sheet.Cells.ExportDataTable(0, 0, sheet.Cells.MaxRow + 1, sheet.Cells.MaxColumn + 1);
            dt.TableName = sheet.Name;
            if (dt.Rows.Count > 2)
            {
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    dt.Columns[k].ColumnName = dt.Rows[1][k].ToString();
                }
                dt.Rows[1].Delete();
                dt.Rows[0].Delete();
                dt.AcceptChanges();
            }
        }
        return dt;
    }

    /// <summary>
    /// Excel文件转换为DataTable.
    /// </summary>
    /// <param name="filepath">Excel文件的全路径</param>
    /// <param name="datatable">DataTable:返回值</param>
    /// <param name="error">错误信息:返回错误信息，没有错误返回""</param>
    /// <returns>true:函数正确执行 false:函数执行错误</returns>
    public  bool ExcelFileToDataSet(string filepath, string guidStr,out DataSet dataset, out string error)
    {
        error = "";
        //dataset = null;
        try
        {
            if (File.Exists(filepath) == false)
            {
                error = "文件不存在";
                dataset = null;
                return false;
            }
            else
            {
                dataset = new DataSet();
            }
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            workbook.Open(filepath);
            int tableCount = workbook.Worksheets.Count;
            for (int i = 0; i < tableCount; i++)
            {
                DataTable datatable = new DataTable();
                Aspose.Cells.Worksheet worksheet = workbook.Worksheets[i];
                if (worksheet.Cells.Rows.Count > 1)
                {
                    datatable = worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1);
                    datatable.TableName = workbook.Worksheets[i].Name;
                    if (datatable.Rows.Count > 2)
                    {
                        for (int k = 0; k < datatable.Columns.Count; k++)
                        {
                            datatable.Columns[k].ColumnName = datatable.Rows[1][k].ToString();
                        }
                        datatable.Rows[1].Delete();
                        datatable.Rows[0].Delete();
                        datatable.AcceptChanges();
                    }
                    dataset.Tables.Add(datatable.Copy());
                    dataset.Tables[dataset.Tables.Count - 1].TableName = workbook.Worksheets[i].Name;
                }
            }
            return true;
        }
        catch (System.Exception e)
        {
            logger.Error(string.Format("{0}:[{1}]:[{2}]:[{3}]", "ExcelFileToDataSet", filepath, "数据保存异常", e.Message + e.StackTrace));
            DBLogger.Insert(DBLogger.GetLoggerInfo(filepath, e.Message+e.StackTrace, guidStr, 0));
            error = e.Message;
            throw e;
            return false;
        }
    }

    public  bool ListsToExcelFile(string filepath, IList[] lists, out string error)
    {
        error = "";
        //----------Aspose变量初始化----------------
        Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
        Aspose.Cells.Worksheet sheet = workbook.Worksheets[0];
        Aspose.Cells.Cells cells = sheet.Cells;
        //-------------输入数据-------------
        int nRow = 0;
        sheet.Pictures.Clear();
        cells.Clear();
        foreach (IList list in lists)
        {

            for (int i = 0; i <= list.Count - 1; i++)
            {
                try
                {
                    System.Console.WriteLine(i.ToString() + "  " + list[i].GetType());
                    if (list[i].GetType().ToString() == "System.Drawing.Bitmap")
                    {
                        //插入图片数据
                        System.Drawing.Image image = (System.Drawing.Image)list[i];

                        MemoryStream mstream = new MemoryStream();

                        image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);

                        sheet.Pictures.Add(nRow, i, mstream);
                    }
                    else
                    {
                        cells[nRow, i].PutValue(list[i]);
                    }
                }
                catch (System.Exception e)
                {
                    error = error + e.Message;
                }

            }

            nRow++;
        }
        //-------------保存-------------
        workbook.Save(filepath);

        return true;
    }

    public  bool DsToExcelCommon(DataSet ds, string filepath, out string error)
    {
        error = "";
        try
        {
            if (ds == null)
            {
                error = "DataTableToExcel:datatable 为空";
                return false;
            }
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();//工作簿 

            Worksheet sheet = wb.Worksheets[0];//工作表

            Cells cells = sheet.Cells;//单元格
            Style style1 = wb.Styles[wb.Styles.Add()];//新增样式 
            style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            style1.Font.Name = "宋体";//文字字体 
            style1.Font.Size = 14;//文字大小 
            style1.Font.IsBold = true;//粗体 
            style1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
            style1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
            style1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
            style1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线 

            Style style2 = wb.Styles[wb.Styles.Add()];//新增样式 
            style2.HorizontalAlignment = TextAlignmentType.Left;//文字居中 
            style2.Font.Name = "宋体";//文字字体 
            //style2.Font.Size = 14;//文字大小 
            // style2.Font.IsBold = true;//粗体 
            style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin; //应用边界线 左边界线 
            style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线 
            style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin; //应用边界线 上边界线 
            style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin; //应用边界线 下边界线 
            style2.IsTextWrapped = true; //自动换行

            //设定导出项目的列数
            int ColumnsCount = ds.Tables[2].Rows.Count;

            int rowIndex = 0;
            cells.Merge(rowIndex, 0, 1, ColumnsCount);//合并单元格 
            for (int i = 0; i < ColumnsCount; i++)
            {
                cells[rowIndex, i].SetStyle(style1); //cells[0, 1].SetStyle(style2);
            }

            cells[rowIndex, 0].PutValue(ds.Tables[1].Rows[0]["title"].ToString());//填写内容 
            cells.SetRowHeight(rowIndex, 22);//设置行高 


            //导出日期： 2013-12-04 09:32:04

            rowIndex++;
            cells.Merge(rowIndex, 0, 1, ColumnsCount);//合并单元格 
            for (int i = 0; i < ColumnsCount; i++)
            {
                cells[rowIndex, i].SetStyle(style2); //cells[0, 1].SetStyle(style2);
            }

            cells[rowIndex, 0].PutValue("导出日期： " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//填写内容 
            cells.SetRowHeight(rowIndex, 22);//设置行高 

            rowIndex++;
            //设置标题
            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
            {
                wb.Worksheets[0].Cells[rowIndex, i].PutValue(ds.Tables[2].Rows[i]["columnName"].ToString());
                cells[rowIndex, i].SetStyle(style2);
                cells.SetColumnWidth(i, 20);
            }

            rowIndex++;

            //设置数据行
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    wb.Worksheets[0].Cells[rowIndex, i].PutValue(row[ds.Tables[2].Rows[i]["columnId"].ToString()].ToString());
                    cells[rowIndex, i].SetStyle(style2);
                }
                cells.SetRowHeight(rowIndex, 18);//设置行高 
                rowIndex++;
            }

            //创建合并用的数据结构
            DataTable dtMerge = new DataTable();
            dtMerge.Columns.Add("groupId");
            dtMerge.Columns.Add("columnId");
            dtMerge.Columns.Add("startRowIndex");
            dtMerge.Columns.Add("endRowIndex");
            dtMerge.Columns.Add("startColumnIndex");
            dtMerge.Columns.Add("endColumnIndex");
            dtMerge.Columns.Add("groupIdValue");
            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
            {
                if (ds.Tables[2].Rows[i]["mergeColumn"] != DBNull.Value && ds.Tables[2].Rows[i]["mergeColumn"].ToString().Length > 0)
                {
                    dtMerge.Rows.Add();
                    dtMerge.Rows[dtMerge.Rows.Count - 1]["groupId"] = ds.Tables[2].Rows[i]["mergeColumn"];
                    dtMerge.Rows[dtMerge.Rows.Count - 1]["columnId"] = ds.Tables[2].Rows[i]["columnId"];
                    dtMerge.Rows[dtMerge.Rows.Count - 1]["startColumnIndex"] = i;
                    dtMerge.Rows[dtMerge.Rows.Count - 1]["endColumnIndex"] = i;
                }
            }

            //合并列
            rowIndex = rowIndex - ds.Tables[0].Rows.Count;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                if (j == 0)
                {
                    //第一行数据的处理
                    for (int i = 0; i < dtMerge.Rows.Count; i++)
                    {
                        dtMerge.Rows[i]["groupIdValue"] = ds.Tables[0].Rows[j][dtMerge.Rows[i]["groupId"].ToString()];
                        dtMerge.Rows[i]["startRowIndex"] = rowIndex;
                    }

                }
                else if (j == ds.Tables[0].Rows.Count - 1)
                {
                    //最后一行数据的处理
                    for (int i = 0; i < dtMerge.Rows.Count; i++)
                    {
                        if (!dtMerge.Rows[i]["groupIdValue"].ToString().Equals(ds.Tables[0].Rows[j][dtMerge.Rows[i]["groupId"].ToString()]))
                        {
                            //分组名称改变时合并单元格
                            int startRowIndex = Convert.ToInt32(dtMerge.Rows[i]["startRowIndex"].ToString());
                            cells.Merge(startRowIndex, Convert.ToInt32(dtMerge.Rows[i]["startColumnIndex"].ToString()), rowIndex - startRowIndex, 1);//合并单元格 
                            //初始化值
                            dtMerge.Rows[i]["groupIdValue"] = ds.Tables[0].Rows[j][dtMerge.Rows[i]["groupId"].ToString()];
                            dtMerge.Rows[i]["startRowIndex"] = rowIndex;
                        }
                        else
                        {
                            //分组名称改变时合并单元格
                            int startRowIndex = Convert.ToInt32(dtMerge.Rows[i]["startRowIndex"].ToString());
                            cells.Merge(startRowIndex, Convert.ToInt32(dtMerge.Rows[i]["startColumnIndex"].ToString()), rowIndex - startRowIndex + 1, 1);//合并单元格 
                        }
                    }
                }
                else
                {

                    for (int i = 0; i < dtMerge.Rows.Count; i++)
                    {
                        if (!dtMerge.Rows[i]["groupIdValue"].ToString().Equals(ds.Tables[0].Rows[j][dtMerge.Rows[i]["groupId"].ToString()]))
                        {
                            //分组名称改变时合并单元格
                            int startRowIndex = Convert.ToInt32(dtMerge.Rows[i]["startRowIndex"].ToString());
                            cells.Merge(startRowIndex, Convert.ToInt32(dtMerge.Rows[i]["startColumnIndex"].ToString()), rowIndex - startRowIndex, 1);//合并单元格 
                            //初始化值
                            dtMerge.Rows[i]["groupIdValue"] = ds.Tables[0].Rows[j][dtMerge.Rows[i]["groupId"].ToString()];
                            dtMerge.Rows[i]["startRowIndex"] = rowIndex;
                        }
                    }


                }
                rowIndex++;
            }
            wb.Save(filepath);
            return true;
        }
        catch (Exception e)
        {
            error = error + " DataTableToExcel: " + e.Message;
            return false;
        }

    }
}
