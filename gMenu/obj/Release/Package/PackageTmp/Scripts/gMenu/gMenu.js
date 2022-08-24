$(function () {
    //Menu
    var $path = location.pathname,
        $index,
        $names = $('.name'),
        $selected;

    $('ul#menu a[href="' + $path + '"]').addClass('current');

    //restaurant profile
    var $href = $(location).attr('href').split('?')[0].split('/');

    var $rid = $href.pop();

    $.ajax({
        url: '/Home/RestaurantProfile',
        type: 'GET',
        data: { rid: $rid },
        dataType: 'html'
    }).success(function (result) {
        $('#restaurant-profile').html(result)
    }).error(function (xhr, status) {
        alert("No restaurant setup.");
    });

    //which category to open
    var $categoryToOpen = readCookie("categoryToOpen");

    var $co;

    if ($categoryToOpen != null) {
        $co = $('.category:eq(' + $categoryToOpen + ')');
    } else {
        $co = $('.category:eq(0)');
    }

    $co.siblings('li').nextUntil('.category').hide();

    $('.category').on('click', function () {
        $(this).next('li').show();
        $(this).siblings('li').not('.category').not($(this).next('li')).hide();
        createCookie('categoryToOpen', $(this).index() / 2, 1 / 4);
    });

    //show ticked items
    var $ticked = $('.ticked'),
        $qty = $('.qty');

    $ticked.each(function () {
        $qty.eq($ticked.index(this)).toggle(this.checked);
    });

    $names.on('click', function () {
        $index = $names.index(this);
        $selected = !$ticked.eq($index).prop('checked');
        $ticked.eq($index).prop('checked', $selected).trigger('change');
    })
})