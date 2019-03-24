using ChatApp.Models.Dto;
using ChatApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    public class NotifiController : Controller
    {
        private ChatDbcontext db = new ChatDbcontext();
        // GET: Notifi
        public JsonResult GetNotifi()
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            List<NotifiDto> listNotifi = db.Notifications.Where(s => s.UserId == user.Id)
                .Select(s => new NotifiDto
                {
                    NameOfUser = s.NameOfUser,
                    SubjectName = s.Post.Subject.Name,
                    Avatar = s.Avatar,
                    SubjectId = s.Post.SubjectId,
                    TimeNotifi = s.Post.CreatedDate,
                    PostId = s.PostId,
                    TextNoti = s.TextNoti,
                    ClassIconName = s.ClassIconName,
                    NotificationState = s.NotificationState,
                    NotificationId = s.Id
                }).ToList();
            return Json(listNotifi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetListUserSendRequest()
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            List<InforFriendDto> inforFriendDto = user.ListFriends.First().MemberOfListFriends.Where(s => s.AccessRequest == false)
                .Select(s => new InforFriendDto
                {
                    IdUser = s.UserId,
                    UserName = s.User.UserName,
                    Avatar = s.User.Avatar,
                    Name = s.User.Name,
                    UrlPersonal = "/Home/Personal?id=" + s.UserId
                }).ToList();
            return Json(inforFriendDto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSeenNotifi(int? Id)
        {
            int CheckSeen = 0;
            Notification noti = db.Notifications.FirstOrDefault(s => s.Id == Id);
            if (!noti.NotificationState)
            {
                CheckSeen = 1;
                noti.NotificationState = true;
            }
            db.SaveChanges();
            return Json(CheckSeen, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSeenAllNotifi()
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            List<Notification> listnoti = db.Notifications.Where(s => s.UserId == user.Id).ToList();
            foreach (var item in listnoti)
            {
                item.NotificationState = true;
            }
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLikePost(int? id)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            LikeDto result = new LikeDto
            {
                Check = false,
                UserName = ""
            };
            Like like = db.Likes.FirstOrDefault(s => s.UserId == user.Id && s.PostId == id);
            if (like != null)
            {
                db.Likes.Remove(like);
            }else
            {
                Like like2 = new Like { PostId = id, UserId = user.Id };
                db.Likes.Add(like2);
                result.Check = true;
            }
            if (result.Check)
            {
                Post post = db.Posts.FirstOrDefault(s => s.Id == id);
                if (post.UserId != user.Id)
                {
                    Notification noti = new Notification
                    {
                        UserId = post.UserId,
                        PostId = post.Id,
                        NameOfUser = user.Name,
                        Avatar = user.Avatar,
                        TextNoti = "Đã thích bài viết của bạn",
                        ClassIconName = "far fa-thumbs-up",
                        NotificationState = false
                    };
                    result.UserName = post.User.UserName;
                    db.Notifications.Add(noti);
                }
            }
            db.SaveChanges();
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLikeComment(int? id)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            LikeDto result = new LikeDto
            {
                Check = false,
                UserName = ""
            };
            Like like = db.Likes.FirstOrDefault(s => s.UserId == user.Id && s.CommentId == id);
            if (like != null)
            {
                db.Likes.Remove(like);
            }
            else
            {
                Like like2 = new Like { CommentId = id, UserId = user.Id };
                db.Likes.Add(like2);
                result.Check = true;
            }
            if (result.Check)
            {
                Comment comment = db.Comments.FirstOrDefault(s => s.Id == id);
                if (comment.UserId != user.Id)
                {
                    Notification noti = new Notification
                    {
                        UserId = comment.UserId,
                        PostId = comment.PostId,
                        NameOfUser = user.Name,
                        Avatar = user.Avatar,
                        TextNoti = "Đã bình thích bình luận của bạn",
                        ClassIconName = "far fa-thumbs-up",
                        NotificationState = false
                    };
                    result.UserName = comment.User.UserName;
                    db.Notifications.Add(noti);
                }
            }
            db.SaveChanges();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLikeSubComment(int? id)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            LikeDto result = new LikeDto
            {
                Check = false,
                UserName = ""
            };
            Like like = db.Likes.FirstOrDefault(s => s.UserId == user.Id && s.SubCommentId == id);
            if (like != null)
            {
                db.Likes.Remove(like);
            }
            else
            {
                Like like2 = new Like { SubCommentId = id, UserId = user.Id };
                db.Likes.Add(like2);
                result.Check = true;
            }
            if (result.Check)
            {
                SubComment subcomment = db.SubComments.FirstOrDefault(s => s.Id == id);
                if (subcomment.UserId != user.Id)
                {
                    Notification noti = new Notification
                    {
                        UserId = subcomment.UserId,
                        PostId = subcomment.Comment.PostId,
                        NameOfUser = user.Name,
                        Avatar = user.Avatar,
                        TextNoti = "Đã bình thích bình luận của bạn",
                        ClassIconName = "far fa-thumbs-up",
                        NotificationState = false
                    };
                    result.UserName = subcomment.User.UserName;
                    db.Notifications.Add(noti);
                }
            }
            db.SaveChanges();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}