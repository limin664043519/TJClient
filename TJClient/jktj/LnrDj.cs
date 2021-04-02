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
using TJClient.sys.Bll;
using System.Drawing.Drawing2D;
using TJClient.Bll;
using TJClient.ComForm;
using TJClient.sys;
using TJClient.jktj;
using TJClient.model;
using TJClient.NeedToUseForm;
using TJClient.Properties;

namespace FBYClient
{
    public partial class LnrDj : sysCommonForm
    {
        #region 变量声明
        /// <summary>
        /// 用户名
        /// </summary>
        private static string userId = "";

        /// <summary>
        /// 分组
        /// </summary>
        private static string yhfz = "";

        /// <summary>
        /// 医疗机构
        /// </summary>
        private static string yljg = "";

        /// <summary>
        /// 村庄编码
        /// </summary>
        private static string czbm = "";

        /// <summary>
        /// 年龄
        /// </summary>
        private string nl = "";

        /// <summary>
        /// 保存每个页面要显示的数据
        /// </summary>
        public DataTable dtPage = null;

        /// <summary>
        /// 完整的数据列表
        /// </summary>
        private DataTable dt_tem = new DataTable();

        /// <summary>
        /// 选中行
        /// </summary>
        private int selectedRowIndex = -1;

        //读取身份证号
        public Form_readSfzh form_readsfzh = new Form_readSfzh();

        //是否折叠
        private bool isZhedie = true;

        /// <summary>
        /// 前页面传过来的参数
        /// </summary>
        DataTable dt_para_sys = null;

        /// <summary>
        /// 前一页面传过来的信息
        /// </summary>
        DataTable dt_paraFromParent = null;

        #endregion

        #region 窗体加载处理

        public LnrDj()
        {
            InitializeComponent();
        }

        public bool setPara(DataTable dtpara)
        {
            try
            {
                dt_para_sys = dtpara;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddForm_Load(object sender, EventArgs e)
        {
            //禁用列表自动增加列
            dataGridView_list.AutoGenerateColumns = false;

            form_readsfzh.Show();
            form_readsfzh.Owner = this;
            form_readsfzh.Visible = false;
            //取得前以页面传递的数据
            //DataTable dt = (DataTable)((jktj)this.Owner).Tag;
            //用户id
            userId = dt_para_sys.Rows[0]["userId"].ToString();
            //工作组
            yhfz = dt_para_sys.Rows[0]["gzz"].ToString();
            //医疗机构
            yljg = dt_para_sys.Rows[0]["yljg"].ToString();

            //村庄编码
            //czbm = dt_para_sys.Rows[0]["czbm"].ToString();

            DBAccess dBAccess = new DBAccess();
            //取得村庄
            //            string sqlCunzhaung = @"SELECT T_BS_CUNZHUANG.B_RGID, T_BS_CUNZHUANG.B_NAME
            //                                    FROM T_TJ_YLJG_XIANGZHEN INNER JOIN T_BS_CUNZHUANG ON    T_TJ_YLJG_XIANGZHEN.XZBM = left(T_BS_CUNZHUANG.B_RGID,len(T_TJ_YLJG_XIANGZHEN.XZBM))
            //
            //                                    where  T_TJ_YLJG_XIANGZHEN.YLJGBM='{YLJGBM}'
            //                                     order by T_BS_CUNZHUANG.B_RGID;";
            // DataTable dtCunzhuang = dBAccess.ExecuteQueryBySql(sqlCunzhaung.Replace("{YLJGBM}", UserInfo.Yybm));


            //AddFormBll addformbll_cz = new AddFormBll();
            //DataTable dtCunzhuang = addformbll_cz.GetMoHuList(string.Format(" and YLJGBM='{0}' ", UserInfo.Yybm), "sql038_cunzhuang");
            listboxFormBll listbox = new listboxFormBll();
            DataTable dtCunzhuang = listbox.GetMoHuList(string.Format("and YLJGBM = '{0}'", UserInfo.Yybm), "sql038");
            //绑定村庄
            DataRow dtRow = dtCunzhuang.NewRow();
            dtRow["czbm"] = "";
            dtRow["czmc"] = "--请选择--";
            dtCunzhuang.Rows.InsertAt(dtRow, 0);
            comboBox_cunzhuang.DataSource = dtCunzhuang;
            comboBox_cunzhuang.DisplayMember = "czmc";
            comboBox_cunzhuang.ValueMember = "czbm";
            comboBox_cunzhuang.SelectedValue = czbm;

            comboBox_cunzhuang.Focus();

            //卫生室

            //取得卫生室
            string sqlWss = " select  T_TJ_YLJG_XIANGZHEN.YLJGBM_JK as codeid  , sys_org.[NAMES] as codename  from  T_TJ_YLJG_XIANGZHEN  left join  sys_org   on T_TJ_YLJG_XIANGZHEN.YLJGBM=sys_org.CODES  ";
            DataTable dtWss = dBAccess.ExecuteQueryBySql(sqlWss);

            //绑定卫生室
            DataRow dtRow_wss = dtWss.NewRow();
            dtRow_wss["codeid"] = "";
            dtRow_wss["codename"] = "--请选择--";
            dtWss.Rows.InsertAt(dtRow_wss, 0);
            comboBox_wss.DataSource = dtWss;
            comboBox_wss.DisplayMember = "codename";
            comboBox_wss.ValueMember = "codeid";


            //最大顺序号
            AddFormBll addformbll = new AddFormBll();
            string sqlWhere = string.Format(" and YLJGBM='{0}' and TJSJ='{1}' and TJTYPE='0'", UserInfo.Yybm, DateTime.Now.ToString("yyyy-MM-dd"));
            DataTable dt_MaxSxh = addformbll.GetMoHuList(sqlWhere, "SQL063_TJZT_MAXSXH");

            if (dt_MaxSxh != null && dt_MaxSxh.Rows.Count > 0)
            {
                if (dt_MaxSxh.Rows[0]["sxh_dj"] == DBNull.Value)
                {
                    textBox_sxh_dj.Text = "1";
                }
                else
                {
                    textBox_sxh_dj.Text = (int.Parse(dt_MaxSxh.Rows[0]["sxh_dj"].ToString()) + 1).ToString();
                }
            }

            //新增人员信息详情页
            /*Form_PeopleAdd form_peopleadd = new Form_PeopleAdd();
            form_peopleadd.TopLevel = false;
            form_peopleadd.Parent = panel_center;
            form_peopleadd.Dock = DockStyle.Fill;
            form_peopleadd.FormBorderStyle = FormBorderStyle.None;
            form_peopleadd.Show();*/

            //新增签名拍照页
            Form_photo form_photo = new Form_photo();
            form_photo.Name = "Form_photo";
            form_photo.TopLevel = false;
            form_photo.Parent = panel_right;
            form_photo.Dock = DockStyle.Fill;
            form_photo.FormBorderStyle = FormBorderStyle.None;
            form_photo.Show();
        }

        #endregion

        #region 检索

        /// <summary>
        /// 姓名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_xm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                jkda_select(true);
            }

        }

        /// <summary>
        /// 身份证号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_sfzh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string errMsg = "";
                string sqlWhere = "";

                //身份证号
                if (textBox_sfzh.Text.Length > 0)
                {
                    sqlWhere = sqlWhere + " and  D_SFZH like '%" + textBox_sfzh.Text + "%'";
                }

                //按照读物的身份证号查询人员信息
                bool selectResult = getRyxxByWhere(sqlWhere, ref errMsg);

                //人员信息不存在  按照一代身份证号查询信息 或者姓名
                /* if (selectResult == false && (textBox_sfzh.Text.Length >= 15 || textBox_xm.Text.Length > 0))
                 {

                     DialogResult dr = MessageBox.Show("未查询到人员信息，是否按照 [姓名] 或 [转换成一代身份证号] 重新查询？", "信息", MessageBoxButtons.OKCancel);
                      if (dr == DialogResult.OK)//如果点击“确定”按钮
                      {

                          string strsfzh_15 = textBox_sfzh.Text.Length >= 17 ? textBox_sfzh.Text.Substring(0, 6) + textBox_sfzh.Text.Substring(8, 9) : textBox_sfzh.Text.Substring(0, 6) + textBox_sfzh.Text.Substring(8);
                          string strsfzh_17 = textBox_sfzh.Text.Length >= 17 ? textBox_sfzh.Text.Substring(0, 17) : textBox_sfzh.Text;

                          string strxm = textBox_xm.Text.Length > 0 ? textBox_xm.Text : "没有姓名";
                          sqlWhere = string.Format(" and  ( D_SFZH like '%{0}%' or  D_SFZH like '%{1}%' or D_XM like '%{2}%' ) ", strsfzh_15, strsfzh_17, strxm);
                          selectResult = getRyxxByWhere(sqlWhere, ref errMsg);
                      }
                 }*/

                //按照身份证号取得信息
                if (selectResult == false)
                {
                    DialogResult dr = MessageBox.Show("没有查询到人员信息，是否添加新建人员？", "信息", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)//如果点击“确定”按钮
                    {
                        //建议档案
                        Form_PeopleAdd form_peopleadd = new Form_PeopleAdd();
                        form_peopleadd.czbm = comboBox_cunzhuang.SelectedValue != null ? comboBox_cunzhuang.SelectedValue.ToString() : "";
                        form_peopleadd.peopleAddType = "1";//0:体检是增加  1:登记时增加    
                        form_peopleadd.jkdah = textBox_jkda_tem.Text.Trim();
                        form_peopleadd.sfzh = textBox_sfzh.Text.Trim();
                        form_peopleadd.csrq = label_csrq.Text.Trim();
                        form_peopleadd.xm = label_xm.Text.Trim();
                        form_peopleadd.xb_mc = label_xb.Text.Trim();
                        form_peopleadd.Owner = this;
                        form_peopleadd.ShowDialog();
                        ////完整档案创建
                        //Form_daxq_New form_daxq_new = new Form_daxq_New();
                        //form_daxq_new.Show();
                    }
                }
                else
                {
                    //姓名验证
                    if (checkBox_xmcheck.Checked == true && dt_tem != null && dt_tem.Rows.Count > 0)
                    {
                        //if (dt_tem.Rows.Count == 0)
                        //{
                        string xm_str = "";
                        if (dt_tem.Rows[0]["D_XM"] != DBNull.Value && dt_tem.Rows[0]["D_XM"] != null && dt_tem.Rows[0]["D_XM"].ToString().Length > 0)
                        {
                            //档案中的姓名
                            xm_str = dt_tem.Rows[0]["D_XM"].ToString().Trim();
                        }
                        //if (!label_xm.Text.Trim().Equals(xm_str))
                        //{
                        //档案中的姓名与身份证的姓名不符
                        peopleXmUpdate peoplexmupdate = new peopleXmUpdate();
                        peoplexmupdate.Owner = this;

                        //档案信息
                        peoplexmupdate.dt_info = dt_tem;
                        //身份证号
                        peoplexmupdate.sfzh = textBox_sfzh.Text.Trim();
                        //姓名
                        peoplexmupdate.xm_sfz = label_xm.Text.Trim();

                        //验证身份证号码
                        if (Common.CheckIDCard(textBox_sfzh.Text.Trim()) == true)
                        {
                            string[] GetCardIdInfo = Common.GetCardIdInfo(textBox_sfzh.Text.Trim());
                            //性别
                            peoplexmupdate.xb_sfz = GetCardIdInfo[3];
                            //出生日期
                            peoplexmupdate.csrq_sfz = GetCardIdInfo[1];
                        }
                        peoplexmupdate.Show();
                    }
                }
            }
        }
        /// <summary>
        /// 档案号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_jkda_tem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //按照档案号检索
                if (jkda_select(false) == false)
                {
                    Form_PeopleAdd form_peopleadd = new Form_PeopleAdd();
                    //form_peopleadd.tjbh = textBox_TJBH.Text;
                    form_peopleadd.czbm = comboBox_cunzhuang.SelectedValue != null ? comboBox_cunzhuang.SelectedValue.ToString() : "";
                    form_peopleadd.peopleAddType = "1";//0:体检是增加  1:登记时增加
                    form_peopleadd.jkdah = textBox_jkda_tem.Text;
                    form_peopleadd.sfzh = textBox_sfzh.Text;
                    form_peopleadd.xm = textBox_xm.Text;
                    form_peopleadd.Owner = this;
                    form_peopleadd.ShowDialog();
                }
            }
        }

        /// <summary>
        /// 设定返回的结果
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public override bool setParentFormDo(object para)
        {
            //DataTable dt = new DataTable();
            //if (para != null)
            //{
            //    dt = (DataTable)para;
            //}


            jkda_select(true);
            return true;
        }


        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click(object sender, EventArgs e)
        {
            jkda_select(true);
        }

        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="isShowMsg">是否显示提示信息：true：显示 false 不显示</param>
        /// <returns></returns>
        public bool jkda_select(bool isShowMsg)
        {
            string sqlWhere = "";
            try
            {
                //村庄
                if (comboBox_cunzhuang.SelectedValue != null && comboBox_cunzhuang.SelectedValue.ToString().Length > 0)
                {
                    //sqlWhere = " and  D_JWH like '%" + comboBox_cunzhuang.SelectedValue.ToString() + "%'";
                    sqlWhere = " and  P_RGID ='" + comboBox_cunzhuang.SelectedValue.ToString() + "'";
                }
                //姓名
                if (textBox_xm.Text.Length > 0)
                {
                    sqlWhere = sqlWhere + " and   D_XM like '%" + textBox_xm.Text + "%'";
                }

                //身份证号
                if (textBox_sfzh.Text.Length > 0)
                {
                    sqlWhere = sqlWhere + " and  D_SFZH like '%" + textBox_sfzh.Text + "%'";
                }

                //登记
                if (checkBox_wdj.Checked == true && checkBox_nlcheck.Checked == true)
                {
                    //sqlWhere = sqlWhere + " and  T_JK_TJRYXX.djzt  is not null ";
                }
                else
                {
                    //未登记
                    if (checkBox_wdj.Checked == true)
                    {
                        sqlWhere = sqlWhere + " and   zt  is null ";
                    }
                    //已登记
                    if (checkBox_ydj.Checked == true)
                    {
                        sqlWhere = sqlWhere + " and  zt  ='1' ";
                    }
                }

                //登记时间
                if (dateTimePicker_start.Checked == true)
                {
                    //UPDATETIME
                    sqlWhere = sqlWhere + string.Format(" and  tjsj  >='{0}' ", dateTimePicker_start.Value.ToString("yyyy-MM-dd"));
                }

                //登记时间
                if (dateTimePicker_end.Checked == true)
                {
                    //UPDATETIME
                    sqlWhere = sqlWhere + string.Format(" and  tjsj  <='{0}' ", dateTimePicker_end.Value.ToString("yyyy-MM-dd"));
                }

                //选择了更多条件时
                if (checkBox_tj.Checked == true)
                {
                    //健康档案号
                    if (textBox_jkda_tem.Text.Length > 0)
                    {
                        sqlWhere = sqlWhere + " and  D_GRDABH like '%" + textBox_jkda_tem.Text + "%'";
                    }

                    if (comboBox_wss.SelectedValue != null && comboBox_wss.SelectedValue.ToString().Length > 0)
                    {
                        sqlWhere = sqlWhere + " and  P_rgid like '%" + comboBox_wss.SelectedValue.ToString() + "%'";

                    }

                    string sqlWhereAnd = " and ({0})";
                    string sqlWhereOr = "";

                    //年龄
                    if (textBox_nl.Text.Trim().Length > 0)
                    {
                        sqlWhereOr = string.Format(" {0} or Cint(iif(len(nl)>0,nl,'0'))>={1} ", sqlWhereOr, textBox_nl.Text.Trim());
                    }
                    //高血压
                    if (checkBox_gxy.Checked == true)
                    {
                        sqlWhereOr = string.Format(" {0} or Instr(','+diseases+',',',{1},')>0 ", sqlWhereOr, "2");

                    }
                    //糖尿病
                    if (checkBox_tnb.Checked == true)
                    {
                        sqlWhereOr = string.Format(" {0} or Instr(','+diseases+',',',{1},')>0 ", sqlWhereOr, "3");
                    }
                    //冠心病
                    if (checkBox_gxb.Checked == true)
                    {
                        sqlWhereOr = string.Format(" {0} or Instr(','+diseases+',',',{1},')>0  ", sqlWhereOr, "4");
                    }
                    //脑卒中
                    if (checkBox_ncz.Checked == true)
                    {
                        sqlWhereOr = string.Format(" {0} or Instr(','+diseases+',',',{1},')>0  ", sqlWhereOr, "7");
                    }
                    //精神疾病
                    if (checkBox_jsjb.Checked == true)
                    {
                        sqlWhereOr = string.Format(" {0} or Instr(','+diseases+',',',{1},')>0  ", sqlWhereOr, "8");
                    }
                    //其他
                    if (checkBox_qt.Checked == true)
                    {
                        sqlWhereOr = string.Format(" {0} or Instr(','+diseases+',',',{1},')>0  ", sqlWhereOr, "13");
                    }
                    //在这里追加checkbox_photo
                    if (checkbox_photo.Checked == true)
                    {
                        sqlWhere += " and photourl<>''";
                    }
                    //在这里追加checkbox_signname
                    if (checkbox_signname.Checked == true)
                    {
                        sqlWhere += " and (brsignname<>'' or jssignname<>'' or brrealname<>'' or jsrealname<>'' )";
                    }
                    if (sqlWhereOr.Length > 0)
                    {
                        sqlWhereAnd = string.Format(sqlWhereAnd, " 1=2 " + sqlWhereOr);
                        sqlWhere = sqlWhere + sqlWhereAnd;
                    }


                    //数据类型
                    string sqlWhereAnd_type = " and ({0})";
                    string sqlWhereOr_type = "";

                    //已修改
                    if (checkBox_update.Checked == true)
                    {
                        sqlWhereOr_type = string.Format(" {0} or updateFlag='1' ", sqlWhereOr_type);
                    }
                    //else {

                    //    sqlWhereOr_type = string.Format(" {0} or updateFlag='' or  updateFlag is null ", sqlWhereOr_type);
                    //}

                    //已删除
                    if (checkBox_delete.Checked == true)
                    {
                        sqlWhereOr_type = string.Format(" {0} or deleteFlag='1' ", sqlWhereOr_type);
                    }
                    //else
                    //{
                    //    sqlWhereOr_type = string.Format(" {0} or deleteFlag='' or  deleteFlag is null ", sqlWhereOr_type);
                    //}

                    //新增
                    if (checkBox_add.Checked == true)
                    {
                        sqlWhereOr_type = string.Format(" {0} or ISNEWDOC='1' ", sqlWhereOr_type);
                    }

                    if (sqlWhereOr_type.Length > 0)
                    {
                        sqlWhereAnd_type = string.Format(sqlWhereAnd_type, " 1=2 " + sqlWhereOr_type);
                        sqlWhere = sqlWhere + sqlWhereAnd_type;
                    }
                }
                else
                {
                    sqlWhere = sqlWhere + " and (deleteFlag='' or deleteFlag is null ) ";
                }

                string errMsg = "";

                if (getRyxxByWhere(sqlWhere, ref errMsg) == false)
                {
                    if (isShowMsg == true)
                    {
                        MessageBox.Show(errMsg);
                    }
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }
        /// <summary>
        /// 取得当前页要显示的数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public DataTable GetPageFromAll(int start, int end)
        {
            dtPage = new DataTable();
            dtPage = dt_tem.Clone();
            for (int i = start; ((i < dt_tem.Rows.Count) && (i <= end)); i++)
            {
                if (dt_tem.Rows[i]["diseases"] != null && dt_tem.Rows[i]["diseases"] != DBNull.Value)
                {
                    dt_tem.Rows[i]["diseases"] = getDiseasesName(dt_tem.Rows[i]["diseases"].ToString());
                }
                dtPage.ImportRow(dt_tem.Rows[i]);
            }
            return dtPage;

        }

        /// <summary>
        /// 取得疾病的名称
        /// </summary>
        /// <param name="DiseasesCode"></param>
        /// <returns></returns>
        public string getDiseasesName(string DiseasesCode)
        {
            string DiseasesName = "";
            string[] DiseasesCodeList = DiseasesCode.Split(new char[] { ',' });
            if (DiseasesCodeList != null && DiseasesCodeList.Length > 0)
            {
                for (int i = 0; i < DiseasesCodeList.Length; i++)
                {
                    if (DiseasesCodeList[i].Equals("2"))//高血压2
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "高血压");
                    }
                    else if (DiseasesCodeList[i].Equals("3"))//糖尿病3
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "糖尿病");
                    }
                    else if (DiseasesCodeList[i].Equals("4")) //冠心病4
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "冠心病");
                    }
                    else if (DiseasesCodeList[i].Equals("5"))//慢性阻塞性肺疾病5
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "慢性阻塞性肺疾病");
                    }
                    else if (DiseasesCodeList[i].Equals("6"))//恶性肿瘤6
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "恶性肿瘤");
                    }
                    else if (DiseasesCodeList[i].Equals("7"))//脑卒中7
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "脑卒中");
                    }
                    else if (DiseasesCodeList[i].Equals("8")) //重性精神疾病8
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "重性精神疾病");
                    }
                    else if (DiseasesCodeList[i].Equals("9")) //结核病9
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "结核病");
                    }
                    else if (DiseasesCodeList[i].Equals("10")) //肝炎10
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "肝炎");
                    }
                    else if (DiseasesCodeList[i].Equals("11")) //其他法定传染病11
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "其他法定传染病");
                    }
                    else if (DiseasesCodeList[i].Equals("12")) //职业病12
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "职业病");
                    }
                    else if (DiseasesCodeList[i].Equals("13")) //其他13
                    {
                        DiseasesName = string.Format(" {0} {1}", DiseasesName, "其他");
                    }
                }
            }
            return DiseasesName.Trim().Length == 0 ? DiseasesCode : DiseasesName.Trim();
        }

        /// <summary>   
        /// GridViw数据绑定   
        /// </summary>   
        /// <returns></returns>   
        private int BindDgv()
        {

            //传入要取的第一条和最后一条   
            int start = pager1.PageCurrent > 0 ? (pager1.PageSize * (pager1.PageCurrent - 1)) : 0;
            int end = pager1.PageCurrent > 0 ? (pager1.PageSize * pager1.PageCurrent) : pager1.PageSize;

            //数据源   
            dtPage = GetPageFromAll(start, end);
            //绑定分页控件   
            pager1.bindingSource1.DataSource = dtPage;
            pager1.bindingNavigator1.BindingSource = pager1.bindingSource1;
            //讲分页控件绑定DataGridView   
            dataGridView_list.DataSource = dtPage;
            //返回总记录数   
            return dt_tem.Rows.Count;
        }

        /// <summary>   
        /// 分页控件产生的事件   
        /// </summary>   
        private int pager1_EventPaging(HuishengFS.Controls.EventPagingArg e)
        {
            return BindDgv();
        }

        /// <summary>   
        /// 加载分页 或许写在Load事件里面   
        /// </summary>   
        private void FrmPage_Shown(object sender, EventArgs e)
        {
            #region DataGridView与Pager控件绑定
            this.pager1.PageCurrent = 1;//当前页为第一页   

            pager1.PageSize = 30;//页数   
            this.pager1.Bind();//绑定  
            #endregion
        }

        /// <summary>
        /// 按照条件获取信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <param name="Errmsg"></param>
        /// <returns></returns>
        public bool getRyxxByWhere(string sqlWhere, ref string Errmsg)
        {
            Errmsg = "";
            try
            {
                //按照条件检索
                AddFormBll addformbll = new AddFormBll();
                DataTable dt_jktj = addformbll.GetMoHuList(sqlWhere, "sql050");
                //dataGridView_list.DataSource = dt_jktj;

                dt_tem = dt_jktj;

                if (dt_jktj == null || dt_jktj.Rows.Count == 0)
                {
                    Errmsg = "没有取到数据！";
                    //数据绑定
                    BindDgv();
                    return false;
                }
                else
                {
                    DataColumn datacolumn = new DataColumn("select_ZT", System.Type.GetType("System.Int32"));
                    datacolumn.DefaultValue = 0;
                    dt_jktj.Columns.Add(datacolumn);
                    //数据绑定
                    BindDgv();
                }
                this.pager1.init();//初始化
                this.pager1.Bind();//绑定 
            }
            catch (Exception ex)
            {
                Errmsg = ex.Message;
                return false;
            }
            return true;
        }
        #endregion

        private bool QmLisTmCorresponding(string grdabh)
        {
            List<Tm> tmList = TmBll.GetTmList();
            if (tmList.Count <= 0)
            {
                MessageBox.Show("未设置检验化验条码，Lis条码对应失败");
                return false;
            }

            TjryTxm txm = TjryTxmBll.GetLisTjryTxm(tmList[0].Tmbm, grdabh);
            if (txm != null && !string.IsNullOrEmpty(txm.Txmbh))
            {
                //执行插入操作
                return LisTmCorrespondingBll.Operation(new LisTmCorresponding()
                {
                    CreateDate = DateHelper.CurrDate(),
                    LisTm = txm.Txmbh,
                    QmLisTm = QmLisTm.Info,
                    IncludeBlood = QmLisTm.IncludeBlood,
                    Status = 0
                });
            }
            else
            {
                MessageBox.Show("当前体检人员没有检验化验条码");
                return false;
            }
        }

        #region  打印

        /// <summary>
        /// 当前选择行的行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = e.RowIndex;
        }

        /// <summary>
        /// 明细单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_list_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //详情
                if (dataGridView_list.Columns[e.ColumnIndex].Name == "xq")
                {
                    DataRowView dr = (DataRowView)dataGridView_list.Rows[e.RowIndex].DataBoundItem;
                    //Form_daxq form = new Form_daxq();
                    //form.Owner = this;

                    ////id
                    ////form.str_id = dr["id"].ToString();
                    //form.str_jkdah = dr["D_GRDABH"].ToString();
                    //form.str_sfzh = dr["D_SFZH"].ToString();
                    //form.str_xm = dr["D_XM"].ToString();
                    ////村庄编码
                    //if (comboBox_cunzhuang.SelectedValue != null)
                    //{
                    //    form.czbm = comboBox_cunzhuang.SelectedValue.ToString();
                    //}

                    //form.ShowDialog();



                    //姓名验证
                    //if (checkBox_xmcheck.Checked == true && dt_tem != null && dt_tem.Rows.Count > 0)
                    //{
                    //if (dt_tem.Rows.Count == 0)
                    //{
                    //string xm_str = "";
                    //if (dt_tem.Rows[0]["D_XM"] != DBNull.Value && dt_tem.Rows[0]["D_XM"] != null && dt_tem.Rows[0]["D_XM"].ToString().Length > 0)
                    //{
                    //    //档案中的姓名
                    //    xm_str = dt_tem.Rows[0]["D_XM"].ToString().Trim();
                    //}
                    //if (!label_xm.Text.Trim().Equals(xm_str))
                    //{

                    //取得当前明细的结果集
                    DataTable dt_updateRows = (DataTable)dataGridView_list.DataSource;
                    DataRow[] dt_rows_tem = dt_updateRows.Select("id=" + dr["id"].ToString());
                    DataTable dt_updateRows_one = dt_updateRows.Clone();
                    dt_updateRows_one.ImportRow(dt_rows_tem[0]);

                    //档案中的姓名与身份证的姓名不符
                    peopleXmUpdate peoplexmupdate = new peopleXmUpdate();
                    peoplexmupdate.Owner = this;

                    string str_sfzh = dr["d_sfzh"].ToString();
                    //档案信息
                    peoplexmupdate.dt_info = dt_updateRows_one;
                    //身份证号
                    peoplexmupdate.sfzh = str_sfzh.Trim();
                    //姓名
                    peoplexmupdate.xm_sfz = dr["d_xm"].ToString();

                    //验证身份证号码
                    if (Common.CheckIDCard(str_sfzh.Trim()) == true)
                    {
                        string[] GetCardIdInfo = Common.GetCardIdInfo(str_sfzh.Trim());
                        //性别
                        peoplexmupdate.xb_sfz = GetCardIdInfo[3];
                        //出生日期
                        peoplexmupdate.csrq_sfz = GetCardIdInfo[1];
                    }
                    peoplexmupdate.Show();
                    //}

                }
                else if (dataGridView_list.Columns[e.ColumnIndex].Name == "printDo")
                {

                    DataRowView dr = (DataRowView)dataGridView_list.Rows[e.RowIndex].DataBoundItem;
                    string grdabh = dr["d_grdabh"].ToString(); //选择打印条码的个人档案编号
                    if (!ConfigHelper.DontNeedToCorrespondingLisTm())
                    {
                        //在这里加入弹出千麦lis条码的输入
                        FrmLisTmCorresponding frm = new FrmLisTmCorresponding();
                        frm.ShowDialog();
                        if (!QmLisTm.AssignedValue) //如果没有千麦Lis条码
                        {
                            return;
                        }
                        if (!QmLisTmCorresponding(grdabh))
                        {
                            return;
                        }
                    }
                    //条码打印控制 1:按照旧号码打印 2:按照新号码打印  3:不打印
                    string iftmdy = "2";

                    //选中打印条码行

                    //dr["sxh_dj"]  dr["zt"]
                    //已登记的人员再次打印条码
                    if (dr["zt"] != null && dr["zt"].ToString().Equals("1"))
                    {
                        //上次登记打印条码时的条码号
                        if (dr["sxh_dj"] != null && dr["sxh_dj"].ToString().Length > 0)
                        {
                            DialogResult a = MessageBox.Show(string.Format("已经存在登记号 [{0}],是否 [放弃] 该登记号重新生成登记号，打印条码?", dr["sxh_dj"].ToString()), "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (a == DialogResult.Yes)
                            {
                                // 1:按照旧号码打印 2:按照新号码打印  3:不打印
                                iftmdy = "2";
                            }
                            else if (a == DialogResult.No)
                            {
                                // 1:按照旧号码打印 2:按照新号码打印  3:不打印
                                iftmdy = "1";
                            }
                            else
                            {
                                // 1:按照旧号码打印 2:按照新号码打印  3:不打印
                                iftmdy = "3";
                                return;
                            }
                        }
                    }

                    //如果是合法的身份证取信息
                    if (Common.CheckIDCard(dr["D_SFZH"].ToString()) == true)
                    {
                        label_csrq.Text = Common.GetBirthdayByIdentityCardId(dr["D_SFZH"].ToString().Trim(), true);
                        string message = "";
                        if (checksfz_nl(out message) == false)
                        {
                            //如果需要验证年龄，显示验证结果
                            if (checkBox_nlcheck.Checked == true)
                            {
                                MessageBox.Show(message);
                                return;
                            }
                        }
                    }
                    int rowNo = (pager1.PageCurrent - 1) * pager1.PageSize + e.RowIndex;

                    //打印条码
                    bool result_print = printTm(dt_tem.Rows[rowNo], iftmdy);
                    if (result_print == true)
                    {
                        //数据绑定
                        BindDgv();
                    }
                }
                else if (dataGridView_list.Columns[e.ColumnIndex].Name == "select_ZT")
                {
                    //选择条形码
                    DataRowView dr = (DataRowView)dataGridView_list.Rows[e.RowIndex].DataBoundItem;
                    DataGridViewCheckBoxCell checkbox = dataGridView_list.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell; // 获得checkbox列单元格
                    int rowNo = (pager1.PageCurrent - 1) * pager1.PageSize + e.RowIndex;
                    if (checkbox != null && checkbox.Value.ToString().Equals("0"))
                    {
                        dt_tem.Rows[rowNo]["select_ZT"] = "1";
                        dt_tem.AcceptChanges();
                    }
                    else
                    {
                        dt_tem.Rows[rowNo]["select_ZT"] = "0";
                        dt_tem.AcceptChanges();
                    }
                }
                else if (dataGridView_list.Columns[e.ColumnIndex].Name == "D_XM" && !isZhedie)
                {
                    DataRowView drr = (DataRowView)dataGridView_list.Rows[e.RowIndex].DataBoundItem;
                    setXX(drr["D_GRDABH"].ToString());
                }

            }
        }

        private void dataGridView_list_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (isZhedie)//dataGridView_list.Columns[e.ColumnIndex].Name == "D_XM" && 
                {
                    //显示右侧信息
                    panel_right.Enabled = true;

                    DataGridViewSelectedRowCollection dt_row = dataGridView_list.SelectedRows;

                    panel_right.Controls.Clear();
                    Form_photo form_photo = new Form_photo();
                    string jkdah = dt_row[0].Cells["D_GRDABH"].Value.ToString();
                    string sfzh = dt_row[0].Cells["D_SFZH"].Value.ToString();
                    string xm = dt_row[0].Cells["D_XM"].Value.ToString();
                    string createDate = dt_row[0].Cells["TJSJ"].Value.ToString();

                    if (string.IsNullOrEmpty(createDate))
                    {
                        createDate = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    form_photo.jkdah = jkdah;
                    form_photo.sfzh = sfzh;
                    TJClient.Signname.Operation.HealthExaminedUserInfoInit(xm, jkdah, sfzh, createDate);
                    form_photo.TopLevel = false;
                    form_photo.Parent = panel_right;
                    form_photo.Dock = DockStyle.Fill;
                    form_photo.FormBorderStyle = FormBorderStyle.None;
                    form_photo.Show();
                }
            }
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ifTmDy"> 1:按照旧号码打印 2:按照新号码打印  3:不打印</param>
        /// <returns></returns>
        public bool printTm(DataRow dr, string ifTmDy)
        {
            try
            {
                //dt_tem

                //打印
                PrintHelper printDemo = new PrintHelper();

                //如果没有体检信息增加体检信息
                Form_PeopleAdd form_peopleadd = new Form_PeopleAdd();
                string errmsg = "";
                form_peopleadd.Add_jktj(dr["D_GRDABH"].ToString(), dr["D_SFZH"].ToString().Trim(), dr["D_XM"].ToString(), dr["XBName"].ToString(), dr["D_CSRQ"].ToString(), dr["nl"].ToString(), "", "", "", ref errmsg);

                //登记
                AddFormBll addformbll = new AddFormBll();
                //DataTable dt_dengji = (DataTable)dataGridView_list.DataSource;

                dr["ztName"] = "已登记";
                dr["zt"] = "1";
                dr["TJSJ"] = DateTime.Now.ToString("yyyy-MM-dd");
                dr["czy"] = UserInfo.userId;

                //数据库操作标志
                bool IsDbOk = true;
                // 1:按照旧号码打印 2:按照新号码打印  3:不打印
                if (dr["sxh_dj"] == DBNull.Value)
                {
                    //dr["sxh_dj"] = textBox_sxh_dj.Text;
                    dr["sxh_dj"] = textBox_sxh_dj.Text;
                    dr["yljgbm"] = UserInfo.Yybm;
                    dt_tem.AcceptChanges();
                    dr.SetAdded();
                    IsDbOk = addformbll.Add(dt_tem, "SQL063_TJZT_INSERT");
                    textBox_sxh_dj.Text = (int.Parse(textBox_sxh_dj.Text) + 1).ToString();
                }
                else if (ifTmDy.Equals("2"))
                {
                    dr["sxh_dj"] = textBox_sxh_dj.Text;
                    dr["yljgbm"] = UserInfo.Yybm;
                    dt_tem.AcceptChanges();
                    if (dr.RowState == DataRowState.Unchanged)
                    {
                        dr.SetModified();
                    }
                    IsDbOk = addformbll.Upd(dt_tem, "SQL063_TJZT_UPDATE");
                    textBox_sxh_dj.Text = (int.Parse(textBox_sxh_dj.Text) + 1).ToString();
                }
                else if (ifTmDy.Equals("1"))
                {
                    if (dr.RowState == DataRowState.Unchanged)
                    {
                        dr.SetModified();
                    }
                    IsDbOk = addformbll.Upd(dt_tem, "SQL063_TJZT_UPDATE");
                }


                if (IsDbOk == true)
                {
                    string xbmc = dr["XBName"] != null ? dr["XBName"].ToString() : "";
                    string printflag = System.Configuration.ConfigurationSettings.AppSettings["printhideset"];
                    string[] printSfzhNo;
                    string str_sfzh = dr["D_SFZH"].ToString();
                    if (!string.IsNullOrEmpty(printflag))
                    {
                        if (printflag != "0")
                        {
                            printSfzhNo = printflag.Split(',');
                            if (printSfzhNo != null && printSfzhNo.Length > 0)
                            {
                                foreach (string s in printSfzhNo)
                                {
                                    str_sfzh = str_sfzh.Remove(int.Parse(s) - 1, 1);
                                    str_sfzh = str_sfzh.Insert(int.Parse(s) - 1, "*");
                                }
                            }
                        }
                    }

                    bool result = printDemo.printBarCode(getPrintBarCode(dr["D_GRDABH"].ToString(), dr["sxh_dj"].ToString(), dr["D_XM"].ToString(), str_sfzh, dr["nl"].ToString(), xbmc), Code128.Encode.EAN128);
                    if (result == false)
                    {
                        throw new Exception("打印错误请重新打印！");
                        //MessageBox.Show("打印错误请重新打印！");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("条码打印错误" + ex.Message);
                // MessageBox.Show("条码打印错误"+ex.Message );
            }
        }


        /// <summary>
        /// 获取BarCode打印列表
        /// </summary>
        /// <returns></returns>
        public ArrayList getPrintBarCode(string jkdah, string sxh, string name, string sfzh, string nl, string xbmc)
        {
            AddFormBll addformbll = new AddFormBll();

            string sqlWhere = "";
            if (nl.Length > 0 && Convert.ToInt32(nl) >= 65)
            {
                sqlWhere = string.Format(" and jkdah='{0}' and YLJGBM='{1}' and (iflnr='1' or iflnr is null or iflnr='') ", jkdah, UserInfo.Yybm);
            }
            else
            {
                sqlWhere = string.Format(" and jkdah='{0}' and YLJGBM='{1}'  and (iflnr='0' or iflnr is null or iflnr='') ", jkdah, UserInfo.Yybm);
            }

            DataTable dt_tm = addformbll.GetMoHuList(sqlWhere, "sql_select_tjry_txm");
            ArrayList TmList = new ArrayList();
            if (dt_tm != null && dt_tm.Rows.Count > 0)
            {
                for (int i = 0; i < dt_tm.Rows.Count; i++)
                {
                    int strlength = 28 - (dt_tm.Rows[i]["TMMC"].ToString().Length * 2);
                    if (strlength <= 0)
                    {
                        strlength = 0;
                    }
                    TmList.Add(sxh.PadRight(4, ' ') + dt_tm.Rows[i]["TMMC"].ToString().PadRight(strlength, ' ') + nl + "岁" + "|" + name.PadRight(4, ' ') + xbmc + " " + sfzh + "|" + dt_tm.Rows[i]["txmbh"].ToString());
                }
            }

            return TmList;
        }

        #endregion

        #region 行号处理
        /// <summary>
        /// 行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_list_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_list.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView_list.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView_list.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private GraphicsPath GetGraphicsPath(Rectangle rc, int r)
        {
            int x = rc.X, y = rc.Y, w = rc.Width, h = rc.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, r, r, 180, 90);                //
            path.AddArc(x + w - r, y, r, r, 270, 90);            //
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90);        //
            path.AddArc(x, y + h - r, r, r, 90, 90);            //
            path.CloseFigure();
            return path;
        }

        private void panelContaioner_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rc = new Rectangle(0 + 2, 0 + 2, this.Width - 10, this.Height - 5);
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush, 1);
            g.DrawPath(pen, this.GetGraphicsPath(rc, 20));
        }
        #endregion

        #region 身份证号处理

        /// <summary>
        /// 读取身份证号并检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_redsfz_Click(object sender, EventArgs e)
        {
            //读取身份证号
            DataTable dtsfzh = form_readsfzh.readSfzh();
            if (dtsfzh != null && dtsfzh.Rows.Count > 0)
            {
                textBox_sfzh.Text = dtsfzh.Rows[0]["身份证号"].ToString().Trim();
                label_csrq.Text = dtsfzh.Rows[0]["出生日期"].ToString().Trim();
                label_xm.Text = dtsfzh.Rows[0]["姓名"].ToString().Trim();
                label_xb.Text = dtsfzh.Rows[0]["性别"].ToString().Trim();

                string message = "";
                if (checksfz_nl(out message) == false)
                {
                    MessageBox.Show(message);
                }

                string errMsg = "";
                string sqlWhere = "";

                //身份证号
                if (textBox_sfzh.Text.Length > 0)
                {
                    sqlWhere = sqlWhere + " and ( D_SFZH like '%" + textBox_sfzh.Text + "%' or  D_SFZH like '%" + textBox_sfzh.Text.ToLower() + "%')";
                }

                //按照读物的身份证号查询人员信息
                bool selectResult = getRyxxByWhere(sqlWhere, ref errMsg);

                //人员信息不存在  按照一代身份证号查询信息 或者姓名
                /*if (selectResult == false)
                {
                    DialogResult dr = MessageBox.Show("未查询到人员信息，是否按照 [姓名] 或 [转换成一代身份证号] 重新查询？", "信息", MessageBoxButtons.OKCancel);
                     if (dr == DialogResult.OK)//如果点击“确定”按钮
                     {

                         string strsfzh_15 = textBox_sfzh.Text.Substring(0, 6) + textBox_sfzh.Text.Substring(8, 9);
                         string strsfzh_17 = textBox_sfzh.Text.Substring(0, 17);

                         string strxm= label_xm.Text.Length >0? label_xm.Text:"没有姓名";
                         sqlWhere = string.Format(" and  ( D_SFZH like '%{0}%' or  D_SFZH like '%{1}%' or D_XM like '%{2}%' ) ", strsfzh_15, strsfzh_17, strxm);
                          selectResult = getRyxxByWhere(sqlWhere, ref errMsg);
                     }
                }*/

                //按照身份证号取得信息
                if (selectResult == false)
                {
                    DialogResult dr = MessageBox.Show("没有查询到人员信息，是否添加新建人员？", "信息", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)//如果点击“确定”按钮
                    {
                        Form_PeopleAdd form_peopleadd = new Form_PeopleAdd();
                        //form_peopleadd.tjbh = textBox_TJBH.Text;
                        form_peopleadd.czbm = comboBox_cunzhuang.SelectedValue != null ? comboBox_cunzhuang.SelectedValue.ToString() : "";
                        form_peopleadd.peopleAddType = "1";//0:体检是增加  1:登记时增加    
                        form_peopleadd.jkdah = textBox_jkda_tem.Text.Trim();
                        form_peopleadd.sfzh = textBox_sfzh.Text.Trim();
                        form_peopleadd.csrq = label_csrq.Text.Trim();
                        form_peopleadd.xm = label_xm.Text.Trim();
                        form_peopleadd.xb_mc = dtsfzh.Rows[0]["性别"].ToString().Trim();
                        form_peopleadd.Owner = this;
                        form_peopleadd.ShowDialog();
                    }
                }
                else
                {
                    //姓名验证
                    if (checkBox_xmcheck.Checked == true && dt_tem != null && dt_tem.Rows.Count > 0)
                    {
                        if (dt_tem.Rows.Count == 1)
                        {
                            if (dt_tem.Rows[0]["D_XM"] != DBNull.Value && dt_tem.Rows[0]["D_XM"] != null && dt_tem.Rows[0]["D_XM"].ToString().Length > 0)
                            {
                                //档案中的姓名
                                string xm_str = dt_tem.Rows[0]["D_XM"].ToString().Trim();
                                if (!label_xm.Text.Trim().Equals(xm_str))
                                {
                                    //档案中的姓名与身份证的姓名不符
                                    peopleXmUpdate peoplexmupdate = new peopleXmUpdate();
                                    peoplexmupdate.Owner = this;
                                    peoplexmupdate.jkdah = dt_tem.Rows[0]["D_GRDABH"].ToString().Trim();
                                    peoplexmupdate.sfzh = textBox_sfzh.Text.Trim();
                                    peoplexmupdate.xm_da = xm_str;
                                    peoplexmupdate.xm_sfz = label_xm.Text.Trim();
                                    peoplexmupdate.lxdh = dt_tem.Rows[0]["D_LXDH"].ToString().Trim();
                                    peoplexmupdate.Show();
                                    //MessageBox.Show("档案中的姓名与身份证的姓名不符！");
                                }
                            }
                            else
                            {
                                //档案中没有姓名
                                MessageBox.Show("档案中没有录入姓名！");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证身份证是否符合年龄
        /// </summary>
        /// <returns></returns>
        public bool checksfz_nl(out string message)
        {
            message = "";
            if (checkBox_nlcheck.Checked == false)
            {
                return true;
            }

            if (label_csrq.Text.Length > 0)
            {
                //当年度年底
                DateTime now_year = Convert.ToDateTime(string.Format("{0}-12-31", DateTime.Now.Year.ToString())).AddYears(-65);
                DateTime csrq = Convert.ToDateTime(label_csrq.Text);
                //年龄
                nl = (DateTime.Now.Year - csrq.Year).ToString();
                if (csrq > now_year)
                {

                    message = string.Format("出生日期为:{0} 到本年年底:{1} 还不满65岁！", csrq.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 手动输入身份证号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_sfzh_MouseLeave(object sender, EventArgs e)
        {
            if (textBox_sfzh.Text.Length > 0)
            {
                //如果是合法的身份证取信息
                if (Common.CheckIDCard(textBox_sfzh.Text.Trim()) == true)
                {
                    label_csrq.Text = Common.GetBirthdayByIdentityCardId(textBox_sfzh.Text.Trim(), true);
                    string message = "";
                    if (checksfz_nl(out message) == false)
                    {
                        MessageBox.Show(message);
                    }
                }
            }
            else
            {
                label_csrq.Text = "";
            }
        }

        #endregion

        #region 创建建议档案处理

        ///// <summary>
        ///// 创建一份简易的档案
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private bool createRozlx(DataTable dt_sfz, ref string ErrMsg)
        //{
        //    try
        //    {
        //        ErrMsg = "";

        //        //创建人口资料学表结构
        //        DataTable dt_T_DA_JKDA_RKXZL = initTable("sql056");
        //        dt_T_DA_JKDA_RKXZL.Rows.Add();

        //        //健康档案号  健康档案人口学资料（T_DA_JKDA_RKXZL） 按照时间自动生成
        //        string D_GRDABH = DateTime.Now.ToString("MMddhhmmssff");
        //        dt_T_DA_JKDA_RKXZL.Rows[0]["D_GRDABH"] = D_GRDABH;

        //        //档案状态 健康档案人口学资料（T_DA_JKDA_RKXZL）
        //        dt_T_DA_JKDA_RKXZL.Rows[0]["D_DAZT"] = "1";

        //        //姓名 健康档案人口学资料（T_DA_JKDA_RKXZL）
        //        string name_bh = "";
        //        if (dt_sfz.Rows[0]["D_XM"] == null || (dt_sfz.Rows[0]["D_XM"] != null && dt_sfz.Rows[0]["D_XM"].ToString().Length == 0))
        //        {
        //            name_bh = Microsoft.VisualBasic.Interaction.InputBox("请输入姓名", "信息录入", "姓名", 100, 100);
        //        }
        //        else
        //        {
        //            name_bh = dt_sfz.Rows[0]["姓名"].ToString();
        //        }

        //        dt_T_DA_JKDA_RKXZL.Rows[0]["D_XM"] = name_bh;

        //        //性别 健康档案人口学资料（T_DA_JKDA_RKXZL）
        //        dt_T_DA_JKDA_RKXZL.Rows[0]["D_XB"] = dt_sfz.Rows[0]["XBName"].ToString().Equals("男") ? "1" : "2";

        //        //证件编号 身份证号 健康档案人口学资料（T_DA_JKDA_RKXZL）
        //        string sfzh_bh = "";
        //        if (dt_sfz.Rows[0]["D_SFZH"] == null || (dt_sfz.Rows[0]["D_SFZH"] != null && dt_sfz.Rows[0]["D_SFZH"].ToString().Length == 0))
        //        {
        //            sfzh_bh = Microsoft.VisualBasic.Interaction.InputBox("请输入身份证号", "信息录入", "身份证号", 100, 100);
        //        }
        //        else
        //        {
        //            sfzh_bh = dt_sfz.Rows[0]["D_SFZH"].ToString();
        //        }

        //        dt_T_DA_JKDA_RKXZL.Rows[0]["D_SFZH"] = sfzh_bh;

        //        //出生日期    健康档案人口学资料（T_DA_JKDA_RKXZL）
        //        dt_T_DA_JKDA_RKXZL.Rows[0]["D_CSRQ"] = Convert.ToDateTime(dt_sfz.Rows[0]["D_CSRQ"].ToString()).ToString("yyyy-MM-dd");

        //        //标识该条数据是否进行过修改。1：未修改  2：已修改  3：新增
        //        dt_T_DA_JKDA_RKXZL.Rows[0]["zt"] = "3";

        //        //增量标
        //        dt_T_DA_JKDA_RKXZL.Rows[0]["zlbz"] = "1";

        //        //创建者姓名
        //        dt_T_DA_JKDA_RKXZL.Rows[0]["CREATEUSER"] = UserInfo.userId;

        //        //创建时间
        //        dt_T_DA_JKDA_RKXZL.Rows[0]["CREATETIME"] = DateTime.Now.ToString("yyyy-MM-dd");

        //        Form_daxqBll form_daxqbll = new Form_daxqBll();

        //        //年度
        //        dt_T_DA_JKDA_RKXZL.Rows[0]["nd"] = DateTime.Now.Year.ToString();

        //        //新增
        //        form_daxqbll.Add(dt_T_DA_JKDA_RKXZL, "sql067");


        //        //加入体检
        //        string errMsg = "";
        //        string result = Add_jktj(D_GRDABH, dt_sfz, ref errMsg);
        //        if (result.Equals("1") == false)
        //        {
        //            ErrMsg = errMsg;
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrMsg = ex.Message;
        //        return false;
        //    }

        //}

        ///// <summary>
        ///// 初始化表结构
        ///// </summary>
        ///// <param name="dt_table"></param>
        ///// <returns></returns>
        //public DataTable initTable(string sqlcode)
        //{
        //    DataTable dt = new DataTable();
        //    Form_daxqBll form_daxq = new Form_daxqBll();
        //    //获取数据库表结构
        //    dt = form_daxq.GetMoHuList("and 1=2", sqlcode);
        //    return dt;
        //}

        ///// <summary>
        ///// 生成体检
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //public string Add_jktj(string grdabh, DataTable dt_ryxx, ref string errMsg)
        //{
        //    Form_daxqBll form_daxqbll = new Form_daxqBll();
        //    try
        //    {

        //        string message = "";

        //        //生成体检人员信息
        //        DataTable dt_tjryxx = new DataTable();
        //        dt_tjryxx = form_daxqbll.GetMoHuList(string.Format(" and JKDAH='{0}' ", grdabh), "sql069");

        //        //已经存在
        //        if (dt_tjryxx != null && dt_tjryxx.Rows.Count > 0)
        //        {
        //            errMsg = "已经存在体检信息！";
        //            return "0";
        //        }

        //        dt_tjryxx.Rows.Add();
        //        dt_tjryxx.Rows[0]["YLJGBM"] = UserInfo.Yybm;//医疗机构编码
        //        dt_tjryxx.Rows[0]["TJJHBM"] = DateTime.Now.ToString("yyyyMMdd");//体检计划编码
        //        dt_tjryxx.Rows[0]["TJPCH"] = DateTime.Now.ToString("hhmmss");//体检批次号

        //        //取得顺番号
        //        DataTable dt_SFH = form_daxqbll.GetMoHuList("", "sql076");
        //        if (dt_SFH != null && dt_SFH.Rows.Count > 0 && dt_SFH.Rows[0]["SFH"] != null && dt_SFH.Rows[0]["SFH"].ToString().Length > 0)
        //        {
        //            dt_tjryxx.Rows[0]["SFH"] = int.Parse(dt_SFH.Rows[0]["SFH"].ToString()) + 1;//顺番号
        //        }
        //        else
        //        {
        //            dt_tjryxx.Rows[0]["SFH"] = "0";//顺番号
        //        }

        //        //dt_tjryxx.Rows[0]["SFH"] = dt_tjryxx.Rows[0]["SFH"].ToString();//顺番号
        //        ////取得顺序号码
        //        //DataTable dt_SXHM = form_daxqbll.GetMoHuList(string.Format(" and YLJGBM='{0}' and CZBM='{1}'", UserInfo.Yybm, comboBox_D_JWH.SelectedValue != null ? comboBox_D_JWH.SelectedValue.ToString() : ""), "sql072");
        //        //if (dt_SXHM != null && dt_SXHM.Rows.Count > 0)
        //        //{
        //        //    dt_tjryxx.Rows[0]["SXHM"] = (int.Parse(dt_SXHM.Rows[0]["SXHM"].ToString()) + 1).ToString();//顺序号码
        //        //}
        //        //else
        //        //{
        //        //    dt_tjryxx.Rows[0]["SXHM"] = "1";//顺序号码
        //        //}

        //        dt_tjryxx.Rows[0]["SXHM"] = "0";

        //        string strTJBM = grdabh.PadLeft(12, '0');
        //        dt_tjryxx.Rows[0]["TJBM"] = strTJBM.Substring(strTJBM.Length - 12);//个人体检编号
        //        dt_tjryxx.Rows[0]["JKDAH"] = strTJBM;//个人健康档案号
        //        dt_tjryxx.Rows[0]["GRBM"] = "0";//个人编码
        //        dt_tjryxx.Rows[0]["XM"] = dt_ryxx.Rows[0]["D_XM"].ToString();// lTextBox_D_XM.Text;//姓名
        //        dt_tjryxx.Rows[0]["XB"] = dt_ryxx.Rows[0]["XBName"].ToString().Equals("男") ? "1" : "2";// comboBox_D_XB.SelectedValue != null ? comboBox_D_XB.SelectedValue.ToString() : "";//性别
        //        dt_tjryxx.Rows[0]["SFZH"] = dt_ryxx.Rows[0]["D_SFZH"].ToString();//身份证号
        //        //dt_tjryxx.Rows[0]["LXDH"] = lTextBox_D_LXDH.Text;//联系电话
        //        dt_tjryxx.Rows[0]["CSRQ"] = Convert.ToDateTime(dt_ryxx.Rows[0]["D_CSRQ"].ToString()).ToString("yyyy-MM-dd");//dateTimePicker_D_CSRQ.Value.ToString("yyyy-MM-dd");//出生日期
        //        //dt_tjryxx.Rows[0]["CZBM"] = comboBox_D_JWH.SelectedValue != null ? comboBox_D_JWH.SelectedValue.ToString() : "";//村庄编码
        //        //dt_tjryxx.Rows[0]["TJZT"] = "2";//体检状态
        //        //dt_tjryxx.Rows[0]["TJSJ"] = "";//体检时间
        //        //dt_tjryxx.Rows[0]["TJFZR"] = "";//体检负责人
        //        dt_tjryxx.Rows[0]["FL"] = "2";//体检人员分类
        //        dt_tjryxx.Rows[0]["BZ"] = "";//备注
        //        dt_tjryxx.Rows[0]["TJBH_TEM"] = strTJBM;//临时个人体检编号
        //        dt_tjryxx.Rows[0]["CREATETIME"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//创建时间
        //        dt_tjryxx.Rows[0]["CREATEUSER"] = UserInfo.userId;//创建者
        //        dt_tjryxx.Rows[0]["UPDATETIME"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//更新时间
        //        dt_tjryxx.Rows[0]["UPDATEUSER"] = UserInfo.userId;//更新者
        //        dt_tjryxx.Rows[0]["SCZT"] = "2";//数据上传状态
        //        //dt_tjryxx.Rows[0]["LISZT"] = "2";//是否已经同步过LIS数据
        //        //dt_tjryxx.Rows[0]["TJZT_zytz"] = "2";//体检状态中医体质
        //        //dt_tjryxx.Rows[0]["TJSJ_zytz"] = "";//体检时间
        //        //dt_tjryxx.Rows[0]["TJZT_jkzp"] = "2";//体检状态生活自理能力
        //        //dt_tjryxx.Rows[0]["TJSJ_jkzp"] = "";//体检时间
        //        dt_tjryxx.Rows[0]["ZLBZ"] = "1";//增量标志
        //        dt_tjryxx.Rows[0]["nd"] = DateTime.Now.Year.ToString();//年度
        //        //dt_tjryxx.Rows[0]["djzt"] = "0";//登记
        //        dt_tjryxx.Rows[0]["ISSH"] = "0";//登记
        //        dt_tjryxx.Rows[0]["ISNEWDOC"] = "0";//登记

        //        //dt_tjryxx.Rows[0]["PRID"] = comboBox_ssjg.SelectedValue != null ? comboBox_ssjg.SelectedValue.ToString() : "";//所属机构

        //        //增加体检人员信息
        //        form_daxqbll.Add(dt_tjryxx, "sql_add_people");

        //        //生成lis信息
        //        DataTable dt_tmList = new DataTable(); //sql030
        //        dt_tmList = form_daxqbll.GetMoHuList(string.Format(" and  YLJGBM ='{0}' and SFDY='1' and fl ='0' ", UserInfo.Yybm), "sql030");

        //        if (dt_tmList != null && dt_tmList.Rows.Count > 0)
        //        {
        //            //主表
        //            DataTable dt_reqmain = new DataTable();
        //            dt_reqmain = form_daxqbll.GetMoHuList(" and 1=2 ", "sql070");
        //            //明细表
        //            DataTable dt_reqdetail = new DataTable();
        //            dt_reqdetail = form_daxqbll.GetMoHuList(" and 1=2 ", "sql071");

        //            for (int i = 0; i < dt_tmList.Rows.Count; i++)
        //            {
        //                string sqh = grdabh.ToString().PadLeft(12, '0') + dt_tmList.Rows[i]["TMBM"].ToString();
        //                sqh = sqh.Substring(sqh.Length - 14, 14);

        //                dt_reqmain.Rows.Add();
        //                //主表
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sqh"] = sqh; //申请号
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ksdh"] = "";//送检科室代码
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sqys"] = "";//申请医生代码
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sqsj"] = DateTime.Now.ToString("yyyy-MM-dd");//申请时间
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jsys"] = "";//接收医生
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jssj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//接收时间
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zt"] = "1";//状态
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jjzt"] = "1";//计价状态
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brly"] = "4";//病人来源
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brdh"] = grdabh.ToString();//病历号
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brxm"] = dt_ryxx.Rows[0]["D_XM"].ToString();// lTextBox_D_XM.Text;//病人姓名
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brxb"] = dt_ryxx.Rows[0]["XBName"].ToString().Equals("男") ? "1" : "2";// comboBox_D_XB.SelectedValue != null ? comboBox_D_XB.SelectedValue.ToString() : "";//病人性别
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["brsr"] = Convert.ToDateTime(dt_ryxx.Rows[0]["D_CSRQ"].ToString()).ToString("yyyy-MM-dd"); //dateTimePicker_D_CSRQ.Value.ToString("yyyy-MM-dd");//病人生日
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bz1"] = "";
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bz2"] = "";
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bz3"] = "";
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jzbz"] = "0";//急诊标志
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["txm"] = "";
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ch"] = "";//床号
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["yblx"] = "";//样本类型
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zxys"] = "";//执行医生
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zxsj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//执行时间
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["bgddh"] = "";
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["costs"] = 0;
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["nl"] = 0;//年龄
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["nldw"] = "";//年龄单位
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zd"] = "";//临床诊断
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["cysj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//采样时间
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["cksj"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ckzj"] = "";
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["ckyh"] = "";
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["sfzh"] = dt_ryxx.Rows[0]["D_SFZH"].ToString();//身份证号
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["jkdah"] = grdabh;// lTextBox_D_GRDABH.Text.ToString();//健康档案号
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["yybm"] = UserInfo.Yybm;//医院编码
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["dataFrom"] = "1";//数据来源
        //                dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["zlbz"] = "1";//增量标志
        //                //dt_reqmain.Rows[dt_reqmain.Rows.Count - 1]["nd"] = DateTime.Now.Year.ToString() ;//年度

        //                //生成明细
        //                string tmfl = dt_tmList.Rows[i]["sqxmdh"] != null ? dt_tmList.Rows[i]["sqxmdh"].ToString() : "";
        //                DataTable dt_tmList_reqdetail = form_daxqbll.GetMoHuList(string.Format(" and  YLJGBM ='{0}' and SQXMDH='{1}' ", UserInfo.Yybm, tmfl), "sql078");
        //                if (dt_tmList_reqdetail != null)
        //                {
        //                    for (int j = 0; j < dt_tmList_reqdetail.Rows.Count; j++)
        //                    {
        //                        dt_reqdetail.Rows.Add();
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sqh"] = sqh; //申请号
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["xh"] = (j + 1).ToString();//序号
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sqxmdh"] = dt_tmList_reqdetail.Rows[j]["ITEM_CODE"].ToString();//申请项目代码
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sqxmmc"] = dt_tmList_reqdetail.Rows[j]["ITEM_NAME"].ToString();//申请项目名称
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["sl"] = "1";//数量
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dj"] = "0";//单价
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["zt"] = "1";//状态
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["jjzt"] = "1";//计价状态
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["bz1"] = "";//备注1字段
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["bz2"] = "";//
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["costs"] = 0;//
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["numbk1"] = 0;//
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["numbk2"] = 0;//
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dtbk1"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dtbk2"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["yybm"] = UserInfo.Yybm;//医院编码
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["dataFrom"] = "1";//数据来源
        //                        dt_reqdetail.Rows[dt_reqdetail.Rows.Count - 1]["zlbz"] = "1";//增量标志
        //                    }
        //                }
        //            }

        //            //检验主表
        //            form_daxqbll.Add(dt_reqmain, "sql074");

        //            //检验明细表
        //            form_daxqbll.Add(dt_reqdetail, "sql075");

        //        }
        //        return "1";
        //    }
        //    catch (Exception ex)
        //    {
        //        errMsg = ex.Message;
        //        return "-1";
        //    }
        //}

        #endregion

        #region 打开新建档案窗体
        /// <summary>
        /// 新建档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_new_Click(object sender, EventArgs e)
        {
            FrmQmLisOutput frm = new FrmQmLisOutput();
            frm.Show();
            //Form_daxq_New form = new Form_daxq_New();
            //form.Owner = this;

            ////DataRowView dr = (DataRowView)dataGridView_list.Rows[e.RowIndex].DataBoundItem;
            //if (comboBox_cunzhuang.SelectedValue != null && comboBox_cunzhuang.SelectedValue.ToString().Length > 0)
            //{
            //    form.czbm = comboBox_cunzhuang.SelectedValue.ToString();
            //}
            ////村庄编码
            //if (comboBox_cunzhuang.SelectedValue != null)
            //{
            //    form.czbm = comboBox_cunzhuang.SelectedValue.ToString();
            //}

            //form.ShowDialog();

        }

        #endregion

        #region 通用处理
        /// <summary>
        /// Enter转换为tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Tab_KeyDown(object sender, KeyEventArgs e)
        {
            //Enter转换为tab
            if (sender.GetType().ToString().Equals("System.Windows.Forms.CheckBox"))
            {
                if (e.KeyCode == Keys.Enter)
                {
                    CheckBox checkBox = (CheckBox)sender;
                    if (checkBox.Checked == true)
                    {
                        checkBox.Checked = false;
                    }
                    else
                    {
                        checkBox.Checked = true;
                    }
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }

            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }

            }

            if (sender.GetType().ToString().Equals("System.Windows.Forms.ComboBox"))
            {
                if (e.KeyCode == Keys.Left)
                {
                    //shift+Tab
                    SendKeys.Send("+{Tab}");
                    e.Handled = false;
                }
                if (e.KeyCode == Keys.Right)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
            }
            else
            {

                if (e.KeyCode == Keys.Up)
                {
                    //shift+Tab
                    SendKeys.Send("+{Tab}");
                    e.Handled = false;
                }
                if (e.KeyCode == Keys.Down)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = false;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                e.Handled = false;
            }

        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_close_Click(object sender, EventArgs e)
        {
            this.Owner.Visible = true;
            this.Close();
        }
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

        /// <summary>
        /// 字体设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                FontDialog fontDialog = new FontDialog();
                fontDialog.Color = dataGridView_list.RowTemplate.DefaultCellStyle.ForeColor;
                fontDialog.Font = dataGridView_list.RowTemplate.DefaultCellStyle.Font;
                fontDialog.AllowScriptChange = true;
                if (fontDialog.ShowDialog() != DialogResult.Cancel)
                {
                    dataGridView_list.RowTemplate.DefaultCellStyle.Font = fontDialog.Font;//将当前选定的文字改变字体
                }
                BindDgv();
            }
            catch { }
        }

        /// <summary>
        /// 未登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_wdj_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_wdj.Checked == true)
            {
                checkBox_ydj.Checked = false;
            }
        }

        /// <summary>
        /// 已登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_ydj_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ydj.Checked == true)
            {
                checkBox_wdj.Checked = false;
            }
        }

        /// <summary>
        /// 导出体检名单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_tjmd_Click(object sender, EventArgs e)
        {
            //导出名单
            if (dt_tem != null && dt_tem.Rows.Count > 0)
            {
                DataRow[] dt_row = dt_tem.Select("select_ZT='1'");
                if (dt_row == null || dt_row.Length == 0) return;
                DataTable dt_export = dt_row[0].Table.Clone();  // 复制DataRow的表结构
                foreach (DataRow row in dt_row)
                {
                    if (row["diseases"] != null && row["diseases"] != DBNull.Value)
                    {
                        row["diseases"] = getDiseasesName(row["diseases"].ToString());
                    }
                    dt_export.ImportRow(row);  // 将DataRow添加到DataTable中
                }
                //排序
                DataView dv = dt_export.DefaultView;
                dv.Sort = " TJSJ Asc,sxh_dj Asc, D_XM asc";
                dt_export = dv.ToTable();


                for (int i = dt_export.Columns.Count - 1; i >= 0; i--)
                {
                    if (dt_export.Columns[i].ColumnName.ToLower().Equals("d_xm") == true)
                    {
                        dt_export.Columns[i].ColumnName = "姓名";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("xbname") == true)
                    {
                        dt_export.Columns[i].ColumnName = "性别";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("d_lxdh") == true)
                    {
                        dt_export.Columns[i].ColumnName = "联系电话";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("d_lxrdh") == true)
                    {
                        dt_export.Columns[i].ColumnName = "联系人电话";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("d_csrq") == true)
                    {
                        dt_export.Columns[i].ColumnName = "出生日期";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("d_sfzh") == true)
                    {
                        dt_export.Columns[i].ColumnName = "身份证号";
                    }
                    //else if (dt_export.Columns[i].ColumnName.ToLower().Equals("d_grdabh") == true)
                    //{
                    //    dt_export.Columns[i].ColumnName = "健康档案号";
                    //}
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("tjsj") == true)
                    {
                        dt_export.Columns[i].ColumnName = "日期";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("diseases") == true)
                    {
                        dt_export.Columns[i].ColumnName = "人群";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("sxh_dj") == true)
                    {
                        dt_export.Columns[i].ColumnName = "顺序号";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("nl") == true)
                    {
                        dt_export.Columns[i].ColumnName = "年龄";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("datazt") == true)
                    {
                        dt_export.Columns[i].ColumnName = "数据状态";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("names") == true)
                    {
                        dt_export.Columns[i].ColumnName = "医疗机构";
                    }
                    else if (dt_export.Columns[i].ColumnName.ToLower().Equals("b_name") == true)
                    {
                        dt_export.Columns[i].ColumnName = "村庄";
                    }
                    else
                    {
                        dt_export.Columns.RemoveAt(i);
                    }

                }

                dt_export.Columns["顺序号"].SetOrdinal(0);
                dt_export.Columns["日期"].SetOrdinal(1);
                dt_export.Columns["姓名"].SetOrdinal(2);
                dt_export.Columns["性别"].SetOrdinal(3);
                dt_export.Columns["年龄"].SetOrdinal(4);
                dt_export.Columns["联系电话"].SetOrdinal(5);
                dt_export.Columns["联系人电话"].SetOrdinal(6);
                dt_export.Columns["出生日期"].SetOrdinal(7);
                dt_export.Columns["身份证号"].SetOrdinal(8);
                dt_export.Columns["人群"].SetOrdinal(9);
                dt_export.Columns["医疗机构"].SetOrdinal(10);
                dt_export.Columns["村庄"].SetOrdinal(11);
                //dt_export.Columns["健康档案号"].SetOrdinal(9);
                dt_export.TableName = DateTime.Now.ToString("yyyy-MM-dd");

                //获取文件保存的路径
                string filePath = "";
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = "C:\\Users\\Administrator\\Desktop\\";
                sfd.Filter = "excel文件(*.xls)|*.xls|excel文件(*.xlsx)|*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    filePath = sfd.FileName;
                }


                string errmsg = "";
                bool exportResult = commonExcel.OutFileToDisk(dt_export, "体检人员名单", filePath, out errmsg);

                if (exportResult == true)
                {
                    MessageBox.Show("文件导出完成！");
                }


            }
            else
            {
                MessageBox.Show("请选择要导出的数据！");
            }

        }

        /// <summary>
        /// 条码打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_print_Click(object sender, EventArgs e)
        {
            // 1:按照旧号码打印 2:按照新号码打印  3:不打印
            string iftmdy = "2";

            DataRow[] dtRow = dt_tem.Select(" select_ZT='1' ");
            DataRow[] dtRowPrinted = dt_tem.Select(" select_ZT='1' and Zt='1' ");

            if (dtRow != null && dtRow.Length > 0)
            {
                //存在已经打印过的数据
                if (dtRowPrinted != null && dtRowPrinted.Length > 0)
                {
                    DialogResult a = MessageBox.Show("已经打印过的数据被选中，是否 [放弃] 原登记号重新生成登记号，打印条码?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (a == DialogResult.Yes)
                    {
                        // 1:按照旧号码打印 2:按照新号码打印  3:不打印
                        iftmdy = "2";
                    }
                    else if (a == DialogResult.No)
                    {
                        // 1:按照旧号码打印 2:按照新号码打印  3:不打印
                        iftmdy = "1";
                    }
                    else
                    {
                        // 1:按照旧号码打印 2:按照新号码打印  3:不打印
                        iftmdy = "3";
                        return;
                    }

                }

                //打印条码
                for (int i = 0; i < dtRow.Length; i++)
                {
                    printTm(dtRow[i], iftmdy);
                    //数据绑定
                    BindDgv();
                }
            }
            //数据绑定
            // BindDgv();

        }

        /// <summary>
        /// 更多查询条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_tj_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_tj.Checked == true)
            {
                panel_top.Height = panel_top.Height + 65;
                panel_tj.Height = 65;
            }
            else
            {
                panel_top.Height = panel_top.Height - 65;
                panel_tj.Height = 0;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_all_Click(object sender, EventArgs e)
        {
            if (button_select_all.Text == "全选")
            {
                if (dt_tem != null && dt_tem.Rows.Count > 0)
                {

                    for (int i = 0; i < dt_tem.Rows.Count; i++)
                    {
                        dt_tem.Rows[i]["select_ZT"] = true;
                    }
                    //显示数据
                    BindDgv();

                    button_select_all.Text = "全不选";
                }
            }
            else if (button_select_all.Text == "全不选")
            {
                if (dt_tem != null && dt_tem.Rows.Count > 0)
                {

                    for (int i = 0; i < dt_tem.Rows.Count; i++)
                    {
                        dt_tem.Rows[i]["select_ZT"] = false;
                    }

                    //显示数据
                    BindDgv();

                    button_select_all.Text = "全选";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form_txmSet ss = new Form_txmSet();
            ss.Show();
        }

        private void HealthExaminedUserInfoInit(DataRow dtRow)
        {

        }
        private void btn_pz_Click(object sender, EventArgs e)
        {
            //导出名单
            if (dt_tem != null && dt_tem.Rows.Count > 0)
            {
                //dt_tem.Select().Length
                //DataRow[] dt_row = dt_tem.Select("select_ZT='1'");

                DataGridViewSelectedRowCollection dt_row = dataGridView_list.SelectedRows;

                if (dt_row == null || dt_row.Count != 1)
                {
                    MessageBox.Show("请选中一个人员进行签名拍照。");
                    return;
                }
                //DataTable dt_export = dt_row[0].Table.Clone();  // 复制DataRow的表结构

                Form_photo form = new Form_photo();
                string jkdah = dt_row[0].Cells["D_GRDABH"].Value.ToString();
                string sfzh = dt_row[0].Cells["D_SFZH"].Value.ToString();
                string xm = dt_row[0].Cells["D_XM"].Value.ToString();
                string createDate = dt_row[0].Cells["TJSJ"].Value.ToString();

                if (string.IsNullOrEmpty(createDate))
                {
                    createDate = DateTime.Now.ToString("yyyy-MM-dd");
                    //MessageBox.Show("请先登记再进行签名拍照操作。");
                    //return;
                }

                form.jkdah = jkdah;
                form.sfzh = sfzh;
                TJClient.Signname.Operation.HealthExaminedUserInfoInit(xm, jkdah, sfzh, createDate);
                form.ShowDialog();
            }
        }

        /// <summary>
        /// 设定既往病史
        /// </summary>
        /// <param name="JKDAH"></param>
        /// <returns></returns>
        public string setJwbs(string JKDAH)
        {
            //label_jwbs.Text = "";

            jktjBll jktjbll = new jktjBll();
            string strjwbs = "";
            //按照档案号获取既往病史
            DataTable dt_jwbs = jktjbll.GetMoHuList(string.Format(" and d_grdabh='{0}' and d_jblx='疾病' ", JKDAH), "sql060");
            if (dt_jwbs != null && dt_jwbs.Rows.Count > 0)
            {
                DataTable dt_sjzd = jktjbll.GetMoHuList(string.Format(" and zdlxbm='jwsjb' "), "sql_select_sjzd");
                for (int i = 0; i < dt_jwbs.Rows.Count; i++)
                {
                    string jwsbm = dt_jwbs.Rows[i]["d_jbmc"] != null ? dt_jwbs.Rows[i]["d_jbmc"].ToString() : "";

                    string[] jwsbmlist = jwsbm.Split(new char[] { ',' });
                    if (jwsbmlist != null && jwsbmlist.Length > 0)
                    {
                        for (int j = 0; j < jwsbmlist.Length; j++)
                        {
                            DataRow[] dtrow = dt_sjzd.Select(string.Format(" zdbm='{0}'", jwsbmlist[j]));
                            if (dtrow != null && dtrow.Length > 0)
                            {
                                strjwbs = strjwbs + "  " + (dtrow[0]["zdmc"] != null ? dtrow[0]["zdmc"].ToString() : "");
                            }
                        }
                    }
                }
            }
            //label_jwbs.Text = string.Format("慢病人群:{0}", strjwbs.Length > 0 ? strjwbs : "无");
            return strjwbs;
        }

        /// <summary>
        /// 设定血型
        /// </summary>
        /// <param name="JKDAH"></param>
        /// <returns></returns>
        public void setXX(string JKDAH)
        {
            //label_jwbs.Text = "";

            jktjBll jktjbll = new jktjBll();
            string xxinfo = "";
            //按照档案号获取档案血型
            DataTable dt_xx = jktjbll.GetMoHuList(string.Format(" and d_grdabh='{0}' ", JKDAH), "sql_xx_select");
            if (dt_xx != null && dt_xx.Rows.Count > 0)
            {
                string xx = dt_xx.Rows[0]["d_xx"] != null ? dt_xx.Rows[0]["d_xx"].ToString() : " ";
                string rh = dt_xx.Rows[0]["d_sfrhyx"] != null ? dt_xx.Rows[0]["d_sfrhyx"].ToString() : " ";
                xxinfo = string.Format("血型:{0} RH阴性:{1} ", xx, rh);

            }
            else
            {
                xxinfo = string.Format("血型:{0} RH阴性:{1} ", "  ", "  ");
            }
            MessageBox.Show(xxinfo);
        }

        //折叠页面
        private void btn_zhedie_Click(object sender, EventArgs e)
        {
            if (btn_zhedie.Text == "展开")
            {
                btn_zhedie.Text = "折叠";
                panel_body2.Visible = true;
                btn_zhedie.Image = Resources.btn_left;
                btn_zhedie.ImageAlign = ContentAlignment.MiddleRight;
                btn_zhedie.TextAlign = ContentAlignment.MiddleLeft;

                isZhedie = true;
            }
            else if (btn_zhedie.Text == "折叠")
            {
                btn_zhedie.Text = "展开";
                panel_body2.Visible = false;
                btn_zhedie.Image = Resources.btn_right;
                btn_zhedie.ImageAlign = ContentAlignment.MiddleLeft;
                btn_zhedie.TextAlign = ContentAlignment.MiddleRight;

                isZhedie = false;
            }
        }
    }
}
