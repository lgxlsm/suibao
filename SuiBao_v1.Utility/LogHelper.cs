using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiBao_v1.Utility
{
    public static class LogHelper
    {
        /// <summary>
        /// 异常日志信息
        /// </summary>
        /// <param name="strMessage">异常日志信息</param>
        public static void Exception(string strMessage)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("wolflogInfo");
            if (log.IsErrorEnabled)
            {
                log.Error(strMessage);
            }
        }

        /// <summary>
        /// 普通的日志信息
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("wolflogInfo");
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// 短信日志信息
        /// </summary>
        /// <param name="message">日志内容</param> 
        public static void SmsLog(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("wolflogInfo");
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// 权益日志
        /// </summary>
        /// <param name="message">日志内容</param> 
        public static void RightLog(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("wolflogInfo");
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }
    }
}
