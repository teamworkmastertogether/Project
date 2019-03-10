$(document).ready(function () {
    $('.three-dot span').click(function () {
        // body...
        $('.timeline-setting').toggleClass('show1');
        $('.arrow-up').toggleClass('show2');
    });
    notify = false;
    $('.icon-notify').off().click(function () {
        $('#notifi').toggle(150);
        //if (notify) {
        //    $('.icon-notify').css("background-color", "#444753");
        //    notify = !notify;
        //}
        //else {
        //    $('.icon-notify').css("background-color", "#5167cc");
        //    notify = !notify;
        //}
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
});


