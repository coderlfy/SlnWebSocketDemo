using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace WSSocket
{
    abstract class AbstractSender
    {
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

        public abstract void Start();
        private string transferSplit(
            string content)
        {
            #region
            return content.Replace(";", "%split");
            #endregion
        }
        /// <summary>
        /// 打包服务器数据
        /// </summary>
        /// <param name="message">数据</param>
        /// <returns>数据包</returns>
        protected byte[] PackData(
            string message)
        {
            #region
            byte[] contentBytes = null;
            byte[] temp = Encoding.UTF8.GetBytes(message);

            if (temp.Length < 126)
            {
                contentBytes = new byte[temp.Length + 2];
                contentBytes[0] = 0x81;
                contentBytes[1] = (byte)temp.Length;
                Array.Copy(temp, 0, contentBytes, 2, temp.Length);
            }
            else if (temp.Length < 0xFFFF)
            {
                contentBytes = new byte[temp.Length + 4];
                contentBytes[0] = 0x81;
                contentBytes[1] = 126;
                contentBytes[2] = (byte)(temp.Length & 0xFF);
                contentBytes[3] = (byte)(temp.Length >> 8 & 0xFF);
                Array.Copy(temp, 0, contentBytes, 4, temp.Length);
            }
            else
            {
                // 暂不处理超长内容  
            }

            return contentBytes;
            #endregion
        }

        protected string GetCMDSplit4(
            string c, FirstLevelProtocal first, 
            SecondLevelProtocal second)
        {
            #region
            string format = "{0};{1};{2};";
            int lengtharealen = 4;
            string temp = string.Format(format,
                FirstLevelProtocalUtil.GetName(first), SecondLevelProtocalUtil.GetName(second), transferSplit(c));

            string length = (temp.Length + lengtharealen).ToString().PadLeft(lengtharealen - 1, '0');
            return string.Format("{0};{1}", length, temp);
            #endregion
        }

    }
}
