using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WSSocket
{
    public class WSServerSwitcher : AbstractSwitcher
    {
        private AbstractReceiver _receiver = null;
        private AbstractSender _sender = null;
        
        public override bool Execute()
        {
            #region
            if (base._Data != null)
            {
                WebSocketHandler websocket = new WebSocketHandler();
                string handShakeText = Encoding.UTF8.GetString(base._Data);
                //与websocket客户端握手
                if (handShakeText.IndexOf("GET /MessageSystem HTTP/1.1") != -1)
                {
                    base.PostBack(websocket.PackHandShakeData(
                        base._Data, base._Data.Length));
                }
                else
                {
                    string metadata = websocket.AnalyticData(
                        base._Data, base._Data.Length);

                    Console.WriteLine("接收到数据：{0}, 时刻：{1}", metadata, DateTime.Now);

                    BusinessDataPackage businessdata = 
                        new BusinessDataPackage(metadata);

                    if (businessdata.IsValid())
                    { 
                        this._receiver = this.getReceiver(businessdata);
                        if(this._receiver == null)
                            return true;
                        if(this._receiver.Start())
                        {
                            this._sender = this.getSender(businessdata);
                            if (this._sender == null)
                                return true;
                            this._sender.Start();
                        }
                    }
                }
            }
            return true;
            #endregion
        }

        private AbstractReceiver getReceiver(
            BusinessDataPackage businessData)
        {
            #region
            AbstractReceiver receiver = null;
            switch (businessData._SencondLevelProtocal)
            {
                case SecondLevelProtocal.M0001:
                    receiver = new ReceiverLogon();
                    break;
                case SecondLevelProtocal.M0002:
                    receiver = new ReceiverLoudly();
                    break;
                case SecondLevelProtocal.M0004:
                    receiver = new ReceiverBeatToOnline();
                    break;
            }
            receiver._BusinessDataPackage = businessData;
            receiver._Switcher = this;
            return receiver;
            #endregion
        }

        private AbstractSender getSender(
            BusinessDataPackage businessData)
        {
            #region
            AbstractSender sender = null;
            switch (businessData._SencondLevelProtocal)
            {
                case SecondLevelProtocal.M0001:
                    sender = new SenderLogon();
                    break;
                case SecondLevelProtocal.M0002:
                    sender = new SenderLoudly();
                    break;
            }
            sender._BusinessDataPackage = businessData;
            sender._Switcher = this;
            return sender;
            #endregion
        }
    }
}
