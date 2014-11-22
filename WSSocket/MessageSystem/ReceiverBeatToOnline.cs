/****************************************
***创建人：bhlfy
***创建时间：2014-11-20 15:00:29
***公司：龙浩通信公司
***文件描述：接收在线心跳包，更新在线时刻。
*****************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WSSocket
{
    class ReceiverBeatToOnline : AbstractReceiver
    {

        protected override bool Receive()
        {
            #region
            if (this._Switcher._ClientManager == null)
                return false;

            IPEndPoint endremotepoint = (System.Net.IPEndPoint)base._Switcher._Client.RemoteEndPoint;
            Customer customer = new Customer()
            {
                _UId = base._BusinessDataPackage._Entity[0],
                IPAddress = endremotepoint.Address.ToString(),
                Port = endremotepoint.Port
            };
            lock(this._Switcher._ClientManager._CustomLocker)
            { 
                Customer findcustomer = this._Switcher._ClientManager.IsExist(customer);
                if (findcustomer != null)
                    findcustomer._UpdateTime = DateTime.Now;
            }
            return false;
            #endregion
        }
    }
}
