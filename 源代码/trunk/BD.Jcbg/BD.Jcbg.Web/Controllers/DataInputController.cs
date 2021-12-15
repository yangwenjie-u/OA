using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;
using BD.IDataInputBll;
using BD.DataInputParam;
using BD.DataInputCommon;
using Newtonsoft.Json;
using System.Text;

namespace Web.Controllers
{
    public class DataInputController : BaseController
    {
        #region 网页

        /// <summary>
        /// 测试界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Test1()
        {
            DataParam param = new DataParam();
            //param.csylb = "BB";
            param.zdzdtable = "DATAZDZD";
            //param.sylbZdzdtable = "DATAZDZD_SYLB";
            //param.jydbh = "M00007";

            //param.view = true;
            //param.sham = true;
            //**** 主表信息 ****
            //表名
            param.t1_tablename = "MBGTEST_A";
            //param.t1_tablename = "MBGTEST_A,MBGTEST_B";
            //主键
            param.t1_pri = "JYDBH";
            //标题
            param.t1_title = "单位信息";
            ////**** 从表信息 ****
            ////表名
            ////param.t2_tablename = "SBGTEST1_A,SBGTEST1_B|SBGTEST2_A,SBGTEST2_B";
           // param.t2_tablename = "S1";
            //主键
            //param.t2_pri = "JYDBH,DZBH";
            //param.t2_pri = "JYDBH1,JYDBH2|DZBH1,DZBH2";
            ////标题
            //param.t2_title = "人员信息";
            ////**** 明细信息 ****
            ////表名
            ////param.t3_tablename = "SBGTEST1_A,SBGTEST1_B|SK1,SK2||SBGTEST2_A,SBGTEST2_B|TT1,TT2";
           // param.t3_tablename = "S1|MX1";
            ////主键
            //param.t3_pri = "JYDBH,DZBH,MXBH";
            //JYDBH1,JYDBH2|DZBH1,DZBH2|MXBH1,MXBH2||
            ////标题
            //param.t3_title = "附件信息";
            //前置存储过程
            param.preproc = "sp_insertlog|$JYDBH$|aa||sp_insertlog|bb|cc";
            //按钮
            param.button = "暂存1|ZC|/WebForm/Index?FormDm=QYBA&FormStatus=0| |暂存成功！||返回|FH|http://www.baidu.com";
            //自定义按钮
            param.custombutton = "执行1|ZX1|check1('')||执行2|ZX2|check2('')";
            //每行列数
            param.rownum = 2;
            param.isolationlevel = "readuncommitted";
            return RedirectToAction("Index", "DataInput", param);
        }

        /// <summary>
        /// 测试界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {
            DataParam param = new DataParam();
            param.encode = false;
            bool isencode = param.encode;
            //用户名
            //param.csylb = "BB".GetBDEncode(isencode);
            param.zdzdtable = "DATAZDZD".GetBDEncode(isencode);
            param.companycode = "A".GetBDEncode(isencode);
            param.syxmbh = "B".GetBDEncode(isencode);
            
            //param.companyZdzdtable = "DATAZDZD_COMPANY";
            param.individualZdzdtable = "DATAZDZD_INDIVIDUAL".GetBDEncode(isencode);
            //param.flowZdzdtable = "";
            //param.jydbh = "M00012";

            //param.view = true;
            //param.sham = true;
            //param.fieldparam = "A1,ABC,001";
            //**** 主表信息 ****
            //表名
            //param.t1_tablename = "H_QYLX";
            param.t1_tablename = "M1".GetBDEncode(isencode);
            //param.t1_tablename = "MBGTEST_A,MBGTEST_B";
            //主键
            param.t1_pri = "JYDBH".GetBDEncode(isencode);
            //标题
            param.t1_title = "工程信息".GetBDEncode(isencode);

            param.t2_tablename = "S1".GetBDEncode(isencode);
            //主键
            param.t2_pri = "JYDBH,DZBH".GetBDEncode(isencode);
            //标题
            param.t2_title = "人员信息1".GetBDEncode(isencode);
            ////记录标题
            //param.t2_subtitle = "人员".GetBDEncode(isencode);
            //
            //param.t2_titlefield = "S1.A2".GetBDEncode(isencode);
            //附加SQL
            //param.t2_sql = CryptFun.Encode("and dzbh='F2017000019'");
            //param.t2_title = "人员信息1|人员信息2";
            ////判断保存时至少有的记录数
            //param.t2_checknum = "1";
            //排序字段
            //param.t2_order = "MC,jydbh";
            //排序顺序
            //param.t2_orderseq = "desc,asc";
            //限制新建与删除
            //param.t2_limit = "1|0";
            //显示或隐藏
            //param.t2_hidden = "1|0";

            param.button = "暂存|ZC|/WebForm/Index?FormDm=companysupplies1&FormStatus=0|fa fa-check|暂存1成功！||保存|TJ|/WebForm/Index?FormDm=companysupplies1&FormStatus=0|fa fa-calendar-times-o|保存成功||返回|FH|http://www.baidu.com".GetBDEncode(isencode);
            //自定义按钮
            param.custombutton = "执行1|ZX1|check1('')|fa fa-check||执行2|ZX2|check2('')|fa fa-calendar-times-o".GetBDEncode(isencode);
            ////**** 从表信息 ****
            ////表名
            ////param.t2_tablename = "SBGTEST1_A,SBGTEST1_B|SBGTEST2_A,SBGTEST2_B";
            //param.t2_tablename = "ISGcFgc|ISgcJzry|ISgcSyry";
            ////主键
            //param.t2_pri = "JYDBH,DZBH";
            //param.t2_pri = "GCBH,FGCBH|GCBH,JZRYBH|GCBH,SYRYBH";
            ////标题
            //param.t2_title = "分工程信息|见证人员信息|送样人员信息";
            ////**** 明细信息 ****
            ////表名
            ////param.t3_tablename = "SBGTEST1_A,SBGTEST1_B|SK1,SK2||SBGTEST2_A,SBGTEST2_B|TT1,TT2";
            //param.t3_tablename = "S1|MX1||S2|MX2";
            //param.t3_tablename = "S1|MX1||S2|MX2";
            //主键
            //param.t3_pri = "JYDBH,DZBH,MXBH|JYDBH,DZBH,MXBH";
            //param.t3_pri = "JYDBH,DZBH,MXBH";
            //JYDBH1,JYDBH2|DZBH1,DZBH2|MXBH1,MXBH2||
            //标题
            param.t3_title = "明细A|明细B";
            //param.t3_title = "明细A|明细B";
            //限制新建与删除
            //param.t3_limit = "0|1";

            #region 操作参数
            //是否显示从表跳转
            param.jump = false;
            //回调函数
            //param.callback = "alert('$$GCBH$$');";
            #endregion
            param.js = "aa/a.js?v2016".GetBDEncode(isencode);
            //网页参数
            //param.fieldparam = "IMGc,GCMC,华中1234";
            //自定义按钮
           // param.custombutton = "执行1|ZX1|check1($ABC$)||执行2|ZX2|check2($cdf$)";
            //每行列数
            param.rownum = 2;
            //是否显示底部栏
            //param.bottomdiv = true;

            //param.jydbh = "19000006".GetBDEncode(isencode);
            //param.copyjydbh = "F2016000004";
            param.type = "".GetBDEncode(isencode);// detaillist"leftright";leftrightlist double
            //重新编号
            //param.redefinebh = "S1.FJ";
            //param.stage = "1";
            param.lx = "";
            //param.view = true;
            #region DLL计算
            //param.dllname = "BD.JC.JS.Common.dll";
            //param.dllclass = "BD.JC.JS.Common.ComSetJcyj";
            #endregion

            #region 编号字段
            //param.customNumberField = "M1.JYDBH".GetBDEncode(isencode);
            #endregion
            #region 自定义Sesson
            SessionMgr.SetSession("SZSF", "浙江省");
            #endregion
            //前置存储过程
            param.preproc = "sp_insertlog|$JYDBH$|aa||sp_insertlog|bb|cc".GetBDEncode(isencode);
            //后置存储过程
            param.sufproc = "sp_insertlog|$JYDBH$|h1||sp_insertlog|bb|h2".GetBDEncode(isencode);
            //事务级别
            //param.isolationlevel = "readuncommitted".GetBDEncode(isencode);
            param.type = "leftrightlist";//"leftrightlist";// "leftright";
            param.hiddenOpt = "1";
            //修改申请
            //param.sham = true;
            //param.sqdbh = "A001";
            //param.shamtype = "XGSQ";
            return RedirectToAction("Index", "DataInput", param);
        }

        /// <summary>
        /// 测试界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Test2()
        {
            DataParam param = new DataParam();
            param.encode = false;
            bool isencode = param.encode;
            //用户名
            //param.csylb = "BB".GetBDEncode(isencode);
            param.zdzdtable = "DATAZDZD".GetBDEncode(isencode);
            param.companycode = "A".GetBDEncode(isencode);
            param.syxmbh = "B".GetBDEncode(isencode);

            //param.companyZdzdtable = "DATAZDZD_COMPANY";
            param.individualZdzdtable = "DATAZDZD_INDIVIDUAL".GetBDEncode(isencode);
            //param.flowZdzdtable = "";
            //param.jydbh = "M00012";

            //param.view = true;
            //param.sham = true;
            //param.fieldparam = "A1,ABC,001";
            //**** 主表信息 ****
            //表名
            //param.t1_tablename = "H_QYLX";
            param.t1_tablename = "M1".GetBDEncode(isencode);
            //param.t1_tablename = "MBGTEST_A,MBGTEST_B";
            //主键
            param.t1_pri = "JYDBH".GetBDEncode(isencode);
            //标题
            param.t1_title = "工程信息".GetBDEncode(isencode);

            //param.t2_tablename = "S1|S2";
            param.t2_tablename = "S1".GetBDEncode(isencode);
            ////主键
            param.t2_pri = "JYDBH,DZBH".GetBDEncode(isencode);
            //param.t2_pri = "JYDBH,DZBH|JYDBH,DZBH";
            ////标题
            param.t2_title = "人员信息1".GetBDEncode(isencode);
            ////记录标题
            param.t2_subtitle = "人员".GetBDEncode(isencode);
            //
            param.t2_titlefield = "S1.A2".GetBDEncode(isencode);
            //附加SQL
            //param.t2_sql = CryptFun.Encode("and dzbh='F2017000019'");
            //param.t2_title = "人员信息1|人员信息2";
            ////判断保存时至少有的记录数
            //param.t2_checknum = "1";
            //排序字段
            //param.t2_order = "MC,jydbh";
            //排序顺序
            //param.t2_orderseq = "desc,asc";
            //限制新建与删除
            //param.t2_limit = "1|0";

            param.button = "暂存|ZC|/WebForm/Index?FormDm=companysupplies1&FormStatus=0|fa fa-check|暂存1成功！||保存|TJ|/WebForm/Index?FormDm=companysupplies1&FormStatus=0|fa fa-calendar-times-o|保存成功||返回|FH|http://www.baidu.com".GetBDEncode(isencode);
            //自定义按钮
            param.custombutton = "执行1|ZX1|check1('')|fa fa-check||执行2|ZX2|check2('')|fa fa-calendar-times-o".GetBDEncode(isencode);
            ////**** 从表信息 ****
            ////表名
            ////param.t2_tablename = "SBGTEST1_A,SBGTEST1_B|SBGTEST2_A,SBGTEST2_B";
            //param.t2_tablename = "ISGcFgc|ISgcJzry|ISgcSyry";
            ////主键
            //param.t2_pri = "JYDBH,DZBH";
            //param.t2_pri = "GCBH,FGCBH|GCBH,JZRYBH|GCBH,SYRYBH";
            ////标题
            //param.t2_title = "分工程信息|见证人员信息|送样人员信息";
            ////**** 明细信息 ****
            ////表名
            ////param.t3_tablename = "SBGTEST1_A,SBGTEST1_B|SK1,SK2||SBGTEST2_A,SBGTEST2_B|TT1,TT2";
            //param.t3_tablename = "S1|MX1||S2|MX2";
            param.t3_tablename = "S1|MX1";
            //主键
            //param.t3_pri = "JYDBH,DZBH,MXBH|JYDBH,DZBH,MXBH";
            param.t3_pri = "JYDBH,DZBH,MXBH";
            //JYDBH1,JYDBH2|DZBH1,DZBH2|MXBH1,MXBH2||
            //标题
            //param.t3_title = "明细A|明细B";
            param.t3_title = "明细A";
            //限制新建与删除
            //param.t3_limit = "0|1";

            #region 操作参数
            //是否显示从表跳转
            param.jump = false;
            //回调函数
            //param.callback = "alert('$$GCBH$$');";
            #endregion
            param.js = "aa/a.js?v2016".GetBDEncode(isencode);
            //网页参数
            //param.fieldparam = "IMGc,GCMC,华中1234";
            //自定义按钮
            // param.custombutton = "执行1|ZX1|check1($ABC$)||执行2|ZX2|check2($cdf$)";
            //每行列数
            param.rownum = 2;
            //是否显示底部栏
            //param.bottomdiv = true;

            //param.jydbh = "18000009".GetBDEncode(isencode);
            //param.copyjydbh = "F2016000004";
            param.type = "three".GetBDEncode(isencode);// detaillist"leftright";leftrightlist double
            //重新编号
            //param.redefinebh = "S1.FJ";
            //param.stage = "1";
            param.lx = "";
            //param.view = true;
            #region DLL计算
            //param.dllname = "BD.JC.JS.Common.dll";
            //param.dllclass = "BD.JC.JS.Common.ComSetJcyj";
            #endregion

            #region 编号字段
            //param.customNumberField = "M1.JYDBH".GetBDEncode(isencode);
            #endregion
            #region 自定义Sesson
            SessionMgr.SetSession("SZSF", "浙江省");
            #endregion
            //前置存储过程
            param.preproc = "sp_insertlog|$JYDBH$|aa||sp_insertlog|bb|cc".GetBDEncode(isencode);
            //后置存储过程
            param.sufproc = "sp_insertlog|$JYDBH$|h1||sp_insertlog|bb|h2".GetBDEncode(isencode);
            //事务级别
            //param.isolationlevel = "readuncommitted".GetBDEncode(isencode); ;
            return RedirectToAction("Index", "DataInput", param);
        }

        /// <summary>
        /// 录入界面
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult Index(DataParam param)
        {
            IApplicationContext webApplicationContext = ContextRegistry.GetContext();
            IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");

            ////复制对象
            //DataParam param = TransExpV2<DataParam, DataParam>.Trans(p);
            ////根据参数设置是否需要解密
            //param.objectencode = param.encode;
            //param.objectbackground = true;
            //参数编码
            ViewData["param"] = Base64Util.EncodeBase64(JsonConvert.SerializeObject(param));
            //设置后台处理
            param.isbackground = true;
            //自定义JS
            ViewData["js"] = DataInputService.GetCustomJs(param.js); 
            //存储类型
            ViewData["storagetype"] = FileOssConfig.StorageType.GetString();
            //判断是否由helplink发起
            ViewData["helplinkopt"] = param.helplinkopt;
            //类型
            string ViewType = "";
            switch(param.type.ToLower())
            {
                //list
                case "detaillist":
                    ViewType = "IndexDetailList";
                    break;
                //左右结构
                case "leftright":
                    ViewType = "IndexLeft";
                    break;
                //上下结构列表（上控件下列表，在使用）
                case "leftrightlist":
                    ViewType = "IndexLeftList";
                    break;
                //双层
                case "double":
                    ViewType = "IndexDouble";
                    break;
                //左右结构列表
                case "leftrightlist2":
                    ViewType = "IndexLeftRightList";
                    break;
                //新三层结构
                case "three":
                    ViewType = "IndexThree";
                    break;
                //上下三层结构
                default:
                    ViewType = "Index";
                    break;
            }
            return View(ViewType);
        }
        #endregion

        #region 数据处理
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public ContentResult SaveData() {
            string msg = String.Empty;
            try
            {
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.SaveData();
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
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public ContentResult SearchData() {
            string msg = String.Empty;
            try
            {
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.SearchData();
                if (!ret.Successed)
                    msg = JsonUtil.GetError(ret.Msg);
                else
                    msg = JsonUtil.GetSuccess(ret.Msg, ret.Data);
            }
            catch (Exception ex)
            {
                msg = JsonUtil.GetError(String.Format("SearchData出错，原因：{0}",ex.Message));
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
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.CtrlStringData();
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
        public ActionResult Helplink(string helplink, string type, string prestr,string index)
        {
            //初始化参数
            if (type == null)
                type = "";
            if (prestr == null)
                prestr = "";
            if (index == null)
                index = "";
            IApplicationContext webApplicationContext = ContextRegistry.GetContext();
            IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
            //helplink解码
            helplink = helplink.GetBDDecode(GlobalUtil.CtrlstringAndHelplinkEncrypt);
            //helplink参数
            ViewData["helplink"] = helplink;
            //生成显示字段
            ViewData["zdzd"] = DataInputService.GetHelplinkZdzd(helplink);
            //双击事件
            ViewData["dbclick"] = type == "list" ? DataInputService.GetHelplinkListDbClick(helplink, prestr,index) : DataInputService.GetHelplinkDbClick(helplink);
            //JS
            ViewData["js"] = DataInputService.GetCustomJs(DataInputService.GetHelplinkJs(helplink));
            //自定义双击前判断事件
            ViewData["checkfun"] = DataInputService.GetHelplinkCheckFun(helplink);
            //自定义双击后判断事件
            ViewData["afterfun"] = DataInputService.GetHelplinkAfterFun(helplink);
            //helplink的新建
            HelplinkParam helplinkParam =  DataInputService.GetDataInputFun(helplink);
            //helplink内容
            ViewData["datainputfun"] = helplinkParam.Helplink;
            //helplink新建
            ViewData["datainputnew"] = helplinkParam.IsNew;
            //helplink修改
            ViewData["datainputmodify"] = helplinkParam.IsModify;
            //helplink的jydbh字段
            ViewData["datainputjydbhfield"] = helplinkParam.JydbhField;
            //helplink的选中按钮名
            ViewData["datainputchoosename"] = helplinkParam.ChooseName == "" ? "确定" : helplinkParam.ChooseName;
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
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.HelplinkData();
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

        #region 相关文件操作
        /// <summary>
        /// 服务操作
        /// </summary>
        public void FileService()
        {
            Response.ContentType = "text/plain";
            Response.ClearContent();
            try
            {
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.FileServiceData();
                if (!ret.Successed)
                    Response.Write(ret.Msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(String.Format("服务出错，提示：{0}", ex.Message));
                Response.Write(String.Format("服务出错，提示：{0}", ex.Message));
            }
            Response.End();
        }
        #endregion



        #region ******* 列表界面 *******
        #region 左右结构从表
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        public ContentResult SearchLeftListData()
        {
            string msg = String.Empty;
            try
            {
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.SearchLeftListData();
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
        /// 获取CtrlString数据(触发改变第三方控件值)
        /// </summary>
        /// <returns></returns>
        public ContentResult GetCtrlStringLeftListData()
        {
            string msg = String.Empty;
            try
            {
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.GetCtrlStringLeftListData();
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


        #region 从表
        /// <summary>
        /// 录入界面
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult IndexList(DataParam param)
        {
            //判断跳转
            IApplicationContext webApplicationContext = ContextRegistry.GetContext();
            IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
            //自定义JS
            ViewData["js"] = DataInputService.GetCustomJs(param.js);
            //参数编码
            ViewData["param"] = Base64Util.EncodeBase64(JsonConvert.SerializeObject(param));
            //从表初始化JS信息
            ViewData["cbjs"] = DataInputService.GetCbJs(param);
            return View();
        }

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        public ContentResult SearchListData()
        {
            string msg = String.Empty;
            try
            {
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.SearchListData();
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

        #region 明细表
        /// <summary>
        /// 录入界面明细列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult IndexDetailList(DataParam param)
        {
            //判断跳转
            IApplicationContext webApplicationContext = ContextRegistry.GetContext();
            IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
            //自定义JS
            ViewData["js"] = DataInputService.GetCustomJs(param.js);
            //参数编码
            ViewData["param"] = Base64Util.EncodeBase64(JsonConvert.SerializeObject(param));

            return View();
        }

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        public ContentResult SearchDetailListData()
        {
            string msg = String.Empty;
            try
            {
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.SearchDetailListData();
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
        /// 获取CtrlString数据(触发改变第三方控件值)
        /// </summary>
        /// <returns></returns>
        public ContentResult CtrlStringDataEasyUI()
        {
            string msg = String.Empty;
            try
            {
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                IDataInputService DataInputService = (IDataInputService)webApplicationContext.GetObject("DataInputService");
                ResultParam ret = DataInputService.CtrlStringDataEasyUI();
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


    }
}
