using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using System.Data;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.WorkFlow.IBll;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using BD.Jcbg.Web.Func;
using System.Threading;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Collections;
using BD.Jcbg.Web.ViewModels;
using Newtonsoft.Json;
//using NPOI.XWPF.UserModel;

namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 温州瓯海监督站个性化控制器
    /// </summary>
    public class DwgxWzOh_ProxyController : Controller
    {
        public string Proxy(string url,string param,string type="post")
        {
            string result = "";
            try
            {
                var host = Configs.GetConfigItem("VideoMonitorUrl");
                var token = Configs.GetConfigItem("VideoMonitorToken");
                if (type == "post")
                {
                    result = MyHttp.HttpPost(host + url, param, "application/json", token);
                }
                else
                {
                    result = MyHttp.HttpGet(host + url, param, token);
                }
                
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return result;
        }
        /// <summary>
        /// 和监测监管对接的代理方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string ProxyForExperiment(string url, string param, string type = "post")
        {
            string result = "";
            JsonData json = new JsonData();
            try
            {
                string stationId = Configs.GetConfigItem("JCJGStationId"); //"F8C0243D674E47FEBAA5DEB41134906E";
                string key = Configs.GetConfigItem("JCJGKEY"); //"D153D200EF1D3A1F009D8C9C967E3AA9";
                var host = "http://wzjcjg.jzyglxt.com";
                var paramList = JsonConvert.DeserializeObject<List<paramModel>>(param);
                string paramStr = "StationId="+ stationId+"&Key="+ key;
                foreach (var p in paramList)
                {
                    paramStr = paramStr + "&" + p.Name + "=" + p.Value;
                }
                if (type == "post")
                {
                    result = MyHttp.SendDataByPost(host + url, paramStr);
                   
                }
                else
                {
                    result = MyHttp.SendDataByGET(host + url + "?" + paramStr);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return result;
        }

    }
    public class paramModel
    { 
        public string Name { get; set; }

        public string Value { get; set; }
    }
}