﻿//Settings Preview
$(document).ready(function () {
    $("#wizard-picture").change(function () {
        readURL(this);
    });
});

//Settings Load
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#wizardPicturePreview').attr('src', e.target.result).fadeIn('slow');
        }
        reader.readAsDataURL(input.files[0]);
    }
}