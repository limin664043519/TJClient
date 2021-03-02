using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TJClient.Devices.Com;
using Yq.Common.Model;

namespace TJClient.Devices.Common
{
    public class Xueyaji
    {
        public static string GetBreathRate(string pul)
        {
            return (int.Parse(pul) / 4).ToString();
        }

        public static DataTable GetDt(string yqxh,string ybh, string jyrq, string hysj, string sys, string dia, string pul)
        {
            DataTable dt = TJClient.Devices.Com.TjDataTable.GetStructureDataTable(yqxh);
            TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "DATE", "检验日期", jyrq, "", ""), ref dt);

            TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "ERRORCODE", "错误编码", "", "", ""), ref dt);

            TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "SYSTOLIC", "收缩压", sys, "", ""), ref dt);

            TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "MEANBLOODPRESSURE", "平均血压", "", "", ""), ref dt);
            TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "DIASTOLIC", "舒张压", dia, "", ""), ref dt);
            TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "PULSERATE", "脉率", pul, "", ""), ref dt);
            TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "BREATHRATE", "呼吸频率", GetBreathRate(pul), "", ""), ref dt);
            return dt;
        }
    }
}
