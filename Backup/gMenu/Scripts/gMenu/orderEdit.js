$(function () {
    var $index,
        $ticked = $('input.ticked'), $qty = $('.qty'),
        $savedCheckeds = [], $saveQuantities = [],
        $btnOrder = $('#btn-order');

    //original order details
    $ticked.each(function (i) {
        if (this.checked) $savedCheckeds.push(i);
    });

    $qty.each(function (i) {
        $saveQuantities[i] = $(this).val();
    });

    //disable save button
    btnToggle($btnOrder, true);

    //display selected dishes quantities
    $ticked.each(function () {
        $qty.eq($ticked.index(this)).toggle(this.checked);
    });

    $ticked.change(function () {
        var $this = $(this);

        $index = $ticked.index(this);
        $qty.eq($index).toggle($this.prop('checked'));

        //enable button order changes
        btnToggle($btnOrder, sameOrder($ticked, $savedCheckeds, $qty, $saveQuantities));
    });

    $qty.change(function () {
        //alert(sameOrder($ticked, $savedCheckeds, $qty, $saveQuantities));
        btnToggle($btnOrder, sameOrder($ticked, $savedCheckeds, $qty, $saveQuantities));
    });

    $btnOrder.on('click', function () {
        confirmDialog('gMenu', 'Save order', saveEdit);
    })
})
