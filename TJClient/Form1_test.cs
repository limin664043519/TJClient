using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using gwtjUpload;
using TJClient.Common;
using lis信息下载上传接口;

namespace TJClient
{
    public partial class Form1_test : Form
    {
        public Form1_test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserInfo.Yybm = "001";

            UserInfo.Yymc = "张店中西你卫生院";

            LisImport lisimport = new LisImport();
            lisimport.Show();
        }
    }
}
