﻿using ChatApp.Extensions;
using ChatApp.Models.Dto;
using ChatApp.Models.Entities;
using ChatApp.Models.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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
		public ActionResult Index()
        {
			var userName = Session["username"] as string;
			var user = db.Admins.FirstOrDefault(ad => ad.Username.Equals(userName));
			return View();
        }

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

		// Đăng xuất tài khoản người dùng
		public ActionResult Logout()
		{
			// Hủy session lưu user và tên
			Session.Remove("username");
			// Chuyển hướng đến trang login
			return RedirectToAction("Login");
		}

		[HttpGet]
		public ActionResult GetMembers ()
		{
			var listMembers = db.Users.ToList();
			var listMemberVMs = AutoMapper.Mapper.Map<IEnumerable<UserViewModel>>(listMembers);
			return View(listMemberVMs);
		}

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

		//import data from excel
		[HttpPost]
		public ActionResult ImportUserFromExcel(HttpPostedFileBase fileUpload)
		{
			int count = 0;
			var package = new ExcelPackage(fileUpload.InputStream);
			if (ImportData(out count, package))
			{
				ViewBag.message = "Bạn đã import dữ liệu học sinh thành công";
			}else
			{
				ViewBag.message = "Import thất bại. Vui lòng thử lại";
			}
			return RedirectToAction("GetMembers");
		}

		//get data to above function
		public bool ImportData(out int count, ExcelPackage package)
		{
			count = 0;
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
						var isImported = SaveStudent(Name.ToString()
							, Email.ToString(), db);
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
				throw;
			}
			return result;
		}

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
					user.Name = fullname;
					user.Email = email;
					user.PassWord = HashPassword.ComputeSha256Hash(rd.Next(1000,9000).ToString());
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

		private bool ExportData()
		{
			bool result = false;
			string fileName = Server.MapPath("/") + "\\ExportFiles\\UserData.xlsx";
			ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(fileName));
			//create new sheet
			var ws = package.Workbook.Worksheets.Add("User Sheet");

			int startRow = 2;

			//get data from db
			ChatDbcontext db = new ChatDbcontext();
			var users = db.Users.ToList();
			int i = 0;
			foreach (var item in users)
			{
				ws.Cells[1, 1].Value = "STT";
				ws.Cells[1, 2].Value = "Tên học sinh";
				ws.Cells[1, 3].Value = "Email";
				ws.Cells[startRow, 1].Value = i + 1;
				ws.Cells[startRow, 2].Value = item.Name;
				ws.Cells[startRow, 3].Value = item.Email;
				startRow++;
				i++;
			}
			package.Save();
			result = true;
			return result;
		}

		[HttpPost]
		public JsonResult CreateUser (UserViewModel userVM)
		{
			User user = AutoMapper.Mapper.Map<User>(userVM);
			user.DoB = DateTime.Now;
			user.Avatar = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_01.jpg";
			Random random = new Random();
			var newpass = random.Next(1000, 9000).ToString();

			string emailAddress = user.Email;
			string[] emailSplitString = emailAddress.Split('@');
			user.UserName = emailSplitString[0];
			user.PassWord = HashPassword.ComputeSha256Hash(newpass);
			db.Users.Add(user);
			db.SaveChanges();

			string content = "<h1>Thông tin tài khoản là : </h1></br> ";
			content += "<h1> Username:  " + user.UserName + "</h1></br> ";
			content += "<h1> Mật khẩu: " + newpass + "</h1></br> ";
			GuiEmail("Thông tin tài khoản", emailAddress, "teamworkmastertogether@gmail.com",
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
			smtp.Credentials = new NetworkCredential(FromEmail, PassWord);//Tài khoản password người gửi
			smtp.EnableSsl = true; //kích hoạt giao tiếp an toàn SSL
			smtp.Send(mail); //Gửi mail đi
		}
	}
}