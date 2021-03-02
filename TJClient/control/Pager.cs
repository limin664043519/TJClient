#region  版权信息
/*---------------------------------------------------------------------*
// 项目  名称：《Winform分页控件》
// 文  件  名： Pager.cs
// 描      述： 分页控件
// 作      者：kwon yan
*----------------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HuishengFS.Controls
{
    /**/
    /// <summary>
    /// 申明委托
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate int EventPagingHandler(EventPagingArg e);
    /**/
    /// <summary>
    /// 分页控件呈现
    /// </summary>
    public partial class Pager : UserControl
    {
        public Pager()
        {
            InitializeComponent();
        }
        public event EventPagingHandler EventPaging;
        /**/
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        private int _pageSize = 50;
        /**/
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                GetPageCount();
                toolStripComboBox1.Text = value.ToString();
            }
        }

        private int _nMax = 0;
        /**/
        /// <summary>
        /// 总记录数
        /// </summary>
        public int NMax
        {
            get { return _nMax; }
            set
            {
                _nMax = value;
                GetPageCount();
            }
        }

        private int _pageCount = 0;
        /**/
        /// <summary>
        /// 页数=总记录数/每页显示记录数
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        private int _pageCurrent = 1;
        /**/
        /// <summary>
        /// 当前页号
        /// </summary>
        public int PageCurrent
        {
            get { return _pageCurrent; }
            set { _pageCurrent = value; }
        }

        /// <summary>
        /// 设置页面大小
        /// </summary>
        private void GetPageCount()
        {
            if (this.NMax > 0)
            {
                this.PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.NMax) / Convert.ToDouble(this.PageSize)));
                lblPageCount.Text = " / " + PageCount.ToString();
                lblPageCount1.Text = " ，共 "+PageCount.ToString()+" 页";

                //lblPageCount1.Text = "Page no: " + PageSize.ToString() + ",Total:" + PageCount.ToString() + " pages";
            }
            else
            {
                this.PageCount = 0;
            }
        }

        /// <summary>
        /// 控件状态初始化
        /// </summary>
        public void init()
        {
            //清空页号的列表
            cmbPagecount.Items.Clear();
            this.PageCount = 1;
            _pageCurrent = 1;
            lblPageCount.Text = "";
            lblcurentpage.Text = "";
            lblPageCount1.Text = "";
            if (toolStripComboBox1.Text == null || toolStripComboBox1.Text.Length == 0)
            {
                toolStripComboBox1.Text = _pageSize.ToString();
            }
        }


        /**/
        /// <summary>
        /// 翻页控件数据绑定的方法 关键是这步，都是调用这里
        /// </summary>
        public void Bind()
        {
            if (this.EventPaging != null)
            {
                this.NMax = this.EventPaging(new EventPagingArg(this.PageCurrent));
            }

            if (this.PageCurrent > this.PageCount)
            {
                this.PageCurrent = this.PageCount;
            }
            if (this.PageCount == 1)
            {
                this.PageCurrent = 1;
            }
            lblcurentpage.Text = PageCurrent.ToString();
            lblRecordCount.Text = "共有 " + NMax.ToString() + " 条记录";
            //lblRecordCount.Text = "Total: " + NMax.ToString() + " records";
          

            btnPrev.Enabled = true;
            btnFirst.Enabled = true;
            btnLast.Enabled = true;
            btnNext.Enabled = true;

            if (this.PageCurrent == 1)
            {
                this.btnPrev.Enabled = false;
                this.btnFirst.Enabled = false;
            }
      

            if (this.PageCurrent == this.PageCount)
            {
                this.btnLast.Enabled = false;
                this.btnNext.Enabled = false;
            }
     
            if (this.NMax == 0)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
            }

            if (cmbPagecount.Items.Count == 0)
            {
                cmbPagecount.Items.Clear();
                for (int i = 1; i <= PageCount; i++)
                    cmbPagecount.Items.Add(i.ToString());
            }

            cmbPagecount.SelectedIndex = PageCurrent - 1;
            
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFirst_Click(object sender, EventArgs e)
        {
            PageCurrent = 1;
            this.Bind();
        }
        //上一页
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, EventArgs e)
        {
            PageCurrent -= 1;
            if (PageCurrent <= 0)
            {
                PageCurrent = 1;
            }
            this.Bind();
        }
        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            this.PageCurrent += 1;
            if (PageCurrent > PageCount)
            {
                PageCurrent = PageCount;
            }
            this.Bind();
        }
        /// <summary>
        /// 最后页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLast_Click(object sender, EventArgs e)
        {
            PageCurrent = PageCount;
            this.Bind();
        }
        /// <summary>
        /// 转到新页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnGo_Click(object sender, EventArgs e)
        {
            if ( cmbPagecount.SelectedItem !=null && Int32.TryParse(cmbPagecount.SelectedItem.ToString(), out _pageCurrent))
            {
                this.Bind();
            } 
        }

        private void cmbPagecount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Int32.Parse(cmbPagecount.SelectedItem.ToString()) == 1)
            {
                return;
            }
            if (Int32.TryParse(cmbPagecount.SelectedItem.ToString(), out _pageCurrent))
            {
                this.Bind();
            } 
        }

        /// <summary>
        /// 设定每页的显示条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(toolStripComboBox1.Text.ToString(), out _pageSize))
            {
                //this.PageSize = Convert.ToInt32(toolStripComboBox1.Text.ToString());

                cmbPagecount.Items.Clear();
                this.Bind();
            }
        }
    }
    /**/
    /// <summary>
    /// 自定义事件数据基类
    /// </summary>
    public class EventPagingArg : EventArgs
    {
        private int _intPageIndex;
        public EventPagingArg(int PageIndex)
        {
            _intPageIndex = PageIndex;
        }
    }
}
