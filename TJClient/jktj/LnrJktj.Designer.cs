using System.Windows.Forms;
namespace FBYClient
{
    partial class LnrJktj
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.shapeContainer13 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.timer_yq = new System.Windows.Forms.Timer(this.components);
            this.panel_Form_p = new System.Windows.Forms.Panel();
            this.panel_body = new System.Windows.Forms.Panel();
            this.panel_top = new System.Windows.Forms.Panel();
            this.panel_top_button = new System.Windows.Forms.Panel();
            this.textBox_realname = new System.Windows.Forms.TextBox();
            this.picSignname1111 = new System.Windows.Forms.PictureBox();
            this.cboSignname = new System.Windows.Forms.ComboBox();
            this.btnTabletSignname = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.checkBox_yq = new System.Windows.Forms.CheckBox();
            this.button_lis = new System.Windows.Forms.Button();
            this.comboBox_yq = new System.Windows.Forms.ComboBox();
            this.button_result = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.label_yq = new System.Windows.Forms.Label();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.check_dw = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
            this.panel_Form_p.SuspendLayout();
            this.panel_top.SuspendLayout();
            this.panel_top_button.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSignname1111)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem1.Text = "删除";
            // 
            // shapeContainer13
            // 
            this.shapeContainer13.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer13.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer13.Name = "shapeContainer13";
            this.shapeContainer13.Size = new System.Drawing.Size(1138, 86);
            this.shapeContainer13.TabIndex = 305;
            this.shapeContainer13.TabStop = false;
            // 
            // timer_yq
            // 
            this.timer_yq.Tick += new System.EventHandler(this.timer_yq_Tick);
            // 
            // panel_Form_p
            // 
            this.panel_Form_p.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panel_Form_p.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel_Form_p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Form_p.Controls.Add(this.panel_body);
            this.panel_Form_p.Controls.Add(this.panel_top);
            this.panel_Form_p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Form_p.Location = new System.Drawing.Point(0, 0);
            this.panel_Form_p.Name = "panel_Form_p";
            this.panel_Form_p.Size = new System.Drawing.Size(1018, 722);
            this.panel_Form_p.TabIndex = 1004;
            // 
            // panel_body
            // 
            this.panel_body.AutoScroll = true;
            this.panel_body.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_body.Location = new System.Drawing.Point(0, 51);
            this.panel_body.Name = "panel_body";
            this.panel_body.Size = new System.Drawing.Size(1016, 669);
            this.panel_body.TabIndex = 2;
            // 
            // panel_top
            // 
            this.panel_top.Controls.Add(this.panel_top_button);
            this.panel_top.Controls.Add(this.checkBox_yq);
            this.panel_top.Controls.Add(this.button_lis);
            this.panel_top.Controls.Add(this.comboBox_yq);
            this.panel_top.Controls.Add(this.button_result);
            this.panel_top.Controls.Add(this.button_stop);
            this.panel_top.Controls.Add(this.button_start);
            this.panel_top.Controls.Add(this.label_yq);
            this.panel_top.Controls.Add(this.comboBox_type);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1016, 51);
            this.panel_top.TabIndex = 3;
            // 
            // panel_top_button
            // 
            this.panel_top_button.Controls.Add(this.check_dw);
            this.panel_top_button.Controls.Add(this.textBox_realname);
            this.panel_top_button.Controls.Add(this.picSignname1111);
            this.panel_top_button.Controls.Add(this.cboSignname);
            this.panel_top_button.Controls.Add(this.btnTabletSignname);
            this.panel_top_button.Controls.Add(this.button_clear);
            this.panel_top_button.Controls.Add(this.button_save);
            this.panel_top_button.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_top_button.Location = new System.Drawing.Point(446, 0);
            this.panel_top_button.Name = "panel_top_button";
            this.panel_top_button.Size = new System.Drawing.Size(570, 51);
            this.panel_top_button.TabIndex = 307;
            // 
            // textBox_realname
            // 
            this.textBox_realname.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox_realname.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_realname.Location = new System.Drawing.Point(9, 11);
            this.textBox_realname.Name = "textBox_realname";
            this.textBox_realname.Size = new System.Drawing.Size(100, 26);
            this.textBox_realname.TabIndex = 307;
            this.textBox_realname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // picSignname1111
            // 
            this.picSignname1111.Location = new System.Drawing.Point(117, 6);
            this.picSignname1111.Name = "picSignname1111";
            this.picSignname1111.Size = new System.Drawing.Size(110, 40);
            this.picSignname1111.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSignname1111.TabIndex = 306;
            this.picSignname1111.TabStop = false;
            // 
            // cboSignname
            // 
            this.cboSignname.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboSignname.FormattingEnabled = true;
            this.cboSignname.Location = new System.Drawing.Point(233, 6);
            this.cboSignname.Name = "cboSignname";
            this.cboSignname.Size = new System.Drawing.Size(79, 25);
            this.cboSignname.TabIndex = 305;
            this.cboSignname.SelectedIndexChanged += new System.EventHandler(this.cboSignname_SelectedIndexChanged);
            // 
            // btnTabletSignname
            // 
            this.btnTabletSignname.Location = new System.Drawing.Point(318, 4);
            this.btnTabletSignname.Name = "btnTabletSignname";
            this.btnTabletSignname.Size = new System.Drawing.Size(78, 30);
            this.btnTabletSignname.TabIndex = 304;
            this.btnTabletSignname.Text = "写字板签名";
            this.btnTabletSignname.UseVisualStyleBackColor = true;
            this.btnTabletSignname.Click += new System.EventHandler(this.btnTabletSignname_Click);
            // 
            // button_clear
            // 
            this.button_clear.BackColor = System.Drawing.SystemColors.Control;
            this.button_clear.BackgroundImage = global::TJClient.Properties.Resources.btn_close;
            this.button_clear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_clear.FlatAppearance.BorderSize = 0;
            this.button_clear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_clear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_clear.Location = new System.Drawing.Point(486, 4);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(78, 30);
            this.button_clear.TabIndex = 301;
            this.button_clear.TabStop = false;
            this.button_clear.UseVisualStyleBackColor = false;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_save
            // 
            this.button_save.BackColor = System.Drawing.SystemColors.Control;
            this.button_save.BackgroundImage = global::TJClient.Properties.Resources.btn_save;
            this.button_save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_save.FlatAppearance.BorderSize = 0;
            this.button_save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_save.Location = new System.Drawing.Point(402, 4);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(78, 30);
            this.button_save.TabIndex = 300;
            this.button_save.TabStop = false;
            this.button_save.UseVisualStyleBackColor = false;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // checkBox_yq
            // 
            this.checkBox_yq.AutoSize = true;
            this.checkBox_yq.Location = new System.Drawing.Point(52, 18);
            this.checkBox_yq.Name = "checkBox_yq";
            this.checkBox_yq.Size = new System.Drawing.Size(48, 16);
            this.checkBox_yq.TabIndex = 305;
            this.checkBox_yq.Text = "启用";
            this.checkBox_yq.UseVisualStyleBackColor = true;
            this.checkBox_yq.CheckedChanged += new System.EventHandler(this.checkBox_yq_CheckedChanged);
            // 
            // button_lis
            // 
            this.button_lis.Location = new System.Drawing.Point(511, 4);
            this.button_lis.Name = "button_lis";
            this.button_lis.Size = new System.Drawing.Size(92, 30);
            this.button_lis.TabIndex = 303;
            this.button_lis.Text = "同步检验结果";
            this.button_lis.UseVisualStyleBackColor = true;
            this.button_lis.Visible = false;
            this.button_lis.Click += new System.EventHandler(this.button_lis_Click);
            // 
            // comboBox_yq
            // 
            this.comboBox_yq.FormattingEnabled = true;
            this.comboBox_yq.Location = new System.Drawing.Point(106, 14);
            this.comboBox_yq.Name = "comboBox_yq";
            this.comboBox_yq.Size = new System.Drawing.Size(121, 20);
            this.comboBox_yq.TabIndex = 306;
            this.comboBox_yq.Visible = false;
            this.comboBox_yq.SelectedIndexChanged += new System.EventHandler(this.comboBox_yq_SelectedIndexChanged);
            // 
            // button_result
            // 
            this.button_result.Location = new System.Drawing.Point(390, 9);
            this.button_result.Name = "button_result";
            this.button_result.Size = new System.Drawing.Size(50, 30);
            this.button_result.TabIndex = 304;
            this.button_result.Text = "结果";
            this.button_result.UseVisualStyleBackColor = true;
            this.button_result.Visible = false;
            this.button_result.Click += new System.EventHandler(this.button_result_Click);
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(381, 9);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(50, 30);
            this.button_stop.TabIndex = 304;
            this.button_stop.Text = "停止";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Visible = false;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(233, 10);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(50, 30);
            this.button_start.TabIndex = 303;
            this.button_start.Text = "开始";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Visible = false;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // label_yq
            // 
            this.label_yq.AutoSize = true;
            this.label_yq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label_yq.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Bold);
            this.label_yq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(163)))), ((int)(((byte)(161)))));
            this.label_yq.Location = new System.Drawing.Point(9, 16);
            this.label_yq.Name = "label_yq";
            this.label_yq.Size = new System.Drawing.Size(35, 19);
            this.label_yq.TabIndex = 302;
            this.label_yq.Text = "仪器";
            // 
            // comboBox_type
            // 
            this.comboBox_type.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Items.AddRange(new object[] {
            "1,已体检",
            "2,未体检"});
            this.comboBox_type.Location = new System.Drawing.Point(289, 13);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(95, 25);
            this.comboBox_type.TabIndex = 41;
            this.comboBox_type.Visible = false;
            // 
            // check_dw
            // 
            this.check_dw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.check_dw.AutoSize = true;
            this.check_dw.Checked = true;
            this.check_dw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.check_dw.Location = new System.Drawing.Point(447, 34);
            this.check_dw.Name = "check_dw";
            this.check_dw.Size = new System.Drawing.Size(120, 16);
            this.check_dw.TabIndex = 306;
            this.check_dw.Text = "验证漏项、无效项";
            this.check_dw.UseVisualStyleBackColor = true;
            // 
            // LnrJktj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1018, 722);
            this.Controls.Add(this.panel_Form_p);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LnrJktj";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "公共卫生健康体检";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.jktj_FormClosing);
            this.Load += new System.EventHandler(this.jktj_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel_Form_p.ResumeLayout(false);
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            this.panel_top_button.ResumeLayout(false);
            this.panel_top_button.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSignname1111)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Form_p;
        private System.Windows.Forms.Panel panel_body;
        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_save;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer13;
        private Label label_yq;
        private Button button_stop;
        private Button button_start;
        private Button button_result;
        private Timer timer_yq;
        private CheckBox checkBox_yq;
        private ComboBox comboBox_yq;
        private ComboBox comboBox_type;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private Panel panel_top_button;
        private Button button_lis;
        private Button btnTabletSignname;
        private PictureBox picSignname1111;
        private ComboBox cboSignname;
        private TextBox textBox_realname;
        private CheckBox check_dw;
    }
}