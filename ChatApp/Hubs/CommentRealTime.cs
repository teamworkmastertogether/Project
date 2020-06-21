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
        /// <summary>
        /// Tạo bài đăng
        /// Created By: NBDuong 22.05.2020
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="postDto"></param>
        /// <param name="notiDto"></param>
        public void CreatePostNew(string GroupName,PostDto postDto ,NotifiDto notiDto)
        {
            Clients.All.CreatePost(GroupName, postDto,notiDto);
        }

        /// <summary>
        /// Tạo comment cấp 1
        /// Created By: NBDuong 24.05.2020
        /// </summary>
        /// <param name="myuserName"></param>
        /// <param name="groupName"></param>
        /// <param name="postId"></param>
        /// <param name="commentDto"></param>
        /// <param name="notiDto"></param>
        public void CreateCommentNew(string myuserName,string groupName,int postId, CommentDto commentDto,NotifiDto notiDto)
        {
            Clients.All.CreateComment(groupName, postId, commentDto);
            if (myuserName != commentDto.UserName)
            {
                List<string> listId = GetListConnectIdByUserName("No value", commentDto.UserName);
                Clients.Clients(listId).UpdateNotiRealtime(notiDto);
            }
        }
        
        /// <summary>
        /// Tạo comment cấp 2 
        /// Created By: NBDuong 26.05.2020
        /// </summary>
        /// <param name="myuserName"></param>
        /// <param name="groupName"></param>
        /// <param name="commentId"></param>
        /// <param name="subCommentDto"></param>
        /// <param name="notiDto"></param>
        public void CreateSubCommentNew(string myuserName,string groupName, int commentId, SubCommentDto subCommentDto,NotifiDto notiDto)
        {
            Clients.All.CreateSubComment(groupName, commentId, subCommentDto);
            if (myuserName != subCommentDto.UserName)
            {
                List<string> listId = GetListConnectIdByUserName("No value", subCommentDto.UserName);
                Clients.Clients(listId).UpdateNotiRealtime(notiDto);
            }
        }

        /// <summary>
        /// Xóa bài đăng
        /// Created by: NBDuong 27.05.2020
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="postId"></param>
        public void DeletePost(string groupName, int postId)
        {
            Clients.All.DeletePost(groupName, postId);
        }

        /// <summary>
        /// Xóa comment cấp 1
        /// Created By: NBDuong 27.05.2020
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="commentId"></param>
        public void DeleteComment(string groupName, int commentId)
        {
            Clients.All.DeleteComment(groupName, commentId);
        }

        /// <summary>
        /// Xóa comment cấp 2
        /// Created By: NBDuong 27.05.2020
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="subcommentId"></param>
        public void DeleteSubComment(string groupName, int subcommentId)
        {
            Clients.All.DeleteSubComment(groupName, subcommentId);
        }

        /// <summary>
        /// Sửa bài post
        /// Created By: NBDuong 28.05.2020
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="postDto"></param>
        public void EditPost(string groupName, PostDto postDto)
        {
            Clients.All.EditPost(groupName, postDto);
        }

        /// <summary>
        /// Sửa comment cấp 1
        /// Created by: NBDuong 28.05.2020
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="commentDto"></param>
        public void EditComment(string groupName, CommentDto commentDto)
        {
            Clients.All.EditComment(groupName, commentDto);
        }

        /// <summary>
        /// Sửa comment cấp 2
        /// Created By: NBDuong 28.05.2020
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="subcommentDto"></param>
        public void EditSubComment(string groupName, SubCommentDto subcommentDto)
        {
            Clients.All.EditSubComment(groupName, subcommentDto);
        }

        /// <summary>
        /// Like bài post
        /// Created By: NBDuong 29.05.2020
        /// </summary>
        /// <param name="myuserName"></param>
        /// <param name="likeDto"></param>
        /// <param name="postId"></param>
        /// <param name="groupName"></param>
        public void SaveLikePost(string myuserName, LikeDto likeDto, int postId, string groupName)
        {
            Clients.All.SaveLikePost(likeDto.Check, postId, groupName);
            if (myuserName != likeDto.UserName)
            {
                List<string> listId = GetListConnectIdByUserName("No value", likeDto.UserName);
                Clients.Clients(listId).UpdateNotiRealtime();
            }
        }

        /// <summary>
        /// Like comment cấp 1
        /// Created By: NBDuong 29.05.2020
        /// </summary>
        /// <param name="myuserName"></param>
        /// <param name="likeDto"></param>
        /// <param name="commentId"></param>
        /// <param name="groupName"></param>
        public void SaveLikeComment(string myuserName, LikeDto likeDto, int commentId, string groupName)
        {
            Clients.All.SaveLikeComment(likeDto.Check, commentId, groupName);
            if (myuserName != likeDto.UserName)
            {
                List<string> listId = GetListConnectIdByUserName("No value", likeDto.UserName);
                Clients.Clients(listId).UpdateNotiRealtime();
            }
        }

        /// <summary>
        /// Like comment cấp 2
        /// Created By: NBDuong 29.05.2020
        /// </summary>
        /// <param name="myuserName"></param>
        /// <param name="likeDto"></param>
        /// <param name="subCommentId"></param>
        /// <param name="groupName"></param>
        public void SaveLikeSubComment(string myuserName, LikeDto likeDto, int subCommentId, string groupName)
        {
            Clients.All.SaveLikeSubComment(likeDto.Check, subCommentId, groupName);
            if (myuserName != likeDto.UserName)
            {
                List<string> listId = GetListConnectIdByUserName("No value", likeDto.UserName);
                Clients.Clients(listId).UpdateNotiRealtime();
            }
        }

        /// <summary>
        /// Gửi lời mời kết bạn
        /// Created By: NBDuong 30.05.2020
        /// </summary>
        /// <param name="username"></param>
        public void SendRequestAddFriend(string username)
        {
            List<string> listId = GetListConnectIdByUserName("No value", username);
            Clients.Clients(listId).UpdateNotiRequestAddFriend();
        }
    }
}