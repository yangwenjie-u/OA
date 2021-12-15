using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface ICompanyChangeTotalItemDao : IBaseDao<CompanyChangeTotalItem, int>
    {
        IList<CompanyChangeTotalItem> GetItems(int changetotalid);
    }
}
