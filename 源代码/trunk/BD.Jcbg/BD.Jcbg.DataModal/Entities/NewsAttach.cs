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
	/// INewsAttach interface for NHibernate mapped table 'News_Attach'.
	/// </summary>
	public interface INewsAttach
	{
		#region Public Properties
		
		int Attachid
		{
			get ;
			set ;
			  
		}
		
		int? Articleid
		{
			get ;
			set ;
			  
		}
		
		string DocName
		{
			get ;
			set ;
			  
		}
		
		string SaveName
		{
			get ;
			set ;
			  
		}
		
		byte[] Filecontent
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// NewsAttach object for NHibernate mapped table 'News_Attach'.
	/// </summary>
	[Serializable]
	public class NewsAttach : ICloneable,INewsAttach
	{
		#region Member Variables

		protected int _attachid;
		protected int? _articleid;
		protected string _docname;
		protected string _savename;
		protected byte[] _filecontent;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public NewsAttach() {}
		
		public NewsAttach(int? pArticleid, string pDocName, string pSaveName, byte[] pFilecontent)
		{
			this._articleid = pArticleid; 
			this._docname = pDocName; 
			this._savename = pSaveName; 
			this._filecontent = pFilecontent; 
		}
		
		public NewsAttach(int pAttachid)
		{
			this._attachid = pAttachid; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int Attachid
		{
			get { return _attachid; }
			set { _bIsChanged |= (_attachid != value); _attachid = value; }
			
		}
		
		public int? Articleid
		{
			get { return _articleid; }
			set { _bIsChanged |= (_articleid != value); _articleid = value; }
			
		}
		
		public string DocName
		{
			get { return _docname; }
			set 
			{
			  if (value != null && value.Length > 256)
			    throw new ArgumentOutOfRangeException("DocName", "DocName value, cannot contain more than 256 characters");
			  _bIsChanged |= (_docname != value); 
			  _docname = value; 
			}
			
		}
		
		public string SaveName
		{
			get { return _savename; }
			set 
			{
			  if (value != null && value.Length > 256)
			    throw new ArgumentOutOfRangeException("SaveName", "SaveName value, cannot contain more than 256 characters");
			  _bIsChanged |= (_savename != value); 
			  _savename = value; 
			}
			
		}
		
		public byte[] Filecontent
		{
			get { return _filecontent; }
			set { _bIsChanged |= (_filecontent != value); _filecontent = value; }
			
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
			NewsAttach castObj = null;
			try
			{
				castObj = (NewsAttach)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._attachid == castObj.Attachid );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _attachid.GetHashCode();
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
	
	#region Custom ICollection interface for NewsAttach 

	
	public interface INewsAttachCollection : ICollection
	{
		NewsAttach this[int index]{	get; set; }
		void Add(NewsAttach pNewsAttach);
		void Clear();
	}
	
	[Serializable]
	public class NewsAttachCollection : INewsAttachCollection
	{
		private IList<NewsAttach> _arrayInternal;

		public NewsAttachCollection()
		{
			_arrayInternal = new List<NewsAttach>();
		}
		
		public NewsAttachCollection( IList<NewsAttach> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<NewsAttach>();
			}
		}

		public NewsAttach this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((NewsAttach[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(NewsAttach pNewsAttach) { _arrayInternal.Add(pNewsAttach); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<NewsAttach> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
