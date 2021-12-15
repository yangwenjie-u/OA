using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace BD.Jcbg.Common
{

    public class GZipUtil
    {

        /// <summary>    
        /// 字节数组压缩    
        /// </summary>    
        /// <param name="strSource"></param>    
        /// <returns></returns>    
        public static byte[] Compress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(data, 0, data.Length);
                zip.Close();
                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>    
        /// 字节数组解压缩    
        /// </summary>    
        /// <param name="strSource"></param>    
        /// <returns></returns>    
        public static byte[] Decompress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>  
        /// 字符串压缩  
        /// </summary>  
        /// <returns>The string.</returns>  
        /// <param name="str">String.</param>  
        public static string CompressString(string str)
        {
            try
            {
                string compressString = "";
                byte[] compressBeforeByte = Encoding.UTF8.GetBytes(str);
                byte[] compressAfterByte = Compress(compressBeforeByte);
                compressString = Convert.ToBase64String(compressAfterByte);
                return compressString;
            }
            catch (Exception e )
            {
                throw e;
            }

            
        }
        /// <summary>  
        /// 字符串解压缩  
        /// </summary>  
        /// <returns>The string.</returns>  
        /// <param name="str">String.</param>  
        public static string DecompressString(string str)
        {
            try
            {
                string decompressString = "";
                byte[] decompressBeforeByte = Convert.FromBase64String(str);
                byte[] decompressAfterByte = Decompress(decompressBeforeByte);
                decompressString = Encoding.UTF8.GetString(decompressAfterByte);
                return decompressString;
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

    }
}
