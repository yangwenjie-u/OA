using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace BD.JC.JS.Common
{
	public static class DynamicWebService
	{
		/// < summary>          
		/// 动态调用web服务 
		/// < /summary>          
		/// < param name="url">WSDL服务地址< /param>
		/// < param name="classname">类名< /param>  
		/// < param name="methodname">方法名< /param>  
		/// < param name="args">参数< /param> 
		/// < returns>< /returns>
		public static object InvokeWebService(string filename, string classname, string methodname, object[] args, string serviceUrl = "")
		{
			string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
			if ((classname == null) || (classname == ""))
			{
				classname = System.IO.Path.GetFileNameWithoutExtension(filename);
			}
			try
			{                   
				//获取WSDL  
                ServiceDescription sd = null;
                if (serviceUrl != "")
                {
                    WebClient wc = new WebClient();
                    Stream stream = wc.OpenRead(serviceUrl + "?wsdl");
                    sd = ServiceDescription.Read(stream);
                    stream.Close();
                }
                else
                {
                    FileStream stream = new FileStream(filename, FileMode.Open);
                    sd = ServiceDescription.Read(stream);
                    stream.Close();
                }
				
				ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
				sdi.AddServiceDescription(sd, "", "");
				CodeNamespace cn = new CodeNamespace(@namespace);
				//生成客户端代理类代码          
				CodeCompileUnit ccu = new CodeCompileUnit();
				ccu.Namespaces.Add(cn);
				sdi.Import(cn, ccu);
				CSharpCodeProvider icc = new CSharpCodeProvider();
				//设定编译参数                 
				CompilerParameters cplist = new CompilerParameters();
				cplist.GenerateExecutable = false;
				cplist.GenerateInMemory = true;
				cplist.ReferencedAssemblies.Add("System.dll");
				cplist.ReferencedAssemblies.Add("System.XML.dll");
				cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
				cplist.ReferencedAssemblies.Add("System.Data.dll");
				//编译代理类                 
				CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
				if (true == cr.Errors.HasErrors)
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
					{
						sb.Append(ce.ToString());
						sb.Append(System.Environment.NewLine);
					}
					throw new Exception(sb.ToString());
				}
				//生成代理实例，并调用方法   
				System.Reflection.Assembly assembly = cr.CompiledAssembly;
				Type t = assembly.GetType(@namespace + "." + classname, true, true);
				object obj = Activator.CreateInstance(t);
				System.Reflection.MethodInfo mi = t.GetMethod(methodname);
				return mi.Invoke(obj, args);
				// PropertyInfo propertyInfo = type.GetProperty(propertyname);     
				//return propertyInfo.GetValue(obj, null); 
			}
			catch (Exception ex)
			{
			}
			return null;
		}
		/// <summary>
		/// 从url获取class名称
		/// </summary>
		/// <param name="wsUrl"></param>
		/// <returns></returns>
		private static string GetWsClassName(string wsUrl)
		{
			string[] parts = wsUrl.Split('/');
			string[] pps = parts[parts.Length - 1].Split('.');
			return pps[0];
		}
		/// <summary>
		/// 获取返回对象的各列
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static IList<DynamicWebServiceField> GetFields(object obj)
		{
			IList<DynamicWebServiceField> ret = new List<DynamicWebServiceField>();

			Type Tp = obj.GetType();
			FieldInfo[] fields = Tp.GetFields();
			for (int i = 0; i < fields.Length; ++i)
			{
				FieldInfo fi = fields.GetValue(i) as FieldInfo;

				DynamicWebServiceField field = new DynamicWebServiceField() { FieldName = fi.Name, FieldValue = fi.GetValue(obj) };
				ret.Add(field);
			}
			return ret;
		}


	}
	/// <summary>
	/// 返回复杂对象的列
	/// </summary>
	public class DynamicWebServiceField
	{
		public string FieldName { get; set; }
		public object FieldValue { get; set; }
	}
}
