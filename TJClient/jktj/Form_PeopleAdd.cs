using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.Common;
using TJClient.sys.Bll;
using TJClient.sys;
using FBYClient;

namespace TJClient.jktj
{
    public partial class Form_PeopleAdd : Form
    {
        public Form_PeopleAdd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 增加人员的类型 0:体检是增加  1:登记时增加
        /// </summary>
        public string peopleAddType = "0";

        /// <summary>
        /// 体检编号
        /// </summary>
        public string tjbh = "";

        /// <summary>
        /// 村庄编码
        /// </summary>
        public string czbm = "";

        /// <summary>
        /// 健康档案号
        /// </summary>
        public string jkdah = "";

        /// <summary>
        /// 身份证号
        /// </summary>
        public string sfzh = "";

        /// <summary>
        /// 出生日期
        /// </summary>
        public string csrq = "";

        /// <summary>
        /// 姓名
        /// </summary>
        public string xm = "";

        /// <summary>
        /// 性别
        /// </summary>
        public string xb = "";

        /// <summary>
        /// 性别名称
        /// </summary>
        public string xb_mc = "";

        private bool drpflag = false;

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_PeopleAdd_Load(object sender, EventArgs e)
        {
           
            //条码号
            textBox_tjhm.Text = tjbh;

            //健康档案号
            if (this.jkdah.Length == 0)
            {
                if (textBox_tjhm.Text.Length >= 12)
                {
                    this.jkdah = textBox_tjhm.Text.Substring(0, 12);
                }
                else
                {
                    this.jkdah = DateTime.Now.ToString("MMddHHmmssff");
                }
            }
            textBox_jkdah.Text = this.jkdah ;

            //身份证号
            textBox_sfzh.Text = this.sfzh;

            //姓名
            textBox_xm.Text = xm;

            //年龄
            if (this.sfzh.Length > 0)
            {
                //验证身份证号码
                if (TJClient.Common.Common.CheckIDCard(this.sfzh) == true)
                {
                    string[] GetCardIdInfo = TJClient.Common.Common.GetCardIdInfo(this.sfzh.Trim().ToLower());
                    string birth = GetCardIdInfo[1];
                    //string birth = TJClient.Common.Common.GetBirthdayByIdentityCardId(this.sfzh, true);
                    string nl = (DateTime.Now.Year - Convert.ToDateTime(birth).Year).ToString();
                    textBox_nl.Text = nl.ToString();

                    //性别
                    xb = GetCardIdInfo[3];
                }
                else
                {
                    MessageBox.Show("身份证不合法！");
                }
            }

            comboBox_head_XB.DataSource = getSjzdList("xb_xingbie");
            comboBox_head_XB.DisplayMember = "ZDMC";
            comboBox_head_XB.ValueMember = "ZDBM";

            if (xb.Length > 0)
            {
                comboBox_head_XB.SelectedValue = xb;
            }

            //所属机构
            setDrp(comboBox_ssjg, "", "sql085", true);
            string defaut_prgid = System.Configuration.ConfigurationManager.AppSettings["defaut_prgid"];
            if (defaut_prgid.Length > 0)
            {
                comboBox_ssjg.SelectedValue = defaut_prgid;
            }
            //sql_T_JK_systemConfig_select
            //数据存贮
            setDrp(comboBox_sjcc, "", "sql_T_JK_systemConfig_select", false);


            //省
            setDrp(comboBox_D_SHENG, "", "sql051", false);
            string defaut_sheng = System.Configuration.ConfigurationManager.AppSettings["defaut_sheng"];
            if (defaut_sheng.Length > 0)
            {
                comboBox_D_SHENG.SelectedValue = defaut_sheng;
            }
            //市
            setDrp(comboBox_D_SHI, "", "sql052", false);
            string defaut_shi = System.Configuration.ConfigurationManager.AppSettings["defaut_shi"];
            if (defaut_shi.Length > 0)
            {
                comboBox_D_SHI.SelectedValue = defaut_shi;
            }
            //县
            setDrp(comboBox_D_QU, "", "sql053", false);
            string defaut_xian = System.Configuration.ConfigurationManager.AppSettings["defaut_xian"];
            if (defaut_xian.Length > 0)
            {
                comboBox_D_QU.SelectedValue = defaut_xian;
            }
            //街道
            setDrp(comboBox_D_JD, "", "sql054", false);
            string defaut_jiedao = System.Configuration.ConfigurationManager.AppSettings["defaut_jiedao"];
            if (defaut_jiedao.Length > 0)
            {
                comboBox_D_JD.SelectedValue = defaut_jiedao;
            }
            //居委会
            setDrp(comboBox_D_JWH, "", "sql055", false);
            string defaut_juweihui = System.Configuration.ConfigurationManager.AppSettings["defaut_juweihui"];
            if (defaut_juweihui.Length > 0)
            {
                comboBox_D_JWH.SelectedValue = defaut_juweihui;
            }


        }

        /// <summary>
        /// 取得数据字典中的项目
        /// </summary>
        /// <param name="Sjzdbm"></param>
        /// <returns></returns>
        private DataTable getSjzdList(string Sjzdbm)
        {
            //获取该工作组对应的体检项目
            string sql = @"select * from  T_JK_SJZD where ZDLXBM='{ZDLXBM}' order by id ";

            DataTable checkList = new DataTable();
            DBAccess dBAccess = new DBAccess();
            checkList = dBAccess.ExecuteQueryBySql(sql.Replace("{ZDLXBM}", Sjzdbm));
            if (checkList != null && checkList.Rows.Count > 0)
            {
                return checkList;
            }
            return null;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
           string message="";
           xb_mc = comboBox_head_XB.Text ;
           xb = comboBox_head_XB.SelectedValue != null ? comboBox_head_XB.SelectedValue.ToString() : "";

           string comboBox_ssjg_str = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";
           string comboBox_sjcc_str = comboBox_sjcc.SelectedValue != null ? comboBox_sjcc.SelectedValue.ToString() : "";


           bool result= false;
            //0:体检时增加  1：登记时增加
           if (this.peopleAddType.Equals("0"))
           {
               if (comboBox_ssjg.SelectedValue == null || comboBox_ssjg.SelectedValue.ToString().Length == 0)
               {
                   MessageBox.Show("[所属机构] 必须选择！");
                   return;
               }
               result = peopleAdd(out message);
           }
           else
           {
               if (comboBox_ssjg.SelectedValue == null || comboBox_ssjg.SelectedValue.ToString().Length == 0)
               {
                   MessageBox.Show("[所属机构] 必须选择！");
                   return;
               }

               if (comboBox_D_JWH.SelectedValue == null || comboBox_D_JWH.SelectedValue.ToString().Length == 0)
               {
                   MessageBox.Show("[居/村委会] 必须选择！");
                   return;
               }

               result = createRkzlx(out message);
           }

           if (result == true)
           {
               sysCommonForm syscommonform = (sysCommonForm)this.Owner;
               syscommonform.setParentFormDo("");
               this.Close();
           }
           else
           {
               MessageBox.Show("保存失败！" + message);
           }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    sysCommonForm syscommonform = (sysCommonForm)this.Owner;
        //    syscommonform.setParentFormDo("");
        //    this.Close();
        //}

        #region  体检时增加不存在的人员进行体检

        public bool peopleAdd(out string message)
        {
            message = "";
            message = "";
            try
            {
                jktjBll jktjbll = new jktjBll();
                DataTable dt_tjry_add = new DataTable();
                dt_tjry_add.Columns.Add("YLJGBM");//医疗机构编码
                dt_tjry_add.Columns.Add("TJJHBM");//体检计划编码
                dt_tjry_add.Columns.Add("TJPCH");//体检批次号
                dt_tjry_add.Columns.Add("SFH");//顺番号
                dt_tjry_add.Columns.Add("SXHM");//顺序号码
                dt_tjry_add.Columns.Add("TJBM");//个人体检编号
                dt_tjry_add.Columns.Add("JKDAH");//个人健康档案号
                dt_tjry_add.Columns.Add("XM");//姓名
                dt_tjry_add.Columns.Add("XB");//性别
                dt_tjry_add.Columns.Add("SFZH");//身份证号
                dt_tjry_add.Columns.Add("LXDH");//联系电话
                dt_tjry_add.Columns.Add("LXrDH");//联系人电话
                dt_tjry_add.Columns.Add("CSRQ");//出生日期
                dt_tjry_add.Columns.Add("CZBM");//村庄编码
                dt_tjry_add.Columns.Add("FL");//体检人员分类
                dt_tjry_add.Columns.Add("BZ");//备注
                dt_tjry_add.Columns.Add("TJBH_TEM");//临时个人体检编号
                dt_tjry_add.Columns.Add("CREATETIME");//创建时间
                dt_tjry_add.Columns.Add("CREATEUSER");//创建者
                dt_tjry_add.Columns.Add("UPDATETIME");//更新时间
                dt_tjry_add.Columns.Add("UPDATEUSER");//更新者
                dt_tjry_add.Columns.Add("SCZT");//数据上传状态
                dt_tjry_add.Columns.Add("ZLBZ");//增量标志
                dt_tjry_add.Columns.Add("nd");//年度
                dt_tjry_add.Columns.Add("ISSH");//是否审核
                dt_tjry_add.Columns.Add("ISNEWDOC");//是否新建档案
                dt_tjry_add.Columns.Add("prgid");//是否新建档案
                dt_tjry_add.Rows.Add();

                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJJHBM"] = DateTime.Now.ToString("yyyyMMdd");//体检计划编码
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJPCH"] = DateTime.Now.ToString("HHmmss");//体检批次号

                //取得顺番号
                DataTable dt_SFH = jktjbll.GetMoHuList("", "sql076");
                if (dt_SFH != null && dt_SFH.Rows.Count > 0 && dt_SFH.Rows[0]["SFH"] != null && dt_SFH.Rows[0]["SFH"].ToString().Length > 0)
                {
                    dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SFH"] = int.Parse(dt_SFH.Rows[0]["SFH"].ToString()) + 1;//顺番号
                }
                else
                {
                    dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SFH"] = "0";//顺番号
                }

                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SXHM"] = "0";//顺序号码
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJBM"] = textBox_jkdah.Text.Trim();//个人体检编号
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["JKDAH"] = textBox_jkdah.Text.Trim();//个人健康档案号
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["XM"] = textBox_xm.Text.Trim();//姓名
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["XB"] = "";//性别
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SFZH"] = textBox_sfzh.Text;//身份证号
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["LXDH"] = textBox_lxdh.Text;//联系电话
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["LXrDH"] = textBox_lxrdh.Text;//联系人电话
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CSRQ"] = "";//出生日期
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CZBM"] = czbm;//村庄编码
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["FL"] = "2";//体检人员分类
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["BZ"] = "";//备注
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["TJBH_TEM"] = textBox_jkdah.Text.Trim();//临时个人体检编号
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CREATETIME"] = DateTime.Now.ToString();//创建时间
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["CREATEUSER"] = UserInfo.userId;//创建者
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["UPDATETIME"] = DateTime.Now.ToString();//更新时间
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["UPDATEUSER"] = UserInfo.userId;//更新者
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["SCZT"] = "2";//数据上传状态
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["ZLBZ"] = "0";//增量标志
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString();//年度
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["ISSH"] = "0";//登记
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["ISNEWDOC"] = "0";//新建档案
                dt_tjry_add.Rows[dt_tjry_add.Rows.Count - 1]["prgid"] = comboBox_ssjg.SelectedValue.ToString();//所属机构编码

                //增加体检人员信息
                jktjbll.Add(dt_tjry_add, "sql_add_people");

                //体检人员条形码对应表(T_JK_TJRY_TXM）
                DataTable t_jk_tjry_txm = new DataTable();
                t_jk_tjry_txm.Columns.Add("YLJGBM");//医疗机构编码
                t_jk_tjry_txm.Columns.Add("TXMBH");//条形码号码
                t_jk_tjry_txm.Columns.Add("TMBM");//条码分类
                t_jk_tjry_txm.Columns.Add("JKDAH");//个人健康档案号
                t_jk_tjry_txm.Columns.Add("SFZH");//身份证号
                t_jk_tjry_txm.Columns.Add("BZ");//备注
                t_jk_tjry_txm.Columns.Add("ZLBZ");//增量标志
                t_jk_tjry_txm.Columns.Add("nd");//年度
                t_jk_tjry_txm.Rows.Add();

                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["TXMBH"] = textBox_tjhm.Text.Trim();//条形码号码
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["TMBM"] = textBox_tjhm.Text.Length > 2 ? textBox_tjhm.Text.Substring(textBox_tjhm.Text.Length - 2, 2) : "";//条码分类
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["JKDAH"] = textBox_jkdah.Text.Trim();//个人健康档案号
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["SFZH"] = textBox_sfzh.Text;//身份证号
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["BZ"] = "**";//备注
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["ZLBZ"] = "0";//增量标志
                t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString();//年度

                //增加体检人员条形码对应关系
                jktjbll.Add(t_jk_tjry_txm, "sql_add_people_txm");
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            return true;
        }
        #endregion

        #region 创建建议档案处理

        /// <summary>
        /// 创建一份简易的档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool createRkzlx( out string ErrMsg)
        {
            try
            {
                ErrMsg = "";
                //所属机构
                string comboBox_ssjg_str = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";
                //数据存储
                string comboBox_sjcc_str = comboBox_sjcc.SelectedValue != null ? comboBox_sjcc.SelectedValue.ToString() : "";

                string sqlwhere =" and 1=2 ";

                //创建人口资料学表结构
                DataTable dt_T_DA_JKDA_RKXZL = initTable("sql056", sqlwhere);
                dt_T_DA_JKDA_RKXZL.Rows.Add();

                //健康档案号  健康档案人口学资料（T_DA_JKDA_RKXZL） 按照时间自动生成
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_GRDABH"] = textBox_jkdah.Text;

                //档案状态 健康档案人口学资料（T_DA_JKDA_RKXZL）
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_DAZT"] = "1";

                //姓名 健康档案人口学资料（T_DA_JKDA_RKXZL）
                string name_bh = "";
                if (textBox_xm.Text.Trim().Length != 0)
                {
                    name_bh = textBox_xm.Text;
                }
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_XM"] = name_bh;

                //性别 健康档案人口学资料（T_DA_JKDA_RKXZL）
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_XB"] =xb_mc.Length >0?( this.xb_mc.ToString().Equals("男") ? "1" : "2"):"";

                //户主身份证号 与本人身份证号一致
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_SFZH"] = this.sfzh;

                dt_T_DA_JKDA_RKXZL.Rows[0]["D_zjhqt"] = this.sfzh;
                //联系电话
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_LXDH"] = textBox_lxdh.Text;

                //联系人电话
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_LXrDH"] = textBox_lxrdh.Text;

                //出生日期    健康档案人口学资料（T_DA_JKDA_RKXZL）
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_CSRQ"] = this.csrq;

                //标识该条数据是否进行过修改。1：未修改  2：已修改  3：新增
                dt_T_DA_JKDA_RKXZL.Rows[0]["zt"] = "3";

                //增量标
                dt_T_DA_JKDA_RKXZL.Rows[0]["zlbz"] = "1";

                //创建者姓名
                dt_T_DA_JKDA_RKXZL.Rows[0]["CREATEUSER"] = UserInfo.userId;

                //创建时间
                dt_T_DA_JKDA_RKXZL.Rows[0]["CREATETIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //创建者姓名
                dt_T_DA_JKDA_RKXZL.Rows[0]["updateUSER"] = UserInfo.userId;

                //创建时间
                dt_T_DA_JKDA_RKXZL.Rows[0]["updateTIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Form_daxqBll form_daxqbll = new Form_daxqBll();

                //年度
                dt_T_DA_JKDA_RKXZL.Rows[0]["nd"] = DateTime.Now.Year.ToString();

                //修改创建时间
                dt_T_DA_JKDA_RKXZL.Rows[0]["HAPPENTIME"] = DateTime.Now.ToString("yyyy-MM-dd");

                //修改创建时间
                dt_T_DA_JKDA_RKXZL.Rows[0]["isnewdoc"] = "1";

                //所属机构
                dt_T_DA_JKDA_RKXZL.Rows[0]["P_RGID"] = comboBox_ssjg_str;

                //数据存储
                dt_T_DA_JKDA_RKXZL.Rows[0]["DATAFROM"] = comboBox_sjcc_str;

                //居住地址
                //省   健康档案人口学资料（T_DA_JKDA_RKXZL）
                if (comboBox_D_SHENG.SelectedValue != null)
                {
                    dt_T_DA_JKDA_RKXZL.Rows[0]["D_SHENG"] = comboBox_D_SHENG.SelectedValue.ToString();
                }

                //市   健康档案人口学资料（T_DA_JKDA_RKXZL）
                if (comboBox_D_SHI.SelectedValue != null)
                {
                    dt_T_DA_JKDA_RKXZL.Rows[0]["D_SHI"] = comboBox_D_SHI.SelectedValue.ToString();
                }

                //县   健康档案人口学资料（T_DA_JKDA_RKXZL）
                if (comboBox_D_QU.SelectedValue != null)
                {
                    dt_T_DA_JKDA_RKXZL.Rows[0]["D_QU"] = comboBox_D_QU.SelectedValue.ToString();
                }
                //镇   健康档案人口学资料（T_DA_JKDA_RKXZL）
                if (comboBox_D_JD.SelectedValue != null)
                {
                    dt_T_DA_JKDA_RKXZL.Rows[0]["D_JD"] = comboBox_D_JD.SelectedValue.ToString();
                }
                //村   健康档案人口学资料（T_DA_JKDA_RKXZL）
                if (comboBox_D_JWH.SelectedValue != null)
                {
                    dt_T_DA_JKDA_RKXZL.Rows[0]["D_JWH"] = comboBox_D_JWH.SelectedValue.ToString();
                }

                //详细地址   健康档案人口学资料（T_DA_JKDA_RKXZL）
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_XXDZ"] = lTextBox_D_XXDZ.Text;




                //年龄
                //if (TJClient.Common.Common.CheckIDCard(this.sfzh) == true)
                //{
                //    string birth = TJClient.Common.Common.GetBirthdayByIdentityCardId(this.sfzh, true);
                //    string nl = (DateTime.Now.Year - Convert.ToDateTime(birth).Year ).ToString();
                //    dt_T_DA_JKDA_RKXZL.Rows[0]["NL"] = nl.ToString();
                //}
                dt_T_DA_JKDA_RKXZL.Rows[0]["NL"] = textBox_nl.Text;

                dt_T_DA_JKDA_RKXZL.Rows[0]["IFUPLOADE"] = "1";

                //与户主关系
                dt_T_DA_JKDA_RKXZL.Rows[0]["D_YHZGX"] = "1";
                

                //新增
                form_daxqbll.Add(dt_T_DA_JKDA_RKXZL, "sql067");

                //加入体检
                string errMsg = "";
                string result = Add_jktj(textBox_jkdah.Text,textBox_sfzh .Text ,textBox_xm .Text ,xb_mc ,csrq ,textBox_nl.Text,comboBox_ssjg_str,comboBox_sjcc_str,textBox_lxdh.Text, ref errMsg);
                if (result.Equals("1") == false)
                {
                    ErrMsg = errMsg;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 初始化表结构
        /// </summary>
        /// <param name="dt_table"></param>
        /// <returns></returns>
        public DataTable initTable(string sqlcode,string sqlwhere)
        {
            DataTable dt = new DataTable();
            Form_daxqBll form_daxq = new Form_daxqBll();
            //获取数据库表结构
            dt = form_daxq.GetMoHuList(sqlwhere, sqlcode);
            //dt = form_daxq.GetMoHuList("and 1=2", sqlcode);
            return dt;
        }

        /// <summary>
        /// 生成体检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string Add_jktj(string grdabh,string sfzh_para,string xm_para,string xb_para,string csrq_para,string nl,string p_rgid ,string datafrom ,string lxdh,ref string errMsg)
        {
            Form_daxqBll form_daxqbll = new Form_daxqBll();
            string strtmbm = grdabh;
            try
            {
                //生成体检人员信息
                DataTable dt_tjryxx = new DataTable();
                dt_tjryxx = form_daxqbll.GetMoHuList(string.Format(" and JKDAH='{0}' ", grdabh), "sql_select_TJRYXX");

                //已经存在
                if (dt_tjryxx != null && dt_tjryxx.Rows.Count > 0)
                {
                    //判断条码信息是否存在  
                    DataTable dt_tjry_txm_tem = form_daxqbll.GetMoHuList(string.Format(" and JKDAH='{0}' ", grdabh), "sql_select_people_txm");
                    if (dt_tjry_txm_tem != null && dt_tjry_txm_tem.Rows.Count > 0)
                    {
                        errMsg = "已经存在体检信息！";
                        return "0";
                    }
                    //体检编号
                    strtmbm = dt_tjryxx.Rows[0]["TJBM"] != null ? dt_tjryxx.Rows[0]["TJBM"].ToString() : grdabh;
                }
                else
                {
                    dt_tjryxx.Rows.Add();
                    dt_tjryxx.Rows[0]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
                    dt_tjryxx.Rows[0]["TJJHBM"] = DateTime.Now.ToString("yyyyMMdd");//体检计划编码
                    dt_tjryxx.Rows[0]["TJPCH"] = DateTime.Now.ToString("hhmmss");//体检批次号

                    //取得顺番号
                    DataTable dt_SFH = form_daxqbll.GetMoHuList("", "sql076");
                    if (dt_SFH != null && dt_SFH.Rows.Count > 0 && dt_SFH.Rows[0]["SFH"] != null && dt_SFH.Rows[0]["SFH"].ToString().Length > 0)
                    {
                        dt_tjryxx.Rows[0]["SFH"] = int.Parse(dt_SFH.Rows[0]["SFH"].ToString()) + 1;//顺番号
                    }
                    else
                    {
                        dt_tjryxx.Rows[0]["SFH"] = "0";//顺番号
                    }

                    dt_tjryxx.Rows[0]["SXHM"] = "0";
                    string strTJBM = grdabh.PadLeft(12, '0');
                    dt_tjryxx.Rows[0]["TJBM"] = strTJBM.Substring(strTJBM.Length - 12);//个人体检编号
                    dt_tjryxx.Rows[0]["JKDAH"] = grdabh;//个人健康档案号
                    //dt_tjryxx.Rows[0]["GRBM"] = "0";//个人编码
                    dt_tjryxx.Rows[0]["XM"] = xm_para;//姓名
                    dt_tjryxx.Rows[0]["XB"] = xb_para.Length > 0 ? (xb_para.ToString().Equals("男") ? "1" : "2") : "";//性别
                    dt_tjryxx.Rows[0]["SFZH"] = sfzh_para;//身份证号
                    dt_tjryxx.Rows[0]["CSRQ"] = csrq_para;//出生日期
                    dt_tjryxx.Rows[0]["LXDH"] = lxdh;//出生日期
                    dt_tjryxx.Rows[0]["FL"] = "2";//体检人员分类
                    dt_tjryxx.Rows[0]["BZ"] = "";//备注
                    dt_tjryxx.Rows[0]["TJBH_TEM"] = grdabh;//临时个人体检编号
                    dt_tjryxx.Rows[0]["CREATETIME"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//创建时间
                    dt_tjryxx.Rows[0]["CREATEUSER"] = UserInfo.userId;//创建者
                    dt_tjryxx.Rows[0]["UPDATETIME"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//更新时间
                    dt_tjryxx.Rows[0]["UPDATEUSER"] = UserInfo.userId;//更新者
                    dt_tjryxx.Rows[0]["SCZT"] = "2";//数据上传状态
                    dt_tjryxx.Rows[0]["ZLBZ"] = "1";//增量标志
                    dt_tjryxx.Rows[0]["nd"] = DateTime.Now.Year.ToString();//年度
                    dt_tjryxx.Rows[0]["ISSH"] = "0";//登记
                    dt_tjryxx.Rows[0]["ISNEWDOC"] = "1";//登记

                    //所属机构
                    dt_tjryxx.Rows[0]["PRGID"] = p_rgid;

                    //数据存储
                    dt_tjryxx.Rows[0]["DATAFROM"] = datafrom;

                    //增加体检人员信息
                    form_daxqbll.Add(dt_tjryxx, "sql_add_people");
                }

                //生成lis信息
                DataTable dt_tmList = new DataTable(); //sql030
                dt_tmList = form_daxqbll.GetMoHuList(string.Format(" and  YLJGBM ='{0}' ", UserInfo.Yybm), "sql030");

                if (dt_tmList != null && dt_tmList.Rows.Count > 0)
                {
                    //主表
                    DataTable dt_reqmain = new DataTable();
                    dt_reqmain = form_daxqbll.GetMoHuList(" and 1=2 ", "sql070");
                    //明细表
                    DataTable dt_reqdetail = new DataTable();
                    dt_reqdetail = form_daxqbll.GetMoHuList(" and 1=2 ", "sql071");

                    for (int i = 0; i < dt_tmList.Rows.Count; i++)
                    {
                        //string sqh = grdabh.ToString().PadLeft(10, '0') + dt_tmList.Rows[i]["TMBM"].ToString();
                        string sqh = strtmbm.ToString().PadLeft(10, '0') + dt_tmList.Rows[i]["TMBM"].ToString();
                        //sqh = DateTime .Now.Year .ToString ().Substring (2,2)+ sqh.Substring(sqh.Length - 12, 12);

                        //体检人员条形码对应表(T_JK_TJRY_TXM）
                        DataTable t_jk_tjry_txm = new DataTable();
                        t_jk_tjry_txm.Columns.Add("YLJGBM");//医疗机构编码
                        t_jk_tjry_txm.Columns.Add("TXMBH");//条形码号码
                        t_jk_tjry_txm.Columns.Add("TMBM");//条码分类
                        t_jk_tjry_txm.Columns.Add("JKDAH");//个人健康档案号
                        t_jk_tjry_txm.Columns.Add("SFZH");//身份证号
                        t_jk_tjry_txm.Columns.Add("BZ");//备注
                        t_jk_tjry_txm.Columns.Add("ZLBZ");//增量标志
                        t_jk_tjry_txm.Columns.Add("nd");//年度
                        t_jk_tjry_txm.Rows.Add();
                        //增加体检人员条形码对应关系
                        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
                        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["TXMBH"] = sqh;//条形码号码
                        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["TMBM"] = dt_tmList.Rows[i]["TMBM"].ToString();//条码分类
                        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["JKDAH"] = grdabh;//个人健康档案号
                        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["SFZH"] = sfzh_para;//身份证号
                        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["BZ"] = "**";//备注
                        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["ZLBZ"] = "1";//增量标志
                        t_jk_tjry_txm.Rows[t_jk_tjry_txm.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString();//年度

                        //增加体检人员条形码对应关系
                        form_daxqbll.Add(t_jk_tjry_txm, "sql_add_people_txm");


                        dt_reqmain.Rows.Add();
                        //主表
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sqh"] = sqh; //申请号
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ksdh"] = "";//送检科室代码
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sqys"] = "";//申请医生代码
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sqsj"] = DateTime.Now.ToString("yyyy-MM-dd");//申请时间
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jsys"] = "";//接收医生
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jssj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//接收时间
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zt"] = "1";//状态
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jjzt"] = "1";//计价状态
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brly"] = getBrly(UserInfo.Yybm);//病人来源
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brdh"] = grdabh.ToString();//病历号
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brxm"] = xm_para;//病人姓名
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brxb"] = xb_para.Length > 0 ? (xb_para.ToString().Equals("男") ? "1" : "2") : "";//病人性别
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brsr"] = "";// csrq_para;//病人生日
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bz1"] = dt_tmList.Rows[i]["SQXMDH"].ToString();
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bz2"] = dt_tmList.Rows[i]["ybName"]!=null? dt_tmList.Rows[i]["ybName"].ToString():"";
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bz3"] = "";
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jzbz"] = "0";//急诊标志
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["txm"] = "";
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ch"] = "";//床号
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["yblx"] = "";//样本类型
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zxys"] = "";//执行医生
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zxsj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//执行时间
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bgddh"] = "";
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["costs"] = 0;
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["nl"] = nl;//年龄
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["nldw"] = "1";//年龄单位
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zd"] = "";//临床诊断
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["cysj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//采样时间
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["cksj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ckzj"] = "";
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ckyh"] = "";
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sfzh"] = sfzh_para;//身份证号
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jkdah"] = grdabh;//健康档案号
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["yybm"] = UserInfo.Yybm;//医院编码
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["dataFrom"] = "1";//数据来源
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zlbz"] = "1";//增量标志
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString() ;//年度
                        dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["TMBM"] = dt_tmList.Rows[i]["TMBM"].ToString();//条码编码
                        //生成明细
                        string tmfl = dt_tmList.Rows[i]["sqxmdh"] != null ? dt_tmList.Rows[i]["sqxmdh"].ToString() : "";
                        DataTable dt_tmList_reqdetail = form_daxqbll.GetMoHuList(string.Format(" and  YLJGBM ='{0}' and SQXMDH='{1}' ", UserInfo.Yybm, tmfl), "sql078");
                        if (dt_tmList_reqdetail != null)
                        {
                            for (int j = 0; j < dt_tmList_reqdetail.Rows.Count; j++)
                            {
                                dt_reqdetail.Rows.Add();
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sqh"] = sqh; //申请号
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["xh"] = (j + 1).ToString();//序号
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sqxmdh"] = dt_tmList_reqdetail.Rows[j]["ITEM_CODE"].ToString();//申请项目代码
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sqxmmc"] = dt_tmList_reqdetail.Rows[j]["ITEM_NAME"].ToString();//申请项目名称
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sl"] = "1";//数量
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dj"] = "0";//单价
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["zt"] = "1";//状态
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["jjzt"] = "1";//计价状态
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["bz1"] = "";//备注1字段
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["bz2"] = "";//
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["costs"] = 0;//
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["numbk1"] = 0;//
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["numbk2"] = 0;//
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dtbk1"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dtbk2"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["yybm"] = UserInfo.Yybm;//医院编码
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dataFrom"] = "1";//数据来源
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["zlbz"] = "1";//增量标志
                                dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString();//年度
                            }
                        }
                    }

                    //检验主表
                    form_daxqbll.Add(dt_reqmain, "sql074");

                    //检验明细表
                    form_daxqbll.Add(dt_reqdetail, "sql075");

                }
                return "1";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return "-1";
            }
        }

        /// <summary>
        /// 获取病人来源
        /// </summary>
        /// <param name="yljgbm"></param>
        /// <returns></returns>
        public string getBrly(string yljgbm)
        {
            try
            {
                DBAccess dBAccess = new DBAccess();
                string sqlBrly="select * from T_TJ_YLJG_XIANGZHEN where YLJGBM='{YLJGBM}'";
                DataTable dtBrly = dBAccess.ExecuteQueryBySql(sqlBrly.Replace("{YLJGBM}", yljgbm));
                if (dtBrly != null && dtBrly.Rows.Count > 0)
                {
                    return dtBrly.Rows [0]["brly"]!=null? dtBrly.Rows [0]["brly"].ToString ():"4";
                }
            }
            catch (Exception ex)
            {

            }
            return "4";
        }


        #endregion

        /// <summary>
        /// 姓名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_xm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }
        /// <summary>
        /// 绑定数据值
        /// </summary>
        /// <param name="drp"></param>
        /// <param name="initValue"></param>
        /// <returns></returns>
        public bool setDrp(ComboBox drp, string strwhere, string sqlCode, bool ifkh)
        {
            try
            {
                drpflag = false;
                Form_daxqBll form_daxq = new Form_daxqBll();

                //获取结果集
                DataTable dt = form_daxq.GetMoHuList(strwhere, sqlCode);
                if (dt == null)
                {
                    return false;
                }
                if (ifkh == true)
                {
                    DataRow dtRow = dt.NewRow();
                    dt.Rows.InsertAt(dtRow, 0);
                }

                drp.DisplayMember = "B_NAME";
                drp.ValueMember = "B_RGID";
                drp.DataSource = dt;
                drpflag = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        //private void comboBox_head_XB_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    xb_mc = comboBox_head_XB.SelectedText;
        //    xb = comboBox_head_XB.SelectedValue != null ? comboBox_head_XB.SelectedValue.ToString() : "";
        //}


        #region 居住地址
        /// <summary>
        /// 省
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_SHENG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpflag == false)
            {
                return;
            }
        }

        /// <summary>
        /// 市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_SHI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpflag == false)
            {
                return;
            }
            //县
            if (comboBox_D_SHI.SelectedValue != null && comboBox_D_SHI.SelectedValue.ToString().Length > 0)
            {
                setDrp(comboBox_D_QU, string.Format(" and B_RGID like '%{0}%'", comboBox_D_SHI.SelectedValue.ToString()), "sql053", true);
            }
            else
            {
                comboBox_D_QU.DataSource = null;
            }
            //镇
            comboBox_D_JD.DataSource = null;
            //村
            comboBox_D_JWH.DataSource = null;
        }
        /// <summary>
        /// 县
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_QU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpflag == false)
            {
                return;
            }
            //镇
            if (comboBox_D_QU.SelectedValue != null && comboBox_D_QU.SelectedValue.ToString().Length > 0)
            {
                setDrp(comboBox_D_JD, string.Format(" and B_RGID like '%{0}%'", comboBox_D_QU.SelectedValue.ToString()), "sql054", true);
            }
            else
            {
                comboBox_D_JD.DataSource = null;
            }
            //村
            comboBox_D_JWH.DataSource = null;
        }
        /// <summary>
        /// 镇
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_JD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpflag == false)
            {
                return;
            }
            //村
            if (comboBox_D_JD.SelectedValue != null && comboBox_D_JD.SelectedValue.ToString().Length > 0)
            {
                setDrp(comboBox_D_JWH, string.Format(" and B_RGID like '%{0}%'", comboBox_D_JD.SelectedValue.ToString()), "sql055", true);
            }
            else
            {
                comboBox_D_JWH.DataSource = null;
            }

        }
        /// <summary>
        /// 村
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_D_JWH_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (drpflag == false)
            {
                return;
            }
            czbm = comboBox_D_JWH.SelectedValue != null ? comboBox_D_JWH.SelectedValue.ToString() : "";

        }
        #endregion
    }
}
