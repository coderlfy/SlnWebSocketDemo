/****************************************
***创建人：bhlfy
***创建时间：2014-11-20 15:00:29
***公司：龙浩通信公司
***文件描述：接收登录信息。
*****************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WSSocket
{
    class ReceiverLogon : AbstractReceiver
    {

        protected override bool Receive()
        {
            #region
            if (this._Switcher._ClientManager == null)
                this._Switcher._ClientManager = new CustomerCollector();

            IPEndPoint endremotepoint = (System.Net.IPEndPoint)base._Switcher._Client.RemoteEndPoint;
            Customer customer = new Customer()
            {
                _UId = base._BusinessDataPackage._Entity[0],
                IPAddress = endremotepoint.Address.ToString(),
                Port = endremotepoint.Port,
                _SrcSocket = base._Switcher._Client,
                _LogonTime = DateTime.Now,
                _UpdateTime = DateTime.Now
            };
            lock (this._Switcher._ClientManager._CustomLocker)
            {
                if (this._Switcher._ClientManager.IsExist(customer) == null)
                    this._Switcher._ClientManager.Add(customer);
            }
            return true;
            #endregion
        }
    }
}
