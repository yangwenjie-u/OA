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
	/// ICompanyReader interface for NHibernate mapped table 'CompanyReader'.
	/// </summary>
	public interface ICompanyReader
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string ParentEntity
		{
			get ;
			set ;
			  
		}
		
		string ParentId
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
		
		bool? HasReader
		{
			get ;
			set ;
			  
		}
		
		DateTime? ReaderTime
		{
			get ;
			set ;
			  
		}
		
		bool? HasDelete
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// CompanyReader object for NHibernate mapped table 'CompanyReader'.
	/// </summary>
	[Serializable]
	public class CompanyReader : ICloneable,ICompanyReader
	{
		#region Member Variables

		protected int _recid;
		protected string _parententity;
		protected string _parentid;
		protected string _username;
		protected string _realname;
		protected bool? _hasreader;
		protected DateTime? _readertime;
		protected bool? _hasdelete;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public CompanyReader() {}
		
		public CompanyReader(string pParentEntity, string pParentId, string pUserName, string pRealName, bool? pHasReader, DateTime? pReaderTime, bool? pHasDelete)
		{
			this._parententity = pParentEntity; 
			this._parentid = pParentId; 
			this._username = pUserName; 
			this._realname = pRealName; 
			this._hasreader = pHasReader; 
			this._readertime = pReaderTime; 
			this._hasdelete = pHasDelete; 
		}
		
		public CompanyReader(int pRecid)
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
		
		public string ParentEntity
		{
			get { return _parententity; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ParentEntity", "ParentEntity value, cannot contain more than 100 characters");
			  _bIsChanged |= (_parententity != value); 
			  _parententity = value; 
			}
			
		}
		
		public string ParentId
		{
			get { return _parentid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ParentId", "ParentId value, cannot contain more than 50 characters");
			  _bIsChanged |= (_parentid != value); 
			  _parentid = value; 
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
		
		public bool? HasReader
		{
			get { return _hasreader; }
			set { _bIsChanged |= (_hasreader != value); _hasreader = value; }
			
		}
		
		public DateTime? ReaderTime
		{
			get { return _readertime; }
			set { _bIsChanged |= (_readertime != value); _readertime = value; }
			
		}
		
		public bool? HasDelete
		{
			get { return _hasdelete; }
			set { _bIsChanged |= (_hasdelete != value); _hasdelete = value; }
			
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
			CompanyReader castObj = null;
			try
			{
				castObj = (CompanyReader)obj;
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
	
	#region Custom ICollection interface for CompanyReader 

	
	public interface ICompanyReaderCollection : ICollection
	{
		CompanyReader this[int index]{	get; set; }
		void Add(CompanyReader pCompanyReader);
		void Clear();
	}
	
	[Serializable]
	public class CompanyReaderCollection : ICompanyReaderCollection
	{
		private IList<CompanyReader> _arrayInternal;

		public CompanyReaderCollection()
		{
			_arrayInternal = new List<CompanyReader>();
		}
		
		public CompanyReaderCollection( IList<CompanyReader> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<CompanyReader>();
			}
		}

		public CompanyReader this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((CompanyReader[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(CompanyReader pCompanyReader) { _arrayInternal.Add(pCompanyReader); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<CompanyReader> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
