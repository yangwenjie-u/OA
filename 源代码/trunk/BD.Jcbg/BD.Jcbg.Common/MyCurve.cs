using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Drawing;

namespace BD.Jcbg.Common
{
    struct ExtraPoint
    {
        public int imgindex;
        public int index;
        public int pointsum;
    };

    public class MyCurve
    {
        int COLLECTION_FREQ = 20;	// 数据采样频率
        const int PIC_SPACING = 10;		// 图片上下间距
        const int PIC_HEIHGT = 300;		// 图片高度
        const int PIC_WIDTH = 780;		// 图片宽度
        const int TITLE_HEIGHT = 30;		// 标题高度

        const int PIC_Y_TXT_WIDTH = 20;	// 纵坐标文字宽度
        const int PIC_X_TXT_HEIGHT = 20;	// 横坐标文字高度

        const int PIC_ARRAY_WIDTH = 5;	// 坐标箭头宽度
        const int PIC_ARRAY_HEIGHT = 5;	// 坐标箭头高度

        static Color PIC_COLOR_BG = Color.FromArgb(0, 0, 0);	// 背景颜色
        static Color PIC_COLOR_LINE = Color.FromArgb(255, 255, 0);	// 曲线颜色
        static Color PIC_COLOR_TEXT = Color.FromArgb(0, 255, 0);	// 坐标文字颜色
        static Color PIC_COLOR_ARR = Color.FromArgb(0, 128, 64);	// 坐标线颜色
        static Color PIC_COLOR_POINT = Color.FromArgb(255, 0, 0);	// 着重点颜色
        static Color PIC_COLOR_POINT_TXT = Color.FromArgb(255, 255, 0);	// 着重点文字颜色
        static Color PIC_COLOR_TITIE = Color.FromArgb(0, 255, 0);	// 标题文字颜色

        // 画整个图片
        public void DrawImage(HttpResponseBase resp, string strDataList, string strSybh, string strSyr, DateTime dtSyrq)
        {
            string[] arr1 = strDataList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arr2 = arr1[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            resp.ContentType = "image/gif";
            // 无内容
            if (arr1.Length < 2)
            {
                Bitmap image1 = new Bitmap(300, 60);
                Graphics g1 = Graphics.FromImage(image1);

                DrawError(g1, "无采集数据");

                image1.Save(resp.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);

                return;
            }
            // 数据格式不正确
            if (arr2.Length < 2)
            {
                Bitmap image1 = new Bitmap(300, 60);
                Graphics g1 = Graphics.FromImage(image1);

                DrawError(g1, "采集数据格式不正确");

                image1.Save(resp.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);

                return;
            }
            int portSum = arr2[0].GetSafeInt();		// 端口数
            int colFreq = arr2[1].GetSafeInt();		// 采样频率
            // 端口数或者采样频率不正确
            if (portSum > 2 || portSum < 1 || colFreq <= 0)
            {
                Bitmap image1 = new Bitmap(300, 60);
                Graphics g1 = Graphics.FromImage(image1);

                DrawError(g1, "采集数据格式不正确");

                image1.Save(resp.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);

                return;
            }
            COLLECTION_FREQ = colFreq;

            ArrayList arrPics = GetPicDatas(arr1[1]);

            resp.ContentType = "image/gif";

            int nWidth = GetImageWidth(arrPics);
            int nHeight = GetImageHeight(arrPics);
            Bitmap image = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(image);

            g.Clear(PIC_COLOR_BG);

            DrawTitle(g, nWidth, strSybh, strSyr, dtSyrq);

            int nTop = PIC_SPACING + TITLE_HEIGHT * 2 + PIC_SPACING;

            int maxT = GetMaxTime(arrPics);
            int maxD1 = GetMaxData1(arrPics);
            int maxD2 = GetMaxData2(arrPics);

            ArrayList arrT = new ArrayList();
            ArrayList arrD1 = new ArrayList();
            ArrayList arrD2 = new ArrayList();

            foreach (string str in arrPics)
            {
                GetData(str, arrD1, arrD2, arrT, COLLECTION_FREQ);

                if (portSum == 2)
                {
                    ArrayList arrExtPoints = GetExtraPoints(str, 1);
                    DrawOnePic(g, arrT, arrD1, arrExtPoints, 0, nTop, PIC_WIDTH / 3, PIC_HEIHGT, maxT, maxD1);
                    arrExtPoints = GetExtraPoints(str, 2);
                    DrawOnePic(g, arrT, arrD2, arrExtPoints, PIC_WIDTH / 3, nTop, PIC_WIDTH / 3, PIC_HEIHGT, maxT, maxD2);
                    arrExtPoints = GetExtraPoints(str, 3);
                    DrawOnePic(g, arrD2, arrD1, arrExtPoints, (PIC_WIDTH * 2) / 3, nTop, PIC_WIDTH / 3, PIC_HEIHGT, maxD2, maxD1);
                }
                else
                {
                    ArrayList arrExtPoints = GetExtraPoints(str, 0);
                    DrawOnePic(g, arrT, arrD1, arrExtPoints, 0, nTop, PIC_WIDTH, PIC_HEIHGT, maxT, maxD1);
                }
                nTop += PIC_HEIHGT + PIC_SPACING;
            }

            image.Save(resp.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
        }
        // 获取着重画的点
        protected ArrayList GetExtraPoints(string str, int index)
        {
            ArrayList ret = new ArrayList();

            string[] arr1 = str.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr1.Length < 2)
                return ret;

            string[] arr2 = arr1[1].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr2.Length; i++)
            {
                string[] arr3 = arr2[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr3.Length < 3)
                    continue;

                ExtraPoint point = new ExtraPoint();
                point.imgindex = arr3[0].GetSafeInt();
                point.index = arr3[1].GetSafeInt();
                point.pointsum = arr3[2].GetSafeInt();
                if (index < 1 || point.imgindex == index)
                    ret.Add(point);
            }
            return ret;
        }
        // 获取3坐标点
        protected void GetData(string str, ArrayList arrD1, ArrayList arrD2, ArrayList arrT, int colFreq)
        {
            arrD1.Clear();
            arrD2.Clear();
            arrT.Clear();

            double tSum = 1.0 / colFreq;

            string[] arr1 = str.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            str = arr1[0];

            string[] data = arr1[0].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < data.Length; i++)
            {
                string[] tmp = data[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                arrT.Add(i * tSum);
                arrD1.Add(tmp[0]);
                if (tmp.Length > 1)
                    arrD2.Add(tmp[1]);
            }
        }
        // 整个图片高度
        protected int GetImageHeight(ArrayList arrPicDatas)
        {
            return PIC_SPACING + TITLE_HEIGHT * 2 + arrPicDatas.Count * (PIC_SPACING + PIC_HEIHGT);
        }
        // 整个图片宽度
        protected int GetImageWidth(ArrayList arrPicDatas)
        {
            return PIC_WIDTH;
        }
        // 获取图片数据
        protected ArrayList GetPicDatas(string strDataList)
        {
            ArrayList arrRet = new ArrayList();

            try
            {
                string str = strDataList;

                string str1 = str.Trim(new char[] { '*' });
                string[] arr = str1.Split(new char[] { '*' });
                for (int i = 0; i < arr.Length; i++)
                    arrRet.Add(arr[i]);
            }
            catch { }
            return arrRet;
        }
        // 画标题
        protected void DrawTitle(Graphics g, int nWidth, string strSybh, string strSyr, DateTime dtSyrq)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            Font fo = new Font("宋体", 12, FontStyle.Regular);

            SolidBrush br = new SolidBrush(PIC_COLOR_TITIE);

            RectangleF rec = new RectangleF(0, PIC_SPACING, nWidth, TITLE_HEIGHT);

            g.DrawString("原始曲线(试验号：" + strSybh + "，试验人：" + strSyr + ")", fo, br, rec, sf);

            fo = new Font("宋体", 10, FontStyle.Regular);
            rec = new RectangleF(0, PIC_SPACING + TITLE_HEIGHT, nWidth, TITLE_HEIGHT);
            g.DrawString("试验时间：" + dtSyrq.ToString(), fo, br, rec, sf);
        }

        // 画空数据
        protected void DrawError(Graphics g, string err)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            Font fo = new Font("宋体", 12, FontStyle.Regular);

            SolidBrush br = new SolidBrush(PIC_COLOR_TITIE);

            RectangleF rec = new RectangleF(0, PIC_SPACING, 300, TITLE_HEIGHT);

            g.DrawString(err, fo, br, rec, sf);

        }


        // 画一个端口图片
        protected void DrawOnePic(Graphics g, ArrayList arrX, ArrayList arrY, ArrayList arrExtraPoints, int nPicLeft, int nPicTop, int nWidth, int nHeight, int nMaxX, int nMaxY)
        {
            // 横坐标最大值
            int nAllTime = nMaxX;
            int nMax = nMaxY;
            int nXCellValue = nAllTime / 10;	// 每个横坐标单位的值
            int nYCellValue = nMax / 10;		// 每个纵坐标单位的值
            if (nYCellValue == 0)
                nYCellValue = 1;

            // 坐标矩形
            int nLeft = PIC_Y_TXT_WIDTH + nPicLeft;
            int nTop = nPicTop;
            int nRight = nWidth + nPicLeft - PIC_Y_TXT_WIDTH;
            int nBottom = nTop + PIC_HEIHGT - PIC_X_TXT_HEIGHT;

            // 坐标
            g.DrawLine(new Pen(PIC_COLOR_ARR), new Point(nLeft, nTop), new Point(nLeft, nBottom));
            g.DrawLine(new Pen(PIC_COLOR_ARR), new Point(nLeft, nBottom), new Point(nRight, nBottom));
            g.DrawLine(new Pen(PIC_COLOR_ARR), new Point(nLeft - PIC_ARRAY_WIDTH, nTop + PIC_ARRAY_HEIGHT), new Point(nLeft, nTop));
            g.DrawLine(new Pen(PIC_COLOR_ARR), new Point(nLeft + PIC_ARRAY_WIDTH, nTop + PIC_ARRAY_HEIGHT), new Point(nLeft, nTop));
            g.DrawLine(new Pen(PIC_COLOR_ARR), new Point(nRight - PIC_ARRAY_WIDTH, nBottom - PIC_ARRAY_HEIGHT), new Point(nRight, nBottom));
            g.DrawLine(new Pen(PIC_COLOR_ARR), new Point(nRight - PIC_ARRAY_WIDTH, nBottom + PIC_ARRAY_HEIGHT), new Point(nRight, nBottom));
            // 坐标单位
            int nXSum = 10, nYSum = 10;	// 横坐标和纵坐标单元格数
            int nXPics = (nRight - nLeft) / nXSum;	// 横坐标每格像素值
            int nYPics = (nBottom - nTop) / nXSum;	// 纵坐标每格像素值
            // 纵坐标
            for (int i = 0; i < nYSum; i++)
            {
                g.DrawLine(new Pen(PIC_COLOR_ARR), new Point(nLeft, nTop + i * nYPics), new Point(nRight, nTop + i * nYPics));

                // 文字
                StringFormat sf = new StringFormat();
                sf.FormatFlags = StringFormatFlags.DirectionVertical;

                Font fo = new Font("Arial", 8, FontStyle.Regular);

                SolidBrush br = new SolidBrush(PIC_COLOR_TEXT);

                RectangleF rec = new RectangleF(nLeft - PIC_Y_TXT_WIDTH, nTop + i * nYPics - 10, nLeft, nTop + (i + 1) * nYPics);
                g.DrawString((nMax - i * nYCellValue).ToString(), fo, br, rec, sf);
            }
            // 横坐标
            for (int i = 0; i <= nXSum; i++)
            {
                g.DrawLine(new Pen(PIC_COLOR_ARR), new Point(nLeft + i * nXPics, nTop), new Point(nLeft + i * nXPics, nBottom));

                // 文字
                StringFormat sf = new StringFormat();

                Font fo = new Font("Arial", 8, FontStyle.Regular);

                SolidBrush br = new SolidBrush(PIC_COLOR_TEXT);

                RectangleF rec = new RectangleF(nLeft + i * nXPics - 5, nBottom, nLeft + (i + 1) * nXPics, nBottom + PIC_X_TXT_HEIGHT);
                g.DrawString((i * nXCellValue).ToString(), fo, br, rec, sf);
            }
            // 画曲线
            for (int i = 1; i < arrX.Count && i < arrY.Count; i++)
            {
                try
                {
                    int nXPos1 = nLeft + (int)(arrX[i - 1].GetSafeDecimal() * nXPics / nXCellValue);
                    int nYPos1 = nBottom - (int)(arrY[i - 1].GetSafeDecimal() * nYPics / nYCellValue);
                    int nXPos2 = nLeft + (int)(arrX[i].GetSafeDecimal() * nXPics / nXCellValue);
                    int nYPos2 = nBottom - (int)(arrY[i].GetSafeDecimal() * nYPics / nYCellValue);
                    g.DrawLine(new Pen(PIC_COLOR_LINE), new Point(nXPos1, nYPos1), new Point(nXPos2, nYPos2));
                }
                catch { }
            }
            // 着重点
            if (arrExtraPoints != null)
            {
                foreach (ExtraPoint point in arrExtraPoints)
                {
                    if (point.index >= arrY.Count || point.index <= 0)
                        continue;
                    decimal d1 = arrY[point.index].GetSafeDecimal();
                    string strtmp = d1.ToString();	// 数值

                    string strFormat = ".";
                    int nd2 = point.pointsum;
                    while (nd2-- > 0)
                    {
                        strFormat += "#";
                    }
                    strtmp = d1.ToString(strFormat);

                    try
                    {
                        // 坐标
                        int nXPos1 = nLeft + (int)(arrX[point.index].GetSafeDecimal() * nXPics / nXCellValue);
                        int nYPos1 = nBottom - (int)(d1 * nYPics / nYCellValue);
                        // 圆点
                        int nCircle = 3;
                        SolidBrush br = new SolidBrush(PIC_COLOR_POINT);
                        g.FillEllipse(br, new Rectangle(nXPos1 - nCircle, nYPos1 - nCircle, nCircle * 2, nCircle * 2));
                        // 文字
                        br = new SolidBrush(PIC_COLOR_POINT_TXT);
                        StringFormat sf = new StringFormat();
                        Font fo = new Font("Arial", 8, FontStyle.Regular);
                        RectangleF rec = new RectangleF(nXPos1 - 10, nYPos1 - nCircle - 20, nXPos1 + 200, nYPos1 - nCircle);
                        g.DrawString(strtmp, fo, br, rec, sf);
                    }
                    catch { }

                }
            }
        }


        // 获取所有图片纵坐标最大值
        protected int GetMaxY(ArrayList arrPics, int index)
        {
            if (index == 1 || index == 3)
                return GetMaxData1(arrPics);
            return GetMaxData2(arrPics);
        }
        // 获取所有图片横坐标最大值
        protected int GetMaxX(ArrayList arrPics, int index)
        {
            if (index == 1 || index == 2)
                return GetMaxTime(arrPics);

            return GetMaxData2(arrPics);
        }

        protected int GetMaxTime(ArrayList arrPics)
        {
            int ret = 0;
            foreach (string str in arrPics)
            {
                string[] arrAll = str.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                string strData = arrAll[0];
                string[] arrData = strData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                // 横坐标最大值
                int nAllTime = arrData.Length / COLLECTION_FREQ + (arrData.Length % COLLECTION_FREQ > 0 ? 1 : 0);
                if (nAllTime % 10 > 0)
                    nAllTime += (10 - nAllTime % 10);
                if (nAllTime > ret)
                    ret = nAllTime;
            }
            return ret;
        }

        protected int GetMaxData1(ArrayList arrPics)
        {
            int ret = 0;
            foreach (string str in arrPics)
            {
                string[] arrAll = str.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                string strData = arrAll[0];
                string[] arrData = strData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                decimal dMax = 0;
                for (int i = 0; i < arrData.Length; i++)
                {
                    string[] arrtmp = arrData[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    decimal d = arrtmp[0].GetSafeDecimal();
                    if (d > dMax)
                        dMax = d;
                }
                int n1 = 1;
                while (dMax / n1 > 1)
                {
                    n1 *= 10;
                }
                decimal nMax = 0;	// 纵坐标最大值
                while (nMax <= dMax && dMax > 0)
                {
                    nMax += n1 / 10;
                }
                if (nMax > ret)
                    ret = (int)nMax;
            }
            if (ret < 10)
                ret = 10;
            return ret;
        }

        protected int GetMaxData2(ArrayList arrPics)
        {
            int ret = 0;
            foreach (string str in arrPics)
            {
                string[] arrAll = str.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                string strData = arrAll[0];
                string[] arrData = strData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                decimal dMax = 0;
                for (int i = 0; i < arrData.Length; i++)
                {
                    string[] arrtmp = arrData[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arrtmp.Length < 2)
                        continue;
                    decimal d = arrtmp[1].GetSafeDecimal();
                    if (d > dMax)
                        dMax = d;
                }
                if (dMax == 0)
                    return ret;
                int n1 = 1;
                while (dMax / n1 > 1)
                {
                    n1 *= 10;
                }
                decimal nMax = 0;	// 纵坐标最大值
                while (nMax <= dMax && dMax > 0)
                {
                    nMax += n1 / 10;
                }
                if (nMax > ret)
                    ret = (int)nMax;
            }
            if (ret < 10)
                ret = 10;
            return ret;
        }
    }
}
