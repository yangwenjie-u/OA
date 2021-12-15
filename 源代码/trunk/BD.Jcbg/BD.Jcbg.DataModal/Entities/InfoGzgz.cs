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
	/// IInfoGzgz interface for NHibernate mapped table 'InfoGzgz'.
	/// </summary>
	public interface IInfoGzgz
	{
		#region Public Properties
		
		string Recid
		{
			get ;
			set ;
			  
		}
		
		string ScheduleId
		{
			get ;
			set ;
			  
		}
		
		string CompanyId
		{
			get ;
			set ;
			  
		}
		
		string GzId
		{
			get ;
			set ;
			  
		}
		
		string PaySum
		{
			get ;
			set ;
			  
		}
		
		string ProjectId
		{
			get ;
			set ;
			  
		}
		
		string HasDelete
		{
			get ;
			set ;
			  
		}
		
		string Zt
		{
			get ;
			set ;
			  
		}
		
		string Sfjs
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// InfoGzgz object for NHibernate mapped table 'InfoGzgz'.
	/// </summary>
	[Serializable]
	public class InfoGzgz : ICloneable,IInfoGzgz
	{
		#region Member Variables

		protected string _recid;
		protected string _scheduleid;
		protected string _companyid;
		protected string _gzid;
		protected string _paysum;
		protected string _projectid;
		protected string _hasdelete;
		protected string _zt;
		protected string _sfjs;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public InfoGzgz() {}
		
		public InfoGzgz(string pRecid, string pScheduleId, string pCompanyId, string pGzId, string pPaySum, string pProjectId, string pHasDelete, string pZt, string pSfjs)
		{
			this._recid = pRecid; 
			this._scheduleid = pScheduleId; 
			this._companyid = pCompanyId; 
			this._gzid = pGzId; 
			this._paysum = pPaySum; 
			this._projectid = pProjectId; 
			this._hasdelete = pHasDelete; 
			this._zt = pZt; 
			this._sfjs = pSfjs; 
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
		
		public string ScheduleId
		{
			get { return _scheduleid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ScheduleId", "ScheduleId value, cannot contain more than 50 characters");
			  _bIsChanged |= (_scheduleid != value); 
			  _scheduleid = value; 
			}
			
		}
		
		public string CompanyId
		{
			get { return _companyid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("CompanyId", "CompanyId value, cannot contain more than 50 characters");
			  _bIsChanged |= (_companyid != value); 
			  _companyid = value; 
			}
			
		}
		
		public string GzId
		{
			get { return _gzid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("GzId", "GzId value, cannot contain more than 50 characters");
			  _bIsChanged |= (_gzid != value); 
			  _gzid = value; 
			}
			
		}
		
		public string PaySum
		{
			get { return _paysum; }
			set 
			{
			  if (value != null && value.Length > 20)
			    throw new ArgumentOutOfRangeException("PaySum", "PaySum value, cannot contain more than 20 characters");
			  _bIsChanged |= (_paysum != value); 
			  _paysum = value; 
			}
			
		}
		
		public string ProjectId
		{
			get { return _projectid; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("ProjectId", "ProjectId value, cannot contain more than 50 characters");
			  _bIsChanged |= (_projectid != value); 
			  _projectid = value; 
			}
			
		}
		
		public string HasDelete
		{
			get { return _hasdelete; }
			set 
			{
			  if (value != null && value.Length > 1)
			    throw new ArgumentOutOfRangeException("HasDelete", "HasDelete value, cannot contain more than 1 characters");
			  _bIsChanged |= (_hasdelete != value); 
			  _hasdelete = value; 
			}
			
		}
		
		public string Zt
		{
			get { return _zt; }
			set 
			{
			  if (value != null && value.Length > 1)
			    throw new ArgumentOutOfRangeException("Zt", "Zt value, cannot contain more than 1 characters");
			  _bIsChanged |= (_zt != value); 
			  _zt = value; 
			}
			
		}
		
		public string Sfjs
		{
			get { return _sfjs; }
			set 
			{
			  if (value != null && value.Length > 1)
			    throw new ArgumentOutOfRangeException("Sfjs", "Sfjs value, cannot contain more than 1 characters");
			  _bIsChanged |= (_sfjs != value); 
			  _sfjs = value; 
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
			InfoGzgz castObj = null;
			try
			{
				castObj = (InfoGzgz)obj;
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
	
	#region Custom ICollection interface for InfoGzgz 

	
	public interface IInfoGzgzCollection : ICollection
	{
		InfoGzgz this[int index]{	get; set; }
		void Add(InfoGzgz pInfoGzgz);
		void Clear();
	}
	
	[Serializable]
	public class InfoGzgzCollection : IInfoGzgzCollection
	{
		private IList<InfoGzgz> _arrayInternal;

		public InfoGzgzCollection()
		{
			_arrayInternal = new List<InfoGzgz>();
		}
		
		public InfoGzgzCollection( IList<InfoGzgz> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<InfoGzgz>();
			}
		}

		public InfoGzgz this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((InfoGzgz[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(InfoGzgz pInfoGzgz) { _arrayInternal.Add(pInfoGzgz); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<InfoGzgz> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
