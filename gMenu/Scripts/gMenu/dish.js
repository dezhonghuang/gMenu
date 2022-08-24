$(function () {
    $('#file').change(function (evt) {
        var f = evt.target.files[0];
        var fileReader = new FileReader();

        //check if the selected file is an image file
        if (!f.type.match('image.*')) {
            alert("Selected file is not an image file, please try again.");
            return;
        }

        //change img tag attribute
        fileReader.onload = function (e) {
            $('#image').attr('src', e.target.result);
        }

        fileReader.readAsDataURL(f);
    });
});