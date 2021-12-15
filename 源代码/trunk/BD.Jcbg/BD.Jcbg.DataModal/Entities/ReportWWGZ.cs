
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region ReportWWGZ

	/// <summary>
	/// ReportWWGZ object for NHibernate mapped table 'ReportWWGZ'.
	/// </summary>
	[Serializable]
	public class ReportWWGZ
		{
		#region Member Variables
		
		protected int _rECID;
		protected string _jDZCH;
		protected string _dWMC;
		protected string _gCMC;
		protected string _jDY;
		protected DateTime _createdOn;
		protected string _createdBy;
		protected DateTime _jDDate;
		protected string _jDNR;
		protected int _isconfirm;

		#endregion

		#region Constructors

		public ReportWWGZ() { }

		public ReportWWGZ( string jDZCH, string dWMC, string gCMC, string jDY, DateTime createdOn, string createdBy, DateTime jDDate, string jDNR, int isconfirm )
		{
			this._jDZCH = jDZCH;
			this._dWMC = dWMC;
			this._gCMC = gCMC;
			this._jDY = jDY;
			this._createdOn = createdOn;
			this._createdBy = createdBy;
			this._jDDate = jDDate;
			this._jDNR = jDNR;
			this._isconfirm = isconfirm;
		}

		#endregion

		#region Public Properties

		public int RECID
		{
			get {return _rECID;}
			set {_rECID = value;}
		}

		public string JDZCH
		{
			get { return _jDZCH; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for JDZCH", value, value.ToString());
				_jDZCH = value;
			}
		}

		public string DWMC
		{
			get { return _dWMC; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for DWMC", value, value.ToString());
				_dWMC = value;
			}
		}

		public string GCMC
		{
			get { return _gCMC; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for GCMC", value, value.ToString());
				_gCMC = value;
			}
		}

		public string JDY
		{
			get { return _jDY; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for JDY", value, value.ToString());
				_jDY = value;
			}
		}

		public DateTime CreatedOn
		{
			get { return _createdOn; }
			set { _createdOn = value; }
		}

		public string CreatedBy
		{
			get { return _createdBy; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for CreatedBy", value, value.ToString());
				_createdBy = value;
			}
		}

		public DateTime JDDate
		{
			get { return _jDDate; }
			set { _jDDate = value; }
		}

		public string JDNR
		{
			get { return _jDNR; }
			set
			{
				_jDNR = value;
			}
		}

		public int Isconfirm
		{
			get { return _isconfirm; }
			set { _isconfirm = value; }
		}

        
		#endregion
		
        
	}

	#endregion
}
