using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

namespace BD.Jcbg.Common
{
    public class JsonDeSerializer<T>
    {
        public T DeSerializer(string str, out string msg)
        {
            T ret = default(T);
            msg = "";
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                var mStream = new MemoryStream(Encoding.UTF8.GetBytes(str));
                mStream.Seek(0, SeekOrigin.Begin);

                ret = (T)serializer.ReadObject(mStream);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(str, e);
                msg = e.Message;
            }
            return ret;
        }
        
    }
}
