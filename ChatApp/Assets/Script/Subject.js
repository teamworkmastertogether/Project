$(function () {
    loadListFriendSuggest();
});

function loadListFriendSuggest() {
    $.ajax({
        type: "GET",
        url: "/Home/GetFriendSuggest",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (array) {
            $(".list-friend-sugggest").html("");
            for (var i = 0; i < array.length; i++) {
                $(".friend-suggest-clone img").attr("src", array[i].Avatar);
                $(".friend-suggest-clone h5").text(array[i].Name);
                $(".friend-suggest-clone a").attr("href", array[i].UrlPersonal);
                $(".friend-suggest-clone .add-friend").attr("id", array[i].IdUser);
                itemClone = $(".friend-suggest-clone").html();
                $(".list-friend-sugggest").append(itemClone);
            }
            $(".friend-suggest-clone .add-friend").attr("id", 0);
        },
        error: function (message) {
            alert(message.responseText);
        }
    });
}

$(".list-friend-sugggest").on("click", ".add-friend", function () {
    IdUser = parseInt($(this).attr("id"));
    $(this).text("Đã gửi yêu cầu");
    url = "/Home/SendRequestAddFriend?id=" + IdUser;
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (username) {
            hub.server.sendRequestAddFriend(username);
        },
        error: function (message) {
            alert(message.responseText);
        }
    });
});

$(".list-friend-sugggest").on("click",".delete-suggest",function() {
    $(this).closest(".content-listFriendSuggest").slideUp(300);
});