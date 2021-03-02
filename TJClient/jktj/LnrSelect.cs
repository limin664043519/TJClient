using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using TJClient.Common;
using TJClient.sys.Bll;
using System.Drawing.Drawing2D;
using TJClient.ComForm;
using TJClient.sys;
using System.IO;
using TJClient.UploadInfo;

namespace FBYClient
{
    public partial class LnrSelect : sysCommonForm
    {
        #region 变量声明
        /// <summary>
        /// 医疗机构
        /// </summary>
        private static string yljg = "";

        /// <summary>
        /// 村庄编码
        /// </summary>
        private static string czbm = "";

        private int selectedRowIndex = -1;

        /// <summary>
        /// 保存每个页面要显示的数据
        /// </summary>
        public DataTable dtPage = null;

        /// <summary>
        /// 完整的数据列表
        /// </summary>
        //public DataTable dtAll = null;
        private DataTable dt_tem = new DataTable();

        //读取身份证号
        public Form_readSfzh form_readsfzh = new Form_readSfzh();

        #endregion

        #region 窗体初始化

        public LnrSelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddForm_Load(object sender, EventArgs e)
        {
            //禁用列表自动创建列
            dataGridView_list.AutoGenerateColumns = false;

            DBAccess dBAccess = new DBAccess();

            //取得村庄
//            string sqlCunzhaung = @"SELECT T_BS_CUNZHUANG.B_RGID, T_BS_CUNZHUANG.B_NAME
//                                    FROM T_TJ_YLJG_XIANGZHEN INNER JOIN T_BS_CUNZHUANG ON    T_TJ_YLJG_XIANGZHEN.XZBM = left(T_BS_CUNZHUANG.B_RGID,len(T_TJ_YLJG_XIANGZHEN.XZBM))
//
//                                    where  T_TJ_YLJG_XIANGZHEN.YLJGBM='{YLJGBM}'
//                                     order by T_BS_CUNZHUANG.B_RGID;";

//            DataTable dtCunzhuang = dBAccess.ExecuteQueryBySql(sqlCunzhaung.Replace("{YLJGBM}", UserInfo.Yybm));

            //AddFormBll addformbll_cz = new AddFormBll();
            //DataTable dtCunzhuang = addformbll_cz.GetMoHuList(string.Format(" and YLJGBM='{0}' ", UserInfo.Yybm), "sql038_cunzhuang");
            listboxFormBll listbox = new listboxFormBll();
            DataTable dtCunzhuang = listbox.GetMoHuList(string.Format("and YLJGBM = '{0}'", UserInfo.Yybm), "sql038");
            //绑定村庄
            DataRow dtRow = dtCunzhuang.NewRow();
            //dtRow["B_RGID"] = "";
            //dtRow["B_NAME"] = "--请选择--";
            dtRow["czbm"] = "";
            dtRow["czmc"] = "--请选择--";
            dtCunzhuang.Rows.InsertAt(dtRow, 0);
            comboBox_cunzhuang.DataSource = dtCunzhuang;
            //comboBox_cunzhuang.DisplayMember = "B_NAME";
            //comboBox_cunzhuang.ValueMember = "B_RGID";
            comboBox_cunzhuang.DisplayMember = "czmc";
            comboBox_cunzhuang.ValueMember = "czbm";
            comboBox_cunzhuang.SelectedValue = czbm;

            comboBox_cunzhuang.Focus();
        }

        #endregion

        #region 检索

        /// <summary>
        /// 姓名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_xm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                jkda_select();
            }

        }

        /// <summary>
        /// 身份证号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_sfzh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                jkda_select();
            }
        }

        /// <summary>
        /// 档案号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_jkda_tem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                jkda_select();
            }
        }

        /// <summary>
        /// 检索
        /// </summary>
        /// <returns></returns>
        public bool jkda_select()
        {
            string sqlWhere = "";
            try
            {
                //医疗机构编码
                sqlWhere = sqlWhere + " and YLJGBM = '" + UserInfo .Yybm+ "'";

                //村庄
                if (comboBox_cunzhuang.SelectedValue != null && comboBox_cunzhuang.SelectedValue.ToString().Length > 0)
                {
                    //sqlWhere = sqlWhere + " and CZBM = '" + comboBox_cunzhuang.SelectedValue.ToString() + "'";
                    sqlWhere = sqlWhere + " and PRGID = '" + comboBox_cunzhuang.SelectedValue + "'";
                }

                //体检号
                if (textBox_TJBH.Text.Trim().Length > 0)
                {
                    sqlWhere = sqlWhere + " and (TJBM like '%" + textBox_TJBH.Text.Trim() + "%' or TJBH_TEM like '%" + textBox_TJBH.Text.Trim() + "%')";
                }

                //姓名
                if (textBox_xm.Text.Trim().Length > 0)
                {
                    sqlWhere = sqlWhere + " and XM like '%" + textBox_xm.Text.Trim() + "%'";
                }

                //体检状态
                string strWhere_jktj = "";
                //健康体检
                if (checkBox_jktj.Checked == true)
                {
                    if (!((checkBox_wtj.Checked == true && checkBox_ytj.Checked == true) || (checkBox_wtj.Checked == false && checkBox_ytj.Checked == false)))
                    {
                        if (checkBox_wtj.Checked == true)
                        {
                            strWhere_jktj = "and  ZT_jktj is null ";
                        }
                        else
                        {
                            strWhere_jktj = " and ZT_jktj='1'  ";
                        }
                    }

                    //体检时间
                    if (dateTimePicker_tjsj.Checked == true)
                    {
                        strWhere_jktj = strWhere_jktj + " and TJSJ_jktj = '" + dateTimePicker_tjsj.Value.ToString("yyy-MM-dd") + "'";
                    }
                }

                //体质辨识
                string strWhere_tzbs = "";
                if (checkBox_tzbs.Checked == true)
                {
                    if (!((checkBox_wtj.Checked == true && checkBox_ytj.Checked == true) || (checkBox_wtj.Checked == false && checkBox_ytj.Checked == false)))
                    {
                        if (checkBox_wtj.Checked == true)
                        {
                            strWhere_tzbs = "and ZT_tzbs is null ";
                        }
                        else
                        {
                            strWhere_tzbs = " and ZT_tzbs='1'  ";
                        }
                    }

                    //体检时间
                    if (dateTimePicker_tjsj.Checked == true)
                    {
                        strWhere_tzbs = strWhere_tzbs + " and TJSJ_tzbs = '" + dateTimePicker_tjsj.Value.ToString("yyy-MM-dd") + "'";
                    }
                }

                //自理行为能力评估
                string strWhere_zlnlpg = "";
                if (checkBox_zlnlpg.Checked == true)
                {
                    if (!((checkBox_wtj.Checked == true && checkBox_ytj.Checked == true) || (checkBox_wtj.Checked == false && checkBox_ytj.Checked == false)))
                    {
                        if (checkBox_wtj.Checked == true)
                        {
                            strWhere_zlnlpg = "and ZT_zlpg is null ";
                        }
                        else
                        {
                            strWhere_zlnlpg = " and ZT_zlpg='1'  ";
                        }
                    }

                    //体检时间
                    if (dateTimePicker_tjsj.Checked == true)
                    {
                        strWhere_zlnlpg = strWhere_zlnlpg + " and TJSJ_zlpg = '" + dateTimePicker_tjsj.Value.ToString("yyy-MM-dd") + "'";
                    }
                }

                //人员状态
                //登记
                if (checkBox_dj.Checked == true)
                {
                    //TJSJ_dj   //ZT_dj
                    sqlWhere = sqlWhere + " and ZT_dj = '" + Common .ZT .确定状态 + "'";
                    if (dateTimePicker_tjsj.Checked == true)
                    {
                        sqlWhere = sqlWhere + " and TJSJ_dj = '" + dateTimePicker_tjsj.Value .ToString ("yyyy-MM-dd") + "'";
                    }
                }

                //新建档
                if (checkBox_add.Checked == true)
                {
                    //TJSJ_dj   //ZT_dj

                    if (checkBox_update.Checked == true)
                    {
                        sqlWhere = sqlWhere + " and (isnewdoc = '1' or  fl='2' )";
                    }
                    else
                    {
                        sqlWhere = sqlWhere + " and isnewdoc = '1' ";
                    }
                }
                else if (checkBox_update.Checked == true)
                {
                    sqlWhere = sqlWhere + " and ( isnewdoc = '0' and   fl='2'  )";
                }

                sqlWhere = sqlWhere + strWhere_jktj + strWhere_tzbs + strWhere_zlnlpg;

                //按照条件检索
                AddFormBll addformbll = new AddFormBll();
                DataTable dt_jktj = addformbll.GetMoHuList(sqlWhere, "sql803");
                //dataGridView_list.DataSource = dt_jktj;
                dt_tem = dt_jktj;
                if (dt_jktj == null || dt_jktj.Rows.Count == 0)
                {
                    MessageBox.Show("没有取到数据！");
                }
                else
                {
                    BindDgv();
                }
                this.pager1.init();//初始化
                this.pager1.Bind();//绑定 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;

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
            if (dt_tem != null)
            {
                dtPage = dt_tem.Clone();
                for (int i = start; ((i < dt_tem.Rows.Count) && (i <= end)); i++)
                {
                    dtPage.ImportRow(dt_tem.Rows[i]);
                }
            }
            return dtPage;

        }

        /// <summary>   
        /// GridViw数据绑定   
        /// </summary>   
        /// <returns></returns>   
        private int BindDgv()
        {
           
            //传入要取的第一条和最后一条   
            int start = pager1.PageCurrent > 0 ? (pager1.PageSize * (pager1.PageCurrent - 1) + 1)-1 : 0;
            int end = pager1.PageCurrent > 0 ? (pager1.PageSize * pager1.PageCurrent)-1 : pager1.PageSize;

            //数据源   
            dtPage = GetPageFromAll(start, end);
            //绑定分页控件   
            pager1.bindingSource1.DataSource = dtPage;
            pager1.bindingNavigator1.BindingSource = pager1.bindingSource1;
            //讲分页控件绑定DataGridView   
            dataGridView_list.DataSource = pager1.bindingSource1;
            //返回总记录数   
            return dt_tem!=null? dt_tem.Rows.Count:0;
        }

        /// <summary>   
        /// 分页控件产生的事件   
        /// </summary>   
        private int pager1_EventPaging(HuishengFS.Controls.EventPagingArg e)
        {
            return BindDgv();
        }

        /// <summary>   
        /// 加载分页 或许写在Load事件里面   
        /// </summary>   
        private void FrmPage_Shown(object sender, EventArgs e)
        {
            #region DataGridView与Pager控件绑定
            this.pager1.PageCurrent = 1;//当前页为第一页   
            
            pager1.PageSize = 30;//页数   
            this.pager1.Bind();//绑定  
            #endregion
        }

        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click(object sender, EventArgs e)
        {
            jkda_select();
        }

        /// <summary>
        /// 当前选择行的行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = e.RowIndex;
        }

        #endregion

        #region 明细的行号的显示
        /// <summary>
        /// 行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_list_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,e.RowBounds.Location.Y,dataGridView_list.RowHeadersWidth - 4,e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView_list.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView_list.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private GraphicsPath GetGraphicsPath(Rectangle rc, int r)
        {
            int x = rc.X, y = rc.Y, w = rc.Width, h = rc.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, r, r, 180, 90);                
            path.AddArc(x + w - r, y, r, r, 270, 90);            
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90);        
            path.AddArc(x, y + h - r, r, r, 90, 90);            
            path.CloseFigure();
            return path;
        }

        private void panelContaioner_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rc = new Rectangle(0 + 2, 0 + 2, this.Width - 10, this.Height - 5);
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush, 1);
            g.DrawPath(pen, this.GetGraphicsPath(rc, 20));
        }
        #endregion

        #region  上传

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_uploade_Click(object sender, EventArgs e)
        {
            if (dt_tem == null || dt_tem.Rows.Count == 0)
            {
                MessageBox.Show("没有要上传的数据！");
                return;
            }
            //启动上传处理
            DataTable dt_para = dt_tem.Clone();
            DataRow[] dtRow = dt_tem.Select("xz='1'");

            //string sqlWhere_jkdah = "";
            //string sqlWhere_sfzh = "";
            //string sqlWhere_tjbh = ""; 
            if (dtRow.Length > 0)
            {
                for(int i = 0; i < dtRow.Length; i++)
                {
                    dt_para.ImportRow(dtRow[i]);
                }
            }

            upLoad_tj upload_tj = new upLoad_tj();
            upload_tj.dt_Para = dt_para;
            upload_tj.Show();

        }

        /// <summary>
        /// 上传LIS数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_uploadeLis_Click(object sender, EventArgs e)
        {
            if (dt_tem == null || dt_tem.Rows.Count == 0)
            {
                MessageBox.Show("没有要上传的数据！");
                return;
            }
            //启动上传处理
            DataTable dt_para = dt_tem.Clone();
            DataRow[] dtRow = dt_tem.Select("xz='1'");
            if (dtRow.Length > 0)
            {
                for (int i = 0; i < dtRow.Length; i++)
                {
                    dt_para.ImportRow(dtRow[i]);
                }
            }
            upLoad_jkda upload_jkda = new upLoad_jkda();
            upload_jkda.dt_Para = dt_para;
            upload_jkda.dataType = Common.UploadTYPE.lis信息;
            upload_jkda.Show();
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        public byte[] getImg(DataRow dtRow)
        {
            //图片
            DataTable dtImg = new DataTable();
            jktjBll jktjbll = new jktjBll();
            //dtImg = jktjbll.GetMoHuList(string.Format(" and D_GRDABH='{0}' and czy='{1}' and HAPPENTIME like '%{2}%'", dtRow["JKDAH"].ToString(), UserInfo.userId, DateTime.Now.Year.ToString()), "sql089");
            dtImg = jktjbll.GetMoHuList(string.Format(" and D_GRDABH='{0}' and HAPPENTIME like '%{1}%'", dtRow["JKDAH"].ToString(), DateTime.Now.Year.ToString()), "sql089");
            
            if (dtImg != null && dtImg.Rows.Count > 0)
            {
                string imgUrl = dtImg.Rows[0]["XDTURL"] != null ? dtImg.Rows[0]["XDTURL"].ToString() : "";

                if (imgUrl.Length > 0)
                {
                    //判断文件的存在
                    if (File.Exists(imgUrl) == true)
                    {
                        FileStream fs = File.OpenRead(imgUrl); //OpenRead
                        int filelength = 0;
                        filelength = (int)fs.Length; //获得文件长度 
                        Byte[] image = new Byte[filelength]; //建立一个字节数组 
                        fs.Read(image, 0, filelength); //按字节流读取 
                        fs.Close();
                        return image;
                    }
                }
            }
            return null;
        }
        #endregion

        #region  明细选择处理
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
                    dt_tem.Rows[i]["xz"] = true;
                }
                //显示数据
                BindDgv();
            }

        }

        /// <summary>
        /// 全不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_no_Click(object sender, EventArgs e)
        {
            if (dt_tem != null && dt_tem.Rows.Count > 0)
            {

                for (int i = 0; i < dt_tem.Rows.Count; i++)
                {
                    dt_tem.Rows[i]["xz"] = false;
                }

                //显示数据
                BindDgv();
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

        private void dataGridView_list_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            DataGridView grid = sender as DataGridView;
            if (grid != null && e.RowIndex >= 0)
            {
                if (grid.Columns[e.ColumnIndex].Name == "checklist")
                {
                    DataGridViewCheckBoxCell checkbox = grid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell; // 获得checkbox列单元格
                    int rowNo = (pager1.PageCurrent-1) * pager1.PageSize + e.RowIndex;
                    if (checkbox != null &&   checkbox.Value.ToString() == "1")
                    {
                        
                        dt_tem.Rows[rowNo]["xz"] = "1";
                        dt_tem.AcceptChanges();
                    }
                    else
                    {
                        dt_tem.Rows[rowNo]["xz"] = "0";
                        dt_tem.AcceptChanges();
                    }
                }
            }



        }

        #endregion

        #region 共通处理

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
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            //this.Owner.Visible = true;
            this.Close();
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
        #endregion

        private void dataGridView_list_Sorted(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
        }

        /// <summary>
        /// 健康体检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_jktj_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_jktj.Checked == true)
            {
                checkBox_wtj.Enabled = true;
                checkBox_ytj.Enabled = true;
                //dateTimePicker_tjsj.Enabled = true;
            }
            else if (checkBox_tzbs.Checked == false && checkBox_zlnlpg.Checked == false)
            {
                checkBox_wtj.Enabled = false;
                checkBox_ytj.Enabled = false;
                //dateTimePicker_tjsj.Enabled = false;
            }
        }
        /// <summary>
        /// 中医体质辨识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_tzbs_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_tzbs.Checked == true)
            {
                checkBox_wtj.Enabled = true;
                checkBox_ytj.Enabled = true;
                //dateTimePicker_tjsj.Enabled = true;
            }
            else if (checkBox_jktj.Checked == false && checkBox_zlnlpg.Checked == false)
            {
                checkBox_wtj.Enabled = false;
                checkBox_ytj.Enabled = false;
                //dateTimePicker_tjsj.Enabled = false;
            }
        }

        /// <summary>
        /// 生活自理能力评估
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_zlnlpg_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_zlnlpg.Checked == true)
            {
                checkBox_wtj.Enabled = true;
                checkBox_ytj.Enabled = true;
                //dateTimePicker_tjsj.Enabled = true;
            }
            else if (checkBox_tzbs.Checked == false && checkBox_jktj.Checked == false)
            {
                checkBox_wtj.Enabled = false;
                checkBox_ytj.Enabled = false;
                //dateTimePicker_tjsj.Enabled = false;
            }
        }

        /// <summary>
        /// 已体检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_ytj_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ytj.Checked == true)
            {
                checkBox_wtj.Checked = false;
            }
        }

        /// <summary>
        /// 未体检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_wtj_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_wtj.Checked == true)
            {
                checkBox_ytj.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmUploadInfos frm = new FrmUploadInfos();
            frm.Show();
        }

    }
}
