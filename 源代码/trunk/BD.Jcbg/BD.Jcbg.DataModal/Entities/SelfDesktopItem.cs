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
	/// ISelfDesktopItem interface for NHibernate mapped table 'SelfDesktopItem'.
	/// </summary>
	public interface ISelfDesktopItem
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string ItemKey
		{
			get ;
			set ;
			  
		}
		
		string UserName
		{
			get ;
			set ;
			  
		}
		
		string ItemColumn
		{
			get ;
			set ;
			  
		}
		
		int? DisplayOrder
		{
			get ;
			set ;
			  
		}
		
		bool? IsDisplay
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// SelfDesktopItem object for NHibernate mapped table 'SelfDesktopItem'.
	/// </summary>
	[Serializable]
	public class SelfDesktopItem : ICloneable,ISelfDesktopItem
	{
		#region Member Variables

		protected int _recid;
		protected string _itemkey;
		protected string _username;
		protected string _itemcolumn;
		protected int? _displayorder;
		protected bool? _isdisplay;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public SelfDesktopItem() {}
		
		public SelfDesktopItem(string pItemKey, string pUserName, string pItemColumn, int? pDisplayOrder, bool? pIsDisplay)
		{
			this._itemkey = pItemKey; 
			this._username = pUserName; 
			this._itemcolumn = pItemColumn; 
			this._displayorder = pDisplayOrder; 
			this._isdisplay = pIsDisplay; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int Recid
		{
			get { return _recid; }
			set { _bIsChanged |= (_recid != value); _recid = value; }
			
		}
		
		public string ItemKey
		{
			get { return _itemkey; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ItemKey", "ItemKey value, cannot contain more than 50 characters");
			  _bIsChanged |= (_itemkey != value); 
			  _itemkey = value; 
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
		
		public string ItemColumn
		{
			get { return _itemcolumn; }
			set 
			{
			  if (value != null && value.Length > 1)
			    throw new ArgumentOutOfRangeException("ItemColumn", "ItemColumn value, cannot contain more than 1 characters");
			  _bIsChanged |= (_itemcolumn != value); 
			  _itemcolumn = value; 
			}
			
		}
		
		public int? DisplayOrder
		{
			get { return _displayorder; }
			set { _bIsChanged |= (_displayorder != value); _displayorder = value; }
			
		}
		
		public bool? IsDisplay
		{
			get { return _isdisplay; }
			set { _bIsChanged |= (_isdisplay != value); _isdisplay = value; }
			
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
			SelfDesktopItem castObj = null;
			try
			{
				castObj = (SelfDesktopItem)obj;
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
	
	#region Custom ICollection interface for SelfDesktopItem 

	
	public interface ISelfDesktopItemCollection : ICollection
	{
		SelfDesktopItem this[int index]{	get; set; }
		void Add(SelfDesktopItem pSelfDesktopItem);
		void Clear();
	}
	
	[Serializable]
	public class SelfDesktopItemCollection : ISelfDesktopItemCollection
	{
		private IList<SelfDesktopItem> _arrayInternal;

		public SelfDesktopItemCollection()
		{
			_arrayInternal = new List<SelfDesktopItem>();
		}
		
		public SelfDesktopItemCollection( IList<SelfDesktopItem> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<SelfDesktopItem>();
			}
		}

		public SelfDesktopItem this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((SelfDesktopItem[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(SelfDesktopItem pSelfDesktopItem) { _arrayInternal.Add(pSelfDesktopItem); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<SelfDesktopItem> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
