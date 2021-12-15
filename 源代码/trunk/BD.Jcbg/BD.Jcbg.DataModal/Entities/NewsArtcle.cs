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
	/// INewsArtcle interface for NHibernate mapped table 'News_artcle'.
	/// </summary>
	public interface INewsArtcle
	{
		#region Public Properties
		
		int Articleid
		{
			get ;
			set ;
			  
		}
		
		int Categoryid
		{
			get ;
			set ;
			  
		}
		
		int? Templateid
		{
			get ;
			set ;
			  
		}
		
		string ArticleTitle
		{
			get ;
			set ;
			  
		}
		
		string ArticleKey
		{
			get ;
			set ;
			  
		}
		
		string ArticleFrom
		{
			get ;
			set ;
			  
		}
		
		DateTime ArticleDate
		{
			get ;
			set ;
			  
		}
		
		string ArticleContent
		{
			get ;
			set ;
			  
		}
		
		bool IsImage
		{
			get ;
			set ;
			  
		}
		
		string ImageUrl
		{
			get ;
			set ;
			  
		}
		
		bool IsLink
		{
			get ;
			set ;
			  
		}
		
		string ArticleLink
		{
			get ;
			set ;
			  
		}
		
		bool? IsFile
		{
			get ;
			set ;
			  
		}
		
		string FileName
		{
			get ;
			set ;
			  
		}
		
		bool IsAudited
		{
			get ;
			set ;
			  
		}
		
		bool IsRecommand
		{
			get ;
			set ;
			  
		}
		
		int Hits
		{
			get ;
			set ;
			  
		}
		
		bool? IsImportant
		{
			get ;
			set ;
			  
		}
		
		string CreatedBy
		{
			get ;
			set ;
			  
		}
		
		DateTime? CreatedOn
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// NewsArtcle object for NHibernate mapped table 'News_artcle'.
	/// </summary>
	[Serializable]
	public class NewsArtcle : ICloneable,INewsArtcle
	{
		#region Member Variables

		protected int _articleid;
		protected int _categoryid;
		protected int? _templateid;
		protected string _articletitle;
		protected string _articlekey;
		protected string _articlefrom;
		protected DateTime _articledate;
		protected string _articlecontent;
		protected bool _isimage;
		protected string _imageurl;
		protected bool _islink;
		protected string _articlelink;
		protected bool? _isfile;
		protected string _filename;
		protected bool _isaudited;
		protected bool _isrecommand;
		protected int _hits;
		protected bool? _isimportant;
		protected string _createdby;
		protected DateTime? _createdon;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public NewsArtcle() {}
		
		public NewsArtcle(int pCategoryid, int? pTemplateid, string pArticleTitle, string pArticleKey, string pArticleFrom, DateTime pArticleDate, string pArticleContent, bool pIsImage, string pImageUrl, bool pIsLink, string pArticleLink, bool? pIsFile, string pFileName, bool pIsAudited, bool pIsRecommand, int pHits, bool? pIsImportant, string pCreatedBy, DateTime? pCreatedOn)
		{
			this._categoryid = pCategoryid; 
			this._templateid = pTemplateid; 
			this._articletitle = pArticleTitle; 
			this._articlekey = pArticleKey; 
			this._articlefrom = pArticleFrom; 
			this._articledate = pArticleDate; 
			this._articlecontent = pArticleContent; 
			this._isimage = pIsImage; 
			this._imageurl = pImageUrl; 
			this._islink = pIsLink; 
			this._articlelink = pArticleLink; 
			this._isfile = pIsFile; 
			this._filename = pFileName; 
			this._isaudited = pIsAudited; 
			this._isrecommand = pIsRecommand; 
			this._hits = pHits; 
			this._isimportant = pIsImportant; 
			this._createdby = pCreatedBy; 
			this._createdon = pCreatedOn; 
		}
		
		public NewsArtcle(int pCategoryid, string pArticleTitle, DateTime pArticleDate)
		{
			this._categoryid = pCategoryid; 
			this._articletitle = pArticleTitle; 
			this._articledate = pArticleDate; 
		}
		
		public NewsArtcle(int pArticleid)
		{
			this._articleid = pArticleid; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int Articleid
		{
			get { return _articleid; }
			set { _bIsChanged |= (_articleid != value); _articleid = value; }
			
		}
		
		public int Categoryid
		{
			get { return _categoryid; }
			set { _bIsChanged |= (_categoryid != value); _categoryid = value; }
			
		}
		
		public int? Templateid
		{
			get { return _templateid; }
			set { _bIsChanged |= (_templateid != value); _templateid = value; }
			
		}
		
		public string ArticleTitle
		{
			get { return _articletitle; }
			set 
			{
			  if (value != null && value.Length > 200)
			    throw new ArgumentOutOfRangeException("ArticleTitle", "ArticleTitle value, cannot contain more than 200 characters");
			  _bIsChanged |= (_articletitle != value); 
			  _articletitle = value; 
			}
			
		}
		
		public string ArticleKey
		{
			get { return _articlekey; }
			set 
			{
			  if (value != null && value.Length > 200)
			    throw new ArgumentOutOfRangeException("ArticleKey", "ArticleKey value, cannot contain more than 200 characters");
			  _bIsChanged |= (_articlekey != value); 
			  _articlekey = value; 
			}
			
		}
		
		public string ArticleFrom
		{
			get { return _articlefrom; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ArticleFrom", "ArticleFrom value, cannot contain more than 100 characters");
			  _bIsChanged |= (_articlefrom != value); 
			  _articlefrom = value; 
			}
			
		}
		
		public DateTime ArticleDate
		{
			get { return _articledate; }
			set { _bIsChanged |= (_articledate != value); _articledate = value; }
			
		}
		
		public string ArticleContent
		{
			get { return _articlecontent; }
			set 
			{
			  if (value != null && value.Length > 1073741823)
			    throw new ArgumentOutOfRangeException("ArticleContent", "ArticleContent value, cannot contain more than 1073741823 characters");
			  _bIsChanged |= (_articlecontent != value); 
			  _articlecontent = value; 
			}
			
		}
		
		public bool IsImage
		{
			get { return _isimage; }
			set { _bIsChanged |= (_isimage != value); _isimage = value; }
			
		}
		
		public string ImageUrl
		{
			get { return _imageurl; }
			set 
			{
			  if (value != null && value.Length > 300)
			    throw new ArgumentOutOfRangeException("ImageUrl", "ImageUrl value, cannot contain more than 300 characters");
			  _bIsChanged |= (_imageurl != value); 
			  _imageurl = value; 
			}
			
		}
		
		public bool IsLink
		{
			get { return _islink; }
			set { _bIsChanged |= (_islink != value); _islink = value; }
			
		}
		
		public string ArticleLink
		{
			get { return _articlelink; }
			set 
			{
			  if (value != null && value.Length > 300)
			    throw new ArgumentOutOfRangeException("ArticleLink", "ArticleLink value, cannot contain more than 300 characters");
			  _bIsChanged |= (_articlelink != value); 
			  _articlelink = value; 
			}
			
		}
		
		public bool? IsFile
		{
			get { return _isfile; }
			set { _bIsChanged |= (_isfile != value); _isfile = value; }
			
		}
		
		public string FileName
		{
			get { return _filename; }
			set 
			{
			  if (value != null && value.Length > 300)
			    throw new ArgumentOutOfRangeException("FileName", "FileName value, cannot contain more than 300 characters");
			  _bIsChanged |= (_filename != value); 
			  _filename = value; 
			}
			
		}
		
		public bool IsAudited
		{
			get { return _isaudited; }
			set { _bIsChanged |= (_isaudited != value); _isaudited = value; }
			
		}
		
		public bool IsRecommand
		{
			get { return _isrecommand; }
			set { _bIsChanged |= (_isrecommand != value); _isrecommand = value; }
			
		}
		
		public int Hits
		{
			get { return _hits; }
			set { _bIsChanged |= (_hits != value); _hits = value; }
			
		}
		
		public bool? IsImportant
		{
			get { return _isimportant; }
			set { _bIsChanged |= (_isimportant != value); _isimportant = value; }
			
		}
		
		public string CreatedBy
		{
			get { return _createdby; }
			set 
			{
			  if (value != null && value.Length > 50)
			    throw new ArgumentOutOfRangeException("CreatedBy", "CreatedBy value, cannot contain more than 50 characters");
			  _bIsChanged |= (_createdby != value); 
			  _createdby = value; 
			}
			
		}
		
		public DateTime? CreatedOn
		{
			get { return _createdon; }
			set { _bIsChanged |= (_createdon != value); _createdon = value; }
			
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
			NewsArtcle castObj = null;
			try
			{
				castObj = (NewsArtcle)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._articleid == castObj.Articleid );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _articleid.GetHashCode();
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
	
	#region Custom ICollection interface for NewsArtcle 

	
	public interface INewsArtcleCollection : ICollection
	{
		NewsArtcle this[int index]{	get; set; }
		void Add(NewsArtcle pNewsArtcle);
		void Clear();
	}
	
	[Serializable]
	public class NewsArtcleCollection : INewsArtcleCollection
	{
		private IList<NewsArtcle> _arrayInternal;

		public NewsArtcleCollection()
		{
			_arrayInternal = new List<NewsArtcle>();
		}
		
		public NewsArtcleCollection( IList<NewsArtcle> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<NewsArtcle>();
			}
		}

		public NewsArtcle this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((NewsArtcle[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(NewsArtcle pNewsArtcle) { _arrayInternal.Add(pNewsArtcle); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<NewsArtcle> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
