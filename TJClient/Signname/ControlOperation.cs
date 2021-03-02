using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Cells.Drawing;
using TJClient.Signname.Model;

namespace TJClient.Signname
{
    public class ControlOperation
    {
        /// <summary>
        /// 设置下拉框的签名
        /// </summary>
        /// <param name="cbo">需要操作的控件</param>
        /// <param name="signnames">签名列表</param>
        public static void SignnameCboInit(System.Windows.Forms.ComboBox cbo,List<UserSignname> signnames)
        {
            cbo.Items.Clear();

            foreach (UserSignname signname in signnames)
            {
                //cbo.Items.Add(signname.SignnameTitle);

                cbo.Items.Add(signname);
            }
        }
        /// <summary>
        /// 获取下拉框的签名标题
        /// </summary>
        /// <param name="cbo">需要操作的控件</param>
        /// <returns></returns>
        public static string SignnameTitle(System.Windows.Forms.ComboBox cbo)
        {
            return cbo.Text;
        }

        /// <summary>
        /// 获取签名图片的绝对路径
        /// </summary>
        /// <param name="picPath">相对路径</param>
        /// <returns></returns>
        private static string GetPicAbsolutePath(string picPath)
        {
            string dir = Common.GetTabletSignnameDirectory();
            if (picPath.ToLower().Contains("default"))
            {
                dir = Common.GetCurrRunExeDir();
                //dir = Directory.GetCurrentDirectory();
            }
            if (picPath.IndexOf('/') == 0)
            {
                picPath = picPath.Substring(1, picPath.Length - 1);
            }
            return Path.Combine(dir, picPath);
        }

        /// <summary>
        /// 在picturebox中设置显示签名图片
        /// </summary>
        /// <param name="pic">需要操作的picturebox控件</param>
        /// <param name="picPath">签名图片路径</param>
        public static void SignnamePicInit(System.Windows.Forms.PictureBox pic, string picPath,string realname,System.Windows.Forms.TextBox txt)
        {
            string path = GetPicAbsolutePath(picPath);
            if (!FileHelper.FileExists(path))
            {
                //签名图片不存在，清空签名图片
                pic.Image = null;
                pic.ImageLocation = "";
                //return;
                //errMsg = "签名图片不存在！";
            }
            else
            {
                //设定签名图片
                pic.Image = Image.FromFile(path);
                pic.ImageLocation = path;
            }
            //文字签名框赋值
            if (txt != null)
            {
                txt.Text = realname;
            }
            
        }
    }
}
