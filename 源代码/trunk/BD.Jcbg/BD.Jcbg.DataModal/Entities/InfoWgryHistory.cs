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
	/// IInfoWgryHistory interface for NHibernate mapped table 'InfoWgryHistory'.
	/// </summary>
	public interface IInfoWgryHistory
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string Sfzhm
		{
			get ;
			set ;
			  
		}
		
		string RealName
		{
			get ;
			set ;
			  
		}
		
		string CompanyId
		{
			get ;
			set ;
			  
		}
		
		string CompanyName
		{
			get ;
			set ;
			  
		}
		
		string ProjectId
		{
			get ;
			set ;
			  
		}
		
		string ProjectName
		{
			get ;
			set ;
			  
		}
		
		string Bzfzr
		{
			get ;
			set ;
			  
		}
		
		string BzfzrRealName
		{
			get ;
			set ;
			  
		}
		
		DateTime? InTime
		{
			get ;
			set ;
			  
		}
		
		DateTime? OutTime
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// InfoWgryHistory object for NHibernate mapped table 'InfoWgryHistory'.
	/// </summary>
	[Serializable]
	public class InfoWgryHistory : ICloneable,IInfoWgryHistory
	{
		#region Member Variables

		protected int _recid;
		protected string _sfzhm;
		protected string _realname;
		protected string _companyid;
		protected string _companyname;
		protected string _projectid;
		protected string _projectname;
		protected string _bzfzr;
		protected string _bzfzrrealname;
		protected DateTime? _intime;
		protected DateTime? _outtime;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public InfoWgryHistory() {}
		
		public InfoWgryHistory(string pSfzhm, string pRealName, string pCompanyId, string pCompanyName, string pProjectId, string pProjectName, string pBzfzr, string pBzfzrRealName, DateTime? pInTime, DateTime? pOutTime)
		{
			this._sfzhm = pSfzhm; 
			this._realname = pRealName; 
			this._companyid = pCompanyId; 
			this._companyname = pCompanyName; 
			this._projectid = pProjectId; 
			this._projectname = pProjectName; 
			this._bzfzr = pBzfzr; 
			this._bzfzrrealname = pBzfzrRealName; 
			this._intime = pInTime; 
			this._outtime = pOutTime; 
		}
		
		public InfoWgryHistory(int pRecid)
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

        public string Sfzhm
		{
			get { return _sfzhm; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Sfzhm", "Sfzhm value, cannot contain more than 50 characters");
			  _bIsChanged |= (_sfzhm != value); 
			  _sfzhm = value; 
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
		
		public string CompanyName
		{
			get { return _companyname; }
			set 
			{
			  if (value != null && value.Length > 400)
			    throw new ArgumentOutOfRangeException("CompanyName", "CompanyName value, cannot contain more than 400 characters");
			  _bIsChanged |= (_companyname != value); 
			  _companyname = value; 
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
		
		public DateTime? InTime
		{
			get { return _intime; }
			set { _bIsChanged |= (_intime != value); _intime = value; }
			
		}
		
		public DateTime? OutTime
		{
			get { return _outtime; }
			set { _bIsChanged |= (_outtime != value); _outtime = value; }
			
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
			InfoWgryHistory castObj = null;
			try
			{
				castObj = (InfoWgryHistory)obj;
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
	
	#region Custom ICollection interface for InfoWgryHistory 

	
	public interface IInfoWgryHistoryCollection : ICollection
	{
		InfoWgryHistory this[int index]{	get; set; }
		void Add(InfoWgryHistory pInfoWgryHistory);
		void Clear();
	}
	
	[Serializable]
	public class InfoWgryHistoryCollection : IInfoWgryHistoryCollection
	{
		private IList<InfoWgryHistory> _arrayInternal;

		public InfoWgryHistoryCollection()
		{
			_arrayInternal = new List<InfoWgryHistory>();
		}
		
		public InfoWgryHistoryCollection( IList<InfoWgryHistory> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<InfoWgryHistory>();
			}
		}

		public InfoWgryHistory this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((InfoWgryHistory[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(InfoWgryHistory pInfoWgryHistory) { _arrayInternal.Add(pInfoWgryHistory); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<InfoWgryHistory> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
