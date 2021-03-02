using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TJClient.sys;

namespace TJClient.ComForm
{
    public partial class Form_readSfzh : Form
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct IDCardData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Name; //姓名   
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string Sex;   //性别
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string Nation; //名族
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string Born; //出生日期
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 72)]
            public string Address; //住址
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
            public string IDCardNo; //身份证号
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string GrantDept; //发证机关
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string UserLifeBegin; // 有效开始日期
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string UserLifeEnd;  // 有效截止日期
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
            public string reserved; // 保留
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
            public string PhotoFileName; // 照片路径
        }
        /************************端口类API *************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetCOMBaud", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetCOMBaud(int iPort, ref uint puiBaudRate);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetCOMBaud", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetCOMBaud(int iPort, uint uiCurrBaud, uint uiSetBaud);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_OpenPort", CharSet = CharSet.Ansi)]
        public static extern int Syn_OpenPort(int iPort);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ClosePort", CharSet = CharSet.Ansi)]
        public static extern int Syn_ClosePort(int iPort);
        /**************************SAM类函数 **************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetMaxRFByte", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetMaxRFByte(int iPort, byte ucByte, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ResetSAM", CharSet = CharSet.Ansi)]
        public static extern int Syn_ResetSAM(int iPort, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMStatus", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMStatus(int iPort, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMID", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMID(int iPort, ref byte pucSAMID, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMIDToStr", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMIDToStr(int iPort, ref byte pcSAMID, int iIfOpen);
        /*************************身份证卡类函数 ***************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_StartFindIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_StartFindIDCard(int iPort, ref byte pucIIN, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SelectIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_SelectIDCard(int iPort, ref byte pucSN, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseMsg(int iPort, ref byte pucCHMsg, ref uint puiCHMsgLen, ref byte pucPHMsg, ref uint puiPHMsgLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseMsgToFile", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseMsgToFile(int iPort, ref byte pcCHMsgFileName, ref uint puiCHMsgFileLen, ref byte pcPHMsgFileName, ref uint puiPHMsgFileLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseFPMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseFPMsg(int iPort, ref byte pucCHMsg, ref uint puiCHMsgLen, ref byte pucPHMsg, ref uint puiPHMsgLen, ref byte pucFPMsg, ref uint puiFPMsgLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadBaseFPMsgToFile", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadBaseFPMsgToFile(int iPort, ref byte pcCHMsgFileName, ref uint puiCHMsgFileLen, ref byte pcPHMsgFileName, ref uint puiPHMsgFileLen, ref byte pcFPMsgFileName, ref uint puiFPMsgFileLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadNewAppMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadNewAppMsg(int iPort, ref byte pucAppMsg, ref uint puiAppMsgLen, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetBmp", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetBmp(int iPort, ref byte Wlt_File);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadMsg(int iPortID, int iIfOpen, ref IDCardData pIDCardData);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadFPMsg", CharSet = CharSet.Ansi)]
        public static extern int Syn_ReadFPMsg(int iPortID, int iIfOpen, ref IDCardData pIDCardData, ref byte cFPhotoname);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindReader", CharSet = CharSet.Ansi)]
        public static extern int Syn_FindReader();
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindUSBReader", CharSet = CharSet.Ansi)]
        public static extern int Syn_FindUSBReader();
        /***********************设置附加功能函数 ************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetPhotoPath", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetPhotoPath(int iOption, ref byte cPhotoPath);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetPhotoType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetPhotoType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetPhotoName", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetPhotoName(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetSexType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetSexType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetNationType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetNationType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetBornType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetBornType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetUserLifeBType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetUserLifeBType(int iType);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SetUserLifeEType", CharSet = CharSet.Ansi)]
        public static extern int Syn_SetUserLifeEType(int iType, int iOption);

        int m_iPort;

        //身份证号
        public DataTable dtSfzh = null;


        public Form_readSfzh()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 测试读卡器
        /// </summary>
        /// <returns></returns>
        private string  readCardTest()
        {
            string stmp="";
            int i, nRet;
            uint[] iBaud = new uint[1];
            i = Syn_FindReader();
            m_iPort = i;
            if (i > 0)
            {
                if (i > 1000)
                {
                    //stmp = Convert.ToString(i);
                    //stmp = Convert.ToString(System.DateTime.Now) + "  读卡器连接在USB " + stmp;
                    //listBox1.Items.Add(stmp);
                }
                else
                {
                    System.Threading.Thread.Sleep(200);
                    nRet = Syn_GetCOMBaud(i, ref iBaud[0]);
                    //stmp = Convert.ToString(i);
                    //stmp = Convert.ToString(System.DateTime.Now) + "  读卡器连接在COM " + stmp + ";当前波特率为 " + Convert.ToString(iBaud[0]);
                    //listBox1.Items.Add(stmp);

                }
            }
            else
            {
                stmp = Convert.ToString(System.DateTime.Now) + "  没有找到读卡器";
                //listBox1.Items.Add(stmp);
            }
            return stmp;
        }

   

        /// <summary>
        /// 读取身份证号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public  DataTable readSfzh()
        {
            IDCardData CardMsg = new IDCardData();
            DataTable dt = new DataTable();
            int nRet, nPort, iPhotoType;
            string stmp;
            byte[] cPath = new byte[255];
            byte[] pucIIN = new byte[4];
            byte[] pucSN = new byte[8];
            nPort = m_iPort;
            try
            {

                string readcard = readCardTest();
                if (readcard.Length > 0)
                {
                    //stmp = Convert.ToString(System.DateTime.Now) + "  读取身份证信息错误";
                    throw new Exception(readcard);
                }

                Syn_SetPhotoPath(0, ref cPath[0]);	//设置照片路径	iOption 路径选项	0=C:	1=当前路径	2=指定路径
                //cPhotoPath	绝对路径,仅在iOption=2时有效
                iPhotoType = 0;
                Syn_SetPhotoType(0); //0 = bmp ,1 = jpg , 2 = base64 , 3 = WLT ,4 = 不生成
                Syn_SetPhotoName(2); // 生成照片文件名 0=tmp 1=姓名 2=身份证号 3=姓名_身份证号 

                Syn_SetSexType(1);	// 0=卡中存储的数据	1=解释之后的数据,男、女、未知
                Syn_SetNationType(1);// 0=卡中存储的数据	1=解释之后的数据 2=解释之后加"族"
                Syn_SetBornType(1);			// 0=YYYYMMDD,1=YYYY年MM月DD日,2=YYYY.MM.DD,3=YYYY-MM-DD,4=YYYY/MM/DD
                Syn_SetUserLifeBType(1);	// 0=YYYYMMDD,1=YYYY年MM月DD日,2=YYYY.MM.DD,3=YYYY-MM-DD,4=YYYY/MM/DD
                Syn_SetUserLifeEType(1, 1);	// 0=YYYYMMDD(不转换),1=YYYY年MM月DD日,2=YYYY.MM.DD,3=YYYY-MM-DD,4=YYYY/MM/DD,
                // 0=长期 不转换,	1=长期转换为 有效期开始+50年           
                if (Syn_OpenPort(nPort) == 0)
                {
                    if (Syn_SetMaxRFByte(nPort, 80, 0) == 0)
                    {
                        nRet = Syn_StartFindIDCard(nPort, ref pucIIN[0], 0);
                        nRet = Syn_SelectIDCard(nPort, ref pucSN[0], 0);
                        nRet = Syn_ReadMsg(nPort, 0, ref CardMsg);
                        if (nRet == 0)
                        {
                            //新中新
                            dt.Columns.Add("姓名");
                            dt.Columns.Add("性别");
                            dt.Columns.Add("性别编码");
                            dt.Columns.Add("民族");
                            dt.Columns.Add("出生日期");
                            dt.Columns.Add("地址");
                            dt.Columns.Add("身份证号");
                            dt.Columns.Add("发证机关");
                            dt.Columns.Add("有效期开始");
                            dt.Columns.Add("有效期结束");
                            dt.Columns.Add("照片文件名");
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["姓名"] = CardMsg.Name;
                            dt.Rows[dt.Rows.Count - 1]["性别"] = CardMsg.Sex;
                            dt.Rows[dt.Rows.Count - 1]["性别编码"] = CardMsg.Sex.Equals("男") ? "1" : "2";
                            dt.Rows[dt.Rows.Count - 1]["民族"] = CardMsg.Nation;
                            dt.Rows[dt.Rows.Count - 1]["出生日期"] = CardMsg.Born;
                            dt.Rows[dt.Rows.Count - 1]["地址"] = CardMsg.Address;
                            dt.Rows[dt.Rows.Count - 1]["身份证号"] = CardMsg.IDCardNo;
                            dt.Rows[dt.Rows.Count - 1]["发证机关"] = CardMsg.GrantDept;
                            dt.Rows[dt.Rows.Count - 1]["有效期开始"] = CardMsg.UserLifeBegin;
                            dt.Rows[dt.Rows.Count - 1]["有效期结束"] = CardMsg.UserLifeEnd;
                            dt.Rows[dt.Rows.Count - 1]["照片文件名"] = CardMsg.PhotoFileName;

                        }
                        else if (nRet == 1)
                        {
                            //神思
                            dt.Columns.Add("姓名");
                            dt.Columns.Add("性别");
                            dt.Columns.Add("性别编码");
                            dt.Columns.Add("民族");
                            dt.Columns.Add("出生日期");
                            dt.Columns.Add("地址");
                            dt.Columns.Add("身份证号");
                            dt.Columns.Add("发证机关");
                            dt.Columns.Add("有效期开始");
                            dt.Columns.Add("有效期结束");
                            dt.Columns.Add("照片文件名");
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["姓名"] = CardMsg.Name;
                            dt.Rows[dt.Rows.Count - 1]["性别"] = CardMsg.Sex;
                            dt.Rows[dt.Rows.Count - 1]["性别编码"] = CardMsg.Sex.Equals ("男")? "1" :"2";
                            dt.Rows[dt.Rows.Count - 1]["民族"] = CardMsg.Nation;
                            dt.Rows[dt.Rows.Count - 1]["出生日期"] = CardMsg.Born;
                            dt.Rows[dt.Rows.Count - 1]["地址"] = CardMsg.Address;
                            dt.Rows[dt.Rows.Count - 1]["身份证号"] = CardMsg.IDCardNo;
                            dt.Rows[dt.Rows.Count - 1]["发证机关"] = CardMsg.GrantDept;
                            dt.Rows[dt.Rows.Count - 1]["有效期开始"] = CardMsg.UserLifeBegin;
                            dt.Rows[dt.Rows.Count - 1]["有效期结束"] = CardMsg.UserLifeEnd;
                            dt.Rows[dt.Rows.Count - 1]["照片文件名"] = CardMsg.PhotoFileName;
                        }
                        else
                        {
                            stmp = Convert.ToString(System.DateTime.Now) + "  读取身份证信息错误";
                            throw new Exception(stmp);

                        }
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(1);
                    stmp = Convert.ToString(System.DateTime.Now) + "  读取故障，请重新读取！";
                    // listBox1.Items.Add(stmp);
                    //throw new Exception(stmp);
                    return readSfzh();
                }
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                if(this.Owner!=null){
                sysCommonForm owerForm = (sysCommonForm)this.Owner;
                owerForm.setStatus("1");
            }
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timer1_Tick(object sender, EventArgs e)
        {
            DataTable dt = readSfzh();
            if (dt != null && dt.Rows.Count > 0)
            {
                dtSfzh = dt;
                sysCommonForm owerForm = (sysCommonForm)this.Owner;
                owerForm.setTextFromDb(dtSfzh);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void autoReadShzh_start()
        {
            timer1.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void autoReadShzh_stop()
        {
            timer1.Enabled = false;
        }

    }
}
