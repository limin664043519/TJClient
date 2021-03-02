namespace TJClient.sys
{
    partial class Form_txmSet
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
            this.button_true = new System.Windows.Forms.Button();
            this.button_return = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_save_TM = new System.Windows.Forms.Button();
            this.button_save_TC = new System.Windows.Forms.Button();
            this.button_defaut = new System.Windows.Forms.Button();
            this.checkedListBox_TC = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox_tm = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBox_MX = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_true
            // 
            this.button_true.Location = new System.Drawing.Point(491, 2);
            this.button_true.Name = "button_true";
            this.button_true.Size = new System.Drawing.Size(75, 28);
            this.button_true.TabIndex = 4;
            this.button_true.Text = "确定";
            this.button_true.UseVisualStyleBackColor = true;
            this.button_true.Click += new System.EventHandler(this.button_true_Click);
            // 
            // button_return
            // 
            this.button_return.Location = new System.Drawing.Point(567, 2);
            this.button_return.Name = "button_return";
            this.button_return.Size = new System.Drawing.Size(75, 28);
            this.button_return.TabIndex = 5;
            this.button_return.Text = "返回";
            this.button_return.UseVisualStyleBackColor = true;
            this.button_return.Click += new System.EventHandler(this.button_return_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_return);
            this.panel1.Controls.Add(this.button_save_TM);
            this.panel1.Controls.Add(this.button_save_TC);
            this.panel1.Controls.Add(this.button_true);
            this.panel1.Controls.Add(this.button_defaut);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 33);
            this.panel1.TabIndex = 6;
            // 
            // button_save_TM
            // 
            this.button_save_TM.Location = new System.Drawing.Point(197, 2);
            this.button_save_TM.Name = "button_save_TM";
            this.button_save_TM.Size = new System.Drawing.Size(75, 28);
            this.button_save_TM.TabIndex = 9;
            this.button_save_TM.Text = "保存条码";
            this.button_save_TM.UseVisualStyleBackColor = true;
            this.button_save_TM.Visible = false;
            this.button_save_TM.Click += new System.EventHandler(this.button_save_TM_Click);
            // 
            // button_save_TC
            // 
            this.button_save_TC.Location = new System.Drawing.Point(89, 2);
            this.button_save_TC.Name = "button_save_TC";
            this.button_save_TC.Size = new System.Drawing.Size(75, 28);
            this.button_save_TC.TabIndex = 9;
            this.button_save_TC.Text = "保存套餐";
            this.button_save_TC.UseVisualStyleBackColor = true;
            this.button_save_TC.Visible = false;
            this.button_save_TC.Click += new System.EventHandler(this.button_save_TC_Click);
            // 
            // button_defaut
            // 
            this.button_defaut.Location = new System.Drawing.Point(8, 2);
            this.button_defaut.Name = "button_defaut";
            this.button_defaut.Size = new System.Drawing.Size(75, 28);
            this.button_defaut.TabIndex = 9;
            this.button_defaut.Text = "设为默认";
            this.button_defaut.UseVisualStyleBackColor = true;
            this.button_defaut.Click += new System.EventHandler(this.button_defaut_Click);
            // 
            // checkedListBox_TC
            // 
            this.checkedListBox_TC.CheckOnClick = true;
            this.checkedListBox_TC.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox_TC.FormattingEnabled = true;
            this.checkedListBox_TC.HorizontalScrollbar = true;
            this.checkedListBox_TC.Location = new System.Drawing.Point(8, 61);
            this.checkedListBox_TC.Name = "checkedListBox_TC";
            this.checkedListBox_TC.Size = new System.Drawing.Size(150, 364);
            this.checkedListBox_TC.TabIndex = 7;
            this.checkedListBox_TC.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_TC_ItemCheck);
            this.checkedListBox_TC.SelectedValueChanged += new System.EventHandler(this.checkedListBox_TC_SelectedValueChanged);
            // 
            // checkedListBox_tm
            // 
            this.checkedListBox_tm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox_tm.FormattingEnabled = true;
            this.checkedListBox_tm.HorizontalScrollbar = true;
            this.checkedListBox_tm.Location = new System.Drawing.Point(163, 61);
            this.checkedListBox_tm.Name = "checkedListBox_tm";
            this.checkedListBox_tm.Size = new System.Drawing.Size(150, 364);
            this.checkedListBox_tm.TabIndex = 7;
            this.checkedListBox_tm.Click += new System.EventHandler(this.checkedListBox_tm_Click);
            this.checkedListBox_tm.SelectedIndexChanged += new System.EventHandler(this.checkedListBox_tm_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(56, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "条码套餐";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(214, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "条码";
            // 
            // checkedListBox_MX
            // 
            this.checkedListBox_MX.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox_MX.FormattingEnabled = true;
            this.checkedListBox_MX.HorizontalScrollbar = true;
            this.checkedListBox_MX.Location = new System.Drawing.Point(321, 61);
            this.checkedListBox_MX.Name = "checkedListBox_MX";
            this.checkedListBox_MX.Size = new System.Drawing.Size(334, 364);
            this.checkedListBox_MX.TabIndex = 7;
            this.checkedListBox_MX.SelectedValueChanged += new System.EventHandler(this.checkedListBox_MX_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(431, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "检验条码明细";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(12, 432);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(337, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "说明：套餐、条码设置修改后只在本客户端有效！";
            // 
            // Form_txmSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 463);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBox_MX);
            this.Controls.Add(this.checkedListBox_tm);
            this.Controls.Add(this.checkedListBox_TC);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_txmSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "条形码打印";
            this.Load += new System.EventHandler(this.Form_txmSet_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_true;
        private System.Windows.Forms.Button button_return;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckedListBox checkedListBox_TC;
        private System.Windows.Forms.CheckedListBox checkedListBox_tm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBox_MX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_defaut;
        private System.Windows.Forms.Button button_save_TC;
        private System.Windows.Forms.Button button_save_TM;
        private System.Windows.Forms.Label label4;
    }
}