using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoScrew
{
    public partial class MainStream : Component
    {
        public Station1AxisStatus _station1AxisStatus;
        public Station2AxisStatus _station2AxisStatus;
        public Station3AxisStatus _station3AxisStatus;
        public Station4AxisStatus _station4AxisStatus;
        List<double> Dis = new List<double>();
        public ColorLight _ColorLight;
        private Class_Motion ClassMotion;
        public static object mObjects = new object();
        public static MainStream mMainFlow = null;
        public static MainStream instance
        {
            get
            {
                lock (mObjects)
                {

                    if (mMainFlow == null)
                    {
                        mMainFlow = new MainStream();
                    }
                    return mMainFlow;
                }

            }
        }
        public Dictionary<string, string> HSG_SN_Sate = new Dictionary<string, string>();   //  SN 对应的 NG  OK
        public Queue Station1Queue = new Queue();   //SN 传递
        public Queue Station2Queue = new Queue();
        public Queue Station3Queue = new Queue();

        public MainStream()
        {
            InitializeComponent();
            ClassMotion = Class_Motion.instance();
            Dis.Add(0);
            Dis.Add(0);

            _station1AxisStatus = Station1AxisStatus.Feeding;
            _station2AxisStatus = Station2AxisStatus.Feeding;
            _station3AxisStatus = Station3AxisStatus.Feeding;
            _station4AxisStatus = Station4AxisStatus.Feeding;
        }

        public MainStream(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            
        }

        #region  初始化 三色灯
        public void MachineInit()
        {
            ClassMotion.OpenCard();
            ClassMotion.InitialScale();
            selectLight(ColorLight.Yellow);
            ClassMotion.AXISInit();
            ClassMotion.EnableAllAxis();
           // ResetPort1();
            
        }
        public void selectLight(ColorLight color)
        {
            switch (color)
            {
                case ColorLight.Yellow:   //黄
                    ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.YellowLamp);
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.GreenLamp);
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.RedLamp);
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.BuzzerCall);
                    break;
                case ColorLight.Green: //绿
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.YellowLamp);
                    ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.GreenLamp);
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.RedLamp);
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.BuzzerCall);
                    break;
                case ColorLight.Red: //红
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.YellowLamp);
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.GreenLamp);
                    ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.RedLamp);
                    ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.BuzzerCall);
                    break;
                case ColorLight.None:  //无
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.YellowLamp);
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.GreenLamp);
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.RedLamp);
                    ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.BuzzerCall);
                    break;
            }
        }
        #endregion

        #region 一些功能
        public void ConveyorRun()
        {
            GlobalVar.Instance.UpdateRunMessage("工位1入料皮带启动", LogName.runLog);
            ClassMotion.VMove(GlobalVar.Conveyor[0], 0);//工位1皮带正向运动
            GlobalVar.Instance.UpdateRunMessage("工位2入料皮带启动", LogName.runLog);
            ClassMotion.VMove(GlobalVar.Conveyor[1], 0);//工位2皮带正向运动
            GlobalVar.Instance.UpdateRunMessage("工位2入料皮带启动", LogName.runLog);
            ClassMotion.VMove(GlobalVar.Conveyor[2], 0);//工位3皮带正向运动
        }
        public void ConveyorStop()
        {
            GlobalVar.Instance.UpdateRunMessage("工位1入料皮带停止", LogName.runLog);
            ClassMotion.StopAxis(GlobalVar.Conveyor[0]);//工位1皮带正向运动
            GlobalVar.Instance.UpdateRunMessage("工位2入料皮带停止", LogName.runLog);
            ClassMotion.StopAxis(GlobalVar.Conveyor[1]);//工位2皮带正向运动
            GlobalVar.Instance.UpdateRunMessage("工位2入料皮带停止", LogName.runLog);
            ClassMotion.StopAxis(GlobalVar.Conveyor[2]);//工位3皮带正向运动
        }
        //判断信号是否到位
        public bool WaitIN(ushort CardNumber, ushort IOPort, bool sts, string strerror, int stime)
        {
            bool returnsts = false;
            DateTime starttime = DateTime.Now;
            if (sts)
            {
                while (ClassMotion.ReadInputBit(CardNumber, IOPort) != 0)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                    DateTime endtime = DateTime.Now;
                    TimeSpan spantime = endtime - starttime;
                    if (spantime.TotalSeconds > stime)
                    {
                        returnsts = true;
                        break;
                    }
                }
            }
            else
            {
                while (ClassMotion.ReadInputBit(CardNumber, IOPort) == 0)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                    DateTime endtime = DateTime.Now;
                    TimeSpan spantime = endtime - starttime;
                    if (spantime.TotalSeconds > stime)
                    {
                        returnsts = true;
                        break;
                    }
                }
            }
            return returnsts;
        }

        public bool WaitCommIN(ushort IOPort, bool sts, int stime)
        {
            bool returnsts = false;
            DateTime starttime = DateTime.Now;
            if (sts)
            {
                while (ClassMotion.ReadCommonInput(IOPort) != 0)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                    DateTime endtime = DateTime.Now;
                    TimeSpan spantime = endtime - starttime;
                    if (spantime.TotalSeconds > stime)
                    {
                        returnsts = true;
                        break;
                    }
                }
            }
            return returnsts;
        }

        public bool WaitCamera(bool CameraDone, int stime)
        {
            bool returnsts = true;
            DateTime starttime = DateTime.Now;
            while (!CameraDone)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime;
                if (spantime.TotalSeconds > stime)
                {
                    returnsts = false;
                    break;
                }
            }
            return returnsts;
        }
        public void Station1LineMove(ushort AxisNumber, ushort[] AxisList, RobotPos Point, double DisZ, ushort PosMode, List<double> CameraDis, bool Z_Move_Zero) //轴数量 轴列表 点位 Z轴抬起或下降高度 运动模式，绝对 相对
        {
            if (Z_Move_Zero)
            {
                double[] Station1_P = { Point.PosX + CameraDis[0], Point.PosY + CameraDis[1], -20 };   //Z轴走到0位（原点），防止撞机
                ClassMotion.Station1Line(AxisNumber, AxisList, Station1_P, PosMode);//直线插补绝对运动
                ClassMotion.Station1WaitStopPlus(AxisNumber, AxisList, Station1_P, PosMode);   //等待轴运动到位
            }
            else
            {
                double[] Station1_P = { Point.PosX + CameraDis[0], Point.PosY + CameraDis[1], Point.PosZ1 };  //Z轴走到正确点位
                ClassMotion.Station1Line(AxisNumber, AxisList, Station1_P, PosMode);//直线插补绝对运动
                ClassMotion.Station1WaitStopPlus(AxisNumber, AxisList, Station1_P, PosMode);   //等待轴运动到位
            }
        }
        public void Station2LineMove(ushort AxisNumber, ushort[] AxisList, RobotPos Point, double DisZ, ushort PosMode, List<double> CameraDis, bool Z_Move_Zero) //轴数量 轴列表 点位 Z轴抬起或下降高度 运动模式，绝对 相对
        {
            if (Z_Move_Zero)
            {
                double[] Station2_P = { Point.PosX + CameraDis[0], Point.PosY + CameraDis[1], -20 };//Z轴走到0位（原点），防止撞机
                ClassMotion.Station2Line(AxisNumber, AxisList, Station2_P, PosMode);//直线插补绝对运动
                ClassMotion.Station2WaitStopPlus(AxisNumber, AxisList, Station2_P, PosMode);   //等待轴运动到位
            }
            else
            {
                double[] Station2_P = { Point.PosX + CameraDis[0], Point.PosY + CameraDis[1], Point.PosZ1 };//Z轴走到正确点位
                ClassMotion.Station2Line(AxisNumber, AxisList, Station2_P, PosMode);//直线插补绝对运动
                ClassMotion.Station2WaitStopPlus(AxisNumber, AxisList, Station2_P, PosMode);   //等待轴运动到位
            }
        }
        public void Station3LineMove(ushort AxisNumber, ushort[] AxisList, RobotPos Point, double DisZ, ushort PosMode, List<double> CameraDis, bool Z_Move_Zero) //轴数量 轴列表 点位 Z轴抬起或下降高度 运动模式，绝对 相对
        {
            if (Z_Move_Zero)
            {
                double[] Station3_P = { Point.PosX + CameraDis[0], Point.PosY + CameraDis[1], -20 };//Z轴走到0位（原点），防止撞机
                ClassMotion.Station3Line(AxisNumber, AxisList, Station3_P, PosMode);//直线插补绝对运动
                ClassMotion.Station3WaitStopPlus(AxisNumber, AxisList, Station3_P, PosMode);   //等待轴运动到位
            }
            else
            {
                double[] Station3_P = { Point.PosX + CameraDis[0], Point.PosY + CameraDis[1], Point.PosZ1 };//Z轴走到正确点位
                ClassMotion.Station3Line(AxisNumber, AxisList, Station3_P, PosMode);//直线插补绝对运动
                ClassMotion.Station3WaitStopPlus(AxisNumber, AxisList, Station3_P, PosMode);   //等待轴运动到位
            }
        }
        public void Station4LineMove(ushort AxisNumber, ushort[] AxisList, double X,double Z,ushort PosMode, bool Z_Move_Zero) //轴数量 轴列表 点位 Z轴抬起或下降高度 运动模式，绝对 相对
        {
            if (Z_Move_Zero)
            {
                double[] Station4_P = { X, 0};//Z轴走到0位（原点），防止撞机
                ClassMotion.Station4Line(AxisNumber, AxisList, Station4_P, PosMode);//直线插补绝对运动
                ClassMotion.Station4WaitStopPlus(AxisNumber, AxisList, Station4_P, PosMode);   //等待轴运动到位
            }
            else
            {
                double[] Station4_P = { X, Z };//Z轴走到正确点位
                ClassMotion.Station4Line(AxisNumber, AxisList, Station4_P, PosMode);//直线插补绝对运动
                ClassMotion.Station4WaitStopPlus(AxisNumber, AxisList, Station4_P, PosMode);   //等待轴运动到位
            }
        }
        #endregion

        #region 工位1主线程
        private void Station1Feeding()
        {
            //大量的单条指令在打印log，如果去掉打印log，代码行数，可以减低1/3，可以用其他方法来解决log的问题
            //尽量少用goto语句，无条件跳转，容易引起思路被打断
        CheckFeedBlockUpArrived:
            GlobalVar.Instance.UpdateRunMessage("入料阻挡气缸到工作位", LogName.runLog);
            //封装本身有些问题，可以加一层封装
            //另外，输出和检测，可以封装到一块儿
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.HSGFeedBlockUp, (ushort)IOListOutput.HSGFeedBlockDown);   //入料阻挡气缸上位
            Thread.Sleep(200);
            if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.HSGFeedBlockUp, true, "入料阻挡气缸到工作位超时", GlobalVar.CylinderAlarmTime))
            {
                #region
                GlobalVar.Instance.UpdateRunMessage("入料阻挡气缸到工作位超时", LogName.runLog);
                //selectLight(ColorLight.Red);
                //如要弹出对话框，应该在统一的地方进行处理，除非是特殊的位置。
                //目前这个位置，不应该还有继续工作这样的选项，如果继续工作，可能引起其他问题。
                Notice FeedBlockUpArrived = new Notice();
                FeedBlockUpArrived.SetNotice = "入料阻挡气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                {
                    GlobalVar.Instance.UpdateRunMessage("选择继续等待入料气缸到工作位", LogName.runLog);
                    goto CheckFeedBlockUpArrived;   //检查顶升气缸始位信号
                }
                else
                {
                    GlobalVar.Instance.UpdateRunMessage("选择忽略入料阻挡气缸无到工作位信号", LogName.runLog);
                }
                #endregion
            }
            else
            {

            }
        CheckStation1BlockUpArrived:
            GlobalVar.Instance.UpdateRunMessage("工位1阻挡气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1BlockUp, (ushort)IOListOutput.Station1BlockDown);   //工位1阻挡气缸上位
            Thread.Sleep(200);
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1BlockUp, true, "工位1阻挡气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1阻挡气缸到工作位超时", LogName.runLog);
                    Notice NoticeLocateClamp2Begun = new Notice();
                    NoticeLocateClamp2Begun.SetNotice = "工位1阻挡气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (NoticeLocateClamp2Begun.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1阻挡气缸到工作位信号", LogName.runLog);
                        goto CheckStation1BlockUpArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1阻挡气缸未到工作位信号", LogName.runLog);
                    }
                    #endregion
                }
            }
            //else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("判断工位1是否满足进料条件", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station1BackRay) != 1 &&
            ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1JackUpDown) != 1&&
            ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1BlockUp) == 1)    //利用工位1的对射光纤判断工位1是否有物料存在 并且判断工位1顶升气缸是否在始位，工位1入料阻挡气缸在工作位
                {
                    Application.DoEvents();
                    Thread.Sleep(50);
                }
            }
            else
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1JackUpDown) != 0)    //利用工位1的对射光纤判断工位1是否有物料存在 并且判断工位1顶升气缸是否在始位
                {
                    Application.DoEvents();
                    Thread.Sleep(50);
                }
                Thread.Sleep(3000);
            }
            GlobalVar.Instance.UpdateRunMessage("工位1满足进料条件", LogName.runLog);

        CheckFeedBlockDownArrived:
            GlobalVar.Instance.UpdateRunMessage("入料阻挡气缸下降", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.HSGFeedBlockDown, (ushort)IOListOutput.HSGFeedBlockUp);   //入料阻挡气缸下位
            Thread.Sleep(200);
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.HSGFeedBlockDown, true, "入料阻挡气缸下降到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("入料阻挡气缸下降到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "入料阻挡气缸下降到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待入料阻挡气缸下降到原位", LogName.runLog);
                        goto CheckFeedBlockDownArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略入料阻挡气缸下降未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("等待载具通过入料阻挡进入工位1", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station1FrontRay) != 1 ||
            ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.FeedBackRay) != 1)    //进料工位后对射  工位1前对射 确保两个都没有信号 HSG才算通过  
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }
            else
            {
                Thread.Sleep(3000);
            }
            GlobalVar.Instance.UpdateRunMessage("载具到达工位1", LogName.runLog);
        CheckFeedBlockUpArrived2:
            GlobalVar.Instance.UpdateRunMessage("进料完成后，入料阻挡气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.HSGFeedBlockUp, (ushort)IOListOutput.HSGFeedBlockDown);   //入料阻挡气缸上位
            Thread.Sleep(200);
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.HSGFeedBlockUp, true, "入料阻挡气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("入料阻挡气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "入料阻挡气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待入料传感器到工作位", LogName.runLog);
                        goto CheckFeedBlockUpArrived2;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略入料阻挡气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("等待载具完全到达工位1", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station1FrontRay) != 1 ||
            ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station1BackRay) == 1)    //工站1前对射无信号，工站1 后对射有信号的情况，才表示物料到位，顶升气缸才能上升
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }
            else
            {
                Thread.Sleep(3000);
            }
            GlobalVar.Instance.UpdateRunMessage("载具完全到达工位1", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                KeyenceBarCode.instance().sendData("");
                View_EarthBarcode.instance().sendData("");
            }

        CheckStation1JackUpArrived:
            GlobalVar.Instance.UpdateRunMessage("工位1顶升气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1JackUpUp, (ushort)IOListOutput.Station1JackUpDown);   //工位1顶升气缸上位
            Thread.Sleep(200);
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1JackUpUp, true, "工位1顶升气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1顶升气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1顶升气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1顶升气缸到工作位", LogName.runLog);
                        goto CheckStation1JackUpArrived;   //检查顶升气缸始位信号            
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1顶升气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }

        CheckStation1PressArrived:
            GlobalVar.Instance.UpdateRunMessage("工位1压料气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1PressArrived, (ushort)IOListOutput.Station1PressBegun);   //工位1压料气缸压紧
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1PressArrived, true, "工位1压料气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1压料气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1压料气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1压料气缸到工作位", LogName.runLog);
                        goto CheckStation1PressArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1压紧气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
                else
                {

                }
            }
            _station1AxisStatus = Station1AxisStatus.TakePhoto1;
        }

        private void Station1TaKePhoto1()
        {
            GlobalVar.Instance.UpdateRunMessage("工位1组合轴运动到第一个拍照点上方", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[1], 10, 1, Dis,true);//直线插补绝对运动到P2位置  第1个拍照点位
            GlobalVar.Instance.UpdateRunMessage("工位1组合轴运动到第一个拍照点", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[1], 0, 1, Dis,false);//直线插补绝对运动到P2位置  第1个拍照点位
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Light1ON); //打开光源
        CheckStation1Camera0:
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                GlobalVar.Station1CameraResult[0] = false;  //清空第一个螺丝孔位拍照完成信号
                
                GlobalVar.Instance.UpdateRunMessage("工位1开始第一个孔位拍照", LogName.runLog);
                KeyenceVision1.instance().sendData("CC,27,0");  //发送基恩士拍照
                GlobalVar.Instance.UpdateRunMessage("等待工位1第一个孔位拍照完成", LogName.runLog);
                if (WaitCamera(GlobalVar.Station1CameraResult[0], GlobalVar.CylinderAlarmTime))   //等待拍照完成
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1第一个孔位拍照完成超时，请检查", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1CCD拍照异常，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("工位1选择再次进行拍照", LogName.runLog);
                        goto CheckStation1Camera0;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1拍照结果，NG处理", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {
                Thread.Sleep(3000);
            }
        _station1AxisStatus = Station1AxisStatus.TakePhoto2;
        }
        private void Station1TaKePhoto2()
        {
            GlobalVar.Instance.UpdateRunMessage("工位1组合轴运动到第二个拍照点", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[2], 0, 1, Dis,false);//直线插补绝对运动到P3位置 第二个拍照点位
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Light1ON); //打开光源
        CheckStation1Camera1:
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                GlobalVar.Station1CameraResult[1] = false;  //清空第二个螺丝孔位拍照完成信号
               
                GlobalVar.Instance.UpdateRunMessage("工位1开始第二个孔位拍照", LogName.runLog);
                KeyenceVision1.instance().sendData("CC,27,1");  //发送基恩士拍照
                GlobalVar.Instance.UpdateRunMessage("等待工位1第二次拍照结果", LogName.runLog);
                if (WaitCamera(GlobalVar.Station1CameraResult[1], GlobalVar.CylinderAlarmTime))   //等待拍照完成
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位1第二次拍照结果超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1CCD拍照异常，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("工位1选择再次进行拍照", LogName.runLog);
                        goto CheckStation1Camera1;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1拍照结果，NG处理", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {
                Thread.Sleep(3000);
            }
        _station1AxisStatus = Station1AxisStatus.Lean;
        }
        private void Station1Lean()
        {
        CheckStation1UnlockForwardFront:
            GlobalVar.Instance.UpdateRunMessage("工位1解锁前推气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1UnlockForwardFront, (ushort)IOListOutput.Station1UnlockForwardBack);   //解锁前推
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1UnlockForwardFront, true, "工位1解锁前推气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1解锁前推气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1解锁前推气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1解锁前推气缸到工作位", LogName.runLog);
                        goto CheckStation1UnlockForwardFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1解锁前推气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {
                Thread.Sleep(1000);
            }
        CheckStation1UnlockClampFront:
            GlobalVar.Instance.UpdateRunMessage("工位1解锁夹紧气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1UnlockClampFront, (ushort)IOListOutput.Station1UnlockClampBack);   //解锁夹紧
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1UnlockClampFront, true, "工位1解锁夹紧气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1解锁夹紧气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1解锁夹紧气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1解锁夹紧气缸到工作位", LogName.runLog);
                        goto CheckStation1UnlockClampFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1解锁夹紧气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }    
        CheckStation1UnlockLeanBack:
            GlobalVar.Instance.UpdateRunMessage("工位1解锁倾斜气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1UnlockLeanBack, (ushort)IOListOutput.Station1UnlockLeanFront);   //解锁倾斜
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1UnlockLeanBack, true, "工位1解锁倾斜气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1解锁倾斜气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1解锁倾斜气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1解锁倾斜气缸到原位", LogName.runLog);
                        goto CheckStation1UnlockLeanBack;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1解锁倾斜气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            Thread.Sleep(1000);
        CheckStation1UnlockLeanFront:
            GlobalVar.Instance.UpdateRunMessage("工位1解锁倾斜气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1UnlockLeanFront, (ushort)IOListOutput.Station1UnlockLeanBack);   //解锁倾斜
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1UnlockLeanFront, true, "工位1解锁倾斜气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1解锁倾斜气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1解锁倾斜气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1解锁倾斜气缸到工作位", LogName.runLog);
                        goto CheckStation1UnlockLeanFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1解锁倾斜气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
            _station1AxisStatus = Station1AxisStatus.TakePhoto3;
        }
        private void Station1TaKePhoto3()
        {
        CheckStation1Light2Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸2到拍照位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station1Light2Begun, (ushort)IOListOutput.Station1Light2Arrived);   //工位1压料气缸压紧
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station1Light2Arrived, true, "工位1光源1气缸2未到拍照位", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸2到拍照位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1光源1气缸2不到位，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1光源1气缸2到拍照位", LogName.runLog);
                        goto CheckStation1Light2Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1光源1气缸2未到拍照位", LogName.runLog);
                    }
                    #endregion
                }
                else
                {

                }
            }
            GlobalVar.Instance.UpdateRunMessage("等待工位1组合轴运动到第三个拍照点", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[3], 0, 1, Dis,false);//直线插补绝对运动到P4位置 第三个拍照点位
            GlobalVar.Instance.UpdateRunMessage("工位1组合轴运动到第三个拍照点", LogName.runLog);
        CheckStation1Camera2:
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                GlobalVar.Station1CameraResult[2] = false;  //清空第三个螺丝孔位拍照完成信号
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Light1ON); //打开光源
                GlobalVar.Instance.UpdateRunMessage("工位1开始第三个孔位拍照", LogName.runLog);
                KeyenceVision1.instance().sendData("CC,27,2");  //发送基恩士拍照
                GlobalVar.Instance.UpdateRunMessage("等待工位1第三次拍照完成", LogName.runLog);
                if (WaitCamera(GlobalVar.Station1CameraResult[2], GlobalVar.CylinderAlarmTime))   //等待拍照完成
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位1第三次拍照完成超时，请检查", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1CCD拍照异常，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择再次进行工位1第三次拍照", LogName.runLog);
                        goto CheckStation1Camera2;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1第三次拍照结果，NG处理", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {
                Thread.Sleep(3000);
            }
        _station1AxisStatus = Station1AxisStatus.TakePhoto4;
        }
        private void Station1TaKePhoto4()
        {
        CheckStation1Light1Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸1到拍照位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station1Light1Begun, (ushort)IOListOutput.Station1Light1Arrived);   //工位1压料气缸压紧
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station1Light1Arrived, true, "工位1光源1气缸1未到拍照位", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸1到拍照位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1光源1气缸1不到位，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1光源1气缸1到拍照位", LogName.runLog);
                        goto CheckStation1Light1Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1光源1气缸1未到拍照位", LogName.runLog);
                    }
                    #endregion
                }
                else
                {

                }
            }
        
            GlobalVar.Instance.UpdateRunMessage("等待工位1组合轴运动到第四个拍照点", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[4], 0, 1, Dis,false);//直线插补绝对运动到P5位置 第四个拍照点位
            GlobalVar.Instance.UpdateRunMessage("工位1组合轴运动到第四个拍照点", LogName.runLog);
        CheckStation1Camera3:
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                GlobalVar.Station1CameraResult[3] = false;  //清空第四个螺丝孔位拍照完成信号
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Light1ON); //打开光源
                GlobalVar.Instance.UpdateRunMessage("工位1开始第四个孔位拍照", LogName.runLog);
                KeyenceVision1.instance().sendData("CC,27,3");  //发送基恩士拍照
                GlobalVar.Instance.UpdateRunMessage("等待工位1第四次开始拍照", LogName.runLog);
                if (WaitCamera(GlobalVar.Station1CameraResult[3], GlobalVar.CylinderAlarmTime))   //等待拍照完成
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位1第四次开始拍照超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "CCD拍照异常，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择再次进行工位1第四次拍照", LogName.runLog);
                        goto CheckStation1Camera3;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1第四次拍照结果，NG处理", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {
                Thread.Sleep(3000);
            }
            GlobalVar.Instance.UpdateRunMessage("等待工位1组合轴运动到第四个拍照点上方", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[4], 10, 1, Dis, true);//直线插补绝对运动到P5位置 第四个拍照点位
            GlobalVar.Instance.UpdateRunMessage("等待工位1组合轴运动到第3个拍照点上方", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[3], 10, 1, Dis, true);//直线插补绝对运动到P5位置 第四个拍照点位
            GlobalVar.Instance.UpdateRunMessage("等待工位1组合轴运动到第1个拍照点上方", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[1], 10, 1, Dis, true);//直线插补绝对运动到P5位置 第四个拍照点位
            GlobalVar.Instance.UpdateRunMessage("等待工位1组合轴运动到等待位上方", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[0], 10, 1, Dis,true);//直线插补绝对运动到P1位置 等待位
            GlobalVar.Instance.UpdateRunMessage("等待工位1组合轴运动到等待位", LogName.runLog);
            Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[0], 0, 1, Dis,false);//直线插补绝对运动到P1位置 等待位
            GlobalVar.Instance.UpdateRunMessage("工位1组合轴运动到等待位", LogName.runLog);
            _station1AxisStatus = Station1AxisStatus.CarrierReset;
        }
        private void Station1CarrierReset()
        {
        CheckStation1Light2Begun:
            GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸2到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station1Light2Arrived, (ushort)IOListOutput.Station1Light2Begun);   //工位1压料气缸压紧
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station1Light2Begun, true, "工位1光源1气缸2到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸2到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1光源1气缸2到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1光源1气缸2到原位", LogName.runLog);
                        goto CheckStation1Light2Begun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1光源1气缸2未到原位位", LogName.runLog);
                    }
                    #endregion
                }
                else
                {

                }
            }
        CheckStation1Light1Begun:
            GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸1到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station1Light1Arrived, (ushort)IOListOutput.Station1Light1Begun);   //工位1压料气缸压紧
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station1Light1Arrived, true, "工位1光源1气缸1到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸1到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1光源1气缸1到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1光源1气缸1到原位", LogName.runLog);
                        goto CheckStation1Light1Begun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1光源1气缸1未到原位", LogName.runLog);
                    }
                    #endregion
                }
                else
                {

                }
            }
        CheckStation1LeanBackArrived:
            GlobalVar.Instance.UpdateRunMessage("工位1倾斜气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1LeanBackArrived, (ushort)IOListOutput.Station1LeanBackBegun);   //倾斜气缸回退
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1LeanBackArrived, true, "工位1倾斜气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1倾斜气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1倾斜气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("继续等待工位1倾斜气缸到工作位", LogName.runLog);
                        goto CheckStation1LeanBackArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1倾斜气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
            Thread.Sleep(1000);
        CheckStation1LeanBackBegun:
            GlobalVar.Instance.UpdateRunMessage("工位1倾斜气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1LeanBackBegun, (ushort)IOListOutput.Station1LeanBackArrived);   //倾斜气缸回退
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1LeanBackBegun, true, "工位1倾斜气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1倾斜气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1倾斜气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("继续等待工位1倾斜气缸到原位", LogName.runLog);
                        goto CheckStation1LeanBackBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1倾斜气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
        CheckStation1UnlockClampFront:
            GlobalVar.Instance.UpdateRunMessage("工位1解锁夹紧气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1UnlockClampBack, (ushort)IOListOutput.Station1UnlockClampFront);   //解锁夹紧
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1UnlockClampBack, true, "工位1解锁夹紧气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1解锁夹紧气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1解锁夹紧气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1解锁夹紧气缸到原位", LogName.runLog);
                        goto CheckStation1UnlockClampFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1解锁夹紧气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //  else
            {
                Thread.Sleep(1000);
            }    
        CheckStation1UnlockForwardFront:
            GlobalVar.Instance.UpdateRunMessage("工位1解锁前推气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1UnlockForwardBack, (ushort)IOListOutput.Station1UnlockForwardFront);   //解锁前推
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1UnlockForwardBack, true, "工位1解锁前推气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1解锁前推气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1解锁前推气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1解锁前推气缸到原位", LogName.runLog);
                        goto CheckStation1UnlockForwardFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1解锁前推气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {
                Thread.Sleep(1000);
            }
        
        CheckStation1PressBegun:
            GlobalVar.Instance.UpdateRunMessage("工位1压料气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1PressBegun, (ushort)IOListOutput.Station1PressArrived);   //工位1压料气缸松开
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1PressBegun, true, "工位1压料气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1压料气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1压料气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1压料气缸到原位", LogName.runLog);
                        goto CheckStation1PressBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1压料气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
        CheckStation1JackUpDown:
            GlobalVar.Instance.UpdateRunMessage("工位1顶升气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1JackUpDown, (ushort)IOListOutput.Station1JackUpUp);   //工位1顶升气缸下位
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1JackUpDown, true, "工位1顶升气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1顶升气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1顶升气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1顶升气缸到原位", LogName.runLog);
                        goto CheckStation1JackUpDown;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1顶升气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
            _station1AxisStatus = Station1AxisStatus.DisCharge;
        }
        public void Station1Discharge()
        {
            DateTime starttime = DateTime.Now;
            GlobalVar.Instance.UpdateRunMessage("等待工位2空闲允许工位1出料", LogName.runLog);
            GlobalVar.Station1DischargeLabel = true;
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2BackRay) != 1 &&
               ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2JackUpDown) != 1&&
               ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2BlockUp) == 1)      //利用对射光纤判断皮带上是否有物料，并判断顶升气缸是否在始位,工位2入料阻挡气缸在工作位置
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                    DateTime endtime = DateTime.Now;
                    TimeSpan spantime = endtime - starttime;
                    if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                    {
                        #region
                        Notice FeedBlockUpArrived = new Notice();
                        FeedBlockUpArrived.SetNotice = "工位2对射光纤有信号或者工位2顶升气缸不在始位，不允许向工位2出料，按“OK”继续检查，按“Cancel”继续工作";
                        if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                        {

                        }
                        else
                        {
                            break;
                        }
                        #endregion
                    }
                }
            }
            else
            {

            }
            DateTime starttime1 = DateTime.Now;
            while (!GlobalVar.Station2EnableStatuon1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime1;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位1做料完成向工位2出料", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    starttime1 = DateTime.Now;
                    #endregion
                }
            }
            GlobalVar.Station2EnableStatuon1 = false;
            _station1AxisStatus = Station1AxisStatus.Feeding;
        }
        #endregion

        #region 工位2主线程

        private void Station2Feeding()
        {
        CheckFeedBlockUpArrived:
            GlobalVar.Instance.UpdateRunMessage("工位2阻挡气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2BlockUp, (ushort)IOListOutput.Station2BlockDown);   //入料阻挡气缸上位
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2BlockUp, true, "工位2阻挡气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2阻挡气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2阻挡气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2阻挡气缸到工作位", LogName.runLog);
                        goto CheckFeedBlockUpArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2阻挡气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //else
            {

            }
            DateTime starttime = DateTime.Now;
            while (!GlobalVar.Station1DischargeLabel)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位1做料完成向工位2出料", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    starttime = DateTime.Now;
                    #endregion
                }
            }
            GlobalVar.Station1DischargeLabel = false;
        CheckStation1BlockDown:
            GlobalVar.Instance.UpdateRunMessage("工位1阻挡气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1BlockDown, (ushort)IOListOutput.Station1BlockUp);   //工位1阻挡气缸下位
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1BlockDown, true, "工位1阻挡气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1阻挡气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1阻挡气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1阻挡气缸到原位", LogName.runLog);
                        goto CheckStation1BlockDown;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1阻挡气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("等待载具通过工位1", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station1BackRay) != 1 ||
                ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2FrontRay) != 1)    //  工位1后对射  工位2前对射 确保两个都没有信号 HSG才算通过  
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }
            else
            {
                Thread.Sleep(1000);
            }
            GlobalVar.Instance.UpdateRunMessage("载具通过工位1", LogName.runLog);
        CheckStation1BlockUp:
            GlobalVar.Instance.UpdateRunMessage("工位1阻挡气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1BlockUp, (ushort)IOListOutput.Station1BlockDown);   //工位1阻挡气缸上位
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1BlockUp, true, "工位1阻挡气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1阻挡气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1阻挡气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1阻挡气缸到工作位", LogName.runLog);
                        goto CheckStation1BlockUp;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1阻挡气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("等待载具到达工位2", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station1BackRay) != 1 ||
                ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2FrontRay) != 1)    //工站2前对射无信号，工站2后对射有信号的情况，才表示物料到位，顶升气缸才能上升
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }
            //else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("载具到达工位2", LogName.runLog);
        CheckStation2JackUpUp:
            GlobalVar.Instance.UpdateRunMessage("工位2顶升气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2JackUpUp, (ushort)IOListOutput.Station2JackUpDown);   //工位2顶升气缸上位
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2JackUpUp, true, "工位2顶升气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2顶升气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2顶升气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2顶升气缸到工作位", LogName.runLog);
                        goto CheckStation2JackUpUp;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2阻挡气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
        CheckStation2PressArrived:
            GlobalVar.Instance.UpdateRunMessage("工位2压料气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2PressArrived, (ushort)IOListOutput.Station2PressBegun);   //工位2压料气缸压紧
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2PressArrived, true, "工位2压料气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2压料气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2压料气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2压料气缸到工作位", LogName.runLog);
                        goto CheckStation2PressArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2压料气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
            GlobalVar.Station2EnableStatuon1 = true;
            _station2AxisStatus = Station2AxisStatus.TakePhoto;
        }
        private void Station2TakePtoto()
        {

        CheckStation2Light1Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位2光源1气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2Light1Arrived, (ushort)IOListOutput.Station2Light1Begun);   //工位2光源1气缸伸出
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2Light1Arrived, true, "工位2光源1气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2光源1气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2光源1气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2光源1气缸到工作位", LogName.runLog);
                        goto CheckStation2Light1Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2光源1气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("工位2发送拍照指令", LogName.runLog);
        CheckStation1Camera0:
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                GlobalVar.Station2CameraResult[0] = false; //清除拍照完成信号
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Light1ON); //打开光源
                GlobalVar.Instance.UpdateRunMessage("工位2开始三号孔位拍照", LogName.runLog);
                KeyenceVision2.instance().sendData("CC,25");  //发送基恩士拍照
                GlobalVar.Instance.UpdateRunMessage("工位2三号孔等待拍照完成", LogName.runLog);
                if (WaitCamera(GlobalVar.Station2CameraResult[0], GlobalVar.CylinderAlarmTime))   //等待拍照完成
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2三号孔等待拍照完成超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2三号孔等待拍照完成超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2CCD等待拍照完成", LogName.runLog);
                        goto CheckStation1Camera0;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位二拍照未完成，NG处理", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
        CheckStation2Light1Begun:
            GlobalVar.Instance.UpdateRunMessage("工位2光源1气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2Light1Begun, (ushort)IOListOutput.Station2Light1Arrived);   //工位2光源1气缸退回
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2Light1Begun, true, "工位2光源1气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2光源1气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2光源1气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2光源1气缸到原位", LogName.runLog);
                        goto CheckStation2Light1Begun;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2光源1气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            _station2AxisStatus = Station2AxisStatus.AutoScrew1;
        }
        private void Station2AotoScrew1()
        {
        CheckStation2LeftScrewBoxReady:
            GlobalVar.Instance.UpdateRunMessage("等待工位2左螺丝盒准备好", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2LeftScrewBoxReady, true, "工位2左螺丝盒没有准备好", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位2左螺丝盒准备好超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2左螺丝盒没有准备好，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2左螺丝盒准备好", LogName.runLog);
                        goto CheckStation2LeftScrewBoxReady;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续取消等待工位2左螺丝盒准备好", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("工位2组合轴移动到取螺丝位上方", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[1], 10, 1, Dis,true);  //移动到取螺丝位上方10mm处
            GlobalVar.Instance.UpdateRunMessage("工位2组合轴移动到取螺丝位", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[1], 0, 1, Dis, false);  //移动到取螺丝位
        CheckStation2EleScrewReady:
            GlobalVar.Instance.UpdateRunMessage("等待电批1准备好", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2EleScrewReady, true, "电批1没有准备好", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批1准备好超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "电批1没有准备好，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待电批1准备好", LogName.runLog);
                        goto CheckStation2EleScrewReady;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略电批1未准备好", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {

            }
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD0);   //电批预设
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD1);    //电批预设
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD2);    //电批预设
            GlobalVar.Instance.UpdateRunMessage("打开电批真空", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2ScrewVacuum);  //打开真空
            }
            else
            {

            }

        CheckStation2ScrewCameraDown:
            GlobalVar.Instance.UpdateRunMessage("工位2螺丝臂摄像头到工作位", LogName.runLog);
       // ClassMotion.CylinderComm((ushort)IOListOutput.Station2ScrewCameraUP, (ushort)IOListOutput.Station2ScrewCameraDown); 
            ClassMotion.CylinderComm((ushort)IOListOutput.Station2ScrewCameraUP, (ushort)IOListOutput.Station2ScrewCameraDown);   //工位2光源1气缸退回
         // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitCommIN((ushort)IOListInput.Station2ScrewCameraDown, true, GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2螺丝臂摄像头到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2螺丝臂摄像头到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2螺丝臂摄像头到工作位", LogName.runLog);
                        goto CheckStation2ScrewCameraDown;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2螺丝臂摄像头未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }


            GlobalVar.Instance.UpdateRunMessage("组合轴2取完螺丝后向上抬起", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[1], 10, 1, Dis,true);  //移动到取螺丝位上方10mm处   分左右
            GlobalVar.Instance.UpdateRunMessage("组合轴2移动到第一个螺丝空位上方", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[2], 10, 1, GlobalVar.Station2Screw3Location,true);  //移动到第一个安装位上方10mm处  添加视觉补偿
            GlobalVar.Instance.UpdateRunMessage("组合轴2移动到添加相机补偿的第一个螺丝孔位", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[2], 0, 1, GlobalVar.Station2Screw3Location, false);  //移动到第一个安装位  添加
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD0);   //电批预设
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD1);    //电批预设
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD2);    //电批预设
        CheckStation2ScrewCameraUP:
            GlobalVar.Instance.UpdateRunMessage("工位2螺丝臂摄像头到原位", LogName.runLog);
            ClassMotion.CylinderComm((ushort)IOListOutput.Station2ScrewCameraDown, (ushort)IOListOutput.Station2ScrewCameraUP);   //工位2光源1气缸退回
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitCommIN((ushort)IOListInput.Station2ScrewCameraDown, true, GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2螺丝臂摄像头到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2螺丝臂摄像头到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2螺丝臂摄像头到原位", LogName.runLog);
                        goto CheckStation2ScrewCameraUP;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2螺丝臂摄像头未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("工位2启动电批", LogName.runLog);
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2EleScrewStart);  //启动电批
            }
           // else
            {

            }

            DateTime starttime1 = DateTime.Now;
            while ( ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2EleScrewRun) == 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime1;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批1启动", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    #endregion
                    starttime1 = DateTime.Now;
                }
            }

            DateTime starttime = DateTime.Now;
            while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2EleScrewDone) == 1 && ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2EleScrewError) == 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批1完成", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    #endregion
                    starttime = DateTime.Now;
                }
            }
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2EleScrewStart);  //关闭电批
            GlobalVar.Instance.UpdateRunMessage("关闭真空", LogName.runLog);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2ScrewVacuum);
            GlobalVar.Instance.UpdateRunMessage("打开破真空吹气", LogName.runLog);
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2Blow); //破真空
            Thread.Sleep(100);
            GlobalVar.Instance.UpdateRunMessage("关闭破真空吹气", LogName.runLog);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2Blow); //关闭破真空
            GlobalVar.Instance.UpdateRunMessage("工位2组合轴在第一个螺丝孔位上方抬起", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[2], 10, 1, GlobalVar.Station2Screw3Location, true);  //移动到第一个安装位上方10mm处
            _station2AxisStatus = Station2AxisStatus.AutoScrew2;
        }
        private void Station2AotoScrew2()
        {
            /////////////////////////////////////////////////////////////
            GlobalVar.Instance.UpdateRunMessage("工位2组合轴移动到第右螺丝盒上方", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[3], 10, 1, Dis,true);  //移动到取螺丝位上方10mm处
            GlobalVar.Instance.UpdateRunMessage("工位2组合轴移动到第右螺丝盒位", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[3], 0, 1, Dis,false);  //移动到取螺丝位上方    分左右
        CheckStation2EleScrewReady1:
            GlobalVar.Instance.UpdateRunMessage("等待电批1准备好", LogName.runLog);
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD0);   //电批预设
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD1);    //电批预设
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD2);    //电批预设
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2EleScrewReady, true, "电批2没有准备好", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批1准备好超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "电批2没有准备好，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待等待电批1准备好", LogName.runLog);
                        goto CheckStation2EleScrewReady1;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略电批1未准备好", LogName.runLog);
                    }
                    #endregion
                }
                GlobalVar.Instance.UpdateRunMessage("打开电批1真空", LogName.runLog);
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2ScrewVacuum);  //打开真空
            }
            else
            {

            }


        CheckStation2ScrewCameraDown:
            GlobalVar.Instance.UpdateRunMessage("工位2螺丝臂摄像头到工作位", LogName.runLog);
        ClassMotion.CylinderComm((ushort)IOListOutput.Station2ScrewCameraUP, (ushort)IOListOutput.Station2ScrewCameraDown); 
             //工位2光源1气缸退回
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitCommIN((ushort)IOListInput.Station2ScrewCameraUP, true, GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2螺丝臂摄像头到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2螺丝臂摄像头未工作位，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2螺丝臂摄像头到工作位", LogName.runLog);
                        goto CheckStation2ScrewCameraDown;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2螺丝臂摄像头未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }


            GlobalVar.Instance.UpdateRunMessage("组合轴2右螺丝盒取螺丝后抬起", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[3], 10, 1, Dis,true);  //移动到取螺丝位上方10mm处   分左右
            GlobalVar.Instance.UpdateRunMessage("组合轴2移动到第二个螺丝孔位上方", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[4], 10, 1, GlobalVar.Station2Screw4Location, true);  //移动到第二个安装位上方10mm处  添加视觉补偿
            GlobalVar.Instance.UpdateRunMessage("组合轴2移动到第二个螺丝孔位", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[4], 0, 1, GlobalVar.Station2Screw4Location, false);  //移动到第二个安装位  添加
            

            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD0);   //电批预设
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD1);    //电批预设
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation2EleScrewSelectD2);    //电批预设
        CheckStation2ScrewCameraUP:
            GlobalVar.Instance.UpdateRunMessage("工位2螺丝臂摄像头到原位", LogName.runLog);
        ClassMotion.CylinderComm((ushort)IOListOutput.Station2ScrewCameraDown, (ushort)IOListOutput.Station2ScrewCameraUP);  
             //工位2光源1气缸退回
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitCommIN((ushort)IOListInput.Station2ScrewCameraDown, true,GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2螺丝臂摄像头到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2螺丝臂摄像头未原位，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2螺丝臂摄像头到原位", LogName.runLog);
                        goto CheckStation2ScrewCameraUP;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2螺丝臂摄像头未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }

            GlobalVar.Instance.UpdateRunMessage("启动电批1", LogName.runLog);
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2EleScrewStart);  //启动电批
            DateTime starttime1 = DateTime.Now;
            while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2EleScrewRun) == 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime1;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批1启动", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    #endregion
                    starttime1 = DateTime.Now;
                }
            }
            DateTime starttime = DateTime.Now;
            while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2EleScrewDone) == 1 && ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2EleScrewError) == 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批1完成", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    #endregion
                    starttime = DateTime.Now;
                }
            }
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2EleScrewStart);  //关闭电批
            GlobalVar.Instance.UpdateRunMessage("关闭电批1真空", LogName.runLog);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2ScrewVacuum); //关掉真空吸
            GlobalVar.Instance.UpdateRunMessage("开启电批1破真空吹气", LogName.runLog);
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2Blow); //破真空
            Thread.Sleep(100);
            GlobalVar.Instance.UpdateRunMessage("关闭电批1破真空吹气", LogName.runLog);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2Blow); //关闭破真空

            GlobalVar.Instance.UpdateRunMessage("组合轴2在第二个螺丝孔抬起", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[4], 10, 1, Dis,true);  //移动到第二个放螺丝上方10mm处   分左右
            GlobalVar.Instance.UpdateRunMessage("组合轴2移动到等待位上方", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[0], 10, 1, Dis,true);  //移动到等待位上方10mm处   分左右
            GlobalVar.Instance.UpdateRunMessage("组合轴2移动到等待位", LogName.runLog);
            Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[0], 0, 1, Dis,false);  //移动到等待位
            _station2AxisStatus = Station2AxisStatus.Lean;
        }
        private void Station2Lean()
        {

        CheckStation2UnlockForwardFront:
            GlobalVar.Instance.UpdateRunMessage("工位2解锁前推气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2UnlockForwardFront, (ushort)IOListOutput.Station2UnlockForwardBack);   //解锁前推
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2UnlockForwardFront, true, "工位2解锁前推气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2解锁前推气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2解锁前推气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2解锁前推气缸到工作位", LogName.runLog);
                        goto CheckStation2UnlockForwardFront;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2解锁前推气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
        CheckStation2UnlockClampFront:
            GlobalVar.Instance.UpdateRunMessage("工位2解锁夹紧气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2UnlockClampBack, (ushort)IOListOutput.Station2UnlockClampFront);   //工站2解锁夹紧
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2UnlockClampBack, true, "工位2解锁夹紧气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2解锁夹紧气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2解锁夹紧气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2解锁夹紧气缸到工作位", LogName.runLog);
                        goto CheckStation2UnlockClampFront;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2解锁夹紧气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
        CheckStation2LeanBack:
            GlobalVar.Instance.UpdateRunMessage("工位2倾斜气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2LeanBack, (ushort)IOListOutput.Station2LeanFront);   //工位2倾斜
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2LeanBack, true, "工位2倾斜气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2倾斜气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2倾斜气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2倾斜气缸到工作位", LogName.runLog);
                        goto CheckStation2LeanBack;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2倾斜气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
            Thread.Sleep(1000);
        CheckStation2LeanFront:
            GlobalVar.Instance.UpdateRunMessage("工位2倾斜气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2LeanFront, (ushort)IOListOutput.Station2LeanBack);   //工位2倾斜
            //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2LeanFront, true, "工位2倾斜气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2倾斜气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2倾斜气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2倾斜气缸到原位", LogName.runLog);
                        goto CheckStation2LeanFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2倾斜气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //  else
            {
                Thread.Sleep(1000);
            }
            _station2AxisStatus = Station2AxisStatus.CarrierReset;
        }
        private void Station2CarrierReset()
        {
       
        CheckStation2UnlockClampFront:
            GlobalVar.Instance.UpdateRunMessage("工位2解锁夹紧气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2UnlockClampFront, (ushort)IOListOutput.Station2UnlockClampBack);   //工站2解锁夹紧
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2UnlockClampFront, true, "工位2解锁夹紧气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2解锁夹紧气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2解锁夹紧气缸到工原超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2解锁夹紧气缸到原位", LogName.runLog);
                        goto CheckStation2UnlockClampFront;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2解锁夹紧气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //  else
            {
                Thread.Sleep(200);
            }
        CheckStation2UnlockForwardFront:
            GlobalVar.Instance.UpdateRunMessage("工位2解锁前推气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2UnlockForwardBack, (ushort)IOListOutput.Station2UnlockForwardFront);   //解锁前推
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2UnlockForwardBack, true, "工位2解锁前推气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2解锁前推气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2解锁前推气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2解锁前推气缸到原位", LogName.runLog);
                        goto CheckStation2UnlockForwardFront;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2解锁前推气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
        CheckStation2PressBegun:
            GlobalVar.Instance.UpdateRunMessage("工位2压料气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2PressBegun, (ushort)IOListOutput.Station2PressArrived);   //工位2压料气缸松开
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2PressBegun, true, "工位2压料气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2压料气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2压料气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2压料气缸到原位", LogName.runLog);
                        goto CheckStation2PressBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2压料气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
        CheckStation2JackUpDown:
            GlobalVar.Instance.UpdateRunMessage("工位2顶升气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2JackUpDown, (ushort)IOListOutput.Station2JackUpUp);   //工位2顶升气缸下位
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2JackUpDown, true, "工位2顶升气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2顶升气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2顶升气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2顶升气缸到原位", LogName.runLog);
                        goto CheckStation2JackUpDown;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2顶升气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
            _station2AxisStatus = Station2AxisStatus.DisCharge;
        }
        public void Station2Discharge()
        {
            DateTime starttime = DateTime.Now;
            GlobalVar.Instance.UpdateRunMessage("等待工位3允许进料", LogName.runLog);
            GlobalVar.Station2DischargeLabel = true;
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3BackRay) != 1 &&
               ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3JackUpDown) != 1 &&
              ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3BlockUp) != 1)      //利用对射光纤判断皮带上是否有物料，并判断顶升气缸是否在始位
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                    DateTime endtime = DateTime.Now;
                    TimeSpan spantime = endtime - starttime;
                    if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                    {
                        #region
                        Notice FeedBlockUpArrived = new Notice();
                        FeedBlockUpArrived.SetNotice = "工位3对射光纤有信号或者工位3顶升气缸不在始位，不允许向工位3出料，按“OK”继续检查，按“Cancel”继续工作";
                        if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                        {

                        }
                        else
                        {
                            break;
                        }
                        #endregion
                    }
                }
            }
            else
            {

            }
            
            DateTime starttime1 = DateTime.Now;
            while (!GlobalVar.Station3EnableStatuon2)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime1;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位1做料完成向工位2出料", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    starttime1 = DateTime.Now;
                    #endregion
                }
            }
            GlobalVar.Station3EnableStatuon2 = false;
           
            _station2AxisStatus = Station2AxisStatus.Feeding;
        }
        #endregion

        #region 工位3主线程
        private void Station3Feeding()
        {
        CheckFeedBlockUpArrived:
            GlobalVar.Instance.UpdateRunMessage("工位3阻挡气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3BlockUp, (ushort)IOListOutput.Station3BlockDown);   //入料阻挡气缸上位
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3BlockUp, true, "工位3阻挡气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3阻挡气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3阻挡气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3阻挡气缸到工作位", LogName.runLog);
                        goto CheckFeedBlockUpArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3阻挡气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            DateTime starttime = DateTime.Now;
            while (!GlobalVar.Station2DischargeLabel)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位2做料完成向工位3出料", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    #endregion
                    starttime = DateTime.Now;
                }
            }
            GlobalVar.Station2DischargeLabel = false;
        CheckStation2BlockDown:
            GlobalVar.Instance.UpdateRunMessage("工位2阻挡气缸到原位", LogName.runLog);
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2BlockDown, (ushort)IOListOutput.Station2BlockUp);   //工位2阻挡气缸下位
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2BlockDown, true, "工位2阻挡气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2阻挡气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2阻挡气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2阻挡气缸到原位", LogName.runLog);
                        goto CheckStation2BlockDown;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2阻挡气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("等待载具完全通过工位2", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station2BackRay) != 1 ||
                ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3FrontRay) != 1)    //  工位2后对射  工位3前对射 确保两个都没有信号 HSG才算通过  
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }
            else
            {
                Thread.Sleep(1000);
            }
            GlobalVar.Instance.UpdateRunMessage("载具完全通过工位2", LogName.runLog);
        CheckStation2BlockUp:
            GlobalVar.Instance.UpdateRunMessage("工位2阻挡气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2BlockUp, (ushort)IOListOutput.Station2BlockDown);   //工位2阻挡气缸上位
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2BlockUp, true, "工位2阻挡气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2阻挡气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2阻挡气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2阻挡气缸到工作位", LogName.runLog);
                        goto CheckStation2BlockUp;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2阻挡气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("等待载具完全到达工位3", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3BackRay) != 1 ||
                ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3FrontRay) == 1)    //工站2前对射无信号，工站2后对射有信号的情况，才表示物料到位，顶升气缸才能上升
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }
            else
            {

            }
            GlobalVar.Instance.UpdateRunMessage("载具完全到达工位3", LogName.runLog);
        CheckStation3JackUpUp:
            GlobalVar.Instance.UpdateRunMessage("工位3顶升气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3JackUpUp, (ushort)IOListOutput.Station3JackUpDown);   //工位3顶升气缸上位
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3JackUpUp, true, "工位3顶升气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3顶升气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3顶升气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3顶升气缸到工作位", LogName.runLog);
                        goto CheckStation3JackUpUp;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3顶升气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
        CheckStation3PressArrived:
            GlobalVar.Instance.UpdateRunMessage("工位3压料气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3PressArrived, (ushort)IOListOutput.Station3PressBegun);   //工位3压料气缸压紧
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3PressArrived, true, "工位3压料气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3压料气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3压料气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3压料气缸到工作位", LogName.runLog);
                        goto CheckStation3PressArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3压料气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
            GlobalVar.Station3EnableStatuon2 = true;
            _station3AxisStatus = Station3AxisStatus.TakePhoto1;
        }
        private void Station3TakePhoto1()
        {
        CheckStation3Light1Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位3光源1气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3Light1Arrived, (ushort)IOListOutput.Station3Light1Begun);   //工位2光源1气缸伸出
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3Light1Arrived, true, "工位3光源1气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源1气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源1气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源1气缸到工作位", LogName.runLog);
                        goto CheckStation3Light1Arrived;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源1未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {
                Thread.Sleep(500);
            }
        CheckStation3Light2Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位3光源2气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3Light2Arrived, (ushort)IOListOutput.Station3Light2Begun);   //工位3光源2气缸伸出
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3Light2Arrived, true, "工位3光源2气缸伸出不到位", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源2气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源2气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源2气缸到工作位", LogName.runLog);
                        goto CheckStation3Light2Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源2未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
         //   else
            {
                Thread.Sleep(500);
            }
        CheckStation3Light3Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位3光源3气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Light3Begun, (ushort)IOListOutput.Station3Light3Arrived);   //工位3光源2气缸伸出
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3Light3Arrived, true, "工位3光源3气缸伸出不到位", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源3气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源3气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源3气缸到工作位", LogName.runLog);
                        goto CheckStation3Light3Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源3未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {
                Thread.Sleep(500);
            }
        CheckStation3Light4Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位3光源4气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Light4Begun, (ushort)IOListOutput.Station3Light4Arrived);   //工位3光源2气缸伸出
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3Light4Arrived, true, "工位3光源4气缸伸出不到位", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源4气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源4气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源4气缸到工作位", LogName.runLog);
                        goto CheckStation3Light4Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源4未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
        CheckStation3Light5Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位3光源5气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Light5Begun, (ushort)IOListOutput.Station3Light5Arrived);   //工位3光源2气缸伸出
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3Light5Arrived, true, "工位3光源5气缸伸出不到位", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源5气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源5气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源5气缸到工作位", LogName.runLog);
                        goto CheckStation3Light5Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源5未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {
                Thread.Sleep(500);
            }
        CheckStation3Camera0:
            GlobalVar.Instance.UpdateRunMessage("工位3相机1开始拍照", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                GlobalVar.Station3CameraResult[0] = false; //清除拍照完成信号
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Light1ON); //打开光源
                GlobalVar.Instance.UpdateRunMessage("工位3开始五号孔位拍照", LogName.runLog);
                KeyenceVision2.instance().sendData("CC,29");  //发送基恩士拍照
                GlobalVar.Instance.UpdateRunMessage("等待工位3相机1拍照结果", LogName.runLog);
                if (WaitCamera(GlobalVar.Station3CameraResult[0], GlobalVar.CylinderAlarmTime))   //等待拍照完成
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3相机1拍照结果超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "CCD3拍照异常，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("工位3相机1选择再次拍照", LogName.runLog);
                        goto CheckStation3Camera0;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3相机1拍照结果，NG处理", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {
                Thread.Sleep(500);
            }
            _station3AxisStatus = Station3AxisStatus.TakePhoto2;
        }
        private void Station3TakePhoto2()
        {
        CheckStation3Light1Begun:
            GlobalVar.Instance.UpdateRunMessage("工位3光源1到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3Light1Begun, (ushort)IOListOutput.Station3Light1Arrived);   //工位2光源1气缸退回
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3Light1Begun, true, "工位3光源1到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源1到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源1到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源1到原位", LogName.runLog);
                        goto CheckStation3Light1Begun;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源1未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {
                Thread.Sleep(500);
            }
        CheckStation3Light2Begun:
            GlobalVar.Instance.UpdateRunMessage("工位3光源2到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3Light2Begun, (ushort)IOListOutput.Station3Light2Arrived);   //工位3光源2气缸退回
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3Light2Begun, true, "工位3光源2到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源2到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源2到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源2到原位", LogName.runLog);
                        goto CheckStation3Light2Begun;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源2未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {
                Thread.Sleep(500);
            }
        CheckStation3Light4Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位3光源4气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Light4Arrived, (ushort)IOListOutput.Station3Light4Begun);   //工位3光源2气缸伸出
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3Light4Arrived, true, "工位3光源4气缸原位", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源4气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源4气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源4气缸到原位", LogName.runLog);
                        goto CheckStation3Light4Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源4未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {
                Thread.Sleep(500);
            }
        CheckStation3Camera0:
            GlobalVar.Instance.UpdateRunMessage("工位3相机2开始拍照", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                GlobalVar.Station3CameraResult[1] = false; //清除拍照完成信号
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Light1ON); //打开光源
                GlobalVar.Instance.UpdateRunMessage("工位3开始6号孔位拍照", LogName.runLog);
                KeyenceVision2.instance().sendData("CC,33");  //发送基恩士拍照
                GlobalVar.Instance.UpdateRunMessage("等待工位3相机2拍照结果", LogName.runLog);
                if (WaitCamera(GlobalVar.Station3CameraResult[1], GlobalVar.CylinderAlarmTime))   //等待拍照完成
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3相机2拍照结果超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "CCD3拍照异常，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("工位3相机2选择再次拍照", LogName.runLog);
                        goto CheckStation3Camera0;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3相机2拍照结果，NG处理", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {
                Thread.Sleep(500);
            }

        CheckStation3Light5Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位3光源5气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Light5Arrived, (ushort)IOListOutput.Station3Light5Begun);   //工位3光源2气缸伸出
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3Light5Arrived, true, "工位3光源5气缸原位", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源5气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源5气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源5气缸到原位", LogName.runLog);
                        goto CheckStation3Light5Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源5未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {
                Thread.Sleep(500);
            }
        CheckStation3Light3Arrived:
            GlobalVar.Instance.UpdateRunMessage("工位3光源3气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Light3Arrived, (ushort)IOListOutput.Station3Light3Begun);   //工位3光源2气缸伸出
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3Light3Arrived, true, "工位3光源3气缸原位", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3光源3气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3光源3气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3光源3气缸到原位", LogName.runLog);
                        goto CheckStation3Light3Arrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3光源3未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {
                Thread.Sleep(500);
            }
            _station3AxisStatus = Station3AxisStatus.AutoScrew1;
        }
        private void Station3AutoScrew1()
        {
        CheckStation3LeftScrewBoxReady:
            GlobalVar.Instance.UpdateRunMessage("等待工位3左螺丝盒OK", LogName.runLog);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3LeftScrewBoxReady, true, "工位3左螺丝盒没有准备好", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位3左螺丝盒OK超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3左螺丝盒没有准备好，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3左螺丝盒OK", LogName.runLog);
                        goto CheckStation3LeftScrewBoxReady;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略螺丝盒未准备完成", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {
                Thread.Sleep(500);
            }
            //double[] Dis = { 0, 0 };
            GlobalVar.Instance.UpdateRunMessage("工位3组合轴移动到左螺丝盒上方", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[1], 10, 1, Dis,true);  //移动到取螺丝位上方10mm处
            GlobalVar.Instance.UpdateRunMessage("工位3组合轴移动到左螺丝盒位", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[1], 0, 1, Dis,false);  //移动到取螺丝位
        CheckStation3EleScrewReady:
            GlobalVar.Instance.UpdateRunMessage("等待电批2准备好", LogName.runLog);
        if (GlobalVar.Production)  //生产模式判断感应器是否到位
        {
            if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3EleScrewReady, true, "电批2没有准备好", GlobalVar.CylinderAlarmTime))
            {
                #region
                GlobalVar.Instance.UpdateRunMessage("等待电批2准备好超时", LogName.runLog);
                Notice FeedBlockUpArrived = new Notice();
                FeedBlockUpArrived.SetNotice = "电批3没有准备好，按“OK”继续检查，按“Cancel”继续工作";
                if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                {
                    GlobalVar.Instance.UpdateRunMessage("选择继续等待电批2准备好", LogName.runLog);
                    goto CheckStation3EleScrewReady;
                }
                else
                {
                    GlobalVar.Instance.UpdateRunMessage("选择忽略电批2未准备好", LogName.runLog);
                }
                #endregion
            }
        }
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD0); //电批预设
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD1);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD2);

            GlobalVar.Instance.UpdateRunMessage("打开电批2真空吸", LogName.runLog);


        CheckStation3ScrewCameraDown:
            GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到工作位", LogName.runLog);
            ClassMotion.CylinderComm((ushort)IOListOutput.Station3ScrewCameraUP, (ushort)IOListOutput.Station3ScrewCameraDown);   //工位2光源1气缸退回
            Thread.Sleep(500);
            {
                if (WaitCommIN((ushort)IOListInput.Station3ScrewCameraDown, true, GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3螺丝臂摄像头到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3螺丝臂摄像头到工作位", LogName.runLog);
                        goto CheckStation3ScrewCameraDown;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3螺丝臂摄像头未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {
                Thread.Sleep(500);
            }


            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3ScrewVacuum);  //打开真空
            }
            else
            {
                Thread.Sleep(500);
            }
            GlobalVar.Instance.UpdateRunMessage("工站3组合轴左取螺丝位抬起", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[1], 10, 1, Dis,true);  //移动到取螺丝位上方10mm处   分左右
            GlobalVar.Instance.UpdateRunMessage("工站3组合轴移动到第一个螺丝孔位上方", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[2], 10, 1, GlobalVar.Station3Screw5Location,true);  //移动到第一个安装位上方10mm处  添加视觉补偿
            GlobalVar.Instance.UpdateRunMessage("工站3组合轴移动到第一个螺丝孔位", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[2], 0, 1, GlobalVar.Station3Screw5Location, false);  //移动到第一个安装位  添加
            
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD0);  //电批预设
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD1);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD2);

        CheckStation3ScrewCameraUP:
            GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到原位", LogName.runLog);
            ClassMotion.CylinderComm((ushort)IOListOutput.Station3ScrewCameraDown, (ushort)IOListOutput.Station3ScrewCameraUP);   //工位2光源1气缸退回
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            Thread.Sleep(500);
            {
                if (WaitCommIN((ushort)IOListInput.Station3ScrewCameraUP, true, GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3螺丝臂摄像头到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3螺丝臂摄像头到原位", LogName.runLog);
                        goto CheckStation3ScrewCameraUP;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3螺丝臂摄像头未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //else
            {
                Thread.Sleep(500);
            }

            GlobalVar.Instance.UpdateRunMessage("启动电批2", LogName.runLog);
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3EleScrewStart);  //启动电批
            DateTime starttime1 = DateTime.Now;
            while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3EleScrewRun) == 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime1;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批2启动", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    #endregion
                    starttime1 = DateTime.Now;
                }
            }
            DateTime starttime = DateTime.Now;
            while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3EleScrewDone) == 1 && ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3EleScrewError) == 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批2完成", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    #endregion
                    starttime = DateTime.Now;
                }
            }
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            //{
               
            //CheckStation3EleScrewDone:
            //    GlobalVar.Instance.UpdateRunMessage("等待电批2完成信号", LogName.runLog);
            //    if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3EleScrewDone, true, "电批2没有完成信号", GlobalVar.CylinderAlarmTime))
            //    {
            //        #region
            //        GlobalVar.Instance.UpdateRunMessage("等待电批2完成信号超时", LogName.runLog);
            //        Notice FeedBlockUpArrived = new Notice();
            //        FeedBlockUpArrived.SetNotice = "电批2没有完成信号，按“OK”继续检查，按“Cancel”继续工作";
            //        if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
            //        {
            //            GlobalVar.Instance.UpdateRunMessage("选择继续等待电批2完成信号", LogName.runLog);
            //            goto CheckStation3EleScrewDone;
            //        }
            //        else
            //        {
            //            GlobalVar.Instance.UpdateRunMessage("选择忽略电批2未完成", LogName.runLog);
            //        }
            //        #endregion
            //    }
               
            //}
            //else
            //{
            //    Thread.Sleep(500);
            //}
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3EleScrewStart);  //关闭电批
            GlobalVar.Instance.UpdateRunMessage("关闭电批2真空吸", LogName.runLog);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station2ScrewVacuum); //断真空
            GlobalVar.Instance.UpdateRunMessage("打开电批2破真空吹气", LogName.runLog);
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Blow); //破真空
            Thread.Sleep(1000);
            GlobalVar.Instance.UpdateRunMessage("关闭破真空吹气", LogName.runLog);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Blow); //关闭破真空
            GlobalVar.Instance.UpdateRunMessage("工站3组合轴移动到第一个螺丝孔位上方", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[2], 10, 1, GlobalVar.Station3Screw5Location, true);  //移动到第一个安装位上方10mm处
            _station3AxisStatus = Station3AxisStatus.AutoScrew2;
        }
        private void Station3AutoScrew2()
        {
            GlobalVar.Instance.UpdateRunMessage("工站3组合轴移动到右螺丝盒上方", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[3], 10, 1, Dis,true);  //移动到取螺丝位上方10mm处
            GlobalVar.Instance.UpdateRunMessage("工站3组合轴移动到右螺丝盒位", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[3], 0, 1, Dis,false);  //移动到取螺丝位上方    分左右
        CheckStation3EleScrewReady1:
            GlobalVar.Instance.UpdateRunMessage("等待电批2准备好", LogName.runLog);


        CheckStation3ScrewCameraDown:
            GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到工作位", LogName.runLog);
            ClassMotion.CylinderComm((ushort)IOListOutput.Station3ScrewCameraUP, (ushort)IOListOutput.Station3ScrewCameraDown);   //工位2光源1气缸退回
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitCommIN((ushort)IOListInput.Station3ScrewCameraDown, true, GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3螺丝臂摄像头未工作位，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3螺丝臂摄像头到工作位", LogName.runLog);
                        goto CheckStation3ScrewCameraDown;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3螺丝臂摄像头未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {
                Thread.Sleep(500);
            }

            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD0);  //电批预设
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD1);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD2);
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3EleScrewReady, true, "电批2没有准备好", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("电批2准备超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "电批2没有准备好，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待电批2准备好", LogName.runLog);
                        goto CheckStation3EleScrewReady1;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略电批2未准备好", LogName.runLog);
                    }
                    #endregion
                }
                GlobalVar.Instance.UpdateRunMessage("打开电批2真空吸", LogName.runLog);
                ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3ScrewVacuum);  //打开真空
            }
            else
            {
                Thread.Sleep(500);
            }
            GlobalVar.Instance.UpdateRunMessage("工站3移动到右取螺丝位上方", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[3], 10, 1, Dis,true);  //移动到取螺丝位上方10mm处   分左右
            GlobalVar.Instance.UpdateRunMessage("工站3移动到第二个螺丝孔位上方", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[4], 10, 1, GlobalVar.Station3Screw6Location,true);  //移动到第二个安装位上方10mm处  添加视觉补偿
            GlobalVar.Instance.UpdateRunMessage("工站3移动到第二个螺丝孔位", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[4], 0, 1, GlobalVar.Station3Screw6Location, false);  //移动到第二个安装位  添加
           
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD0);  //电批预设
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD1);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Sation3EleScrewSelectD2);

        CheckStation3ScrewCameraUP:
            GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到原位", LogName.runLog);
            ClassMotion.CylinderComm((ushort)IOListOutput.Station3ScrewCameraDown, (ushort)IOListOutput.Station3ScrewCameraUP);   //工位2光源1气缸退回
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitCommIN((ushort)IOListInput.Station3ScrewCameraUP, true, GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3螺丝臂摄像头未原位，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3螺丝臂摄像头到原位", LogName.runLog);
                        goto CheckStation3ScrewCameraUP;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3螺丝臂摄像头未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {
                Thread.Sleep(500);
            }

            GlobalVar.Instance.UpdateRunMessage("启动电批2", LogName.runLog);
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3EleScrewStart);  //启动电批
            DateTime starttime1 = DateTime.Now;
            while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3EleScrewRun) == 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime1;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批2启动", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    #endregion
                    starttime1 = DateTime.Now;
                }
            }
            DateTime starttime = DateTime.Now;
            while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3EleScrewDone) == 1 && ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station3EleScrewError) == 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待电批2完成", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    #endregion
                    starttime = DateTime.Now;
                }
            }
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3EleScrewStart);  //关闭电批
            GlobalVar.Instance.UpdateRunMessage("关闭电批2真空吸", LogName.runLog);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3ScrewVacuum); //关掉真空吸
            GlobalVar.Instance.UpdateRunMessage("打开电批2破真空", LogName.runLog);
            ClassMotion.SetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Blow); //破真空
            Thread.Sleep(100);
            GlobalVar.Instance.UpdateRunMessage("关闭电批2破真空", LogName.runLog);
            ClassMotion.ResetOutPut(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station3Blow); //关闭破真空
            GlobalVar.Instance.UpdateRunMessage("工站3组合轴移动到第二个螺丝孔上方", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[4], 10, 1, Dis,true);  //移动到第二个放螺丝上方10mm处   分左右
            GlobalVar.Instance.UpdateRunMessage("工站3组合轴移动到第等待位上方", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[0], 10, 1, Dis,true);  //移动到等待位上方10mm处   分左右
            GlobalVar.Instance.UpdateRunMessage("工站3组合轴移动到第等待位", LogName.runLog);
            Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[0], 0, 1, Dis,false);  //移动到等待位上方10mm处   
            _station3AxisStatus = Station3AxisStatus.CarrierReset;
        }
        private void Station3CarrierReset()
        {
        CheckStation3LeanBackArrived:
            GlobalVar.Instance.UpdateRunMessage("工站3倾斜气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3LeanBackArrived, (ushort)IOListOutput.Station3LeanBackBegun);   //工站3倾斜气缸回退
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3LeanBackArrived, true, "工站3倾斜气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工站3倾斜气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工站3倾斜气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工站3倾斜气缸到工作位", LogName.runLog);
                        goto CheckStation3LeanBackArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工站3倾斜气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //  else
            {

            }
            Thread.Sleep(1000);
        CheckStation3LeanBackBegun:
            GlobalVar.Instance.UpdateRunMessage("工站3倾斜气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3LeanBackBegun, (ushort)IOListOutput.Station3LeanBackArrived);   //工站3倾斜气缸回退
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3LeanBackBegun, true, "工站3倾斜气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工站3倾斜气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工站3倾斜气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工站3倾斜气缸到原位", LogName.runLog);
                        goto CheckStation3LeanBackBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工站3倾斜气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
        CheckStation3PressBegun:
            GlobalVar.Instance.UpdateRunMessage("工站3压料气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3PressBegun, (ushort)IOListOutput.Station3PressArrived);   //工位3压料气缸松开
          //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3PressBegun, true, "工站3压料气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工站3压料气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工站3压料气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工站3压料气缸到原位", LogName.runLog);
                        goto CheckStation3PressBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工站3压料气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
           // else
            {

            }
        CheckStation3JackUpDown:
            GlobalVar.Instance.UpdateRunMessage("工站3顶升气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3JackUpDown, (ushort)IOListOutput.Station3JackUpUp);   //工位1顶升气缸下位
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3JackUpDown, true, "工站3顶升气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工站3顶升气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工站3顶升气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工站3顶升气缸到原位", LogName.runLog);
                        goto CheckStation3JackUpDown;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工站3顶升气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //else
            {

            }
            _station3AxisStatus = Station3AxisStatus.DisCharge;
        }
        public void Station3Discharge()
        {
            DateTime starttime = DateTime.Now;
            GlobalVar.Instance.UpdateRunMessage("等待出料", LogName.runLog);
            GlobalVar.Station3DischargeLabel = true;
           
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.DiachargeFrontRay) == 1)      //利用对射光纤判断皮带上是否有物料，并判断顶升气缸是否在始位
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                    DateTime endtime = DateTime.Now;
                    TimeSpan spantime = endtime - starttime;
                    if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                    {
                        #region
                        Notice FeedBlockUpArrived = new Notice();
                        FeedBlockUpArrived.SetNotice = "工位3对射光纤有信号或者工位3顶升气缸不在始位，不允许向工位3出料，按“OK”继续检查，按“Cancel”继续工作";
                        if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                        {

                        }
                        else
                        {
                            break;
                        }
                        #endregion
                    }
                }
            }
            else
            {

            }
            DateTime starttime1 = DateTime.Now;
            while (!GlobalVar.StationDisEnableStatuon3)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime1;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位3向下料工位出料完成", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    starttime1 = DateTime.Now;
                    #endregion
                }
            }
            GlobalVar.StationDisEnableStatuon3 = false;
            _station3AxisStatus = Station3AxisStatus.Feeding;
        }
        #endregion
        #region 工位4主线程
        private void Station4Feeding()
        {
        CheckFeedBlockUpArrived:
            GlobalVar.Instance.UpdateRunMessage("工位4阻挡气缸到工作位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.DisChargeBlockUP, (ushort)IOListOutput.DisChargeBlockDown);   //工位4阻挡气缸到工作位
           // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.DisChargeBlockUP, true, "工位4阻挡气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位4阻挡气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位4阻挡气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位4阻挡气缸到工作位", LogName.runLog);
                        goto CheckFeedBlockUpArrived;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位4阻挡气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
          //  else
            {

            }
            DateTime starttime = DateTime.Now;
            while (!GlobalVar.Station3DischargeLabel)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime;
                if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("等待工位3做料完成向工位4出料", LogName.runLog);
                    //Notice FeedBlockUpArrived = new Notice();
                    //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
                    //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}
                    starttime = DateTime.Now;
                    #endregion
                }
            }
            GlobalVar.Station3DischargeLabel = false;
        CheckStation2BlockDown:
            GlobalVar.Instance.UpdateRunMessage("工位3阻挡气缸到原位", LogName.runLog);

            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3BlockDown, (ushort)IOListOutput.Station3BlockUp);   //工位2阻挡气缸下位
               // if (GlobalVar.Production)  //生产模式判断感应器是否到位
                {
                    if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3BlockDown, true, "工位3阻挡气缸到原位超时", GlobalVar.CylinderAlarmTime))
                    {
                        #region
                        GlobalVar.Instance.UpdateRunMessage("工位3阻挡气缸到原位超时", LogName.runLog);
                        Notice FeedBlockUpArrived = new Notice();
                        FeedBlockUpArrived.SetNotice = "工位3阻挡气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                        if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                        {
                            GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3阻挡气缸到原位", LogName.runLog);
                            goto CheckStation2BlockDown;   //检查顶升气缸始位信号
                        }
                        else
                        {
                            GlobalVar.Instance.UpdateRunMessage("选择忽略工位3阻挡气缸未到原位", LogName.runLog);
                        }
                        #endregion
                    }
                }
               // else
                {

                }
                GlobalVar.StationDisEnableStatuon3 = true;
            DateTime starttime1 = DateTime.Now;
           
            //while (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[2], (ushort)IOListInput.DiachargeFrontRay) == 1)
            //{
            //    Application.DoEvents();
            //    Thread.Sleep(10);
            //    DateTime endtime = DateTime.Now;
            //    TimeSpan spantime = endtime - starttime1;
            //    if (spantime.TotalSeconds > GlobalVar.CylinderAlarmTime)
            //    {
            //        #region
            //        GlobalVar.Instance.UpdateRunMessage("等待载具到下料位", LogName.runLog);
            //        //Notice FeedBlockUpArrived = new Notice();
            //        //FeedBlockUpArrived.SetNotice = "等待工位1做料完成向工位2出料";
            //        //if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
            //        //{

            //        //}
            //        //else
            //        //{
            //        //    break;
            //        //}
            //        starttime1 = DateTime.Now;
            //        #endregion
            //    }
            //}
        CheckStation3BlockUp:
            GlobalVar.Instance.UpdateRunMessage("工位3阻挡气缸到工作位", LogName.runLog);

            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3BlockUp, (ushort)IOListOutput.Station3BlockDown);   //工位2阻挡气缸下位
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3BlockUp, true, "工位3阻挡气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3阻挡气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3阻挡气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3阻挡气缸到工作位", LogName.runLog);
                        goto CheckStation3BlockUp;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3阻挡气缸未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
        CheckDisChargeJackUpUp:
            GlobalVar.Instance.UpdateRunMessage("下料顶升气缸到工作位", LogName.runLog);

            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.DisChargeJackUpUp, (ushort)IOListOutput.DisChargeJackUpDown);   //工位2阻挡气缸下位
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.DisChargeJackUpUp, true, "下料顶升气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("下料顶升气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "下料顶升气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待下料顶升气缸到工作位", LogName.runLog);
                        goto CheckDisChargeJackUpUp;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略下料顶升未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
            _station4AxisStatus = Station4AxisStatus.DisCharge;
        }
        private void Station4DiachargeNGOK()
        {
            Station4LineMove(2, GlobalVar.SingleAxis, GlobalVar.LayingOffX[0], GlobalVar.LayingOffZ[0], 1, false);  //下料轴到等待位
            Station4LineMove(2, GlobalVar.SingleAxis, GlobalVar.LayingOffX[1], GlobalVar.LayingOffZ[1], 1, true);  //下料轴到载具位

        CheckDisChargeClampBegun:
            GlobalVar.Instance.UpdateRunMessage("下料夹爪气缸到工作位", LogName.runLog);

            ClassMotion.CylinderComm((ushort)IOListOutput.DisChargeClampArrived, (ushort)IOListOutput.DisChargeClampBegun);   //工位2光源1气缸退回
            Thread.Sleep(500);
            {
                if (WaitCommIN((ushort)IOListInput.DisChargeClampArrived, true, GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3螺丝臂摄像头到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3螺丝臂摄像头到工作位", LogName.runLog);
                        goto CheckDisChargeClampBegun;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3螺丝臂摄像头未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            if (true)
            {
                Station4LineMove(2, GlobalVar.SingleAxis, GlobalVar.LayingOffX[2], GlobalVar.LayingOffZ[2], 1, true);  //下料轴到OK位
                Station4LineMove(2, GlobalVar.SingleAxis, GlobalVar.LayingOffX[2], GlobalVar.LayingOffZ[2], 1, false);  //下料轴到OK位
            }
            else
            {
                Station4LineMove(2, GlobalVar.SingleAxis, GlobalVar.LayingOffX[3], GlobalVar.LayingOffZ[3], 1, true);  //下料轴到NG位
            }
        CheckDisChargeJackUpUp:
            GlobalVar.Instance.UpdateRunMessage("下料顶升气缸到工作位", LogName.runLog);

            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.DisChargeJackUpDown, (ushort)IOListOutput.DisChargeJackUpUp);   //工位2阻挡气缸下位
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.DisChargeJackUpDown, true, "下料顶升气缸到工作位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("下料顶升气缸到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "下料顶升气缸到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待下料顶升气缸到工作位", LogName.runLog);
                        goto CheckDisChargeJackUpUp;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略下料顶升未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
        CheckDisChargeClampArrived:
            GlobalVar.Instance.UpdateRunMessage("下料夹爪气缸到原位", LogName.runLog);

            ClassMotion.CylinderComm((ushort)IOListOutput.DisChargeClampBegun, (ushort)IOListOutput.DisChargeClampArrived);   //工位2光源1气缸退回
            Thread.Sleep(500);
            {
                if (WaitCommIN((ushort)IOListInput.DisChargeClampBegun, true, GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位3螺丝臂摄像头到工作位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位3螺丝臂摄像头到工作位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位3螺丝臂摄像头到工作位", LogName.runLog);
                        goto CheckDisChargeClampArrived;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位3螺丝臂摄像头未到工作位", LogName.runLog);
                    }
                    #endregion
                }
            }
            Station4LineMove(2, GlobalVar.SingleAxis, GlobalVar.LayingOffX[0], GlobalVar.LayingOffZ[0], 1, true);  //下料轴到等待位
            Station4LineMove(2, GlobalVar.SingleAxis, GlobalVar.LayingOffX[0], GlobalVar.LayingOffZ[0], 1, false); 
            _station4AxisStatus = Station4AxisStatus.Feeding;
        }
        #endregion

        #region 工位1线程
        public void startStation1Work()
        {
            if (Station1Work.IsBusy != true)
            {
                Station1Work.RunWorkerAsync();
            }
        }
        public void stopStation1Work()
        {
            if (Station1Work.IsBusy == true)
            {
                Station1Work.CancelAsync();
            }
        }
        private void Station1Work_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                if (Station1Work.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (GlobalVar.bStartRunning)
                {
                    switch (_station1AxisStatus)
                    {
                        case Station1AxisStatus.None:
                            break;
                        case Station1AxisStatus.Feeding:
                            Station1Feeding();     //进料
                            break;
                        case Station1AxisStatus.TakePhoto1:
                            Station1TaKePhoto1();   //第一次拍照    还不确定拍几次
                            break;
                        case Station1AxisStatus.TakePhoto2:
                            Station1TaKePhoto2();    //第2次拍照    还不确定拍几次
                            break;
                        case Station1AxisStatus.Lean:
                            Station1Lean();      //载具倾斜
                            break;
                        case Station1AxisStatus.TakePhoto3:
                            Station1TaKePhoto3();     //第3次拍照    还不确定拍几次
                            break;
                        case Station1AxisStatus.TakePhoto4:
                            Station1TaKePhoto4();     //第4次拍照    还不确定拍几次
                            break;
                        case Station1AxisStatus.CarrierReset:
                            Station1CarrierReset();    //载具倾斜复位
                            break;
                        case Station1AxisStatus.DisCharge:
                            Station1Discharge();      //工位1出料
                            break;
                    }
                }
            }
        }
        #endregion

        #region 工位2线程
        public void startStation2Work()
        {
            if (Station2Work.IsBusy != true)
            {
                Station2Work.RunWorkerAsync();
            }
        }
        public void stopStation2Work()
        {
            if (Station2Work.IsBusy == true)
            {
                Station2Work.CancelAsync();
            }
        }
        private void Station2Work_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                if (Station2Work.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (GlobalVar.bStartRunning)
                {
                    switch (_station2AxisStatus)
                    {
                        case Station2AxisStatus.None:

                            break;
                        case Station2AxisStatus.Feeding:
                            Station2Feeding();
                            break;
                        case Station2AxisStatus.TakePhoto:
                            Station2TakePtoto();
                            break;
                        case Station2AxisStatus.AutoScrew1:
                            Station2AotoScrew1();
                            break;
                        case Station2AxisStatus.AutoScrew2:
                            Station2AotoScrew2();
                            break;
                        case Station2AxisStatus.Lean:
                            Station2Lean();
                            break;
                        case Station2AxisStatus.CarrierReset:
                            Station2CarrierReset();
                            break;
                        case Station2AxisStatus.DisCharge:
                            Station2Discharge();
                            break;
                    }
                }
            }
        }
        #endregion


        #region 工位3线程
        public void startStation3Work()
        {
            if (Station3Work.IsBusy != true)
            {
                Station3Work.RunWorkerAsync();
            }
        }
        public void stopStation3Work()
        {
            if (Station3Work.IsBusy == true)
            {
                Station3Work.CancelAsync();
            }
        }
        private void Station3Work_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                if (Station3Work.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (GlobalVar.bStartRunning)
                {
                    switch (_station3AxisStatus)
                    {
                        case Station3AxisStatus.None:

                            break;
                        case Station3AxisStatus.Feeding:
                            Station3Feeding();
                            break;
                        case Station3AxisStatus.TakePhoto1:
                            Station3TakePhoto1();
                            break;
                        case Station3AxisStatus.TakePhoto2:
                            Station3TakePhoto2();
                            break;
                        case Station3AxisStatus.AutoScrew1:
                            Station3AutoScrew1();
                            break;
                        case Station3AxisStatus.AutoScrew2:
                            Station3AutoScrew2();
                            break;
                        case Station3AxisStatus.CarrierReset:
                            Station3CarrierReset();
                            break;
                        case Station3AxisStatus.DisCharge:
                            Station3Discharge();
                            break;
                    }
                }
            }
        }
        #endregion
        #region 工位4线程
        public void startStation4Work()
        {
            if (Station4Worker.IsBusy != true)
            {
                Station4Worker.RunWorkerAsync();
            }
        }
        public void stopStation4Work()
        {
            if (Station4Worker.IsBusy == true)
            {
                Station4Worker.CancelAsync();
            }
        }
        private void Station4Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                if (Station4Worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (GlobalVar.bStartRunning)
                {
                    switch (_station4AxisStatus)
                    {
                        case Station4AxisStatus.None:

                            break;
                        case Station4AxisStatus.Feeding:
                            Station4Feeding();
                            break;
                        case Station4AxisStatus.DisCharge:
                            Station4DiachargeNGOK();
                            break;
                    }
                }
            }
        }
        #endregion
        #region 安全门线程
        public void startbackgroundSafeDoor()
        {
            if (backgroundSafeDoor.IsBusy != true)
            {
                backgroundSafeDoor.RunWorkerAsync();
            }
        }
        public void stopbackgroundSafeDoor()
        {
            if (backgroundSafeDoor.IsBusy == true)
            {
                backgroundSafeDoor.CancelAsync();
            }
        }
        private void backgroundsafeDoor_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> DoorAlarm = new List<string>();
            while (true)
            {
                DoorAlarm.Clear();
                Application.DoEvents();
                Thread.Sleep(5000);
                if (backgroundSafeDoor.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                for (int i = 0; i < 8; i++)
                {
                    if (ClassMotion.ReadInputBit(GlobalVar.IOCardNumber[0], (ushort)(i + 8)) == 0)
                    {
                        switch (i)
                        {
                            case 0:
                                DoorAlarm.Add("前门1");
                                break;
                            case 1:
                                DoorAlarm.Add("前门2");
                                break;
                            case 2:
                                DoorAlarm.Add("右门1");
                                break;
                            case 3:
                                DoorAlarm.Add("右门2");
                                break;
                            case 4:
                                DoorAlarm.Add("后门1");
                                break;
                            case 5:
                                DoorAlarm.Add("后门2");
                                break;
                            case 6:
                                DoorAlarm.Add("左门1");
                                break;
                            case 7:
                                DoorAlarm.Add("左门2");
                                break;
                        }
                        //MessageBox.Show(DoorAlarm[i]+"安全门被打开");                       
                    }
                }
                if (DoorAlarm.Count() != 0)
                {
                    string Dooralarm = "";
                    for (int i = 0; i < DoorAlarm.Count(); i++)
                    {
                        Dooralarm += DoorAlarm[i] + "  ";
                    }
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = Dooralarm + "  安全门被打开";
                    FeedBlockUpArrived.ShowDialog();
                }
            }
        }
        #endregion

        #region 等待信号
        private void WaitInport(ushort cartio, ushort port, int status, string warning)
        {
            DateTime starttime = DateTime.Now;

            while (ClassMotion.ReadInputBit(cartio, port) == status)
            {
                Thread.Sleep(50);
                TimeSpan spantime = DateTime.Now - starttime;
                if (spantime.TotalSeconds > 10)
                {
                    if (MessageBox.Show(warning + "，Retray继续检查，Cancle取消本次检查", "通知", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    {

                        return;
                    }
                    else
                    {
                        starttime = DateTime.Now;
                    }

                }
            }
        }
        #endregion

        #region 复位动作
        private void Station1CarrierReset1()
        {

        CheckStation1UnlockForwardFront:
            GlobalVar.Instance.UpdateRunMessage("工位1解锁前推气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1UnlockForwardBack, (ushort)IOListOutput.Station1UnlockForwardFront);   //解锁前推
                                                                                                                                                                // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1UnlockForwardBack, true, "工位1解锁前推气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1解锁前推气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1解锁前推气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1解锁前推气缸到原位", LogName.runLog);
                        goto CheckStation1UnlockForwardFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1解锁前推气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {
                Thread.Sleep(1000);
            }
        CheckStation1UnlockClampFront:
            GlobalVar.Instance.UpdateRunMessage("工位1解锁夹紧气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1UnlockClampBack, (ushort)IOListOutput.Station1UnlockClampFront);   //解锁夹紧
                                                                                                                                                            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1UnlockClampBack, true, "工位1解锁夹紧气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1解锁夹紧气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1解锁夹紧气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1解锁夹紧气缸到原位", LogName.runLog);
                        goto CheckStation1UnlockClampFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1解锁夹紧气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //  else
            {

            }
        CheckStation1UnlockLeanFront:
            GlobalVar.Instance.UpdateRunMessage("工位1解锁倾斜气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1UnlockLeanFront, (ushort)IOListOutput.Station1UnlockLeanBack);   //解锁倾斜
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1UnlockLeanFront, true, "工位1解锁倾斜气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1解锁倾斜气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1解锁倾斜气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1解锁倾斜气缸到原位", LogName.runLog);
                        goto CheckStation1UnlockLeanFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1解锁倾斜气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
        CheckStation1Light2Begun:
            GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸2到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[2], (ushort)IOListOutput.Station1Light2Arrived, (ushort)IOListOutput.Station1Light2Begun);   //工位1压料气缸压紧
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[2], (ushort)IOListInput.Station1Light2Begun, true, "工位1光源1气缸2到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸2到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1光源1气缸2到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1光源1气缸2到原位", LogName.runLog);
                        goto CheckStation1Light2Begun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1光源1气缸2未到原位位", LogName.runLog);
                    }
                    #endregion
                }
                else
                {

                }
            }
        CheckStation1Light1Begun:
            GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸1到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station1Light1Arrived, (ushort)IOListOutput.Station1Light1Begun);   //工位1压料气缸压紧
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station1Light1Arrived, true, "工位1光源1气缸1到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1光源1气缸1到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1光源1气缸1到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1光源1气缸1到原位", LogName.runLog);
                        goto CheckStation1Light1Begun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1光源1气缸1未到原位", LogName.runLog);
                    }
                    #endregion
                }
                else
                {

                }
            }

        CheckStation1LeanBackBegun:
            GlobalVar.Instance.UpdateRunMessage("工位1倾斜气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1LeanBackBegun, (ushort)IOListOutput.Station1LeanBackArrived);   //倾斜气缸回退
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1LeanBackBegun, true, "工位1倾斜气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1倾斜气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1倾斜气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("继续等待工位1倾斜气缸到原位", LogName.runLog);
                        goto CheckStation1LeanBackBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1倾斜气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }

        CheckStation1PressBegun:
            GlobalVar.Instance.UpdateRunMessage("工位1压料气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1PressBegun, (ushort)IOListOutput.Station1PressArrived);   //工位1压料气缸松开
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1PressBegun, true, "工位1压料气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1压料气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1压料气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1压料气缸到原位", LogName.runLog);
                        goto CheckStation1PressBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1压料气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
        CheckStation1JackUpDown:
            GlobalVar.Instance.UpdateRunMessage("工位1顶升气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[0], (ushort)IOListOutput.Station1JackUpDown, (ushort)IOListOutput.Station1JackUpUp);   //工位1顶升气缸下位
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[0], (ushort)IOListInput.Station1JackUpDown, true, "工位1顶升气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位1顶升气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位1顶升气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位1顶升气缸到原位", LogName.runLog);
                        goto CheckStation1JackUpDown;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位1顶升气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //  else
            {

            }
        }
        private void Station2CarrierReset1()
        {

        CheckStation2PressBegun:
            GlobalVar.Instance.UpdateRunMessage("工位2压料气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2PressBegun, (ushort)IOListOutput.Station2PressArrived);   //工位2压料气缸松开
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2PressBegun, true, "工位2压料气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2压料气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2压料气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2压料气缸到原位", LogName.runLog);
                        goto CheckStation2PressBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2压料气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {

            }
        CheckStation2JackUpDown:
            GlobalVar.Instance.UpdateRunMessage("工位2顶升气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2JackUpDown, (ushort)IOListOutput.Station2JackUpUp);   //工位2顶升气缸下位
            if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2JackUpDown, true, "工位2顶升气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2顶升气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2顶升气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2顶升气缸到原位", LogName.runLog);
                        goto CheckStation2JackUpDown;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2顶升气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            else
            {

            }
       
        CheckStation2UnlockClampFront:
            GlobalVar.Instance.UpdateRunMessage("工位2解锁夹紧气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2UnlockClampFront, (ushort)IOListOutput.Station2UnlockClampBack);   //工站2解锁夹紧
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2UnlockClampFront, true, "工位2解锁夹紧气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2解锁夹紧气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2解锁夹紧气缸到工原超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2解锁夹紧气缸到原位", LogName.runLog);
                        goto CheckStation2UnlockClampFront;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2解锁夹紧气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //  else
            {

            }
        CheckStation2UnlockForwardFront:
            GlobalVar.Instance.UpdateRunMessage("工位2解锁前推气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2UnlockForwardBack, (ushort)IOListOutput.Station2UnlockForwardFront);   //解锁前推
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2UnlockForwardBack, true, "工位2解锁前推气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2解锁前推气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2解锁前推气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2解锁前推气缸到原位", LogName.runLog);
                        goto CheckStation2UnlockForwardFront;
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2解锁前推气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
        CheckStation2LeanFront:
            GlobalVar.Instance.UpdateRunMessage("工位2倾斜气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station2LeanFront, (ushort)IOListOutput.Station2LeanBack);   //工位2倾斜
                                                                                                                                              //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station2LeanFront, true, "工位2倾斜气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工位2倾斜气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工位2倾斜气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工位2倾斜气缸到原位", LogName.runLog);
                        goto CheckStation2LeanFront;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工位2倾斜气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //  else
            {

            }
        }
        private void Station3CarrierReset1()
        {

        CheckStation3LeanBackBegun:
            GlobalVar.Instance.UpdateRunMessage("工站3倾斜气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3LeanBackBegun, (ushort)IOListOutput.Station3LeanBackArrived);   //工站3倾斜气缸回退
            //if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3LeanBackBegun, true, "工站3倾斜气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工站3倾斜气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工站3倾斜气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工站3倾斜气缸到原位", LogName.runLog);
                        goto CheckStation3LeanBackBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工站3倾斜气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //  else
            {

            }
        CheckStation3PressBegun:
            GlobalVar.Instance.UpdateRunMessage("工站3压料气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3PressBegun, (ushort)IOListOutput.Station3PressArrived);   //工位3压料气缸松开
            //  if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3PressBegun, true, "工站3压料气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工站3压料气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工站3压料气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工站3压料气缸到原位", LogName.runLog);
                        goto CheckStation3PressBegun;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工站3压料气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            // else
            {

            }
        CheckStation3JackUpDown:
            GlobalVar.Instance.UpdateRunMessage("工站3顶升气缸到原位", LogName.runLog);
            ClassMotion.Cylinder(GlobalVar.IOCardNumber[1], (ushort)IOListOutput.Station3JackUpDown, (ushort)IOListOutput.Station3JackUpUp);   //工位1顶升气缸下位
            // if (GlobalVar.Production)  //生产模式判断感应器是否到位
            {
                if (WaitIN(GlobalVar.IOCardNumber[1], (ushort)IOListInput.Station3JackUpDown, true, "工站3顶升气缸到原位超时", GlobalVar.CylinderAlarmTime))
                {
                    #region
                    GlobalVar.Instance.UpdateRunMessage("工站3顶升气缸到原位超时", LogName.runLog);
                    Notice FeedBlockUpArrived = new Notice();
                    FeedBlockUpArrived.SetNotice = "工站3顶升气缸到原位超时，按“OK”继续检查，按“Cancel”继续工作";
                    if (FeedBlockUpArrived.ShowDialog() == DialogResult.OK)
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择继续等待工站3顶升气缸到原位", LogName.runLog);
                        goto CheckStation3JackUpDown;   //检查顶升气缸始位信号
                    }
                    else
                    {
                        GlobalVar.Instance.UpdateRunMessage("选择忽略工站3顶升气缸未到原位", LogName.runLog);
                    }
                    #endregion
                }
            }
            //else
            {

            }
     
        }
        public void ResetPort1()
        {
            //for (int a = 27; a <= 41; a+=2)
            //{
            //    ClassMotion.SetOutPut(1001, (ushort)(a - 11));
            //    ClassMotion.ResetOutPut(1001, (ushort)(a - 10));
            //    //string name = (CheckBox)this.Controls.Find("checkBox8000" + i, true)[0]
            //    //WaitInport(1011, (ushort)(a - 10),1,"");
            //}
            //for (int a = 11; a <= 41; a+=2)
            //{
            //    ClassMotion.SetOutPut(1002, (ushort)(a - 11));
            //    ClassMotion.ResetOutPut(1002, (ushort)(a - 10));
            //
            ClassMotion.StopAxis(GlobalVar.Conveyor[0]);
            ClassMotion.StopAxis(GlobalVar.Conveyor[1]);
            ClassMotion.StopAxis(GlobalVar.Conveyor[2]);
            //Station1AxisHome();
            //Station2AxisHome();
            //Station3AxisHome();
            Station1CarrierReset1();
            Station2CarrierReset1();
            Station3CarrierReset1();
        }
        public void Station1AxisHome()
        {
            //Task task = new Task(() =>
            //  {
                  ClassMotion.Home(GlobalVar.Sation1XYZ[2]);
                  GlobalVar.Instance.UpdateRunMessage("工位1-Z轴回原点", LogName.runLog);
                  ClassMotion.HomeWaitStop(GlobalVar.Sation1XYZ[2]);
                  ClassMotion.Home(GlobalVar.Sation1XYZ[1]);
                  GlobalVar.Instance.UpdateRunMessage("工位1-Y轴回原点", LogName.runLog);
                  ClassMotion.HomeWaitStop(GlobalVar.Sation1XYZ[1]);
                  ClassMotion.Home(GlobalVar.Sation1XYZ[0]);
                  GlobalVar.Instance.UpdateRunMessage("工位1-X轴回原点", LogName.runLog);
                  ClassMotion.HomeWaitStop(GlobalVar.Sation1XYZ[0]);
             //});
             //task.Start();      
        }
        public void Station2AxisHome()
        {
            //Task task = new Task(() =>
            //{
                ClassMotion.Home(GlobalVar.Sation2XYZ[2]);
                GlobalVar.Instance.UpdateRunMessage("工位2-Z轴回原点", LogName.runLog);
                ClassMotion.HomeWaitStop(GlobalVar.Sation2XYZ[2]);
                ClassMotion.Home(GlobalVar.Sation2XYZ[1]);
                GlobalVar.Instance.UpdateRunMessage("工位2-Y轴回原点", LogName.runLog);
                ClassMotion.HomeWaitStop(GlobalVar.Sation2XYZ[1]);
                ClassMotion.Home(GlobalVar.Sation2XYZ[0]);
                GlobalVar.Instance.UpdateRunMessage("工位2-X轴回原点", LogName.runLog);
                ClassMotion.HomeWaitStop(GlobalVar.Sation2XYZ[0]);
            //});
            //task.Start();
            
        }
        public void Station3AxisHome()
        {
           
            //Task task = new Task(() =>
            //{
                ClassMotion.Home(GlobalVar.Sation3XYZ[2]);
                GlobalVar.Instance.UpdateRunMessage("工位3-Z轴回原点", LogName.runLog);
                ClassMotion.HomeWaitStop(GlobalVar.Sation3XYZ[2]);
                ClassMotion.Home(GlobalVar.Sation3XYZ[1]);
                GlobalVar.Instance.UpdateRunMessage("工位3-Y轴回原点", LogName.runLog);
                ClassMotion.HomeWaitStop(GlobalVar.Sation3XYZ[1]);
                ClassMotion.Home(GlobalVar.Sation3XYZ[0]);
                GlobalVar.Instance.UpdateRunMessage("工位3-X轴回原点", LogName.runLog);
                ClassMotion.HomeWaitStop(GlobalVar.Sation3XYZ[0]);
            //});
            //task.Start();
        }
        #endregion

        #region 扫码枪
        public void KeyenceReadBarCode(string strdata)
        {
            KeyenceBarCode.instance().sendData(strdata);
        }
        public void View_EarthBarCode(string strdata)
        {
            View_EarthBarcode.instance().sendData(strdata);
        }
        #endregion

        #region PDCA
        public void PDCASendData()
        {
            PDCA.instance().sendData("");
        }
        #endregion

        #region 相机标定
        #region CCD1 标定
        public void startbackgroundCCD1Calibration()
        {
            if (backgroundCCD1Calibration.IsBusy != true)
            {
                backgroundCCD1Calibration.RunWorkerAsync();
            }
        }
        public void stopbackgroundCCD1Calibration()
        {
            if (backgroundCCD1Calibration.IsBusy == true)
            {
                backgroundCCD1Calibration.CancelAsync();
            }
        }
        private void backgroundCCD1Calibration_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(500);
                if (backgroundCCD1Calibration.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (GlobalVar.CCD1Calibration.Count == 0)
                {
                    Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[10], 0, 1, Dis,false);    //标定第一个点
                    KeyenceVision1.instance().sendData("CC,02");   //第一个点发送标定拍照指令
                 
                }
                else
                {
                    switch (GlobalVar.CCD1Calibration[GlobalVar.CCD1Calibration.Count-1])
                    {
                        case "OK":
                            switch (GlobalVar.CCD1Calibration.Count)
                            {
                                case 1:
                                    Dis[0] = 5;
                                    Dis[1] = 0;
                                    Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[10], 0, 1, Dis, false);    //标定第2个点
                                    KeyenceVision1.instance().sendData("CC,02");
                                    
                                    break;
                                case 2:
                                    Dis[0] = 5;
                                    Dis[1] = 5;
                                    Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[10], 0, 1, Dis, false);    //标定第3个点
                                    KeyenceVision1.instance().sendData("CC,02");
                                   
                                    break;
                                case 3:
                                    Dis[0] = 0;
                                    Dis[1] = 5;
                                    Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[10], 0, 1, Dis, false);    //标定第4个点
                                    KeyenceVision1.instance().sendData("CC,02");
                                   
                                    break;
                                case 4:
                                    Dis[0] = -5;
                                    Dis[1] = 5;
                                    Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[10], 0, 1, Dis, false);    //标定第5个点
                                    KeyenceVision1.instance().sendData("CC,02");
                                  
                                    break;
                                case 5:
                                    Dis[0] = -5;
                                    Dis[1] = 0;
                                    Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[10], 0, 1, Dis, false);    //标定第6个点
                                    KeyenceVision1.instance().sendData("CC,02");
                                    
                                    break;
                                case 6:
                                    Dis[0] = -5;
                                    Dis[1] = -5;
                                    Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[10], 0, 1, Dis, false);    //标定第7个点
                                    KeyenceVision1.instance().sendData("CC,02");
                                    
                                    break;
                                case 7:
                                    Dis[0] = 0;
                                    Dis[1] = -5;
                                    Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[10], 0, 1, Dis, false);    //标定第8个点
                                    KeyenceVision1.instance().sendData("CC,02");
                                  
                                    break;
                                case 8:
                                    Dis[0] = 5;
                                    Dis[1] = -5;
                                    Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[10], 0, 1, Dis, false);    //标定第9个点
                                    KeyenceVision1.instance().sendData("CC,02");
                                    
                                    break;
                            }
                            break;
                        case "Finish":
                            
                            break;
                        case "Alarm":

                            break;
                    }
                }
            }
        }
        #endregion
        #region CCD2 标定
        public void startbackgroundCCD2Calibration()
        {
            if (backgroundCCD2Calibration.IsBusy != true)
            {
                backgroundCCD2Calibration.RunWorkerAsync();
            }
        }
        public void stopbackgroundCCD2Calibration()
        {
            if (backgroundCCD2Calibration.IsBusy == true)
            {
                backgroundCCD2Calibration.CancelAsync();
            }
        }
        private void backgroundCCD2Calibration_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(500);
                if (backgroundCCD2Calibration.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (GlobalVar.CCD2Calibration.Count == 0)
                {
                    Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[10], 0, 1, Dis, false);    //标定第一个点
                    KeyenceVision2.instance().sendData("CC,03");   //第一个点发送标定拍照指令

                }
                else
                {
                    switch (GlobalVar.CCD2Calibration[GlobalVar.CCD2Calibration.Count - 1])
                    {
                        case "OK":
                            switch (GlobalVar.CCD2Calibration.Count)
                            {
                                case 1:
                                    Dis[0] = 5;
                                    Dis[1] = 0;
                                    Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[10], 0, 1, Dis, false);    //标定第2个点
                                    KeyenceVision2.instance().sendData("CC,03");

                                    break;
                                case 2:
                                    Dis[0] = 5;
                                    Dis[1] = 5;
                                    Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[10], 0, 1, Dis, false);    //标定第3个点
                                    KeyenceVision2.instance().sendData("CC,03");

                                    break;
                                case 3:
                                    Dis[0] = 0;
                                    Dis[1] = 5;
                                    Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[10], 0, 1, Dis, false);    //标定第4个点
                                    KeyenceVision2.instance().sendData("CC,03");

                                    break;
                                case 4:
                                    Dis[0] = -5;
                                    Dis[1] = 5;
                                    Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[10], 0, 1, Dis, false);    //标定第5个点
                                    KeyenceVision2.instance().sendData("CC,03");

                                    break;
                                case 5:
                                    Dis[0] = -5;
                                    Dis[1] = 0;
                                    Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[10], 0, 1, Dis, false);    //标定第6个点
                                    KeyenceVision2.instance().sendData("CC,03");

                                    break;
                                case 6:
                                    Dis[0] = -5;
                                    Dis[1] = -5;
                                    Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[10], 0, 1, Dis, false);    //标定第7个点
                                    KeyenceVision2.instance().sendData("CC,03");

                                    break;
                                case 7:
                                    Dis[0] = 0;
                                    Dis[1] = -5;
                                    Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[10], 0, 1, Dis, false);    //标定第8个点
                                    KeyenceVision2.instance().sendData("CC,03");

                                    break;
                                case 8:
                                    Dis[0] = 5;
                                    Dis[1] = -5;
                                    Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[10], 0, 1, Dis, false);    //标定第9个点
                                    KeyenceVision2.instance().sendData("CC,03");

                                    break;
                            }
                            break;
                        case "Finish":

                            break;
                        case "Alarm":

                            break;
                    }
                }
            }
        }
        #endregion
        #region CCD3 标定
        public void startbackgroundCCD3Calibration()
        {
            if (backgroundCCD3Calibration.IsBusy != true)
            {
                backgroundCCD3Calibration.RunWorkerAsync();
            }
        }
        public void stopbackgroundCCD3Calibration()
        {
            if (backgroundCCD3Calibration.IsBusy == true)
            {
                backgroundCCD3Calibration.CancelAsync();
            }
        }
        private void backgroundCCD3Calibration_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(500);
                if (backgroundCCD3Calibration.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (GlobalVar.CCD3Calibration.Count == 0)
                {
                    Station2LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[10], 0, 1, Dis,false);    //标定第一个点    对应CCD3
                    KeyenceVision2.instance().sendData("CC,04");   //第一个点发送标定拍照指令

                }
                else
                {
                    switch (GlobalVar.CCD3Calibration[GlobalVar.CCD3Calibration.Count - 1])
                    {
                        case "OK":
                            switch (GlobalVar.CCD3Calibration.Count)
                            {
                                case 1:
                                    Dis[0] = 5;
                                    Dis[1] = 0;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[10], 0, 1, Dis, false);    //标定第2个点    对应CCD3与电批2
                                    KeyenceVision2.instance().sendData("CC,04");

                                    break;
                                case 2:
                                    Dis[0] = 5;
                                    Dis[1] = 5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[10], 0, 1, Dis, false);    //标定第3个点    对应CCD3与电批2
                                    KeyenceVision2.instance().sendData("CC,04");

                                    break;
                                case 3:
                                    Dis[0] = 0;
                                    Dis[1] = 5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[10], 0, 1, Dis, false);    //标定第4个点    对应CCD3与电批2
                                    KeyenceVision2.instance().sendData("CC,04");

                                    break;
                                case 4:
                                    Dis[0] = -5;
                                    Dis[1] = 5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[10], 0, 1, Dis, false);    //标定第5个点    对应CCD3与电批2
                                    KeyenceVision2.instance().sendData("CC,04");

                                    break;
                                case 5:
                                    Dis[0] = -5;
                                    Dis[1] = 0;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[10], 0, 1, Dis, false);    //标定第6个点    对应CCD3与电批2
                                    KeyenceVision2.instance().sendData("CC,04");

                                    break;
                                case 6:
                                    Dis[0] = -5;
                                    Dis[1] = -5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[10], 0, 1, Dis, false);    //标定第7个点    对应CCD3与电批2
                                    KeyenceVision2.instance().sendData("CC,04");

                                    break;
                                case 7:
                                    Dis[0] = 0;
                                    Dis[1] = -5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[10], 0, 1, Dis, false);    //标定第8个点    对应CCD3与电批2
                                    KeyenceVision2.instance().sendData("CC,04");

                                    break;
                                case 8:
                                    Dis[0] = 5;
                                    Dis[1] = -5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[10], 0, 1, Dis, false);    //标定第9个点    对应CCD3与电批2
                                    KeyenceVision2.instance().sendData("CC,04");

                                    break;
                            }
                            break;
                        case "Finish":

                            break;
                        case "Alarm":

                            break;
                    }
                }
            }
        }
        #endregion
        #region CCD4 标定
        public void startbackgroundCCD4Calibration()
        {
            if (backgroundCCD4Calibration.IsBusy != true)
            {
                backgroundCCD4Calibration.RunWorkerAsync();
            }
        }
        public void stopbackgroundCCD4Calibration()
        {
            if (backgroundCCD4Calibration.IsBusy == true)
            {
                backgroundCCD4Calibration.CancelAsync();
            }
        }
        private void backgroundCCD4Calibration_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(500);
                if (backgroundCCD4Calibration.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (GlobalVar.CCD4Calibration.Count == 0)
                {
                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[11], 0, 1, Dis, false);    //标定第一个点    对应CCD4与电批2
                    KeyenceVision2.instance().sendData("CC,05");   //第一个点发送标定拍照指令

                }
                else
                {
                    switch (GlobalVar.CCD4Calibration[GlobalVar.CCD4Calibration.Count - 1])
                    {
                        case "OK":
                            switch (GlobalVar.CCD4Calibration.Count)
                            {
                                case 1:
                                    Dis[0] = 5;
                                    Dis[1] = 0;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[11], 0, 1, Dis, false);    //标定第2个点    对应CCD4与电批2
                                    KeyenceVision2.instance().sendData("CC,05");

                                    break;
                                case 2:
                                    Dis[0] = 5;
                                    Dis[1] = 5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[11], 0, 1, Dis, false);    //标定第3个点    对应CCD4与电批2
                                    KeyenceVision2.instance().sendData("CC,05");

                                    break;
                                case 3:
                                    Dis[0] = 0;
                                    Dis[1] = 5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[11], 0, 1, Dis, false);    //标定第4个点    对应CCD4与电批2
                                    KeyenceVision2.instance().sendData("CC,05");

                                    break;
                                case 4:
                                    Dis[0] = -5;
                                    Dis[1] = 5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[11], 0, 1, Dis, false);    //标定第5个点    对应CCD4与电批2
                                    KeyenceVision2.instance().sendData("CC,05");

                                    break;
                                case 5:
                                    Dis[0] = -5;
                                    Dis[1] = 0;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[11], 0, 1, Dis, false);    //标定第6个点    对应CCD4与电批2
                                    KeyenceVision2.instance().sendData("CC,05");

                                    break;
                                case 6:
                                    Dis[0] = -5;
                                    Dis[1] = -5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[11], 0, 1, Dis, false);    //标定第7个点    对应CCD4与电批2
                                    KeyenceVision2.instance().sendData("CC,05");

                                    break;
                                case 7:
                                    Dis[0] = 0;
                                    Dis[1] = -5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[11], 0, 1, Dis, false);    //标定第8个点    对应CCD4与电批2
                                    KeyenceVision2.instance().sendData("CC,05");

                                    break;
                                case 8:
                                    Dis[0] = 5;
                                    Dis[1] = -5;
                                    Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[11], 0, 1, Dis, false);    //标定第9个点    对应CCD4与电批2
                                    KeyenceVision2.instance().sendData("CC,02");

                                    break;
                            }
                            break;
                        case "Finish":

                            break;
                        case "Alarm":

                            break;
                    }
                }
            }
        }
        #endregion

      

        //private void backgroundReset_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    ResetPort1();
        //}
        #endregion
        #region 备份

        #endregion

    }
}
