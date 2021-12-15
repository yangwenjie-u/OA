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
	/// INewsCategory interface for NHibernate mapped table 'News_Category'.
	/// </summary>
	public interface INewsCategory
	{
		#region Public Properties
		
		int Categoryid
		{
			get ;
			set ;
			  
		}
		
		int Fatherid
		{
			get ;
			set ;
			  
		}
		
		bool? IsLeaf
		{
			get ;
			set ;
			  
		}
		
		string Name
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// NewsCategory object for NHibernate mapped table 'News_Category'.
	/// </summary>
	[Serializable]
	public class NewsCategory : ICloneable,INewsCategory
	{
		#region Member Variables

		protected int _categoryid;
		protected int _fatherid;
		protected bool? _isleaf;
		protected string _name;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public NewsCategory() {}
		
		public NewsCategory(int pFatherid, bool? pIsLeaf, string pName)
		{
			this._fatherid = pFatherid; 
			this._isleaf = pIsLeaf; 
			this._name = pName; 
		}
		

		
		public NewsCategory(int pCategoryid)
		{
			this._categoryid = pCategoryid; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int Categoryid
		{
			get { return _categoryid; }
			set { _bIsChanged |= (_categoryid != value); _categoryid = value; }
			
		}
		
		public int Fatherid
		{
			get { return _fatherid; }
			set { _bIsChanged |= (_fatherid != value); _fatherid = value; }
			
		}
		
		public bool? IsLeaf
		{
			get { return _isleaf; }
			set { _bIsChanged |= (_isleaf != value); _isleaf = value; }
			
		}
		
		public string Name
		{
			get { return _name; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Name", "Name value, cannot contain more than 50 characters");
			  _bIsChanged |= (_name != value); 
			  _name = value; 
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
			NewsCategory castObj = null;
			try
			{
				castObj = (NewsCategory)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._categoryid == castObj.Categoryid );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _categoryid.GetHashCode();
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
	
	#region Custom ICollection interface for NewsCategory 

	
	public interface INewsCategoryCollection : ICollection
	{
		NewsCategory this[int index]{	get; set; }
		void Add(NewsCategory pNewsCategory);
		void Clear();
	}
	
	[Serializable]
	public class NewsCategoryCollection : INewsCategoryCollection
	{
		private IList<NewsCategory> _arrayInternal;

		public NewsCategoryCollection()
		{
			_arrayInternal = new List<NewsCategory>();
		}
		
		public NewsCategoryCollection( IList<NewsCategory> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<NewsCategory>();
			}
		}

		public NewsCategory this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((NewsCategory[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(NewsCategory pNewsCategory) { _arrayInternal.Add(pNewsCategory); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<NewsCategory> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
