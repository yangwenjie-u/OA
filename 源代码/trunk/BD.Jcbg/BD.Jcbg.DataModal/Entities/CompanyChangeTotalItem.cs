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
	/// ICompanyChangeTotalItem interface for NHibernate mapped table 'CompanyChangeTotalItem'.
	/// </summary>
	public interface ICompanyChangeTotalItem
	{
		#region Public Properties
		
		int ChangeTotalItemid
		{
			get ;
			set ;
			  
		}
		
		int? ChangeTotalid
		{
			get ;
			set ;
			  
		}
		
		string ProjectType
		{
			get ;
			set ;
			  
		}
		
		string DepartmentName
		{
			get ;
			set ;
			  
		}
		
		string DepartmentId
		{
			get ;
			set ;
			  
		}
		
		string OldMoney
		{
			get ;
			set ;
			  
		}
		
		string TempMoney
		{
			get ;
			set ;
			  
		}
		
		int? Cno
		{
			get ;
			set ;
			  
		}
		
		decimal? Cmoney
		{
			get ;
			set ;
			  
		}
		
		int? CtotalNo
		{
			get ;
			set ;
			  
		}
		
		decimal? CtotalMoney
		{
			get ;
			set ;
			  
		}
		
		int? Bno
		{
			get ;
			set ;
			  
		}
		
		decimal? Bmoney
		{
			get ;
			set ;
			  
		}
		
		int? BtotalNo
		{
			get ;
			set ;
			  
		}
		
		decimal? BtotalMoney
		{
			get ;
			set ;
			  
		}
		
		int? Ano
		{
			get ;
			set ;
			  
		}
		
		decimal? Amoney
		{
			get ;
			set ;
			  
		}
		
		int? AtotalNo
		{
			get ;
			set ;
			  
		}
		
		decimal? AtotalMoney
		{
			get ;
			set ;
			  
		}
		
		int? BigNo
		{
			get ;
			set ;
			  
		}
		
		decimal? BigMoney
		{
			get ;
			set ;
			  
		}
		
		int? BigTotalNo
		{
			get ;
			set ;
			  
		}
		
		decimal? BigTotalMoney
		{
			get ;
			set ;
			  
		}
		
		int? AlllNo
		{
			get ;
			set ;
			  
		}
		
		decimal? AllMoney
		{
			get ;
			set ;
			  
		}
		
		int? TotalNo
		{
			get ;
			set ;
			  
		}
		
		decimal? TotalMoney
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// CompanyChangeTotalItem object for NHibernate mapped table 'CompanyChangeTotalItem'.
	/// </summary>
	[Serializable]
	public class CompanyChangeTotalItem : ICloneable,ICompanyChangeTotalItem
	{
		#region Member Variables

		protected int _changetotalitemid;
		protected int? _changetotalid;
		protected string _projecttype;
		protected string _departmentname;
		protected string _departmentid;
		protected string _oldmoney;
		protected string _tempmoney;
		protected int? _cno;
		protected decimal? _cmoney;
		protected int? _ctotalno;
		protected decimal? _ctotalmoney;
		protected int? _bno;
		protected decimal? _bmoney;
		protected int? _btotalno;
		protected decimal? _btotalmoney;
		protected int? _ano;
		protected decimal? _amoney;
		protected int? _atotalno;
		protected decimal? _atotalmoney;
		protected int? _bigno;
		protected decimal? _bigmoney;
		protected int? _bigtotalno;
		protected decimal? _bigtotalmoney;
		protected int? _alllno;
		protected decimal? _allmoney;
		protected int? _totalno;
		protected decimal? _totalmoney;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public CompanyChangeTotalItem() {}
		
		public CompanyChangeTotalItem(int? pChangeTotalid, string pProjectType, string pDepartmentName, string pDepartmentId, string pOldMoney, string pTempMoney, int? pCno, decimal? pCmoney, int? pCtotalNo, decimal? pCtotalMoney, int? pBno, decimal? pBmoney, int? pBtotalNo, decimal? pBtotalMoney, int? pAno, decimal? pAmoney, int? pAtotalNo, decimal? pAtotalMoney, int? pBigNo, decimal? pBigMoney, int? pBigTotalNo, decimal? pBigTotalMoney, int? pAlllNo, decimal? pAllMoney, int? pTotalNo, decimal? pTotalMoney)
		{
			this._changetotalid = pChangeTotalid; 
			this._projecttype = pProjectType; 
			this._departmentname = pDepartmentName; 
			this._departmentid = pDepartmentId; 
			this._oldmoney = pOldMoney; 
			this._tempmoney = pTempMoney; 
			this._cno = pCno; 
			this._cmoney = pCmoney; 
			this._ctotalno = pCtotalNo; 
			this._ctotalmoney = pCtotalMoney; 
			this._bno = pBno; 
			this._bmoney = pBmoney; 
			this._btotalno = pBtotalNo; 
			this._btotalmoney = pBtotalMoney; 
			this._ano = pAno; 
			this._amoney = pAmoney; 
			this._atotalno = pAtotalNo; 
			this._atotalmoney = pAtotalMoney; 
			this._bigno = pBigNo; 
			this._bigmoney = pBigMoney; 
			this._bigtotalno = pBigTotalNo; 
			this._bigtotalmoney = pBigTotalMoney; 
			this._alllno = pAlllNo; 
			this._allmoney = pAllMoney; 
			this._totalno = pTotalNo; 
			this._totalmoney = pTotalMoney; 
		}
		
		public CompanyChangeTotalItem(int pChangeTotalItemid)
		{
			this._changetotalitemid = pChangeTotalItemid; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int ChangeTotalItemid
		{
			get { return _changetotalitemid; }
			set { _bIsChanged |= (_changetotalitemid != value); _changetotalitemid = value; }
			
		}
		
		public int? ChangeTotalid
		{
			get { return _changetotalid; }
			set { _bIsChanged |= (_changetotalid != value); _changetotalid = value; }
			
		}
		
		public string ProjectType
		{
			get { return _projecttype; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ProjectType", "ProjectType value, cannot contain more than 50 characters");
			  _bIsChanged |= (_projecttype != value); 
			  _projecttype = value; 
			}
			
		}
		
		public string DepartmentName
		{
			get { return _departmentname; }
			set 
			{
			  if (value != null && value.Length > 500)
			    throw new ArgumentOutOfRangeException("DepartmentName", "DepartmentName value, cannot contain more than 500 characters");
			  _bIsChanged |= (_departmentname != value); 
			  _departmentname = value; 
			}
			
		}
		
		public string DepartmentId
		{
			get { return _departmentid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("DepartmentId", "DepartmentId value, cannot contain more than 50 characters");
			  _bIsChanged |= (_departmentid != value); 
			  _departmentid = value; 
			}
			
		}
		
		public string OldMoney
		{
			get { return _oldmoney; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("OldMoney", "OldMoney value, cannot contain more than 50 characters");
			  _bIsChanged |= (_oldmoney != value); 
			  _oldmoney = value; 
			}
			
		}
		
		public string TempMoney
		{
			get { return _tempmoney; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("TempMoney", "TempMoney value, cannot contain more than 50 characters");
			  _bIsChanged |= (_tempmoney != value); 
			  _tempmoney = value; 
			}
			
		}
		
		public int? Cno
		{
			get { return _cno; }
			set { _bIsChanged |= (_cno != value); _cno = value; }
			
		}
		
		public decimal? Cmoney
		{
			get { return _cmoney; }
			set { _bIsChanged |= (_cmoney != value); _cmoney = value; }
			
		}
		
		public int? CtotalNo
		{
			get { return _ctotalno; }
			set { _bIsChanged |= (_ctotalno != value); _ctotalno = value; }
			
		}
		
		public decimal? CtotalMoney
		{
			get { return _ctotalmoney; }
			set { _bIsChanged |= (_ctotalmoney != value); _ctotalmoney = value; }
			
		}
		
		public int? Bno
		{
			get { return _bno; }
			set { _bIsChanged |= (_bno != value); _bno = value; }
			
		}
		
		public decimal? Bmoney
		{
			get { return _bmoney; }
			set { _bIsChanged |= (_bmoney != value); _bmoney = value; }
			
		}
		
		public int? BtotalNo
		{
			get { return _btotalno; }
			set { _bIsChanged |= (_btotalno != value); _btotalno = value; }
			
		}
		
		public decimal? BtotalMoney
		{
			get { return _btotalmoney; }
			set { _bIsChanged |= (_btotalmoney != value); _btotalmoney = value; }
			
		}
		
		public int? Ano
		{
			get { return _ano; }
			set { _bIsChanged |= (_ano != value); _ano = value; }
			
		}
		
		public decimal? Amoney
		{
			get { return _amoney; }
			set { _bIsChanged |= (_amoney != value); _amoney = value; }
			
		}
		
		public int? AtotalNo
		{
			get { return _atotalno; }
			set { _bIsChanged |= (_atotalno != value); _atotalno = value; }
			
		}
		
		public decimal? AtotalMoney
		{
			get { return _atotalmoney; }
			set { _bIsChanged |= (_atotalmoney != value); _atotalmoney = value; }
			
		}
		
		public int? BigNo
		{
			get { return _bigno; }
			set { _bIsChanged |= (_bigno != value); _bigno = value; }
			
		}
		
		public decimal? BigMoney
		{
			get { return _bigmoney; }
			set { _bIsChanged |= (_bigmoney != value); _bigmoney = value; }
			
		}
		
		public int? BigTotalNo
		{
			get { return _bigtotalno; }
			set { _bIsChanged |= (_bigtotalno != value); _bigtotalno = value; }
			
		}
		
		public decimal? BigTotalMoney
		{
			get { return _bigtotalmoney; }
			set { _bIsChanged |= (_bigtotalmoney != value); _bigtotalmoney = value; }
			
		}
		
		public int? AlllNo
		{
			get { return _alllno; }
			set { _bIsChanged |= (_alllno != value); _alllno = value; }
			
		}
		
		public decimal? AllMoney
		{
			get { return _allmoney; }
			set { _bIsChanged |= (_allmoney != value); _allmoney = value; }
			
		}
		
		public int? TotalNo
		{
			get { return _totalno; }
			set { _bIsChanged |= (_totalno != value); _totalno = value; }
			
		}
		
		public decimal? TotalMoney
		{
			get { return _totalmoney; }
			set { _bIsChanged |= (_totalmoney != value); _totalmoney = value; }
			
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
			CompanyChangeTotalItem castObj = null;
			try
			{
				castObj = (CompanyChangeTotalItem)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._changetotalitemid == castObj.ChangeTotalItemid );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _changetotalitemid.GetHashCode();
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
	
	#region Custom ICollection interface for CompanyChangeTotalItem 

	
	public interface ICompanyChangeTotalItemCollection : ICollection
	{
		CompanyChangeTotalItem this[int index]{	get; set; }
		void Add(CompanyChangeTotalItem pCompanyChangeTotalItem);
		void Clear();
	}
	
	[Serializable]
	public class CompanyChangeTotalItemCollection : ICompanyChangeTotalItemCollection
	{
		private IList<CompanyChangeTotalItem> _arrayInternal;

		public CompanyChangeTotalItemCollection()
		{
			_arrayInternal = new List<CompanyChangeTotalItem>();
		}
		
		public CompanyChangeTotalItemCollection( IList<CompanyChangeTotalItem> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<CompanyChangeTotalItem>();
			}
		}

		public CompanyChangeTotalItem this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((CompanyChangeTotalItem[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(CompanyChangeTotalItem pCompanyChangeTotalItem) { _arrayInternal.Add(pCompanyChangeTotalItem); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<CompanyChangeTotalItem> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
