using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.IBll
{
    public interface IDwgxSxjzyService
    {
        bool DelJSFZR(string rybh, out string msg);

        bool DelZCJZS(string rybh, out string msg);

        bool DelZJYSZCRY(string rybh, out string msg);

        bool DelXCGLRY(string rybh, out string msg);

        bool DelJSGR(string rybh, out string msg);

        bool DelJXSB(int recid, out string msg);

        bool DelGCYJ(int recid, out string msg);

        IList<IDictionary<string, object>> GetQyzzSblx();

        bool GetQyzzSqb(string id, string reporttype, string reportfile,out string msg, out byte[] file);


        bool GetAreaAK(string areaKey, out string msg, out string areaAK);

        IList<IDictionary<string, string>> GetAreaList();

        bool WriteZjhq(string idlist, string userlist, out string msg);

        bool Zjqrhq(int recid, string sfty, string zjqryj,out string msg);

        IList<IDictionary<string, object>> GetQyzzsbYCRY(string id);

        bool GetQyQGCXPTID(string qybh, out string msg, out string ptid);

        bool DelQYZZ(string id, out string msg);

        bool GetQyzzReportFile(string id, string reporttype, out byte[] file, out string msg);

        IList<IDictionary<string, object>> GetYcQyzzInfo(string id);

        bool GetHCQKFile(string id, string reporttype, out byte[] file, out string msg);
    }
}
