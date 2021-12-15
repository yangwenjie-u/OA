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
	/// ICompanyAnnounce interface for NHibernate mapped table 'CompanyAnnounce'.
	/// </summary>
	public interface ICompanyAnnounce
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string Title
		{
			get ;
			set ;
			  
		}
		
		string Body
		{
			get ;
			set ;
			  
		}
		
		string UserName
		{
			get ;
			set ;
			  
		}
		
		string RealName
		{
			get ;
			set ;
			  
		}
		
		DateTime? CreatedTime
		{
			get ;
			set ;
			  
		}
		
		string FileIds
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// CompanyAnnounce object for NHibernate mapped table 'CompanyAnnounce'.
	/// </summary>
	[Serializable]
	public class CompanyAnnounce : ICloneable,ICompanyAnnounce
	{
		#region Member Variables

		protected int _recid;
		protected string _title;
		protected string _body;
		protected string _username;
		protected string _realname;
		protected DateTime? _createdtime;
		protected string _fileids;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public CompanyAnnounce() {}
		
		public CompanyAnnounce(string pTitle, string pBody, string pUserName, string pRealName, DateTime? pCreatedTime, string pFileIds)
		{
			this._title = pTitle; 
			this._body = pBody; 
			this._username = pUserName; 
			this._realname = pRealName; 
			this._createdtime = pCreatedTime; 
			this._fileids = pFileIds; 
		}
		
		public CompanyAnnounce(int pRecid)
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
		
		public string Title
		{
			get { return _title; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("Title", "Title value, cannot contain more than 100 characters");
			  _bIsChanged |= (_title != value); 
			  _title = value; 
			}
			
		}
		
		public string Body
		{
			get { return _body; }
			set 
			{
			  if (value != null && value.Length > 1073741823)
			    throw new ArgumentOutOfRangeException("Body", "Body value, cannot contain more than 1073741823 characters");
			  _bIsChanged |= (_body != value); 
			  _body = value; 
			}
			
		}
		
		public string UserName
		{
			get { return _username; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("UserName", "UserName value, cannot contain more than 50 characters");
			  _bIsChanged |= (_username != value); 
			  _username = value; 
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
		
		public DateTime? CreatedTime
		{
			get { return _createdtime; }
			set { _bIsChanged |= (_createdtime != value); _createdtime = value; }
			
		}
		
		public string FileIds
		{
			get { return _fileids; }
			set 
			{
			  if (value != null && value.Length > 2000)
			    throw new ArgumentOutOfRangeException("FileIds", "FileIds value, cannot contain more than 2000 characters");
			  _bIsChanged |= (_fileids != value); 
			  _fileids = value; 
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
			CompanyAnnounce castObj = null;
			try
			{
				castObj = (CompanyAnnounce)obj;
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
	
	#region Custom ICollection interface for CompanyAnnounce 

	
	public interface ICompanyAnnounceCollection : ICollection
	{
		CompanyAnnounce this[int index]{	get; set; }
		void Add(CompanyAnnounce pCompanyAnnounce);
		void Clear();
	}
	
	[Serializable]
	public class CompanyAnnounceCollection : ICompanyAnnounceCollection
	{
		private IList<CompanyAnnounce> _arrayInternal;

		public CompanyAnnounceCollection()
		{
			_arrayInternal = new List<CompanyAnnounce>();
		}
		
		public CompanyAnnounceCollection( IList<CompanyAnnounce> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<CompanyAnnounce>();
			}
		}

		public CompanyAnnounce this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((CompanyAnnounce[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(CompanyAnnounce pCompanyAnnounce) { _arrayInternal.Add(pCompanyAnnounce); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<CompanyAnnounce> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
