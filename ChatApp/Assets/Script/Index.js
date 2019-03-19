
var checkAvarta = true;
$(function () {

    $('.three-dot span').click(function () {
        $('.timeline-setting').toggleClass('show1');
        $('.arrow-up').toggleClass('show2');
    });
    notify = false;
    $('.icon-notify').off().click(function () {
        $.ajax({
            type: "POST",
            url: "/Notifi/GetNotifi",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (array) {
                $(".notifyClass").html("");
                for (var i = 0; i < array.length; i++) {
                    tagIcon = '<i class="' + array[i].ClassIconName + '"></i>';
                    $(".notifi-clone .identify-icon").html(tagIcon);
                    $(".notifi-clone img").attr("src", array[i].Avatar);
                    $(".notifi-clone h3").text(array[i].NameOfUser);
                    $(".notifi-clone h4").text(array[i].TimeNotifi);
                    $(".notifi-clone em").text(array[i].TextNoti);
                    $(".notifi-clone span").text(array[i].SubjectName);
                    href = "/Subject/GetSubject?id=" + array[i].SubjectId + "#" + array[i].PostId;
                    $(".notifi-clone a").attr("href", href);
                    $(".notifi-clone a").attr("id", array[i].NotificationId);
                    if (array[i].NotificationState) {
                        $(".notifi-clone a").addClass("notifi-seen");
                    }
                    item = $(".notifi-clone").html();
                    $(".notifyClass").prepend(item);
                    $(".notifi-clone a").removeClass("notifi-seen");
                }
                $(".notifi-clone a").attr("id", 0);
            },
            error: function (message) {
                alert(message.responseText);
            }
        });
        $('#notifi').toggle(150);
    });

    $(".notifyClass").on('mouseup', 'a', function (e) {
        Id = parseInt($(this).attr("id"));
        url = "/Notifi/SaveSeenNotifi?Id=" + Id;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    countNoti = parseInt($(".badge").text()) - 1;
                    if (countNoti) {
                        $(".badge").text(countNoti).show();
                    } else {
                        $(".badge").text(countNoti).hide();
                    }
                }
                $('#notifi').hide();
            },
            error: function (message) {
                alert(message.responseText);
            }
        });

    });

    $('.icon-friend').off().click(function () {
        $('#add-friend_invitation').toggle(150);
    });

    $('body').on('click', '#Main-content, #people-list, .icon-home, .icon-friend ', function (e) {
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

countNoti = parseInt($(".badge").text());
if (countNoti) {
    $(".badge").show();
} else {
    $(".badge").hide();
}