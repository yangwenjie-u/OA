using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ZXing;
using ZXing.Common;

namespace BD.Jcbg.Common
{
    public class QrCodeHelper
    {
        /// <summary>
        /// 读取二维码图片的内容
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ReadQrCode(byte[] bytes)
        {
            //返回值
            string content = "";
            try
            {
                var reader = new BarcodeReader
                {
                    Options = new DecodingOptions
                    {
                        CharacterSet = "UTF-8"
                    }
                };
                using (var ms = new MemoryStream(bytes))
                {
                    using (var bmp = new Bitmap(ms))
                    {
                        var code = reader.Decode(bmp);
                        content = code == null ? "" : code.Text;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return content;
        }
    }
}
