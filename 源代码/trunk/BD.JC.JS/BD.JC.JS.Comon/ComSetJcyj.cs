using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.JC.JS.Common
{
    /// <summary>
    /// 委托时设置检测依据
    /// </summary>
    public class ComSetJcyj:ICalculate
    {
        /// <summary>
        /// 实现接口函数
        /// </summary>
        /// <param name="inparam"></param>
        /// <returns></returns>
        public string Calculate(string inparam)
        {
            ReturnParam ret = new ReturnParam(false, "");
            string msg = "";
            bool code = SetJcyj(inparam, out msg);
            ret.code = code;
            ret.msg = msg;
            if (code)
            {
                ret.code = BillMstable.NullToOther(inparam, "----", out msg);
                ret.msg = msg;
            }
            return ret.GetJson();
           
        }
        public bool SetJcyj(string inparam,out string msg)
        {
            bool retBool = false; 
            string syxmbh=this.GetSyxmbh(inparam,out msg);
            string sql=this.GetJcyjSql(inparam,syxmbh);
            string jcyj = "";
            string pdbz = "";
            string updateSql = "";
            int vp = 0;
            try
            {
                IList<IDictionary<string, string>> dt = SqlHelper.GetDataTable(sql, out msg);
                if (dt != null)
                {
                    for (vp = 0; vp < dt.Count;++vp)
                    {   
                        if(jcyj=="")
                        { 
                            jcyj = dt[vp]["jcyj"];
                        }
                        else
                        {
                            jcyj = jcyj + "、" + dt[vp]["jcyj"];
                        }
                        if(pdbz=="")
                        { 
                            pdbz = dt[vp]["pdbz"];
                        }
                        else
                        {
                            pdbz = pdbz + "、" + dt[vp]["pdbz"];
                        }
                    }
                }
                string[] jcyjArr = jcyj.Split('、');
                string[] pdbzArr = pdbz.Split('、');
                int jcyjCount = jcyjArr.Count();
                int pdbzCount = pdbzArr.Count();
                jcyj = "、"; pdbz = "、";
                for (vp = 0; vp < jcyjCount; ++vp)
                {
                    if (jcyj.IndexOf("、" + jcyjArr[vp] + "、") == -1 && jcyjArr[vp]!="")
                    {
                        jcyj = jcyj + jcyjArr[vp] + "、";
                    }
                }
                for (vp = 0; vp < pdbzCount; ++vp)
                {
                    if (pdbz.IndexOf("、" + pdbzArr[vp] + "、") == -1 && pdbzArr[vp]!="")
                    {
                        pdbz = pdbz + pdbzArr[vp] + "、";
                    }
                }
                if(jcyj=="、")
                {
                    jcyj = "----";
                }
                else
                {
                    jcyj = jcyj.Substring(1, jcyj.Length - 2);
                }
                if (pdbz == "、")
                {
                    pdbz = "----";
                }
                else
                {
                    pdbz = pdbz.Substring(1, pdbz.Length - 2);
                }
                updateSql = "update m_by set jcyj='" + jcyj + "',pdbz='" + pdbz + "' where recid='" + inparam+"'";
                retBool = SqlHelper.Execute(updateSql, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return retBool;
        }
        //获取需要从表View_PR_JCXM中查询符合要求的SQL
        public string GetJcyjSql(string inparam, string syxmbh)
        {
            string[] retArr = this.GetPr_jcxm_cxgl(syxmbh);
            string msg = "";
            string sql = "SELECT  ISNULL(BGXSMC,'') AS BGXSMC,  ISNULL(JCYJ,'') AS JCYJ, ISNULL(PDBZ,'') AS PDBZ FROM View_PR_JCXM WHERE SYXMBH='" + syxmbh + "'";
            string tmpSql = "";
            string cpmc = "";
            string lx1 = "";
            string lx2 = "";
            string jcxm = "";
            try
            {
                //cpmc是否有定义
                if (retArr[0] != null && retArr[0] != "" && retArr[1] != null && retArr[1] != "")
                {
                    if (retArr[0].ToUpper() != "M_BY")
                    {
                        tmpSql = "select " + retArr[1] + " from  " + retArr[0] + " where byzbrecid='" + inparam + "'";
                    }
                    else
                    {
                        tmpSql = "select " + retArr[1] + " from  " + retArr[0] + " where recid='" + inparam + "'";
                    }
                    IList<IDictionary<string, string>> dt1 = SqlHelper.GetDataTable(tmpSql, out msg);
                    if (dt1 != null)
                    {
                        if (dt1.Count > 0)
                        {
                            cpmc = dt1[0][retArr[1].ToLower()];
                            sql = sql + " AND CPMC='" + cpmc + "'";
                        }
                    }
                }
                //lx1是否有定义
                if (retArr[2] != null && retArr[2] != "" && retArr[3] != null && retArr[3] != "")
                {
                    if (retArr[2].ToUpper() != "M_BY")
                    {
                        tmpSql = "select " + retArr[3] + " from  " + retArr[2] + " where byzbrecid='" + inparam + "'";
                    }
                    else
                    {
                        tmpSql = "select " + retArr[3] + " from  " + retArr[2] + " where recid='" + inparam + "'";
                    }
                    IList<IDictionary<string, string>> dt2 = SqlHelper.GetDataTable(tmpSql, out msg);
                    if (dt2 != null)
                    {
                        if (dt2.Count > 0)
                        {
                            lx1 = dt2[0][retArr[3].ToLower()];
                            sql = sql + " AND LX1='" + lx1 + "'";
                        }
                    }
                }
                //lx2是否有定义
                if (retArr[4] != null && retArr[4] != "" && retArr[5] != null && retArr[5] != "")
                {
                    if (retArr[4].ToUpper() != "M_BY")
                    {
                        tmpSql = "select " + retArr[5] + " from  " + retArr[4] + " where byzbrecid='" + inparam + "'";
                    }
                    else
                    {
                        tmpSql = "select " + retArr[5] + " from  " + retArr[4] + " where recid='" + inparam + "'";
                    }
                    IList<IDictionary<string, string>> dt3 = SqlHelper.GetDataTable(tmpSql, out msg);
                    if (dt3 != null)
                    {
                        if (dt3.Count > 0)
                        {
                            lx2 = dt3[0][retArr[5].ToLower()];
                            sql = sql + " AND LX2='" + lx2 + "'";
                        }
                    }
                }
                //添加JCXM字段，过滤条件
                tmpSql = "select isnull(jcxm,'') as jcxm from  s_"+syxmbh+" where byzbrecid='" + inparam + "'";
                IList<IDictionary<string, string>> dt4 = SqlHelper.GetDataTable(tmpSql, out msg);
                if (dt4 != null)
                {
                    if (dt4.Count > 0)
                    {
                        jcxm = dt4[0]["jcxm"];
                        sql = sql + " AND ( '," + jcxm + ",' like '%,'+ bgxsmc +',%' or '、" + jcxm + "、' like '%、'+ bgxsmc +'、%')";
                    }
                }
                    
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return sql;
        }

        //用recid从m_by表中查询到试验项目编号（syxmbh）        
        public string GetSyxmbh(string inparam,out string msg)
        {
            string retStr="";
            try
            {
                IList<IDictionary<string, string>> dt = SqlHelper.GetDataTable("select syxmbh from m_by where recid='" + inparam + "'", out msg);
                if (dt != null)               
                    if (dt.Count > 0)
                    {
                        retStr=dt[0]["syxmbh"];
                    }
                    else
                        msg="找不到对应的记录";
                else
                    msg="找不到对应的记录";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return retStr;
        }
        /// 查询pr_jcxm_cxgl表，获取当前项目从表与View_PR_JCXM表中cpmc,lx1,lx2的对应字段。  
        public string[] GetPr_jcxm_cxgl(string syxmbh)
        {
            string msg="";
            string[] retArr=new string[6];
            try
            {
                IList<IDictionary<string, string>> dt = SqlHelper.GetDataTable("select isnull(cpmc_tab,'') as cpmc_tab,isnull(cpmc,'') as cpmc,isnull(lx1_tab,'') as lx1_tab,isnull(lx1,'') as lx1,isnull(lx2_tab,'') as lx2_tab,isnull(lx2,'') as lx2 from pr_jcxm_cxgl where syxmbh='" + syxmbh + "'", out msg);
                if (dt != null)  
                {
                    if (dt.Count > 0)
                    {
                        retArr[0] = dt[0]["cpmc_tab"];
                        retArr[1] = dt[0]["cpmc"];
                        retArr[2] = dt[0]["lx1_tab"];
                        retArr[3] = dt[0]["lx1"];
                        retArr[4] = dt[0]["lx2_tab"];
                        retArr[5] = dt[0]["lx2"];
                    }                
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return retArr;
        }
        
    }
    
}
