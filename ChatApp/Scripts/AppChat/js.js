$(document).ready(function () {
    $('.three-dot span').click(function () {
        // body...
        $('.timeline-setting').toggleClass('show1');
        $('.arrow-up').toggleClass('show2');
    });
    notify = false;
    $('.icon-notify').off().click(function () {
        $('#notifi').toggle(150);
    });
    $('.maincontent,#people-list,.icon-home,.icon-friend').off().mouseup(function (e) {
        $('#notifi').hide();
    });
    $(".textNoti p").shorten({
        "showChars": 120,
        "moreText": "Xem thêm...",
        "lessText": "Rút gọn"
    });
    $("#edit-info").click(function () {
        $(".edit-user").css("display", "block");
    });
    $(".avatar").mouseover(function () {
        $(this).css("cursor", "pointer");
        $(".update-img").addClass(function () {
            $(this).css("display", "block","cursor","pointer");
        });
    });
    $(".avatar").mouseout(function () {
        $(this).css("cursor", "pointer");
        $(".update-img").addClass(function () {
            $(this).css("display", "none","cursor","pointer");
        });
    });
    $("#selectFile").click(function () {
        $("#UploadImage").trigger('click');
    })
    $("#submited").click(function () {
        $("#exampleModalCenter").hide();
    });
});


