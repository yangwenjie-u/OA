using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;


namespace BD.Jcbg.IBll
{
    public interface IDataFileService
	{
        bool SaveDataFile(string fileid, string filename, byte[] filecontent, string fileext, string cjsj, out string msg, string storgetype = "");
    }
}
