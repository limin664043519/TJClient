namespace YCYL.Client.AllForms
{
    partial class printForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(printForm));
            this.panel_form = new System.Windows.Forms.Panel();
            this.panel_body = new System.Windows.Forms.Panel();
            this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
            this.panel_head = new System.Windows.Forms.Panel();
            this.button_print = new System.Windows.Forms.Button();
            this.panel_form.SuspendLayout();
            this.panel_body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
            this.panel_head.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_form
            // 
            this.panel_form.Controls.Add(this.panel_body);
            this.panel_form.Controls.Add(this.panel_head);
            this.panel_form.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_form.Location = new System.Drawing.Point(0, 0);
            this.panel_form.Name = "panel_form";
            this.panel_form.Size = new System.Drawing.Size(692, 472);
            this.panel_form.TabIndex = 0;
            // 
            // panel_body
            // 
            this.panel_body.AutoScroll = true;
            this.panel_body.Controls.Add(this.axWebBrowser1);
            this.panel_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_body.Location = new System.Drawing.Point(0, 49);
            this.panel_body.Name = "panel_body";
            this.panel_body.Size = new System.Drawing.Size(692, 423);
            this.panel_body.TabIndex = 1;
            // 
            // axWebBrowser1
            // 
            this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWebBrowser1.Enabled = true;
            this.axWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
            this.axWebBrowser1.Size = new System.Drawing.Size(692, 423);
            this.axWebBrowser1.TabIndex = 0;
            this.axWebBrowser1.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.axWebBrowser1_NavigateComplete2);
            // 
            // panel_head
            // 
            this.panel_head.Controls.Add(this.button_print);
            this.panel_head.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_head.Location = new System.Drawing.Point(0, 0);
            this.panel_head.Name = "panel_head";
            this.panel_head.Size = new System.Drawing.Size(692, 49);
            this.panel_head.TabIndex = 0;
            // 
            // button_print
            // 
            this.button_print.Location = new System.Drawing.Point(558, 10);
            this.button_print.Name = "button_print";
            this.button_print.Size = new System.Drawing.Size(124, 30);
            this.button_print.TabIndex = 0;
            this.button_print.Text = "打印";
            this.button_print.UseVisualStyleBackColor = true;
            this.button_print.Click += new System.EventHandler(this.button_print_Click);
            // 
            // printForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 472);
            this.Controls.Add(this.panel_form);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "printForm";
            this.Text = "文档打印";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.printForm_FormClosing);
            this.Load += new System.EventHandler(this.printForm_Load);
            this.panel_form.ResumeLayout(false);
            this.panel_body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
            this.panel_head.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_form;
        private System.Windows.Forms.Panel panel_body;
        private AxSHDocVw.AxWebBrowser axWebBrowser1;
        private System.Windows.Forms.Panel panel_head;
        private System.Windows.Forms.Button button_print;
    }
}