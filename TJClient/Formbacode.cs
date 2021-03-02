using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TJClient
{
    public partial class Formbacode : Form
    {
        public Formbacode()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //TJClient.Common.BarCode _Code = new TJClient.Common.BarCode(); 
            //_Code.ValueFont = new Font("宋体", 20);
            //System.Drawing.Bitmap imgTemp = _Code.GetCodeImage("18000263198501", TJClient.Common.BarCode.Encode.Code128C);
            //imgTemp.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\" + "BarCode.gif", System.Drawing.Imaging.ImageFormat.Gif);

            string resultTest = textBox1.Text;
            int tzindex = resultTest.ToLower().IndexOf("w");
            //ErrorLog(resultTest.Substring(tzindex + 1, 6));
            decimal tz = Convert.ToDecimal(resultTest.Substring(tzindex + 2, 5));
            int sgindex = resultTest.ToLower().IndexOf("h");
            decimal sg = Convert.ToDecimal(resultTest.Substring(sgindex + 2, 5));
            decimal bmi = (tz / sg / sg)*10000;
            int yaowei = Convert.ToInt32(Convert.ToDouble(bmi) * 3.5);


        }
    }
}
