
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region SysYcdyParam

	/// <summary>
	/// SysYcdyParam object for NHibernate mapped table 'SysYcdyParam'.
	/// </summary>
    [Serializable]
	public class SysYcdyParam
		{
		#region Member Variables
		
		protected int _rECID;
		protected string _callId;
		protected string _paramName;
		protected string _paramDesc;
		protected string _ctrlType;
		protected string _ctrlString;
		protected string _versionNo;
		protected decimal _displayOrder;

		#endregion

		#region Constructors

		public SysYcdyParam() { }

		public SysYcdyParam( string callId, string paramName, string paramDesc, string ctrlType, string ctrlString, string versionNo, decimal displayOrder )
		{
			this._callId = callId;
			this._paramName = paramName;
			this._paramDesc = paramDesc;
			this._ctrlType = ctrlType;
			this._ctrlString = ctrlString;
			this._versionNo = versionNo;
			this._displayOrder = displayOrder;
		}

		#endregion

		#region Public Properties

		public int RECID
		{
			get {return _rECID;}
			set {_rECID = value;}
		}

		public string CallId
		{
			get { return _callId; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for CallId", value, value.ToString());
				_callId = value;
			}
		}

		public string ParamName
		{
			get { return _paramName; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for ParamName", value, value.ToString());
				_paramName = value;
			}
		}

		public string ParamDesc
		{
			get { return _paramDesc; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for ParamDesc", value, value.ToString());
				_paramDesc = value;
			}
		}

		public string CtrlType
		{
			get { return _ctrlType; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for CtrlType", value, value.ToString());
				_ctrlType = value;
			}
		}

		public string CtrlString
		{
			get { return _ctrlString; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for CtrlString", value, value.ToString());
				_ctrlString = value;
			}
		}

		public string VersionNo
		{
			get { return _versionNo; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for VersionNo", value, value.ToString());
				_versionNo = value;
			}
		}

		public decimal DisplayOrder
		{
			get { return _displayOrder; }
			set { _displayOrder = value; }
		}

        
		#endregion
		
        
	}

	#endregion
}
