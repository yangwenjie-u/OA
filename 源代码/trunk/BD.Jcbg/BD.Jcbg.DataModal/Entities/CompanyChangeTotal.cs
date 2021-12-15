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
	/// ICompanyChangeTotal interface for NHibernate mapped table 'CompanyChangeTotal'.
	/// </summary>
	public interface ICompanyChangeTotal
	{
		#region Public Properties
		
		int ChangeTotalid
		{
			get ;
			set ;
			  
		}
		
		string TotoleName
		{
			get ;
			set ;
			  
		}
		
		string CreatedBy
		{
			get ;
			set ;
			  
		}
		
		string CreatedOn
		{
			get ;
			set ;
			  
		}
		
		string UserName
		{
			get ;
			set ;
			  
		}
		
		string RealName
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
	/// CompanyChangeTotal object for NHibernate mapped table 'CompanyChangeTotal'.
	/// </summary>
	[Serializable]
	public class CompanyChangeTotal : ICloneable,ICompanyChangeTotal
	{
		#region Member Variables

		protected int _changetotalid;
		protected string _totolename;
		protected string _createdby;
		protected string _createdon;
		protected string _username;
		protected string _realname;
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
		public CompanyChangeTotal() {}
		
		public CompanyChangeTotal(string pTotoleName, string pCreatedBy, string pCreatedOn, string pUserName, string pRealName, string pOldMoney, string pTempMoney, int? pCno, decimal? pCmoney, int? pCtotalNo, decimal? pCtotalMoney, int? pBno, decimal? pBmoney, int? pBtotalNo, decimal? pBtotalMoney, int? pAno, decimal? pAmoney, int? pAtotalNo, decimal? pAtotalMoney, int? pBigNo, decimal? pBigMoney, int? pBigTotalNo, decimal? pBigTotalMoney, int? pAlllNo, decimal? pAllMoney, int? pTotalNo, decimal? pTotalMoney)
		{
			this._totolename = pTotoleName; 
			this._createdby = pCreatedBy; 
			this._createdon = pCreatedOn; 
			this._username = pUserName; 
			this._realname = pRealName; 
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
		
		public CompanyChangeTotal(int pChangeTotalid)
		{
			this._changetotalid = pChangeTotalid; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int ChangeTotalid
		{
			get { return _changetotalid; }
			set { _bIsChanged |= (_changetotalid != value); _changetotalid = value; }
			
		}
		
		public string TotoleName
		{
			get { return _totolename; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("TotoleName", "TotoleName value, cannot contain more than 50 characters");
			  _bIsChanged |= (_totolename != value); 
			  _totolename = value; 
			}
			
		}
		
		public string CreatedBy
		{
			get { return _createdby; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("CreatedBy", "CreatedBy value, cannot contain more than 50 characters");
			  _bIsChanged |= (_createdby != value); 
			  _createdby = value; 
			}
			
		}
		
		public string CreatedOn
		{
			get { return _createdon; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("CreatedOn", "CreatedOn value, cannot contain more than 50 characters");
			  _bIsChanged |= (_createdon != value); 
			  _createdon = value; 
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
		
		public string RealName
		{
			get { return _realname; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("RealName", "RealName value, cannot contain more than 50 characters");
			  _bIsChanged |= (_realname != value); 
			  _realname = value; 
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
			CompanyChangeTotal castObj = null;
			try
			{
				castObj = (CompanyChangeTotal)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._changetotalid == castObj.ChangeTotalid );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _changetotalid.GetHashCode();
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
	
	#region Custom ICollection interface for CompanyChangeTotal 

	
	public interface ICompanyChangeTotalCollection : ICollection
	{
		CompanyChangeTotal this[int index]{	get; set; }
		void Add(CompanyChangeTotal pCompanyChangeTotal);
		void Clear();
	}
	
	[Serializable]
	public class CompanyChangeTotalCollection : ICompanyChangeTotalCollection
	{
		private IList<CompanyChangeTotal> _arrayInternal;

		public CompanyChangeTotalCollection()
		{
			_arrayInternal = new List<CompanyChangeTotal>();
		}
		
		public CompanyChangeTotalCollection( IList<CompanyChangeTotal> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<CompanyChangeTotal>();
			}
		}

		public CompanyChangeTotal this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((CompanyChangeTotal[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(CompanyChangeTotal pCompanyChangeTotal) { _arrayInternal.Add(pCompanyChangeTotal); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<CompanyChangeTotal> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
