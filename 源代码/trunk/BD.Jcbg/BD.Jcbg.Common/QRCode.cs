using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class QRCode
    {
        public static byte[] GetQrCode2(string text, int pixel)
        {
            byte[] ret = null;
            try
            {
                QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                //QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCoder.QRCodeGenerator.ECCLevel.M/* 这里设置容错率的一个级别 */, true, true, QRCoder.QRCodeGenerator.EciMode.Utf8, 6);
                QRCoder.QRCode qrcode = new QRCoder.QRCode(qrCodeData);

                Bitmap qrCode2 = qrcode.GetGraphic(pixel, Color.Black, Color.White, null, 15, 6, false);

                using (MemoryStream stream = new MemoryStream())
                {
                    qrCode2.Save(stream, ImageFormat.Jpeg);
                    ret = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(ret, 0, Convert.ToInt32(stream.Length));
                }

                //File.WriteAllBytes("d:\\QRCode.jpg", ret);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
    }
}
