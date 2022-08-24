(function ($) {
    readCookie = function (name) {
        var $nameEQ = name + '=';
        var $ca = document.cookie.split(';');

        for (i = 0; i < $ca.length; i++) {
            var $c = $ca[i];

            //get rid of leading spaces
            while ($c.charAt(0) == ' ') $c = $c.substring(1, $c.length);
            if ($c.indexOf($nameEQ) == 0) return $c.substring($nameEQ.length, $c.length);
        }

        return null;
    }

    createCookie = function (name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = '; expires = ' + date.toGMTString();
        } else expires = '';
        
        document.cookie = name + '=' + value + expires + '; path = /';
    }
})(jQuery)