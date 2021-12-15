using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespRegisterUser:VTransPayRespBase
    {
        public VTransPayRespRegisterUserData data;
    }

    public class VTransPayRespRegisterUserData
    {
        public string UserId { get; set; }
    }
}
