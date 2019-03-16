$(document).ready(function () {
    $('.post-space').on('keypress','.inputComment', function (e) {
        if (e.which === 13 && $(this).val() !== "") {

            var CommentDto = {
                PostId: $(this).closest('.post').data("id"),
                Text: $(this).val()
            };
            $(this).val('');

            current = $(this);
         
            $.ajax({
                type: "POST",
                url: "/Subject/SaveComment",
                data: JSON.stringify(CommentDto),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    current.closest('.textbox-comment').prev().children().find('img').attr('src', result.Avatar);
                    current.closest('.textbox-comment').prev().children().find('img').eq(0).css('width', '35px');
                    current.closest('.textbox-comment').prev().children().find('img').eq(1).css('width', '25px');
                    current.closest('.textbox-comment').prev().find('.name-comment').text(result.NameOfUser);
                    current.closest('.textbox-comment').prev().find('span').eq(0).text(result.Text);
                    current.closest('.textbox-comment').prev().children().attr('data-id', result.CommentId);
                    var demo = current.closest('.textbox-comment').prev().html();
                    current.closest('.textbox-comment').prev().prev().append(demo);
                },
                error: function (message) {
                    alert(message.responseText);
                }
            });
        }
    });

    $('.post-space').on('click', '.btnReply', function () {
        $(this).closest('.row').next().next().next().show();
        $(this).css('text-decoration', 'none');
    });

    $('.post-space').on('keypress', '.inputReply', function (e) {
        if (e.which === 13 && $(this).val() !== "") {
            var username = "Nigga";
            var message = $(this).val();
            $(this).val('');
            $(this).closest('.textbox-reply').prev().children().find('img').attr('src', 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_01.jpg');
            $(this).closest('.textbox-reply').prev().children().find('img').css('width', '25px');
            $(this).closest('.textbox-reply').prev().find('.name-reply').eq(0).text(username);
            $(this).closest('.textbox-reply').prev().find('span').eq(0).text(message);

            var demo2 = $(this).closest('.textbox-reply').prev().html();
            $(this).closest('.textbox-reply').prev().prev().append(demo2);
        }
    });

    $('.post-space').on('click', '.life', function () {
        $(this).parent().parent().next().toggle();
    });

    $('.post-space').on('click', '.list-icon-post span', function () {
        var input = $(this).parents('.tool').next().find('textarea');
        input.val(input.val() + $(this).html());
    });

    $(document).mouseup(function (e) {
        var container = $('.list-icon-post');
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            container.hide();
        }
    });

    $('.post-space').on('click','.pick-emoji',function () {
        $(this).next().toggle();
    });

    $(document).mouseup(function (e) {
        var container = $('.list-icon-comment');
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            container.hide();
        }
    });

    $('.post-space').on('click', '.pick-emoji-reply', function () {
        $(this).next().show();
    });

    $(document).mouseup(function (e) {
        var container = $('.setting-menu');
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            container.hide();
        }
    });

    $(document).mouseup(function (e) {
        var container = $('.list-icon-reply');
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            container.hide();
        }
    });

    $('.post-space').on('click','.setting',function () {
        $(this).next().toggle();
    });

    $('.post-space').on('click', '.setting-comment', function () {
        $(this).next().toggle();
    });

    $('.post-space').on('click', '.setting-reply', function () {
        $(this).next().toggle();
    });

    $(document).mouseup(function (e) {
        var container = $('.setting-comment_list');
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            container.hide();
        }
    });

    $(document).mouseup(function (e) {
        var container = $('.setting-reply_list');
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            container.hide();
        }
    });

    $('.post-space').on('click','.setting-menu_list-item', function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('.post-space').on('click', '.setting-comment_list-item', function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('.post-space').on('click', '.setting-reply_list-item', function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('a[href^="#"]').click(function (e) {
        e.preventDefault();
    });

    $(".post-space").on('click', '.icon', function () {
        var input = $(this).parent().prev().prev();
        input.val(input.val() + $(this).html());
    });

    $('.post-space').on('click','.like-post',function () {
        if ($(this).hasClass('clicked')) {
            $(this).removeClass('clicked');
            $(this).next().find('.countLike_post').html(parseInt($(this).next().find('.countLike_post').html()) - 1);
            $(this).children().css('font-weight', 'normal').css('color', 'blue');
        } else {
            $(this).addClass('clicked');
            $(this).next().find('.countLike_post').html(parseInt($(this).next().find('.countLike_post').html()) + 1);
            $(this).children().css('font-weight', 'bold').css('text-decoration', 'none').css('color', 'red');
        }
    });

    $('.post-space').on('click','.save-post',function () {
        //$(this).children().css('font-weight', 'bold').css('text-decoration', 'none').css('color', 'red');
        $('.confirm').show();
    }); 

    $('.confirm').on('click','#modal-btn-si',function () {
        $('.confirm').hide();
    });

    $('.post-space').on('click', '.btnLikeComment', function () {
        if ($(this).hasClass('clicked')) {
            $(this).removeClass('clicked');
            $(this).next().next().find('.countLike_comment')
                .html(parseInt($(this).next().next().find('.countLike_comment').html()) - 1);
            $(this).find('a span').css('font-weight', 'normal').css('color', '#337ab7');
        } else {
            $(this).addClass('clicked');
            $(this).next().next().find('.countLike_comment').html(parseInt($(this).next().next().find('.countLike_comment').html()) + 1);
            $(this).find('a').css('text-decoration', 'none');
            $(this).find('a span').css('font-weight', 'bold').css('color', '#ff0000');
        }

        if (parseInt($(this).next().next().find('.countLike_comment').html()) > 0) {
            $(this).next().next().css('display', 'block');
        } else {
            $(this).next().next().css('display', 'none');
        }
    });

    $('.post-space').on('click', '.btnLikeReply', function () {
        if ($(this).hasClass('clicked')) {
            $(this).removeClass('clicked');
            $(this).next().find('.countLike_reply')
                .html(parseInt($(this).next().find('.countLike_reply').html()) - 1);
            $(this).find('a').css('font-weight', 'normal').css('color', '#337ab7');
        } else {
            $(this).addClass('clicked');
            $(this).next().find('.countLike_reply').html(parseInt($(this).next().find('.countLike_reply').html()) + 1);
            $(this).find('a').css('font-weight', 'bold').css('text-decoration', 'none').css('color', 'red');
        }

        if (parseInt($(this).next().find('.countLike_reply').html()) > 0) {
            $(this).next().css('display', 'block');
        } else {
            $(this).next().css('display', 'none');
        }
    });

    $('.btnShowComment').click(function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('.post-space').on('click', '.delete-comment', function () {
        $(this).closest('.comment-level').remove();
    });

    $('.post-space').on('click', '.delete-reply', function () {
        $(this).closest('.row').remove();
    });

    $('.post-space').on('click', '.delete-post', function () {
        $(this).closest('.post').remove();
    });

    $('.post-space').on('click','.edit-post', function () {
        var oldData = $(this).parents('.header-post').next().children().eq(0).text();
        $(this).parents('.header-post').next().children().eq(0).hide();
        var res = $(this).parents('.header-post').next().find('textarea');
        res.next().show();
        res.val(oldData).show();
        $(this).parent().hide();
    });

    $('.post-space').on('click', '.edit-comment', function () {
        var oldData = $(this).parents('.comment-content_setting').prev().find('span').text();
        $(this).closest('.comment-content_setting').prev().find('span').hide();
        var res = $(this).closest('.comment-content_setting').prev().find('textarea');
        res.val(oldData).show();
        $(this).parent().hide();
        $(this).parents('.comment-content_setting').next().next().hide();
    }); 

    $('.post-space').on('click', '.edit-reply', function () {
        var oldData = $(this).parents('.reply-content_setting').prev().find('span').text();
        $(this).closest('.reply-content_setting').prev().find('span').hide();
        $(this).parents('.reply-content_setting').prev().find('textarea').val(oldData).show();
        $(this).parent().hide();
        $(this).parents('.reply-content').next().hide();
    });

    $('.post-space').on('keypress', '.edit-clone', function (e) {
        if (e.which === 13 && $(this).prev().text($(this).val()) !== "") {
            $(this).prev().text($(this).val());
            $(this).hide();
            $(this).prev().show();
            $(this).parents('.comment-content').children().eq(3).show();
            $(this).parents('.reply-content').next().show();
        }
    });

    $('.post-space').on('click', '.save-edited-post', function () {
        if ($(this).parent().prev().val() !== "") {
            $(this).parent().prev().prev().text($(this).parent().prev().val());
            $(this).parent().hide();
            $(this).parent().prev().hide();
            $(this).parent().prev().prev().show();
        }
    });

    $('.post-space').on('click', '.cancel-edited-post', function () {
        $(this).parent().prev().prev().show();
        $(this).parent().hide();
        $(this).parent().prev().hide();
        $(this).parent().prev().prev().show();
    });

    $('.post-space').on('click', '#postNew', function () {
        if ($(this).closest('.box').find('textarea').val() !== "") {
            var PostDto = {
                GroupName: $(".ToolFb .ToolLeft h2:eq(0)").text().trim(),
                PostText: $(this).closest('.box').find('textarea').val(),
                TimePost: GetDateNow()
            };

            $(this).closest('.box').find('textarea').val('');
            $.ajax({
                type: "POST",
                url: "/Subject/SavePost",
                data: JSON.stringify(PostDto),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $(".post-clone").find('img').attr('src', result.avatar);
                    $(".post-clone").find('.name-user a').text(result.NameOfUser);
                    $(".post-clone").find('.time-post').text(result.TimePost);
                    $(".post-clone").find('.content-post').eq(1).find('p').text(result.PostText);
                    $(".post-clone").find('.countLike_post').text(result.LikeNumber);
                    $(".post-clone .post").attr('data-id', result.PostId);
                    var demo = $(".post-clone").html();
                    $(".post-append").prepend(demo);
                    //id = $(".post-clone .post").data("id");
                },
                error: function (message) {
                    alert(message.responseText);
                }

            });
            
        }
    });

    $('.post-space').on('keyup', '.think', function () {
        $(this).height(this.scrollHeight);
        $(this).css('overflow', 'auto');
    });

    $('.post-space').find('.think').keyup();
});


