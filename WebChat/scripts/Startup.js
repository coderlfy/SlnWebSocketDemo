Ext.onReady(function () {
    var logoninforusers = [
        { username: 'lifengyan', fullname: '李峰艳' },
        { username: 'liyuerong', fullname: '李悦荣' },
        { username: 'wangmin', fullname: '王敏' }
    ];
    var edithandler = {
        viewlogininfor: function () {
            var users = '';
            for (var u in logoninforusers)
                users += Ext.String.format('用户名：{0}, 用户姓名：{1} <br>', logoninforusers[u].username, logoninforusers[u].fullname);

            Ext.Msg.show({
                title: '系统提示',
                msg: users,
                autoScroll: true,
                icon: Ext.Msg.INFO,
                buttons: Ext.Msg.OK
            });
        },
        buttonlogin: function () {
            var editform = loginform.getForm();
            if (!editform.isValid())
                return;

            var username = loginform.down('#username').getValue();
            var fullname = '';
            for (var u in logoninforusers)
                if (logoninforusers[u].username == username) {
                    fullname = logoninforusers[u].fullname;
                    break;
                }
            if (fullname == ''){
                Ext.Msg.show({
                    title: '错误提示',
                    icon: Ext.Msg.ERROR,
                    msg: '用户名未登记，请点击“点这里看用户信息”试试！',
                    autoScroll: true,
                    buttons: Ext.Msg.OK
                });
                return;
            }
            var sysmsgui = new Sys.MsgStartUp();
            sysmsgui.init(username);
            sysmsgui.setToolBar(fullname);
            loginwindow.close();

        }
    };
    var loginform = Ext.create('Sys.App.TopForm', {
        fieldDefaults: {
            labelWidth: 100, //可微调，以适应不同的界面。
            anchor: '90%',  //控件所占宽度比例，可微调。
            labelAlign: 'right', //标签内容靠左\右
            msgTarget: 'side'
        },
        items: [
            {
                fieldLabel: '用户名',
                itemId: 'username',
                allowBlank: false,
                xtype: 'textfield'
            }
        ]
    });
    var loginwindow = Ext.create('Sys.App.TopWindow', {
        title: '模拟登录',
        width: 400,
        height: 120,
        minWidth: 400,
        minHeight: 120,
        iconCls: Sys.App.Icon.login,
        items: loginform,
        buttons: [
            { minWidth: 80, text: '登录', handler: edithandler.buttonlogin },
            { minWidth: 80, text: '点这里看用户信息', handler: edithandler.viewlogininfor }
        ]
    });

    loginwindow.show();
    var map = new Ext.util.KeyMap({
        target: loginwindow.getId(),
        binding: [{
            key: Ext.EventObject.ENTER,
            fn: edithandler.buttonlogin
        }]
    });
});
