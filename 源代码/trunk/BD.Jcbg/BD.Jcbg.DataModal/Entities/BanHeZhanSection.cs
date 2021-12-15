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
	/// IBanHeZhanSection interface for NHibernate mapped table 'BanHeZhanSection'.
	/// </summary>
	public interface IBanHeZhanSection
	{
		#region Public Properties
		
		int SectionId
		{
			get ;
			set ;
			  
		}
		
		string SectionName
		{
			get ;
			set ;
			  
		}
		
		int? CurrPage
		{
			get ;
			set ;
			  
		}
		
		int? CurrIndex
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// BanHeZhanSection object for NHibernate mapped table 'BanHeZhanSection'.
	/// </summary>
	[Serializable]
	public class BanHeZhanSection : ICloneable,IBanHeZhanSection
	{
		#region Member Variables

		protected int _sectionid;
		protected string _sectionname;
		protected int? _currpage;
		protected int? _currindex;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public BanHeZhanSection() {}
		
		public BanHeZhanSection(int pSectionId, string pSectionName, int? pCurrPage, int? pCurrIndex)
		{
			this._sectionid = pSectionId; 
			this._sectionname = pSectionName; 
			this._currpage = pCurrPage; 
			this._currindex = pCurrIndex; 
		}
		
		public BanHeZhanSection(int pSectionId)
		{
			this._sectionid = pSectionId; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int SectionId
		{
			get { return _sectionid; }
			set { _bIsChanged |= (_sectionid != value); _sectionid = value; }
			
		}
		
		public string SectionName
		{
			get { return _sectionname; }
			set 
			{
			  if (value != null && value.Length > 10)
			    throw new ArgumentOutOfRangeException("SectionName", "SectionName value, cannot contain more than 10 characters");
			  _bIsChanged |= (_sectionname != value); 
			  _sectionname = value; 
			}
			
		}
		
		public int? CurrPage
		{
			get { return _currpage; }
			set { _bIsChanged |= (_currpage != value); _currpage = value; }
			
		}
		
		public int? CurrIndex
		{
			get { return _currindex; }
			set { _bIsChanged |= (_currindex != value); _currindex = value; }
			
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
			BanHeZhanSection castObj = null;
			try
			{
				castObj = (BanHeZhanSection)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._sectionid == castObj.SectionId );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _sectionid.GetHashCode();
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
	
	#region Custom ICollection interface for BanHeZhanSection 

	
	public interface IBanHeZhanSectionCollection : ICollection
	{
		BanHeZhanSection this[int index]{	get; set; }
		void Add(BanHeZhanSection pBanHeZhanSection);
		void Clear();
	}
	
	[Serializable]
	public class BanHeZhanSectionCollection : IBanHeZhanSectionCollection
	{
		private IList<BanHeZhanSection> _arrayInternal;

		public BanHeZhanSectionCollection()
		{
			_arrayInternal = new List<BanHeZhanSection>();
		}
		
		public BanHeZhanSectionCollection( IList<BanHeZhanSection> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<BanHeZhanSection>();
			}
		}

		public BanHeZhanSection this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((BanHeZhanSection[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(BanHeZhanSection pBanHeZhanSection) { _arrayInternal.Add(pBanHeZhanSection); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<BanHeZhanSection> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
