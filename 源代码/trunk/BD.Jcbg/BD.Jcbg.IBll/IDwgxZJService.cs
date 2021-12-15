using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.IBll
{
    public interface IDwgxZJService
    {
        IList<IDictionary<string, object>> GetSbcqbaByWybh(string wybh);

        bool GetSbReportFile(string serial, string reporttype, out byte[] file, out string msg);
    }

    
}
