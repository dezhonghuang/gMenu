$(function() {
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
})
