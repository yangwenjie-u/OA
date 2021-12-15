
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region SysYcdyTableRelation

	/// <summary>
	/// SysYcdyTableRelation object for NHibernate mapped table 'SysYcdyTableRelation'.
	/// </summary>
    [Serializable]
	public class SysYcdyTableRelation
		{
		#region Member Variables
		
		protected int _rECID;
		protected string _callId;
		protected string _localTableName;
		protected string _localParentTable;
		protected string _localPrimaryKey;
		protected string _remotePrimaryKey;
		protected string _localForeignKey;
		protected string _remoteForeignKey;
		protected string _versionNo;

		#endregion

		#region Constructors

		public SysYcdyTableRelation() { }

		public SysYcdyTableRelation( string callId, string localTableName, string localParentTable, string localPrimaryKey, string remotePrimaryKey, string localForeignKey, string remoteForeignKey, string versionNo )
		{
			this._callId = callId;
			this._localTableName = localTableName;
			this._localParentTable = localParentTable;
			this._localPrimaryKey = localPrimaryKey;
			this._remotePrimaryKey = remotePrimaryKey;
			this._localForeignKey = localForeignKey;
			this._remoteForeignKey = remoteForeignKey;
			this._versionNo = versionNo;
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
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LocalTableName", value, value.ToString());
				_localTableName = value;
			}
		}

		public string LocalParentTable
		{
			get { return _localParentTable; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LocalParentTable", value, value.ToString());
				_localParentTable = value;
			}
		}

		public string LocalPrimaryKey
		{
			get { return _localPrimaryKey; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LocalPrimaryKey", value, value.ToString());
				_localPrimaryKey = value;
			}
		}

		public string RemotePrimaryKey
		{
			get { return _remotePrimaryKey; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for RemotePrimaryKey", value, value.ToString());
				_remotePrimaryKey = value;
			}
		}

		public string LocalForeignKey
		{
			get { return _localForeignKey; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LocalForeignKey", value, value.ToString());
				_localForeignKey = value;
			}
		}

		public string RemoteForeignKey
		{
			get { return _remoteForeignKey; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for RemoteForeignKey", value, value.ToString());
				_remoteForeignKey = value;
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

        
		#endregion
		
        
	}

	#endregion
}
