namespace TJClient
{
    partial class FrmTestTablet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTestTablet));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_singtext = new System.Windows.Forms.TextBox();
            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.btnSign = new System.Windows.Forms.Button();
            this.pictureBox_show = new System.Windows.Forms.PictureBox();
            this.sigImage = new System.Windows.Forms.PictureBox();
            this.axSigCtl1 = new Florentis.AxSigCtl();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_show)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sigImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axSigCtl1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(567, 149);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 41);
            this.button2.TabIndex = 11;
            this.button2.Text = "showImg";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 361);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "singText";
            // 
            // textBox_singtext
            // 
            this.textBox_singtext.Location = new System.Drawing.Point(170, 379);
            this.textBox_singtext.Multiline = true;
            this.textBox_singtext.Name = "textBox_singtext";
            this.textBox_singtext.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_singtext.Size = new System.Drawing.Size(392, 94);
            this.textBox_singtext.TabIndex = 8;
            // 
            // txtDisplay
            // 
            this.txtDisplay.Location = new System.Drawing.Point(170, 245);
            this.txtDisplay.Multiline = true;
            this.txtDisplay.Name = "txtDisplay";
            this.txtDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDisplay.Size = new System.Drawing.Size(392, 94);
            this.txtDisplay.TabIndex = 9;
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(405, 149);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(94, 41);
            this.btnSign.TabIndex = 7;
            this.btnSign.Text = "Sign";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // pictureBox_show
            // 
            this.pictureBox_show.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox_show.Location = new System.Drawing.Point(568, 235);
            this.pictureBox_show.Name = "pictureBox_show";
            this.pictureBox_show.Size = new System.Drawing.Size(70, 30);
            this.pictureBox_show.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_show.TabIndex = 5;
            this.pictureBox_show.TabStop = false;
            // 
            // sigImage
            // 
            this.sigImage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.sigImage.Location = new System.Drawing.Point(170, 88);
            this.sigImage.Name = "sigImage";
            this.sigImage.Size = new System.Drawing.Size(70, 30);
            this.sigImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.sigImage.TabIndex = 6;
            this.sigImage.TabStop = false;
            // 
            // axSigCtl1
            // 
            this.axSigCtl1.Location = new System.Drawing.Point(374, 37);
            this.axSigCtl1.Name = "axSigCtl1";
            this.axSigCtl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSigCtl1.OcxState")));
            this.axSigCtl1.Size = new System.Drawing.Size(105, 30);
            this.axSigCtl1.TabIndex = 12;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(488, 37);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(74, 30);
            this.button3.TabIndex = 13;
            this.button3.Text = "Sign";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FrmTestTablet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 561);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.axSigCtl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_singtext);
            this.Controls.Add(this.txtDisplay);
            this.Controls.Add(this.btnSign);
            this.Controls.Add(this.pictureBox_show);
            this.Controls.Add(this.sigImage);
            this.Controls.Add(this.button1);
            this.Name = "FrmTestTablet";
            this.Text = "FrmTestTablet";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_show)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sigImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axSigCtl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_singtext;
        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.PictureBox pictureBox_show;
        private System.Windows.Forms.PictureBox sigImage;
        private Florentis.AxSigCtl axSigCtl1;
        private System.Windows.Forms.Button button3;
    }
}