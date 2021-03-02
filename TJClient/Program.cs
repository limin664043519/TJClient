using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FBYClient;
using TJClient.sys;
using TJClient.SignnameForm;

namespace TJClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1_test());
            //Application.Run(new Login());
            //Application.Run(new Form_sysEdit());
            //Application.Run(new Form200B()); 
            //Application.Run(new Formbacode());
                //Application.Run(new Form_HanwangEsp560Signname());
                Application.Run(new Login());
        }
    }
}
