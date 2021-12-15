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
	/// IHelpDesktopItem interface for NHibernate mapped table 'HelpDesktopItem'.
	/// </summary>
	public interface IHelpDesktopItem
	{
		#region Public Properties
		
		string Recid
		{
			get ;
			set ;
			  
		}
		
		string ItemKey
		{
			get ;
			set ;
			  
		}
		
		string ItemName
		{
			get ;
			set ;
			  
		}
		
		string ItemPower
		{
			get ;
			set ;
			  
		}
		
		string ItemIcon
		{
			get ;
			set ;
			  
		}
		
		int? ItemHeight
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
	/// HelpDesktopItem object for NHibernate mapped table 'HelpDesktopItem'.
	/// </summary>
	[Serializable]
	public class HelpDesktopItem : ICloneable,IHelpDesktopItem
	{
		#region Member Variables

		protected string _recid;
		protected string _itemkey;
		protected string _itemname;
		protected string _itempower;
		protected string _itemicon;
		protected int? _itemheight;
		protected int? _displayorder;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public HelpDesktopItem() {}
		
		public HelpDesktopItem(string pRecid, string pItemKey, string pItemName, string pItemPower, string pItemIcon, int? pItemHeight, int? pDisplayOrder)
		{
			this._recid = pRecid; 
			this._itemkey = pItemKey; 
			this._itemname = pItemName; 
			this._itempower = pItemPower; 
			this._itemicon = pItemIcon; 
			this._itemheight = pItemHeight; 
			this._displayorder = pDisplayOrder; 
		}
		
		#endregion
		
		#region Public Properties
		
		public string Recid
		{
			get { return _recid; }
			set 
			{
			  if (value != null && value.Length > 10)
			    throw new ArgumentOutOfRangeException("Recid", "Recid value, cannot contain more than 10 characters");
			  _bIsChanged |= (_recid != value); 
			  _recid = value; 
			}
			
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
		
		public string ItemName
		{
			get { return _itemname; }
			set 
			{
			  if (value != null && value.Length > 200)
			    throw new ArgumentOutOfRangeException("ItemName", "ItemName value, cannot contain more than 200 characters");
			  _bIsChanged |= (_itemname != value); 
			  _itemname = value; 
			}
			
		}
		
		public string ItemPower
		{
			get { return _itempower; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ItemPower", "ItemPower value, cannot contain more than 50 characters");
			  _bIsChanged |= (_itempower != value); 
			  _itempower = value; 
			}
			
		}
		
		public string ItemIcon
		{
			get { return _itemicon; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ItemIcon", "ItemIcon value, cannot contain more than 50 characters");
			  _bIsChanged |= (_itemicon != value); 
			  _itemicon = value; 
			}
			
		}
		
		public int? ItemHeight
		{
			get { return _itemheight; }
			set { _bIsChanged |= (_itemheight != value); _itemheight = value; }
			
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
			HelpDesktopItem castObj = null;
			try
			{
				castObj = (HelpDesktopItem)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
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
	
	#region Custom ICollection interface for HelpDesktopItem 

	
	public interface IHelpDesktopItemCollection : ICollection
	{
		HelpDesktopItem this[int index]{	get; set; }
		void Add(HelpDesktopItem pHelpDesktopItem);
		void Clear();
	}
	
	[Serializable]
	public class HelpDesktopItemCollection : IHelpDesktopItemCollection
	{
		private IList<HelpDesktopItem> _arrayInternal;

		public HelpDesktopItemCollection()
		{
			_arrayInternal = new List<HelpDesktopItem>();
		}
		
		public HelpDesktopItemCollection( IList<HelpDesktopItem> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<HelpDesktopItem>();
			}
		}

		public HelpDesktopItem this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((HelpDesktopItem[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(HelpDesktopItem pHelpDesktopItem) { _arrayInternal.Add(pHelpDesktopItem); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<HelpDesktopItem> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
