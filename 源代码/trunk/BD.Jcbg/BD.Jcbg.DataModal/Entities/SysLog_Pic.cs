
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
    #region SysLogPic

    /// <summary>
    /// SysLogPic object for NHibernate mapped table 'SysLog_Pic'.
    /// </summary>
    [Serializable]
    public class SysLogPic
    {
        #region Member Variables

        protected int _iD;
        protected string _userCode;
        protected string _picName;
        protected byte[] _picContent;
        protected DateTime _createTime;

        #endregion

        #region Constructors

        public SysLogPic() { }

        public SysLogPic(string userCode, string picName, byte[] picContent, DateTime createTime)
        {
            this._userCode = userCode;
            this._picName = picName;
            this._picContent = picContent;
            this._createTime = createTime;
        }

        #endregion

        #region Public Properties

        public int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        public string UserCode
        {
            get { return _userCode; }
            set
            {
                if (value != null && value.Length > 40)
                    throw new ArgumentOutOfRangeException("Invalid value for UserCode", value, value.ToString());
                _userCode = value;
            }
        }

        public string PicName
        {
            get { return _picName; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for PicName", value, value.ToString());
                _picName = value;
            }
        }

        public byte[] PicContent
        {
            get { return _picContent; }
            set { _picContent = value; }
        }

        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }


        #endregion
    }

    #endregion
}
