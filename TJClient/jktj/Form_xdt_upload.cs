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

namespace FBYClient
{
    public partial class Form_xdt_upload : sysCommonForm
    {
        /// <summary>
        /// 用户名
        /// </summary>
        private static string userId = "";

        ///// <summary>
        ///// 分组
        ///// </summary>
        //private static string yhfz = "";

        /// <summary>
        /// 医疗机构
        /// </summary>
        private static string yljg = "";

        /// <summary>
        /// 村庄编码
        /// </summary>
        private static string czbm = "";

        private DataTable dt_tem = new DataTable();

        private int selectedRowIndex = -1;

        //读取身份证号
        public Form_readSfzh form_readsfzh = new Form_readSfzh();

        public Form_xdt_upload()
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

            DBAccess dBAccess = new DBAccess();
            //取得村庄
            string sqlCunzhaung = @"SELECT T_BS_CUNZHUANG.B_RGID, T_BS_CUNZHUANG.B_NAME
                                    FROM T_TJ_YLJG_XIANGZHEN INNER JOIN T_BS_CUNZHUANG ON    T_TJ_YLJG_XIANGZHEN.XZBM = left(T_BS_CUNZHUANG.B_RGID,len(T_TJ_YLJG_XIANGZHEN.XZBM))

                                    where  T_TJ_YLJG_XIANGZHEN.YLJGBM='{YLJGBM}'
                                     order by T_BS_CUNZHUANG.B_RGID;";

            DataTable dtCunzhuang = dBAccess.ExecuteQueryBySql(sqlCunzhaung.Replace("{YLJGBM}", UserInfo.Yybm));

            //绑定村庄
            DataRow dtRow = dtCunzhuang.NewRow();
            dtRow["B_RGID"] = "";
            dtRow["B_NAME"] = "--请选择--";
            dtCunzhuang.Rows.InsertAt(dtRow, 0);
            comboBox_cunzhuang.DataSource = dtCunzhuang;
            comboBox_cunzhuang.DisplayMember = "B_NAME";
            comboBox_cunzhuang.ValueMember = "B_RGID";
            comboBox_cunzhuang.SelectedValue = czbm;

            comboBox_cunzhuang.Focus();
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
                    sqlWhere = sqlWhere + " and CZBM = '" + comboBox_cunzhuang.SelectedValue.ToString() + "'";
                }

                //体检时间
                if (dateTimePicker_tjsj.Checked == true)
                {
                    sqlWhere = sqlWhere + " and TJSJ = '" + dateTimePicker_tjsj.Value.ToString("yyy-MM-dd") + "'";
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
                string strTJZT = "TJZT in ('{0}','{1}')";
                if (checkBox_wtj.Checked == true)
                {
                    strTJZT = strTJZT.Replace("{0}", "2");
                }
                if (checkBox_ytj.Checked == true)
                {
                    strTJZT = strTJZT.Replace("{1}", "1");
                }
                if (checkBox_wtj.Checked == true || checkBox_ytj.Checked == true)
                {
                    sqlWhere = sqlWhere + " and " + strTJZT;
                }


                //按照条件检索
                AddFormBll addformbll = new AddFormBll();
                DataTable dt_jktj = addformbll.GetMoHuList(sqlWhere, "sql092");
                dataGridView_list.DataSource = dt_jktj;
                dt_tem = dt_jktj;
                if (dt_jktj == null || dt_jktj.Rows.Count == 0)
                {
                    MessageBox.Show("没有取到数据！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;

        }


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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click(object sender, EventArgs e)
        {
            jkda_select();
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_add_Click(object sender, EventArgs e)
        {

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
            path.AddArc(x, y, r, r, 180, 90);                //
            path.AddArc(x + w - r, y, r, r, 270, 90);            //
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90);        //
            path.AddArc(x, y + h - r, r, r, 90, 90);            //
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

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_uploade_Click(object sender, EventArgs e)
        {
            //TJClient.WebReference.JktjServiceService webService = new TJClient.WebReference.JktjServiceService();
            //string url = System.Configuration.ConfigurationManager.AppSettings["HisUrl"].ToString();
            //DataTable dt = ((DataTable)dataGridView_list.DataSource).Copy();
            try
            {
                //if (dt_tem != null && dt_tem.Rows.Count > 0)
                //{
                //    DataRow[] dtRow = dt_tem.Select(" xz='1'");
                //    progressBar1.Visible = true;
                //    progressBar1.Maximum = dtRow.Length ;
                //    progressBar1.Value = 0;
                //    progressBar1.Step = 1;
                //    int countall = 0;
                //    int uploadall = 0;

                //    for (int i = 0; i < dtRow.Length ; i++)
                //    {
                //        progressBar1.Value = i;
                //        //if (dt.Rows[i]["xz"].ToString().Equals("1"))
                //        //{
                //            countall += 1;
                //            //DataRow dtRow = dt.Rows[i];
                //            byte[] imgbyte = getImg(dtRow[i]);
                //            if (imgbyte != null)
                //            {
                //                string result = webService.getdealXdt(dtRow[i]["JKDAH"].ToString(), dtRow[i]["TJSJ"].ToString(), dtRow[i]["XM"].ToString(), imgbyte, url);
                //              if (result.Length > 0)
                //              {
                //                  string[] resultList = result.Split(new char[] { '-' });
                //                  //上传成功
                //                  if (resultList.Length > 0 && "s".Equals(resultList[0]))
                //                  {
                //                      dt_tem.Rows[i]["xdtzt"] = "上传成功";
                //                      uploadall += 1;
                //                  }
                //                  else
                //                  {
                //                      //上传失败
                //                      dt_tem.Rows[i]["xdtzt"] = "上传失败";
                //                  }
                //              }
                //            }
                //        //}
                //    }

                //    //progressBar1.BindingContext = "ddddd";
                //    progressBar1.Visible = false;
                //    progressBar1.Value = 0;
                //    if (uploadall > 0)
                //    {
                //        DataView dt_view = dt_tem.DefaultView;
                //        dt_view.Sort = "xdtzt";
                //        DataTable dt_xdt_tem = dt_view.ToTable();
                //        dataGridView_list.DataSource = dt_tem;
                //        MessageBox.Show(string.Format("上传结束！[{0}/{1}]", uploadall.ToString(), countall.ToString()));
                //    }
                //    else
                //    {
                //        MessageBox.Show(string.Format("上传结束！[{0}/{1}]", uploadall.ToString(), countall.ToString()));
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("上传失败！" + ex.Message );
                progressBar1.Visible = false;
                progressBar1.Value = 0;
                progressBar1.Step = 1;
            }
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

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_all_Click(object sender, EventArgs e)
        {
            DataTable dt = ((DataTable)dataGridView_list.DataSource).Copy();
            if (dt != null && dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["xz"] = true;
                }

                dataGridView_list.DataSource = dt;
                dt_tem = dt.Copy();
            }

        }
        /// <summary>
        /// 全不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_no_Click(object sender, EventArgs e)
        {
            DataTable dt = ((DataTable)dataGridView_list.DataSource).Copy();
            if (dt != null && dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["xz"] = false;
                }
                dataGridView_list.DataSource = dt;
                dt_tem = dt.Copy();
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

                    if (checkbox != null &&   checkbox.Value.ToString() == "1")
                    {
                        dt_tem.Rows[e.RowIndex]["xz"] = "1";
                        dt_tem.AcceptChanges();
                    }
                    else
                    {
                        dt_tem.Rows[e.RowIndex]["xz"] = "0";
                        dt_tem.AcceptChanges();
                    }
                }
            }



        }
    }
}
