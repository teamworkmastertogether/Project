
var checkAvarta = 0;
var count = 1;
$(function () {

    countNoti = parseInt($(".icon-notify .badge").text());
    if (countNoti) {
        $(".icon-notify .badge").show();
    } else {
        $(".icon-notify .badge").hide();
    }

    countNoti2 = parseInt($(".icon-friend .badge").text());
    if (countNoti2) {
        $(".icon-friend .badge").show();
    } else {
        $(".icon-friend .badge").hide();
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
                    countNoti = parseInt($(".icon-notify .badge").text()) - 1;
                    if (countNoti) {
                        $(".icon-notify .badge").text(countNoti).show();
                    } else {
                        $(".icon-notify .badge").text(countNoti).hide();
                    }
                }
                $('#notifi').hide();
            },
            error: function (message) {
                alert(message.responseText);
            }
        });
        
    });

   
    $('body').on('click', ' .icon-friend ', function (e) {
        $.ajax({
            type: "POST",
            url: "/Notifi/GetListUserSendRequest",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (array) {
                $("#add-friend_invitation").html("");
                for (var i = 0; i < array.length; i++) {
                    $(".list-user-sendRequest-clone img").attr("src", array[i].Avatar);
                    $(".list-user-sendRequest-clone .invitation-content_username").text(array[i].Name);
                    $(".list-user-sendRequest-clone .invitation-content_username").attr("href", array[i].UrlPersonal);
                    $(".list-user-sendRequest-clone .add-friend_invitation_btnAdd").attr("id", array[i].IdUser);
                    itemClone = $(".list-user-sendRequest-clone").html();
                    $("#add-friend_invitation").append(itemClone);
                }
            },
            error: function (message) {
                alert(message.responseText);
            }
        });

        $('#add-friend_invitation').toggle();
    });

    $('#add-friend_invitation').on("click", ".add-friend_invitation_btnAdd", function () {
        $(this).closest(".invitation_space").remove();
        IdUser = parseInt($(this).attr("id"));
        url = "/Home/AcceptRequest?id=" + IdUser;
        $.ajax({
            type: "POST",
            url: url ,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (array) {
               
            },
            error: function (message) {
                alert(message.responseText);
            }
        });
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

        Id = parseInt($(".MyId").attr("id"));
        url = "/Home/Edit?id=" + Id;
        //get dữ liệu
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "JSON",
            success: function (res) {
                $(".edit-user #Name").val(res.Name);
                $(".edit-user #SchoolName").val(res.SchoolName);
                var date = res.DoB;
                var resTime = new Date(parseInt(date.replace("/Date(", "").replace(")/")));
                var month = resTime.getMonth() + 1, dates = resTime.getDate();
                if (month < 10) {
                    month = "0" + month;
                }
                if (dates < 10) {
                    dates = "0" + dates;
                }
                var dateTime = resTime.getFullYear() + "-" + month + "-" + dates;

                $(".edit-user #DoB").val(dateTime);
                $(".edit-user #PhoneNumber").val(res.PhoneNumber);
                $(".edit-user #Address").val(res.Address);
            }

        });
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
   
    $(".think").click(function () {
        $(".bot #huy").show();
    });
    $("#gioithieu").click(function () {
        $(".modalGioiThieu").show();
    });
    $(".lef-2").on("click",".dropbtn",function () {
        $(this).next().toggle();
    });
    $(".lef-2").on("click", ".removeFriend", function () {
        IdUser = parseInt($(this).attr("id"));
        url = "/Home/RemoveFriend?id=" + IdUser;
        //get dữ liệu
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "JSON",
            success: function (res) {
                Swal.fire(
                    'Thành công!',
                    'Bạn đã hủy kết bạn thành công!',
                    'success'
                );
            }

        });
        $(this).closest(".col-md-12").remove();
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
        } else if ($(this).hasClass("update-background")) {
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
    Id = parseInt($(".MyId").attr("id"));
    url = "/Home/GetListFriend?id=" + Id;
    //get dữ liệu
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "JSON",
        success: function (array) {
            $(".lef-2").html("");
            for (var i = 0; i < array.length; i++) {
                $(".list-friend-clone img").attr("src", array[i].Avatar);
                $(".list-friend-clone .nameFriend p").text(array[i].Name);
                $(".list-friend-clone .nameFriend a").attr("href", array[i].UrlPersonal);
                $(".list-friend-clone .dropdown-content a").eq(0).attr("href", array[i].UrlPersonal);
                $(".list-friend-clone .removeFriend").attr("id", array[i].IdUser);
                itemClone =  $(".list-friend-clone").html();
                $(".lef-2").append(itemClone);
            }
        }
    });
    $(".lef-2").show();
});
$("#gioithieu").click(function () {
    $(".lef-1").show();
    $(".lef-2").hide();
});
$("#close,.close").on("click", function () {
    $("#upImg").hide();
});



$(".content-store").on("click", ".EditPostStore", function () {
    $(this).next().toggle();
});

$(".content-store").on("click", ".edit-poststore", function () {
    IdPost = parseInt($(this).closest(".post-store").attr("id"));
    url = "/Home/DeletePostSaved?id=" + IdPost;
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "JSON",
        success: function (res) {
            $(".post-store[id=" + IdPost + "]").remove();
            Swal.fire(
                'Thành công!',
                'Bạn đã xóa bài viết thành công!',
                'success'
            );
        }
    })
});

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
});

$('#have-seen').on('click', function () {
    $('#have-seen').css('text-decoration', 'none');
});
$('.container').on('click', '.img-postsave', function () {
    $('.show-image img').attr('src', $(this).attr('src'));
    $('.show-image').show();
    if ($('.show-image img').height() < 300) {
        $('.new-modal').css('padding-top', '150px');
    }
});

$('.close-seen-image').click(function () {
    $('.show-image').hide();
});