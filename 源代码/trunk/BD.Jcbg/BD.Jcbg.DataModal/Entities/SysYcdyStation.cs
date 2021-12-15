
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region SysYcdyStation

	/// <summary>
	/// SysYcdyStation object for NHibernate mapped table 'SysYcdyStation'.
	/// </summary>
	[Serializable]
	public class SysYcdyStation
		{
		#region Member Variables
		
		protected int _stationId;
		protected string _stationName;
		protected string _rootUrl;
		protected string _versionNo;
		protected bool _autoSync;

		#endregion

		#region Constructors

		public SysYcdyStation() { }

		public SysYcdyStation( string stationName, string rootUrl, string versionNo, bool autoSync )
		{
			this._stationName = stationName;
			this._rootUrl = rootUrl;
			this._versionNo = versionNo;
			this._autoSync = autoSync;
		}

		#endregion

		#region Public Properties

		public int StationId
		{
			get {return _stationId;}
			set {_stationId = value;}
		}

		public string StationName
		{
			get { return _stationName; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for StationName", value, value.ToString());
				_stationName = value;
			}
		}

		public string RootUrl
		{
			get { return _rootUrl; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for RootUrl", value, value.ToString());
				_rootUrl = value;
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

		public bool AutoSync
		{
			get { return _autoSync; }
			set { _autoSync = value; }
		}

        
		#endregion
		
        
	}

	#endregion
}
