using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using System.Threading;
using System.IO;
using ReportPrint.Common;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 温州市站使用
    /// 往监管平台推送监督抽查联系单
    /// </summary>
    public class JobPushJdcclxd : ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒
        #region 服务
        private ICommonService _commonService = null;
        private ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                }
                return _commonService;
            }
        }

        private IDataFileService _dataFileService = null;
        private IDataFileService DataFileService
        {
            get
            {
                if (_dataFileService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dataFileService = webApplicationContext.GetObject("DataFileService") as IDataFileService;
                }
                return _dataFileService;
            }
        }
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobPushJdcclxd");
            while (true)
            {
                try
                {
                    string msg = "";
                    string sql = "select * from view_jdbg_jdccrwwtjl where isdeal=0 and spzt=1";
                    IList<IDictionary<string, object>> lxdlist = CommonService.GetDataTable2(sql);
                    if (lxdlist.Count > 0)
                    {
                        sql = "select * from h_jcjg_jdcclxd_config where lx='JDCCLXD'";
                        IList<IDictionary<string, string>> configs = CommonService.GetDataTable(sql);
                        string url = "";
                        string fixedparam = "";
                        if (configs.Count > 0)
                        {
                            url = configs[0]["url"];
                            fixedparam = configs[0]["fixedparam"];
                        }

                        foreach (var lxd in lxdlist)
                        {
                            string serial = lxd["workserial"].GetSafeString();
                            int recid = lxd["recid"].GetSafeInt();
                            string fileid = lxd["fileid"].GetSafeString();
                            string filename = "";
                            byte[] word = null;
                            bool hasword = false;
                            // 已经生成word了，获取word数据
                            if (fileid != "")
                            {
                                sql = string.Format("select filename, filecontent,storagetype, fileurl from datafile where fileid='{0}'", fileid);
                                IList<IDictionary<string, object>> flist = CommonService.GetDataTable2(sql);
                                if (flist.Count > 0)
                                {
                                    filename = flist[0]["filename"].GetSafeString();
                                    word = flist[0]["filecontent"] as byte[];
                                    string storagetype = flist[0]["storagetype"].GetSafeString();
                                    string fileurl = flist[0]["storagetype"].GetSafeString();
                                    if(word == null || word.Length == 0)
                                    {
                                        if (storagetype.Equals("oss", StringComparison.OrdinalIgnoreCase) && (fileurl!=""))
                                        {
                                            word = OssCdnUtil.DownFile(url);
                                        }
                                    }
                                    hasword = true;
                                }
                            }
                            else // 否则生成word文档 docx格式
                            {
                                var g = new ReportPrint.GenerateGuid();
                                var c = g.Get();
                                c.type = ReportPrint.EnumType.Word;
                                c.libType = ReportPrint.LibType.OpenXmlSdk;
                                c.openType = ReportPrint.OpenType.Print;
                                c.fileindex = "0";
                                c.table = "view_jdbg_jdccrwwtjl";
                                c.filename = "监督抽查联系单V1";
                                c.where = "workserial=" + serial;
                                c.AllowVisitNum = 1;
                                // 生成word
                                if (g.GetFile(c, out word, out msg))
                                {
                                    //保存word
                                    fileid = Guid.NewGuid().ToString("N");
                                    filename = fileid + ".docx";
                                    bool success = DataFileService.SaveDataFile(fileid, filename, word, ".docx", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out msg);
                                    if (success)
                                    {
                                        sql = string.Format("update jdbg_jdccrwwtjl set fileid='{0}' where recid={1}", fileid, recid);
                                        CommonService.Execsql(sql);
                                        hasword = true;
                                    }
                                    else
                                    {
                                        SysLog4.WriteError(msg);
                                    }
                                    //string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                                    //IList<IDataParameter> sqlparams = new List<IDataParameter>();
                                    //IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                                    //sqlparams.Add(sqlparam);
                                    //sqlparam = new SqlParameter("@FILENAME", filename);
                                    //sqlparams.Add(sqlparam);
                                    //sqlparam = new SqlParameter("@FILECONTENT", word);
                                    //sqlparams.Add(sqlparam);
                                    //sqlparam = new SqlParameter("@FILEEXT", ".docx");
                                    //sqlparams.Add(sqlparam);
                                    //sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //sqlparams.Add(sqlparam);
                                    //// 保存附件成功，需要更新当前联系单
                                    //if (CommonService.ExecTrans(sqlstr, sqlparams, out msg))
                                    //{
                                    //    sql = string.Format("update jdbg_jdccrwwtjl set fileid='{0}' where recid={1}", fileid, recid);
                                    //    CommonService.Execsql(sql);
                                    //    hasword = true;
                                    //}
                                    //else
                                    //{
                                    //    SysLog4.WriteError(msg);
                                    //}
                                }
                            }

                            // 推送联系单
                            if (hasword && (url != "") && (word != null && word.Length > 0))
                            {
                                
                                Dictionary<string, string> datas = new Dictionary<string, string>();
                                foreach (var item in lxd)
                                {
                                    datas.Add(item.Key, item.Value.GetSafeString());
                                }
                                if (fixedparam != "")
                                {
                                    List<string> pl = fixedparam.Split(new char[] { '&' }).ToList();
                                    if (pl.Count > 0)
                                    {
                                        foreach (var item in pl)
                                        {
                                            string[] ps = item.Split(new char[] { '=' });
                                            if (ps.Length == 2)
                                            {
                                                if (!datas.Keys.Contains(ps[0].ToLower()))
                                                {
                                                    datas.Add(ps[0].ToLower(), ps[1]);
                                                }
                                            }
                                        }
                                    }
                                }

                                // word内容base64
                                string wordBase64 = Convert.ToBase64String(GZipUtil.Compress(word));
                                datas.Add("word", wordBase64);
                                // 发送请求
                                string retmsg = "";
                                if (MyHttp.Post(url, datas, null, out retmsg))
                                {
                                    SysLog4.WriteError("retmsg: " + retmsg);
                                    // 解析返回返回结果
                                    if (retmsg != "")
                                    {
                                        JavaScriptSerializer jss = new JavaScriptSerializer();
                                        jss.MaxJsonLength = int.MaxValue;
                                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                                        if (retdata !=null)
                                        {
                                            string code = retdata["code"].GetSafeString();
                                            string message = retdata["msg"].GetSafeString();
                                            if (code == "0")
                                            {
                                                sql = string.Format("update jdbg_jdccrwwtjl set isdeal=1 where recid={0}", recid.ToString());
                                                CommonService.Execsql(sql);
                                            }
                                            else
                                            {
                                                SysLog4.WriteError("recid: " + recid + ", 推送接口调用返回错误，错误信息：" + message);
                                            }
                                        }
                                        else
                                        {
                                            SysLog4.WriteError("recid: " + recid + ", 推送接口调用返回数据格式不正确！");
                                        }
                                    }
                                    else
                                    {
                                        SysLog4.WriteError("recid: " + recid + ", 推送接口调用返回为空！");
                                    }
                                }

                            }
                            else
                            {
                                SysLog4.WriteError("recid : " + recid + ", url为空或者word内容为空");
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);

                }


                Thread.Sleep(Interval);
            }
            
        }
        
        
        
       
       
        
    }
}