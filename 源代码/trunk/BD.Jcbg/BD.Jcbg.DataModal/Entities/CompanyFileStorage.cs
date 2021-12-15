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
	/// ICompanyFileStorage interface for NHibernate mapped table 'CompanyFileStorage'.
	/// </summary>
	public interface ICompanyFileStorage
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string FileName
		{
			get ;
			set ;
			  
		}
		
		string FileDesc
		{
			get ;
			set ;
			  
		}
		
		long? FileSize
		{
			get ;
			set ;
			  
		}
		
		byte[] FileContent
		{
			get ;
			set ;
			  
		}
		
		DateTime? CreatedTime
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
		
		string ImageName
		{
			get ;
			set ;
			  
		}
		
		string FileTypeDesc
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// CompanyFileStorage object for NHibernate mapped table 'CompanyFileStorage'.
	/// </summary>
	[Serializable]
	public class CompanyFileStorage : ICloneable,ICompanyFileStorage
	{
		#region Member Variables

		protected int _recid;
		protected string _filename;
		protected string _filedesc;
		protected long? _filesize;
		protected byte[] _filecontent;
		protected DateTime? _createdtime;
		protected string _username;
		protected string _realname;
		protected string _imagename;
		protected string _filetypedesc;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public CompanyFileStorage() {}
		
		public CompanyFileStorage(string pFileName, string pFileDesc, long? pFileSize, byte[] pFileContent, DateTime? pCreatedTime, string pUserName, string pRealName, string pImageName, string pFileTypeDesc)
		{
			this._filename = pFileName; 
			this._filedesc = pFileDesc; 
			this._filesize = pFileSize; 
			this._filecontent = pFileContent; 
			this._createdtime = pCreatedTime; 
			this._username = pUserName; 
			this._realname = pRealName; 
			this._imagename = pImageName; 
			this._filetypedesc = pFileTypeDesc; 
		}
		
		public CompanyFileStorage(int pRecid)
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
		
		public string FileName
		{
			get { return _filename; }
			set 
			{
			  if (value != null && value.Length > 255)
			    throw new ArgumentOutOfRangeException("FileName", "FileName value, cannot contain more than 255 characters");
			  _bIsChanged |= (_filename != value); 
			  _filename = value; 
			}
			
		}
		
		public string FileDesc
		{
			get { return _filedesc; }
			set 
			{
			  if (value != null && value.Length > 200)
			    throw new ArgumentOutOfRangeException("FileDesc", "FileDesc value, cannot contain more than 200 characters");
			  _bIsChanged |= (_filedesc != value); 
			  _filedesc = value; 
			}
			
		}
		
		public long? FileSize
		{
			get { return _filesize; }
			set { _bIsChanged |= (_filesize != value); _filesize = value; }
			
		}
		
		public byte[] FileContent
		{
			get { return _filecontent; }
			set { _bIsChanged |= (_filecontent != value); _filecontent = value; }
			
		}
		
		public DateTime? CreatedTime
		{
			get { return _createdtime; }
			set { _bIsChanged |= (_createdtime != value); _createdtime = value; }
			
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
		
		public string ImageName
		{
			get { return _imagename; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ImageName", "ImageName value, cannot contain more than 100 characters");
			  _bIsChanged |= (_imagename != value); 
			  _imagename = value; 
			}
			
		}
		
		public string FileTypeDesc
		{
			get { return _filetypedesc; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("FileTypeDesc", "FileTypeDesc value, cannot contain more than 100 characters");
			  _bIsChanged |= (_filetypedesc != value); 
			  _filetypedesc = value; 
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
			CompanyFileStorage castObj = null;
			try
			{
				castObj = (CompanyFileStorage)obj;
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
	
	#region Custom ICollection interface for CompanyFileStorage 

	
	public interface ICompanyFileStorageCollection : ICollection
	{
		CompanyFileStorage this[int index]{	get; set; }
		void Add(CompanyFileStorage pCompanyFileStorage);
		void Clear();
	}
	
	[Serializable]
	public class CompanyFileStorageCollection : ICompanyFileStorageCollection
	{
		private IList<CompanyFileStorage> _arrayInternal;

		public CompanyFileStorageCollection()
		{
			_arrayInternal = new List<CompanyFileStorage>();
		}
		
		public CompanyFileStorageCollection( IList<CompanyFileStorage> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<CompanyFileStorage>();
			}
		}

		public CompanyFileStorage this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((CompanyFileStorage[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(CompanyFileStorage pCompanyFileStorage) { _arrayInternal.Add(pCompanyFileStorage); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<CompanyFileStorage> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
