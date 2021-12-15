using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface ICompanyChangeDao : IBaseDao<CompanyChange, int>
    {
        IList<CompanyChange> GetItems(string start, string end);
    }
}
