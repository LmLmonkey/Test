using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AutoScrew
{
    class KeyenceVision2
    {
        private Socket client;
        public string StrIPAddress = "";
        public string receivemsg = string.Empty;
        public static object SendLock = new object();
        private bool ConnectSts = false;
        byte[] buf = new byte[1024];
        public static KeyenceVision2 mTCPIPClient;
        public static KeyenceVision2 instance()
        {
            if (mTCPIPClient == null) mTCPIPClient = new KeyenceVision2();
            return mTCPIPClient;
        }
        public KeyenceVision2()
        {

        }
        public bool GetTCPIPConnectSts
        {
            get
            {
                if (client != null)
                {
                    ConnectSts = client.Connected;
                }
                else
                {
                    ConnectSts = false;
                }
                return ConnectSts;
            }
        }
        public void TheTCPIPConnect()
        {
            if (client != null && client.Connected)
            {
                client.Close();
            }
            try
            {
                IPEndPoint m_ip;
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                string ip = "192.168.1.20";
                IPAddress ipad = IPAddress.Parse(ip);
                m_ip = new IPEndPoint(ipad, 8500);
                client.Connect(m_ip);
                client.BeginReceive(buf, 0, buf.Length, SocketFlags.None, new AsyncCallback(Receive), client);
            }
            catch
            {

            }
        }
        void Receive(IAsyncResult ia)
        {
            try
            {
                client = ia.AsyncState as Socket;
                int count = client.EndReceive(ia);
                client.BeginReceive(buf, 0, buf.Length, SocketFlags.None, new AsyncCallback(Receive), client);
                string context = Encoding.ASCII.GetString(buf, 0, count);
                GlobalVar.Instance.UpdateRunMessage("PC收到基恩士控制器2发来的：" + context, LogName.runLog);
                if (context.Length > 0)
                {
                    lock (SendLock)
                    {
                        processdata(context);
                    }
                }
            }
            catch (Exception e)
            {
                //GlobalVar.Instance.UpdateRunMessage("PC收到恩士扫码枪发来的：" + context, LogName.runLog);
            }
        }

        public void DisconnectTCPIP()
        {
            if (client != null) client.Close();
        }
        public void sendData(string StrData)
        {
            lock (SendLock)
            {
                try
                {
                    byte[] buf1;
                    buf1 = Encoding.ASCII.GetBytes(StrData + "\r");
                    client.Send(buf1, 0, buf1.Length, SocketFlags.None);
                    GlobalVar.Instance.UpdateRunMessage("PC发送：" + StrData + "给基恩士控制器2", LogName.runLog);
                }
                catch (Exception e)
                {
                    GlobalVar.Instance.UpdateRunMessage("PC发送向基恩士控制器2发送数据失败：" + e.Message, LogName.runLog);
                }
            }
        }
        private void processdata(string recvdata) //以#号的形式发送命令串#  eg.#Cam1-1 1(\0) 0#
        {
            string[] singlecmddata = recvdata.Split(new char[] { ',' });
            string result = singlecmddata[0];    //标定与生产标志 2  3  4     生产 25 26 ……

            switch (result)
            {
                case "2":
                    #region 2号相机标定
                    switch (result)
                    {
                        case "0":
                            GlobalVar.CCD2Calibration.Add("OK");
                            break;
                        case "1":
                            GlobalVar.CCD2Calibration.Add("Finish");
                            break;
                        case "2":
                            GlobalVar.CCD2Calibration.Add("Alarm");
                            break;
                    }
                    #endregion
                    break;
                case "3":
                    #region 三号相机标定
                    switch (result)
                    {
                        case "0":
                            GlobalVar.CCD3Calibration.Add("OK");
                            break;
                        case "1":
                            GlobalVar.CCD3Calibration.Add("Finish");
                            break;
                        case "2":
                            GlobalVar.CCD3Calibration.Add("Alarm");
                            break;
                    }
                    #endregion
                    break;
                case "4":
                    #region  四号相机标定
                    switch (result)
                    {
                        case "0":
                            GlobalVar.CCD4Calibration.Add("OK");
                            break;
                        case "1":
                            GlobalVar.CCD4Calibration.Add("Finish");
                            break;
                        case "2":
                            GlobalVar.CCD4Calibration.Add("Alarm");
                            break;
                    }
                    #endregion
                    break;
                case "25":
                    #region 3螺丝孔定位数据
                    GlobalVar.Station2Screw3Location.Add(double.Parse(singlecmddata[1]));  //X
                    GlobalVar.Station2Screw3Location.Add(double.Parse(singlecmddata[2]));   //Y
                    GlobalVar.Station2CameraResult[0] = true;
                    #endregion
                    break;
                case "26":
                    #region 3螺丝孔复检数据
                    GlobalVar.Station2Screw3ReCheck.Add(double.Parse(singlecmddata[1]));  //X
                    GlobalVar.Station2Screw3ReCheck.Add(double.Parse(singlecmddata[2]));   //Y
                    GlobalVar.Station2Screw3ReCheck.Add(double.Parse(singlecmddata[3]));   //CC
                    #endregion
                    break;
                case "21":
                    #region 3螺丝孔定位数据
                    GlobalVar.Station2Screw4Location.Add(double.Parse(singlecmddata[1]));  //X
                    GlobalVar.Station2Screw4Location.Add(double.Parse(singlecmddata[2]));   //Y
                    #endregion
                    break;
                case "27":    //为什么用26 ???
                    #region 3螺丝孔复检数据
                    GlobalVar.Station2Screw4ReCheck.Add(double.Parse(singlecmddata[1]));  //X
                    GlobalVar.Station2Screw4ReCheck.Add(double.Parse(singlecmddata[2]));   //Y
                    GlobalVar.Station2Screw4ReCheck.Add(double.Parse(singlecmddata[3]));   //CC
                    #endregion
                    break;
                case "29":
                    #region 3螺丝孔定位数据
                    GlobalVar.Station3Screw5Location.Add(double.Parse(singlecmddata[1]));  //X
                    GlobalVar.Station3Screw5Location.Add(double.Parse(singlecmddata[2]));   //Y
                    GlobalVar.Station3CameraResult[0] = true;
                    #endregion
                    break;
                case "30":    
                    #region 3螺丝孔复检数据
                    GlobalVar.Station3Screw5ReCheck.Add(double.Parse(singlecmddata[1]));  //X
                    GlobalVar.Station3Screw5ReCheck.Add(double.Parse(singlecmddata[2]));   //Y
                    GlobalVar.Station3Screw5ReCheck.Add(double.Parse(singlecmddata[3]));   //CC
                    #endregion
                    break;
                case "33":
                    #region 3螺丝孔定位数据
                    GlobalVar.Station3Screw6Location.Add(double.Parse(singlecmddata[1]));  //X
                    GlobalVar.Station3Screw6Location.Add(double.Parse(singlecmddata[2]));   //Y
                    GlobalVar.Station3CameraResult[1] = true;
                    #endregion
                    break;
                case "34":   
                    #region 3螺丝孔复检数据
                    GlobalVar.Station3Screw6ReCheck.Add(double.Parse(singlecmddata[1]));  //X
                    GlobalVar.Station3Screw6ReCheck.Add(double.Parse(singlecmddata[2]));   //Y
                    GlobalVar.Station3Screw6ReCheck.Add(double.Parse(singlecmddata[3]));   //CC
                    #endregion
                    break;
            }
        }
    }
}
