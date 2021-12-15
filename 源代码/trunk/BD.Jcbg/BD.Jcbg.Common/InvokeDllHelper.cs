using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BD.Jcbg.Common
{
    public class InvokeDllHelper
    {
        public static bool InvokeCalculate(string wtdwyh)
        {
            if (string.IsNullOrEmpty(wtdwyh))
            {
                return false;
            }

            string dllName = "BD.JC.JS.Common.dll";
            string dllClass = "BD.JC.JS.Common.ComSetJcyj";
            string dllMethod = "Calculate";

            return InvokeDll(wtdwyh, dllName, dllClass, dllMethod);
        }

        public static bool InvokeDll(string jydbh, string dllName, string dllClass, string dllMethod)
        {
            string dllPath = String.Format(@"{0}bin\{1}", SysEnvironment.CurPath, dllName);
            if (File.Exists(dllPath))
            {
                string jsonData = String.Empty;
                ReturnParam dllParam = null;
                Assembly assembly;
                Type type;
                MethodInfo mi;

                try
                {
                    assembly = Assembly.LoadFile(dllPath);
                    type = assembly.GetType(dllClass, true);
                    object obj = System.Activator.CreateInstance(type);
                    //通过方法名称获得方法
                    mi = type.GetMethod(dllMethod);
                    if (mi != null)
                    {
                        //对方法进行调用,多态性利用参数进行控制
                        jsonData = (string)mi.Invoke(obj, new object[] { jydbh });
                        //分析
                        dllParam = JsonSerializer.Deserialize<ReturnParam>(jsonData);
                        if (dllParam.code)
                            return true;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteError(String.Format("保存成功，计算出错，原因：{0}", e.Message));
                }
            }

            return false;
        }
    }
}
