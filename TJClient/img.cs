using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TJClient
{
    public partial class img : Form
    {
        Bitmap bufferpic;//加快GDI读取用缓存图片

        Size stopScalingSize; // 图片停止缩放时图片大小（如图片大于窗体大小则为窗体大小）
        int mouse_offset_x = 0; // 鼠标x位置与位置中心的偏移量
        int mouse_offset_y = 0;// 鼠标y位置与位置中心的偏移量
        float scale_x = 1f; //图片x位置变化幅度
        float scale_y = 1f; //图片y位置变化幅度

        Point mouseOriginalLocation; //鼠标原始位置
        int mouse_move_offset_x = 0; //鼠标移动x方向上的偏移量
        int mouse_move_offset_y = 0; //鼠标移动y方向上的偏移量
        Point picLocation = new Point(); //图片当前位置


        public img()
        {
            InitializeComponent();
            //双缓存
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
        }

        public bool setImg(Image img)
        {

            bufferpic = new Bitmap(img);
            pictureBox_img.Image = bufferpic;


            //pictureBox_img.ImageLocation = imgstr;
            return true;
        }

        private void img_Load(object sender, EventArgs e)
        {
            Init();

            this.pictureBox_img.MouseWheel += new MouseEventHandler(panel1_MouseWheel);//鼠标滚轮事件
        }

        //鼠标事件
        private void panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouse_offset_x = e.X - this.pictureBox_img.Width / 2;
            mouse_offset_y = e.Y - this.pictureBox_img.Height / 2;

            scale_x = GetLocationScale(pictureBox_img.Width / 2, mouse_offset_x);
            scale_y = GetLocationScale(pictureBox_img.Height / 2, mouse_offset_y);

            System.Drawing.Size t = pictureBox_img.Size;
            t.Width += e.Delta;
            t.Height += e.Delta;

            Point p = picLocation;
            p.X += (int)(((float)(pictureBox_img.Width - t.Width)) / 2 * scale_x);
            p.Y += (int)(((float)(pictureBox_img.Height - t.Height)) / 2 * scale_y);

            if (t.Width > stopScalingSize.Width || t.Height > stopScalingSize.Height)
            {
                pictureBox_img.Width = t.Width;
                pictureBox_img.Height = t.Height;
                picLocation = p;
            }
            this.pictureBox_img.Location = picLocation;
        }


        private void Init()
        {
            float p1 = (float)this.pictureBox_img.Image.Width / (float)this.pictureBox_img.Image.Height; //图片宽高比
            float p2 = (float)this.ClientRectangle.Width / (float)this.ClientRectangle.Height; //窗体宽高比
            if (p1 > p2)
            {
                if (this.pictureBox_img.Image.Width > this.ClientRectangle.Width)
                {
                    this.pictureBox_img.Width = this.ClientRectangle.Width;
                    this.pictureBox_img.Height = (int)((float)this.pictureBox_img.Width / p1);
                }
                else
                {
                    this.pictureBox_img.Size = this.pictureBox_img.Image.Size;
                }
            }
            else
            {
                if (this.pictureBox_img.Image.Height > this.ClientRectangle.Height)
                {
                    this.pictureBox_img.Height = this.ClientRectangle.Height;
                    this.pictureBox_img.Width = (int)((float)this.pictureBox_img.Height * p1);
                }
                else
                {
                    this.pictureBox_img.Size = this.pictureBox_img.Image.Size;
                }

            }
            picLocation = new Point((this.ClientRectangle.Width - this.pictureBox_img.Width) / 2, (this.ClientRectangle.Height - this.pictureBox_img.Height) / 2);
            this.pictureBox_img.Location = picLocation;
            stopScalingSize = new Size(this.pictureBox_img.Width, this.pictureBox_img.Height);
        }
        private void pictureBox_img_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBox_img.Focus();
        }

        private void pictureBox_img_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouse_move_offset_x = mouseOriginalLocation.X - e.Location.X;
                mouse_move_offset_y = mouseOriginalLocation.Y - e.Location.Y;

                picLocation.X = this.pictureBox_img.Location.X - mouse_move_offset_x;
                picLocation.Y = this.pictureBox_img.Location.Y - mouse_move_offset_y;
                this.pictureBox_img.Location = picLocation;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        private void pictureBox_img_MouseDown(object sender, MouseEventArgs e)
        {
            mouseOriginalLocation = e.Location; //记录下鼠标原始位置
            Cursor = Cursors.SizeAll;
        }

        /// <summary>
        /// 计算图片坐标变化幅度
        /// </summary>
        /// <param name="range"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private float GetLocationScale(int range, int offset)
        {
            float s = 1f;
            if (offset < 0)
            {
                s = 1f - (float)(-offset) / (float)range;
            }
            else if (offset > 0)
            {
                s = 1f + (float)offset / (float)range;
            }
            return s;
        }

        private void PicViewForm_Resize(object sender, EventArgs e)
        {
            Init();
        }


        private void img_Resize(object sender, EventArgs e)
        {

            pictureBox_img.Width = this.Width;
            pictureBox_img.Height  = this.Height ;

        }

    }
}
