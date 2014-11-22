using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace WSSocket
{
    abstract class AbstractReceiver
    {
        private const char _splitMarkupChar = ';';
        private AbstractSwitcher _switcher;

        public AbstractSwitcher _Switcher
        {
            get { return _switcher; }
            set { _switcher = value; }
        }
        
        private BusinessDataPackage _businessDataPackage;

        public BusinessDataPackage _BusinessDataPackage
        {
            get { return _businessDataPackage; }
            set { _businessDataPackage = value; }
        }


        public bool Start()
        {
            #region
            if (_BusinessDataPackage == null) { 
                Console.WriteLine("error：未给receiver传入业务数据！");
                return true;
            }
            this._businessDataPackage._Entity = this._businessDataPackage
                ._EntityData.Split(_splitMarkupChar).ToList<string>();

            for (int i = 0; i < this._businessDataPackage._Entity.Count; i++)
                this._businessDataPackage._Entity[i] = 
                    this._businessDataPackage._Entity[i].Replace("%split", ";");

            return this.Receive();
            #endregion
        }
        /// <summary>
        /// 解析实体数据
        /// </summary>
        /// <returns>返回是否要回复</returns>
        protected abstract bool Receive();
    }
}
