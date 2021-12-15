
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region Alert

	/// <summary>
	/// Alert object for NHibernate mapped table 'Alert'.
	/// </summary>
	[Serializable]
	public class Alert
		{
		#region Member Variables
		
		protected int _alertID;
		protected string _reader;
		protected string _alertTitle;
		protected string _alertBody;
		protected DateTime _createdOn;
		protected string _createdBy;
		protected bool _hasRead;
		protected int _alertType;

		#endregion

		#region Constructors

		public Alert() { }

		public Alert( string reader, string alertTitle, string alertBody, DateTime createdOn, string createdBy, bool hasRead, int alertType )
		{
			this._reader = reader;
			this._alertTitle = alertTitle;
			this._alertBody = alertBody;
			this._createdOn = createdOn;
			this._createdBy = createdBy;
			this._hasRead = hasRead;
			this._alertType = alertType;
		}

		#endregion

		#region Public Properties

		public int AlertID
		{
			get {return _alertID;}
			set {_alertID = value;}
		}

		public string Reader
		{
			get { return _reader; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Reader", value, value.ToString());
				_reader = value;
			}
		}

		public string AlertTitle
		{
			get { return _alertTitle; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for AlertTitle", value, value.ToString());
				_alertTitle = value;
			}
		}

		public string AlertBody
		{
			get { return _alertBody; }
			set
			{
				_alertBody = value;
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

		public bool HasRead
		{
			get { return _hasRead; }
			set { _hasRead = value; }
		}

		public int AlertType
		{
			get { return _alertType; }
			set { _alertType = value; }
		}

        
		#endregion
		
        
	}

	#endregion
}
