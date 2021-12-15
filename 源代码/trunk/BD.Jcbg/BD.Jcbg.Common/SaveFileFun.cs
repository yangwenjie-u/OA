using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web;


namespace BD.Jcbg.Common
{
    public class SaveFileFun
    {
        public static void saveExcel(string filename, byte[] bytes, string fpath)
        {
            try
            {
                string filepath = System.AppDomain.CurrentDomain.BaseDirectory + fpath+"\\";
                string path = filepath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + CurrentUser.RealName + "_" + filename;//设定上传的文件路径
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
            }
            catch (Exception e)
            { }
        }

        public static bool saveExcelthread(string filename, byte[] bytes, string fpath)
        {
            bool code = true;
            try
            {
                string filepath = System.AppDomain.CurrentDomain.BaseDirectory + fpath + "\\";
                string path = filepath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + filename;//设定上传的文件路径
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
            }
            catch (Exception e)
            {
                code = false;
            }
            return code;
        }
    }
}
