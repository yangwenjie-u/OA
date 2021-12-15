using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface IReportWWGZfileService
    {
        ReportWWGZfile Get(int id);

        void Update(ReportWWGZfile itm);


        /// <summary>
        /// </summary>
        /// <param name="ReportWWGZfile"></param>
        /// <returns></returns>
        bool deleteReportWWGZfile(ReportWWGZfile ReportWWGZfile, out string msg);

        bool deleteReportWWGZfile(int ReportWWGZfileid, out string msg);

        bool saveReportWWGZfile(ReportWWGZfile ReportWWGZfile, out string msg);
        ReportWWGZfile Save(ReportWWGZfile ReportWWGZfile);

    }
}
