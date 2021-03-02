using System;
using System.Collections.Generic;
using System.Text;

namespace TJClient.Common
{
    public class CommomSysInfo
    {
        /// <summary>
        /// 页面项目是否进行了编辑 0：未编辑 1:编辑过 
        /// </summary>
        public static string IsEdited { get; set; }

        /// <summary>
        /// 体检类型  0:登记，1:健康体检表，2:中医体质辨识，3:老年人生活自理能力评估
        /// </summary>
        public static string TJTYPE { get; set; }

        #region 健康体检

        /// <summary>
        /// 是否启用辅助录入功能 0：未开启 1:开启 
        /// </summary>
        public static string IsFzlr { get; set; }

        /// <summary>
        /// 开启腰围换算 0：未开启 1:开启 
        /// </summary>
        public static string IsYwhs { get; set; }

        /// <summary>
        /// 开启提示上次体检结果 0：未开启 1:开启 
        /// </summary>
        public static string IsSctjjg { get; set; }

        /// <summary>
        /// 开启提示上次体检结果的用药情况 0：未开启 1:开启 
        /// </summary>
        public static string IsSctjjg_yyqk { get; set; }

         /// <summary>
        /// 体检负责人编码 
        /// </summary>
        public static string TJFZR_BM { get; set; }

         /// <summary>
        /// 体检负责人名称 
        /// </summary>
        public static string TJFZR_MC { get; set; }

        /// <summary>
        ///  体检日期 
        /// </summary>
        public static string tjsj { get; set; }

        /// <summary>
        ///  体检状态
        /// </summary>
        public static string tjzt { get; set; }

        #endregion

    }
}
