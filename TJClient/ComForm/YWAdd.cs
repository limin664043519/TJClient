using FBYClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TJClient.ComForm
{
    public partial class YWAdd : Form
    {
        public YWAdd()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBAccess dBAccess = new DBAccess();
            dBAccess.ExecuteNonQueryBySql("insert into SYS_YW_DICS (YWMC,YWPYM)values('" + txt_ywmc.Text + "','" + txt_ywpym.Text.ToUpper() + "')");
            this.Close();
        }
    }
}
