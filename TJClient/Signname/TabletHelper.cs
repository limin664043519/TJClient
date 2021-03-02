using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Signname.Model;
using Florentis;
using TJClient.Signname.Tablets;

namespace TJClient.Signname
{
    public class TabletHelper
    {
        private static bool SignnameOperation(Tablet tablet, out string msg)
        {
            SignnameResult.Signnamed = false;
            SignnameResult.Msg = "Custom:签字版连接错误";
            switch (tablet.TabletType)
            {
                case "1":
                    WebComTablet.Capture(tablet); //在里面更改了SignnameResult的值。
                    break;
                case "2":
                    HwTablet.Capture(tablet);
                    break;
            }
            msg = SignnameResult.Msg;
            return SignnameResult.Signnamed;
        }

        public static string TabletSignname(string way,PictureBox picControl,string saveSignnamePicPath)
        {
            Tablet tablet = new Tablet();
            tablet.Who = SignnameConst.TabletWho;
            tablet.Why = way;
            tablet.What = SignnameConst.TabletWhat;
            tablet.SaveSignnamePicPath = saveSignnamePicPath;
            string msg = "";
            if (SignnameOperation(tablet, out msg))
            {
                ControlOperation.SignnamePicInit(picControl, tablet.SaveSignnamePicPath,"",null);
            }
            return msg;
        }

        
    }
}
