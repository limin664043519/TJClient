namespace TJClient.sys
{
    partial class Form_ShowTmPrint
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_rq = new System.Windows.Forms.Label();
            this.label_sfzh = new System.Windows.Forms.Label();
            this.label_xm = new System.Windows.Forms.Label();
            this.button_return = new System.Windows.Forms.Button();
            this.button_true = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_rq);
            this.panel2.Controls.Add(this.label_sfzh);
            this.panel2.Controls.Add(this.label_xm);
            this.panel2.Controls.Add(this.button_return);
            this.panel2.Controls.Add(this.button_true);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(666, 282);
            this.panel2.TabIndex = 98;
            // 
            // label_rq
            // 
            this.label_rq.AutoSize = true;
            this.label_rq.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_rq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.label_rq.Location = new System.Drawing.Point(35, 41);
            this.label_rq.Name = "label_rq";
            this.label_rq.Size = new System.Drawing.Size(50, 20);
            this.label_rq.TabIndex = 118;
            this.label_rq.Text = "label1";
            // 
            // label_sfzh
            // 
            this.label_sfzh.AutoSize = true;
            this.label_sfzh.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_sfzh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.label_sfzh.Location = new System.Drawing.Point(244, 13);
            this.label_sfzh.Name = "label_sfzh";
            this.label_sfzh.Size = new System.Drawing.Size(50, 20);
            this.label_sfzh.TabIndex = 117;
            this.label_sfzh.Text = "label1";
            // 
            // label_xm
            // 
            this.label_xm.AutoSize = true;
            this.label_xm.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_xm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.label_xm.Location = new System.Drawing.Point(35, 13);
            this.label_xm.Name = "label_xm";
            this.label_xm.Size = new System.Drawing.Size(50, 20);
            this.label_xm.TabIndex = 116;
            this.label_xm.Text = "label1";
            // 
            // button_return
            // 
            this.button_return.Location = new System.Drawing.Point(550, 247);
            this.button_return.Name = "button_return";
            this.button_return.Size = new System.Drawing.Size(75, 28);
            this.button_return.TabIndex = 115;
            this.button_return.Text = "关闭";
            this.button_return.UseVisualStyleBackColor = true;
            this.button_return.Click += new System.EventHandler(this.button_return_Click);
            // 
            // button_true
            // 
            this.button_true.Location = new System.Drawing.Point(464, 247);
            this.button_true.Name = "button_true";
            this.button_true.Size = new System.Drawing.Size(75, 28);
            this.button_true.TabIndex = 114;
            this.button_true.Text = "打印";
            this.button_true.UseVisualStyleBackColor = true;
            this.button_true.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBox1);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(39, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(586, 173);
            this.groupBox1.TabIndex = 111;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "条码";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.ColumnWidth = 200;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(3, 21);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(30, 10, 10, 10);
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(580, 140);
            this.checkedListBox1.TabIndex = 0;
            // 
            // Form_ShowTmPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(666, 282);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_ShowTmPrint";
            this.Text = "数据字典管理";
            this.Load += new System.EventHandler(this.para_load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_return;
        private System.Windows.Forms.Button button_true;
        private System.Windows.Forms.Label label_xm;
        private System.Windows.Forms.Label label_rq;
        private System.Windows.Forms.Label label_sfzh;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
    }
}