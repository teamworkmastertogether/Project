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
    }
}