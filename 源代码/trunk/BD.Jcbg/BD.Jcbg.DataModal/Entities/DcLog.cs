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
    /// IDcLog interface for NHibernate mapped table 'DCLog'.
    /// </summary>
    public interface IDcLog
    {
        #region Public Properties

        int Recid
        {
            get;
            set;

        }

        string DepCode
        {
            get;
            set;

        }

        string Csylb
        {
            get;
            set;

        }

        decimal? Nsylb
        {
            get;
            set;

        }

        decimal? Xz
        {
            get;
            set;

        }

        string Wtbh
        {
            get;
            set;

        }

        string Zh
        {
            get;
            set;

        }

        string Syr
        {
            get;
            set;

        }

        string Syfhr
        {
            get;
            set;

        }

        DateTime? Syrq
        {
            get;
            set;

        }

        decimal? Qycs
        {
            get;
            set;

        }

        string Sbmcbh
        {
            get;
            set;

        }

        string Beizhu
        {
            get;
            set;

        }

        bool? Sfwc
        {
            get;
            set;

        }

        string Sysj1
        {
            get;
            set;

        }

        string Sysj2
        {
            get;
            set;

        }

        string Sysj3
        {
            get;
            set;

        }

        string Sysj4
        {
            get;
            set;

        }

        string Sysj5
        {
            get;
            set;

        }

        string Sysj6
        {
            get;
            set;

        }

        string Sysj7
        {
            get;
            set;

        }

        string Sysj8
        {
            get;
            set;

        }

        string Sysj9
        {
            get;
            set;

        }

        string Sysj10
        {
            get;
            set;

        }

        string Sysj11
        {
            get;
            set;

        }

        string Sysj12
        {
            get;
            set;

        }

        string Sysj13
        {
            get;
            set;

        }

        string Sysj14
        {
            get;
            set;

        }

        string Sysj15
        {
            get;
            set;

        }

        string Sysj16
        {
            get;
            set;

        }

        string Sysj17
        {
            get;
            set;

        }

        string Sysj18
        {
            get;
            set;

        }

        string Sysj19
        {
            get;
            set;

        }

        string Sysj20
        {
            get;
            set;

        }

        string Sysj21
        {
            get;
            set;

        }

        string Sysj22
        {
            get;
            set;

        }

        string Sysj23
        {
            get;
            set;

        }

        string Sysj24
        {
            get;
            set;

        }

        string Sysj25
        {
            get;
            set;

        }

        string Sysj26
        {
            get;
            set;

        }

        string Sysj27
        {
            get;
            set;

        }

        string Sysj28
        {
            get;
            set;

        }

        string Sysj29
        {
            get;
            set;

        }

        string Sysj30
        {
            get;
            set;

        }

        string Sysj31
        {
            get;
            set;

        }

        string Sysj32
        {
            get;
            set;

        }

        string Sysj33
        {
            get;
            set;

        }

        string Sysj34
        {
            get;
            set;

        }

        string Sysj35
        {
            get;
            set;

        }

        string Sysj36
        {
            get;
            set;

        }

        string Sysj37
        {
            get;
            set;

        }

        string Sysj38
        {
            get;
            set;

        }

        string Sysj39
        {
            get;
            set;

        }

        string Sysj40
        {
            get;
            set;

        }

        string Sysj41
        {
            get;
            set;

        }

        string Sysj42
        {
            get;
            set;

        }

        string Sysj43
        {
            get;
            set;

        }

        string Sysj44
        {
            get;
            set;

        }

        string Sysj45
        {
            get;
            set;

        }

        string Sysj46
        {
            get;
            set;

        }

        string Sysj47
        {
            get;
            set;

        }

        string Sysj48
        {
            get;
            set;

        }

        string Sysj49
        {
            get;
            set;

        }

        string Sysj50
        {
            get;
            set;

        }

        string Sysj51
        {
            get;
            set;

        }

        string Sysj52
        {
            get;
            set;

        }

        string Sysj53
        {
            get;
            set;

        }

        string Sysj54
        {
            get;
            set;

        }

        string Sysj55
        {
            get;
            set;

        }

        string Sysj56
        {
            get;
            set;

        }

        string Sysj57
        {
            get;
            set;

        }

        string Sysj58
        {
            get;
            set;

        }

        string Sysj59
        {
            get;
            set;

        }

        string Sysj60
        {
            get;
            set;

        }

        string DataList
        {
            get;
            set;

        }

        string UniqCode
        {
            get;
            set;

        }

        string VideoFile
        {
            get;
            set;

        }

        DateTime? Syksrq
        {
            get;
            set;

        }

        bool IsDeleted { get; set; }
        bool IsChanged { get; set; }

        #endregion
    }

    /// <summary>
    /// DcLog object for NHibernate mapped table 'DCLog'.
    /// </summary>
    [Serializable]
    public class DcLog : ICloneable, IDcLog
    {
        #region Member Variables

        protected int _recid;
        protected string _depcode;
        protected string _csylb;
        protected decimal? _nsylb;
        protected decimal? _xz;
        protected string _wtbh;
        protected string _zh;
        protected string _syr;
        protected string _syfhr;
        protected DateTime? _syrq;
        protected decimal? _qycs;
        protected string _sbmcbh;
        protected string _beizhu;
        protected bool? _sfwc;
        protected string _sysj1;
        protected string _sysj2;
        protected string _sysj3;
        protected string _sysj4;
        protected string _sysj5;
        protected string _sysj6;
        protected string _sysj7;
        protected string _sysj8;
        protected string _sysj9;
        protected string _sysj10;
        protected string _sysj11;
        protected string _sysj12;
        protected string _sysj13;
        protected string _sysj14;
        protected string _sysj15;
        protected string _sysj16;
        protected string _sysj17;
        protected string _sysj18;
        protected string _sysj19;
        protected string _sysj20;
        protected string _sysj21;
        protected string _sysj22;
        protected string _sysj23;
        protected string _sysj24;
        protected string _sysj25;
        protected string _sysj26;
        protected string _sysj27;
        protected string _sysj28;
        protected string _sysj29;
        protected string _sysj30;
        protected string _sysj31;
        protected string _sysj32;
        protected string _sysj33;
        protected string _sysj34;
        protected string _sysj35;
        protected string _sysj36;
        protected string _sysj37;
        protected string _sysj38;
        protected string _sysj39;
        protected string _sysj40;
        protected string _sysj41;
        protected string _sysj42;
        protected string _sysj43;
        protected string _sysj44;
        protected string _sysj45;
        protected string _sysj46;
        protected string _sysj47;
        protected string _sysj48;
        protected string _sysj49;
        protected string _sysj50;
        protected string _sysj51;
        protected string _sysj52;
        protected string _sysj53;
        protected string _sysj54;
        protected string _sysj55;
        protected string _sysj56;
        protected string _sysj57;
        protected string _sysj58;
        protected string _sysj59;
        protected string _sysj60;
        protected string _datalist;
        protected string _uniqcode;
        protected string _videofile;
        protected DateTime? _syksrq;
        protected bool _bIsDeleted;
        protected bool _bIsChanged;
        #endregion

        #region Constructors
        public DcLog() { }

        public DcLog(string pDepCode, string pCsylb, decimal? pNsylb, decimal? pXz, string pWtbh, string pZh, string pSyr, string pSyfhr, DateTime? pSyrq, decimal? pQycs, string pSbmcbh, string pBeizhu, bool? pSfwc, string pSysj1, string pSysj2, string pSysj3, string pSysj4, string pSysj5, string pSysj6, string pSysj7, string pSysj8, string pSysj9, string pSysj10, string pSysj11, string pSysj12, string pSysj13, string pSysj14, string pSysj15, string pSysj16, string pSysj17, string pSysj18, string pSysj19, string pSysj20, string pSysj21, string pSysj22, string pSysj23, string pSysj24, string pSysj25, string pSysj26, string pSysj27, string pSysj28, string pSysj29, string pSysj30, string pSysj31, string pSysj32, string pSysj33, string pSysj34, string pSysj35, string pSysj36, string pSysj37, string pSysj38, string pSysj39, string pSysj40, string pSysj41, string pSysj42, string pSysj43, string pSysj44, string pSysj45, string pSysj46, string pSysj47, string pSysj48, string pSysj49, string pSysj50, string pSysj51, string pSysj52, string pSysj53, string pSysj54, string pSysj55, string pSysj56, string pSysj57, string pSysj58, string pSysj59, string pSysj60, string pDataList, string pUniqCode, string pVideoFile, DateTime? pSyksrq)
        {
            this._depcode = pDepCode;
            this._csylb = pCsylb;
            this._nsylb = pNsylb;
            this._xz = pXz;
            this._wtbh = pWtbh;
            this._zh = pZh;
            this._syr = pSyr;
            this._syfhr = pSyfhr;
            this._syrq = pSyrq;
            this._qycs = pQycs;
            this._sbmcbh = pSbmcbh;
            this._beizhu = pBeizhu;
            this._sfwc = pSfwc;
            this._sysj1 = pSysj1;
            this._sysj2 = pSysj2;
            this._sysj3 = pSysj3;
            this._sysj4 = pSysj4;
            this._sysj5 = pSysj5;
            this._sysj6 = pSysj6;
            this._sysj7 = pSysj7;
            this._sysj8 = pSysj8;
            this._sysj9 = pSysj9;
            this._sysj10 = pSysj10;
            this._sysj11 = pSysj11;
            this._sysj12 = pSysj12;
            this._sysj13 = pSysj13;
            this._sysj14 = pSysj14;
            this._sysj15 = pSysj15;
            this._sysj16 = pSysj16;
            this._sysj17 = pSysj17;
            this._sysj18 = pSysj18;
            this._sysj19 = pSysj19;
            this._sysj20 = pSysj20;
            this._sysj21 = pSysj21;
            this._sysj22 = pSysj22;
            this._sysj23 = pSysj23;
            this._sysj24 = pSysj24;
            this._sysj25 = pSysj25;
            this._sysj26 = pSysj26;
            this._sysj27 = pSysj27;
            this._sysj28 = pSysj28;
            this._sysj29 = pSysj29;
            this._sysj30 = pSysj30;
            this._sysj31 = pSysj31;
            this._sysj32 = pSysj32;
            this._sysj33 = pSysj33;
            this._sysj34 = pSysj34;
            this._sysj35 = pSysj35;
            this._sysj36 = pSysj36;
            this._sysj37 = pSysj37;
            this._sysj38 = pSysj38;
            this._sysj39 = pSysj39;
            this._sysj40 = pSysj40;
            this._sysj41 = pSysj41;
            this._sysj42 = pSysj42;
            this._sysj43 = pSysj43;
            this._sysj44 = pSysj44;
            this._sysj45 = pSysj45;
            this._sysj46 = pSysj46;
            this._sysj47 = pSysj47;
            this._sysj48 = pSysj48;
            this._sysj49 = pSysj49;
            this._sysj50 = pSysj50;
            this._sysj51 = pSysj51;
            this._sysj52 = pSysj52;
            this._sysj53 = pSysj53;
            this._sysj54 = pSysj54;
            this._sysj55 = pSysj55;
            this._sysj56 = pSysj56;
            this._sysj57 = pSysj57;
            this._sysj58 = pSysj58;
            this._sysj59 = pSysj59;
            this._sysj60 = pSysj60;
            this._datalist = pDataList;
            this._uniqcode = pUniqCode;
            this._videofile = pVideoFile;
            this._syksrq = pSyksrq;
        }

        public DcLog(int pRecid)
        {
            this._recid = pRecid;
        }

        #endregion

        #region Public Properties

        public virtual int Recid
        {
            get { return _recid; }
            set { _bIsChanged |= (_recid != value); _recid = value; }

        }

        public virtual string DepCode
        {
            get { return _depcode; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("DepCode", "DepCode value, cannot contain more than 50 characters");
                _bIsChanged |= (_depcode != value);
                _depcode = value;
            }

        }

        public virtual string Csylb
        {
            get { return _csylb; }
            set
            {
                if (value != null && value.Length > 5)
                    throw new ArgumentOutOfRangeException("Csylb", "Csylb value, cannot contain more than 5 characters");
                _bIsChanged |= (_csylb != value);
                _csylb = value;
            }

        }

        public virtual decimal? Nsylb
        {
            get { return _nsylb; }
            set { _bIsChanged |= (_nsylb != value); _nsylb = value; }

        }

        public virtual decimal? Xz
        {
            get { return _xz; }
            set { _bIsChanged |= (_xz != value); _xz = value; }

        }

        public virtual string Wtbh
        {
            get { return _wtbh; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Wtbh", "Wtbh value, cannot contain more than 50 characters");
                _bIsChanged |= (_wtbh != value);
                _wtbh = value;
            }

        }

        public virtual string Zh
        {
            get { return _zh; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Zh", "Zh value, cannot contain more than 50 characters");
                _bIsChanged |= (_zh != value);
                _zh = value;
            }

        }

        public virtual string Syr
        {
            get { return _syr; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Syr", "Syr value, cannot contain more than 20 characters");
                _bIsChanged |= (_syr != value);
                _syr = value;
            }

        }

        public virtual string Syfhr
        {
            get { return _syfhr; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Syfhr", "Syfhr value, cannot contain more than 20 characters");
                _bIsChanged |= (_syfhr != value);
                _syfhr = value;
            }

        }

        public virtual DateTime? Syrq
        {
            get { return _syrq; }
            set { _bIsChanged |= (_syrq != value); _syrq = value; }

        }

        public virtual decimal? Qycs
        {
            get { return _qycs; }
            set { _bIsChanged |= (_qycs != value); _qycs = value; }

        }

        public virtual string Sbmcbh
        {
            get { return _sbmcbh; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Sbmcbh", "Sbmcbh value, cannot contain more than 20 characters");
                _bIsChanged |= (_sbmcbh != value);
                _sbmcbh = value;
            }

        }

        public virtual string Beizhu
        {
            get { return _beizhu; }
            set
            {
                if (value != null && value.Length > 40)
                    throw new ArgumentOutOfRangeException("Beizhu", "Beizhu value, cannot contain more than 40 characters");
                _bIsChanged |= (_beizhu != value);
                _beizhu = value;
            }

        }

        public virtual bool? Sfwc
        {
            get { return _sfwc; }
            set { _bIsChanged |= (_sfwc != value); _sfwc = value; }

        }

        public virtual string Sysj1
        {
            get { return _sysj1; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj1", "Sysj1 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj1 != value);
                _sysj1 = value;
            }

        }

        public virtual string Sysj2
        {
            get { return _sysj2; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj2", "Sysj2 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj2 != value);
                _sysj2 = value;
            }

        }

        public virtual string Sysj3
        {
            get { return _sysj3; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj3", "Sysj3 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj3 != value);
                _sysj3 = value;
            }

        }

        public virtual string Sysj4
        {
            get { return _sysj4; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj4", "Sysj4 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj4 != value);
                _sysj4 = value;
            }

        }

        public virtual string Sysj5
        {
            get { return _sysj5; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj5", "Sysj5 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj5 != value);
                _sysj5 = value;
            }

        }

        public virtual string Sysj6
        {
            get { return _sysj6; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj6", "Sysj6 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj6 != value);
                _sysj6 = value;
            }

        }

        public virtual string Sysj7
        {
            get { return _sysj7; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj7", "Sysj7 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj7 != value);
                _sysj7 = value;
            }

        }

        public virtual string Sysj8
        {
            get { return _sysj8; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj8", "Sysj8 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj8 != value);
                _sysj8 = value;
            }

        }

        public virtual string Sysj9
        {
            get { return _sysj9; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj9", "Sysj9 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj9 != value);
                _sysj9 = value;
            }

        }

        public virtual string Sysj10
        {
            get { return _sysj10; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj10", "Sysj10 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj10 != value);
                _sysj10 = value;
            }

        }

        public virtual string Sysj11
        {
            get { return _sysj11; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj11", "Sysj11 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj11 != value);
                _sysj11 = value;
            }

        }

        public virtual string Sysj12
        {
            get { return _sysj12; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj12", "Sysj12 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj12 != value);
                _sysj12 = value;
            }

        }

        public virtual string Sysj13
        {
            get { return _sysj13; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj13", "Sysj13 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj13 != value);
                _sysj13 = value;
            }

        }

        public virtual string Sysj14
        {
            get { return _sysj14; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj14", "Sysj14 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj14 != value);
                _sysj14 = value;
            }

        }

        public virtual string Sysj15
        {
            get { return _sysj15; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj15", "Sysj15 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj15 != value);
                _sysj15 = value;
            }

        }

        public virtual string Sysj16
        {
            get { return _sysj16; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj16", "Sysj16 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj16 != value);
                _sysj16 = value;
            }

        }

        public virtual string Sysj17
        {
            get { return _sysj17; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj17", "Sysj17 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj17 != value);
                _sysj17 = value;
            }

        }

        public virtual string Sysj18
        {
            get { return _sysj18; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj18", "Sysj18 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj18 != value);
                _sysj18 = value;
            }

        }

        public virtual string Sysj19
        {
            get { return _sysj19; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj19", "Sysj19 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj19 != value);
                _sysj19 = value;
            }

        }

        public virtual string Sysj20
        {
            get { return _sysj20; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj20", "Sysj20 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj20 != value);
                _sysj20 = value;
            }

        }

        public virtual string Sysj21
        {
            get { return _sysj21; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj21", "Sysj21 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj21 != value);
                _sysj21 = value;
            }

        }

        public virtual string Sysj22
        {
            get { return _sysj22; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj22", "Sysj22 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj22 != value);
                _sysj22 = value;
            }

        }

        public virtual string Sysj23
        {
            get { return _sysj23; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj23", "Sysj23 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj23 != value);
                _sysj23 = value;
            }

        }

        public virtual string Sysj24
        {
            get { return _sysj24; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj24", "Sysj24 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj24 != value);
                _sysj24 = value;
            }

        }

        public virtual string Sysj25
        {
            get { return _sysj25; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj25", "Sysj25 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj25 != value);
                _sysj25 = value;
            }

        }

        public virtual string Sysj26
        {
            get { return _sysj26; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj26", "Sysj26 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj26 != value);
                _sysj26 = value;
            }

        }

        public virtual string Sysj27
        {
            get { return _sysj27; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj27", "Sysj27 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj27 != value);
                _sysj27 = value;
            }

        }

        public virtual string Sysj28
        {
            get { return _sysj28; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj28", "Sysj28 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj28 != value);
                _sysj28 = value;
            }

        }

        public virtual string Sysj29
        {
            get { return _sysj29; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj29", "Sysj29 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj29 != value);
                _sysj29 = value;
            }

        }

        public virtual string Sysj30
        {
            get { return _sysj30; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj30", "Sysj30 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj30 != value);
                _sysj30 = value;
            }

        }

        public virtual string Sysj31
        {
            get { return _sysj31; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj31", "Sysj31 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj31 != value);
                _sysj31 = value;
            }

        }

        public virtual string Sysj32
        {
            get { return _sysj32; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj32", "Sysj32 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj32 != value);
                _sysj32 = value;
            }

        }

        public virtual string Sysj33
        {
            get { return _sysj33; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj33", "Sysj33 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj33 != value);
                _sysj33 = value;
            }

        }

        public virtual string Sysj34
        {
            get { return _sysj34; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj34", "Sysj34 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj34 != value);
                _sysj34 = value;
            }

        }

        public virtual string Sysj35
        {
            get { return _sysj35; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj35", "Sysj35 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj35 != value);
                _sysj35 = value;
            }

        }

        public virtual string Sysj36
        {
            get { return _sysj36; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj36", "Sysj36 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj36 != value);
                _sysj36 = value;
            }

        }

        public virtual string Sysj37
        {
            get { return _sysj37; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj37", "Sysj37 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj37 != value);
                _sysj37 = value;
            }

        }

        public virtual string Sysj38
        {
            get { return _sysj38; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj38", "Sysj38 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj38 != value);
                _sysj38 = value;
            }

        }

        public virtual string Sysj39
        {
            get { return _sysj39; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj39", "Sysj39 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj39 != value);
                _sysj39 = value;
            }

        }

        public virtual string Sysj40
        {
            get { return _sysj40; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj40", "Sysj40 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj40 != value);
                _sysj40 = value;
            }

        }

        public virtual string Sysj41
        {
            get { return _sysj41; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj41", "Sysj41 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj41 != value);
                _sysj41 = value;
            }

        }

        public virtual string Sysj42
        {
            get { return _sysj42; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj42", "Sysj42 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj42 != value);
                _sysj42 = value;
            }

        }

        public virtual string Sysj43
        {
            get { return _sysj43; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj43", "Sysj43 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj43 != value);
                _sysj43 = value;
            }

        }

        public virtual string Sysj44
        {
            get { return _sysj44; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj44", "Sysj44 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj44 != value);
                _sysj44 = value;
            }

        }

        public virtual string Sysj45
        {
            get { return _sysj45; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj45", "Sysj45 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj45 != value);
                _sysj45 = value;
            }

        }

        public virtual string Sysj46
        {
            get { return _sysj46; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj46", "Sysj46 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj46 != value);
                _sysj46 = value;
            }

        }

        public virtual string Sysj47
        {
            get { return _sysj47; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj47", "Sysj47 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj47 != value);
                _sysj47 = value;
            }

        }

        public virtual string Sysj48
        {
            get { return _sysj48; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj48", "Sysj48 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj48 != value);
                _sysj48 = value;
            }

        }

        public virtual string Sysj49
        {
            get { return _sysj49; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj49", "Sysj49 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj49 != value);
                _sysj49 = value;
            }

        }

        public virtual string Sysj50
        {
            get { return _sysj50; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj50", "Sysj50 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj50 != value);
                _sysj50 = value;
            }

        }

        public virtual string Sysj51
        {
            get { return _sysj51; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj51", "Sysj51 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj51 != value);
                _sysj51 = value;
            }

        }

        public virtual string Sysj52
        {
            get { return _sysj52; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj52", "Sysj52 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj52 != value);
                _sysj52 = value;
            }

        }

        public virtual string Sysj53
        {
            get { return _sysj53; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj53", "Sysj53 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj53 != value);
                _sysj53 = value;
            }

        }

        public virtual string Sysj54
        {
            get { return _sysj54; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj54", "Sysj54 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj54 != value);
                _sysj54 = value;
            }

        }

        public virtual string Sysj55
        {
            get { return _sysj55; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj55", "Sysj55 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj55 != value);
                _sysj55 = value;
            }

        }

        public virtual string Sysj56
        {
            get { return _sysj56; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj56", "Sysj56 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj56 != value);
                _sysj56 = value;
            }

        }

        public virtual string Sysj57
        {
            get { return _sysj57; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj57", "Sysj57 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj57 != value);
                _sysj57 = value;
            }

        }

        public virtual string Sysj58
        {
            get { return _sysj58; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj58", "Sysj58 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj58 != value);
                _sysj58 = value;
            }

        }

        public virtual string Sysj59
        {
            get { return _sysj59; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj59", "Sysj59 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj59 != value);
                _sysj59 = value;
            }

        }

        public virtual string Sysj60
        {
            get { return _sysj60; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Sysj60", "Sysj60 value, cannot contain more than 50 characters");
                _bIsChanged |= (_sysj60 != value);
                _sysj60 = value;
            }

        }

        public virtual string DataList
        {
            get { return _datalist; }
            set
            {
                if (value != null && value.Length > 2147483647)
                    throw new ArgumentOutOfRangeException("DataList", "DataList value, cannot contain more than 2147483647 characters");
                _bIsChanged |= (_datalist != value);
                _datalist = value;
            }

        }

        public virtual string UniqCode
        {
            get { return _uniqcode; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("UniqCode", "UniqCode value, cannot contain more than 50 characters");
                _bIsChanged |= (_uniqcode != value);
                _uniqcode = value;
            }

        }

        public virtual string VideoFile
        {
            get { return _videofile; }
            set
            {
                if (value != null && value.Length > 1000)
                    throw new ArgumentOutOfRangeException("VideoFile", "VideoFile value, cannot contain more than 1000 characters");
                _bIsChanged |= (_videofile != value);
                _videofile = value;
            }

        }

        public virtual DateTime? Syksrq
        {
            get { return _syksrq; }
            set { _bIsChanged |= (_syksrq != value); _syksrq = value; }

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
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            DcLog castObj = null;
            try
            {
                castObj = (DcLog)obj;
            }
            catch (Exception) { return false; }
            return (castObj != null) &&
                (this._recid == castObj.Recid);
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

    #region Custom ICollection interface for DcLog


    public interface IDcLogCollection : ICollection
    {
        DcLog this[int index] { get; set; }
        void Add(DcLog pDcLog);
        void Clear();
    }

    [Serializable]
    public class DcLogCollection : IDcLogCollection
    {
        private IList<DcLog> _arrayInternal;

        public DcLogCollection()
        {
            _arrayInternal = new List<DcLog>();
        }

        public DcLogCollection(IList<DcLog> pSource)
        {
            _arrayInternal = pSource;
            if (_arrayInternal == null)
            {
                _arrayInternal = new List<DcLog>();
            }
        }

        public DcLog this[int index]
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
        public void CopyTo(Array array, int index) { _arrayInternal.CopyTo((DcLog[])array, index); }
        public IEnumerator GetEnumerator() { return _arrayInternal.GetEnumerator(); }
        public void Add(DcLog pDcLog) { _arrayInternal.Add(pDcLog); }
        public void Clear() { _arrayInternal.Clear(); }
        public IList<DcLog> GetList() { return _arrayInternal; }
    }

    #endregion
}
