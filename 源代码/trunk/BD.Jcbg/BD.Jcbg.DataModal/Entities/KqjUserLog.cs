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
	/// IKqjUserLog interface for NHibernate mapped table 'KqjUserLog'.
	/// </summary>
	public interface IKqjUserLog
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string UserId
		{
			get ;
			set ;
			  
		}
		
		string LogType
		{
			get ;
			set ;
			  
		}
		
		string Serial
		{
			get ;
			set ;
			  
		}
		
		DateTime? LogDate
		{
			get ;
			set ;
			  
		}
		
		string CompanyId
		{
			get ;
			set ;
			  
		}
		
		string PlaceId
		{
			get ;
			set ;
			  
		}

        string CompanyName
        {
            get;
            set;

        }

        string ProjectName
        {
            get;
            set;

        }
				
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// KqjUserLog object for NHibernate mapped table 'KqjUserLog'.
	/// </summary>
	[Serializable]
	public class KqjUserLog : ICloneable,IKqjUserLog
	{
		#region Member Variables

		protected int _recid;
		protected string _userid;
		protected string _logtype;
		protected string _serial;
		protected DateTime? _logdate;
		protected string _companyid;
		protected string _placeid;
        protected string _companyname;
        protected string _projectname;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion 
		
		#region Constructors
		public KqjUserLog() {}

        public KqjUserLog(string pUserId, string pLogType, string pSerial, DateTime? pLogDate, string pCompanyId, string pPlaceId, string pCompanyName, string pProjectName)
		{
			this._userid = pUserId; 
			this._logtype = pLogType; 
			this._serial = pSerial; 
			this._logdate = pLogDate; 
			this._companyid = pCompanyId;
            this._placeid = pPlaceId;
            this._companyname = pCompanyName;
            this._projectname = pProjectName; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int Recid
		{
			get { return _recid; }
			set { _bIsChanged |= (_recid != value); _recid = value; }
			
		}
		
		public string UserId
		{
			get { return _userid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("UserId", "UserId value, cannot contain more than 50 characters");
			  _bIsChanged |= (_userid != value); 
			  _userid = value; 
			}
			
		}
		
		public string LogType
		{
			get { return _logtype; }
			set 
			{
			  if (value != null && value.Length > 2)
			    throw new ArgumentOutOfRangeException("LogType", "LogType value, cannot contain more than 2 characters");
              _bIsChanged |= (_logtype != value);
              _logtype = value; 
			}
			
		}
		
		public string Serial
		{
			get { return _serial; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Serial", "Serial value, cannot contain more than 50 characters");
			  _bIsChanged |= (_serial != value); 
			  _serial = value; 
			}
			
		}
		
		public DateTime? LogDate
		{
			get { return _logdate; }
			set { _bIsChanged |= (_logdate != value); _logdate = value; }
			
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
		
		public string PlaceId
		{
			get { return _placeid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ProjectId", "ProjectId value, cannot contain more than 50 characters");
              _bIsChanged |= (_placeid != value);
              _placeid = value; 
			}
			
		}

        public string CompanyName
        {
            get { return _companyname; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("CompanyName", "CompanyName value, cannot contain more than 200 characters");
                _bIsChanged |= (_companyname != value);
                _companyname = value;
            }

        }

        public string ProjectName
        {
            get { return _projectname; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("ProjectName", "ProjectName value, cannot contain more than 200 characters");
                _bIsChanged |= (_projectname != value);
                _projectname = value;
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
		public override bool Equals( object obj )
		{
			if( this == obj ) return true;
			KqjUserLog castObj = null;
			try
			{
				castObj = (KqjUserLog)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._recid == castObj.Recid );
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
	
	#region Custom ICollection interface for KqjUserLog 

	
	public interface IKqjUserLogCollection : ICollection
	{
		KqjUserLog this[int index]{	get; set; }
		void Add(KqjUserLog pKqjUserLog);
		void Clear();
	}
	
	[Serializable]
	public class KqjUserLogCollection : IKqjUserLogCollection
	{
		private IList<KqjUserLog> _arrayInternal;

		public KqjUserLogCollection()
		{
			_arrayInternal = new List<KqjUserLog>();
		}
		
		public KqjUserLogCollection( IList<KqjUserLog> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<KqjUserLog>();
			}
		}

		public KqjUserLog this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((KqjUserLog[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(KqjUserLog pKqjUserLog) { _arrayInternal.Add(pKqjUserLog); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<KqjUserLog> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
