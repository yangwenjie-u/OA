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
	/// IKqjUserDayLog interface for NHibernate mapped table 'KqjUserDayLog'.
	/// </summary>
	public interface IKqjUserDayLog
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
		
		string ScheduleId
		{
			get ;
			set ;
			  
		}
		
		DateTime? LogDay
		{
			get ;
			set ;
			  
		}
		
		string CompanyId
		{
			get ;
			set ;
			  
		}
		
		string ProjectId
		{
			get ;
			set ;
			  
		}
		
		string GzId
		{
			get ;
			set ;
			  
		}

        string GW
        {
            get;
            set;

        }
		
		decimal? RealSum
		{
			get ;
			set ;
			  
		}
		
		string Bzfzr
		{
			get ;
			set ;
			  
		}
		
		string RealName
		{
			get ;
			set ;
			  
		}
		
		decimal? SetSum
		{
			get ;
			set ;
			  
		}
		
		decimal? RealPay
		{
			get ;
			set ;
			  
		}
		
		string CompanyName
		{
			get ;
			set ;
			  
		}
		
		string ProjectName
		{
			get ;
			set ;
			  
		}
		
		decimal? SetPay
		{
			get ;
			set ;
			  
		}
		
		string BzfzrRealName
		{
			get ;
			set ;
			  
		}
		
		int? ShouldMinutes
		{
			get ;
			set ;
			  
		}
		
		int? RealMinutes
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// KqjUserDayLog object for NHibernate mapped table 'KqjUserDayLog'.
	/// </summary>
	[Serializable]
	public class KqjUserDayLog : ICloneable,IKqjUserDayLog
	{
		#region Member Variables

		protected int _recid;
		protected string _userid;
		protected string _scheduleid;
		protected DateTime? _logday;
		protected string _companyid;
		protected string _projectid;
		protected string _gzid;
        protected string _gw;
		protected decimal? _realsum;
		protected string _bzfzr;
		protected string _realname;
		protected decimal? _setsum;
		protected decimal? _realpay;
		protected string _companyname;
		protected string _projectname;
		protected decimal? _setpay;
		protected string _bzfzrrealname;
		protected int? _shouldminutes;
		protected int? _realminutes;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public KqjUserDayLog() {}
		
		public KqjUserDayLog(string pUserId, string pScheduleId, DateTime? pLogDay, string pCompanyId, string pProjectId, string pGzId, string pGw,decimal? pRealSum, string pBzfzr, string pRealName, decimal? pSetSum, decimal? pRealPay, string pCompanyName, string pProjectName, decimal? pSetPay, string pBzfzrRealName, int? pShouldMinutes, int? pRealMinutes)
		{
			this._userid = pUserId; 
			this._scheduleid = pScheduleId; 
			this._logday = pLogDay; 
			this._companyid = pCompanyId; 
			this._projectid = pProjectId; 
			this._gzid = pGzId; 
            this._gw=pGw;
			this._realsum = pRealSum; 
			this._bzfzr = pBzfzr; 
			this._realname = pRealName; 
			this._setsum = pSetSum; 
			this._realpay = pRealPay; 
			this._companyname = pCompanyName; 
			this._projectname = pProjectName; 
			this._setpay = pSetPay; 
			this._bzfzrrealname = pBzfzrRealName; 
			this._shouldminutes = pShouldMinutes; 
			this._realminutes = pRealMinutes; 
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
		
		public string ScheduleId
		{
			get { return _scheduleid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ScheduleId", "ScheduleId value, cannot contain more than 50 characters");
			  _bIsChanged |= (_scheduleid != value); 
			  _scheduleid = value; 
			}
			
		}
		
		public DateTime? LogDay
		{
			get { return _logday; }
			set { _bIsChanged |= (_logday != value); _logday = value; }
			
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
		
		public string GzId
		{
			get { return _gzid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("GzId", "GzId value, cannot contain more than 50 characters");
			  _bIsChanged |= (_gzid != value); 
			  _gzid = value; 
			}
			
		}
        public string GW
        {
            get { return _gw; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("GW", "GW value, cannot contain more than 50 characters");
                _bIsChanged |= (_gw != value);
                _gw = value;
            }

        }
		
		public decimal? RealSum
		{
			get { return _realsum; }
			set { _bIsChanged |= (_realsum != value); _realsum = value; }
			
		}
		
		public string Bzfzr
		{
			get { return _bzfzr; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Bzfzr", "Bzfzr value, cannot contain more than 50 characters");
			  _bIsChanged |= (_bzfzr != value); 
			  _bzfzr = value; 
			}
			
		}
		
		public string RealName
		{
			get { return _realname; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("RealName", "RealName value, cannot contain more than 50 characters");
			  _bIsChanged |= (_realname != value); 
			  _realname = value; 
			}
			
		}
		
		public decimal? SetSum
		{
			get { return _setsum; }
			set { _bIsChanged |= (_setsum != value); _setsum = value; }
			
		}
		
		public decimal? RealPay
		{
			get { return _realpay; }
			set { _bIsChanged |= (_realpay != value); _realpay = value; }
			
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
		
		public decimal? SetPay
		{
			get { return _setpay; }
			set { _bIsChanged |= (_setpay != value); _setpay = value; }
			
		}
		
		public string BzfzrRealName
		{
			get { return _bzfzrrealname; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("BzfzrRealName", "BzfzrRealName value, cannot contain more than 50 characters");
			  _bIsChanged |= (_bzfzrrealname != value); 
			  _bzfzrrealname = value; 
			}
			
		}
		
		public int? ShouldMinutes
		{
			get { return _shouldminutes; }
			set { _bIsChanged |= (_shouldminutes != value); _shouldminutes = value; }
			
		}
		
		public int? RealMinutes
		{
			get { return _realminutes; }
			set { _bIsChanged |= (_realminutes != value); _realminutes = value; }
			
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
			KqjUserDayLog castObj = null;
			try
			{
				castObj = (KqjUserDayLog)obj;
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
	
	#region Custom ICollection interface for KqjUserDayLog 

	
	public interface IKqjUserDayLogCollection : ICollection
	{
		KqjUserDayLog this[int index]{	get; set; }
		void Add(KqjUserDayLog pKqjUserDayLog);
		void Clear();
	}
	
	[Serializable]
	public class KqjUserDayLogCollection : IKqjUserDayLogCollection
	{
		private IList<KqjUserDayLog> _arrayInternal;

		public KqjUserDayLogCollection()
		{
			_arrayInternal = new List<KqjUserDayLog>();
		}
		
		public KqjUserDayLogCollection( IList<KqjUserDayLog> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<KqjUserDayLog>();
			}
		}

		public KqjUserDayLog this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((KqjUserDayLog[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(KqjUserDayLog pKqjUserDayLog) { _arrayInternal.Add(pKqjUserDayLog); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<KqjUserDayLog> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
