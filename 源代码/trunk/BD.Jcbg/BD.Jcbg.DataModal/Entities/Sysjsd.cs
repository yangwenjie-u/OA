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
	/// ISysjsd interface for NHibernate mapped table 'SYSJSD'.
	/// </summary>
	public interface ISysjsd
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
		
		int? MId
		{
			get ;
			set ;
			  
		}
		
		int? Rsylb
		{
			get ;
			set ;
			  
		}
		
		int? Nsylb
		{
			get ;
			set ;
			  
		}
		
		string Commsylb
		{
			get ;
			set ;
			  
		}
		
		string Otheritem
		{
			get ;
			set ;
			  
		}
		
		int? Syxz
		{
			get ;
			set ;
			  
		}
		
		string SysjZd
		{
			get ;
			set ;
			  
		}
		
		string SubZd
		{
			get ;
			set ;
			  
		}
		
		string Sy
		{
			get ;
			set ;
			  
		}
		
		int? DataOff
		{
			get ;
			set ;
			  
		}
		
		int? DataLen
		{
			get ;
			set ;
			  
		}
		
		int? DotPos
		{
			get ;
			set ;
			  
		}
		
		int? Manual
		{
			get ;
			set ;
			  
		}
		
		int? DisplayOrder
		{
			get ;
			set ;
			  
		}
		
		string DefaultValue
		{
			get ;
			set ;
			  
		}
		
		int? BlockNumber
		{
			get ;
			set ;
			  
		}
		
		string CalculFunc
		{
			get ;
			set ;
			  
		}
		
		int? ManuPicIdx
		{
			get ;
			set ;
			  
		}
		
		string AltFunc
		{
			get ;
			set ;
			  
		}
		
		int? PortNum
		{
			get ;
			set ;
			  
		}
		
		bool? EnterValue
		{
			get ;
			set ;
			  
		}
		
		short? ShowImageIndex
		{
			get ;
			set ;
			  
		}
		
		string CjZd
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// Sysjsd object for NHibernate mapped table 'SYSJSD'.
	/// </summary>
	[Serializable]
	public class Sysjsd : ICloneable,ISysjsd
	{
		#region Member Variables

		protected int _recid;
		protected string _depcode;
		protected string _csylb;
		protected int? _mid;
		protected int? _rsylb;
		protected int? _nsylb;
		protected string _commsylb;
		protected string _otheritem;
		protected int? _syxz;
		protected string _sysjzd;
		protected string _subzd;
		protected string _sy;
		protected int? _dataoff;
		protected int? _datalen;
		protected int? _dotpos;
		protected int? _manual;
		protected int? _displayorder;
		protected string _defaultvalue;
		protected int? _blocknumber;
		protected string _calculfunc;
		protected int? _manupicidx;
		protected string _altfunc;
		protected int? _portnum;
		protected bool? _entervalue;
		protected short? _showimageindex;
		protected string _cjzd;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public Sysjsd() {}
		
		public Sysjsd(string pDepCode, string pCsylb, int? pMid, int? pRsylb, int? pNsylb, string pCommsylb, string pOtheritem, int? pSyxz, string pSysjZd, string pSubZd, string pSy, int? pDataOff, int? pDataLen, int? pDotPos, int? pManual, int? pDisplayOrder, string pDefaultValue, int? pBlockNumber, string pCalculFunc, int? pManuPicIdx, string pAltFunc, int? pPortNum, bool? pEnterValue, short? pShowImageIndex, string pCjZd)
		{
			this._depcode = pDepCode; 
			this._csylb = pCsylb; 
			this._mid = pMid; 
			this._rsylb = pRsylb; 
			this._nsylb = pNsylb; 
			this._commsylb = pCommsylb; 
			this._otheritem = pOtheritem; 
			this._syxz = pSyxz; 
			this._sysjzd = pSysjZd; 
			this._subzd = pSubZd; 
			this._sy = pSy; 
			this._dataoff = pDataOff; 
			this._datalen = pDataLen; 
			this._dotpos = pDotPos; 
			this._manual = pManual; 
			this._displayorder = pDisplayOrder; 
			this._defaultvalue = pDefaultValue; 
			this._blocknumber = pBlockNumber; 
			this._calculfunc = pCalculFunc; 
			this._manupicidx = pManuPicIdx; 
			this._altfunc = pAltFunc; 
			this._portnum = pPortNum; 
			this._entervalue = pEnterValue; 
			this._showimageindex = pShowImageIndex; 
			this._cjzd = pCjZd; 
		}
		
		public Sysjsd(int pRecid)
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
			  if (value != null && value.Length > 40)
			    throw new ArgumentOutOfRangeException("Csylb", "Csylb value, cannot contain more than 40 characters");
			  _bIsChanged |= (_csylb != value); 
			  _csylb = value; 
			}
			
		}
		
		public virtual int? MId
		{
			get { return _mid; }
			set { _bIsChanged |= (_mid != value); _mid = value; }
			
		}
		
		public virtual int? Rsylb
		{
			get { return _rsylb; }
			set { _bIsChanged |= (_rsylb != value); _rsylb = value; }
			
		}
		
		public virtual int? Nsylb
		{
			get { return _nsylb; }
			set { _bIsChanged |= (_nsylb != value); _nsylb = value; }
			
		}
		
		public virtual string Commsylb
		{
			get { return _commsylb; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Commsylb", "Commsylb value, cannot contain more than 50 characters");
			  _bIsChanged |= (_commsylb != value); 
			  _commsylb = value; 
			}
			
		}
		
		public virtual string Otheritem
		{
			get { return _otheritem; }
			set 
			{
			  if (value != null && value.Length > 12)
			    throw new ArgumentOutOfRangeException("Otheritem", "Otheritem value, cannot contain more than 12 characters");
			  _bIsChanged |= (_otheritem != value); 
			  _otheritem = value; 
			}
			
		}
		
		public virtual int? Syxz
		{
			get { return _syxz; }
			set { _bIsChanged |= (_syxz != value); _syxz = value; }
			
		}
		
		public virtual string SysjZd
		{
			get { return _sysjzd; }
			set 
			{
			  if (value != null && value.Length > 20)
			    throw new ArgumentOutOfRangeException("SysjZd", "SysjZd value, cannot contain more than 20 characters");
			  _bIsChanged |= (_sysjzd != value); 
			  _sysjzd = value; 
			}
			
		}
		
		public virtual string SubZd
		{
			get { return _subzd; }
			set 
			{
			  if (value != null && value.Length > 20)
			    throw new ArgumentOutOfRangeException("SubZd", "SubZd value, cannot contain more than 20 characters");
			  _bIsChanged |= (_subzd != value); 
			  _subzd = value; 
			}
			
		}
		
		public virtual string Sy
		{
			get { return _sy; }
			set 
			{
			  if (value != null && value.Length > 40)
			    throw new ArgumentOutOfRangeException("Sy", "Sy value, cannot contain more than 40 characters");
			  _bIsChanged |= (_sy != value); 
			  _sy = value; 
			}
			
		}
		
		public virtual int? DataOff
		{
			get { return _dataoff; }
			set { _bIsChanged |= (_dataoff != value); _dataoff = value; }
			
		}
		
		public virtual int? DataLen
		{
			get { return _datalen; }
			set { _bIsChanged |= (_datalen != value); _datalen = value; }
			
		}
		
		public virtual int? DotPos
		{
			get { return _dotpos; }
			set { _bIsChanged |= (_dotpos != value); _dotpos = value; }
			
		}
		
		public virtual int? Manual
		{
			get { return _manual; }
			set { _bIsChanged |= (_manual != value); _manual = value; }
			
		}
		
		public virtual int? DisplayOrder
		{
			get { return _displayorder; }
			set { _bIsChanged |= (_displayorder != value); _displayorder = value; }
			
		}
		
		public virtual string DefaultValue
		{
			get { return _defaultvalue; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("DefaultValue", "DefaultValue value, cannot contain more than 50 characters");
			  _bIsChanged |= (_defaultvalue != value); 
			  _defaultvalue = value; 
			}
			
		}
		
		public virtual int? BlockNumber
		{
			get { return _blocknumber; }
			set { _bIsChanged |= (_blocknumber != value); _blocknumber = value; }
			
		}
		
		public virtual string CalculFunc
		{
			get { return _calculfunc; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("CalculFunc", "CalculFunc value, cannot contain more than 100 characters");
			  _bIsChanged |= (_calculfunc != value); 
			  _calculfunc = value; 
			}
			
		}
		
		public virtual int? ManuPicIdx
		{
			get { return _manupicidx; }
			set { _bIsChanged |= (_manupicidx != value); _manupicidx = value; }
			
		}
		
		public virtual string AltFunc
		{
			get { return _altfunc; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("AltFunc", "AltFunc value, cannot contain more than 100 characters");
			  _bIsChanged |= (_altfunc != value); 
			  _altfunc = value; 
			}
			
		}
		
		public virtual int? PortNum
		{
			get { return _portnum; }
			set { _bIsChanged |= (_portnum != value); _portnum = value; }
			
		}
		
		public virtual bool? EnterValue
		{
			get { return _entervalue; }
			set { _bIsChanged |= (_entervalue != value); _entervalue = value; }
			
		}
		
		public virtual short? ShowImageIndex
		{
			get { return _showimageindex; }
			set { _bIsChanged |= (_showimageindex != value); _showimageindex = value; }
			
		}
		
		public virtual string CjZd
		{
			get { return _cjzd; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("CjZd", "CjZd value, cannot contain more than 50 characters");
			  _bIsChanged |= (_cjzd != value); 
			  _cjzd = value; 
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
			Sysjsd castObj = null;
			try
			{
				castObj = (Sysjsd)obj;
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
	
	#region Custom ICollection interface for Sysjsd 

	
	public interface ISysjsdCollection : ICollection
	{
		Sysjsd this[int index]{	get; set; }
		void Add(Sysjsd pSysjsd);
		void Clear();
	}
	
	[Serializable]
	public class SysjsdCollection : ISysjsdCollection
	{
		private IList<Sysjsd> _arrayInternal;

		public SysjsdCollection()
		{
			_arrayInternal = new List<Sysjsd>();
		}
		
		public SysjsdCollection( IList<Sysjsd> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<Sysjsd>();
			}
		}

		public Sysjsd this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((Sysjsd[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(Sysjsd pSysjsd) { _arrayInternal.Add(pSysjsd); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<Sysjsd> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
