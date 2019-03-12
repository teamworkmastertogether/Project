$(document).ready(function () {
    $('#inputComment').keypress(function (e) {
        if (e.which === 13) {
            var username = "Nguyễn Tiến Xuân";
            var message = $(this).val();
            $(this).val('');
            $('#demo .comment-level img').attr('src', 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_01.jpg');
            $('#demo .comment-level img:eq(0)').css('width', '35px');
            $('#demo .comment-level img:eq(1)').css('width', '25px');
            $('#demo .comment-level a:eq(0)').text(username);
            $('#demo .comment-level span:eq(0)').text(message);

            var demo = $('#demo').html();
            $('.comment-post-wrapper').append(demo);
        }
    });

    $('.comment-post-wrapper').on('click', '#btnReply', function () {
        $(this).parent().parent().parent().parent().next().next().show();
        $(this).css('text-decoration', 'none');
    });

    $('.comment-post-wrapper').on('keypress', '#inputReply', function (e) {
        if (e.which === 13) {

            var username = "Nigga";
            var message = $(this).val();
            $(this).val('');
            $('#demo2 .comment-level2 img').attr('src', 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_01.jpg');
            $('#demo2 .comment-level2 img').css('width', '25px');
            $('#demo2 .comment-level2 a:eq(0)').text(username);
            $('#demo2 .comment-level2 span:eq(0)').text(message);

            var demo2 = $('#demo2').html();
            $(this).parent().parent().prev().append(demo2);
        }
    });

    $('.pick-emoji').click(function () {
        $('.list-icon-comment').toggle();
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

    $('#setting').click(function () {
        $('.setting-menu').toggle();
    });

    $('.comment-post-wrapper').on('click', '#setting-comment', function () {
        $(this).next().toggle();
    });

    $('.comment-post-wrapper').on('click', '#setting-reply', function () {
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

    $('.setting-menu_list-item').click(function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('.comment-post-wrapper').on('click', '.setting-comment_list-item', function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('a[href^="#"]').click(function (e) {
        e.preventDefault();
    });

    $(".comment-post").on('click', '.icon', function () {
        var input = $(this).parent().prev().prev();
        input.val(input.val() + $(this).html());
    });

    $('#like-post').click(function () {
        if ($(this).hasClass('clicked')) {
            $(this).removeClass('clicked');
            $(this).next().find('.countLike_post').html(parseInt($(this).next().find('.countLike_post').html()) - 1);
            $('#like-post a').css('font-weight', 'normal').css('color', 'blue');
        } else {
            $(this).addClass('clicked');
            $(this).next().find('.countLike_post').html(parseInt($(this).next().find('.countLike_post').html()) + 1);
            $('#like-post a').css('font-weight', 'bold').css('text-decoration', 'none').css('color', 'red');
        }
    });

    $('#save-post').click(function () {
        $('#save-post a').css('font-weight', 'bold').css('text-decoration', 'none').css('color', 'red');
    });

    $('.comment-post-wrapper').on('click', '#btnLikeComment', function () {
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

    $('.comment-post-wrapper').on('click', '#btnLikeReply', function () {
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

    $('#btnShowComment').click(function () {
        $('#btnShowComment a').css('text-decoration', 'none');
    });

    $('.comment-post-wrapper').on('click', '#delete-comment', function () {
        $(this).parent().parent().parent().parent().parent().remove();
    });

    $('.comment-post-wrapper').on('click', '#delete-reply', function () {
        $(this).parent().parent().parent().parent().parent().remove();
    });

    $('.post').on('click', '#delete-post', function () {
        $('.post').remove();
    });

    $('.comment-post').on('click', '#edit-comment', function () {
        var oldData = $(this).parents('.comment-content_setting').prev().find('span').text();
        $(this).parents('.comment-content_setting').prev().find('span').hide();
        var res = $(this).parents('.comment-content_setting').prev().find('textarea');
        res.val(oldData).show();
        $(this).parent().hide();
        $(this).parents('.comment-content_setting').next().next().hide();
    });

    $('.comment-post').on('click', '#edit-reply', function () {
        var oldData = $(this).parents('.reply-content_setting').prev().find('span').text();
        $(this).parents('.reply-content_setting').prev().find('textarea').val(oldData).show();
        $(this).parent().hide();
    });

    $('.comment-post').on('keypress', '#edit-clone', function (e) {
        if (e.which === 13) {
            $(this).prev().text($(this).val());
            $(this).hide();
            $(this).prev().show();
            $(this).parents('.comment-content').children().eq(3).show();
        }
    });

});

