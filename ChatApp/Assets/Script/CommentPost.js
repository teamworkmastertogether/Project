
var IdPost;
var GroupNameCurrent = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();

$(document).ready(function () {

    $('.post-space').on('keypress','.inputComment', function (e) {
        if (e.which === 13 && $(this).val() !== "") {
            var CommentDto = {
                PostId: parseInt($(this).closest('.post').attr("id")),
                Text: $(this).val()
            };
            $(this).val('');
            $.ajax({
                type: "POST",
                url: "/Subject/SaveComment",
                data: JSON.stringify(CommentDto),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                        hub.server.createCommentNew(MyUserName,GroupNameCurrent, CommentDto.PostId, result);
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
            var SubCommentDto = {
                CommentId: parseInt($(this).closest('.comment-level').attr("id")),
                Text: $(this).val()
            };
            $(this).val('');

            current = $(this);

            $.ajax({
                type: "POST",
                url: "/Subject/SaveSubComment",
                data: JSON.stringify(SubCommentDto),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                        hub.server.createSubCommentNew(MyUserName,GroupNameCurrent, SubCommentDto.CommentId, result);
                },
                error: function (message) {
                    alert(message.responseText);
                }
            });
        }
    });

    $('.post-space').on('click', '.life', function () {
        $(this).parent().parent().next().toggle();
    });

    $('.post-space').on('click', '.list-icon-post span', function () {
        var input = $(this).parents('.tool').next().find('textarea');
        input.val(input.val() + $(this).text());
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
        input.val(input.val() + $(this).text());
    });

    $('.post-space').on('click','.like-post',function () {
        if ($(this).find("a").hasClass('clicked')) {
            $(this).find("a").removeClass('clicked');
        } else {
            $(this).find("a").addClass('clicked');
        }

        postId = parseInt($(this).closest(".post").attr("id"));
        url = "/Notifi/SaveLikePost?id=" + postId;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                hub.server.saveLikePost(MyUserName,result, postId, GroupNameCurrent);
            },
            error: function (message) {
                alert(message.responseText);
            }
        });
    });

    $('.post-space').on('click','.save-post',function () {
        $('.confirm').show();
        IdPost = parseInt($(this).closest(".post").attr("id"));
    }); 

    $('.confirm').on('click','#modal-btn-si',function () {
        $('.confirm').hide();
        url = "/Subject/CreatePostSaved?id=" + IdPost;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                Swal.fire(
                    'Thành công!',
                    'Bạn đã lưu bài viết thành công!',
                    'success'
                );
            },
            error: function (message) {
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: 'Something went wrong!',
                    footer: '<a href>Why do I have this issue?</a>'
                })
            }
        });
    });

    $('.confirm').on('click', '#modal-btn-no', function () {
        $('.confirm').hide();
    });

    $('.confirm').on('click', 'span', function () {
        $('.confirm').hide();
    });

    $('.post-space').on('click', '.btnLikeComment', function () {
        if ($(this).find("span").hasClass('clicked')) {
            $(this).find("span").removeClass('clicked');
        } else {
            $(this).find("span").addClass('clicked');
        }
        commentId = parseInt($(this).closest(".comment-level").attr("id"));
        url = "/Notifi/SaveLikeComment?id=" + commentId;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                hub.server.saveLikeComment(MyUserName,result, commentId, GroupNameCurrent);
            },
            error: function (message) {
                alert(message.responseText);
            }
        });

    });

    $('.post-space').on('click', '.btnLikeReply', function () {
        if ($(this).find("a").hasClass('clicked')) {
            $(this).find("a").removeClass('clicked');
        } else {
            $(this).find("a").addClass('clicked');
        }
        subCommentId = parseInt($(this).closest(".comment-level2").attr("id"));
        url = "/Notifi/SaveLikeSubComment?id=" + subCommentId;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                hub.server.saveLikeSubComment(MyUserName,result, subCommentId, GroupNameCurrent);
            },
            error: function (message) {
                alert(message.responseText);
            }
        });
    });

    $('.btnShowComment').click(function () {
        $(this).find('a').css('text-decoration', 'none');
    });

    $('.post-space').on('click', '.delete-comment', function () {
        commentId = parseInt($(this).closest('.comment-level').attr("id"));
        url = "/Subject/DeleteComment?commentId=" + commentId;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                hub.server.deleteComment(GroupNameCurrent, commentId);
            },
            error: function (message) {
                alert(message.responseText);
            }
        });

    });

    $('.post-space').on('click', '.delete-reply', function () {
        subcommentId = parseInt($(this).closest('.comment-level2').attr("id"));
        url = "/Subject/DeleteSubComment?subcommentId=" + subcommentId;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                hub.server.deleteSubComment(GroupNameCurrent, subcommentId);
            },
            error: function (message) {
                alert(message.responseText);
            }
        });
    });

    $('.post-space').on('click', '.delete-post', function () {

        postId = parseInt($(this).closest('.post').attr("id"));
        url = "/Subject/DeletePost?postId=" + postId;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                hub.server.deletePost(GroupNameCurrent, postId);
                Swal.fire(
                    'Thành công!',
                    'Bạn đã xóa bài viết thành công!',
                    'success'
                );
            },
            error: function (message) {
                alert(message.responseText);
            }
        });

    });

    $('.post-space').on('click', '.edit-post', function () {
        var oldData = $(this).parents('.header-post').next().children().eq(0).html().trim().replace(/\<br>/g, '\n');
        $(this).parents('.header-post').next().children().eq(0).hide();
        var res = $(this).parents('.header-post').next().find('textarea');
        res.next().show();
        res.val(oldData).show();
        $(this).parent().hide();
    });

    $('.post-space').on('click', '.edit-comment', function () {
        $('.edit-clone').hide();
        $('.comment-level span').show();
        $('.reply-button').show();
        $('.subcomment-button').show();
        var oldData = $(this).parents('.comment-content_setting').prev().find('span').text().trim();
        $(this).closest('.comment-content_setting').prev().find('span').hide();
        var res = $(this).closest('.comment-content_setting').prev().find('textarea');
        res.val(oldData).show();
        $(this).parent().hide();
        $(this).parents('.comment-content_setting').next().next().hide();
    }); 

    $('.post-space').on('click', '.edit-reply', function () {
        $('.edit-clone').hide();
        $('.comment-level span').show();
        $('.reply-button').show();
        $('.subcomment-button').show();

        var oldData = $(this).parents('.reply-content_setting').prev().find('span').text().trim();
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
            textEdit = $(this).val();
            if ($(this).hasClass("check-edit-comment")) {
                commentId = parseInt($(this).closest('.comment-level').attr("id"));
                CommentDto = {
                    CommentId: commentId,
                    Text: textEdit
                };
                $.ajax({
                    type: "POST",
                    url: "/Subject/EditComment",
                    data: JSON.stringify(CommentDto),
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        hub.server.editComment(GroupNameCurrent, CommentDto);
                    },
                    error: function (message) {
                        alert(message.responseText);
                    }
                });
            } else {
                subcommentId = parseInt($(this).closest('.comment-level2').attr("id"));
                SubCommentDto = {
                    SubCommentId: subcommentId,
                    Text: textEdit
                };
                $.ajax({
                    type: "POST",
                    url: "/Subject/EditSubComment",
                    data: JSON.stringify(SubCommentDto),
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        hub.server.editSubComment(GroupNameCurrent, SubCommentDto);
                    },
                    error: function (message) {
                        alert(message.responseText);
                    }
                });
            }
        }
    });

    $('.post-space').on('click', '.save-edited-post', function () {
        if ($(this).parent().prev().val() !== "") {
            $(this).parent().prev().prev().text($(this).parent().prev().val());
            $(this).parent().hide();
            $(this).parent().prev().hide();
            $(this).parent().prev().prev().show();
            textEdit = $(this).parent().prev().val();
            postId = parseInt($(this).closest('.post').attr("id"));
            PostDto = {
                PostId: postId,
                PostText: textEdit
            };
            $.ajax({
                type: "POST",
                url: "/Subject/EditPost",
                data: JSON.stringify(PostDto),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    hub.server.editPost(GroupNameCurrent, PostDto);
                    Swal.fire(
                        'Thành công!',
                        'Bạn đã sửa bài viết thành công!',
                        'success'
                    );
                },
                error: function (message) {
                    alert(message.responseText);
                }
            });

        }
    });

    $('.post-space').on('click', '.cancel-edited-post', function () {
        $(this).parent().prev().prev().show();
        $(this).parent().hide();
        $(this).parent().prev().hide();
        $(this).parent().prev().prev().show();
    });

    $('.post-space').on('click', '#postNew', function () {
        $('.think').height(50);
        $('.rig').height(200);
        if ($(this).closest('.box').find('textarea').val() !== "") {
            var PostDto = {
                GroupName: GroupNameCurrent,
                PostText: $(this).closest('.box').find('textarea').val().replace(/\n/g,'<br>'),
                TimePost: GetDateNow()
            };

            $(this).closest('.box').find('textarea').val('');

            //$.ajax({
            //    type: "POST",
            //    url: "/Subject/SavePost",
            //    data: JSON.stringify(PostDto),
            //    contentType: "application/json;charset=utf-8",
            //    dataType: "json",
            //    success: function (result) {
            //        hub.server.createPostNew(GroupNameCurrent, result);
            //        Swal.fire(
            //            'Thành công!',
            //            'Bạn đã đăng bài thành công!',
            //            'success'
            //        );
            //    },
            //    error: function (message) {
            //        Swal.fire({
            //            type: 'error',
            //            title: 'Oops...',
            //            text: 'Something went wrong!',
            //            footer: '<a href>Why do I have this issue?</a>'
            //        })
            //    }

            //});
            
        }
    });

    $('.post-space').on('keyup', '.think', function () {
        $(this).height(50);
        $(this).height(this.scrollHeight);
        $(this).closest('.rig').height(this.scrollHeight + this.next().height() + 150);
        $(this).css('overflow', 'auto');
    });

    $('.post-space').find('.think').keyup();
});

$('.confirm').on('click', '#modal-btn-no', function () {
    $('.confirm').hide();
});

$('.confirm').on('click', 'span', function () {
    $('.confirm').hide();
});

function GetUrlPostImage(formData) {
    url = "/Subject/GetUrlPostImage";
    var ajaxConfig = {
        type: "POST",
        url: url,
        data: new FormData(formData),
        success: function (result) {
            //alert(result);
            $('.textbox img').attr('src', result);
            $('#myModal #close').click();
            $('#xButton').show();
        }
    }
    if ($(formData).attr('enctype') == "multipart/form-data") {
        ajaxConfig["contentType"] = false;
        ajaxConfig["processData"] = false;
    }
    $.ajax(ajaxConfig);

    return false;
}

$("#selectFile").click(function() {
    $("#upload-post-image").trigger('click');
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#blah').attr('src', e.target.result);
        };
        $("#upImg").show();
        reader.readAsDataURL(input.files[0]);
    }
}

$("#upload-post-image").change(function () {
    readURL(this);
});

$('#xButton').click(function () {
    $('.post-image').attr('src', '');
    $(this).hide();
});


