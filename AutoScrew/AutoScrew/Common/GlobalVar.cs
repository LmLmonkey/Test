using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AutoScrew
{
    class GlobalVar
    {

        public static SQLiteConnection ms_objConnection = null;   //数据库

        public static string ms_ssCfgPath = Directory.GetCurrentDirectory() + @"\Config";      //数据库

        public static int LevelForUser = 0;

        public static double PrintDis1 = 0;
        public static double PrintDis2 = 0;
        public static double Calibration = 0;

        public static bool bStartRunning = false;//启动运行标识
        public static bool bPauseStatus = false;//暂停状态标识
        public static bool bStopStatus = false;//停止状态标识

        public static ushort[] Sation1XYZ = { 5, 3, 6 }; //工站1轴号
        public static ushort[] Sation2XYZ = { 7, 4, 8 }; //工站2轴号
        public static ushort[] Sation3XYZ = { 10, 9, 11 }; //工站3轴号
        public static ushort[] SingleAxis = { 12, 13 }; //下料轴
        public static ushort[] Conveyor = { 0, 1, 2 }; //皮带轴

        public const string Station1Name = "工位1组合轴";
        public const string Station2Name = "工位2组合轴";
        public const string Station3Name = "工位3组合轴";

        public const string SingleXName = "下料X";
        public const string SingleZName = "下料Z";

        public const ushort AlarmIOCard = 1001;

        public static bool Production = false;

        public static bool[] Station1CameraResult = new bool[4];  //相机拍照完成信号
        public static bool[] Station2CameraResult = new bool[4];  //相机拍照完成信号
        public static bool[] Station3CameraResult = new bool[4];  //相机拍照完成信号
        public static double[] Station2CameraDia = new double[2];  //X，Y 偏差
        public static double[] Station3CameraDia = new double[2];  //X，Y 偏差

        public static double[] Station1AxisXParameter = new double[6];   //轴号&&6个参数
        public static double[] Station1AxisYParameter = new double[6];   //轴号&&6个参数
        public static double[] Station1AxisZParameter = new double[6];   //轴号&&6个参数
        public static double[] Station2AxisXParameter = new double[6];   //轴号&&6个参数
        public static double[] Station2AxisYParameter = new double[6];   //轴号&&6个参数
        public static double[] Station2AxisZParameter = new double[6];   //轴号&&6个参数
        public static double[] Station3AxisXParameter = new double[6];   //轴号&&6个参数
        public static double[] Station3AxisYParameter = new double[6];   //轴号&&6个参数
        public static double[] Station3AxisZParameter = new double[6];   //轴号&&6个参数


        public static double[] Station1ConveyorParameter = new double[6];   //轴号&&6个参数
        public static double[] Station2ConveyorParameter = new double[6];   //轴号&&6个参数
        public static double[] Station3ConveyorParameter = new double[6];   //轴号&&6个参数

        public static double[] SingleAxisXParameter = new double[6];   //轴号&&6个参数
        public static double[] SingleAxisZParameter = new double[6];   //轴号&&6个参数

        public static RobotPos[] Station1CombinedAxis = new RobotPos[12];  //工站1点位  
        public static RobotPos[] Station2CombinedAxis = new RobotPos[12]; //工站2点位 
        public static RobotPos[] Station3CombinedAxis = new RobotPos[12]; //工站3点位 

        public static double[] LayingOffX = new double[5]; //取料位 OK位  NG位
        public static double[] LayingOffZ = new double[5]; //取料位 OK位  NG位

        public static ushort[] IOCardNumber = { 1001, 1002, 1003,111 };

        public static bool StartLabel = false;
        public static bool PauseLabel = false;
        public static bool StopLabel = false;

        public static int CylinderAlarmTime = 3;
        public static string EleScrew1IP = "192.168.100.11";
        public static string EleScrew2IP = "192.168.100.22";

        public static bool ResetOKLable = false;    //复位完成标志

        public static string KeyenceBarCodeIP = "192.168.100.2";

        public static int DataSaveDays = -10;
        public static List<string> CCD1Calibration = new List<string>();
        public static List<string> CCD2Calibration = new List<string>();
        public static List<string> CCD3Calibration = new List<string>();
        public static List<string> CCD4Calibration = new List<string>();

        public static List<double> Station1Screw3 = new List<double>(); //CCD1螺丝孔3数据
        public static List<double> Station1Screw4 = new List<double>(); //CCD1螺丝孔4数据
        public static List<double> Station1Screw5 = new List<double>(); //CCD1螺丝孔5数据
        public static List<double> Station1Screw6 = new List<double>(); //CCD1螺丝孔6数据

        public static List<double> Station2Screw3Location = new List<double>(); //工位2螺丝孔3定位数据
        public static List<double> Station2Screw4Location = new List<double>(); //工位2螺丝孔4定位数据
        public static List<double> Station3Screw5Location = new List<double>(); //工位3螺丝孔5定位数据
        public static List<double> Station3Screw6Location = new List<double>(); //工位3螺丝孔6定位数据

        public static List<double> Station2Screw3ReCheck = new List<double>(); //工位2螺丝孔3复检数据
        public static List<double> Station2Screw4ReCheck = new List<double>(); //工位2螺丝孔4复检数据
        public static List<double> Station3Screw5ReCheck = new List<double>(); //工位3螺丝孔5复检数据
        public static List<double> Station3Screw6ReCheck = new List<double>(); //工位3螺丝孔6复检数据

        public static bool LoginStatus = false;
        public static bool ConnectStatus = false;

        public static double SpeedAccPercent = 0;

        public static bool PDCADisEnable = true;
        public static bool BarCodeDisEnable = true;
        public static bool VisionDisEnable = true;
        public static bool SafeDoorDisEnable = true;

        public static bool Station1DischargeLabel = false;
        public static bool Station2DischargeLabel = false;
        public static bool Station3DischargeLabel = false;

        public static bool Station2EnableStatuon1 = false;
        public static bool Station3EnableStatuon2 = false;
        public static bool StationDisEnableStatuon3 = false;

        #region 电批
        public static byte[] originalData1;    //截取原始数据
        public static double[] torq_double_1 = new double[200];    //电批1已算好的扭力
        public static double[] Angle_double_1 = new double[200];   //电批1已算好的角度
        public static double Max_Torque_1;   //电批1最大扭力
        public static double Max_Angle_1;    //电批1最大角度  
        public static string titleAxisY_1 = "Torque / kgf.cm";//Y轴标题
        public static int indexSSL1 = 0;    //SSL位置
        public static int optID_net_1 = -1; //操作序号（网络）

        public static byte[] originalData2;    //截取原始数据
        public static double[] torq_double_2 = new double[200];    //电批1已算好的扭力
        public static double[] Angle_double_2 = new double[200];   //电批1已算好的角度
        public static double Max_Torque_2;   //电批1最大扭力
        public static double Max_Angle_2;    //电批1最大角度  
        public static string titleAxisY_2 = "Torque / kgf.cm";//Y轴标题
        public static int indexSSL2 = 0;    //SSL位置
        public static int optID_net_2 = -1; //操作序号（网络）

        public static bool TKnetDataEnter1 = false;   //1网络数据进来标志
        public static bool TKcomDataEnter1 = false;   //1串口数据进来标志
        public static bool TKnetDataEnter2 = false;   //2网络数据进来标志
        public static bool TKcomDataEnter2 = false;   //2串口数据进来标志
        #endregion

        #region 加锁

        private static readonly object mobjectGlobalVar = new object();
        private static object sendLog = new object();
        public static GlobalVar mGlobalVar = null;
        public static GlobalVar Instance
        {
            get
            {
                lock (mobjectGlobalVar)
                {
                    if (mGlobalVar == null)
                    {
                        mGlobalVar = new GlobalVar();
                    }
                    return mGlobalVar;
                }
            }
        }
        #endregion

        #region   报警&提示
        public event Action<string, LogName> RunMessageDisplay;
        public void UpdateRunMessage(string message, LogName name)
        {
            lock (sendLog)
            {
                if (RunMessageDisplay != null)
                {
                    RunMessageDisplay(message, name);
                }
            }
        }

        public event Action<List<string>, LogName> RunResultDisplay;
        public void UpdateRunResult(List<string> message, LogName name)
        {
            lock (sendLog)
            {
                if (RunResultDisplay != null)
                {
                    RunResultDisplay(message, name);
                }
            }
        }


        public event Action<int, ST_EquError> evt_ShowErrInfoAdd;
        public void post_ShowErrInfoAdd(ST_EquError p_stInfo)
        {
            if (evt_ShowErrInfoAdd != null)
            {
                evt_ShowErrInfoAdd(1, p_stInfo);
            }
        }

        public event Action<int, ST_EquError> evt_ShowErrInfo_Remove;
        public void post_ShowErrInfo_Remove(ST_EquError p_stInfo)
        {
            if (evt_ShowErrInfo_Remove != null)
            {
                evt_ShowErrInfo_Remove(1, p_stInfo);
            }
        }
        #endregion


    }
    public struct RobotPos
    {
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ1 { get; set; }

        public void Init()
        {
            PosX = 0;
            PosY = 0;
            PosZ1 = 0;
        }

        public void Set(double x, double y, double z1)
        {
            PosX = x;
            PosY = y;
            PosZ1 = z1;

        }
        public string ToString1(int i)
        {
            if (i == 1)
            {
                return PosX.ToString("0.000");
            }
            else if (i == 2)
            {
                return PosY.ToString("0.000");
            }
            else if (i == 3)
            {
                return PosZ1.ToString("0.000");
            }
            else
            {
                return PosX.ToString("0.000");
            }
        }

        public override string ToString()
        {
            return PosX.ToString("0.000") + " "
                   + PosY.ToString("0.000") + " "
                   + PosZ1.ToString("0.000");
        }


        public static RobotPos operator +(RobotPos btn1Pos, RobotPos btn2Pos)
        {
            btn1Pos.PosZ1 += btn2Pos.PosZ1;
            btn1Pos.PosX += btn2Pos.PosX;
            btn1Pos.PosY += btn2Pos.PosY;

            return btn1Pos;
        }
    }

    public enum LogName
    {
        runLog,
        errorLog,
        QrCodeLog,
        SocketLog,
        CCD,
        Barcode,
        PDCA,
        Net,
        PLC,
    };

    public enum SelectPageItem
    {
        None,
        lbMainPage,
        lbDebuggingPage,
        lbCamPage,
        lbAlarmPage,
        lbDataPage,
        lbMachineInfoPage,
        lbStart,
        lbReset,
        lbStop,
        lbOpenExel,
        lbOpenImage,
        lbUserPage,
        lbLogPage,
        lbCalibrationPage,
        lbUserpage,
    };
    public enum Station1AxisStatus
    {
        None,
        Feeding,
        TakePhoto1,
        TakePhoto2,
        TakePhoto3,
        TakePhoto4,
        Lean,
        CarrierReset,
        DisCharge,
    };
    public enum Station2AxisStatus
    {
        None,
        Feeding,
        TakePhoto,
        AutoScrew1,
        AutoScrew2,
        Lean,
        CarrierReset,
        DisCharge,
    };
    public enum Station3AxisStatus
    {
        None,
        Feeding,
        TakePhoto1,
        TakePhoto2,
        AutoScrew1,
        AutoScrew2,
        CarrierReset,
        DisCharge,
    };
    public enum Station4AxisStatus
    {
        None,
        Feeding,
        DisCharge,
    };
    public enum ColorLight
    {
        None,
        Green,
        Yellow,
        Red,
        Buzzer,
    }

    public enum EM_ErrLevel
    {
        E_green = 0,
        E_grey = 1,
        E_orange = 2,
        E_yellow = 3,
        E_white = 4,
        E_blue = 5,
        E_red = 6,
        E_black = 7,
    }
    public struct ST_EquError
    {
        public int nCategory;

        public EM_ErrLevel emLevel;

        public EM_ErrID emID;

        public string ssCode;

        public string ssDesc;//故障描述

        public string ssDesc_en;

        public string ssResolve;

        public DateTime stTime_beg;

        public DateTime stTime_end;

        public int nPlcTag;

        public int Category;   //1IO  2气缸  3通信   4安全门

        public void Reset()
        {
            nCategory = 0;
            emLevel = EM_ErrLevel.E_green;
            emID = EM_ErrID.NoError;
            ssCode = "";
            ssDesc = "";
            ssDesc_en = "";
            ssResolve = "";
            nPlcTag = 0;
        }

    } //end struct
}
