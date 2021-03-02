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
using System.IO.Ports;
using Microsoft.Win32;

namespace TJClient.sys
{
    public partial class Form_lisSet : sysCommonForm
    {
        private static string strWhere = "";
        //private static DataTable dt_yq = null;

        private bool drpflag = false;

        public Form_lisSet()
        {
            InitializeComponent();
        }

        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void para_load(object sender, EventArgs e)
        {
            //项目初始化
            initPage();
            //初始化仪器设置
            initYqpz();
        }

        /// <summary>
        /// 选择仪器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_yq_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpflag ==true)
            {
                //初始化仪器设置
                initYqpz();
            }
        }
        /// <summary>
        /// 页面控件初始化
        /// </summary>
        /// <returns></returns>
        public bool initPage()
        {

            //仪器
            setDrp(comboBox_yq, "hyyq", false);

            //端口
            initPorts();

            //波特率
            setDrp(comboBox_btl, "hyyqbtl", true);

            //数据位
            setDrp(comboBox_sjw, "hyyqsjw", true);

            //奇偶校验
            setDrp(comboBox_jojy, "hyyqjojy", true);

            //停止位
            setDrp(comboBox_tzw, "hyyqtzw", true);

            return true;
        }

        /// <summary>
        /// 仪器参数初始化
        /// </summary>
        /// <returns></returns>
        public bool initYqpz()
        {
            string YqXmlPath = "";
            try
            {



                //if (System.Configuration.ConfigurationSettings.AppSettings[comboBox_yq.SelectedValue.ToString()] == null)
                //{
                //    return false;
                //}

                ////xmlpath
                //YqXmlPath = System.Configuration.ConfigurationSettings.AppSettings[comboBox_yq.SelectedValue.ToString()].ToString();
                
                if (Common.Common.getyqPath(comboBox_yq.SelectedValue.ToString()).Length <=0)
                {
                    MessageBox.Show("配置文件不存在!");
                    return false;
                }

                //xmlpath
                YqXmlPath = Common.Common.getyqPath(comboBox_yq.SelectedValue.ToString());
                //端口
                comboBox_dk.Text = XmlRW.GetValueFormXML(YqXmlPath, "YQ_COM", "value");

                //波特率
                setDrpValue(comboBox_btl, XmlRW.GetValueFormXML(YqXmlPath, "YQ_BaudRate", "value"));

                //数据位
                setDrpValue(comboBox_sjw, XmlRW.GetValueFormXML(YqXmlPath, "YQ_DataBits", "value"));

                //奇偶校验
                setDrpValue(comboBox_jojy, XmlRW.GetValueFormXML(YqXmlPath, "YQ_Parity", "value"));

                //停止位
                setDrpValue(comboBox_tzw, XmlRW.GetValueFormXML(YqXmlPath, "YQ_StopBits", "value"));

                //jg数据处理间隔时间
                textBox_jg.Text = XmlRW.GetValueFormXML(YqXmlPath, "YQ_Interval", "value");

                //rqlx	标本日期类型
                if (XmlRW.GetValueFormXML(YqXmlPath, "YQ_DateType", "value").Equals("1"))
                {
                    checkBox_bbrq.Checked = true;
                }
                else
                {
                    checkBox_bbrq.Checked = false;
                }

                //写超时时间(豪秒)
                textBox_writetimeout.Text = XmlRW.GetValueFormXML(YqXmlPath, "YQ_writetimeout", "value");
                //读超时时间(豪秒)
                textBox_readtimeout.Text = XmlRW.GetValueFormXML(YqXmlPath, "YQ_readtimeout", "value");
                //解码程序版本
                richTextBox_version.Text = XmlRW.GetValueFormXML(YqXmlPath, "YQ_Version", "value");
                //注册码
                textBox_register.Text = XmlRW.GetValueFormXML(YqXmlPath, "YQ_RegisterCode", "value");

                //显示仪器标识
                string keyCode = "gwtjyq";
                textBox_registerid.Text = redKey(keyCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }
        #endregion


        /// <summary>
        /// 仪器设置保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ok_left_Click(object sender, EventArgs e)
        {

            try
            {
                string YqXmlPath = "";

                //xmlpath
                YqXmlPath = Common.Common.getyqPath(comboBox_yq.SelectedValue.ToString());

                //端口
                XmlRW.WriteXML(YqXmlPath, "value", comboBox_dk.Text, "YQ_COM");

                //波特率
                XmlRW.WriteXML(YqXmlPath, "value", comboBox_btl.SelectedValue!=null? comboBox_btl.SelectedValue.ToString():"", "YQ_BaudRate");

                //数据位
                XmlRW.WriteXML(YqXmlPath, "value", comboBox_sjw.SelectedValue!=null? comboBox_sjw.SelectedValue.ToString():"", "YQ_DataBits");

                //奇偶校验
                XmlRW.WriteXML(YqXmlPath, "value", comboBox_jojy.SelectedValue!=null? comboBox_jojy.SelectedValue.ToString():"", "YQ_Parity");

                //停止位
                XmlRW.WriteXML(YqXmlPath, "value", comboBox_tzw.SelectedValue!=null? comboBox_tzw.SelectedValue.ToString():"", "YQ_StopBits");

                //jg数据处理间隔时间
                XmlRW.WriteXML(YqXmlPath, "value", textBox_jg.Text, "YQ_Interval");

                //rqlx	标本日期类型
                if (checkBox_bbrq.Checked == true)
                {
                    XmlRW.WriteXML(YqXmlPath, "value", "1", "YQ_DateType");
                    
                }
                else
                {
                    XmlRW.WriteXML(YqXmlPath, "value", "0", "YQ_DateType");
                }

                //写超时时间(豪秒)
                XmlRW.WriteXML(YqXmlPath, "value", textBox_writetimeout.Text, "YQ_writetimeout");
                //读超时时间(豪秒)
                XmlRW.WriteXML(YqXmlPath, "value", textBox_readtimeout.Text, "YQ_readtimeout");
                //解码程序版本
                XmlRW.WriteXML(YqXmlPath, "value", richTextBox_version.Text, "YQ_Version");

                //注册码
                XmlRW.WriteXML(YqXmlPath, "value", textBox_register.Text, "YQ_RegisterCode");

                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #region 公共方法
        /// <summary>
        /// 串口
        /// </summary>
        public void initPorts()
        {
            //初始化下拉串口名称列表框
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            comboBox_dk.Items.AddRange(ports);
        }
        /// <summary>
        /// 设定下拉框的值
        /// </summary>
        /// <returns></returns>
        public bool setDrpValue(ComboBox drp, object drpValue)
        {
            try
            {
                drp.SelectedValue = drpValue;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 绑定数据值
        /// </summary>
        /// <param name="drp"></param>
        /// <param name="initValue"></param>
        /// <returns></returns>
        public bool setDrp(ComboBox drp, string zdCode,bool ifkh)
        {
            try
            {
                drpflag = false;
                //获取结果集
                DataTable dt = Common.Common.getsjzd(zdCode, "sql_select_sjzd");
                if (ifkh == true)
                {
                    DataRow dtRow = dt.NewRow();
                    dt.Rows.InsertAt(dtRow, 0);
                }
               
                drp.DisplayMember = "ZDMC";
                drp.ValueMember = "ZDBM";
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

        /// <summary>
        /// 重写父类的方法，设定当前页面的值 strText： 编码|名称
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public override bool setText(string strText)
        {
            if (strText.Length > 0)
            {
                string[] textList = strText.Split(new char[] { '|' });

                //bm.Text = textList[0];
                //mc.Text = textList[1];
                //dataSelect();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 注册表操作

        /// <summary>
        /// 创建(设置值、修改)   对键值的创建修改等操作主要用到RegistryKey 的SetValue()方法
        /// </summary>
        public void createKey(string keyCode)
        {
            RegistryKey key = Registry.CurrentUser;
            if (IsRegeditItemExist(keyCode) == false)
            {
                RegistryKey software = key.CreateSubKey("software\\" + keyCode);

            }
            key.Close();
        }

        /// <summary>
        /// 创建(设置值、修改)   对键值的创建修改等操作主要用到RegistryKey 的SetValue()方法
        /// </summary>
        public void ReCreateKey(string keyCode)
        {
            RegistryKey key = Registry.CurrentUser;
            if (IsRegeditItemExist(keyCode) == false)
            {
                RegistryKey software = key.CreateSubKey("software\\" + keyCode);
            }
            else
            {
                deleteKey(keyCode);
                RegistryKey software = key.CreateSubKey("software\\" + keyCode);
            }
            key.Close();
        }

        /// <summary>
        /// 写
        /// </summary>
        public void writKey(string keycode, string value)
        {
            RegistryKey key = Registry.CurrentUser;
            if (IsRegeditKeyExit(keycode) == false)
            {
                RegistryKey software = key.OpenSubKey("software\\" + keycode, true); //该项必须已存在
                software.SetValue(keycode, value);
                //在HKEY_LOCAL_MACHINE\SOFTWARE\test下创建一个名为“test”，值为“博客园”的键值。如果该键值原本已经存在，则会修改替换原来的键值，如果不存在则是创建该键值。
                // 注意：SetValue()还有第三个参数，主要是用于设置键值的类型，如：字符串，二进制，Dword等等~~默认是字符串。如：
                // software.SetValue("test", "0", RegistryValueKind.DWord); //二进制信息
            }
            key.Close();
        }

        /// <summary>
        /// 读
        /// </summary>
        public string redKey(string keycode)
        {
            string info = "";
            RegistryKey Key;
            Key = Registry.CurrentUser;
            try
            {
                RegistryKey myreg = Key.OpenSubKey("software\\" + keycode);
                info = myreg.GetValue(keycode).ToString();
                myreg.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return info;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void deleteKey(string keycode)
        {
            
            RegistryKey delKey = Registry.CurrentUser.OpenSubKey("Software\\" + keycode, true);
            delKey.DeleteValue(keycode);
            delKey.Close();
        }

        /// <summary>
        /// 判断注册表项是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsRegeditItemExist(string keycode)
        {
            string[] subkeyNames;
            RegistryKey hkml = Registry.CurrentUser;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE");
            //RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);  
            subkeyNames = software.GetSubKeyNames();
            //取得该项下所有子项的名称的序列，并传递给预定的数组中  
            foreach (string keyName in subkeyNames)
            //遍历整个数组  
            {
                if (keyName == keycode)
                //判断子项的名称  
                {
                    hkml.Close();
                    return true;
                }
            }
            hkml.Close();
            return false;
        }

        /// <summary>
        /// 判断键值是否存在这里写代码片
        /// </summary>
        /// <returns></returns>
        public bool IsRegeditKeyExit(string keycode)
        {
            string[] subkeyNames;
            RegistryKey hkml = Registry.CurrentUser;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE\\gwtjyq");
            //RegistryKey software = hkml.OpenSubKey("SOFTWARE\\test", true);
            subkeyNames = software.GetValueNames();
            //取得该项下所有键值的名称的序列，并传递给预定的数组中
            foreach (string keyName in subkeyNames)
            {
                if (keyName == keycode) //判断键值的名称
                {
                    hkml.Close();
                    return true;
                }
            }
            hkml.Close();
            return false;
        }
      #endregion

        /// <summary>
        /// 查看仪器标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_getid_Click(object sender, EventArgs e)
        {
            string keyCode = "gwtjyq";
            textBox_registerid.Text = redKey(keyCode);
            if (textBox_registerid.Text.Length == 0)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("还没有生成设备标识，确定要生成设备标识码?", "仪器标识", messButton);
                if (dr == DialogResult.OK)
                {
                    string value = Guid.NewGuid().ToString();
                    ReCreateKey(keyCode);
                    writKey(keyCode, value);
                    textBox_registerid.Text = redKey(keyCode);
                    MessageBox.Show("请将仪器标识号，返回给软件服务商！");
                }
            }
           
        }

        /// <summary>
        /// 生成仪器标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_regetid_Click(object sender, EventArgs e)
        {
            string keyCode = "gwtjyq";
            string value = Guid.NewGuid().ToString();
            if (textBox_registerid.Text.Length == 0)
            {
                ReCreateKey(keyCode);
                writKey(keyCode, value);
                textBox_registerid.Text = redKey(keyCode);

                MessageBox.Show("请将仪器标识号，返回给软件服务商！");
            }
            else
            {

                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("设备标识已存在，确定要重新生成?  （重新生成将需要重新注册，否则设备无法使用）", "仪器标识", messButton);
                if (dr == DialogResult.OK)
                {
                    ReCreateKey(keyCode);
                    writKey(keyCode, value);
                    textBox_registerid.Text = redKey(keyCode);
                    MessageBox.Show("请将新生成的仪器标识号，返回给软件服务商！");
                }
            }
            
        }

    }
}
