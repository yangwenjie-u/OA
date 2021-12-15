using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.WorkFlow.IBll;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using Spring.Context;
using Spring.Context.Support;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;

namespace BD.Jcbg.Web.Func
{
    public partial class ReportPrintMethodNew:ReportPrint.ReportPrintBaseMethod
    {
        #region 服务
        private IWorkFlowService _workflowService = null;
        private IWorkFlowService WorkFlowService
        {
            get
            {
                if (_workflowService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _workflowService = webApplicationContext.GetObject("WorkFlowService") as IWorkFlowService;
                }
                return _workflowService;
            }
        }
        #endregion
        #region 绍兴市建筑业资质管理系统
        /// <summary>
        /// 格式化资质申请类型
        /// </summary>
        /// <returns></returns>
        public string FormatZzsqlx()
        {
            string ret = "\u25A1";
            List<string> ls = datas["JDBG_QYZZSB_XQ"].Where(x => x["itemname"].GetSafeString() == "zzsblxbh").Select(x => x["itemvalue"].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                ret = v == Param1 ? "\u2611" : "\u25A1";
            }
            return ret;
        }

        /// <summary>
        /// 格式化是否
        /// </summary>
        /// <returns></returns>
        public string FormatSF()
        {
            string ret = "\u25A1";
            List<string> ls = datas["JDBG_QYZZSB_XQ"].Where(x => x["itemname"].GetSafeString() == Param1).Select(x => x["itemvalue"].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                ret = v == Param2 ? "\u2611" : "\u25A1";
            }
            return ret;
        }

        public string FormatHCQKSF()
        {
            string ret = "\u25A1";
            List<string> ls = datas[TableName].Where(x => x["itemname"].GetSafeString() == Param1).Select(x => x[FieldName].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                ret = v == Param2 ? "\u2611" : "\u25A1";
            }
            return ret;
        }

        /// <summary>
        /// 格式化港澳台投资方
        /// </summary>
        /// <returns></returns>
        public string FormatGATTZF()
        {
            string ret = "\u25A1";
            List<string> ls = datas["JDBG_QYZZSB_XQ"].Where(x => x["itemname"].GetSafeString() == Param1).Select(x => x["itemvalue"].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                ret = v == Param2 ? "\u2611" : "\u25A1";
            }
            return ret;
        }

        /// <summary>
        /// 格式化现有资质信息
        /// 把\n替换成\r\n
        /// </summary>
        /// <returns></returns>
        public string FormatXYZZXX()
        {
            string ret = "";
            List<string> ls = datas["JDBG_QYZZSB_XQ"].Where(x => x["itemname"].GetSafeString() == Param1).Select(x => x["itemvalue"].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                ret = v.Replace("\n", "\r\n");
            }
            return ret;
        }

        /// <summary>
        /// 格式化工程承包方式
        /// </summary>
        /// <returns></returns>
        public string FormatGccbfs()
        {
            string ret = "\u25A1";
            IDictionary<string, object> info = datas[TableName][Index];
            if (info != null)
            {
                string v = info[FieldName].GetSafeString();
                ret = v == Param1 ? "\u2611" : "\u25A1";
            }
            return ret;
        }

        /// <summary>
        /// 格式化施工组织方式
        /// </summary>
        /// <returns></returns>
        public string FormatSgzzfs()
        {
            string ret = "\u25A1";
            IDictionary<string, object> info = datas[TableName][Index];
            if (info != null)
            {
                string v = info[FieldName].GetSafeString();
                ret = v == Param1 ? "\u2611" : "\u25A1";
            }
            return ret;
        }
        /// <summary>
        /// 格式化资质变更类型
        /// </summary>
        /// <returns></returns>
        public string FormatZZBGLX()
        {
            string ret = "\u25A1";
            List<string> ls = datas["JDBG_QYZZSB_XQ"].Where(x => x["itemname"].GetSafeString() == Param1).Select(x => x["itemvalue"].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                ret = v == Param2 ? "\u2611" : "\u25A1";
            }
            return ret;
        }

        /// <summary>
        ///  二维码 -- 资质申请表url
        /// </summary>
        /// <returns></returns>
        public string FormatzzsqbUrl()
        {
            string ret = "http://220.191.224.245:22002/dwgxsxjzy/ViewQyzzsqFile?id=";
            List<string> ls = datas["JDBG_QYZZSB_XQ"].Where(x => x["itemname"].GetSafeString() == "id").Select(x => x["itemvalue"].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                if (v != "")
                {
                    ret += v;
                }
                ret += "&reporttype=ZZSQB";

            }
            SysLog4.WriteError(ret);
            return ret;
        }

        #endregion

        #region 诸暨市智慧监管云平台
        /// <summary>
        /// 格式化设备备案产权登记资料审核
        /// </summary>
        /// <returns></returns>
        public string FormatSbcqdjzlsh()
        {
            string ret = "\u25A1";
            List<string> ls = datas[TableName].Select(x=>x[FieldName].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                if (v !="")
                {
                    List<string> vl = v.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries).ToList();
                    ret = v.Contains(Param1)? "\u2611" : "\u25A1";
                }
                
            }
            return ret;
        }

        /// <summary>
        /// 二维码 -- 格式化设备产权备案表URL
        /// </summary>
        /// <returns></returns>
        public string FormatSbcqbaUrl()
        {
            string ret = "http://zjzhjg.jzyglxt.com/dwgxzj/ViewSbcqba?id=";
            List<string> ls = datas[TableName].Select(x => x[FieldName].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                if (v != "")
                {
                    ret += v;
                }

            }
            return ret;
        }

        /// <summary>
        ///  二维码 -- 格式化设备安装、使用、拆卸告知表url
        /// </summary>
        /// <returns></returns>
        public string FormatAzsycxGzbUrl()
        {
            string ret = "http://zjzhjg.jzyglxt.com/dwgxzj/ViewSBReport?serial=";
            List<string> ls = datas["view_sb_reportsbsy"].Select(x => x["workserial"].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                string v = ls[0];
                if (v != "")
                {
                    ret += v;
                }
                ret += "&reporttype=" + Param1;

            }
            return ret;
        }
        #endregion

        #region 温州市建设工程质量监督站
        public string FormatStFile()
        {
            string s = "";
            var filelist = datas[TableName];
            if (filelist !=null && filelist.Count >0)
            {
                // where 条件 过滤
                if (ReportPrintField.conditions !=null && ReportPrintField.conditions.Count > 0)
                {
                    foreach (var item in ReportPrintField.conditions)
                    {
                        string[] winfo = item.Split(new char[] { '=' });
                        if (winfo.Length == 2)
                        {
                            string key = winfo[0];
                            string value = winfo[1];
                            //SysLog4.WriteError(string.Format("FormatStFile: where, {0},{1},index,{2}", key, value,Index.ToString()));
                            if (value.Contains("%?"))
                            {
                                var pattern = value.Replace("%?", ".*");
                                Regex tmpReg = new Regex(pattern, RegexOptions.IgnoreCase);
                                filelist = filelist.Where(x => tmpReg.Match(x[key].GetSafeString()).Success).ToList();
                            }
                            else if (value.StartsWith("%REGEX%"))
                            {
                                var pattern = value.Substring(8);
                                Regex tmpReg = new Regex(pattern, RegexOptions.IgnoreCase);
                                filelist = filelist.Where(x => tmpReg.Match(x[key].GetSafeString()).Success).ToList();
                            }
                            else
                            {
                                filelist = filelist.Where(x => x[key].GetSafeString() == value.GetSafeString()).ToList();
                            }
                            
                        }
                    }
                }
                // 排序
                if (ReportPrintField.orderbys != null && ReportPrintField.orderbys.Count > 0)
                {
                    foreach (var item in ReportPrintField.orderbys)
                    {
                        ReportPrintService.EnumExcelOrderByField obf = item.orderby;
                        if (obf == ReportPrintService.EnumExcelOrderByField.Reverse)
                        {
                            filelist = filelist.Reverse().ToList();
                        }
                        else if(obf == ReportPrintService.EnumExcelOrderByField.Asc)
                        {
                            filelist = filelist.OrderBy(x => x[item.fieldname]).ToList();
                        }
                        else if (obf == ReportPrintService.EnumExcelOrderByField.Desc)
                        {
                            filelist = filelist.OrderByDescending(x => x[item.fieldname]).ToList();
                        }
                        else if (obf == ReportPrintService.EnumExcelOrderByField.NumAsc)
                        {
                            filelist = filelist.OrderBy(x => x[item.fieldname].GetSafeDouble()).ToList();
                        }
                        else if (obf == ReportPrintService.EnumExcelOrderByField.NumDesc)
                        {
                            filelist = filelist.OrderByDescending(x => x[item.fieldname].GetSafeDouble()).ToList();
                        }


                    }
                }

                newDatas = filelist;
                bool isbig = Param1.GetSafeBool();
                string fileid = filelist[Index]["fileid"].GetSafeString();
                if (filelist[Index].ContainsKey(TableName + FieldName + fileid))
                {
                    s = filelist[Index][TableName + FieldName + fileid].GetSafeString();
                }
                else
                {
                    byte[] file = WorkFlowService.GetFileContentOrThumbnail(fileid.GetSafeInt(), isbig);
                    if (file != null && file.Length > 0)
                    {
                        s = file.EncodeBase64();
                        filelist[Index].Add(TableName + FieldName + fileid, s);
                    }
                }


            }


            return s;

        }

        public string FormatStFile2()
        {
            string s = "";
            s = Index.ToString() +":" + Param1.GetSafeString();
            
            return s;

        }


        #endregion

        #region 公共使用

        /// <summary>
        /// 根据表名、字段名获取字段值
        /// 如果有多条记录，取第一条记录的值
        /// </summary>
        /// <returns></returns>
        private object GetFieldValueByTableAndField(string table, string field, string searchV="")
        {
            object ret = null;

            List<object> ls = null;
            if (searchV == "")
            {
                ls = datas[table].Select(x => x[field]).ToList();
            }
            else
            {
                ls = datas[table].Select(x => x[field]).Where(x => x.GetSafeString() == searchV).ToList();
            }
            if (ls.Count > 0)
            {
                ret = ls[0];
            }
            return ret;
        }
        #endregion


    }
}