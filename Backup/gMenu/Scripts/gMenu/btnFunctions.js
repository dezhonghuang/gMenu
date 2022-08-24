(function ($) {
    //toggle button
    btnToggle = function (btn, on) {
        btn.prop('disabled', on);
    };

    sameOrder = function (t, st, q, sq) {
        /*        if (!ticked.equals(saveTicked)) {
            return false;
        }
        
        var k;

        for (var i = 0; i < saveQtys.length; i++) {
            if (ticked.eq(i).val() && qtys.eq(i).val() != saveQtys[i]) {
                return false;
            }
        } 
        $.each(saveQtys, function (i, v) {
            //alert(ticked.eq(0).val());
        }); */
        //$.each(t, function() {
        //    alert('t: ' + $(this).val());
        //});

        //$.each(st, function () {
        //    alert('st: ' + $(this).val());
        //});
        var tArray = [];

        t.each(function (i) {
            if (this.checked) tArray.push(i);
        });

        var rv = true;
        if (($(st).not(tArray).length == 0) && ($(tArray).not(st).length == 0)) {
            $.each(st, function (i, v) {
                if (q.eq(v).val() != sq[v]) return rv = false;
            });
        } else rv = false;

        return rv;
    };

    $.fn.equals = function (compareTo) {
        if (!compareTo || this.length != compareTo.length) {
            return false;
        }

        for (var i = 0; i < this.length; i++) {
            if (this[i] !== compareTo[i]) {
                return false;
            }
        }

        return true;
    };

    createOrder = function () {
        $('#order-form').submit();
        //disable button to stop multiple submit
        $('#btn-order').attr('disabled', 'disabled');
    };

    saveEdit = function () {
        $('#edit-form').submit();
        //disable button to stop multiple submit
        $('#btn-order').attr('disabled', 'disabled');
    };

    confirmDialog = function (title, message, yesFunction) {
        $('<div></div>').attr('title', title)
            .appendTo('body').html('<h6>' + message + '?</h6>')
            .dialog({
                modal: true,
                resizable: false,
                zindex: 1000,
                closeOnEscape:true,
                buttons: {
                    Yes: function () {
                        yesFunction();
                        $(this).dialog('close');
                    },
                    Cancel: function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    $(this).remove();
                }
            });
    };
})(jQuery)