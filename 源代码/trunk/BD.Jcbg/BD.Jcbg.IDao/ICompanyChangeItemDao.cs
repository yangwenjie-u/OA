using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface ICompanyChangeItemDao : IBaseDao<CompanyChangeItem, int>
    {
        IList<CompanyChangeItem> GetItems(int changeid);
        void GetSum(int changeid, int type, out int no, out decimal money);
        void GetAllSum(string depid, int type, out int no, out decimal money);
    }
}
