using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
	/// <summary>
	/// 列表按深度优先排序，可以排除一些节点
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TreeOrder<T>
	{
		IList<T> Objects = null;		// 待排序的节点
		string ParentIdField = "";		// 存储父节点字段名
		string IdField = "";			// ID字段名，逗号分隔
		string IdSplit = "";			// 多个字段组成ID时分隔符
		string TextField = "";			// 显示字符字段名，逗号分隔
		string TextSplit = "";			// 多个字段组成Text时分隔符
		string FirstParentValue = "";	// 第一个父节点值
		string FirstParentText = "";	// 第一个父节点文字，显示成Json时用到
		bool DisplayFirstParent = false;	// 是否显示第一个父节点，显示成Json时用到
		string ExecludeRoots = "";		// 排除的节点，逗号分隔

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lst">节点列表，也可以后续传入</param>
		/// <param name="idfield">键值字段列表，逗号分隔</param>
		/// <param name="parentidfield">存储父节点字段名</param>
		/// <param name="firstparentvalue">第一个父节点键值</param>
		/// <param name="execluderoots">排除的节点列表，多个逗号分隔，同时会排除子节点</param>
		/// <param name="textfield">显示字段列表，逗号分隔</param>
		/// <param name="idsplit">多个字段组成键值时分隔符</param>
		/// <param name="textsplit">多个字段组成显示值时分隔符</param>
		/// <param name="firstparenttext">第一个父节点显示值，组成JSON时用到</param>
		/// <param name="dispfirstparent">是否显示第一个父节点，组成JSON时用到</param>
		public TreeOrder(IList<T> lst, string idfield, string parentidfield, string firstparentvalue = "0", 
			string execluderoots="", string textfield = "",	string idsplit="_", string textsplit="_", 
			string firstparenttext="根目录", bool dispfirstparent=true)
		{
			Objects = lst;
			IdField = idfield;
			TextField = textfield;
			FirstParentValue = firstparentvalue;
			ExecludeRoots = execluderoots;
			IdSplit = idsplit;
			TextSplit = textsplit;
			FirstParentText = firstparenttext;
			DisplayFirstParent = dispfirstparent;
			ParentIdField = parentidfield;
		}
		/// <summary>
		/// 获取深度优先排序的列表
		/// </summary>
		/// <param name="lst">如果为空，排序构造函数中的列表</param>
		/// <returns></returns>
		public IList<T> GetOrderTree(IList<T> lst=null)
		{
			if (lst == null)
				lst = Objects;

			IList<T> ret = new List<T>();
			if (lst == null)
				return ret;
			
			try
			{
				// 排除列表
				List<string> exclude = new List<string>();
				if (ExecludeRoots != "")
				{
					string[] arr = ExecludeRoots.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					exclude.AddRange(arr);
				}
				// 排除列表的所有子节点，包含自己
				List<T> excludeNodes = new List<T>();
				foreach (string str in exclude)
					excludeNodes.AddRange(GetSubNodes(lst, str, true));
				// 把非排除节点添加到返回列表中
				foreach (T node in lst)
				{
					var q = from e in excludeNodes where GetIdValue(e).Equals(GetIdValue(node)) select e;
					if (q.Count() == 0)
						ret.Add(node);
				}
				// 调用获取子节点函数
				ret = GetSubNodes(ret, FirstParentValue, false);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;

		}
		/// <summary>
		/// 获取节点的子节点列表，并按深度优先排序
		/// </summary>
		/// <param name="lst"></param>
		/// <param name="nodeid">根节点</param>
		/// <param name="includeself">是否包含根节点</param>
		/// <returns></returns>
		public IList<T> GetSubNodes(IList<T> lst, string nodeid, bool includeself=false)
		{
			List<T> ret = new List<T>();
			if (lst == null)
				return ret;
			IList<string> parentNodes = new List<string>();
			parentNodes.Add(nodeid);
			
			try
			{
				// 添加自己
				if (includeself)
				{
					var q = from e in lst where GetIdValue(e).Equals(nodeid) select e;
					if (q.Count()>0)
						ret.Add(q.First());
				}
				// 循环添加子节点
				while (parentNodes.Count > 0)
				{
					string curParent = parentNodes[0];
					parentNodes.RemoveAt(0);
					// 父节点所有子节点
					var q = from e in lst where e.MyGetObjectProperty(ParentIdField).Equals(curParent) select e;
					// 父节点在已添加列表的索引
					var qadd = from e in ret where GetIdValue(e).Equals(curParent) select e;
					int index = -1;
					if (qadd.Count() > 0)
						index = ret.IndexOf(qadd.First());
					// 添加子节点到已添加节点
					if (index == -1)
						ret.AddRange(q);
					else
						ret.InsertRange(index + 1, q);
					// 添加子节点到父节点
					foreach (T itm in q)
						parentNodes.Add(GetIdValue(itm));
				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}

			return ret;

		}
		/// <summary>
		/// 获取Json格式，easyui中的tree和combotree使用
		/// </summary>
		/// <param name="lst">如果为空，获取构造函数中的列表</param>
		/// <returns></returns>
		public string GetJsonTree(IList<T> lst =null)
		{
			if (lst == null)
				lst = Objects;
			// 排序列表
			IList<T> orderlst = GetOrderTree(lst);
			
			StringBuilder sb = new StringBuilder();
			// 输出成Json
			sb.Append("[");
			try
			{
				if (DisplayFirstParent)
					sb.Append("{\"id\":\"" + FirstParentValue + "\",\"text\":\"" + FirstParentText + "\",\"children\":[");

				for (int i=0; i<orderlst.Count; i++)
				{
					T node = orderlst[i];
					// 是否有子节点
					bool haschild = GetSubNodes(orderlst, GetIdValue(node), false).Count > 0;
					// 当前深度
					int curLevel = GetLevel(orderlst, node);
					// 上一个深度
					int preLevel = 0;
					if (i != 0)
						preLevel = GetLevel(orderlst, orderlst[i - 1]);
					
					// 前面已经有兄弟节点，添加逗号
					if (curLevel == preLevel && i!=0)
						sb.Append(",");
					// 当前是上层节点，添加括号和逗号
					else if (curLevel<preLevel)
					{
						int diffLevel = curLevel - preLevel;
						while (diffLevel-- > 0)
						{
							sb.Append("]}");
						}
						sb.Append(",");
					}

					sb.Append("{\"id\":\"" + GetIdValue(node) + "\",\"text\":\"" + GetTextValue(node) + "\"");
					if (haschild)
						sb.Append(",\"children\":[");
					else
						sb.Append("}");

					// 补上最后一个节点深度个]}
					if (i == orderlst.Count - 1)
					{
						for (int j = 0; j < curLevel; j++)
							sb.Append("]}");
					}

				}
				

				if (DisplayFirstParent)
					sb.Append("]}");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			sb.Append("]");
			return sb.ToString();

		}
		/// <summary>
		/// 获取对象的ID值
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		private string GetIdValue(T node)
		{
			string ret = "";
			try
			{
				ret = GetValue(node, IdField, IdSplit);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 获取对象的显示值
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		private string GetTextValue(T node)
		{
			string ret = "";
			try
			{
				ret = GetValue(node, TextField, TextSplit);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 获取多个字段的值，以split分隔
		/// </summary>
		/// <param name="node"></param>
		/// <param name="fields"></param>
		/// <param name="split"></param>
		/// <returns></returns>
		private string GetValue(T node, string fields, string split)
		{
			string ret = "";
			try
			{
				string[] arr = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string str in arr)
				{
					if (ret != "")
						ret += split;
					ret += node.MyGetObjectProperty(str);
				}

			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 获取节点深度，第一层返回0，依次递增
		/// </summary>
		/// <param name="lst"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		private int GetLevel(IList<T> lst, T node)
		{
			int ret = 0;
			try
			{
				// 查找父节点
				do
				{
					var q = from e in lst where GetIdValue(e).Equals(node.MyGetObjectProperty(ParentIdField)) select e;
					if (q.Count() == 0)
						break;
					ret++;
					node = q.First();
				} while (true);
				
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
	}
}
