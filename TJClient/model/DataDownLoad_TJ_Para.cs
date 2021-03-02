using System;
using System.Collections.Generic;
using System.Text;

namespace TJClient.Common
{
    public class DataDownLoad_TJ_Para
    {
        ///// <summary>
        ///// 健康体检
        ///// </summary>
        //public static string jkdaxx { get; set; }

        ///// <summary>
        ///// 上次体检信息
        ///// </summary>
        //public static string sctjxx { get; set; }

        /// <summary>
        /// 村庄
        /// </summary>
        public static string czList { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public static string czy{ get; set; }

        /// <summary>
        /// 医疗机构编码
        /// </summary>
        public static string yljgbm { get; set; }

        /// <summary>
        /// 区分 1:下载导入 2:导入
        /// </summary>
        public static string qf { get; set; }

        /// <summary>
        /// 数据分类
        /// </summary>
        public static string fileType { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public static string filePath_da { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public static string filePath_tjxx { get; set; }

    }
}
