using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class Wgrydata
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public IList<IDictionary<string, string>> Datas { get; set; }
    }

    public class WgrydataList
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public IDictionary<string, string> Datas { get; set; }
    }

    public class FORMDATA
    {
        public bool success { get; set; }
        public string msg { get; set; }

        public S_FORMDATA data { get; set; }

    }
    public class S_FORMDATA
    {
        public string total { get; set; }
        public IList<IDictionary<string, string>> rows { get; set; }
    }
    public class FORMDATA2
    {
        public bool success { get; set; }
        public string msg { get; set; }

        public S_FORMDATA2 data { get; set; }

    }
    public class S_FORMDATA2
    {
         public string total { get; set; }
         public IList<IList<IDictionary<string, string>>> rows { get; set; }
    }

    //企业端
    public class WgryQYdata
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public WgryQYdataRows Datas { get; set; }
    }

    public class WgryQYdataRows
    {
        public List<WgryQYdataRow> p1 { get; set; }
        public List<WgryQYdataRow> p2 { get; set; }
        public List<WgryQYdataRow> p3 { get; set; }
    }

    public class WgryQYdataRow
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
