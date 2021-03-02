using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Florentis;
using TJClient.Signname.Model;

namespace TJClient.Signname.Tablets
{
    public class WebComTablet
    {
        public static void Capture(Tablet tablet)
        {
            SigCtl sigCtl = new SigCtl();
            sigCtl.Licence = Common.TabletKey();
            sigCtl.BackColor = 12;
            DynamicCapture dc = new DynamicCaptureClass();
            DynamicCaptureResult res = dc.Capture(sigCtl, tablet.Who, tablet.Why, tablet.What, null);
            if (res == DynamicCaptureResult.DynCaptOK)
            {
                SigObj sigObj = (SigObj)sigCtl.Signature;
                String filename = tablet.SaveSignnamePicPath;
                try
                {
                    sigObj.RenderBitmap(filename, 150, 100, "image/png", 2.0f, 0x00000000, 0x00FFFFFF, 0, 0, RBFlags.RenderOutputFilename | RBFlags.RenderColor32BPP | RBFlags.RenderEncodeData);
                    SignnameResult.Msg = "";
                    SignnameResult.Signnamed = true;
                }
                catch (Exception ex)
                {
                    SignnameResult.Msg = ex.Message;
                }
            }
            else
            {
                SignnameResult.Signnamed = false;
                SignnameResult.Msg = "";
            }
            if (res == DynamicCaptureResult.DynCaptNotLicensed)
            {
                SignnameResult.Msg = "签字版连接故障！请检查签字版是否已经正确连接！";
            }

        }
    }
}
