using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region PRMWJ

	/// <summary>
	/// PRMWJ object for NHibernate mapped table 'PR_M_WJ'.
	/// </summary>
	[Serializable]
	public class PRMWJ
	{
		#region Member Variables
		
		protected int _rECID;
		protected string _wJLX;
		protected string _sSDWBH;
		protected string _sYXMBH;
		protected string _gJZ1;
		protected string _wJM;
		protected byte[] _wJNR;
		protected string _lRRZH;
		protected string _lRRXM;
		protected DateTime _lRSJ;
		protected string _gJZ2;

		#endregion

		#region Constructors

		public PRMWJ() { }

		public PRMWJ( string wJLX, string sSDWBH, string sYXMBH, string gJZ1, string wJM, byte[] wJNR, string lRRZH, string lRRXM, DateTime lRSJ, string gJZ2 )
		{
			this._wJLX = wJLX;
			this._sSDWBH = sSDWBH;
			this._sYXMBH = sYXMBH;
			this._gJZ1 = gJZ1;
			this._wJM = wJM;
			this._wJNR = wJNR;
			this._lRRZH = lRRZH;
			this._lRRXM = lRRXM;
			this._lRSJ = lRSJ;
			this._gJZ2 = gJZ2;
		}

		#endregion

		#region Public Properties

		public int RECID
		{
			get {return _rECID;}
			set {_rECID = value;}
		}

		public string WJLX
		{
			get { return _wJLX; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for WJLX", value, value.ToString());
				_wJLX = value;
			}
		}

		public string SSDWBH
		{
			get { return _sSDWBH; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for SSDWBH", value, value.ToString());
				_sSDWBH = value;
			}
		}

		public string SYXMBH
		{
			get { return _sYXMBH; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for SYXMBH", value, value.ToString());
				_sYXMBH = value;
			}
		}

		public string GJZ1
		{
			get { return _gJZ1; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for GJZ1", value, value.ToString());
				_gJZ1 = value;
			}
		}

		public string WJM
		{
			get { return _wJM; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for WJM", value, value.ToString());
				_wJM = value;
			}
		}

		public byte[] WJNR
		{
			get { return _wJNR; }
			set { _wJNR = value; }
		}

		public string LRRZH
		{
			get { return _lRRZH; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LRRZH", value, value.ToString());
				_lRRZH = value;
			}
		}

		public string LRRXM
		{
			get { return _lRRXM; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for LRRXM", value, value.ToString());
				_lRRXM = value;
			}
		}

		public DateTime LRSJ
		{
			get { return _lRSJ; }
			set { _lRSJ = value; }
		}

		public string GJZ2
		{
			get { return _gJZ2; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for GJZ2", value, value.ToString());
				_gJZ2 = value;
			}
		}

		

		#endregion
	}
	#endregion
}