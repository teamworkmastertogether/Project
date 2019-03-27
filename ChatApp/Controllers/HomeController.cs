using ChatApp.Models.Entities;
using ChatApp.Models.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatApp.Extensions;
using System.Net.Mail;

namespace ChatApp.Controllers
{
    public class HomeController : Controller
    {
        ChatDbcontext db = new ChatDbcontext();

        public ActionResult Personal(int? id = 0)
        {
            string userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            bool checkUser = false;
            if (id == user.Id || id == 0)
            {
                checkUser = true;
                ViewBag.checkFriend = true;
            }
            else
            {
                 user = db.Users.FirstOrDefault(us => us.Id == id);
                 ViewBag.checkFriend = db.ListFriends.First(s => s.UserId == id).MemberOfListFriends
                                         .Any(s => s.UserId == user.Id);
            }
            ViewBag.checkUser = checkUser;
            ViewBag.Img = user.Avatar;
            ViewBag.Bg = user.CoverPhoto;
            ViewBag.Name = user.Name;
            ViewBag.Id = user.Id;
            return View();
        }

        public JsonResult GetListPostSave(int? id)
        {
            List<PostSaveDto> listPost = db.Users.FirstOrDefault(us => us.Id == id).PostSaves
                .Select(s => new PostSaveDto
                {
                    Photo = s.Post.Photo,
                    IdPostSave = s.Id,
                    NameUser = s.Post.User.Name,
                    UrlPost = "/Subject/GetSubject?id=" + s.Post.SubjectId.ToString() + "#" + s.PostId.ToString(),
                    Avatar = s.Post.User.Avatar,
                    SubjectName = s.Post.Subject.Name,
                    TimePost = s.Post.CreatedDate,
                    TextContent = s.Post.Text
                }).ToList();
            return Json(listPost, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Login()
        {
            // Lấy user từ session
            var userName = Session["userName"];

            if (userName != null)
            {
                return RedirectToAction("Personal");
            }
            return View();
        }

        // Submit đăng nhập từ form
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            // Lấy username từ form
            string username = form["username"].ToString().Trim();
			string password = form["password"].ToString().Trim();
			string hashedPassword = HashPassword.ComputeSha256Hash(password);
            // Lấy user có username và password trùng với form submit
            User user = db.Users.FirstOrDefault(x => x.UserName.Trim().Equals(username) && x.PassWord.Trim().Equals(hashedPassword));
            // Kiểm tra xem user có tồn tại không
            if (user != null)
            {
                Session["userName"] = user.UserName;
                return RedirectToAction("Personal");
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
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            ViewBag.UrlPersonal = "/Home/Personal?id=" + db.Users.FirstOrDefault(s => s.UserName.Equals(userName)).Id.ToString();
            ViewBag.SumNoti = db.Notifications.Where(s => s.User.UserName.Equals(userName) && !s.NotificationState).ToList().Count();
            ViewBag.SumUserSendRequest = user.ListFriends.First().MemberOfListFriends.Where(s => s.AccessRequest == false).ToList().Count();
            return PartialView();
        }

        public JsonResult GetListFriend(int? id)
        {
            List<InforFriendDto> listUser;
            if (id == 0)
            {
                var userName = Session["userName"] as string;
                 listUser = db.Users.FirstOrDefault(s => s.UserName.Equals(userName))
                .ListFriends.First().MemberOfListFriends.Where(s => s.AccessRequest).OrderByDescending(s => s.TimeLastChat)
                .Select(s => new InforFriendDto
                {
                    IdUser = s.UserId,
                    UrlPersonal = "/Home/Personal?id=" + s.UserId,
                    Avatar = s.User.Avatar,
                    Name = s.User.Name,
                    UserName = s.User.UserName,
                    SeenMessage = s.SeenMessage
                })
                .Take(20).ToList();
            }
            else
            {
                 listUser = db.Users.FirstOrDefault(s => s.Id == id)
                .ListFriends.First().MemberOfListFriends.Where(s => s.AccessRequest).OrderByDescending(s => s.TimeLastChat)
                .Select(s => new InforFriendDto
                {
                    IdUser = s.UserId,
                    UrlPersonal = "/Home/Personal?id=" + s.UserId,
                    Avatar = s.User.Avatar,
                    Name = s.User.Name,
                    UserName = s.User.UserName,
                    SeenMessage = s.SeenMessage
                })
                .Take(20).ToList();
            }
            return Json(listUser, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveFriend(int? id)
        {
            string userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            MemberOfListFriend mem = user
            .ListFriends.First().MemberOfListFriends.FirstOrDefault(s => s.UserId == id);
            MemberOfListFriend mem2 = db.Users.FirstOrDefault(us => us.Id == id)
            .ListFriends.First().MemberOfListFriends.FirstOrDefault(s => s.UserId == user.Id);
            Contact contact = db.Contacts.FirstOrDefault(s => (s.FromUserId == id && s.ToUserId == user.Id) ||
            (s.FromUserId == user.Id && s.ToUserId == id));

            db.Contacts.Remove(contact);
            db.MemberOfListFriends.Remove(mem);
            db.MemberOfListFriends.Remove(mem2);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AcceptRequest(int? id)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(s => s.UserName.Equals(userName));
            MemberOfListFriend mem = user.ListFriends.First().MemberOfListFriends.FirstOrDefault(s => s.UserId == id);
            mem.AccessRequest = true;
            ListFriend list = db.ListFriends.FirstOrDefault(s => s.UserId == id);
            MemberOfListFriend mem2 = new MemberOfListFriend
            {
                UserId = user.Id,
                AccessRequest = true,
                ListFriendId = list.Id,
                TimeLastChat = DateTime.Now,
                SeenMessage = true
            };
            Contact contact = new Contact
            {
                FromUserId = user.Id,
                ToUserId = id
            };
            db.Contacts.Add(contact);
            db.MemberOfListFriends.Add(mem2);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFriendSuggest()
        {
            var userName = Session["userName"] as string;
            List<string> listUserName1 = db.Users.FirstOrDefault(s => s.UserName.Equals(userName))
            .ListFriends.First().MemberOfListFriends.Select(s => s.User.UserName).ToList();
            Random rnd = new Random();
            int from = rnd.Next(1, 30);
            List<InforFriendDto> listUser = db.Users.Where(s => !listUserName1.Contains(s.UserName) && !s.UserName.Equals(userName))
           .Select(s => new InforFriendDto
           {
               IdUser = s.Id,
               UserName = s.UserName,
               Avatar = s.Avatar,
               Name = s.Name,
               UrlPersonal = "/Home/Personal?id=" + s.Id
           }).OrderBy(s => s.Name).Skip(from).Take(4).ToList();
           return Json(listUser, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendRequestAddFriend(int? id)
        {
            var userName = Session["userName"] as string;
            var user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            
            ListFriend listFriend = db.ListFriends.FirstOrDefault(s => s.UserId == id);
            bool check = listFriend.MemberOfListFriends.Any(s => s.UserId == user.Id);
            if (!check)
            {
                MemberOfListFriend mem = new MemberOfListFriend
                {
                    AccessRequest = false,
                    SeenMessage = true,
                    TimeLastChat = DateTime.Now,
                    ListFriendId = listFriend.Id,
                    UserId = user.Id
                };
                db.MemberOfListFriends.Add(mem);
                db.SaveChanges();
                return Json(listFriend.User.UserName, JsonRequestBehavior.AllowGet);
            }
            return Json("No value", JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult Edit(int? id)
        {
            var user = db.Users.FirstOrDefault(us => us.Id == id);
            var personDto = new PersonalDto { Name = user.Name,SchoolName=user.SchoolName,DoB=user.DoB,Address=user.Address,PhoneNumber=user.PhoneNumber };
            return Json(personDto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ConfirmPassword(int? id, PersonalDto personalDto)
        {
            var user = db.Users.FirstOrDefault(us => us.Id == id);
            personalDto.PassWord = HashPassword.ComputeSha256Hash(personalDto.PassWord);
            
            if(string.Compare(personalDto.PassWord,user.PassWord,true)==0)
            {
                return Json(new { isvalid=true}, JsonRequestBehavior.AllowGet);
            }
            return Json(new { isvalid = false }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadAvatar(int id,HttpPostedFileBase UploadImage)

        {
            if(UploadImage!=null)
            {
                var userName = Session["userName"] as string;
                var user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
                string fileName = Path.GetFileNameWithoutExtension(UploadImage.FileName);
                string extension = Path.GetExtension(UploadImage.FileName);
                fileName = fileName + extension;
                UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/ImagesUpload"), fileName));

                if (id == 1)
                {
                    user.Avatar = "http://localhost:54576/Assets/ImagesUpload/" + fileName;
                }
                else if(id==2)
                {
                    user.CoverPhoto = "http://localhost:54576/Assets/ImagesUpload/" + fileName;
                }
                
                db.SaveChanges();
                var userDto = new PersonalDto { Avatar = user.Avatar, CoverPhoto = user.CoverPhoto };
                return Json(userDto, JsonRequestBehavior.AllowGet);
            }

            return Json("Vui lòng chọn ảnh thích hợp !", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveData(int? id, PersonalDto personalDto)
        {
            var user = db.Users.FirstOrDefault(us => us.Id == id);
            user.Name = personalDto.Name;
            user.SchoolName = personalDto.SchoolName;
            user.DoB = personalDto.DoB;
            user.Address = personalDto.Address;
            user.PhoneNumber = personalDto.PhoneNumber;
            db.SaveChanges();
            
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SavePassword(int? id, PersonalDto personalDto)
        {
            var user = db.Users.FirstOrDefault(us => us.Id == id);
            user.PassWord = HashPassword.ComputeSha256Hash(personalDto.NewPassword);
            db.SaveChanges();
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
		public JsonResult GetPassWord(FormCollection form)
		{
			// Lấy username và email từ form submit
			string username = form["Username"].ToString().Trim();
			string email = form["Email"].ToString().Trim();

			// Lấy ra username có tên và email thỏa mãn
			User user = db.Users.FirstOrDefault(s => s.UserName.Equals(username) && s.Email.Equals(email));
			if (user != null)
			{
				Random random = new Random();
				int length = 6;
				var str = "";
				for(var i = 0; i < length; i++)
				{
					str += ((char)(random.Next(1, 26) + 64)).ToString();
				}

				string emailAddress = user.Email;
				user.PassWord = HashPassword.ComputeSha256Hash(str);
				db.SaveChanges();
				string content = "<h1>Thông tin tài khoản là : </h1></br> ";
				content += "<h1> Tên đăng nhập:  " + username + "</h1></br> ";
				content += "<h1> Mật khẩu: " + str + "</h1></br> ";
				GuiEmail("Thông tin tài khoản", emailAddress, "teamworkmastertogether@gmail.com",
					"teamworkmastertogether@123", content);
				return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
		}

        public JsonResult DeletePostSaved(int? id)
        {
            PostSave postSave = db.PostSaves.FirstOrDefault(s => s.Id == id);
            db.PostSaves.Remove(postSave);
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }


        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
		{
			// goi email
			MailMessage mail = new MailMessage();
			mail.To.Add(ToEmail); // Địa chỉ nhận
			mail.From = new MailAddress(ToEmail); // Địa chửi gửi
			mail.Subject = Title; // tiêu đề gửi
			mail.Body = Content; // Nội dung
			mail.IsBodyHtml = true;
			SmtpClient smtp = new SmtpClient();
			smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
			smtp.Port = 587; //port của Gmail
			smtp.UseDefaultCredentials = false;
			smtp.Credentials = new System.Net.NetworkCredential
			(FromEmail, PassWord);//Tài khoản password người gửi
			smtp.EnableSsl = true; //kích hoạt giao tiếp an toàn SSL
			smtp.Send(mail); //Gửi mail đi
		}

        //public JsonResult SearchFriend(string keyword)
        //{
        //    var data = db.Users.Where(m => m.UserName.Contains(keyword)).Select(m => m.Name).ToList();
        //    return Json(new { data = data, status = true }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult DisplaySeach(string keyword)
        {
            List<PersonalDto> data = db.Users.Where(m => m.Name.Contains(keyword)).Select(s => new PersonalDto { Name = s.Name,Avatar=s.Avatar, UrlUser = "/Home/Personal?id=" + s.Id }).ToList();
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}