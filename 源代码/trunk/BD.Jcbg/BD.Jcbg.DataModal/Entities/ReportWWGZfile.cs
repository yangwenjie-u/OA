
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region ReportWWGZfile

	/// <summary>
	/// ReportWWGZfile object for NHibernate mapped table 'ReportWWGZ_file'.
	/// </summary>
	[Serializable]
	public class ReportWWGZfile
		{
		#region Member Variables
		
		protected int _fileID;
		protected string _fileName;
		protected string _saveName;
		protected int _wWGZid;
		protected byte[] _fILECONTENT;

		#endregion

		#region Constructors

		public ReportWWGZfile() { }

		public ReportWWGZfile( string fileName, string saveName, int wWGZid, byte[] fILECONTENT )
		{
			this._fileName = fileName;
			this._saveName = saveName;
			this._wWGZid = wWGZid;
			this._fILECONTENT = fILECONTENT;
		}

		#endregion

		#region Public Properties

		public int FileID
		{
			get {return _fileID;}
			set {_fileID = value;}
		}

		public string FileName
		{
			get { return _fileName; }
			set
			{
				if ( value != null && value.Length > 300)
					throw new ArgumentOutOfRangeException("Invalid value for FileName", value, value.ToString());
				_fileName = value;
			}
		}

		public string SaveName
		{
			get { return _saveName; }
			set
			{
				if ( value != null && value.Length > 300)
					throw new ArgumentOutOfRangeException("Invalid value for SaveName", value, value.ToString());
				_saveName = value;
			}
		}

		public int WWGZid
		{
			get { return _wWGZid; }
			set { _wWGZid = value; }
		}

		public byte[] FILECONTENT
		{
			get { return _fILECONTENT; }
			set { _fILECONTENT = value; }
		}

        
		#endregion
		
        
	}

	#endregion
}
