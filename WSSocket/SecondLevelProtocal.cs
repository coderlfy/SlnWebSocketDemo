using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSSocket
{
    public enum SecondLevelProtocal
    {
        M0001,
        M0002,
        M0003,
        M0004,
        NULL
    }
    public class SecondLevelProtocalUtil
    {
        public static string GetName(
            SecondLevelProtocal secondLevel)
        {
            #region
            string discribe = "";
            switch (secondLevel)
            {
                case SecondLevelProtocal.M0001:
                    discribe = "M0001";
                    break;
                case SecondLevelProtocal.M0002:
                    discribe = "M0002";
                    break;
                case SecondLevelProtocal.M0003:
                    discribe = "M0003";
                    break;
                case SecondLevelProtocal.M0004:
                    discribe = "M0004";
                    break;
                default:
                    discribe = "未知";
                    break;
            }
            return discribe;
            #endregion
        }
        public static SecondLevelProtocal Get(
            string secondLevel)
        {
            #region
            SecondLevelProtocal discribe = SecondLevelProtocal.NULL;
            switch (secondLevel)
            {
                case "M0001":
                    discribe = SecondLevelProtocal.M0001;
                    break;
                case "M0002":
                    discribe = SecondLevelProtocal.M0002;
                    break;
                case "M0003":
                    discribe = SecondLevelProtocal.M0003;
                    break;
                case "M0004":
                    discribe = SecondLevelProtocal.M0004;
                    break;
                default:
                    discribe = SecondLevelProtocal.NULL;
                    break;
            }
            return discribe;
            #endregion
        }
    }
}
