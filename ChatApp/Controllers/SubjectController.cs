using ChatApp.Models.Dto;
using ChatApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
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
                return RedirectToAction("Personal");
            }
            List<PostDto> listPostDto = db.Posts.Where(s => s.SubjectId == sub.Id)
                .Select(s => new PostDto
                {
                    Photo = s.Photo,
                    UrlPersonal = "/Home/Personal?id=" + s.UserId.ToString(),
                    CheckLiked = db.Likes.Any(a => a.UserId == user.Id && a.PostId == s.Id),
                    UserName = user.UserName,
                    Myavatar = user.Avatar,
                    TimePost = s.CreatedDate,
                    LikeNumber = db.Likes.Where(p => p.PostId == s.Id).ToList().Count(),
                    NameOfUser = s.User.Name,
                    PostText = s.Text,
                    avatar = s.User.Avatar,
                    PostId = s.Id,
                    listComment = db.Comments.Where(k => k.PostId == s.Id)
                    .Select(k => new CommentDto
                    {
                        UrlPersonal = "/Home/Personal?id=" + k.UserId.ToString(),
                        CheckLiked = db.Likes.Any(a => a.UserId == user.Id && a.CommentId == k.Id),
                        LikeNumber = db.Likes.Where(p => p.CommentId == k.Id).ToList().Count(),
                        NameOfUser = k.User.Name,
                        Text = k.Text,
                        Avatar = k.User.Avatar,
                        CommentId = k.Id,
                        listSubComment = db.SubComments.Where(p => p.CommentId == k.Id)
                        .Select(p => new SubCommentDto
                        {
                            UrlPersonal = "/Home/Personal?id=" + p.UserId.ToString(),
                            CheckLiked = db.Likes.Any(a => a.UserId == user.Id && a.SubCommentId == p.Id),
                            LikeNumber = db.Likes.Where(h => h.SubCommentId == p.Id).ToList().Count(),
                            NameOfUser = p.User.Name,
                            Text = p.Text,
                            Avatar = p.User.Avatar,
                            SubCommentId = p.Id
                        }).ToList()
                    }).OrderBy(k => k.CommentId).ToList()
                }).OrderByDescending(s => s.PostId).ToList();
            ViewBag.MyName = user.Name;
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
            post.SubjectId = subject.Id;
            post.Text = postDto.PostText;
            post.Photo = postDto.Photo;
            post.UserId = user.Id;
            post.CreatedDate = postDto.TimePost;
            db.Posts.Add(post);
            db.SaveChanges();
            int Postid = db.Posts.Max(s => s.Id);
            PostDto result = new PostDto
            {
                Photo = postDto.Photo,
                UrlPersonal = "/Home/Personal?id=" + user.Id.ToString(),
                UserName = userName,
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
                        Avatar = user.Avatar,
                        NameOfUser = user.Name,
                        TextNoti = "Đã đăng",
                        ClassIconName = "far fa-clock",
                        NotificationState = false
                    };
                    db.Notifications.Add(noti);
                }
            }
            NotifiDto notiDto = new NotifiDto
            {
                NameOfUser = user.Name,
                SubjectName = postDto.GroupName,
                Avatar = user.Avatar,
                SubjectId = subject.Id,
                TimeNotifi = postDto.TimePost,
                PostId = Postid,
                TextNoti = "Đã đăng"
            };
            db.SaveChanges();
            return Json(new { result, notiDto }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveComment(CommentDto commentDto)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            Post post = db.Posts.FirstOrDefault(s => s.Id == commentDto.PostId);
            Comment comment = new Comment
            {
                Text = commentDto.Text,
                PostId = commentDto.PostId,
                UserId = user.Id
            };
            db.Comments.Add(comment);
            db.SaveChanges();
            int CommentId = db.Comments.Max(s => s.Id);
            CommentDto result = new CommentDto
            {
                UserNameComment = userName,
                UrlPersonal = "/Home/Personal?id=" + user.Id.ToString(),
                UserName = post.User.UserName,
                LikeNumber = 0,
                NameOfUser = user.Name,
                Text = commentDto.Text,
                Avatar = user.Avatar,
                CommentId = CommentId
            };
            if (user.Id != post.User.Id)
            {
                Notification noti = new Notification
                {
                    UserId = comment.Post.User.Id,
                    PostId = commentDto.PostId,
                    NameOfUser = user.Name,
                    Avatar = user.Avatar,
                    TextNoti = "Đã bình luận bài viết của bạn",
                    ClassIconName = "far fa-comments",
                    NotificationState = false
                };
                db.Notifications.Add(noti);
            }
            NotifiDto notiDto = new NotifiDto
            {
                NameOfUser = user.Name,
                SubjectName = post.Subject.Name,
                Avatar = user.Avatar,
                SubjectId = post.SubjectId,
                TimeNotifi = post.CreatedDate,
                PostId = post.Id,
                TextNoti = "Đã bình luận bài viết của bạn"
            };
            db.SaveChanges();
            return Json(new { result, notiDto }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveSubComment(SubCommentDto subCommentDto)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            Comment comment = db.Comments.FirstOrDefault(s => s.Id == subCommentDto.CommentId);
            SubComment subComment = new SubComment
            {
                Text = subCommentDto.Text,
                CommentId = subCommentDto.CommentId,
                UserId = user.Id
            };
            db.SubComments.Add(subComment);
            db.SaveChanges();
            int SubCommentId = db.SubComments.Max(s => s.Id);
            SubCommentDto result = new SubCommentDto
            {
                UserNameComment = userName,
                UrlPersonal = "/Home/Personal?id=" + user.Id.ToString(),
                UserName = comment.User.UserName,
                LikeNumber = 0,
                NameOfUser = user.Name,
                Text = subCommentDto.Text,
                Avatar = user.Avatar,
                SubCommentId = SubCommentId
            };
            if (user.Id != comment.User.Id)
            {
                Notification noti = new Notification
                {
                    UserId = comment.User.Id,
                    PostId = comment.Post.Id,
                    NameOfUser = user.Name,
                    Avatar = user.Avatar,
                    TextNoti = "Đã trả lời bình luận của bạn",
                    ClassIconName = "far fa-comments",
                    NotificationState = false
                };
                db.Notifications.Add(noti);
            }
            NotifiDto notiDto = new NotifiDto
            {
                NameOfUser = user.Name,
                SubjectName = comment.Post.Subject.Name,
                Avatar = user.Avatar,
                SubjectId = comment.Post.SubjectId,
                TimeNotifi = comment.Post.CreatedDate,
                PostId = comment.Post.Id,
                TextNoti = "Đã trả lời bình luận của bạn"
            };
            db.SaveChanges();
            return Json(new { result, notiDto }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeletePost(int? postId)
        {
            Post post = db.Posts.FirstOrDefault(s => s.Id == postId);
            List<Like> list = db.Likes.Where(s => s.PostId == postId || postId == s.Comment.PostId || postId == s.SubComment.Comment.PostId).ToList();
            db.Likes.RemoveRange(list);
            db.Posts.Remove(post);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteComment(int? commentId)
        {
            Comment comment = db.Comments.FirstOrDefault(s => s.Id == commentId);
            List<Like> list = db.Likes.Where(s => s.CommentId == commentId ||  commentId == s.SubComment.CommentId).ToList();
            List<SubComment> list2 = db.SubComments.Where(s => commentId == s.CommentId).ToList();
            db.SubComments.RemoveRange(list2);
            db.Likes.RemoveRange(list);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteSubComment(int? subcommentId)
        {
            SubComment subcomment = db.SubComments.FirstOrDefault(s => s.Id == subcommentId);
            List<Like> list = db.Likes.Where(s => subcommentId == s.SubCommentId).ToList();
            db.Likes.RemoveRange(list);
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

        [HttpPost]
        public ActionResult CreatePostSaved(int id)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            PostSave postSave = new PostSave
            {
                PostId = id,
                UserId = user.Id
            };
            db.PostSaves.Add(postSave);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
		public ActionResult GetUrlPostImage(HttpPostedFileBase UploadImage)
		{
			if (UploadImage != null)
			{ 
				string fileName = Path.GetFileNameWithoutExtension(UploadImage.FileName);
				string extension = Path.GetExtension(UploadImage.FileName);
				fileName = fileName + extension;
				UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/ImagesUpload"), fileName));
			
				var link = "http://localhost:54576/Assets/ImagesUpload/" + fileName;

				return Json(link, JsonRequestBehavior.AllowGet);
			}
			return Json("Vui lòng chọn ảnh thích hợp !", JsonRequestBehavior.AllowGet);
		}
	}
}
