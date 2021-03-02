namespace FBYClient
{
    partial class update_tj
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel_top = new System.Windows.Forms.Panel();
            this.panel_top_top = new System.Windows.Forms.Panel();
            this.label_jd = new System.Windows.Forms.Label();
            this.label_title = new System.Windows.Forms.Label();
            this.panel_body = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.RichTextBox();
            this.panel_top.SuspendLayout();
            this.panel_top_top.SuspendLayout();
            this.panel_body.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(0, 34);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(574, 46);
            this.progressBar1.TabIndex = 0;
            // 
            // panel_top
            // 
            this.panel_top.Controls.Add(this.progressBar1);
            this.panel_top.Controls.Add(this.panel_top_top);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(574, 80);
            this.panel_top.TabIndex = 2;
            // 
            // panel_top_top
            // 
            this.panel_top_top.Controls.Add(this.label_jd);
            this.panel_top_top.Controls.Add(this.label_title);
            this.panel_top_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top_top.Name = "panel_top_top";
            this.panel_top_top.Size = new System.Drawing.Size(574, 34);
            this.panel_top_top.TabIndex = 0;
            // 
            // label_jd
            // 
            this.label_jd.BackColor = System.Drawing.SystemColors.Control;
            this.label_jd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_jd.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_jd.Location = new System.Drawing.Point(300, 0);
            this.label_jd.Name = "label_jd";
            this.label_jd.Size = new System.Drawing.Size(274, 34);
            this.label_jd.TabIndex = 1;
            this.label_jd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_title
            // 
            this.label_title.BackColor = System.Drawing.SystemColors.Control;
            this.label_title.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_title.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_title.Location = new System.Drawing.Point(0, 0);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(300, 34);
            this.label_title.TabIndex = 1;
            this.label_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel_body
            // 
            this.panel_body.AutoScroll = true;
            this.panel_body.AutoSize = true;
            this.panel_body.Controls.Add(this.lblMsg);
            this.panel_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_body.Location = new System.Drawing.Point(0, 80);
            this.panel_body.Name = "panel_body";
            this.panel_body.Size = new System.Drawing.Size(574, 236);
            this.panel_body.TabIndex = 3;
            // 
            // lblMsg
            // 
            this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMsg.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMsg.Location = new System.Drawing.Point(0, 0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(574, 236);
            this.lblMsg.TabIndex = 4;
            this.lblMsg.Text = "";
            // 
            // update_tj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 316);
            this.Controls.Add(this.panel_body);
            this.Controls.Add(this.panel_top);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "update_tj";
            this.Text = "数据下载";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.update_FormClosing);
            this.Load += new System.EventHandler(this.update_Load);
            this.panel_top.ResumeLayout(false);
            this.panel_top_top.ResumeLayout(false);
            this.panel_body.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Label label_jd;
        private System.Windows.Forms.Panel panel_body;
        private System.Windows.Forms.Panel panel_top_top;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.RichTextBox lblMsg;
    }
}

