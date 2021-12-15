
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region SysYcdyUrl

	/// <summary>
	/// SysYcdyUrl object for NHibernate mapped table 'SysYcdyUrl'.
	/// </summary>
    [Serializable]
	public class SysYcdyUrl
		{
		#region Member Variables
		
		protected int _rECID;
		protected string _urlPath;
		protected string _callId;
		protected string _callDesc;
		protected bool _inUse;
		protected string _versionNo;
		protected int _pageSize;
		protected string _totalProperty;
		protected string _dataRoot;
		protected string _downAllUrl;
		protected string _downFileUrl;

		#endregion

		#region Constructors

		public SysYcdyUrl() { }

		public SysYcdyUrl( string urlPath, string callId, string callDesc, bool inUse, string versionNo, int pageSize, string totalProperty, string dataRoot, string downAllUrl, string downFileUrl )
		{
			this._urlPath = urlPath;
			this._callId = callId;
			this._callDesc = callDesc;
			this._inUse = inUse;
			this._versionNo = versionNo;
			this._pageSize = pageSize;
			this._totalProperty = totalProperty;
			this._dataRoot = dataRoot;
			this._downAllUrl = downAllUrl;
			this._downFileUrl = downFileUrl;
		}

		#endregion

		#region Public Properties

		public int RECID
		{
			get {return _rECID;}
			set {_rECID = value;}
		}

		public string UrlPath
		{
			get { return _urlPath; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for UrlPath", value, value.ToString());
				_urlPath = value;
			}
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

		public string CallDesc
		{
			get { return _callDesc; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for CallDesc", value, value.ToString());
				_callDesc = value;
			}
		}

		public bool InUse
		{
			get { return _inUse; }
			set { _inUse = value; }
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

		public int PageSize
		{
			get { return _pageSize; }
			set { _pageSize = value; }
		}

		public string TotalProperty
		{
			get { return _totalProperty; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for TotalProperty", value, value.ToString());
				_totalProperty = value;
			}
		}

		public string DataRoot
		{
			get { return _dataRoot; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for DataRoot", value, value.ToString());
				_dataRoot = value;
			}
		}

		public string DownAllUrl
		{
			get { return _downAllUrl; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for DownAllUrl", value, value.ToString());
				_downAllUrl = value;
			}
		}

		public string DownFileUrl
		{
			get { return _downFileUrl; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for DownFileUrl", value, value.ToString());
				_downFileUrl = value;
			}
		}

        
		#endregion
		
        
	}

	#endregion
}
