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

namespace TJClient.sys
{
    public partial class Form_PageSet : sysCommonForm
    {
        //private static string strWhere = "";
        private  string  xmlpath = "";
        public Form_PageSet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void para_load(object sender, EventArgs e)
        {
            xmlpath = System.Configuration.ConfigurationManager.AppSettings["sysSetPath"].ToString();  
            //checkBox_nlcheck_print 打印条码时验证年龄
            loadCheckboxValue(checkBox_nlcheck_print);

            //checkBox_showAll_print 显示条码选择(每次打印)
            loadCheckboxValue(checkBox_showAll_print);

            //checkBox_zdrq_print 按重点人群打印条码
            loadCheckboxValue(checkBox_zdrq_print);

            //checkBox_showZdrq_print 显示条码选择(重点人群没有设定)
            loadCheckboxValue(checkBox_showZdrq_print);



        }
        /// <summary>
        /// 设定页面上的项目
        /// </summary>
        /// <param name="checkbox"></param>
        private void loadCheckboxValue(CheckBox checkbox)
        {
            try
            {
                string value = (new XmlRW()).GetValueFormXML(xmlpath, checkbox.Name);
                if (value.ToLower().Equals("true"))
                {
                    checkbox.Checked = true;
                }
                else
                {
                    checkbox.Checked = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        //保存
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //checkBox_nlcheck_print 打印条码时验证年龄
                xmlSave(checkBox_nlcheck_print);

                //checkBox_showAll_print 显示条码选择(每次打印)
                xmlSave(checkBox_showAll_print);

                //checkBox_zdrq_print 按重点人群打印条码
                xmlSave(checkBox_zdrq_print);

                //checkBox_showZdrq_print 显示条码选择(重点人群没有设定)
                xmlSave(checkBox_showZdrq_print);

                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 将页面中的值设定到xml文件中
        /// </summary>
        /// <param name="checkbox"></param>
        private void xmlSave(CheckBox checkbox)
        {
            if (checkbox.Checked == true)
            {
                XmlRW.WriteXML_set(xmlpath, "", "true", checkbox.Name);
            }
            else
            {
                XmlRW.WriteXML_set(xmlpath, "", "false", checkbox.Name);
            }
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
