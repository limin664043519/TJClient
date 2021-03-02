using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LIS;

namespace TJClient
{
    public partial class FCP7101 : Form
    {
        public FCP7101()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string yqVersion = System.Configuration.ConfigurationSettings.AppSettings["FCP7101"].ToString();
              IInterface yqDemo    = LisFactory.LisCreate(yqVersion);
              string ddd = "";
              yqDemo.YQDataReturn("", out ddd);





            //richTextBox1.Text = richTextBox1.Text + "::" + yqDemo.getDataReceived(out errMsg);
        }
    }
}
