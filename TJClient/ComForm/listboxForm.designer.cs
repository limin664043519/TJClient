namespace TJClient.sys
{
    partial class listboxForm
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
            this.listBox_com = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox_com
            // 
            this.listBox_com.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_com.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox_com.FormattingEnabled = true;
            this.listBox_com.ItemHeight = 15;
            this.listBox_com.Location = new System.Drawing.Point(0, 0);
            this.listBox_com.Name = "listBox_com";
            this.listBox_com.Size = new System.Drawing.Size(211, 304);
            this.listBox_com.TabIndex = 0;
            this.listBox_com.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox_jbzd_KeyDown);
            // 
            // listboxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 305);
            this.Controls.Add(this.listBox_com);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "listboxForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "listboxForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_com;
    }
}