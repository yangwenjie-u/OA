using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BD.Jcbg.Common
{
    public class BarcodeOptionReport
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }

        public int Top { get; set; }

        public int PageModule { get; set; }

        public int PositionModule { get; set; }

        public int HSpan { get; set; }
        public int VSpan { get; set; }

    }
}
