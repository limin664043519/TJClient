namespace TJClient.NeedToUseForm
{
    partial class FrmLisTmCorresponding
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
            this.TxtLisTm = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkBlood = new System.Windows.Forms.CheckBox();
            this.chkHadUsed = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "外部Lis条码";
            // 
            // TxtLisTm
            // 
            this.TxtLisTm.Location = new System.Drawing.Point(100, 23);
            this.TxtLisTm.Name = "TxtLisTm";
            this.TxtLisTm.Size = new System.Drawing.Size(268, 21);
            this.TxtLisTm.TabIndex = 1;
            this.TxtLisTm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtLisTm_KeyPress);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(293, 83);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkBlood
            // 
            this.chkBlood.AutoSize = true;
            this.chkBlood.Location = new System.Drawing.Point(100, 53);
            this.chkBlood.Name = "chkBlood";
            this.chkBlood.Size = new System.Drawing.Size(72, 16);
            this.chkBlood.TabIndex = 3;
            this.chkBlood.Text = "不含血型";
            this.chkBlood.UseVisualStyleBackColor = true;
            // 
            // chkHadUsed
            // 
            this.chkHadUsed.AutoSize = true;
            this.chkHadUsed.Checked = true;
            this.chkHadUsed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHadUsed.Location = new System.Drawing.Point(188, 53);
            this.chkHadUsed.Name = "chkHadUsed";
            this.chkHadUsed.Size = new System.Drawing.Size(144, 16);
            this.chkHadUsed.TabIndex = 4;
            this.chkHadUsed.Text = "判断条码是否已经使用";
            this.chkHadUsed.UseVisualStyleBackColor = true;
            // 
            // FrmLisTmCorresponding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 132);
            this.Controls.Add(this.chkHadUsed);
            this.Controls.Add(this.chkBlood);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.TxtLisTm);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLisTmCorresponding";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LIs条码对应";
            this.Load += new System.EventHandler(this.FrmLisTmCorresponding_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtLisTm;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkBlood;
        private System.Windows.Forms.CheckBox chkHadUsed;
    }
}