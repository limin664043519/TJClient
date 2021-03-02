using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Common;
//using LISYQ;
using LIS;
using System.IO.Ports;
using FBYClient;

namespace TJClient
{
    public partial class Form1test : Form
    {
        public static IInterface yqDemo = null;
        public SerialPort commPort = null;
        public SerialPort commPort3 = null;
        public Form1test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Code128 _Code = new Code128();
                        _Code.ValueFont = new Font("宋体", 9);
                        _Code.Height = 50;
                        System.Drawing.Bitmap imgTemp = _Code.GetCodeImage("12345678945645", Code128.Encode.EAN128, "ceshi ", "ceshi ");
                        string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + "BarCode.gif";
                        imgTemp.Save(path, System.Drawing.Imaging.ImageFormat.Gif);
                        _Code.Code128Path.Add(path);
                        _Code.Code128Path.Add(path);
                        _Code.Code128Path.Add(path);
                        _Code.BarCodeShow();
        }


        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {


            try
            {
                //string typeName = typeof(TM2655P).AssemblyQualifiedName;
                //LISYQ.TM2655P, TM2655P, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
                string errMsg = "";
              
                if (yqDemo == null)
                {
                    string yqVersion = System.Configuration.ConfigurationSettings.AppSettings["TM2655P"].ToString();
                    yqDemo = LisFactory.LisCreate(yqVersion);
                }


                yqDemo.start(out errMsg);
                //yqDemo.YQDataReceived("", out errMsg);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }




            //int n = commPort.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
            //byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据
            //commPort.Read(buf, 0, n);//读取缓冲数据
            //builder = new StringBuilder();//清除字符串构造器的内容


            //string[] ssslist=richTextBox1.



            //builder.Append(Encoding.ASCII.GetString(buf));


            //int DataLength = serialPort1.BytesToRead;
            //byte[] ds = new byte[DataLength];
            //int len = serialPort1.Read(ds, 0, DataLength);
            ////然后对这个byte[]中的每个byte转为十六进制显示出来就行了。
            //string returnStr = "";
            //for (int i = 0; i < len; i++)
            //{
            //    returnStr += ds[i].ToString("X2") + " ";
            //}
            //textbox(returnStr);
        }



        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //string typeName = typeof(TM2655P).AssemblyQualifiedName;
                //LISYQ.TM2655P, TM2655P, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
                string errMsg = "";


                if (yqDemo == null)
                {
                    string yqVersion = System.Configuration.ConfigurationSettings.AppSettings["TM2655P"].ToString();
                    yqDemo = LisFactory.LisCreate(yqVersion);
                }

                yqDemo.stop(out errMsg);
                //yqDemo.YQDataReceived("", out errMsg);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
               // string typeName = typeof(TM2655P).AssemblyQualifiedName;
                //LISYQ.TM2655P, TM2655P, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
                string errMsg = "";


                if (yqDemo == null)
                {
                    string yqVersion = System.Configuration.ConfigurationSettings.AppSettings["TM2655P"].ToString();
                    yqDemo = LisFactory.LisCreate(yqVersion);
                }

                richTextBox1.Text = richTextBox1.Text + "::" + yqDemo.getDataReceived(out errMsg);

            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                MessageBox.Show(ex.Message);
                
            }
        }

        /// <summary>
        /// 接收结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string errMsg = "";
           
                commPort = new SerialPort();
                //Comm comm = new Comm();
                commPort.PortName = "com3";
                //波特率
                commPort.BaudRate = 9600;
                //数据位
                commPort.DataBits = 8;
                //停止位
                //StopBits stopbit=new StopBits();
                //if (IntStopbit == 1)
                //{
                //    stopbit = StopBits.One;
                //}
                //else if (IntStopbit == 2)
                //{
                //    stopbit = StopBits.Two;
                //}
                //else
                //{
                //    stopbit = StopBits.None;
                //}
                commPort.StopBits = StopBits.One;
                //无奇偶校验位
                //Parity parity = new Parity();
                //if(IntParity==1){
                //    parity = Parity.Even;
                //}else  if(IntParity==2){
                //    parity = Parity.Odd ;

                //}else  if(IntParity==3){
                //    parity = Parity.None ;

                //}else  if(IntParity==4){
                //    parity = Parity.Mark;

                //}else  if(IntParity==5){
                //    parity = Parity.Space;

                //}
                commPort.Parity = Parity.None;

                //获取或设置读取操作未完成时发生超时之前的毫秒数
                commPort.ReadTimeout = 1000;

                //获取或设置写入操作未完成时发生超时之前的毫秒数
                commPort.WriteTimeout = -1;

                //获取或设置一个值，该值指示在串行通信中是否启用请求发送 (RTS) 信号
                 commPort.RtsEnable = true;//根据实际情况吧。

                commPort.Open();

                //if (commPort.IsOpen)
                //{
                //    commPort.DataReceived += comm_DataReceived;
                //}

                commPort.Write("16H 16H 01H 30H 30H 02H 53H 54H 03H 07H");

           

                //commPort.Close();
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            //string errMsg = "";

            commPort = new SerialPort();
            commPort.PortName = "com4";
            //波特率
            commPort.BaudRate = 9600;
            //数据位
            commPort.DataBits = 8;
            //停止位
            commPort.StopBits = StopBits.One;
            //无奇偶校验位

            //}
            commPort.Parity = Parity.None;

            //获取或设置读取操作未完成时发生超时之前的毫秒数
            commPort.ReadTimeout = 2000;

            //获取或设置写入操作未完成时发生超时之前的毫秒数
            //commPort.WriteTimeout = -1;
            commPort.Open();
            //获取或设置一个值，该值指示在串行通信中是否启用请求发送 (RTS) 信号
            commPort.RtsEnable = true;//根据实际情况吧。

            

            //////if (commPort.IsOpen)
            //////{
            //////    commPort.DataReceived += comm_DataReceived;
            //////}

            ////commPort.Write("16H 16H 01H 30H 30H 02H 53H 54H 03H 07H");

            ////commPort.Close();


            //byte[] data = new byte[commPort.BytesToRead];
            //commPort.Read(data, 0, data.Length);
          
            //    //Received(null, new PortDataReciveEventArgs(data));
            //    string strDataReceived = System.Text.Encoding.Default.GetString(data);


            //textBox1 .Text =strDataReceived;
            //    //写日志
            //   // logWrite("DataReceived::" + strDataReceived);
            //   // return strDataReceived;
            //commPort.Close();
            //}
            if(!commPort.IsOpen ){
                return ;
            }
             string strDataReceived="";
            while (strDataReceived.Length == 0)
            {
                System.Threading.Thread.Sleep(2000);
                commPort.RtsEnable = true;
                byte[] data = new byte[commPort.BytesToRead];
                commPort.Read(data, 0, data.Length);

                //Received(null, new PortDataReciveEventArgs(data));
                 strDataReceived =strDataReceived+System.Text.Encoding.Default.GetString(data);
                textBox1.Text = strDataReceived;
                //strDataReceived = "";
            }
            commPort.Close();
        }

        DataTable dtList_jb = new DataTable();
        private void button8_Click(object sender, EventArgs e)
        {
            //DataTable dtList_jb = new DataTable();
            if (dtList_jb.Rows.Count == 0)
            {
                dtList_jb.Columns.Add("value");
                dtList_jb.Columns.Add("name");
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "2";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "高血压";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "3";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "糖尿病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "4";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "冠心病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "5";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "慢性阻塞性肺疾病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "6";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "恶性肿瘤";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "7";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "脑卒中";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "8";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "重性精神疾病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "9";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "结核病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "10";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "肝炎";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "11";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "其他法定传染病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "12";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "职业病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "13";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "其他";
            }



            //int count = this.tableLayoutPanel1.RowCount + 1;
            //this.tableLayoutPanel1.RowCount = count;

            //this.tableLayoutPanel1.Controls.Add(addJb(count, dtList_jb), 0, count - 1);

            //this.tableLayoutPanel1.Controls.Add(this.button_add, 1, count - 1);

        }



        public Panel addJb(int rowNo, DataTable dtList)
        {
            Panel panelDemo = new Panel();
            panelDemo.Name = "jb" + rowNo.ToString();
            panelDemo.Location = new Point(0, 0);
            //panelDemo.Dock = DockStyle.Fill;
            panelDemo.AutoSize = true;
            CheckBox check_tem = new CheckBox();
            check_tem.Width = 0;
            int x = 5, y = 0;
            for (int i = 0; i < dtList.Rows.Count; i++)
            {
                x = x + check_tem.Width+5;
                if (i == 5)
                {
                    LTextBox ltextbox = new LTextBox();
                    ltextbox.Name = "text_" + rowNo.ToString() + "_" + "EXZL";
                    ltextbox.Location = new Point(x,y);
                    ltextbox.LineType = LTextBox.BorderType.Bottom;
                    ltextbox.LineColor = Color.Black;
                    ltextbox.Width = 150;
                    panelDemo.Controls.Add(ltextbox);

                    // 增加 录入框  换行
                    x = 5;
                    y = y + 30;
                }
                else if (i == 11)
                {
                    LTextBox ltextbox = new LTextBox();
                    ltextbox.Name = "text_" + rowNo.ToString() + "_" + "zybqt";
                    ltextbox.Location = new Point(x, y);
                    ltextbox.LineColor = Color.Black;
                    ltextbox.LineType = LTextBox.BorderType.Bottom;
                    ltextbox.Width = 150;
                    panelDemo.Controls.Add(ltextbox);

                    // 增加 录入框  换行
                    x = 5;
                    y = y + 30;
                }
                CheckBox check = new CheckBox();
                check.AutoSize = true;
                check.Name = "chk_" + rowNo.ToString() + "_" + dtList.Rows[i]["value"].ToString();
                check.Text = dtList.Rows[i]["name"].ToString();
                check.Tag = dtList.Rows[i]["value"].ToString();
                //check.BackColor = Color.Red;
                check.Location = new Point(x, y);
                check_tem = check;
                
                panelDemo.Controls.Add(check);
            }
            x = x + check_tem.Width + 5;
            LTextBox ltextbox1 = new LTextBox();
            ltextbox1.Name = "text_" + rowNo.ToString() + "_" + "JBQT";
            ltextbox1.Location = new Point(x, y);
            ltextbox1.LineColor = Color.Black;
            ltextbox1.LineType = LTextBox.BorderType.Bottom;
            ltextbox1.Width = 150;
            panelDemo.Controls.Add(ltextbox1);

           
            //确诊时间
            x = 5;
            y = y + 30;
            Label label = new Label();
            label.Text = "确诊时间";
            label.Location = new Point(x,y);
            label.AutoSize = true;
            panelDemo.Controls.Add(label);
            
            x = x + label.Width + 5;
            DateTimePicker datetimepicker = new DateTimePicker();
            datetimepicker.Location = new Point(x, y);
            datetimepicker.Name = "dtp_" + rowNo.ToString() + "_" + "D_ZDRQ";
            datetimepicker.Width = 150;
            panelDemo.Controls.Add(datetimepicker);
            return panelDemo;
        }

    //遍历

        public DataTable sss(DataTable dtList)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("D_GRDABH");
            dt.Columns.Add("D_JBMC");
            dt.Columns.Add("D_JBBM");
            dt.Columns.Add("D_ZDRQ");
            dt.Columns.Add("D_ZDDW");
            dt.Columns.Add("D_CLQK");
            dt.Columns.Add("EXZL");
            dt.Columns.Add("JBQT");
            dt.Columns.Add("zybqt");
            dt.Columns.Add("zt");
            dt.Columns.Add("zlbz");
            dt.Columns.Add("nd");
           
            for (int i = 1; i <= tableLayoutPanel3.RowCount; i++)
            {
                dt.Rows.Add();
                string strD_JBMC = "";//疾病名称
                string D_JBBM = "";//疾病编码
                string D_ZDRQ = "";//诊断日期
                string EXZL = "";//恶性肿瘤
                string JBQT = "";//疾病其他
                string zybqt = "";//职业病其他

                for (int j = 0; j < dtList.Rows .Count; j++)
                {
                   // Panel panelInCell = tableLayoutPanel1.GetControlFromPosition(j, i) as Panel;
                    string D_JBBM_tem = GetControlValueByName("chk_" + i.ToString() + "_" + dtList.Rows[j]["value"].ToString());
                    if (D_JBBM_tem.Length > 0)
                    {
                        D_JBBM = D_JBBM + D_JBBM_tem + ",";
                    }
                    string strD_JBMC_tem = GetControlTextByName("chk_" + i.ToString() + "_" + dtList.Rows[j]["value"].ToString());
                    if (strD_JBMC_tem.Length > 0)
                    {
                        strD_JBMC = strD_JBMC + strD_JBMC_tem + ",";
                    }
                }
                //恶性肿瘤
                EXZL = GetControlValueByName("text_" + i.ToString() + "_" + "EXZL");
                //疾病其他
                JBQT = GetControlValueByName("text_" + i.ToString() + "_" + "JBQT");
                //职业病其他
                zybqt = GetControlValueByName("text_" + i.ToString() + "_" + "zybqt");
                //诊断日期
                D_ZDRQ = GetControlValueByName("dtp_" + i.ToString() + "_" + "D_ZDRQ");

                dt.Rows[dt.Rows .Count -1]["D_JBBM"] = D_JBBM;
                dt.Rows[dt.Rows.Count - 1]["D_JBMC"] = strD_JBMC;
                dt.Rows[dt.Rows.Count - 1]["EXZL"] = EXZL;
                dt.Rows[dt.Rows.Count - 1]["JBQT"] = JBQT;
                dt.Rows[dt.Rows.Count - 1]["zybqt"] = zybqt;
                dt.Rows[dt.Rows.Count - 1]["D_ZDRQ"] = D_ZDRQ;
                dt.Rows[dt.Rows.Count - 1]["zt"] = "2";
                dt.Rows[dt.Rows.Count - 1]["zlbz"] = "1";
                dt.Rows[dt.Rows.Count - 1]["nd"] = DateTime .Now .Year .ToString();
            }
            return dt;
        }


        /// <summary>
        /// 按照控件名称获取控件的值
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private string GetControlValueByName(string ControlName)
        {
            Control control = Controls.Find(ControlName, true)[0];
            string value = "";
            //text
            if (ControlName.IndexOf("text_") > -1)
            {
                TextBox TextBox_tem = (TextBox)control;
                value = TextBox_tem.Text;
            }
            //checkBox
            else if (ControlName.IndexOf("chk_") > -1)
            {
                CheckBox checkBox_tem = (CheckBox)control;
                if (checkBox_tem.Checked == true)
                {
                    value = checkBox_tem.Tag.ToString();
                }
                else
                {
                    value = "";
                }
            }
            else if (ControlName.IndexOf("dtp_") > -1)
            {
                DateTimePicker checkBox_tem=(DateTimePicker)control;
                if (checkBox_tem.Checked == true)
                {
                    value = checkBox_tem.Value .ToString ("yyyy-MM");
                }
                else
                {
                    value = "";
                }
            }
            return value;
        }

        /// <summary>
        /// 按照控件名称获取控件的名称
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private string GetControlTextByName(string ControlName)
        {
            Control control = Controls.Find(ControlName, true)[0];
            string text = "";
            //checkBox
             if (ControlName.IndexOf("chk_") > -1)
            {
                CheckBox checkBox_tem = (CheckBox)control;
                if (checkBox_tem.Checked == true)
                {
                    text = checkBox_tem.Text.ToString();
                }
                else
                {
                    text = "";
                }
            }
            return text;
        }
        /// <summary>
        /// 给控件赋值
        /// </summary>
        /// <param name="ControlId"></param>
        /// <param name="ControlValue"></param>
        /// <returns></returns>
        private void setValueToControl(string ControlId, string ControlValue)
        {
            try
            {
                Control control = Controls.Find(ControlId, true)[0];
                //text
                if (ControlId.IndexOf("text_") > -1)
                {
                    TextBox TextBox_tem = (TextBox)control;
                    TextBox_tem.Text = ControlValue;
                }
                //checkBox
                else if (ControlId.IndexOf("chk_") > -1)
                {
                    CheckBox checkBox_tem = (CheckBox)control;
                    if (ControlValue.Trim().Length > 0)
                    {
                        checkBox_tem.Checked = true;
                    }
                    else
                    {
                        checkBox_tem.Checked = false;
                    }
                }
               
            }
            catch (Exception ex)
            {
            }
        }


        private void button8_Click_1(object sender, EventArgs e)
        {
            DataTable ddd = sss(dtList_jb);
        }


       static int RowCount_jb = 0;

        private void button10_Click(object sender, EventArgs e)
        {
            //DataTable dtList_jb = new DataTable();
            if (dtList_jb.Rows.Count == 0)
            {
                dtList_jb.Columns.Add("value");
                dtList_jb.Columns.Add("name");
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "2";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "高血压";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "3";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "糖尿病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "4";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "冠心病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "5";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "慢性阻塞性肺疾病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "6";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "恶性肿瘤";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "7";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "脑卒中";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "8";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "重性精神疾病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "9";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "结核病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "10";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "肝炎";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "11";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "其他法定传染病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "12";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "职业病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "13";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "其他";
            }



            RowCount_jb = RowCount_jb + 1;
            this.tableLayoutPanel3.RowCount = RowCount_jb;
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle());

            this.tableLayoutPanel3.Controls.Add(addJb(RowCount_jb, dtList_jb), 0, RowCount_jb - 1);

            Button button_add = new Button();
            button_add.Click += button10_Click_1;
            button_add.Text = "删除";
            button_add.Tag = (RowCount_jb-1).ToString();
            this.tableLayoutPanel3.Controls.Add(button_add, 1, RowCount_jb - 1);
        }

        public bool init_jb()
        {
            if (dtList_jb.Rows.Count == 0)
            {
                dtList_jb.Columns.Add("value");
                dtList_jb.Columns.Add("name");
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "2";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "高血压";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "3";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "糖尿病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "4";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "冠心病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "5";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "慢性阻塞性肺疾病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "6";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "恶性肿瘤";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "7";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "脑卒中";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "8";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "重性精神疾病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "9";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "结核病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "10";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "肝炎";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "11";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "其他法定传染病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "12";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "职业病";
                dtList_jb.Rows.Add();
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["value"] = "13";
                dtList_jb.Rows[dtList_jb.Rows.Count - 1]["name"] = "其他";
            }


            int index = tableLayoutPanel3.RowCount++;
            RowCount_jb = RowCount_jb + 1;
            RowStyle style = new RowStyle(SizeType.AutoSize);
            tableLayoutPanel3.RowStyles.Add(style);
            //return index;

            //int rowIndex = AddTableRow();
            this.tableLayoutPanel3.Controls.Add(addJb(RowCount_jb, dtList_jb), 0, RowCount_jb - 1);
            Button button_add = new Button();
            button_add.Click += button10_Click;
            button_add.Text = "添加";
            this.tableLayoutPanel3.Controls.Add(button_add, 1, RowCount_jb - 1);

            //tableLayoutPanel3.Controls.Add(label, LabelColumnIndex, index);
            //if (value != null)
            //{
            //    detailTable.Controls.Add(value, ValueColumnIndex, rowIndex);
            //}



            //RowCount_jb = RowCount_jb + 1;
            ////this.tableLayoutPanel3.RowCount = RowCount_jb;

            //this.tableLayoutPanel3.Controls.Add(addJb(RowCount_jb, dtList_jb), 0, RowCount_jb - 1);

            //Button button_add = new Button();
            //button_add.Click += button10_Click;
            //button_add.Text = "添加";
            //this.tableLayoutPanel3.Controls.Add(button_add, 1, RowCount_jb - 1);
            return true;
        }

        private void Form1test_Load(object sender, EventArgs e)
        {
            init_jb();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            int iRowId = int.Parse(((Button)sender).Tag.ToString());

            this.tableLayoutPanel3.SuspendLayout();

            Control c = null;

            //delete current row controls
            for (int j = 0; j < this.tableLayoutPanel3.ColumnCount; j++)
            {
                c = this.tableLayoutPanel3.GetControlFromPosition(j, iRowId);
                if (c != null)
                {
                    this.tableLayoutPanel3.Controls.Remove(c);
                }
            }

            //need to shift all controls up one row from delete point
            TableLayoutPanelCellPosition controlPosition;
            for (int i = iRowId; i < this.tableLayoutPanel3.RowCount; i++)
            {
                for (int j = 0; j < this.tableLayoutPanel3.ColumnCount; j++)
                {
                    c = this.tableLayoutPanel3.GetControlFromPosition(j, i + 1);
                    if (c == null)
                        break;
                    c.Tag = i;
                    controlPosition = new TableLayoutPanelCellPosition(j, i);
                    this.tableLayoutPanel3.SetCellPosition(c, controlPosition);
                }
            }

            //need to remove the row
            this.tableLayoutPanel3.RowCount--;

            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();

           
        }

    }
}
