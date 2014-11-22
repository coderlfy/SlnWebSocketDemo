using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSSocket
{
    class BusinessDataPackage
    {
        private FirstLevelProtocal _fistLevelProtocal;
        /// <summary>
        /// 
        /// </summary>
        public FirstLevelProtocal _FirstLevelProtocal
        {
            get { return _fistLevelProtocal; }
            set { _fistLevelProtocal = value; }
        }

        private SecondLevelProtocal _sencondLevelProtocal;
        /// <summary>
        /// 
        /// </summary>
        public SecondLevelProtocal _SencondLevelProtocal
        {
            get { return _sencondLevelProtocal; }
            set { _sencondLevelProtocal = value; }
        }

        private int _packageLength;
        /// <summary>
        /// 
        /// </summary>
        public int _PackageLength
        {
            get { return _packageLength; }
        }

        private string _metaData;
        /// <summary>
        /// 元数据
        /// </summary>
        public string _MetaData
        {
            get { return _metaData; }
        }

        private string _entityData;

        public string _EntityData
        {
            get { return _entityData; }
            set { _entityData = value; }
        }
        private List<string> _entity;

        public List<string> _Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }


        private BusinessSplitResult _splitResult = BusinessSplitResult.haventSplit;

        public BusinessSplitResult _SplitResult
        {
            get { return _splitResult; }
        }

        public bool IsValid()
        {
            return _splitResult == BusinessSplitResult.Ok;
        }
        public BusinessDataPackage(string metaData)
        {
            _metaData = metaData;
            _splitResult = this.split();
            if (_splitResult != BusinessSplitResult.Ok)
                Console.WriteLine("错误发生，错误类型为：{0}", _splitResult);

        }

        private const int _metaDataLengthArea = 4;
        private const int _fistLevelProtocalLengthArea = 5;
        private const int _sencondLevelProtocalLengthArea = 6;
        private const string _splitMarkupChar = ";";
        private BusinessSplitResult split()
        {
            #region
            BusinessSplitResult result = this.getMetaDataLength();
            if (result == BusinessSplitResult.Ok)
            { 
                result = this.getFirstProtocal();
                if (result == BusinessSplitResult.Ok)
                { 
                    result = this.getSecondProtocal();
                    if (result == BusinessSplitResult.Ok)
                        result = this.getEntityData();
                }
            }
            return result;
            #endregion
        }

        private BusinessSplitResult getEntityData()
        {
            #region
            int entitydatastartindex = _metaDataLengthArea+_fistLevelProtocalLengthArea+_sencondLevelProtocalLengthArea;
            this._entityData = this._metaData.Substring(entitydatastartindex, _packageLength - entitydatastartindex);
            return BusinessSplitResult.Ok;
            #endregion
        }

        private BusinessSplitResult getFirstProtocal()
        {
            #region
            int tempindex = 0;
            string tempname = "";

            tempindex = _metaDataLengthArea + _fistLevelProtocalLengthArea;
            if (this._metaData.Length < tempindex)
                return BusinessSplitResult.metaDataTooShort;

            if (this._metaData.Substring(tempindex - 1, 1)
                != _splitMarkupChar)
                return BusinessSplitResult.metaDataLengthSplitArea;

            tempname = this._metaData.Substring(_metaDataLengthArea, 
                _fistLevelProtocalLengthArea - 1);

            this._fistLevelProtocal = FirstLevelProtocalUtil.Get(tempname);
            if (this._fistLevelProtocal == FirstLevelProtocal.NULL)
                return BusinessSplitResult.nullFirstLevelProtocal;

            return BusinessSplitResult.Ok;
            #endregion
        }

        private BusinessSplitResult getSecondProtocal()
        {
            #region
            int tempindex = 0;
            string tempname = "";

            tempindex = _metaDataLengthArea + _fistLevelProtocalLengthArea + _sencondLevelProtocalLengthArea;
            if (this._metaData.Length < tempindex)
                return BusinessSplitResult.metaDataTooShort;

            if (this._metaData.Substring(tempindex - 1, 1)
                != _splitMarkupChar)
                return BusinessSplitResult.metaDataLengthSplitArea;

            tempname = this._metaData.Substring(_metaDataLengthArea + _fistLevelProtocalLengthArea,
                _sencondLevelProtocalLengthArea - 1);

            this._sencondLevelProtocal = SecondLevelProtocalUtil.Get(tempname);
            if (this._sencondLevelProtocal == SecondLevelProtocal.NULL)
                return BusinessSplitResult.nullSecondLevelProtocal;

            return BusinessSplitResult.Ok;
            #endregion
        }

        private BusinessSplitResult getMetaDataLength()
        {
            #region
            if (this._metaData.Length < _metaDataLengthArea)
                return BusinessSplitResult.metaDataTooShort;

            if (this._metaData.Substring(_metaDataLengthArea - 1, 1)
                != _splitMarkupChar)
                return BusinessSplitResult.metaDataLengthSplitArea;

            if (!int.TryParse(this._metaData.Substring(0, _metaDataLengthArea - 1)
                , out _packageLength))
                return BusinessSplitResult.metaDataLengthIsNotInt;

            if (_packageLength != this._metaData.Length)
                return BusinessSplitResult.metaDataLoseBit;

            return BusinessSplitResult.Ok;
            #endregion
        }
    }

    enum BusinessSplitResult
    { 
        Ok,
        metaDataTooShort,
        metaDataLoseBit,
        metaDataLengthSplitArea,
        metaDataLengthIsNotInt,
        nullFirstLevelProtocal,
        nullSecondLevelProtocal,
        haventSplit
    }

    class BusinessSplitResultDiscribe 
    {
        public static string Get(BusinessSplitResult result)
        {
            return "未知错误，还未实现哦！";
        }
    }
}
