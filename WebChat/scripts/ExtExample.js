Ext.example = function () {
    var msgCt;

    function createBox(t, s) {
        // return ['<div class="msg">',
        //         '<div class="x-box-tl"><div class="x-box-tr"><div class="x-box-tc"></div></div></div>',
        //         '<div class="x-box-ml"><div class="x-box-mr"><div class="x-box-mc"><h3>', t, '</h3>', s, '</div></div></div>',
        //         '<div class="x-box-bl"><div class="x-box-br"><div class="x-box-bc"></div></div></div>',
        //         '</div>'].join('');
        return '<div class="msg"><h3>' + t + '</h3><p>' + s + '</p></div>';
    }
    return {
        msg: function (title, format) {
            if (!msgCt) {
                msgCt = Ext.DomHelper.insertFirst(document.body, { id: 'msg-div' }, true);
            }
            var s = Ext.String.format.apply(String, Array.prototype.slice.call(arguments, 1));
            var m = Ext.DomHelper.append(msgCt, createBox(title, s), true);
            m.hide();
            m.slideIn('t').ghost("t", { delay: 5000, remove: true });
        },

        init: function () {
            if (!msgCt) {
                // It's better to create the msg-div here in order to avoid re-layouts 
                // later that could interfere with the HtmlEditor and reset its iFrame.
                msgCt = Ext.DomHelper.insertFirst(document.body, { id: 'msg-div' }, true);
            }
            //            var t = Ext.get('exttheme');
            //            if(!t){ // run locally?
            //                return;
            //            }
            //            var theme = Cookies.get('exttheme') || 'aero';
            //            if(theme){
            //                t.dom.value = theme;
            //                Ext.getBody().addClass('x-'+theme);
            //            }
            //            t.on('change', function(){
            //                Cookies.set('exttheme', t.getValue());
            //                setTimeout(function(){
            //                    window.location.reload();
            //                }, 250);
            //            });
            //
            //            var lb = Ext.get('lib-bar');
            //            if(lb){
            //                lb.show();
            //            }
        }
    };
}();