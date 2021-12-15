using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BD.Jcbg.IBll;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using Spring.Transaction.Interceptor;
using BD.IDataInputDao;
using System.IO;
using System.Web;
using BD.DataInputModel.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.Bll
{
    public class SelfService:ISelfService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }

        IDataFileDao dataFileDao { get; set; }
        #endregion

        #region 方法

        public bool SaveDataFile(HttpPostedFileBase file, HttpServerUtilityBase server,string fileid,out string msg)
        {
            bool code = false;
            try
            {
                Stream stream = file.InputStream;//new MemoryStream();
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                //设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);

                //savePng(Guid.NewGuid().ToString("N") + ".png", bytes, server); //保存png
                //判断是否为图片,生成缩略图
                byte[] FileSmallArray = null;
                try
                {
                    MyImage myImage = new MyImage(bytes);
                    if (myImage.IsImage())
                    {
                        FileSmallArray = myImage.ConvertToJpg(100, 100);
                    }
                }
                catch (Exception ex)
                { }
                string filename = fileid + ".jpg";

                DataFile dataFileModel = new DataFile();
                //文件代码
                dataFileModel.FILEID = fileid;
                //文件内容
                dataFileModel.FILECONTENT = bytes;
                //OSS对象存储
                dataFileModel.STORAGETYPE = "";
                //文件名
                dataFileModel.FILENAME = filename;// file.FileName;
                //扩展名
                dataFileModel.FILEEXT = Path.GetExtension(filename); //Path.GetExtension(file.FileName);
                //上传时间
                dataFileModel.CJSJ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //判断缩略图是否有
                if (FileSmallArray != null)
                    dataFileModel.SMALLCONTENT = FileSmallArray;
                //保存
                msg = "";
                code = dataFileDao.SaveFile(dataFileModel);
            }
            catch(Exception e)
            {
                code = false;
                msg = e.Message;
            }
               
            return code;
        }

        private void savePng(string filename, byte[] bytes, HttpServerUtilityBase server)
        {
            try
            {
                string path = server.MapPath("~/htpng/") + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + CurrentUser.RealName + "_" + filename;//设定上传的文件路径
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
            }
            catch (Exception e)
            { }
        }
        /// <summary>
        /// 天气-根据gcbh获取所在城市
        /// </summary>
        /// <param name="gcid"></param>
        /// <returns></returns>
        public string getCity(string gcid)
        {
            string city = "";
            string sql = "select szcs from i_m_gc where gcbh='" + gcid + "'";
            IList<IDictionary<string,string>> list=CommonDao.GetDataTable(sql);
            if(list.Count>0)
            {
                city = list[0]["szcs"];
            }
            return city;
        }

        public   IList<IList<IDictionary<string, string>>> GetSelfFormData(IList<IDictionary<string, string>> rows)
        {
            IList<IList<IDictionary<string, string>>> alllist = new List<IList<IDictionary<string, string>>>();
            foreach (IDictionary<string, string> row in rows)
            {
                IList<IDictionary<string, string>> datalist = new List<IDictionary<string, string>>();
                foreach (string key in row.Keys)
                {
                    IDictionary<string, string> temp_row = new Dictionary<string, string>();
                    string value = row[key];
                    temp_row.Add("fieldname", key);
                    temp_row.Add("fieldvalue", value);
                    datalist.Add(temp_row);
                }
                alllist.Add(datalist);

            }
            return alllist;
        }
        public IList<IDictionary<string, string>> getBzfzrlist(string bzfzr, string jdzch)
        {
            string sql = "select ryxm,sfzhm from i_m_wgry where bzfzr='" + bzfzr + "' and sfbzfzr='是' and jdzch='" + jdzch + "'";
            IList<IDictionary<string, string>> list = CommonDao.GetDataTable(sql);
            return list;
        }
        //递归函数
        public void getBzfzrlist(string bzfzr, string jdzch, ref List<string> fzrlist)
        {
            string sql = "select ryxm,sfzhm from i_m_wgry where bzfzr='" + bzfzr + "' and sfbzfzr='是' and jdzch='" + jdzch + "' and sfzhm!='"+bzfzr+"'";
            IList<IDictionary<string, string>> list = CommonDao.GetDataTable(sql);
            foreach (IDictionary<string, string> item in list)
            {
                fzrlist.Add(item["sfzhm"]);
                getBzfzrlist(item["sfzhm"], jdzch, ref fzrlist);
            }
        }

        public string getbzfzr_str(string bzfzrxm,string jdzch)
        {
            string fzrstr = "";
            string msg = "";
            bool code = false;
            List<string> fzrlist = new List<string>();
            try
            {
                List<string> fzr_sfzlist = new List<string>();
                string sql = "select ryxm,sfzhm from i_m_wgry where ryxm='" + bzfzrxm + "' and sfbzfzr='是' and jdzch='" + jdzch + "'";
                IList<IDictionary<string, string>> rylist = CommonDao.GetDataTable(sql);
                if (rylist.Count == 0)
                {
                    code = false;
                    msg = "没有该人员信息";
                }
                else
                {
                    fzrlist.Add(rylist[0]["sfzhm"]);
                    getBzfzrlist(rylist[0]["sfzhm"], jdzch, ref fzrlist);
                    for (int i = 0; i < fzrlist.Count; i++)
                    {
                        fzrstr += fzrlist[i] + ",";
                    }
                    fzrstr = fzrstr.FormatSQLInStr();
                    code = true;
                    msg = jdzch;
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            return fzrstr;
        }

        public List<string> getFormZDZD(string formdm ,string formStatus)
        {
            string sql = "select zdname from formzdzd where formdm='" + formdm + "' and formStatus='" + formStatus + "'";
            IList<IDictionary<string,string>> list=CommonDao.GetDataTable(sql);
            List<string> zdlist = new List<string>();
            for(int i=0;i<list.Count;i++)
            {
                zdlist.Add(list[i]["zdname"]);
            }
            return zdlist;
        }

        public string getSqlfzr(string filterRules, List<string> zdlist, string jdzch,bool ym, string bzfzrzd = "bzfzr")
        {
            //解析过滤条件
            JToken jsons = JToken.Parse(filterRules);//转化为JToken（JObject基类）   //[]表示数组，{} 表示对象
            string where = " ";
            bool h_year = false; //筛选条件有年
            bool h_month = false; //筛选条件有月
            for (int j = 0; j < zdlist.Count; j++)
            {
                foreach (JToken baseJ in jsons)//遍历数组  //[]表示数组要循环
                {
                    string fieldname = baseJ.Value<string>("fieldname");
                    string fieldvalue = baseJ.Value<string>("fieldvalue");
                    string fieldopt = baseJ.Value<string>("fieldopt");
                    string filtertype = baseJ.Value<string>("filtertype");

                    if (fieldname.ToLower() == zdlist[j].ToLower())
                    {
                        if (fieldname.ToLower() == "logyear")
                        {
                            h_year = true;
                        }
                        if (fieldname.ToLower() == "logmonth")
                        {
                            h_month = true;
                        }              
                        if(!string.IsNullOrEmpty(fieldvalue))
                        {
                            if (fieldname.ToLower() == "bzfzrxm" || fieldname.ToLower() == "bzzryxm")
                            {
                                string fzr_str = getbzfzr_str(fieldvalue, jdzch);
                                where += " and " + bzfzrzd + " in (" + fzr_str + ")";
                                break;
                            }
                            else
                            {
                                where += " and " + fieldname + " like '%" + fieldvalue + "%'";
                                break;
                            }
                        }
                      
                    }
                }

            }
            DateTime dt = DateTime.Now;
            string year = dt.Year.ToString();
            string month = dt.Month.ToString();
            if(ym)
            {
                if (!h_year)
                    where += " and logyear ='" + year + "'";
                if (!h_month)
                    where += " and logmonth = '" + month + "'";
            }         
            return where;
        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string PostData(string url, string datas)
        {
            string result = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(datas);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";// "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接收返回信息
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            Stream stream = response.GetResponseStream();
            if (stream != null)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            return result;
        }
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public string GetHttpResponse(string url)
        {
            string retString = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";// "text/html;charset=UTF-8";
            request.UserAgent = null;
           // request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            if (stream != null)
            {
                StreamReader myStreamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                stream.Close();
            }
            return retString;
        }


        /// <summary>
        /// 工程区域统计
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="district"></param>
        /// <param name="jd"></param>
        /// <param name="qybh"></param>
        /// <param name="gcbh"></param>
        /// <param name="gclx"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetGC_QYFBTJ(string province, string city, string district, string jd, string qybh, string gcbh, string gclx, string key)
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string where = " where 1=1 ";
                //判断是否企业账户登录
                if (CurrentUser.CurUser.DepartmentId.Equals(Configs.GetConfigItem("wfztbmdm"))) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where a.yhzh=  '" + CurrentUser.UserCode + "' and sfqyzzh=1";
                    dtt = CommonDao.GetDataTable(sqlqybh);
                    if (dtt.Count > 0)
                        qybh = dtt[0]["qybh"].ToString();
                    else
                    {
                        msg = "找不到企业信息";

                    }
                }

                if (province != "")
                    where += " and a.szsf='" + province + "'";
                if (city != "")
                    where += " and a.szcs='" + city + "'";
                if (qybh != "")
                    where += " and a.sgdwbh = '" + qybh + "'";
                if (gcbh != "")
                    where += " and a.gcbh ='" + gcbh + "'";
                if (gclx != "")
                    where += " and a.gclxbh='" + gclx + "'";

                if (key != "")
                    where += " and (a.gcmc like '%" + key + "%' or a.sgdwmc like '%" + key + "%')";



                //判断是显示区还是街道分布
                if (district != "")
                    where += " and a.szxq='" + district + "'";

                string sql = "select count(1) as value ";
                if (district != "")
                {
                    sql += ",szjd as name ";
                    where += " group by szjd";
                }
                else
                {
                    sql += ",szxq as name ";
                    where += " group by szxq";
                }

                sql += " from View_I_M_GC_ZS a ";
                sql += where;
                dt = CommonDao.GetDataTable(sql);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            return dt;
        }


        public IList<IDictionary<string, string>> GetQYGCZT(string province, string city, string district, string jd, string qybh, string gcbh, string gclx, string key, string gczt)
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string where = " where 1=1 ";
                //判断是否企业账户登录
                if (CurrentUser.CurUser.DepartmentId.Equals(Configs.GetConfigItem("wfztbmdm"))) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where a.yhzh=  '" + CurrentUser.UserCode + "' and sfqyzzh=1";
                    dtt = CommonDao.GetDataTable(sqlqybh);
                    if (dtt.Count > 0)
                        qybh = dtt[0]["qybh"].ToString();
                    else
                    {
                        msg = "找不到企业信息";

                    }
                }

                if (province != "")
                    where += " and a.szsf='" + province + "'";
                if (city != "")
                    where += " and a.szcs='" + city + "'";
                if (qybh != "")
                    where += " and a.sgdwbh = '" + qybh + "'";
                if (gcbh != "")
                    where += " and a.gcbh ='" + gcbh + "'";
                if (gclx != "")
                    where += " and a.gclxbh='" + gclx + "'";
                if(gczt!="")
                    where += " and a.gczt='" + gczt + "'";

                if (key != "")
                    where += " and (a.gcmc like '%" + key + "%' or a.sgdwmc like '%" + key + "%')";

                where += " group by sy_gczt ";

                string sql = "select count(1) as value ";

                sql += " from View_I_M_GC_ZS a ";
                sql += where;
                dt = CommonDao.GetDataTable(sql);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            return dt;
        }

        #endregion

        #region 个人操作
        /// <summary>
        /// 获取人员手机号码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetPhone(string usercode, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<IDictionary<string,string>> dts = CommonDao.GetDataTable("select QYBH,ZHLX from i_m_qyzh where yhzh='" + usercode + "'");
                if (dts.Count == 0)
                    msg = "帐号无效";
                else
                {
                    var qylx = dts[0]["zhlx"];
                    var bh = dts[0]["qybh"];
                    if (qylx.StartsWith("Q", StringComparison.OrdinalIgnoreCase))
                    {
                        dts = CommonDao.GetDataTable("select lxsj from i_m_qy where qybh='" + bh + "'");
                        if (dts.Count == 0)
                        {
                            msg = "找不到企业记录";
                        }
                        else
                        {
                            ret = true;
                            msg = dts[0]["lxsj"];
                        }
                    }else if (qylx.StartsWith("R", StringComparison.OrdinalIgnoreCase))
                    {
                        dts = CommonDao.GetDataTable("select sjhm from i_m_ry where rybh='" + bh + "'");
                        if (dts.Count == 0)
                        {
                            msg = "找不到人员记录";
                        }
                        else
                        {
                            ret = true;
                            msg = dts[0]["sjhm"];
                        }
                    }
                    else if (qylx.StartsWith("N", StringComparison.OrdinalIgnoreCase))
                    {
                        dts = CommonDao.GetDataTable("select sjhm from i_m_nbry where rybh='" + bh + "'");
                        if (dts.Count == 0)
                        {
                            msg = "找不到内部人员记录";
                        }
                        else
                        {
                            ret = true;
                            msg = dts[0]["sjhm"];
                        }
                    }
                    else
                    {
                        msg = "人员类型无效";
                    }
                    if (ret)
                    {
                        if (string.IsNullOrEmpty(msg))
                        {
                            msg = "手机号码为空";
                            ret = false;
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(msg, @"^[1]+[3,5]+\d{9}"))
                        {
                            msg = "手机号码格式无效";
                            ret = false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        #endregion
    }
}
