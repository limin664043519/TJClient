using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using TJClient.Common;
using TJClient.Signname.Model;

namespace FBYClient
{
    public partial class sysAutoForm : Form
    {
        /// <summary>
        /// 工作组的项目
        /// </summary>
        public DataTable dtResult = null;
        /// <summary>
        /// 操作员的默认签名
        /// </summary>
        public DataTable dtSignname = null;

        /// <summary>
        /// 签名的信息
        /// </summary>
        JktjSignname jktjSignname = new TJClient.Signname.Model.JktjSignname()
                {
                    Czy = UserInfo.userId,
                    Yljgbm = UserInfo.Yybm,
                    Tjsj = "",
                    D_Grdabh = "",
                    SignnamePicPath = "",
                };

        public sysAutoForm()
        {
            InitializeComponent();

            //初始化工作组内容
         
            
        }

        /// <summary>
        /// 设定仪器的型号
        /// </summary>
        /// <param name="yqxh"></param>
        /// <returns></returns>
        public virtual bool setStart(string yqxhPara)
        {
            
            return true;
        }

        #region 签名

        public string ChangeCharacterSignnameRandom()
        {
            if (TJClient.Signname.Common.ShowSignnameOperation())
            {
                if (dtSignname.Rows.Count > 0)
                {
                    return dtSignname.Rows[0]["realname"] != null ? dtSignname.Rows[0]["realname"].ToString() : "";
                }
            }
            return "";
        }

        /// <summary>
        /// 产生随机的签名
        /// </summary>
        public string ChangeSignnamePicRandom()
        {
            if (TJClient.Signname.Common.ShowSignnameOperation())
            {
                int count = dtSignname != null ? dtSignname.Rows.Count : 0;
                if (count == 1)
                {
                    return dtSignname.Rows[0]["signnamepicpath"] != null ? dtSignname.Rows[0]["signnamepicpath"].ToString() : "";
                }
                else if (count > 1)
                {
                    int cboSignnameSelectedIndex = TJClient.Signname.Common.GetRandomInRange(count);
                    return dtSignname.Rows[cboSignnameSelectedIndex]["signnamepicpath"] != null ? dtSignname.Rows[cboSignnameSelectedIndex]["signnamepicpath"].ToString() : ""; ;
                }
            }
            return "";
        }

        /// <summary>
        /// 默认签名
        /// </summary>
        public void SaveJktjSignname(string tjsj, string jkdah)
        {
            //先判断是否启用签名
            if (TJClient.Signname.Common.ShowSignnameOperation())
            {
                if (dtResult == null) //判断是否存在签名组
                {
                    return;
                }

                string realName = ChangeCharacterSignnameRandom();
                if (realName.Length > 0)
                {
                    jktjSignname.Tjsj = tjsj;
                    jktjSignname.D_Grdabh = jkdah;
                    jktjSignname.SignnamePicPath = "";
                    jktjSignname.Realname = realName;
                    TJClient.Signname.Operation.SaveJktjSignname(dtResult, jktjSignname);
                }
            }
        }


        /// <summary>
        /// 获取工作组的相关信息
        /// </summary>
        public void SignNameGroupInit()
        {
            //获取该工作组对应的体检项目
            string sql = @"SELECT T_JK_TJXM.SIGNNAMEGROUP,T_JK_GZZ_XM.YLJGBM, T_JK_GZZ_XM.GZZBM, T_JK_TJXM.XMFLBM, T_JK_TJXM.XMBM, T_JK_TJXM.XMMC, T_JK_TJXM.KJXSMC, T_JK_TJXM.KJLX, T_JK_TJXM.SJZDBM, T_JK_TJXM.KJID, T_JK_TJXM.KJKD, T_JK_TJXM.KJGD, T_JK_TJXM.KJMRZ, T_JK_TJXM.JKDA_DB, T_JK_TJXM.HIS_DB, T_JK_TJXM.SL, T_JK_TJXM.DJ, T_JK_TJXM.parentxm, T_JK_TJXM.parentxmvalue, T_JK_TJXM.maxcount, T_JK_TJXM.dw, T_JK_XMFL.XMFLMC,T_JK_TJXM.rowNo,T_JK_TJXM.jj,T_JK_TJXM.valueHeigh,T_JK_TJXM.valueLower,T_JK_TJXM.isNotNull,T_JK_TJXM.fzlritem
                           FROM (T_JK_GZZ_XM INNER JOIN T_JK_TJXM ON T_JK_GZZ_XM.XMBM = T_JK_TJXM.XMBM) INNER JOIN T_JK_XMFL ON T_JK_TJXM.XMFLBM = T_JK_XMFL.XMFLBM
                           WHERE (((T_JK_GZZ_XM.YLJGBM)='{YLJGBM}') AND ((T_JK_GZZ_XM.GZZBM)='{GZZBM}')) order by T_JK_TJXM.XMFLBM, T_JK_TJXM.rowNo,T_JK_TJXM.ORDERBY ";

            //从数据库中取值
            DBAccess dBAccess = new DBAccess();
             dtResult = dBAccess.ExecuteQueryBySql(sql.Replace("{YLJGBM}", UserInfo.Yybm).Replace("{GZZBM}", UserInfo.gzz));

            //初始化签名列表
            TJClient.Signname.DataOperation dataoperation = new TJClient.Signname.DataOperation();
            dtSignname = dataoperation.GetSignname(UserInfo.userId);
        }

        #endregion

    }
}
