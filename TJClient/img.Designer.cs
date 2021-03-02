namespace TJClient
{
    partial class img
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox_img = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_img)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_img
            // 
            this.pictureBox_img.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pictureBox_img.Location = new System.Drawing.Point(3, 1);
            this.pictureBox_img.Name = "pictureBox_img";
            this.pictureBox_img.Size = new System.Drawing.Size(735, 448);
            this.pictureBox_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_img.TabIndex = 0;
            this.pictureBox_img.TabStop = false;
            this.pictureBox_img.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_img_MouseMove);
            this.pictureBox_img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_img_MouseDown);
            this.pictureBox_img.MouseEnter += new System.EventHandler(this.pictureBox_img_MouseEnter);
            // 
            // img
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 451);
            this.Controls.Add(this.pictureBox_img);
            this.Name = "img";
            this.Text = "图形";
            this.Load += new System.EventHandler(this.img_Load);
            this.Resize += new System.EventHandler(this.img_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_img)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_img;
    }
}