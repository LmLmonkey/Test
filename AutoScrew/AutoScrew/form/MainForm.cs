using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AutoScrew
{
    public partial class Form1 : Form
    {
        
        //private Class_Motion ClassMotion;
        MainStream mMainstream = new MainStream();
        private IOTest IOAxis = null;
        public Form1()
        {
           // 
           
            InitializeComponent();
           
           // ClassMotion.OpenCard();      
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //设置自动换行  
            this.dataGridViewRunMessage.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //设置自动调整高度  
            this.dataGridViewRunMessage.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tabControlMain.Region = new Region(new RectangleF(UserPage.Left, UserPage.Top, UserPage.Width, UserPage.Height));
           
            DelegateAndAction();
            FileOp.CopyFolder();
            ReadXML();
            _Init();
            ReadPoint(GlobalVar.Station1Name);    //数据库中获取已保存的点位
            ReadPoint(GlobalVar.Station2Name);    //数据库中获取已保存的点位
            ReadPoint(GlobalVar.Station3Name);    //数据库中获取已保存的点位
            LableAdd();                           //电批数据展示Lable
            mMainstream.MachineInit();            //运动控制卡初始化
            StartALLThread();                     //启动线程
            CErrorMgr.Instance._InitErrDefine();  //加载ErrorCode
            GlobalVar.ConnectStatus = true;
            GlobalVar.Station2Screw3Location.Add(1);
            GlobalVar.Station2Screw3Location.Add(1);
            GlobalVar.Station2Screw3Location.Add(1);
            GlobalVar.Station2Screw4Location.Add(1);
            GlobalVar.Station2Screw4Location.Add(1);
            GlobalVar.Station2Screw4Location.Add(1);

            GlobalVar.Station3Screw5Location.Add(1);
            GlobalVar.Station3Screw5Location.Add(1);
            GlobalVar.Station3Screw5Location.Add(1);
            GlobalVar.Station3Screw6Location.Add(1);
            GlobalVar.Station3Screw6Location.Add(1);
            GlobalVar.Station3Screw6Location.Add(1);
        }

        public void StartALLThread()
        {
            startbackgroundEleScrew();//电批 串口  网口通信
            mMainstream.startStation1Work();
            mMainstream.startStation2Work();
            mMainstream.startStation3Work();
            mMainstream.startStation4Work();
            //mMainstream.startbackgroundSafeDoor();
            startbackgroundConnectStatu();
            startbackgroundUpDate();
            //startbackgroundTest();
        }

        #region 初始化
        public void _Init()
        {
            string ssDbPath = GlobalVar.ms_ssCfgPath + @"\Auto_Screw.db3";
            CDBhelper._Get_Instance()._Init(ssDbPath);
            CDBhelper._Get_Instance()._DeleteHistoryData();
            if (GlobalVar.Production)
            {
                Production.Checked = true;
            }
            else
            {
                DryRun.Checked = true;
            }
            if (GlobalVar.SafeDoorDisEnable)
            {
                checkBoxSafeDoor.Checked = true;
            }
            else
            {
                checkBoxSafeDoor.Checked = false;
            }
            if (GlobalVar.PDCADisEnable)
            {
                checkBoxPDCA.Checked = true;
            }
            else
            {
                checkBoxPDCA.Checked = false;
            }
            if (GlobalVar.BarCodeDisEnable)
            {
                checkBoxBarCode.Checked = true;
            }
            else
            {
                checkBoxBarCode.Checked = false;
            }
            if (GlobalVar.VisionDisEnable)
            {
                checkBoxVision.Checked = true;
            }
            else
            {
                checkBoxVision.Checked = false;
            }
            ERRCode.Add("IO信号");
            ERRCode.Add("气缸");
            ERRCode.Add("通信");
            ERRCode.Add("安全门");
            string[] ssDGVHeader2 = new string[] { "CodeID", "Message","Category", "StartTime","EndTime" };
            initdataGridView2(ssDGVHeader2,dataGridView2);
            string[] ssDGVHeader1 = new string[] { "SN", "CC_3", "CC_4", "CC_5", "CC_6", "ScrewCC_3", "ScrewCC_4", "ScrewCC_5", "ScrewCC_6", "Result" ,"StartTime"};
            initdataGridView2(ssDGVHeader1,dataGridView1);
        }
        private void initdataGridView2(string[] columnName,DataGridView Grid)
        {
            int count = columnName.Length;
            Grid.ColumnCount = count;
            for (int i = 0; i < count; i++)
            {
                string str = columnName[i];
                Grid.Columns[i].HeaderCell.Value = str;
                Grid.Columns[i].Width = (((int)(Grid.Width) - 45) / count);
            }
        }
        #endregion

        #region 获取已保存点位
        public SQLiteDataReader ReadSqlite(string tablename,string point)
        {
            SQLiteDataReader reader = null;
            reader = CDBhelper._Get_Instance().PointMessage(tablename,point);
            return reader;
        }
        public void ReadPoint(string tablename)
        {
            for (int i = 0; i < 12; i++)
            {
                SQLiteDataReader reader = ReadSqlite(tablename,"P"+(i+1).ToString());
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
        #endregion

        #region 按钮选项卡
        private void SelectPageItemFun(SelectPageItem Select)
        {
            switch (Select)
            {
                case SelectPageItem.None:
                    break;
                case SelectPageItem.lbMainPage:
                    tabControlMain.SelectedTab = MainPage;
                    break;
                case SelectPageItem.lbUserPage:
                    tabControlMain.SelectedTab = UserPage;
                    break;
                case SelectPageItem.lbDataPage:
                    tabControlMain.SelectedTab = DataPage;
                    break;
                case SelectPageItem.lbAlarmPage:
                    tabControlMain.SelectedTab = ErrorPage;
                    break;
            }
        }
        #endregion

        #region 读取配置文件
        private void ReadXML()
        {
            try
            {
                string str = "";
                #region 组合轴参数
                FileOp.ReadDataFromXMLRP("Parameter", "XAxis", GlobalVar.Station1Name + "速度", ref str);
                GlobalVar.Station1AxisXParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "XAxis", GlobalVar.Station1Name + "加速时间", ref str);
                GlobalVar.Station1AxisXParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "XAxis", GlobalVar.Station1Name + "减速时间", ref str);
                GlobalVar.Station1AxisXParameter[2] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "YAxis", GlobalVar.Station1Name + "速度", ref str);
                GlobalVar.Station1AxisYParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "YAxis", GlobalVar.Station1Name + "加速时间", ref str);
                GlobalVar.Station1AxisYParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "YAxis", GlobalVar.Station1Name + "减速时间", ref str);
                GlobalVar.Station1AxisYParameter[2] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "ZAxis", GlobalVar.Station1Name + "速度", ref str);
                GlobalVar.Station1AxisZParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "ZAxis", GlobalVar.Station1Name + "加速时间", ref str);
                GlobalVar.Station1AxisZParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "ZAxis", GlobalVar.Station1Name + "减速时间", ref str);
                GlobalVar.Station1AxisZParameter[2] = double.Parse(str);

                FileOp.ReadDataFromXMLRP("Parameter", "XAxis", GlobalVar.Station2Name + "速度", ref str);
                GlobalVar.Station2AxisXParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "XAxis", GlobalVar.Station2Name + "加速时间", ref str);
                GlobalVar.Station2AxisXParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "XAxis", GlobalVar.Station2Name + "减速时间", ref str);
                GlobalVar.Station2AxisXParameter[2] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "YAxis", GlobalVar.Station2Name + "速度", ref str);
                GlobalVar.Station2AxisYParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "YAxis", GlobalVar.Station2Name + "加速时间", ref str);
                GlobalVar.Station2AxisYParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "YAxis", GlobalVar.Station2Name + "减速时间", ref str);
                GlobalVar.Station2AxisYParameter[2] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "ZAxis", GlobalVar.Station2Name + "速度", ref str);
                GlobalVar.Station2AxisZParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "ZAxis", GlobalVar.Station2Name + "加速时间", ref str);
                GlobalVar.Station2AxisZParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "ZAxis", GlobalVar.Station2Name + "减速时间", ref str);
                GlobalVar.Station2AxisZParameter[2] = double.Parse(str);

                FileOp.ReadDataFromXMLRP("Parameter", "XAxis", GlobalVar.Station3Name + "速度", ref str);
                GlobalVar.Station3AxisXParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "XAxis", GlobalVar.Station3Name + "加速时间", ref str);
                GlobalVar.Station3AxisXParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "XAxis", GlobalVar.Station3Name + "减速时间", ref str);
                GlobalVar.Station3AxisXParameter[2] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "YAxis", GlobalVar.Station3Name + "速度", ref str);
                GlobalVar.Station3AxisYParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "YAxis", GlobalVar.Station3Name + "加速时间", ref str);
                GlobalVar.Station3AxisYParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "YAxis", GlobalVar.Station3Name + "减速时间", ref str);
                GlobalVar.Station3AxisYParameter[2] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "ZAxis", GlobalVar.Station3Name + "速度", ref str);
                GlobalVar.Station3AxisZParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "ZAxis", GlobalVar.Station3Name + "加速时间", ref str);
                GlobalVar.Station3AxisZParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "ZAxis", GlobalVar.Station3Name + "减速时间", ref str);
                GlobalVar.Station3AxisZParameter[2] = double.Parse(str);
                #endregion

                #region 单轴参数
                FileOp.ReadDataFromXMLRP("Parameter", "SingleXAxis", GlobalVar.SingleXName + "速度", ref str);
                GlobalVar.SingleAxisXParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleXAxis", GlobalVar.SingleXName + "加速时间", ref str);
                GlobalVar.SingleAxisXParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleXAxis", GlobalVar.SingleXName + "减速时间", ref str);
                GlobalVar.SingleAxisXParameter[2] = double.Parse(str);

                FileOp.ReadDataFromXMLRP("Parameter", "SingleZAxis", GlobalVar.SingleZName + "速度", ref str);
                GlobalVar.SingleAxisZParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleZAxis", GlobalVar.SingleZName + "加速时间", ref str);
                GlobalVar.SingleAxisZParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleZAxis", GlobalVar.SingleZName + "减速时间", ref str);
                GlobalVar.SingleAxisZParameter[2] = double.Parse(str);
                #endregion

                #region 皮带参数
                FileOp.ReadDataFromXMLRP("Parameter", "Conveyor", GlobalVar.Station1Name + "速度", ref str);
                GlobalVar.Station1ConveyorParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "Conveyor", GlobalVar.Station1Name + "加速时间", ref str);
                GlobalVar.Station1ConveyorParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "Conveyor", GlobalVar.Station1Name + "减速时间", ref str);
                GlobalVar.Station1ConveyorParameter[2] = double.Parse(str);

                FileOp.ReadDataFromXMLRP("Parameter", "Conveyor", GlobalVar.Station2Name + "速度", ref str);
                GlobalVar.Station2ConveyorParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "Conveyor", GlobalVar.Station2Name + "加速时间", ref str);
                GlobalVar.Station2ConveyorParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "Conveyor", GlobalVar.Station2Name + "减速时间", ref str);
                GlobalVar.Station2ConveyorParameter[2] = double.Parse(str);

                FileOp.ReadDataFromXMLRP("Parameter", "Conveyor", GlobalVar.Station3Name + "速度", ref str);
                GlobalVar.Station3ConveyorParameter[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "Conveyor", GlobalVar.Station3Name + "加速时间", ref str);
                GlobalVar.Station3ConveyorParameter[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "Conveyor", GlobalVar.Station3Name + "减速时间", ref str);
                GlobalVar.Station3ConveyorParameter[2] = double.Parse(str);
                #endregion

                #region 单轴点位
                FileOp.ReadDataFromXMLRP("Parameter", "SingleX", "P1", ref str);
                GlobalVar.LayingOffX[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleX", "P2", ref str);
                GlobalVar.LayingOffX[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleX", "P3", ref str);
                GlobalVar.LayingOffX[2] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleX", "P4", ref str);
                GlobalVar.LayingOffX[3] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleX", "P5", ref str);
                GlobalVar.LayingOffX[4] = double.Parse(str);

                FileOp.ReadDataFromXMLRP("Parameter", "SingleZ", "P1", ref str);
                GlobalVar.LayingOffZ[0] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleZ", "P2", ref str);
                GlobalVar.LayingOffZ[1] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleZ", "P3", ref str);
                GlobalVar.LayingOffZ[2] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleZ", "P4", ref str);
                GlobalVar.LayingOffZ[3] = double.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SingleZ", "P5", ref str);
                GlobalVar.LayingOffZ[4] = double.Parse(str);
                #endregion
                FileOp.ReadDataFromXMLRP("Parameter", "SpeedAccPercent", "SpeedAccPercent", ref str);
                GlobalVar.SpeedAccPercent = double.Parse(str);

                FileOp.ReadDataFromXMLRP("Parameter", "Production", "Production", ref str);
                GlobalVar.Production = bool.Parse(str);

                #region 屏蔽设置
                FileOp.ReadDataFromXMLRP("Parameter", "SystemSetting", "PDCADisEnable", ref str);
                GlobalVar.PDCADisEnable = bool.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SystemSetting", "BarCodeDisEnable", ref str);
                GlobalVar.BarCodeDisEnable = bool.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SystemSetting", "VisionDisEnable", ref str);
                GlobalVar.VisionDisEnable = bool.Parse(str);
                FileOp.ReadDataFromXMLRP("Parameter", "SystemSetting", "SafeDoorDisEnable", ref str);
                GlobalVar.SafeDoorDisEnable = bool.Parse(str);
                #endregion
            }
            catch
            {
                MessageBox.Show("XML文件读取出错");
            }
        }
        #endregion

        #region 点击事件
        private void label13_Click(object sender, EventArgs e)
        {
            if (GlobalVar.LoginStatus)
            {
                SelectPageItemFun(SelectPageItem.lbUserPage);
                GlobalVar.Instance.UpdateRunMessage("用户界面切换", LogName.runLog);
            }
            else
            {
                MessageBox.Show("请先登录", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {           
            if (GlobalVar.LoginStatus)
            {
                SelectPageItemFun(SelectPageItem.lbMainPage);
                GlobalVar.Instance.UpdateRunMessage("主界面切换", LogName.runLog);
            }
            else
            {
                MessageBox.Show("请先登录", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            if (GlobalVar.LoginStatus)
            {
                SelectPageItemFun(SelectPageItem.lbDataPage);
                GlobalVar.Instance.UpdateRunMessage("数据展示切换", LogName.runLog);
            }
            else
            {
                MessageBox.Show("请先登录", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {
            if (GlobalVar.LoginStatus)
            {
                SelectPageItemFun(SelectPageItem.lbAlarmPage);
                GlobalVar.Instance.UpdateRunMessage("报警展示切换", LogName.runLog);
            }
            else
            {
                MessageBox.Show("请先登录", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Form IO = Application.OpenForms["IOTest"];

            if (GlobalVar.LoginStatus)
            {
                if ((IO == null) || (IO.IsDisposed))
                {
                    IOAxis = new IOTest(mMainstream);
                    IOAxis.Owner = this;
                    IOAxis.Hide();
                    IOAxis.Show();
                }
                else
                {
                    IO.Activate();
                    IO.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                MessageBox.Show("请先登录", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void AdminMode_Click(object sender, EventArgs e)
        {
            foreach (Label _Label in Userpanel.Controls)
            {
                if (_Label == sender)
                {
                    string name = _Label.Name.ToString();
                    switch (name)
                    {
                        case "AdminMode":
                            AdminMode.Image = Properties.Resources.gray250;
                            btEngineeringMode.Image = Properties.Resources.purple250;
                            OperaporMode.Image = Properties.Resources.purple250;
                            GlobalVar.LevelForUser = 3;
                            break;
                        case "btEngineeringMode":
                            AdminMode.Image = Properties.Resources.purple250;
                            btEngineeringMode.Image = Properties.Resources.gray250;
                            OperaporMode.Image = Properties.Resources.purple250;
                            GlobalVar.LevelForUser = 2;
                            break;
                        case "OperaporMode":
                            AdminMode.Image = Properties.Resources.purple250;
                            btEngineeringMode.Image = Properties.Resources.purple250;
                            OperaporMode.Image = Properties.Resources.gray250;
                            GlobalVar.LevelForUser = 1;
                            break;
                    }
                    break;
                }
            }
        }
        private void label11_Click(object sender, EventArgs e)
        {
            if (GlobalVar.LoginStatus)
            {
                System.Diagnostics.Process.Start("explorer.exe", @"D:\软件安装包");
            }
            else
            {
                MessageBox.Show("请先登录", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void label12_Click(object sender, EventArgs e)
        {
            if (GlobalVar.LoginStatus)
            {
                System.Diagnostics.Process.Start("explorer.exe", @"D:\软件安装包");
            }
            else
            {
                MessageBox.Show("请先登录", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Login_Out_Click(object sender, EventArgs e)
        {
            if (GlobalVar.LevelForUser != 0)
            {
                int Level = 0;
                string PassWord = "";
                SQLiteDataReader reader = null;
                reader = CDBhelper._Get_Instance().User_Message(AccounttextBox.Text.Trim());
                while (reader.Read())
                {
                    PassWord = reader.GetValue(2).ToString();
                    Level = int.Parse(reader.GetValue(3).ToString());
                }
                if ((PasswordtextBox.Text.Trim() == PassWord) && ((Level == GlobalVar.LevelForUser) && (Level == 1 || Level == 2)))
                {

                    userManagePanel.Visible = false;
                    GlobalVar.Instance.UpdateRunMessage("用户<" + AccounttextBox.Text.Trim() + ">登入系统", LogName.runLog);
                    SelectPageItemFun(SelectPageItem.lbCamPage);
                    if (GlobalVar.LevelForUser == 2)
                    {
                        //LoginStartPage();
                        SelectPageItemFun(SelectPageItem.lbMainPage);
                    }
                    else if (GlobalVar.LevelForUser == 1)
                    {
                        // LoginStartPageForOP();
                        SelectPageItemFun(SelectPageItem.lbMainPage);
                    }
                    GlobalVar.LevelForUser = 0;
                    AdminMode.Image = Properties.Resources.purple250;
                    btEngineeringMode.Image = Properties.Resources.purple250;
                    OperaporMode.Image = Properties.Resources.purple250;
                    GlobalVar.LoginStatus = true;
                }
                else if ((PasswordtextBox.Text.Trim() == PassWord) && ((Level == 3) && (Level == GlobalVar.LevelForUser)))
                {
                    GlobalVar.LevelForUser = 0;
                    userManagePanel.Visible = true;
                    AdminMode.Image = Properties.Resources.purple250;
                    btEngineeringMode.Image = Properties.Resources.purple250;
                    OperaporMode.Image = Properties.Resources.purple250;
                }
                else
                {
                    MessageBox.Show("用户名或密码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("请选择登录模式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void AddUserbtn_Click(object sender, EventArgs e)
        {
            if (AddlevelNumTB.Text == "" || AddUserNameTB.Text == "" || AddPasswordTB.Text == "")
            {
                MessageBox.Show("不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SQLiteDataReader reader = null;
                string name = "";
                reader = CDBhelper._Get_Instance().User_Message(AddUserNameTB.Text.Trim());
                if (reader.Read())
                {
                    MessageBox.Show("用户名已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (int.Parse(AddlevelNumTB.Text.Trim()) == 1 || int.Parse(AddlevelNumTB.Text.Trim()) == 2 || int.Parse(AddlevelNumTB.Text.Trim()) == 3)
                    {
                        CDBhelper._Get_Instance().User_Add(AddUserNameTB.Text.Trim(), AddPasswordTB.Text.Trim(), AddlevelNumTB.Text.Trim());
                        GlobalVar.Instance.UpdateRunMessage("用户<" + AccounttextBox.Text.Trim() + ">添加新的账户<" + AddUserNameTB.Text.Trim() + ">", LogName.runLog);
                        MessageBox.Show("添加用户成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("只能添加等级1或者等级2的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认关闭？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                GlobalVar.ConnectStatus = false;
                //Thread.Sleep(500);
                stopbackgroundConnectStatu();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            LTDMC.dmc_board_close();
            System.Environment.Exit(0);
        }

        private void Production_CheckedChanged(object sender, EventArgs e)
        {
            if (Production.Checked)
            {
                GlobalVar.Production = true;
                FileOp.SaveDataToXMLDPRP("Parameter", "Production", "Production", "true");
            }
            else
            {
                GlobalVar.Production = false;
                FileOp.SaveDataToXMLDPRP("Parameter", "Production", "Production", "false");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EM_ErrID emDes = CErrorMgr.Instance.Get_ErrID(4, 0);
            CErrorMgr.Instance._Add_One(emDes);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EM_ErrID emDes = CErrorMgr.Instance.Get_ErrID(4, 0);
            CErrorMgr.Instance._Remove_One(emDes);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EM_ErrID emDes = CErrorMgr.Instance.Get_ErrID(1, 0);
            CErrorMgr.Instance._Add_One(emDes);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EM_ErrID emDes = CErrorMgr.Instance.Get_ErrID(1, 0);
            CErrorMgr.Instance._Remove_One(emDes);
        }

        private void checkBoxPDCA_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPDCA.Checked)
            {
                GlobalVar.PDCADisEnable = true;
                GlobalVar.Instance.UpdateRunMessage("选择屏蔽PDCA", LogName.runLog);
            }
            else
            {
                GlobalVar.PDCADisEnable = false;
                GlobalVar.Instance.UpdateRunMessage("选择取消屏蔽PDCA", LogName.runLog);
            }
            FileOp.SaveDataToXMLDPRP("Parameter", "SystemSetting", "PDCADisEnable", checkBoxPDCA.Checked.ToString());
        }

        private void checkBoxBarCode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBarCode.Checked)
            {
                GlobalVar.BarCodeDisEnable = true;
                GlobalVar.Instance.UpdateRunMessage("选择屏蔽扫码枪", LogName.runLog);
            }
            else
            {
                GlobalVar.BarCodeDisEnable = false;
                GlobalVar.Instance.UpdateRunMessage("选择取消屏蔽扫码枪", LogName.runLog);
            }
            FileOp.SaveDataToXMLDPRP("Parameter", "SystemSetting", "BarCodeDisEnable", checkBoxBarCode.Checked.ToString());
        }

        private void checkBoxVision_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxVision.Checked)
            {
                GlobalVar.VisionDisEnable = true;
                GlobalVar.Instance.UpdateRunMessage("选择屏蔽视觉", LogName.runLog);
            }
            else
            {
                GlobalVar.VisionDisEnable = false;
                GlobalVar.Instance.UpdateRunMessage("选择取消屏蔽视觉", LogName.runLog);
            }
            FileOp.SaveDataToXMLDPRP("Parameter", "SystemSetting", "VisionDisEnable", checkBoxVision.Checked.ToString());
        }

        private void checkBoxSafeDoor_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSafeDoor.Checked)
            {
                GlobalVar.SafeDoorDisEnable = true;
                GlobalVar.Instance.UpdateRunMessage("选择屏蔽安全门", LogName.runLog);
            }
            else
            {
                GlobalVar.SafeDoorDisEnable = false;
                GlobalVar.Instance.UpdateRunMessage("选择取消屏蔽安全门", LogName.runLog);
            }
            FileOp.SaveDataToXMLDPRP("Parameter", "SystemSetting", "SafeDoorDisEnable", checkBoxSafeDoor.Checked.ToString());
        }
        #endregion

        #region 初始化chart
        //public void _Init(Chart chart)
        //{
        //    #region Chart设置
        //    chart2.ChartAreas[0].AxisX.Interval = 10;
        //    chart2.ChartAreas[0].AxisX.Maximum = 120;
        //    chart2.ChartAreas[0].AxisX.Minimum = 0;
        //    chart2.ChartAreas[0].AxisY.Interval = 5;
        //    chart2.ChartAreas[0].AxisY.Minimum = -5;
        //    chart2.ChartAreas[0].AxisY.Maximum = 55;
        //    #endregion
        //}
        #endregion
        #region 进度条
        public void startwork_ProcessDisplayingWorker()
        {
            if (backgroundResetProcess.IsBusy != true)
            {
                backgroundResetProcess.RunWorkerAsync();
            }
        }
        public void stopwork_ProcessDisplayingWorker()
        {
            if (backgroundResetProcess.IsBusy == true)
            {
                backgroundResetProcess.CancelAsync();
            }
        }

        ResetProcess NoticeForm1 = new ResetProcess();
        private void backgroundResetProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            if (NoticeForm1 != null && NoticeForm1.Created)
            {
                NoticeForm1.WindowState = FormWindowState.Normal;
                NoticeForm1.Select();
                return;
            }
            NoticeForm1.ShowDialog();
        }
        #endregion
        #region 机器运行状态显示以及保存Log

        private void UpdateRunResultDisplay(List<string> message, LogName name)
        {
            Invoke(new Action<List<string>, LogName>(RunResultDisplayDataView), message, name);
        }

        public void RunResultDisplayDataView(List<string> message, LogName name)
        {
            try
            {
                if (dataGridViewRunResult.Rows.Count > 50)
                {
                    dataGridViewRunResult.Rows.RemoveAt(0);
                }
                string displayTime;
                displayTime = DateTime.Now.ToString("MM/dd HH:mm:ss:fff");
                dataGridViewRunResult.RowCount += 1;
                int row = dataGridViewRunResult.RowCount;
                dataGridViewRunResult.ColumnCount = 10;
                dataGridViewRunResult.Rows[row - 1].Cells[0].Value = message[0];
                dataGridViewRunResult.Rows[row - 1].Cells[1].Value = message[1];
                dataGridViewRunResult.Rows[row - 1].Cells[2].Value = message[2];
                dataGridViewRunResult.Rows[row - 1].Cells[3].Value = message[3];
                dataGridViewRunResult.Rows[row - 1].Cells[4].Value = message[4];
                dataGridViewRunResult.Rows[row - 1].Cells[5].Value = message[5];
                dataGridViewRunResult.Rows[row - 1].Cells[6].Value = message[6];
                dataGridViewRunResult.Rows[row - 1].Cells[7].Value = message[7];
                dataGridViewRunResult.Rows[row - 1].Cells[8].Value = message[8];
                dataGridViewRunResult.Rows[row - 1].Cells[9].Value = message[9];

                //始终显示最后一行
                dataGridViewRunResult.FirstDisplayedScrollingRowIndex = dataGridViewRunResult.RowCount - 1;
               // FileOp.WriteRunTxt(message);
            }
            catch (Exception)
            {

            }
        }





        private void UpdateRunMessageDisplay(string message, LogName name)
        {
            Invoke(new Action<string, LogName>(RunMessageDisplayDataView), message, name);
        }

        public void RunMessageDisplayDataView(string message, LogName name)
        {
            try
            {
                if (dataGridViewRunMessage.Rows.Count > 50)
                {
                    dataGridViewRunMessage.Rows.RemoveAt(0);
                }
                string displayTime;
                displayTime = DateTime.Now.ToString("MM/dd HH:mm:ss:fff");
                dataGridViewRunMessage.RowCount += 1;
                int row = dataGridViewRunMessage.RowCount;
                //if (message.Contains("PC-->PLC"))
                //{
                //    dataGridViewRunMessage.Rows[row - 1].DefaultCellStyle.BackColor = Color.LightGreen;
                //}
                //if (message.Contains("PLC-->PC"))
                //{
                //    dataGridViewRunMessage.Rows[row - 1].DefaultCellStyle.BackColor = Color.LightPink;
                //}
                //if (message.Contains("出错"))
                //{
                //    dataGridViewRunMessage.Rows[row - 1].DefaultCellStyle.BackColor = Color.Red;
                //}
                dataGridViewRunMessage.ColumnCount = 2;
                dataGridViewRunMessage.Rows[row - 1].Cells[0].Value = displayTime;
                dataGridViewRunMessage.Rows[row - 1].Cells[1].Value = message;
                //始终显示最后一行
                dataGridViewRunMessage.FirstDisplayedScrollingRowIndex = dataGridViewRunMessage.RowCount - 1;
                FileOp.WriteRunTxt(message);
            }
            catch (Exception)
            {

            }
        }
        private void hdl_ShowErrInfoAdd(int p_nEquNo, ST_EquError p_stErrInfo)
        {
            Invoke(new Action<int, ST_EquError>(ShowAddOne), p_nEquNo, p_stErrInfo);
        }

        public void ShowAddOne(int p_nEquNo, ST_EquError p_stErrInfo)
        {
            try
            {
                if (dGAlarmMsg1.Rows.Count > 50)
                {
                    //dGAlarmMsg1.Rows.RemoveAt(0);
                }
               // tbErrorMsg.Text = p_stErrInfo.ssDesc_en;


                tbErrorMsg.Text = /*"              " + */"Code:" + p_stErrInfo.emID.ToString("D") + Environment.NewLine
                   /*+ "                        "*/ + p_stErrInfo.ssDesc_en;
                tbErrorMsg.Image = Properties.Resources.pink536;
                ModePageAlarmStatus.Image = Properties.Resources.pink128;
                ModePageAlarmStatus.Text = "ERROR CODE" + Environment.NewLine + p_stErrInfo.emID.ToString("D");
                ModePageAlarmTime.Image = Properties.Resources.pink128;
                ModePageAlarmTime.Text = "ALARM TIME" + Environment.NewLine + p_stErrInfo.stTime_beg.ToString();
                lbAlarmLight.Visible = true;

                string displayTime;
                displayTime = DateTime.Now.ToString("MM/dd HH:mm:ss:fff");
                dGAlarmMsg1.RowCount += 1;
                int row = dGAlarmMsg1.RowCount;
                dGAlarmMsg1.ColumnCount = 5;
                dGAlarmMsg1.Rows[row - 1].Cells[0].Value = displayTime;
                dGAlarmMsg1.Rows[row - 1].Cells[1].Value = p_stErrInfo.emID.ToString("D");
                string Category = "";
                switch (p_stErrInfo.Category)
                {
                    case 1:
                        Category = "IO信号";
                        
                        break;
                    case 2:
                        Category = "气缸";
                        break;
                    case 3:
                        Category = "通信";
                        break;
                    case 4:
                        Category = "安全门";
                        break;
                }

                dGAlarmMsg1.Rows[row - 1].Cells[2].Value = Category;
                dGAlarmMsg1.Rows[row - 1].Cells[3].Value = p_stErrInfo.ssDesc_en;
                dGAlarmMsg1.Rows[row - 1].Cells[4].Value = "";
                //始终显示最后一行
                dGAlarmMsg1.FirstDisplayedScrollingRowIndex = dGAlarmMsg1.RowCount - 1;
            }
            catch (Exception e)
            {

            }
        }
        private List<string> CategoryERRName = new List<string>();
        private List<double> CategoryerrPercent = new List<double>();
        private List<string> ERRCode = new List<string>();
        private List<double> erPercent = new List<double>();
        private List<double> downtime = new List<double>();
        private void hdl_ShowErrInfoRemove(int p_nEquNo, ST_EquError p_stErrInfo)
        {
            Invoke(new Action<int, ST_EquError>(ShowRemoveOne), p_nEquNo, p_stErrInfo);
        }

        int IOTime = 0;
        int AirTime = 0;
        int ComTime = 0;
        int DoorTime = 0;
        public void ShowRemoveOne(int p_nEquNo, ST_EquError p_stInfo)
        {
            try
            {
                TimeSpan span = p_stInfo.stTime_end - p_stInfo.stTime_beg;
                downtime.Clear();
                //string ssDuration = string.Format("{0}:{1}:{2}", span.Hours, span.Minutes, span.Seconds);
                string ssDuration = string.Format("{0}", span.Hours * 3600 + span.Minutes * 60 + span.Seconds);

                DataGridView dgv = dGAlarmMsg1;
                if (dgv == null)
                    return;
                for (int i = dgv.Rows.Count - 1; i >= 0; --i)
                {
                    if (dgv.Rows[i].Cells[1].Value.ToString() == p_stInfo.emID.ToString("D") && dgv.Rows[i].Cells[4].Value.ToString() == "")
                    {
                        DataGridViewRow row = dgv.Rows[i];
                        dgv.Rows[i].Cells[4].Value = ssDuration;
                        dgv.Invalidate();
                        tbErrorMsg.Text = "";
                        tbErrorMsg.Image = Properties.Resources.green536;
                        lbAlarmLight.Visible = false;
                        ModePageAlarmStatus.Image = Properties.Resources.green128;
                        ModePageAlarmStatus.Text = "No Alarm";
                        ModePageAlarmTime.Image = Properties.Resources.green128;
                        ModePageAlarmTime.Text = "ALARM TIME";
                        switch (dgv.Rows[i].Cells[2].Value.ToString())
                        {
                            case "IO信号":
                                int ioTime = int.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                                IOTime += ioTime;
                                
                                break;
                            case "气缸":
                                int airTime = int.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                                AirTime += airTime;
                               
                                break;
                            case "通信":
                                int comTime = int.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                                ComTime += comTime;
                                
                                break;
                            case "安全门":
                                int doorTime = int.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                                DoorTime += doorTime;
                               
                                break;
                        }                        
                    }
                }
                downtime.Add(IOTime);
                downtime.Add(AirTime);
                downtime.Add(ComTime);
                downtime.Add(DoorTime);
                AlarmPieDisplay(ERRCode, downtime);
            }
            catch (Exception)
            {

            }
        }
        #region Alarm
        Color pink = Color.FromArgb(0XFC, 0XDF, 0XDE);
        Color green = Color.FromArgb(0XAE, 0XDA, 0X97);//(174, 218, 151);
        Color purple = Color.FromArgb(0XC4, 0XCC, 0XEA);//(196, 204, 234);
        Color yellow = Color.FromArgb(0XFD, 0XFD, 0XBF);//(253, 253, 191);
        Color gray = Color.FromArgb(0XEA, 0XEA, 0XEB);//(234, 234, 235);
        Color cyan = Color.FromArgb(0XCB, 0XFF, 0XF3);
        Color red = Color.FromArgb(0XC8, 0X25, 0X06);
        Color blue = Color.FromArgb(0XB3, 0XCA, 0XFF);
        Color lightpurple = Color.FromArgb(0XDE, 0XBC, 0XE4);
        Color wheat = Color.FromArgb(0XE4, 0X75, 0X2B);
        public void AlarmPieDisplay( List<string> errCode, List<double> downtime)
        {
            if (errCode.Count == downtime.Count)
            {
                GlobalVar.Instance.UpdateRunMessage(errCode[0].ToString() + "  " + errCode[1].ToString() + "  " + errCode[2].ToString() + "  " + errCode[3].ToString() 
                    + "  " + downtime[0].ToString() + "  " + downtime[1].ToString() + "  " + downtime[2].ToString() + "  " + downtime[3].ToString(), LogName.runLog);
                try
                {
                    AlarmDownTime.Series[0].Points.DataBindXY(errCode, downtime);
                    AlarmDownTime.Series[0].Points[0].Color = yellow;
                    AlarmDownTime.Series[0].Points[1].Color = pink;
                    AlarmDownTime.Series[0].Points[2].Color = green;
                    AlarmDownTime.Series[0].Points[3].Color = blue;
                    //AlarmDownTime.Series[0].Points[4].Color = purple;
                    //AlarmDownTime.Series[0].Points[5].Color = cyan;
                    //AlarmDownTime.Series[0].Points[6].Color = red;
                    //AlarmDownTime.Series[0].Points[7].Color = blue;
                    //AlarmDownTime.Series[0].Points[8].Color = wheat;
                    //AlarmDownTime.Series[0].Points[9].Color = lightpurple;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Alarm饼状图出错" + e.Message);
                }
            }
        }
        #endregion
        private void DelegateAndAction()
        {
            GlobalVar.Instance.RunMessageDisplay += UpdateRunMessageDisplay;
            GlobalVar.Instance.RunResultDisplay += UpdateRunResultDisplay;
            GlobalVar.Instance.evt_ShowErrInfoAdd += hdl_ShowErrInfoAdd;
            GlobalVar.Instance.evt_ShowErrInfo_Remove += hdl_ShowErrInfoRemove;
        }
        private void RemoveDelegate()
        {
            GlobalVar.Instance.RunMessageDisplay -= UpdateRunMessageDisplay;
        }

        #endregion

        #region 电批操作
        #region 串口1通信
        int index_TK1 = 0;          //检测关键字$60的位置
        string data = "";           //串口数据
        string optID_com_1 = "999"; //操作序号（串口）
        string[] arrResultTK1 = new string[44];   //电批1过程数据
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string rec = serialPort1.ReadExisting();   //读取缓存数据
            data = data + rec;

            if ((index_TK1 = data.IndexOf("$60")) > -1 && data.Contains("\r\n"))  //"$60"开头，"\r\n"结束
            {
                string[] readdata = new string[44];
                data = data.Substring(index_TK1);     //保证开头为$60
               // Log("电批1串口数据:" + data, listBox1);
                readdata = data.Split(',');            //以‘，’分隔成数组
                arrResultTK1 = readdata;
                optID_com_1 = arrResultTK1[1];
                data = "";

                GlobalVar.TKcomDataEnter1 = true;

                if (driverOptTime1 < 6)
                    MessageBox.Show("电批1操作间隔时间过短！                   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                driverOptTime1 = 0;
            }
        }

        public void OpenSerialPort1()
        {
            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Open();
                }
                catch
                {
                    serialPort1.Close();
                    Thread.Sleep(100);
                }
            }
        }



        int countPinChanged1;
        private void serialPort1_PinChanged(object sender, SerialPinChangedEventArgs e)   //离线检测
        {
            countPinChanged1++;
            bool comOfflineCheck = serialPort1.CDHolding;
            if (!comOfflineCheck && countPinChanged1 % 2 == 0)  //一次掉线会触发两次，只取一次
            {
                MessageBox.Show("电批1串口已离线！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
               // Log("电批1串口已离线！", listBox1);
                countPinChanged1 = 0;
            }
        }

        #endregion

        #region 串口2通信
        int index_TK2 = 0;          //检测关键字$60的位置
        string data2 = "";          //串口数据
        string optID_com_2 = "999";//操作序号（串口）
        string[] arrResultTK2 = new string[44];  //电批2过程数据
        private void serialPort2_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string rec = serialPort2.ReadExisting();     //读取缓存数据
            data2 += rec;

            if ((index_TK2 = data2.IndexOf("$60")) > -1 && data2.Contains("\r\n"))
            {
                string[] readdata2 = new string[44];
                data2 = data2.Substring(index_TK2);        //保证开头为$60
               // Log("电批2串口数据:" + data2, listBox1);
                readdata2 = data2.Split(',');              //以‘，’分隔成数组
                arrResultTK2 = readdata2;
                optID_com_2 = arrResultTK2[1];
                data2 = "";

                GlobalVar.TKcomDataEnter2 = true;

                if (driverOptTime2 < 6)
                    MessageBox.Show("电批2操作间隔时间过短！                   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                driverOptTime2 = 0;
            }
        }

        public void OpenSerialPort2()
        {
            if (!serialPort2.IsOpen)
            {
                try
                {
                    serialPort2.Open();
                }
                catch
                {
                    serialPort2.Close();
                    Thread.Sleep(100);
                }
            }
        }

        int countPinChanged2;
        private void serialPort2_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            countPinChanged2++;
            bool comOfflineCheck = serialPort2.CDHolding;
            if (!comOfflineCheck && countPinChanged2 % 2 == 0)     //一次掉线会触发两次，只取一次
            {
                MessageBox.Show("电批2串口已离线！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
               // Log("电批2串口已离线！", listBox1);
                countPinChanged2 = 0;
            }
        }
        #endregion
        long driverOptTime1 = 0;  //电批1空转到锁付操作计时
        long driverOptTime2 = 0;  //电批2空转到锁付操作计时
        private void timer2_Tick(object sender, EventArgs e)
        {
            driverOptTime1++;
            driverOptTime2++;
        }
        #region 定义变量
        public int tkDataMode = 0;            //0:实时模式，1：阅读模式
        public int tkRecvCount = 500;         //实时模式时待接收到一定数量再判断是否接收结束
        Label[] lb_1T = new Label[12];        //用于把电批1扭力对时间曲线图右边的Label控件打包进数组
        Label[] lb_1A = new Label[12];        //用于把电批1扭力对角度曲线图右边的Label控件打包进数组
        Label[] lb_2T = new Label[12];        //用于把电批2扭力对时间曲线图右边的Label控件打包进数组
        Label[] lb_2A = new Label[12];        //用于把电批2扭力对角度曲线图右边的Label控件打包进数组
        string dataSavePath = "D:\\UI_data";  //数据保存路径
        //string startupTime = "";              //软件启动时间

        ToolStripMenuItem listRightMenu = new ToolStripMenuItem("Copy");  //listBox1右键弹出Copy按钮
        #endregion
        #region 保存TCR/RTM 算好的扭矩值和角度值
        public void Updata_AngleTorque1()   //电批1算好的扭矩值和角度值
        {
            string fileName = DateTime.Now.ToString("yyyy-M-d_HH-mm-ss-fff");
            string savePath = dataSavePath + "\\STD1\\TCR\\";
            string title = "No" + "," + "Torque1" + "," + "Angle1";
            string value = "";
            int len = GlobalVar.torq_double_1.Length;

            for (int i = 0; i < len; i++)
            {
                value = i + 1 + "," + GlobalVar.torq_double_1[i] + "," + GlobalVar.Angle_double_1[i];
                FileOp.WriteRecordValueCSV(savePath, fileName, title, value);
            }
        }

        public void Updata_AngleTorque2()   //电批2算好的扭矩值和角度值
        {
            string fileName = DateTime.Now.ToString("yyyy-M-d_HH-mm-ss-fff");
            string savePath = dataSavePath + "\\STD2\\TCR\\";
            string title = "No" + "," + "Torque2" + "," + "Angle2";
            string value = "";
            int len = GlobalVar.torq_double_2.Length;
            for (int i = 0; i < len; i++)
            {
                value = i + 1 + "," + GlobalVar.torq_double_2[i] + "," + GlobalVar.Angle_double_2[i];
                FileOp.WriteRecordValueCSV(savePath, fileName, title, value);
            }
        }

        #endregion

        #region 保存过程监视数据 PRM
        string titlePRM = "Time" + "," + "HD" + "," + "ID" + "," + "MC" + "," + "EC" + "," + "ALT1" + "," + "ALT2" + "," + "WARN" + ","
                      + "T1" + "," + "T2" + "," + "T3" + "," + "T4" + "," + "Tt" + "," + "A1" + "," + "A2" + "," + "A3" + "," + "Af" + ","
                      + "Abr" + "," + "A4" + "," + "At" + "," + "PRMCT" + "," + "MECCT" + "," + "Tbr" + "," + "Dra" + "," + "Drp" + ","
                      + "TU" + "," + "SSL" + "," + "SR1" + "," + "SR2" + "," + "SN1" + "," + "SN2" + "," + "OTA" + "," + "TM";
        //+ "," + "As1" + "," + "As2" + "," + "St1" + "," + "St2" + "," + "sA1" + "," + "sA2" + "," + "sMECCT" + "," + "Ast" + "," + "LoT" + "," + "LoA" + "," + "LoM" + ",";

        public void Updata_PRM1()   //记录电批1过程数据 PRM
        {
            string fileName = DateTime.Now.ToString("yyyy-M-d");
            string savePath = dataSavePath + "\\STD1\\PRM\\";
            string value = DateTime.Now.ToString("HH:mm:ss");
            string[] temp = arrResultTK1[31].Split('\r');
            arrResultTK1[31] = temp[0];   //去掉换行符
            for (int i = 0; i < 32; i++)
            {
                value = value + "," + arrResultTK1[i];
            }
            FileOp.WriteRecordValueCSV(savePath, fileName, titlePRM, value);
           // recordValuePRM1.RecordValue(savePath, fileName, titlePRM, value);
        }


        public void Updata_PRM2()   //记录电批2过程数据 PRM
        {
            string fileName = DateTime.Now.ToString("yyyy-M-d");
            string savePath = dataSavePath + "\\STD2\\PRM\\";
            string value = DateTime.Now.ToString("HH:mm:ss");
            string[] temp = arrResultTK2[31].Split('\r');
            arrResultTK2[31] = temp[0];  //去掉换行符
            for (int i = 0; i < 32; i++)
            {
                value = value + "," + arrResultTK2[i];
            }
            FileOp.WriteRecordValueCSV(savePath, fileName, titlePRM, value);
            // recordValuePRM2.RecordValue(savePath, fileName, titlePRM, value);
        }

        #endregion

        #region 检测数据都完成接收，进行绘图、截图
        public int tkDataInterval = 8;         //采样时间间隔(ms)
        MyChart myChart1_T = new MyChart();    //电批1扭力对时间绘图实例化
        MyChart myChart1_A = new MyChart();    //电批1扭力对角度绘图实例化
        MyChart myChart2_T = new MyChart();    //电批2扭力对时间绘图实例化
        MyChart myChart2_A = new MyChart();    //电批2扭力对角度绘图实例化
        int dataEnterTime1 = 0;   //电批1两路数据进来的时间间隔
        int dataEnterTime2 = 0;   //电批1两路数据进来的时间间隔
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (GlobalVar.TKnetDataEnter1 || GlobalVar.TKcomDataEnter1)   //检查电批1两路相隔时间
            {
                if (dataEnterTime1 > 50)
                {
                    MessageBox.Show("电批1接收数据超时！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    // Log("电批1接收数据超时！", listBox1);
                    GlobalVar.TKnetDataEnter1 = false;
                    GlobalVar.TKcomDataEnter1 = false;
                    dataEnterTime1 = 0;
                }
                dataEnterTime1++;
            }

            if (GlobalVar.TKnetDataEnter1 && GlobalVar.TKcomDataEnter1)   //电批1两路数据均已接收
            {
                dataEnterTime1 = 0;
                GlobalVar.TKnetDataEnter1 = false;
                GlobalVar.TKcomDataEnter1 = false;
                if (optID_com_1 == GlobalVar.optID_net_1.ToString())  //判断电批1两路数据编号相同，确保为同一次锁付
                {
                    Updata_AngleTorque1();  //电批1算好的扭矩值和角度值
                    Updata_PRM1();          //记录电批1过程数据 PRM
                    string picName = DateTime.Now.ToString("yyyy-M-d_HH-mm-ss-fff");
                    string savePath = dataSavePath + "\\STD1\\Curve\\Torque_Time\\";
                    myChart1_T.Draw(chartTorqueTime1, lb_1T, tkDataInterval, 0, arrResultTK1, GlobalVar.Max_Torque_1, GlobalVar.Max_Angle_1, GlobalVar.torq_double_1, GlobalVar.Angle_double_1, GlobalVar.titleAxisY_1, GlobalVar.indexSSL1);   //绘图扭力对时间
                   // myChart1_T.ShotPicture(panel_1T, savePath, picName);   //截图
                    //Log("电批1 Torque_Time截图完成", listBox1);

                   // savePath = dataSavePath + "\\STD1\\Curve\\Torque_Angle\\";
                   // //tabControl1.SelectedIndex = 1;   //把该页显示出来防止截黑图
                   // myChart1_A.Draw(chartTorqueAngle1, lb_1A, tkDataInterval, 1, arrResultTK1, GlobalVar.Max_Torque_1, GlobalVar.Max_Angle_1, GlobalVar.torq_double_1, GlobalVar.Angle_double_1, GlobalVar.titleAxisY_1, GlobalVar.indexSSL1);   //绘图扭力对角度
                   //// myChart1_A.ShotPicture(panel_1A, savePath, picName);   //截图
                   //// Log("电批1 Torque_Angle截图完成", listBox1);
                   // //tabControl1.SelectedIndex = 0;   //把该页显示出来防止截黑图
                }
                else
                {
                    MessageBox.Show("电批1两路数据编号不一致！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                   // Log("电批1两路数据编号不一致！", listBox1);
                }
            }

            if (GlobalVar.TKnetDataEnter2 || GlobalVar.TKcomDataEnter2)   //检查电批2两路相隔时间
            {
                if (dataEnterTime2 > 50)
                {
                    MessageBox.Show("电批2接收数据超时！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    //Log("电批2接收数据超时！", listBox1);
                    //this.TopMost = true;   
                    GlobalVar.TKnetDataEnter2 = false;
                    GlobalVar.TKcomDataEnter2 = false;
                    dataEnterTime2 = 0;
                }
                dataEnterTime2++;
            }

            if (GlobalVar.TKnetDataEnter2 && GlobalVar.TKcomDataEnter2)   //电批2两路数据均已接收
            {
                dataEnterTime2 = 0;
                GlobalVar.TKnetDataEnter2 = false;
                GlobalVar.TKcomDataEnter2 = false;
                if (optID_com_2 == GlobalVar.optID_net_2.ToString())  //判断电批2两路数据编号相同，确保为同一次锁付
                {
                    Updata_AngleTorque2();   //电批2算好的扭矩值和角度值
                    Updata_PRM2();           //记录电批2过程数据 PRM

                    string picName = DateTime.Now.ToString("yyyy-M-d_HH-mm-ss-fff");
                    string savePath = dataSavePath + "\\STD2\\Curve\\Torque_Time\\";
                    myChart2_T.Draw(chartTorqueTime2, lb_2T, tkDataInterval, 0, arrResultTK2, GlobalVar.Max_Torque_2, GlobalVar.Max_Angle_2, GlobalVar.torq_double_2, GlobalVar.Angle_double_2, GlobalVar.titleAxisY_2, GlobalVar.indexSSL2);   //绘图扭力对时间
                    myChart2_T.ShotPicture(panel_2T, savePath, picName);   //截图
                    // Log("电批2 Torque_Time截图完成", listBox1);

                   // savePath = dataSavePath + "\\STD2\\Curve\\Torque_Angle\\";
                   //// tabControl2.SelectedIndex = 1;   //把该页显示出来防止截黑图
                   // myChart2_A.Draw(chartTorqueAngle2, lb_2A, tkDataInterval, 1, arrResultTK2, GlobalVar.Max_Torque_2, GlobalVar.Max_Angle_2, GlobalVar.torq_double_2, GlobalVar.Angle_double_2, GlobalVar.titleAxisY_2, GlobalVar.indexSSL2);   //绘图扭力对角度
                   // myChart2_A.ShotPicture(panel_2A, savePath, picName);   //截图
                   //// Log("电批2 Torque_Angle截图完成", listBox1);
                   //// tabControl2.SelectedIndex = 0;   //把该页显示出来防止截黑图
                }
                else
                {
                    MessageBox.Show("电批2两路数据编号不一致！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                   // Log("电批2两路数据编号不一致！", listBox1);
                }
            }
        }

        #endregion
        #region  电批Label打包
        public void LableAdd()
        {
            lb_1T[0] = lb_1T_EC;       //把电批1扭力对时间曲线图右边的Label控件打包进数组
            lb_1T[1] = lb_1T_T1;
            lb_1T[2] = lb_1T_T2;
            lb_1T[3] = lb_1T_T3;
            lb_1T[4] = lb_1T_Tt;
            lb_1T[5] = lb_1T_A1;
            lb_1T[6] = lb_1T_A2;
            lb_1T[7] = lb_1T_A3;
            lb_1T[8] = lb_1T_MaxAngle;
            lb_1T[9] = lb_1T_MaxTorque;
            lb_1T[10] = lb_1T_SSL;
            lb_1T[11] = lb_1T_MC;

            //lb_1A[0] = lb_1A_EC;       //把电批1扭力对角度曲线图右边的Label控件打包进数组
            //lb_1A[1] = lb_1A_T1;
            //lb_1A[2] = lb_1A_T2;
            //lb_1A[3] = lb_1A_T3;
            //lb_1A[4] = lb_1A_Tt;
            //lb_1A[5] = lb_1A_A1;
            //lb_1A[6] = lb_1A_A2;
            //lb_1A[7] = lb_1A_A3;
            //lb_1A[8] = lb_1A_MaxAngle;
            //lb_1A[9] = lb_1A_MaxTorque;
            //lb_1A[10] = lb_1A_SSL;
            //lb_1A[11] = lb_1A_MC;

            lb_2T[0] = lb_2T_EC;       //把电批2扭力对时间曲线图右边的Label控件打包进数组
            lb_2T[1] = lb_2T_T1;
            lb_2T[2] = lb_2T_T2;
            lb_2T[3] = lb_2T_T3;
            lb_2T[4] = lb_2T_Tt;
            lb_2T[5] = lb_2T_A1;
            lb_2T[6] = lb_2T_A2;
            lb_2T[7] = lb_2T_A3;
            lb_2T[8] = lb_2T_MaxAngle;
            lb_2T[9] = lb_2T_MaxTorque;
            lb_2T[10] = lb_2T_SSL;
            lb_2T[11] = lb_2T_MC;

            //lb_2A[0] = lb_2A_EC;       //把电批2扭力对角度曲线图右边的Label控件打包进数组
            //lb_2A[1] = lb_2A_T1;
            //lb_2A[2] = lb_2A_T2;
            //lb_2A[3] = lb_2A_T3;
            //lb_2A[4] = lb_2A_Tt;
            //lb_2A[5] = lb_2A_A1;
            //lb_2A[6] = lb_2A_A2;
            //lb_2A[7] = lb_2A_A3;
            //lb_2A[8] = lb_2A_MaxAngle;
            //lb_2A[9] = lb_2A_MaxTorque;
            //lb_2A[10] = lb_2A_SSL;
            //lb_2A[11] = lb_2A_MC;
        }
        #endregion

        #region
        public void OpenSerial()
        {
            OpenSerialPort1();
            OpenSerialPort2();
        }
        #endregion
        public void startbackgroundEleScrew()
        {
            if (backgroundEleScrew.IsBusy != true)
            {
                backgroundEleScrew.RunWorkerAsync();
            }
        }
        public void stopbackgroundEleScrew()
        {
            if (backgroundEleScrew.IsBusy == true)
            {
                backgroundEleScrew.CancelAsync();
            }
        }
        private void backgroundEleScrew_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(5);
                if (backgroundEleScrew.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                OpenSerial();   //打开串口
                if (!EleScrew1.instance().GetTCPIPConnectSts)
                {
                    EleScrew1.instance().TheTCPIPConnect();
                }
                if (!EleScrew2.instance().GetTCPIPConnectSts)
                {
                    EleScrew2.instance().TheTCPIPConnect();
                }
                if (!KeyenceBarCode.instance().GetTCPIPConnectSts)
                {
                    KeyenceBarCode.instance().TheTCPIPConnect();
                }
            }
        }


        #endregion

        #region 界面刷新外接设备连接状态
        public void startbackgroundConnectStatu()
        {
            if (backgroundConnectStatu.IsBusy != true)
            {
                backgroundConnectStatu.RunWorkerAsync();
            }
        }
        public void stopbackgroundConnectStatu()
        {
            if (backgroundConnectStatu.IsBusy == true)
            {
                backgroundConnectStatu.CancelAsync();
            }
        }
        private void backgroundConnectStatu_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                if (backgroundEleScrew.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                if (GlobalVar.ConnectStatus)
                {
                    Invoke(new Action(() =>
                    {
                        if (!EleScrew1.instance().GetTCPIPConnectSts)
                        {
                            LbEleScrew1.Image = Properties.Resources.red;                           
                        }
                        else
                        {
                            LbEleScrew1.Image = Properties.Resources.green;
                        }
                        if (!EleScrew2.instance().GetTCPIPConnectSts)
                        {
                            LbEleScrew2.Image = Properties.Resources.red;
                        }
                        else
                        {
                            LbEleScrew2.Image = Properties.Resources.green;
                        }
                    }));
                }
            }
        }
        #endregion
        Random ran = new Random();
        int a = 1;
        int a1 = 1;
        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                int b = ran.Next(100);
                int c = ran.Next(10);
                chartYiledRate1.UpdateChartDayOkRate(b+1,c+1 );
                chartYiledRate1.UpdateChartMonthOkRate(b+40, c+10);
            }
            catch (Exception e1)
            {
                MessageBox.Show("环形饼状图出错" + e1.Message);
            }

            EM_ErrID emDes;
            switch (a)
            {
                case 1:
                    emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.HSGFeedBlockUp);
                    CErrorMgr.Instance._Add_One(emDes);
                    break;
                case 2:
                    emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.HSGFeedBlockUp);
                    CErrorMgr.Instance._Remove_One(emDes);
                    break;
                case 3:
                    emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.StartButton);
                    CErrorMgr.Instance._Add_One(emDes);
                    break;
                case 4:
                    emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.StartButton);
                    CErrorMgr.Instance._Remove_One(emDes);
                    break;
                case 5:
                    emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.FrontDoor1);
                    CErrorMgr.Instance._Add_One(emDes);
                    break;
                case 6:
                    emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.FrontDoor1);
                    CErrorMgr.Instance._Remove_One(emDes);
                    break;
                case 7:
                    emDes = CErrorMgr.Instance.Get_ErrID(3, 0);
                    CErrorMgr.Instance._Add_One(emDes);
                    break;
                case 8:
                    emDes = CErrorMgr.Instance.Get_ErrID(3, 0);
                    CErrorMgr.Instance._Remove_One(emDes);
                    break;
            }
            a++;
            a1++;
            if (a > 8)
            {
                a = 0;
            }
            CDBhelper._Get_Instance()._ProductData_Add(a1.ToString() + "1111", "111", "111", "111", "111", DateTime.Now);
            CDBhelper._Get_Instance()._Up_Product_CCD2_CC_By_SN(a1.ToString() + "1111", "111", "111");
            CDBhelper._Get_Instance()._Up_Product_CCD3_CC_By_SN(a1.ToString() + "1111", "111", "111");
            SQLiteDataReader reader = null;
            reader = CDBhelper._Get_Instance().Get_ProductMessage_BySN(a1.ToString() + "1111");
            
            List<string> mess = new List<string>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount - 1; i++)
                {
                    mess.Add(reader.GetValue(i + 1).ToString());
                }
            }
            GlobalVar.Instance.UpdateRunResult(mess, LogName.runLog);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mMainstream.KeyenceReadBarCode("LON");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mMainstream.KeyenceReadBarCode("LOFF");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            mMainstream.View_EarthBarCode("");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            mMainstream.View_EarthBarCode("");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            startbackgroundUpdataAlarm();
        }
        private bool DatePickers_CompareAndGet1(ref DateTime begTime, ref DateTime endTime)
        {
            try
            {
                int nBegYear = dateTimePicker4.Value.Year;
                int nBegMonth = dateTimePicker4.Value.Month;
                int nBegDay = dateTimePicker4.Value.Day;
                int nBegHour = Convert.ToInt32(comboBox8.Text);
                int nBegMinute = Convert.ToInt32(comboBox6.Text);

                int nEndYear = dateTimePicker3.Value.Year;
                int nEndMonth = dateTimePicker3.Value.Month;
                int nEndDay = dateTimePicker3.Value.Day;
                int nEndHour = Convert.ToInt32(comboBox7.Text);
                int nEndMinute = Convert.ToInt32(comboBox5.Text);

                begTime = new DateTime(nBegYear, nBegMonth, nBegDay, nBegHour, nBegMinute, 0);
                endTime = new DateTime(nEndYear, nEndMonth, nEndDay, nEndHour, nEndMinute, 0);
                if (begTime.Ticks >= endTime.Ticks)
                {
                    //MessageBox.Show("起始时间不能大于截止时间", "SF IST");
                    return false;
                }
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string name = "D:\\Report\\Alarm\\";
            ExportData_AsCSV(dataGridView2, name);
        }
        private void ExportData_AsCSV(DataGridView dataGridView, string name)
        {
            string filePath = "";

            filePath = name + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".csv"; //2019-04-19

            if (dataGridView.Rows.Count <= 1)
            {
                MessageBox.Show("not data Export!"); return;
            }
            string dir = filePath.Substring(0, filePath.LastIndexOf("\\"));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            List<string> lisData = new List<string>();

            string ssHeader = "";

            try
            {
                for (int column = 0; column < dataGridView.Columns.Count; column++)
                {
                    if (dataGridView.Columns[column].Visible == true)
                    {
                        ssHeader += dataGridView.Columns[column].HeaderText + ",";
                    }
                }
                lisData.Add(ssHeader.Substring(0, ssHeader.Length - 1));
                for (int i = 0; i < dataGridView.Rows.Count; ++i)
                {
                    string ssLine = "";
                    for (int j = 0; j < dataGridView.Rows[i].Cells.Count; ++j)
                    {
                        if (null == dataGridView.Rows[i].Cells[j].Value)
                        {
                            break; //???
                        }
                        ssLine += dataGridView.Rows[i].Cells[j].Value.ToString() + ",";
                    }
                    lisData.Add(ssLine.Substring(0, ssLine.Length - 1));
                }
                if (lisData.Count > 1)
                    FileOp.WriteLines(filePath, ref lisData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("export data Failed" + ex.Message);
                return;
            }
            MessageBox.Show("export data success");
        }
        List<int> count = new List<int>();
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int a = dataGridView2.SelectedCells.Count;
            count.Clear();
            Action act = () =>
            {
                for (int i = 0; i < a; i++)
                {
                    int b = dataGridView2.SelectedCells[i].RowIndex;
                    count.Add(b);

                }
                int[] array = count.ToArray();
                Array.Sort(array);
                Array.Reverse(array);
                for (int i = 0; i < array.Length; i++)
                {
                    dataGridView2.Rows.RemoveAt(array[i]);
                }
            };
            this.Invoke(new Action(act));
        }
        private bool DatePickers_CompareAndGet(ref DateTime begTime, ref DateTime endTime)
        {
            try
            {
                int nBegYear = dateTimePicker1.Value.Year;
                int nBegMonth = dateTimePicker1.Value.Month;
                int nBegDay = dateTimePicker1.Value.Day;
                int nBegHour = Convert.ToInt32(comboBox1.Text);
                int nBegMinute = Convert.ToInt32(comboBox3.Text);

                int nEndYear = dateTimePicker2.Value.Year;
                int nEndMonth = dateTimePicker2.Value.Month;
                int nEndDay = dateTimePicker2.Value.Day;
                int nEndHour = Convert.ToInt32(comboBox2.Text);
                int nEndMinute = Convert.ToInt32(comboBox4.Text);

                begTime = new DateTime(nBegYear, nBegMonth, nBegDay, nBegHour, nBegMinute, 0);
                endTime = new DateTime(nEndYear, nEndMonth, nEndDay, nEndHour, nEndMinute, 0);
                if (begTime.Ticks >= endTime.Ticks)
                {
                    //MessageBox.Show("起始时间不能大于截止时间", "SF IST");
                    return false;
                }

            }
            catch (Exception e)
            {
                // MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            startbackgroundUpdataProduction();
        }

        private void ExportExcelBtn_Click(object sender, EventArgs e)
        {
            string name = "D:\\Report\\Production\\";
            ExportData_AsCSV(dataGridView1, name);
        }

        public void startbackgroundUpDate()
        {
            if (backgroundUpDate.IsBusy != true)
            {
                backgroundUpDate.RunWorkerAsync();
            }
        }
        public void stopbackgroundUpDate()
        {
            if (backgroundUpDate.IsBusy == true)
            {
                backgroundUpDate.CancelAsync();
            }
        }
        double[] TimeArray = new double[] { 1, 2, 3, 4, 5, 6, 7, 8,10,11,12,13,14,15 };
        double[] PreArray = new double[] { 14, 25, 13, 21, 22, 30, 12, 23, 25, 13, 21, 22, 30, 12 };
        double[] PreArray1 = new double[] { 24, 15, 23, 11, 12, 20, 22, 23, 15, 23, 11, 12, 30, 22 };
        private void backgroundUpDate_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(50);
                if (backgroundEleScrew.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                //Invoke(new Action(() =>
                //{
                //    chartTorqueTime1.Series[0].Points.DataBindXY(TimeArray, PreArray);  //记得删除
                //    tabControl2.SelectedIndex = 1;
                //    string picName = DateTime.Now.ToString("yyyy-M-d_HH-mm-ss-fff");
                //    string savePath = dataSavePath + "\\STD1\\Curve\\Torque_Time\\";              
                //    myChart1_T.ShotPicture(panel_1T, savePath, picName);   //截图
                //}));
                if (GlobalVar.TKnetDataEnter1 || GlobalVar.TKcomDataEnter1)   //检查电批1两路相隔时间
                {
                    if (dataEnterTime1 > 50)
                    {
                        MessageBox.Show("电批1接收数据超时！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        // Log("电批1接收数据超时！", listBox1);
                        GlobalVar.TKnetDataEnter1 = false;
                        GlobalVar.TKcomDataEnter1 = false;
                        dataEnterTime1 = 0;
                    }
                    dataEnterTime1++;
                }

                if (GlobalVar.TKnetDataEnter1 && GlobalVar.TKcomDataEnter1)   //电批1两路数据均已接收
                {
                    dataEnterTime1 = 0;
                    GlobalVar.TKnetDataEnter1 = false;
                    GlobalVar.TKcomDataEnter1 = false;
                    if (optID_com_1 == GlobalVar.optID_net_1.ToString())  //判断电批1两路数据编号相同，确保为同一次锁付
                    {
                        Updata_AngleTorque1();  //电批1算好的扭矩值和角度值
                        Updata_PRM1();          //记录电批1过程数据 PRM
                        string picName = DateTime.Now.ToString("yyyy-M-d_HH-mm-ss-fff");
                        string savePath = dataSavePath + "\\STD1\\Curve\\Torque_Time\\";
                        myChart1_T.Draw(chartTorqueTime1, lb_1T, tkDataInterval, 0, arrResultTK1, GlobalVar.Max_Torque_1, GlobalVar.Max_Angle_1, GlobalVar.torq_double_1, GlobalVar.Angle_double_1, GlobalVar.titleAxisY_1, GlobalVar.indexSSL1);   //绘图扭力对时间
                        Invoke(new Action(() =>
                        {
                            tabControl2.SelectedIndex = 0;
                            myChart1_T.ShotPicture(panel_1T, savePath, picName);   //截图
                        }));                                                 //Log("电批1 Torque_Time截图完成", listBox1);

                        //savePath = dataSavePath + "\\STD1\\Curve\\Torque_Angle\\";
                        ////tabControl1.SelectedIndex = 1;   //把该页显示出来防止截黑图
                        //myChart1_A.Draw(chartTorqueAngle1, lb_1A, tkDataInterval, 1, arrResultTK1, GlobalVar.Max_Torque_1, GlobalVar.Max_Angle_1, GlobalVar.torq_double_1, GlobalVar.Angle_double_1, GlobalVar.titleAxisY_1, GlobalVar.indexSSL1);   //绘图扭力对角度
                        ////myChart1_A.ShotPicture(panel_1A, savePath, picName);   //截图
                        //// Log("电批1 Torque_Angle截图完成", listBox1);
                        ////tabControl1.SelectedIndex = 0;   //把该页显示出来防止截黑图
                    }
                    else
                    {
                        MessageBox.Show("电批1两路数据编号不一致！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        // Log("电批1两路数据编号不一致！", listBox1);
                    }
                }

                if (GlobalVar.TKnetDataEnter2 || GlobalVar.TKcomDataEnter2)   //检查电批2两路相隔时间
                {
                    if (dataEnterTime2 > 50)
                    {
                        MessageBox.Show("电批2接收数据超时！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        //Log("电批2接收数据超时！", listBox1);
                        //this.TopMost = true;   
                        GlobalVar.TKnetDataEnter2 = false;
                        GlobalVar.TKcomDataEnter2 = false;
                        dataEnterTime2 = 0;
                    }
                    dataEnterTime2++;
                }

                if (GlobalVar.TKnetDataEnter2 && GlobalVar.TKcomDataEnter2)   //电批2两路数据均已接收
                {
                    dataEnterTime2 = 0;
                    GlobalVar.TKnetDataEnter2 = false;
                    GlobalVar.TKcomDataEnter2 = false;
                    if (optID_com_2 == GlobalVar.optID_net_2.ToString())  //判断电批2两路数据编号相同，确保为同一次锁付
                    {
                        Updata_AngleTorque2();   //电批2算好的扭矩值和角度值
                        Updata_PRM2();           //记录电批2过程数据 PRM

                        string picName = DateTime.Now.ToString("yyyy-M-d_HH-mm-ss-fff");
                        string savePath = dataSavePath + "\\STD2\\Curve\\Torque_Time\\";
                        myChart2_T.Draw(chartTorqueTime2, lb_2T, tkDataInterval, 0, arrResultTK2, GlobalVar.Max_Torque_2, GlobalVar.Max_Angle_2, GlobalVar.torq_double_2, GlobalVar.Angle_double_2, GlobalVar.titleAxisY_2, GlobalVar.indexSSL2);   //绘图扭力对时间
                       Invoke(new Action(() =>
                        {
                            tabControl2.SelectedIndex = 1;
                            myChart2_T.ShotPicture(panel_2T, savePath, picName);   //截图
                        }));                                                        // Log("电批2 Torque_Time截图完成", listBox1);

                        //savePath = dataSavePath + "\\STD2\\Curve\\Torque_Angle\\";
                        //// tabControl2.SelectedIndex = 1;   //把该页显示出来防止截黑图
                        //myChart2_A.Draw(chartTorqueAngle2, lb_2A, tkDataInterval, 1, arrResultTK2, GlobalVar.Max_Torque_2, GlobalVar.Max_Angle_2, GlobalVar.torq_double_2, GlobalVar.Angle_double_2, GlobalVar.titleAxisY_2, GlobalVar.indexSSL2);   //绘图扭力对角度
                        //myChart2_A.ShotPicture(panel_2A, savePath, picName);   //截图
                                                                               // Log("电批2 Torque_Angle截图完成", listBox1);
                                                                               // tabControl2.SelectedIndex = 0;   //把该页显示出来防止截黑图
                    }
                    else
                    {
                        MessageBox.Show("电批2两路数据编号不一致！                     ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        // Log("电批2两路数据编号不一致！", listBox1);
                    }
                }
                //Thread.Sleep(5000);
                //Invoke(new Action(() =>
                //{
                //    chartTorqueTime2.Series[0].Points.DataBindXY(TimeArray, PreArray1);  //记得删除
                //    tabControl2.SelectedIndex = 0;
                //    string picName = DateTime.Now.ToString("yyyy-M-d_HH-mm-ss-fff");
                //    string savePath = dataSavePath + "\\STD2\\Curve\\Torque_Time\\";                  
                //    myChart2_T.ShotPicture(panel_2T, savePath, picName);   //截图
                //}));
            }
        }
        public void startbackgroundTest()
        {
            if (backgroundTest.IsBusy != true)
            {
                backgroundTest.RunWorkerAsync();
            }
        }
        public void stopbackgroundTest()
        {
            if (backgroundTest.IsBusy == true)
            {
                backgroundTest.CancelAsync();
            }
        }
        private void backgroundTest_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(1500);
                if (backgroundEleScrew.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                try
                {
                    int b = ran.Next(100);
                    int c = ran.Next(10);
                    chartYiledRate1.UpdateChartDayOkRate(b + 1, c + 1);
                    chartYiledRate1.UpdateChartMonthOkRate(b + 40, c + 10);
                }
                catch (Exception e1)
                {
                    MessageBox.Show("环形饼状图出错" + e1.Message);
                }

                EM_ErrID emDes;
                switch (a)
                {
                    case 1:
                        emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.HSGFeedBlockUp);
                        CErrorMgr.Instance._Add_One(emDes);
                        break;
                    case 2:
                        emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.HSGFeedBlockUp);
                        CErrorMgr.Instance._Remove_One(emDes);
                        break;
                    case 3:
                        emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.StartButton);
                        CErrorMgr.Instance._Add_One(emDes);
                        break;
                    case 4:
                        emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.StartButton);
                        CErrorMgr.Instance._Remove_One(emDes);
                        break;
                    case 5:
                        emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.FrontDoor1);
                        CErrorMgr.Instance._Add_One(emDes);
                        break;
                    case 6:
                        emDes = CErrorMgr.Instance.Get_ErrID(0, (int)IOListInput.FrontDoor1);
                        CErrorMgr.Instance._Remove_One(emDes);
                        break;
                    case 7:
                        emDes = CErrorMgr.Instance.Get_ErrID(3, 0);
                        CErrorMgr.Instance._Add_One(emDes);
                        break;
                    case 8:
                        emDes = CErrorMgr.Instance.Get_ErrID(3, 0);
                        CErrorMgr.Instance._Remove_One(emDes);
                        break;
                }
                a++;
                a1++;
                if (a > 8)
                {
                    a = 0;
                }
                CDBhelper._Get_Instance()._ProductData_Add(a1.ToString() + "1111", "111", "111", "111", "111", DateTime.Now);
                CDBhelper._Get_Instance()._Up_Product_CCD2_CC_By_SN(a1.ToString() + "1111", "111", "111");
                CDBhelper._Get_Instance()._Up_Product_CCD3_CC_By_SN(a1.ToString() + "1111", "111", "111");
                SQLiteDataReader reader = null;
                reader = CDBhelper._Get_Instance().Get_ProductMessage_BySN(a1.ToString() + "1111");

                List<string> mess = new List<string>();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount - 1; i++)
                    {
                        mess.Add(reader.GetValue(i + 1).ToString());
                    }
                }
                GlobalVar.Instance.UpdateRunResult(mess, LogName.runLog);
            }
        }
        public void startbackgroundUpdataAlarm()
        {
            if (backgroundUpdataAlarm.IsBusy != true)
            {
                backgroundUpdataAlarm.RunWorkerAsync();
            }
        }
        public void stopbackgroundUpdataAlarm()
        {
            if (backgroundUpdataAlarm.IsBusy == true)
            {
                backgroundUpdataAlarm.CancelAsync();
            }
        }
        private void backgroundUpdataAlarm_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke(new Action(() =>
                {
                    dataGridView2.Rows.Clear();
                    DateTime beg = new DateTime();
                    DateTime end = new DateTime();
                    if (!DatePickers_CompareAndGet1(ref beg, ref end))
                    {

                        MessageBox.Show("time select invalid.");
                        return;
                    }

                    TimeSpan tspan = end - beg;

                    if (tspan.TotalDays > 10)
                    {
                        MessageBox.Show("it is not more than 10 days for time range");
                        return;
                    }
                    string Category = "";
                    SQLiteDataReader reader = null;
                    reader = CDBhelper._Get_Instance().Get_Resistance1Message_Time(beg, end);
                    int rows = 0;
                    while (reader.Read())
                    {
                        rows = dataGridView2.Rows.Add();
                        for (int i = 0; i < reader.FieldCount - 1; i++)
                        {
                            switch (int.Parse(reader.GetValue(3).ToString()))
                            {
                                case 1:
                                    Category = "IO信号";
                                    break;
                                case 2:
                                    Category = "气缸";
                                    break;
                                case 3:
                                    Category = "通信";
                                    break;
                                case 4:
                                    Category = "安全门";
                                    break;
                            }
                            if (i == 2)
                            {
                                dataGridView2.Rows[rows].Cells[2].Value = Category;
                            }
                            else
                            {
                                dataGridView2.Rows[rows].Cells[i].Value = reader.GetValue(i + 1);
                            }
                        }
                    }
                }));
        }
        public void startbackgroundUpdataProduction()
        {
            if (backgroundUpdateProduction.IsBusy != true)
            {
                backgroundUpdateProduction.RunWorkerAsync();
            }
        }
        public void stopbackgroundUpdataProduction()
        {
            if (backgroundUpdateProduction.IsBusy == true)
            {
                backgroundUpdateProduction.CancelAsync();
            }
        }
        private void backgroundUpdateProduction_DoWork(object sender, DoWorkEventArgs e)
        {
             Invoke(new Action(() =>
                {
                    dataGridView1.Rows.Clear();
                    DateTime beg = new DateTime();
                    DateTime end = new DateTime();
                    if (!DatePickers_CompareAndGet(ref beg, ref end))
                    {

                        MessageBox.Show("time select invalid.");
                        return;
                    }

                    TimeSpan tspan = end - beg;

                    if (tspan.TotalDays > 10)
                    {
                        MessageBox.Show("it is not more than 10 days for time range");
                        return;
                    }

                    SQLiteDataReader reader = null;
                    reader = CDBhelper._Get_Instance().Get_ProductMessage_Time(beg, end);
                    int rows = 0;
                    while (reader.Read())
                    {
                        rows = dataGridView1.Rows.Add();
                        for (int i = 0; i < reader.FieldCount - 1; i++)
                        {
                            dataGridView1.Rows[rows].Cells[i].Value = reader.GetValue(i + 1);
                        }
                    }
                }));
        }

        private void label7_Click(object sender, EventArgs e)
        {
            if (GlobalVar.LoginStatus)
            {
                //if (GlobalVar.ResetOKLable)
                {
                    mMainstream.ConveyorRun();
                    GlobalVar.bStartRunning = true;
                    GlobalVar.bPauseStatus = false;
                }
               // else
                {
                 //   MessageBox.Show("请先复位", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("请先登录", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // 
        }

        private void label8_Click(object sender, EventArgs e)
        {
            GlobalVar.bPauseStatus = true;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (GlobalVar.LoginStatus)
            {
                startwork_ProcessDisplayingWorker();  //进度条
                Thread.Sleep(500);
                NoticeForm1.SetprogressBarValue = 10;
                NoticeForm1.SetProcessText("工位1复位中……", "工位1复位中……");
                mMainstream.Station1AxisHome();
                NoticeForm1.SetprogressBarValue = 35;
                Thread.Sleep(500);
                NoticeForm1.SetProcessText("工位2复位中……", "工位2复位中……");
                mMainstream.Station2AxisHome();
                NoticeForm1.SetprogressBarValue = 60;
                Thread.Sleep(500);
                NoticeForm1.SetProcessText("工位3复位中……", "工位3复位中……");
                mMainstream.Station3AxisHome();
                NoticeForm1.SetprogressBarValue = 85;
                Thread.Sleep(500);
                NoticeForm1.SetProcessText("气缸复位中……", "工位2气缸复位中……");
                mMainstream.ResetPort1();
                NoticeForm1.SetprogressBarValue = 100;
                NoticeForm1.SetProcessText("完成", "完成");
                Thread.Sleep(500);
                NoticeForm1.CloseResetProcessForm();
                GlobalVar.ResetOKLable = true;
            }
            else
            {
                MessageBox.Show("请先登录", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }          
        }
       
        private void label10_Click(object sender, EventArgs e)
        {
           // ShowInputPanel();
            System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\osk.exe");//调出屏幕键盘
        }
    }
}

