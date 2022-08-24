$(function () {
    //var $index, $ids, $qtys, $dishIds, $dishQtys;
    //var $ticked = $('input.ticked');
    //var $qty = $('.qty');
    alert('555');
    var $btnOrder = $('button.w3-btn');
    ////load order cookie for page load
    //var $dishCookie = readCookie("selectedDishes");

    //if ($dishCookie != null) {
    //    var $cookieArray = $dishCookie.split('&');
    //    $ids = $cookieArray[0];
    //    $qtys = $cookieArray[1];
    //}

    //$dishIds = $ids ? $ids.split(',') : [];
    //$dishQtys = $qtys ? $qtys.split(',') : [];

    //$ticked.each(function () {
    //    $qty.eq($ticked.index(this)).toggle(this.checked);
    //});

    ////disable the save order button
    //btnToggle($btnOrder, true);

    //$qty.change(function () {
    //    $this = $(this);
    //    $index = $qty.index($this) + 1;
    //    $index = $.inArray($index.toString(), $dishIds);
    //    $dishQtys[$index] = $this.val();
    //    createCookie('selectedDishes', $dishIds.join(',') + '&' + $dishQtys.join(','), 0.5)
    //});

    //$ticked.change(function () {
    //    var $this = $(this);
    //    //find the dish id
    //    //var $did = $this.parents().find('a').attr('href').split('/')[3];
    //    var $did = $this.siblings("[id$='Dish_Id']").first().attr('value');

    //    btnToggle($btnOrder, !$('input:checked').length);

    //    if ($this.prop('checked')) {
    //        //show quantity
    //        //index of quantity
    //        $index = $('.ticked').index(this);
    //        $qty.eq($index).show();
    //        //add this dish to array if it's not there
    //        if ($.inArray($did, $dishIds) == -1) {
    //            $dishIds.push($did);
    //            $dishQtys.push($qty.eq($index).val());
    //        }
    //    } else {
    //        //hide quantity
    //        $index = $('.ticked').index(this);
    //        $qty.eq($index).hide();
    //        //remove this dish id and quantity
    //        $index = $.inArray($did, $dishIds)
    //        $dishIds.splice($index, 1);
    //        $dishQtys.splice($index, 1);
    //    }

    //    //remove if none selected
    //    if ($dishIds.length) {
    //        createCookie('selectedDishes', $dishIds.join(',') + '&' + $dishQtys.join(','), 0.5);
    //    } else {
    //        createCookie('selectedDishes', '', -1);
    //    }
    //});

    $btnOrder.on('click', function () {
        alert('javascript');

        confirmDialog('gMenu', 'Place order', createOrder);
    })
})
