namespace FBYClient
{
    partial class AutoForm_bclzzyy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoForm_bclzzyy));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CloseTheForm = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.lab_bcstart = new System.Windows.Forms.Label();
            this.dateTimePicker_end = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_start = new System.Windows.Forms.DateTimePicker();
            this.btn_bcstop = new System.Windows.Forms.Button();
            this.button_again = new System.Windows.Forms.Button();
            this.btn_bcstart = new System.Windows.Forms.Button();
            this.label_title = new System.Windows.Forms.Label();
            this.btnLog = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "B超同步接口";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseTheForm});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // CloseTheForm
            // 
            this.CloseTheForm.Name = "CloseTheForm";
            this.CloseTheForm.Size = new System.Drawing.Size(124, 22);
            this.CloseTheForm.Text = "退出系统";
            this.CloseTheForm.Click += new System.EventHandler(this.CloseTheForm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(234, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "至";
            // 
            // lab_bcstart
            // 
            this.lab_bcstart.AutoSize = true;
            this.lab_bcstart.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_bcstart.Location = new System.Drawing.Point(32, 117);
            this.lab_bcstart.Name = "lab_bcstart";
            this.lab_bcstart.Size = new System.Drawing.Size(70, 14);
            this.lab_bcstart.TabIndex = 23;
            this.lab_bcstart.Text = "检验时间:";
            // 
            // dateTimePicker_end
            // 
            this.dateTimePicker_end.Checked = false;
            this.dateTimePicker_end.Location = new System.Drawing.Point(257, 114);
            this.dateTimePicker_end.Name = "dateTimePicker_end";
            this.dateTimePicker_end.ShowCheckBox = true;
            this.dateTimePicker_end.Size = new System.Drawing.Size(121, 21);
            this.dateTimePicker_end.TabIndex = 22;
            // 
            // dateTimePicker_start
            // 
            this.dateTimePicker_start.Checked = false;
            this.dateTimePicker_start.Location = new System.Drawing.Point(107, 114);
            this.dateTimePicker_start.Name = "dateTimePicker_start";
            this.dateTimePicker_start.ShowCheckBox = true;
            this.dateTimePicker_start.Size = new System.Drawing.Size(121, 21);
            this.dateTimePicker_start.TabIndex = 21;
            // 
            // btn_bcstop
            // 
            this.btn_bcstop.Location = new System.Drawing.Point(236, 174);
            this.btn_bcstop.Name = "btn_bcstop";
            this.btn_bcstop.Size = new System.Drawing.Size(75, 23);
            this.btn_bcstop.TabIndex = 20;
            this.btn_bcstop.Text = "结束";
            this.btn_bcstop.UseVisualStyleBackColor = true;
            this.btn_bcstop.Click += new System.EventHandler(this.btn_bcstop_Click);
            // 
            // button_again
            // 
            this.button_again.Location = new System.Drawing.Point(126, 221);
            this.button_again.Name = "button_again";
            this.button_again.Size = new System.Drawing.Size(75, 23);
            this.button_again.TabIndex = 18;
            this.button_again.Text = "重新获取检查结果";
            this.button_again.UseVisualStyleBackColor = true;
            this.button_again.Click += new System.EventHandler(this.button_again_Click);
            // 
            // btn_bcstart
            // 
            this.btn_bcstart.Location = new System.Drawing.Point(126, 174);
            this.btn_bcstart.Name = "btn_bcstart";
            this.btn_bcstart.Size = new System.Drawing.Size(75, 23);
            this.btn_bcstart.TabIndex = 19;
            this.btn_bcstart.Text = "开始";
            this.btn_bcstart.UseVisualStyleBackColor = true;
            this.btn_bcstart.Click += new System.EventHandler(this.btn_bcstart_Click);
            // 
            // label_title
            // 
            this.label_title.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_title.ForeColor = System.Drawing.Color.Red;
            this.label_title.Location = new System.Drawing.Point(22, 31);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(367, 23);
            this.label_title.TabIndex = 17;
            this.label_title.Text = "B超结果处理";
            // 
            // btnLog
            // 
            this.btnLog.Location = new System.Drawing.Point(236, 221);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(75, 23);
            this.btnLog.TabIndex = 51;
            this.btnLog.Text = "查看日志";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // AutoForm_bclzzyy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 278);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lab_bcstart);
            this.Controls.Add(this.dateTimePicker_end);
            this.Controls.Add(this.dateTimePicker_start);
            this.Controls.Add(this.btn_bcstop);
            this.Controls.Add(this.button_again);
            this.Controls.Add(this.btn_bcstart);
            this.Controls.Add(this.label_title);
            this.Name = "AutoForm_bclzzyy";
            this.Text = "B超";
            this.Load += new System.EventHandler(this.AutoForm_bclzzyy_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CloseTheForm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lab_bcstart;
        private System.Windows.Forms.DateTimePicker dateTimePicker_end;
        private System.Windows.Forms.DateTimePicker dateTimePicker_start;
        private System.Windows.Forms.Button btn_bcstop;
        private System.Windows.Forms.Button button_again;
        private System.Windows.Forms.Button btn_bcstart;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Button btnLog;
    }
}