﻿namespace TJClient.UploadInfo
{
    partial class FrmUploadInfos
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
            this.rt = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rt
            // 
            this.rt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rt.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rt.Location = new System.Drawing.Point(0, 0);
            this.rt.Name = "rt";
            this.rt.Size = new System.Drawing.Size(716, 563);
            this.rt.TabIndex = 0;
            this.rt.Text = "";
            // 
            // FrmUploadInfos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 563);
            this.Controls.Add(this.rt);
            this.Name = "FrmUploadInfos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "上传日志";
            this.Load += new System.EventHandler(this.FrmUploadInfos_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rt;
    }
}