$(function() {
    var $href = $(location).attr("href");
    var $actionUrl;
    var $rid = queryString('rid');
    alert($rid);

    if ($rid + 0 == 0) {
        $rid = $href.split('/').pop();
        if (!isNumeric($rid))
            $rid = 0;      
    }

    $actionUrl = '/Home/RestaurantProfile/' + $rid;
    alert($actionUrl);

    $.ajax({
        url: $actionUrl,
        type: "GET",
        success: function (data) {
            $('#restaurant-profile').html(data);
        }
    });
})

/* function queryString(key) {
    return decodeURIComponent((RegExp("[?&]" + key + "=([^&]*)").exec(location.href) || [, 0])[1]);
}

function isNumeric(n)
{
    return !isNaN(parseFloat(n)) && is(n);
} */