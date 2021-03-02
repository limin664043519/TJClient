using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LIS;
using TJClient.Common;
using LISYQ;
using FBYClient;

namespace TJClient
{
    public partial class Form200B : Form
    {
        /// <summary>
        /// 仪器
        /// </summary>
        public static IInterface yqDemo = null;

        public Form200B()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string typeName = typeof(LISYQ.CBS).AssemblyQualifiedName;
            //string outMsg = "";
            //if (yqDemo != null)
            //{
            //    if (yqDemo.IsOpen(out outMsg) == false)
            //    {
            //        yqDemo.open(out outMsg);
            //    }
            //}
            //else
            //{

            //    string yqVersion = XmlRW.GetValueFormXML(System.Configuration.ConfigurationSettings.AppSettings["URT200B"].ToString(), "YQ_Version", "value");
            //    yqDemo = LisFactory.LisCreate(yqVersion);
            //}

            //DataTable dt = yqDemo.YQDataReturn("",out outMsg);

            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //string outMsg = "";
            //if (yqDemo != null)
            //{
            //    if (yqDemo.IsOpen(out outMsg) == false)
            //    {
            //        yqDemo.open(out outMsg);
            //    }
            //}
            //else
            //{

            //    string yqVersion = XmlRW.GetValueFormXML(System.Configuration.ConfigurationSettings.AppSettings["URT200B"].ToString(), "YQ_Version", "value");
            //    yqDemo = LisFactory.LisCreate(yqVersion);
            //}



            //DataTable dt = yqDemo.YQDataReturn("", out outMsg);
            ////string sss = yqDemo.getDataReceived(out outMsg);
            ////richTextBox1.Text = sss;
            //if (dt!=null)
            //{
            //    timer1.Enabled = false;
            //}
            //DataTable dt = yqDemo.YQDataReturn("", out outMsg);

            string outMsg = "";
            if (yqDemo != null)
            {
                if (yqDemo.IsOpen(out outMsg) == false)
                {
                    yqDemo.open(out outMsg);
                }
            }
            else
            {

                string yqVersion = TJClient.Common.XmlRW.GetValueFormXML(System.Configuration.ConfigurationSettings.AppSettings["US300"].ToString(), "YQ_Version", "value");
                yqDemo = LisFactory.LisCreate(yqVersion);
            }

            DataTable dt = yqDemo.YQDataReturn("", out outMsg);
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ////string typeName = typeof(US300).AssemblyQualifiedName;
            ////LISYQ.US300, US300, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            ////LISYQ.TM2655P, TM2655P, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            ////string typeName = typeof(LISYQ.URT200B).AssemblyQualifiedName;
            //string outMsg = "";
            //if (yqDemo != null)
            //{
            //    if (yqDemo.IsOpen(out outMsg) == false)
            //    {
            //        yqDemo.open(out outMsg);
            //    }
            //}
            //else
            //{

            //    string yqVersion = TJClient.Common.XmlRW.GetValueFormXML(System.Configuration.ConfigurationSettings.AppSettings["US300"].ToString(), "YQ_Version", "value");
            //    yqDemo = LisFactory.LisCreate(yqVersion);
            //}

            //DataTable dt = yqDemo.YQDataReturn("", out outMsg);

            string YqXmlPath = "";
            try
            {
                //if (drpFlag == true && comboBox_yq.SelectedValue != null && comboBox_yq.SelectedValue.ToString().Length > 0)
                //{
                    //xmlpath
                    YqXmlPath = System.Configuration.ConfigurationSettings.AppSettings["US300"].ToString();
                    //jg数据处理间隔时间
                    //YQ_Interval = XmlRW.GetValueFormXML(YqXmlPath, "YQ_Interval", "value");

                    ////rqlx	标本日期类型
                    //YQ_DateType = XmlRW.GetValueFormXML(YqXmlPath, "YQ_DateType", "value");

                    ////仪器数据处理类型
                    //YQ_Type = XmlRW.GetValueFormXML(YqXmlPath, "YQ_YQType", "value");

                    //启动自动获取数据
                    AutoForm autoform = new AutoForm();
                    //autoform.Owner = this;
                    autoform.Show();
                    autoform.Visible = false;
                    autoform.setStart("US300");

                    //检验结果初始化（右侧列表）
                    //init_dataGridView_hybb();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }
    }
}
