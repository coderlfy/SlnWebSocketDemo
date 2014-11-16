using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace WSSocket
{
    public abstract class AbstractSwitcher
    {

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
    }
}
