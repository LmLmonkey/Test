using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AutoScrew
{
    class KeyenceBarCode
    {
        private Socket client;
        public string StrIPAddress = "";
        public string receivemsg = string.Empty;
        public static object SendLock = new object();
        private bool ConnectSts = false;
        byte[] buf = new byte[1024];
        public static KeyenceBarCode mTCPIPClient;
        public static KeyenceBarCode instance()
        {
            if (mTCPIPClient == null) mTCPIPClient = new KeyenceBarCode();
            return mTCPIPClient;
        }
        public KeyenceBarCode()
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
                string ip = GlobalVar.KeyenceBarCodeIP;
                IPAddress ipad = IPAddress.Parse(ip);
                m_ip = new IPEndPoint(ipad, 9004);
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
                GlobalVar.Instance.UpdateRunMessage("PC收到基恩士扫码枪发来的：" + context, LogName.runLog);
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
                    GlobalVar.Instance.UpdateRunMessage("PC发送：" + StrData + "给基恩士扫码枪", LogName.runLog);
                }
                catch (Exception e)
                {
                    GlobalVar.Instance.UpdateRunMessage("PC发送向基恩士扫码枪发送数据失败：" + e.Message, LogName.runLog);
                }
            }
        }
        private void processdata(string recvdata) //以#号的形式发送命令串#  eg.#Cam1-1 1(\0) 0#
        {

        }
    }
}
