using System.Windows.Forms;
namespace FBYClient
{
    partial class LnrDataUp
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
            this.shapeContainer13 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.SuspendLayout();
            // 
            // shapeContainer13
            // 
            this.shapeContainer13.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer13.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer13.Name = "shapeContainer13";
            this.shapeContainer13.Size = new System.Drawing.Size(1138, 86);
            this.shapeContainer13.TabIndex = 305;
            this.shapeContainer13.TabStop = false;
            // 
            // LnrDataUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(287, 148);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LnrDataUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "数据上传";
            this.Load += new System.EventHandler(this.jktj_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer13;
    }
}