using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Xml;
using System.IO;

namespace TJClient
{
    public partial class Form_test : Form
    {

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private const int WM_NCPAINT = 0x0085;
        private const int WM_NCACTIVATE = 0x0086;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int WM_DESTROY = 0x0002;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            Rectangle vRectangle = new Rectangle(3, 3, Width - 6, 21);
            switch (m.Msg)
            {
                default:
                    //MessageBox.Show(m.Msg.ToString()); break;
                case WM_NCPAINT:
                case WM_NCACTIVATE:
                    //IntPtr vHandle = GetWindowDC(m.HWnd);
                    //Graphics vGraphics = Graphics.FromHdc(vHandle);
                    //vGraphics.FillRectangle(new LinearGradientBrush(vRectangle,
                    //    Color.Pink, Color.Purple, LinearGradientMode.BackwardDiagonal),
                    //    vRectangle);

                    //StringFormat vStringFormat = new StringFormat();
                    //vStringFormat.Alignment = StringAlignment.Center;
                    //vStringFormat.LineAlignment = StringAlignment.Center;
                    //vGraphics.DrawString("Form1", Font, Brushes.BlanchedAlmond,
                    //    vRectangle, vStringFormat);

                    //vGraphics.Dispose();
                    //ReleaseDC(m.HWnd, vHandle);
                    break;
                case WM_NCLBUTTONDOWN:
                    Point vPoint = new Point((int)m.LParam);
                    vPoint.Offset(-Left, -Top);
                    if (vRectangle.Contains(vPoint))
                        MessageBox.Show(vPoint.ToString());
                    //break;
                    IntPtr vHandle = GetWindowDC(m.HWnd);
                    Graphics vGraphics = Graphics.FromHdc(vHandle);
                    vGraphics.FillRectangle(new LinearGradientBrush(vRectangle,
                        Color.Pink, Color.Purple, LinearGradientMode.BackwardDiagonal),
                        vRectangle);

                    StringFormat vStringFormat = new StringFormat();
                    vStringFormat.Alignment = StringAlignment.Center;
                    vStringFormat.LineAlignment = StringAlignment.Center;
                    vGraphics.DrawString("Form1", Font, Brushes.BlanchedAlmond,
                        vRectangle, vStringFormat);

                    vGraphics.Dispose();
                    ReleaseDC(m.HWnd, vHandle);
                    break;
                
            }

        }








        public  DataTable dtPage=null;

        public DataTable dtAll = null;

        public Form_test()
        {
            InitializeComponent();
        }


        public DataTable createTable()
        {
            dtAll = new DataTable();

            dtAll.Columns.Add("a1");
            dtAll.Columns.Add("a2");
            dtAll.Columns.Add("a3");

            for (int i = 0; i < 100; i++)
            {
                dtAll.Rows.Add();
                dtAll.Rows[i]["a1"] = i;
            }

            return dtAll;
        }


        public DataTable GetAll(int start, int end)
        {
            dtPage = new DataTable();
            dtPage = dtAll.Clone();
            for (int i = start; ((i < dtAll.Rows.Count) && (i <= end)); i++)
            {
                dtPage.ImportRow(dtAll.Rows[i]);
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
            int start = (pager1.PageSize * (pager1.PageCurrent - 1) + 1);
            int  end = (pager1.PageSize * pager1.PageCurrent);

            //数据源   
            dtPage = GetAll(start, end);
            //绑定分页控件   
            pager1.bindingSource1.DataSource = dtPage;
            pager1.bindingNavigator1.BindingSource = pager1.bindingSource1;
            //讲分页控件绑定DataGridView   
            dataGridView1.DataSource = pager1.bindingSource1;
            //返回总记录数   
            return dtAll.Rows.Count;
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
            pager1.PageSize = 15;//页数   
            this.pager1.Bind();//绑定  
            #endregion
        }

        private void Form_test_Load(object sender, EventArgs e)
        {
            createTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {

             
            //调用接口用对象
            GwtjDoService.GwtjDoService gwtjdoservice = new GwtjDoService.GwtjDoService();
            //gwtjdoservice.Url = "http://localhost:3699/GwtjDoService.asmx";

            string mainXml = loadXmlFile("E:\\ceshi.xml");
            string   result = gwtjdoservice.DoServiceGwtj(mainXml, "");
        }

        private string loadXmlFile(string fileName)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            doc.Save(writer);

            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return xmlString;
        }

    }
}
