using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml;
using BD.Jcbg.Common;

namespace BD.Jcbg.Web.Func
{
    public static class DllApi
    {
        /// <summary>
        /// 报警回调函数。
        ///@param   								nPDLLHandle				SDK句柄
        ///@param   								szAlarmId               报警Id
        ///@param   								nDeviceType             设备类型
        ///@param   								szCameraId              通道Id
        ///@param   								szDeviceName            设备名称
        ///@param   								szChannelName           通道名称
        ///@param   								szCoding                编码
        ///@param   								szMessage               报警信息
        ///@param   								nAlarmType              报警类型，参考dpsdk_alarm_type_e
        ///@param   								nEventType              报警发生类型，参考dpsdk_event_type_e
        ///@param   								nLevel                  报警等级
        ///@param   								nTime                   报警时间
        ///@param   								pAlarmData              报警数据
        ///@param   								nAlarmDataLen           报警数据长度
        ///@param   								pPicData                图片数据
        ///@param   								nPicDataLen             图片数据长度
        ///@param   								pUserParam              用户数据
        ///@remark									
        /// </summary>
        public delegate IntPtr fDPSDKAlarmCallback(IntPtr nPDLLHandle,
                                                    [MarshalAs(UnmanagedType.LPStr)] StringBuilder szAlarmId,
                                                    IntPtr nDeviceType,
                                                    [MarshalAs(UnmanagedType.LPStr)] StringBuilder szCameraId,
                                                    [MarshalAs(UnmanagedType.LPStr)] StringBuilder szDeviceName,
                                                    [MarshalAs(UnmanagedType.LPStr)] StringBuilder szChannelName,
                                                    [MarshalAs(UnmanagedType.LPStr)] StringBuilder szCoding,
                                                    [MarshalAs(UnmanagedType.LPStr)] StringBuilder szMessage,
                                                    IntPtr nAlarmType,
                                                    IntPtr nEventType,
                                                    IntPtr nLevel,
                                                    Int64 nTime,
                                                    [MarshalAs(UnmanagedType.LPStr)] StringBuilder pAlarmData,
                                                    IntPtr nAlarmDataLen,
                                                    [MarshalAs(UnmanagedType.LPStr)] StringBuilder pPicData,
                                                    IntPtr nPicDataLen,
                                                    IntPtr pUserParam);
        public static fDPSDKAlarmCallback nFun;

        /// 设备状态回调函数。
        ///@param   								nPDLLHandle				SDK句柄
        ///@param   								szDeviceId              设备Id
        ///@param   								nStatus		            状态  1在线  2离线
        ///@param   								pUserParam              用户数据
        ///@remark									
        /// </summary>
        public delegate IntPtr fDPSDKDevStatusCallback(IntPtr nPDLLHandle,
                                                   [MarshalAs(UnmanagedType.LPStr)] StringBuilder szDeviceId,
                                                   IntPtr nStatus,
                                                   IntPtr pUserParam);
        public static fDPSDKDevStatusCallback fDevStatus;

        /// Json传输协议回调。
        ///@param   								nPDLLHandle				SDK句柄
        ///@param                                   szJson	                Json字符串
        ///@param                                   JsonLen	                Json字符串长度
        ///@param   								pUserParam              用户数据
        ///@remark									
        /// </summary>
        public delegate IntPtr fDPSDKGeneralJsonTransportCallbackEx(IntPtr nPDLLHandle,
                                                                    IntPtr ptrJson,
                                                                    int JsonLen,
                                                                    IntPtr pUserParam);

        public static fDPSDKGeneralJsonTransportCallbackEx fJsonCallback;

        //违章报警回调
        public delegate IntPtr fDPSDKTrafficAlarmCallback(IntPtr nPDLLHandle, ref Traffic_Alarm_Info_t pRetInfo, IntPtr pUserParam);

        public static fDPSDKTrafficAlarmCallback fTrafficAlarmCallback;

        //卡口过车信息回调
        public delegate IntPtr fDPSDKGetBayCarInfoCallbackEx(IntPtr nPDLLHandle,
                                                            IntPtr szDeviceId,
                                                            int nDeviceIdLen,
                                                            int nDevChnId,
                                                            IntPtr szChannelId,
                                                            int nChannelIdLen,
                                                            IntPtr szDeviceName,
                                                            int nDeviceNameLen,
                                                            IntPtr szDeviceChnName,
                                                            int nChanNameLen,
                                                            IntPtr szCarNum,
                                                            int nCarNumLen,
                                                            int nCarNumType,
                                                            int nCarNumColor,
                                                            int nCarSpeed,
                                                            int nCarType,
                                                            int nCarColor,
                                                            int nCarLen,
                                                            int nCarDirect,
                                                            int nWayId,
                                                            UInt64 lCaptureTime,
                                                            UInt32 lPicGroupStoreID,
                                                            int nIsNeedStore,
                                                            int nIsStoraged,
                                                            IntPtr szCaptureOrg,
                                                            int nCaptureOrgLen,
                                                            IntPtr szOptOrg,
                                                            int nOptOrgLen,
                                                            IntPtr szOptUser,
                                                            int nOptUserLen,
                                                            IntPtr szOptNote,
                                                            int nOptNoteLen,
                                                            IntPtr szImg0Path,
                                                            int nImg0PathLen,
                                                            IntPtr szImg1Path,
                                                            int nImg1PathLen,
                                                            IntPtr szImg2Path,
                                                            int nImg2PathLen,
                                                            IntPtr szImg3Path,
                                                            int nImg3PathLen,
                                                            IntPtr szImg4Path,
                                                            int nImg4PathLen,
                                                            IntPtr szImg5Path,
                                                            int nImg5PathLen,
                                                            IntPtr szImgPlatePath,
                                                            int nImgPlatePathLen,
                                                            int icarLog,
                                                            int iPlateLeft,
                                                            int iPlateRight,
                                                            int iPlateTop,
                                                            int iPlateBottom,
                                                            IntPtr pUserParam);
        public static fDPSDKGetBayCarInfoCallbackEx fBayCarInfoEx;

        public delegate IntPtr fDPSDKGetBayCarInfoCallback(IntPtr nPDLLHandle, ref Bay_Car_Info_t pRetInfo, IntPtr pUserParam);
        public static fDPSDKGetBayCarInfoCallback fBayCarInfo;

        /// 门禁状态上报接收回调函数。
        ///@param   								nPDLLHandle				SDK句柄
        ///@param   								szCameraId              通道Id
        ///@param                                   nStatus                 门状态
        ///@param   								nTime                   上报时间
        ///@param   								pUserParam              用户数据
        ///@remark									
        public delegate IntPtr fDPSDKPecDoorStarusCallBack(IntPtr nPDLLHandle,
                                                    [MarshalAs(UnmanagedType.LPStr)] StringBuilder szCameraId,
                                                    dpsdk_door_status_e nStatus,
                                                    Int64 nTime,
                                                    IntPtr pUserParam);
        public static fDPSDKPecDoorStarusCallBack fPecDoorStatus;

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_Create(dpsdk_sdk_type_e nType, ref IntPtr nPDLLHandle);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_Destroy(IntPtr nPDLLHandle);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetLog(IntPtr nPDLLHandle, dpsdk_log_level_e nLevel, [MarshalAs(UnmanagedType.LPStr)] string szFilename, Boolean bScreen, Boolean bDebugger);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_StartMonitor(IntPtr nPDLLHandle, [MarshalAs(UnmanagedType.LPStr)] string szFilename);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_Login(IntPtr nPDLLHandle, ref Login_Info_t pLoginInfo, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_Logout(IntPtr nPDLLHandle, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_LoadDGroupInfo(IntPtr nPDLLHandle, ref IntPtr nGroupLen, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Unicode)]
        private extern static IntPtr DPSDK_GetDGroupStr(IntPtr nPDLLHandle, ref byte szGroupStr, IntPtr nGroupLen, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Unicode)]
        private extern static IntPtr DPSDK_GetDGroupCount(IntPtr nPDLLHandle, ref Get_Dep_Count_Info_t pGetInfo);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Unicode)]
        private extern static IntPtr DPSDK_GetDGroupInfoEx(IntPtr nPDLLHandle, ref Get_Dep_Info_Ex_t pGetInfo);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Unicode)]
        private extern static IntPtr DPSDK_GetCameraIdbyDevInfo(IntPtr nPDLLHandle, byte[] szDevIp, int nPort, int nChnlNum, ref byte szCameraId, dpsdk_dev_unit_type_e nUnitType);

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_InitExt();

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_UnitExt();

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_StartRealplay(IntPtr nPDLLHandle, out IntPtr nRealSeq, ref Get_RealStream_Info_t pGetInfo, IntPtr hwnd, IntPtr nTimeout);

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_StopRealplayBySeq(IntPtr nPDLLHandle, IntPtr nRealSeq, IntPtr nTimeout);
        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static int DPSDK_QueryRecord(IntPtr nPDLLHandle, ref  Query_Record_Info_t pQueryInfo, out int nRecordCount, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static int DPSDK_GetRecordInfo(IntPtr nPDLLHandle, ref Record_Info_t pRecords);

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_StartPlaybackByTime(IntPtr nPDLLHandle, out IntPtr nPlaybackSeq, ref Get_RecordStream_Time_Info_t pGetInfo, IntPtr hwnd, IntPtr nTimeout);

        //本地dav文件回放
        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_StartPlaybackByLocal(IntPtr nPDLLHandle, out IntPtr nPlaybackSeq, ref Get_Record_Local_Info_t pRecordInfo, IntPtr hwnd);

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetPlaybackSpeed(IntPtr nPDLLHandle, IntPtr nPlaybackSeq, dpsdk_playback_speed_e nSpeed, IntPtr nTimeout);

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_PausePlaybackBySeq(IntPtr nPDLLHandle, IntPtr nPlaybackSeq, IntPtr nTimeout);

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_ResumePlaybackBySeq(IntPtr nPDLLHandle, IntPtr nPlaybackSeq, IntPtr nTimeout);

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_StopPlaybackBySeq(IntPtr nPDLLHandle, IntPtr nPlaybackSeq, IntPtr nTimeout);

        //按时间下载录像，存储成dav格式
        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_DownloadRecordByTime(IntPtr nPDLLHandle, out int nPlaybackSeq, byte[] szCameraId, dpsdk_recsource_type_e nSourceType, UInt64 uBeginTime, UInt64 uEndTime, byte[] szFileName, IntPtr nTimeout);

        //按时间下载录像，存储成其他格式
        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_DownloadRecordByTimeEx(IntPtr nPDLLHandle, out int nPlaybackSeq, byte[] szCameraId, dpsdk_recsource_type_e nSourceType, UInt64 uBeginTime, UInt64 uEndTime, byte[] szFileName, file_type_e nFileType, IntPtr nTimeout);
        //停止下载
        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_StopDownloadRecord(IntPtr nPDLLHandle, int nPlaybackSeq);

        /// 录像下载进度通知。
        ///@param   								nPDLLHandle				SDK句柄
        ///@param                                   nDownloadSeq	        下载录像的序列号
        ///@param                                   nPos                    进度度，范围0--100
        ///@param   								pUserParam              用户数据
        ///@remark									
        /// </summary>
        public delegate IntPtr fDownloadProgressCallback(IntPtr nPDLLHandle,
                                                         int downloadSeq,
                                                         int position,
                                                         IntPtr pUserParam);

        public static fDownloadProgressCallback fDownLoadProcess;

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetDownloadProgressCallback(IntPtr nPDLLHandle, fDownloadProgressCallback fDownLoadFinish, IntPtr pUser);

        /// 录像下载结束通知。
        ///@param   								nPDLLHandle				SDK句柄
        ///@param                                   downloadSeq	            下载序列号
        ///@param   								pUserParam              用户数据
        ///@remark									
        /// </summary>
        public delegate IntPtr fDownloadFinishedCallback(IntPtr nPDLLHandle,
                                                         int downloadSeq,
                                                         IntPtr pUserParam);

        public static fDownloadFinishedCallback fDownLoadFinish;

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetDownloadFinishedCallback(IntPtr nPDLLHandle, fDownloadFinishedCallback fDownLoadFinish, IntPtr pUser);

        /* 第一帧码流回调通知。
        ///@param   								nPDLLHandle				SDK句柄
        ///@param                                   nSequence	            码流序列号
        ///@param                                   szCamearId	            码流对应的通道ID
        ///@param                                   nCameraIDLen	        通道ID字符串长度
        ///@param                                   nPlayMode	            播放类型，1实时，2回放，详细参考dpsdk_media_func_e
        ///@param                                   nFactoryType	        厂商标示*- 0表示包含大华头的厂商码流，需要内部分析是哪个厂商，目前只支持大华、海康、华三
																					*- 1表示大华厂家
																					*- 2表示海康厂家
																					*- 4表示汉邦厂商
																					*- 5表示天地伟业
																					*- 6表示恒忆
																					*- 7表示黄河
																					*- 8表示朗驰
																					*- 9表示浩特
																					*- 10表示卡尔 
																					*- 11表示景阳
																					*- 12表示中维世纪，后缀名通常为801
																					*- 13表示中维世纪板卡,后缀名通常为sv4(和通用版本是2套SDK）
																					*- 14表示东方网力
																					*- 15表示恒通
																					*- 16表示立元的DB33
																					*- 17表示环视
																					*- 18表示蓝星
        ///@param   								pUserParam              用户数据
        ///@remark									
        /// </summary>  */
        public delegate IntPtr fMediaDataFirstFrameCallback(IntPtr nPDLLHandle,
                                                            int nSequence,
                                                            IntPtr szCamearId,
                                                            int nCameraIDLen,
                                                            int nPlayMode,
                                                            int nFactoryType,
                                                            IntPtr pUserParam);

        public static fMediaDataFirstFrameCallback fMediaDataFirstFrame;

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetMediaDataFirstFrameCallback(fMediaDataFirstFrameCallback pFun, IntPtr pUser);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetDPSDKAlarmCallback(IntPtr nPDLLHandle, fDPSDKAlarmCallback pFun, IntPtr pUser);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetDPSDKDeviceStatusCallback(IntPtr nPDLLHandle, fDPSDKDevStatusCallback pFun, IntPtr pUser);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi, EntryPoint = "DPSDK_SetGeneralJsonTransportCallbackEx", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        private extern static IntPtr DPSDK_SetGeneralJsonTransportCallbackEx(IntPtr nPDLLHandle, fDPSDKGeneralJsonTransportCallbackEx pFun, IntPtr pUser);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_EnableAlarm(IntPtr nPDLLHandle, ref Alarm_Enable_Info_t pSourceInfo, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static int DPSDK_DisableAlarm(IntPtr nPDLLHandle, int nTimeout);


        /** 查询电视墙列表个数.
         @param	  IN	nPDLLHandle		SDK句柄
         @param	  OUT	nCount			返回个数
         @param   IN	nTimeout		超时时长，单位毫秒
         @return  函数返回错误类型，参考dpsdk_retval_e
         @remark  
        */
        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        public extern static IntPtr DPSDK_QueryTvWallList(IntPtr nPDLLHandle, ref UInt32 nCount, IntPtr nTimeout);

        /** 查询电视墙布局信息
         @param	  IN	nPDLLHandle		SDK句柄
         @param	  IN	nTvWallId		电视墙ID
         @param	  OUT	nCount			返回个数
         @param   IN	nTimeout		超时时长，单位毫秒
         @return  函数返回错误类型，参考dpsdk_retval_e
         @remark  
        */
        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        public extern static IntPtr DPSDK_QueryTvWallLayout(IntPtr nPDLLHandle, Int32 nTvWallId, ref UInt32 nCount, IntPtr nTimeout);


        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_GetTvWallListCount(IntPtr nPDLLHandle, ref int nCount, int nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_GetTvWallList(IntPtr nPDLLHandle, ref TvWall_List_Info_t pTvWallListInfo, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_GetTvWallLayoutCount(IntPtr nPDLLHandle, int nTvWallId, ref uint nCount, int nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_GetTvWallLayout(IntPtr nPDLLHandle, ref TvWall_Layout_Info_t pTvWallLayoutInfo);

        /// <summary>
        /// 开窗必须是融合屏,非融合屏只能分割，融合的NVD只分割
        /// </summary>
        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetTvWallScreenSplit(IntPtr nPDLLHandle, ref TvWall_Screen_Split_t pSplitInfo, int nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_TvWallScreenOpenWindow(IntPtr nPDLLHandle, ref TvWall_Screen_Open_Window_t pOpenWindowInfo, int nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_TvWallScreenColseWindow(IntPtr nPDLLHandle, ref TvWall_Screen_Close_Window_t pCloseWindowInfo, int nTimeout);
        
        /** 窗口移动.
         @param	  IN	nPDLLHandle		SDK句柄
         @param	  INOUT	pMoveWindowInfo		电视墙屏窗口移动信息
         @param   IN	nTimeout		超时时长，单位毫秒
         @return  函数返回错误类型，参考dpsdk_retval_e
         @remark  
        */
        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        public extern static IntPtr DPSDK_TvWallScreenMoveWindow(IntPtr nPDLLHandle, ref TvWall_Screen_Move_Window_t pMoveWindowInfo, int nTimeout);

        /** 屏窗口置顶（对于开窗有效）.
         @param	  IN	nPDLLHandle		SDK句柄
         @param	  IN	pTopWindowInfo		电视墙屏窗口置顶信息
         @param   IN	nTimeout		超时时长，单位毫秒
         @return  函数返回错误类型，参考dpsdk_retval_e
         @remark  
        */
        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        public extern static IntPtr DPSDK_TvWallScreenSetTopWindow(IntPtr nPDLLHandle, ref TvWall_Screen_Set_Top_Window_t pTopWindowInfo, int nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetTvWallScreenWindowSource(IntPtr nPDLLHandle, ref Set_TvWall_Screen_Window_Source_t pSourceInfo, int nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_CloseTvWallScreenWindowSource(IntPtr nPDLLHandle, ref TvWall_Screen_Close_Source_t pSourceInfo, int nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_ClearTvWallScreen(IntPtr nPDLLHandle, int nTvWallId, int nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_QueryPrePoint(IntPtr nPDLLHandle, ref Ptz_Prepoint_Info_t pPrepoint, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_PtzPrePointOperation(IntPtr nPDLLHandle, ref Ptz_Prepoint_Operation_Info_t pPrepoint, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetDPSDKTrafficAlarmCallback(IntPtr nPDLLHandle, fDPSDKTrafficAlarmCallback pFun, IntPtr pUser);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetDPSDKGetBayCarInfoCallback(IntPtr nPDLLHandle, fDPSDKGetBayCarInfoCallback pFun, IntPtr pUser);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetDPSDKGetBayCarInfoCallbackEx(IntPtr nPDLLHandle, fDPSDKGetBayCarInfoCallbackEx pFun, IntPtr pUser);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SubscribeBayCarInfo(IntPtr nPDLLHandle, ref Subscribe_Bay_Car_Info_t pSourceInfo, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_GetRecordStreamByTime(IntPtr nPDLLHandle, out IntPtr nPlaybackSeq,
          ref Get_RecordStream_Time_Info_t pRecordInfo, fMediaDataCallback pFun, IntPtr pUser, int nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetPecDoorStatusCallback(IntPtr nPDLLHandle, fDPSDKPecDoorStarusCallBack pFun, IntPtr pUser);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_SetDoorCmd(IntPtr nPDLLHandle, ref SetDoorCmd_Request_t pSetDoorCmdRequest, IntPtr nTimeout);

        public delegate IntPtr fMediaDataCallback(IntPtr nPDLLHandle,
            IntPtr nSeq, int nMediaType, [MarshalAs(UnmanagedType.LPStr)] StringBuilder szNodeId, int nParamVal, IntPtr szData,
            int nDataLen, IntPtr pUserParam);

        public static fMediaDataCallback fMediaData;

        // 电视墙屏窗口关闭视频源
        public struct TvWall_Screen_Close_Source_t
        {
            public UInt32 nTvWallId;									// 电视墙ID
            public UInt32 nScreenId;									// 屏ID
            public UInt32 nWindowId;									// 窗口ID(若窗口ID=-1则表示关闭该屏中所有窗口的视频源)
        }

        // 电视墙屏窗口设置视频源
        [StructLayout(LayoutKind.Sequential)]
        public struct Set_TvWall_Screen_Window_Source_t
        {
            public UInt32 nTvWallId;									// 电视墙ID
            public UInt32 nScreenId;									// 屏ID
            public UInt32 nWindowId;									// 窗口ID
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;			                        // 通道ID
            public dpsdk_stream_type_e enStreamType;								// 码流类型
            public UInt64 nStayTime;									// 停留时间
        }

        // 电视墙屏关窗信息
        [StructLayout(LayoutKind.Sequential)]
        public struct TvWall_Screen_Close_Window_t
        {
            public UInt32 nTvWallId;									// 电视墙ID
            public UInt32 nScreenId;									// 屏ID
            public UInt32 nWindowId;									// 窗口ID
        }

        // 编码通道信息 扩展
        [StructLayout(LayoutKind.Sequential)]
        public struct Enc_Channel_Info_Ex_t
        {
            public dpsdk_camera_type_e nCameraType;		        // 类型，参见CameraType_e
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szId;				        // 通道ID:设备ID+通道号
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szName;	                    // 名称
            public UInt64 nRight;                     // 权限信息
            public int nChnlType;                  // 通道类型
            public int nStatus;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szChnlSN;			        // 互联编码SN
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szLatitude;		        // 纬度
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szLongitude;		        // 经度
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szMulticastIp;		        // 组播IP
            public int nMulticastPort;		    // 组播端口
        }

        // 设备状态
        public enum dpsdk_dev_status_e
        {
            DPSDK_DEV_STATUS_UNKNOW = 0,                         // 未知状态
            DPSDK_DEV_STATUS_ONLINE = 1,					     // 在线
            DPSDK_DEV_STATUS_OFFLINE = 2,					     // 离线

            // 废弃
            DPSDK_CORE_DEVICE_STATUS_ONLINE = DPSDK_DEV_STATUS_ONLINE,
            DPSDK_CORE_DEVICE_STATUS_OFFLINE = DPSDK_DEV_STATUS_OFFLINE,
        }

        // 设备类型，需要和web统一
        public enum dpsdk_dev_type_e
        {
            DEV_TYPE_ENC_BEGIN = 0,		// 编码设备
            DEV_TYPE_DVR = DEV_TYPE_ENC_BEGIN + 1,			// DVR
            DEV_TYPE_IPC = DEV_TYPE_ENC_BEGIN + 2,			// IPC
            DEV_TYPE_NVS = DEV_TYPE_ENC_BEGIN + 3,			// NVS
            DEV_TYPE_MCD = DEV_TYPE_ENC_BEGIN + 4,			// MCD
            DEV_TYPE_MDVR = DEV_TYPE_ENC_BEGIN + 5,			// MDVR
            DEV_TYPE_NVR = DEV_TYPE_ENC_BEGIN + 6,			// NVR
            DEV_TYPE_SVR = DEV_TYPE_ENC_BEGIN + 7,			// SVR
            DEV_TYPE_PCNVR = DEV_TYPE_ENC_BEGIN + 8,			// PCNVR，PSS自带的一个小型服务
            DEV_TYPE_PVR = DEV_TYPE_ENC_BEGIN + 9,			// PVR
            DEV_TYPE_EVS = DEV_TYPE_ENC_BEGIN + 10,			// EVS
            DEV_TYPE_MPGS = DEV_TYPE_ENC_BEGIN + 11,			// MPGS
            DEV_TYPE_SMART_IPC = DEV_TYPE_ENC_BEGIN + 12,			// SMART_IPC
            DEV_TYPE_SMART_TINGSHEN = DEV_TYPE_ENC_BEGIN + 13,			// 庭审主机
            DEV_TYPE_SMART_NVR = DEV_TYPE_ENC_BEGIN + 14,			// SMART_NVR
            DEV_TYPE_PRC = DEV_TYPE_ENC_BEGIN + 15,			// 防护舱
            DEV_TYPE_JT808 = DEV_TYPE_ENC_BEGIN + 18,			// 部标JT808
            DEV_TYPE_ALARM_STUB_VTA = DEV_TYPE_ENC_BEGIN + 25,          // VTA

            DEV_TYPE_ENC_END,

            DEV_TYPE_TVWALL_BEGIN = 100,
            DEV_TYPE_BIGSCREEN = DEV_TYPE_TVWALL_BEGIN + 1,		// 大屏
            DEV_TYPE_TVWALL_END,

            DEV_TYPE_DEC_BEGIN = 200,		// 解码设备
            DEV_TYPE_NVD = DEV_TYPE_DEC_BEGIN + 1,			// NVD
            DEV_TYPE_SNVD = DEV_TYPE_DEC_BEGIN + 2,			// SNVD
            DEV_TYPE_UDS = DEV_TYPE_DEC_BEGIN + 5,			// UDS
            DEV_TYPE_DEC_END,

            DEV_TYPE_MATRIX_BEGIN = 300,		// 矩阵设备
            DEV_MATRIX_M60 = DEV_TYPE_MATRIX_BEGIN + 1,		// M60
            DEV_MATRIX_NVR6000 = DEV_TYPE_MATRIX_BEGIN + 2,		// NVR6000
            DEV_TYPE_MATRIX_END,

            DEV_TYPE_IVS_BEGIN = 400,		// 智能设备
            DEV_TYPE_ISD = DEV_TYPE_IVS_BEGIN + 1,			// ISD 智能球
            DEV_TYPE_IVS_B = DEV_TYPE_IVS_BEGIN + 2,			// IVS-B 行为分析服务
            DEV_TYPE_IVS_V = DEV_TYPE_IVS_BEGIN + 3,			// IVS-V 视频质量诊断服务
            DEV_TYPE_IVS_FR = DEV_TYPE_IVS_BEGIN + 4,			// IVS-FR 人脸识别服务
            DEV_TYPE_IVS_PC = DEV_TYPE_IVS_BEGIN + 5,			// IVS-PC 人流量统计服务
            DEV_TYPE_IVS_M = DEV_TYPE_IVS_BEGIN + 6,			// IVS_M 主从跟踪智能盒
            DEV_TYPE_IVS_PC_BOX = DEV_TYPE_IVS_BEGIN + 7,			// IVS-PC 智能盒 
            DEV_TYPE_IVS_B_BOX = DEV_TYPE_IVS_BEGIN + 8,			// IVS-B 智能盒
            DEV_TYPE_IVS_M_BOX = DEV_TYPE_IVS_BEGIN + 9,			// IVS-M 盒子
            DEV_TYPE_IVS_PRC = DEV_TYPE_IVS_BEGIN + 10,			// 防护舱
            DEV_TYPE_IVS_END,

            DEV_TYPE_BAYONET_BEGIN = 500,		// -C相关设备
            DEV_TYPE_CAPTURE = DEV_TYPE_BAYONET_BEGIN + 1,		// 卡口设备
            DEV_TYPE_SPEED = DEV_TYPE_BAYONET_BEGIN + 2,		// 测速设备
            DEV_TYPE_TRAFFIC_LIGHT = DEV_TYPE_BAYONET_BEGIN + 3,		// 闯红灯设备
            DEV_TYPE_INCORPORATE = DEV_TYPE_BAYONET_BEGIN + 4,		// 一体化设备
            DEV_TYPE_PLATEDISTINGUISH = DEV_TYPE_BAYONET_BEGIN + 5,		// 车牌识别设备
            DEV_TYPE_VIOLATESNAPPIC = DEV_TYPE_BAYONET_BEGIN + 6,		// 违停检测设备
            DEV_TYPE_PARKINGSTATUSDEV = DEV_TYPE_BAYONET_BEGIN + 7,		// 车位检测设备
            DEV_TYPE_ENTRANCE = DEV_TYPE_BAYONET_BEGIN + 8,		// 出入口设备
            DEV_TYPE_VIOLATESNAPBALL = DEV_TYPE_BAYONET_BEGIN + 9,		// 违停抓拍球机
            DEV_TYPE_THIRDBAYONET = DEV_TYPE_BAYONET_BEGIN + 10,		// 第三方卡口设备
            DEV_TYPE_ULTRASONIC = DEV_TYPE_BAYONET_BEGIN + 11,		// 超声波车位检测器
            DEV_TYPE_FACE_CAPTURE = DEV_TYPE_BAYONET_BEGIN + 12,		// 人脸抓拍设备
            DEV_TYPE_ITC_SMART_NVR = DEV_TYPE_BAYONET_BEGIN + 13,		// 卡口智能NVR设备
            DEV_TYPE_BAYONET_END,

            DEV_TYPE_ALARM_BEGIN = 600,		// 报警设备
            DEV_TYPE_ALARMHOST = DEV_TYPE_ALARM_BEGIN + 1,			// 报警主机
            DEV_TYPE_ALARM_END,

            DEV_TYPE_DOORCTRL_BEGIN = 700,
            DEV_TYPE_DOORCTRL_DOOR = DEV_TYPE_DOORCTRL_BEGIN + 1,		// 门禁
            DEV_TYPE_DOORCTRL_END,

            DEV_TYPE_PE_BEGIN = 800,
            DEV_TYPE_PE_PE = DEV_TYPE_PE_BEGIN + 1,			// 动环
            DEV_TYPE_PE_AE6016 = DEV_TYPE_PE_BEGIN + 2,			// AE6016设备
            DEV_TYPE_PE_NVS = DEV_TYPE_PE_BEGIN + 3,			// 带动环功能的NVS设备
            DEV_TYPE_PE_END,

            DEV_TYPE_VOICE_BEGIN = 900,		// ip对讲
            DEV_TYPE_VOICE_MIKE = DEV_TYPE_VOICE_BEGIN + 1,
            DEV_TYPE_VOICE_NET = DEV_TYPE_VOICE_BEGIN + 2,
            DEV_TYPE_VOICE_END,

            DEV_TYPE_IP_BEGIN = 1000,		// IP设备（通过网络接入的设备）
            DEV_TYPE_IP_SCNNER = DEV_TYPE_IP_BEGIN + 1,			// 扫描枪
            DEV_TYPE_IP_SWEEP = DEV_TYPE_IP_BEGIN + 2,			// 地磅
            DEV_TYPE_IP_POWERCONTROL = DEV_TYPE_IP_BEGIN + 3,			// 电源控制器
            DEV_TYPE_IP_END,

            DEV_TYPE_MULTIFUNALARM_BEGIN = 1100,		// 多功能报警主机
            DEV_TYPE_VEDIO_ALARMHOST = DEV_TYPE_MULTIFUNALARM_BEGIN + 1,	// 视频报警主机
            DEV_TYPE_MULTIFUNALARM_END,

            DEV_TYPE_SLUICE_BEGIN = 1200,
            DEV_TYPE_SLUICE_DEV = DEV_TYPE_SLUICE_BEGIN + 1,		// 出入口道闸设备
            DEV_TYPE_SLUICE_PARKING = DEV_TYPE_SLUICE_BEGIN + 2,		// 停车场道闸设备
            DEV_TYPE_SLUICE_STOPBUFFER = DEV_TYPE_SLUICE_BEGIN + 3,		// 视频档车器
            DEV_TYPE_SLUICE_END,

            DEV_TYPE_ELECTRIC_BEGIN = 1300,
            DEV_TYPE_ELECTRIC_DEV = DEV_TYPE_ELECTRIC_BEGIN + 1,		// 电网设备
            DEV_TYPE_ELECTRIC_END,

            DEV_TYPE_LED_BEGIN = 1400,
            DEV_TYPE_LED_DEV = DEV_TYPE_LED_BEGIN + 1,			// LED屏设备
            DEV_TYPE_LED_END,

            DEV_TYPE_VIBRATIONFIBER_BEGIN = 1500,
            DEV_TYPE_VIBRATIONFIBER_DEV = DEV_TYPE_VIBRATIONFIBER_BEGIN + 1,// 震动光纤设备 
            DEV_TYPE_VIBRATIONFIBER_END,

            DEV_TYPE_PATROL_BEGIN = 1600,
            DEV_TYPE_PATROL_DEV = DEV_TYPE_PATROL_BEGIN + 1,		// 巡更棒设备
            DEV_TYPE_PATROL_SPOT = DEV_TYPE_PATROL_BEGIN + 2,		// 巡更点设备
            DEV_TYPE_PATROL_END,

            DEV_TYPE_SENTRY_BOX_BEGIN = 1700,
            DEV_TYPE_SENTRY_BOX_DEV = DEV_TYPE_SENTRY_BOX_BEGIN + 1,	// 哨位箱设备
            DEV_TYPE_SENTRY_BOX_END,

            DEV_TYPE_COURT_BEGIN = 1800,
            DEV_TYPE_COURT_DEV = DEV_TYPE_COURT_BEGIN + 1,			// 庭审设备
            DEV_TYPE_COURT_END,

            DEV_TYPE_VIDEO_TALK_BEGIN = 1900,
            DEV_TYPE_VIDEO_TALK_VTNC = DEV_TYPE_VIDEO_TALK_BEGIN + 1,
            DEV_TYPE_VIDEO_TALK_VTO = DEV_TYPE_VIDEO_TALK_BEGIN + 2,
            DEV_TYPE_VIDEO_TALK_VTH = DEV_TYPE_VIDEO_TALK_BEGIN + 3,
            DEV_TYPE_VIDEO_TALK_END,

            DEV_TYPE_BROADCAST_BEGIN = 2000,
            DEV_TYPE_BROADCAST_ITC_T6700R = DEV_TYPE_BROADCAST_BEGIN + 1,	// ITC_T6700R广播设备
            DEV_TYPE_BROADCAST_END,

            DEV_TYPE_VIDEO_RECORD_SERVER_BEGIN = 2100,
            DEV_TYPE_VIDEO_RECORD_SERVER_BNVR = DEV_TYPE_VIDEO_RECORD_SERVER_BEGIN + 1, // BNVR设备
            DEV_TYPE_VIDEO_RECORD_SERVER_OE = DEV_TYPE_VIDEO_RECORD_SERVER_BEGIN + 2, // 手术设备(operation equipment)
            DEV_TYPE_VIDEO_RECORD_SERVER_END,
        }

        // 设备厂商类型
        public enum dpsdk_device_factory_type_e
        {
            DPSDK_CORE_DEVICE_FACTORY_UNDEFINE = 0,					 // 未定义
            DPSDK_CORE_DEVICE_FACTORY_DAHUA = 1,					 // 大华			
            DPSDK_CORE_DEVICE_FACTORY_HIK = 2,					 // 海康
            DPSDK_CORE_DEVICE_FACTORY_H264 = 16,					 // 国标
            DPSDK_CORE_DEVICE_FACTORY_H3C = 17,					 // 华三
            DPSDK_CORE_DEVICE_FACTORY_XC = 18,					 // 信产
            DPSDK_CORE_DEVICE_FACTORY_LIYUAN = 19,					 // 立元
            DPSDK_CORE_DEVICE_FACTORY_BIT = 20,					 // 比特
            DPSDK_CORE_DEVICE_FACTORY_H3TS = 21,					 // 华三ts流
            DPSDK_CORE_DEVICE_FACTORY_TIANSHI = 36,					 // 天视上传的海康流
        }

        //设备信息
        [StructLayout(LayoutKind.Sequential)]
        public struct Device_Info_Ex_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szId;                                       // 设备ID
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szName;	                                    // 名称
            public dpsdk_device_factory_type_e nFactory;									// 厂商类型
            public int szModel;					                // 模式
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szUser;			                    // 用户名
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szPassword;		                    // 密码
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string szIP;					            // 设备IP
            public dpsdk_dev_type_e nDevType;						            // 设备type 参考dpsdk_dev_type_e 
            public int nPort;										// 设备端口
            public int szLoginType;				                // 登陆类型
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szRegID;				            // 主动注册设备ID
            public int nProxyPort;					                // 代理端口
            public int nUnitNum;					                // 单元数目--对于矩阵设备代表卡槽数
            public dpsdk_dev_status_e nStatus;									// 设备状态
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCN;				                // 设备序列号
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szSN;   	 		                // 互联编码SN
            public UInt64 nRight;						                // 权限信息(只有IP对讲设备中的话筒才有)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string szDevIP;					        // 设备真实IP
            public int nDevPort;					                // 设备真实port
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string dev_Maintainer;                    // 设备联系人
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string dev_MaintainerPh;                  // 设备联系人号码
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string dev_Location;                      // 设备所在位置
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string desc;                              // 设备描述 
            public int nEncChannelChildCount;						// 编码子通道个数
            public int iAlarmInChannelcount;						// 报警输入通道个数
            public int nSort;									    // 组织排序
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string szCallNum;				            // 设备呼叫号码
        }

        // 组织信息
        [StructLayout(LayoutKind.Sequential)]
        public struct Dep_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCoding;          // 节点code
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szDepName;         // 节点名称	
            public int nDepSort;		   // 组织排序
        }

        // 组织下的子组织，通道，设备信息
        [StructLayout(LayoutKind.Sequential)]
        public struct Get_Dep_Info_Ex_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCoding;           // 节点code
            public IntPtr nDepCount;			// 组织个数
            public IntPtr pDepInfo;			// Dep_Info_t组织信息，在外部创建，如果为NULL则只返回个数
            public IntPtr nDeviceCount;		// 设备个数
            public IntPtr pDeviceInfo;		// Device_Info_Ex_t设备信息
            public IntPtr nChannelCount;      // 通道个数
            public IntPtr pEncChannelnfo;		// Enc_Channel_Info_Ex_t通道信息
        }

        // 获取组织个数请求信息
        [StructLayout(LayoutKind.Sequential)]
        public struct Get_Dep_Count_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCoding;          // 节点code
            public UInt32 nDepCount;		   // 组织个数
            public UInt32 nDeviceCount;	   // 设备个数
            public UInt32 nChannelCount;     // 通道个数
        }

        public enum dpsdk_playback_speed_e
        {
            DPSDK_CORE_PB_NORMAL = 8,						  // 1倍数
            DPSDK_CORE_PB_FAST2 = DPSDK_CORE_PB_NORMAL * 2,   // 2倍数
            DPSDK_CORE_PB_FAST4 = DPSDK_CORE_PB_NORMAL * 4,   // 4倍数
            DPSDK_CORE_PB_FAST8 = DPSDK_CORE_PB_NORMAL * 8,   // 8倍数
            DPSDK_CORE_PB_FAST16 = DPSDK_CORE_PB_NORMAL * 16,  // 16倍数
            DPSDK_CORE_PB_SLOW2 = DPSDK_CORE_PB_NORMAL / 2,   // 1/2倍数
            DPSDK_CORE_PB_SLOW4 = DPSDK_CORE_PB_NORMAL / 4,   // 1/4倍数
            DPSDK_CORE_PB_SLOW8 = DPSDK_CORE_PB_NORMAL / 8,   // 1/8倍数
            DPSDK_CORE_PB_SLOW16 = 0,						  // 1/16倍数
        }

        // 单元类型
        public enum dpsdk_dev_unit_type_e
        {
            DPSDK_DEV_UNIT_UNKOWN,		                                        // 未知
            DPSDK_DEV_UNIT_ENC,			                                        // 编码
            DPSDK_DEV_UNIT_DEC,			                                        // 解码
            DPSDK_DEV_UNIT_ALARMIN,		                                        // 报警输入
            DPSDK_DEV_UNIT_ALARMOUT,	                                        // 报警输出
            DPSDK_DEV_UNIT_TVWALLIN,	                                        // TvWall输入
            DPSDK_DEV_UNIT_TVWALLOUT,	                                        // TvWall输出
            DPSDK_DEV_UNIT_DOORCTRL,	                                        // 门禁
            DPSDK_DEV_UNIT_VOICE,	                                          	// 对讲
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Record_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;          // 通道ID
            public uint nBegin;                 // 录像起始
            public uint nCount;                 // 请求录像数
            public uint nRetCount;              //实际返回个数
            public IntPtr pSingleRecord;
        }
        public struct Single_Record_Info_t
        {
            public uint nFileIndex;                                 // 文件索引
            public dpsdk_recsource_type_e nSource;                            // 录像源类型
            public dpsdk_record_type_e nRecordType;                        // 录像类型
            public UInt64 uBeginTime;                                 // 起始时间
            public UInt64 uEndTime;                                   // 结束时间
            public UInt64 uLength;                                    // 文件大小
        }

        public struct Query_Record_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;          // 通道ID
            public dpsdk_check_right_e nRight;                                 // 是否检查通道权限
            public dpsdk_recsource_type_e nSource;                            // 录像源类型
            public dpsdk_record_type_e nRecordType;                            // 录像类型
            public UInt64 uBeginTime;                                 // 起始时间
            public UInt64 uEndTime;                                   // 结束时间
        }
        public enum dpsdk_record_type_e
        {
            DPSDK_CORE_PB_RECORD_UNKONWN = 0,                    // 全部录像
            DPSDK_CORE_PB_RECORD_MANUAL = 1,                    // 手动录像
            DPSDK_CORE_PB_RECORD_ALARM = 2,                    // 报警录像
            DPSDK_CORE_PB_RECORD_MOTION_DETECT = 3,                    // 移动侦测
            DPSDK_CORE_PB_RECORD_VIDEO_LOST = 4,                    // 视频丢失
            DPSDK_CORE_PB_RECORD_VIDEO_SHELTER = 5,                    // 视频遮挡
            DPSDK_CORE_PB_RECORD_TIMER = 6,                    // 定时录像
            DPSDK_CORE_PB_RECORD_ALL_DAY = 7,                    // 全天录像
            DPSDK_CORE_PB_RECORD_CARD = 25,                   // 卡号录像
        }

        //录像文件类型
        public enum file_type_e
        {
            FILE_TYPE_NONE = -1,        //原始格式dav
            FILE_TYPE_TS = 0,
            FILE_TYPE_PS = 1,
            FILE_TYPE_RTP = 2,
            FILE_TYPE_MP4 = 3,          //mp4格式
            FILE_TYPE_GDPS = 4,
            FILE_TYPE_GAYSPS = 5,
            FILE_TYPE_FLV = 6,          //FLV格式
            FILE_TYPE_ASF_FILE = 7,
            FILE_TYPE_ASF_STREAM = 8,
            FILE_TYPE_FLV_STREAM = 9,
            FILE_TYPE_AVI = 10,         //AVI格式
        }


        // 摄像头类型
        public enum dpsdk_camera_type_e
        {
            CAMERA_TYPE_NORMAL = 1,		             // 枪机
            CAMERA_TYPE_SD = 2,           			 // 球机
            CAMERA_TYPE_HALFSD = 3,                 	 // 半球
        }

        //本地录像回放信息
        [StructLayout(LayoutKind.Sequential)]
        public struct Get_Record_Local_Info_t
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] szFilePath;  // 文件全路径和文件名 
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            //public string szFilePath;			// 通道ID:设备ID+通道号
        }

        // 编码通道信息
        [StructLayout(LayoutKind.Sequential)]
        public struct Enc_Channel_Info_t
        {
            public dpsdk_camera_type_e nCameraType;	// 类型，参见CameraType_e
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szId;			// 通道ID:设备ID+通道号
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szName;	        // 名称
            public int nSort;			// 组织排序
        }

        //订阅卡口过车(或区间测速)信息请求信息
        [StructLayout(LayoutKind.Sequential)]
        public struct Subscribe_Bay_Car_Info_t
        {
            public int nChnlCount;             // 订阅通道的数量 nChnlCount =0 pEncChannelnfo = null表示订阅/取消订阅全部
            public IntPtr pEncChannelnfo;         // 通道信息
            public int nSubscribeFlag;          // 订阅标记。0:取消订阅，1：订阅
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Pic_Url_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string szPicUrl;                                 // 图片URL
        }

        //违章报警信息
        [StructLayout(LayoutKind.Sequential)]
        public struct Traffic_Alarm_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;                 // 通道ID
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string nPtsIp;                     // pts内网
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string nPtsIpy;                    // pts外网
            public int nPicPort;                   // pic内网port
            public int nPicPorty;                  // pic外网port
            public dpsdk_alarm_type_e type;                       // 违章类型
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szCarNum;                   // 车牌
            public int nLicentype;                 // 车牌颜色类型
            public int nCarColor;					// 车身颜色
            public int nCarLogo;					// 车标类型
            public int nWay;						// 车道号
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 6)]
            public Pic_Url_t[] szPicUrl;                   // 图片URL
            public UInt32 nPicGroupStoreID;           // 图片组存储ID
            public int bNeedStore;					// 是否需存盘 0：不需存盘 1：需存盘
            public int bStored;					// 是否已存盘 0：未存盘 1：已存盘int	
            public int nAlarmLevel;				// 报警级别
            public int nAlarmTime;                 // 报警发生时间,精度为秒，值为time(NULL)值

            //新增字段
            public int nChannel;                   // 通道
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szDeviceId;                 // 设备ID
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szDeviceName;               // 设备名称
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szDeviceChnName;            // 通道名称
            public int nCarType;                   // 车类型
            public int nCarSpeed;                  // 车速
            public int nCarLen;                    // 车身长度单位
            public int nCardirect;                 // 行车方向
            public int nMaxSpeed;                  // 限制速度
            public int nMinSpeed;                  // 最低限制速度
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] nRtPlate;                   // 车牌坐标
        }

        //卡口过车信息
        [StructLayout(LayoutKind.Sequential)]
        public struct Bay_Car_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szDeviceId;              //设备ID           
            public int nDevChnId;               //通道号
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szChannelId;             //通道ID
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szDeviceName;            //设备名称
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szDeviceChnName;         //通道名称
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szCarNum;                //车牌号
            public int nCarNumType;             //车牌类型
            public int nCarNumColor;            //车牌颜色
            public int nCarSpeed;               //车速
            public int nCarType;                //车类型
            public int nCarColor;               //车颜色
            public int nCarLen;                 //车长
            public int nCarDirect;              //行车方向
            public int nWayId;                  //车道号
            public UInt64 lCaptureTime;            //抓图时间，精确到毫秒级
            public UInt32 lPicGroupStoreID;        //图片组存盘ID 
            public int nIsNeedStore;            //图片组是否需要存盘 0：不需要 1：需要
            public int nIsStoraged;             //图片组是否已经存盘 0：未完成存盘 1：已成功存盘，保留，目前未使用
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szCaptureOrg;            //通缉机构
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szOptOrg;                //布控机构
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szOptUser;               //布控人员
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szOptNote;               //备注信息
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szImg0Path;              //图片路径
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szImg1Path;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szImg2Path;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szImg3Path;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szImg4Path;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szImg5Path;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szImgPlatePath;		    //车牌小图片坐标
            public int icarLog;			        //车标类型
            public int iPlateLeft;
            public int iPlateRight;
            public int iPlateTop;
            public int iPlateBottom;
        }

        //预置点操作
        [StructLayout(LayoutKind.Sequential)]
        public struct Ptz_Prepoint_Operation_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;                                               // 通道ID
            public dpsdk_ptz_prepoint_cmd_e nCmd;									// 是否检测权限
            public Ptz_Single_Prepoint_Info_t pPoint;								// 录像流请求类型
        }

        //预置点操作类型
        public enum dpsdk_ptz_prepoint_cmd_e
        {
            DPSDK_CORE_PTZ_PRESET_LOCATION = 1,			        // 预置点定位
            DPSDK_CORE_PTZ_PRESET_ADD = 2,					    // 预置点增加
            DPSDK_CORE_PTZ_PRESET_DEL = 3,					    // 预置点删除
        }

        // 日志等级
        public enum dpsdk_log_level_e
        {
            DPSDK_LOG_LEVEL_DEBUG = 2,					// 调试
            DPSDK_LOG_LEVEL_INFO = 4,					// 信息
            DPSDK_LOG_LEVEL_ERROR = 6,					// 错误
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Get_RecordStream_Time_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;                                                           // 通道ID
            public dpsdk_check_right_e nRight;									// 是否检测权限
            public dpsdk_pb_mode_e nMode;									// 录像流请求类型

            public dpsdk_recsource_type_e nSource;							    // 录像源类型
            public UInt64 uBeginTime;								// 播放起始
            public UInt64 uEndTime;								// 播放结束
            public int nTrackID;                               // 拉流TrackID，默认0
        }

        public enum dpsdk_pb_mode_e
        {
            DPSDK_CORE_PB_MODE_NORMAL = 1,					     // 回放
            DPSDK_CORE_PB_MODE_DOWNLOAD = 2,					 // 下载
        }

        public enum dpsdk_recsource_type_e
        {
            DPSDK_CORE_PB_RECSOURCE_DEVICE = 2,	    			     // 设备录像
            DPSDK_CORE_PB_RECSOURCE_PLATFORM = 3,					 // 平台录像
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Get_RealStream_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;                                                           // 通道ID
            public dpsdk_check_right_e nRight;										// 是否检测权限
            public dpsdk_stream_type_e nStreamType;								// 码流类型 参考dpsdk_stream_type_e；预览接口，此参数为分割数，参考dpsdk_stream_real_video_slite_e
            public dpsdk_media_type_e nMediaType;									// 媒体类型
            public dpsdk_trans_type_e nTransType;									// 传输类型
            public int nTrackID;                                   // 拉流TrackID，默认0
        }

        public enum dpsdk_check_right_e
        {
            DPSDK_CORE_CHECK_RIGHT = 0,					    // 检查
            DPSDK_CORE_NOT_CHECK_RIGHT = 1,					// 不检查
        }

        // 码流类型
        public enum dpsdk_stream_type_e
        {
            DPSDK_CORE_STREAMTYPE_MAIN = 1,					 // 主码流
            DPSDK_CORE_STREAMTYPE_SUB = 2,					 // 辅码流
        }

        // 媒体类型
        public enum dpsdk_media_type_e
        {
            DPSDK_CORE_MEDIATYPE_VIDEO = 1,					 // 视频
            DPSDK_CORE_MEDIATYPE_AUDI = 2,					 // 音频
            DPSDK_CORE_MEDIATYPE_ALL = 3,					 // 音频 + 视频
        }

        // 传输类型
        public enum dpsdk_trans_type_e
        {
            DPSDK_CORE_TRANSTYPE_UDP = 0,					 // UDP
            DPSDK_CORE_TRANSTYPE_TCP = 1,					 // TCP
        }

        /// <summary>
        /// 电视墙列表
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TvWall_List_Info_t
        {
            /// <summary>
            /// 个数
            /// </summary>
            public int nCount;

            /// <summary>
            /// 电视墙信息列表
            /// </summary>
            public IntPtr TvWallInfo;
        }

        /// <summary>
        /// 电视墙信息
        /// </summary>
        public struct TvWall_Info_t
        {
            /// <summary>
            /// 电视墙ID
            /// </summary>
            public uint nTvWallId;
            /// <summary>
            /// 状态
            /// </summary>
            public uint nState;

            /// <summary>
            /// 名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szName;
        }


        /// <summary>
        /// 电视墙列表
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TvWall_Layout_Info_t
        {
            /// <summary>
            /// 电视墙ID
            /// </summary>
            public uint nTvWallId;

            /// <summary>
            /// 屏数量
            /// </summary>
            public uint nCount;

            /// <summary>
            /// 电视墙信息列表
            /// </summary>
            public IntPtr pScreenInfo;
        }

        /// <summary>
        /// 电视墙信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TvWall_Screen_Info_t
        {
            /// <summary>
            /// 屏ID
            /// </summary>
            public uint nScreenId;

            /// <summary>
            /// 屏的名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szName;

            /// <summary>
            /// 屏幕绑定的解码器ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szDecoderId;

            /// <summary>
            /// 左边距
            /// </summary>
            public Single fLeft;

            /// <summary>
            /// 左边距
            /// </summary>
            public Single fTop;

            /// <summary>
            /// 左边距
            /// </summary>
            public Single fWidth;

            /// <summary>
            /// 左边距
            /// </summary>
            public Single fHeight;

            /// <summary>
            /// 是否绑定解码器
            /// </summary>
            public byte bBind;

            /// <summary>
            /// 是否是融合屏
            /// </summary>
            public byte bCombine;
        }


        /// <summary>
        /// 电视墙屏分割（分割后窗口ID从0开始，从左到右，从上到下依次递增，使用者自己维护）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TvWall_Screen_Split_t
        {
            /// <summary>
            /// 电视墙ID
            /// </summary>
            public uint nTvWallId;
            /// <summary>
            /// 屏ID
            /// </summary>
            public uint nScreenId;
            /// <summary>
            /// 分割数量
            /// </summary>
            public tvwall_screen_split_caps enSplitNum;
        }

        public enum tvwall_screen_split_caps
        {
            Screen_Split_1 = 1,
            Screen_Split_4 = 4,
            Screen_Split_9 = 9,
            Screen_Split_16 = 16,
        }

        // 电视墙屏开窗信息
        [StructLayout(LayoutKind.Sequential)]
        public struct TvWall_Screen_Open_Window_t
        {
            public uint nTvWallId;									// 电视墙ID
            public uint nScreenId;									// 屏ID
            public Single fLeft;									// 窗口左边距
            public Single fTop;										// 窗口上边距
            public Single fWidth;									// 窗口宽度
            public Single fHeight;									// 窗口高度
            //下面两个是输出结果
            public uint nWindowId;									// 窗口ID
            public uint nZorder;									// 窗口Z序
        }

        // 电视墙屏窗口移动
        [StructLayout(LayoutKind.Sequential)]
        public struct TvWall_Screen_Move_Window_t
        {
            public UInt32 nTvWallId;                                    // 电视墙ID
            public UInt32 nScreenId;                                    // 屏ID
            public UInt32 nWindowId;                                    // 窗口ID
            public float fLeft;                                     // 窗口左边距
            public float fTop;                                      // 窗口上边距
            public float fWidth;                                        // 窗口宽度
            public float fHeight;									// 窗口高度
        }

        // 电视墙屏窗口置顶
        [StructLayout(LayoutKind.Sequential)]
        public struct TvWall_Screen_Set_Top_Window_t
        {
            public UInt32 nTvWallId;                                    // 电视墙ID
            public UInt32 nScreenId;                                    // 屏ID
            public UInt32 nWindowId;									// 窗口ID
        }
        /// <summary>
        /// 预置点信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Ptz_Prepoint_Info_t
        {
            /// <summary>
            /// 通道ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;

            /// <summary>
            /// 预置点数量
            /// </summary>
            public int nCount;

            /// <summary>
            /// 预置点信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 300)]
            public Ptz_Single_Prepoint_Info_t[] pPoints;
        }

        /// <summary>
        /// 预置点信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Ptz_Single_Prepoint_Info_t
        {
            /// <summary>
            /// 预置点编号
            /// </summary>
            public uint nCode;

            /// <summary>
            /// 名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Alarm_Enable_Info_t
        {
            public int nCount;										    // 报警布控个数
            //[MarshalAs(UnmanagedType.ByValArray)]
            public IntPtr pSources;				// 报警内容
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Alarm_Single_Enable_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szAlarmDevId;		                                            // 报警设备ID
            public int nVideoNo;						// 视频通道 视频相关的报警 -1接收所有通道
            public int nAlarmInput;					// 报警输入通道 报警输入相关的报警 -1接收所有通道
            public dpsdk_alarm_type_e nAlarmType;						// 报警类型
        }

        public enum dpsdk_alarm_type_e
        {
            DPSDK_CORE_ALARM_TYPE_Unknown = 0,				                      // 未知
            DPSDK_CORE_ALARM_TYPE_VIDEO_LOST = 1,								 // 视频丢失
            DPSDK_CORE_ALARM_TYPE_EXTERNAL_ALARM = 2,							 // 外部报警
            DPSDK_CORE_ALARM_TYPE_MOTION_DETECT,								 // 移动侦测
            DPSDK_CORE_ALARM_TYPE_VIDEO_SHELTER,								 // 视频遮挡
            DPSDK_CORE_ALARM_TYPE_DISK_FULL,									 // 硬盘满
            DPSDK_CORE_ALARM_TYPE_DISK_FAULT,									 // 硬盘故障
            DPSDK_CORE_ALARM_TYPE_FIBER,										 // 光纤报警
            DPSDK_CORE_ALARM_TYPE_GPS,											 // GPS信息
            DPSDK_CORE_ALARM_TYPE_3G,											 // 3G
            DPSDK_CORE_ALARM_TYPE_STATUS_RECORD,								 // 设备录像状态
            DPSDK_CORE_ALARM_TYPE_STATUS_DEVNAME,								 // 设备名
            DPSDK_CORE_ALARM_TYPE_STATUS_DISKINFO,								 // 硬盘信息
            DPSDK_CORE_ALARM_TYPE_IPC_OFF,										 // 前端IPC断线

            //门禁
            DPSDK_CORE_ALARM_DOOR_BEGIN = 40,		    	     // 门禁设备报警起始
            DPSDK_CORE_ALARM_FORCE_CARD_OPENDOOR = 41,				     // 胁迫刷卡开门
            DPSDK_CORE_ALARM_VALID_PASSWORD_OPENDOOR = 42,				     // 合法密码开门
            DPSDK_CORE_ALARM_INVALID_PASSWORD_OPENDOOR = 43,				     // 非法密码开门
            DPSDK_CORE_ALARM_FORCE_PASSWORD_OPENDOOR = 44,				     // 胁迫密码开门
            DPSDK_CORE_ALARM_VALID_FINGERPRINT_OPENDOOR = 45,			         // 合法指纹开门
            DPSDK_CORE_ALARM_INVALID_FINGERPRINT_OPENDOOR = 46,				 // 非法指纹开门
            DPSDK_CORE_ALARM_FORCE_FINGERPRINT_OPENDOOR = 47,				     // 胁迫指纹开门

            DPSDK_CORE_ALARM_TYPE_VALID_CARD_READ = 51,				     // 合法刷卡/开门
            DPSDK_CORE_ALARM_TYPE_INVALID_CARD_READ,							 // 非法刷卡/开门
            DPSDK_CORE_ALARM_TYPE_DOOR_MAGNETIC_ERROR,							 // 门磁报警
            DPSDK_CORE_ALARM_TYPE_DOOR_BREAK,									 // 破门报警和开门超时报警
            DPSDK_CORE_ALARM_TYPE_DOOR_ABNORMAL_CLOSED,							 // 门非正常关闭
            DPSDK_CORE_ALARM_TYPE_DOOR_NORMAL_CLOSED,							 // 门正常关闭
            DPSDK_CORE_ALARM_TYPE_DOOR_OPEN,									 // 门打开

            DPSDK_CORE_ALARM_DOOR_OPEN_TIME_OUT_BEG = 60,
            DPSDK_CORE_ALARM_DOOR_OPEN_TIME_OUT_END = 70,

            //报警主机
            DPSDK_CORE_ALARM_TYPE_ALARM_CONTROL_ALERT = 81,				     // 报警主机报警
            DPSDK_CORE_ALARM_TYPE_FIRE_ALARM,									 // 火警
            DPSDK_CORE_ALARM_TYPE_ZONE_DISABLED,								 // 防区失效
            DPSDK_CORE_ALARM_TYPE_BATTERY_EMPTY,								 // 电池没电

            DPSDK_CORE_ALARM_FILESYSTEM = 100,					 // 文件系统
            DPSDK_CORE_ALARM_RAID_FAULT,										 // raid故障
            DPSDK_CORE_ALARM_RECORDCHANNELFUNCTION_ABNORMAL,					 // 录像通道功能异常
            DPSDK_CORE_SVR_HARDDISK_STATUS,										 // 硬盘状态
            DPSDK_CORE_ALARM_RECORD_REPAIR,										 // 录像补全 -P3.0

            //-M的相关报警在这里添加
            DPSDK_CORE_ALARM_MOTOR_BEGIN = 200,
            DPSDK_CORE_ALARM_OVERSPEED_OCCURE = 201, 			     // 超速报警产生
            DPSDK_CORE_ALARM_OVERSPEED_DISAPPEAR,  								 // 超速报警消失
            DPSDK_CORE_ALARM_DRIVEROUT_DRIVERALLOW,								 // 驶出行区
            DPSDK_CORE_ALARM_DRIVERIN_DRIVERALLOW,								 // 驶入行区
            DPSDK_CORE_ALARM_DRIVEROUT_FORBIDDRIVE,								 // 驶出禁入区
            DPSDK_CORE_ALARM_DRIVERIN_FORBIDDRIVE,								 // 驶入禁入区
            DPSDK_CORE_ALARM_DRIVEROUT_LOADGOODS,								 // 驶出装货区
            DPSDK_CORE_ALARM_DRIVERIN_LOADGOODS,								 // 驶入装货区
            DPSDK_CORE_ALARM_DRIVEROUT_UNLOADGOODS,								 // 驶出卸货区
            DPSDK_CORE_ALARM_DRIVERIN_UNLOADGOODS,								 // 驶入卸货区
            DPSDK_CORE_ALARM_CAR_OVER_LOAD,										 // 超载
            DPSDK_CORE_ALARM_SPEED_SOON_ZERO,									 // 急刹车
            DPSDK_CORE_ALARM_3GFLOW,											 // 3G流量
            DPSDK_CORE_ALARM_AAC_POWEROFF,										 // ACC断电报警
            DPSDK_CORE_ALARM_SPEEDLIMIT_LOWERSPEED,								 // 限速报警 LowerSpeed
            DPSDK_CORE_ALARM_SPEEDLIMIT_UPPERSPEED,								 // 限速报警 UpperSpeed 
            DPSDK_CORE_ALARM_VEHICLEINFOUPLOAD_CHECKIN,							 // 车载自定义信息上传 CheckIn
            DPSDK_CORE_ALARM_VEHICLEINFOUPLOAD_CHECKOUT,						 // 车载自定义信息上传 CheckOut
            DPSDK_CORE_ALARM_GAS_LOWLEVEL = 236,				 // 油耗报警
            DPSDK_CORE_ALARM_MOTOR_END = 300,

            //智能报警
            DPSDK_CORE_ALARM_IVS_ALARM_BEGIN = 300,				 // 智能设备报警类型在dhnetsdk.h基础上+300（DMS服务中添加）
            DPSDK_CORE_ALARM_IVS_ALARM,											 // 智能设备报警
            DPSDK_CORE_ALARM_CROSSLINEDETECTION,								 // 警戒线事件
            DPSDK_CORE_ALARM_CROSSREGIONDETECTION,								 // 警戒区事件
            DPSDK_CORE_ALARM_PASTEDETECTION,									 // 贴条事件
            DPSDK_CORE_ALARM_LEFTDETECTION,										 // 物品遗留事件
            DPSDK_CORE_ALARM_STAYDETECTION,										 // 停留事件
            DPSDK_CORE_ALARM_WANDERDETECTION,									 // 徘徊事件
            DPSDK_CORE_ALARM_PRESERVATION,										 // 物品保全事件
            DPSDK_CORE_ALARM_MOVEDETECTION,										 // 移动事件
            DPSDK_CORE_ALARM_TAILDETECTION,										 // 尾随事件
            DPSDK_CORE_ALARM_RIOTERDETECTION,									 // 聚众事件
            DPSDK_CORE_ALARM_FIREDETECTION,										 // 火警事件
            DPSDK_CORE_ALARM_SMOKEDETECTION,									 // 烟雾报警事件
            DPSDK_CORE_ALARM_FIGHTDETECTION,									 // 斗殴事件
            DPSDK_CORE_ALARM_FLOWSTAT,											 // 流量统计事件
            DPSDK_CORE_ALARM_NUMBERSTAT,										 // 数量统计事件
            DPSDK_CORE_ALARM_CAMERACOVERDDETECTION,								 // 摄像头覆盖事件
            DPSDK_CORE_ALARM_CAMERAMOVEDDETECTION,								 // 摄像头移动事件
            DPSDK_CORE_ALARM_VIDEOABNORMALDETECTION,							 // 视频异常事件
            DPSDK_CORE_ALARM_VIDEOBADDETECTION,									 // 视频损坏事件
            DPSDK_CORE_ALARM_TRAFFICCONTROL,									 // 交通管制事件
            DPSDK_CORE_ALARM_TRAFFICACCIDENT,									 // 交通事故事件
            DPSDK_CORE_ALARM_TRAFFICJUNCTION,									 // 交通路口事件
            DPSDK_CORE_ALARM_TRAFFICGATE,										 // 交通卡口事件
            DPSDK_CORE_ALARM_TRAFFICSNAPSHOT,									 // 交通抓拍事件
            DPSDK_CORE_ALARM_FACEDETECT,										 // 人脸检测事件 
            DPSDK_CORE_ALARM_TRAFFICJAM,										 // 交通拥堵事件

            DPSDK_CORE_ALARM_TRAFFIC_RUNREDLIGHT = 0x00000100 + 300,	 // 交通违章-闯红灯事件
            DPSDK_CORE_ALARM_TRAFFIC_OVERLINE = 0x00000101 + 300,	 // 交通违章-压车道线事件
            DPSDK_CORE_ALARM_TRAFFIC_RETROGRADE = 0x00000102 + 300,	 // 交通违章-逆行事件
            DPSDK_CORE_ALARM_TRAFFIC_TURNLEFT = 0x00000103 + 300,	 // 交通违章-违章左转
            DPSDK_CORE_ALARM_TRAFFIC_TURNRIGHT = 0x00000104 + 300,	 // 交通违章-违章右转
            DPSDK_CORE_ALARM_TRAFFIC_UTURN = 0x00000105 + 300,	 // 交通违章-违章掉头
            DPSDK_CORE_ALARM_TRAFFIC_OVERSPEED = 0x00000106 + 300,	 // 交通违章-超速
            DPSDK_CORE_ALARM_TRAFFIC_UNDERSPEED = 0x00000107 + 300,	 // 交通违章-低速
            DPSDK_CORE_ALARM_TRAFFIC_PARKING = 0x00000108 + 300,	 // 交通违章-违章停车
            DPSDK_CORE_ALARM_TRAFFIC_WRONGROUTE = 0x00000109 + 300,	 // 交通违章-不按车道行驶
            DPSDK_CORE_ALARM_TRAFFIC_CROSSLANE = 0x0000010A + 300,	 // 交通违章-违章变道
            DPSDK_CORE_ALARM_TRAFFIC_OVERYELLOWLINE = 0x0000010B + 300,	 // 交通违章-压黄线
            DPSDK_CORE_ALARM_TRAFFIC_DRIVINGONSHOULDER = 0x0000010C + 300,	 // 交通违章-路肩行驶事件  
            DPSDK_CORE_ALARM_TRAFFIC_YELLOWPLATEINLANE = 0x0000010E + 300,	 // 交通违章-黄牌车占道事件
            DPSDK_CORE_ALARM_CROSSFENCEDETECTION = 0x0000011F + 300,	 // 翻越围栏事件
            DPSDK_CORE_ALARM_ELECTROSPARKDETECTION = 0X00000110 + 300,	 // 电火花事件
            DPSDK_CORE_ALARM_TRAFFIC_NOPASSING = 0x00000111 + 300,	 // 交通违章-禁止通行事件
            DPSDK_CORE_ALARM_ABNORMALRUNDETECTION = 0x00000112 + 300,	 // 异常奔跑事件
            DPSDK_CORE_ALARM_RETROGRADEDETECTION = 0x00000113 + 300,	 // 人员逆行事件
            DPSDK_CORE_ALARM_INREGIONDETECTION = 0x00000114 + 300,	 // 区域内检测事件
            DPSDK_CORE_ALARM_TAKENAWAYDETECTION = 0x00000115 + 300,	 // 物品搬移事件
            DPSDK_CORE_ALARM_PARKINGDETECTION = 0x00000116 + 300,	 // 非法停车事件
            DPSDK_CORE_ALARM_FACERECOGNITION = 0x00000117 + 300,	 // 人脸识别事件
            DPSDK_CORE_ALARM_TRAFFIC_MANUALSNAP = 0x00000118 + 300,	 // 交通手动抓拍事件
            DPSDK_CORE_ALARM_TRAFFIC_FLOWSTATE = 0x00000119 + 300,	 // 交通流量统计事件
            DPSDK_CORE_ALARM_TRAFFIC_STAY = 0x0000011A + 300,	 // 交通滞留事件
            DPSDK_CORE_ALARM_TRAFFIC_VEHICLEINROUTE = 0x0000011B + 300,	 // 有车占道事件
            DPSDK_CORE_ALARM_MOTIONDETECT = 0x0000011C + 300,	 // 视频移动侦测事件
            DPSDK_CORE_ALARM_LOCALALARM = 0x0000011D + 300,	 // 外部报警事件
            DPSDK_CORE_ALARM_PRISONERRISEDETECTION = 0X0000011E + 300,	 // 看守所囚犯起身事件
            DPSDK_CORE_ALARM_IVS_B_ALARM_END,									 // 以上报警都为IVS_B服务的报警类型，与SDK配合
            DPSDK_CORE_ALARM_VIDEODIAGNOSIS = 0X00000120 + 300,	 // 视频诊断结果事件

            //新增智能报警start
            DPSDK_CORE_ALARM_IVS_AUDIO_ABNORMALDETECTION = 0x00000126 + 300,		// 声音异常检测
            DPSDK_CORE_ALARM_CLIMB_UP = 0x00000128 + 300,		// 攀高检测 
            DPSDK_CORE_ALARM_LEAVE_POST = 0x00000129 + 300,		// 离岗检测
            //新增智能报警End

            DPSDK_CORE_ALARM_IVS_V_ALARM = DPSDK_CORE_ALARM_VIDEODIAGNOSIS,	// 
            DPSDK_CORE_ALARM_IVS_ALARM_END = 1000,				 // 智能设备报警类型的范围为300-1000
            DPSDK_CORE_ALARM_OSD,												 // osd信息
            DPSDK_CORE_ALARM_CROSS_INFO,										 // 十字路口

            DPSDK_CORE_ALARM_CLIENT_ALARM_BEGIN = 1100,				 // 客户端平台报警开始
            DPSDK_CORE_ALARM_CLIENT_DERELICTION,								 // 遗留检测[交通事件-抛洒物]
            DPSDK_CORE_ALARM_CLIENT_RETROGRADATION,								 // 逆行 [交通事件]
            DPSDK_CORE_ALARM_CLIENT_OVERSPEED,									 // 超速  [交通事件]
            DPSDK_CORE_ALARM_CLIENT_LACK_ALARM,									 // 欠速  [交通事件]
            DPSDK_CORE_ALARM_CLIENT_FLUX_COUNT,									 // 流量统计[交通事件]
            DPSDK_CORE_ALARM_CLIENT_PARKING,									 // 停车检测[交通事件]
            DPSDK_CORE_ALARM_CLIENT_PASSERBY,									 // 行人检测[交通事件]
            DPSDK_CORE_ALARM_CLIENT_JAM,										 // 拥堵检测[交通事件]
            DPSDK_CORE_ALARM_CLIENT_AREA_INBREAK,								 // 特殊区域入侵
            DPSDK_CORE_ALARM_CLIENT_ALARM_END = 1200,				 // 客户端平台报警结束

            //动环(PE)报警-(SCS_ALARM_SWITCH_START 取名直接来自SCS动环文档)
            //系统工程动环增加报警类型ALARM_SCS_BEGIN
            //开关量，不可控
            ALARM_SCS_SWITCH_START = 1800,
            ALARM_SCS_INFRARED,											// 红外对射告警
            ALARM_SCS_SMOKE,											// 烟感告警
            ALARM_SCS_WATER,                							// 水浸告警
            ALARM_SCS_COMPRESSOR,           							// 压缩机故障告警
            ALARM_SCS_OVERLOAD,             							// 过载告警
            ALARM_SCS_BUS_ANOMALY,          							// 母线异常
            ALARM_SCS_LIFE,                 							// 寿命告警
            ALARM_SCS_SOUND,                							// 声音告警
            ALARM_SCS_TIME,                 							// 时钟告警
            ALARM_SCS_FLOW_LOSS,            							// 气流丢失告警
            ALARM_SCS_FUSING,               							// 熔断告警
            ALARM_SCS_BROWN_OUT,            							// 掉电告警
            ALARM_SCS_LEAKING,              							// 漏水告警
            ALARM_SCS_JAM_UP,               							// 堵塞告警
            ALARM_SCS_TIME_OUT,             							// 超时告警
            ALARM_SCS_REVERSE_ORDER,        							// 反序告警
            ALARM_SCS_NETWROK_FAILURE,      							// 组网失败告警
            ALARM_SCS_UNIT_CODE_LOSE,       							// 机组码丢失告警
            ALARM_SCS_UNIT_CODE_DISMATCH,   							// 机组码不匹配告警
            ALARM_SCS_FAULT,                							// 故障告警
            ALARM_SCS_UNKNOWN,              							// 未知告警
            ALARM_SCS_CUSTOM,               							// 自定义告警
            ALARM_SCS_NOPERMISSION,         							// 无权限告警
            ALARM_SCS_INFRARED_DOUBLE,      							// 红外双鉴告警
            ALARM_SCS_ELECTRONIC_FENCE,     							// 电子围栏告警
            ALARM_SCS_UPS_MAINS,            							// 市电正常市电异常
            ALARM_SCS_UPS_BATTERY,          							// 电池正常电池异常
            ALARM_SCS_UPS_POWER_SUPPLY,     							// UPS正常输出旁路供电
            ALARM_SCS_UPS_RUN_STATE,        							// UPS正常UPS故障
            ALARM_SCS_UPS_LINE_STYLE,       							// UPS类型为在线式UPS类  型为后备式
            ALARM_SCS_XC,                   							// 小车
            ALARM_SCS_DRQ,                  							// 断路器
            ALARM_SCS_GLDZ,                 							// 隔离刀闸
            ALARM_SCS_JDDZ,                								// 接地刀闸
            ALARM_SCS_IN_END,											// 请注意这个值，不用把他作为判断值；只标记说“开关量，不可控”结束；
            //因为接下来的“开关量，可控”没有开始标记如ALARM_SCS_DOOR_START

            //开关量，可控，请注意接下来的ALARM_SCS_DOOR_SWITCH这个不能作为BEGIN用
            ALARM_SCS_DOOR_SWITCH = 1850,					// 门禁控制器开关告警
            ALARM_SCS_UPS_SWITCH,										// UPS开关告警,
            ALARM_SCS_DBCB_SWITCH,          							// 配电柜开关告警
            ALARM_SCS_ACDT_SWITCH,          							// 空调开关告警
            ALARM_SCS_DTPW_SWITCH,          							// 直流电源开关告警
            ALARM_SCS_LIGHT_SWITCH,         							// 灯光控制器开关告警
            ALARM_SCS_FAN_SWITCH,           							// 风扇控制器开关告警
            ALARM_SCS_PUMP_SWITCH,          							// 水泵开关告警
            ALARM_SCS_BREAKER_SWITCH,       							// 刀闸开关告警
            ALARM_SCS_RELAY_SWITCH,         							// 继电器开关告警
            ALARM_SCS_METER_SWITCH,        								// 电表开关告警
            ALARM_SCS_TRANSFORMER_SWITCH,   							// 变压器开关告警
            ALARM_SCS_SENSOR_SWITCH,        							// 传感器开关告警
            ALARM_SCS_RECTIFIER_SWITCH,     							// 整流器告警
            ALARM_SCS_INVERTER_SWITCH,      							// 逆变器告警
            ALARM_SCS_PRESSURE_SWITCH,      							// 压力开关告警
            ALARM_SCS_SHUTDOWN_SWITCH,      							// 关机告警
            ALARM_SCS_WHISTLE_SWITCH,	   								// 警笛告警
            ALARM_SCS_SWITCH_END,
            //模拟量
            ALARM_SCS_ANALOG_START = 1880,
            ALARM_SCS_TEMPERATURE,										// 温度告警
            ALARM_SCS_HUMIDITY,             							// 湿度告警
            ALARM_SCS_CONCENTRATION,        							// 浓度告警
            ALARM_SCS_WIND,                 							// 风速告警
            ALARM_SCS_VOLUME,               							// 容量告警
            ALARM_SCS_VOLTAGE,              							// 电压告警
            ALARM_SCS_ELECTRICITY,          							// 电流告警
            ALARM_SCS_CAPACITANCE,          							// 电容告警
            ALARM_SCS_RESISTANCE,           							// 电阻告警
            ALARM_SCS_CONDUCTANCE,          							// 电导告警
            ALARM_SCS_INDUCTANCE,           							// 电感告警
            ALARM_SCS_CHARGE,               							// 电荷量告警
            ALARM_SCS_FREQUENCY,            							// 频率告警
            ALARM_SCS_LIGHT_INTENSITY,      							// 发光强度告警(坎)
            ALARM_SCS_PRESS,                							// 力告警（如牛顿，千克力）
            ALARM_SCS_PRESSURE,             							// 压强告警（帕，大气压）
            ALARM_SCS_HEAT_TRANSFER,        							// 导热告警（瓦每平米）
            ALARM_SCS_THERMAL_CONDUCTIVITY, 							// 热导告警（kcal/(m*h*℃)）
            ALARM_SCS_VOLUME_HEAT,          							// 比容热告（kcal/(kg*℃)）
            ALARM_SCS_HOT_WORK,             							// 热功告警（焦耳）
            ALARM_SCS_POWER,                							// 功率告警（瓦）
            ALARM_SCS_PERMEABILITY,         							// 渗透率告警（达西）
            ALARM_SCS_PROPERTION,										// 比例（包括电压电流变比，功率因素，负载单位为%） 
            ALARM_SCS_ENERGY,											// 电能（单位为J）
            ALARM_SCS_ANALOG_END,
        }

        public enum dpsdk_sdk_type_e
        {
            /// <summary>
            /// 服务模式使用
            /// </summary>
            DPSDK_CORE_SDK_SERVER = 1
        }
        /// <summary>
        /// 初始化参数
        /// </summary>
        public class DPSDK_CreateParam_t
        {
            /// <summary>
            /// 用户id标识符
            /// </summary>
            dpsdk_ipproto_type_e eSipProto;
            /// <summary>
            /// SCAgent设置，默认为DSSCClient，可设置为APPClient
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string szSCAgent;
        }

        public enum dpsdk_ipproto_type_e
        {
            DPSDK_IPPROTO_UDP = 1,	    //UDP
            DPSDK_IPPROTO_TCP = 2,		//TCP
        }

        /// <summary>
        /// 登录信息
        /// </summary>
        public struct Login_Info_t
        {
            /// <summary>
            /// 服务IP
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 46)]
            public string szIp;
            /// <summary>
            /// 服务端口
            /// </summary>
            public uint nPort;
            /// <summary>
            /// 用户名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szUsername;
            /// <summary>
            /// 密码
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szPassword;
            /// <summary>
            /// 协议库类型
            /// </summary>
            public dpsdk_protocol_version_e nProtocol;
            /// <summary>
            /// 登陆类型，1为PC客户端, 2为手机客户端
            /// </summary>
            public uint iType;
        }

        public enum dpsdk_protocol_version_e
        {
            /// <summary>
            /// 一代协议
            /// </summary>
            DPSDK_PROTOCOL_VERSION_I = 1,
            /// <summary>
            /// 二代协议
            /// </summary>
            DPSDK_PROTOCOL_VERSION_II = 2,
        }

        public enum dpsdk_door_status_e
        {
            Door_Close = 0,                                //门关闭
            Door_Open = 1,                                //门打开
            Door_DisConn = 2,                                //门离线
        }

        public enum Core_EnumSetDoorCmd
        {
            CORE_DOOR_CMD_PROGARM,
            CORE_DOOR_CMD_OPEN = 5,				                        // 开门
            CORE_DOOR_CMD_CLOSE = 6,				                    // 关门
            CORE_DOOR_CMD_ALWAYS_OPEN,						            // 门常开
            CORE_DOOR_CMD_ALWAYS_CLOSE,						            // 门常关
            CORE_DOOR_CMD_HOLIDAY_MAG_OPEN,					            // 假日管理门常开
            CORE_DOOR_CMD_HOLIDAY_MAG_CLOSE,				            // 假日管理门常关
            CORE_DOOR_CMD_RESET,							            // 复位
            CORE_DOOR_CMD_HOST_ALWAYS_OPEN,					            // 报警主机下的门禁通道，常开
            CORE_DOOR_CMD_HOST_ALWAYS_CLOSE,				            // 报警主机下的门禁通道，常关
        }

        // 门控制请求
        [StructLayout(LayoutKind.Sequential)]
        public struct SetDoorCmd_Request_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;			                            //通道ID
            public Core_EnumSetDoorCmd cmd;									//控制命令
            public Int64 start;										        //开始时间
            public Int64 end;										        //结束时间
        }




        public static IntPtr nPDLLHandle = (IntPtr)0;
        public static IntPtr nGroupLen = IntPtr.Zero;


        public static bool init( out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                IntPtr result1 = DPSDK_Create(dpsdk_sdk_type_e.DPSDK_CORE_SDK_SERVER, ref nPDLLHandle);//初始化数据交互接口
                IntPtr result2 = DPSDK_InitExt();//初始化解码播放接口
                if (result1 == (IntPtr)0 && result2 == (IntPtr)0)
                {
                    
                }
                else
                {
                    ret = false;
                    msg = "初始化失败";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
            
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool login( out string msg)
        {

            bool ret = true;
            msg = "";
            try
            {
                //DPSDK_Login
                Login_Info_t loginInfo = new Login_Info_t();
                loginInfo.szIp = "112.16.114.227";
                loginInfo.nPort = (uint)9000;
                loginInfo.szUsername = "admin";
                loginInfo.szPassword = "zjjg-2019";
                loginInfo.nProtocol = dpsdk_protocol_version_e.DPSDK_PROTOCOL_VERSION_II;
                loginInfo.iType = 1;
                IntPtr result = DPSDK_Login(nPDLLHandle, ref loginInfo, (IntPtr)10000);
                if (result == (IntPtr)0)
                {
                    ret = true;
                    msg = "";
                }
                else
                {
                    ret = false;
                    msg = "登录失败，错误码：" + result.ToString();
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            
            return ret;
        }
        /// <summary>
        /// 加载组织结构树
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool loadGroup( out string msg)
        {
            //DPSDK_LoadDGroupInfo
            bool ret = true;
            msg = "";
            try
            {
                IntPtr result = DPSDK_LoadDGroupInfo(nPDLLHandle, ref nGroupLen, (IntPtr)60000);
                if (result == (IntPtr)0)
                {
                    ret = true;
                    msg = "";
                }
                else
                {
                    //return err
                    ret = false;
                    msg = "加载组织结构失败，错误码：" + result.ToString();
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取组织结构树
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool  getGroup(out string msg) {
            bool ret = true;
            msg = "";
            try
            {
                byte[] szGroupStr = new byte[(int)nGroupLen + 1];
                IntPtr result = DPSDK_GetDGroupStr(nPDLLHandle, ref szGroupStr[0], nGroupLen, (IntPtr)10000);
                if (result == IntPtr.Zero)
                {
                    string xml = Encoding.UTF8.GetString(szGroupStr).ToString().Replace("\n", "").Replace("\t", "");
                    XmlDocument XML = new XmlDocument();
                    XML.LoadXml(@xml);

                    string JSON = XmlToJson.XmlToJSON(XML);

                    // Replace \ with \\ because string is being decoded twice
                    JSON = JSON.Replace(@"\", @"\\");
                    msg = JSON;
                }
                else
                {
                    ret = false;
                    msg = "获取组织树XML失败，错误码：" + result.ToString();
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }

            return ret;
            
        }

        public static bool OpenWindow(uint nTvWallId, uint nScreenId, out uint wid, out string msg)
        {
            bool ret = true;
            msg = "";
            wid = 0;
            try
            {
                IntPtr nRet = IntPtr.Zero;
                TvWall_Screen_Open_Window_t stuOpenInfo = new TvWall_Screen_Open_Window_t();
                stuOpenInfo.nTvWallId = nTvWallId;
                stuOpenInfo.nScreenId = Convert.ToUInt32(nScreenId);
                stuOpenInfo.nWindowId = 0;//nWindowId;
                stuOpenInfo.fLeft = Convert.ToSingle(0); //m_fLeft; Convert.ToDouble(left.Text);
                stuOpenInfo.fTop = Convert.ToSingle(0);//m_fTop;
                stuOpenInfo.fHeight = Convert.ToSingle(1.0);//m_fHeight;
                stuOpenInfo.fWidth = Convert.ToSingle(1.0);//m_fWidth;
                nRet = DPSDK_TvWallScreenOpenWindow(nPDLLHandle, ref stuOpenInfo, 10000);
                if (nRet == IntPtr.Zero)
                {
                    wid = stuOpenInfo.nTvWallId;
                }
                else
                {
                    ret = false;
                    msg = "开窗失败，错误码：" + nRet.ToString();
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;

        }

        public static bool SetWindowSource(string cameraId,uint nTvWallId, uint nScreenId, uint nWindowId, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                IntPtr nRet = IntPtr.Zero;
                Set_TvWall_Screen_Window_Source_t stuSourceinfo = new Set_TvWall_Screen_Window_Source_t();
                stuSourceinfo.nTvWallId = nTvWallId;
                stuSourceinfo.nScreenId = Convert.ToUInt32(nScreenId);
                stuSourceinfo.nWindowId = 0;//nWindowId;
                stuSourceinfo.szCameraId = cameraId;
                stuSourceinfo.enStreamType = dpsdk_stream_type_e.DPSDK_CORE_STREAMTYPE_MAIN;
                stuSourceinfo.nStayTime = 30;
                nRet = DPSDK_SetTvWallScreenWindowSource(nPDLLHandle, ref stuSourceinfo, 10000);
                if (nRet != IntPtr.Zero)
                {
                    ret = false;
                    msg = "设置窗口视频源失败，错误码：" + nRet.ToString();
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;

        }

        
    }
}