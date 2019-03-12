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
    count=1
    $("#edit-info").click(function () {
        count++;
        if (count % 2 == 0) {
            $(".edit-user").css("display", "block", "transition", "1s");
            $(".info-user").css("display", "none", "transition", "1s");
        }
        else {
            $(".edit-user").css("display", "none", "transition", "1s");
            $(".info-user").css("display", "block", "transition", "1s");
        }
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
    //$("#submited").click(function () {
    //    $(".modal").hide();
    //});
});


