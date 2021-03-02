using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Common;
using TJClient.Signname.Model;

namespace TJClient.Signname
{
    public class Operation
    {
        private static List<UserSignname> _signnames = null;

        public static Dictionary<string, int> DataGridViewTables = null;

        public static bool AddDataGridViewTableToDict(string table,int count)
        {
            if (DataGridViewTables == null)
            {
                DataGridViewTables = new Dictionary<string, int>();
            }
            else
            {
                DataGridViewTables.Clear();
            }
            DataGridViewTables.Add(table,count);
            return true;
        }

        private static void SetSignnames(DataTable dt)
        {
            if (_signnames == null)
            {
                _signnames = new List<UserSignname>();
            }
            _signnames.Clear();
            foreach (DataRow row in dt.Rows)
            {
                _signnames.Add(new UserSignname(row["signnametitle"].ToString(), row["signnamepicpath"].ToString(), row["realname"].ToString()));
            }
        }

        private static List<string> SignnameGroupList(DataTable dt)
        {
            List<string> result = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string signnameGroup = row["SIGNNAMEGROUP"].ToString();
                if (!String.IsNullOrEmpty(signnameGroup) && !result.Contains(signnameGroup))
                {
                    result.Add(signnameGroup);
                }
            }
            return result;
        }

        public static List<string> GetAllRealname(string yljgbm)
        {
            DataTable dt = DataOperation.Init().GetAllRealname(yljgbm);
            List<string> result = new List<string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(row["realname"].ToString());
                }
            }

            return result;
        }

        /// <summary>
        /// 按操作员获取签名
        /// </summary>
        /// <param name="czy">操作员</param>
        /// <returns></returns>
        public static List<UserSignname> UserSignnames(string czy)
        {
            DataTable dt = DataOperation.Init().GetSignname(czy);
            SetSignnames(dt);
            return _signnames;
        }

        /// <summary>
        /// 按签名标题获取签名图片的路径
        /// </summary>
        /// <param name="signnameTitle"></param>
        /// <returns></returns>
        public static string SignnamePicPath(string signnameTitle)
        {
            if (_signnames == null || _signnames.Count <= 0 || signnameTitle=="")
            {
                return "";
            }
            UserSignname signname = _signnames.Find(x => x.SignnameTitle == signnameTitle);
            if (signname == null)
            {
                return "";
            }
            return signname.SignnamePicPath;
        }

        /// <summary>
        /// 按健康档案号与体检时间获取签名图片路径，从数据库获取。
        /// </summary>
        /// <param name="jktjSignname">健康体检签名信息</param>
        /// <returns></returns>
        public static string SignnamePicPath(JktjSignname jktjSignname , out string realname)
        {
            realname="";
            DataTable dt = DataOperation.Init().GetJktjSignnameShow(jktjSignname);
            if (dt.Rows.Count <= 0)
            {
                return "";
            }
            realname = dt.Rows[0]["realname"].ToString();
            return dt.Rows[0]["signnamepicpath"].ToString();
        }

        /// <summary>
        /// 获取本人签字，和家属签字
        /// </summary>
        /// <param name="jktjSignname"></param>
        /// <returns></returns>
        public static Dictionary<string, string> FkSignnamePicPath(JktjSignname jktjSignname, out DataTable dt_Signname)
        {
            dt_Signname = null;
            DataTable dt=DataOperation.Init().GetFkSignnameShow(jktjSignname);
            Dictionary<string,string> result = new Dictionary<string,string>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(row["signnamegroup"].ToString(), row["signnamepicpath"].ToString());
            }
            dt_Signname = dt;
            return result;
        }

        /// <summary>
        /// 对于特殊的签名组要进行判断，是否设置了数据，如果不设置数据，就不签名。
        /// </summary>
        /// <param name="signnameGroup"></param>
        /// <returns>true为签名，false不签名</returns>
        private static bool CheckDataGridViewSignnameGroupRowsThanZero(string signnameGroup)
        {
            if (DataGridViewTables == null)
            {
                return true;
            }
            Dictionary<string, string> specialSignnameGroups = new Dictionary<string, string>()
            {
                {"ZYYYQKQMZ","t_tj_yyqkb"},{"ZYZLQKQMZ","t_tj_zyzlqkb"},{"FMYGHYFJZSQMZ","t_tj_fmyghyfb"}
            };
            if (specialSignnameGroups.Keys.Contains(signnameGroup))
            {
                var curr = specialSignnameGroups.First(x => x.Key == signnameGroup);
                var table = DataGridViewTables.FirstOrDefault(x => x.Key.ToLower() == curr.Value);
                if (table.Value <= 0)
                {
                    return false;
                }
            }
            return true;
        }


        private static void AddBaseInfoToJktjSignname(ref JktjSignname jktjSignname)
        {
            jktjSignname.UserSignnameId = 0;
            jktjSignname.CreateUser = UserInfo.userId;
            jktjSignname.CreateDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            jktjSignname.UpdateUser = UserInfo.userId;
            jktjSignname.UpdateDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 直接保存健康体检签名信息。
        /// </summary>
        /// <param name="jktjSignname"></param>
        public static void SaveJktjSignname(JktjSignname jktjSignname)
        {
            List<JktjSignname> jktjSignnames = new List<JktjSignname>();
            AddBaseInfoToJktjSignname(ref jktjSignname);
            if (string.IsNullOrEmpty(jktjSignname.SignnamePicPath)==false ){
            jktjSignname.SignnamePicPath = Common.GetPicRelativePath(jktjSignname.SignnamePicPath);
            }
            if (string.IsNullOrEmpty(jktjSignname.Realname) == true && string.IsNullOrEmpty(jktjSignname.SignnamePicPath) == true)
            {
                throw new Exception("没有设定签名!");
            }
            jktjSignnames.Add(jktjSignname);
            DataOperation.Init().SaveJktjSignname(jktjSignnames);
        }


        public static bool DeleteJktjSignname(string grdabh,List<string> signnameGroup)
        {
            return DataOperation.Init().DeleteSignname(grdabh,signnameGroup);
        }

        /// <summary>
        /// 保存健康体检时的签名
        /// </summary>
        /// <param name="tjxmDt">体检项目的DataTable,用于获取签名组</param>
        /// <param name="jktjSignname"></param>
        public static void SaveJktjSignname(DataTable tjxmDt,JktjSignname jktjSignname)
        {
            List<JktjSignname> jktjSignnames = new List<JktjSignname>();
            List<string> signnameGroupList = SignnameGroupList(tjxmDt);
            foreach (string signnameGroup in signnameGroupList)
            {
                if (!CheckDataGridViewSignnameGroupRowsThanZero(signnameGroup))
                {
                    continue;
                }
                JktjSignname jktjSignname_tem = new JktjSignname()
                {
                    Czy = jktjSignname.Czy ,
                    Yljgbm = jktjSignname.Yljgbm,
                    Tjsj =jktjSignname.Tjsj,
                    D_Grdabh = jktjSignname.D_Grdabh,
                    Realname = jktjSignname.Realname ,
                    //SignnamePicPath = jktjSignname.SignnamePicPath,
                };
                AddBaseInfoToJktjSignname(ref jktjSignname_tem);
                if (!string.IsNullOrEmpty(jktjSignname.SignnamePicPath))
                {
                    jktjSignname_tem.SignnamePicPath = Common.GetPicRelativePath(jktjSignname.SignnamePicPath);
                }
                jktjSignname_tem.SignnameGroup = signnameGroup;
                jktjSignnames.Add(jktjSignname_tem);
            }
            DataOperation.Init().SaveJktjSignname(jktjSignnames);
        }
        /// <summary>
        /// 获取手写板签名图片列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>签名图片列表，包含的是绝对路径</returns>
        public static List<string> GetTabletSignnamePics(DataTable dt)
        {
            string noTabletSignnamePic = "default";
            List<string> result = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string signnamePicPath = row["SIGNNAMEPICPATH"].ToString();
                if (!signnamePicPath.ToLower().Contains(noTabletSignnamePic))
                {
                    result.Add(Path.Combine(Common.GetTabletSignnameDirectory(), signnamePicPath));
                }
            }
            return result;
        }
        /// <summary>
        /// 获取手写板签名图片路径
        /// </summary>
        /// <returns>绝对路径</returns>
        public static string GetTabletSignnamePicPath()
        {
            string settingDirPath = Common.GetTabletSignnameDirectory();
            DirectoryHelper.CreateDirectory(settingDirPath);
            string saveSignnamePicDirPath = Path.Combine(settingDirPath, DirectoryHelper.GetDirectory());
            DirectoryHelper.CreateDirectory(saveSignnamePicDirPath);
            return Path.Combine(saveSignnamePicDirPath, FilenameHelper.SignnamePicFileName());
        }

        private static string DefaultSignnamePicsDirPath()
        {
            string path = Path.Combine(Common.GetCurrRunExeDir(), "unzipfiles");
            DirectoryHelper.CreateDirectory(path);
            return path;
        }

        private static string DefaultSignnamePicsUnzipFilePath()
        {
            string dir = DefaultSignnamePicsDirPath();
            string unzipDir = DirectoryHelper.GetRandomDirectory();
            string path = Path.Combine(dir, unzipDir);
            DirectoryHelper.CreateDirectory(path);
            return path;
        }
        /// <summary>
        /// 默认签名图片的压缩文件解压缩
        /// </summary>
        /// <param name="zipfilePath">压缩文件路径</param>
        /// <returns></returns>
        public static bool DefaultSignnamePicsUnzip(string zipfilePath)
        {
            string unzipDirPath = DefaultSignnamePicsUnzipFilePath();
            CompressHelper.UnZip(zipfilePath, unzipDirPath);
            List<string> files = DirectoryHelper.GetAllFiles(unzipDirPath);
            foreach (string file in files)
            {
                FileHelper.CopyTo(file, Common.DefaultSignnamePicsDirPath());
            }
            DirectoryHelper.DeleteDirectory(unzipDirPath);
            return true;
        }
        /// <summary>
        /// 对体检人员信息初始化，用于体检人员签名
        /// </summary>
        /// <param name="dtRow"></param>
        public static void HealthExaminedUserInfoInit(DataRow dtRow)
        {
            HealthExaminedUserInfo.Name = dtRow["D_XM"].ToString();
            HealthExaminedUserInfo.DGrdabh = dtRow["D_GRDABH"].ToString();
            HealthExaminedUserInfo.CardNo = dtRow["D_SFZH"].ToString();
        }

        public static void HealthExaminedUserInfoInit(string name, string dGrdabh, string dSfzh,string createDate)
        {
            HealthExaminedUserInfo.Name = name;
            HealthExaminedUserInfo.DGrdabh = dGrdabh;
            HealthExaminedUserInfo.CardNo = dSfzh;
            HealthExaminedUserInfo.CreateDate=createDate;
        }


    }
}
