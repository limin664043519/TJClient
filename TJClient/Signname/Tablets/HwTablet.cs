using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Imaging;
using TJClient.Signname.Model;
using TJClient.SignnameForm;

namespace TJClient.Signname.Tablets
{
    public class HwTablet
    {
        public static void Capture(Tablet tablet)
        {
            Form_HanwangEsp560Signname frm = new Form_HanwangEsp560Signname();
            frm.TabletInfo = tablet;
            frm.ShowDialog();
        }
    }
}
