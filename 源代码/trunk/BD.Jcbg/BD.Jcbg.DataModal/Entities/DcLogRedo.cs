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
	/// IDcLogRedo interface for NHibernate mapped table 'DCLogRedo'.
	/// </summary>
	public interface IDcLogRedo
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string DepCode
		{
			get ;
			set ;
			  
		}
		
		string Csylb
		{
			get ;
			set ;
			  
		}
		
		string Wtbh
		{
			get ;
			set ;
			  
		}
		
		string Zh
		{
			get ;
			set ;
			  
		}
		
		string Syr
		{
			get ;
			set ;
			  
		}
		
		string Syfhr
		{
			get ;
			set ;
			  
		}
		
		DateTime? Syrq
		{
			get ;
			set ;
			  
		}
		
		string Sbmcbh
		{
			get ;
			set ;
			  
		}
		
		string Jh
		{
			get ;
			set ;
			  
		}
		
		string DataList
		{
			get ;
			set ;
			  
		}
		
		string UniqCode
		{
			get ;
			set ;
			  
		}
		
		string CheckUser
		{
			get ;
			set ;
			  
		}
		
		string Czyy
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// DcLogRedo object for NHibernate mapped table 'DCLogRedo'.
	/// </summary>
	[Serializable]
	public class DcLogRedo : ICloneable,IDcLogRedo
	{
		#region Member Variables

		protected int _recid;
		protected string _depcode;
		protected string _csylb;
		protected string _wtbh;
		protected string _zh;
		protected string _syr;
		protected string _syfhr;
		protected DateTime? _syrq;
		protected string _sbmcbh;
		protected string _jh;
		protected string _datalist;
		protected string _uniqcode;
		protected string _checkuser;
		protected string _czyy;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public DcLogRedo() {}
		
		public DcLogRedo(string pDepCode, string pCsylb, string pWtbh, string pZh, string pSyr, string pSyfhr, DateTime? pSyrq, string pSbmcbh, string pJh, string pDataList, string pUniqCode, string pCheckUser, string pCzyy)
		{
			this._depcode = pDepCode; 
			this._csylb = pCsylb; 
			this._wtbh = pWtbh; 
			this._zh = pZh; 
			this._syr = pSyr; 
			this._syfhr = pSyfhr; 
			this._syrq = pSyrq; 
			this._sbmcbh = pSbmcbh; 
			this._jh = pJh; 
			this._datalist = pDataList; 
			this._uniqcode = pUniqCode; 
			this._checkuser = pCheckUser; 
			this._czyy = pCzyy; 
		}
		
		public DcLogRedo(int pRecid)
		{
			this._recid = pRecid; 
		}
		
		#endregion
		
		#region Public Properties
		
		public virtual int Recid
		{
			get { return _recid; }
			set { _bIsChanged |= (_recid != value); _recid = value; }
			
		}
		
		public virtual string DepCode
		{
			get { return _depcode; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("DepCode", "DepCode value, cannot contain more than 50 characters");
			  _bIsChanged |= (_depcode != value); 
			  _depcode = value; 
			}
			
		}
		
		public virtual string Csylb
		{
			get { return _csylb; }
			set 
			{
			  if (value != null && value.Length > 5)
			    throw new ArgumentOutOfRangeException("Csylb", "Csylb value, cannot contain more than 5 characters");
			  _bIsChanged |= (_csylb != value); 
			  _csylb = value; 
			}
			
		}
		
		public virtual string Wtbh
		{
			get { return _wtbh; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Wtbh", "Wtbh value, cannot contain more than 50 characters");
			  _bIsChanged |= (_wtbh != value); 
			  _wtbh = value; 
			}
			
		}
		
		public virtual string Zh
		{
			get { return _zh; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Zh", "Zh value, cannot contain more than 50 characters");
			  _bIsChanged |= (_zh != value); 
			  _zh = value; 
			}
			
		}
		
		public virtual string Syr
		{
			get { return _syr; }
			set 
			{
			  if (value != null && value.Length > 20)
			    throw new ArgumentOutOfRangeException("Syr", "Syr value, cannot contain more than 20 characters");
			  _bIsChanged |= (_syr != value); 
			  _syr = value; 
			}
			
		}
		
		public virtual string Syfhr
		{
			get { return _syfhr; }
			set 
			{
			  if (value != null && value.Length > 20)
			    throw new ArgumentOutOfRangeException("Syfhr", "Syfhr value, cannot contain more than 20 characters");
			  _bIsChanged |= (_syfhr != value); 
			  _syfhr = value; 
			}
			
		}
		
		public virtual DateTime? Syrq
		{
			get { return _syrq; }
			set { _bIsChanged |= (_syrq != value); _syrq = value; }
			
		}
		
		public virtual string Sbmcbh
		{
			get { return _sbmcbh; }
			set 
			{
			  if (value != null && value.Length > 20)
			    throw new ArgumentOutOfRangeException("Sbmcbh", "Sbmcbh value, cannot contain more than 20 characters");
			  _bIsChanged |= (_sbmcbh != value); 
			  _sbmcbh = value; 
			}
			
		}
		
		public virtual string Jh
		{
			get { return _jh; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Jh", "Jh value, cannot contain more than 50 characters");
			  _bIsChanged |= (_jh != value); 
			  _jh = value; 
			}
			
		}
		
		public virtual string DataList
		{
			get { return _datalist; }
			set 
			{
			  if (value != null && value.Length > 2147483647)
			    throw new ArgumentOutOfRangeException("DataList", "DataList value, cannot contain more than 2147483647 characters");
			  _bIsChanged |= (_datalist != value); 
			  _datalist = value; 
			}
			
		}
		
		public virtual string UniqCode
		{
			get { return _uniqcode; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("UniqCode", "UniqCode value, cannot contain more than 50 characters");
			  _bIsChanged |= (_uniqcode != value); 
			  _uniqcode = value; 
			}
			
		}
		
		public virtual string CheckUser
		{
			get { return _checkuser; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("CheckUser", "CheckUser value, cannot contain more than 50 characters");
			  _bIsChanged |= (_checkuser != value); 
			  _checkuser = value; 
			}
			
		}
		
		public virtual string Czyy
		{
			get { return _czyy; }
			set 
			{
			  if (value != null && value.Length > 500)
			    throw new ArgumentOutOfRangeException("Czyy", "Czyy value, cannot contain more than 500 characters");
			  _bIsChanged |= (_czyy != value); 
			  _czyy = value; 
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
			DcLogRedo castObj = null;
			try
			{
				castObj = (DcLogRedo)obj;
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
	
	#region Custom ICollection interface for DcLogRedo 

	
	public interface IDcLogRedoCollection : ICollection
	{
		DcLogRedo this[int index]{	get; set; }
		void Add(DcLogRedo pDcLogRedo);
		void Clear();
	}
	
	[Serializable]
	public class DcLogRedoCollection : IDcLogRedoCollection
	{
		private IList<DcLogRedo> _arrayInternal;

		public DcLogRedoCollection()
		{
			_arrayInternal = new List<DcLogRedo>();
		}
		
		public DcLogRedoCollection( IList<DcLogRedo> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<DcLogRedo>();
			}
		}

		public DcLogRedo this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((DcLogRedo[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(DcLogRedo pDcLogRedo) { _arrayInternal.Add(pDcLogRedo); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<DcLogRedo> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
