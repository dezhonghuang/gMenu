(function ($) {
    $.fn.extend({
        cookieList: function (cookieName, expireTime) {
            return {
                add: function (val) {
                    var items = this.items();

                    var index = items.indexOf(val);

                    if (index == -1) {
                        items.push(val);
                        $.cookie(cookieName, items.join(','), { expires: expireTime, path: '/' });
                    }
                },
                remove: function (val) {
                    var items = this.items;

                    var index = items.indexOf(val);

                    if (index != -1) {
                        items.splice(val, 1);
                        $.cookie(cookieName, items.join(','), { expires: expireTime, path: '/' });
                    }
                },
                indexOf: function(val) {
                    return this.items().indexOf(val);
                },
                clear: function () {
                    $.cookie(cookieName, null, { expires: expireTime, path: '/' });
                },
                items: function () {
                    var cookie = $.cookie(cookieName);

                    return cookie ? eval("([" + cookie + "])") : [];
                },
                length: function () {
                    return this.items.length;
                },
                join: function (separator) {
                    return this.items.join(separator);
                }
            }
        }
    });
})(jQuery);