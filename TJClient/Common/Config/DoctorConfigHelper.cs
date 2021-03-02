using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJClient.Common.Config
{
    public class DoctorConfigHelper:Config
    {

        public static string GetDoctor()
        {
            string doctorConfigPath = ConfigHelper.GetDoctorConfigFilePath();
            string content= GetContent(doctorConfigPath);
            return content;
        }

        public static bool SetDoctor(string doctor)
        {
            string doctorConfigPath = ConfigHelper.GetDoctorConfigFilePath();
            return SetContent(doctorConfigPath, doctor);
        }
    }
}
