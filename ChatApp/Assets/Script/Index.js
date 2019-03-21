
var checkAvarta = 0;
var count = 1;
$(function () {

    countNoti = parseInt($(".badge").text());
    if (countNoti) {
        $(".badge").show();
    } else {
        $(".badge").hide();
    }
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

   
    $('body').on('click',' .icon-friend ', function (e) {
        $('#add-friend_invitation').toggle();
    });

    $('body').on('click', '#Main-content, #people-list, .icon-home, .icon-friend ', function (e) {
        $('#notifi').hide();
    });

    $('body').on('click', '#Main-content, #people-list, .icon-home, .icon-notify ', function (e) {
        $('#add-friend_invitation').hide();

    });

    
    $("#edit-info").click(function () {
        count++;
        if (count % 2 === 0) {
            $(".edit-user").show();
            $(".info-user").hide();
        }
        else {
            $(".edit-user").hide();
            $(".info-user").show();
        }
        //get dữ liệu
        $.ajax({
            type: "GET",
            url: "/Home/Edit",
            contentType: "application/json;charset=utf-8",
            dataType: "JSON",
            success: function (res) {          
                $(".edit-user #Name").val(res.Name);
                $(".edit-user #SchoolName").val(res.SchoolName);
                var date = res.DoB;
                
                var resTime = new Date(parseInt(date.replace("/Date(", "").replace(")/")));
                var month = resTime.getMonth()+1, dates = resTime.getDate();
                if (month < 10) {
                    month = "0" + month;
                }
                if (dates < 10) {
                    dates = "0" + dates;
                }
                var dateTime = resTime.getFullYear()+"-" + month + "-" + dates  ;
                
                $(".edit-user #DoB").val(dateTime);
                $(".edit-user #PhoneNumber").val(res.PhoneNumber);
                $(".edit-user #Address").val(res.Address);
            }

        })
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
    $(".background").hover(function () {
        $(".update-background span").toggle();
        $(".update-background").toggleClass('edit-background');
    });
  
    $('.maincontent,#people-list,.icon-home,.icon-friend').off().mouseup(function (e) {
        $(".update-background span").hide();
        $(".update-background").removeClass('edit-background');
        $('.showInfoFriend .dropdown .dropbtn').next().hide();
        $(".edit-poststore").hide();
    });


    $(".update-background,.update-img").click(function () {
        src = $(this).prev().attr("src");
        $("#FormAvatar img").attr("src", src);
        if ($(this).hasClass("update-img")) {
            checkAvarta = 1;
        } else if ($(this).hasClass(".update-background")) {
            checkAvarta = 2;
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
//click vào giới thiệu trang cá nhân
$("#banbe").click(function () {
    $(".lef-1").hide();
    $(".lef-2").show();
})
$("#gioithieu").click(function () {
    $(".lef-1").show();
    $(".lef-2").hide();
})
$("#close,.close").on("click", function () {
    $("#upImg").hide();
})
$('#banbe').click(function () {
    // body...
    if ($(".show-notify").hasClass('transform')) {
        $(".show-notify").removeClass('transform');

    }
    else {
        $(".show-notify").addClass('transform');
    }

});
$(".EditPostStore").click(function () {
    $(this).next().toggle();
})
$("#Huy").click(function () {
    count++;
    if (count % 2 === 0) {
        $(".edit-user").show();
        $(".info-user").hide();
    }
    else {
        $(".edit-user").hide();
        $(".info-user").show();
    }
})
