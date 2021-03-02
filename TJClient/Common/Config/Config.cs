using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJClient.Common.Config
{
    public class Config
    {
        public static string GetContent(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path, Encoding.UTF8);
            }

            return "";
        }

        public static bool SetContent(string path,string content)
        {

            File.WriteAllText(path, content, Encoding.UTF8);
            return true;
        }
    }
}
