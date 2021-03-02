using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Florentis;

namespace TJClient
{
    public partial class FrmTestTablet : Form
    {
        public FrmTestTablet()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SigCtl sigCtl = new SigCtl();
            sigCtl.Licence = "AgAZAPZTkH0EBVdhY29tClNESyBTYW1wbGUBAoECA2UA";
            sigCtl.BackColor = 12;
            DynamicCapture dc = new DynamicCaptureClass();
            DynamicCaptureResult res = dc.Capture(sigCtl, "老年人健康体检  ", "  姓名:赵玉滨    性别:男    年龄:75\r\n  身份证号:370725198311181976\r\n  地址:潍坊市昌乐县乔官镇赵家淳于村", "what", null);
            if (res == DynamicCaptureResult.DynCaptOK)
            {
                //print("signature captured successfully");
                SigObj sigObj = (SigObj)sigCtl.Signature;
                sigObj.set_ExtraData("AdditionalData", "C# test: Additional data");
                String filename = "sig1.png";
                try
                {
                    sigObj.RenderBitmap(filename, 280, 120, "image/png", 2.5f, 0x000000, 0xffffff, 10.0f, 10.0f, RBFlags.RenderOutputFilename | RBFlags.RenderColor32BPP | RBFlags.RenderEncodeData);

                    using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        Bitmap bmp = new Bitmap(filename);

                        MemoryStream ms = new MemoryStream();
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                        byte[] arr = new byte[ms.Length];
                        ms.Position = 0;
                        ms.Read(arr, 0, (int)ms.Length);
                        ms.Close();
                        sigImage.Image = System.Drawing.Image.FromStream(fs);
                        fs.Close();

                        String strbaser64 = Convert.ToBase64String(arr);
                        textBox_singtext.Text = strbaser64;

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                //print("Signature capture error res=" + (int)res + "  ( " + res + " )");
                //switch (res)
                //{
                //    case DynamicCaptureResult.DynCaptCancel: print("signature cancelled"); break;
                //    case DynamicCaptureResult.DynCaptError: print("no capture service available"); break;
                //    case DynamicCaptureResult.DynCaptPadError: print("signing device error"); break;
                //    default: print("Unexpected error code "); break;
                //}
            }
        }

        private void btnSign_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            String msg;
            print("btnSign clicked");
            CaptureResult res = axSigCtl1.CtlCapture("Who", "Why");
            switch (res)
            {
                case CaptureResult.CaptureOK:
                    msg = "Signature captured successfully";
                    break;
                case CaptureResult.CaptureCancel:
                    msg = "Signature cancelled";
                    break;
                default: msg = "Capture error: " + res.ToString();
                    break;
            }
            print(msg);
        }
        private void print(string txt)
        {
            txtDisplay.Text += txt + "\r\n";
            txtDisplay.SelectionStart = txtDisplay.Text.Length;  // scroll to end
            txtDisplay.ScrollToCaret();
            txtDisplay.Refresh();
        }
    }
}
