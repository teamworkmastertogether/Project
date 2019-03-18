using ChatApp.Models.Dto;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Hubs
{
    public partial class ChatHub : Hub
    {
        public void CreatePostNew(string GroupName,PostDto postDto )
        {
            Clients.All.CreatePost(GroupName, postDto);
        }

        public void CreateCommentNew(string groupName,int postId, CommentDto commentDto)
        {
            Clients.All.CreateComment(groupName, postId, commentDto);
        }

        public void CreateSubCommentNew(string groupName, int commentId, SubCommentDto subCommentDto)
        {
            Clients.All.CreateSubComment(groupName, commentId, subCommentDto);
        }

        public void DeletePost(string groupName, int postId)
        {
            Clients.All.DeletePost(groupName, postId);
        }

        public void DeleteComment(string groupName, int commentId)
        {
            Clients.All.DeleteComment(groupName, commentId);
        }

        public void DeleteSubComment(string groupName, int subcommentId)
        {
            Clients.All.DeleteSubComment(groupName, subcommentId);
        }
    }
}