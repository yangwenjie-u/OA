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
    /// IInfoSchedule interface for NHibernate mapped table 'InfoSchedule'.
    /// </summary>
    public interface IInfoSchedule
    {
        #region Public Properties

        string Recid
        {
            get;
            set;

        }

        string ScheduleName
        {
            get;
            set;

        }

        string CompanyId
        {
            get;
            set;

        }

        string StartTime
        {
            get;
            set;

        }

        string EndTime
        {
            get;
            set;

        }

        string ScheduleSum
        {
            get;
            set;

        }

        string ProjectId
        {
            get;
            set;

        }

        string HasDelete
        {
            get;
            set;

        }

        string Zt
        {
            get;
            set;

        }

        string Sfjs
        {
            get;
            set;

        }

        int? FreeTime
        {
            get;
            set;

        }
        int? KqTimes
        {
            get;
            set;

        }
        string LjTime
        {
            get;
            set;

        }
        bool IsDeleted { get; set; }
        bool IsChanged { get; set; }

        #endregion
    }

    /// <summary>
    /// InfoSchedule object for NHibernate mapped table 'InfoSchedule'.
    /// </summary>
    [Serializable]
    public class InfoSchedule : ICloneable, IInfoSchedule
    {
        #region Member Variables

        protected string _recid;
        protected string _schedulename;
        protected string _companyid;
        protected string _starttime;
        protected string _endtime;
        protected string _schedulesum;
        protected string _projectid;
        protected string _hasdelete;
        protected string _zt;
        protected string _sfjs;
        protected int? _freetime;
        protected int? _kqtimes;
        protected string _ljtime;
        protected bool _bIsDeleted;
        protected bool _bIsChanged;
        #endregion

        #region Constructors
        public InfoSchedule() { }

        public InfoSchedule(string pRecid, string pScheduleName, string pCompanyId, string pStartTime, string pEndTime, string pScheduleSum, string pProjectId, string pHasDelete, string pZt, string pSfjs, int? pFreeTime, int? pKqTimes, string pLjTime)
        {
            this._recid = pRecid;
            this._schedulename = pScheduleName;
            this._companyid = pCompanyId;
            this._starttime = pStartTime;
            this._endtime = pEndTime;
            this._schedulesum = pScheduleSum;
            this._projectid = pProjectId;
            this._hasdelete = pHasDelete;
            this._zt = pZt;
            this._sfjs = pSfjs;
            this._freetime = pFreeTime;
            this._kqtimes = pKqTimes;
            this._ljtime = pLjTime;
        }

        #endregion

        #region Public Properties

        public string Recid
        {
            get { return _recid; }
            set
            {
                if (value != null && value.Length > 10)
                    throw new ArgumentOutOfRangeException("Recid", "Recid value, cannot contain more than 10 characters");
                _bIsChanged |= (_recid != value);
                _recid = value;
            }

        }

        public string ScheduleName
        {
            get { return _schedulename; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("ScheduleName", "ScheduleName value, cannot contain more than 200 characters");
                _bIsChanged |= (_schedulename != value);
                _schedulename = value;
            }

        }

        public string CompanyId
        {
            get { return _companyid; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("CompanyId", "CompanyId value, cannot contain more than 50 characters");
                _bIsChanged |= (_companyid != value);
                _companyid = value;
            }

        }

        public string StartTime
        {
            get { return _starttime; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("StartTime", "StartTime value, cannot contain more than 50 characters");
                _bIsChanged |= (_starttime != value);
                _starttime = value;
            }

        }

        public string EndTime
        {
            get { return _endtime; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("EndTime", "EndTime value, cannot contain more than 50 characters");
                _bIsChanged |= (_endtime != value);
                _endtime = value;
            }

        }

        public string ScheduleSum
        {
            get { return _schedulesum; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("ScheduleSum", "ScheduleSum value, cannot contain more than 20 characters");
                _bIsChanged |= (_schedulesum != value);
                _schedulesum = value;
            }

        }

        public string ProjectId
        {
            get { return _projectid; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("ProjectId", "ProjectId value, cannot contain more than 50 characters");
                _bIsChanged |= (_projectid != value);
                _projectid = value;
            }

        }

        public string HasDelete
        {
            get { return _hasdelete; }
            set
            {
                if (value != null && value.Length > 1)
                    throw new ArgumentOutOfRangeException("HasDelete", "HasDelete value, cannot contain more than 1 characters");
                _bIsChanged |= (_hasdelete != value);
                _hasdelete = value;
            }

        }

        public string Zt
        {
            get { return _zt; }
            set
            {
                if (value != null && value.Length > 1)
                    throw new ArgumentOutOfRangeException("Zt", "Zt value, cannot contain more than 1 characters");
                _bIsChanged |= (_zt != value);
                _zt = value;
            }

        }

        public string Sfjs
        {
            get { return _sfjs; }
            set
            {
                if (value != null && value.Length > 1)
                    throw new ArgumentOutOfRangeException("Sfjs", "Sfjs value, cannot contain more than 1 characters");
                _bIsChanged |= (_sfjs != value);
                _sfjs = value;
            }

        }

        public int? FreeTime
        {
            get { return _freetime; }
            set { _bIsChanged |= (_freetime != value); _freetime = value; }

        }
        public int? KqTimes
        {
            get { return _kqtimes; }
            set { _bIsChanged |= (_kqtimes != value); _kqtimes = value; }
        }

        public string LjTime
        {
            get { return _ljtime; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("LjTime", "LjTime value, cannot contain more than 20 characters");
                _bIsChanged |= (_ljtime != value);
                _ljtime = value;
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
            InfoSchedule castObj = null;
            try
            {
                castObj = (InfoSchedule)obj;
            }
            catch (Exception) { return false; }
            return (castObj != null);
        }
        /// <summary>
        /// local implementation of GetHashCode based on unique value members
        /// </summary>
        public override int GetHashCode()
        {


            int hash = 57;
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

    #region Custom ICollection interface for InfoSchedule


    public interface IInfoScheduleCollection : ICollection
    {
        InfoSchedule this[int index] { get; set; }
        void Add(InfoSchedule pInfoSchedule);
        void Clear();
    }

    [Serializable]
    public class InfoScheduleCollection : IInfoScheduleCollection
    {
        private IList<InfoSchedule> _arrayInternal;

        public InfoScheduleCollection()
        {
            _arrayInternal = new List<InfoSchedule>();
        }

        public InfoScheduleCollection(IList<InfoSchedule> pSource)
        {
            _arrayInternal = pSource;
            if (_arrayInternal == null)
            {
                _arrayInternal = new List<InfoSchedule>();
            }
        }

        public InfoSchedule this[int index]
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
        public void CopyTo(Array array, int index) { _arrayInternal.CopyTo((InfoSchedule[])array, index); }
        public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
        public void Add(InfoSchedule pInfoSchedule) { _arrayInternal.Add(pInfoSchedule); }
        public void Clear() { _arrayInternal.Clear(); }
        public IList<InfoSchedule> GetList() { return _arrayInternal; }
    }

    #endregion
}
