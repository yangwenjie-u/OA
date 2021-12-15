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
	/// IBanHeZhanData interface for NHibernate mapped table 'BanHeZhanData'.
	/// </summary>
	public interface IBanHeZhanData
	{
		#region Public Properties
		
		int Id
		{
			get ;
			set ;
			  
		}
		
		string SheBeiBianHao
		{
			get ;
			set ;
			  
		}
		
		string GongDanHao
		{
			get ;
			set ;
			  
		}
		
		string ChaoZuoZhe
		{
			get ;
			set ;
			  
		}
		
		string SheJiFangLiang
		{
			get ;
			set ;
			  
		}
		
		string GuJiFangShu
		{
			get ;
			set ;
			  
		}
		
		string XiGuLiao1ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string XiGuLiao1LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string XiGuLiao2ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string XiGuLiao2LiLnZhi
		{
			get ;
			set ;
			  
		}
		
		string CuGuLiao1ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string CuGuLiao1LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string CuGuLiao2ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string CuGuLiao2LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string CuGuLiao3ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string CuGuLiao3LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string ShuiNi1ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string ShuiNi1LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string ShuiNi2ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string ShuiNi2LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string KuangFen3ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string KuangFen3LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string FenMeiHui4ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string FenMeiHui4LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string FenLiao5ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string FenLiao5LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string FenLiao6ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string FenLiao6LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string Shui1ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string Shui1LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string Shui2ShijiZhi
		{
			get ;
			set ;
			  
		}
		
		string Shui2LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string WaiJiaJi1ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string WaiJiaJi1LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string WaiJiaJi2ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string WaiJiaJi2LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string WaiJiaJi3ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string WaiJiaJi3LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string WaiJiaJi4ShiJiZhi
		{
			get ;
			set ;
			  
		}
		
		string WaiJiaJi4LiLunZhi
		{
			get ;
			set ;
			  
		}
		
		string ChuLiaoShiJian
		{
			get ;
			set ;
			  
		}
		
		string GongChengMingMheng
		{
			get ;
			set ;
			  
		}
		
		string SiGongDiDian
		{
			get ;
			set ;
			  
		}
		
		string JiaoZuoBuWei
		{
			get ;
			set ;
			  
		}
		
		string ShuiNiPingZhong
		{
			get ;
			set ;
			  
		}
		
		string PeiFangHao
		{
			get ;
			set ;
			  
		}
		
		string QiangDuDengJi
		{
			get ;
			set ;
			  
		}
		
		string JiaoBanShiJian
		{
			get ;
			set ;
			  
		}
		
		string BaoCunShiJian
		{
			get ;
			set ;
			  
		}
		
		string KeHuDuanBianhao
		{
			get ;
			set ;
			  
		}
		
		DateTime GetTime
		{
			get ;
			set ;
			  
		}
		
		int? SectionId
		{
			get ;
			set ;
			  
		}
		
		bool IsDeleted { get; set; }
		bool IsChanged { get; set; }
		
		#endregion 
	}

	/// <summary>
	/// BanHeZhanData object for NHibernate mapped table 'BanHeZhanData'.
	/// </summary>
	[Serializable]
	public class BanHeZhanData : ICloneable,IBanHeZhanData
	{
		#region Member Variables

		protected int _id;
		protected string _shebeibianhao;
		protected string _gongdanhao;
		protected string _chaozuozhe;
		protected string _shejifangliang;
		protected string _gujifangshu;
		protected string _xiguliao1shijizhi;
		protected string _xiguliao1lilunzhi;
		protected string _xiguliao2shijizhi;
		protected string _xiguliao2lilnzhi;
		protected string _cuguliao1shijizhi;
		protected string _cuguliao1lilunzhi;
		protected string _cuguliao2shijizhi;
		protected string _cuguliao2lilunzhi;
		protected string _cuguliao3shijizhi;
		protected string _cuguliao3lilunzhi;
		protected string _shuini1shijizhi;
		protected string _shuini1lilunzhi;
		protected string _shuini2shijizhi;
		protected string _shuini2lilunzhi;
		protected string _kuangfen3shijizhi;
		protected string _kuangfen3lilunzhi;
		protected string _fenmeihui4shijizhi;
		protected string _fenmeihui4lilunzhi;
		protected string _fenliao5shijizhi;
		protected string _fenliao5lilunzhi;
		protected string _fenliao6shijizhi;
		protected string _fenliao6lilunzhi;
		protected string _shui1shijizhi;
		protected string _shui1lilunzhi;
		protected string _shui2shijizhi;
		protected string _shui2lilunzhi;
		protected string _waijiaji1shijizhi;
		protected string _waijiaji1lilunzhi;
		protected string _waijiaji2shijizhi;
		protected string _waijiaji2lilunzhi;
		protected string _waijiaji3shijizhi;
		protected string _waijiaji3lilunzhi;
		protected string _waijiaji4shijizhi;
		protected string _waijiaji4lilunzhi;
		protected string _chuliaoshijian;
		protected string _gongchengmingmheng;
		protected string _sigongdidian;
		protected string _jiaozuobuwei;
		protected string _shuinipingzhong;
		protected string _peifanghao;
		protected string _qiangdudengji;
		protected string _jiaobanshijian;
		protected string _baocunshijian;
		protected string _kehuduanbianhao;
		protected DateTime _gettime;
		protected int? _sectionid;
		protected bool _bIsDeleted;
		protected bool _bIsChanged;
		#endregion
		
		#region Constructors
		public BanHeZhanData() {}
		
		public BanHeZhanData(string pSheBeiBianHao, string pGongDanHao, string pChaoZuoZhe, string pSheJiFangLiang, string pGuJiFangShu, string pXiGuLiao1ShiJiZhi, string pXiGuLiao1LiLunZhi, string pXiGuLiao2ShiJiZhi, string pXiGuLiao2LiLnZhi, string pCuGuLiao1ShiJiZhi, string pCuGuLiao1LiLunZhi, string pCuGuLiao2ShiJiZhi, string pCuGuLiao2LiLunZhi, string pCuGuLiao3ShiJiZhi, string pCuGuLiao3LiLunZhi, string pShuiNi1ShiJiZhi, string pShuiNi1LiLunZhi, string pShuiNi2ShiJiZhi, string pShuiNi2LiLunZhi, string pKuangFen3ShiJiZhi, string pKuangFen3LiLunZhi, string pFenMeiHui4ShiJiZhi, string pFenMeiHui4LiLunZhi, string pFenLiao5ShiJiZhi, string pFenLiao5LiLunZhi, string pFenLiao6ShiJiZhi, string pFenLiao6LiLunZhi, string pShui1ShiJiZhi, string pShui1LiLunZhi, string pShui2ShijiZhi, string pShui2LiLunZhi, string pWaiJiaJi1ShiJiZhi, string pWaiJiaJi1LiLunZhi, string pWaiJiaJi2ShiJiZhi, string pWaiJiaJi2LiLunZhi, string pWaiJiaJi3ShiJiZhi, string pWaiJiaJi3LiLunZhi, string pWaiJiaJi4ShiJiZhi, string pWaiJiaJi4LiLunZhi, string pChuLiaoShiJian, string pGongChengMingMheng, string pSiGongDiDian, string pJiaoZuoBuWei, string pShuiNiPingZhong, string pPeiFangHao, string pQiangDuDengJi, string pJiaoBanShiJian, string pBaoCunShiJian, string pKeHuDuanBianhao, DateTime pGetTime, int? pSectionId)
		{
			this._shebeibianhao = pSheBeiBianHao; 
			this._gongdanhao = pGongDanHao; 
			this._chaozuozhe = pChaoZuoZhe; 
			this._shejifangliang = pSheJiFangLiang; 
			this._gujifangshu = pGuJiFangShu; 
			this._xiguliao1shijizhi = pXiGuLiao1ShiJiZhi; 
			this._xiguliao1lilunzhi = pXiGuLiao1LiLunZhi; 
			this._xiguliao2shijizhi = pXiGuLiao2ShiJiZhi; 
			this._xiguliao2lilnzhi = pXiGuLiao2LiLnZhi; 
			this._cuguliao1shijizhi = pCuGuLiao1ShiJiZhi; 
			this._cuguliao1lilunzhi = pCuGuLiao1LiLunZhi; 
			this._cuguliao2shijizhi = pCuGuLiao2ShiJiZhi; 
			this._cuguliao2lilunzhi = pCuGuLiao2LiLunZhi; 
			this._cuguliao3shijizhi = pCuGuLiao3ShiJiZhi; 
			this._cuguliao3lilunzhi = pCuGuLiao3LiLunZhi; 
			this._shuini1shijizhi = pShuiNi1ShiJiZhi; 
			this._shuini1lilunzhi = pShuiNi1LiLunZhi; 
			this._shuini2shijizhi = pShuiNi2ShiJiZhi; 
			this._shuini2lilunzhi = pShuiNi2LiLunZhi; 
			this._kuangfen3shijizhi = pKuangFen3ShiJiZhi; 
			this._kuangfen3lilunzhi = pKuangFen3LiLunZhi; 
			this._fenmeihui4shijizhi = pFenMeiHui4ShiJiZhi; 
			this._fenmeihui4lilunzhi = pFenMeiHui4LiLunZhi; 
			this._fenliao5shijizhi = pFenLiao5ShiJiZhi; 
			this._fenliao5lilunzhi = pFenLiao5LiLunZhi; 
			this._fenliao6shijizhi = pFenLiao6ShiJiZhi; 
			this._fenliao6lilunzhi = pFenLiao6LiLunZhi; 
			this._shui1shijizhi = pShui1ShiJiZhi; 
			this._shui1lilunzhi = pShui1LiLunZhi; 
			this._shui2shijizhi = pShui2ShijiZhi; 
			this._shui2lilunzhi = pShui2LiLunZhi; 
			this._waijiaji1shijizhi = pWaiJiaJi1ShiJiZhi; 
			this._waijiaji1lilunzhi = pWaiJiaJi1LiLunZhi; 
			this._waijiaji2shijizhi = pWaiJiaJi2ShiJiZhi; 
			this._waijiaji2lilunzhi = pWaiJiaJi2LiLunZhi; 
			this._waijiaji3shijizhi = pWaiJiaJi3ShiJiZhi; 
			this._waijiaji3lilunzhi = pWaiJiaJi3LiLunZhi; 
			this._waijiaji4shijizhi = pWaiJiaJi4ShiJiZhi; 
			this._waijiaji4lilunzhi = pWaiJiaJi4LiLunZhi; 
			this._chuliaoshijian = pChuLiaoShiJian; 
			this._gongchengmingmheng = pGongChengMingMheng; 
			this._sigongdidian = pSiGongDiDian; 
			this._jiaozuobuwei = pJiaoZuoBuWei; 
			this._shuinipingzhong = pShuiNiPingZhong; 
			this._peifanghao = pPeiFangHao; 
			this._qiangdudengji = pQiangDuDengJi; 
			this._jiaobanshijian = pJiaoBanShiJian; 
			this._baocunshijian = pBaoCunShiJian; 
			this._kehuduanbianhao = pKeHuDuanBianhao; 
			this._gettime = pGetTime; 
			this._sectionid = pSectionId; 
		}
		
		public BanHeZhanData(int pId)
		{
			this._id = pId; 
		}
		
		#endregion
		
		#region Public Properties
		
		public int Id
		{
			get { return _id; }
			set { _bIsChanged |= (_id != value); _id = value; }
			
		}
		
		public string SheBeiBianHao
		{
			get { return _shebeibianhao; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("SheBeiBianHao", "SheBeiBianHao value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shebeibianhao != value); 
			  _shebeibianhao = value; 
			}
			
		}
		
		public string GongDanHao
		{
			get { return _gongdanhao; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("GongDanHao", "GongDanHao value, cannot contain more than 100 characters");
			  _bIsChanged |= (_gongdanhao != value); 
			  _gongdanhao = value; 
			}
			
		}
		
		public string ChaoZuoZhe
		{
			get { return _chaozuozhe; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ChaoZuoZhe", "ChaoZuoZhe value, cannot contain more than 100 characters");
			  _bIsChanged |= (_chaozuozhe != value); 
			  _chaozuozhe = value; 
			}
			
		}
		
		public string SheJiFangLiang
		{
			get { return _shejifangliang; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("SheJiFangLiang", "SheJiFangLiang value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shejifangliang != value); 
			  _shejifangliang = value; 
			}
			
		}
		
		public string GuJiFangShu
		{
			get { return _gujifangshu; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("GuJiFangShu", "GuJiFangShu value, cannot contain more than 100 characters");
			  _bIsChanged |= (_gujifangshu != value); 
			  _gujifangshu = value; 
			}
			
		}
		
		public string XiGuLiao1ShiJiZhi
		{
			get { return _xiguliao1shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("XiGuLiao1ShiJiZhi", "XiGuLiao1ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_xiguliao1shijizhi != value); 
			  _xiguliao1shijizhi = value; 
			}
			
		}
		
		public string XiGuLiao1LiLunZhi
		{
			get { return _xiguliao1lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("XiGuLiao1LiLunZhi", "XiGuLiao1LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_xiguliao1lilunzhi != value); 
			  _xiguliao1lilunzhi = value; 
			}
			
		}
		
		public string XiGuLiao2ShiJiZhi
		{
			get { return _xiguliao2shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("XiGuLiao2ShiJiZhi", "XiGuLiao2ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_xiguliao2shijizhi != value); 
			  _xiguliao2shijizhi = value; 
			}
			
		}
		
		public string XiGuLiao2LiLnZhi
		{
			get { return _xiguliao2lilnzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("XiGuLiao2LiLnZhi", "XiGuLiao2LiLnZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_xiguliao2lilnzhi != value); 
			  _xiguliao2lilnzhi = value; 
			}
			
		}
		
		public string CuGuLiao1ShiJiZhi
		{
			get { return _cuguliao1shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("CuGuLiao1ShiJiZhi", "CuGuLiao1ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_cuguliao1shijizhi != value); 
			  _cuguliao1shijizhi = value; 
			}
			
		}
		
		public string CuGuLiao1LiLunZhi
		{
			get { return _cuguliao1lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("CuGuLiao1LiLunZhi", "CuGuLiao1LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_cuguliao1lilunzhi != value); 
			  _cuguliao1lilunzhi = value; 
			}
			
		}
		
		public string CuGuLiao2ShiJiZhi
		{
			get { return _cuguliao2shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("CuGuLiao2ShiJiZhi", "CuGuLiao2ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_cuguliao2shijizhi != value); 
			  _cuguliao2shijizhi = value; 
			}
			
		}
		
		public string CuGuLiao2LiLunZhi
		{
			get { return _cuguliao2lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("CuGuLiao2LiLunZhi", "CuGuLiao2LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_cuguliao2lilunzhi != value); 
			  _cuguliao2lilunzhi = value; 
			}
			
		}
		
		public string CuGuLiao3ShiJiZhi
		{
			get { return _cuguliao3shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("CuGuLiao3ShiJiZhi", "CuGuLiao3ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_cuguliao3shijizhi != value); 
			  _cuguliao3shijizhi = value; 
			}
			
		}
		
		public string CuGuLiao3LiLunZhi
		{
			get { return _cuguliao3lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("CuGuLiao3LiLunZhi", "CuGuLiao3LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_cuguliao3lilunzhi != value); 
			  _cuguliao3lilunzhi = value; 
			}
			
		}
		
		public string ShuiNi1ShiJiZhi
		{
			get { return _shuini1shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ShuiNi1ShiJiZhi", "ShuiNi1ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shuini1shijizhi != value); 
			  _shuini1shijizhi = value; 
			}
			
		}
		
		public string ShuiNi1LiLunZhi
		{
			get { return _shuini1lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ShuiNi1LiLunZhi", "ShuiNi1LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shuini1lilunzhi != value); 
			  _shuini1lilunzhi = value; 
			}
			
		}
		
		public string ShuiNi2ShiJiZhi
		{
			get { return _shuini2shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ShuiNi2ShiJiZhi", "ShuiNi2ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shuini2shijizhi != value); 
			  _shuini2shijizhi = value; 
			}
			
		}
		
		public string ShuiNi2LiLunZhi
		{
			get { return _shuini2lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ShuiNi2LiLunZhi", "ShuiNi2LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shuini2lilunzhi != value); 
			  _shuini2lilunzhi = value; 
			}
			
		}
		
		public string KuangFen3ShiJiZhi
		{
			get { return _kuangfen3shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("KuangFen3ShiJiZhi", "KuangFen3ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_kuangfen3shijizhi != value); 
			  _kuangfen3shijizhi = value; 
			}
			
		}
		
		public string KuangFen3LiLunZhi
		{
			get { return _kuangfen3lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("KuangFen3LiLunZhi", "KuangFen3LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_kuangfen3lilunzhi != value); 
			  _kuangfen3lilunzhi = value; 
			}
			
		}
		
		public string FenMeiHui4ShiJiZhi
		{
			get { return _fenmeihui4shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("FenMeiHui4ShiJiZhi", "FenMeiHui4ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_fenmeihui4shijizhi != value); 
			  _fenmeihui4shijizhi = value; 
			}
			
		}
		
		public string FenMeiHui4LiLunZhi
		{
			get { return _fenmeihui4lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("FenMeiHui4LiLunZhi", "FenMeiHui4LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_fenmeihui4lilunzhi != value); 
			  _fenmeihui4lilunzhi = value; 
			}
			
		}
		
		public string FenLiao5ShiJiZhi
		{
			get { return _fenliao5shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("FenLiao5ShiJiZhi", "FenLiao5ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_fenliao5shijizhi != value); 
			  _fenliao5shijizhi = value; 
			}
			
		}
		
		public string FenLiao5LiLunZhi
		{
			get { return _fenliao5lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("FenLiao5LiLunZhi", "FenLiao5LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_fenliao5lilunzhi != value); 
			  _fenliao5lilunzhi = value; 
			}
			
		}
		
		public string FenLiao6ShiJiZhi
		{
			get { return _fenliao6shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("FenLiao6ShiJiZhi", "FenLiao6ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_fenliao6shijizhi != value); 
			  _fenliao6shijizhi = value; 
			}
			
		}
		
		public string FenLiao6LiLunZhi
		{
			get { return _fenliao6lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("FenLiao6LiLunZhi", "FenLiao6LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_fenliao6lilunzhi != value); 
			  _fenliao6lilunzhi = value; 
			}
			
		}
		
		public string Shui1ShiJiZhi
		{
			get { return _shui1shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("Shui1ShiJiZhi", "Shui1ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shui1shijizhi != value); 
			  _shui1shijizhi = value; 
			}
			
		}
		
		public string Shui1LiLunZhi
		{
			get { return _shui1lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("Shui1LiLunZhi", "Shui1LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shui1lilunzhi != value); 
			  _shui1lilunzhi = value; 
			}
			
		}
		
		public string Shui2ShijiZhi
		{
			get { return _shui2shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("Shui2ShijiZhi", "Shui2ShijiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shui2shijizhi != value); 
			  _shui2shijizhi = value; 
			}
			
		}
		
		public string Shui2LiLunZhi
		{
			get { return _shui2lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("Shui2LiLunZhi", "Shui2LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shui2lilunzhi != value); 
			  _shui2lilunzhi = value; 
			}
			
		}
		
		public string WaiJiaJi1ShiJiZhi
		{
			get { return _waijiaji1shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("WaiJiaJi1ShiJiZhi", "WaiJiaJi1ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_waijiaji1shijizhi != value); 
			  _waijiaji1shijizhi = value; 
			}
			
		}
		
		public string WaiJiaJi1LiLunZhi
		{
			get { return _waijiaji1lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("WaiJiaJi1LiLunZhi", "WaiJiaJi1LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_waijiaji1lilunzhi != value); 
			  _waijiaji1lilunzhi = value; 
			}
			
		}
		
		public string WaiJiaJi2ShiJiZhi
		{
			get { return _waijiaji2shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("WaiJiaJi2ShiJiZhi", "WaiJiaJi2ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_waijiaji2shijizhi != value); 
			  _waijiaji2shijizhi = value; 
			}
			
		}
		
		public string WaiJiaJi2LiLunZhi
		{
			get { return _waijiaji2lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("WaiJiaJi2LiLunZhi", "WaiJiaJi2LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_waijiaji2lilunzhi != value); 
			  _waijiaji2lilunzhi = value; 
			}
			
		}
		
		public string WaiJiaJi3ShiJiZhi
		{
			get { return _waijiaji3shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("WaiJiaJi3ShiJiZhi", "WaiJiaJi3ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_waijiaji3shijizhi != value); 
			  _waijiaji3shijizhi = value; 
			}
			
		}
		
		public string WaiJiaJi3LiLunZhi
		{
			get { return _waijiaji3lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("WaiJiaJi3LiLunZhi", "WaiJiaJi3LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_waijiaji3lilunzhi != value); 
			  _waijiaji3lilunzhi = value; 
			}
			
		}
		
		public string WaiJiaJi4ShiJiZhi
		{
			get { return _waijiaji4shijizhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("WaiJiaJi4ShiJiZhi", "WaiJiaJi4ShiJiZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_waijiaji4shijizhi != value); 
			  _waijiaji4shijizhi = value; 
			}
			
		}
		
		public string WaiJiaJi4LiLunZhi
		{
			get { return _waijiaji4lilunzhi; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("WaiJiaJi4LiLunZhi", "WaiJiaJi4LiLunZhi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_waijiaji4lilunzhi != value); 
			  _waijiaji4lilunzhi = value; 
			}
			
		}
		
		public string ChuLiaoShiJian
		{
			get { return _chuliaoshijian; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ChuLiaoShiJian", "ChuLiaoShiJian value, cannot contain more than 100 characters");
			  _bIsChanged |= (_chuliaoshijian != value); 
			  _chuliaoshijian = value; 
			}
			
		}
		
		public string GongChengMingMheng
		{
			get { return _gongchengmingmheng; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("GongChengMingMheng", "GongChengMingMheng value, cannot contain more than 100 characters");
			  _bIsChanged |= (_gongchengmingmheng != value); 
			  _gongchengmingmheng = value; 
			}
			
		}
		
		public string SiGongDiDian
		{
			get { return _sigongdidian; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("SiGongDiDian", "SiGongDiDian value, cannot contain more than 100 characters");
			  _bIsChanged |= (_sigongdidian != value); 
			  _sigongdidian = value; 
			}
			
		}
		
		public string JiaoZuoBuWei
		{
			get { return _jiaozuobuwei; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("JiaoZuoBuWei", "JiaoZuoBuWei value, cannot contain more than 100 characters");
			  _bIsChanged |= (_jiaozuobuwei != value); 
			  _jiaozuobuwei = value; 
			}
			
		}
		
		public string ShuiNiPingZhong
		{
			get { return _shuinipingzhong; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("ShuiNiPingZhong", "ShuiNiPingZhong value, cannot contain more than 100 characters");
			  _bIsChanged |= (_shuinipingzhong != value); 
			  _shuinipingzhong = value; 
			}
			
		}
		
		public string PeiFangHao
		{
			get { return _peifanghao; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("PeiFangHao", "PeiFangHao value, cannot contain more than 100 characters");
			  _bIsChanged |= (_peifanghao != value); 
			  _peifanghao = value; 
			}
			
		}
		
		public string QiangDuDengJi
		{
			get { return _qiangdudengji; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("QiangDuDengJi", "QiangDuDengJi value, cannot contain more than 100 characters");
			  _bIsChanged |= (_qiangdudengji != value); 
			  _qiangdudengji = value; 
			}
			
		}
		
		public string JiaoBanShiJian
		{
			get { return _jiaobanshijian; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("JiaoBanShiJian", "JiaoBanShiJian value, cannot contain more than 100 characters");
			  _bIsChanged |= (_jiaobanshijian != value); 
			  _jiaobanshijian = value; 
			}
			
		}
		
		public string BaoCunShiJian
		{
			get { return _baocunshijian; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("BaoCunShiJian", "BaoCunShiJian value, cannot contain more than 100 characters");
			  _bIsChanged |= (_baocunshijian != value); 
			  _baocunshijian = value; 
			}
			
		}
		
		public string KeHuDuanBianhao
		{
			get { return _kehuduanbianhao; }
			set 
			{
			  if (value != null && value.Length > 100)
			    throw new ArgumentOutOfRangeException("KeHuDuanBianhao", "KeHuDuanBianhao value, cannot contain more than 100 characters");
			  _bIsChanged |= (_kehuduanbianhao != value); 
			  _kehuduanbianhao = value; 
			}
			
		}
		
		public DateTime GetTime
		{
			get { return _gettime; }
			set { _bIsChanged |= (_gettime != value); _gettime = value; }
			
		}
		
		public int? SectionId
		{
			get { return _sectionid; }
			set { _bIsChanged |= (_sectionid != value); _sectionid = value; }
			
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
			BanHeZhanData castObj = null;
			try
			{
				castObj = (BanHeZhanData)obj;
			} catch(Exception) { return false; } 
			return ( castObj != null ) &&
				( this._id == castObj.Id );
		}
		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
		  
			
			int hash = 57; 
			hash = 27 * hash * _id.GetHashCode();
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
	
	#region Custom ICollection interface for BanHeZhanData 

	
	public interface IBanHeZhanDataCollection : ICollection
	{
		BanHeZhanData this[int index]{	get; set; }
		void Add(BanHeZhanData pBanHeZhanData);
		void Clear();
	}
	
	[Serializable]
	public class BanHeZhanDataCollection : IBanHeZhanDataCollection
	{
		private IList<BanHeZhanData> _arrayInternal;

		public BanHeZhanDataCollection()
		{
			_arrayInternal = new List<BanHeZhanData>();
		}
		
		public BanHeZhanDataCollection( IList<BanHeZhanData> pSource )
		{
			_arrayInternal = pSource;
			if(_arrayInternal == null)
			{
				_arrayInternal = new List<BanHeZhanData>();
			}
		}

		public BanHeZhanData this[int index]
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
		public void CopyTo(Array array, int index){ _arrayInternal.CopyTo((BanHeZhanData[])array, index); }
		public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
		public void Add(BanHeZhanData pBanHeZhanData) { _arrayInternal.Add(pBanHeZhanData); }
		public void Clear() { _arrayInternal.Clear(); }
		public IList<BanHeZhanData> GetList() { return _arrayInternal; }
	 }
	
	#endregion
}
