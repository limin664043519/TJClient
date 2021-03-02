using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TJClient.Helper;

namespace TJClient.NeedToUseForm
{
    public partial class FrmXdtSendLog : Form
    {

        public static string _xdType = "";

        public FrmXdtSendLog()
        {
            InitializeComponent();
        }

        private void Operation()
        {
            txtLog.Text = "";
            LogHelper helper=new LogHelper(_xdType,false);
            txtLog.Text = helper.GetMessage();
        }
        private void FrmXdtSendLog_Load(object sender, EventArgs e)
        {
            Operation();
        }
    }
}
