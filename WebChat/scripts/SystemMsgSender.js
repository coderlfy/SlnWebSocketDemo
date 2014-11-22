//系统消息数据构建器
(function () {

    MsgSenderLevel1 = {
        NormalMsg: 'F001',
    };
    MsgSenderLevel2 = {
        login: 'M0001',
        loudly: 'M0002',
        p2p: 'M0003',
        beat: 'M0004'
};
    Sys.MsgSenderConfig = {
        lengthAreaLength: 4,
        level1Length: 5,
        level2Length: 6,
        splitMarkupChar: ';'
    };
    function transferSplit(content) {
        return content.replace(/;/gi, '%split');
    }
    function getCMDSplit4WithoutTransfer(content, first, second) {
        var format = '{0};{1};{2};';
        var lengtharealen = Sys.MsgSenderConfig.lengthAreaLength;
        var temp = Ext.String.format(format,
            first, second, content);

        var length = Ext.String.leftPad(temp.length + lengtharealen, lengtharealen - 1, '0');
        return Ext.String.format('{0};{1}', length, temp);
    }
    function getCMDSplit4(content, first, second) {
        return getCMDSplit4WithoutTransfer(transferSplit(content), first, second);
    }
    function getCMDSplitForMoreContent(contents, first, second) {
        var content = '';
        var lengtharealen = Sys.MsgSenderConfig.lengthAreaLength;

        for (var c in contents) {
            content += Ext.String.format('{0};', transferSplit(contents[c]));
        }

        if (content.length > 0)
            content = content.substr(0, content.length - 1);

        return getCMDSplit4WithoutTransfer(content, first, second);
    }
    Sys.MsgSender = function () {
        this.getLogin = function (username) {
            return getCMDSplit4(username,
                MsgSenderLevel1.NormalMsg,
                MsgSenderLevel2.login);
        };
        this.getLoudly = function (cArr) {

            return getCMDSplitForMoreContent(cArr,
                MsgSenderLevel1.NormalMsg,
                MsgSenderLevel2.loudly);
        };
        this.getP2P = function (cArr) {
            return getCMDSplitForMoreContent(cArr,
                MsgSenderLevel1.NormalMsg,
                MsgSenderLevel2.p2p);
        };
        this.getBeat = function (username) {
            return getCMDSplit4(username,
                MsgSenderLevel1.NormalMsg,
                MsgSenderLevel2.beat);
        }
    }
})();