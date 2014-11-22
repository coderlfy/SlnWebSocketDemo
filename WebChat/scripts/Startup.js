Ext.onReady(function () {
    var sysmsgui = new Sys.MsgStartUp();
    sysmsgui.init(Sys.Paramters.userid);
    sysmsgui.setToolBar(Sys.Paramters.fullname);
});
