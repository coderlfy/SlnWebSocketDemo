(function () {
    var heartBeat = 10000;
    var hbTimer = undefined;
    var sysMsgRequestTimer = undefined;
    function keepAlive(sysmsg) {
        if (!hbTimer) {
            hbTimer = window.setInterval(function () {
                sysmsg.beatServer();
            }, heartBeat);
        }
    }
    
    function stopHeartBeat() {

        if (hbTimer) {
            window.clearInterval(hbTimer);
            hbTimer = undefined;
        }

    }
    
    Sys.Msg = function (options) {
        var ws = null;
        var sender = new Sys.MsgSender();
        var receiver = new Sys.MsgReceiver();
        var opts = {};
        Ext.apply(opts, options);
        this.isOpen = false;
        this.Executer = null;
        this.init = function (username) {
            var me = this;
            if (username == null) {
                console.log('需要参数：使用系统消息的用户:' + username + '！');
                return;
            }
            me.Executer = username;
            ws = new Ext.WebSocket({
                domain: Sys.MsgConfig.serverDomain,
                protocol: Sys.MsgConfig.protocol,
                port: Sys.MsgConfig.port,
                onOpen: function (event) {
                    stopHeartBeat();
                    ws.send(sender.getLogin(username));
                    keepAlive(me);
                },
                onError: function (event) {
                    console.log(event)
                },
                onMessage: function (result) {
                    var recedata = receiver.analysis(result);
                    me.isOpen = true;
                    if (opts.onReceive) {
                        if (recedata.isSuccess) {
                            for (var i = 0; i < recedata.businessData.length; i++)
                                recedata.businessData[i] = recedata.businessData[i].replace(/%split/gi, ';');
                            opts.onReceive(recedata);
                        }
                    }

                },
                onClose: function (event) {
                    ws = null;
                    if (!me.isOpen) {
                        if (!Ext.isIE)
                            window.setTimeout(function () {
                                me.init(me.Executer);
                            }, heartBeat);
                        else
                            me.init(me.Executer);
                    }
                }
            });
        };

        this.loudly = function (content, from) {
            if (this.checkMsgPipe(this)) {
                ws.send(sender.getLoudly([content, from]));
            }
        };
        this.p2p = function (dest, content) {
            var me = this;
            if (me.checkMsgPipe(me)) {
                var carr = [me.Executer, dest, content];
                ws.send(sender.getP2P(carr));
            }
        };
        this.beatServer = function () {
            if (ws && this.isOpen)
                ws.send(sender.getBeat(this.Executer));
            else
                this.init(this.Executer);
        };
        this.checkMsgPipe = function(scope){
            var me = scope || this;
            if (ws && me.isOpen) 
                return true;
            else{
                console.log('喇叭坏了，喊不出话喽！');
                return false;
            }
        }
        this.close = function () {
            
        };
        return this;

    };

    Sys.MsgStartUp = function (options) {
        var opts = {};
        var me = this;
        Ext.apply(opts, options);
        this.sysMsg = null;
        this.userid = null;
        this.fullname = null;
        var toolbar = Ext.widget({
            xtype: 'toolbar',
            border: true,
            rtl: false,
            floating: true,
            fixed: true,
            preventFocusOnActivate: true,
            draggable: {
                constrain: true
            },
            items: [{
                itemId: 'lbLogonInformation',
                xtype: 'label'
            }, {
                xtype: 'button',
                text: '广播喊话',
                iconCls: Sys.App.Icon.broadcast,
                itemId: 'btnLoudly',
                handler: function () {
                    var edithandler = {
                        buttonclose: function () {
                            broadcastwindow.close();
                        },
                        buttonsave: function () {
                            var editform = broadcastform.getForm();
                            if (!editform.isValid())
                                return;
                            var content = broadcastform.down('#content');

                            Ext.Msg.confirm('系统提示', '会被所有在线人员听到哦，您确认要喊吗？', function (y) {
                                if (y == 'yes')
                                    if (me.sysMsg) {
                                        me.sysMsg.loudly(content.getValue(), me.userid);
                                        broadcastwindow.close();
                                    }
                            }, this);
                        }
                    };
                    var broadcastform = Ext.create('Sys.App.TopForm', {
                        fieldDefaults: {
                            labelWidth: 100, //可微调，以适应不同的界面。
                            anchor: '90%',  //控件所占宽度比例，可微调。
                            labelAlign: 'right', //标签内容靠左\右
                            msgTarget: 'side'
                        },
                        items: [
                            {
                                fieldLabel: '广播内容',
                                itemId: 'content',
                                allowBlank: false,
                                height: 70,
                                xtype: 'textareafield'
                            }
                        ]
                    });
                    var broadcastwindow = Ext.create('Sys.App.TopWindow', {
                        title: '广播喊话',
                        width: 550,
                        height: 170,
                        minWidth: 550,
                        minHeight: 170,
                        iconCls: Sys.App.Icon.broadcast,
                        items: broadcastform,
                        buttons: [
                            { minWidth: 80, text: '发送', handler: edithandler.buttonsave },
                            { minWidth: 80, text: '关闭', handler: edithandler.buttonclose }
                        ]
                    });

                    broadcastwindow.show();

                }
            }, '-', {
                xtype: 'button',
                text: '系统消息',
                iconCls: Sys.App.Icon.systeminformation,
                handler: function () {
                    if (me.sysMsg)
                        me.sysMsg.p2p('bbbbb', '你该走了！');
                }
            }/*, '-', {
                xtype: 'tool',
                type: 'close',
                rtl: false,
                handler: function () {
                    toolbar.destroy();
                }
            }*/],
            constraintInsets: '0 -' + (Ext.getScrollbarSize().width + 4) + ' 0 0'
        });
        toolbar.show();
        
        toolbar.alignTo(
            document.body,
            Ext.optionsToolbarAlign || 'tr-tr',
            [
                (Ext.getScrollbarSize().width + 4) * (Ext.rootHierarchyState.rtl ? 1 : -1)-150,
                -(document.body.scrollTop || document.documentElement.scrollTop)
            ]
        );
        
        var constrainer = function () {
            toolbar.doConstrain();
        };

        Ext.EventManager.onWindowResize(constrainer);
        toolbar.on('destroy', function () {
            Ext.EventManager.removeResizeListener(constrainer);
        });
        this.setToolBar = function (fullname) {
            var lginfor = toolbar.down('#lbLogonInformation');
            lginfor.setText(Ext.String.format('用户【{0}】已登录！', fullname));
            this.fullname = fullname;
        };
        this.init = function (userid) {
            me.sysMsg = new Sys.Msg({
                onReceive: function (data) {
                    switch (data.level2.protocal) {
                        case 'M0001':
                            //
                            break;
                        case 'M0002':
                            Ext.example.msg('系统消息', data.businessData[0]);
                            break;
                        default:
                            break;
                    }
                }
            });
            me.sysMsg.init(userid);
            this.userid = userid;
        };
    }
})();