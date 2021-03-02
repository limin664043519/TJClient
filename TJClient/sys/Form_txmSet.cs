using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJClient.sys.Bll;
using TJClient.Common;
using FBYClient;

namespace TJClient.sys
{
    public partial class Form_txmSet : Form
    {
        public Form_txmSet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设定
        /// </summary>
        /// <param name="pym"></param>
        public bool setListData(object paraFromParent)
        {
            //setItemSelect(paraFromParent);
            return true;
        }

        /// <summary>
        /// 套餐项目设定
        /// </summary>
        public DataTable dt_tc_mx = null;


        public bool isinit = true;

        /// <summary>
        /// 窗体初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_txmSet_Load(object sender, EventArgs e)
        {
            listboxFormBll listbox = new listboxFormBll();
            isinit = true;
            //条码初始化
            DataTable dt_tm = listbox.GetMoHuList(string.Format("and YLJGBM='{0}'  order by TMBM ", UserInfo.Yybm), "sql030");
            if (dt_tm != null && dt_tm.Rows.Count > 0)
            {
                this.checkedListBox_tm.DataSource = dt_tm;
                this.checkedListBox_tm.ValueMember = "TMBM";
                this.checkedListBox_tm.DisplayMember = "TMMC";
            }

            //条码明细初始化
            DataTable dt_mx = listbox.GetMoHuList(string.Format("and yybm='{0}'  order by item_code ", UserInfo.Yybm), "sql_t_jk_lisitems_select");
            if (dt_mx != null && dt_mx.Rows.Count > 0)
            {
                this.checkedListBox_MX.DataSource = dt_mx;
                this.checkedListBox_MX.ValueMember = "item_code";
                this.checkedListBox_MX.DisplayMember = "item_name";
            }

            //套餐初始化
            DataTable dt_TC = listbox.GetMoHuList(string.Format("and yybm = '{0}' order by  barcode ", UserInfo.Yybm), "sql_t_jk_barcodeclass_select");
            if (dt_TC != null && dt_TC.Rows.Count > 0)
            {
                this.checkedListBox_TC.DataSource = dt_TC;
                this.checkedListBox_TC.ValueMember = "BARCODE";
                this.checkedListBox_TC.DisplayMember = "BARNAME";

                for (int i = 0; i < dt_TC.Rows.Count; i++)
                {
                    string BARCODE = dt_TC.Rows[i]["BARCODE"].ToString();
                    string ISDEFAUT = dt_TC.Rows[i]["ISDEFAUT"]!=null?dt_TC.Rows[i]["ISDEFAUT"].ToString ():"";
                    if (ISDEFAUT.Equals("1") == true)
                    {
                        //设定套餐选中状态
                        setItemSelectTc(BARCODE);

                        //设定条码选中状态
                        setItemSelectTm(BARCODE);

                        //设定条码明细
                        setItemSelectTm();

                        break;
                    }
                }
            }
            isinit = false;
          

        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_true_Click(object sender, EventArgs e)
        {
            try
            {
                sysCommonForm owerForm = (sysCommonForm)this.Owner;
                owerForm.setParentFormDo(dt_tc_mx);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_return_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 组合选中的村庄编码
        /// </summary>
        /// <returns></returns>
        public string getSelectList()
        {
            //int listCount = 0;
            //DataTable dt = (DataTable)checkedListBox_cz.DataSource;
            //string strCollected = string.Empty;
            //for (int i = 0; i < checkedListBox_cz.Items.Count; i++)
            //{
            //    if (checkedListBox_cz.GetItemChecked(i)==true)
            //    {
            //        listCount++;
            //        if (strCollected == string.Empty)
            //        {
            //            //strCollected = checkedListBox_cz.GetItemText(checkedListBox_cz.Items[i]);
            //            strCollected = dt.Rows[i]["czbm"].ToString();
            //        }
            //        else
            //        {
            //            //strCollected = strCollected + "|" + checkedListBox_cz.GetItemText(checkedListBox_cz.Items[i]);
            //            strCollected = strCollected + "," + dt.Rows[i]["czbm"].ToString();
            //        }
            //    }
            //}
            //return strCollected + "|" + listCount.ToString() ;
            return "";
        }

        /// <summary>
        /// 设定选中套餐
        /// </summary>
        /// <param name="selectvalue"></param>
        public void setItemSelectTc(object selectvalue)
        {
            if (selectvalue == null || selectvalue.ToString().Length == 0)
            {
                return;
            }

            DataTable dt = (DataTable)checkedListBox_TC.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            string[] selectList = selectvalue.ToString().Split(new char[] { ',' });
            for (int i = 0; i < selectList.Length; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                    if (selectList[i].Equals(dt.Rows[j]["BARCODE"].ToString()))
                    {
                        checkedListBox_TC.SetItemChecked(j, true);
                        break;
                    }
            }
        }

        /// <summary>
        /// 设定选中条码
        /// </summary>
        /// <param name="BARCODE">套餐编码</param>
        public void setItemSelectTm(string BARCODE)
        {
            listboxFormBll listbox = new listboxFormBll();
            //条码初始化
            DataTable dt = (DataTable)checkedListBox_tm.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    checkedListBox_tm.SetItemCheckState(i, System.Windows.Forms.CheckState.Unchecked );
                }
            }


            if (BARCODE == null || BARCODE.Length == 0)
            {
                return;
            }
            
            //条码初始化
            DataTable dt_tm_tc = listbox.GetMoHuList(string.Format("and yybm='{0}' and barcode='{1}' order by bar_code ", UserInfo.Yybm, BARCODE), "sql_t_jk_barcodeclass_barcodeitem_select");
            
            //string[] selectList = selectvalue.ToString().Split(new char[] { ',' });
            if (dt_tm_tc == null || dt_tm_tc.Rows.Count == 0)
            {
                return;
            }
            for (int i = 0; i < dt_tm_tc.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)

                    if (dt_tm_tc.Rows[i]["BAR_CODE"].Equals(dt.Rows[j]["tmbm"].ToString()))
                    {
                        checkedListBox_tm.SetItemChecked(j, true);
                        break;
                    }
            }
        }

        /// <summary>
        /// 设定条码明细
        /// </summary>
        /// <param name="BARCODE">套餐编码</param>
        public void setItemSelectTm()
        {
            listboxFormBll listbox = new listboxFormBll();
            //checkedListBox_MX.Items.Clear();
            //条码明细初始化
            DataTable dt_mx = listbox.GetMoHuList(string.Format("and yybm='{0}'  order by item_code ", UserInfo.Yybm), "sql_t_jk_lisitems_select");
            if (dt_mx != null && dt_mx.Rows.Count > 0)
            {
                this.checkedListBox_MX.DataSource = dt_mx;
                this.checkedListBox_MX.ValueMember = "item_code";
                this.checkedListBox_MX.DisplayMember = "item_name";
            }

            //条码明细
            DataTable dt_tm_mx = listbox.GetMoHuList(string.Format("and yljgbm='{0}'  order by item_code ", UserInfo.Yybm), "sql_tm_mx_select");
            if (dt_tm_mx != null && dt_tm_mx.Rows.Count > 0)
            {
                for (int i = 0; i < dt_tm_mx.Rows.Count; i++)
                {
                    //明细编码
                    string tc_mc_item_code = dt_tm_mx.Rows[i]["item_code"].ToString();
                    //条码编码
                    string tc_mc_tmbm = dt_tm_mx.Rows[i]["tmbm"].ToString();
                    string tc_mc_tmmc = dt_tm_mx.Rows[i]["tmmc"].ToString();

                    for (int j = 0; j < dt_mx.Rows.Count; j++)
                    {
                        if (tc_mc_item_code.Equals(dt_mx.Rows[j]["item_code"].ToString()) == true)
                        {
                            //设定选中状态
                            dt_mx.Rows[j]["item_name"] = dt_mx.Rows[j]["item_name"] + "  |  " + tc_mc_tmmc;
                        }
                    }
                }

            }

            ////条码初始化
            //DataTable dt_MX = (DataTable)checkedListBox_MX.DataSource;
            //if (dt_MX != null && dt_MX.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt_MX.Rows.Count; i++)
            //    {
            //        checkedListBox_MX.SetItemCheckState(i, System.Windows.Forms.CheckState.Unchecked);
            //    }
            //}

            //套餐编码
            string BARCODE =checkedListBox_TC.SelectedValue.ToString();
            //条码编码
            string tmbm =checkedListBox_tm.SelectedValue.ToString();

            //套餐条码明细
            if (dt_tc_mx == null)
            {
                DataTable dt_mx_TC = listbox.GetMoHuList(string.Format("and yybm='{0}' and barcode='{1}'  order by tmbm ", UserInfo.Yybm, BARCODE), "sql_tc_tm_mx_select");

                if (dt_mx_TC != null && dt_mx_TC.Rows.Count > 0)
                {
                    if (dt_mx_TC.Columns.Contains("zt") == false)
                    {
                        DataColumn dtc = new DataColumn();
                        //0:没有变化  1：增加  2：删除
                        dtc.DefaultValue = "0";
                        dtc.ColumnName = "zt";
                        dt_mx_TC.Columns.Add(dtc);
                    }

                    dt_tc_mx = dt_mx_TC.Copy();
                }
            }
            
            //dt_mx_TC = dt_TC;
            if (dt_tc_mx != null && dt_tc_mx.Rows.Count > 0)
            {
                for (int i = 0; i < dt_tc_mx.Rows.Count; i++)
                {
                    //明细编码
                    string tc_mc_item_code = dt_tc_mx.Rows[i]["item_code"].ToString();

                    //条码编码
                    string tc_mc_tmbm = dt_tc_mx.Rows[i]["tmbm"].ToString();
                    string tc_mc_tmmc = dt_tc_mx.Rows[i]["tmmc"].ToString();
                    string tc_mc_zt = dt_tc_mx.Rows[i]["zt"].ToString();

                    for (int j = 0; j < dt_mx.Rows.Count; j++)
                    {
                        if (tc_mc_item_code.Equals(dt_mx.Rows[j]["item_code"].ToString()) == true )
                        {
                            //设定选中状态
                            //dt_mx.Rows[j]["item_name"] = dt_mx.Rows[j]["item_name"] + "  |  " + tc_mc_tmmc;
                            if (tmbm.Equals(tc_mc_tmbm) == true)
                            {
                                if (tc_mc_zt.Equals("2") == true)
                                {
                                    checkedListBox_MX.SetItemCheckState(j, System.Windows.Forms.CheckState.Unchecked);
                                }
                                else
                                {
                                    checkedListBox_MX.SetItemCheckState(j, System.Windows.Forms.CheckState.Checked);
                                }
                            }
                            else
                            {
                                if (tc_mc_zt.Equals("2") == true)
                                {
                                    checkedListBox_MX.SetItemCheckState(j, System.Windows.Forms.CheckState.Unchecked);
                                }
                                else
                                {
                                    checkedListBox_MX.SetItemCheckState(j, System.Windows.Forms.CheckState.Indeterminate);
                                }
                                
                            }

                        }
                       
                    }
                }
            }

            ////条码初始化
            //DataTable dt_tm_tc = listbox.GetMoHuList(string.Format("and yybm='{0}' and barcode='{1}' order by bar_code ", UserInfo.Yybm, BARCODE), "sql_t_jk_barcodeclass_barcodeitem_select");

            ////string[] selectList = selectvalue.ToString().Split(new char[] { ',' });
            //if (dt_tm_tc == null || dt_tm_tc.Rows.Count == 0)
            //{
            //    return;
            //}
            //for (int i = 0; i < dt_tm_tc.Rows.Count; i++)
            //{
            //    for (int j = 0; j < dt.Rows.Count; j++)

            //        if (dt_tm_tc.Rows[i]["BAR_CODE"].Equals(dt.Rows[j]["tmbm"].ToString()))
            //        {
            //            checkedListBox_tm.SetItemChecked(j, true);
            //            break;
            //        }
            //}
        }


        /// <summary>
        /// 设置为默认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_defaut_Click(object sender, EventArgs e)
        {
            try
            {
                string barcode = checkedListBox_TC.SelectedValue.ToString();
                string updateSql = string.Format(" update  t_jk_barcodeclass set isdefaut='1' where yybm='{0}' and barcode='{1}' ", UserInfo.Yybm, barcode);
                string updateSql_init = string.Format(" update  t_jk_barcodeclass set isdefaut='' where yybm='{0}'  ", UserInfo.Yybm);

                DBAccess db = new DBAccess();

                db.ExecuteNonQueryBySql(updateSql_init);
                db.ExecuteNonQueryBySql(updateSql);


                MessageBox.Show("设定成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("设定失败!" + ex.Message);
            }


        }

        /// <summary>
        /// 保存套餐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_TC_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 保存条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_TM_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 套餐单选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox_TC_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int index = 0;
            for (int i = 0; i < checkedListBox_TC.Items.Count; i++)
            {
                if (i != e.Index) // 不是单击的项
                {
                    //checkedListBox1.SetItemChecked(i,false);    这一句也可以
                    checkedListBox_TC.SetItemCheckState(i, System.Windows.Forms.CheckState.Unchecked); //设置单选核心代码
                }
            }
            string SelectedValue = checkedListBox_TC.Items[e.Index] .ToString().Trim();//获取选定的值

        }

        /// <summary>
        /// 套餐选择改变后的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox_TC_SelectedValueChanged(object sender, EventArgs e)
        {
            //DataTable dt_tem = (DataTable)checkedListBox_TC.DataSource;
            dt_tc_mx = null;
            for (int i = 0; i < checkedListBox_MX.Items.Count; i++)
            {
                checkedListBox_MX.SetItemCheckState(i, System.Windows.Forms.CheckState.Unchecked);
            }

                if (checkedListBox_TC.CheckedItems.Count == 0)
                {
                    checkedListBox_TC.SetItemCheckState(0, System.Windows.Forms.CheckState.Checked);
                    setItemSelectTm(checkedListBox_TC.SelectedValue.ToString());
                    //设定条码明细选中状态
                    setItemSelectTm();
                }
                else
                {
                    //设定条码选中状态
                    setItemSelectTm(checkedListBox_TC.SelectedValue.ToString());
                    //设定条码明细选中状态
                    setItemSelectTm();
                }

        }


        //public CheckState checkResult_clieck = CheckState.Unchecked;
        private void checkedListBox_tm_Click(object sender, EventArgs e)
        {
            setItemSelectTm();
            //string str = checkedListBox_tm.GetItemText(((CheckedListBox)sender).SelectedItem);

            //for (int i = 0; i < checkedListBox_tm.Items.Count; i++)
            //{
            //    if (checkedListBox_tm.GetItemText(checkedListBox_tm.Items[i]).Equals(str) == true)
            //    {
            //        checkResult_clieck = checkedListBox_tm.GetItemCheckState(i);

            //    }


            //    // checkedListBox_tm.GetItemChecked[i];
            //}

           
        }

        /// <summary>
        /// 条码改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox_tm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isinit == true) return;

            string selectstr = checkedListBox_tm.SelectedValue .ToString();
            CheckState checkResult_clieck = CheckState.Indeterminate;

            if (dt_tc_mx != null && dt_tc_mx.Rows.Count > 0)
            {
                string str = checkedListBox_tm.GetItemText(((CheckedListBox)sender).SelectedItem);
                //获取当前选项的状态 是否是选中
                for (int i = 0; i < checkedListBox_tm.Items.Count; i++)
                {
                    if (checkedListBox_tm.GetItemText(checkedListBox_tm.Items[i]).Equals(str) == true)
                    {
                         checkResult_clieck = checkedListBox_tm.GetItemCheckState(i);
                    }
                }

                //处理数据
                if (checkResult_clieck != CheckState.Indeterminate)
                {
                    bool isexist = false;
                    for (int i = 0; i < dt_tc_mx.Rows.Count; i++)
                    {
                        //存在选中的条码
                        if (dt_tc_mx.Rows[i]["tmbm"].ToString().Equals(selectstr))
                        {
                            //条码已经存在了
                            isexist = true;

                            //0:没有变化  1：增加  2：删除
                            //没有选中
                            if (checkResult_clieck == CheckState.Unchecked && dt_tc_mx.Rows[i]["zt"].ToString().Equals ("0")==true)
                            {
                                dt_tc_mx.Rows[i]["zt"] = "2";
                            }
                            else if (checkResult_clieck == CheckState.Checked && dt_tc_mx.Rows[i]["zt"].ToString().Equals("0") == true) //选中
                            {
                                dt_tc_mx.Rows[i]["zt"] = "1";
                            }
                        }

                    }
                    //新选中的条码在套餐中不存在，添加到套餐中
                    if (isexist == false && checkResult_clieck == CheckState.Checked)
                    {
                        string yybm = UserInfo.Yybm;
                        string barcode = checkedListBox_TC.SelectedValue.ToString();//套餐编码
                        string tmbm = checkedListBox_tm.SelectedValue.ToString();//条码编码
                        string tmmc = checkedListBox_tm.GetItemText(((CheckedListBox)sender).SelectedItem);//条码名称

                        listboxFormBll listbox = new listboxFormBll();
                        DataTable dt_tm_mx = listbox.GetMoHuList(string.Format("and yljgbm='{0}'  order by item_code ", UserInfo.Yybm), "sql_tm_mx_select");
                        if (dt_tm_mx != null && dt_tm_mx.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt_tm_mx.Rows.Count; i++)
                            {
                                //明细编码
                                string item_code = dt_tm_mx.Rows[i]["item_code"].ToString(); ;//明细编码
                                string item_name = dt_tm_mx.Rows[i]["item_name"].ToString(); ;//明细名称
                                string shortname = dt_tm_mx.Rows[i]["shortname"].ToString(); ;//简称

                                //条码编码
                                string tc_mc_tmbm = dt_tm_mx.Rows[i]["tmbm"].ToString();
                                //string tc_mc_tmmc = dt_tm_mx.Rows[i]["tmmc"].ToString();

                                if (tc_mc_tmbm.Equals(tmbm) == true)
                                {
                                    dt_tc_mx.Rows.Add();
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["yybm"] = yybm;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["barcode"] = barcode;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["tmbm"] = tmbm;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["tmmc"] = tmmc;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["item_code"] = item_code;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["item_name"] = item_name;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["shortname"] = shortname;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["zt"] = "1";
                                }
                            }
                        }
                    }
                }
                setItemSelectTm();
            }
        }

        /// <summary>
        /// 套餐条码明细改变处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox_MX_SelectedValueChanged(object sender, EventArgs e)
        {

            if (isinit == true) return;

            //条码编码
            string selectstr = checkedListBox_tm.SelectedValue.ToString();

            //明细编码
            string selectstr_mx = checkedListBox_MX.SelectedValue.ToString();

            CheckState checkResult_clieck = CheckState.Indeterminate;

            if (dt_tc_mx != null && dt_tc_mx.Rows.Count > 0)
            {
                string str = checkedListBox_MX.GetItemText(((CheckedListBox)sender).SelectedItem);
                //获取当前选项的状态 是否是选中
                for (int i = 0; i < checkedListBox_MX.Items.Count; i++)
                {
                    if (checkedListBox_MX.GetItemText(checkedListBox_MX.Items[i]).Equals(str) == true)
                    {
                        checkResult_clieck = checkedListBox_MX.GetItemCheckState(i);
                    }
                }

                //处理数据
                if (checkResult_clieck != CheckState.Indeterminate)
                {
                    bool isexist = false;
                    for (int i = 0; i < dt_tc_mx.Rows.Count; i++)
                    {
                        //存在选中的条码
                        if (dt_tc_mx.Rows[i]["item_code"].ToString().Equals(selectstr_mx))
                        {
                            //条码已经存在了
                            isexist = true;

                            //0:没有变化  1：增加  2：删除
                            //没有选中
                            if (checkResult_clieck == CheckState.Unchecked)
                            {
                                dt_tc_mx.Rows[i]["zt"] = "2";
                            }
                            else if (checkResult_clieck == CheckState.Checked) //选中
                            {
                                dt_tc_mx.Rows[i]["zt"] = "1";
                            }
                        }
                    }

                    //新选中的条码在套餐中不存在，添加到套餐中
                    if (isexist == false && checkResult_clieck == CheckState.Checked)
                    {
                        string yybm = UserInfo.Yybm;
                        string barcode = checkedListBox_TC.SelectedValue.ToString();//套餐编码
                        string tmbm = checkedListBox_tm.SelectedValue.ToString();//条码编码
                        string tmmc = checkedListBox_tm.GetItemText(((CheckedListBox)sender).SelectedItem);//条码名称

                        listboxFormBll listbox = new listboxFormBll();

                        DataTable dt_tm_mx = listbox.GetMoHuList(string.Format("and yybm='{0}'  order by item_code ", UserInfo.Yybm), "sql_t_jk_lisitems_select");
                        if (dt_tm_mx != null && dt_tm_mx.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt_tm_mx.Rows.Count; i++)
                            {
                                //明细编码
                                string item_code = dt_tm_mx.Rows[i]["item_code"].ToString(); ;//明细编码
                                string item_name = dt_tm_mx.Rows[i]["item_name"].ToString(); ;//明细名称
                                string shortname = dt_tm_mx.Rows[i]["shortname"].ToString(); ;//简称

                                //条码编码
                               // string tc_mc_tmbm = dt_tm_mx.Rows[i]["tmbm"].ToString();
                                //string tc_mc_tmmc = dt_tm_mx.Rows[i]["tmmc"].ToString();

                                if (item_code.Equals(selectstr_mx) == true)
                                {

                                    dt_tc_mx.Rows.Add();
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["yybm"] = yybm;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["barcode"] = barcode;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["tmbm"] = tmbm;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["tmmc"] = tmmc;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["item_code"] = item_code;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["item_name"] = item_name;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["shortname"] = shortname;
                                    dt_tc_mx.Rows[dt_tc_mx.Rows.Count - 1]["zt"] = "1";
                                    break;
                                }
                            }
                        }
                    }
                }
                //setItemSelectTm();
            }

        }

        //private void checkedListBox_tm_DoubleClick(object sender, EventArgs e)
        //{
        //    string str = checkedListBox_tm.GetItemText(((CheckedListBox)sender).SelectedItem);
        //}
       

    }
}
