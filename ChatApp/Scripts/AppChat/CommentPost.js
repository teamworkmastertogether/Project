$(document).ready(function () {
    $('.post').on('keypress','.inputComment', function (e) {
        if (e.which === 13 && $(this).val() !== "") {
            var username = "Nguyễn Tiến Xuân";
            var message = $(this).val();
            $(this).val('');
            $(this).closest('.textbox-comment').prev().children().find('img').attr('src', 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_01.jpg');
            $(this).closest('.textbox-comment').prev().children().find('img').eq(0).css('width', '35px');
            $(this).closest('.textbox-comment').prev().children().find('img').eq(1).css('width', '25px');
            $(this).closest('.textbox-comment').prev().find('.name-comment').text(username);
            $(this).closest('.textbox-comment').prev().find('span').eq(0).text(message);

            var demo = $(this).closest('.textbox-comment').prev().html();
            $(this).closest('.textbox-comment').prev().prev().append(demo);
        }
    });

    $('.comment-post-wrapper').on('click', '.btnReply', function () {
        $(this).closest('.row').next().next().next().show();
        $(this).css('text-decoration', 'none');
    });

    $('.comment-post-wrapper').on('keypress', '.inputReply', function (e) {
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

    $('.textbox-comment').on('click','.pick-emoji',function () {
        $(this).next().toggle();
    });

    $(document).mouseup(function (e) {
        var container = $('.list-icon-comment');
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            container.hide();
        }
    });

    $('.comment-post-wrapper').on('click', '.pick-emoji-reply', function () {
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

    $('.post').on('click','.setting',function () {
        $(this).next().toggle();
    });

    $('.comment-post-wrapper').on('click', '.setting-comment', function () {
        $(this).next().toggle();
    });

    $('.comment-post-wrapper').on('click', '.setting-reply', function () {
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

    $('.post').on('click','.setting-menu_list-item', function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('.comment-post-wrapper').on('click', '.setting-comment_list-item', function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('.comment-post-wrapper').on('click', '.setting-reply_list-item', function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('a[href^="#"]').click(function (e) {
        e.preventDefault();
    });

    $(".comment-post").on('click', '.icon', function () {
        var input = $(this).parent().prev().prev();
        input.val(input.val() + $(this).html());
    });

    $('.footer-post').on('click','.like-post',function () {
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

    $('.footer-post').on('click','.save-post',function () {
        $(this).children().css('font-weight', 'bold').css('text-decoration', 'none').css('color', 'red');
    });

    $('.comment-post-wrapper').on('click', '.btnLikeComment', function () {
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

    $('.comment-post-wrapper').on('click', '.btnLikeReply', function () {
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

    $('.comment-post-wrapper').on('click', '.delete-comment', function () {
        $(this).closest('.row').remove();
    });

    $('.comment-post-wrapper').on('click', '.delete-reply', function () {
        $(this).closest('.row').remove();
    });

    $('.post-space').on('click', '.delete-post', function () {
        $(this).closest('.post').remove();
    });

    $('.edit-post').click(function () {
        var oldData = $(this).parents('.header-post').next().children().eq(0).text();
        $(this).parents('.header-post').next().children().eq(0).hide();
        var res = $(this).parents('.header-post').next().find('textarea');
        res.next().show();
        res.val(oldData).show();
        $(this).parent().hide();
    });

    $('.comment-post').on('click', '.edit-comment', function () {
        var oldData = $(this).parents('.comment-content_setting').prev().find('span').text();
        $(this).parents('.comment-content_setting').prev().find('span').hide();
        var res = $(this).closest('.comment-content_setting').prev().find('textarea');
        res.val(oldData).show();
        $(this).parent().hide();
        $(this).parents('.comment-content_setting').next().next().hide();
    });

    $('.comment-post').on('click', '.edit-reply', function () {
        var oldData = $(this).parents('.reply-content_setting').prev().find('span').text();
        $(this).parents('.reply-content_setting').prev().find('textarea').val(oldData).show();
        $(this).parent().hide();
        $(this).parents('.reply-content').next().hide();
    });

    $('.comment-post').on('keypress', '.edit-clone', function (e) {
        if (e.which === 13 && $(this).prev().text($(this).val()) !== "") {
            $(this).prev().text($(this).val());
            $(this).hide();
            $(this).prev().show();
            $(this).parents('.comment-content').children().eq(3).show();
            $(this).parents('.reply-content').next().show();
        }
    });

    $('.post').on('click', '.save-edited-post', function () {
        if ($(this).parent().prev().val() !== "") {
            $(this).parent().prev().prev().text($(this).parent().prev().val());
            $(this).parent().hide();
            $(this).parent().prev().hide();
            $(this).parent().prev().prev().show();
        }
    });

    $('.post').on('click', '.cancel-edited-post', function () {
        $(this).parent().prev().prev().show();
        $(this).parent().hide();
        $(this).parent().prev().hide();
        $(this).parent().prev().prev().show();
    });
});

