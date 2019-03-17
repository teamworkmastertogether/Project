$(document).ready(function () {
    $('#forgot').click(function () {
        $('.verify-email').show();
    });

    $('#studentCancel').click(function () {
        $('.verify-email').hide();
    });

    $('.input-information').blur(function () {
        if ($(this).val() !== "") {
            $(this).removeClass('.border-red');
        } else {
            $(this).addClass('.border-red');
        }
    });

    $()
});