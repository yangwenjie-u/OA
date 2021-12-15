using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BD.Jcbg.IBll
{
    public interface IUpBgsdscService
    {
        bool UpReport(int recid, string pdf, string dwbh, out string barcodepdf, out string msg);

        bool GetReportFile(string pdfid, out byte[] file,  out string msg);

        bool GetReportFileByEWM(string ewm, out string thumbfileid, out string pdfid, out string msg);

        bool GetReportFileBySlt(string strSaveName, byte[] postcontent, string ext, Dictionary<string, object> dt);

        bool ReGenerateReport(string id, out string msg);

    }
}
