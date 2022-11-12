using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScrew
{
    public enum EM_ErrID
    {
        NoError = 0, //正常，报警清除
        Others = 1, //其它或未知
        hand=7000,
        /// <summary>
        /// 报警【0】
        /// </summary>
        S1_0_0_use = 1000,
        S1_0_1_use = 1001,
        S1_0_2_use = 1002,
        S1_0_3_use = 1003,
        S1_0_4_use = 1004,
        S1_0_5_use = 1005,
        S1_0_6_use = 1006,
        S1_0_7_use = 1007,
        S1_0_8_use = 1008,
        S1_0_9_use = 1009,
        S1_0_10_use = 1010,
        S1_0_11_use = 1011,
        S1_0_12_use = 1012,
        S1_0_13_use = 1013,
        S1_0_14_use = 1014,
        S1_0_15_use = 1015,

        S1_1_0_use = 1100,
        S1_1_1_use = 1101,
        S1_1_2_use = 1102,
        S1_1_3_use = 1103,
        S1_1_4_use = 1104,
        S1_1_5_use = 1105,
        S1_1_6_use = 1106,
        S1_1_7_use = 1107,
        S1_1_8_use = 1108,
        S1_1_9_use = 1109,
        S1_1_10_use = 1110,
        S1_1_11_use = 1111,
        S1_1_12_use = 1112,
        S1_1_13_use = 1113,
        S1_1_14_use = 1114,
        S1_1_15_use = 1115,

        S1_4_0_use = 4100,
        S1_4_1_use = 4101,
        S1_4_2_use = 4102,
        S1_4_3_use = 4103,
        S1_4_4_use = 4104,
        S1_4_5_use = 4105,
        S1_4_6_use = 4106,
        S1_4_7_use = 4107,
        S1_4_8_use = 4108,
        S1_4_9_use = 4109,
        S1_4_10_use = 4110,
        S1_4_11_use = 4111,
        S1_4_12_use = 4112,
        S1_4_13_use = 4113,
        S1_4_14_use = 4114,
        S1_4_15_use = 4115,

        S1_4_20_use = 4200,
        S1_4_21_use = 4201,
        S1_4_22_use = 4202,
        S1_4_23_use = 4203,
        S1_4_24_use = 4204,
        S1_4_25_use = 4205,

        S1_5_0_use = 5100,
        S1_5_1_use = 5101,
        S1_5_2_use = 5102,
        S1_5_3_use = 5103,
        S1_5_4_use = 5104,
        S1_5_5_use = 5105,
        S1_5_6_use = 5106,
        S1_5_7_use = 5107,
        S1_5_8_use = 5108,
        S1_5_9_use = 5109,
        S1_5_10_use = 5110,
        S1_5_11_use = 5111,
        S1_5_12_use = 5112,
        S1_5_13_use = 5113,
        S1_5_14_use = 5114,
        S1_5_15_use = 5115,

        S1_6_0_use = 6100,
        S1_6_1_use = 6101,
        S1_6_2_use = 6102,
        S1_6_3_use = 6103,
        S1_6_4_use = 6104,
        S1_6_5_use = 6105,
        S1_6_6_use = 6106,
        S1_6_7_use = 6107,
        S1_6_8_use = 6108,
        S1_6_9_use = 6109,
        S1_6_10_use = 6110,
        S1_6_11_use = 6111,
        S1_6_12_use = 6112,
        S1_6_13_use = 6113,
        S1_6_14_use = 6114,
        S1_6_15_use = 6115,

        S1_8_0_use = 8100,
        S1_8_1_use = 8101,
        S1_8_2_use = 8102,
        S1_8_3_use = 8103,
        S1_8_4_use = 8104,
        S1_8_5_use = 8105,
        S1_8_6_use = 8106,
        S1_8_7_use = 8107,
        S1_8_8_use = 8108,
        S1_8_9_use = 8109,
        S1_8_10_use = 8110,
        S1_8_11_use = 8111,
        S1_8_12_use = 8112,
        S1_8_13_use = 8113,
        S1_8_14_use = 8114,
        S1_8_15_use = 8115,

        S1_9_0_use = 9100,
        S1_9_1_use = 9101,
        S1_9_2_use = 9102,
        S1_9_3_use = 9103,
        S1_9_4_use = 9104,
        S1_9_5_use = 9105,
        S1_9_6_use = 9106,
        S1_9_7_use = 9107,
        S1_9_8_use = 9108,
        S1_9_9_use = 9109,
        S1_9_10_use = 9110,
        S1_9_11_use = 9111,
        S1_9_12_use = 9112,
        S1_9_13_use = 9113,
        S1_9_14_use = 9114,
        S1_9_15_use = 9115,

        S1_10_0_use = 10100,
        S1_10_1_use = 10101,
        S1_10_2_use = 10102,
        S1_10_3_use = 10103,
        S1_10_4_use = 10104,
        S1_10_5_use = 10105,
        S1_10_6_use = 10106,
        S1_10_7_use = 10107,
        S1_10_8_use = 10108,
        S1_10_9_use = 10109,
        S1_10_10_use = 10110,
        S1_10_11_use = 10111,
        S1_10_12_use = 10112,
        S1_10_13_use = 10113,
        S1_10_14_use = 10114,
        S1_10_15_use = 10115,

        S1_11_0_use = 11100,
        S1_11_1_use = 11101,
        S1_11_2_use = 11102,
        S1_11_3_use = 11103,
        S1_11_4_use = 11104,
        S1_11_5_use = 11105,
        S1_11_6_use = 11106,
        S1_11_7_use = 11107,
        S1_11_8_use = 11108,
        S1_11_9_use = 11109,
        S1_11_10_use = 11110,
        S1_11_11_use = 11111,
        S1_11_12_use = 11112,
        S1_11_13_use = 11113,
        S1_11_14_use = 11114,
        S1_11_15_use = 11115,

        S1_12_0_use = 12100,
        S1_12_1_use = 12101,
        S1_12_2_use = 12102,
        S1_12_3_use = 12103,
        S1_12_4_use = 12104,
        S1_12_5_use = 12105,
        S1_12_6_use = 12106,
        S1_12_7_use = 12107,
        S1_12_8_use = 12108,
        S1_12_9_use = 12109,
        S1_12_10_use = 12110,
        S1_12_11_use = 12111,
        S1_12_12_use = 12112,
        S1_12_13_use = 12113,
        S1_12_14_use = 12114,
        S1_12_15_use = 12115,

        S1_13_0_use = 13100,
        S1_13_1_use = 13101,
        S1_13_2_use = 13102,
        S1_13_3_use = 13103,
        S1_13_4_use = 13104,
        S1_13_5_use = 13105,
        S1_13_6_use = 13106,
        S1_13_7_use = 13107,
        S1_13_8_use = 13108,
        S1_13_9_use = 13109,
        S1_13_10_use = 13110,
        S1_13_11_use = 13111,
        S1_13_12_use = 13112,
        S1_13_13_use = 13113,
        S1_13_14_use = 13114,
        S1_13_15_use = 13115,

        S1_14_0_use = 14100,
        S1_14_1_use = 14101,
        S1_14_2_use = 14102,
        S1_14_3_use = 14103,
        S1_14_4_use = 14104,
        S1_14_5_use = 14105,
        S1_14_6_use = 14106,
        S1_14_7_use = 14107,
        S1_14_8_use = 14108,
        S1_14_9_use = 14109,
        S1_14_10_use = 14110,
        S1_14_11_use = 14111,
        S1_14_12_use = 14112,
        S1_14_13_use = 14113,
        S1_14_14_use = 14114,
        S1_14_15_use = 14115,

        S1_15_0_use = 15100,
        S1_15_1_use = 15101,
        S1_15_2_use = 15102,
        S1_15_3_use = 15103,
        S1_15_4_use = 15104,
        S1_15_5_use = 15105,
        S1_15_6_use = 15106,
        S1_15_7_use = 15107,
        S1_15_8_use = 15108,
        S1_15_9_use = 15109,
        S1_15_10_use = 15110,
        S1_15_11_use = 15111,
        S1_15_12_use = 15112,
        S1_15_13_use = 15113,
        S1_15_14_use = 15114,
        S1_15_15_use = 15115,

        S1_16_0_use = 16100,
        S1_16_1_use = 16101,
        S1_16_2_use = 16102,
        S1_16_3_use = 16103,
        S1_16_4_use = 16104,
        S1_16_5_use = 16105,
        S1_16_6_use = 16106,
        S1_16_7_use = 16107,
        S1_16_8_use = 16108,
        S1_16_9_use = 16109,
        S1_16_10_use = 16110,
        S1_16_11_use = 16111,
        S1_16_12_use = 16112,
        S1_16_13_use = 16113,
        S1_16_14_use = 16114,
        S1_16_15_use = 16115,

        S1_17_0_use = 17100,
        S1_17_1_use = 17101,
        S1_17_2_use = 17102,
        S1_17_3_use = 17103,
        S1_17_4_use = 17104,
        S1_17_5_use = 17105,
        S1_17_6_use = 17106,
        S1_17_7_use = 17107,
        S1_17_8_use = 17108,
        S1_17_9_use = 17109,
        S1_17_10_use = 17110,
        S1_17_11_use = 17111,
        S1_17_12_use = 17112,
        S1_17_13_use = 17113,
        S1_17_14_use = 17114,
        S1_17_15_use = 17115,

        S1_18_0_use = 18100,
        S1_18_1_use = 18101,
        S1_18_2_use = 18102,
        S1_18_3_use = 18103,
        S1_18_4_use = 18104,
        S1_18_5_use = 18105,
        S1_18_6_use = 18106,
        S1_18_7_use = 18107,
        S1_18_8_use = 18108,
        S1_18_9_use = 18109,
        S1_18_10_use = 18110,
        S1_18_11_use = 18111,
        S1_18_12_use = 18112,
        S1_18_13_use = 18113,
        S1_18_14_use = 18114,
        S1_18_15_use = 18115,

        S1_19_0_use = 19100,
        S1_19_1_use = 19101,
        S1_19_2_use = 19102,
        S1_19_3_use = 19103,
        S1_19_4_use = 19104,
        S1_19_5_use = 19105,
        S1_19_6_use = 19106,
        S1_19_7_use = 19107,
        S1_19_8_use = 19108,
        S1_19_9_use = 19109,
        S1_19_10_use = 19110,
        S1_19_11_use = 19111,
        S1_19_12_use = 19112,
        S1_19_13_use = 19113,
    }

    public class CErrorMgr
    {
        public Dictionary<EM_ErrID, ST_EquError> m_dicDefine = new Dictionary<EM_ErrID, ST_EquError>();

        private Dictionary<int, EM_ErrID> m_dicPlcBit2ID = new Dictionary<int, EM_ErrID>();
        private Dictionary<EM_ErrID, ST_EquError> m_dicOccurred = new Dictionary<EM_ErrID, ST_EquError>();

        private static readonly object mobjCErrorMgr = new object();
        public static CErrorMgr mCErrorMgr = null;
        public static CErrorMgr Instance
        {
            get
            {
                lock (mobjCErrorMgr)
                {
                    if (mCErrorMgr == null)
                    {
                        mCErrorMgr = new CErrorMgr();
                    }
                    return mCErrorMgr;
                }
            }
        }
        public EM_ErrID Get_ErrID(int p_nPlcByteIndex, int p_nPlcBitPos)
        {
            //byteIdx begin = 0
            //bitPos begin = 0

            EM_ErrID emDes = EM_ErrID.NoError;

            int nFinal = (p_nPlcByteIndex * 32) + (p_nPlcBitPos + 1);

            if (m_dicPlcBit2ID.ContainsKey(nFinal))
                emDes = this.m_dicPlcBit2ID[nFinal];

            return emDes;
        }
        public void _Add_One(EM_ErrID p_emId)
        {
            if (!m_dicDefine.ContainsKey(p_emId))
                return;
            if (m_dicOccurred.ContainsKey(p_emId))
                return;

            ST_EquError stInfo = m_dicDefine[p_emId];

            stInfo.stTime_beg = DateTime.Now;//发生时间

            m_dicOccurred.Add(p_emId, stInfo);
            CDBhelper._Get_Instance()._EquError_Add(stInfo);
            GlobalVar.Instance.post_ShowErrInfoAdd(stInfo);//展示到界面
        }

        public void _Remove_One(EM_ErrID p_emId)
        {
            if (!m_dicOccurred.ContainsKey(p_emId))
                return;

            ST_EquError stSrc = m_dicOccurred[p_emId];
            stSrc.stTime_end = DateTime.Now;//故障结束的时间
            m_dicOccurred.Remove(p_emId);
            //try
            //{
            //    CDataCsvFileProc.ErrorCodeCSV(m_nEquNumber, stSrc);
            //}
            //catch (Exception)
            //{

            //}
            CDBhelper._Get_Instance()._EquError_Alter_ByErrEnd(stSrc);
            GlobalVar.Instance.post_ShowErrInfo_Remove(stSrc);//展示到界面

            //CDBhelper._Get_Instance()._EquError_Alter_ByErrEnd(m_nEquNumber, stSrc);//更新到DB

            //ATSLog.Log_ErrCode(string.Format(",故障解除,{0},{1}", (UInt32)stSrc.emID, stSrc.ssDesc));
        }
       

        public void _InitErrDefine()
        {
            this.InitDefine1();
        }
        void adderrcode(EM_ErrID ID, string Desc, int Dbyte, int bit,int category)
        {
            ST_EquError stInfo = new ST_EquError();

            EM_ErrID emErrId = ID;
            stInfo.Reset();
            stInfo.emID = ID;
            stInfo.emLevel = EM_ErrLevel.E_yellow;
            stInfo.ssDesc = Desc;
            stInfo.ssDesc_en = Desc;
            stInfo.Category = category;
            stInfo.ssResolve = "";
            stInfo.nPlcTag = Dbyte * 32 + bit + 1; ;
            m_dicDefine.Add(emErrId, stInfo);
            m_dicPlcBit2ID.Add(stInfo.nPlcTag, emErrId);
        }
        ushort IOCardNumber = 0;
        private void InitDefine1()
        {
            ST_EquError stInfo = new ST_EquError();

            EM_ErrID emErrId = EM_ErrID.NoError;
            stInfo.Reset();
            stInfo.emID = emErrId;
            stInfo.emLevel = EM_ErrLevel.E_green;
            stInfo.ssDesc = "机器正常";
            stInfo.ssDesc_en = "Equipment Normal";
            stInfo.ssResolve = "";
            m_dicDefine.Add(emErrId, stInfo);
            IOCardNumber = (ushort)(GlobalVar.IOCardNumber[0] - GlobalVar.AlarmIOCard);
            adderrcode(EM_ErrID.S1_0_0_use, "启动按钮", IOCardNumber, (int)IOListInput.StartButton,1);
            adderrcode(EM_ErrID.S1_0_1_use, "停止按钮", IOCardNumber, (int)IOListInput.StopButtopn, 1);
            adderrcode(EM_ErrID.S1_0_2_use, "暂停按钮", IOCardNumber, (int)IOListInput.PuaseButton, 1);
            adderrcode(EM_ErrID.S1_0_3_use, "复位按钮1", IOCardNumber, (int)IOListInput.ResetButton1, 1);
            adderrcode(EM_ErrID.S1_0_4_use, "复位按钮2", IOCardNumber, (int)IOListInput.ResetButton2, 1);
            adderrcode(EM_ErrID.S1_0_5_use, "复位按钮3", IOCardNumber, (int)IOListInput.ResetButton3, 1);
            adderrcode(EM_ErrID.S1_0_6_use, "急停按钮1", IOCardNumber, (int)IOListInput.ScramButton, 1);
            adderrcode(EM_ErrID.S1_0_7_use, "", IOCardNumber, 7, 0);
            adderrcode(EM_ErrID.S1_0_8_use, "前门1", IOCardNumber, (int)IOListInput.FrontDoor1,4);
            adderrcode(EM_ErrID.S1_0_9_use, "前门2", IOCardNumber, (int)IOListInput.FrontDoor2, 4);
            adderrcode(EM_ErrID.S1_0_10_use, "右门3", IOCardNumber, (int)IOListInput.RightDoor3, 4);
            adderrcode(EM_ErrID.S1_0_11_use, "右门4", IOCardNumber, (int)IOListInput.RightDoor4, 4);
            adderrcode(EM_ErrID.S1_0_12_use, "后门5", IOCardNumber, (int)IOListInput.BackDoor5, 4);
            adderrcode(EM_ErrID.S1_0_13_use, "后门6", IOCardNumber, (int)IOListInput.BackDoor6, 4);
            adderrcode(EM_ErrID.S1_0_14_use, "左门7", IOCardNumber, (int)IOListInput.LeftDoor7, 4);
            adderrcode(EM_ErrID.S1_0_15_use, "左门8", IOCardNumber, (int)IOListInput.LeftDoor8, 4);
            adderrcode(EM_ErrID.S1_1_0_use, "进料阻挡气缸上位", IOCardNumber, (int)IOListInput.HSGFeedBlockUp, 2);
            adderrcode(EM_ErrID.S1_1_1_use, "进料阻挡气缸下位", IOCardNumber, (int)IOListInput.HSGFeedBlockDown, 2);
            adderrcode(EM_ErrID.S1_1_2_use, "工位1阻挡气缸上位", IOCardNumber, (int)IOListInput.Station1BlockUp, 2);
            adderrcode(EM_ErrID.S1_1_3_use, "工位1阻挡气缸下位", IOCardNumber, (int)IOListInput.Station1BlockDown, 2);
            adderrcode(EM_ErrID.S1_1_4_use, "工位1顶升气缸上位", IOCardNumber, (int)IOListInput.Station1JackUpUp, 2);
            adderrcode(EM_ErrID.S1_1_5_use, "工位1顶升气缸下位", IOCardNumber, (int)IOListInput.Station1JackUpDown, 2);
            adderrcode(EM_ErrID.S1_1_6_use, "工位1解锁前推气缸前位", IOCardNumber, (int)IOListInput.Station1UnlockForwardFront, 2);
            adderrcode(EM_ErrID.S1_1_7_use, "工位1解锁前推气缸后位", IOCardNumber, (int)IOListInput.Station1UnlockForwardBack, 2);
            adderrcode(EM_ErrID.S1_1_8_use, "工位1解锁夹紧气缸夹位", IOCardNumber, (int)IOListInput.Station1UnlockClampFront, 2);
            adderrcode(EM_ErrID.S1_1_9_use, "工位1解锁夹紧气缸松位", IOCardNumber, (int)IOListInput.Station1UnlockClampBack, 2);
            adderrcode(EM_ErrID.S1_1_10_use, "工位1解锁倾斜气缸正位", IOCardNumber, (int)IOListInput.Station1UnlockLeanFront, 2);
            adderrcode(EM_ErrID.S1_1_11_use, "工位1解锁倾斜气缸斜位", IOCardNumber, (int)IOListInput.Station1UnlockLeanBack, 2);
            adderrcode(EM_ErrID.S1_1_12_use, "工位1压料气缸原位", IOCardNumber, (int)IOListInput.Station1PressBegun, 2);
            adderrcode(EM_ErrID.S1_1_13_use, "工位1压料气缸到位", IOCardNumber, (int)IOListInput.Station1PressArrived, 2);
            adderrcode(EM_ErrID.S1_1_14_use, "工位1斜料回退气缸回位", IOCardNumber, (int)IOListInput.Station1LeanBackBegun, 2);
            adderrcode(EM_ErrID.S1_1_15_use, "工位1斜料回退气缸出位", IOCardNumber, (int)IOListInput.Station1LeanBackArrived, 2);

            IOCardNumber = (ushort)(GlobalVar.IOCardNumber[1] - GlobalVar.AlarmIOCard);
            adderrcode(EM_ErrID.S1_4_0_use, "工位2光源1气缸原位", IOCardNumber, (int)IOListInput.Station2Light1Begun, 2);
            adderrcode(EM_ErrID.S1_4_1_use, "工位2光源1气缸到位", IOCardNumber, (int)IOListInput.Station2Light1Arrived, 2);
            adderrcode(EM_ErrID.S1_4_2_use, "工位2阻挡气缸上位", IOCardNumber, (int)IOListInput.Station2BlockUp, 2);
            adderrcode(EM_ErrID.S1_4_3_use, "工位2阻挡气缸下位", IOCardNumber, (int)IOListInput.Station2BlockDown, 2);
            adderrcode(EM_ErrID.S1_4_4_use, "工位2顶升气缸上位", IOCardNumber, (int)IOListInput.Station2JackUpUp, 2);
            adderrcode(EM_ErrID.S1_4_5_use, "工位2顶升气缸下位", IOCardNumber, (int)IOListInput.Station2JackUpDown, 2);
            adderrcode(EM_ErrID.S1_4_6_use, "工位2解锁前推气缸前位", IOCardNumber, (int)IOListInput.Station2UnlockForwardFront, 2);
            adderrcode(EM_ErrID.S1_4_7_use, "工位2解锁前推气缸后位", IOCardNumber, (int)IOListInput.Station2UnlockForwardBack, 2);
            adderrcode(EM_ErrID.S1_4_8_use, "工位2解锁夹紧气缸夹位", IOCardNumber, (int)IOListInput.Station2UnlockClampFront, 2);
            adderrcode(EM_ErrID.S1_4_9_use, "工位2解锁夹紧气缸松位", IOCardNumber, (int)IOListInput.Station2UnlockClampBack, 2);
            adderrcode(EM_ErrID.S1_4_10_use, "工位2斜料倾斜气缸正位", IOCardNumber, (int)IOListInput.Station2LeanFront, 2);
            adderrcode(EM_ErrID.S1_4_11_use, "工位2斜料倾斜气缸斜位", IOCardNumber, (int)IOListInput.Station2LeanBack, 2);
            adderrcode(EM_ErrID.S1_4_12_use, "工位2压料气缸原位", IOCardNumber, (int)IOListInput.Station2PressBegun, 2);
            adderrcode(EM_ErrID.S1_4_13_use, "工位2压料气缸到位", IOCardNumber, (int)IOListInput.Station2PressArrived, 2);
            adderrcode(EM_ErrID.S1_4_14_use, "工位1光源1气缸原位", IOCardNumber, (int)IOListInput.Station1Light1Begun, 2);
            adderrcode(EM_ErrID.S1_4_15_use, "工位1光源1气缸到位", IOCardNumber, (int)IOListInput.Station1Light1Arrived, 2);
            adderrcode(EM_ErrID.S1_5_0_use, "工位3光源1气缸原位", IOCardNumber, (int)IOListInput.Station3Light1Begun, 2);
            adderrcode(EM_ErrID.S1_5_1_use, "工位3光源1气缸到位", IOCardNumber, (int)IOListInput.Station3Light1Arrived, 2);
            adderrcode(EM_ErrID.S1_5_2_use, "工位3阻挡气缸上位", IOCardNumber, (int)IOListInput.Station3BlockUp, 2);
            adderrcode(EM_ErrID.S1_5_3_use, "工位3阻挡气缸下位", IOCardNumber, (int)IOListInput.Station3BlockDown, 2);
            adderrcode(EM_ErrID.S1_5_4_use, "工位3顶升气缸上位", IOCardNumber, (int)IOListInput.Station3JackUpUp, 2);
            adderrcode(EM_ErrID.S1_5_5_use, "工位3顶升气缸下位", IOCardNumber, (int)IOListInput.Station3JackUpDown, 2);
            adderrcode(EM_ErrID.S1_5_6_use, "工位3斜料回退气缸回位", IOCardNumber, (int)IOListInput.Station3LeanBackBegun, 2);
            adderrcode(EM_ErrID.S1_5_7_use, "工位3斜料回退气缸出位", IOCardNumber, (int)IOListInput.Station3LeanBackArrived, 2);
            adderrcode(EM_ErrID.S1_5_8_use, "工位3压料气缸原位", IOCardNumber, (int)IOListInput.Station3PressBegun, 2);
            adderrcode(EM_ErrID.S1_5_9_use, "工位3压料气缸到位", IOCardNumber, (int)IOListInput.Station3PressArrived, 2);
            adderrcode(EM_ErrID.S1_5_10_use, "工位3光源2气缸原位", IOCardNumber, (int)IOListInput.Station3Light2Begun, 2);
            adderrcode(EM_ErrID.S1_5_11_use, "工位3光源2气缸到位", IOCardNumber, (int)IOListInput.Station3Light2Arrived, 2);
            adderrcode(EM_ErrID.S1_5_12_use, "下料阻挡气缸上位", IOCardNumber, (int)IOListInput.DisChargeBlockUP, 2);
            adderrcode(EM_ErrID.S1_5_13_use, "下料阻挡气缸下位", IOCardNumber, (int)IOListInput.DisChargeBlockDown, 2);
            adderrcode(EM_ErrID.S1_5_14_use, "下料顶升气缸上位", IOCardNumber, (int)IOListInput.DisChargeJackUpUp, 2);
            adderrcode(EM_ErrID.S1_5_15_use, "下料顶升气缸下位", IOCardNumber, (int)IOListInput.DisChargeJackUpDown, 2);

            IOCardNumber = (ushort)(GlobalVar.IOCardNumber[2] - GlobalVar.AlarmIOCard);
            adderrcode(EM_ErrID.S1_6_0_use, "工位2螺丝臂真空表", IOCardNumber, (int)IOListInput.Station2ScrewVacuum, 1);
            adderrcode(EM_ErrID.S1_6_1_use, "工位3螺丝臂真空表", IOCardNumber, (int)IOListInput.Station3ScrewVacuum, 1);
            adderrcode(EM_ErrID.S1_6_2_use, "工位2左螺丝盒ready", IOCardNumber, (int)IOListInput.Station2LeftScrewBoxReady, 1);
            adderrcode(EM_ErrID.S1_6_3_use, "工位2右螺丝盒ready", IOCardNumber, (int)IOListInput.Station2RightScrewBoxReady, 1);
            adderrcode(EM_ErrID.S1_6_4_use, "工位3左螺丝盒ready", IOCardNumber, (int)IOListInput.Station3LeftScrewBoxReady, 1);
            adderrcode(EM_ErrID.S1_6_5_use, "工位3右螺丝盒ready", IOCardNumber, (int)IOListInput.Station3RightScrewBoxReady, 1);
            adderrcode(EM_ErrID.S1_6_6_use, "工位2电批完成信号", IOCardNumber, (int)IOListInput.Station2EleScrewDone, 1);
            adderrcode(EM_ErrID.S1_6_7_use, "工位2电批错误信号", IOCardNumber, (int)IOListInput.Station2EleScrewError, 1);
            adderrcode(EM_ErrID.S1_6_8_use, "工位2电批准备好信号", IOCardNumber, (int)IOListInput.Station2EleScrewReady, 1);
            adderrcode(EM_ErrID.S1_6_9_use, "工位2电批运行中", IOCardNumber, (int)IOListInput.Station2EleScrewRun, 1);
            adderrcode(EM_ErrID.S1_6_10_use, "工位3电批完成信号", IOCardNumber, (int)IOListInput.Station3EleScrewDone, 1);
            adderrcode(EM_ErrID.S1_6_11_use, "工位3电批错误信号", IOCardNumber, (int)IOListInput.Station3EleScrewError, 1);
            adderrcode(EM_ErrID.S1_6_12_use, "工位3电批准备好信号", IOCardNumber, (int)IOListInput.Station3EleScrewReady, 1);
            adderrcode(EM_ErrID.S1_6_13_use, "工位3电批运行中", IOCardNumber, (int)IOListInput.Station3EleScrewRun, 1);

            adderrcode(EM_ErrID.S1_6_15_use, "工位3光源5气缸到位", IOCardNumber, (int)IOListInput.Station3Light5Arrived, 2);

            adderrcode(EM_ErrID.S1_8_0_use, "进料工位后对射", IOCardNumber, (int)IOListInput.FeedBackRay, 1);
            adderrcode(EM_ErrID.S1_8_1_use, "工位1前对射", IOCardNumber, (int)IOListInput.Station1FrontRay, 1);
            adderrcode(EM_ErrID.S1_8_2_use, "工位1后对射", IOCardNumber, (int)IOListInput.Station1BackRay, 1);
            adderrcode(EM_ErrID.S1_8_3_use, "工位2前对射", IOCardNumber, (int)IOListInput.Station2FrontRay, 1);
            adderrcode(EM_ErrID.S1_8_4_use, "工位2后对射", IOCardNumber, (int)IOListInput.Station2BackRay, 1);
            adderrcode(EM_ErrID.S1_8_5_use, "工位3前对射", IOCardNumber, (int)IOListInput.Station3FrontRay, 1);
            adderrcode(EM_ErrID.S1_8_6_use, "工位3后对射", IOCardNumber, (int)IOListInput.Station3BackRay, 1);
            adderrcode(EM_ErrID.S1_8_7_use, "下料工位前对射", IOCardNumber, (int)IOListInput.DiachargeFrontRay, 1);
            adderrcode(EM_ErrID.S1_8_8_use, "下料NG有料感应", IOCardNumber, (int)IOListInput.DiaChargeNG, 1);
            adderrcode(EM_ErrID.S1_8_9_use, "工位1光源2气缸原位", IOCardNumber, (int)IOListInput.Station1Light2Begun, 2);
            adderrcode(EM_ErrID.S1_8_10_use, "工位1光源2气缸到位", IOCardNumber, (int)IOListInput.Station1Light2Arrived, 2);
            adderrcode(EM_ErrID.S1_8_11_use, "工位3光源3气缸原位", IOCardNumber, (int)IOListInput.Station3Light3Begun, 2);
            adderrcode(EM_ErrID.S1_8_12_use, "工位3光源3气缸到位", IOCardNumber, (int)IOListInput.Station3Light3Arrived, 2);
            adderrcode(EM_ErrID.S1_8_13_use, "工位3光源4气缸原位", IOCardNumber, (int)IOListInput.Station3Light4Begun, 2);
            adderrcode(EM_ErrID.S1_8_14_use, "工位3光源4气缸到位", IOCardNumber, (int)IOListInput.Station3Light4Arrived, 2);
            adderrcode(EM_ErrID.S1_8_15_use, "工位3光源5气缸原位", IOCardNumber, (int)IOListInput.Station3Light5Begun, 2);




            //四号卡暂定

            //IOCardNumber = (ushort)(GlobalVar.IOCardNumber[0] - GlobalVar.AlarmIOCard);
            //adderrcode(EM_ErrID.S1_9_0_use, "下料移栽夹爪松开", 9, 0);
            //adderrcode(EM_ErrID.S1_9_1_use, "下料移栽夹爪夹紧", 9, 1);
            //adderrcode(EM_ErrID.S1_9_2_use, "工位2螺丝臂摄像头上位", 9, 2);
            //adderrcode(EM_ErrID.S1_9_3_use, "工位2螺丝臂摄像头下位", 9, 3);
            //adderrcode(EM_ErrID.S1_9_4_use, "工位3螺丝臂摄像头上位", 9, 4);
            //adderrcode(EM_ErrID.S1_9_5_use, "工位3螺丝臂摄像头下位", 9, 5);
            //
            IOCardNumber = 3;
            adderrcode(EM_ErrID.S1_10_0_use, "电批1通信", IOCardNumber, 0, 3);
            adderrcode(EM_ErrID.S1_10_1_use, "电批2通信", IOCardNumber, 1, 3);
            adderrcode(EM_ErrID.S1_10_2_use, "扫码枪1连接", IOCardNumber, 2, 3);
            adderrcode(EM_ErrID.S1_10_3_use, "扫码枪2连接", IOCardNumber, 3, 3);
            adderrcode(EM_ErrID.S1_10_4_use, "PDCA连接", IOCardNumber, 4, 3);
            adderrcode(EM_ErrID.S1_10_5_use, "视觉连接", IOCardNumber, 5, 3);

        }

    }
}
