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
	/// IViewSelfDesktopItem interface for NHibernate mapped table 'ViewSelfDesktopItem'.
	/// </summary>
	public interface IViewSelfDesktopItem
	{
		#region Public Properties
		
		int Recid
		{
			get ;
		}
		
		string UserName
		{
			get ;
		}
		
		string ItemColumn
		{
			get ;
		}
		
		int? DisplayOrder
		{
			get ;
		}
		
		bool? IsDisplay
		{
			get ;
		}
		
		string ItemKey
		{
			get ;
		}
		
		string ItemName
		{
			get ;
		}
		
		string ItemPower
		{
			get ;
		}
		
		string ItemIcon
		{
			get ;
		}
		
		int? ItemHeight
		{
			get ;
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// ViewSelfDesktopItem object for NHibernate mapped table 'ViewSelfDesktopItem'.
	/// </summary>
	[Serializable]
	public class ViewSelfDesktopItem : ICloneable,IViewSelfDesktopItem
	{
		#region Member Variables

		protected int _recid;
		protected string _username;
		protected string _itemcolumn;
		protected int? _displayorder;
		protected bool? _isdisplay;
		protected string _itemkey;
		protected string _itemname;
		protected string _itempower;
		protected string _itemicon;
		protected int? _itemheight;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public ViewSelfDesktopItem() {}
		
		#endregion
		
		#region Public Properties
		
		public int Recid
		{
			get { return _recid; }
		}
		
		public string UserName
		{
			get { return _username; }
		}
		
		public string ItemColumn
		{
			get { return _itemcolumn; }
		}
		
		public int? DisplayOrder
		{
			get { return _displayorder; }
		}
		
		public bool? IsDisplay
		{
			get { return _isdisplay; }
		}
		
		public string ItemKey
		{
			get { return _itemkey; }
		}
		
		public string ItemName
		{
			get { return _itemname; }
		}
		
		public string ItemPower
		{
			get { return _itempower; }
		}
		
		public string ItemIcon
		{
			get { return _itemicon; }
		}
		
		public int? ItemHeight
		{
			get { return _itemheight; }
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
			ViewSelfDesktopItem castObj = null;
			try
			{
				castObj = (ViewSelfDesktopItem)obj;
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
	
	#region Custom ICollection interface for ViewSelfDesktopItem 

	
	public interface IViewSelfDesktopItemCollection : ICollection
	{
		ViewSelfDesktopItem this[int index]{	get; set; }
		void Add(ViewSelfDesktopItem pViewSelfDesktopItem);
		void Clear();
	}
	
	[Serializable]
	public class ViewSelfDesktopItemCollection : IViewSelfDesktopItemCollection
	{
		private IList<ViewSelfDesktopItem> _arrayInternal;

		public ViewSelfDesktopItemCollection()
		{
			_arrayInternal = new List<ViewSelfDesktopItem>();
		}
		
		public ViewSelfDesktopItemCollection( IList<ViewSelfDesktopItem> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<ViewSelfDesktopItem>();
			}
		}

		public ViewSelfDesktopItem this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((ViewSelfDesktopItem[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(ViewSelfDesktopItem pViewSelfDesktopItem) { _arrayInternal.Add(pViewSelfDesktopItem); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<ViewSelfDesktopItem> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
