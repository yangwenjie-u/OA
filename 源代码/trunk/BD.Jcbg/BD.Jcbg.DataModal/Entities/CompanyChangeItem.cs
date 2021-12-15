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
	/// ICompanyChangeItem interface for NHibernate mapped table 'CompanyChangeItem'.
	/// </summary>
	public interface ICompanyChangeItem
	{
		#region Public Properties
		
		int ChangeItemid
		{
			get ;
			set ;
			  
		}
		
		int? Changeid
		{
			get ;
			set ;
			  
		}
		
		int? ChangeType
		{
			get ;
			set ;
			  
		}
		
		string ChangeTypeName
		{
			get ;
			set ;
			  
		}
		
		string ChangeNo
		{
			get ;
			set ;
			  
		}
		
		string ChangeDate
		{
			get ;
			set ;
			  
		}
		
		string ChangeContent
		{
			get ;
			set ;
			  
		}
		
		string ChangeWork
		{
			get ;
			set ;
			  
		}
		
		decimal? ChangeMoney
		{
			get ;
			set ;
			  
		}
		
		string ChangeApprove
		{
			get ;
			set ;
			  
		}
		
		string ChangeRemark
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// CompanyChangeItem object for NHibernate mapped table 'CompanyChangeItem'.
	/// </summary>
	[Serializable]
	public class CompanyChangeItem : ICloneable,ICompanyChangeItem
	{
		#region Member Variables

		protected int _changeitemid;
		protected int? _changeid;
		protected int? _changetype;
		protected string _changetypename;
		protected string _changeno;
		protected string _changedate;
		protected string _changecontent;
		protected string _changework;
		protected decimal? _changemoney;
		protected string _changeapprove;
		protected string _changeremark;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public CompanyChangeItem() {}
		
		public CompanyChangeItem(int? pChangeid, int? pChangeType, string pChangeTypeName, string pChangeNo, string pChangeDate, string pChangeContent, string pChangeWork, decimal? pChangeMoney, string pChangeApprove, string pChangeRemark)
		{
			this._changeid = pChangeid; 
			this._changetype = pChangeType; 
			this._changetypename = pChangeTypeName; 
			this._changeno = pChangeNo; 
			this._changedate = pChangeDate; 
			this._changecontent = pChangeContent; 
			this._changework = pChangeWork; 
			this._changemoney = pChangeMoney; 
			this._changeapprove = pChangeApprove; 
			this._changeremark = pChangeRemark; 
		}
		
		public CompanyChangeItem(int pChangeItemid)
		{
			this._changeitemid = pChangeItemid; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int ChangeItemid
		{
			get { return _changeitemid; }
			set { _bIsChanged |= (_changeitemid != value); _changeitemid = value; }
			
		}
		
		public int? Changeid
		{
			get { return _changeid; }
			set { _bIsChanged |= (_changeid != value); _changeid = value; }
			
		}
		
		public int? ChangeType
		{
			get { return _changetype; }
			set { _bIsChanged |= (_changetype != value); _changetype = value; }
			
		}
		
		public string ChangeTypeName
		{
			get { return _changetypename; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ChangeTypeName", "ChangeTypeName value, cannot contain more than 50 characters");
			  _bIsChanged |= (_changetypename != value); 
			  _changetypename = value; 
			}
			
		}
		
		public string ChangeNo
		{
			get { return _changeno; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ChangeNo", "ChangeNo value, cannot contain more than 50 characters");
			  _bIsChanged |= (_changeno != value); 
			  _changeno = value; 
			}
			
		}
		
		public string ChangeDate
		{
			get { return _changedate; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ChangeDate", "ChangeDate value, cannot contain more than 50 characters");
			  _bIsChanged |= (_changedate != value); 
			  _changedate = value; 
			}
			
		}
		
		public string ChangeContent
		{
			get { return _changecontent; }
			set 
			{
			  if (value != null && value.Length > 1073741823)
			    throw new ArgumentOutOfRangeException("ChangeContent", "ChangeContent value, cannot contain more than 1073741823 characters");
			  _bIsChanged |= (_changecontent != value); 
			  _changecontent = value; 
			}
			
		}
		
		public string ChangeWork
		{
			get { return _changework; }
			set 
			{
			  if (value != null && value.Length > 1073741823)
			    throw new ArgumentOutOfRangeException("ChangeWork", "ChangeWork value, cannot contain more than 1073741823 characters");
			  _bIsChanged |= (_changework != value); 
			  _changework = value; 
			}
			
		}
		
		public decimal? ChangeMoney
		{
			get { return _changemoney; }
			set { _bIsChanged |= (_changemoney != value); _changemoney = value; }
			
		}
		
		public string ChangeApprove
		{
			get { return _changeapprove; }
			set 
			{
			  if (value != null && value.Length > 1073741823)
			    throw new ArgumentOutOfRangeException("ChangeApprove", "ChangeApprove value, cannot contain more than 1073741823 characters");
			  _bIsChanged |= (_changeapprove != value); 
			  _changeapprove = value; 
			}
			
		}
		
		public string ChangeRemark
		{
			get { return _changeremark; }
			set 
			{
			  if (value != null && value.Length > 1073741823)
			    throw new ArgumentOutOfRangeException("ChangeRemark", "ChangeRemark value, cannot contain more than 1073741823 characters");
			  _bIsChanged |= (_changeremark != value); 
			  _changeremark = value; 
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
			CompanyChangeItem castObj = null;
			try
			{
				castObj = (CompanyChangeItem)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._changeitemid == castObj.ChangeItemid );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _changeitemid.GetHashCode();
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
	
	#region Custom ICollection interface for CompanyChangeItem 

	
	public interface ICompanyChangeItemCollection : ICollection
	{
		CompanyChangeItem this[int index]{	get; set; }
		void Add(CompanyChangeItem pCompanyChangeItem);
		void Clear();
	}
	
	[Serializable]
	public class CompanyChangeItemCollection : ICompanyChangeItemCollection
	{
		private IList<CompanyChangeItem> _arrayInternal;

		public CompanyChangeItemCollection()
		{
			_arrayInternal = new List<CompanyChangeItem>();
		}
		
		public CompanyChangeItemCollection( IList<CompanyChangeItem> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<CompanyChangeItem>();
			}
		}

		public CompanyChangeItem this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((CompanyChangeItem[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(CompanyChangeItem pCompanyChangeItem) { _arrayInternal.Add(pCompanyChangeItem); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<CompanyChangeItem> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
