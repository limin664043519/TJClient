using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections;
using System.Data;
using Aspose;
using Aspose.Cells;
/// <summary>
///commonExcel 的摘要说明
/// </summary>
public class commonExcel
{

    public static bool DataTableToExcel(DataTable datatable, string filepath, DataTable dtTitle, out string error)
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

    /// <summary> 
    /// 导出数据到本地 
    /// </summary> 
    /// <param name="dt">要导出的数据</param> 
    /// <param name="tableName">表格标题</param> 
    /// <param name="path">保存路径</param> 
    public static bool OutFileToDisk(DataTable datatable, string tableName, string path, out string error)
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
            //数据条数
            int ColumnsCount = datatable.Rows.Count;
            int sheetCount = 60000;
            int countSheet = ColumnsCount % sheetCount == 0 ? ColumnsCount / sheetCount : (ColumnsCount / sheetCount) + 1;
            workbook.Worksheets.Clear();
            //创建存放数据的sheet
            for (int j = 0; j < countSheet; j++)
            {
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

                //生成数据行 
                for (int i = 0; i < Rownum; i++)
                {
                    for (int k = 0; k < Colnum; k++)
                    {
                        cells[2 + i, k].PutValue(datatable.Rows[i][k].ToString());
                        cells[2 + i, k].SetStyle(style3);
                        cells.SetColumnWidth(k,18);

                    }
                    cells.SetRowHeight(2 + i, 24);
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
    /// Excel文件转换为DataTable.
    /// </summary>
    /// <param name="filepath">Excel文件的全路径</param>
    /// <param name="datatable">DataTable:返回值</param>
    /// <param name="error">错误信息:返回错误信息，没有错误返回""</param>
    /// <returns>true:函数正确执行 false:函数执行错误</returns>
    public static bool ExcelFileToDataTable(string filepath, out DataTable datatable, out string error)
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
    /// Excel文件转换为DataTable.
    /// </summary>
    /// <param name="filepath">Excel文件的全路径</param>
    /// <param name="datatable">DataTable:返回值</param>
    /// <param name="error">错误信息:返回错误信息，没有错误返回""</param>
    /// <returns>true:函数正确执行 false:函数执行错误</returns>
    public static bool ExcelFileToDataSet(string filepath, out DataSet dataset, out string error)
    {
        error = "";
        dataset = null;
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
            error = e.Message;
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
    public static bool ExcelFileToDataSet1(string filepath, out DataSet dataset, out string error)
    {
        error = "";
        dataset = null;
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

                    if (datatable.Rows.Count > 1)
                    {

                        for (int k = 0; k < datatable.Columns.Count; k++)
                        {
                            datatable.Columns[k].ColumnName = datatable.Rows[0][k].ToString();
                        }

                        //datatable.Rows[1].Delete();
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
            error = e.Message;
            return false;
        }

    }
    public static bool ListsToExcelFile(string filepath, IList[] lists, out string error)
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
}