using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.JC.JS.Common.Entities
{
    /// <summary>
    /// zdzd实体类
    /// </summary>
    public class EntityZdzd
    {
        public int SJGJ_ID { get; set; }
        public string SJBMC { get; set; }
        public string ZDMC { get; set; }
        public string SY{get;set;}
        public string ZDLX { get; set; }
        public int ZDCD1 { get; set; }
        public int ZDCD2 { get; set; }
        public string INPUTZDLX { get; set; }
        public string KJLX { get; set; }
        public bool SFBHZD { get; set; }
        public string BHMS { get; set; }
        public bool ZDSX { get; set; }
        public bool SFXS { get; set; }
        public int XSCD { get; set; }
        public decimal XSSX { get; set; }
        public bool SFGD { get; set; }
        public bool MUSTIN { get; set; }
        public string DEFAVAL { get; set; }
        public string HELPLNK { get; set; }
        public string CTRLSTRING { get; set; }
        public string ZDXZ { get; set; }
        public decimal WXSSX { get; set; }
        public bool WSFXS { get; set; }
        public string MSGINFO { get; set; }
        public string EQLFUNC { get; set; }
        public string HELPWHERE { get; set; }
        public bool GETBYBH { get; set; }
        public string SSJCX { get; set; }
        public bool SFBGZD { get; set; }
        public string VALIDPROC { get; set; }
        public string LX { get; set; }

        public bool IsValid { get; set; }
        public bool Load(IDictionary<string, string> row)
        {
            IsValid = false;
            try
            {
                SJGJ_ID = row["SJGJ_ID".ToLower()].GetSafeInt();
                SJBMC = row["SJBMC".ToLower()].GetSafeString();
                ZDMC = row["ZDMC".ToLower()].GetSafeString();
                SY = row["SY".ToLower()].GetSafeString();
                ZDLX = row["ZDLX".ToLower()].GetSafeString();
                ZDCD1 = row["ZDCD1".ToLower()].GetSafeInt();
                ZDCD2 = row["ZDCD2".ToLower()].GetSafeInt();
                INPUTZDLX = row["INPUTZDLX".ToLower()].GetSafeString();
                KJLX = row["KJLX".ToLower()].GetSafeString();
                SFBHZD = row["SFBHZD".ToLower()].GetSafeBool();
                BHMS = row["BHMS".ToLower()].GetSafeString();
                ZDSX = row["ZDSX".ToLower()].GetSafeBool();
                SFXS = row["SFXS".ToLower()].GetSafeBool();
                XSCD = row["XSCD".ToLower()].GetSafeInt();
                XSSX = row["XSSX".ToLower()].GetSafeDecimal();
                SFGD = row["SFGD".ToLower()].GetSafeBool();
                MUSTIN = row["MUSTIN".ToLower()].GetSafeBool();
                DEFAVAL = row["DEFAVAL".ToLower()].GetSafeString();
                HELPLNK = row["HELPLNK".ToLower()].GetSafeString();
                CTRLSTRING = row["CTRLSTRING".ToLower()].GetSafeString();
                ZDXZ = row["ZDXZ".ToLower()].GetSafeString();
                WXSSX = row["WXSSX".ToLower()].GetSafeDecimal();
                WSFXS = row["WSFXS".ToLower()].GetSafeBool();
                MSGINFO = row["MSGINFO".ToLower()].GetSafeString();
                EQLFUNC = row["EQLFUNC".ToLower()].GetSafeString();
                HELPWHERE = row["HELPWHERE".ToLower()].GetSafeString();
                GETBYBH = row["GETBYBH".ToLower()].GetSafeBool();
                SSJCX = row["SSJCX".ToLower()].GetSafeString();
                SFBGZD = row["SFBGZD".ToLower()].GetSafeBool();
                VALIDPROC = row["VALIDPROC".ToLower()].GetSafeString();
                LX = row["LX".ToLower()].GetSafeString();

                IsValid = true;
            }
            catch (Exception ex) { }
            return IsValid;
        }

        public bool IsStringField
        {
            get
            {
                return ZDLX.Equals("nvarchar", StringComparison.OrdinalIgnoreCase) ||
                    ZDLX.Equals("char", StringComparison.OrdinalIgnoreCase) ||
                    ZDLX.Equals("varchar", StringComparison.OrdinalIgnoreCase) ||
                    ZDLX.Equals("nchar", StringComparison.OrdinalIgnoreCase) ||
                    ZDLX.Equals("text", StringComparison.OrdinalIgnoreCase) ||
                    ZDLX.Equals("ntext", StringComparison.OrdinalIgnoreCase);
            }
        }

    }
}
