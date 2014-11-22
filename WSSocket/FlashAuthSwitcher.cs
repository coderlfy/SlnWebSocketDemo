using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSSocket
{
    public class FlashAuthSwitcher: AbstractSwitcher
    {
        private string webflashpolicy = "<cross-domain-policy><site-control permitted-cross-domain-policies=\"all\"/><allow-access-from domain=\"*\" to-ports=\"*\" /></cross-domain-policy> \0";

        public override bool Execute()
        {
            if (base._Data != null)
            {
                string handShakeText = Encoding.UTF8.GetString(base._Data);
                if (handShakeText.IndexOf("<policy-file-request/>") != -1)
                    base.PostBack(Encoding.UTF8.GetBytes(webflashpolicy));
            }
            return true;
        }
    }
}
