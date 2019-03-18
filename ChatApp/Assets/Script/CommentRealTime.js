
$(function () {
    hub.client.createPost = function (GroupName, postDto) {
        name = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();
        if (postDto.UserName !== MyUserName) {
            countNoti = parseInt($(".badge").text()) + 1;
            $(".badge").text(countNoti).show();
        }
        if (name === GroupName) {
            $(".post-clone .avatar-post").find('img').attr('src', postDto.avatar);
            $(".post-clone").find('.name-user a').text(postDto.NameOfUser);
            $(".post-clone").find('.time-post').text(postDto.TimePost);
            $(".post-clone").find('.content-post').eq(1).find('p').text(postDto.PostText);
            $(".post-clone").find('.countLike_post').text(postDto.LikeNumber);
            $(".post-clone .post").attr('id', postDto.PostId);
            var demo = $(".post-clone").html();
            $(".post-clone .post").attr('id', 0);
            $(".post-append").prepend(demo);
        }
    };

    hub.client.createComment = function (groupName, postId, commentDto) {
        name = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();
        Post = $(".post[id=" + postId + "]");
        if (name === groupName) {
            $('.post-clone .comment-level .img-comment img').attr('src', commentDto.Avatar);
            $('.post-clone .name-comment').text(commentDto.NameOfUser);
            $('.post-clone .name-comment').next().text(commentDto.Text);
            $('.post-clone .comment-level').attr('id', commentDto.CommentId);
            var demo = $('.post-clone .comment-level').parent().html();
            $('.post-clone .comment-level').attr('id',0);
            Post.find('.comment-post-wrapper').append(demo);
        }
    };

    hub.client.updateNotiRealtime = function (userName) {
            countNoti = parseInt($(".badge").text()) + 1;
            $(".badge").text(countNoti).show();
    };

    hub.client.createSubComment = function (groupName, commentId, subCommentDto) {
        name = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();
        Comment = $(".comment-level[id=" + commentId + "]");
        if (name === groupName) {
            $('.post-clone .comment-level2 img:eq(0)').attr('src', subCommentDto.Avatar);
            $('.post-clone .comment-level2').find('.name-reply').eq(0).text(subCommentDto.NameOfUser);
            $('.post-clone .comment-level2').find('.name-reply').eq(0).next().text(subCommentDto.Text);
            $('.post-clone .comment-level2').attr('id', subCommentDto.SubCommentId);
            var demo2 = $('.post-clone .comment-level2').parent().html();
            $('.post-clone .comment-level2').attr('id',0);
            Comment.find('.comment-wrapper2').append(demo2);
        }
    };

    hub.client.deletePost = function (groupName, postId) {
        name = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();
        Post = $(".post[id=" + postId + "]");
        if (name === groupName) {
            Post.remove();
        }
    };

    hub.client.deleteComment = function (groupName, commentId) {
        name = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();
        Comment = $(".comment-level[id=" + commentId + "]");
        if (name === groupName) {
            Comment.remove();
        }
    };

    hub.client.deleteSubComment = function (groupName, subcommentId) {
        name = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();
        SubComment = $(".comment-level2[id=" + subcommentId + "]");
        if (name === groupName) {
            SubComment.remove();
        }
    };

    hub.client.editPost = function (groupName, postDto) {
        name = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();
        Post = $(".post[id=" + postDto.PostId + "]");
        if (name === groupName) {
            Post.find(".content-post p:eq(0)").text(postDto.PostText);
        }
    };

    hub.client.editComment = function (groupName, commentDto) {
        name = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();
        Comment = $(".comment-level[id=" + commentDto.CommentId + "]");
        if (name === groupName) {
            Comment.find(".comment-content span:eq(0)").text(commentDto.Text);
        }
    };

    hub.client.editSubComment = function (groupName, subcommentDto) {
        name = $(".ToolFb .ToolLeft h2:eq(0)").text().trim();
        SubComment = $(".comment-level2[id=" + subcommentDto.SubCommentId + "]");
        if (name === groupName) {
            SubComment.find(".reply-content span:eq(0)").text(subcommentDto.Text);
        }
    };
});