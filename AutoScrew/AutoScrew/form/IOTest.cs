using AutoScrew.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoScrew
{
    public partial class IOTest : Form
    {
        private MainStream mMainstream;
        private Class_Motion ClassMotion;
        List<double> Dis = new List<double>();
        public IOTest(MainStream mpMainstream)
        {
            InitializeComponent();
            ClassMotion = Class_Motion.instance();
            mMainstream = mpMainstream;
            Dis.Add(0);
            Dis.Add(0);
        }
        #region 点位描述
        private void PointsDisplay()
        {
            switch (cbSelect_CombinedAxisPoints.Text)
            {
                case "P1":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 等待位置";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 等待位置";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：等待位置";
                            break;
                    }
                    break;
                case "P2":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 螺丝孔拍照位置";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P3":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P4":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P5":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P6":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P7":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P8":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P9":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P10":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P11":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                case "P12":
                    switch (cbSelect_OneCombinedAxis.Text)
                    {
                        case GlobalVar.Station1Name:
                            richTextBox_PointsDisplay.Text = "工位1组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station2Name:
                            richTextBox_PointsDisplay.Text = "工位2组合轴: 吸传送带PCB位点";
                            break;
                        case GlobalVar.Station3Name:
                            richTextBox_PointsDisplay.Text = "工位3组合轴：吸取键帽位点";
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion
        #region 获取已保存点位
        public SQLiteDataReader ReadSqlite(string tablename, string point)
        {
            SQLiteDataReader reader = null;
            reader = CDBhelper._Get_Instance().PointMessage(tablename, point);
            return reader;
        }
        public void ReadPoint(string tablename)
        {
            for (int i = 0; i < 12; i++)
            {
                SQLiteDataReader reader = ReadSqlite(tablename, "P" + (i + 1).ToString());
                switch (tablename)
                {
                    case GlobalVar.Station1Name:
                        while (reader.Read())
                        {
                            GlobalVar.Station1CombinedAxis[i].Set(double.Parse(reader.GetValue(2).ToString()), double.Parse(reader.GetValue(3).ToString()), double.Parse(reader.GetValue(4).ToString()));
                        }
                        break;
                    case GlobalVar.Station2Name:
                        while (reader.Read())
                        {
                            GlobalVar.Station2CombinedAxis[i].Set(double.Parse(reader.GetValue(2).ToString()), double.Parse(reader.GetValue(3).ToString()), double.Parse(reader.GetValue(4).ToString()));
                        }
                        break;
                    case GlobalVar.Station3Name:
                        while (reader.Read())
                        {
                            GlobalVar.Station3CombinedAxis[i].Set(double.Parse(reader.GetValue(2).ToString()), double.Parse(reader.GetValue(3).ToString()), double.Parse(reader.GetValue(4).ToString()));
                        }
                        break;
                    default:
                        break;

                }

            }
        }

        public void ReadSinglePoint(string tablename)
        {
            for (int i = 0; i < 5; i++)
            {
                string str = "";
                switch (tablename)
                {
                    case GlobalVar.SingleXName:
                        FileOp.ReadDataFromXMLRP("Parameter", "SingleX", "P" + (i + 1), ref str);
                        GlobalVar.LayingOffX[i] = double.Parse(str);
                        break;
                    case GlobalVar.SingleZName:
                        FileOp.ReadDataFromXMLRP("Parameter", "SingleZ", "P" + (i + 1), ref str);
                        GlobalVar.LayingOffZ[i] = double.Parse(str);
                        break;
                    default:
                        break;

                }

            }
        }
        #endregion
        #region 点位显示
        private int _selectnum = 0;
        private void selectnum()
        {
            switch (cbSelect_CombinedAxisPoints.Text)
            {
                case "P1":
                    _selectnum = 0;
                    break;
                case "P2":
                    _selectnum = 1;
                    break;
                case "P3":
                    _selectnum = 2;
                    break;
                case "P4":
                    _selectnum = 3;
                    break;
                case "P5":
                    _selectnum = 4;
                    break;
                case "P6":
                    _selectnum = 5;
                    break;
                case "P7":
                    _selectnum = 6;
                    break;
                case "P8":
                    _selectnum = 7;
                    break;
                case "P9":
                    _selectnum = 8;
                    break;
                case "P10":
                    _selectnum = 9;
                    break;
                case "P11":
                    _selectnum = 10;
                    break;
                case "P12":
                    _selectnum = 11;
                    break;
                default:
                    MessageBox.Show("操作异常，请重启软件");
                    break;
            }
        }
        private void DisplaySavePoints()
        {
            TextBox[] testboxlist = { tbCombinedAxisSavePosition_X, tbCombinedAxisSavePosition_Y, tbCombinedAxisSavePosition_Z1 };
            selectnum();
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    GlobalVar.Station1CombinedAxis[_selectnum].Init();
                    ReadPoint(GlobalVar.Station1Name);
                    for (int i = 0; i < 3; i++)
                    {
                        testboxlist[i].Text = GlobalVar.Station1CombinedAxis[_selectnum].ToString1(i + 1);
                    }
                    break;
                case GlobalVar.Station2Name:
                    GlobalVar.Station2CombinedAxis[_selectnum].Init();
                    ReadPoint(GlobalVar.Station2Name);
                    for (int i = 0; i < 3; i++)
                    {
                        testboxlist[i].Text = GlobalVar.Station2CombinedAxis[_selectnum].ToString1(i + 1);
                    }
                    break;
                case GlobalVar.Station3Name:                  
                    GlobalVar.Station3CombinedAxis[_selectnum].Init();
                    ReadPoint(GlobalVar.Station3Name);
                    for (int i = 0; i < 3; i++)
                    {
                        testboxlist[i].Text = GlobalVar.Station3CombinedAxis[_selectnum].ToString1(i + 1);
                    }
                    break;
                default:
                    break;
            }
        }
        private void DisplaySaveSinglePoints()
        {
            Select_SingleAxisPoints();
            switch (cbSelect_OneSingleAxis.Text)
            {
                case GlobalVar.SingleXName:
                    ReadSinglePoint(GlobalVar.SingleXName);
                    textBox1.Text = GlobalVar.LayingOffX[_selectPoint-1].ToString("0.000");
                    break;
                case GlobalVar.SingleZName:
                    ReadSinglePoint(GlobalVar.SingleZName);
                    textBox1.Text = GlobalVar.LayingOffZ[_selectPoint-1].ToString("0.000");
                    break;
                default:
                    break;
            }
        }
        #endregion
        private void cbSelect_CombinedAxisPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            PointsDisplay();
            DisplaySavePoints();
        }
        private void CombinedAxisPara(ushort AxisX, ushort AxisY, ushort AxisZ)   //m1轴号 m2  m6 卡号   m1 x轴  m2 Y轴 m3 Z1 m4 Z2 m5 Z3
        {
            
            tbCombinedAxisPosition_NowX.Text = ClassMotion.GetPosition(AxisX).ToString();
            tbCombinedAxisPosition_NowY.Text = ClassMotion.GetPosition(AxisY).ToString();
            tbCombinedAxisPosition_NowZ1.Text = ClassMotion.GetPosition(AxisZ).ToString();
            HomeX.Image = (!ClassMotion.CheckServoStatus(4, AxisX)) ? (Resources.GreenLight) : (Resources.redButton);
            Positivelimit_X.Image = (!ClassMotion.CheckServoStatus(1, AxisX)) ? (Resources.GreenLight) : (Resources.redButton);
            Negtivelimit_X.Image = (!ClassMotion.CheckServoStatus(2, AxisX)) ? (Resources.GreenLight) : (Resources.redButton);
            ErrorX.Image = (!ClassMotion.CheckServoStatus(0, AxisX)) ? (Resources.GreenLight) : (Resources.redButton);
            HomeY.Image = (!ClassMotion.CheckServoStatus(4, AxisY)) ? (Resources.GreenLight) : (Resources.redButton);
            Positivelimit_Y.Image = (!ClassMotion.CheckServoStatus(1, AxisY)) ? (Resources.GreenLight) : (Resources.redButton);
            Negtivelimit_Y.Image = (!ClassMotion.CheckServoStatus(2, AxisY)) ? (Resources.GreenLight) : (Resources.redButton);
            ErrorY.Image = (!ClassMotion.CheckServoStatus(0, AxisY)) ? (Resources.GreenLight) : (Resources.redButton);
            HomeZ.Image = (!ClassMotion.CheckServoStatus(4, AxisZ)) ? (Resources.GreenLight) : (Resources.redButton);
            Positivelimit_Z.Image = (!ClassMotion.CheckServoStatus(1, AxisZ)) ? (Resources.GreenLight) : (Resources.redButton);
            Negtivelimit_Z.Image = (!ClassMotion.CheckServoStatus(2, AxisZ)) ? (Resources.GreenLight) : (Resources.redButton);
            ErrorZ.Image = (!ClassMotion.CheckServoStatus(0, AxisZ)) ? (Resources.GreenLight) : (Resources.redButton);
        }
        private void SingleAxisPara(ushort Axis)
        {
            //SingleAxisSet();
            tbSingleAxisPosition_Now.Text = ClassMotion.GetPosition(Axis).ToString();
            SingleHome.Image = (!ClassMotion.CheckServoStatus(4, Axis)) ? (Resources.GreenLight) : (Resources.redButton);
            SinglePositivelimit.Image = (!ClassMotion.CheckServoStatus(1, Axis)) ? (Resources.GreenLight) : (Resources.redButton);
            SingleNegtivelimit.Image = (!ClassMotion.CheckServoStatus(2, Axis)) ? (Resources.GreenLight) : (Resources.redButton);
            SingleError.Image = (!ClassMotion.CheckServoStatus(0, Axis)) ? (Resources.GreenLight) : (Resources.redButton);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case "工位1组合轴":
                    CombinedAxisPara(GlobalVar.Sation1XYZ[0], GlobalVar.Sation1XYZ[1], GlobalVar.Sation1XYZ[2]);
                    break;
                case "工位2组合轴":
                    CombinedAxisPara(GlobalVar.Sation2XYZ[0], GlobalVar.Sation2XYZ[1], GlobalVar.Sation2XYZ[2]);
                    break;
                case "工位3组合轴":
                    CombinedAxisPara(GlobalVar.Sation3XYZ[0], GlobalVar.Sation3XYZ[1], GlobalVar.Sation3XYZ[2]);
                    break;
                default:
                    break;
            }
            switch (cbSelect_OneSingleAxis.Text)
            {
                case "下料X":
                    SingleAxisPara(GlobalVar.SingleAxis[0]);
                    break;
                case "下料Z":
                    SingleAxisPara(GlobalVar.SingleAxis[1]);
                    break;
            }
            uint n1 = ClassMotion.ReadInput(1001);
            IOState("InPut10", n1,Input1);
            uint n2 = ClassMotion.ReadInput(1002);
            IOState("InPut20", n2, Input2);
            uint n3 = ClassMotion.ReadInput(1003);
            IOState("InPut30", n3, Input3);
            CommonIOState(Input4);
        }
        #region   IO输出
        private void OutPut1011_CheckedChanged(object sender, EventArgs e)    //1#IO卡 IO控制
        {
            foreach (CheckBox _CheckBox in OutPut1.Controls)
            {
                if (_CheckBox == sender)
                {
                    string name = _CheckBox.Name.ToString();
                    ushort Number = ushort.Parse(name.Substring(8, 2));
                    if (_CheckBox.Checked)
                    {
                        ClassMotion.SetOutPut(1001, (ushort)(Number - 11));
                    }
                    else
                    {
                        ClassMotion.ResetOutPut(1001, (ushort)(Number - 11));
                    }
                    break;
                }
            }
        }

        private void OutPut1027_CheckedChanged(object sender, EventArgs e)  //1#IO卡 气缸 控制
        {
            foreach (CheckBox _CheckBox in OutPut1.Controls)
            {
                if (_CheckBox == sender)
                {
                    string name = _CheckBox.Name.ToString();
                    ushort Number = ushort.Parse(name.Substring(8, 2));
                    if (!_CheckBox.Checked)
                    {
                        ClassMotion.SetOutPut(1001, (ushort)(Number - 11));
                        ClassMotion.ResetOutPut(1001, (ushort)(Number - 10));
                    }
                    else
                    {
                        ClassMotion.SetOutPut(1001, (ushort)(Number - 10));
                        ClassMotion.ResetOutPut(1001, (ushort)(Number - 11));
                    }
                    break;
                }
            }
        }

        private void OutPut2011_CheckedChanged(object sender, EventArgs e)  //2#IO卡 气缸 控制
        {
            foreach (CheckBox _CheckBox in OutPut2.Controls)
            {
                if (_CheckBox == sender)
                {
                    string name = _CheckBox.Name.ToString();
                    ushort Number = ushort.Parse(name.Substring(8, 2));
                    if (!_CheckBox.Checked)
                    {
                        ClassMotion.SetOutPut(1002, (ushort)(Number - 11));
                        ClassMotion.ResetOutPut(1002, (ushort)(Number - 10));
                    }
                    else
                    {
                        ClassMotion.SetOutPut(1002, (ushort)(Number - 10));
                        ClassMotion.ResetOutPut(1002, (ushort)(Number - 11));
                    }
                    break;
                }
            }
        }

        private void OutPut3011_CheckedChanged(object sender, EventArgs e) //3#IO卡 IO控制
        {
            foreach (CheckBox _CheckBox in OutPut3.Controls)
            {
                if (_CheckBox == sender)
                {
                    string name = _CheckBox.Name.ToString();
                    ushort Number = ushort.Parse(name.Substring(8, 2));
                    if (_CheckBox.Checked)
                    {
                        ClassMotion.SetOutPut(1003, (ushort)(Number - 11));
                    }
                    else
                    {
                        ClassMotion.ResetOutPut(1003, (ushort)(Number - 11));
                    }
                    break;
                }
            }
        }
        #endregion
        #region 输入
        private CheckBox GetINCheckBox(string name, int index,GroupBox Input)
        {
            string _name = name + (index + 11).ToString();
            foreach (CheckBox _CheckBox in Input.Controls)
            {
                if (_CheckBox != null && _CheckBox.Name == _name)
                {
                    return _CheckBox;
                }
            }
            return null;
        }
        private void SetCheckBox(CheckBox _CheckBox, bool status)
        {
            if (_CheckBox != null)
            {
                if (status)
                {
                    _CheckBox.Checked = false;
                }
                else
                {
                    _CheckBox.Checked = true;
                }
            }
        }
        public void IOState(string name, uint n,GroupBox Input)
        {
            for (int i = 0; i < 32; i++)
            {
                CheckBox _CheckBox = GetINCheckBox(name, i, Input);
                if (_CheckBox != null)
                {
                    SetCheckBox(_CheckBox, (n & 1) == 1);
                }
                n = n >> 1;
            }
        }

        public void CommonIOState(GroupBox Input)
        {
            for (int i = 0; i < 32; i++)
            {
                CheckBox _CheckBox = GetINCheckBox("InPut40", i, Input);
                if (_CheckBox != null)
                {
                    SetCheckBox(_CheckBox, ClassMotion.ReadCommonInput((ushort)i)==1);
                }
            }
        }

        #endregion
        #region 轴操作
        private void cbSelect_OneCombinedAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            Button[] buttonAxis ={btMaualCombinedAxisMovePositive_X,btMaualCombinedAxisMoveNegative_X,btCombinedAxisGoHome_X
                                       ,btMaualCombinedAxisMovePositive_Z1,btMaualCombinedAxisMoveNegative_Z1
                                       ,btCombinedAxisGoHome_Z1,XEableOpen,XEableClose,Z1EableOpen,Z1EableClose};
            TextBox[] textboxAxis ={tbCombinedAxisStepDistance_X,tbCombinedAxisPosition_NowX,tbCombinedAxisSavePosition_X
                                   ,tbCombinedAxisStepDistance_Z1,tbCombinedAxisPosition_NowZ1,tbCombinedAxisSavePosition_Z1};
            Label[] lablelistAxis = { HomeX, Negtivelimit_X, Positivelimit_X, ErrorX, HomeY, Negtivelimit_Y, Positivelimit_Y, ErrorY, HomeZ, Negtivelimit_Z, Positivelimit_Z, ErrorZ };
        }

        private void btMaualCombinedAxisMoveNegative_X_MouseDown(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[0], 0, GlobalVar.Station1AxisXParameter[0], GlobalVar.Station1AxisXParameter[1]
                    , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation1XYZ[0], 0);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[0], 0, GlobalVar.Station1AxisXParameter[0], GlobalVar.Station1AxisXParameter[1]
                    , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation1XYZ[0], (-1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case GlobalVar.Station2Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[0], 0, GlobalVar.Station2AxisXParameter[0], GlobalVar.Station2AxisXParameter[1]
                    , GlobalVar.Station2AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation2XYZ[0], 1);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[0], 0, GlobalVar.Station2AxisXParameter[0], GlobalVar.Station2AxisXParameter[1]
                    , GlobalVar.Station2AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation2XYZ[0], (1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case GlobalVar.Station3Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[0], 0, GlobalVar.Station3AxisXParameter[0], GlobalVar.Station3AxisXParameter[1]
                    , GlobalVar.Station3AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation3XYZ[0], 0);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[0], 0, GlobalVar.Station3AxisXParameter[0], GlobalVar.Station3AxisXParameter[1]
                    , GlobalVar.Station3AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation3XYZ[0], (-1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMoveNegative_X_MouseUp(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation1XYZ[0]);
                    }

                    break;
                case GlobalVar.Station2Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation2XYZ[0]);
                    }

                    break;
                case GlobalVar.Station3Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation3XYZ[0]);
                    }

                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMovePositive_X_MouseDown(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[0], 0, GlobalVar.Station1AxisXParameter[0], GlobalVar.Station1AxisXParameter[1]
                    , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation1XYZ[0], 1);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[0], 0, GlobalVar.Station1AxisXParameter[0], GlobalVar.Station1AxisXParameter[1]
                    , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation1XYZ[0], (1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case GlobalVar.Station2Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[0], 0, GlobalVar.Station2AxisXParameter[0], GlobalVar.Station2AxisXParameter[1]
                    , GlobalVar.Station2AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation2XYZ[0], 0);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[0], 0, GlobalVar.Station2AxisXParameter[0], GlobalVar.Station2AxisXParameter[1]
                    , GlobalVar.Station2AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation2XYZ[0], (-1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case GlobalVar.Station3Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[0], 0, GlobalVar.Station3AxisXParameter[0], GlobalVar.Station3AxisXParameter[1]
                    , GlobalVar.Station3AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation3XYZ[0], 1);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[0], 0, GlobalVar.Station3AxisXParameter[0], GlobalVar.Station3AxisXParameter[1]
                    , GlobalVar.Station3AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation3XYZ[0], (1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMovePositive_X_MouseUp(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation1XYZ[0]);
                    }

                    break;
                case GlobalVar.Station2Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation2XYZ[0]);
                    }

                    break;
                case GlobalVar.Station3Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation3XYZ[0]);
                    }

                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMovePositive_Y_MouseDown(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[1], 0, GlobalVar.Station1AxisXParameter[0], GlobalVar.Station1AxisXParameter[1]
                    , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation1XYZ[1], 1);

                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[1], 0, GlobalVar.Station1AxisXParameter[0], GlobalVar.Station1AxisXParameter[1]
                    , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation1XYZ[1], (1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case GlobalVar.Station2Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[1], 0, GlobalVar.Station2AxisXParameter[0], GlobalVar.Station2AxisXParameter[1]
                    , GlobalVar.Station2AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation2XYZ[1], 1);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[1], 0, GlobalVar.Station2AxisXParameter[0], GlobalVar.Station2AxisXParameter[1]
                    , GlobalVar.Station2AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation2XYZ[1], (1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case GlobalVar.Station3Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[1], 0, GlobalVar.Station3AxisXParameter[0], GlobalVar.Station3AxisXParameter[1]
                    , GlobalVar.Station3AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation3XYZ[1], 1);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[1], 0, GlobalVar.Station3AxisXParameter[0], GlobalVar.Station3AxisXParameter[1]
                    , GlobalVar.Station3AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation3XYZ[1], (1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }

                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMovePositive_Y_MouseUp(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation1XYZ[1]);
                    }

                    break;
                case GlobalVar.Station2Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation2XYZ[1]);
                    }

                    break;
                case GlobalVar.Station3Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation3XYZ[1]);
                    }

                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMoveNegative_Y_MouseDown(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[1], 0, GlobalVar.Station1AxisXParameter[0], GlobalVar.Station1AxisXParameter[1]
                    , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation1XYZ[1], 0);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[1], 0, GlobalVar.Station1AxisXParameter[0], GlobalVar.Station1AxisXParameter[1]
                    , GlobalVar.Station1AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation1XYZ[1], (-1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case GlobalVar.Station2Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[1], 0, GlobalVar.Station2AxisXParameter[0], GlobalVar.Station2AxisXParameter[1]
                    , GlobalVar.Station2AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation2XYZ[1], 0);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[1], 0, GlobalVar.Station2AxisXParameter[0], GlobalVar.Station2AxisXParameter[1]
                    , GlobalVar.Station2AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation2XYZ[1], (-1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case GlobalVar.Station3Name:
                    if (Z_Axis_Safe(0, 1))
                    {
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[1], 0, GlobalVar.Station3AxisXParameter[0], GlobalVar.Station3AxisXParameter[1]
                    , GlobalVar.Station3AxisXParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation3XYZ[1], 0);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[1], 0, GlobalVar.Station3AxisXParameter[0], GlobalVar.Station3AxisXParameter[1]
                    , GlobalVar.Station3AxisXParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation3XYZ[1], (-1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                    }
                    else
                    {
                        MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMoveNegative_Y_MouseUp(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation1XYZ[1]);
                    }

                    break;
                case GlobalVar.Station2Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation2XYZ[1]);
                    }

                    break;
                case GlobalVar.Station3Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation3XYZ[1]);
                    }

                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMoveNegative_Z1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                  
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[2], 0, GlobalVar.Station1AxisZParameter[0], GlobalVar.Station1AxisZParameter[1]
                     , GlobalVar.Station1AxisZParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation1XYZ[2], 0);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[2], 0, GlobalVar.Station1AxisZParameter[0], GlobalVar.Station1AxisZParameter[1]
                     , GlobalVar.Station1AxisZParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation1XYZ[2], (-1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                   
                    break;
                case GlobalVar.Station2Name:
                   
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[2], 0, GlobalVar.Station2AxisZParameter[0], GlobalVar.Station2AxisZParameter[1]
                      , GlobalVar.Station2AxisZParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation2XYZ[2], 0);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[2], 0, GlobalVar.Station2AxisZParameter[0], GlobalVar.Station2AxisZParameter[1]
                      , GlobalVar.Station2AxisZParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation2XYZ[2], (-1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                  
                    break;
                case GlobalVar.Station3Name:
                    
                        if (rbSelect_CombinedAxisContinuousMove.Checked)
                        {

                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[2], 0, GlobalVar.Station3AxisZParameter[0], GlobalVar.Station3AxisZParameter[1]
                     , GlobalVar.Station3AxisZParameter[2], 0, 0.01);
                            ClassMotion.VMove(GlobalVar.Sation3XYZ[2], 0);
                        }
                        else
                        {
                            ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[2], 0, GlobalVar.Station3AxisZParameter[0], GlobalVar.Station3AxisZParameter[1]
                   , GlobalVar.Station3AxisZParameter[2], 0, 0.01);
                            ClassMotion.PMove(GlobalVar.Sation3XYZ[2], (-1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                        }
                   
                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMoveNegative_Z1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation1XYZ[2]);
                    }

                    break;
                case GlobalVar.Station2Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation2XYZ[2]);
                    }

                    break;
                case GlobalVar.Station3Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation3XYZ[2]);
                    }

                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMovePositive_Z1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[2], 0, GlobalVar.Station1AxisZParameter[0], GlobalVar.Station1AxisZParameter[1]
                , GlobalVar.Station1AxisZParameter[2], 0, 0.01);
                        ClassMotion.VMove(GlobalVar.Sation1XYZ[2], 1);
                    }
                    else
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation1XYZ[2], 0, GlobalVar.Station1AxisZParameter[0], GlobalVar.Station1AxisZParameter[1]
             , GlobalVar.Station1AxisZParameter[2], 0, 0.01);
                        ClassMotion.PMove(GlobalVar.Sation1XYZ[2], (1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                    }
                    break;
                case GlobalVar.Station2Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {

                        ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[2], 0, GlobalVar.Station2AxisZParameter[0], GlobalVar.Station2AxisZParameter[1]
             , GlobalVar.Station2AxisZParameter[2], 0, 0.01);
                        ClassMotion.VMove(GlobalVar.Sation2XYZ[2], 1);
                    }
                    else
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation2XYZ[2], 0, GlobalVar.Station2AxisZParameter[0], GlobalVar.Station2AxisZParameter[1]
             , GlobalVar.Station2AxisZParameter[2], 0, 0.01);
                        ClassMotion.PMove(GlobalVar.Sation2XYZ[2], (1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                    }
                    break;
                case GlobalVar.Station3Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {

                        ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[2], 0, GlobalVar.Station3AxisZParameter[0], GlobalVar.Station3AxisZParameter[1]
                                     , GlobalVar.Station3AxisZParameter[2], 0, 0.01);
                        ClassMotion.VMove(GlobalVar.Sation3XYZ[2], 1);
                    }
                    else
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.Sation3XYZ[2], 0, GlobalVar.Station3AxisZParameter[0], GlobalVar.Station3AxisZParameter[1]
                                      , GlobalVar.Station3AxisZParameter[2], 0, 0.01);
                        ClassMotion.PMove(GlobalVar.Sation3XYZ[2], (1) * double.Parse(tbCombinedAxisStepDistance_X.Text), 0);   //按相对位移  移动对应距离
                    }
                    break;
                default:
                    break;
            }
        }

        private void btMaualCombinedAxisMovePositive_Z1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation1XYZ[2]);
                    }

                    break;
                case GlobalVar.Station2Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation2XYZ[2]);
                    }

                    break;
                case GlobalVar.Station3Name:
                    if (rbSelect_CombinedAxisContinuousMove.Checked)
                    {
                        ClassMotion.StopAxis(GlobalVar.Sation3XYZ[2]);
                    }

                    break;
                default:
                    break;
            }
        }
        #endregion

        private void btSelect_CombinedAxisPointsSave_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认保存？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                switch (cbSelect_OneCombinedAxis.Text)
                {
                    case GlobalVar.Station1Name:
                        CDBhelper._Get_Instance().PointRemove(GlobalVar.Station1Name, cbSelect_CombinedAxisPoints.Text);
                        Thread.Sleep(500);
                        DialogResult result1 = MessageBox.Show("再次确认是否保存" + GlobalVar.Station1Name + cbSelect_CombinedAxisPoints.Text + "点？", "提示", MessageBoxButtons.YesNo);
                        if (result1 == DialogResult.Yes)
                        {
                            CDBhelper._Get_Instance().PointAdd(GlobalVar.Station1Name, cbSelect_CombinedAxisPoints.Text, tbCombinedAxisPosition_NowX.Text, tbCombinedAxisPosition_NowY.Text, tbCombinedAxisPosition_NowZ1.Text);
                        }
                        break;
                    case GlobalVar.Station2Name:
                        CDBhelper._Get_Instance().PointRemove(GlobalVar.Station2Name, cbSelect_CombinedAxisPoints.Text);
                        Thread.Sleep(500);
                        DialogResult result2 = MessageBox.Show("再次确认是否保存" + GlobalVar.Station2Name + cbSelect_CombinedAxisPoints.Text + "点？", "提示", MessageBoxButtons.YesNo);
                        if (result2 == DialogResult.Yes)
                        {
                            CDBhelper._Get_Instance().PointAdd(GlobalVar.Station2Name, cbSelect_CombinedAxisPoints.Text, tbCombinedAxisPosition_NowX.Text, tbCombinedAxisPosition_NowY.Text, tbCombinedAxisPosition_NowZ1.Text);
                        }
                        break;
                    case GlobalVar.Station3Name:
                        CDBhelper._Get_Instance().PointRemove(GlobalVar.Station3Name, cbSelect_CombinedAxisPoints.Text);
                        Thread.Sleep(500);
                        DialogResult result3 = MessageBox.Show("再次确认是否保存" + GlobalVar.Station3Name + cbSelect_CombinedAxisPoints.Text + "点？", "提示", MessageBoxButtons.YesNo);
                        if (result3 == DialogResult.Yes)
                        {
                            CDBhelper._Get_Instance().PointAdd(GlobalVar.Station3Name, cbSelect_CombinedAxisPoints.Text, tbCombinedAxisPosition_NowX.Text, tbCombinedAxisPosition_NowY.Text, tbCombinedAxisPosition_NowZ1.Text);
                        }
                        break;
                    default:
                        break;

                }
                DisplaySavePoints();
            }
        }

        private void XEableOpen_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    ClassMotion.EnableAxis(GlobalVar.Sation1XYZ[0]);
                    break;
                case GlobalVar.Station2Name:
                    ClassMotion.EnableAxis(GlobalVar.Sation2XYZ[0]);
                    break;
                case GlobalVar.Station3Name:
                    ClassMotion.EnableAxis(GlobalVar.Sation3XYZ[0]);
                    break;
                default:
                    break;
            }
           
        }

        private void XEableClose_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    ClassMotion.DisableAxis(GlobalVar.Sation1XYZ[0]);
                    break;
                case GlobalVar.Station2Name:
                    ClassMotion.DisableAxis(GlobalVar.Sation2XYZ[0]);
                    break;
                case GlobalVar.Station3Name:
                    ClassMotion.DisableAxis(GlobalVar.Sation3XYZ[0]);
                    break;
                default:
                    break;
            }
        }

        private void YEableOpen_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    ClassMotion.EnableAxis(GlobalVar.Sation1XYZ[1]);
                    break;
                case GlobalVar.Station2Name:
                    ClassMotion.EnableAxis(GlobalVar.Sation2XYZ[1]);
                    break;
                case GlobalVar.Station3Name:
                    ClassMotion.EnableAxis(GlobalVar.Sation3XYZ[1]);
                    break;
                default:
                    break;
            }
        }

        private void YEableClose_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    ClassMotion.DisableAxis(GlobalVar.Sation1XYZ[1]);
                    break;
                case GlobalVar.Station2Name:
                    ClassMotion.DisableAxis(GlobalVar.Sation2XYZ[1]);
                    break;
                case GlobalVar.Station3Name:
                    ClassMotion.DisableAxis(GlobalVar.Sation3XYZ[1]);
                    break;
                default:
                    break;
            }
        }

        private void Z1EableOpen_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    ClassMotion.EnableAxis(GlobalVar.Sation1XYZ[2]);
                    break;
                case GlobalVar.Station2Name:
                    ClassMotion.EnableAxis(GlobalVar.Sation2XYZ[2]);
                    break;
                case GlobalVar.Station3Name:
                    ClassMotion.EnableAxis(GlobalVar.Sation3XYZ[2]);
                    break;
                default:
                    break;
            }
        }

        private void Z1EableClose_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    ClassMotion.DisableAxis(GlobalVar.Sation1XYZ[2]);
                    break;
                case GlobalVar.Station2Name:
                    ClassMotion.DisableAxis(GlobalVar.Sation2XYZ[2]);
                    break;
                case GlobalVar.Station3Name:
                    ClassMotion.DisableAxis(GlobalVar.Sation3XYZ[2]);
                    break;
                default:
                    break;
            }
        }

        private void btCombinedAxisGoHome_X_Click(object sender, EventArgs e)
        {
            if (Z_Axis_Safe(0, 1))
            {
                switch (cbSelect_OneCombinedAxis.Text)
                {
                    case GlobalVar.Station1Name:
                        ClassMotion.Home(GlobalVar.Sation1XYZ[0]);
                        break;
                    case GlobalVar.Station2Name:
                        ClassMotion.Home(GlobalVar.Sation2XYZ[0]);
                        break;
                    case GlobalVar.Station3Name:
                        ClassMotion.Home(GlobalVar.Sation3XYZ[0]);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btCombinedAxisGoHome_Y_Click(object sender, EventArgs e)
        {
            if (Z_Axis_Safe(0, 1))
            {
                switch (cbSelect_OneCombinedAxis.Text)
                {
                    case GlobalVar.Station1Name:
                        ClassMotion.Home(GlobalVar.Sation1XYZ[1]);
                        break;
                    case GlobalVar.Station2Name:
                        ClassMotion.Home(GlobalVar.Sation2XYZ[1]);
                        break;
                    case GlobalVar.Station3Name:
                        ClassMotion.Home(GlobalVar.Sation3XYZ[1]);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Z轴不在安全位置，不允许动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btCombinedAxisGoHome_Z1_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    ClassMotion.Home(GlobalVar.Sation1XYZ[2]);
                    break;
                case GlobalVar.Station2Name:
                    ClassMotion.Home(GlobalVar.Sation2XYZ[2]);
                    break;
                case GlobalVar.Station3Name:
                    ClassMotion.Home(GlobalVar.Sation3XYZ[2]);
                    break;
                default:
                    break;
            }
        }

        private void SaveSpeed_Click(object sender, EventArgs e)
        {
            #region 组合轴速度参数
            switch (comboBoxSpeed.Text)
            {
                case GlobalVar.Station1Name:
                    FileOp.SaveDataToXMLDPRP("Parameter", "XAxis", GlobalVar.Station1Name + "速度", textBox1010.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "XAxis", GlobalVar.Station1Name + "加速时间", textBox1011.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "XAxis", GlobalVar.Station1Name + "减速时间", textBox1012.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "YAxis", GlobalVar.Station1Name + "速度", textBox1013.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "YAxis", GlobalVar.Station1Name + "加速时间", textBox1014.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "YAxis", GlobalVar.Station1Name + "减速时间", textBox1015.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "ZAxis", GlobalVar.Station1Name + "速度", textBox1016.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "ZAxis", GlobalVar.Station1Name + "加速时间", textBox1017.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "ZAxis", GlobalVar.Station1Name + "减速时间", textBox1018.Text);
                    GlobalVar.Station1AxisXParameter[0] = double.Parse(textBox1010.Text);
                    GlobalVar.Station1AxisXParameter[1] = double.Parse(textBox1011.Text);
                    GlobalVar.Station1AxisXParameter[2] = double.Parse(textBox1012.Text);
                    GlobalVar.Station1AxisYParameter[0] = double.Parse(textBox1013.Text);
                    GlobalVar.Station1AxisYParameter[1] = double.Parse(textBox1014.Text);
                    GlobalVar.Station1AxisYParameter[2] = double.Parse(textBox1015.Text);
                    GlobalVar.Station1AxisZParameter[0] = double.Parse(textBox1016.Text);
                    GlobalVar.Station1AxisZParameter[1] = double.Parse(textBox1017.Text);
                    GlobalVar.Station1AxisZParameter[2] = double.Parse(textBox1018.Text);
                    break;
                case GlobalVar.Station2Name:
                    FileOp.SaveDataToXMLDPRP("Parameter", "XAxis", GlobalVar.Station2Name + "速度", textBox1010.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "XAxis", GlobalVar.Station2Name + "加速时间", textBox1011.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "XAxis", GlobalVar.Station2Name + "减速时间", textBox1012.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "YAxis", GlobalVar.Station2Name + "速度", textBox1013.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "YAxis", GlobalVar.Station2Name + "加速时间", textBox1014.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "YAxis", GlobalVar.Station2Name + "减速时间", textBox1015.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "ZAxis", GlobalVar.Station2Name + "速度", textBox1016.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "ZAxis", GlobalVar.Station2Name + "加速时间", textBox1017.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "ZAxis", GlobalVar.Station2Name + "减速时间", textBox1018.Text);
                    GlobalVar.Station2AxisXParameter[0] = double.Parse(textBox1010.Text);
                    GlobalVar.Station2AxisXParameter[1] = double.Parse(textBox1011.Text);
                    GlobalVar.Station2AxisXParameter[2] = double.Parse(textBox1012.Text);
                    GlobalVar.Station2AxisYParameter[0] = double.Parse(textBox1013.Text);
                    GlobalVar.Station2AxisYParameter[1] = double.Parse(textBox1014.Text);
                    GlobalVar.Station2AxisYParameter[2] = double.Parse(textBox1015.Text);
                    GlobalVar.Station2AxisZParameter[0] = double.Parse(textBox1016.Text);
                    GlobalVar.Station2AxisZParameter[1] = double.Parse(textBox1017.Text);
                    GlobalVar.Station2AxisZParameter[2] = double.Parse(textBox1018.Text);
                    break;
                case GlobalVar.Station3Name:
                    FileOp.SaveDataToXMLDPRP("Parameter", "XAxis", GlobalVar.Station3Name + "速度", textBox1010.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "XAxis", GlobalVar.Station3Name + "加速时间", textBox1011.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "XAxis", GlobalVar.Station3Name + "减速时间", textBox1012.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "YAxis", GlobalVar.Station3Name + "速度", textBox1013.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "YAxis", GlobalVar.Station3Name + "加速时间", textBox1014.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "YAxis", GlobalVar.Station3Name + "减速时间", textBox1015.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "ZAxis", GlobalVar.Station3Name + "速度", textBox1016.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "ZAxis", GlobalVar.Station3Name + "加速时间", textBox1017.Text);
                    FileOp.SaveDataToXMLDPRP("Parameter", "ZAxis", GlobalVar.Station3Name + "减速时间", textBox1018.Text);
                    GlobalVar.Station3AxisXParameter[0] = double.Parse(textBox1010.Text);
                    GlobalVar.Station3AxisXParameter[1] = double.Parse(textBox1011.Text);
                    GlobalVar.Station3AxisXParameter[2] = double.Parse(textBox1012.Text);
                    GlobalVar.Station3AxisYParameter[0] = double.Parse(textBox1013.Text);
                    GlobalVar.Station3AxisYParameter[1] = double.Parse(textBox1014.Text);
                    GlobalVar.Station3AxisYParameter[2] = double.Parse(textBox1015.Text);
                    GlobalVar.Station3AxisZParameter[0] = double.Parse(textBox1016.Text);
                    GlobalVar.Station3AxisZParameter[1] = double.Parse(textBox1017.Text);
                    GlobalVar.Station3AxisZParameter[2] = double.Parse(textBox1018.Text);
                    break;
                default:
                    break;
            }
            #endregion
            #region
            FileOp.SaveDataToXMLDPRP("Parameter", "SingleXAxis", GlobalVar.SingleXName + "速度", textBox2010.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "SingleXAxis", GlobalVar.SingleXName + "加速时间", textBox2011.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "SingleXAxis", GlobalVar.SingleXName + "减速时间", textBox2012.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "SingleZAxis", GlobalVar.SingleZName + "速度", textBox2013.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "SingleZAxis", GlobalVar.SingleZName + "加速时间", textBox2014.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "SingleZAxis", GlobalVar.SingleZName + "减速时间", textBox2015.Text);
            GlobalVar.SingleAxisXParameter[0] = double.Parse(textBox2010.Text);
            GlobalVar.SingleAxisXParameter[1] = double.Parse(textBox2011.Text);
            GlobalVar.SingleAxisXParameter[2] = double.Parse(textBox2012.Text);
            GlobalVar.SingleAxisZParameter[0] = double.Parse(textBox2013.Text);
            GlobalVar.SingleAxisZParameter[1] = double.Parse(textBox2014.Text);
            GlobalVar.SingleAxisZParameter[2] = double.Parse(textBox2015.Text);


            FileOp.SaveDataToXMLDPRP("Parameter", "Conveyor", GlobalVar.Station1Name + "速度", textBox3010.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "Conveyor", GlobalVar.Station1Name + "加速时间", textBox3011.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "Conveyor", GlobalVar.Station1Name + "减速时间", textBox3012.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "Conveyor", GlobalVar.Station2Name + "速度", textBox3013.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "Conveyor", GlobalVar.Station2Name + "加速时间", textBox3014.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "Conveyor", GlobalVar.Station2Name + "减速时间", textBox3015.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "Conveyor", GlobalVar.Station3Name + "速度", textBox3016.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "Conveyor", GlobalVar.Station3Name + "加速时间", textBox3017.Text);
            FileOp.SaveDataToXMLDPRP("Parameter", "Conveyor", GlobalVar.Station3Name + "减速时间", textBox3018.Text);
            GlobalVar.Station1ConveyorParameter[0] = double.Parse(textBox3010.Text);
            GlobalVar.Station1ConveyorParameter[1] = double.Parse(textBox3011.Text);
            GlobalVar.Station1ConveyorParameter[2] = double.Parse(textBox3012.Text);
            GlobalVar.Station2ConveyorParameter[0] = double.Parse(textBox3013.Text);
            GlobalVar.Station2ConveyorParameter[1] = double.Parse(textBox3014.Text);
            GlobalVar.Station2ConveyorParameter[2] = double.Parse(textBox3015.Text);
            GlobalVar.Station3ConveyorParameter[0] = double.Parse(textBox3016.Text);
            GlobalVar.Station3ConveyorParameter[1] = double.Parse(textBox3017.Text);
            GlobalVar.Station3ConveyorParameter[2] = double.Parse(textBox3018.Text);

            #endregion
            MessageBox.Show("保存成功");
        }

        private void textBox1010_TextChanged(object sender, EventArgs e)
        {
            foreach (TextBox _TextBox in panelSpeed.Controls)
            {
                if (_TextBox == sender)
                {
                    try
                    {
                        double Para = double.Parse(_TextBox.Text);
                    }
                    catch
                    {
                        _TextBox.Text = "0";
                        MessageBox.Show("请输入数字");
                        Thread.Sleep(100);
                    }
                }
            }
        }

        private void btSingleAxisForward_FeedingConveyor_Click(object sender, EventArgs e)
        {
            ClassMotion.StopAxis(GlobalVar.Conveyor[0]);
            ClassMotion.PmoveSetAxisParameter(GlobalVar.Conveyor[0], 0, GlobalVar.Station1ConveyorParameter[0], GlobalVar.Station1ConveyorParameter[1]
                  , GlobalVar.Station1ConveyorParameter[2], 0, 0.01);
            ClassMotion.VMove(GlobalVar.Conveyor[0], 1);
        }

        private void btSingleAxisReversal_FeedingConveyor_Click(object sender, EventArgs e)
        {
            ClassMotion.StopAxis(GlobalVar.Conveyor[0]);
            ClassMotion.PmoveSetAxisParameter(GlobalVar.Conveyor[0], 0, GlobalVar.Station1ConveyorParameter[0], GlobalVar.Station1ConveyorParameter[1]
                  , GlobalVar.Station1ConveyorParameter[2], 0, 0.01);
            ClassMotion.VMove(GlobalVar.Conveyor[0], 0);
        }

        private void btSingleAxisStop_FeedingConveyor_Click(object sender, EventArgs e)
        {
            ClassMotion.StopAxis(GlobalVar.Conveyor[0]);
        }

        private void btSingleAxisForward_Working1UpperConveyor_Click(object sender, EventArgs e)
        {
            ClassMotion.StopAxis(GlobalVar.Conveyor[1]);
            ClassMotion.PmoveSetAxisParameter(GlobalVar.Conveyor[1], 0, GlobalVar.Station2ConveyorParameter[0], GlobalVar.Station2ConveyorParameter[1]
                   , GlobalVar.Station2ConveyorParameter[2], 0, 0.01);
            ClassMotion.VMove(GlobalVar.Conveyor[1], 1);
        }

        private void btSingleAxisReversal_Working1UpperConveyor_Click(object sender, EventArgs e)
        {
            ClassMotion.StopAxis(GlobalVar.Conveyor[1]);
            ClassMotion.PmoveSetAxisParameter(GlobalVar.Conveyor[1], 0, GlobalVar.Station2ConveyorParameter[0], GlobalVar.Station2ConveyorParameter[1]
                              , GlobalVar.Station2ConveyorParameter[2], 0, 0.01);
            ClassMotion.VMove(GlobalVar.Conveyor[1], 0);
        }

        private void btSingleAxisStop_Working1UpperConveyor_Click(object sender, EventArgs e)
        {
            ClassMotion.StopAxis(GlobalVar.Conveyor[1]);
        }

        private void btSingleAxisForward_Working2UpperConveyor_Click(object sender, EventArgs e)
        {
            ClassMotion.StopAxis(GlobalVar.Conveyor[2]);
            ClassMotion.PmoveSetAxisParameter(GlobalVar.Conveyor[2], 0, GlobalVar.Station3ConveyorParameter[0], GlobalVar.Station3ConveyorParameter[1]
                  , GlobalVar.Station3ConveyorParameter[2], 0, 0.01);
            ClassMotion.VMove(GlobalVar.Conveyor[2], 1);
        }

        private void btSingleAxisReversal_Working2UpperConveyor_Click(object sender, EventArgs e)
        {
            ClassMotion.StopAxis(GlobalVar.Conveyor[2]);
            ClassMotion.PmoveSetAxisParameter(GlobalVar.Conveyor[2], 0, GlobalVar.Station3ConveyorParameter[0], GlobalVar.Station3ConveyorParameter[1]
                              , GlobalVar.Station3ConveyorParameter[2], 0, 0.01);
            ClassMotion.VMove(GlobalVar.Conveyor[2], 0);
        }

        private void btSingleAxisStop_Working2UpperConveyor_Click(object sender, EventArgs e)
        {
            ClassMotion.StopAxis(GlobalVar.Conveyor[2]);
        }

        private void btSelect_SingleAxisPointsGohome_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneSingleAxis.Text)
            {
                case GlobalVar.SingleXName:
                    ClassMotion.Home(GlobalVar.SingleAxis[0]);
                    break;
                case GlobalVar.SingleZName:
                    ClassMotion.Home(GlobalVar.SingleAxis[1]);
                    break;
            }
        }

        private void btMaualSingleAxisMove_Positive_MouseDown(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneSingleAxis.Text)
            {
                case GlobalVar.SingleXName:
                    if (rbSelect_SingleAxisContinuousMove.Checked)
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.SingleAxis[0], 0, GlobalVar.SingleAxisXParameter[0], GlobalVar.SingleAxisXParameter[1]
                     , GlobalVar.SingleAxisXParameter[2], 0, 0.01);
                        ClassMotion.VMove(GlobalVar.SingleAxis[0], 1);
                    }
                    else
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.SingleAxis[0], 0, GlobalVar.SingleAxisXParameter[0], GlobalVar.SingleAxisXParameter[1]
                     , GlobalVar.SingleAxisXParameter[2], 0, 0.01);
                        ClassMotion.PMove(GlobalVar.SingleAxis[0], (1) * double.Parse(tbSingleAxisStepDistance.Text), 0);   //按相对位移  移动对应距离
                    }
                    break;
                case GlobalVar.SingleZName:
                    if (rbSelect_SingleAxisContinuousMove.Checked)
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.SingleAxis[1], 0, GlobalVar.SingleAxisZParameter[0], GlobalVar.SingleAxisZParameter[1]
                   , GlobalVar.SingleAxisZParameter[2], 0, 0.01);
                        ClassMotion.VMove(GlobalVar.SingleAxis[1], 1);
                    }
                    else
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.SingleAxis[1], 0, GlobalVar.SingleAxisZParameter[0], GlobalVar.SingleAxisZParameter[1]
                   , GlobalVar.SingleAxisZParameter[2], 0, 0.01);
                        ClassMotion.PMove(GlobalVar.SingleAxis[1], (1) * double.Parse(tbSingleAxisStepDistance.Text), 0);   //按相对位移  移动对应距离
                    }
                    break;
            }
        }

        private void btMaualSingleAxisMove_Positive_MouseUp(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneSingleAxis.Text)
            {
                case GlobalVar.SingleXName:
                    ClassMotion.StopAxis(GlobalVar.SingleAxis[0]);
                    break;
                case GlobalVar.SingleZName:
                    ClassMotion.StopAxis(GlobalVar.SingleAxis[1]);
                    break;
            }
        }

        private void btMaualSingleAxisMove_Negative_MouseDown(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneSingleAxis.Text)
            {
                case GlobalVar.SingleXName:
                    if (rbSelect_SingleAxisContinuousMove.Checked)
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.SingleAxis[0], 0, GlobalVar.SingleAxisXParameter[0], GlobalVar.SingleAxisXParameter[1]
                    , GlobalVar.SingleAxisXParameter[2], 0, 0.01);
                        ClassMotion.VMove(GlobalVar.SingleAxis[0], 0);
                    }
                    else
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.SingleAxis[0], 0, GlobalVar.SingleAxisXParameter[0], GlobalVar.SingleAxisXParameter[1]
                   , GlobalVar.SingleAxisXParameter[2], 0, 0.01);
                        ClassMotion.PMove(GlobalVar.SingleAxis[0], (-1) * double.Parse(tbSingleAxisStepDistance.Text), 0);   //按相对位移  移动对应距离
                    }
                    break;
                case GlobalVar.SingleZName:
                    if (rbSelect_SingleAxisContinuousMove.Checked)
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.SingleAxis[1], 0, GlobalVar.SingleAxisZParameter[0], GlobalVar.SingleAxisZParameter[1]
                    , GlobalVar.SingleAxisZParameter[2], 0, 0.01);
                        ClassMotion.VMove(GlobalVar.SingleAxis[1], 0);
                    }
                    else
                    {
                        ClassMotion.PmoveSetAxisParameter(GlobalVar.SingleAxis[1], 0, GlobalVar.SingleAxisZParameter[0], GlobalVar.SingleAxisZParameter[1]
                    , GlobalVar.SingleAxisZParameter[2], 0, 0.01);
                        ClassMotion.PMove(GlobalVar.SingleAxis[1], (-1) * double.Parse(tbSingleAxisStepDistance.Text), 0);   //按相对位移  移动对应距离
                    }
                    break;
            }
        }

        private void btMaualSingleAxisMove_Negative_MouseUp(object sender, MouseEventArgs e)
        {
            switch (cbSelect_OneSingleAxis.Text)
            {
                case GlobalVar.SingleXName:
                    ClassMotion.StopAxis(GlobalVar.SingleAxis[0]);
                    break;
                case GlobalVar.SingleZName:
                    ClassMotion.StopAxis(GlobalVar.SingleAxis[1]);
                    break;
            }
        }
        public int _selectPoint;
        private void Select_SingleAxisPoints()
        {
            switch (cbSelect_SingleAxisPoints.Text)
            {
                case "P1":
                    _selectPoint = 1;
                    break;
                case "P2":
                    _selectPoint = 2;
                    break;
                case "P3":
                    _selectPoint = 3;
                    break;
                case "P4":
                    _selectPoint = 4;
                    break;
                case "P5":
                    _selectPoint = 5;
                    break;
                default:
                    break;

            }
        }
        private void btSelect_SingleAxisPointsSave_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认保存？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Select_SingleAxisPoints();
                switch (cbSelect_OneSingleAxis.Text)
                {
                    case GlobalVar.SingleXName:
                        DialogResult result9 = MessageBox.Show("确认保存" + cbSelect_OneSingleAxis.Text + "P" + _selectPoint.ToString() + "点" + "？", "提示", MessageBoxButtons.YesNo);
                        if (result9 == DialogResult.Yes)
                        {
                            try
                            {
                                GlobalVar.LayingOffX[_selectPoint - 1] = double.Parse(tbSingleAxisPosition_Now.Text);
                                FileOp.SaveDataToXMLDPRP("Parameter", "SingleX", "P" + _selectPoint.ToString(), tbSingleAxisPosition_Now.Text);
                            }
                            catch
                            {
                                MessageBox.Show("输入数据格式错误，请重新输入！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        break;
                    case GlobalVar.SingleZName:
                        DialogResult result10 = MessageBox.Show("确认保存" + cbSelect_OneSingleAxis.Text + "P" + _selectPoint.ToString() + "点" + "？", "提示", MessageBoxButtons.YesNo);
                        if (result10 == DialogResult.Yes)
                        {
                            try
                            {
                                GlobalVar.LayingOffZ[_selectPoint - 1] = double.Parse(tbSingleAxisPosition_Now.Text);
                                FileOp.SaveDataToXMLDPRP("Parameter", "SingleZ", "P" + _selectPoint.ToString(), tbSingleAxisPosition_Now.Text);
                            }
                            catch
                            {
                                MessageBox.Show("输入数据格式错误，请重新输入！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        break;
                }
                DisplaySaveSinglePoints();
            }
        }

        private void btSelect_SingleAxisPointsMove_Click(object sender, EventArgs e)
        {
            Select_SingleAxisPoints();
            switch (cbSelect_OneSingleAxis.Text)
            {
                case GlobalVar.SingleXName:
                    ClassMotion.PMove(GlobalVar.SingleAxis[0], GlobalVar.LayingOffX[_selectPoint], 1);
                    break;
                case GlobalVar.SingleZName:
                    ClassMotion.PMove(GlobalVar.SingleAxis[1], GlobalVar.LayingOffZ[_selectPoint], 1);
                    break;
            }
        }

        private void btMaualSingleAxisMove_Stop_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneSingleAxis.Text)
            {
                case GlobalVar.SingleXName:
                    ClassMotion.StopAxis(GlobalVar.SingleAxis[0]);
                    break;
                case GlobalVar.SingleZName:
                    ClassMotion.StopAxis(GlobalVar.SingleAxis[1]);
                    break;
            }
        }

        private void textBox2010_TextChanged(object sender, EventArgs e)
        {
            foreach (TextBox _TextBox in panelSingleSpeed.Controls)
            {
                if (_TextBox == sender)
                {
                    try
                    {
                        double Para = double.Parse(_TextBox.Text);
                    }
                    catch
                    {
                        _TextBox.Text = "0";
                        MessageBox.Show("请输入数字");
                        Thread.Sleep(100);
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("是否再三确认更改各轴速度/加速度比例？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    GlobalVar.SpeedAccPercent = (double)(numericUpDown_CombinedAxisSpeed.Value / 100);
                    FileOp.SaveDataToXMLDPRP("Parameter", "SpeedAccPercent", "SpeedAccPercent", GlobalVar.SpeedAccPercent.ToString());
                }
            }
            catch
            {
                MessageBox.Show("输入速度百分数格式错误，请重新输入！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void IOTest_Load(object sender, EventArgs e)
        {
            #region
            foreach (TextBox _TextBox in panelSingleSpeed.Controls)
            {
                int a = int.Parse(_TextBox.Name.Substring(10, 1));
                if (a >= 0 && a <= 2)
                {
                    _TextBox.Text = GlobalVar.SingleAxisXParameter[a].ToString();
                }
                else
                {
                    _TextBox.Text = GlobalVar.SingleAxisZParameter[a - 3].ToString();
                }
            }
            #endregion
            #region
            foreach (TextBox _TextBox in panelConveyor.Controls)
            {
                int a = int.Parse(_TextBox.Name.Substring(10, 1));
                if (a >= 0 && a <= 2)
                {
                    _TextBox.Text = GlobalVar.Station1ConveyorParameter[a].ToString();
                }
                else if (a >= 3 && a <= 5)
                {
                    _TextBox.Text = GlobalVar.Station2ConveyorParameter[a - 3].ToString();
                }
                else
                {
                    _TextBox.Text = GlobalVar.Station3ConveyorParameter[a - 6].ToString();
                }
            }
            #endregion
            numericUpDown_CombinedAxisSpeed.Value = (decimal)GlobalVar.SpeedAccPercent*100;
        }

        private void comboBoxSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 组合轴速度参数
            switch (comboBoxSpeed.Text)
            {
                case GlobalVar.Station1Name:
                    foreach (TextBox _TextBox in panelSpeed.Controls)
                    {
                        int a = int.Parse(_TextBox.Name.Substring(10, 1));
                        if (a >= 0 && a <= 2)
                        {
                            _TextBox.Text = GlobalVar.Station1AxisXParameter[a].ToString();
                        }
                        else if (a >= 3 && a <= 5)
                        {
                            _TextBox.Text = GlobalVar.Station1AxisYParameter[a-3].ToString();
                        }
                        else
                        {
                            _TextBox.Text = GlobalVar.Station1AxisZParameter[a-6].ToString();
                        }
                    }
                    break;
                case GlobalVar.Station2Name:
                    foreach (TextBox _TextBox in panelSpeed.Controls)
                    {
                        int a = int.Parse(_TextBox.Name.Substring(10, 1));
                        if (a >= 0 && a <= 2)
                        {
                            _TextBox.Text = GlobalVar.Station2AxisXParameter[a].ToString();
                        }
                        else if (a >= 3 && a <= 5)
                        {
                            _TextBox.Text = GlobalVar.Station2AxisYParameter[a - 3].ToString();
                        }
                        else
                        {
                            _TextBox.Text = GlobalVar.Station2AxisZParameter[a - 6].ToString();
                        }
                    }
                    break;
                case GlobalVar.Station3Name:
                    foreach (TextBox _TextBox in panelSpeed.Controls)
                    {
                        int a = int.Parse(_TextBox.Name.Substring(10, 1));
                        if (a >= 0 && a <= 2)
                        {
                            _TextBox.Text = GlobalVar.Station3AxisXParameter[a].ToString();
                        }
                        else if (a >= 3 && a <= 5)
                        {
                            _TextBox.Text = GlobalVar.Station3AxisYParameter[a - 3].ToString();
                        }
                        else
                        {
                            _TextBox.Text = GlobalVar.Station3AxisZParameter[a - 6].ToString();
                        }
                    }
                    break;
                default:
                    break;
            }
            #endregion
           

        }

        private void tbCombinedAxisStepDistance_X_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double Para = double.Parse(tbCombinedAxisStepDistance_X.Text);
            }
            catch
            {
                tbCombinedAxisStepDistance_X.Text = "0";
                MessageBox.Show("请输入数字");
                Thread.Sleep(100);
            }
        }

        private void tbCombinedAxisStepDistance_Y_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double Para = double.Parse(tbCombinedAxisStepDistance_Y.Text);
            }
            catch
            {
                tbCombinedAxisStepDistance_Y.Text = "0";
                MessageBox.Show("请输入数字");
                Thread.Sleep(100);
            }
        }

        private void tbCombinedAxisStepDistance_Z1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double Para = double.Parse(tbCombinedAxisStepDistance_Z1.Text);
            }
            catch
            {
                tbCombinedAxisStepDistance_Z1.Text = "0";
                MessageBox.Show("请输入数字");
                Thread.Sleep(100);
            }
        }

        private void OutPut3035_CheckedChanged(object sender, EventArgs e)
        {
            foreach (CheckBox _CheckBox in OutPut3.Controls)
            {
                if (_CheckBox == sender)
                {
                    string name = _CheckBox.Name.ToString();
                    ushort Number = ushort.Parse(name.Substring(8, 2));
                    if (_CheckBox.Checked)
                    {
                        ClassMotion.SetOutPut(1003, (ushort)(Number - 11));
                        ClassMotion.ResetOutPut(1003, (ushort)(Number - 10));
                    }
                    else
                    {
                        ClassMotion.SetOutPut(1003, (ushort)(Number - 10));
                        ClassMotion.ResetOutPut(1003, (ushort)(Number - 11));
                    }
                    break;
                }
            }
        }

        private void OutPut4011_CheckedChanged(object sender, EventArgs e)
        {
            foreach (CheckBox _CheckBox in OutPut4.Controls)
            {
                if (_CheckBox == sender)
                {
                    string name = _CheckBox.Name.ToString();
                    ushort Number = ushort.Parse(name.Substring(8, 2));
                    if (_CheckBox.Checked)
                    {
                        ClassMotion.SetCommonOutPut((ushort)(Number - 11));
                        ClassMotion.ReSetCommonOutPut((ushort)(Number - 10));
                    }
                    else
                    {
                        ClassMotion.SetCommonOutPut((ushort)(Number - 10));
                        ClassMotion.ReSetCommonOutPut((ushort)(Number - 11));
                    }
                    break;
                }
            }
        }


        public void startwork_AxisJumpWorker()
        {
            if (backgroundAxisMove.IsBusy != true)
            {
                backgroundAxisMove.RunWorkerAsync();
            }
        }

        public void stopwork_AxisJumpWorker()
        {
            if (backgroundAxisMove.IsBusy == true)
            {
                backgroundAxisMove.CancelAsync();
            }
        }
        private void backgroundAxisMove_DoWork(object sender, DoWorkEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认移动？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                
                Invoke(new Action(() =>
                {
                    if (cbSelect_CombinedAxisPoints.Text == "")
                    {
                        MessageBox.Show("请选择点位");
                    }
                    else
                    {
                        selectnum();
                        RobotPos robot = new RobotPos();
                        robot.PosX = double.Parse(tbCombinedAxisPosition_NowX.Text);
                        robot.PosY = double.Parse(tbCombinedAxisPosition_NowY.Text);
                        robot.PosZ1 = 0;
                        switch (cbSelect_OneCombinedAxis.Text)
                        {                             
                            case GlobalVar.Station1Name:
                                ClassMotion.SetVectorProfileUnit(GlobalVar.Sation1XYZ[2], 0, 0, GlobalVar.Station1AxisZParameter[0], GlobalVar.Station1AxisZParameter[1]
                                     , GlobalVar.Station1AxisZParameter[2], 0);
                                mMainstream.Station1LineMove(3, GlobalVar.Sation1XYZ, robot, 10, 1, Dis, true);
                                mMainstream.Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[_selectnum], 10, 1, Dis, true);
                                mMainstream.Station1LineMove(3, GlobalVar.Sation1XYZ, GlobalVar.Station1CombinedAxis[_selectnum], 10, 1, Dis, false);
                                break;
                            case GlobalVar.Station2Name:
                                ClassMotion.SetVectorProfileUnit(GlobalVar.Sation2XYZ[2], 1, 0, GlobalVar.Station2AxisZParameter[0], GlobalVar.Station2AxisZParameter[1]
                                     , GlobalVar.Station2AxisZParameter[2], 0);
                                mMainstream.Station2LineMove(3, GlobalVar.Sation2XYZ, robot, 10, 1, Dis, true);
                                mMainstream.Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[_selectnum], 10, 1, Dis, true);
                                mMainstream.Station2LineMove(3, GlobalVar.Sation2XYZ, GlobalVar.Station2CombinedAxis[_selectnum], 10, 1, Dis, false);
                                break;
                            case GlobalVar.Station3Name:
                                ClassMotion.SetVectorProfileUnit(GlobalVar.Sation3XYZ[2], 2, 0, GlobalVar.Station3AxisZParameter[0], GlobalVar.Station3AxisZParameter[1]
                                     , GlobalVar.Station3AxisZParameter[2], 0);
                                mMainstream.Station3LineMove(3, GlobalVar.Sation3XYZ, robot, 10, 1, Dis, true);
                                mMainstream.Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[_selectnum], 10, 1, Dis, true);
                                mMainstream.Station3LineMove(3, GlobalVar.Sation3XYZ, GlobalVar.Station3CombinedAxis[_selectnum], 10, 1, Dis, false);
                                break;
                            default:
                                break;

                        }
                    }
                }));
            }
        }

        private void btExecuteCombinedAxisModeMove_Click(object sender, EventArgs e)
        {
            startwork_AxisJumpWorker();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClassMotion.EnableAxis(GlobalVar.Conveyor[0]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClassMotion.DisableAxis(GlobalVar.Conveyor[0]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClassMotion.EnableAxis(GlobalVar.Conveyor[1]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClassMotion.DisableAxis(GlobalVar.Conveyor[1]);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClassMotion.EnableAxis(GlobalVar.Conveyor[2]);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClassMotion.DisableAxis(GlobalVar.Conveyor[2]);
        }

        private void btStopCombinedAxis_Move_Click(object sender, EventArgs e)
        {
            switch (cbSelect_OneCombinedAxis.Text)
            {
                case GlobalVar.Station1Name:
                    ClassMotion.Stop3Axis(GlobalVar.Sation1XYZ);
                    break;
                case GlobalVar.Station2Name:
                    ClassMotion.Stop3Axis(GlobalVar.Sation2XYZ);
                    break;
                case GlobalVar.Station3Name:
                    ClassMotion.Stop3Axis(GlobalVar.Sation3XYZ);
                    break;
                default:
                    break;
            }
           
        }

        private void cbSelect_SingleAxisPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySaveSinglePoints();
        }

        public bool Z_Axis_Safe(double SafePoint,double Offset)
        {
            bool Safe;
            double Z_Point = double.Parse(tbCombinedAxisPosition_NowZ1.Text);
            if (Z_Point < SafePoint + Offset && Z_Point > SafePoint - Offset)
            {
                Safe = true;
            }
            else
            {
                Safe = false;
            }
            return Safe;
        }
    }
}