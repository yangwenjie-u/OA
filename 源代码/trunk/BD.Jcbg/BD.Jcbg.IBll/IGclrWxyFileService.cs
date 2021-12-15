using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface IGclrWxyFileService
    {
        GclrWxyFile Get(int id);


        bool deleteGclrWxyFile(int fileid, out string msg);

        bool saveGclrWxyFile(GclrWxyFile GclrWxyFile, out string msg);

        GclrWxyFile Save(GclrWxyFile GclrWxyFile);

    }
}
