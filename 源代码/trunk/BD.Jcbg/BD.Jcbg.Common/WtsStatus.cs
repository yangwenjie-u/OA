using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 委托书状态
    /// </summary>
    public class WtsStatus
    {
        protected string mStatus = "";
        protected const int MinLength = 10;     // 最小状态长度
        // 主状态
        public const int MainStateIndex = 0; // 主状态下标
        public const char MainStateWt = 'W'; // 委托
        public const char MainStateSy = 'S'; // 试验
        public const char MainStateBg = 'B'; // 报告
        public const char MainStateGd = 'G'; // 归档
        // 作废
        public const int ZfStateIndex = 1;   // 作废
        public const int DcStateIndex = 2;   // 待查
        public const int SfStateIndex = 3;   // 收费
        // 委托状态
        public const int WtStateIndex = 4;   // 委托
        public const char WtStateBc = '0';   // 委托保存
        public const char WtStateBd = '1';   // 委托布点
        public const char WtStateTj = '2';   // 委托提交
        public const char WtStateDy = '3';   // 打印
        public const char WtStateXf = '4';   // 下发到检测中心
        public const char WtStateHz = 'A';   // 汇总
        public const char WtStateJf = 'B';   // 计费
        // 试验状态
        protected const int SyStateIndex = 5;   // 试验
        // 报告状态
        protected const int BgStateIndex = 6;   // 报告
        // 见证状态
        public const int JzStateIndex = 7;   // 是否已见证，0-见证人未拍照，1-见证人已拍照，2-收样人已拍照，3-见证人同意收样，4-见证人不同意收样, 5-新见证流程预见证状态
        public const char JzStateNo = '0';
        public const char JzStateTp1 = '1';
        public const char JzStateTp2 = '2';
        public const char JzStateTy = '3';
        public const char JzStateJj = '4';
        public const char JzStateNew = '5';
        //现场检测数据
        public const int Xcjcsj = 8;
        // 委托单锁定
        public const int WtdSdIndex = 9;

        public WtsStatus(string status)
        {
            mStatus = status.GetSafeString();
        }
        #region 主状态
        /// <summary>
        /// 是否委托阶段
        /// </summary>
        public bool StateWt
        {
            get
            {
                return mStatus.Length >= MinLength && mStatus[MainStateIndex].Equals(MainStateWt);
            }
        }

        /// <summary>
        /// 是否试验阶段
        /// </summary>
        public bool StateSy
        {
            get
            {
                return mStatus.Length >= MinLength && mStatus[MainStateIndex].Equals(MainStateSy);
            }
        }
        /// <summary>
        /// 是否出报告阶段
        /// </summary>
        public bool StateBg
        {
            get
            {
                return mStatus.Length >= MinLength && mStatus[MainStateIndex].Equals(MainStateBg);
            }
        }
        /// <summary>
        /// 是否已归档
        /// </summary>
        public bool StateGd
        {
            get
            {
                return mStatus.Length >= MinLength && mStatus[MainStateIndex].Equals(MainStateGd);
            }
        }
        /// <summary>
        /// 委托单作废
        /// </summary>
        public bool StateZf
        {
            get
            {
                return mStatus.Length >= MinLength && mStatus[ZfStateIndex].Equals("1");
            }
        }
        #endregion
        #region 委托状态
        /// <summary>
        /// 委托保存
        /// </summary>
        public bool StateWtbc
        {
            get
            {
                return StateWt && mStatus.Length >= MinLength && mStatus[WtStateIndex].Equals(WtStateBc);
            }
        }
        /// <summary>
        /// 委托布点
        /// </summary>
        public bool StateWtbd
        {
            get
            {
                return StateWt && mStatus.Length >= MinLength && mStatus[WtStateIndex].Equals(WtStateBd);
            }
        }
        /// <summary>
        /// 委托提交
        /// </summary>
        public bool StateWttj
        {
            get
            {
                return StateWt && mStatus.Length >= MinLength && mStatus[WtStateIndex].Equals(WtStateTj);
            }
        }
        /// <summary>
        /// 委托打印
        /// </summary>
        public bool StateWtdy
        {
            get
            {
                return StateWt && mStatus.Length >= MinLength && mStatus[WtStateIndex].Equals(WtStateDy);
            }
        }
        /// <summary>
        /// 委托下发
        /// </summary>
        public bool StateWtxf
        {
            get
            {
                return StateWt && mStatus.Length >= MinLength && mStatus[WtStateIndex].Equals(WtStateXf);
            }
        }

        /// <summary>
        /// 委托见证
        /// </summary>
        public bool StateWtjz
        {
            get
            {
                return StateWt && mStatus.Length >= MinLength && mStatus[JzStateIndex].Equals(JzStateTp1);
            }
        }

        #endregion


        #region 操作判断
        /// <summary>
        /// 委托书是否可以删除
        /// </summary>
        public bool CanDelete
        {
            get { return StateWtbc || StateWtbd || StateWttj || StateWtdy || mStatus.Length < MinLength; }
        }
        /// <summary>
        /// 委托书是否可以提交
        /// </summary>
        public bool CanWtsSubmit
        {
            get { return (StateWtbc || StateWtbd || StateWttj || StateWtdy) && StateWt; }
        }
        /// <summary>
        /// 委托书是否可以下发
        /// </summary>
        public bool CanWtsDown
        {
            get { return (StateWtbc || StateWtbd || StateWttj || StateWtdy) && StateWt; }
        }

        /// <summary>
        /// 委托书是否可以取消下发
        /// </summary>
        public bool CanWtsCancelDown
        {
            get { return StateWtxf && StateWt; }
        }
        /// <summary>
        /// 委托单是否已提交
        /// </summary>

        public bool HasWtdSubmit
        {
            get { return !((StateWtbc || StateWtbd) && StateWt); }
        }
        /// <summary>
        /// 委托单是否已送样
        /// </summary>
        public bool HasWtdDown
        {
            get { return !((StateWtbc || StateWtbd || StateWttj || StateWtdy) && StateWt); }
        }

        /// <summary>
        /// 委托单是否已见证
        /// </summary>
        public bool HasWtdJz
        {
            get { return StateWtjz; }
        }
        /// <summary>
        /// 委托单是否可以上传见证图片
        /// </summary>
        public bool CanUpXcpt
        {
            get { return mStatus[JzStateIndex] != '3'; }
        }
        /// <summary>
        /// 委托单是否可以设置见证状态
        /// </summary>
        public bool CanSetJzzt
        {
            get { return mStatus[JzStateIndex] != '3'; }
        }
        /// <summary>
        /// 图片上传时，是否需要设置见证状态
        /// </summary>
        /// <param name="newZt"></param>
        /// <returns></returns>
        public bool NeedUpdateImageJzzt(char newZt)
        {
            return mStatus[JzStateIndex] < newZt;
        }

        public bool JzStateCompleteTp1
        {
            get { return mStatus[JzStateIndex] == '1'; }
        }

        /// <summary>
        /// 委托单是否作废
        /// </summary>
        public bool HasWtdZf
        {
            get { return mStatus[ZfStateIndex] == '1'; }
        }

        #endregion

        #region 设置操作
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdBc(out string msg)
        {
            bool ret = false;
            msg = "";
            if (mStatus.Length < MinLength)
            {
                msg = "委托单状态长度无效";
                return ret;
            }
            mStatus = mStatus.Substring(0, WtStateIndex) + WtStateBc + mStatus.Substring(WtStateIndex + 1);
            ret = true;
            return ret;
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdTj(out string msg)
        {
            bool ret = false;
            msg = "";
            if (mStatus.Length < MinLength)
            {
                msg = "委托单状态长度无效";
                return ret;
            }
            mStatus = mStatus.Substring(0, WtStateIndex) + WtStateTj + mStatus.Substring(WtStateIndex + 1);
            //mStatus.Remove(WtStateIndex);
            //mStatus.Insert(WtStateIndex, "1");
            ret = true;
            return ret;
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdDy(out string msg)
        {
            bool ret = false;
            msg = "";
            if (mStatus.Length < MinLength)
            {
                msg = "委托单状态长度无效";
                return ret;
            }
            mStatus = mStatus.Substring(0, WtStateIndex) + WtStateDy + mStatus.Substring(WtStateIndex + 1);
            //mStatus.Remove(WtStateIndex);
            //mStatus.Insert(WtStateIndex, "1");
            ret = true;
            return ret;
        }
        /// <summary>
        /// 下发
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdXf(out string msg)
        {
            bool ret = false;
            msg = "";
            if (mStatus.Length < MinLength)
            {
                msg = "委托单状态长度无效";
                return ret;
            }
            mStatus = mStatus.Substring(0, WtStateIndex) + WtStateXf + mStatus.Substring(WtStateIndex + 1);
            //mStatus.Remove(WtStateIndex);
            //mStatus.Insert(WtStateIndex, "1");
            ret = true;
            return ret;
        }
        /// <summary>
        /// 设置见证取样状态
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdJzqyzt(char zt, out string msg)
        {
            bool ret = false;
            msg = "";
            if (mStatus.Length < MinLength)
            {
                msg = "委托单状态长度无效";
                return ret;
            }
            mStatus = mStatus.Substring(0, JzStateIndex) + zt + mStatus.Substring(JzStateIndex + 1);
            ret = true;
            return ret;
        }

        /// <summary>
        /// 设置未送样状态
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdWSY(out string msg)
        {
            bool ret = false;
            msg = "";
            if (mStatus.Length < MinLength)
            {
                msg = "委托单状态长度无效";
                return ret;
            }

            //设置主状态为委托
            mStatus = mStatus.Substring(0, MainStateIndex) + MainStateWt + mStatus.Substring(MainStateIndex + 1);

            //设置为打印状态
            SetWtdDy(out msg);

            //设置为解锁
            SetWtdSd('0', out msg);

            ret = true;
            return ret;
        }

        /// <summary>
        /// 设置锁定状态
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdSd(char zt, out string msg)
        {
            bool ret = false;
            msg = "";
            if (mStatus.Length < MinLength)
            {
                msg = "委托单状态长度无效";
                return ret;
            }

            mStatus = mStatus.Substring(0, WtdSdIndex) + zt + mStatus.Substring(WtdSdIndex + 1);
            ret = true;
            return ret;
        }

        /// <summary>
        /// 设置试验状态
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdSyZt(out string msg)
        {
            bool ret = false;
            msg = "";
            if (mStatus.Length < MinLength)
            {
                msg = "委托单状态长度无效";
                return ret;
            }

            mStatus = mStatus.Substring(0, MainStateIndex) + WtsStatus.MainStateSy + mStatus.Substring(MainStateIndex + 1);
            ret = true;
            return ret;
        }
        #endregion

        #region 返回
        public string GetStatus()
        {
            return mStatus;
        }
        #endregion

    }
}