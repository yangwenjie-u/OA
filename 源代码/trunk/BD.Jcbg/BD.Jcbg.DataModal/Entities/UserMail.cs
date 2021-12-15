/*
using MyGeneration/Template/NHibernate (c) by Sharp 1.4
based on OHM (alvy77@hotmail.com)
*/
using System;
using System.Collections;
using System.Collections.Generic;

namespace BD.Jcbg.DataModal.Entities
{

	/// <summary>
	/// IUserMail interface for NHibernate mapped table 'UserMail'.
	/// </summary>
	public interface IUserMail
	{
		#region Public Properties
		
		int Recid
		{
			get ;
			set ;
			  
		}
		
		string Sender
		{
			get ;
			set ;
			  
		}
		
		string SenderRealName
		{
			get ;
			set ;
			  
		}
		
		string Receiver
		{
			get ;
			set ;
			  
		}
		
		string ReceiverRealName
		{
			get ;
			set ;
			  
		}
		
		int? Folderid
		{
			get ;
			set ;
			  
		}
		
		string Title
		{
			get ;
			set ;
			  
		}
		
		string Content
		{
			get ;
			set ;
			  
		}
		
		DateTime? SendTime
		{
			get ;
			set ;
			  
		}
		
		int? ContentSize
		{
			get ;
			set ;
			  
		}
		
		bool? HasSend
		{
			get ;
			set ;
			  
		}
		
		bool? HasDelete
		{
			get ;
			set ;
			  
		}
		
		string FileIds
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// UserMail object for NHibernate mapped table 'UserMail'.
	/// </summary>
	[Serializable]
	public class UserMail : ICloneable,IUserMail
	{
		#region Member Variables

		protected int _recid;
		protected string _sender;
		protected string _senderrealname;
		protected string _receiver;
		protected string _receiverrealname;
		protected int? _folderid;
		protected string _title;
		protected string _content;
		protected DateTime? _sendtime;
		protected int? _contentsize;
		protected bool? _hassend;
		protected bool? _hasdelete;
		protected string _fileids;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public UserMail() {}
		
		public UserMail(string pSender, string pSenderRealName, string pReceiver, string pReceiverRealName, int? pFolderid, string pTitle, string pContent, DateTime? pSendTime, int? pContentSize, bool? pHasSend, bool? pHasDelete, string pFileIds)
		{
			this._sender = pSender; 
			this._senderrealname = pSenderRealName; 
			this._receiver = pReceiver; 
			this._receiverrealname = pReceiverRealName; 
			this._folderid = pFolderid; 
			this._title = pTitle; 
			this._content = pContent; 
			this._sendtime = pSendTime; 
			this._contentsize = pContentSize; 
			this._hassend = pHasSend; 
			this._hasdelete = pHasDelete; 
			this._fileids = pFileIds; 
		}
		
		public UserMail(int pRecid)
		{
			this._recid = pRecid; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int Recid
		{
			get { return _recid; }
			set { _bIsChanged |= (_recid != value); _recid = value; }
			
		}
		
		public string Sender
		{
			get { return _sender; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("Sender", "Sender value, cannot contain more than 50 characters");
			  _bIsChanged |= (_sender != value); 
			  _sender = value; 
			}
			
		}
		
		public string SenderRealName
		{
			get { return _senderrealname; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("SenderRealName", "SenderRealName value, cannot contain more than 50 characters");
			  _bIsChanged |= (_senderrealname != value); 
			  _senderrealname = value; 
			}
			
		}
		
		public string Receiver
		{
			get { return _receiver; }
			set 
			{
			  if (value != null && value.Length > 2000)
			    throw new ArgumentOutOfRangeException("Receiver", "Receiver value, cannot contain more than 2000 characters");
			  _bIsChanged |= (_receiver != value); 
			  _receiver = value; 
			}
			
		}
		
		public string ReceiverRealName
		{
			get { return _receiverrealname; }
			set 
			{
			  if (value != null && value.Length > 2000)
			    throw new ArgumentOutOfRangeException("ReceiverRealName", "ReceiverRealName value, cannot contain more than 2000 characters");
			  _bIsChanged |= (_receiverrealname != value); 
			  _receiverrealname = value; 
			}
			
		}
		
		public int? Folderid
		{
			get { return _folderid; }
			set { _bIsChanged |= (_folderid != value); _folderid = value; }
			
		}
		
		public string Title
		{
			get { return _title; }
			set 
			{
			  if (value != null && value.Length > 400)
			    throw new ArgumentOutOfRangeException("Title", "Title value, cannot contain more than 400 characters");
			  _bIsChanged |= (_title != value); 
			  _title = value; 
			}
			
		}
		
		public string Content
		{
			get { return _content; }
			set 
			{
			  if (value != null && value.Length > 2147483647)
			    throw new ArgumentOutOfRangeException("Content", "Content value, cannot contain more than 2147483647 characters");
			  _bIsChanged |= (_content != value); 
			  _content = value; 
			}
			
		}
		
		public DateTime? SendTime
		{
			get { return _sendtime; }
			set { _bIsChanged |= (_sendtime != value); _sendtime = value; }
			
		}
		
		public int? ContentSize
		{
			get { return _contentsize; }
			set { _bIsChanged |= (_contentsize != value); _contentsize = value; }
			
		}
		
		public bool? HasSend
		{
			get { return _hassend; }
			set { _bIsChanged |= (_hassend != value); _hassend = value; }
			
		}
		
		public bool? HasDelete
		{
			get { return _hasdelete; }
			set { _bIsChanged |= (_hasdelete != value); _hasdelete = value; }
			
		}
		
		public string FileIds
		{
			get { return _fileids; }
			set 
			{
			  if (value != null && value.Length > 2000)
			    throw new ArgumentOutOfRangeException("FileIds", "FileIds value, cannot contain more than 2000 characters");
			  _bIsChanged |= (_fileids != value); 
			  _fileids = value; 
			}
			
		}
		

		public bool IsDeleted
		{
			get
			{
				return _bIsDeleted;
			}
			set
			{
				_bIsDeleted = value;
			}
		}
		
		public bool IsChanged
		{
			get
			{
				return _bIsChanged;
			}
			set
			{
				_bIsChanged = value;
			}
		}
		
		#endregion 
		
		#region Equals And HashCode Overrides
		/// <summary>
		/// local implementation of Equals based on unique value members
		/// </summary>
		public override bool Equals( object obj )
		{
			if( this == obj ) return true;
			UserMail castObj = null;
			try
			{
				castObj = (UserMail)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._recid == castObj.Recid );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _recid.GetHashCode();
			return hash; 
		}
		#endregion
		
		#region ICloneable methods
		
		public object Clone()
		{
			return this.MemberwiseClone();
		}
		
		#endregion
	}
	
	#region Custom ICollection interface for UserMail 

	
	public interface IUserMailCollection : ICollection
	{
		UserMail this[int index]{	get; set; }
		void Add(UserMail pUserMail);
		void Clear();
	}
	
	[Serializable]
	public class UserMailCollection : IUserMailCollection
	{
		private IList<UserMail> _arrayInternal;

		public UserMailCollection()
		{
			_arrayInternal = new List<UserMail>();
		}
		
		public UserMailCollection( IList<UserMail> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<UserMail>();
			}
		}

		public UserMail this[int index]
		{
			get
			{
				return _arrayInternal[index];
			}
			set
			{
				_arrayInternal[index] = value;
			}
		}

		public int Count { get { return _arrayInternal.Count; } }
		public bool IsSynchronized { get { return false; } }
		public object SyncRoot { get { return _arrayInternal; } }
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((UserMail[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(UserMail pUserMail) { _arrayInternal.Add(pUserMail); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<UserMail> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
