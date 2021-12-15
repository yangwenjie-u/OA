using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;

namespace BD.Jcbg.Common
{
	/// <summary>
	/// 系统环境
	/// </summary>
	public class SysEnvironment
	{
		// 是否是web应用环境
		public static bool IsWeb { get; set; }
		// 获取当前路径
		public static string CurPath
		{
			get
			{
				if (IsWeb)
					return CurWebPath;
				else
					return CurClientPath;
			}
		}
		// 当前Web绝对路径
		protected static string CurWebPath
		{
			get
			{
				return HttpRuntime.AppDomainAppPath;
			}
		}
		// 当前客户端程序的路径
		protected static string CurClientPath
		{
			get
			{
				string assemblyFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
				string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
				return assemblyDirPath;
			}
		}

        protected static string mIP = "";
        public static string IP
        {
            get
            {
                if (string.IsNullOrEmpty(mIP))
                {
                    String[] Ips = GetLocalIpAddress();

                    foreach (String ip in Ips)
                        if (ip.StartsWith("10.80."))
                            mIP = ip;
                    if (string.IsNullOrEmpty(mIP))
                    {
                        foreach (String ip in Ips)
                            if (ip.Contains("."))
                                mIP = ip;
                    }
                    if (string.IsNullOrEmpty(mIP))
                        mIP = "127.0.0.1";
                }
                return mIP;
            }
        }

        private static String[] GetLocalIpAddress()
        {
            string hostName = Dns.GetHostName();                    //获取主机名称  
            IPAddress[] addresses = Dns.GetHostAddresses(hostName); //解析主机IP地址  

            string[] IP = new string[addresses.Length];             //转换为字符串形式  
            for (int i = 0; i < addresses.Length; i++) IP[i] = addresses[i].ToString();

            return IP;
        }
    }
}
