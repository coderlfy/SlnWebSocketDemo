using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSSocket
{
    public enum FirstLevelProtocal
    {
        F001,
        F002,
        NULL
    }

    public class FirstLevelProtocalUtil
    {
        public static string GetName(
            FirstLevelProtocal firstLevel)
        {
            #region
            string discribe = "";
            switch (firstLevel)
            { 
                case FirstLevelProtocal.F001:
                    discribe = "F001";
                    break;
                default:
                    discribe = "未知";
                    break;
            }
            return discribe;
            #endregion
        }
        public static FirstLevelProtocal Get(
            string firstLevel)
        {
            #region
            FirstLevelProtocal discribe = FirstLevelProtocal.NULL;
            switch (firstLevel)
            {
                case "F001":
                    discribe = FirstLevelProtocal.F001;
                    break;
                default:
                    discribe = FirstLevelProtocal.NULL;
                    break;
            }
            return discribe;
            #endregion
        }
    }
}
