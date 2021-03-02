using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Common;

namespace TJClient
{
    public partial class Form_HealthExaminedUserSignname : Form
    {
        public Form_HealthExaminedUserSignname()
        {
            InitializeComponent();
        }

        private void Form_HealthExaminedUserSignname_Load(object sender, EventArgs e)
        {
            txtName.Text = Signname.Model.HealthExaminedUserInfo.Name;
            txtDGrdabh.Text = Signname.Model.HealthExaminedUserInfo.DGrdabh;
            txtSFZH.Text = Signname.Model.HealthExaminedUserInfo.CardNo;
        }

        private void SaveSignnameToDbAfterSignnameSuccess(string saveSignnamePicPath, string signnameGroup)
        {
            Signname.Model.JktjSignname jktjSignname = new Signname.Model.JktjSignname()
            {
                SignnamePicPath = saveSignnamePicPath,
                Czy = UserInfo.userId,
                Yljgbm = UserInfo.Yybm,
                Tjsj = Common.Common.FormatDateTime(CommomSysInfo.tjsj),
                D_Grdabh = txtDGrdabh.Text,
                SignnameGroup = signnameGroup
            };
            Signname.Operation.SaveJktjSignname(jktjSignname);
        }

        private void HealthExaminedUserSignnameOperation(PictureBox picControl,string signnameGroup)
        {
            string way = string.Format("姓名：{0}  身份证号：{1}", txtName.Text, txtSFZH.Text);
            string saveSignnamePicPath = Signname.Operation.GetTabletSignnamePicPath();
            string msg = Signname.TabletHelper.TabletSignname(way, picControl, saveSignnamePicPath);
            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
                return;
            }
            //如果签名成功，将签名保存到数据库
            SaveSignnameToDbAfterSignnameSuccess(saveSignnamePicPath,signnameGroup);

        }

        private void btnTabletSignnameBySelf_Click(object sender, EventArgs e)
        {
            HealthExaminedUserSignnameOperation(picSignnameBySelf, "FKQZBR");
        }

        private void btnSignnameByFamilyMembers_Click(object sender, EventArgs e)
        {
            HealthExaminedUserSignnameOperation(piciSignnameByFamilyMembers, "FKQZJS");
        }
    }
}
