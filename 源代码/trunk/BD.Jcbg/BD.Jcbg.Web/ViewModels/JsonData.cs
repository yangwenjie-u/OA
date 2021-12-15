using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BD.Jcbg.Web.ViewModels
{
    public class JsonData
    {
        public bool Success { get; set; } = false;

        public string Msg { get; set; }

        public object Data { get; set; }
    }
}