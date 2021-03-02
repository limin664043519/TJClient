using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Bll;
using TJClient.model;

namespace TJClient.NeedToUseForm
{
    public partial class FrmLisTmCorresponding : Form
    {
        public FrmLisTmCorresponding()
        {
            InitializeComponent();
        }

        private void FrmLisTmCorresponding_Load(object sender, EventArgs e)
        {
            QmLisTm.Info = "";
            QmLisTm.AssignedValue = false;
            QmLisTm.IncludeBlood = 1;
            TxtLisTm.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string tm = TxtLisTm.Text.Trim();
            if (!string.IsNullOrEmpty(tm))
            {
                if (chkHadUsed.Checked)
                {
                    //判断条码是否存在
                    if (LisTmCorrespondingBll.TmHadUsed(tm))
                    {
                        MessageBox.Show("外部条码已经使用，不能再次对应");
                        return;
                    }
                }
                QmLisTm.Info = tm;
                QmLisTm.AssignedValue = true;
                QmLisTm.IncludeBlood = chkBlood.Checked ? 0 : 1;
                this.Close();
            }
        }

        private void TxtLisTm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnOK_Click(sender, e);
            }
        }
    }
}
