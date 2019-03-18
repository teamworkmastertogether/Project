using ChatApp.Models.Dto;
using ChatApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    public class SubjectController : Controller
    {
        ChatDbcontext db = new ChatDbcontext();

        [HttpGet]
        public ActionResult GetSubject(int? id)
        {
            Subject sub = db.Subjects.FirstOrDefault(s => s.Id == id);
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));

            if (sub == null)
            {
                return RedirectToAction("Index");
            }
            List<PostDto> listPostDto = db.Posts.Where(s => s.SubjectId == sub.Id)
                .Select(s => new PostDto
                {
                    Myavatar = user.Avatar,
                    TimePost = s.CreatedDate,
                    LikeNumber = s.LikeNumber,
                    NameOfUser = s.User.Name,
                    PostText = s.Text,
                    avatar = s.User.Avatar,
                    PostId = s.Id,
                    listComment = db.Comments.Where(k => k.PostId == s.Id)
                    .Select(k => new CommentDto
                    {
                        LikeNumber = k.LikeNumber,
                        NameOfUser = k.User.Name,
                        Text = k.Text,
                        Avatar = k.User.Avatar,
                        CommentId = k.Id,
                        listSubComment = db.SubComments.Where(p => p.CommentId == k.Id)
                        .Select(p => new SubCommentDto
                        {
                            LikeNumber = p.LikeNumber,
                            NameOfUser = p.User.Name,
                            Text = p.Text,
                            Avatar = p.User.Avatar,
                            SubCommentId = p.Id
                        }).ToList()
                    }).OrderBy(k => k.CommentId).ToList()
                }).OrderByDescending(s => s.PostId).ToList();

            ViewBag.photo = sub.Photo;
            ViewBag.name = sub.Name;
            ViewBag.Avatar = user.Avatar;

            return View(listPostDto);
        }

        [HttpPost]
        public ActionResult SavePost(PostDto postDto)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            Subject subject = db.Subjects.FirstOrDefault(us => us.Name.Equals(postDto.GroupName));
            Post post = new Post();
            post.LikeNumber = 0;
            post.SubjectId = subject.Id;
            post.Text = postDto.PostText;
            post.UserId = user.Id;
            post.CreatedDate = postDto.TimePost;
            db.Posts.Add(post);
            db.SaveChanges();
            int Postid = db.Posts.Max(s => s.Id);
            PostDto result = new PostDto
            {
                TimePost = postDto.TimePost,
                LikeNumber = postDto.LikeNumber,
                NameOfUser = user.Name,
                PostText = postDto.PostText,
                avatar = user.Avatar,
                PostId = Postid
            };
            List<User> users = db.Users.ToList();
            foreach (var item in users)
            {
                if (item.Id != user.Id)
                {
                    Notification noti = new Notification
                    {
                        UserId = item.Id,
                        PostId = Postid,
                        TextNoti = "Đã đăng",
                        ClassIconName = "far fa-clock",
                        NotificationState = false
                    };
                    db.Notifications.Add(noti);
                }
            }
            db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveComment(CommentDto commentDto)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            Comment comment = new Comment
            {
                Text = commentDto.Text,
                LikeNumber = 0,
                PostId = commentDto.PostId,
                UserId = user.Id
            };
            db.Comments.Add(comment);
            db.SaveChanges();
            int CommentId = db.Comments.Max(s => s.Id);
            CommentDto result = new CommentDto
            {
                LikeNumber = 0,
                NameOfUser = user.Name,
                Text = commentDto.Text,
                Avatar = user.Avatar,
                CommentId = CommentId
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveSubComment(SubCommentDto subCommentDto)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            SubComment subComment = new SubComment
            {
                Text = subCommentDto.Text,
                LikeNumber = 0,
                CommentId = subCommentDto.CommentId,
                UserId = user.Id
            };
            db.SubComments.Add(subComment);
            db.SaveChanges();
            int SubCommentId = db.SubComments.Max(s => s.Id);
            SubCommentDto result = new SubCommentDto
            {
                LikeNumber = 0,
                NameOfUser = user.Name,
                Text = subCommentDto.Text,
                Avatar = user.Avatar,
                SubCommentId = SubCommentId
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeletePost(int? postId)
        {
            Post post = db.Posts.FirstOrDefault(s => s.Id == postId);
            List<Comment> comments = db.Comments.Where(s => s.PostId == postId).ToList();
            foreach (var item in comments)
            {
                List<SubComment> subcomments = db.SubComments.Where(s => s.CommentId == item.Id).ToList();
                db.SubComments.RemoveRange(subcomments);
            }
            db.Comments.RemoveRange(comments);
            db.Posts.Remove(post);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteComment(int? commentId)
        {
            Comment comment = db.Comments.FirstOrDefault(s => s.Id == commentId);
            List<SubComment> subcomments = db.SubComments.Where(s => s.CommentId == commentId).ToList();
            db.SubComments.RemoveRange(subcomments);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteSubComment(int? subcommentId)
        {
            SubComment subcomment = db.SubComments.FirstOrDefault(s => s.Id == subcommentId);
            db.SubComments.Remove(subcomment);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditPost(PostDto postDto)
        {
            Post post = db.Posts.FirstOrDefault(s => s.Id == postDto.PostId);
            post.Text = postDto.PostText;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditComment(CommentDto commentDto)
        {
            Comment comment = db.Comments.FirstOrDefault(s => s.Id == commentDto.CommentId);
            comment.Text = commentDto.Text;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSubComment(SubCommentDto subcommentDto)
        {
            SubComment subcomment = db.SubComments.FirstOrDefault(s => s.Id == subcommentDto.SubCommentId);
            subcomment.Text = subcommentDto.Text;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}
