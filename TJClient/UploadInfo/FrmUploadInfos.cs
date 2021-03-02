using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Common;
using TJClient.JKTJ;

namespace TJClient.UploadInfo
{
    public partial class FrmUploadInfos : Form
    {
        public FrmUploadInfos()
        {
            InitializeComponent();
        }

        private void FrmUploadInfos_Load(object sender, EventArgs e)
        {
            GetUploadLoggerInfos();
        }

        private void AppendLine(string line, Color color)
        {
            rt.SelectionColor = color;
            rt.AppendText(line);
            rt.AppendText(Environment.NewLine);
        }
        private void ShowMessage(LoggerInfo[] infos)
        {
            foreach (LoggerInfo info in infos)
            {
                Color color = Color.Black;
                if (info.Status == 0)
                {
                    color = Color.Red;
                }
                string line = string.Format("[{0}]:[{1}]:[{2}]:{3}", info.CreateDate,info.IP, info.Czy, info.Info);
                if (info.IP == "")
                {
                    line = string.Format("[{0}]:[{1}]:{2}", info.CreateDate,info.Czy, info.Info);
                }
                AppendLine(line,color);
            }
        }
        private void GetUploadLoggerInfos()
        {
            string czy = UserInfo.userId;
            var client = new ClientDoService();
            client.Url = ConfigurationManager.AppSettings["GwtjUrl"];
            client.GetUploadLoggerInfosCompleted+=new GetUploadLoggerInfosCompletedEventHandler(GetUploadLoggerInfos_Completed);
            client.GetUploadLoggerInfosAsync(czy);
        }

        private void GetUploadLoggerInfos_Completed(object sender,GetUploadLoggerInfosCompletedEventArgs e)
        {
            LoggerInfo[] infos=e.Result;

            ShowMessage(infos);
        }
    }

    //public class LoggerInfo
    //{
    //    public string TableName { set; get; }
    //    public string IP { set; get; }
    //    public string Czy { set; get; }
    //    public string Info { set; get; }
    //    public string GUID { set; get; }
    //    public int Status { set; get; }
    //    public string CreateDate { set; get; }
    //}
}
