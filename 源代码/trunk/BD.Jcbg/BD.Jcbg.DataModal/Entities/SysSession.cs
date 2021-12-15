
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region SysSession

	/// <summary>
	/// SysSession object for NHibernate mapped table 'SysSession'.
	/// </summary>
    [Serializable]
	public class SysSession
		{
		#region Member Variables
		
		protected string _sessionId;
		protected DateTime _loginTime;
		protected string _userName;
		protected string _password;
		protected string _userCode;
		protected string _realName;

		#endregion

		#region Constructors

		public SysSession() { }

		public SysSession( DateTime loginTime, string userName, string password, string userCode, string realName )
		{
			this._loginTime = loginTime;
			this._userName = userName;
			this._password = password;
			this._userCode = userCode;
			this._realName = realName;
		}

		#endregion

		#region Public Properties

		public string SessionId
		{
			get {return _sessionId;}
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for SessionId", value, value.ToString());
				_sessionId = value;
			}
		}

		public DateTime LoginTime
		{
			get { return _loginTime; }
			set { _loginTime = value; }
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

		public string Password
		{
			get { return _password; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Password", value, value.ToString());
				_password = value;
			}
		}

		public string UserCode
		{
			get { return _userCode; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for UserCode", value, value.ToString());
				_userCode = value;
			}
		}

		public string RealName
		{
			get { return _realName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for RealName", value, value.ToString());
				_realName = value;
			}
		}

        
		#endregion
		
        
	}

	#endregion
}
