using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Common;
using ZXing.Rendering;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 条形码/二维码生成
    /// </summary>
    public static class Barcode
    {
        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] GetBarcode1(string text, int width, int height)
        {
            byte[] ret = null;
            try
            {
                EncodingOptions opt = new EncodingOptions() { Width = width, Height = height, Margin = 0 };
                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.CODE_39;
                writer.Options = opt;
                Bitmap bmp = writer.Write(text);
                using (MemoryStream stream = new MemoryStream())
                {
                    bmp.Save(stream, ImageFormat.Jpeg);
                    ret = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(ret, 0, Convert.ToInt32(stream.Length));
                }
                //File.WriteAllBytes("d:\\b1.jpg", ret);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] GetBarcode2(string text, int width, int height)
        {
            byte[] ret = null;
            try
            {
                //int nsize = Math.Min(width, height); 
                // Margin 设置0 有bug
                EncodingOptions opt = new QrCodeEncodingOptions() { Width = width, Height = height, DisableECI = true, CharacterSet = "UTF-8", Margin = 1 };
                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options = opt;
                Bitmap bmp = writer.Write(text);

                using (MemoryStream stream = new MemoryStream())
                {
                    bmp.Save(stream, ImageFormat.Jpeg);
                    ret = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(ret, 0, Convert.ToInt32(stream.Length));
                }

                //File.WriteAllBytes("d:\\GetBarcode2.jpg", ret);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public static byte[] GetBarcode2NoWhite(string text, int width, int height)
        {
            byte[] ret = null;
            try
            {
                var matrix = new MultiFormatWriter().encode(text, BarcodeFormat.QR_CODE, width, height);
                matrix = CutWhiteBorder(matrix);

                QrCodeEncodingOptions options = new QrCodeEncodingOptions();
                options.CharacterSet = "UTF-8";
                options.DisableECI = true;
                options.ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H;
                options.Margin = 1;

                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options = options;

                writer.Options.Width = matrix.Width;
                writer.Options.Height = matrix.Height;
                var bmp = writer.Write(matrix);

                using (var img = new Bitmap(bmp, width, height))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        img.Save(stream, ImageFormat.Jpeg);
                        ret = new byte[stream.Length];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(ret, 0, Convert.ToInt32(stream.Length));
                    }
                }

                //File.WriteAllBytes("d:\\GetBarcode2NoWhite.jpg", ret);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        private static BitMatrix CutWhiteBorder(BitMatrix matrix)
        {
            int[] rec = matrix.getEnclosingRectangle();
            int resWidth = rec[2] + 1;
            int resHeight = rec[3] + 1;
            BitMatrix resMatrix = new BitMatrix(resWidth + 1, resHeight + 1);
            resMatrix.clear();
            for (int i = 0; i < resWidth; i++)
            {
                for (int j = 0; j < resHeight; j++)
                {
                    if (matrix[i + rec[0], j + rec[1]])
                    {
                        resMatrix.flip(i + 1, j + 1);
                    }
                }
            }
            return resMatrix;
        }
    }
}
