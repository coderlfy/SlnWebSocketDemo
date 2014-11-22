//系统消息数据构建器
(function () {

    MsgReceiverLevel1 = {
        Data: [
            { protocal: 'F001', handler: 'normalMsg' },
            { protocal: 'F002', handler: 'sysMsg' },
        ],
        Get: function (protocal) {
            for (var o in this.Data) {
                if (protocal == this.Data[o].protocal)
                    return this.Data[o];
            }
            return null;
        }
    };
    MsgReceiverLevel2 = {
        Data: [
            { protocal: 'M0001', handler: 'normalMsg' },
            { protocal: 'M0002', handler: 'sysMsg' },
        ],
        Get: function (protocal) {
            for (var o in this.Data) {
                if (protocal == this.Data[o].protocal)
                    return this.Data[o];
            }
            return null;
        }
    };
    Sys.MsgBusinessSplitResult = 
    { 
        Ok: 'Ok',
        metaDataTooShort: 'metaDataTooShort',
        metaDataLoseBit: 'metaDataLoseBit',
        metaDataLengthSplitArea: 'metaDataLengthSplitArea',
        metaDataLengthIsNotInt: 'metaDataLengthIsNotInt',
        nullFirstLevelProtocal: 'nullFirstLevelProtocal',
        nullSecondLevelProtocal: 'nullSecondLevelProtocal',
        haventSplit: 'haventSplit'
    };

    Sys.MsgReceiverConfig = {
        lengthAreaLength : 4,
        level1Length : 5,
        level2Length : 6,
        splitMarkupChar : ';'
    };
    Sys.MsgReceiverData = function () {
        this.receLength = 0;
        this.level1 = null;
        this.level2 = null;
        this.businessData = [];
        this.isSuccess = false;
        this.error = null;
    };

    function getMetaDataLength(result, objresult)
    {
        var lengtharealen = Sys.MsgReceiverConfig.lengthAreaLength;
        if (result.length < lengtharealen)
            return Sys.MsgBusinessSplitResult.metaDataTooShort;

        if (result.substr(lengtharealen - 1, 1)
            != Sys.MsgReceiverConfig.splitMarkupChar)
            return Sys.MsgBusinessSplitResult.metaDataLengthSplitArea;

        var resultlen = parseInt(result.substr(0, lengtharealen - 1), 10);
        if (resultlen == 0 || resultlen == NaN)
            return Sys.MsgBusinessSplitResult.metaDataLengthIsNotInt;

        //console.log(Ext.String.format('转化后的长度：{0}', resultlen));
        //console.log(Ext.String.format('实际长度：{0}', result.length));
        if (resultlen != result.length)
            return Sys.MsgBusinessSplitResult.metaDataLoseBit;
        objresult.receLength = resultlen;

        return Sys.MsgBusinessSplitResult.Ok;
    }

    function getFirstProtocal(result, objresult)
    {
        var tempindex = 0;
        var tempname = "";

        tempindex = Sys.MsgReceiverConfig.lengthAreaLength + Sys.MsgReceiverConfig.level1Length;
        if (result.length < tempindex)
            return Sys.MsgBusinessSplitResult.metaDataTooShort;

        if (result.substr(tempindex - 1, 1)
            != Sys.MsgReceiverConfig.splitMarkupChar)
            return Sys.MsgBusinessSplitResult.metaDataLengthSplitArea;

        tempname = result.substr(Sys.MsgReceiverConfig.lengthAreaLength,
            Sys.MsgReceiverConfig.level1Length - 1);

        objresult.level1 = MsgReceiverLevel1.Get(tempname);
        if (!objresult.level1)
            return Sys.MsgBusinessSplitResult.nullFirstLevelProtocal;

        return Sys.MsgBusinessSplitResult.Ok;
    }

    function getSecondProtocal(result, objresult)
    {
        var tempindex = 0;
        var tempname = "";

        tempindex = Sys.MsgReceiverConfig.lengthAreaLength + Sys.MsgReceiverConfig.level1Length + Sys.MsgReceiverConfig.level2Length;
        if (result.length < tempindex)
            return Sys.MsgBusinessSplitResult.metaDataTooShort;

        if (result.substr(tempindex - 1, 1)
            != Sys.MsgReceiverConfig.splitMarkupChar)
            return Sys.MsgBusinessSplitResult.metaDataLengthSplitArea;

        tempname = result.substr(Sys.MsgReceiverConfig.lengthAreaLength + Sys.MsgReceiverConfig.level1Length,
            Sys.MsgReceiverConfig.level2Length - 1);

        objresult.level2 = MsgReceiverLevel2.Get(tempname);
        if (!objresult.level2)
            return Sys.MsgBusinessSplitResult.nullSecondLevelProtocal;

        return Sys.MsgBusinessSplitResult.Ok;
    }


    Sys.MsgReceiver = function () {

        this.analysis = function (result) {
            var objresult = new Sys.MsgReceiverData();
            var splitresult = null;
            splitresult = getMetaDataLength(result, objresult);
            if (splitresult == Sys.MsgBusinessSplitResult.Ok)
            { 
                splitresult = getFirstProtocal(result, objresult);
                if (splitresult == Sys.MsgBusinessSplitResult.Ok)
                    splitresult = getSecondProtocal(result, objresult);
            }

            objresult.isSuccess = (splitresult == Sys.MsgBusinessSplitResult.Ok);
            (objresult.isSuccess) ? '' : splitresult;
            if (objresult.isSuccess) {
                var entitydatastartindex = Sys.MsgReceiverConfig.lengthAreaLength
                    + Sys.MsgReceiverConfig.level1Length
                    + Sys.MsgReceiverConfig.level2Length;

                objresult.businessData = result.substr(entitydatastartindex,
                    objresult.receLength - entitydatastartindex)
                    .split(Sys.MsgReceiverConfig.splitMarkupChar);
            } else
                objresult.error = splitresult;

            return objresult;
            
        }
    };
})();