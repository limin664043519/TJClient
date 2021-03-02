using System;
using System.Collections.Generic;
using System.Text;

namespace TJClient.Common
{
    public class UserInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public static string Username { get; set; }

        /// <summary>
        /// 操作员名称
        /// </summary>
        public static string userId { get; set; }

        /// <summary>
        /// 医疗机构编码
        /// </summary>
        public static string Yybm { get; set; }

        /// <summary>
        /// 医疗机构名称
        /// </summary>
        public static string Yymc { get; set; }

        /// <summary>
        /// 工作组
        /// </summary>
        public static string gzz { get; set; }

        /// <summary>
        /// 社保机构编码
        /// </summary>
        public static string Yybm_sb { get; set; }

        /// <summary>
        /// 用户登陆模式 0:否  1：是
        /// </summary>
        public static string logInMode { get; set; }
        
    }
}
