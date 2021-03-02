namespace TJClient.sys
{
    partial class Form_PageSet
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
            this.button_return = new System.Windows.Forms.Button();
            this.button_true = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_showZdrq_print = new System.Windows.Forms.CheckBox();
            this.checkBox_showAll_print = new System.Windows.Forms.CheckBox();
            this.checkBox_zdrq_print = new System.Windows.Forms.CheckBox();
            this.checkBox_nlcheck_print = new System.Windows.Forms.CheckBox();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_return);
            this.panel2.Controls.Add(this.button_true);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(594, 246);
            this.panel2.TabIndex = 98;
            // 
            // button_return
            // 
            this.button_return.Location = new System.Drawing.Point(508, 12);
            this.button_return.Name = "button_return";
            this.button_return.Size = new System.Drawing.Size(65, 23);
            this.button_return.TabIndex = 115;
            this.button_return.Text = "返回";
            this.button_return.UseVisualStyleBackColor = true;
            this.button_return.Click += new System.EventHandler(this.button_return_Click);
            // 
            // button_true
            // 
            this.button_true.Location = new System.Drawing.Point(432, 12);
            this.button_true.Name = "button_true";
            this.button_true.Size = new System.Drawing.Size(65, 23);
            this.button_true.TabIndex = 114;
            this.button_true.Text = "确定";
            this.button_true.UseVisualStyleBackColor = true;
            this.button_true.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_showZdrq_print);
            this.groupBox1.Controls.Add(this.checkBox_showAll_print);
            this.groupBox1.Controls.Add(this.checkBox_zdrq_print);
            this.groupBox1.Controls.Add(this.checkBox_nlcheck_print);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 186);
            this.groupBox1.TabIndex = 111;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "条码打印设定";
            // 
            // checkBox_showZdrq_print
            // 
            this.checkBox_showZdrq_print.AutoSize = true;
            this.checkBox_showZdrq_print.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_showZdrq_print.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.checkBox_showZdrq_print.Location = new System.Drawing.Point(297, 55);
            this.checkBox_showZdrq_print.Name = "checkBox_showZdrq_print";
            this.checkBox_showZdrq_print.Size = new System.Drawing.Size(234, 24);
            this.checkBox_showZdrq_print.TabIndex = 112;
            this.checkBox_showZdrq_print.Text = "显示条码选择(重点人群没有设定)";
            this.checkBox_showZdrq_print.UseVisualStyleBackColor = true;
            // 
            // checkBox_showAll_print
            // 
            this.checkBox_showAll_print.AutoSize = true;
            this.checkBox_showAll_print.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_showAll_print.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.checkBox_showAll_print.Location = new System.Drawing.Point(297, 25);
            this.checkBox_showAll_print.Name = "checkBox_showAll_print";
            this.checkBox_showAll_print.Size = new System.Drawing.Size(178, 24);
            this.checkBox_showAll_print.TabIndex = 111;
            this.checkBox_showAll_print.Text = "显示条码选择(每次打印)";
            this.checkBox_showAll_print.UseVisualStyleBackColor = true;
            // 
            // checkBox_zdrq_print
            // 
            this.checkBox_zdrq_print.AutoSize = true;
            this.checkBox_zdrq_print.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_zdrq_print.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.checkBox_zdrq_print.Location = new System.Drawing.Point(96, 55);
            this.checkBox_zdrq_print.Name = "checkBox_zdrq_print";
            this.checkBox_zdrq_print.Size = new System.Drawing.Size(154, 24);
            this.checkBox_zdrq_print.TabIndex = 110;
            this.checkBox_zdrq_print.Text = "按重点人群打印条码";
            this.checkBox_zdrq_print.UseVisualStyleBackColor = true;
            // 
            // checkBox_nlcheck_print
            // 
            this.checkBox_nlcheck_print.AutoSize = true;
            this.checkBox_nlcheck_print.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_nlcheck_print.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.checkBox_nlcheck_print.Location = new System.Drawing.Point(96, 25);
            this.checkBox_nlcheck_print.Name = "checkBox_nlcheck_print";
            this.checkBox_nlcheck_print.Size = new System.Drawing.Size(154, 24);
            this.checkBox_nlcheck_print.TabIndex = 109;
            this.checkBox_nlcheck_print.Text = "打印条码时验证年龄";
            this.checkBox_nlcheck_print.UseVisualStyleBackColor = true;
            // 
            // Form_PageSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(594, 246);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_PageSet";
            this.Text = "数据字典管理";
            this.Load += new System.EventHandler(this.para_load);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_zdrq_print;
        private System.Windows.Forms.CheckBox checkBox_nlcheck_print;
        private System.Windows.Forms.CheckBox checkBox_showAll_print;
        private System.Windows.Forms.CheckBox checkBox_showZdrq_print;
        private System.Windows.Forms.Button button_return;
        private System.Windows.Forms.Button button_true;
    }
}