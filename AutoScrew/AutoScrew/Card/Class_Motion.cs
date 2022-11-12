using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoScrew
{
    class Class_Motion
    {
        private ushort _CardID = 0;
        private static Class_Motion IOCCB;
        public double[] scale = new double[16];   //11个轴的脉冲当量  1微米
        public bool initCardState = false;
        public static Class_Motion instance()
        {
            if (IOCCB == null) IOCCB = new Class_Motion();
            return IOCCB;
        }
        public Class_Motion()
        {
            InitialScale();
        }
        public void InitialScale()
        {
            //后期如果没有变化   采用一个变量即可，防止出现差异，分单轴来处理
            scale[0] = 0.001; //  mm X1
            scale[1] = 0.001; //  mm Y1
            scale[2] = 0.001; //  mm Z1
            scale[3] = 0.001; //  mm X2
            scale[4] = 0.001; //  mm Y2
            scale[5] = 0.001; //  mm Z2
            scale[6] = 0.001; //  mm X3
            scale[7] = 0.001; //  mm Y3
            scale[8] = 0.001; //  mm Z3
            scale[9] = 0.001; //  mm 
            scale[10] = 0.001; //  mm
            scale[11] = 0.001; //  mm X3
            scale[12] = 0.001; //  mm Y3
            scale[13] = 0.001; //  mm Z3
            scale[14] = 0.001; //  mm 
            scale[15] = 0.001; //  mm
        }
        public void OpenCard()    //打开运动控制卡
        {
            short num = LTDMC.dmc_board_init();//获取卡数量
            if (num <= 0 || num > 8)
            {
                //Notice NotiCeFormLTDMC = new Notice();
                //NotiCeFormLTDMC.SetNotice = "初始卡失败!未找到运动控制卡！！！";
                //NotiCeFormLTDMC.Show();
                MessageBox.Show("初始卡失败!未找到运动控制卡！！！");
                return;
                //MessageBox.Show("初始卡失败!", "出错");
            }
            ushort _num = 0;
            ushort[] cardids = new ushort[8];
            uint[] cardtypes = new uint[8];
            short res = LTDMC.dmc_get_CardInfList(ref _num, cardtypes, cardids);
            if (res != 0)
            {
                //Notice NotiCeFormLTDMC = new Notice();
                //NotiCeFormLTDMC.SetNotice = "获取卡信息失败！！！";
                //NotiCeFormLTDMC.Show();
                MessageBox.Show("获取卡信息失败！！！");
                return;
            }
            initCardState = true;
            _CardID = cardids[0];
        }
        public void CloseCard()       //关闭运动控制卡
        {
            if (initCardState == true)
            {
                short res = LTDMC.dmc_board_close();
                initCardState = false;
            }
        }
        public void EnableAxis(ushort Axis)     //使能轴
        {
            short res = 0;
            res = LTDMC.nmc_set_axis_enable(_CardID, Axis);// 使能对应轴   只有一张卡          如果Axis = 255 表示使能所有轴
            if (res != 0)
            {
                Notice NotiCeFormLTDMC = new Notice();
                NotiCeFormLTDMC.SetNotice = "nmc_set_axis_enable == " + res.ToString();
                return;
            }
        }
        public void DisableAxis(ushort Axis)// 失能对应轴
        {
            short res = 0;
            res = LTDMC.nmc_set_axis_disable(_CardID, Axis);// 失能对应轴   只有一张卡          如果Axis = 255 表示失能所有轴
            if (res != 0)
            {
                Notice NotiCeFormLTDMC = new Notice();
                NotiCeFormLTDMC.SetNotice = "nmc_set_axis_disable == " + res.ToString();
                return;
            }
        }
        public void PmoveSetAxisParameter(ushort Axis, double dStartVel, double dMaxVel, double dTacc, double dTdec, double dStopVel, double dS_para)
        {
            //double dStartVel;//起始速度
            //double dMaxVel;//运行速度
            //double dTacc;//加速时间
            //double dTdec;//减速时间
            //double dStopVel;//停止速度
            //double dS_para;//S段时间
            //double dDist;//目标位置

            short res = 0;

            dStartVel = dStartVel / scale[Axis];
            dMaxVel = dMaxVel / scale[Axis] * GlobalVar.SpeedAccPercent;
            ////dTacc = decimal.ToDouble(numericUpDown5.Value);
            ////dTdec = decimal.ToDouble(numericUpDown6.Value);
            dStopVel = dStopVel / scale[Axis];
            //dS_para = decimal.ToDouble(numericUpDown7.Value);
            res = LTDMC.dmc_set_profile_unit(_CardID, Axis, dStartVel, dMaxVel, dTacc, dTdec, dStopVel);//设置速度参数
            res = LTDMC.dmc_set_s_profile(_CardID, Axis, 0, dS_para);//设置S段速度参数     S 段时间，单位：s；范围：0~1
            res = LTDMC.dmc_set_dec_stop_time(_CardID, Axis, dTdec); //设置减速停止时间
            if (res != 0)
            {
                // MessageBox.Show("dmc_pmove_unit == " + res.ToString());
                return;
            }
        }
        public void PMove(ushort Axis, double dDist, ushort sPosi_mode)     //定长运动
        {
            //ushort sPosi_mode; //运动模式0：相对坐标模式，1：绝对坐标模式
            short res = 0;
            dDist = dDist / scale[Axis];
            res = LTDMC.dmc_pmove_unit(_CardID, Axis, dDist, sPosi_mode);//定长运动
            if (res != 0)
            {
                // MessageBox.Show("dmc_pmove_unit == " + res.ToString());
                return;
            }
        }
        public void VMoveSetAxisParameter(ushort Axis, double dMaxVel, double dTacc, double dTdec, double dTdec1, double dStopVel, double dS_para)     //连续运动
        {

            //double dStartVel;//起始速度
            //double dMaxVel;//运行速度
            //double dTacc;//加速时间
            //double dTdec;//减速时间
            //double dStopVel;//停止速度
            //double dS_para;//S段时间
            //ushort sDir; //运动方向，0：负方向，1：正方向
            ushort State_Machine = 0;//获取轴状态机

            //dEquiv = decimal.ToDouble(numericUpDown2.Value);
            dMaxVel = dMaxVel / scale[Axis] * GlobalVar.SpeedAccPercent;
            //dMaxVel = dMaxVel / scale[Axis];
            //dTacc = decimal.ToDouble(numericUpDown5.Value);
            //dTdec = decimal.ToDouble(numericUpDown6.Value);
            dStopVel = dStopVel / scale[Axis];
            //dS_para = decimal.ToDouble(numericUpDown7.Value);
            short res = 0;
            res = LTDMC.dmc_set_profile_unit(_CardID, Axis, 0, dMaxVel, dTacc, dTdec, dStopVel);//设置速度参数
            res = LTDMC.dmc_set_s_profile(_CardID, Axis, 0, 0.01);//设置S段速度参数
            res = LTDMC.dmc_set_dec_stop_time(_CardID, Axis, dTdec); //设置减速停止时间
            if (res != 0)
            {
                //MessageBox.Show("dmc_vmove == " + res.ToString());
            }
        }
        public void VMove(ushort Axis, ushort sDir)     //连续运动
        {
            //ushort sDir; //运动方向，0：负方向，1：正方向
            ushort State_Machine = 0;//获取轴状态机
            short res = 0;
            res = LTDMC.dmc_vmove(_CardID, Axis, sDir);//连续运动
            if (res != 0)
            {
                //MessageBox.Show("dmc_vmove == " + res.ToString());
            }
        }

        public void StopAxis(ushort Axis)  //停止指定轴的运动
        {
            // APS168.APS_stop_move(PortId);
            LTDMC.dmc_stop(_CardID, Axis, 0);
        }

        public bool IsAxisStopped(ushort AxisId)   //  指定轴是否已经停止
        {
            if (LTDMC.dmc_check_done(_CardID, AxisId) != 1)
            {
                return false;  // 电机运动中
            }
            else
            {
                return true;  // 电机停止中
            }
        }
        public void WaitStopPlus(ushort AxisId, int Dist, bool Pause, bool Stopstatue)  //等待指定轴停
        {
            while (IsAxisStopped(AxisId) == false)
            {
                Application.DoEvents();
                Thread.Sleep(350);
                if (GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                {
                    StopAxis(AxisId); //停止轴的运动
                    while (GlobalVar.bPauseStatus) //暂停中
                    {
                        Application.DoEvents();
                        Thread.Sleep(350);
                    }
                    if (!GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                    {
                        PMove(AxisId, Dist, 1);  //绝对运动
                    }
                }
            }
        }
        public void HomeWaitStop(ushort AxisId)  //等待指定轴停
        {
            while (IsAxisStopped(AxisId) == false)
            {
                Application.DoEvents();
                Thread.Sleep(350);
                //if (GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                //{
                //    StopAxis(AxisId); //停止轴的运动
                //    while (GlobalVar.bPauseStatus) //暂停中
                //    {
                //        Application.DoEvents();
                //        Thread.Sleep(350);
                //    }
                //    if (!GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                //    {
                //        PMove(AxisId, Dist, 1);  //绝对运动
                //    }
                //}
            }
        }
        public void SetVectorProfileUnit(ushort Axis, ushort Card, double dStartVel, double dMaxVel, double dTacc, double dTdec, double dStopVel) //直线插补运动参数
        {
            short res = 0;
            dMaxVel = dMaxVel / scale[Axis] * GlobalVar.SpeedAccPercent;
            res = LTDMC.dmc_set_vector_profile_unit(_CardID, Card, dStartVel, dMaxVel, dTacc, dTdec, dStopVel);
            if (res != 0)
            {
                MessageBox.Show("dmc_set_vector_profile_unit == " + res.ToString());
            }
        }
        public void dmc_line_unit(ushort Crd, ushort AxisNum, ushort[] AxisList, double[] Target_Pos, ushort posi_mode)    //直线插补运动
        {
            double[] Target_Pos_N = new double[Target_Pos.Length];
            for (int i = 0; i < Target_Pos.Length; i++)
            {
                Target_Pos_N[i] = Target_Pos[i] / scale[AxisList[i]];                            //
            }
            short res = LTDMC.dmc_line_unit(_CardID, Crd, AxisNum, AxisList, Target_Pos_N, posi_mode);
            if (res != 0)
            {
                MessageBox.Show("dmc_line_unit == " + res.ToString());
            }
        }
        public uint ReadInput(ushort nodeID)    //NoteID EtherCAT 地址，从 1001 开始，按从站数往后累加
        {
            uint n = 0;
            LTDMC.nmc_read_inport_extern(_CardID, 2, nodeID, 0, ref n);
            return n;
        }
        public uint ReadInputBit(ushort nodeID, ushort index)    //NoteID EtherCAT 地址，从 1001 开始，按从站数往后累加   读取某一位
        {
            ushort n = 0;
            LTDMC.nmc_read_inbit_extern(_CardID, 2, nodeID, index, ref n);
            return n;
        }
        public uint ReadOutputBit(ushort nodeID, ushort index)    //NoteID EtherCAT 地址，从 1001 开始，按从站数往后累加    读取某一位
        {
            ushort n = 0;
            LTDMC.nmc_read_inbit_extern(_CardID, 2, nodeID, index, ref n);
            return n;
        }

        public short ReadCommonInput(ushort MyInbitno)
        {
            short Value = 0;
            Value = LTDMC.dmc_read_inbit(_CardID, MyInbitno);
            return Value;
        }

        public void SetCommonOutPut(ushort MyInbitno)
        {
            LTDMC.dmc_write_outbit(_CardID, MyInbitno, 1);
        }

        public void ReSetCommonOutPut(ushort MyInbitno)
        {
            LTDMC.dmc_write_outbit(_CardID, MyInbitno, 0);
        }

        public void WriteOutbit(ushort nodeID, ushort index)   //NoteID EtherCAT 地址，从 1001 开始，按从站数往后累加
        {

            short s = 0;
            s = LTDMC.nmc_write_outbit_extern(_CardID, 2, nodeID, index, 0);
            if (s != 0)
            {
                MessageBox.Show("nmc_write_outbit_extern == " + s.ToString());
            }
        }
        public void SetOutPut(ushort nodeID, ushort index)
        {

            short s = 0;
            s = LTDMC.nmc_write_outbit_extern(_CardID, 2, nodeID, (ushort)index, 0);
            if (s != 0)
            {
                MessageBox.Show("nmc_write_outbit_extern == " + s.ToString());
            }
        }
        public void ResetOutPut(ushort nodeID, ushort index)
        {

            short s = 0;
            s = LTDMC.nmc_write_outbit_extern(_CardID, 2, nodeID, (ushort)index, 1);
            if (s != 0)
            {
                MessageBox.Show("nmc_write_outbit_extern == " + s.ToString());
            }
        }
        public void Cylinder(ushort nodeID, ushort Begun, ushort Arrived)
        {
            SetOutPut(nodeID, Arrived);
            ResetOutPut(nodeID, Begun);
        }
        public void CylinderComm(ushort Begun, ushort Arrived)
        {
            SetCommonOutPut( Arrived);
            ReSetCommonOutPut(Begun);
        }
        public double GetPosition(ushort Axis)
        {
            double encPos = 0; //反馈位置
            LTDMC.dmc_get_encoder_unit(_CardID, Axis, ref encPos);//读取指定轴反馈位置值
            return encPos * scale[Axis];
        }

        public bool CheckServoStatus(int shiftNum, int AxisId)   //获取各个轴的IO信号状态,左移OR右移？
        {
            //int status = APS168.APS_motion_io_status(axis_id);
            uint status = LTDMC.dmc_axis_io_status(_CardID, (ushort)AxisId);
            return ((status >> shiftNum) & 1) == 1;
        }

        public void Home(ushort Axis)
        {
            short res = LTDMC.nmc_set_home_profile(_CardID, Axis, 11, 1000, 5000, 0.01, 0.01, 0);
            if (res != 0)
            {
                MessageBox.Show("nmc_set_home_profile == " + res.ToString());
            }
            res = LTDMC.nmc_home_move(_CardID, Axis);//启动回零     
            if (res != 0)
            {
                MessageBox.Show("nmc_home_move == " + res.ToString());
                return;
            }
        }
        public void Station1Line(ushort AxisNum, ushort[] AxisList, double[] Target_Pos, ushort posi_mode)
        {
            dmc_line_unit(0, AxisNum, AxisList, Target_Pos, posi_mode);
        }
        public void Station2Line(ushort AxisNum, ushort[] AxisList, double[] Target_Pos, ushort posi_mode)
        {
            dmc_line_unit(1, AxisNum, AxisList, Target_Pos, posi_mode);
        }
        public void Station3Line(ushort AxisNum, ushort[] AxisList, double[] Target_Pos, ushort posi_mode)
        {
            dmc_line_unit(2, AxisNum, AxisList, Target_Pos, posi_mode);
        }

        public void Station4Line(ushort AxisNum, ushort[] AxisList, double[] Target_Pos, ushort posi_mode)   //下料轴
        {
            dmc_line_unit(3, AxisNum, AxisList, Target_Pos, posi_mode);
        }

        public void Stop3Axis(ushort[] AxisList)
        {
            StopAxis(AxisList[0]); //停止轴的运动
            StopAxis(AxisList[1]); //停止轴的运动
            StopAxis(AxisList[2]); //停止轴的运动
        }
        public void Stop2Axis(ushort[] AxisList)
        {
            StopAxis(AxisList[0]); //停止轴的运动
            StopAxis(AxisList[1]); //停止轴的运动
        }
        public void EnableAllAxis()
        {
            for (int i = 0; i < 16; i++)
            {
                EnableAxis((ushort)i);
            }
        }
        public void Station1WaitStopPlus(ushort AxisNum, ushort[] AxisList, double[] Target_Pos, ushort posi_mode)  //等待指定轴停
        {
            while (IsAxisStopped(AxisList[0]) == false || IsAxisStopped(AxisList[1]) == false || IsAxisStopped(AxisList[2]) == false)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                if (GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                {
                    Stop3Axis(AxisList);
                    while (GlobalVar.bPauseStatus) //暂停中
                    {
                        Application.DoEvents();
                        Thread.Sleep(20);
                    }
                    if (!GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                    {
                        dmc_line_unit(0, AxisNum, AxisList, Target_Pos, posi_mode);
                    }
                }
            }
        }
        public void Station2WaitStopPlus(ushort AxisNum, ushort[] AxisList, double[] Target_Pos, ushort posi_mode)  //等待指定轴停
        {
            while (IsAxisStopped(AxisList[0]) == false || IsAxisStopped(AxisList[1]) == false || IsAxisStopped(AxisList[2]) == false)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                if (GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                {
                    Stop3Axis(AxisList);
                    while (GlobalVar.bPauseStatus) //暂停中
                    {
                        Application.DoEvents();
                        Thread.Sleep(20);
                    }
                    if (!GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                    {
                        dmc_line_unit(1, AxisNum, AxisList, Target_Pos, posi_mode);
                    }
                }
            }
        }
        public void Station3WaitStopPlus(ushort AxisNum, ushort[] AxisList, double[] Target_Pos, ushort posi_mode)  //等待指定轴停
        {
            while (IsAxisStopped(AxisList[0]) == false || IsAxisStopped(AxisList[1]) == false || IsAxisStopped(AxisList[2]) == false)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                if (GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                {
                    Stop3Axis(AxisList);
                    while (GlobalVar.bPauseStatus) //暂停中
                    {
                        Application.DoEvents();
                        Thread.Sleep(20);
                    }
                    if (!GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                    {
                        dmc_line_unit(2, AxisNum, AxisList, Target_Pos, posi_mode);
                    }
                }
            }
        }

        public void Station4WaitStopPlus(ushort AxisNum, ushort[] AxisList, double[] Target_Pos, ushort posi_mode)  //等待指定轴停
        {
            while (IsAxisStopped(AxisList[0]) == false || IsAxisStopped(AxisList[1]) == false)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                if (GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                {
                    Stop2Axis(AxisList);
                    while (GlobalVar.bPauseStatus) //暂停中
                    {
                        Application.DoEvents();
                        Thread.Sleep(20);
                    }
                    if (!GlobalVar.bPauseStatus && GlobalVar.bStartRunning)
                    {
                        dmc_line_unit(3, AxisNum, AxisList, Target_Pos, posi_mode);
                    }
                }
            }
        }

        public void AXISInit()
        {

            //初始化各轴
            PmoveSetAxisParameter(GlobalVar.Sation1XYZ[0], 0, GlobalVar.Station1AxisXParameter[0], GlobalVar.Station1AxisXParameter[1]
                , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
            PmoveSetAxisParameter(GlobalVar.Sation1XYZ[1], 0, GlobalVar.Station1AxisYParameter[0], GlobalVar.Station1AxisYParameter[1]
                   , GlobalVar.Station1AxisYParameter[2], 0, 0.01);
            PmoveSetAxisParameter(GlobalVar.Sation1XYZ[2], 0, GlobalVar.Station1AxisZParameter[0], GlobalVar.Station1AxisZParameter[1]
                   , GlobalVar.Station1AxisZParameter[2], 0, 0.01);

            PmoveSetAxisParameter(GlobalVar.Sation2XYZ[0], 0, GlobalVar.Station2AxisXParameter[0], GlobalVar.Station2AxisXParameter[1]
                , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
            PmoveSetAxisParameter(GlobalVar.Sation2XYZ[1], 0, GlobalVar.Station1AxisYParameter[0], GlobalVar.Station2AxisYParameter[1]
                   , GlobalVar.Station2AxisYParameter[2], 0, 0.01);
            PmoveSetAxisParameter(GlobalVar.Sation2XYZ[2], 0, GlobalVar.Station2AxisZParameter[0], GlobalVar.Station2AxisZParameter[1]
                   , GlobalVar.Station2AxisZParameter[2], 0, 0.01);

            PmoveSetAxisParameter(GlobalVar.Sation3XYZ[0], 0, GlobalVar.Station3AxisXParameter[0], GlobalVar.Station3AxisXParameter[1]
               , GlobalVar.Station3AxisXParameter[2], 0, 0.01);
            PmoveSetAxisParameter(GlobalVar.Sation3XYZ[1], 0, GlobalVar.Station3AxisYParameter[0], GlobalVar.Station3AxisYParameter[1]
                   , GlobalVar.Station2AxisYParameter[2], 0, 0.01);
            PmoveSetAxisParameter(GlobalVar.Sation3XYZ[2], 0, GlobalVar.Station3AxisZParameter[0], GlobalVar.Station3AxisZParameter[1]
                   , GlobalVar.Station3AxisZParameter[2], 0, 0.01);

            PmoveSetAxisParameter(GlobalVar.SingleAxis[0], 0, GlobalVar.SingleAxisXParameter[0], GlobalVar.SingleAxisXParameter[1]
                 , GlobalVar.SingleAxisXParameter[2], 0, 0.01);
            PmoveSetAxisParameter(GlobalVar.SingleAxis[1], 0, GlobalVar.SingleAxisZParameter[0], GlobalVar.SingleAxisZParameter[1]
                   , GlobalVar.SingleAxisZParameter[2], 0, 0.01);

            PmoveSetAxisParameter(GlobalVar.Conveyor[0], 0, GlobalVar.Station1ConveyorParameter[0], GlobalVar.Station1ConveyorParameter[1]
                 , GlobalVar.Station1ConveyorParameter[2], 0, 0.01);
            PmoveSetAxisParameter(GlobalVar.Conveyor[1], 0, GlobalVar.Station2ConveyorParameter[0], GlobalVar.Station2ConveyorParameter[1]
                   , GlobalVar.Station2ConveyorParameter[2], 0, 0.01);
            PmoveSetAxisParameter(GlobalVar.Conveyor[2], 0, GlobalVar.Station3ConveyorParameter[0], GlobalVar.Station3ConveyorParameter[1]
                   , GlobalVar.Station3ConveyorParameter[2], 0, 0.01);


            SetVectorProfileUnit(GlobalVar.Sation1XYZ[2], 0, 0, GlobalVar.Station1AxisZParameter[0], GlobalVar.Station1AxisZParameter[1]
                                        , GlobalVar.Station1AxisZParameter[2], 0);
            SetVectorProfileUnit(GlobalVar.Sation1XYZ[2], 1, 0, GlobalVar.Station2AxisZParameter[0], GlobalVar.Station2AxisZParameter[1]
                                    , GlobalVar.Station2AxisZParameter[2], 0);
            SetVectorProfileUnit(GlobalVar.Sation1XYZ[2], 2, 0, GlobalVar.Station3AxisZParameter[0], GlobalVar.Station3AxisZParameter[1]
                                   , GlobalVar.Station3AxisZParameter[2], 0);

            SetVectorProfileUnit(GlobalVar.SingleAxis[1], 3, 0, GlobalVar.SingleAxisZParameter[0], GlobalVar.SingleAxisZParameter[1]
                                  , GlobalVar.SingleAxisZParameter[2], 0);


        }
    }
}
