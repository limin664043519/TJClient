namespace TJClient
{
    partial class DropDownGrid
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_PYM = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.姓名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D_SFZH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D_CSRQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D_XXDZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.textBox_PYM);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(630, 21);
            this.panel1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(200, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 20);
            this.button1.TabIndex = 3;
            this.button1.Text = "关闭";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox_PYM
            // 
            this.textBox_PYM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_PYM.Location = new System.Drawing.Point(0, 0);
            this.textBox_PYM.Name = "textBox_PYM";
            this.textBox_PYM.Size = new System.Drawing.Size(196, 21);
            this.textBox_PYM.TabIndex = 2;
            this.textBox_PYM.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PYM_KeyDown);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.姓名,
            this.类型,
            this.D_SFZH,
            this.D_CSRQ,
            this.D_XXDZ});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 21);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(630, 301);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ddgrid_KeyDown);
            // 
            // 姓名
            // 
            this.姓名.DataPropertyName = "D_XM";
            this.姓名.HeaderText = "姓名";
            this.姓名.Name = "姓名";
            this.姓名.ReadOnly = true;
            this.姓名.Width = 80;
            // 
            // 类型
            // 
            this.类型.DataPropertyName = "D_ZJLX";
            this.类型.HeaderText = "Column1";
            this.类型.Name = "类型";
            this.类型.ReadOnly = true;
            this.类型.Visible = false;
            // 
            // D_SFZH
            // 
            this.D_SFZH.DataPropertyName = "D_SFZH";
            this.D_SFZH.HeaderText = "证件号码";
            this.D_SFZH.Name = "D_SFZH";
            this.D_SFZH.ReadOnly = true;
            this.D_SFZH.Width = 200;
            // 
            // D_CSRQ
            // 
            this.D_CSRQ.DataPropertyName = "D_CSRQ";
            this.D_CSRQ.HeaderText = "出生日期";
            this.D_CSRQ.Name = "D_CSRQ";
            this.D_CSRQ.ReadOnly = true;
            this.D_CSRQ.Width = 120;
            // 
            // D_XXDZ
            // 
            this.D_XXDZ.DataPropertyName = "D_XXDZ";
            this.D_XXDZ.HeaderText = "详细地址";
            this.D_XXDZ.Name = "D_XXDZ";
            this.D_XXDZ.ReadOnly = true;
            this.D_XXDZ.Width = 200;
            // 
            // DropDownGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(630, 322);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DropDownGrid";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "查询";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DdGrid_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_PYM;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 姓名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn D_SFZH;
        private System.Windows.Forms.DataGridViewTextBoxColumn D_CSRQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn D_XXDZ;
    }
}