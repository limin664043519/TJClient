namespace TJClient.SignnameForm
{
    partial class Form_HanwangEsp560Signname
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_HanwangEsp560Signname));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblWhy = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWhat = new System.Windows.Forms.Label();
            this.lblDatetime = new System.Windows.Forms.Label();
            this.clientDoService1 = new TJClient.JKTJ.ClientDoService();
            this.HWSignname = new AxHWPenSignLib.AxHWPenSign();
            ((System.ComponentModel.ISupportInitialize)(this.HWSignname)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(0, 218);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(125, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(124, 218);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(125, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(249, 218);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(125, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblWhy
            // 
            this.lblWhy.AutoSize = true;
            this.lblWhy.Location = new System.Drawing.Point(13, 13);
            this.lblWhy.Name = "lblWhy";
            this.lblWhy.Size = new System.Drawing.Size(23, 12);
            this.lblWhy.TabIndex = 3;
            this.lblWhy.Text = "why";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(20, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "...............................................";
            // 
            // lblWhat
            // 
            this.lblWhat.AutoSize = true;
            this.lblWhat.Location = new System.Drawing.Point(240, 169);
            this.lblWhat.Name = "lblWhat";
            this.lblWhat.Size = new System.Drawing.Size(29, 12);
            this.lblWhat.TabIndex = 5;
            this.lblWhat.Text = "what";
            // 
            // lblDatetime
            // 
            this.lblDatetime.AutoSize = true;
            this.lblDatetime.Location = new System.Drawing.Point(240, 191);
            this.lblDatetime.Name = "lblDatetime";
            this.lblDatetime.Size = new System.Drawing.Size(29, 12);
            this.lblDatetime.TabIndex = 6;
            this.lblDatetime.Text = "Time";
            // 
            // clientDoService1
            // 
            this.clientDoService1.Url = "http://localhost:13652/ClientDoService.asmx";
            this.clientDoService1.UseDefaultCredentials = true;
            // 
            // HWSignname
            // 
            this.HWSignname.Enabled = true;
            this.HWSignname.Location = new System.Drawing.Point(70, 45);
            this.HWSignname.Name = "HWSignname";
            this.HWSignname.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("HWSignname.OcxState")));
            this.HWSignname.Size = new System.Drawing.Size(224, 103);
            this.HWSignname.TabIndex = 7;
            // 
            // Form_HanwangEsp560Signname
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(374, 240);
            this.Controls.Add(this.HWSignname);
            this.Controls.Add(this.lblDatetime);
            this.Controls.Add(this.lblWhat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblWhy);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnOk);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_HanwangEsp560Signname";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "签名操作";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_HanwangEsp560Signname_FormClosing);
            this.Load += new System.EventHandler(this.Form_HanwangEsp560Signname_Load);
            ((System.ComponentModel.ISupportInitialize)(this.HWSignname)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblWhy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblWhat;
        private System.Windows.Forms.Label lblDatetime;
        private JKTJ.ClientDoService clientDoService1;
        private AxHWPenSignLib.AxHWPenSign HWSignname;
    }
}