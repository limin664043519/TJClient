using FBYClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TJClient.Common;
using TJClient.Helper;
using TJClient.NeedToUseForm;
using TJClient.sys.Bll;

namespace TJClient.jktj
{
    public partial class MedexList : Form
    {
        /// <summary>
        /// 保存每个页面要显示的数据
        /// </summary>
        public DataTable dtPage = null;

        /// <summary>
        /// 完整的数据列表
        /// </summary>
        private DataTable dt_tem = new DataTable();

        public string xdtype = null;
        public MedexList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MedexList_Load(object sender, EventArgs e)
        {
            AddFormBll addformbll_cz = new AddFormBll();
            DataTable dtCunzhuang = addformbll_cz.GetMoHuList(string.Format(" and YLJGBM='{0}' ", UserInfo.Yybm), "sql038_cunzhuang");

            //绑定村庄
            DataRow dtRow = dtCunzhuang.NewRow();
            dtRow["B_RGID"] = "";
            dtRow["B_NAME"] = "--请选择--";
            dtCunzhuang.Rows.InsertAt(dtRow, 0);
            comboBox_cunzhuang.DataSource = dtCunzhuang;
            comboBox_cunzhuang.DisplayMember = "B_NAME";
            comboBox_cunzhuang.ValueMember = "B_RGID";
            comboBox_cunzhuang.SelectedValue = 0;

            comboBox_cunzhuang.Focus();
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {            
            if (MessageBox.Show("是否将所选人员信息导入设备数据库中？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    if (xdtype == "MECG200")
                        setPatinfobymed();
                    else if (xdtype == "ECGNETV260")
                        setPatinfobyecg();
                    else if (xdtype == "ECGNETMS")
                    {
                        EcgNetMsOperation();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发送失败，" + ex.Message);
                }
            }
        }

        private void EcgNetMsOperation()
        {
            Task t = new Task(() =>
            {
                try
                {
                    SetPatInfoByEcgSqlite();
                    MessageBox.Show("发送成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });
            t.Start();
        }

        private void setPatinfobymed()
        {
            //生成需要向medex中插入的语句                             
            ArrayList medex_sql = new ArrayList();
            string mid_sql = "";
            MedExSqlHelper sq = new MedExSqlHelper();
            DataRow[] dtRow = dt_tem.Select("select_zt='1'");
            if (dtRow.Length > 0)
            {
                for (int i = 0; i < dtRow.Length; i++)
                {
                    if (sq.ExcuteDataTable(@"select auto_id from PACS_APPLY where PATIENT_ID='" + dtRow[i]["PATIENT_ID"].ToString() + "' and WORK_STATUS=0").Rows.Count > 0)
                    {
                        continue;
                    }
                    mid_sql = @"insert into PACS_APPLY(PATIENT_ID,PATIENT_SOURCE,NAME,SEX,BIRTHDAY,NOTE_NO,OPER_DTIME,WORK_STATUS)
                                    values('" + dtRow[i]["PATIENT_ID"].ToString() + "','" + dtRow[i]["PATIENT_SOURCE"].ToString() + "','"
                                      + dtRow[i]["NAME"].ToString() + "','" + dtRow[i]["SEX"].ToString() + "','"
                                      + dtRow[i]["BIRTHDAY"].ToString() + "','" + dtRow[i]["NOTE_NO"].ToString() + "','"
                                      + dtRow[i]["OPER_DTIME"].ToString() + "'," + dtRow[i]["WORK_STATUS"].ToString() + ")";//根据协议
                    medex_sql.Add(mid_sql);
                }
            }
            //批量插入

            if (medex_sql.Count > 0)
            {
                int result = sq.ExecuteNonQueryBySqlList(medex_sql);
                if (result > 0)
                {
                    MessageBox.Show("发送成功");
                }
            }
            else
            {
                MessageBox.Show("请确认选中的信息是否已经发送至设备");
            }
        }

        private void SetPatInfoByEcgSqlite()
        {
            DataRow[] dtRow = dt_tem.Select("select_zt='1'");
            int count = dtRow.Length-1;
            LogHelper log=new LogHelper(xdtype,true);
            log.AppendMessageToTxt(string.Format("共需操作{0}条数据",count));
            int i = 1;
            foreach (DataRow row in dtRow)
            {
                try
                {
                    //首先判断是否已经发送过去了
                    if (ECGNETMS.EcgDbHelper.HadExistsInPatients(row["patient_id"].ToString()))
                    {
                        log.AppendMessageToTxt(string.Format("姓名为{0}，条码为{1}的数据曾经发送过，本次不再发送", row["name"],
                            row["patient_id"]));
                        continue;
                    }

                    ECGNETMS.EcgDbHelper.InsertPatient(row);
                    if (i % 100 == 0 || i >= count)
                    {
                        log.AppendMessageToTxt(string.Format("{1}:共处理了{0}条",i,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    }
                    i++;
                }
                catch (Exception ex)
                {
                    log.AppendMessageToTxt(string.Format("姓名为{0}，条码为{1}的数据发送错误,{2}",
                        row["name"],row["patient_id"],ex.Message));
                }
                
            }
        }

        private void setPatinfobyecg()
        {
            //生成需要向medex中插入的语句                             
            ArrayList medex_sql = new ArrayList();
            string mid_sql = "";
            ECGNETV260.XDAccess sq = new ECGNETV260.XDAccess();
            DataRow[] dtRow = dt_tem.Select("select_zt='1'");
            string xmlPath = Common.Common.getyqPath("ECGNETV260");
            string xdh = ECGNETV260.XmlRW.GetValueFormXML(xmlPath, "YQ_XDFormat");
            int mno = 1;
            int lenxd = xdh.Length;
            var dtno = sq.ExcuteDataTable("select max(m_no)+1 from main");
            if (dtno != null && dtno.Rows.Count > 0)
            {
                if (dtno.Rows[0][0] != DBNull.Value && dtno.Rows[0][0].ToString()!="")
                    mno = Convert.ToInt32(dtno.Rows[0][0]);
            }
            if (dtRow.Length > 0)
            {
                for (int i = 0; i < dtRow.Length; i++)
                {
                    if (sq.ExcuteDataTable(@"select p_pat_no from patients where p_pat_no='" + dtRow[i]["PATIENT_ID"].ToString() + "' ").Rows.Count > 0)
                    {
                        continue;
                    }
                    string sqltime = "";
                    if (dtRow[i]["OPER_DTIME"] == DBNull.Value || dtRow[i]["OPER_DTIME"].ToString()=="")
                    {
                        sqltime = "null";
                    }
                    else sqltime = "'" + dtRow[i]["OPER_DTIME"].ToString() + "'";

                    int sex = 0;
                    if (dtRow[i]["SEX"] != DBNull.Value && dtRow[i]["SEX"].ToString() != "")
                        sex = Convert.ToInt32(dtRow[i]["SEX"]);
                    if (dtRow[i]["BIRTHDAY"] == DBNull.Value || dtRow[i]["BIRTHDAY"].ToString() == "")
                    {
                        mid_sql = @"insert into patients(p_id,P_NAME,P_SEX,p_age,P_BIRDATE,P_BUILD_DATE,p_pat_no)
                                    select max(p_id)+1,'" + dtRow[i]["NAME"].ToString() + "'," + sex + ",0,null," + sqltime + ",'" + 
                                  dtRow[i]["PATIENT_ID"].ToString() + "' from patients ";
                    }
                    else
                    {
                        DateTime dtbir = Convert.ToDateTime(dtRow[i]["BIRTHDAY"]);
                        int m_Y1 = dtbir.Year;
                        int m_M1 = dtbir.Month;
                        int m_Y2 = DateTime.Now.Year;
                        int m_M2 = DateTime.Now.Month;
                        int age = m_Y2 - m_Y1;
                        if (m_M2 < m_M1)
                            age = age - 1;
                        mid_sql = @"insert into patients(p_id,P_NAME,P_SEX,p_age,P_BIRDATE,P_BUILD_DATE,p_pat_no)
                                    select  iif(IsNull(max(p_id)),1,max(p_id)+1),'" + dtRow[i]["NAME"].ToString() + "'," + sex + "," + age + ",'"
                                          + dtRow[i]["BIRTHDAY"].ToString() + "'," + sqltime + ",'" + dtRow[i]["PATIENT_ID"].ToString() + "' from patients ";
                    }
                    medex_sql.Add(mid_sql);
                    //心电记录表更新

                    mid_sql = @"insert into main(ty_id,m_no,m_date,m_therapy,p_id) select top 1 1,'" + 
                              mno.ToString().PadLeft(lenxd, '0') + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "', '检查室', p_id from patients where p_pat_no='" + dtRow[i]["PATIENT_ID"].ToString() + "'";
                    medex_sql.Add(mid_sql);
                    mno++;
                }

            }
            //批量插入

            if (medex_sql.Count > 0)
            {
                int result = sq.ExecuteNonQueryBySqlList(medex_sql);
                if (result > 0)
                {
                    MessageBox.Show("发送成功");
                }
            }
            else
            {
                MessageBox.Show("请确认选中的信息是否已经发送至设备");
            }
        }
        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click(object sender, EventArgs e)
        {
            jkda_select(true);
        }

        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="isShowMsg">是否显示提示信息：true：显示 false 不显示</param>
        /// <returns></returns>
        public bool jkda_select(bool isShowMsg)
        {
            string sqlWhere = "";
            try
            {
                //村庄
                if (comboBox_cunzhuang.SelectedValue != null && comboBox_cunzhuang.SelectedValue.ToString().Length > 0)
                {
                    sqlWhere = " and  ryxx.czbm like '%" + comboBox_cunzhuang.SelectedValue.ToString() + "%'";
                }
                //姓名
                if (textBox_xm.Text.Length > 0)
                {
                    sqlWhere = sqlWhere + " and xm like '%" + textBox_xm.Text + "%'";
                }                            
                //条码号
                if (txt_tmh.Text.Trim().Length > 0)
                {
                    sqlWhere = sqlWhere + " and txm.txmbh ='" + txt_tmh.Text.Trim() + "'";
                } 
                //出生日期
                if (dateTimePicker_start.Checked == true)
                {                    
                    sqlWhere = sqlWhere + string.Format(" and  ryxx.csrq  >='{0}' ", dateTimePicker_start.Value.ToString("yyyy-MM-dd"));
                }

                //出生日期
                if (dateTimePicker_end.Checked == true)
                {                    
                    sqlWhere = sqlWhere + string.Format(" and  ryxx.csrq  <='{0}' ", dateTimePicker_end.Value.ToString("yyyy-MM-dd"));
                }                                                             

                string errMsg = "";

                if (getRyxxByWhere(sqlWhere, ref errMsg) == false)
                {
                    if (isShowMsg == true)
                    {
                        MessageBox.Show(errMsg);
                    }
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }

        /// <summary>
        /// 按照条件获取信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <param name="Errmsg"></param>
        /// <returns></returns>
        public bool getRyxxByWhere(string sqlWhere, ref string Errmsg)
        {
            Errmsg = "";
            try
            {
                //按照条件检索
                AddFormBll addformbll = new AddFormBll();
                DataTable dt_jktj = addformbll.GetMoHuList(sqlWhere, "sql050_1");
                //dataGridView_list.DataSource = dt_jktj;

                dt_tem = dt_jktj;

                if (dt_jktj == null || dt_jktj.Rows.Count == 0)
                {
                    Errmsg = "没有取到数据！";
                    //数据绑定
                    BindDgv();
                    return false;
                }
                else
                {
                    DataColumn datacolumn = new DataColumn("select_zt", System.Type.GetType("System.Int32"));
                    datacolumn.DefaultValue = 0;
                    dt_jktj.Columns.Add(datacolumn);
                    //数据绑定
                    BindDgv();
                }
                this.pager1.init();//初始化
                this.pager1.Bind();//绑定 
            }
            catch (Exception ex)
            {
                Errmsg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>   
        /// GridViw数据绑定   
        /// </summary>   
        /// <returns></returns>   
        private int BindDgv()
        {

            //传入要取的第一条和最后一条   
            int start = pager1.PageCurrent > 0 ? (pager1.PageSize * (pager1.PageCurrent - 1)) : 0;
            int end = pager1.PageCurrent > 0 ? (pager1.PageSize * pager1.PageCurrent) : pager1.PageSize;

            //数据源   
            dtPage = GetPageFromAll(start, end);
            //绑定分页控件   
            pager1.bindingSource1.DataSource = dtPage;
            pager1.bindingNavigator1.BindingSource = pager1.bindingSource1;
            //讲分页控件绑定DataGridView   
            dataGridView_list.DataSource = dtPage;
            //返回总记录数   
            return dt_tem.Rows.Count;
        }
       
        /// <summary>
        /// 取得当前页要显示的数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public DataTable GetPageFromAll(int start, int end)
        {
            dtPage = new DataTable();
            dtPage = dt_tem.Clone();
            for (int i = start; ((i < dt_tem.Rows.Count) && (i <= end)); i++)
            {                
                dtPage.ImportRow(dt_tem.Rows[i]);
            }
            return dtPage;

        }        

        /// <summary>   
        /// 分页控件产生的事件   
        /// </summary>   
        private int pager1_EventPaging(HuishengFS.Controls.EventPagingArg e)
        {
            return BindDgv();
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_all_Click(object sender, EventArgs e)
        {            
            if (dt_tem != null && dt_tem.Rows.Count > 0)
            {
                for (int i = 0; i < dt_tem.Rows.Count; i++)
                {
                    dt_tem.Rows[i]["select_zt"] = true;
                }
                //显示数据
                BindDgv();
            }
        }

        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_no_Click(object sender, EventArgs e)
        {
            if (dt_tem != null && dt_tem.Rows.Count > 0)
            {
                for (int i = 0; i < dt_tem.Rows.Count; i++)
                {
                    dt_tem.Rows[i]["select_zt"] = false;
                }
                //显示数据
                BindDgv();
            }
        }

        private void dataGridView_list_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null && e.RowIndex >= 0)
            {
                if (grid.Columns[e.ColumnIndex].Name == "select_zt")
                {
                    DataGridViewCheckBoxCell checkbox = grid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell; // 获得checkbox列单元格
                    int rowNo = (pager1.PageCurrent - 1) * pager1.PageSize + e.RowIndex;
                    if (checkbox != null && checkbox.Value.ToString() == "1")
                    {

                        dt_tem.Rows[rowNo]["select_zt"] = "1";
                        dt_tem.AcceptChanges();
                    }
                    else
                    {
                        dt_tem.Rows[rowNo]["select_zt"] = "0";
                        dt_tem.AcceptChanges();
                    }
                }
            }
        }

        private void dataGridView_list_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void txt_tmh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                jkda_select(true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmXdtSendLog._xdType = xdtype;
            FrmXdtSendLog frm=new FrmXdtSendLog();
            frm.Show();
        }
    }
}
