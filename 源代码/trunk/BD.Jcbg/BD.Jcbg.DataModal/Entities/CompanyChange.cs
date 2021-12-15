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
	/// ICompanyChange interface for NHibernate mapped table 'CompanyChange'.
	/// </summary>
	public interface ICompanyChange
	{
		#region Public Properties
		
		int Changeid
		{
			get ;
			set ;
			  
		}
		
		string DepartmentId
		{
			get ;
			set ;
			  
		}
		
		string DepartmentName
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
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// CompanyChange object for NHibernate mapped table 'CompanyChange'.
	/// </summary>
	[Serializable]
	public class CompanyChange : ICloneable,ICompanyChange
	{
		#region Member Variables

		protected int _changeid;
		protected string _departmentid;
		protected string _departmentname;
		protected string _createdby;
		protected string _createdon;
		protected string _username;
		protected string _realname;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public CompanyChange() {}
		
		public CompanyChange(string pDepartmentId, string pDepartmentName, string pCreatedBy, string pCreatedOn, string pUserName, string pRealName)
		{
			this._departmentid = pDepartmentId; 
			this._departmentname = pDepartmentName; 
			this._createdby = pCreatedBy; 
			this._createdon = pCreatedOn; 
			this._username = pUserName; 
			this._realname = pRealName; 
		}
		
		public CompanyChange(int pChangeid)
		{
			this._changeid = pChangeid; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int Changeid
		{
			get { return _changeid; }
			set { _bIsChanged |= (_changeid != value); _changeid = value; }
			
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
			CompanyChange castObj = null;
			try
			{
				castObj = (CompanyChange)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._changeid == castObj.Changeid );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _changeid.GetHashCode();
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
	
	#region Custom ICollection interface for CompanyChange 

	
	public interface ICompanyChangeCollection : ICollection
	{
		CompanyChange this[int index]{	get; set; }
		void Add(CompanyChange pCompanyChange);
		void Clear();
	}
	
	[Serializable]
	public class CompanyChangeCollection : ICompanyChangeCollection
	{
		private IList<CompanyChange> _arrayInternal;

		public CompanyChangeCollection()
		{
			_arrayInternal = new List<CompanyChange>();
		}
		
		public CompanyChangeCollection( IList<CompanyChange> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<CompanyChange>();
			}
		}

		public CompanyChange this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((CompanyChange[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(CompanyChange pCompanyChange) { _arrayInternal.Add(pCompanyChange); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<CompanyChange> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
