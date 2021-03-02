using System;
using System.Collections.Generic;
using System.Linq;
using System.Logger;
using System.Web;

namespace clientDoWebService.common
{
    public class TxtLogger
    {
        public enum WriteType
        {
            Dubug=1,
            Error=2
        }
        public static bool Debug(string message)
        {
            WriteMessage(message, WriteType.Dubug);
            return true;
        }

        public static bool Error(string message)
        {
            WriteMessage(message, WriteType.Error);
            return true;
        }

        public static bool WriteMessage(string message, TxtLogger.WriteType type)
        {
            SimpleLogger logger = SimpleLogger.GetInstance();
            try
            {

                switch (type)
                {
                    case WriteType.Error:
                        logger.Error(message);
                        break;
                    default:
                        logger.Debug(message);
                        break;
                }
            }
            catch
            {
            }
            return true;
        }
    }
}