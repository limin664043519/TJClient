using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJClient.Devices.Com
{
    public class SerialPortOperation
    {
        private string _portName = "";
        private int _baudRate = 9600;
        private Parity _parity = Parity.None;
        private int _dataBits = 8;
        private StopBits _stopBits = StopBits.None;
        private SerialPort _serialPort = null;
        private string _error = "";
        private string _yqxh = ""; //暂时先不用
        private Func<string,string> _executeFunction = null;


        public void SetExecuteFunction(Func<string, string> executeFunction)
        {
            _executeFunction = executeFunction;
        }

        public void SetYqxh(string yqxh)
        {
            _yqxh = yqxh;
        }

        private void InitSerialPort()
        {
            try
            {
                _serialPort = new SerialPort(_portName, _baudRate, _parity, _dataBits, StopBits.One);
                _serialPort.DataReceived += Port_DataReceived;
            }catch(Exception ex)
            {
                _error = ex.Message;
            }
            
        }
        

        public SerialPortOperation(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _portName = portName;
            _baudRate = baudRate;
            _parity = parity;
            _dataBits = dataBits;
            _stopBits = stopBits;
            InitSerialPort();
        }

        public SerialPortOperation(string portName, int baudRate, int dataBits)
        {
            _portName = portName;
            _baudRate = baudRate;
            _dataBits = dataBits;
            InitSerialPort();
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            byte[] data = new byte[_serialPort.BytesToRead];
            _serialPort.Read(data, 0, data.Length);
            string strDataReceived = TJClient.Devices.Com.Common.ByteToHexStr(data).Trim();
            if (_executeFunction != null)
            {
                _executeFunction(strDataReceived); //获取数据后执行
            }
        }

        public string GetError()
        {
            return _error;
        }
        public bool Open()
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }

                _serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                _serialPort.Close();
                _serialPort.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                return false;
            }
        }

        public bool Send(byte[] command)
        {
            try
            {
                _serialPort.Write(command, 0, command.Length);
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
