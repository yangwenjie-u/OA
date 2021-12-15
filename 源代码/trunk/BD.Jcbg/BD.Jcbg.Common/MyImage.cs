using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace BD.Jcbg.Common
{
	public class MyImage
	{
		byte[] FileData = null;

		public MyImage(byte[] filedata)
		{
			FileData = filedata;
		}

		public MyImage(string path)
		{
			try
			{
				FileData = System.IO.File.ReadAllBytes(path);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
		/// <summary>
		/// 判断是否是图片
		/// </summary>
		/// <returns></returns>
		public bool IsImage()
		{
            bool ret = false;
            if (FileData == null)
                return false;
            Stream stream = null;
            Image img = null;
            try
            {
                stream = new MemoryStream(FileData);
                img = Image.FromStream(stream);
                if (img == null)
                    ret = false;
                else
                    ret =
                    img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg) ||
                    img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp) ||
                    img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif) ||
                    img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png) ||
                    img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff);
            }
            catch (Exception e)
			{
				SysLog4.WriteLog(e);
                ret = false;
			}
            finally
            {
                if (img != null)
                    img.Dispose();
                if (stream != null)
                    stream.Dispose();
            }
			return ret;
		}
		/// <summary>
		/// 获取允许的图片格式
		/// </summary>
		/// <returns></returns>
		public string GetValidImageDesc()
		{
			return "jpeg,bmp,gif,png,tiff,jpg";
		}
		/// <summary>
		/// 其他格式转换成jpg
		/// </summary>
		/// <returns></returns>
        public byte[] ConvertToJpg(int maxwidth, int maxheight, bool zoomoutImage = false, bool lockRate = true)
		{
            byte[] ret = null;
            if (!IsImage())
                return ret;
            try
            {
                using (Stream stream = new MemoryStream(FileData))
                {
                    Image img = Image.FromStream(stream);
                    Image sImage = GetThumbnail(img, maxheight, maxwidth, zoomoutImage, lockRate);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        sImage.Save(ms, ImageFormat.Jpeg);


                        ret = new byte[ms.Length];
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Read(ret, 0, ret.Length);
                    }

                    sImage.Dispose();
                    img.Dispose();
                }
            }
            catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		/// <summary>
		/// 图片等比缩放
		/// </summary>
		/// <param name="b"></param>
		/// <param name="destHeight"></param>
		/// <param name="destWidth"></param>
		/// <returns></returns>
        public Image GetThumbnail(Image b, int destHeight, int destWidth, bool zoomoutImage = false, bool lockRate = true)
		{
            Bitmap outBmp = null;
            try
            {
                System.Drawing.Image imgSource = b;

                System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;

                int sW = 0, sH = 0;
                // 按比例缩放   

                int sWidth = imgSource.Width;
                int sHeight = imgSource.Height;

                if (destHeight == 0)
                    destHeight = sHeight;
                if (destWidth == 0)
                    destWidth = sWidth;

                if (!zoomoutImage)
                {
                    if (destHeight > sHeight)
                        destHeight = sHeight;
                    if (destWidth > sWidth)
                        destWidth = sWidth;
                }
                if (lockRate)
                {
                    if ((sWidth * destHeight) > (sHeight * destWidth))
                    {
                        destHeight = (destWidth * sHeight) / sWidth;
                    }
                    else
                    {
                        destWidth = (sWidth * destHeight) / sHeight;
                    }

                }

                if ((sHeight > destHeight) || (sWidth > destWidth) || zoomoutImage || !lockRate)
                {
                    if ((sWidth * destHeight) > (sHeight * destWidth))
                    {
                        sW = destWidth;
                        sH = (destWidth * sHeight) / sWidth;
                    }
                    else
                    {
                        sH = destHeight;
                        sW = (sWidth * destHeight) / sHeight;
                    }

                    outBmp = new Bitmap(destWidth, destHeight);
                    Graphics g = Graphics.FromImage(outBmp);
                    g.Clear(Color.White);

                    // 设置画布的描绘质量     
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
                    g.Dispose();
                    // 以下代码为保存图片时，设置压缩质量  
                    /*
					EncoderParameters encoderParams = new EncoderParameters();
					long[] quality = new long[1];
					quality[0] = 70;
					EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
					encoderParams.Param[0] = encoderParam;*/
                    imgSource.Dispose();
                }
                else
                {
                    //sW = sWidth;
                    //sH = sHeight;
                    return b;
                }




            }
            catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return outBmp;

		}

        /// <summary>
        /// 获取原数据，非图片直接返回
        /// </summary>
        /// <returns></returns>
        public byte[] GetOrgImage()
        {
            if (!IsImage())
                return FileData;
            if (!Configs.OrgImageDeal)
                return FileData;
            byte[] ret = ConvertToJpg(Configs.OrgImageWidth, Configs.OrgImageHeight, Configs.OrgImageZoomOut, Configs.OrgImageLockRate);
            if (ret == null)
                ret = FileData;
            return ret;
        }
        /// <summary>
        /// 获取缩略图，非图片直接返回
        /// </summary>
        /// <returns></returns>
        public byte[] GetThumbnail()
        {
            if (!IsImage())
                return FileData;
            byte[] ret = ConvertToJpg(Configs.ThumbImageWidth, Configs.ThumbImageHeight, Configs.ThumbImageZoomOut, Configs.ThumbImageLockRate);
            if (ret == null)
                ret = FileData;
            return ret;
        }
	}
}
