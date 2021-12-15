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
	/// INewsMenu interface for NHibernate mapped table 'News_Menu'.
	/// </summary>
	public interface INewsMenu
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string MenuName
		{
			get ;
			set ;
			  
		}
		
		int? MenuType
		{
			get ;
			set ;
			  
		}
		
		string LinkUrl
		{
			get ;
			set ;
			  
		}
		
		int? CategoryId
		{
			get ;
			set ;
			  
		}
		
		bool? InUse
		{
			get ;
			set ;
			  
		}
		
		int? DispOrder
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// NewsMenu object for NHibernate mapped table 'News_Menu'.
	/// </summary>
	[Serializable]
	public class NewsMenu : ICloneable,INewsMenu
	{
		#region Member Variables

		protected int _recid;
		protected string _menuname;
		protected int? _menutype;
		protected string _linkurl;
		protected int? _categoryid;
		protected bool? _inuse;
		protected int? _disporder;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public NewsMenu() {}
		
		public NewsMenu(string pMenuName, int? pMenuType, string pLinkUrl, int? pCategoryId, bool? pInUse, int? pDispOrder)
		{
			this._menuname = pMenuName; 
			this._menutype = pMenuType; 
			this._linkurl = pLinkUrl; 
			this._categoryid = pCategoryId; 
			this._inuse = pInUse; 
			this._disporder = pDispOrder; 
		}
		
		public NewsMenu(int pRecid)
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
		
		public string MenuName
		{
			get { return _menuname; }
			set 
			{
			  if (value != null && value.Length > 200)
			    throw new ArgumentOutOfRangeException("MenuName", "MenuName value, cannot contain more than 200 characters");
			  _bIsChanged |= (_menuname != value); 
			  _menuname = value; 
			}
			
		}
		
		public int? MenuType
		{
			get { return _menutype; }
			set { _bIsChanged |= (_menutype != value); _menutype = value; }
			
		}
		
		public string LinkUrl
		{
			get { return _linkurl; }
			set 
			{
			  if (value != null && value.Length > 200)
			    throw new ArgumentOutOfRangeException("LinkUrl", "LinkUrl value, cannot contain more than 200 characters");
			  _bIsChanged |= (_linkurl != value); 
			  _linkurl = value; 
			}
			
		}
		
		public int? CategoryId
		{
			get { return _categoryid; }
			set { _bIsChanged |= (_categoryid != value); _categoryid = value; }
			
		}
		
		public bool? InUse
		{
			get { return _inuse; }
			set { _bIsChanged |= (_inuse != value); _inuse = value; }
			
		}
		
		public int? DispOrder
		{
			get { return _disporder; }
			set { _bIsChanged |= (_disporder != value); _disporder = value; }
			
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
			NewsMenu castObj = null;
			try
			{
				castObj = (NewsMenu)obj;
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
	
	#region Custom ICollection interface for NewsMenu 

	
	public interface INewsMenuCollection : ICollection
	{
		NewsMenu this[int index]{	get; set; }
		void Add(NewsMenu pNewsMenu);
		void Clear();
	}
	
	[Serializable]
	public class NewsMenuCollection : INewsMenuCollection
	{
		private IList<NewsMenu> _arrayInternal;

		public NewsMenuCollection()
		{
			_arrayInternal = new List<NewsMenu>();
		}
		
		public NewsMenuCollection( IList<NewsMenu> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<NewsMenu>();
			}
		}

		public NewsMenu this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((NewsMenu[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(NewsMenu pNewsMenu) { _arrayInternal.Add(pNewsMenu); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<NewsMenu> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
