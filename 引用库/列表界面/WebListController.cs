using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;
using Newtonsoft.Json;
using BD.WebListCommon;
using BD.WebListParam;
using BD.IWebListBll;
using BD.WebListDataModel.Entities;

namespace Web.Controllers
{
    public class WebListController : BaseController
    {
        #region 列表显示界面
        #region 界面
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {
            bool isencode = false;

            ListParam param = new ListParam();
            param.FormDm = "HTDJ".GetBDEncode(isencode);//ZGHMC
            param.FormStatus = "0".GetBDEncode(isencode);
            param.FormParam = "Gridline--solidline||CHECKBOX--切11,切2|切2||SELECT--所有,切1,切2|所有||Gridwrap--1||PARAM--1=1|DYBB".GetBDEncode(isencode);
            param.FormHidden = "name,0|mc,1".GetBDEncode(isencode);
            param.Log = "0".GetBDEncode(isencode);
            param.encode = isencode;
            SessionMgr.USERNAME = "UR201805000883";
            SessionMgr.USERCODE = "BD";
            return RedirectToAction("EasyUIIndex", "WebList", param);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult Index(ListParam param)
        {
            //SessionMgr.USERNAME = "jcxtadmin";
            //SessionMgr.SetSession("QYBH", "Q000393");
            //判断sessioin是否存在
            if (SessionMgr.USERNAME == "")
            {
                return View("ErrorTimeout");
            }
            IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
            ////判断特殊字符
            //if (webListService.CheckParam(param))
            //{
            //    return View("Error","");
            //}
            //保留原参数信息
            string sourceparam = Base64Util.EncodeBase64(JsonConvert.SerializeObject(param));
            //设置后台解密
            param.isbackground = true;
            //判断是否有权限
            Form form = webListService.GetForm(param);
            if (form == null)
            {
                return View("Error");
            }
            //判断是否在权限中
            if (form.MenuCode.GetString() != "" && !webListService.CheckPower(form.MenuCode.GetString()))
            {
                return View("Error");
            }

            //参数编码
            ViewData["param"] = sourceparam;
            //引用JS文件
            ViewData["css"] = webListService.GetCustomCssList(param);
            //引用JS文件
            ViewData["js"] = webListService.GetCustomJsList(param);
            //自定义隐藏字段
            ViewData["hidden"] = webListService.GetCustomHiddenList(param);
            //显示Form
            ViewData["form"] = webListService.GetForm(param);
            //显示字段字典
            ViewData["zdzd"] = webListService.GetContentList(param);

            return View();
        }

        public ActionResult ElementIndex(ListParam param)
        {
            //判断sessioin是否存在
            //if (param.CheckSession != "no" && SessionMgr.USERNAME == "")
            //{
            //    return View("ErrorTimeout");
            //}
            IApplicationContext webApplicationContext = ContextRegistry.GetContext();
            IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
            //保留原参数信息
            string sourceparam = Base64Util.EncodeBase64(JsonConvert.SerializeObject(param));
            //设置后台解密
            param.isbackground = true;
            //判断是否有权限
            Form form = webListService.GetForm(param);
            if (form == null)
            {
                return View("Error");
            }
            //判断是否在权限中
            if (form.MenuCode.GetString() != "" && !webListService.CheckPower(form.MenuCode.GetString()))
            {
                return View("Error");
            }
            //参数编码
            ViewData["param"] = sourceparam;
            //引用JS文件
            ViewData["css"] = webListService.GetCustomCssList(param);
            //引用JS文件
            ViewData["js"] = webListService.GetCustomJsList(param);
            //自定义隐藏字段
            ViewData["hidden"] = webListService.GetCustomHiddenList(param);
            //显示Form
            ViewData["form"] = webListService.GetForm(param);
            //显示字段字典
            ViewData["zdzd"] = webListService.GetContentList(param);
            //列表换行
            ViewData["gridwrap"] = webListService.GetGridwrap(param);
            //显示虚实线
            ViewData["gridline"] = webListService.GetGridline(param);
            return View();
        }
        #endregion



        #region 数据包
        /// <summary>
        /// 导出数据
        /// </summary>
        public void ExportData() {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.ExportData();
                if (!ret.Successed)
                    msg = ret.Msg;
                else
                    msg = ret.Data;
            }
            catch (Exception ex)
            {
                msg = String.Format("导出数据出错，原因：{0}", ex.Message);
            }
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            resp.Clear();
            resp.Buffer = true;
            resp.Charset = "GB2312";
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("导出信息", System.Text.Encoding.UTF8).ToString() + ".csv");
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
            resp.Write(msg);
            resp.End();
        }

        /// <summary>
        /// 加密导出
        /// </summary>
        public void ExportEncData() {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.ExportEncData();
                if (!ret.Successed)
                    System.Web.HttpContext.Current.Response.Write(ret.Msg);
            }
            catch (Exception ex)
            {
                msg = String.Format("导出数据出错，原因：{0}", ex.Message);
                System.Web.HttpContext.Current.Response.Write(msg);
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        public void ExportExcelData() {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.ExportExcelData();
                if (!ret.Successed)
                    System.Web.HttpContext.Current.Response.Write(ret.Msg);
            }
            catch (Exception ex)
            {
                msg = String.Format("导出数据出错，原因：{0}", ex.Message);
                System.Web.HttpContext.Current.Response.Write(msg);
            }
        }

        /// <summary>
        /// 初始化界面数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult InitFormData() {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.InitFormData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }

        /// <summary>
        /// 查询数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult SearchFormData() {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.SearchFormData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = ret.Data;// JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                //msg = "       [{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"东城区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"西城区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"崇文区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"宣武区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"朝阳区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"丰台区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"石景山区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"海淀区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"门头沟区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"房山区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"通州区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"顺义区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"昌平区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"大兴区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"怀柔区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"平谷区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"密云县\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"延庆县\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"和平区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"河东区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"河西区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"南开区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"河北区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"红桥区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"塘沽区\"}           ]";
                //msg = "       {     \"total\":3213,           \"rows\":           [{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"东城区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"西城区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"崇文区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"宣武区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"朝阳区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"丰台区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"石景山区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"海淀区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"门头沟区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"房山区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"通州区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"顺义区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"昌平区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"大兴区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"怀柔区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"平谷区\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"密云县\"},{\"SZSF\":\"北京市\",\"SZCS\":\"北京市\",\"SZXQ\":\"延庆县\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"和平区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"河东区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"河西区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"南开区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"河北区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"红桥区\"},{\"SZSF\":\"天津市\",\"SZCS\":\"天津市\",\"SZXQ\":\"塘沽区\"}           ]       }";
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }
        #endregion

        #region 触发事件CtrlString与HelpLink
        /// <summary>
        /// 获取CtrlString数据(触发改变第三方控件值)
        /// </summary>
        /// <returns></returns>
        public ContentResult CtrlStringData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.CtrlStringData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(String.Format("提示：{0}", ex.Message));
            }
            return Content(msg);
        }


        #endregion

        #region Helplink
        /// <summary>
        /// helplink内容
        /// </summary>
        /// <param name="helplink"></param>
        /// <returns></returns>
        public ActionResult Helplink(string helplink)
        {
            IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
            //helplink参数
            ViewData["helplink"] = helplink;
            //生成显示字段
            ViewData["zdzd"] = webListService.GetHelplinkZdzd(helplink);
            //双击事件
            ViewData["dbclick"] = webListService.GetHelplinkDbClick(helplink);
            return View();
        }

        /// <summary>
        /// 获取helplinkData数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult HelplinkData() 
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.HelplinkData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(String.Format("提示：{0}", ex.Message));
            }
            return Content(msg);
        }
        #endregion

        #endregion

        #region EasyUi列表界面
        #region Helplink
        /// <summary>
        /// 获取过滤下拉Easyui的helplink数据
        /// </summary>
        /// <returns></returns>
        public ContentResult GetHelplinkDataEasyUI() {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.HelplinkDataEasyUI();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(String.Format("提示：{0}", ex.Message));
            }
            return Content(msg);
        }
        #endregion
        #region 新界面
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult LayUIIndex(ListParam param)
        {
            //判断sessioin是否存在
            if (SessionMgr.USERNAME == "")
            {
                return View("ErrorTimeout");
            }
            IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
            //判断特殊字符
            if (webListService.CheckParam(param))
            {
                return View("ErrorChar");
            }
            //保留原参数信息
            string sourceparam = Base64Util.EncodeBase64(JsonConvert.SerializeObject(param));
            //设置后台解密
            param.isbackground = true;
            //判断是否有权限
            Form form = webListService.GetForm(param);
            if (form == null)
            {
                return View("Error");
            }
            //判断是否在权限中
            if (form.MenuCode.GetString() != "" && !webListService.CheckPower(form.MenuCode.GetString()))
            {
                return View("Error");
            }
            //参数编码
            ViewData["param"] = sourceparam;
            //引用JS文件
            ViewData["css"] = webListService.GetCustomCssList(param);
            //引用JS文件
            ViewData["js"] = webListService.GetCustomJsList(param);
            //自定义隐藏字段
            ViewData["hidden"] = webListService.GetCustomHiddenList(param);
            //显示Form
            ViewData["form"] = webListService.GetForm(param);
            //显示字段字典
            ViewData["zdzd"] = webListService.GetContentList(param);
            //列表换行
            ViewData["gridwrap"] = webListService.GetGridwrap(param);
            //显示虚实线
            ViewData["gridline"] = webListService.GetGridline(param);
            return View();
        }
        #endregion
        #region 界面
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult EasyUiIndex(ListParam param)
        {
            //SessionMgr.USERCODE = "wzzz";
            //SessionMgr.USERNAME = "wzzz";
            //判断类型
            if (GlobalUtil.WebListType == "LayUI")
                return RedirectToAction("LayUIIndex", "WebList", param);
            //判断sessioin是否存在
            if (param.CheckSession != "no" && SessionMgr.USERNAME == "")
            {
                return View("ErrorTimeout");
            }
            IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
            //判断特殊字符
            if (webListService.CheckParam(param))
            {
                return View("ErrorChar");
            }
            //保留原参数信息
            string sourceparam = Base64Util.EncodeBase64(JsonConvert.SerializeObject(param));
            //设置后台解密
            param.isbackground = true;
            //判断是否有权限
            Form form = webListService.GetForm(param);
            if (form == null)
            {
                return View("Error");
            }
            //判断是否在权限中
            if (form.MenuCode.GetString() != "" && !webListService.CheckPower(form.MenuCode.GetString()))
            {
                return View("Error");
            }
            //参数编码
            ViewData["param"] = sourceparam;
            //引用JS文件
            ViewData["css"] = webListService.GetCustomCssList(param);
            //引用JS文件
            ViewData["js"] = webListService.GetCustomJsList(param);
            //自定义隐藏字段
            ViewData["hidden"] = webListService.GetCustomHiddenList(param);
            //显示Form
            ViewData["form"] = webListService.GetForm(param);
            //显示字段字典
            ViewData["zdzd"] = webListService.GetContentList(param);
            //列表换行
            ViewData["gridwrap"] = webListService.GetGridwrap(param);
            //显示虚实线
            ViewData["gridline"] = webListService.GetGridline(param);
            return View();
        }


        /// <summary>
        /// 初始化界面数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult InitEasyUIFormData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.InitEasyUIFormData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }

        /// <summary>
        /// 查询数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult SearchEasyUIFormData() {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.SearchEasyUIFormData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = ret.Data;
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }
        #endregion
        #endregion


        #region 列表录入界面
        #region 测试
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public ActionResult TestInput()
        {
            InputParam param = new InputParam();
            param.css = "a.css,b.css";
            param.js = "aa.js,bb.js";
            param.pagesize = "10";
            param.limitsize = "[1,10,20,30]";
            param.button = "mc-查询|funname-searchRecord()|icon-icon-search||mc-新建|funname-addRecord()|icon-icon-add||mc-删除|funname-delRecord()|icon-icon-del";

            param.zdzdtable = "DATAZDZD";
            param.tablename = "H_QYLX";
            param.singleselect = true;
            return RedirectToAction("InputList", "WebList", param);
        }

        #endregion
        #region 界面
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult InputList(InputParam param)
        {
            IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
            //判断是否在权限中
            //if (!webListService.CheckPower(param.menucode))
            //{
            //    return View("Error");
            //}
            //参数编码
            ViewData["param"] = Base64Util.EncodeBase64(JsonConvert.SerializeObject(param));
            //引用JS文件
            ViewData["css"] = webListService.GetInputCssList(param);
            //引用JS文件
            ViewData["js"] = webListService.GetInputJsList(param);
            return View();
        }
        #endregion

        #region 数据包
        /// <summary>
        /// 初始化界面数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult InitInputFormData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.InitInputFormData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }

        /// <summary>
        /// 查询数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult SearchInputFormData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.SearchInputFormData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = ret.Data;
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public ContentResult SaveData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.SaveData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ContentResult DelData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.DelData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }
        #endregion

        #endregion


        #region EasyUi列表录入界面
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public ActionResult TestInputEasyUI()
        {
            InputParam param = new InputParam();
            param.css = "a.css,b.css";
            param.js = "aa.js,bb.js";
            param.pagesize = "10";
            param.limitsize = "[1,10,20,30]";
            param.button = "mc-查询|funname-searchRecord()|icon-icon-search||mc-新建|funname-addRecord()|icon-icon-add||mc-修改|funname-modifyRecord()|icon-icon-modify||mc-保存|funname-saveRecord()|icon-icon-save||mc-删除|funname-delRecord()|icon-icon-del";

            param.zdzdtable = "DATAZDZD";
            param.tablename = "H_QYLX";
            param.singleselect = true;
            return RedirectToAction("InputEasyUiList", "WebList", param);
        }


        /// <summary>
        /// 列表录入界面
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult InputEasyUiList(InputParam param)
        {
            //判断sessioin是否存在
            if (SessionMgr.USERNAME == "")
            {
                return View("ErrorTimeout");
            }
            IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
            //判断是否在权限中
            //if (!webListService.CheckPower(param.menucode))
            //{
            //    return View("Error");
            //}
             //参数编码
            ViewData["param"] = Base64Util.EncodeBase64(JsonConvert.SerializeObject(param));
            //引用JS文件
            ViewData["css"] = webListService.GetInputCssList(param);
            //引用JS文件
            ViewData["js"] = webListService.GetInputJsList(param);
            return View();
        }

        /// <summary>
        /// 初始化界面数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult InitInputEasyUiFormData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.InitInputEasyUiFormData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }

        /// <summary>
        /// 查询数据包
        /// </summary>
        /// <returns></returns>
        public ContentResult SearchInputEasyUiFormData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.SearchInputEasyUiFormData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = ret.Data;
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }

        /// <summary>
        /// 获取CtrlString数据(触发改变第三方控件值)
        /// </summary>
        /// <returns></returns>
        public ContentResult CtrlStringDataEasyUI()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.CtrlStringDataEasyUI();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(String.Format("提示：{0}", ex.Message));
            }
            return Content(msg);
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public ContentResult SaveEasyUIData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.SaveEasyUIData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ContentResult DelEasyUIData()
        {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.DelEasyUIData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }
        #endregion

        #region 特殊访问
        /// <summary>
        /// 检验按钮数据信息
        /// </summary>
        /// <returns></returns>
        public ContentResult CheckButtonData() {
            string msg = String.Empty;
            try
            {
                IWebListService webListService = (IWebListService)webApplicationContext.GetObject("WebListService");
                ResultParam ret = webListService.CheckButtonData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(ex.Message);
            }
            return Content(msg);
        }
        #endregion

        #region 特殊字符串判断
        
        #endregion
    }
}
