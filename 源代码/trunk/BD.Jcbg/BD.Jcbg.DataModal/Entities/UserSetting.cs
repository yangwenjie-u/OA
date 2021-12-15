using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region UserSetting

	/// <summary>
	/// UserSetting object for NHibernate mapped table 'UserSetting'.
	/// </summary>
	[Serializable]
	public class UserSetting
	{
		#region Member Variables
		
		protected int _rECID;
		protected string _userName;
		protected string _settingId;
		protected string _settingValue;

		#endregion

		#region Constructors

		public UserSetting() { }

		public UserSetting( string userName, string settingId, string settingValue )
		{
			this._userName = userName;
			this._settingId = settingId;
			this._settingValue = settingValue;
		}

		#endregion

		#region Public Properties

		public int RECID
		{
			get {return _rECID;}
			set {_rECID = value;}
		}

		public string UserName
		{
			get { return _userName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for UserName", value, value.ToString());
				_userName = value;
			}
		}

		public string SettingId
		{
			get { return _settingId; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for SettingId", value, value.ToString());
				_settingId = value;
			}
		}

		public string SettingValue
		{
			get { return _settingValue; }
			set
			{
				if ( value != null && value.Length > 1000)
					throw new ArgumentOutOfRangeException("Invalid value for SettingValue", value, value.ToString());
				_settingValue = value;
			}
		}

		

		#endregion
	}
	#endregion
}