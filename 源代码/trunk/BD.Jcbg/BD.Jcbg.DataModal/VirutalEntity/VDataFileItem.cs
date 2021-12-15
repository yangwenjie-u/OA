using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [Serializable]
    public class VDataFileItem
    {
        public string FILEID { get; set; }
        public string FILENAME { get; set; }
        public byte[] FILECONTENT { get; set; }
        public string FILEEXT { get; set; }
        public string CJSJ { get; set; }
        public byte[] SMALLCONTENT { get; set; }

        public string NewFileName { get; set; }

    }
}
