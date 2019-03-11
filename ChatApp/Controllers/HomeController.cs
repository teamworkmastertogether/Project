using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    public class HomeController : Controller
    {
        ChatDbcontext db = new ChatDbcontext();

        public ActionResult Index()
        {
            return View(GetFriendSuggest());
        }

        [HttpGet]
        public ActionResult Login()
        {
            // Lấy user từ session
            var userName = Session["userName"];

            if (userName != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // Submit đăng nhập từ form
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            // Lấy username từ form
            string username = form["username"].ToString().Trim();
            // Lấy user có username và password trùng với form submit
            User user = db.Users.FirstOrDefault(x => x.UserName.Equals(username));
            // Kiểm tra xem user có tồn tại không
            if (user != null)
            {
                Session["userName"] = user.UserName;
                return RedirectToAction("Index");
            }
            ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
            return View();
        }

        // Đăng xuất tài khoản người dùng
        public ActionResult Logout()
        {
            // Hủy session lưu user và tên
            Session.Remove("userName");
            // Chuyển hướng đến trang login
            return RedirectToAction("Login");
        }

        public PartialViewResult _MenuPartialView()
        {
            var userName = Session["userName"] as string;
            List<InforFriendDto> listUser = db.Users.FirstOrDefault(s => s.UserName.Equals(userName))
            .ListFriends.First().MemberOfListFriends.Where(s => s.AccessRequest).OrderByDescending(s => s.TimeLastChat)
            .Select(s => new InforFriendDto { Avatar = s.User.Avatar, Name = s.User.Name, UserName = s.User.UserName, SeenMessage = s.SeenMessage })
            .Take(20).ToList();
            return PartialView(listUser);
        }
        public List<InforFriendDto> GetFriendSuggest()
        {
            var userName = Session["userName"] as string;
            List<string> listUserName1 = db.Users.FirstOrDefault(s => s.UserName.Equals(userName))
            .ListFriends.First().MemberOfListFriends.Select(s => s.User.UserName).ToList();
            Random rnd = new Random();
            //int from = rnd.Next(1, 100);
            int from = 1;
            List<InforFriendDto> listUser = db.Users.Where(s => !listUserName1.Contains(s.UserName) && !s.UserName.Equals(userName))
           .Select(s => new InforFriendDto { UserName = s.UserName, Avatar = s.Avatar, Name = s.Name }).Take(4).ToList();
            return listUser;
        }
        [HttpPost]
        public ActionResult GetMessage(UserDto userDto)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Contact contact = db.Contacts
            .FirstOrDefault(s => (s.FromUser.UserName.Equals(userDto.MyUserName) && s.ToUser.UserName.Equals(userDto.PartnerUserName)) ||
              (s.FromUser.UserName.Equals(userDto.PartnerUserName) && s.ToUser.UserName.Equals(userDto.MyUserName)));
            int sumMess = db.Messages.Where(s => s.ContactId == contact.Id).Count();
            var listMess = sumMess - userDto.QuantityMessage * 10 - userDto.QuantityMessageNew > 10 ?
                 db.Messages.Where(s => s.ContactId == contact.Id)
                 .Select(a => new { UserName = a.User.UserName, MessageSend = a.MessageSend, TimeSend = a.TimeSend, Name = a.User.Name }).ToList()
                 .Skip(sumMess - userDto.QuantityMessage * 10 - userDto.QuantityMessageNew - 10).Take(10)
                 :
                 db.Messages.Where(s => s.ContactId == contact.Id)
                 .Select(a => new { UserName = a.User.UserName, MessageSend = a.MessageSend, TimeSend = a.TimeSend, Name = a.User.Name }).ToList()
                 .Take(sumMess - userDto.QuantityMessage * 10 - userDto.QuantityMessageNew);
            return Json(listMess, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Edit()
        {
            var userName = Session["userName"] as string;
            var user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            var personDto = new PersonalDto { UserName = user.UserName, Name = user.Name };

            return Json(personDto, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadAvatar(HttpPostedFileBase UploadImage)

        {
            var userName = Session["userName"] as string;
            var user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            string fileName = Path.GetFileNameWithoutExtension(UploadImage.FileName);
            string extension = Path.GetExtension(UploadImage.FileName);
           // fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
            fileName = fileName + extension;
            UploadImage.SaveAs(Path.Combine(Server.MapPath("~/AppFile/Images"), fileName));
            user.Avatar = "http://localhost:54576/AppFile/Images/" + fileName;
            db.SaveChanges();
            var userDto = new PersonalDto { Avatar = user.Avatar };
            return Json(userDto, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Subject(int id)
        {
            Subject sub = db.Subjects.FirstOrDefault(s => s.Id == id);
            if (sub == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}