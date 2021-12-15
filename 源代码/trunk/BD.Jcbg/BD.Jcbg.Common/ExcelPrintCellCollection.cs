using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class ExcelPrintCellCollection
    {
        IList<ExcelPrintCell> mCells = new List<ExcelPrintCell>();

        public IList<ExcelPrintCell> Cells
        {
            get { return mCells; }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="cell"></param>
        public void Add(ExcelPrintCell cell)
        {
            try
            {
                mCells.Add(cell);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        /// <summary>
        /// 清除全部
        /// </summary>
        public void Clear()
        {
            try
            {
                mCells.Clear();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="cell"></param>
        public void Remove(ExcelPrintCell cell)
        {
            try
            {
                mCells.Remove(cell);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        /// <summary>
        /// 获取需要用到的表
        /// </summary>
        /// <returns></returns>
        public IList<string> GetTables()
        {
            IList<string> ret = new List<string>();
            try
            {
                foreach (ExcelPrintCell cell in mCells)
                {
                    if (!cell.IsSpecial)
                        continue;
                    foreach (ExcelPrintField field in cell.Fields)
                    {
                        if (!field.IsSpecial)
                            continue;
                        string table = field.TableName;
                        if (table != "")
                        {
                            var q = from e in ret where e.Equals(table, StringComparison.OrdinalIgnoreCase) select e;
                            if (q.Count() == 0)
                                ret.Add(table);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 获取表格及表格字段
        /// </summary>
        /// <returns></returns>
        public IDictionary<string,string> GetTableFields()
        {
            IDictionary<string,string> ret = new Dictionary<string,string>();
            ExcelPrintField curField = null;
            try
            {
                foreach (ExcelPrintCell cell in mCells)
                {
                    if (!cell.IsSpecial)
                        continue;
                    foreach (ExcelPrintField field in cell.Fields)
                    {
                        curField = field;
                        if (!field.IsSpecial)
                            continue;
                        string tablename = field.TableName.ToLower();
                        string fieldname = field.FieldName;

                        if (tablename != "")
                        {
                            string orgValue = "";

                            if (!ret.TryGetValue(tablename, out orgValue))
                                ret.Add(tablename, fieldname);
                            else
                            {
                                if (("," + orgValue + ",").IndexOf(","+fieldname+",", StringComparison.OrdinalIgnoreCase) == -1)
                                {
                                    orgValue += "," + fieldname;
                                    ret[tablename] = orgValue;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string str = "";
                if (curField != null)
                {
                    str = curField.SpecialFormat;
                }
                SysLog4.WriteLog(str, e);
            }
            return ret;
        }
        /// <summary>
        /// 单元格行逆序排列，列不变
        /// </summary>
        public void DescOrder()
        {
            try
            {
                var q = from e in mCells orderby e.StartColumn descending select e;
                mCells = q.ToList();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        /// <summary>
        /// 单元格行顺序排列，列不变
        /// </summary>
        public void AscOrder()
        {
            try
            {
                var q = from e in mCells orderby e.StartColumn ascending select e;
                mCells = q.ToList();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        /// <summary>
        /// 设置插入行数，rownum以下的单元格及字段，行数加上insertSum
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="insertSum"></param>
        public void SetInsertRow(int rowNum, int insertSum)
        {
            try
            {
                foreach (ExcelPrintCell cell in mCells)
                    cell.RemoveRow(rowNum, insertSum);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

    }
}
