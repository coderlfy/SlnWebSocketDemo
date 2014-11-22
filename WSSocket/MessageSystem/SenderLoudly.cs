using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace WSSocket
{
    class SenderLoudly : AbstractSender
    {

        public override void Start()
        {
            #region
            string content = "";
            lock (base._Switcher._ClientManager._CustomLocker)
            {
                foreach (Customer c in base._Switcher._ClientManager._Customers)
                {
                    content = (c._SrcSocket == base._Switcher._Client)
                        ?string.Format("已将消息“{0}”发送给大家！", this._BusinessDataPackage._Entity[0])
                        : string.Format("来自“{0}”的消息“{1}”", this._BusinessDataPackage._Entity[1], this._BusinessDataPackage._Entity[0]);

                    string msg = base.GetCMDSplit4(content, FirstLevelProtocal.F001, SecondLevelProtocal.M0002);

                    SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                    byte[] buffer = base.PackData(msg);
                    e.SetBuffer(buffer, 0, buffer.Length);
                    e.Completed += e_Completed;

                    if (!c._SrcSocket.SendAsync(e))
                        e_Completed(this, e);
                }
            }
            #endregion
        }

        private void e_Completed(
            object sender, SocketAsyncEventArgs e)
        {
            #region
            if (e.SocketError != SocketError.Success)
                Console.WriteLine("Socket Error: {0}", e.SocketError);
            #endregion
        }
    }
}
