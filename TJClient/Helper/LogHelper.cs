using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJClient.Helper
{
    public class LogHelper
    {
        private string _filePath = "";
        public LogHelper(string fileName,bool ifExistedDeleteFile)
        {
            string dir=Path.Combine(TJClient.Signname.Common.GetCurrRunExeDir(),"log");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            _filePath = Path.Combine(dir, string.Format("{0}.txt", fileName));
            if (ifExistedDeleteFile && File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
        public void AppendMessageToTxt(string message)
        {
            File.AppendAllText(_filePath,message+Environment.NewLine,Encoding.UTF8);
        }

        public string GetMessage()
        {
            if (File.Exists(_filePath))
            {
                return File.ReadAllText(_filePath, Encoding.UTF8);
            }

            return "";
        }
    }
}
