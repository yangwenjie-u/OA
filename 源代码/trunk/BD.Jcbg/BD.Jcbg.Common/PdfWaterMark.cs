using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Drawing;

namespace BD.Jcbg.Common
{
    public static class PdfWaterMark
    {
        public static byte[] SetWaterMark(byte[] orgFile, byte[] imgFile, int pageModule, Point pLt, Size wSize)
        {
            byte[] ret = orgFile;
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;

            try
            {
                pdfReader = new PdfReader(orgFile);

                int numberOfPages = pdfReader.NumberOfPages;

                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);

                float width = psize.Width;

                float height = psize.Height;
                string tmpfilepath = SysEnvironment.CurPath + @"\tempfiles\" + DateTime.Now.Ticks.ToString() + ".pdf";
                Stream stream = new FileStream(tmpfilepath, FileMode.Create, FileAccess.ReadWrite); //new MemoryStream();
                pdfStamper = new PdfStamper(pdfReader, stream);

                PdfContentByte waterMarkContent;

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imgFile);

                //image.GrayFill = 20;//透明度，灰色填充
                //image.Rotation//旋转
                //image.RotationDegrees//旋转角度
                //水印的位置 
                if (pLt.X < 0)
                {
                    pLt.X = (int)(width - image.Width + pLt.X);
                }

                image.SetAbsolutePosition(pLt.X, (height - image.Height) - pLt.Y);

                //每一页加水印,也可以设置某一页加水印 
                for (int i = 1; i <= numberOfPages; i++)
                {
                    if (pageModule == 0 || pageModule == i || (pageModule > numberOfPages && i == 1))
                    {
                        waterMarkContent = pdfStamper.GetOverContent(i);

                        waterMarkContent.AddImage(image);
                    }
                }

                pdfStamper.Close();
                pdfReader.Close();
                stream.Close();

                ret = System.IO.File.ReadAllBytes(tmpfilepath);

                System.IO.File.Delete(tmpfilepath);

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally
            {

            }
            return ret;
        }

        public static byte[] SetWaterMark(byte[] orgFile, byte[] imgFile, int pageModule, int positionModule, int hspan, int vspan)
        {
            byte[] ret = orgFile;
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;

            try
            {
                pdfReader = new PdfReader(orgFile);

                int numberOfPages = pdfReader.NumberOfPages;

                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);

                float width = psize.Width;

                float height = psize.Height;
                string tmpfilepath = SysEnvironment.CurPath + @"\tempfiles\" + DateTime.Now.Ticks.ToString() + ".pdf";
                Stream stream = new FileStream(tmpfilepath, FileMode.Create, FileAccess.ReadWrite); //new MemoryStream();
                pdfStamper = new PdfStamper(pdfReader, stream);

                PdfContentByte waterMarkContent;

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imgFile);

                PointF pt = GetStartPoint(psize.Width, psize.Height, image.Width, image.Height, positionModule, hspan, vspan);

                //image.GrayFill = 20;//透明度，灰色填充
                //image.Rotation//旋转
                //image.RotationDegrees//旋转角度
                //水印的位置 
                if (pt.X < 0)
                {
                    pt.X = width - image.Width + pt.X;
                }

                image.SetAbsolutePosition(pt.X, (height - image.Height) - pt.Y);

                //每一页加水印,也可以设置某一页加水印 
                for (int i = 1; i <= numberOfPages; i++)
                {
                    if (pageModule == 0 || pageModule == i || (pageModule > numberOfPages && i == 1))
                    {
                        waterMarkContent = pdfStamper.GetOverContent(i);

                        //兼容横放和竖放的排版
                        if (i > 1)
                        {
                            psize = pdfReader.GetPageSize(i);
                            pt = GetStartPoint(psize.Width, psize.Height, image.Width, image.Height, positionModule, hspan, vspan);

                            //水印的位置 
                            if (pt.X < 0)
                            {
                                pt.X = psize.Width - image.Width + pt.X;
                            }

                            image.SetAbsolutePosition(pt.X, (psize.Height - image.Height) - pt.Y);
                        }

                        waterMarkContent.AddImage(image);
                    }
                }

                pdfStamper.Close();
                pdfReader.Close();
                stream.Close();

                ret = System.IO.File.ReadAllBytes(tmpfilepath);

                System.IO.File.Delete(tmpfilepath);

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally
            {

            }
            return ret;
        }

        /// <summary>
        /// 把位置模式转换为具体坐标
        /// </summary>
        /// <param name="pageWidth"></param>
        /// <param name="pageHeight"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="pageModule"></param>
        /// <param name="hspan"></param>
        /// <param name="vspan"></param>
        /// <returns></returns>
        public static PointF GetStartPoint(float pageWidth, float pageHeight, float imageWidth, float imageHeight,
            int pageModule, int hspan, int vspan)
        {
            PointF ret = PointF.Empty;
            try
            {
                // 左上角
                if (pageModule == 1)
                    ret = new PointF(hspan, vspan);
                // 左下角
                else if (pageModule == 2)
                    ret = new PointF(hspan, pageHeight - imageHeight - vspan);
                // 右上角
                else if (pageModule == 3)
                    ret = new PointF(pageWidth - imageWidth - hspan, vspan);
                // 右下角
                else if (pageModule == 4)
                    ret = new PointF(pageWidth - imageWidth - hspan, pageHeight - imageHeight - vspan);

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public static byte[] MergeFiles(IList<byte[]> files, out string msg)
        {
            byte[] ret = null;
            msg = "";
            PdfReader reader;
            Document doc = new Document();
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                try
                {
                    doc.Open();
                    PdfContentByte cb = writer.DirectContent;
                    PdfImportedPage newPage;
                    if (doc.IsOpen())
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            reader = new PdfReader(files[i]);
                            {
                                int iPageNum = reader.NumberOfPages;
                                for (int j = 1; j <= iPageNum; j++)
                                {
                                    newPage = writer.GetImportedPage(reader, j);
                                    iTextSharp.text.Rectangle r = reader.GetPageSize(j);
                                    doc.SetPageSize(r);
                                    doc.NewPage();
                                    cb.AddTemplate(newPage, 0, 0);
                                }
                            }
                        }
                    }
                    doc.Close();
                    ret = stream.GetBuffer();
                }
                catch (Exception e)
                {
                    throw e;
                }
                
            }
            return ret;
        }
    }
}
