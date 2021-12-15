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
	/// ISysFileImage interface for NHibernate mapped table 'SysFileImage'.
	/// </summary>
	public interface ISysFileImage
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string FileExt
		{
			get ;
			set ;
			  
		}
		
		string FileDesc
		{
			get ;
			set ;
			  
		}
		
		string ImageName
		{
			get ;
			set ;
			  
		}
		
		string ImageType
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// SysFileImage object for NHibernate mapped table 'SysFileImage'.
	/// </summary>
	[Serializable]
	public class SysFileImage : ICloneable,ISysFileImage
	{
		#region Member Variables

		protected int _recid;
		protected string _fileext;
		protected string _filedesc;
		protected string _imagename;
		protected string _imagetype;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public SysFileImage() {}
		
		public SysFileImage(string pFileExt, string pFileDesc, string pImageName, string pImageType)
		{
			this._fileext = pFileExt; 
			this._filedesc = pFileDesc; 
			this._imagename = pImageName; 
			this._imagetype = pImageType; 
		}
		
		public SysFileImage(int pRecid)
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
		
		public string FileExt
		{
			get { return _fileext; }
			set 
			{
			  if (value != null && value.Length > 1000)
			    throw new ArgumentOutOfRangeException("FileExt", "FileExt value, cannot contain more than 1000 characters");
			  _bIsChanged |= (_fileext != value); 
			  _fileext = value; 
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
		
		public string ImageName
		{
			get { return _imagename; }
			set 
			{
			  if (value != null && value.Length > 200)
			    throw new ArgumentOutOfRangeException("ImageName", "ImageName value, cannot contain more than 200 characters");
			  _bIsChanged |= (_imagename != value); 
			  _imagename = value; 
			}
			
		}
		
		public string ImageType
		{
			get { return _imagetype; }
			set 
			{
			  if (value != null && value.Length > 1)
			    throw new ArgumentOutOfRangeException("ImageType", "ImageType value, cannot contain more than 1 characters");
			  _bIsChanged |= (_imagetype != value); 
			  _imagetype = value; 
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
			SysFileImage castObj = null;
			try
			{
				castObj = (SysFileImage)obj;
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
	
	#region Custom ICollection interface for SysFileImage 

	
	public interface ISysFileImageCollection : ICollection
	{
		SysFileImage this[int index]{	get; set; }
		void Add(SysFileImage pSysFileImage);
		void Clear();
	}
	
	[Serializable]
	public class SysFileImageCollection : ISysFileImageCollection
	{
		private IList<SysFileImage> _arrayInternal;

		public SysFileImageCollection()
		{
			_arrayInternal = new List<SysFileImage>();
		}
		
		public SysFileImageCollection( IList<SysFileImage> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<SysFileImage>();
			}
		}

		public SysFileImage this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((SysFileImage[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(SysFileImage pSysFileImage) { _arrayInternal.Add(pSysFileImage); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<SysFileImage> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
