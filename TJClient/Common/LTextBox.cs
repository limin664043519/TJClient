using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
//using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FBYClient
{
    class MyTextBox : TextBox
    {
        const int WM_ERASEBKGND = 0x0014;

        private Image backImage;

        [DisplayName("背景图片。")]
        public Image BackImage
        {
            get { return backImage; }
            set { backImage = value; }
        }

        protected void OnEraseBkgnd(Graphics gs)
        {
            gs.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height); //填充为白色，防止图片太小出现重影  
            if (backImage != null) gs.DrawImage(backImage, 0, 0); //绘制背景。  
            gs.Dispose();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_ERASEBKGND) //绘制背景  
            {
                OnEraseBkgnd(Graphics.FromHdc(m.WParam));
                m.Result = (IntPtr)1;
            }
            base.WndProc(ref m);
        }
    } 

    public class LTextBox : TextBox
    {
        const int WM_ERASEBKGND = 0x0014;
        private Image backImage; 
        public LTextBox()
        {
            //设置允许透明背景色
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.LineType = BorderType.None;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams ps = base.CreateParams;

                ps.ExStyle = ps.ExStyle | 0x20;
                return ps;
            }
        }

        #region 预先定义消息

        [Flags]
        public enum BorderType : int
        {
            /// <summary>
            /// 不绘制
            /// </summary>
            None = 0,
            /// <summary>
            /// 左侧
            /// </summary>
            Left = 1,
            /// <summary>
            /// 上侧
            /// </summary>
            Top = 2,
            /// <summary>
            /// 右侧
            /// </summary>
            Right = 4,
            /// <summary>
            /// 底部
            /// </summary>
            Bottom = 8
        }

        #endregion

        #region 属性

        BorderType _lineType;

        /// <summary>
        /// 要绘制的边框
        /// </summary>
       // [System.Web.Services.Description("要绘制的边框"), DefaultValue(BorderType.None)]
        public BorderType LineType
        {
            get
            { return _lineType; }

            set { _lineType = value; this.Invalidate(); }
        }

        Color _lineColor;

        /// <summary>
        /// 绘制的边框颜色
        /// </summary>
        //[System.Web.Services.Description("绘制的边框颜色")]
        public Color LineColor
        {
            get
            {
                return _lineColor;
            }
            set
            {
                _lineColor = value;
                this.Invalidate();
            }
        }

        #endregion

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// 重写消息处理
        /// </summary>
        /// <param name="m">消息</param>
        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);

            IntPtr hDC = GetWindowDC(m.HWnd);

            string sss = m.Msg.ToString();
            // MessageBox.Show(sss);


            if (hDC.ToInt32() == 0) return;

            if (this.BorderStyle == BorderStyle.None) return;

            switch (m.Msg)
            {
                case 0xf:
                case 0x85:
                case 0x133:

                    Graphics g = Graphics.FromHdc(hDC);

                    Pen p = new Pen(LineColor);
                    Pen b = new Pen(BackColor);

                    g.DrawLine(((LineType & BorderType.Bottom) == BorderType.Bottom) ? b : b, 0, this.Size.Height - 1, this.Size.Width - 1, this.Size.Height - 1);
                    g.DrawLine(((LineType & BorderType.Bottom) == BorderType.Bottom) ? p : b, 0, this.Size.Height - 4, this.Size.Width - 1, this.Size.Height - 4);
                    g.DrawLine(((LineType & BorderType.Left) == BorderType.Left) ? b : b, 0, 0, 0, this.Size.Height - 1);
                    g.DrawLine(((LineType & BorderType.Right) == BorderType.Right) ? b : b, this.Size.Width - 1, 0, this.Size.Width - 1, this.Size.Height - 1);
                    g.DrawLine(((LineType & BorderType.Top) == BorderType.Top) ? b : b, 0, 0, this.Size.Width - 1, 0);

                    g.Dispose();
                    m.Result = IntPtr.Zero;
                    ReleaseDC(m.HWnd, hDC);
                    break;
            }
        }
    }
}
