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
	/// IUserMailFolder interface for NHibernate mapped table 'UserMailFolder'.
	/// </summary>
	public interface IUserMailFolder
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string UserName
		{
			get ;
			set ;
			  
		}
		
		string FolderName
		{
			get ;
			set ;
			  
		}
		
		string SystemType
		{
			get ;
			set ;
			  
		}
		
		int? DisplayOrder
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// UserMailFolder object for NHibernate mapped table 'UserMailFolder'.
	/// </summary>
	[Serializable]
	public class UserMailFolder : ICloneable,IUserMailFolder
	{
		#region Member Variables

		protected int _recid;
		protected string _username;
		protected string _foldername;
		protected string _systemtype;
		protected int? _displayorder;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public UserMailFolder() {}
		
		public UserMailFolder(string pUserName, string pFolderName, string pSystemType, int? pDisplayOrder)
		{
			this._username = pUserName; 
			this._foldername = pFolderName; 
			this._systemtype = pSystemType; 
			this._displayorder = pDisplayOrder; 
		}
		
		public UserMailFolder(int pRecid)
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
		
		public string FolderName
		{
			get { return _foldername; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("FolderName", "FolderName value, cannot contain more than 100 characters");
			  _bIsChanged |= (_foldername != value); 
			  _foldername = value; 
			}
			
		}
		
		public string SystemType
		{
			get { return _systemtype; }
			set 
			{
			  if (value != null && value.Length > 1)
			    throw new ArgumentOutOfRangeException("SystemType", "SystemType value, cannot contain more than 1 characters");
			  _bIsChanged |= (_systemtype != value); 
			  _systemtype = value; 
			}
			
		}
		
		public int? DisplayOrder
		{
			get { return _displayorder; }
			set { _bIsChanged |= (_displayorder != value); _displayorder = value; }
			
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
			UserMailFolder castObj = null;
			try
			{
				castObj = (UserMailFolder)obj;
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
	
	#region Custom ICollection interface for UserMailFolder 

	
	public interface IUserMailFolderCollection : ICollection
	{
		UserMailFolder this[int index]{	get; set; }
		void Add(UserMailFolder pUserMailFolder);
		void Clear();
	}
	
	[Serializable]
	public class UserMailFolderCollection : IUserMailFolderCollection
	{
		private IList<UserMailFolder> _arrayInternal;

		public UserMailFolderCollection()
		{
			_arrayInternal = new List<UserMailFolder>();
		}
		
		public UserMailFolderCollection( IList<UserMailFolder> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<UserMailFolder>();
			}
		}

		public UserMailFolder this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((UserMailFolder[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(UserMailFolder pUserMailFolder) { _arrayInternal.Add(pUserMailFolder); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<UserMailFolder> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
