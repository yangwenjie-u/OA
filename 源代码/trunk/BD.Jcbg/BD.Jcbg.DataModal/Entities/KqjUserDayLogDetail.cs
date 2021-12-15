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
	/// IKqjUserDayLogDetail interface for NHibernate mapped table 'KqjUserDayLogDetail'.
	/// </summary>
	public interface IKqjUserDayLogDetail
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		int? ParentId
		{
			get ;
			set ;
			  
		}
		
		DateTime? InTime
		{
			get ;
			set ;
			  
		}
		
		DateTime? OutTime
		{
			get ;
			set ;
			  
		}
		
		string ScheduleId
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// KqjUserDayLogDetail object for NHibernate mapped table 'KqjUserDayLogDetail'.
	/// </summary>
	[Serializable]
	public class KqjUserDayLogDetail : ICloneable,IKqjUserDayLogDetail
	{
		#region Member Variables

		protected int _recid;
		protected int? _parentid;
		protected DateTime? _intime;
		protected DateTime? _outtime;
		protected string _scheduleid;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public KqjUserDayLogDetail() {}
		
		public KqjUserDayLogDetail(int? pParentId, DateTime? pInTime, DateTime? pOutTime, string pScheduleId)
		{
			this._parentid = pParentId; 
			this._intime = pInTime; 
			this._outtime = pOutTime; 
			this._scheduleid = pScheduleId; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int Recid
		{
			get { return _recid; }
			set { _bIsChanged |= (_recid != value); _recid = value; }
			
		}
		
		public int? ParentId
		{
			get { return _parentid; }
			set { _bIsChanged |= (_parentid != value); _parentid = value; }
			
		}
		
		public DateTime? InTime
		{
			get { return _intime; }
			set { _bIsChanged |= (_intime != value); _intime = value; }
			
		}
		
		public DateTime? OutTime
		{
			get { return _outtime; }
			set { _bIsChanged |= (_outtime != value); _outtime = value; }
			
		}
		
		public string ScheduleId
		{
			get { return _scheduleid; }
			set 
			{
			  if (value != null && value.Length > 20)
			    throw new ArgumentOutOfRangeException("ScheduleId", "ScheduleId value, cannot contain more than 20 characters");
			  _bIsChanged |= (_scheduleid != value); 
			  _scheduleid = value; 
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
			KqjUserDayLogDetail castObj = null;
			try
			{
				castObj = (KqjUserDayLogDetail)obj;
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
	
	#region Custom ICollection interface for KqjUserDayLogDetail 

	
	public interface IKqjUserDayLogDetailCollection : ICollection
	{
		KqjUserDayLogDetail this[int index]{	get; set; }
		void Add(KqjUserDayLogDetail pKqjUserDayLogDetail);
		void Clear();
	}
	
	[Serializable]
	public class KqjUserDayLogDetailCollection : IKqjUserDayLogDetailCollection
	{
		private IList<KqjUserDayLogDetail> _arrayInternal;

		public KqjUserDayLogDetailCollection()
		{
			_arrayInternal = new List<KqjUserDayLogDetail>();
		}
		
		public KqjUserDayLogDetailCollection( IList<KqjUserDayLogDetail> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<KqjUserDayLogDetail>();
			}
		}

		public KqjUserDayLogDetail this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((KqjUserDayLogDetail[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(KqjUserDayLogDetail pKqjUserDayLogDetail) { _arrayInternal.Add(pKqjUserDayLogDetail); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<KqjUserDayLogDetail> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
