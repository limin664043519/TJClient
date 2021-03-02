using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.Configuration;
using TJClient;
using TJClient.sys;
using TJClient.Common;
using TJClient.sys.Bll;
namespace FBYClient
{
    public partial class DataDownLoad : sysCommonForm
    {
        /// <summary>
        /// 存放原始的结果集
        /// </summary>
        private static DataTable dt_tem = new DataTable();

        /// <summary>
        /// 用户id
        /// </summary>
        private static string userId = "";

        /// <summary>
        /// 工作组
        /// </summary>
        private static string yhfz = "";

        /// <summary>
        /// 医疗机构
        /// </summary>
        private static string yljg = "";

        /// <summary>
        /// 要下载的表名称
        /// </summary>
        private static string tableName = "";

        /// <summary>
        /// 村庄列表
        /// </summary>
        private static string czList = "";

        /// <summary>
        /// 前页面传过来的参数
        /// </summary>
        DataTable dt_para_sys = null;

        public bool setPara(DataTable dtpara)
        {
            try
            {
                dt_para_sys = dtpara;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public DataDownLoad()
        {
            InitializeComponent();
        }

        #region 控件事件
        private void Form_downLoad_Load(object sender, EventArgs e)
        {
            //progressBar_xz.Visible = false;
            //取得前以页面传递的数据

            //DataTable dtPara = (DataTable)((jktj)this.Owner).Tag;
            //用户id
            userId = dt_para_sys.Rows[0]["userId"].ToString();

            //工作组
            yhfz = dt_para_sys.Rows[0]["gzz"].ToString();

            //医疗机构
            yljg = dt_para_sys.Rows[0]["yljg"].ToString();

            //取得医疗机构编码
            Form_yljgBll Form_yljg = new Form_yljgBll();
            DataTable dtYljg = Form_yljg.GetMoHuList("", "sql039");

            //绑定医疗机构
            comboBox_yljg.DataSource = dtYljg;
            comboBox_yljg.DisplayMember = "mc";
            comboBox_yljg.ValueMember = "bm";

            comboBox_yljg.SelectedValue = yljg;
            comboBox_yljg.Enabled = false;

            //取得村庄
            listboxFormBll listbox = new listboxFormBll();
            DataTable dtCunzhuang = listbox.GetMoHuList(string.Format("and YLJGBM = '{0}'", UserInfo.Yybm), "sql038");

            //绑定医疗机构
            comboBox_cunzhuang.DataSource = dtCunzhuang;
            comboBox_cunzhuang.DisplayMember = "czmc";
            comboBox_cunzhuang.ValueMember = "czbm";
            comboBox_cunzhuang.Focus();
        }


        private bool GetFileType(ref string fileType)
        {
            //基础数据
            if (checkBox_jcsj.Checked)
            {
                fileType = string.Format("{0},{1}", fileType, "1");
                return true;
            }

            //居民档案
            if (checkBox_jkda.Checked)
            {
                //村编码
                if (czList.Length == 0)
                {
                    fileType="请选择村庄！";
                    return false;
                }
                else
                {
                    //村庄编码
                    DataDownLoad_TJ_Para.czList = czList;
                }

                //居民档案
                fileType = string.Format("{0},{1}", fileType, "2");
                return true;
            }

            //体检结果
            if (checkBox_tjjg.Checked)
            {
                DataDownLoad_TJ_Para.czList = czList;
                //上次体检信息
                fileType = string.Format("{0},{1}", fileType, "3");
                return true;
            }
            fileType = "请选择操作类型";
            return false;
        }

        private void InitDataDownloadTjPara(string fileType)
        {
            //区分  1：下载  2：导入
            DataDownLoad_TJ_Para.qf = "1";

            //操作员编码
            DataDownLoad_TJ_Para.czy = UserInfo.userId;

            //医疗机构编码
            DataDownLoad_TJ_Para.yljgbm = UserInfo.Yybm;

            DataDownLoad_TJ_Para.fileType = fileType.Substring(1);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_download_Click(object sender, EventArgs e)
        {
            try
            {
                //文件类型
                string fileType = "";
                if (!GetFileType(ref fileType))
                {
                    MessageBox.Show(fileType);
                    return;
                }
                InitDataDownloadTjPara(fileType);

                //调用下载处理
                update_tj updateTj = new update_tj();
                updateTj.Show();
            }
            catch (Exception ex)
            {
                //progressBar_xz.Visible = false;
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_import_Click(object sender, EventArgs e)
        {
            try
            {
                //对话框初始目录
                openFileDialog1.InitialDirectory = ConfigurationSettings.AppSettings["filePath"].ToString();

                //文件类型
                //openFileDialog1.Filter = "excel文件(*.xls)|*.xlsx";

                //弹出文件选择框
                openFileDialog1.ShowDialog();
                //打开的文件的全限定名
                String strOpenFileName = openFileDialog1.FileName;
                //导入数据
                bool importResult = importFileTo(strOpenFileName);
                if (importResult)
                {
                    MessageBox.Show("导入完成！");
                }
                else
                {
                    MessageBox.Show("导入失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ////与户主关系
            //setGridViewComboBox("yhzgx", "D_YHZGX", "ZDBM", "ZDMC", e.RowIndex);
            ////// 省
            //////市
            //////区（县）
            ////街道（乡）
            ////居委会
            ////居委会
            //setGridViewComboBox_jwh("D_JWH", "B_RGID", "B_NAME", e.RowIndex);

            ////所属片区

            ////居住状况
            //setGridViewComboBox("jzzk", "D_JZZK", "ZDBM", "ZDMC", e.RowIndex);
            ////性别
            //setGridViewComboBox("xb_xingbie", "D_XB", "ZDBM", "ZDMC", e.RowIndex);
            ////民族
            //setGridViewComboBox("mz_minzu", "D_MZ", "ZDBM", "ZDMC", e.RowIndex);
            ////文化程度
            //setGridViewComboBox("whcd", "D_WHCD", "ZDBM", "ZDMC", e.RowIndex);
            ////职业
            //setGridViewComboBox("zy_zhiye", "D_ZY", "ZDBM", "ZDMC", e.RowIndex);
            ////婚姻状况
            //setGridViewComboBox("hyzk", "D_HYZK", "ZDBM", "ZDMC", e.RowIndex);
            ////医疗费支付类型
            //setGridViewComboBox("ylfzflx", "D_YLFZFLX", "ZDBM", "ZDMC", e.RowIndex);

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            //ArrayList sqlList = new ArrayList();
            //string sql="";
            ////修改后的数据
            //DataTable dt = (DataTable)GridViewList.DataSource;
            //if (dt != null && dt.Rows.Count> 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {

            //        if (dt.Rows[i].RowState == DataRowState.Added)
            //        {
            //            //增加
            //            sqlList.Add(add_Jkda_Rkxzl(dt, i));
            //        }else if(dt.Rows[i].RowState == DataRowState.Deleted){


            //        }
            //        else if (dt.Rows[i].RowState == DataRowState.Modified)
            //        {
            //            //修改
            //            sqlList.Add(update_Jkda_Rkxzl(dt, i));
            //        }
            //    }
            //}

            //DBAccess access = new DBAccess();
            //access.ExecuteNonQueryBySql(sqlList);
            //MessageBox.Show("保存成功！");
        }


        /// <summary>
        /// 生成插入数据的语句
        /// </summary>
        /// <param name="dtPara"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private string add_Jkda_Rkxzl(DataTable dtPara, int rowIndex)
        {
            string sqlColumns = "";
            string sqlValue = "";
            string sql = "";
            //for (int i = 0; i < dtPara.Columns.Count; i++)
            //{
            //    sqlColumns = sqlColumns + dtPara.Columns[i].ColumnName + ",";
            //    if (dtPara.Columns[i].DataType.Equals(Type.GetType("System.String")))
            //    {
            //        sqlValue = sqlValue + "'" + dtPara.Rows[rowIndex][i].ToString() + "' ,";
            //    }
            //    else
            //    {
            //        sqlValue = sqlValue + dtPara.Rows[rowIndex][i].ToString() + ",";
            //    }
            //}

            //sql = " insert into T_DA_JKDA_RKXZL(" + sqlColumns.Substring(0, sqlColumns.Length - 1) + ") values (" + sqlValue.Substring(0, sqlValue.Length - 1) + ") ";

            return sql;
        }

        /// <summary>
        /// 生成修改数据的语句
        /// </summary>
        /// <param name="dtPara"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private string update_Jkda_Rkxzl(DataTable dtPara, int rowIndex)
        {
            //string sqlColumns = "";
            string sqlValue = "zt='1',";
            string sql = "";
            //for (int i = 0; i < dtPara.Columns.Count; i++)
            //{
            //    if (rowIndex <= dt_tem.Rows.Count && dtPara.Rows[rowIndex]["id"].ToString().Equals(dt_tem.Rows[rowIndex]["id"].ToString()) && !dtPara.Rows[rowIndex][i].ToString().Equals(dt_tem.Rows[rowIndex][i].ToString()))
            //    {
            //        if (dtPara.Columns[i].DataType.Equals(Type.GetType("System.String")))
            //        {
            //            sqlValue = sqlValue + dtPara.Columns[i].ColumnName + "='" + dtPara.Rows[rowIndex][i].ToString() + "' ,";
            //        }
            //        else
            //        {
            //            sqlValue = sqlValue + dtPara.Columns[i].ColumnName + dtPara.Rows[rowIndex][i].ToString() + ",";
            //        }
            //    }
            //}

            //sql = " update T_DA_JKDA_RKXZL set " + sqlValue.Substring(0, sqlValue.Length - 1) + " where id =" + dt_tem.Rows[rowIndex]["id"].ToString();

            return sql;
        }

        /// <summary>
        /// 下载对象信息的处理 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            //CheckBox checkBox = (CheckBox)sender;
            //string tableNameList = ConfigurationManager.AppSettings[checkBox.Name.ToString()].ToString();
            //if (checkBox.Checked == true)
            //{
            //    tableName = tableName + "," + tableNameList;
            //    tableName = tableName.Replace(",,", ",");
            //}
            //else
            //{
            //    tableName = tableName.Replace(tableNameList, "");
            //    tableName = tableName.Replace(",,", ",");
            //}
        }

        /// <summary>
        /// 检索处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click(object sender, EventArgs e)
        {
            string sqlWhere = " and  (D_GRDABH like '%{para}%' or D_XM like '%{para}%'  or D_SFZH like '%{para}%' or D_LXDH like '%{para}%'  or CREATEUSER like '%{para}%') ".Replace("{para}", textBox_Select.Text.Trim());
            if (comboBox_cunzhuang.SelectedValue != null && comboBox_cunzhuang.SelectedValue.ToString().Length > 0)
            {
                sqlWhere = sqlWhere + "  and D_JWH ='" + comboBox_cunzhuang.SelectedValue.ToString() + "'";
            }
            Form_downLoadBll form_download = new Form_downLoadBll();
            DataTable dt = form_download.GetMoHuList(sqlWhere, "sql040");
            dataGridView_list.DataSource = dt;
        }
        #endregion

        #region 公用方法

        /// <summary>
        /// 设定明细中下拉框的值
        /// </summary>
        /// <param name="ZDLXBM"></param>
        /// <param name="ColumnsName"></param>
        /// <param name="ValueMember"></param>
        /// <param name="DisplayMember"></param>
        private void setGridViewComboBox_jwh(string ColumnsName, string ValueMember, string DisplayMember, int rowIndex)
        {
            //DBAccess dBAccess = new DBAccess();
            //DataTable dt = dBAccess.ExecuteQueryBySql(" SELECT * FROM T_BS_CUNZHUANG  ");

            //DataRow dtRow = dt.NewRow();
            //dtRow[DisplayMember] = "--请选择--";
            //dtRow[ValueMember] = "";
            //dt.Rows.InsertAt(dtRow, 0);
            //((DataGridViewComboBoxCell)GridViewList.Rows[rowIndex].Cells[ColumnsName]).DataSource = dt.Copy();
            //((DataGridViewComboBoxCell)GridViewList.Rows[rowIndex].Cells[ColumnsName]).ValueMember = ValueMember;
            //((DataGridViewComboBoxCell)GridViewList.Rows[rowIndex].Cells[ColumnsName]).DisplayMember = DisplayMember;
        }

        /// <summary>
        /// 设定明细中下拉框的值
        /// </summary>
        /// <param name="ZDLXBM"></param>
        /// <param name="ColumnsName"></param>
        /// <param name="ValueMember"></param>
        /// <param name="DisplayMember"></param>
        private void setGridViewComboBox(string ZDLXBM, string ColumnsName, string ValueMember, string DisplayMember, int rowIndex)
        {
            //DBAccess dBAccess = new DBAccess();
            //DataTable dt = dBAccess.ExecuteQueryBySql(" SELECT * FROM T_JK_SJZD where  ZDLXBM= '" + ZDLXBM + "' ");

            //DataRow dtRow = dt.NewRow();
            //dtRow[DisplayMember] = "--请选择--";
            //dtRow[ValueMember] = "";
            //dt.Rows.InsertAt(dtRow,0);
            //((DataGridViewComboBoxCell)GridViewList.Rows[rowIndex].Cells[ColumnsName]).DataSource = dt.Copy();
            //((DataGridViewComboBoxCell)GridViewList.Rows[rowIndex].Cells[ColumnsName]).ValueMember = ValueMember;
            //((DataGridViewComboBoxCell)GridViewList.Rows[rowIndex].Cells[ColumnsName]).DisplayMember = DisplayMember;
        }


        /// <summary>
        /// 把指定的文件导入到数据库中
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool importFileTo(string filePath)
        {

            DataSet ds = new DataSet();
            DataTable dbRowName = new DataTable();
            ArrayList sqllist = new ArrayList();
            ArrayList sqllist_sqlserver = new ArrayList();
            ArrayList sqlList = new ArrayList();
            DBAccess access = new DBAccess();
            DataTable dt = new DataTable();
            string tablename = "";
            //string checkBox_lis_str = "";
            try
            {
                ds = getDsFromExcel(filePath);
                if (ds != null && ds.Tables.Count > 0)
                {


                    //进度条
                    //progressBar_xz.Visible = true;
                    //progressBar_xz.Maximum = ds.Tables.Count+2;
                    //progressBar_xz.Minimum = 0;
                    //progressBar_xz.Value = 0;

                    //遍历下载的所有数据库表
                    for (int tableIndex = 0; tableIndex < ds.Tables.Count; tableIndex++)
                    {
                        //progressBar_xz.Value++;
                        Application.DoEvents();
                        //DataTable dt = new DataTable();
                        dt = ds.Tables[tableIndex].Copy();
                        tablename = dt.TableName;

                        if (dt != null && dt.Rows.Count > 0)
                        {

                            ////修改datatable列名称
                            //for (int i = 0; i < dt.Columns.Count; i++)
                            //{
                            //    if (dt.Rows[0][i] != null && dt.Rows[0][i].ToString().Length > 0)
                            //    {
                            //        dt.Columns[i].ColumnName = dt.Rows[0][i].ToString().ToLower();
                            //    }
                            //}

                            //删除数据
                            string sqlwhere = "";
                            if(czList.Length >0){
                                sqlwhere = "'" + czList.Replace(",", "','") + "'";
                            }
                            sqlList = Common.FormatSql(dt, Common.SQLTYPE.delete.ToString(), sqlwhere);
                            if (sqlList != null)
                            {
                                access.ExecuteNonQueryBySqlList(sqlList);
                            }

                            //新增
                            sqlList = Common.FormatSql(dt, Common.SQLTYPE.insert.ToString(), czList);
                            if (sqlList != null)
                            {
                                access.ExecuteNonQueryBySqlList(sqlList);
                            }
                        }
                    }
                    //progressBar_xz.Visible = false;
                }
            }
            catch (Exception ex)
            {
                //progressBar_xz.Visible = false;
                MessageBox.Show(string.Format("表[{0}]{1}", tablename, ex.Message));
                return false;
            }
            return true;
        }

        /// <summary>
        /// excel转化为dataset
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private DataSet getDsFromExcel(string filePath)
        {

            DataSet ds = new DataSet();
            string errMessage = "";

            bool boolResult = commonExcel.ExcelFileToDataSet(filePath, out ds, out errMessage);
            if (boolResult == false)
            {
                return null;
            }
            return ds;
        }
        #endregion
        /// <summary>
        /// Enter转换为tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Tab_KeyDown(object sender, KeyEventArgs e)
        {
            //Enter转换为tab
            if (sender.GetType().ToString().Equals("System.Windows.Forms.CheckBox"))
            {
                if (e.KeyCode == Keys.Enter)
                {
                    CheckBox checkBox = (CheckBox)sender;
                    if (checkBox.Checked == true)
                    {
                        checkBox.Checked = false;
                    }
                    else
                    {
                        checkBox.Checked = true;
                    }
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }

            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }

            }

            if (sender.GetType().ToString().Equals("System.Windows.Forms.ComboBox"))
            {
                if (e.KeyCode == Keys.Left)
                {
                    //shift+Tab
                    SendKeys.Send("+{Tab}");
                    e.Handled = false;
                }
                if (e.KeyCode == Keys.Right)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
            }
            else
            {

                if (e.KeyCode == Keys.Up)
                {
                    //shift+Tab
                    SendKeys.Send("+{Tab}");
                    e.Handled = false;
                }
                if (e.KeyCode == Keys.Down)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                e.Handled = false;
            }

        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            czList = "";
            this.Owner.Visible = true;
            this.Close();
        }

        /// <summary>
        /// 重写父类的方法，设定当前页面的值 strText： 编码|名称
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public override bool setText(string strText)
        {
            if (strText.Length > 0)
            {
                string[] selectList = strText.Split(new char[] { '|' });
                czList = selectList[0];

                link_all_cz.Text = string.Format("选择了[{0}]个机构", selectList[1]);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 选择村庄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void link_all_cz_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listbox_cz form = new listbox_cz();
            form.Owner = this;
            form.Show();
            if (form.setListData(czList, "sql038") == false)
            {
                MessageBox.Show("没有对应的村庄信息！");
            }
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_list_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dataGridView_list.Columns[e.ColumnIndex].Name == "详情")
                {
                    DataRowView dr = (DataRowView)dataGridView_list.Rows[e.RowIndex].DataBoundItem;
                    Form_daxq form = new Form_daxq();
                    form.Owner = this;
                    form.str_id = dr["id"].ToString();
                    form.ShowDialog();
                    //if (form.dataSelect(string.Format(" and id ={0}", dr["id"].ToString()), "sql041") == false)
                    //{
                    //    MessageBox.Show("没有取得详情信息！");
                    //}
                }
            }

        }
        /// <summary>
        /// 行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_list_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_list.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView_list.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView_list.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
