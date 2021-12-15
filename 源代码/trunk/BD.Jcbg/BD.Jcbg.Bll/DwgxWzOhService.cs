using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.IBll;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using Spring.Transaction.Interceptor;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace BD.Jcbg.Bll
{
    public class DwgxWzOhService:IDwgxWzOhService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        #endregion

        #region 获取整改单回复详情

        /// <summary>
        /// 根据整改单编号，获取整改单回复详情
        /// </summary>
        /// <param name="zgdbh"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public IList<IDictionary<string, object>> GetZgdHfxq(string zgdbh)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
          
            return ret;
        }



        #endregion

    }
}
