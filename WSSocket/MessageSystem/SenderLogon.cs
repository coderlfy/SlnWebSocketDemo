using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSSocket
{
    class SenderLogon : AbstractSender
    {

        public override void Start()
        {
            base._Switcher._Client.Send(base.PackData("020;F001;M0001;good;"));
        }
    }
}
