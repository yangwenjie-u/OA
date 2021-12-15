
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region SysYcdyTable

	/// <summary>
	/// SysYcdyTable object for NHibernate mapped table 'SysYcdyTable'.
	/// </summary>
	[Serializable]
	public class SysYcdyTable
		{
		#region Member Variables
		
		protected int _rECID;
		protected string _callId;
		protected string _localTableName;
		protected string _remoteTable;
		protected decimal _displayOrder;
		protected string _versionNo;
		protected string _updateField;
		protected string _tableDesc;
		protected bool _isJsonArray;
		protected string _relateCall;

		#endregion

		#region Constructors

		public SysYcdyTable() { }

		public SysYcdyTable( string callId, string localTableName, string remoteTable, decimal displayOrder, string versionNo, string updateField, string tableDesc, bool isJsonArray, string relateCall )
		{
			this._callId = callId;
			this._localTableName = localTableName;
			this._remoteTable = remoteTable;
			this._displayOrder = displayOrder;
			this._versionNo = versionNo;
			this._updateField = updateField;
			this._tableDesc = tableDesc;
			this._isJsonArray = isJsonArray;
			this._relateCall = relateCall;
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

		public string LocalTableName
		{
			get { return _localTableName; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for LocalTableName", value, value.ToString());
				_localTableName = value;
			}
		}

		public string RemoteTable
		{
			get { return _remoteTable; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for RemoteTable", value, value.ToString());
				_remoteTable = value;
			}
		}

		public decimal DisplayOrder
		{
			get { return _displayOrder; }
			set { _displayOrder = value; }
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

		public string UpdateField
		{
			get { return _updateField; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for UpdateField", value, value.ToString());
				_updateField = value;
			}
		}

		public string TableDesc
		{
			get { return _tableDesc; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for TableDesc", value, value.ToString());
				_tableDesc = value;
			}
		}

		public bool IsJsonArray
		{
			get { return _isJsonArray; }
			set { _isJsonArray = value; }
		}

		public string RelateCall
		{
			get { return _relateCall; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for RelateCall", value, value.ToString());
				_relateCall = value;
			}
		}

        
		#endregion
		
        
	}

	#endregion
}
