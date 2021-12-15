using System.Text;
using System.Web;
using BD.DataInputCommon;
using CryptFun = BD.Jcbg.Common.CryptFun;
using Bd.jcbg.Common;

namespace BD.Jcbg.Web.Common
{
    /// <summary>
    /// ExportExcel 的摘要说明
    /// </summary>
    public class ExportExcel : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string downFileName = Base64Func.DecodeBase64(Encoding.UTF8, context.Request["fileName"]);
            string downFile = CryptFun.Decode(context.Request["url"]);
            if (!string.IsNullOrEmpty(downFileName) && !string.IsNullOrEmpty(downFile))
            {
                context.Response.ContentType = "application/x-xls";
                context.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(downFileName, Encoding.UTF8));
                //指定编码 防止中文文件名乱码
                context.Response.HeaderEncoding = Encoding.UTF8;
                context.Response.TransmitFile(downFile);
            }
            context.Response.ContentType = "text/plain";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}