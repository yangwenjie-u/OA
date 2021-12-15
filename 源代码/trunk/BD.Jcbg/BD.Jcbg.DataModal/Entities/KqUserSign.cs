/*
using MyGeneration/Template/NHibernate (c) by Sharp 1.4
based on OHM (alvy77@hotmail.com)
*/
using System;
using System.Collections;
using System.Collections.Generic;

namespace BD.Jcbg.DataModal.Entities
{

    /// <summary>
    /// IKqUserSign interface for NHibernate mapped table 'KqUserSign'.
    /// </summary>
    public interface IKqUserSign
    {
        #region Public Properties

        int Recid
        {
            get;
            set;

        }

        string UserCode
        {
            get;
            set;

        }

        DateTime? S1
        {
            get;
            set;

        }

        DateTime? S2
        {
            get;
            set;

        }

        DateTime? S3
        {
            get;
            set;

        }

        DateTime? S4
        {
            get;
            set;

        }

        string S1Type
        {
            get;
            set;

        }

        string S2Type
        {
            get;
            set;

        }

        string S3Type
        {
            get;
            set;

        }

        string S4Type
        {
            get;
            set;

        }

        string SignDate
        {
            get;
            set;

        }

        string S1Text
        {
            get;
            set;

        }

        string S2Text
        {
            get;
            set;

        }

        string S3Text
        {
            get;
            set;

        }

        string S4Text
        {
            get;
            set;

        }

        bool IsDeleted { get; set; }
        bool IsChanged { get; set; }

        #endregion
    }

    /// <summary>
    /// KqUserSign object for NHibernate mapped table 'KqUserSign'.
    /// </summary>
    [Serializable]
    public class KqUserSign : ICloneable, IKqUserSign
    {
        #region Member Variables

        protected int _recid;
        protected string _usercode;
        protected DateTime? _s1;
        protected DateTime? _s2;
        protected DateTime? _s3;
        protected DateTime? _s4;
        protected string _s1type;
        protected string _s2type;
        protected string _s3type;
        protected string _s4type;
        protected string _signdate;
        protected string _s1text;
        protected string _s2text;
        protected string _s3text;
        protected string _s4text;
        protected bool _bIsDeleted;
        protected bool _bIsChanged;
        #endregion

        #region Constructors
        public KqUserSign() { }

        public KqUserSign(string pUserCode, DateTime? pS1, DateTime? pS2, DateTime? pS3, DateTime? pS4, string pS1Type, string pS2Type, string pS3Type, string pS4Type, string pSignDate, string pS1Text, string pS2Text, string pS3Text, string pS4Text)
        {
            this._usercode = pUserCode;
            this._s1 = pS1;
            this._s2 = pS2;
            this._s3 = pS3;
            this._s4 = pS4;
            this._s1type = pS1Type;
            this._s2type = pS2Type;
            this._s3type = pS3Type;
            this._s4type = pS4Type;
            this._signdate = pSignDate;
            this._s1text = pS1Text;
            this._s2text = pS2Text;
            this._s3text = pS3Text;
            this._s4text = pS4Text;
        }

        public KqUserSign(string pUserCode)
        {
            this._usercode = pUserCode;
        }

        public KqUserSign(int pRecid)
        {
            this._recid = pRecid;
        }

        #endregion

        #region Public Properties

        public int Recid
        {
            get { return _recid; }
            set { _bIsChanged |= (_recid != value); _recid = value; }

        }

        public string UserCode
        {
            get { return _usercode; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("UserCode", "UserCode value, cannot contain more than 50 characters");
                _bIsChanged |= (_usercode != value);
                _usercode = value;
            }

        }

        public DateTime? S1
        {
            get { return _s1; }
            set { _bIsChanged |= (_s1 != value); _s1 = value; }

        }

        public DateTime? S2
        {
            get { return _s2; }
            set { _bIsChanged |= (_s2 != value); _s2 = value; }

        }

        public DateTime? S3
        {
            get { return _s3; }
            set { _bIsChanged |= (_s3 != value); _s3 = value; }

        }

        public DateTime? S4
        {
            get { return _s4; }
            set { _bIsChanged |= (_s4 != value); _s4 = value; }

        }

        public string S1Type
        {
            get { return _s1type; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("S1Type", "S1Type value, cannot contain more than 50 characters");
                _bIsChanged |= (_s1type != value);
                _s1type = value;
            }

        }

        public string S2Type
        {
            get { return _s2type; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("S2Type", "S2Type value, cannot contain more than 50 characters");
                _bIsChanged |= (_s2type != value);
                _s2type = value;
            }

        }

        public string S3Type
        {
            get { return _s3type; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("S3Type", "S3Type value, cannot contain more than 50 characters");
                _bIsChanged |= (_s3type != value);
                _s3type = value;
            }

        }

        public string S4Type
        {
            get { return _s4type; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("S4Type", "S4Type value, cannot contain more than 50 characters");
                _bIsChanged |= (_s4type != value);
                _s4type = value;
            }

        }

        public string SignDate
        {
            get { return _signdate; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("SignDate", "SignDate value, cannot contain more than 100 characters");
                _bIsChanged |= (_signdate != value);
                _signdate = value;
            }

        }

        public string S1Text
        {
            get { return _s1text; }
            set
            {
                if (value != null && value.Length > 1073741823)
                    throw new ArgumentOutOfRangeException("S1Text", "S1Text value, cannot contain more than 1073741823 characters");
                _bIsChanged |= (_s1text != value);
                _s1text = value;
            }

        }

        public string S2Text
        {
            get { return _s2text; }
            set
            {
                if (value != null && value.Length > 1073741823)
                    throw new ArgumentOutOfRangeException("S2Text", "S2Text value, cannot contain more than 1073741823 characters");
                _bIsChanged |= (_s2text != value);
                _s2text = value;
            }

        }

        public string S3Text
        {
            get { return _s3text; }
            set
            {
                if (value != null && value.Length > 1073741823)
                    throw new ArgumentOutOfRangeException("S3Text", "S3Text value, cannot contain more than 1073741823 characters");
                _bIsChanged |= (_s3text != value);
                _s3text = value;
            }

        }

        public string S4Text
        {
            get { return _s4text; }
            set
            {
                if (value != null && value.Length > 1073741823)
                    throw new ArgumentOutOfRangeException("S4Text", "S4Text value, cannot contain more than 1073741823 characters");
                _bIsChanged |= (_s4text != value);
                _s4text = value;
            }

        }


        public bool IsDeleted
        {
            get
            {
                return _bIsDeleted;
            }
            set
            {
                _bIsDeleted = value;
            }
        }

        public bool IsChanged
        {
            get
            {
                return _bIsChanged;
            }
            set
            {
                _bIsChanged = value;
            }
        }

        #endregion

        #region Equals And HashCode Overrides
        /// <summary>
        /// local implementation of Equals based on unique value members
        /// </summary>
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            KqUserSign castObj = null;
            try
            {
                castObj = (KqUserSign)obj;
            }
            catch (Exception) { return false; }
            return (castObj != null) &&
                (this._recid == castObj.Recid);
        }
        /// <summary>
        /// local implementation of GetHashCode based on unique value members
        /// </summary>
        public override int GetHashCode()
        {


            int hash = 57;
            hash = 27 * hash * _recid.GetHashCode();
            return hash;
        }
        #endregion

        #region ICloneable methods

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    #region Custom ICollection interface for KqUserSign


    public interface IKqUserSignCollection : ICollection
    {
        KqUserSign this[int index] { get; set; }
        void Add(KqUserSign pKqUserSign);
        void Clear();
    }

    [Serializable]
    public class KqUserSignCollection : IKqUserSignCollection
    {
        private IList<KqUserSign> _arrayInternal;

        public KqUserSignCollection()
        {
            _arrayInternal = new List<KqUserSign>();
        }

        public KqUserSignCollection(IList<KqUserSign> pSource)
        {
            _arrayInternal = pSource;
            if (_arrayInternal == null)
            {
                _arrayInternal = new List<KqUserSign>();
            }
        }

        public KqUserSign this[int index]
        {
            get
            {
                return _arrayInternal[index];
            }
            set
            {
                _arrayInternal[index] = value;
            }
        }

        public int Count { get { return _arrayInternal.Count; } }
        public bool IsSynchronized { get { return false; } }
        public object SyncRoot { get { return _arrayInternal; } }
        public void CopyTo(Array array, int index) { _arrayInternal.CopyTo((KqUserSign[])array, index); }
        public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
        public void Add(KqUserSign pKqUserSign) { _arrayInternal.Add(pKqUserSign); }
        public void Clear() { _arrayInternal.Clear(); }
        public IList<KqUserSign> GetList() { return _arrayInternal; }
    }

    #endregion
}
