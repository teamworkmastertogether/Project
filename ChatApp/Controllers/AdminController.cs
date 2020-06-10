using ChatApp.Extensions;
using ChatApp.Models.Dto;
using ChatApp.Models.Entities;
using ChatApp.Models.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    public class AdminController : Controller
    {
		ChatDbcontext db = new ChatDbcontext();
		// GET: Admin
        /// <summary>
        /// Hàm lấy ra dữ liệu thống kê ở màn hình tổng quan
        /// Created By: NBDuong 04.05.2020
        /// </summary>
        /// <returns></returns>
		public ActionResult Index()
        {
			var userName = Session["username"] as string;
			var user = db.Admins.FirstOrDefault(ad => ad.Username.Equals(userName));
			var listUsers = db.Users.ToList();
			var listUserVMs = AutoMapper.Mapper.Map<IEnumerable<UserViewModel>>(listUsers);
			ViewBag.CountUsers = listUserVMs.Count();

			var listPosts = db.Posts.ToList();
			var listPostVMs = AutoMapper.Mapper.Map<IEnumerable<PostViewModel>>(listPosts);
			ViewBag.CountPosts = listPostVMs.Count();

			var listSubjects = db.Subjects.ToList();
			var listSubjectVMs = AutoMapper.Mapper.Map<IEnumerable<SubjectViewModel>>(listSubjects);
			ViewBag.CountSubjects = listSubjectVMs.Count();

			var listUserDisabled = db.Users.Where(x => x.IsActive == false).ToList();
			var listUserDisabledVMs = AutoMapper.Mapper.Map<IEnumerable<UserViewModel>>(listUserDisabled);
			ViewBag.CountUsersDisabled = listUserDisabledVMs.Count();
			return View();
        }

        /// <summary>
        /// Hàm xử lý redirect vào hệ thống admin sau khi đăng nhập thành công
        /// Created By: NBDuong 04.05.2020
        /// </summary>
        /// <returns></returns>
		[HttpGet]
		public ActionResult Login()
		{
			var admin = Session["Admin"];
			if(admin != null)
			{
				return RedirectToAction("Index");
			}
			return View();
		}

        /// <summary>
        /// Hàm xử lý nghiệp vụ đăng nhập vào hệ thống admin
        /// Created By: NBDuong 04.05.2020
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
		[HttpPost]
		public ActionResult Login(FormCollection form)
		{
			string username = form["username"].ToString().Trim();
			string password = form["password"].ToString().Trim();
			string hashedPassword = HashPassword.ComputeSha256Hash(password);
			Admin admin = db.Admins.FirstOrDefault(x => x.Username.Equals("admin") && x.Password.Equals("91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203"));

			if(username == admin.Username && hashedPassword == admin.Password)
			{
				Session["username"] = admin.Username;
				return RedirectToAction("Index");
			}
			// Kiểm tra xem user có tồn tại không
			ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
			return View();
		}

        /// <summary>
        /// Đăng xuất tài khoản người dùng
        /// Created By: NBDuong 08.05.2020
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
		{
			// Hủy session lưu user và tên
			Session.Remove("username");
			// Chuyển hướng đến trang login
			return RedirectToAction("Login");
		}

        /// <summary>
        /// Lấy ra danh sách học sinh trong hệ thống
        /// Created By: NBDuong 13.05.2020
        /// </summary>
        /// <returns></returns>
		[HttpGet]
		public ActionResult GetMembers()
		{
			var listMembers = db.Users.ToList();
			var listMemberVMs = AutoMapper.Mapper.Map<IEnumerable<UserViewModel>>(listMembers);
            ViewBag.Count = Session["Count"];
            if(ViewBag.Count != null)
            {
                Session.Remove("Count");
                if (ViewBag.Count == -1)
                {
                    ViewBag.FailMessage = Session["FailMessage"];
                    Session.Remove("FailMessage");
                    ViewBag.Count = "";
                }
            }
          
			return View(listMemberVMs);
		}

        /// <summary>
        /// Lấy thông tin học sinh
        /// Created By: NBDuong 14.05.2020
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
		public ActionResult GetMemberDetail(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var member = db.Users.FirstOrDefault(x => x.Id == id);
			if (member == null)
			{
				return HttpNotFound();
			}
			var memberVM = AutoMapper.Mapper.Map<UserViewModel>(member);
			return View(memberVM);
		}

        /// <summary>
        /// Lấy ra danh sách bạn bè theo từng đối tượng
        /// Created By: NBDuong 19.05.2020
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
		public ActionResult GetListFriend(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var member = db.Users.Find(id);
			if(member == null)
			{
				return HttpNotFound();
			}
			List<FriendInfoViewModel> listFriends = db.Users.FirstOrDefault(x => x.Id == id).ListFriends.
				First().MemberOfListFriends.Where(x => x.AccessRequest).OrderByDescending(s => s.TimeLastChat)
				.Select(s => new FriendInfoViewModel { Id = s.User.Id, UserName = s.User.UserName, Name = s.User.Name, Email = s.User.Email, Avatar = s.User.Avatar })
				.ToList();
			ViewBag.ListFriend = listFriends.Count();
			ViewBag.Name = member.Name;
			return View(listFriends);
		}

        /// <summary>
        /// Lấy ra danh sách bài đăng theo từng đối tượng
        /// Created By: NBDuong 19.05.2020
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
		public ActionResult GetPostsByUser(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = db.Users.Find(id);
			if(user == null)
			{
				return HttpNotFound();
			}
			var listPosts = db.Posts.Where(x => x.User.Id == id).ToList();
			var listPostVms = AutoMapper.Mapper.Map<IEnumerable<PostViewModel>>(listPosts);
			ViewBag.Count = listPostVms.Count();
			ViewBag.Name = user.Name;
			return View(listPostVms);
		}

        /// <summary>
        /// Hàm xử lý giao diện nhập dữ liệu từ file excel
        /// Created By: NBDuong 20.05.2020
        /// </summary>
        /// <param name="count"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        //import data from excel
        [HttpPost]
		public ActionResult ImportUserFromExcel(HttpPostedFileBase fileUpload)
		{
            int count = 0;
            try
            {
                var package = new ExcelPackage(fileUpload.InputStream);
                if (!ImportData(out count, package))
                {
                    Session["FailMessage"] = "File Excel sai định dạng";
                    count = -1;
                }
            }
            catch (Exception)
            {
                Session["FailMessage"] = "Import thất bại. Vui lòng chọn file Excel";
                count = -1;
            }

            Session["Count"] = count;
			return RedirectToAction("GetMembers");
		}

        /// <summary>
        /// Hàm xử lý nghiệp vụ nhập dữ liệu từ file excel
        /// Created By: NBDuong 20.05.2020
        /// </summary>
        /// <param name="count"></param>
        /// <param name="package"></param>
        /// <returns></returns>
		//get data to above function
		public bool ImportData(out int count, ExcelPackage package)
		{
			count = 0;
            var check = true;
			var result = false;
			try
			{
				//data start at column 1 and row 2
				int startColumn = 1;
				int startRow = 2;
				ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
				object data = null;
				//get DB
				ChatDbcontext db = new ChatDbcontext();

				do
				{
					data = worksheet.Cells[startRow, startColumn].Value;
					//get Username
					object Name = worksheet.Cells[startRow, startColumn + 1].Value;
					//get email
					object Email = worksheet.Cells[startRow, startColumn + 2].Value;

					if (data != null)
					{
						var isImported = SaveStudent(Name.ToString().Trim()
							, Email.ToString().Trim(), db);
						if (isImported)
						{
							count++;
							result = true;
						}
					}
					startRow++;
				} while (data != null);
			}
			catch (Exception)
			{
                check = false;
			}
			return result;
		}

        /// <summary>
        /// Thêm 1 tài khoản học sinh vào hệ thống
        /// Created By: NBDuong 13.05.2020
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="email"></param>
        /// <param name="db"></param>
        /// <returns></returns>
		//check ability to save new student from excel
		public bool SaveStudent(string fullname, string email, ChatDbcontext db)
		{
			var result = false;
			Random rd = new Random();
			try
			{
				//save student
				//if students exist before, then not import again
				//just import new student not exists in system
				if (db.Users.Where(x => x.Email.Equals(email)).Count() == 0)
				{
					var user = new User();
					user.Name = fullname.Trim();
					user.Email = email.Trim();
					user.PassWord = HashPassword.ComputeSha256Hash(1.ToString().Trim());
					user.DoB = DateTime.Now;
					string[] emailSplitString = email.Split('@');
					user.UserName = emailSplitString[0];
					db.Users.Add(user);
					db.SaveChanges();

					int userId = db.Users.Max(x => x.Id);
					ListFriend friend = new ListFriend()
					{
						UserId = userId
					};

					db.ListFriends.Add(friend);
					db.SaveChanges();
					result = true;
				}
			}
			catch (Exception)
			{
				throw;
			}
			return result;
		}

        /// <summary>
        /// Hàm xử lý giao diện khi xuất dữ liệu ra file excel thành công
        /// Created By: NBDuong 20.05.2020
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
		[HttpPost]
		public ActionResult ExportUserToExcel(HttpPostedFileBase fileUpload)
		{
			var result = ExportData();
			if(result)
			{
				ViewBag.Message = "Bạn đã export dữ liệu thành công";
			}else
			{
				ViewBag.Message = "Bạn đã export dữ liệu thất bại. Xin vui lòng thử lại";
			}
			return RedirectToAction("GetMembers");
		}

        /// <summary>
        /// Hàm xử lý nghiệp vụ xuất dữ liệu ra file excel
        /// Created By: NBDuong 20.05.2020
        /// </summary>
        /// <returns></returns>
		private bool ExportData()
		{
			bool result = false;

            var listUsers = db.Users.ToList();
            var userList = AutoMapper.Mapper.Map <IEnumerable<UserViewModel>>(listUsers);
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("New Sheet");
            ws.Cells[1, 1].Value = "STT";
            ws.Cells[1, 2].Value = "Tên học sinh";
            ws.Cells[1, 3].Value = "Email";
            int startRow = 2;
            int i = 0;

            foreach (var item in userList)
            {
                ws.Cells[startRow, 1].Value = i + 1;
                ws.Cells[startRow, 2].Value = item.Name;
                ws.Cells[startRow, 3].Value = item.Email;
                startRow++;
                i++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("content-disposition", "attachment; filename=" + "UserReport.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();
            result = true;
			return result;
		}

        /// <summary>
        /// Hàm khởi tạo người dùng đăng nhập vào hệ thống
        /// Created By: NBDuong 11.05.2020
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
		[HttpPost]
		public JsonResult CreateUser (UserViewModel userVM)
		{
			User user = AutoMapper.Mapper.Map<User>(userVM);
			user.DoB = DateTime.Today;
			user.Avatar = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_01.jpg";
			Random random = new Random();
			var newpass = random.Next(1000, 9000).ToString().Trim();

			string emailAddress = user.Email;
			string[] emailSplitString = emailAddress.Split('@');
			user.UserName = emailSplitString[0].Trim();
			user.PassWord = HashPassword.ComputeSha256Hash(newpass);
            user.IsActive = true;
			db.Users.Add(user);
			db.SaveChanges();

			string content = "<h1>Thông tin tài khoản là : </h1></br> ";
			content += "<h1> Username:  " + user.UserName + "</h1></br> ";
			content += "<h1> Mật khẩu: " + newpass + "</h1></br> ";
			SendEmail("Thông tin tài khoản", emailAddress, "teamworkmastertogether@gmail.com",
				"teamworkmastertogether@123", content);

			int userId = db.Users.Max(x => x.Id);
			ListFriend friend = new ListFriend()
			{
				UserId = userId
			};
			db.ListFriends.Add(friend);
			db.SaveChanges();
			return Json(userVM, JsonRequestBehavior.AllowGet);
		}

        /// <summary>
        /// Hàm kiểm tra check mail tồn tại
        /// Created By: NBDuong 22.05.2020
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
		public JsonResult CheckEmail(UserViewModel userVM)
		{
			if(db.Users.Any(s => s.Email.Equals(userVM.Email)))
			{
				return Json(1, JsonRequestBehavior.AllowGet);
			}else
			{
				return Json(0, JsonRequestBehavior.AllowGet);
			}
		}

        /// <summary>
        /// Hàm xử lý gửi email lấy lại password cho tài khoản
        /// Created By: NBDuong 01.06.2020
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="ToEmail"></param>
        /// <param name="FromEmail"></param>
        /// <param name="PassWord"></param>
        /// <param name="Content"></param>
		public void SendEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
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
			smtp.Credentials = new NetworkCredential(FromEmail, PassWord);//Tài khoản password người gửi
			smtp.EnableSsl = true; //kích hoạt giao tiếp an toàn SSL
			smtp.Send(mail); //Gửi mail đi
		}

        /// <summary>
        /// Khóa tài khoản người dùng đăng nhập
        /// Created By: NBDuong 24.05.2020
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpPost]
		public JsonResult LockUser (int? id)
		{
			User user = db.Users.FirstOrDefault(x => x.Id == id);

			if (user.IsActive == true)
			{
				user.IsActive = false;
				db.SaveChanges();
				return Json(1, JsonRequestBehavior.AllowGet);
			}else
			{
				user.IsActive = true;
				db.SaveChanges();
				return Json(0, JsonRequestBehavior.AllowGet);
			}
		}

        /// <summary>
        /// Hàm xử lý lấy ra danh sách nhóm môn học
        /// Created By: NBDuong 14.05.2020
        /// </summary>
        /// <returns></returns>
		public ActionResult GetSubjects()
		{
			var listSubjects = db.Subjects.ToList();
			var listSubjectVMs = AutoMapper.Mapper.Map<IEnumerable<SubjectViewModel>>(listSubjects);
			return View(listSubjectVMs);
		}

        /// <summary>
        /// Hàm xử lý lấy ra danh sách bài đăng theo từng nhóm môn học
        /// Created By: NBDuong 16.05.2020
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public ActionResult GetPostsBySubjects(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Subject sub = db.Subjects.FirstOrDefault(x => x.Id == id);
			if(sub == null)
			{
				return HttpNotFound();
			}
			var listPosts = db.Posts.Where(x => x.SubjectId == sub.Id).ToList();
			var listPostVMs = AutoMapper.Mapper.Map<IEnumerable<PostViewModel>>(listPosts);
			
			ViewBag.Count = listPostVMs.Count();	
			ViewBag.Name = sub.Name;
			return View(listPostVMs);
		}

        /// <summary>
        /// Xóa post vi phạm
        /// Created By: NBDuong 23.05.2020
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public ActionResult DeletePost(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Post post = db.Posts.FirstOrDefault(x => x.Id == id);
			if(post == null)
			{
				return HttpNotFound();
			}
			List<Like> listLikes = db.Likes.Where(s => s.PostId == id || id == s.Comment.PostId || id == s.SubComment.Comment.PostId).ToList();
			db.Likes.RemoveRange(listLikes);
			db.Posts.Remove(post);
			db.SaveChanges();
			return RedirectToAction("GetSubjects");
		}

        /// <summary>
        /// Cập nhật ảnh bìa cho nhóm môn học
        /// Created By: NBDuong 26.05.2020
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UploadCover"></param>
        /// <returns></returns>
        public ActionResult UploadCover (int id, HttpPostedFileBase UploadCover)
        {
            Subject sub = db.Subjects.FirstOrDefault(x => x.Id == id);
            if(UploadCover != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(UploadCover.FileName);
                string Extension = Path.GetExtension(UploadCover.FileName);
                FileName = FileName + Extension;
                UploadCover.SaveAs(Path.Combine(Server.MapPath("~/Assets/AdminCoverUpload"), FileName));
                sub.Photo = "http://localhost:54576/Assets/AdminCoverUpload/" + FileName;
                db.SaveChanges();
                var subjectDto = new SubjectDto { Name = sub.Name, Photo = sub.Photo };
                return Json(subjectDto, JsonRequestBehavior.AllowGet);
            }
            return Json("Vui lòng chọn ảnh thích hợp !", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy ra danh sách thành viên bị khóa
        /// Created By: NBDuong 27.05.2020
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMembersDisabled()
        {
            var listMembers = db.Users.Where(x => x.IsActive == false).ToList();
            var listMemberVMs = AutoMapper.Mapper.Map<IEnumerable<UserViewModel>>(listMembers);
            return View(listMemberVMs);
        }
    }
}