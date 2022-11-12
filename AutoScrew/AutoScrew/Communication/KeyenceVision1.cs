using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AutoScrew
{
    class KeyenceVision1
    {

        private Socket client;
        public string StrIPAddress = "";
        public string receivemsg = string.Empty;
        public static object SendLock = new object();
        private bool ConnectSts = false;
        byte[] buf = new byte[1024];
        public static KeyenceVision1 mTCPIPClient;
        public static KeyenceVision1 instance()
        {
            if (mTCPIPClient == null) mTCPIPClient = new KeyenceVision1();
            return mTCPIPClient;
        }
        public KeyenceVision1()
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
                string ip = "192.168.0.10";
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
                GlobalVar.Instance.UpdateRunMessage("PC收到基恩士控制器1发来的：" + context, LogName.runLog);
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
                    GlobalVar.Instance.UpdateRunMessage("PC发送：" + StrData + "给基恩士控制器1", LogName.runLog);
                }
                catch (Exception e)
                {
                    GlobalVar.Instance.UpdateRunMessage("PC发送向基恩士控制器1发送数据失败：" + e.Message, LogName.runLog);
                }
            }
        }
        private void processdata(string recvdata) //以#号的形式发送命令串#  eg.#Cam1-1 1(\0) 0#
        {
            string[] singlecmddata = recvdata.Split(new char[] { ',' });
            string result = singlecmddata[0];    //标定与生产标志 1    27
            string result1 = singlecmddata[1];   //标定结果标志或者生产标志数据
            
            switch (result)
            {
                case "1":
                    #region 相机标定
                    switch (result1)
                    {
                        case "0":
                            GlobalVar.CCD1Calibration.Add("OK");
                            break;
                        case "1":
                            GlobalVar.CCD1Calibration.Add("Finish");
                            break;
                        case "2":
                            GlobalVar.CCD1Calibration.Add("Alarm");
                            break;
                    }
                    #endregion
                    break;
                case "27":
                    #region 相机同心度判断
                    string result2 = singlecmddata[5];   //生产时  相机对应螺丝孔标志
                    switch (result2)
                    {
                        case "3":
                            GlobalVar.Station1Screw3.Add(double.Parse(singlecmddata[2]));    //记得清除
                            GlobalVar.Station1Screw3.Add(double.Parse(singlecmddata[3]));
                            GlobalVar.Station1Screw3.Add(double.Parse(singlecmddata[4]));
                            GlobalVar.Station1CameraResult[0] = true;
                            break;
                        case "4":
                            GlobalVar.Station1Screw4.Add(double.Parse(singlecmddata[2]));    //记得清除
                            GlobalVar.Station1Screw4.Add(double.Parse(singlecmddata[3]));
                            GlobalVar.Station1Screw4.Add(double.Parse(singlecmddata[4]));
                            GlobalVar.Station1CameraResult[1] = true;
                            break;
                        case "5":
                            GlobalVar.Station1Screw5.Add(double.Parse(singlecmddata[2]));    //记得清除
                            GlobalVar.Station1Screw5.Add(double.Parse(singlecmddata[3]));
                            GlobalVar.Station1Screw5.Add(double.Parse(singlecmddata[4]));
                            GlobalVar.Station1CameraResult[2] = true;
                            break;
                        case "6":
                            GlobalVar.Station1Screw6.Add(double.Parse(singlecmddata[2]));    //记得清除
                            GlobalVar.Station1Screw6.Add(double.Parse(singlecmddata[3]));
                            GlobalVar.Station1Screw6.Add(double.Parse(singlecmddata[4]));
                            GlobalVar.Station1CameraResult[3] = true;
                            break;
                    }
                    #endregion
                    break;
            }
        }
    }
}
