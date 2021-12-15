﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface ISysycdyurlDao:IBaseDao<SysYcdyUrl, int>
    {
        IList<SysYcdyUrl> Gets(string callid);
    }
}
