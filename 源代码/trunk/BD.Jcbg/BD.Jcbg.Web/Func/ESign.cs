using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EsignSharp.service;
using EsignSharp.service.impl;
using EsignUtils.utils.bean.result;
using System.Collections;
using EsignUtils.utils.bean;
using EsignUtils.utils.bean.constant;
using Spring.Context;
using Spring.Context.Support;

using Newtonsoft.Json;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using System.Threading;

namespace BD.Jcbg.Web.Func
{
    public static class ESign
    {
        public static bool isinit = false;

        public static List<SealBean> seals = null;

        /// <summary>
        ///     appkey登录列表
        ///     如果已经登录过， 值为对应的accountid
        ///     如果没有登录过，值对空字符串
        /// </summary>
        public static Dictionary<string, string> appkeyInitList = new Dictionary<string, string>()
        {
            { "3df500a000013629",""},
            { "3df500d80001362c",""}
        };


        public static void Init()
        {
            if (!isinit)
            {
                string projectid = GlobalVariable.GetConfigValue("project_id");
                string project_secret = GlobalVariable.GetConfigValue("project_secret");

                EsignsdkService es = new EsignsdkServiceImpl();
                Result init = es.Init(projectid, project_secret, "", ""); //初始化项目
                if (init.ErrCode == 0)
                {
                    isinit = true;
                }
            } 
        }

        /// <summary>
        /// 为PDF生成电子签章（不需要短信验证码）
        /// </summary>
        /// <param name="sourcefile"></param>
        /// <param name="destfile"></param>
        /// <param name="pb"></param>
        /// <param name="sealid"></param>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SignPdf(string appkey, string sourcefile, string destfile, EsignUtils.utils.bean.PosBean pb, int sealid, EsignUtils.utils.bean.constant.SignType type, out string msg)
        {
            bool ret = true;
            msg = "";
            Result result = new Result();
            EsignsdkService es = new EsignsdkServiceImpl();
            try
            {
                if (!isinit)
                {
                    Init();
                }
                if ( ! AppLogin(appkey, out msg))
                {
                    ret = false;
                }
                else
                {
                    string accountid = msg;
                    result = es.LocalSignPDF(accountid, sourcefile, destfile, pb, sealid, type);
                    if (result.ErrCode != 0)
                    {
                        ret = false;
                        msg = result.Msg;
                    }

                }
                
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

        /// <summary>
        /// 为PDF生成电子签章（需要短信验证码）
        /// </summary>
        /// <param name="sourcefile"></param>
        /// <param name="destfile"></param>
        /// <param name="pb"></param>
        /// <param name="sealid"></param>
        /// <param name="type"></param>
        /// <param name="identifyCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SignPdf(string appkey, string sourcefile, string destfile, EsignUtils.utils.bean.PosBean pb, int sealid, EsignUtils.utils.bean.constant.SignType type, string identifyCode, out string msg)
        {
            bool ret = true;
            msg = "";
            Result result = new Result();
            EsignsdkService es = new EsignsdkServiceImpl();
            try
            {
                if (!isinit)
                {
                    Init();
                }
                if ( ! AppLogin(appkey, out msg))
                {
                    ret = false;
                }
                else
                {
                    string accountid = msg;
                    result = es.LocalSignPDF(accountid, sourcefile, destfile, pb, sealid, type, identifyCode);
                    if (result.ErrCode != 0)
                    {
                        ret = false;
                        msg = result.Msg;
                    }
                }
                    
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }
        /// <summary>
        /// 发送电子签章短信验证码
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SendSignCode(string appkey, out string msg)
        {
            bool ret = true;
            msg = "";
            Result result = new Result();
            EsignsdkService es = new EsignsdkServiceImpl();
            try
            {
                if (!isinit)
                {
                    Init();
                }

                if (!AppLogin(appkey, out msg))
                {
                    ret = false;
                }
                else
                {
                    string accountid = msg;
                    result = es.sendSignCode(accountid);
                    if (result.ErrCode != 0)
                    {
                        ret = false;
                        msg = result.Msg;
                    }
                }
                
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            
            return ret;
        }

        public static bool AppLogin(string appkey, out string  msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                if (!isinit)
                {
                    Init();
                }
                // 如果不包含appkey, 或者对应appkey值为空字符串，需要登录一下
                if (!appkeyInitList.Keys.Contains(appkey) || appkeyInitList[appkey] == "")
                {
                    EsignsdkService es = new EsignsdkServiceImpl();
                    LoginResult lr = new LoginResult();
                    lr = es.appLogin(appkey);
                    // 登录出错
                    if (lr.ErrCode != 0)
                    {
                        ret = false;
                        msg = lr.Msg;
                    } 
                    // 登录成功， 设置appkey 对应的 accountid
                    else{
                        
                        if (!appkeyInitList.Keys.Contains(appkey))
                        {
                            appkeyInitList.Add(appkey, lr.AccountId);
                        }
                        else
                        {
                            appkeyInitList[appkey] = lr.AccountId;
                        }

                        ret = true;
                        msg = lr.AccountId;
                    }
                }
                else
                {
                    ret = true;
                    msg = appkeyInitList[appkey];
                }

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }

            return ret;
            

        }

        
    }
}