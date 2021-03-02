using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using TJClient.Devices.Common;
using Yq.Common;
using Yq.Common.Model;

namespace TJClient.Devices.Com
{
    public class Maibobo
    {
        private static string _yqxh = "MAIBOBO";
        
        private static int _baudRate = 115200;

        private static Parity _parity = Parity.None;
        private static int _dataBits = 8;
        private static StopBits _stopBits = StopBits.None;
        private static string _error = "";
        private static SerialPortOperation _op = null;

        private static Action<bool, DataTable> _writeAction = null;

        public static void SetWriteAction(Action<bool, DataTable> writeAction)
        {
            _writeAction = writeAction;
        }

        private static string GetYqXmlPath()
        {
            return TJClient.Common.Common.getyqPath(_yqxh);
        }
        public static bool XmlFileExisted()
        {
            if (GetYqXmlPath().Length <= 0)
            {
                return false;
            }
            
            return true;
        }

        private static void Init()
        {
            _error = "";
        }

        private static string GetValueFromXml(string key)
        {
            return TJClient.Common.XmlRW.GetValueFormXML(GetYqXmlPath(), key, "value");
        }

        public static string GetYqCom()
        {
            return GetValueFromXml("YQ_COM");
        }

        public static string GetYqItemType()
        {
            return GetValueFromXml("YQ_itemType");
        }

        public static string GetYqItems()
        {
            return GetValueFromXml("YQ_items");
        }

        private static byte[] GetConnectionCommand()
        {
            byte[] connectionCommand= { 0xcc, 0x80, 0x03, 0x03, 0x01, 0x01, 0x00, 0x00 };
            return connectionCommand;
        }

        private static byte[] GetStartCommand()
        {
            byte[] startCommand = { 0xcc, 0x80, 0x03, 0x03, 0x01, 0x02, 0x00, 0x03 };
            return startCommand;
        }

        private static byte[] GetCloseCommand()
        {
            byte[] closeCommand = { 0xcc, 0x80, 0x03, 0x03, 0x01, 0x03, 0x00, 0x02 };
            return closeCommand;
        }

        public static string GetMaiboboError()
        {
            return _error;
        }

        public static string GetComError()
        {
            return _op.GetError();
        }
        public static bool Start()
        {
            try
            {
                Init();
                _op = new SerialPortOperation(GetYqCom(), _baudRate, _dataBits);
                _op.SetExecuteFunction(Execute);
                _op.Open();
                _op.Send(GetConnectionCommand());
                _op.Send(GetStartCommand());
                return true;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                return false;
            }
        }
        public static bool Test()
        {
            Init();
            TestData();
            return true;
        }
        private static string TestData()
        {
            string content = "AA 80 03 0F 01 06 01 13 07 02 0F 0B 1C 00 8B 00 66 00 48 A1";
            DataTable dt = GetDt(content);
            if (_writeAction != null)
            {
                _writeAction(false, dt);
            }

            return "";
        }
        private static string GetJyrq(string[] arr)
        {
            return string.Format("20{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", Convert.ToInt32(arr[1], 16), Convert.ToInt32(arr[2], 16), Convert.ToInt32(arr[3], 16),
                Convert.ToInt32(arr[4], 16), Convert.ToInt32(arr[5], 16), Convert.ToInt32(arr[6], 16));
        }
        private static string GetSys(string[] arr)
        {
            return Convert.ToInt32(arr[8], 16).ToString();
        }

        private static string GetDia(string[] arr)
        {
            return Convert.ToInt32(arr[10], 16).ToString();
        }

        private static string GetPul(string[] arr)
        {
            return Convert.ToInt32(arr[12], 16).ToString();
        }

        

        private static DataTable GetDt(string content)
        {
            string[] arr = content.Split(' ').Skip(6).ToArray();
            DataTable dt = TJClient.Devices.Com.TjDataTable.GetStructureDataTable(_yqxh);
            if (arr.Length >= 13)
            {
                string ybh = "ybh_" + DateTime.Now.ToString("yyyyMMddhhmmssffff");
                string jyrq = TJClient.Common.DateHelper.CurrDate();
                string hysj = TJClient.Common.DateHelper.CurrDateTime();
                //血压的内容以后全部统一
                TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "DATE", "检验日期", GetJyrq(arr), "", ""), ref dt);

                TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "ERRORCODE", "错误编码", "", "", ""), ref dt);

                TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "SYSTOLIC", "收缩压", GetSys(arr), "", ""), ref dt);

                TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "MEANBLOODPRESSURE", "平均血压", "", "", ""), ref dt);
                TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "DIASTOLIC", "舒张压", GetDia(arr), "", ""), ref dt);
                string pul = GetPul(arr);
                TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "PULSERATE", "脉率", pul, "", ""), ref dt);
                TjDataTable.AddInfoToDt(new BaseInfo(jyrq, hysj, ybh, "BREATHRATE", "呼吸频率", Xueyaji.GetBreathRate(pul), "", ""), ref dt);
            }

            return dt;
        }

        private static string Execute(string content)
        {
            if (content.ToUpper().Contains("AA 80 03 0F 01 06 01")) //结束标识
            {
                DataTable dt = GetDt(content);
                if (dt.Rows.Count > 0)
                {
                    //开始赋值
                    if (_writeAction != null)
                    {
                        _writeAction(false, dt);
                    }
                }
            }
            return "";
        }

        public static bool End()
        {
            try
            {
                _op.Send(GetCloseCommand());
                _op.Close();
                return true;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                return false;
            }
        }

    }
}
