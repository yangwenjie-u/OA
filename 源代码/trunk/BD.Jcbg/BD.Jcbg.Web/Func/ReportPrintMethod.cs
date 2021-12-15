using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.Common;

namespace BD.Jcbg.Web.Func
{
    public class ReportPrintMethod
    {
        public string Test(int Index, IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            return "1";
        }


        public string ForamtZGDQFSJ(int Index, IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            string v = datas["JDBG_JDJL"][Index]["LRSJ"].GetSafeDate().ToString("yyyy年MM月dd日");
            if (v == "1900年01月01日") {
                v = DateTime.Now.ToString("yyyy年MM月dd日");
            }
            
            return v;

        }

        public string ForamtZGQX(int Index, IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            string s = "";
            string v = datas["JDBG_JDJL"][Index]["LRSJ"].GetSafeDate().ToString("yyyy-MM-dd");
            if (v == "1900-01-01")
            {
                s = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            }
            else {
                s = datas["JDBG_JDJL"][Index]["EXTRAINFO14"].ToString();
            }

            return s;

        }

        public string ForamtZGDHFSJ(int Index, IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            string v = datas["JDBG_JDJL"][Index]["JDJLSJ"].GetSafeDate().ToString("yyyy年MM月dd日");
            if (v == "1900年01月01日")
            {
                v = DateTime.Now.ToString("yyyy年MM月dd日");
            }

            return v;

        }

        public string ForamtHFNR(int Index, IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            string v = "";
            string info5 = datas["JDBG_JDJL_XQ"][Index]["INFO5"].GetSafeString();
            string info2=datas["JDBG_JDJL_XQ"][Index]["INFO2"].GetSafeString();
            v = info5 + " " + info2;

            return v;

        }

        public string FormatZGNR(int Index, IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            IList<IDictionary<string, object>> dt = datas["JDBG_JDJL_XQ"].OrderBy(x => x["INFO3"].GetSafeInt()).ToList();

            var value = dt[Index]["INFO1"].ToString();
            var kfz = dt[Index]["INFO12"].ToString().GetSafeString();
            var kfbh = dt[Index]["INFO13"].ToString().GetSafeString();
            var kfxm = dt[Index]["INFO14"].ToString().GetSafeString();
            //value = kfbh != "" ? string.Format("{0} 该行为按照{1}条例：{2}，扣{3}分。", value, kfbh, kfxm, kfz) : value;
            //value = kfbh != "" ? string.Format("{0} 该行为按照{1}条例，扣{2}分。", value, kfbh, kfz) : value;
            if (Index == dt.Count - 2)
            {
                var lineChar = 64;
                var firstPageLines = 31;
                var pageLines = 45;
                var bottomLines = 12;
                var lineCount = 0;
                var lastCount = 0;
                var data = dt;
                for (int i = 0; i < data.Count; i++)
                {
                    var count = 1;
                    var item = data.ElementAt(i);
                    var l = 64;
                    var info = item["INFO1"].ToString();
                    //info = kfbh != "" ? string.Format("{0} 该行为按照{1}条例：{2}，扣{3}分。", info, kfbh, kfxm, kfz) : info;
                    //info = kfbh != "" ? string.Format("{0} 该行为按照{1}条例，扣{2}分。", info, kfbh, kfz) : info;
                    var ss = info.ToCharArray().Select(x => x.ToString()).ToList();
                    foreach (var s in ss)
                    {
                        var len = System.Text.Encoding.Default.GetBytes(s).Length;
                        if (l >= len)
                        {
                            l -= len;
                        }
                        else
                        {
                            count++;
                            l = lineChar;
                            l -= len;
                        }
                    }
                    lineCount += count;
                    if (i == data.Count - 1)
                    {
                        lastCount = count;
                    }
                }

                if (lineCount <= firstPageLines - bottomLines)
                {
                    return value;
                }
                else if (lineCount <= firstPageLines && lineCount > firstPageLines - bottomLines)
                {
                    for (int i = 0; i < firstPageLines - lineCount + 2 + lastCount; i++)
                    {
                        value += "\r\n";
                    }
                    return value;
                } else
                {
                    lineCount -= firstPageLines;
                    lineCount = lineCount % pageLines;
                    if (lineCount <= pageLines - bottomLines)
                    {
                        return value;
                    }
                    else
                    {
                        for (int i = 0; i < pageLines - lineCount + 2 + lastCount; i++)
                        {
                            value += "\r\n";
                        } return value;
                    }
                }
            }
            else return value;
        }

        /// <summary>
        /// 整改单格式化监督登记号
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string ForamtZJDJH(int Index, IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            string s = "";
            string v = datas["VIEW_I_M_GC"][Index]["ZJDJH"].GetSafeString();
            // 获取年份
            int y = v.Substring(0, 4).GetSafeInt();
            if (y >= 2019)
            {
                s = datas["VIEW_I_M_GC"][0]["SGXKZH"].GetSafeString();
            }
            else
            {
                s = v;
            }

            return s;

        }

        /// <summary>
        /// 监督报告中格式化监督登记号
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string ForamtZJDJH2(int Index, IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            string s = "";
            List<string> ls = datas["stformitem"].Where(x=>x["itemname"].GetSafeString()== "jdzch").Select(x=>x["itemvalue"].GetSafeString()).ToList();
            string v = "";
            if (ls.Count > 0)
            {
                v = ls[0];

                // 获取年份
                int y = v.Substring(0, 4).GetSafeInt();
                if (y >= 2019)
                {
                    s = datas["VIEW_I_M_GC"][0]["SGXKZH"].GetSafeString();
                }
                else
                {
                    s = v;
                }

            }
            

            return s;

        }
        /// <summary>
        /// 监督报告中格式化工程名称
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string ForamtGCMC(int Index, IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            string s = "";
            
            // 获取工程名称
            List<string> gcmcls = datas["stformitem"].Where(x => x["itemname"].GetSafeString() == "gcmc").Select(x => x["itemvalue"].GetSafeString()).ToList();
            if (gcmcls.Count > 0)
            {
                s= gcmcls[0];
            }

            // 获取监督登记号
            List<string> ls = datas["stformitem"].Where(x => x["itemname"].GetSafeString() == "jdzch").Select(x => x["itemvalue"].GetSafeString()).ToList();
            if (ls.Count > 0)
            {
                // 获取监督注册号
                string jdzch =  ls[0];                

                // 获取年份
                int y = jdzch.Substring(0, 4).GetSafeInt();
                // 2019年之后的监督登记号，统一改成施工许可证号
                if (y >= 2019)
                {
                    string sgxkzh  = datas["VIEW_I_M_GC"][0]["SGXKZH"].GetSafeString();
                    s = s.Replace(jdzch, sgxkzh);
                }
                

            }


            return s;

        }





    }
}