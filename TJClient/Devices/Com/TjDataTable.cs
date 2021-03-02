using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yq.Common.Model;

namespace TJClient.Devices.Com
{
    public class TjDataTable
    {
        public static DataTable GetStructureDataTable(string yqxh)
        {
            DataTable dtResult = new DataTable();
            //仪器代号
            DataColumn dtcolum_yq = new DataColumn("yq");
            dtcolum_yq.DefaultValue = yqxh;
            dtResult.Columns.Add(dtcolum_yq);

            //化验日期
            dtResult.Columns.Add("jyrq");
            //化验时间
            dtResult.Columns.Add("hysj");
            //样本号
            dtResult.Columns.Add("ybh");
            //项目代号
            dtResult.Columns.Add("xmdh");
            //项目名称
            dtResult.Columns.Add("xmmc");
            //化验结果
            dtResult.Columns.Add("result");
            dtResult.Columns.Add("result1");
            dtResult.Columns.Add("dw");
            //最小值
            dtResult.Columns.Add("lowerValue");
            //最大值
            dtResult.Columns.Add("heightValue");
            return dtResult;
        }
        public static void AddInfoToDt(BaseInfo info, ref DataTable dtResult)
        {
            dtResult.Rows.Add();
            //日期
            dtResult.Rows[dtResult.Rows.Count - 1]["jyrq"] = info.Jyrq;
            dtResult.Rows[dtResult.Rows.Count - 1]["hysj"] = info.Hysj;
            //样本号  条码号,来自文件名
            dtResult.Rows[dtResult.Rows.Count - 1]["ybh"] = info.Ybh;
            //项目名称
            dtResult.Rows[dtResult.Rows.Count - 1]["xmmc"] = info.Xmmc;
            //项目编号
            dtResult.Rows[dtResult.Rows.Count - 1]["xmdh"] = info.Xmdh;
            //项目结果
            dtResult.Rows[dtResult.Rows.Count - 1]["result"] = info.Result;
            dtResult.Rows[dtResult.Rows.Count - 1]["result1"] = info.Result1;
            //单位
            dtResult.Rows[dtResult.Rows.Count - 1]["dw"] = info.Dw;
        }
    }
}
