namespace TJClient.sys
{
    partial class Form_gzzRy
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_del = new System.Windows.Forms.Button();
            this.textBox_pym = new System.Windows.Forms.TextBox();
            this.label_pym = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bm = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.mc = new System.Windows.Forms.TextBox();
            this.button_clear = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(830, 361);
            this.panel1.TabIndex = 97;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(241)))), ((int)(((byte)(251)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(830, 361);
            this.dataGridView1.TabIndex = 107;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_del);
            this.panel2.Controls.Add(this.textBox_pym);
            this.panel2.Controls.Add(this.label_pym);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.bm);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.mc);
            this.panel2.Controls.Add(this.button_clear);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(830, 89);
            this.panel2.TabIndex = 98;
            // 
            // button_del
            // 
            this.button_del.Location = new System.Drawing.Point(594, 50);
            this.button_del.Name = "button_del";
            this.button_del.Size = new System.Drawing.Size(49, 26);
            this.button_del.TabIndex = 106;
            this.button_del.Text = "删除";
            this.button_del.UseVisualStyleBackColor = true;
            this.button_del.Click += new System.EventHandler(this.button_del_Click);
            // 
            // textBox_pym
            // 
            this.textBox_pym.Location = new System.Drawing.Point(428, 12);
            this.textBox_pym.Name = "textBox_pym";
            this.textBox_pym.Size = new System.Drawing.Size(187, 21);
            this.textBox_pym.TabIndex = 100;
            this.textBox_pym.Visible = false;
            this.textBox_pym.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_pym_KeyDown);
            // 
            // label_pym
            // 
            this.label_pym.AutoSize = true;
            this.label_pym.Location = new System.Drawing.Point(383, 18);
            this.label_pym.Name = "label_pym";
            this.label_pym.Size = new System.Drawing.Size(41, 12);
            this.label_pym.TabIndex = 108;
            this.label_pym.Text = "拼音码";
            this.label_pym.Visible = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(249, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 1);
            this.label5.TabIndex = 107;
            this.label5.Text = "asdfasdf";
            this.label5.Visible = false;
            // 
            // bm
            // 
            this.bm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bm.ForeColor = System.Drawing.Color.Blue;
            this.bm.Location = new System.Drawing.Point(249, 12);
            this.bm.Name = "bm";
            this.bm.Size = new System.Drawing.Size(100, 14);
            this.bm.TabIndex = 101;
            this.bm.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(380, 50);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(49, 26);
            this.button3.TabIndex = 103;
            this.button3.Text = "查询";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(81, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(280, 1);
            this.label3.TabIndex = 104;
            this.label3.Text = "asdfasdf";
            // 
            // mc
            // 
            this.mc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mc.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mc.ForeColor = System.Drawing.Color.Blue;
            this.mc.Location = new System.Drawing.Point(81, 57);
            this.mc.Name = "mc";
            this.mc.Size = new System.Drawing.Size(280, 14);
            this.mc.TabIndex = 102;
            this.mc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_Tab_KeyDown);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(435, 50);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(49, 26);
            this.button_clear.TabIndex = 104;
            this.button_clear.Text = "清空";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(529, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(49, 26);
            this.button1.TabIndex = 105;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 99;
            this.label4.Text = "名称";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(219, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 97;
            this.label1.Text = "编码";
            this.label1.Visible = false;
            // 
            // Form_xmfl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(830, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_xmfl";
            this.Text = "数据字典管理";
            this.Load += new System.EventHandler(this.para_load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox bm;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mc;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Label label_pym;
        private System.Windows.Forms.TextBox textBox_pym;
        private System.Windows.Forms.Button button_del;
    }
}