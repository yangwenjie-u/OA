using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace BD.Jcbg.Common
{
    public class ExcelPrintCell
    {
        string mFormat = "";
        int mStartRow;
        int mStartColumn;
        int mRowSpan;
        int mColSpan;
        IList<ExcelPrintField> mFields = new List<ExcelPrintField>();

        public bool IsSpecial { get; set; }

        public IList<ExcelPrintField> Fields
        {
            get { return mFields; }
        }
        public ExcelPrintCell(string format, int row, int column, int rowspan, int colspan)
        {
            mFormat = format;
            mStartRow = row;
            mStartColumn = column;
            mRowSpan = rowspan;
            mColSpan = colspan;

            Load();
        }
        /// <summary>
        /// 原始字符串
        /// </summary>
        public string OrginString
        {
            get { return mFormat; }
        }
        /// <summary>
        /// 开始行号
        /// </summary>
        public int StartRow
        {
            get { return mStartRow; }
        }
        /// <summary>
        /// 开始列号
        /// </summary>
        public int StartColumn
        {
            get { return mStartColumn; }
        }
        /// <summary>
        /// 结束行号
        /// </summary>
        public int RowSpan
        {
            get { return mRowSpan; }
        }
        /// <summary>
        /// 结束列号
        /// </summary>
        public int ColumnSpan
        {
            get { return mColSpan; }
        }
        

        protected void Load()
        {
            IsSpecial = false;
            mFields.Clear();
            try
            {
                Regex reg = new Regex(@"#[^#]*#", RegexOptions.IgnoreCase);
                MatchCollection matchs = reg.Matches(mFormat);

                foreach (Match match in matchs)
                {
                    ExcelPrintField field = new ExcelPrintField(match.Index, match.Index + match.Length-1, match.Value);
                    if (field.Parse())
                        mFields.Add(field);
                }

                IsSpecial = Fields.Count > 0;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        /// <summary>
        /// 移动行号,rowSum必须大于0， 当然行大于startRowNum有效
        /// </summary>
        /// <param name="startRowNum"></param>
        /// <param name="rowSum"></param>
        public void RemoveRow(int startRowNum, int rowSum)
        {
            try
            {
                if (mStartRow <= startRowNum || rowSum <= 0)
                    return;
                mStartRow += rowSum;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

    }
}
