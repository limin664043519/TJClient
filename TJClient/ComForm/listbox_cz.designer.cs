namespace TJClient.sys
{
    partial class listbox_cz
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
            this.checkedListBox_cz = new System.Windows.Forms.CheckedListBox();
            this.button_allSelect = new System.Windows.Forms.Button();
            this.button_notSelect = new System.Windows.Forms.Button();
            this.button_true = new System.Windows.Forms.Button();
            this.button_return = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox_mc = new System.Windows.Forms.TextBox();
            this.button_select = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBox_cz
            // 
            this.checkedListBox_cz.CheckOnClick = true;
            this.checkedListBox_cz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_cz.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox_cz.FormattingEnabled = true;
            this.checkedListBox_cz.HorizontalExtent = 10;
            this.checkedListBox_cz.Location = new System.Drawing.Point(0, 32);
            this.checkedListBox_cz.MultiColumn = true;
            this.checkedListBox_cz.Name = "checkedListBox_cz";
            this.checkedListBox_cz.Size = new System.Drawing.Size(579, 399);
            this.checkedListBox_cz.TabIndex = 1;
            // 
            // button_allSelect
            // 
            this.button_allSelect.Location = new System.Drawing.Point(7, 6);
            this.button_allSelect.Name = "button_allSelect";
            this.button_allSelect.Size = new System.Drawing.Size(65, 23);
            this.button_allSelect.TabIndex = 2;
            this.button_allSelect.Text = "全选";
            this.button_allSelect.UseVisualStyleBackColor = true;
            this.button_allSelect.Click += new System.EventHandler(this.button_allSelect_Click);
            // 
            // button_notSelect
            // 
            this.button_notSelect.Location = new System.Drawing.Point(78, 6);
            this.button_notSelect.Name = "button_notSelect";
            this.button_notSelect.Size = new System.Drawing.Size(65, 23);
            this.button_notSelect.TabIndex = 3;
            this.button_notSelect.Text = "全不选";
            this.button_notSelect.UseVisualStyleBackColor = true;
            this.button_notSelect.Click += new System.EventHandler(this.button_notSelect_Click);
            // 
            // button_true
            // 
            this.button_true.Location = new System.Drawing.Point(438, 6);
            this.button_true.Name = "button_true";
            this.button_true.Size = new System.Drawing.Size(65, 23);
            this.button_true.TabIndex = 4;
            this.button_true.Text = "确定";
            this.button_true.UseVisualStyleBackColor = true;
            this.button_true.Click += new System.EventHandler(this.button_true_Click);
            // 
            // button_return
            // 
            this.button_return.Location = new System.Drawing.Point(507, 6);
            this.button_return.Name = "button_return";
            this.button_return.Size = new System.Drawing.Size(65, 23);
            this.button_return.TabIndex = 5;
            this.button_return.Text = "返回";
            this.button_return.UseVisualStyleBackColor = true;
            this.button_return.Click += new System.EventHandler(this.button_return_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox_mc);
            this.panel1.Controls.Add(this.button_allSelect);
            this.panel1.Controls.Add(this.button_return);
            this.panel1.Controls.Add(this.button_notSelect);
            this.panel1.Controls.Add(this.button_select);
            this.panel1.Controls.Add(this.button_true);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(579, 32);
            this.panel1.TabIndex = 6;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // textBox_mc
            // 
            this.textBox_mc.Location = new System.Drawing.Point(152, 6);
            this.textBox_mc.Name = "textBox_mc";
            this.textBox_mc.Size = new System.Drawing.Size(203, 21);
            this.textBox_mc.TabIndex = 6;
            // 
            // button_select
            // 
            this.button_select.Location = new System.Drawing.Point(361, 6);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(65, 23);
            this.button_select.TabIndex = 4;
            this.button_select.Text = "查询";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // listbox_cz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 431);
            this.Controls.Add(this.checkedListBox_cz);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "listbox_cz";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "listboxForm";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox_cz;
        private System.Windows.Forms.Button button_allSelect;
        private System.Windows.Forms.Button button_notSelect;
        private System.Windows.Forms.Button button_true;
        private System.Windows.Forms.Button button_return;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox_mc;
        private System.Windows.Forms.Button button_select;
    }
}