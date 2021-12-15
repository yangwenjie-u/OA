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
	/// IUserShareFile interface for NHibernate mapped table 'UserShareFile'.
	/// </summary>
	public interface IUserShareFile
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		int? FolderId
		{
			get ;
			set ;
			  
		}
		
		int? FileId
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// UserShareFile object for NHibernate mapped table 'UserShareFile'.
	/// </summary>
	[Serializable]
	public class UserShareFile : ICloneable,IUserShareFile
	{
		#region Member Variables

		protected int _recid;
		protected int? _folderid;
		protected int? _fileid;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public UserShareFile() {}
		
		public UserShareFile(int? pFolderId, int? pFileId)
		{
			this._folderid = pFolderId; 
			this._fileid = pFileId; 
		}
		
		public UserShareFile(int pRecid)
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
		
		public int? FolderId
		{
			get { return _folderid; }
			set { _bIsChanged |= (_folderid != value); _folderid = value; }
			
		}
		
		public int? FileId
		{
			get { return _fileid; }
			set { _bIsChanged |= (_fileid != value); _fileid = value; }
			
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
			UserShareFile castObj = null;
			try
			{
				castObj = (UserShareFile)obj;
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
	
	#region Custom ICollection interface for UserShareFile 

	
	public interface IUserShareFileCollection : ICollection
	{
		UserShareFile this[int index]{	get; set; }
		void Add(UserShareFile pUserShareFile);
		void Clear();
	}
	
	[Serializable]
	public class UserShareFileCollection : IUserShareFileCollection
	{
		private IList<UserShareFile> _arrayInternal;

		public UserShareFileCollection()
		{
			_arrayInternal = new List<UserShareFile>();
		}
		
		public UserShareFileCollection( IList<UserShareFile> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<UserShareFile>();
			}
		}

		public UserShareFile this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((UserShareFile[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(UserShareFile pUserShareFile) { _arrayInternal.Add(pUserShareFile); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<UserShareFile> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
