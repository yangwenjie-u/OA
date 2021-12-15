using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface IAlertService
    {
        Alert Get(int id);

        void Update(Alert itm);


        /// <summary>
        /// </summary>
        /// <param name="Alert"></param>
        /// <returns></returns>
        bool deleteAlert(Alert Alert, out string msg);

        bool deleteAlert(int alertid, out string msg);

        bool saveAlert(Alert Alert, out string msg);
        Alert Save(Alert Alert);

    }
}
