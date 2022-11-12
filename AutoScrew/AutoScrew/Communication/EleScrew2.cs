using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AutoScrew
{
    class EleScrew2
    {
         private Socket client;
        public string StrIPAddress = "";
        public string receivemsg = string.Empty;
        public static object SendLock = new object();
        private bool ConnectSts = false;
        byte[] buf = new byte[1024];
        public static EleScrew2 mTCPIPClient;
        public static EleScrew2 instance()
        {
            if (mTCPIPClient == null) mTCPIPClient = new EleScrew2();
            return mTCPIPClient;
        }
        public EleScrew2()
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
                string ip = GlobalVar.EleScrew2IP;
                IPAddress ipad = IPAddress.Parse(ip);
                m_ip = new IPEndPoint(ipad, 5001);
                client.Connect(m_ip);
                client.BeginReceive(buf, 0, buf.Length, SocketFlags.None, new AsyncCallback(Receive), client);
            }
            catch
            {

            }
        }
        byte[] tempb1 = new byte[50000];  //存放客户端1的全部字节数据
        int count1 = 0;                    //接收的字节总数
        bool dataSaveFlag1 = false;        //允许保存数据标志
        void Receive(IAsyncResult ia)
        {
            //try
            //{
            //    client = ia.AsyncState as Socket;
            //    int count = client.EndReceive(ia);
            //    client.BeginReceive(buf, 0, buf.Length, SocketFlags.None, new AsyncCallback(Receive), client);
            //    string context = Encoding.ASCII.GetString(buf, 0, count);
            //    if (context.Length > 0)
            //    {
            //        lock (SendLock)
            //        {
            //            processdata(context);
            //        }
            //    }
            //}
            //catch
            //{
            //}
            try
            {
                client = ia.AsyncState as Socket;
                if (client != null && client.Connected)
                {
                    int receiveCount = client.EndReceive(ia);   //锁付数据是分多次发过来的，这是一次缓存触发时的字节数量
                    if (receiveCount > 0)
                    {
                        byte[] data = new byte[receiveCount];
                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = buf[i];
                        }

                        //阅读模式************************************************************
                        if (count1 < 2046)
                        {
                            for (int i = 0; i < receiveCount; i++)
                                tempb1[count1 + i] = data[i];         //加入新接收的数据
                            count1 += receiveCount;                   //使用+=的原因是byRead信息個數是分段進來的
                        }

                        if (count1 >= 2046)
                        {
                            torq_update2(tempb1, count1);   //接收完原始数据后转到二次处理
                            count1 = 0;
                        }

                        client.BeginReceive(buf, 0, buf.Length, SocketFlags.None, new AsyncCallback(Receive), client);
                    }
                    else if (receiveCount == 0)
                    {
                        //bConnect1 = true;
                        //Log("电批1客户端连接失败", listBox1);
                        //ConnStatus_NET(cbxProtocolType1, true, btnConnect1, Color.Gainsboro, "连接");  //更新按钮状态
                    }
                }
            }
            catch (Exception ex)
            {
                //if (ex.Message == "远程主机强迫关闭了一个现有的连接。")
                //{
                //    bConnect1 = true;
                //    Log("电批1客户端连接失败", listBox1);
                //    ConnStatus_NET(cbxProtocolType1, true, btnConnect1, Color.Gainsboro, "连接");  //更新按钮状态
                //}
                //else
                //{
                //    MessageBox.Show(ex.ToString());
                //}
            }
        }

        #region 电批2网络数据处理

        TorqueDataConvert mTorqueDataConvert2 = new TorqueDataConvert();   //处理电批1数据实例对象
        public void torq_update2(byte[] data, int count)   //电批1网络数据
        {
            GlobalVar.originalData2 = new byte[count];
            for (int i = 0; i < count; i++)
            {
                GlobalVar.originalData2[i] = data[i];      //截取原始数据
            }
            //Update_OriginalData1(originalData1);    //保存电批1原始数据

            mTorqueDataConvert2.DataConvert_TCR(data, ref GlobalVar.torq_double_2, ref GlobalVar.Angle_double_2, count, ref GlobalVar.titleAxisY_2, ref GlobalVar.indexSSL2, ref GlobalVar.optID_net_2);   //阅读模式数据解析

            int length = GlobalVar.torq_double_2.Length;
            double[] tempTorque = new double[length];
            double[] tempAngle = new double[length];
            for (int i = 0; i < length; i++)
            {
                tempTorque[i] = Math.Abs(GlobalVar.torq_double_2[i]);  //取扭力绝对值
                tempAngle[i] = Math.Abs(GlobalVar.Angle_double_2[i]);  //取角度绝对值
            }
            GlobalVar.Max_Torque_2 = Math.Round(tempTorque.Max(), 4);  //求最大扭力
            GlobalVar.Max_Angle_2 = Math.Round(tempAngle.Max(), 1);    //求最大角度

            GlobalVar.TKnetDataEnter2 = true;
            //Log("电批1网络数据接收完成，编号：" + optID_net_1, listBox1);
        }

        #endregion











        public void DisconnectTCPIP()
        {
            if (client != null) client.Close();
        }
        public void sendData(string NumberName1, string[] X, string[] Y, string[] R, string E)
        {
            lock (SendLock)
            {
                try
                {
                    byte[] buf1;
                    string data = "";
                    for (int i = 0; i < X.Length; i++)
                    {
                        data += NumberName1 + i + "," + X[i] + "," + Y[i] + "," + R[i] + "," + E + ";";
                    }

                    buf1 = Encoding.ASCII.GetBytes(data);
                    //  buf1 = Encoding.ASCII.GetBytes(NumberName1 + "," + X[0] + "," + Y[0] + "," + R[0] + ";" + NumberName2 + "," + X[1] + "," + Y[1] + "," + R[1] + ";");
                    client.Send(buf1, 0, buf1.Length, SocketFlags.None);
                }
                catch
                {
                }
            }
        }
        private void processdata(string recvdata) //以#号的形式发送命令串#  eg.#Cam1-1 1(\0) 0#
        {
                       
        }
    }
}
