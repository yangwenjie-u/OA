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
	/// IUserShareFileFolder interface for NHibernate mapped table 'UserShareFileFolder'.
	/// </summary>
	public interface IUserShareFileFolder
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		int? ParentId
		{
			get ;
			set ;
			  
		}
		
		string FolderName
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
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// UserShareFileFolder object for NHibernate mapped table 'UserShareFileFolder'.
	/// </summary>
	[Serializable]
	public class UserShareFileFolder : ICloneable,IUserShareFileFolder
	{
		#region Member Variables

		protected int _recid;
		protected int? _parentid;
		protected string _foldername;
		protected string _username;
		protected string _realname;
		protected DateTime? _createdtime;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public UserShareFileFolder() {}
		
		public UserShareFileFolder(int? pParentId, string pFolderName, string pUserName, string pRealName, DateTime? pCreatedTime)
		{
			this._parentid = pParentId; 
			this._foldername = pFolderName; 
			this._username = pUserName; 
			this._realname = pRealName; 
			this._createdtime = pCreatedTime; 
		}
		
		public UserShareFileFolder(int pRecid)
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
		
		public int? ParentId
		{
			get { return _parentid; }
			set { _bIsChanged |= (_parentid != value); _parentid = value; }
			
		}
		
		public string FolderName
		{
			get { return _foldername; }
			set 
			{
			  if (value != null && value.Length > 200)
			    throw new ArgumentOutOfRangeException("FolderName", "FolderName value, cannot contain more than 200 characters");
			  _bIsChanged |= (_foldername != value); 
			  _foldername = value; 
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
			UserShareFileFolder castObj = null;
			try
			{
				castObj = (UserShareFileFolder)obj;
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
	
	#region Custom ICollection interface for UserShareFileFolder 

	
	public interface IUserShareFileFolderCollection : ICollection
	{
		UserShareFileFolder this[int index]{	get; set; }
		void Add(UserShareFileFolder pUserShareFileFolder);
		void Clear();
	}
	
	[Serializable]
	public class UserShareFileFolderCollection : IUserShareFileFolderCollection
	{
		private IList<UserShareFileFolder> _arrayInternal;

		public UserShareFileFolderCollection()
		{
			_arrayInternal = new List<UserShareFileFolder>();
		}
		
		public UserShareFileFolderCollection( IList<UserShareFileFolder> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<UserShareFileFolder>();
			}
		}

		public UserShareFileFolder this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((UserShareFileFolder[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(UserShareFileFolder pUserShareFileFolder) { _arrayInternal.Add(pUserShareFileFolder); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<UserShareFileFolder> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
