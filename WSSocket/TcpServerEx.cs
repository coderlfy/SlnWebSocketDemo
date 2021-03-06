﻿/****************************************
###创建人：lify
###创建时间：2014-11-20
###公司：龙浩通信公司
###最后修改时间：2011-09-14
###最后修改人：lify
###内容：websocket服务端入口
****************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;

namespace WSSocket
{

    public class TcpServerEx
    {
        /// <summary>
        /// 服务器端的监听器
        /// </summary>
        private Socket _tcpServer = null;
        private bool _isSuccess = false;

        public bool _IsSuccessStarted
        {
            get { return _isSuccess = false; }
        }
        
        /// <summary>
        /// 保存接收到的数据（字节数组）
        /// </summary>
        private byte[] _recvDataBuffer = new byte[65535];
        /// <summary>
        /// 
        /// </summary>
        private readonly static object WriteErrorLogLock = true;
        /// <summary>
        /// 
        /// </summary>
        //public delegate void ErrorHandler(object sender, ErrorEventArgs e);
        //public event EventHandler<ErrorEventArgs> OnError = null;
        //private Thread _thdReceive = null;

        private AbstractSwitcher _receiver;

        public AbstractSwitcher _Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }
        
        /// <summary>
        /// 获取服务端IP列表
        /// </summary>
        /// <returns></returns>
        private static IPAddress[] GetServerIpList()
        {
            #region
            IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());
            return ip;
            #endregion
        }
        public static string GetServerIpString()
        {
            #region
            string ipstring = "";
            foreach (IPAddress ip in GetServerIpList())
                ipstring += "\r\n" + ip.ToString();
            return ipstring;
            #endregion
        }
        private void writeError(Exception e)
        {
            #region
            /*
            lock (WriteErrorLogLock)
            {
                if (OnError != null)
                    OnError(this, new ErrorEventArgs { SocketException = e });
            }
             * */
            #endregion
        }
        /// <summary>
        /// 开始侦听，侦听成功才开启接收
        /// </summary>
        /// <param name="socketResult">socket操作结果</param>
        /// <returns>侦听是否成功</returns>
        public bool StartListen(int serverPort)
        {
            #region
            try
            {
                new Thread(new ThreadStart(delegate { 
                    IPEndPoint localEP = new IPEndPoint(IPAddress.Any, serverPort);
                    _tcpServer = new Socket(localEP.Address.AddressFamily, 
                        SocketType.Stream, ProtocolType.Tcp);
                    _tcpServer.Bind(localEP);
                    _tcpServer.Listen(100);

                    if (_Receiver != null) 
                        _Receiver.StartOnlineCheck();

                    receiveSocket();
                })){
                    IsBackground = true,
                }.Start();
                _isSuccess = true;
                return true;
            }
            catch (Exception e)
            {
                this.writeError(e);
                return false;
            }
            #endregion
        }
        /// <summary>
        /// 从终端接收信息接口
        /// </summary>
        private void receiveSocket()
        {
            #region
            try
            {
                _tcpServer.BeginAccept(new AsyncCallback(acceptConn), 
                    _tcpServer);
            }
            catch (Exception e)
            {
                this.writeError(e);
            }
            #endregion
        }
        /// <summary>
        /// 接收连接（异步阻塞式）
        /// </summary>
        /// <param name="iar"></param>
        private void acceptConn(IAsyncResult iar)
        {
            #region
            try
            {
                Socket oldserver = (Socket)iar.AsyncState;
                Socket client = oldserver.EndAccept(iar);

                _tcpServer.BeginAccept(new AsyncCallback(acceptConn), 
                    _tcpServer);

                client.BeginReceive(_recvDataBuffer, 0, 
                    _recvDataBuffer.Length, SocketFlags.None,
                            new AsyncCallback(receiveData), client);
            }
            catch (SocketException)
            {
                this.receiveSocket();
            }
            catch (Exception e)
            {
                this.writeError(e);
            }
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cacheLength"></param>
        private bool dispatcher(Socket client, int cacheLength)
        {
            #region
            bool clientsuspend = true;
            byte[] temp = new byte[cacheLength];
            Buffer.BlockCopy(_recvDataBuffer, 0, temp, 0, cacheLength);
            IPEndPoint endremotepoint = (System.Net.IPEndPoint)client.RemoteEndPoint;

            if (_Receiver != null)
            {
                _Receiver._Data = temp;
                _Receiver._Client = client;
                clientsuspend = _Receiver.Execute();
            } 
            /*
            TcpServerDispatcher tcpdispatcher = new TcpServerDispatcher(client);
            tcpdispatcher._UserData = new CustomerByteData
            {
                _SourceData = temp.ToList<byte>(),
                _FromClient = new Customer
                {
                    IPAddress = endremotepoint.Address.ToString(),
                    Port = endremotepoint.Port
                }
            };
            viewTempToConsole(tcpdispatcher);
             * tcpdispatcher.Run();
             * */
            return clientsuspend;
            #endregion
        }
        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tcpDispatcher"></param>
        private void viewTempToConsole(TcpServerDispatcher tcpDispatcher)
        {
            #region
            StringBuilder viewcontent = new StringBuilder();
            
            //构造显示数据
            viewcontent.AppendLine(string.Format(
                "数据来自客户端：{0}, 端口：{1}",
                tcpDispatcher._UserData._FromClient.IPAddress,
                tcpDispatcher._UserData._FromClient.Port
                )); 

            for (int i = 0; i < tcpDispatcher._UserData._SourceData.Count; i++)
            {
                byte temp = tcpDispatcher._UserData._SourceData[i];
                viewcontent.Append(string.Format("0x{0} ",temp.ToString("X2"))); 
            }
            Console.WriteLine(viewcontent);
            #endregion
        }
         * */
        /// <summary>
        /// 从客户端接收信息
        /// </summary>
        /// <param name="iar"></param>
        private void receiveData(IAsyncResult iar)
        {
            #region
            Socket client = null;
            try
            {
                client = (Socket)iar.AsyncState;

                int recvcount = client.EndReceive(iar);

                if (recvcount > 0)
                {

                    if (this.dispatcher(client, recvcount))
                    {
                        client.BeginReceive(_recvDataBuffer, 0,
                        _recvDataBuffer.Length, SocketFlags.None,
                                new AsyncCallback(receiveData), client);
                    }
                }
            }
            catch (SocketException e)
            {
                _Receiver._ClientManager.Remove(client);
                this.writeError(e);
            }
            catch (Exception e)
            {
                this.writeError(e);
            }
            #endregion
        }
    }
}
