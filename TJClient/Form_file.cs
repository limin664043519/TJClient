using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using FBYClient;
using TJClient.Common;

namespace TJClient
{
    public partial class Form_file : Form
    {
        public Form_file()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            JKTJ.ClientDoService client = new JKTJ.ClientDoService();

            //上传服务器后的文件名  一般不修改文件名称
            int start = textBox1.Text.LastIndexOf("\\");
            int length = textBox1.Text.Length;
            string serverfile = DateTime .Now .ToString ("yyyyMMddHHmmssfff")+ Path.GetFileName(textBox1.Text);

            client.CreateFile(serverfile);

            //要上传文件的路径
            string sourceFile = textBox1.Text;
            string md5 = GetMD5(sourceFile);

            FileStream fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            int size = (int)fs.Length;
            int bufferSize = 1024 * 512;
            int count = (int)Math.Ceiling((double)size / (double)bufferSize);
            for (int i = 0; i < count; i++)
            {
                int readSize = bufferSize;
                if (i == count - 1)
                    readSize = size - bufferSize * i;
                byte[] buffer = new byte[readSize];
                fs.Read(buffer, 0, readSize);
                client.Append(serverfile, buffer);
            }

            bool isVerify = client.Verify(serverfile, md5);
            if (isVerify)
            {
                string result = client.DoFileThread("zyb","guid","yljgbm");
                MessageBox.Show("上传成功");
            }
            else
                MessageBox.Show("上传失败");

        }

        /// <summary>
        /// 获取md5加密
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetMD5(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            MD5CryptoServiceProvider p = new MD5CryptoServiceProvider();
            byte[] md5buffer = p.ComputeHash(fs);
            fs.Close();
            string md5Str = "";
            List<string> strList = new List<string>();
            for (int i = 0; i < md5buffer.Length; i++)
            {
                md5Str += md5buffer[i].ToString("x2");
            }
            return md5Str;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            //openDialog.Filter = "视频文件(*.avi,*.wmv,*.mp4)|*.avi;*.wmv;*.mp4";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            JKTJ.ClientDoService client = new JKTJ.ClientDoService();
            //client.test("123");
            string aa = DateTime .Now .ToString ("yyyy-MM-dd HH:mm:ss fff");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UserInfo.userId = "zyb";
            UserInfo.Yybm = "001";
            DataDownLoad datadownload = new DataDownLoad();
            datadownload.Show();
        }
        public int count = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            //RichTextBox richtextbox = new RichTextBox();



            string strInput = "adf";
            int p1 = richTextBox1.TextLength;  //取出未添加时的字符串长度。   
            richTextBox1.AppendText(strInput + "\r\n");  //保留每行的所有颜色。 
           
            int p2 = strInput.Length;  //取出要添加的文本的长度   
            richTextBox1.Select(p1, p2);        //选中要添加的文本   
            //if (count % 2 == 0)
            //{
                richTextBox1.SelectionColor = Color.Red;  //设置要添加的文本的字体色 
            //}
            //else
            //{
            //    //richTextBox1.SelectionColor = Color.Black;  //设置要添加的文本的字体色 
            //}
            //count++;
        }
    }
}
