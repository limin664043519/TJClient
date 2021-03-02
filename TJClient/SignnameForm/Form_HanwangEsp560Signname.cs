using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Signname.Model;

namespace TJClient.SignnameForm
{
    public partial class Form_HanwangEsp560Signname : Form
    {
        public Form_HanwangEsp560Signname()
        {
            InitializeComponent();
        }
        
        public Tablet TabletInfo=null; //记录签字版Tablet信息

        private static int _completeMsg = 0x7ffe;
        private static int _cancelMsg = 0x7ffd;
        private static int _HWeOk = 0;
        private void Form_HanwangEsp560Signname_Load(object sender, EventArgs e)
        {
            HWSignname.HWSetBkColor(0xE0F8E0);
            HWSignname.HWSetCtlFrame(2, 0x000000);
            HWSignname.HWSetExtWndHandle(this.Handle.ToInt32());
            int res = HWSignname.HWInitialize();
            if (res != _HWeOk)
            {
                SignnameResult.Msg="汉王签字版设备开启失败";
                this.Close();
                return;
            }
            lblWhy.Text = TabletInfo.Why;
            lblWhat.Text = TabletInfo.What;
            lblDatetime.Text = DateTime.Now.ToString("F");
            this.Text = "汉王签字板开启签名";
        }



        private void btnClear_Click(object sender, EventArgs e)
        {
            HWSignname.HWClearPenSign();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveSignname();
        }

        private void SaveSignname()
        {
            try
            {
                HWSignname.HWSetFilePath(TabletInfo.SaveSignnamePicPath);
                HWSignname.HWSaveFile();
                SignnameResult.Signnamed = true;
                SignnameResult.Msg = "";
            }
            catch (Exception ex)
            {
                SignnameResult.Msg = ex.Message;

            }
            finally
            {
                this.Close();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == _completeMsg)
            {
                SaveSignname();
            }
            else if (m.Msg == _cancelMsg)
            {
                
            }

            base.WndProc(ref m);
        }

        private void Form_HanwangEsp560Signname_FormClosing(object sender, FormClosingEventArgs e)
        {
            HWSignname.HWFinalize();
        }
    }
}
