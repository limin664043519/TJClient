namespace TJClient.ComForm
{
    partial class Form_photo
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Photograph = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tscbxCameras = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
            this.grpHealthExaminedUserInfo = new System.Windows.Forms.GroupBox();
            this.txtSFZH = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDGrdabh = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker_jiashu = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_benren = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_jiashu = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.grpSignnameByFamilyMembers = new System.Windows.Forms.GroupBox();
            this.textBox_jiashu2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label_jiashu = new System.Windows.Forms.Label();
            this.piciSignnameByFamilyMembers = new System.Windows.Forms.PictureBox();
            this.btnSignnameByFamilyMembers = new System.Windows.Forms.Button();
            this.button_jiashu = new System.Windows.Forms.Button();
            this.grpSignnameBySelf = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label_benren = new System.Windows.Forms.Label();
            this.textBox_benren = new System.Windows.Forms.TextBox();
            this.picSignnameBySelf = new System.Windows.Forms.PictureBox();
            this.button_benren = new System.Windows.Forms.Button();
            this.btnTabletSignnameBySelf = new System.Windows.Forms.Button();
            this.picPhoto = new System.Windows.Forms.PictureBox();
            this.grpHealthExaminedUserInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpSignnameByFamilyMembers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.piciSignnameByFamilyMembers)).BeginInit();
            this.grpSignnameBySelf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSignnameBySelf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // Photograph
            // 
            this.Photograph.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Photograph.Location = new System.Drawing.Point(340, 27);
            this.Photograph.Name = "Photograph";
            this.Photograph.Size = new System.Drawing.Size(75, 23);
            this.Photograph.TabIndex = 1;
            this.Photograph.Text = "拍照";
            this.Photograph.UseVisualStyleBackColor = true;
            this.Photograph.Click += new System.EventHandler(this.Photograph_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(421, 27);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭拍照";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tscbxCameras
            // 
            this.tscbxCameras.FormattingEnabled = true;
            this.tscbxCameras.Location = new System.Drawing.Point(97, 29);
            this.tscbxCameras.Name = "tscbxCameras";
            this.tscbxCameras.Size = new System.Drawing.Size(154, 20);
            this.tscbxCameras.TabIndex = 7;
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(259, 27);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(5, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "视频输入设备：";
            // 
            // videoSourcePlayer
            // 
            this.videoSourcePlayer.Location = new System.Drawing.Point(3, 177);
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new System.Drawing.Size(248, 269);
            this.videoSourcePlayer.TabIndex = 10;
            this.videoSourcePlayer.Text = "videoSourcePlayer";
            this.videoSourcePlayer.VideoSource = null;
            // 
            // grpHealthExaminedUserInfo
            // 
            this.grpHealthExaminedUserInfo.Controls.Add(this.txtSFZH);
            this.grpHealthExaminedUserInfo.Controls.Add(this.label2);
            this.grpHealthExaminedUserInfo.Controls.Add(this.label18);
            this.grpHealthExaminedUserInfo.Controls.Add(this.txtName);
            this.grpHealthExaminedUserInfo.Controls.Add(this.label3);
            this.grpHealthExaminedUserInfo.Controls.Add(this.txtDGrdabh);
            this.grpHealthExaminedUserInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpHealthExaminedUserInfo.Location = new System.Drawing.Point(3, 58);
            this.grpHealthExaminedUserInfo.Name = "grpHealthExaminedUserInfo";
            this.grpHealthExaminedUserInfo.Size = new System.Drawing.Size(248, 103);
            this.grpHealthExaminedUserInfo.TabIndex = 44;
            this.grpHealthExaminedUserInfo.TabStop = false;
            this.grpHealthExaminedUserInfo.Text = "体检人员信息";
            // 
            // txtSFZH
            // 
            this.txtSFZH.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSFZH.Location = new System.Drawing.Point(94, 75);
            this.txtSFZH.Name = "txtSFZH";
            this.txtSFZH.ReadOnly = true;
            this.txtSFZH.Size = new System.Drawing.Size(148, 23);
            this.txtSFZH.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(163)))), ((int)(((byte)(161)))));
            this.label2.Location = new System.Drawing.Point(27, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 19);
            this.label2.TabIndex = 43;
            this.label2.Text = "身份证号";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label18.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Bold);
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(163)))), ((int)(((byte)(161)))));
            this.label18.Location = new System.Drawing.Point(1, 18);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(87, 19);
            this.label18.TabIndex = 37;
            this.label18.Text = "体检人员姓名";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.Location = new System.Drawing.Point(94, 17);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(148, 23);
            this.txtName.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(163)))), ((int)(((byte)(161)))));
            this.label3.Location = new System.Drawing.Point(14, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 19);
            this.label3.TabIndex = 39;
            this.label3.Text = "健康档案号";
            // 
            // txtDGrdabh
            // 
            this.txtDGrdabh.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDGrdabh.Location = new System.Drawing.Point(94, 46);
            this.txtDGrdabh.Name = "txtDGrdabh";
            this.txtDGrdabh.ReadOnly = true;
            this.txtDGrdabh.Size = new System.Drawing.Size(148, 23);
            this.txtDGrdabh.TabIndex = 40;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.dateTimePicker_jiashu);
            this.panel1.Controls.Add(this.dateTimePicker_benren);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.grpSignnameByFamilyMembers);
            this.panel1.Controls.Add(this.grpSignnameBySelf);
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(253, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 380);
            this.panel1.TabIndex = 45;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(3, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 17);
            this.label7.TabIndex = 48;
            this.label7.Text = "家属签字日期";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(3, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 17);
            this.label6.TabIndex = 48;
            this.label6.Text = "本人签字日期";
            // 
            // dateTimePicker_jiashu
            // 
            this.dateTimePicker_jiashu.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker_jiashu.Location = new System.Drawing.Point(86, 192);
            this.dateTimePicker_jiashu.Name = "dateTimePicker_jiashu";
            this.dateTimePicker_jiashu.Size = new System.Drawing.Size(155, 23);
            this.dateTimePicker_jiashu.TabIndex = 47;
            // 
            // dateTimePicker_benren
            // 
            this.dateTimePicker_benren.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker_benren.Location = new System.Drawing.Point(86, 6);
            this.dateTimePicker_benren.Name = "dateTimePicker_benren";
            this.dateTimePicker_benren.Size = new System.Drawing.Size(155, 23);
            this.dateTimePicker_benren.TabIndex = 47;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBox_jiashu);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(10, 227);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 145);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "家属签字";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(10, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 21);
            this.label8.TabIndex = 311;
            this.label8.Text = "家属姓名";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(21, 129);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 16);
            this.label9.TabIndex = 310;
            // 
            // textBox_jiashu
            // 
            this.textBox_jiashu.BackColor = System.Drawing.Color.LemonChiffon;
            this.textBox_jiashu.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_jiashu.Location = new System.Drawing.Point(8, 93);
            this.textBox_jiashu.Name = "textBox_jiashu";
            this.textBox_jiashu.Size = new System.Drawing.Size(110, 29);
            this.textBox_jiashu.TabIndex = 309;
            this.textBox_jiashu.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(8, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(110, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 310;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(124, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 30);
            this.button1.TabIndex = 309;
            this.button1.Text = "写字板签字";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSignnameByFamilyMembers_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(124, 93);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 30);
            this.button2.TabIndex = 307;
            this.button2.Text = "保存文字签字";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button_jiashu_Click);
            // 
            // grpSignnameByFamilyMembers
            // 
            this.grpSignnameByFamilyMembers.Controls.Add(this.textBox_jiashu2);
            this.grpSignnameByFamilyMembers.Controls.Add(this.label5);
            this.grpSignnameByFamilyMembers.Controls.Add(this.label_jiashu);
            this.grpSignnameByFamilyMembers.Controls.Add(this.piciSignnameByFamilyMembers);
            this.grpSignnameByFamilyMembers.Controls.Add(this.btnSignnameByFamilyMembers);
            this.grpSignnameByFamilyMembers.Controls.Add(this.button_jiashu);
            this.grpSignnameByFamilyMembers.Location = new System.Drawing.Point(10, 227);
            this.grpSignnameByFamilyMembers.Name = "grpSignnameByFamilyMembers";
            this.grpSignnameByFamilyMembers.Size = new System.Drawing.Size(231, 122);
            this.grpSignnameByFamilyMembers.TabIndex = 46;
            this.grpSignnameByFamilyMembers.TabStop = false;
            this.grpSignnameByFamilyMembers.Text = "家属签字";
            // 
            // textBox_jiashu2
            // 
            this.textBox_jiashu2.BackColor = System.Drawing.Color.LemonChiffon;
            this.textBox_jiashu2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_jiashu2.Location = new System.Drawing.Point(47, 93);
            this.textBox_jiashu2.Multiline = true;
            this.textBox_jiashu2.Name = "textBox_jiashu2";
            this.textBox_jiashu2.Size = new System.Drawing.Size(80, 46);
            this.textBox_jiashu2.TabIndex = 309;
            this.textBox_jiashu2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(21, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 311;
            this.label5.Text = "家属姓名";
            // 
            // label_jiashu
            // 
            this.label_jiashu.AutoSize = true;
            this.label_jiashu.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_jiashu.ForeColor = System.Drawing.Color.Red;
            this.label_jiashu.Location = new System.Drawing.Point(21, 129);
            this.label_jiashu.Name = "label_jiashu";
            this.label_jiashu.Size = new System.Drawing.Size(0, 16);
            this.label_jiashu.TabIndex = 310;
            // 
            // piciSignnameByFamilyMembers
            // 
            this.piciSignnameByFamilyMembers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.piciSignnameByFamilyMembers.Location = new System.Drawing.Point(17, 20);
            this.piciSignnameByFamilyMembers.Name = "piciSignnameByFamilyMembers";
            this.piciSignnameByFamilyMembers.Size = new System.Drawing.Size(110, 40);
            this.piciSignnameByFamilyMembers.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.piciSignnameByFamilyMembers.TabIndex = 310;
            this.piciSignnameByFamilyMembers.TabStop = false;
            // 
            // btnSignnameByFamilyMembers
            // 
            this.btnSignnameByFamilyMembers.Location = new System.Drawing.Point(133, 30);
            this.btnSignnameByFamilyMembers.Name = "btnSignnameByFamilyMembers";
            this.btnSignnameByFamilyMembers.Size = new System.Drawing.Size(88, 30);
            this.btnSignnameByFamilyMembers.TabIndex = 309;
            this.btnSignnameByFamilyMembers.Text = "写字板签字";
            this.btnSignnameByFamilyMembers.UseVisualStyleBackColor = true;
            this.btnSignnameByFamilyMembers.Click += new System.EventHandler(this.btnSignnameByFamilyMembers_Click);
            // 
            // button_jiashu
            // 
            this.button_jiashu.Location = new System.Drawing.Point(133, 89);
            this.button_jiashu.Name = "button_jiashu";
            this.button_jiashu.Size = new System.Drawing.Size(88, 30);
            this.button_jiashu.TabIndex = 307;
            this.button_jiashu.Text = "保存文字签字";
            this.button_jiashu.UseVisualStyleBackColor = true;
            this.button_jiashu.Click += new System.EventHandler(this.button_jiashu_Click);
            // 
            // grpSignnameBySelf
            // 
            this.grpSignnameBySelf.Controls.Add(this.label4);
            this.grpSignnameBySelf.Controls.Add(this.label_benren);
            this.grpSignnameBySelf.Controls.Add(this.textBox_benren);
            this.grpSignnameBySelf.Controls.Add(this.picSignnameBySelf);
            this.grpSignnameBySelf.Controls.Add(this.button_benren);
            this.grpSignnameBySelf.Controls.Add(this.btnTabletSignnameBySelf);
            this.grpSignnameBySelf.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpSignnameBySelf.Location = new System.Drawing.Point(8, 38);
            this.grpSignnameBySelf.Name = "grpSignnameBySelf";
            this.grpSignnameBySelf.Size = new System.Drawing.Size(233, 148);
            this.grpSignnameBySelf.TabIndex = 45;
            this.grpSignnameBySelf.TabStop = false;
            this.grpSignnameBySelf.Text = "本人签字";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 21);
            this.label4.TabIndex = 311;
            this.label4.Text = "居民姓名";
            // 
            // label_benren
            // 
            this.label_benren.AutoSize = true;
            this.label_benren.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_benren.ForeColor = System.Drawing.Color.Red;
            this.label_benren.Location = new System.Drawing.Point(14, 123);
            this.label_benren.Name = "label_benren";
            this.label_benren.Size = new System.Drawing.Size(0, 20);
            this.label_benren.TabIndex = 310;
            // 
            // textBox_benren
            // 
            this.textBox_benren.BackColor = System.Drawing.Color.LemonChiffon;
            this.textBox_benren.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_benren.Location = new System.Drawing.Point(10, 91);
            this.textBox_benren.Name = "textBox_benren";
            this.textBox_benren.Size = new System.Drawing.Size(110, 29);
            this.textBox_benren.TabIndex = 309;
            this.textBox_benren.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // picSignnameBySelf
            // 
            this.picSignnameBySelf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSignnameBySelf.Location = new System.Drawing.Point(10, 20);
            this.picSignnameBySelf.Name = "picSignnameBySelf";
            this.picSignnameBySelf.Size = new System.Drawing.Size(110, 40);
            this.picSignnameBySelf.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSignnameBySelf.TabIndex = 308;
            this.picSignnameBySelf.TabStop = false;
            // 
            // button_benren
            // 
            this.button_benren.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_benren.Location = new System.Drawing.Point(126, 91);
            this.button_benren.Name = "button_benren";
            this.button_benren.Size = new System.Drawing.Size(88, 30);
            this.button_benren.TabIndex = 307;
            this.button_benren.Text = "保存文字签字";
            this.button_benren.UseVisualStyleBackColor = true;
            this.button_benren.Click += new System.EventHandler(this.button_benren_Click);
            // 
            // btnTabletSignnameBySelf
            // 
            this.btnTabletSignnameBySelf.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTabletSignnameBySelf.Location = new System.Drawing.Point(126, 30);
            this.btnTabletSignnameBySelf.Name = "btnTabletSignnameBySelf";
            this.btnTabletSignnameBySelf.Size = new System.Drawing.Size(88, 30);
            this.btnTabletSignnameBySelf.TabIndex = 307;
            this.btnTabletSignnameBySelf.Text = "写字板签字";
            this.btnTabletSignnameBySelf.UseVisualStyleBackColor = true;
            this.btnTabletSignnameBySelf.Click += new System.EventHandler(this.btnTabletSignnameBySelf_Click);
            // 
            // picPhoto
            // 
            this.picPhoto.Location = new System.Drawing.Point(3, 177);
            this.picPhoto.Name = "picPhoto";
            this.picPhoto.Size = new System.Drawing.Size(248, 269);
            this.picPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPhoto.TabIndex = 311;
            this.picPhoto.TabStop = false;
            // 
            // Form_photo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 452);
            this.Controls.Add(this.picPhoto);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpHealthExaminedUserInfo);
            this.Controls.Add(this.videoSourcePlayer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.tscbxCameras);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.Photograph);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_photo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拍照";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpHealthExaminedUserInfo.ResumeLayout(false);
            this.grpHealthExaminedUserInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpSignnameByFamilyMembers.ResumeLayout(false);
            this.grpSignnameByFamilyMembers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.piciSignnameByFamilyMembers)).EndInit();
            this.grpSignnameBySelf.ResumeLayout(false);
            this.grpSignnameBySelf.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSignnameBySelf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Photograph;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox tscbxCameras;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label1;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer;
        private System.Windows.Forms.GroupBox grpHealthExaminedUserInfo;
        private System.Windows.Forms.TextBox txtSFZH;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDGrdabh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grpSignnameBySelf;
        private System.Windows.Forms.PictureBox picSignnameBySelf;
        private System.Windows.Forms.Button btnTabletSignnameBySelf;
        private System.Windows.Forms.GroupBox grpSignnameByFamilyMembers;
        private System.Windows.Forms.PictureBox piciSignnameByFamilyMembers;
        private System.Windows.Forms.Button btnSignnameByFamilyMembers;
        private System.Windows.Forms.PictureBox picPhoto;
        private System.Windows.Forms.TextBox textBox_benren;
        private System.Windows.Forms.Button button_benren;
        private System.Windows.Forms.TextBox textBox_jiashu2;
        private System.Windows.Forms.Button button_jiashu;
        private System.Windows.Forms.Label label_jiashu;
        private System.Windows.Forms.Label label_benren;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePicker_jiashu;
        private System.Windows.Forms.DateTimePicker dateTimePicker_benren;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_jiashu;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

