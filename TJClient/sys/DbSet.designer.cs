namespace Yibaoxiao.sysmain
{
    partial class DbSet
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
            this.label1 = new System.Windows.Forms.Label();
            this.button_DB_clear = new System.Windows.Forms.Button();
            this.button_exec = new System.Windows.Forms.Button();
            this.richTextBox_sql = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库设定";
            // 
            // button_DB_clear
            // 
            this.button_DB_clear.Location = new System.Drawing.Point(189, 12);
            this.button_DB_clear.Name = "button_DB_clear";
            this.button_DB_clear.Size = new System.Drawing.Size(90, 30);
            this.button_DB_clear.TabIndex = 5;
            this.button_DB_clear.Text = "清空数据库";
            this.button_DB_clear.UseVisualStyleBackColor = true;
            this.button_DB_clear.Click += new System.EventHandler(this.button_DB_clear_Click);
            // 
            // button_exec
            // 
            this.button_exec.Location = new System.Drawing.Point(387, 16);
            this.button_exec.Name = "button_exec";
            this.button_exec.Size = new System.Drawing.Size(75, 23);
            this.button_exec.TabIndex = 6;
            this.button_exec.Text = "执行sql";
            this.button_exec.UseVisualStyleBackColor = true;
            this.button_exec.Click += new System.EventHandler(this.button_exec_Click);
            // 
            // richTextBox_sql
            // 
            this.richTextBox_sql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_sql.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_sql.Name = "richTextBox_sql";
            this.richTextBox_sql.Size = new System.Drawing.Size(667, 372);
            this.richTextBox_sql.TabIndex = 7;
            this.richTextBox_sql.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_DB_clear);
            this.panel1.Controls.Add(this.button_exec);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(667, 62);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.richTextBox_sql);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(667, 372);
            this.panel2.TabIndex = 9;
            // 
            // DbSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(667, 434);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DbSet";
            this.Load += new System.EventHandler(this.printSet_load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_DB_clear;
        private System.Windows.Forms.Button button_exec;
        private System.Windows.Forms.RichTextBox richTextBox_sql;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}