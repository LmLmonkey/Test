using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScrew
{
    public enum IOListInput
    {
        ///一号IO卡输入      1011
        StartButton = 0,    //启动按钮
        StopButtopn = 1,    //停止按钮
        PuaseButton = 2,    //暂停按钮
        ResetButton1 = 3,    //复位按钮1
        ResetButton2 = 4,    //复位按钮2
        ResetButton3 = 5,    //复位按钮3
        ScramButton = 6,    //急停按钮1
        _Null = 7,    //null
        FrontDoor1 = 8,    //前门1
        FrontDoor2 = 9,    //前门2
        RightDoor3 = 10,    //右门3
        RightDoor4 = 11,    //右门4
        BackDoor5 = 12,    //后门5
        BackDoor6 = 13,    //后门6
        LeftDoor7 = 14,    //左门7
        LeftDoor8 = 15,    //左门8
        HSGFeedBlockUp = 16,    //进料阻挡气缸上位
        HSGFeedBlockDown = 17,    //进料阻挡气缸下位
        Station1BlockUp = 18,    //工位1阻挡气缸上位
        Station1BlockDown = 19,    //工位1阻挡气缸下位
        Station1JackUpUp = 20,    //工位1顶升气缸上位
        Station1JackUpDown = 21,    //工位1顶升气缸下位
        Station1UnlockForwardFront = 22,    //工位1解锁前推气缸前位
        Station1UnlockForwardBack = 23,    //工位1解锁前推气缸后位
        Station1UnlockClampFront = 24,    //工位1解锁夹紧气缸夹位
        Station1UnlockClampBack = 25,    //工位1解锁夹紧气缸松位
        Station1UnlockLeanFront = 26,    //工位1解锁倾斜气缸正位
        Station1UnlockLeanBack = 27,    //工位1解锁倾斜气缸斜位
        Station1PressBegun = 28,    //工位1压料气缸原位
        Station1PressArrived = 29,    //工位1压料气缸到位
        Station1LeanBackBegun = 30,    //工位1斜料回退气缸回位
        Station1LeanBackArrived = 31,    //工位1斜料回退气缸出位

        //二号IO卡输入 
        Station2Light1Begun = 0,    //工位2光源1气缸原位
        Station2Light1Arrived = 1,    //工位2光源1气缸到位
        Station2BlockUp = 2,    //工位2阻挡气缸上位
        Station2BlockDown = 3,    //工位2阻挡气缸下位
        Station2JackUpUp = 4,    //工位2顶升气缸上位
        Station2JackUpDown = 5,    //工位2顶升气缸下位
        Station2UnlockForwardFront = 6,    //工位2解锁前推气缸前位
        Station2UnlockForwardBack = 7,    //工位2解锁前推气缸后位
        Station2UnlockClampFront = 8,    //工位2解锁夹紧气缸夹位
        Station2UnlockClampBack = 9,    //工位2解锁夹紧气缸松位
        Station2LeanFront = 10,    //工位2斜料倾斜气缸正位
        Station2LeanBack = 11,    //工位2斜料倾斜气缸斜位
        Station2PressBegun = 12,    //工位2压料气缸原位
        Station2PressArrived = 13,    //工位2压料气缸到位
        Station1Light1Begun = 14,    //工位1光源1气缸1原位
        Station1Light1Arrived = 15,    //工位1光源1气缸1到位
        Station3Light1Begun = 16,    //工位3光源1气缸原位
        Station3Light1Arrived = 17,    //工位3光源1气缸到位
        Station3BlockUp = 18,    //工位3阻挡气缸上位
        Station3BlockDown = 19,    //工位3阻挡气缸下位
        Station3JackUpUp = 20,    //工位3顶升气缸上位
        Station3JackUpDown = 21,    //工位3顶升气缸下位
        Station3LeanBackBegun = 22,    //工位3斜料回退气缸回位
        Station3LeanBackArrived = 23,    //工位3斜料回退气缸出位
        Station3PressBegun = 24,    //工位3压料气缸原位
        Station3PressArrived = 25,    //工位3压料气缸到位
        Station3Light2Begun = 26,    //工位3光源2气缸原位
        Station3Light2Arrived = 27,    //工位3光源2气缸到位
        DisChargeBlockUP = 28,    //下料阻挡气缸上位
        DisChargeBlockDown = 29,    //下料阻挡气缸下位
        DisChargeJackUpUp = 30,    //下料顶升气缸上位
        DisChargeJackUpDown = 31,    //下料顶升气缸下位


        //三号卡IO输入
        Station2ScrewVacuum = 0,    //工位2螺丝臂真空表
        Station3ScrewVacuum = 1,    //工位3螺丝臂真空表
        Station2LeftScrewBoxReady = 2,    //工位2左螺丝盒ready OK
        Station2RightScrewBoxReady = 3,    //工位2右螺丝盒ready OK
        Station3LeftScrewBoxReady = 4,    //工位3左螺丝盒ready OK
        Station3RightScrewBoxReady = 5,    //工位3右螺丝盒ready OK
        Station2EleScrewDone = 6,    //工位2电批完成信号
        Station2EleScrewError = 7,    //工位2电批错误信号
        Station2EleScrewReady = 8,    //工位2电批准备好信号
        Station2EleScrewRun = 9,    //工位2电批运行中
        Station3EleScrewDone = 10,    //工位3电批完成信号
        Station3EleScrewError = 11,    //工位3电批错误信号
        Station3EleScrewReady = 12,    //工位3电批准备好信号
        Station3EleScrewRun = 13,    //工位3电批运行中
        //14
        Station3Light5Arrived = 15,    //工位3电批运行中
        FeedBackRay = 16,    //进料工位后对射
        Station1FrontRay = 17,    //工位1前对射
        Station1BackRay = 18,    //工位1后对射
        Station2FrontRay = 19,    //工位2前对射
        Station2BackRay = 20,    //工位2后对射
        Station3FrontRay = 21,    //工位3前对射
        Station3BackRay = 22,    //工位3后对射
        DiachargeFrontRay = 23,    //下料工位前对射
        DiaChargeNG = 24,     //下料NG有料感应
        Station1Light2Begun = 25,    //工位1光源1气缸2原位
        Station1Light2Arrived = 26,    //工位1光源1气缸2到位
        Station3Light3Begun = 27,    //工位3光源3气缸原位
        Station3Light3Arrived = 28,    //工位3光源3气缸到位
        Station3Light4Begun = 29,    //工位3光源4气缸原位
        Station3Light4Arrived = 30,    //工位3光源4气缸到位
        Station3Light5Begun = 31,     //工位3光源5气缸原位
        //25---31

        //四号卡IO输入
        DisChargeClampBegun = 0,    //下料移栽夹爪松开
        DisChargeClampArrived = 1,    //下料移栽夹爪夹紧
        Station2ScrewCameraUP = 2,    //工位2螺丝臂摄像头上位
        Station2ScrewCameraDown = 3,   //工位2螺丝臂摄像头下位
        Station3ScrewCameraUP = 4,    //工位3螺丝臂摄像头上位
        Station3ScrewCameraDown = 5    //工位3螺丝臂摄像头下位
    }
    public enum IOListOutput
    {
        //一号卡输出IO
        StartButtonLight = 0,    //启动按钮灯
        StopButtonLight = 1,    //停止按钮灯
        PuaseButtonLight =2,    //暂停按钮灯
        ResetButtonLight1 = 3,    //复位按钮灯1
        ResetButtonLight2 = 4,    //复位按钮灯2
        ResetButtonLight3 = 5,    //复位按钮灯3
        FeedLight = 6,    //进料提示指示灯
        DiachargeLight = 7,    //出料提示指示灯
        RedLamp = 8,                           //红
        GreenLamp = 9,                         //绿
        YellowLamp = 10,                        //黄
        BuzzerCall = 11,                        //蜂鸣器
        Light = 12,                        //蜂鸣器
        //12--15
        HSGFeedBlockUp = 16,    //进料阻挡气缸上位电磁阀
        HSGFeedBlockDown = 17,    //进料阻挡气缸下位电磁阀
        Station1BlockUp = 18,    //工位1阻挡气缸上位电磁阀
        Station1BlockDown = 19,    //工位1阻挡气缸下位电磁阀
        Station1JackUpUp = 20,    //工位1顶升气缸上位电磁阀
        Station1JackUpDown = 21,    //工位1顶升气缸下位电磁阀
        Station1UnlockForwardFront = 22,    //工位1解锁前推气缸前位电磁阀
        Station1UnlockForwardBack = 23,    //工位1解锁前推气缸后位电磁阀
        Station1UnlockClampFront = 24,    //工位1解锁夹紧气缸夹位电磁阀
        Station1UnlockClampBack = 25,    //工位1解锁夹紧气缸松位电磁阀
        Station1UnlockLeanFront = 26,    //工位1解锁倾斜气缸正位电磁阀
        Station1UnlockLeanBack = 27,    //工位1解锁倾斜气缸斜位电磁阀
        Station1PressBegun = 28,    //工位1压料气缸原位电磁阀
        Station1PressArrived = 29,    //工位1压料气缸到位电磁阀
        Station1LeanBackBegun = 30,    //工位1斜料回退气缸回位电磁阀
        Station1LeanBackArrived = 31,    //工位1斜料回退气缸出位电磁阀

        //二号卡输出IO
        Station2Light1Begun = 0,    //工位2光源1气缸原位电磁阀
        Station2Light1Arrived = 1,    //工位2光源1气缸到位电磁阀
        Station2BlockUp = 2,    //工位2阻挡气缸上位电磁阀
        Station2BlockDown = 3,    //工位2阻挡气缸下位电磁阀
        Station2JackUpUp = 4,    //工位2顶升气缸上位电磁阀
        Station2JackUpDown = 5,    //工位2顶升气缸下位电磁阀
        Station2UnlockForwardFront = 6,    //工位2解锁前推气缸前位电磁阀
        Station2UnlockForwardBack = 7,    //工位2解锁前推气缸后位电磁阀
        Station2UnlockClampFront = 8,    //工位2解锁夹紧气缸夹位电磁阀
        Station2UnlockClampBack = 9,    //工位2解锁夹紧气缸松位电磁阀
        Station2LeanFront = 10,    //工位2斜料倾斜气缸正位电磁阀
        Station2LeanBack = 11,    //工位2斜料倾斜气缸斜位电磁阀
        Station2PressBegun = 12,    //工位2压料气缸原位电磁阀
        Station2PressArrived = 13,    //工位2压料气缸到位电磁阀
        Station1Light1Begun = 14,    //工位1光源1气缸原位电磁阀
        Station1Light1Arrived = 15,    //工位1光源1气缸到位电磁阀
        Station3Light1Begun = 16,    //工位3光源1气缸原位电磁阀
        Station3Light1Arrived = 17,    //工位3光源1气缸到位电磁阀
        Station3BlockUp = 18,    //工位3阻挡气缸上位电磁阀
        Station3BlockDown = 19,    //工位3阻挡气缸下位电磁阀
        Station3JackUpUp = 20,    //工位3顶升气缸上位电磁阀
        Station3JackUpDown = 21,    //工位3顶升气缸下位电磁阀
        Station3LeanBackBegun = 22,    //工位3斜料回退气缸回位电磁阀
        Station3LeanBackArrived = 23,    //工位3斜料回退气缸出位电磁阀
        Station3PressBegun = 24,    //工位3压料气缸原位电磁阀
        Station3PressArrived = 25,    //工位3压料气缸到位电磁阀
        Station3Light2Begun = 26,    //工位3光源2气缸原位电磁阀
        Station3Light2Arrived = 27,    //工位3光源2气缸到位电磁阀
        DisChargeBlockUP = 28,    //下料阻挡气缸上位电磁阀
        DisChargeBlockDown = 29,    //下料阻挡气缸下位电磁阀
        DisChargeJackUpUp = 30,    //下料顶升气缸上位电磁阀
        DisChargeJackUpDown = 31,    //下料顶升气缸下位电磁阀

        //三号卡输出IO
        Station2ScrewVacuum = 0,    //工位2吸真空阀
        Station3ScrewVacuum = 1,    //工位3吸真空阀
        Station2LeftScrewBoxRun = 2,    //工位2左螺丝盒Start RUN
        Station2RightScrewBoxRun = 3,    //工位2右螺丝盒Start RUN
        Station3LeftScrewBoxRun = 4,    //工位3左螺丝盒Start RUN
        Station3RightScrewBoxRun = 5,    //工位3右螺丝盒Start RUN
        Station2EleScrewStart = 6,    //工位2电批启动
        Sation2EleScrewSelectD0 = 7,    //工位2电批选择D0预设
        Sation2EleScrewSelectD1 = 8,    //工位2电批选择D1预设
        Sation2EleScrewSelectD2 = 9,    //工位2电批选择D2预设
        Station3EleScrewStart = 10,    //工位3电批启动
        Sation3EleScrewSelectD0 = 11,    //工位3电批选择D0预设
        Sation3EleScrewSelectD1 = 12,    //工位3电批选择D1预设
        Sation3EleScrewSelectD2 = 13,    //工位3电批选择D2预设
        Station2Blow = 14,    //工位2破真空
        Station3Blow = 15,    //工位3破真空
        Light1ON = 16,    //光源1触发
        Light2ON = 17,    //光源2触发
        Light3ON = 18,    //光源3触发
        Light4ON = 19,    //光源4触发
        Light5ON = 20,    //光源5触发
        Light6ON = 21,    //光源6触发
        Light7ON = 22,    //光源7触发
        Light8ON = 23,    //光源8触发
        Station1Light2Begun = 24,    //工位1光源2气缸原位电磁阀
        Station1Light2Arrived = 25,    //工位1光源2气缸到位电磁阀
        Station3Light3Begun = 26,    //工位3光源3气缸原位电磁阀
        Station3Light3Arrived = 27,    //工位3光源3气缸到位电磁阀
        Station3Light4Begun = 28,    //工位3光源4气缸原位电磁阀
        Station3Light4Arrived = 29,    //工位3光源4气缸到位电磁阀
        Station3Light5Begun = 30,    //工位3光源5气缸原位电磁阀
        Station3Light5Arrived = 31,    //工位3光源5气缸到位电磁阀

        //四号卡输出IO
        DisChargeClampBegun = 0,    //下料移栽夹爪松开电磁阀
        DisChargeClampArrived = 1,    //下料移栽夹爪夹紧电磁阀
        Station2ScrewCameraUP = 2,    //工位2螺丝臂摄像头上升电磁阀
        Station2ScrewCameraDown = 3,    //工位2螺丝臂摄像头下降电磁阀
        Station3ScrewCameraUP = 4,    //工位3螺丝臂摄像头上升电磁阀
        Station3ScrewCameraDown = 5    //工位3螺丝臂摄像头下降电磁阀
    }
}

