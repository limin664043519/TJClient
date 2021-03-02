using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace clientDoWebService.common
{
    //添加日期：2017-08-24
    //作者：mq
    //作用：判断文件是否存在
    public class CommonFile
    {

        public static bool IsExist(List<string> files)
        {
            bool result = true;
            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    result = false;
                }
            }
            return result;
        }
        public static bool IsExist(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}