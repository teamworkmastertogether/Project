var hub = $.connection.chatHub;
var QuantityMessage = -1;
var QuantityMessageNew = 0;
var checkOpenBoxMess = false;

$(function () {
    hub.client.setStatus = function (LstAllConnections) {
        $(".status").html("<i class='fa fa-circle offline'></i> offline");
        for (var key in LstAllConnections) {
            $("[id=" + LstAllConnections[key] + "]").html("<i class='fa fa-circle online'></i> online");
        }
    };

    hub.client.changeStatusSendRequest = function (suggestUser, check) {
        if (check) {
            $("[id=" + suggestUser + "]").parent().find(".add-friend").text("Đã gửi lời mời");
            $("[id=" + suggestUser + "]").parent().find(".add-friend").css("background", "#ef30c8");
        } else {
            $("[id=" + suggestUser + "]").parent().find(".add-friend").text("Kết bạn");
            $("[id=" + suggestUser + "]").parent().find(".add-friend").css("background", "#428bca");
        }
    };

    hub.client.setUserSeen = function (UserName, PartnerUser, checkseen) {
        var PartnerUserName = $("#partnerUserName").val();
        if (MyUserName === UserName) {
            $("[id=" + PartnerUser + "]").parent().next().html("");
            if (checkseen === true && $("#ContentMessage li:last-child").hasClass("MyMessage") && PartnerUserName === PartnerUser) {
                seenMessage();
            }
        }
        if ($("#ContentMessage li:last-child").hasClass("MyMessage") && PartnerUserName === UserName) {
            seenMessage();
        }
    };

    hub.client.setNotSeenMessage = function (MyUserName) {
        $("[id=" + MyUserName + "]").parent().next().html('<i class="fa fa-comment-o"></i>');
        var partner = $("[id=" + MyUserName + "]").parent().parent().clone(true);
        $("[id=" + MyUserName + "]").parent().parent().remove();
        $("ul.list").prepend(partner);
    };

    hub.client.appendSeen = function (MyUserName) {
        var PartnerUserName = $("#partnerUserName").val();
        if (PartnerUserName === MyUserName) {
            seenMessage();
        }
    };

    hub.client.showChatBox = function (Name, myUserName, partnerUserName, message, currentTime) {
        var PartnerUserName = $("#partnerUserName").val();
        QuantityMessageNew++;
        if (PartnerUserName === myUserName) {
            var item1 = '<li class="partnerMessage"><div class="message-data"><span class="message-data-name"><i class="fa fa-circle online"></i> ';
            var item2 = '</span><span class="message-data-time">';
            var item3 = '</span></div><div class="message my-message">';
            var item4 = '</div> </li>';
            var itemAdd = item1 + Name + item2 + currentTime + item3 + message + item4;
            $(".chat-history ul").append(itemAdd);
            hub.server.saveSeenMessage(PartnerUserName, MyUserName);
            hub.server.appendSeenForChatBox(MyUserName, PartnerUserName);
            setTimeout(function () {
                $(".chat-history ul .fa-eye").remove();
                $(".chat-history").scrollTop(7000);
            }, 200);
        } else if (MyUserName === myUserName && partnerUserName === PartnerUserName) {
            item1 = '<li class="clearfix MyMessage"><div class="message-data align-right"><span class="message-data-time">';
            item2 = '</span> &nbsp; &nbsp;<span class="message-data-name">';
            item3 = '</span> <i class="fa fa-circle me"></i></div><div class="message other-message float-right">';
            item4 = '</div> </li>';
            itemAdd = item1 + currentTime + item2 + Name + item3 + message + item4;
            $(".chat-history ul").append(itemAdd);
            setTimeout(function () {
                $(".chat-history ul .fa-eye").remove();
                $(".chat-history").scrollTop(7000);
            }, 200);
        }
    };

    hub.connection.start().done(function () {
        hub.server.setConnectionUser(MyUserName);
        $("#message-to-send").keypress(function (e) {
            if (e.which === 13) {
                var PartnerUserName = $("#partnerUserName").val();
                var message = $(this).val();
                $(this).val('');
                var currentTime = GetDateNow();
                hub.server.send(PartnerUserName, MyUserName, currentTime, message);
                $(".chat-history ul .fa-eye").remove();
            }
        });
        $("#SendClick").click(function () {
            $(".list-icon").hide();
            checkOpenIcon = true;
            if ($("#message-to-send").val().trim() !== "") {
                var PartnerUserName = $("#partnerUserName").val();
                var currentTime = GetDateNow();
                var message = $("#message-to-send").val();
                $("#message-to-send").val("");
                hub.server.send(PartnerUserName, MyUserName, currentTime, message);
                $(".chat-history ul .fa-eye").remove();
            }
        });
    });
    load();

});

function OpenChatBox(item) {
    $(".list-icon").hide();
    checkOpenIcon = true;
    if (!checkOpenBoxMess) {
        QuantityMessage = 0;
        checkOpenBoxMess = true;
    } else {
        QuantityMessage = -1;
    }
    var partnerUserName = $(item).find(".status").attr('id');
    var partnerName = $(item).find(".name1").text();
    var partnerImage = $(item).find("img").attr('src');
    QuantityMessageNew = 0;
    $(item).find(".message").html("");
    UserDto = {
        MyUserName: MyUserName,
        PartnerUserName: partnerUserName,
        QuantityMessage: 0,
        QuantityMessageNew: 0
    };
    $(".chat").css('display', '');
    $(".chat #partnerUserName").val(partnerUserName);
    $(".chat .chat-with").text(partnerName);
    $(".chat img").attr('src', partnerImage);
    GetMessages(UserDto);
    setTimeout(function () {
        hub.server.saveSeenMessage(partnerUserName, MyUserName);
    }, 250);
}

function GetMessages(UserDto) {
    $(".chat-history ul").html("");
    $.ajax({
        type: "POST",
        url: '/Home/GetMessage',
        data: JSON.stringify(UserDto),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].UserName === UserDto.MyUserName) {
                    var item1 = '<li class="clearfix MyMessage"><div class="message-data align-right"><span class="message-data-time">';
                    var item2 = '</span> &nbsp; &nbsp;<span class="message-data-name">';
                    var item3 = '</span> <i class="fa fa-circle me"></i></div><div class="message other-message float-right">';
                    var item4 = '</div> </li>';
                    var itemAdd = item1 + data[i].TimeSend + item2 + data[i].Name + item3 + data[i].MessageSend + item4;
                    $(".chat-history ul").append(itemAdd);
                } else {
                    item1 = '<li class="partnerMessage"><div class="message-data"><span class="message-data-name"><i class="fa fa-circle online"></i> ';
                    item2 = '</span><span class="message-data-time">';
                    item3 = '</span></div><div class="message my-message">';
                    item4 = '</div> </li>';
                    itemAdd = item1 + data[i].Name + item2 + data[i].TimeSend + item3 + data[i].MessageSend + item4;
                    $(".chat-history ul").append(itemAdd);
                }
            }
        }
    });
    setTimeout(function () {
        $(".chat-history").scrollTop(7000);
    }, 200);
}

function GetDateNow() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();
    var h = today.getHours();
    var M = today.getMinutes();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    today = h + 'h' + M + ' - ' + dd + '/' + mm + '/' + yyyy;
    return today;
}

var searchFilter = {
    options: {
        valueNames: ['name']
    },
    init: function () {
        var userList = new List('people-list', this.options);
        var noItems = $('<li id="no-items-found">Không tìm thấy</li>');

        userList.on('updated', function (list) {
            if (list.matchingItems.length === 0) {
                $(list.list).append(noItems);
            } else {
                noItems.detach();
            }
        });
    }
};

searchFilter.init();

$(".fa.fa-close").click(function () {

    QuantityMessageNew = -1;
    QuantityMessage = 0;
    $("#partnerUserName").val("");
    $(".chat").hide();
});

function seenMessage() {
    setTimeout(function () {
        $(".chat-history ul .fa-eye").remove();
        $(".chat-history ul").append('<i class="fa fa-eye">&nbsp;&nbsp;Đã xem</i>');
    }, 500);
}

function LoadData(item) {
    if ($(item).scrollTop() === 0) {
        QuantityMessage = QuantityMessage + 1;
        if (QuantityMessage !== 0) {
            var partnerUserName = $(".chat #partnerUserName").val();
            UserDto = {
                MyUserName: MyUserName,
                PartnerUserName: partnerUserName,
                QuantityMessage: QuantityMessage,
                QuantityMessageNew: QuantityMessageNew
            };
            $.ajax({
                type: "POST",
                url: '/Home/GetMessage',
                data: JSON.stringify(UserDto),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.length > 0) {
                        for (var i = data.length - 1; i >= 0; i--) {
                            if (data[i].UserName === UserDto.MyUserName) {
                                var item1 = '<li class="clearfix MyMessage"><div class="message-data align-right"><span class="message-data-time">';
                                var item2 = '</span> &nbsp; &nbsp;<span class="message-data-name">';
                                var item3 = '</span> <i class="fa fa-circle me"></i></div><div class="message other-message float-right">';
                                var item4 = '</div> </li>';
                                var itemAdd = item1 + data[i].TimeSend + item2 + data[i].Name + item3 + data[i].MessageSend + item4;
                                $(".chat-history ul").prepend(itemAdd);
                            } else {
                                item1 = '<li class="partnerMessage"><div class="message-data"><span class="message-data-name"><i class="fa fa-circle online"></i> ';
                                item2 = '</span><span class="message-data-time">';
                                item3 = '</span></div><div class="message my-message">';
                                item4 = '</div> </li>';
                                itemAdd = item1 + data[i].Name + item2 + data[i].TimeSend + item3 + data[i].MessageSend + item4;
                                $(".chat-history ul").prepend(itemAdd);
                            }
                        }
                        setTimeout(function () {
                            $(".chat-history").scrollTop(900);
                        }, 200);
                    }
                }
            });
        }
    }
}
var checkOpenIcon = true;
$(".chat-message .icon").click(function () {
    $("#message-to-send").val($("#message-to-send").val() + $(this).text());
});
$(".fa-smile-o").click(function () {
    if (checkOpenIcon) {
        $(".list-icon").show();
        checkOpenIcon = false;
    } else {
        $(".list-icon").hide();
        checkOpenIcon = true;
    }
});

$("#message-to-send").focus(function () {
    $(".list-icon").hide();
    checkOpenIcon = true;
});

$(".delete-suggest").click(function () {
    $(this).parents(".suggest-friend").slideUp(300);
});

$(".add-friend").click(function () {
    var suggestUser = $(this).next().val();
    if ($(this).text().trim() === "Kết bạn") {
        hub.server.sendRequest(suggestUser, MyUserName, true);
    } else {
        hub.server.sendRequest(suggestUser, MyUserName, false);
    }
});
function load() {
    $.ajax({
        type: "GET",
        url: "/Home/Edit",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            html += '<ul>';
            html += '<li>' + '<span class="ht-left">Nick Name</span>' + '<span class="ht-right">' + result.UserName + '</span>' + '</li>';
            html += '<li>' + '<span class="ht-left">Họ tên</span>' + '<span class="ht-right">' + result.Name + '</span>' + '</li>';
            html += '</ul>';
            $('.info-user').html(html);
        },
        error: function (message) {
            alert(message.responseText);
        }

    });
}

function Update() {
    var object = {
        UserName: MyUserName,
        Password: $('#PassWord').val(),
        Name: $('#Name').val()
    };

    $.ajax({
        type: "POST",
        url: "/Home/SaveData",
        data: JSON.stringify(object),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            load();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function UploadAvatar(formData) {
    url = "";
    if (checkAvarta) {
        url = "/Home/UploadAvatar?id=1";
    } else {
        url = "/Home/UploadAvatar?id=2";
    }
    var ajaxConfig = {
        type: "POST",
        url: url,
        data: new FormData(formData),
        success: function (result) {
            $(".avatar .img-responsive img").attr("src", result.Avatar);
            $(".background img").attr("src", result.PicUrl);

            $(".modal-backdrop").remove();
            $("#myModal #close").click();
        }
    }
    if ($(formData).attr('enctype') == "multipart/form-data") {
        ajaxConfig["contentType"] = false;
        ajaxConfig["processData"] = false;
    }
    $.ajax(ajaxConfig);

    return false;
}