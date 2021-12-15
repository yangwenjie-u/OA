
using System;
using System.Collections;

namespace BD.Jcbg.DataModal.Entities
{
	#region GclrWxyFile

	/// <summary>
	/// GclrWxyFile object for NHibernate mapped table 'GCLR_WXY_File'.
	/// </summary>
	[Serializable]
	public class GclrWxyFile
		{
		#region Member Variables
		
		protected int _fileID;
		protected string _fileName;
		protected string _saveName;
		protected int _wXYid;
		protected byte[] _fILECONTENT;

		#endregion

		#region Constructors

		public GclrWxyFile() { }

		public GclrWxyFile( string fileName, string saveName, int wXYid, byte[] fILECONTENT )
		{
			this._fileName = fileName;
			this._saveName = saveName;
			this._wXYid = wXYid;
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

		public int WXYid
		{
			get { return _wXYid; }
			set { _wXYid = value; }
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
