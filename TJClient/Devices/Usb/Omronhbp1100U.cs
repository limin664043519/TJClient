using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TJClient.Devices.Com;
using TJClient.Devices.Common;
using USBHIDDevice;
using Yq.Common.Model;

namespace TJClient.Devices.Usb
{
    public class Omronhbp1100U
    {
        private static string _yqxh = "OMRONHBP1100U";
        private static UsbHidDevice _device;
        private static string _error = "";
        private static Action<bool, DataTable> _writeAction = null;
        private static bool _canWrite = false;
        private static string _deviceName = "";
        private static void Init()
        {
            _error = "";
        }

        public static string GetError()
        {
            return _error;
        }

        public static void SetCanWrite(bool canWrite)
        {
            _canWrite = canWrite;
        }
        public static void SetWriteAction(Action<bool, DataTable> writeAction)
        {
            _writeAction = writeAction;
        }

        public static bool HadSetWriteAction()
        {
            return _writeAction != null;
        }

        private static string GetDevice()
        {
            if (!string.IsNullOrEmpty(_deviceName))
            {
                return _deviceName;
            }
            UsbHidDevice usbhid = new UsbHidDevice();
            List<string> slist = new List<string>();
            usbhid.GetDeviceList(ref slist);
            string strSearch = string.Format("vid_{0:x4}&pid_{1:x4}", 0x0590, 0x00B3);
            foreach (string item in slist)
            {
                if (item.IndexOf(strSearch, StringComparison.Ordinal) >= 0)
                {
                    _deviceName = item;
                    break;
                }
            }

            return _deviceName;
        }

        

        private static string GetSys(string content)
        {
            string info = content.Substring(5, 4);
            int sys = Convert.ToInt32(info, 16);
            sys = sys / 128;
            return sys.ToString();
        }

        private static string GetDia(string content)
        {
            string info = content.Substring(9, 4);
            int dia= Convert.ToInt32(info, 16);
            dia = dia / 128;
            return dia.ToString();
        }

        private static string GetPul(string content)
        {
            string info = content.Substring(13, 2);
            int pul = Convert.ToInt32(info, 16);
            return pul.ToString();
        }

        private static DataTable GetDt(string content)
        {
            DataTable dt = new DataTable();
            if (content.Length >= 15)
            {
                string ybh = "ybh_" + DateTime.Now.ToString("yyyyMMddhhmmssffff");
                string jyrq = TJClient.Common.DateHelper.CurrDate();
                string hysj = TJClient.Common.DateHelper.CurrDateTime();
                string sys = GetSys(content);
                string dia = GetDia(content);
                string pul = GetPul(content);
                dt = Xueyaji.GetDt(_yqxh, ybh, jyrq, hysj, sys, dia, pul);
            }
            return dt;
        }
        private static void DeviceDataReceived(byte[] data)
        {
            string content= Encoding.ASCII.GetString(data, 1, data.Length - 1);//65字节 第一字节为0x00
            DataTable dt = GetDt(content);
            if (dt.Rows.Count > 0)
            {
                //开始赋值
                if (_writeAction != null && _canWrite)
                {
                    _writeAction(false, dt);
                }
            }
        }
        private static bool ConnectDevice(string deviceName)
        {
            Dispose();
            _device = new UsbHidDevice(0x0590, 0x00B3);
            _device.DataReceived += DeviceDataReceived;
            return _device.OpenDevice(deviceName);
        }
        private static void TestData()
        {
            string content = ")rx003180228054ra0202rbOMUDA000F66E7";
            DataTable dt = GetDt(content);
            if (_writeAction != null && _canWrite)
            {
                _writeAction(false, dt);
            }
        }
        public static bool Start()
        {
            Init();
            //TestData();
            //return true;
            string deviceName = GetDevice();
            if (string.IsNullOrEmpty(deviceName))
            {
                _error = "未找到欧姆龙血压计设备";
                return false;
            }

            if (!ConnectDevice(deviceName))
            {
                _error = "欧姆龙血压计打开失败";
                return false;
            }

            return true;
        }

        private static void Dispose()
        {
            if (_device != null)
            {
                _device.Dispose();
                _device = null;
            }
        }
        public static bool End()
        {
            try
            {
                Dispose();
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
