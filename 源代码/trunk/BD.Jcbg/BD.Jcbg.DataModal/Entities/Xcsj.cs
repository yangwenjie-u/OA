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
	/// IXcsj interface for NHibernate mapped table 'XCSJ'.
	/// </summary>
	public interface IXcsj
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
		
		string Commsylb
		{
			get ;
			set ;
			  
		}
		
		string Sy
		{
			get ;
			set ;
			  
		}
		
		string Clysrlb
		{
			get ;
			set ;
			  
		}
		
		string Syxz
		{
			get ;
			set ;
			  
		}
		
		string Manual
		{
			get ;
			set ;
			  
		}
		
		string Csxms1
		{
			get ;
			set ;
			  
		}
		
		string Csxms2
		{
			get ;
			set ;
			  
		}
		
		string Csxms3
		{
			get ;
			set ;
			  
		}
		
		string Dotpos
		{
			get ;
			set ;
			  
		}
		
		string Maxzh
		{
			get ;
			set ;
			  
		}
		
		decimal? Longth
		{
			get ;
			set ;
			  
		}
		
		decimal? Width
		{
			get ;
			set ;
			  
		}
		
		string Printqu
		{
			get ;
			set ;
			  
		}
		
		double MinPercent
		{
			get ;
			set ;
			  
		}
		
		double MaxPercent
		{
			get ;
			set ;
			  
		}
		
		int? EquationTime
		{
			get ;
			set ;
			  
		}
		
		string SpecField
		{
			get ;
			set ;
			  
		}
		
		string Bz
		{
			get ;
			set ;
			  
		}
		
		string Qmtj
		{
			get ;
			set ;
			  
		}
		
		int? PortNum
		{
			get ;
			set ;
			  
		}
		
		string UnitName
		{
			get ;
			set ;
			  
		}
		
		bool? Fhrqm
		{
			get ;
			set ;
			  
		}
		
		bool? Sdks
		{
			get ;
			set ;
			  
		}
		
		bool? IsCsxm
		{
			get ;
			set ;
			  
		}
		
		string Csylb
		{
			get ;
			set ;
			  
		}
		
		bool? Wjsbc
		{
			get ;
			set ;
			  
		}
		
		string Cameras
		{
			get ;
			set ;
			  
		}
		
		short? Yport
		{
			get ;
			set ;
			  
		}
		
		short? Xport
		{
			get ;
			set ;
			  
		}
		
		short? ColType
		{
			get ;
			set ;
			  
		}
		
		short? ScanTool
		{
			get ;
			set ;
			  
		}
		
		bool? IsSingleCol
		{
			get ;
			set ;
			  
		}
		
		bool? UseConstant
		{
			get ;
			set ;
			  
		}
		
		string Syzb
		{
			get ;
			set ;
			  
		}
		
		short? Sfkl
		{
			get ;
			set ;
			  
		}
		
		string Lq
		{
			get ;
			set ;
			  
		}
		
		string ScanSerialType
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// Xcsj object for NHibernate mapped table 'XCSJ'.
	/// </summary>
	[Serializable]
	public class Xcsj : ICloneable,IXcsj
	{
		#region Member Variables

		protected int _recid;
		protected string _depcode;
		protected string _commsylb;
		protected string _sy;
		protected string _clysrlb;
		protected string _syxz;
		protected string _manual;
		protected string _csxms1;
		protected string _csxms2;
		protected string _csxms3;
		protected string _dotpos;
		protected string _maxzh;
		protected decimal? _longth;
		protected decimal? _width;
		protected string _printqu;
		protected double _minpercent;
		protected double _maxpercent;
		protected int? _equationtime;
		protected string _specfield;
		protected string _bz;
		protected string _qmtj;
		protected int? _portnum;
		protected string _unitname;
		protected bool? _fhrqm;
		protected bool? _sdks;
		protected bool? _iscsxm;
		protected string _csylb;
		protected bool? _wjsbc;
		protected string _cameras;
		protected short? _yport;
		protected short? _xport;
		protected short? _coltype;
		protected short? _scantool;
		protected bool? _issinglecol;
		protected bool? _useconstant;
		protected string _syzb;
		protected short? _sfkl;
		protected string _lq;
		protected string _scanserialtype;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public Xcsj() {}
		
		public Xcsj(string pDepCode, string pCommsylb, string pSy, string pClysrlb, string pSyxz, string pManual, string pCsxms1, string pCsxms2, string pCsxms3, string pDotpos, string pMaxzh, decimal? pLongth, decimal? pWidth, string pPrintqu, double pMinPercent, double pMaxPercent, int? pEquationTime, string pSpecField, string pBz, string pQmtj, int? pPortNum, string pUnitName, bool? pFhrqm, bool? pSdks, bool? pIsCsxm, string pCsylb, bool? pWjsbc, string pCameras, short? pYport, short? pXport, short? pColType, short? pScanTool, bool? pIsSingleCol, bool? pUseConstant, string pSyzb, short? pSfkl, string pLq, string pScanSerialType)
		{
			this._depcode = pDepCode; 
			this._commsylb = pCommsylb; 
			this._sy = pSy; 
			this._clysrlb = pClysrlb; 
			this._syxz = pSyxz; 
			this._manual = pManual; 
			this._csxms1 = pCsxms1; 
			this._csxms2 = pCsxms2; 
			this._csxms3 = pCsxms3; 
			this._dotpos = pDotpos; 
			this._maxzh = pMaxzh; 
			this._longth = pLongth; 
			this._width = pWidth; 
			this._printqu = pPrintqu; 
			this._minpercent = pMinPercent; 
			this._maxpercent = pMaxPercent; 
			this._equationtime = pEquationTime; 
			this._specfield = pSpecField; 
			this._bz = pBz; 
			this._qmtj = pQmtj; 
			this._portnum = pPortNum; 
			this._unitname = pUnitName; 
			this._fhrqm = pFhrqm; 
			this._sdks = pSdks; 
			this._iscsxm = pIsCsxm; 
			this._csylb = pCsylb; 
			this._wjsbc = pWjsbc; 
			this._cameras = pCameras; 
			this._yport = pYport; 
			this._xport = pXport; 
			this._coltype = pColType; 
			this._scantool = pScanTool; 
			this._issinglecol = pIsSingleCol; 
			this._useconstant = pUseConstant; 
			this._syzb = pSyzb; 
			this._sfkl = pSfkl; 
			this._lq = pLq; 
			this._scanserialtype = pScanSerialType; 
		}
		
		public Xcsj(int pRecid)
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
		
		public virtual string Sy
		{
			get { return _sy; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Sy", "Sy value, cannot contain more than 50 characters");
			  _bIsChanged |= (_sy != value); 
			  _sy = value; 
			}
			
		}
		
		public virtual string Clysrlb
		{
			get { return _clysrlb; }
			set 
			{
			  if (value != null && value.Length > 2)
			    throw new ArgumentOutOfRangeException("Clysrlb", "Clysrlb value, cannot contain more than 2 characters");
			  _bIsChanged |= (_clysrlb != value); 
			  _clysrlb = value; 
			}
			
		}
		
		public virtual string Syxz
		{
			get { return _syxz; }
			set 
			{
			  if (value != null && value.Length > 2)
			    throw new ArgumentOutOfRangeException("Syxz", "Syxz value, cannot contain more than 2 characters");
			  _bIsChanged |= (_syxz != value); 
			  _syxz = value; 
			}
			
		}
		
		public virtual string Manual
		{
			get { return _manual; }
			set 
			{
			  if (value != null && value.Length > 1)
			    throw new ArgumentOutOfRangeException("Manual", "Manual value, cannot contain more than 1 characters");
			  _bIsChanged |= (_manual != value); 
			  _manual = value; 
			}
			
		}
		
		public virtual string Csxms1
		{
			get { return _csxms1; }
			set 
			{
			  if (value != null && value.Length > 2)
			    throw new ArgumentOutOfRangeException("Csxms1", "Csxms1 value, cannot contain more than 2 characters");
			  _bIsChanged |= (_csxms1 != value); 
			  _csxms1 = value; 
			}
			
		}
		
		public virtual string Csxms2
		{
			get { return _csxms2; }
			set 
			{
			  if (value != null && value.Length > 2)
			    throw new ArgumentOutOfRangeException("Csxms2", "Csxms2 value, cannot contain more than 2 characters");
			  _bIsChanged |= (_csxms2 != value); 
			  _csxms2 = value; 
			}
			
		}
		
		public virtual string Csxms3
		{
			get { return _csxms3; }
			set 
			{
			  if (value != null && value.Length > 2)
			    throw new ArgumentOutOfRangeException("Csxms3", "Csxms3 value, cannot contain more than 2 characters");
			  _bIsChanged |= (_csxms3 != value); 
			  _csxms3 = value; 
			}
			
		}
		
		public virtual string Dotpos
		{
			get { return _dotpos; }
			set 
			{
			  if (value != null && value.Length > 2)
			    throw new ArgumentOutOfRangeException("Dotpos", "Dotpos value, cannot contain more than 2 characters");
			  _bIsChanged |= (_dotpos != value); 
			  _dotpos = value; 
			}
			
		}
		
		public virtual string Maxzh
		{
			get { return _maxzh; }
			set 
			{
			  if (value != null && value.Length > 1)
			    throw new ArgumentOutOfRangeException("Maxzh", "Maxzh value, cannot contain more than 1 characters");
			  _bIsChanged |= (_maxzh != value); 
			  _maxzh = value; 
			}
			
		}
		
		public virtual decimal? Longth
		{
			get { return _longth; }
			set { _bIsChanged |= (_longth != value); _longth = value; }
			
		}
		
		public virtual decimal? Width
		{
			get { return _width; }
			set { _bIsChanged |= (_width != value); _width = value; }
			
		}
		
		public virtual string Printqu
		{
			get { return _printqu; }
			set 
			{
			  if (value != null && value.Length > 2)
			    throw new ArgumentOutOfRangeException("Printqu", "Printqu value, cannot contain more than 2 characters");
			  _bIsChanged |= (_printqu != value); 
			  _printqu = value; 
			}
			
		}
		
		public virtual double MinPercent
		{
			get { return _minpercent; }
			set { _bIsChanged |= (_minpercent != value); _minpercent = value; }
			
		}
		
		public virtual double MaxPercent
		{
			get { return _maxpercent; }
			set { _bIsChanged |= (_maxpercent != value); _maxpercent = value; }
			
		}
		
		public virtual int? EquationTime
		{
			get { return _equationtime; }
			set { _bIsChanged |= (_equationtime != value); _equationtime = value; }
			
		}
		
		public virtual string SpecField
		{
			get { return _specfield; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("SpecField", "SpecField value, cannot contain more than 100 characters");
			  _bIsChanged |= (_specfield != value); 
			  _specfield = value; 
			}
			
		}
		
		public virtual string Bz
		{
			get { return _bz; }
			set 
			{
			  if (value != null && value.Length > 2000)
			    throw new ArgumentOutOfRangeException("Bz", "Bz value, cannot contain more than 2000 characters");
			  _bIsChanged |= (_bz != value); 
			  _bz = value; 
			}
			
		}
		
		public virtual string Qmtj
		{
			get { return _qmtj; }
			set 
			{
			  if (value != null && value.Length > 1000)
			    throw new ArgumentOutOfRangeException("Qmtj", "Qmtj value, cannot contain more than 1000 characters");
			  _bIsChanged |= (_qmtj != value); 
			  _qmtj = value; 
			}
			
		}
		
		public virtual int? PortNum
		{
			get { return _portnum; }
			set { _bIsChanged |= (_portnum != value); _portnum = value; }
			
		}
		
		public virtual string UnitName
		{
			get { return _unitname; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("UnitName", "UnitName value, cannot contain more than 50 characters");
			  _bIsChanged |= (_unitname != value); 
			  _unitname = value; 
			}
			
		}
		
		public virtual bool? Fhrqm
		{
			get { return _fhrqm; }
			set { _bIsChanged |= (_fhrqm != value); _fhrqm = value; }
			
		}
		
		public virtual bool? Sdks
		{
			get { return _sdks; }
			set { _bIsChanged |= (_sdks != value); _sdks = value; }
			
		}
		
		public virtual bool? IsCsxm
		{
			get { return _iscsxm; }
			set { _bIsChanged |= (_iscsxm != value); _iscsxm = value; }
			
		}
		
		public virtual string Csylb
		{
			get { return _csylb; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Csylb", "Csylb value, cannot contain more than 50 characters");
			  _bIsChanged |= (_csylb != value); 
			  _csylb = value; 
			}
			
		}
		
		public virtual bool? Wjsbc
		{
			get { return _wjsbc; }
			set { _bIsChanged |= (_wjsbc != value); _wjsbc = value; }
			
		}
		
		public virtual string Cameras
		{
			get { return _cameras; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Cameras", "Cameras value, cannot contain more than 50 characters");
			  _bIsChanged |= (_cameras != value); 
			  _cameras = value; 
			}
			
		}
		
		public virtual short? Yport
		{
			get { return _yport; }
			set { _bIsChanged |= (_yport != value); _yport = value; }
			
		}
		
		public virtual short? Xport
		{
			get { return _xport; }
			set { _bIsChanged |= (_xport != value); _xport = value; }
			
		}
		
		public virtual short? ColType
		{
			get { return _coltype; }
			set { _bIsChanged |= (_coltype != value); _coltype = value; }
			
		}
		
		public virtual short? ScanTool
		{
			get { return _scantool; }
			set { _bIsChanged |= (_scantool != value); _scantool = value; }
			
		}
		
		public virtual bool? IsSingleCol
		{
			get { return _issinglecol; }
			set { _bIsChanged |= (_issinglecol != value); _issinglecol = value; }
			
		}
		
		public virtual bool? UseConstant
		{
			get { return _useconstant; }
			set { _bIsChanged |= (_useconstant != value); _useconstant = value; }
			
		}
		
		public virtual string Syzb
		{
			get { return _syzb; }
			set 
			{
			  if (value != null && value.Length > 200)
			    throw new ArgumentOutOfRangeException("Syzb", "Syzb value, cannot contain more than 200 characters");
			  _bIsChanged |= (_syzb != value); 
			  _syzb = value; 
			}
			
		}
		
		public virtual short? Sfkl
		{
			get { return _sfkl; }
			set { _bIsChanged |= (_sfkl != value); _sfkl = value; }
			
		}
		
		public virtual string Lq
		{
			get { return _lq; }
			set 
			{
			  if (value != null && value.Length > 10)
			    throw new ArgumentOutOfRangeException("Lq", "Lq value, cannot contain more than 10 characters");
			  _bIsChanged |= (_lq != value); 
			  _lq = value; 
			}
			
		}
		
		public virtual string ScanSerialType
		{
			get { return _scanserialtype; }
			set 
			{
			  if (value != null && value.Length > 1)
			    throw new ArgumentOutOfRangeException("ScanSerialType", "ScanSerialType value, cannot contain more than 1 characters");
			  _bIsChanged |= (_scanserialtype != value); 
			  _scanserialtype = value; 
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
			Xcsj castObj = null;
			try
			{
				castObj = (Xcsj)obj;
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
	
	#region Custom ICollection interface for Xcsj 

	
	public interface IXcsjCollection : ICollection
	{
		Xcsj this[int index]{	get; set; }
		void Add(Xcsj pXcsj);
		void Clear();
	}
	
	[Serializable]
	public class XcsjCollection : IXcsjCollection
	{
		private IList<Xcsj> _arrayInternal;

		public XcsjCollection()
		{
			_arrayInternal = new List<Xcsj>();
		}
		
		public XcsjCollection( IList<Xcsj> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<Xcsj>();
			}
		}

		public Xcsj this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((Xcsj[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(Xcsj pXcsj) { _arrayInternal.Add(pXcsj); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<Xcsj> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
