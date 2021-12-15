using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;


namespace BD.Jcbg.Common
{
    public class ExcelPrintField
    {
        // 特殊字符在字符串的起始位置
        public int StartIndex { get; set; }
        // 特殊字符在字符串的结束位置      
        public int EndIndex { get; set; }
        // 特殊字符串格式
        public string SpecialFormat { get; set; }
        // 表格名
        public string TableName { get; set; }
        // 字段名
        public string FieldName { get; set; }
        // 重复方向
        public EnumExcelPrintRepeater Repeater { get; set; }
        // 汇总类型
        public EnumExcelPrintCollection Collection { get; set; }
        // 汇总分隔符
        public string CollectionSplit { get; set; }
        // 输出类型
        public EnumExcelPrintOutput Output { get; set; }
        // 输出日期格式
        public EnumExcelPrintOutputDate OutputDate { get; set; }
        // 输出图片尺寸
        public Size OutputImageSize { get; set; }
        // 输入输出方向
        public EnumExcelPrintDir Dir { get; set; }
        // 是否有效的特殊字符
        public bool IsValid { get; set; }

        public ExcelPrintField(int start, int end, string format)
        {
            StartIndex = start;
            EndIndex = end;
            SpecialFormat = format;
        }

        public bool Parse()
        {
            IsValid = false;
            try
            {
                if (!IsSpecial)
                    return IsValid;
                Dir = GetInoutDir();
                string tablename, fieldname;
                if (GetTableField(out tablename, out fieldname))
                {
                    TableName = tablename;
                    FieldName = fieldname;
                }
                Repeater = GetRepeater();
                Collection = GetCollection();
                CollectionSplit = GetCollectionSplit();
                Output = GetOutput();
                OutputImageSize = GetOutputSize();
                OutputDate = GetOutputDateFormat();

                IsValid = TableName != "" && FieldName != "";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return IsValid;
        }
        /// <summary>
        /// 是否特殊格式
        /// </summary>
        public bool IsSpecial
        {
            get
            {
                bool ret = false;
                try
                {
                    Regex reg = new Regex(@"#.*(F|M):.*#", RegexOptions.IgnoreCase);
                    MatchCollection matchs = reg.Matches(SpecialFormat);
                    ret = matchs.Count > 0;
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return ret;
            }
        }
        /// <summary>
        /// 获取输入输出类型
        /// </summary>
        protected EnumExcelPrintDir GetInoutDir()
        {
            EnumExcelPrintDir ret = EnumExcelPrintDir.Out;
            try
            {
                Regex reg = new Regex(@"[#-]D:[OIA][#-]", RegexOptions.IgnoreCase);
                MatchCollection matchs = reg.Matches(SpecialFormat);
                if (matchs.Count == 0)
                    return ret;
                string matchStr = matchs[0].Value;
                reg = new Regex(@"D:[OIA]", RegexOptions.IgnoreCase);
                matchs = reg.Matches(matchStr);
                matchStr = matchs[0].Value;
                matchStr = matchStr.Substring(2);
                if (matchStr.Equals("A", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintDir.InOut;
                else if (matchStr.Equals("I", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintDir.In;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取表名，字段名
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        protected bool GetTableField(out string tablename, out string fieldname)
        {
            bool ret = false;
            tablename = "";
            fieldname = "";
            try
            {
                Regex reg = new Regex(@"[#-]F:\w+.\w+[#-]", RegexOptions.IgnoreCase);
                MatchCollection matchs = reg.Matches(SpecialFormat);
                if (matchs.Count == 0)
                    return ret;
                string matchStr = matchs[0].Value;
                reg = new Regex(@"F:\w+.\w+", RegexOptions.IgnoreCase);
                matchs = reg.Matches(matchStr);
                matchStr = matchs[0].Value;
                matchStr = matchStr.Substring(2);
                string[] arr = matchStr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length < 2)
                    return ret;
                tablename = arr[0];
                fieldname = arr[1];
                ret = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取字段重复方向
        /// </summary>
        protected EnumExcelPrintRepeater GetRepeater()
        {
            EnumExcelPrintRepeater ret = EnumExcelPrintRepeater.NoRepeater;
            try
            {
                Regex reg = new Regex(@"[#-]R:[HV][#-]", RegexOptions.IgnoreCase);
                MatchCollection matchs = reg.Matches(SpecialFormat);
                if (matchs.Count == 0)
                    return ret;
                string matchStr = matchs[0].Value;
                reg = new Regex(@"R:[HV]", RegexOptions.IgnoreCase);
                matchs = reg.Matches(matchStr);
                matchStr = matchs[0].Value;
                matchStr = matchStr.Substring(2);
                if (matchStr.Equals("H", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintRepeater.Horizontal;
                else if (matchStr.Equals("V", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintRepeater.Vertical;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取汇总类型
        /// </summary>
        protected EnumExcelPrintCollection GetCollection()
        {
            EnumExcelPrintCollection ret = EnumExcelPrintCollection.NoCollection;
            try
            {
                Regex reg = new Regex(@"[#-]C:(C|T|D(\([^\)]+\))?|A(\([^\)]+\))?)[#-]", RegexOptions.IgnoreCase);
                MatchCollection matchs = reg.Matches(SpecialFormat);
                if (matchs.Count == 0)
                    return ret;
                string matchStr = matchs[0].Value;
                reg = new Regex(@"C:(C|T|D(\([^\)]+\))?|A(\([^\)]+\))?)", RegexOptions.IgnoreCase);
                matchs = reg.Matches(matchStr);
                matchStr = matchs[0].Value;
                matchStr = matchStr.Substring(2);
                if (matchStr.StartsWith("C", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintCollection.Count;
                else if (matchStr.StartsWith("T", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintCollection.Top;
                else if (matchStr.StartsWith("D", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintCollection.Distinct;
                else if (matchStr.StartsWith("A", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintCollection.All;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 汇总分隔符
        /// </summary>
        protected string GetCollectionSplit()
        {
            string ret = "";

            try
            {
                Regex reg = new Regex(@"[#-]C:[CTDA](\(.\))?[#-]", RegexOptions.IgnoreCase);
                MatchCollection matchs = reg.Matches(SpecialFormat);
                if (matchs.Count == 0)
                    return ret;
                string matchStr = matchs[0].Value;
                reg = new Regex(@"(\(.\))", RegexOptions.IgnoreCase);
                matchs = reg.Matches(matchStr);
                if (matchs.Count == 0)
                    return ret;
                matchStr = matchs[0].Value;
                ret = matchStr.Substring(1, matchStr.Length - 2);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取输出类型
        /// </summary>
        protected EnumExcelPrintOutput GetOutput()
        {
            EnumExcelPrintOutput ret = EnumExcelPrintOutput.Text;
            try
            {
                Regex reg = new Regex(@"[#-]O:(TM|DT(\([LS]\))?|B2(\(\d+,\d+\))?|B1(\(\d+,\d+\))?|T|I(\(\d+,\d+\))?|D(\([LS]\))?|S(\(\d+,\d+\))?)[#-]", RegexOptions.IgnoreCase);
                MatchCollection matchs = reg.Matches(SpecialFormat);
                if (matchs.Count == 0)
                    return ret;
                string matchStr = matchs[0].Value;
                reg = new Regex(@"O:(TM|DT(\([LS]\))?|B2(\(\d+,\d+\))?|B1(\(\d+,\d+\))?|T|I(\(\d+,\d+\))?|D(\([LS]\))?|S(\(\d+,\d+\))?)", RegexOptions.IgnoreCase);
                matchs = reg.Matches(matchStr);
                matchStr = matchs[0].Value;
                matchStr = matchStr.Substring(2);
                if (matchStr.StartsWith("TM", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintOutput.Time;
                else if (matchStr.StartsWith("DT", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintOutput.DateTime;
                else if (matchStr.StartsWith("B2", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintOutput.Barcode2;
                else if (matchStr.StartsWith("B1", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintOutput.Barcode1;
                else if (matchStr.StartsWith("T", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintOutput.Text;
                else if (matchStr.StartsWith("I", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintOutput.Image;
                else if (matchStr.StartsWith("D", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintOutput.Date;
                else if (matchStr.StartsWith("S", StringComparison.OrdinalIgnoreCase))
                    ret = EnumExcelPrintOutput.Signature;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取输出类型的宽高
        /// </summary>
        protected Size GetOutputSize()
        {
            Size ret = Size.Empty;
            try
            {
                Regex reg = new Regex(@"[#-]O:(TM|DT(\([LS]\))?|B2(\(\d+,\d+\))?|B1(\(\d+,\d+\))?|T|I(\(\d+,\d+\))?|D(\([LS]\))?|S(\(\d+,\d+\))?)[#-]", RegexOptions.IgnoreCase);
                MatchCollection matchs = reg.Matches(SpecialFormat);
                if (matchs.Count == 0)
                    return ret;
                string matchStr = matchs[0].Value;
                reg = new Regex(@"O:(TM|DT(\([LS]\))?|B2(\(\d+,\d+\))?|B1(\(\d+,\d+\))?|T|I(\(\d+,\d+\))?|D(\([LS]\))?|S(\(\d+,\d+\))?)", RegexOptions.IgnoreCase);
                matchs = reg.Matches(matchStr);
                matchStr = matchs[0].Value;
                matchStr = matchStr.Substring(2);

                if (matchStr.StartsWith("B2", StringComparison.OrdinalIgnoreCase) ||
                    matchStr.StartsWith("B1", StringComparison.OrdinalIgnoreCase) ||
                    matchStr.StartsWith("I", StringComparison.OrdinalIgnoreCase) ||
                    matchStr.StartsWith("S", StringComparison.OrdinalIgnoreCase))
                {
                    reg = new Regex(@"\(\d+,\d+\)", RegexOptions.IgnoreCase);
                    matchs = reg.Matches(matchStr);
                    if (matchs.Count == 0)
                        return ret;
                    matchStr = matchs[0].Value;
                    matchStr = matchStr.Substring(1, matchStr.Length - 2);
                    string[] arr = matchStr.Split(new char[] { ',' });
                    ret = new Size(arr[0].GetSafeInt(), arr[1].GetSafeInt());
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取输出日期或日期时间格式
        /// </summary>
        protected EnumExcelPrintOutputDate GetOutputDateFormat()
        {
            EnumExcelPrintOutputDate ret = EnumExcelPrintOutputDate.Short;
            try
            {
                Regex reg = new Regex(@"[#-]O:(TM|DT(\([LS]\))?|B2(\(\d+,\d+\))?|B1(\(\d+,\d+\))?|T|I(\(\d+,\d+\))?|D(\([LS]\))?|S(\(\d+,\d+\))?)[#-]", RegexOptions.IgnoreCase);
                MatchCollection matchs = reg.Matches(SpecialFormat);
                if (matchs.Count == 0)
                    return ret;
                string matchStr = matchs[0].Value;
                reg = new Regex(@"O:(TM|DT(\([LS]\))?|B2(\(\d+,\d+\))?|B1(\(\d+,\d+\))?|T|I(\(\d+,\d+\))?|D(\([LS]\))?|S(\(\d+,\d+\))?)", RegexOptions.IgnoreCase);
                matchs = reg.Matches(matchStr);
                matchStr = matchs[0].Value;
                matchStr = matchStr.Substring(2);

                if (matchStr.Equals("DT", StringComparison.OrdinalIgnoreCase) ||
                    matchStr.Equals("D", StringComparison.OrdinalIgnoreCase))
                {
                    reg = new Regex(@"\([LS]\)", RegexOptions.IgnoreCase);
                    matchs = reg.Matches(matchStr);
                    if (matchs.Count == 0)
                        return ret;
                    matchStr = matchs[0].Value;
                    matchStr = matchStr.Substring(1, matchStr.Length - 2);
                    if (matchStr.Equals("L"))
                        ret = EnumExcelPrintOutputDate.Long;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 获取格式化的文本
        /// </summary>
        /// <returns></returns>
        public IList<string> GetFormatedString(IDictionary<string, IList<IDictionary<string, object>>> datas)
        {
            IList<string> ret = new List<string>();
            RowCount = 1;
            FormatedString = null;
            try
            {
                if (IsImage())
                    return ret;
                // 表格在记录集中找不到
                IList<IDictionary<string, object>> tabledata;
                if (!datas.TryGetValue(TableName.ToLower(), out tabledata))
                {
                    ret.Add("异常：表格" + TableName + "找不到表数据。" + SpecialFormat);
                    return ret;
                }
                // 分隔符
                string split = CollectionSplit;
                if (split == "")
                    split = ",";
                // 返回内容
                StringBuilder sb = new StringBuilder();
                foreach (IDictionary<string, object> row in tabledata)
                {
                    string cellValue = GetSingleOutputString(row[FieldName.ToLower()]);
                    // 非汇总字段，添加每条记录
                    if (Collection == EnumExcelPrintCollection.NoCollection)
                        ret.Add(cellValue);
                    // 汇总字段
                    else
                    {
                        // 汇总所有记录
                        if (Collection == EnumExcelPrintCollection.All)
                        {
                            if (sb.Length > 0)
                                sb.Append(split);
                            sb.Append(cellValue);
                        }
                        // 计数字段，计数后退出
                        else if (Collection == EnumExcelPrintCollection.Count)
                        {
                            sb.Append(tabledata.Count.ToString());
                            break;
                        }
                        // 汇总不同内容
                        else if (Collection == EnumExcelPrintCollection.Distinct)
                        {
                            string str = split + sb.ToString() + split;
                            string str1 = split + cellValue + split;
                            if (str.IndexOf(str1) == -1)
                            {
                                if (sb.Length > 0)
                                    sb.Append(split);
                                sb.Append(cellValue);
                            }
                        }
                        // 取第一条记录
                        else if (Collection == EnumExcelPrintCollection.Top)
                        {
                            sb.Append(cellValue);
                            break;
                        }
                    }
                }
                // 汇总字段，或者非汇总自读并且当前没有记录
                if (ret.Count == 0)
                {
                    // 汇总字段，并且有记录
                    if (sb.Length > 0)
                        ret.Add(sb.ToString());
                    // 无记录
                    else
                    {
                        if (Collection == EnumExcelPrintCollection.Count)
                            ret.Add("0");
                        else
                            ret.Add("");
                    }
                }
                if (Collection == EnumExcelPrintCollection.NoCollection)
                    RowCount = ret.Count;
                FormatedString = ret;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.Add("异常：" + e.Message + "。" + SpecialFormat);
                FormatedString = ret;
            }
            return ret;
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public IList<byte[]> GetImages(IDictionary<string, IList<IDictionary<string, object>>> datas, SizeF cellSize, IDictionary<string, byte[]> signature)
        {
            IList<byte[]> ret = new List<byte[]>();
            RowCount = 1;
            Images = null;
            try
            {
                if (!IsImage())
                    return ret;
                // 表格在记录集中找不到
                IList<IDictionary<string, object>> tabledata;
                if (!datas.TryGetValue(TableName.ToLower(), out tabledata))
                    return ret;
               
                // 返回内容
                IList<object> addValues = new List<object>();

                foreach (IDictionary<string, object> row in tabledata)
                {
                    object cellValue = row[FieldName.ToLower()];
                    byte[] cellImage = GetSingleOutputImage(cellValue, (int)cellSize.Width, (int)cellSize.Height, signature);
                    // 非汇总字段，添加每条记录
                    if (Collection == EnumExcelPrintCollection.NoCollection)
                    {
                        ret.Add(cellImage);
                    }
                    // 汇总字段
                    else
                    {
                        // 汇总所有记录
                        if (Collection == EnumExcelPrintCollection.All)
                        {
                            ret.Add(cellImage);
                        }
                        // 汇总不同内容
                        else if (Collection == EnumExcelPrintCollection.Distinct)
                        {
                            
                            bool find = false;
                            if (Output == EnumExcelPrintOutput.Signature ||
                                Output == EnumExcelPrintOutput.Barcode1 ||
                                Output == EnumExcelPrintOutput.Barcode2)
                            {
                                var q = from e in addValues where e.GetSafeString().Equals(cellValue.GetSafeString()) select e;
                                find = q.Count() > 0;
                            }
                            else
                            {
                                var q = from e in addValues where e.IsSameArray(cellValue) select e;
                                find = q.Count() > 0;
                            }
                            if (!find)
                            {
                                ret.Add(cellImage);
                                addValues.Add(cellValue);
                            }
                        }
                        // 取第一条记录
                        else if (Collection == EnumExcelPrintCollection.Top)
                        {
                            ret.Add(cellImage);
                            break;
                        }
                    }
                }
                if (Collection == EnumExcelPrintCollection.NoCollection)
                    RowCount = ret.Count;
                Images = ret;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);                
            }
            return ret;
        }
        /// <summary>
        /// 是否是图片
        /// </summary>
        /// <returns></returns>
        public bool IsImage()
        {
            bool ret = false;
            try
            {
                if (!IsSpecial)
                    return ret;
                ret = Output == EnumExcelPrintOutput.Barcode1 || Output == EnumExcelPrintOutput.Barcode2 ||
                    Output == EnumExcelPrintOutput.Image || Output == EnumExcelPrintOutput.Signature;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 是否签名
        /// </summary>
        /// <returns></returns>
        public bool IsSignature(){
            bool ret = false;
            try
            {
                if (!IsSpecial)
                    return ret;
                ret = Output == EnumExcelPrintOutput.Signature;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 行数，GetImages或者GetFormatedString后才有效
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// 格式化的字符串，GetFormatedString后才有效
        /// </summary>
        public IList<string> FormatedString { get; set; }
        /// <summary>
        /// 格式化图片，GetImages后才有效
        /// </summary>
        public IList<byte[]> Images { get; set; }
        
        /// <summary>
        /// 获取单个格式化内容
        /// </summary>
        /// <returns></returns>
        protected string GetSingleOutputString(object objValue)
        {
            string ret = "";
            try
            {
                switch (Output){
                    case EnumExcelPrintOutput.Text:
                        ret = objValue.GetSafeString();
                        break;
                    case EnumExcelPrintOutput.Date:
                        if (OutputDate == EnumExcelPrintOutputDate.Long)
                            ret = objValue.GetSafeDate().ToString("yyyy年MM月dd日");
                        else
                            ret = objValue.GetSafeDate().ToString("yyyy-MM-dd");
                        break;
                    case EnumExcelPrintOutput.DateTime:
                        if (OutputDate == EnumExcelPrintOutputDate.Long)
                            ret = objValue.GetSafeDate().ToString("yyyy年MM月dd日 HH时mm分ss秒");
                        else
                            ret = objValue.GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case EnumExcelPrintOutput.Time:
                        if (OutputDate == EnumExcelPrintOutputDate.Long)
                            ret = objValue.GetSafeDate().ToString("HH时mm分ss秒");
                        else
                            ret = objValue.GetSafeDate().ToString("HH:mm:ss");
                        break;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取单个格式化图片
        /// </summary>
        /// <returns></returns>
        protected byte[] GetSingleOutputImage(object objValue, int width, int height, IDictionary<string, byte[]> signature)
        {
            byte[] ret = null;
            try
            {
                if (OutputImageSize != Size.Empty)
                {
                    width = OutputImageSize.Width;
                    height = OutputImageSize.Height;
                }
                switch (Output)
                {
                    case EnumExcelPrintOutput.Image:
                        ret = objValue as byte[];
                        break;
                    case EnumExcelPrintOutput.Barcode1:
                        if (objValue.GetSafeString() != "")
                            ret = Barcode.GetBarcode1(objValue.GetSafeString(), width, height);
                        break;
                    case EnumExcelPrintOutput.Barcode2:
                        if (objValue.GetSafeString() != "")
                        {
                            int twidth = 75, theight = 75;
                            if (!OutputImageSize.Equals(Size.Empty))
                            {
                                twidth = OutputImageSize.Width;
                                theight = OutputImageSize.Height;
                            }
                            ret = Barcode.GetBarcode2(objValue.GetSafeString(), twidth, theight);
                        }
                        break;
                    case EnumExcelPrintOutput.Signature:
                        signature.TryGetValue(objValue.ToString().ToLower(), out ret);
                        break;
                }
                if (ret != null && OutputImageSize != Size.Empty)
                {
                    MyImage img = new MyImage(ret);
                    ret = img.ConvertToJpg(OutputImageSize.Width, OutputImageSize.Height);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        
    }
}
