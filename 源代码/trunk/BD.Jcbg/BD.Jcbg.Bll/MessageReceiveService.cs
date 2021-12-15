using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using Spring.Transaction.Interceptor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BD.Jcbg.Bll
{
    public class MessageReceiveService : IMessageReceiveService
    {
        #region 用到的Dao
        IMessageReceiveDao MessageReceiveDao { get; set; }

        ICommonDao CommonDao { get; set; }
        #endregion




        public IList<MessageReceive> Gets()
        {
            return MessageReceiveDao.Gets();
        }

        public MessageReceive Get(int Recid)
        {
            return MessageReceiveDao.Get(Recid);
        }



        /// <summary>
        /// 保存MessageReceive对象
        /// </summary>
        /// <param name="itm"></param>
        /// <returns></returns>
        /// 
        [Transaction(ReadOnly = false)]
        public int InsertData(List<MessageReceive> itms, string code, out string msg)
        {

            int result = -1;
            msg = "上传成功";
            try
            {
                IList<IDictionary<string, string>> mb_ret = CommonDao.GetDataTable("SELECT  SettingValue FROM    sxga_syssetting WHERE   (SettingCode = 'TEMPLET1') ");
                //非法字段库
                IList<IDictionary<string, string>> ffzd_ret = CommonDao.GetDataTable("SELECT     RECID, FFZD, THZD, Touse  FROM     sxga_ffzd   WHERE     (Touse = 'True')");
                
                if (mb_ret.Count <= 0)
                {
                    msg = "找不到对应模板!";
                    return -1;
                }


                if (itms.Count <= 0)
                {
                    msg = "数据不能为空";
                    return -1;
                }
                for (int i = 0; i < itms.Count; i++)
                {
                    if (!tools2.IsMobilePhone(itms[i].TelPhone))
                    {
                        msg = "电话号码(TelPhone)不合法!";
                        return -1;
                    }
                    if (itms[i].AcceptTime.GetSafeString().Equals(""))
                    {
                        msg = "处理时间(AcceptTime)不能为空!";
                        return -1;
                    }
                    if (itms[i].ProjId.GetSafeString().Equals(""))
                    {
                        msg = "申报信息的唯一编码(ProjId)不能为空!";
                        return -1;
                    }
                    if (itms[i].AreaCode.GetSafeString().Equals(""))
                    {
                        msg = "办结人所属部门的所在行政区划编码 (AreaCode)不能为空!";
                        return -1;
                    }
                    if (itms[i].AreaName.GetSafeString().Equals(""))
                    {
                        msg = "办结人所属部门的所在行政区名称 (AreaName)不能为空!";
                        return -1;
                    }
                    if (itms[i].ServiceCode.GetSafeString().Equals(""))
                    {
                        msg = "审批事项编号(ServiceCode)不能为空!";
                        return -1;
                    }
                    if (itms[i].ServiceName.GetSafeString().Equals(""))
                    {
                        msg = "权力事项名称(ServiceName)不能为空!";
                        return -1;
                    }
                    if (itms[i].InfoType.GetSafeString().Equals(""))
                    {
                        msg = "办件类型(InfoType)不能为空!";
                        return -1;
                    }
                    if (itms[i].ApplyName.GetSafeString().Equals(""))
                    {
                        msg = "申报者名称(ApplyName)不能为空!";
                        return -1;
                    }
                    if (itms[i].BelongSystem.GetSafeString().Equals(""))
                    {
                        msg = "所属系统 (BelongSystem)不能为空!";
                        return -1;
                    }
                    if (itms[i].PoliceTypeCode.GetSafeString().Equals(""))
                    {
                        msg = "警种代码 (PoliceTypeCode)不能为空!";
                        return -1;
                    }
                    if (itms[i].PoliceType.GetSafeString().Equals(""))
                    {
                        msg = "警种名称(PoliceType)不能为空!";
                        return -1;
                    }
                    if (itms[i].TransactTime.GetSafeString().Equals(""))
                    {
                        msg = "办结日期(TransactTime)不能为空!";
                        return -1;
                    }
                    if (itms[i].TransactResult.GetSafeString().Equals(""))
                    {
                        msg = "办理结果(TransactResult)不能为空!";
                        return -1;
                    }
                    if (itms[i].HanderDeptName.GetSafeString().Equals(""))
                    {
                        msg = "办结人所属部门名称,申报地点（部门）(HanderDeptName)不能为空!";
                        return -1;
                    }
                    if (itms[i].HanderDeptId.GetSafeString().Equals(""))
                    {
                        msg = "办结人所属部门编码 (HanderDeptId)不能为空!";
                        return -1;
                    }

                    
                }



                string templet = mb_ret[0]["settingvalue"].GetSafeString();



                IList<string> sqls = new List<string>();
                //StringBuilder sql = new StringBuilder();
                string tableSql = "", parameterSql = "", sqltemp = "";
                for (int i = 0; i < itms.Count; i++)
                {
                    MessageReceive messageReceive = itms[i];

                    messageReceive.Lsh = "";//DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                    messageReceive.Uuid = Guid.NewGuid().ToString("N");
                    messageReceive.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    messageReceive.UpdateTime = messageReceive.CreateDate;
                    messageReceive.Code = code;
                    //Thread.Sleep(1);
                    //HibernateTemplate.Save(lqSnjbzscsjzList[i]);
                    tableSql = "";
                    parameterSql = "";
                    sqltemp = "";

                    var ps = messageReceive.GetType().GetProperties();

                    foreach (var p in ps)
                    {
                        string type = p.PropertyType.Name;
                        string _name = p.Name;
                        System.Reflection.PropertyInfo pi = messageReceive.GetType().GetProperty(_name);
                        string _value = pi.GetValue(messageReceive, null).GetSafeString();

                        if (_name.ToLower().Equals("recid") || _value.Equals("") || _name.ToLower().Equals("context"))
                            continue;

                        switch (type)
                        {
                            case "Decimal":
                                tableSql += " " + _name + ",";
                                parameterSql += " " + _value + ",";
                                break;
                            default:
                                tableSql += " " + _name + ",";
                                parameterSql += " '" + _value + "',";
                                break;
                        }

                    }




                    string context = templet;
                    Regex r = new Regex("(?<=#@@).*?(?=#)", RegexOptions.IgnoreCase);
                    Regex r2 = new Regex("#@@(?<=#@@).*?(?=#)#", RegexOptions.IgnoreCase);
                    MatchCollection colls2 = Regex.Matches(context, @"(?<=#@@).*?(?=#)");
                    int count = colls2.Count;
                    for (int n = 0; n < count; n++)
                    {
                        string name_ = r.Match(context).Value;
                        System.Reflection.PropertyInfo pi = messageReceive.GetType().GetProperty(name_);
                        string _value = "";
                        if (name_.ToLower().Equals("accepttime"))
                        {
                            DateTime dt = Convert.ToDateTime(pi.GetValue(messageReceive, null).GetSafeString());
                            _value = dt.ToString("MM月dd日");
                        }
                        else
                        {
                            _value = pi.GetValue(messageReceive, null).GetSafeString();
                        }

                        context = r2.Replace(context, _value, 1);

                    }


                    tableSql += " CONTEXT,";
                    parameterSql += " '" + context + "',";

                    tableSql = tableSql.Trim(',');
                    parameterSql = parameterSql.Trim(',');
                    sqltemp = String.Format("Insert into MessageReceive ({0}) values({1});", tableSql, parameterSql);
                    //sql.Append(sqltemp);
                    sqls.Add(sqltemp);


                    if (!messageReceive.DataState.GetSafeString().Equals("0"))
                    {

                        for (int j = 0; j < ffzd_ret.Count; j++)
                        {
                            context = context.GetSafeString().Replace(ffzd_ret[j]["ffzd"].GetSafeString(), ffzd_ret[j]["thzd"].GetSafeString());
                        }

                        sqltemp = String.Format("Insert into MessageAct (lsh, phonenumber, context, status,uuid,CreateDate,SERVICECODE) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}');", messageReceive.Lsh, messageReceive.TelPhone, context, "0", messageReceive.Uuid, messageReceive.CreateDate, messageReceive.PoliceTypeCode);
                        //sql.Append(sqltemp);
                        sqls.Add(sqltemp);
                    }


                }

              //  result = MessageReceiveDao.InsertData(sql.ToString(), out  msg);
                result = sqls.Count;
                foreach (string str in sqls)
                    CommonDao.ExecCommand(str, CommandType.Text);

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);

                msg = ex.Message;
                result = -1;
            }



            return result;
        }
    }
}
