using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface IReportWWGZService
    {
        ReportWWGZ Get(int id);

        void Update(ReportWWGZ itm);


        /// <summary>
        /// </summary>
        /// <param name="ReportWWGZ"></param>
        /// <returns></returns>
        bool deleteReportWWGZ(ReportWWGZ ReportWWGZ, out string msg);

        bool deleteReportWWGZ(int ReportWWGZid, out string msg);

        bool saveReportWWGZ(ReportWWGZ ReportWWGZ, out string msg);
        ReportWWGZ Save(ReportWWGZ ReportWWGZ);

    }
}
