using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace BD.Jcbg.Common
{
	public static class SysLog4
	{
		private static readonly log4net.ILog m_loginfo = log4net.LogManager.GetLogger("InfoLog");
		private static readonly log4net.ILog m_logerror = log4net.LogManager.GetLogger("ErrorLog");

		public static void SetConfig()
		{
			string configFilePath = SysEnvironment.CurPath + "\\configs\\log4net.xml";

			log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilePath));

		}

		public static void WriteLog(string info)
		{
			if (m_loginfo.IsInfoEnabled)
			{
				m_loginfo.Info(info + "\r\n" + GetStackTraceInfo());
			}
		}

		public static void WriteError(string info)
		{
            //File.AppendAllText(SysEnvironment.CurPath + "\\logs\\temp.log", "["+DateTime.Now.ToString("HH:mm:ss")+"]"+ info + "\r\n");
            //return;
            if (m_loginfo.IsErrorEnabled)
			{
                m_logerror.Error(info + "\r\n" + GetStackTraceInfo());
			}
		}

		public static void WriteLog(string info, Exception se)
		{
			if (m_logerror.IsErrorEnabled)
			{
				m_logerror.Error(info + "\r\n" + GetStackTraceInfo(), se);
			}
		}

		public static void WriteLog(Exception se)
		{
			if (m_logerror.IsErrorEnabled)
			{
				m_logerror.Error(GetStackTraceInfo(), se);
			}
		}
		
		public static string GetStackTraceInfo()
		{
            return "";
            //StringBuilder sb = new StringBuilder();
            //StackTrace trace = new StackTrace(1, true);
            //StackFrame[] frames = trace.GetFrames();

            //foreach (StackFrame sf in frames)
            //{
            //    if (sf == frames[0])
            //        continue;
            //    if (sf == null)
            //        continue;
            //    string file = "", line = "";
            //    System.Reflection.MethodBase method = sf.GetMethod();
            //    if (method != null)
            //    {
            //        file = method.ReflectedType.GetSafeString();
            //        line = method.Name;
            //    }
            //    sb.Append(string.Format("\t在{2}.{3} 位置{0} 行号：{1}\r\n",
            //        sf.GetFileName(), sf.GetFileLineNumber(), file, line));
            //}

            //return sb.ToString();
        }
	}
}
