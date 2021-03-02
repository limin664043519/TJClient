using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TJClient.sys.Dal;
using Microsoft.Win32;
namespace TJClient.sys.Bll
{
    public class RegisterBll
    {
        public RegisterDal model = new RegisterDal();


        /// <summary>
        /// 验证仪器
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sqlcode"></param>
        /// <returns></returns>
        public bool YqRegister(string yybm, string yymc, string yqxh, string RegisterCode, out string msg )
        {
            msg = "";
            try
            {
                Yibao.Register.Register RegisterDemo = new Yibao.Register.Register();
                string strUserREGISTER = model.isUserREGISTER(yybm);
                //是否进行验证
                if (RegisterDemo.RegisterCheck(DateTime.Now.ToString("yyyy-MM-dd"), yybm + yymc, strUserREGISTER, out msg)==false)
                {
                    string keyCode = "gwtjyq";
                    string bsf = redKey(keyCode);
                    //Yibao.Register.Register RegisterDemo = new Yibao.Register.Register();
                    //string outMsg = "";
                    return RegisterDemo.RegisterCheck(DateTime.Now.ToString("yyyy-MM-dd"), yybm + yymc + yqxh + bsf, RegisterCode, out msg);
                }
                else
                {
                    //不进行验证直接返回验证通过
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }

        /// <summary>
        /// 读取设备标识
        /// </summary>
        public string redKey(string keycode)
        {
            string info = "";
            RegistryKey Key;
            Key = Registry.CurrentUser;
            RegistryKey myreg = Key.OpenSubKey("software\\" + keycode);
            //
            if (myreg != null)
            {
                info = myreg.GetValue(keycode).ToString();
            }
            else
            {
                throw new Exception("设备标识符错误！请联系管理员重新注册设备");
            }
            myreg.Close();
            return info;
        }

        //获取信息
        public DataTable GetMoHuList(string strWhere, string sqlcode)
        {
            return model.GetMoHuList(strWhere, sqlcode);
        }

        //更新
        public bool Upd(DataTable dt, string sqlcode)
        {
            return model.Update(dt, sqlcode);
        }

        public bool Add(DataTable dt, string sqlcode)
        {
            return model.Add(dt, sqlcode);
        }

        public bool Del(DataTable dt, int rowIndex)
        {
            return model.Del(dt, rowIndex);
        }

        public DataTable isExists(string strWhere, string sqlcode)
        {
            return model.GetMoHuList(strWhere, sqlcode);
        }
    }
}
