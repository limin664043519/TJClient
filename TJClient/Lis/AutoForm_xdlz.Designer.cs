namespace FBYClient
{
    partial class AutoForm_xdlz
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoForm_xdlz));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_lis = new System.Windows.Forms.Timer(this.components);
            this.btn_bcstop = new System.Windows.Forms.Button();
            this.button_again = new System.Windows.Forms.Button();
            this.btn_bcstart = new System.Windows.Forms.Button();
            this.label_title = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "EDANSE300心电同步接口";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出系统ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // 退出系统ToolStripMenuItem
            // 
            this.退出系统ToolStripMenuItem.Name = "退出系统ToolStripMenuItem";
            this.退出系统ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出系统ToolStripMenuItem.Text = "退出系统";
            this.退出系统ToolStripMenuItem.Click += new System.EventHandler(this.退出系统ToolStripMenuItem_Click);
            // 
            // timer_lis
            // 
            this.timer_lis.Interval = 1000;
            this.timer_lis.Tick += new System.EventHandler(this.timer_lis_Tick);
            // 
            // btn_bcstop
            // 
            this.btn_bcstop.Location = new System.Drawing.Point(207, 86);
            this.btn_bcstop.Name = "btn_bcstop";
            this.btn_bcstop.Size = new System.Drawing.Size(75, 23);
            this.btn_bcstop.TabIndex = 35;
            this.btn_bcstop.Text = "结束";
            this.btn_bcstop.UseVisualStyleBackColor = true;
            this.btn_bcstop.Click += new System.EventHandler(this.btn_bcstop_Click);
            // 
            // button_again
            // 
            this.button_again.Location = new System.Drawing.Point(97, 133);
            this.button_again.Name = "button_again";
            this.button_again.Size = new System.Drawing.Size(185, 23);
            this.button_again.TabIndex = 33;
            this.button_again.Text = "重新获取检查结果";
            this.button_again.UseVisualStyleBackColor = true;
            this.button_again.Visible = false;
            this.button_again.Click += new System.EventHandler(this.button_again_Click);
            // 
            // btn_bcstart
            // 
            this.btn_bcstart.Location = new System.Drawing.Point(97, 86);
            this.btn_bcstart.Name = "btn_bcstart";
            this.btn_bcstart.Size = new System.Drawing.Size(75, 23);
            this.btn_bcstart.TabIndex = 34;
            this.btn_bcstart.Text = "开始";
            this.btn_bcstart.UseVisualStyleBackColor = true;
            this.btn_bcstart.Click += new System.EventHandler(this.btn_bcstart_Click);
            // 
            // label_title
            // 
            this.label_title.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_title.ForeColor = System.Drawing.Color.Red;
            this.label_title.Location = new System.Drawing.Point(34, 33);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(248, 23);
            this.label_title.TabIndex = 32;
            this.label_title.Text = "心电结果处理中。。。。";
            // 
            // AutoForm_xdlz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 230);
            this.Controls.Add(this.btn_bcstop);
            this.Controls.Add(this.button_again);
            this.Controls.Add(this.btn_bcstart);
            this.Controls.Add(this.label_title);
            this.Name = "AutoForm_xdlz";
            this.Text = "心电";
            this.Load += new System.EventHandler(this.AutoForm_xdlz_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出系统ToolStripMenuItem;
        private System.Windows.Forms.Timer timer_lis;
        private System.Windows.Forms.Button btn_bcstop;
        private System.Windows.Forms.Button button_again;
        private System.Windows.Forms.Button btn_bcstart;
        private System.Windows.Forms.Label label_title;
    }
}