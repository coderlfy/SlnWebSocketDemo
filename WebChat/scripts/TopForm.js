Ext.define('Sys.App.TopForm', {
    extend: 'Ext.form.Panel',
    initComponent: function (config) {
        var me = this;
        Ext.apply(me, config);

        me.baseCls = 'x-plain';
        me.bodyPadding = 10;
        me.callParent(arguments);
    }
});
Ext.define('Sys.App.TopWindow', {
    extend: 'Ext.window.Window',
    initComponent: function (config) {
        var me = this;

        me.collapsible = true;
        me.animCollapse = true;
        me.layout = 'fit';
        me.modal = 'true';
        me.plain = true;
        me.buttonAlign = 'center';

        Ext.apply(me, config);
        me.callParent(arguments);
    }
});