using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace WSSocket
{
    public abstract class AbstractSwitcher
    {
        private CustomerCollector _clientManager;

        public CustomerCollector _ClientManager
        {
            get { return _clientManager; }
            set { _clientManager = value; }
        }

        private byte[] _data;

        public byte[] _Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private Socket _client;

        public Socket _Client
        {
            get { return _client; }
            set { _client = value; }
        }
        
        
        public abstract bool Execute();

        protected void PostBack(byte[] data)
        {
            _Client.Send(data);
        }

        public void StartOnlineCheck()
        {
            #region
            new Thread(new ThreadStart(delegate {
                while (true)
                {
                    Thread.Sleep(30000);
                    if (this._clientManager == null)
                        continue;

                    lock (this._ClientManager._CustomLocker)
                    {
                        this._clientManager._Customers.RemoveAll((c) =>
                        {
                            return (c._UpdateTime.AddSeconds(30) < DateTime.Now);
                        });
                    }
                }
            })) { IsBackground = true }.Start();
            #endregion
        }
    }
}
