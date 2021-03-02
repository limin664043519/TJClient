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
using System.Xml;
using System.Collections;

namespace TJClient.sys
{
    public partial class Form_ShowTmPrint : sysCommonForm
    {

       // private static string  xmlpath = "./Dll/sysSet.xml";

        private string tjh_para = "";
        private string sxh_para = "";
        private string name_para = "";
        private string sfzh_para = "";
        private string zdrq_para = "";
        private string zdrqmc_para = "";


        public Form_ShowTmPrint()
        {
            InitializeComponent();
        }

        public void setpara(string tjh, string sxh, string name, string sfzh, string zdrq, string zdrqmc)
        {
            tjh_para = tjh;
            sxh_para = sxh;
            name_para = name;
            sfzh_para = sfzh;
            zdrq_para = zdrq;
            zdrqmc_para = zdrqmc;
        }


        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void para_load(object sender, EventArgs e)
        {
            //姓名
            label_xm.Text = name_para;
            //身份证号
            label_sfzh.Text = sfzh_para;
            //人群
            label_rq.Text = zdrqmc_para;
            checkedListBox1.ColumnWidth = 300;
            initCheck();
        }

        /// <summary>
        /// 初始化条码
        /// </summary>
        private void initCheck()
        {
            AddFormBll addformbll = new AddFormBll();
            DataTable dt_tm = addformbll.GetMoHuList(string.Format(" and SFDY='1' and YLJGBM='{0}'", UserInfo.Yybm), "sql030");
            checkedListBox1.DataSource = dt_tm;
            checkedListBox1.DisplayMember = "tmmc";
            checkedListBox1.ValueMember = "tmbm";

            //设定选中项目
            setItemSelect();
        }

        /// <summary>
        /// 设定选中项目
        /// </summary>
        /// <param name="selectvalue"></param>
        public void setItemSelect()
        {
            AddFormBll addformbll = new AddFormBll();
            DataTable dt_tm_rq = addformbll.GetMoHuList(string.Format("  and T_JK_TM_zdRQ.YLJGBM='{0}' and T_JK_TM_zdRQ.ZDRQ='{1}'", UserInfo.Yybm, zdrq_para), "sql100");
            if (dt_tm_rq != null && dt_tm_rq.Rows.Count > 0)
            {
                DataTable dt = (DataTable)checkedListBox1.DataSource;
                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < dt_tm_rq.Rows .Count ; i++)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                        if (dt_tm_rq.Rows[i]["tmbm"].ToString().Equals(dt.Rows[j]["tmbm"].ToString()))
                        {
                            checkedListBox1.SetItemChecked(j, true);
                            break;
                        }
                }
            }
        }

        //打印
        private void button1_Click(object sender, EventArgs e)
        {
            print_tm( tjh_para,  sxh_para,  name_para,  sfzh_para,  zdrq_para,  zdrqmc_para);
        }


        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="tjh"></param>
        /// <param name="sxh"></param>
        /// <param name="name"></param>
        /// <param name="sfzh"></param>
        /// <param name="zdrq"></param>
        /// <param name="zdrqmc"></param>
        private void print_tm(string tjh, string sxh, string name, string sfzh, string zdrq, string zdrqmc)
        {
            //打印
            PrintHelper printDemo = new PrintHelper();
            bool result = printDemo.printBarCode(getPrintBarCode(tjh, sxh, name, sfzh, zdrq, zdrqmc), Code128.Encode.EAN128);
            if (result == false)
            {
                MessageBox.Show("打印错误请重新打印！");
            }
        }


        /// <summary>
        /// 获取BarCode打印列表
        /// </summary>
        /// <param name="tjh"></param>
        /// <param name="sxh"></param>
        /// <param name="name"></param>
        /// <param name="sfzh"></param>
        /// <param name="zdrq"></param>
        /// <param name="zdrqmc"></param>
        /// <returns></returns>
        public ArrayList getPrintBarCode(string tjh, string sxh, string name, string sfzh, string zdrq, string zdrqmc)
        {

            ArrayList TmList = new ArrayList();
            DataTable dt = (DataTable)checkedListBox1.DataSource;
            string strCollected = string.Empty;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    TmList.Add(sxh.PadRight(4, ' ') + dt.Rows[i]["TMMC"].ToString() + "|" + name.PadRight(4, ' ') + sfzh + "|" + tjh + dt.Rows[i]["TMBM"].ToString());
                }
            }
            return TmList;
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_return_Click(object sender, EventArgs e)
        {
            sysCommonForm owerForm = (sysCommonForm)this.Owner;
            //返回时调用父页面方法更新参数
            owerForm.setParentFormDo(true);
            this.Close();
        }
    }
}
