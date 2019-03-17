
var checkAvarta = true;
$(function () {

    $('.three-dot span').click(function () {
        $('.timeline-setting').toggleClass('show1');
        $('.arrow-up').toggleClass('show2');
    });
    notify = false;
    $('.icon-notify').off().click(function () {
        $('#notifi').toggle(150);
    });

    $('.icon-friend').off().click(function () {
        $('#add-friend_invitation').toggle(150);
    });

    $('.maincontent,#people-list,.icon-home,.icon-friend').off().mouseup(function (e) {
        $('#notifi').hide();
    });

    $(".textNoti p").shorten({
        "showChars": 120,
        "moreText": "Xem thêm...",
        "lessText": "Rút gọn"
    });
    count = 1;
    $("#edit-info").click(function () {
        count++;
        if (count % 2 === 0) {
            $(".edit-user").css("display", "block", "transition", "1s");
            $(".info-user").css("display", "none", "transition", "1s");
        }
        else {
            $(".edit-user").css("display", "none", "transition", "1s");
            $(".info-user").css("display", "block", "transition", "1s");
        }
    });
    $(".avatar .img-responsive").mouseover(function () {
        $(this).css("cursor", "pointer");
        $(".update-img").addClass(function () {
            $(this).css("display", "block", "cursor", "pointer");
        });
    });
    $(".avatar .img-responsive").mouseout(function () {
        $(this).css("cursor", "pointer");
        $(".update-img").addClass(function () {
            $(this).css("display", "none", "cursor", "pointer");
        });
    });
    //đổi nút chọn file sang button avatar
    $("#selectFile").click(function () {
        $("#UploadImage").trigger('click');
    });
    //đổi nút chọn file sang button background
    $("#selectFileBg").click(function () {
        $("#UploadBg").trigger('click');
    });


    $(".think").click(function () {
        $(".bot #huy").show();
    });
    $("#gioithieu").click(function () {
        $(".modalGioiThieu").show();
    });
    $(".showInfoFriend .dropdown .dropbtn").click(function () {
        $(this).next().toggle();
    });
    $('.maincontent,#people-list,.icon-home,.icon-friend').off().mouseup(function (e) {
        $('.showInfoFriend .dropdown .dropbtn').next().hide();
    });
    $(".background").hover(function () {
        $(".update-background span").toggle();
        $(".update-background").toggleClass('edit-background');
    });
  
    $('.maincontent,#people-list,.icon-home,.icon-friend').off().mouseup(function (e) {
        $(".update-background span").hide();
        $(".update-background").removeClass('edit-background');
    });


    $(".background,.avatar").click(function () {
        src = $(this).find("img").attr("src");
        $("#FormAvatar img").attr("src", src);
        if ($(this).hasClass("avatar")) {
            checkAvarta = true;
        } else {
            checkAvarta = false;
        }
    });

});
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#blah').attr('src', e.target.result);
        }
        $("#upImg").show();
        reader.readAsDataURL(input.files[0]);
    }
}

$("#UploadImage").change(function () {
    readURL(this);
});
