using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BD.Jcbg.IBll;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using Spring.Transaction.Interceptor;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;

namespace BD.Jcbg.Bll
{
    public class ExcelPrintService : IExcelPrintService
    {
        #region 用到的Dao
		ICommonDao CommonDao { get; set; }
		#endregion

        #region 服务
        public byte[] FormatWts(string filepath,IDictionary<string,string> wheres, MyDelegates.FuncGetUserSign funcGetUserSign)
        {
            byte[] ret = null;
            try
            {
                FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                IWorkbook workbook = null;
                if (filepath.EndsWith(".xlsx")) // 2007版本
                    workbook = new XSSFWorkbook(file);
                else
                    workbook = new HSSFWorkbook(file);
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    // 遍历，获取特殊需要替换的单元格
                    ExcelPrintCellCollection pcells = new ExcelPrintCellCollection();
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null)
                            continue;
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell == null)
                                continue;
                            
                            int rowSpan, colSpan;
                            bool isMerge = IsMergeCell(sheet, i+1, j+1, out rowSpan, out colSpan);
                            ExcelPrintCell pcell = new ExcelPrintCell(cell.ToString(), i, j, rowSpan, colSpan);
                            if (pcell.IsSpecial)
                                pcells.Add(pcell);                            
                        }
                    }
                    // 获取数据库数据
                    IDictionary<string,string> tables = pcells.GetTableFields();
                    IDictionary<string, IList<IDictionary<string, object>>> datas = GetDatas(tables, wheres);
                    // 格式化内容。插入行，插入后重新设置设置单元格行列号
                    IDictionary<int, int> insertRows = new Dictionary<int, int>();  // 记录某行插入的行数
                    foreach (ExcelPrintCell pcell in pcells.Cells)
                    {
                        if (!pcell.IsSpecial)
                            continue;
                        IRow row = sheet.GetRow(pcell.StartRow);
                        ICell cell = row.GetCell(pcell.StartColumn);
                        foreach (ExcelPrintField field in pcell.Fields)
                        {
                            if (!field.IsValid)
                                continue;
                            // 图片
                            if (field.IsImage())
                            {
                                IDictionary<string, byte[]> signatures = null;
                                if (field.IsSignature())
                                    signatures = GetSignatures(field.TableName, field.FieldName, datas, funcGetUserSign);
                                
                                SizeF size = GetCellSizePix(sheet, pcell.StartRow, pcell.StartColumn, pcell.RowSpan, pcell.ColumnSpan);
                                //SysLog4.WriteError(pcell.StartRow + "," + pcell.StartColumn + "," + pcell.RowSpan + "," + pcell.ColumnSpan+","+size.Width+","+size.Height);
                                field.GetImages(datas, size, signatures);
                            }
                            // 文字
                            else 
                                field.GetFormatedString(datas);
                            // 多条记录
                            if (field.RowCount > 1)
                            {
                                // 纵向设置单元格值
                                if (field.Repeater == EnumExcelPrintRepeater.Vertical)
                                {
                                    /*
                                    int fieldNeedInsert = field.RowCount - 1;
                                    int hasInsertSum = 0;
                                    bool hasValue = insertRows.TryGetValue(pcell.StartRow, out hasInsertSum);
                                    int realNeedInsert = fieldNeedInsert - hasInsertSum;
                                    if (realNeedInsert > 0)
                                    {
                                        
                                        Size size = GetCellSize(sheet, pcell.StartRow, pcell.StartColumn, pcell.RowSpan, pcell.ColumnSpan);
                                        InsertRow(sheet, pcell.StartRow, realNeedInsert, row.Height);

                                        pcells.SetInsertRow(pcell.StartRow, realNeedInsert);
                                        if (hasValue)
                                            insertRows[pcell.StartRow] = fieldNeedInsert;
                                        else
                                            insertRows.Add(pcell.StartRow, realNeedInsert);
                                    }*/
                                    for (int i = 1; i < field.RowCount; i++)
                                    {
                                        int rowIndex = pcell.StartRow + i;
                                        IRow tmpRow = sheet.GetRow(rowIndex);
                                        if (tmpRow == null)
                                            tmpRow = sheet.CreateRow(rowIndex);
                                        ICell destCell = tmpRow.GetCell(pcell.StartColumn);
                                        if (destCell == null)
                                            destCell = tmpRow.CreateCell(pcell.StartColumn);
                                        //Size size = GetCellSize(sheet, pcell.StartRow, pcell.StartColumn, pcell.RowSpan, pcell.ColumnSpan);
                                        //sheet.SetColumnWidth(cellIndex, size.Width);
                                        destCell.SetCellValue(cell.GetSafeString());
                                    }
                                }
                                // 横向设置单元格值
                                else
                                {
                                    /*
                                    for (int i = 1; i < field.RowCount; i++)
                                    {
                                        int cellIndex = pcell.StartColumn + i;
                                        ICell destCell = row.GetCell(cellIndex);
                                        if (destCell == null)
                                            destCell = row.CreateCell(cellIndex);
                                        Size size = GetCellSize(sheet, pcell.StartRow, pcell.StartColumn, pcell.RowSpan, pcell.ColumnSpan);
                                        sheet.SetColumnWidth(cellIndex, size.Width);
                                        destCell.SetCellValue(cell.GetSafeString());
                                    }*/
                                    for (int i = 1; i < field.RowCount; i++)
                                    {
                                        int cellIndex = pcell.StartColumn + i;
                                        ICell destCell = row.GetCell(cellIndex);
                                        if (destCell == null)
                                            destCell = row.CreateCell(cellIndex);
                                        //Size size = GetCellSize(sheet, pcell.StartRow, pcell.StartColumn, pcell.RowSpan, pcell.ColumnSpan);
                                        //sheet.SetColumnWidth(cellIndex, size.Width);
                                        destCell.SetCellValue(cell.GetSafeString());
                                    }
                                }
                            }
                        }
                    }
                    // 替换内容
                    foreach (ExcelPrintCell pcell in pcells.Cells)
                    {
                        if (!pcell.IsSpecial)
                            continue;
                        
                        foreach (ExcelPrintField field in pcell.Fields)
                        {
                            if (!field.IsValid)
                                continue;
                            IRow row = sheet.GetRow(pcell.StartRow);
                            ICell cell = row.GetCell(pcell.StartColumn);
                            // 图片
                            if (field.IsImage())
                            {
                                // 只替换非空记录
                                if (field.RowCount > 0)
                                {
                                    // 只有一条记录
                                    if (field.RowCount == 1)
                                    {
                                        // 一个图片，直接用图片填
                                        if (field.Images.Count >= 0)
                                        {
                                            if (field.Images[0] != null)
                                            {
                                                cell.SetCellValue("");
                                                int pictureIdx = workbook.AddPicture(field.Images[0], PictureType.JPEG);
                                                IDrawing patriarch = sheet.CreateDrawingPatriarch();
                                                IClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, pcell.StartColumn, pcell.StartRow, pcell.StartColumn + pcell.ColumnSpan - 1, pcell.StartRow + pcell.RowSpan - 1);
                                                IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);
                                                pict.Resize();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        // 纵向重复
                                        if (field.Repeater == EnumExcelPrintRepeater.Vertical)
                                        {
                                            for (int i = 0; i < field.RowCount; i++)
                                            {
                                                if (field.Images[i] == null)
                                                    continue;
                                                int index = pcell.StartRow + i;
                                                row = sheet.GetRow(index);
                                                cell = row.GetCell(pcell.StartColumn);

                                                cell.SetCellValue("");
                                                int pictureIdx = workbook.AddPicture(field.Images[i], PictureType.JPEG);
                                                IDrawing patriarch = sheet.CreateDrawingPatriarch();
                                                IClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, pcell.StartColumn, index, pcell.StartColumn + pcell.ColumnSpan - 1, index + pcell.RowSpan - 1);
                                                IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);
                                                pict.Resize();
                                            }
                                        }
                                        // 横向重复
                                        else
                                        {
                                            for (int i = 0; i < field.RowCount; i++)
                                            {
                                                if (field.Images[i] == null)
                                                    continue;
                                                int index = pcell.StartColumn + i;
                                                cell = row.GetCell(index);

                                                cell.SetCellValue("");
                                                int pictureIdx = workbook.AddPicture(field.Images[i], PictureType.JPEG);
                                                IDrawing patriarch = sheet.CreateDrawingPatriarch();
                                                IClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, index, pcell.StartRow, index + pcell.ColumnSpan - 1, pcell.StartRow + pcell.RowSpan - 1);
                                                IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);
                                                pict.Resize();
                                            }
                                        }
                                    }
                                }
                            }
                            // 文字
                            else
                            {
                                // 只替换非空记录
                                if (field.RowCount > 0)
                                {
                                    // 替换当前行
                                    string str = cell.GetSafeString();
                                    str = str.Replace(field.SpecialFormat, field.FormatedString[0]);
                                    cell.SetCellValue(str);
                                    // 多行
                                    if (field.RowCount > 1){
                                        // 纵向重复
                                        if (field.Repeater == EnumExcelPrintRepeater.Vertical)
                                        {
                                            for (int i = 1; i < field.RowCount; i++)
                                            {
                                                int index = pcell.StartRow + i;
                                                row = sheet.GetRow(index);
                                                cell = row.GetCell(pcell.StartColumn);

                                                str = cell.GetSafeString();
                                                str = str.Replace(field.SpecialFormat, field.FormatedString[i]);
                                                cell.SetCellValue(str);
                                            }
                                        }
                                        // 横向重复
                                        else
                                        {
                                            for (int i = 1; i < field.RowCount; i++)
                                            {
                                                int index = pcell.StartColumn + i;
                                                cell = row.GetCell(index);

                                                str = cell.GetSafeString();
                                                str = str.Replace(field.SpecialFormat, field.FormatedString[i]);
                                                cell.SetCellValue(str);
                                            }
                                        }

                                    }
                                }
                                
                                
                            }
                        }
                    }

                }
                MemoryStream memoryFile = new MemoryStream();
                workbook.Write(memoryFile);
                ret = memoryFile.ToArray();
                memoryFile.Close();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;

        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 获取单元格合并的内容
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colSpan"></param>
        /// <returns></returns>
        protected bool IsMergeCell(ISheet sheet, int rowNum, int colNum, out int rowSpan, out int colSpan)
        {
            bool result = false;
            rowSpan = 0;
            colSpan = 0;
            if ((rowNum < 1) || (colNum < 1)) return result;
            int rowIndex = rowNum - 1;
            int colIndex = colNum - 1;
            int regionsCount = sheet.NumMergedRegions;
            rowSpan = 1;
            colSpan = 1;
            for (int i = 0; i < regionsCount; i++)
            {
                CellRangeAddress range = sheet.GetMergedRegion(i);
                sheet.IsMergedRegion(range);
                if (range.FirstRow == rowIndex && range.FirstColumn == colIndex)
                {
                    rowSpan = range.LastRow - range.FirstRow + 1;
                    colSpan = range.LastColumn - range.FirstColumn + 1;
                    break;
                }
            }
            try
            {
                result = sheet.GetRow(rowIndex).GetCell(colIndex).IsMergedCell;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return result;
        }
        /// <summary>
        /// 获取数据库数据
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="wheres"></param>
        /// <returns></returns>
        protected IDictionary<string, IList<IDictionary<string,object>>> GetDatas(IDictionary<string, string> tables, IDictionary<string, string> wheres)
        {
            IDictionary<string, IList<IDictionary<string, object>>> ret = new Dictionary<string, IList<IDictionary<string, object>>>();
            try
            {
                for (int i = 0; i < tables.Keys.Count; i++)
                {
                    string tablename = tables.Keys.ElementAt(i);
                    string fields = tables[tablename].Trim(new char[]{','});
                    if (fields.Length == 0)
                        continue;
                    string where = "";
                    if (!wheres.TryGetValue(tablename, out where))
                        where = "1=1";
                    string sql = "select " + fields + " from " + tablename + " where " + where;
                    IList<IDictionary<string, object>> dt = CommonDao.GetBinaryDataTable(sql);
                    ret.Add(tablename, dt);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sourceRowIndex">源行</param>
        /// <param name="rowSum">插入行数</param>
        /// <param name="diff">偏移数</param>
        /// <returns></returns>
        protected bool InsertRow(ISheet sheet, int sourceRowIndex, int rowSum, int rowHeight)
        {
            bool ret = false;
            try
            {
                IRow sourceRow = sheet.GetRow(sourceRowIndex);
                // 下面的行后移
                if (sheet.LastRowNum>sourceRowIndex)
                    sheet.ShiftRows(sourceRowIndex + 1, sheet.LastRowNum, rowSum, true, false);

                // 复制新行
                /*
                for (int i = 1; i <= rowSum; i++)
                {
                    IRow row = sheet.CopyRow(sourceRowIndex, sourceRowIndex + i);
                    row.Height = (short)rowHeight;
                    
                }*/
                
                int sourceCellCount = sourceRow.Cells.Count;
                int startMergeCell = -1; //记录每行的合并单元格起始位置
                for (int i = 1; i <= rowSum; i++)
                {
                    IRow targetRow = null;
                    ICell sourceCell = null;
                    ICell targetCell = null;
                    int targetIndex = sourceRowIndex + i;

                    targetRow = sheet.CreateRow(targetIndex);
                    targetRow.Height = sourceRow.Height;//复制行高

                    for (int m = sourceRow.FirstCellNum; m < sourceRow.LastCellNum; m++)
                    {
                        sourceCell = sourceRow.GetCell(m);
                        if (sourceCell == null)
                            continue;
                        targetCell = targetRow.CreateCell(m);
                        targetCell.CellStyle = sourceCell.CellStyle;//赋值单元格格式
                        targetCell.SetCellType(sourceCell.CellType);

                        //以下为复制模板行的单元格合并格式
                        if (sourceCell.IsMergedCell)
                        {
                            if (startMergeCell <= 0)
                                startMergeCell = m;
                            else if (startMergeCell > 0 && sourceCellCount == m + 1)
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(i, i, startMergeCell, m));
                                startMergeCell = -1;
                            }
                        }
                        else
                        {
                            if (startMergeCell >= 0)
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(i, i, startMergeCell, m - 1));
                                startMergeCell = -1;
                            }
                        }

                        ICell sourceSell = sourceRow.GetCell(m);
                        if (sourceSell != null)
                            targetCell.SetCellValue(sourceSell.GetSafeString());
                    }
                }

                ret = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 获取每行需要插入的最大行数
        /// </summary>
        /// <returns></returns>
        //protected IDictionary<int, int> GetRowInsertSum()
        /// <summary>
        /// 根据表名，字段名，即存储在字段中的值为userid
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        protected IDictionary<string, byte[]> GetSignatures(string tablename, string fieldname,
            IDictionary<string, IList<IDictionary<string, object>>> datas, MyDelegates.FuncGetUserSign funcGetUserSign)
        {
            IDictionary<string, byte[]> ret = new Dictionary<string, byte[]>();
            try
            {
                IList<IDictionary<string, object>> table = null;
                if (datas.TryGetValue(tablename.ToLower(), out table))
                {
                    foreach (IDictionary<string, object> row in table)
                    {
                        object username = null;
                        byte[] arrSign = null;
                        if (row.TryGetValue(fieldname.ToLower(), out username))
                        {
                            if (!ret.TryGetValue(username.GetSafeString(), out arrSign))
                            {
                                string sign = "";
                                if (funcGetUserSign(username.GetSafeString(), out sign))
                                    ret.Add(username.GetSafeString(), sign.DecodeBase64Array());
                            }
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
        /// 获取当前单元格宽高（非像素），单元格可能是多个格子合并的
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        protected Size GetCellSize(ISheet sheet, int row, int column, int rowSpan, int columnSpan)
        {
            Size size = Size.Empty;
            try
            {
                for (int i = row; i < row + rowSpan; i++)
                    size.Width += sheet.GetColumnWidth(i);
                for (int i = column; i < column + columnSpan; i++)
                    size.Height += sheet.GetRow(i).Height;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return size;
         
        }
        /// <summary>
        /// 获取当前单元格宽高（像素），单元格可能是多个格子合并的
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        protected SizeF GetCellSizePix(ISheet sheet, int row, int column, int rowSpan, int columnSpan)
        {
            SizeF size = SizeF.Empty;
            try
            {
                for (int i = row; i < row + rowSpan; i++)
                    size.Height += sheet.GetRow(i).HeightInPoints;
                for (int i = column; i < column + columnSpan; i++)
                    size.Width += sheet.GetColumnWidthInPixels(i);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return size;

        }
        #endregion
    }
}
