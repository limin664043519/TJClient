
namespace TJClient.ComForm
{
    partial class YWDropDownGrid
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
            this.dgv_yw = new System.Windows.Forms.DataGridView();
            this.txt_input = new System.Windows.Forms.TextBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YWMC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YWPYM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_yw)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_yw
            // 
            this.dgv_yw.AllowUserToAddRows = false;
            this.dgv_yw.AllowUserToDeleteRows = false;
            this.dgv_yw.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.dgv_yw.BackgroundColor = System.Drawing.Color.White;
            this.dgv_yw.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_yw.ColumnHeadersVisible = false;
            this.dgv_yw.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.YWMC,
            this.YWPYM});
            this.dgv_yw.ContextMenuStrip = this.contextMenuStrip1;
            this.dgv_yw.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgv_yw.Location = new System.Drawing.Point(0, 56);
            this.dgv_yw.MultiSelect = false;
            this.dgv_yw.Name = "dgv_yw";
            this.dgv_yw.ReadOnly = true;
            this.dgv_yw.RowHeadersWidth = 20;
            this.dgv_yw.RowTemplate.Height = 23;
            this.dgv_yw.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_yw.Size = new System.Drawing.Size(433, 239);
            this.dgv_yw.TabIndex = 7;
            this.dgv_yw.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_yw_CellDoubleClick);
            // 
            // txt_input
            // 
            this.txt_input.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_input.Location = new System.Drawing.Point(1, 28);
            this.txt_input.Name = "txt_input";
            this.txt_input.Size = new System.Drawing.Size(359, 26);
            this.txt_input.TabIndex = 8;
            this.txt_input.TextChanged += new System.EventHandler(this.txt_input_TextChanged);
            // 
            // btn_add
            // 
            this.btn_add.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_add.Location = new System.Drawing.Point(366, 0);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(67, 56);
            this.btn_add.TabIndex = 9;
            this.btn_add.Text = "新增";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(329, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "双击返回药品，若查询不到可点击右侧新增，右键可删除药品";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "YWMC";
            this.dataGridViewTextBoxColumn1.HeaderText = "药物名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 5;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "YWPYM";
            this.dataGridViewTextBoxColumn2.HeaderText = "药物名称(带拼音码)";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // YWMC
            // 
            this.YWMC.DataPropertyName = "YWMC";
            this.YWMC.HeaderText = "药物名称";
            this.YWMC.Name = "YWMC";
            this.YWMC.ReadOnly = true;
            this.YWMC.Visible = false;
            this.YWMC.Width = 5;
            // 
            // YWPYM
            // 
            this.YWPYM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.YWPYM.DataPropertyName = "YWPYM";
            this.YWPYM.HeaderText = "药物名称(带拼音码)";
            this.YWPYM.Name = "YWPYM";
            this.YWPYM.ReadOnly = true;
            // 
            // YWDropDownGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 295);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.dgv_yw);
            this.Controls.Add(this.txt_input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(449, 331);
            this.MinimumSize = new System.Drawing.Size(449, 331);
            this.Name = "YWDropDownGrid";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "药物名称字典";
            this.Load += new System.EventHandler(this.YWDropDownGrid_Load);
            this.Shown += new System.EventHandler(this.YWDropDownGrid_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_yw)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_yw;
        private System.Windows.Forms.TextBox txt_input;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn YWMC;
        private System.Windows.Forms.DataGridViewTextBoxColumn YWPYM;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
    }
}