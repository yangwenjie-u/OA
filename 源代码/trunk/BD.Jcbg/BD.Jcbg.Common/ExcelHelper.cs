using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class ExcelHelper
    {
        public static IWorkbook Export(List<Dictionary<string, string>> list, IList<IDictionary<string, string>> datas)
        {
            //生成Excel
            IWorkbook wk = new XSSFWorkbook();
            //居中样式
            ICellStyle cellstyle = wk.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            cellstyle.Alignment = HorizontalAlignment.Center;
            //创建一个Sheet  
            ISheet sheet = wk.CreateSheet("导出信息");
            //sheet.DefaultColumnWidth = 15 * 10;
            //sheet.DefaultRowHeightInPoints = 15;  
            IRow row;
            ICell cell;
            //定义导出标题
            row = sheet.CreateRow(0);
            for (int i = 0; i < list.Count; i++)
            {
                sheet.SetColumnWidth(i, 20 * 256);
                //定义每一列
                cell = row.CreateCell(i);
                //设置值
                cell.SetCellValue(list[i]["title"]);
                //设置样式
                cell.CellStyle = cellstyle;
            }
            //定义数据行
            int datanum = 0;
            foreach (var data in datas)
            {
                datanum = datanum + 1;
                row = sheet.CreateRow(datanum);
                //创建行数据
                for (int i = 0; i < list.Count; i++)
                {
                    cell = row.CreateCell(i); ;
                    //设置值
                    cell.SetCellValue(data[list[i]["name"]]);
                }
            }

            return wk;
        }
    }
}
