
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region SysYcdyField

	/// <summary>
	/// SysYcdyField object for NHibernate mapped table 'SysYcdyField'.
	/// </summary>
    [Serializable]
	public class SysYcdyField
		{
		#region Member Variables
		
		protected int _rECID;
		protected string _callId;
		protected string _tableName;
		protected string _remoteField;
		protected string _localField;
		protected string _fieldDesc;
		protected bool _listShow;
		protected bool _detailShow;
		protected decimal _listDisplayOrder;
		protected decimal _detailDisplayOrder;
		protected int _subTableFieldType;
		protected int _listWidth;
		protected bool _detailFullRow;
		protected string _versionNo;
		protected string _listAlign;
		protected bool _isFile;

		#endregion

		#region Constructors

		public SysYcdyField() { }

		public SysYcdyField( string callId, string tableName, string remoteField, string localField, string fieldDesc, bool listShow, bool detailShow, decimal listDisplayOrder, decimal detailDisplayOrder, int subTableFieldType, int listWidth, bool detailFullRow, string versionNo, string listAlign, bool isFile )
		{
			this._callId = callId;
			this._tableName = tableName;
			this._remoteField = remoteField;
			this._localField = localField;
			this._fieldDesc = fieldDesc;
			this._listShow = listShow;
			this._detailShow = detailShow;
			this._listDisplayOrder = listDisplayOrder;
			this._detailDisplayOrder = detailDisplayOrder;
			this._subTableFieldType = subTableFieldType;
			this._listWidth = listWidth;
			this._detailFullRow = detailFullRow;
			this._versionNo = versionNo;
			this._listAlign = listAlign;
			this._isFile = isFile;
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

		public string TableName
		{
			get { return _tableName; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for TableName", value, value.ToString());
				_tableName = value;
			}
		}

		public string RemoteField
		{
			get { return _remoteField; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for RemoteField", value, value.ToString());
				_remoteField = value;
			}
		}

		public string LocalField
		{
			get { return _localField; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for LocalField", value, value.ToString());
				_localField = value;
			}
		}

		public string FieldDesc
		{
			get { return _fieldDesc; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for FieldDesc", value, value.ToString());
				_fieldDesc = value;
			}
		}

		public bool ListShow
		{
			get { return _listShow; }
			set { _listShow = value; }
		}

		public bool DetailShow
		{
			get { return _detailShow; }
			set { _detailShow = value; }
		}

		public decimal ListDisplayOrder
		{
			get { return _listDisplayOrder; }
			set { _listDisplayOrder = value; }
		}

		public decimal DetailDisplayOrder
		{
			get { return _detailDisplayOrder; }
			set { _detailDisplayOrder = value; }
		}

		public int SubTableFieldType
		{
			get { return _subTableFieldType; }
			set { _subTableFieldType = value; }
		}

		public int ListWidth
		{
			get { return _listWidth; }
			set { _listWidth = value; }
		}

		public bool DetailFullRow
		{
			get { return _detailFullRow; }
			set { _detailFullRow = value; }
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

		public string ListAlign
		{
			get { return _listAlign; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for ListAlign", value, value.ToString());
				_listAlign = value;
			}
		}

		public bool IsFile
		{
			get { return _isFile; }
			set { _isFile = value; }
		}

        
		#endregion
		
        
	}

	#endregion
}
