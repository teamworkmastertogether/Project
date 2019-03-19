using ChatApp.Extensions;
using ChatApp.Models.Dto;
using ChatApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

			User user = db.Users.FirstOrDefault(x => x.UserName.Equals("admin") && x.PassWord.Equals("91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203"));
			// Kiểm tra xem user có tồn tại không
			if (user != null)
			{
				Session["username"] = user.UserName;
				return RedirectToAction("Index");
			}
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
		public ActionResult GetPosts(int? id)
		{
			Subject subject = db.Subjects.FirstOrDefault(x => x.Id == id);
			string username = Session["userName"] as string;
			User user = db.Users.FirstOrDefault(x => x.UserName.Equals(username));

			return View();
		}
	}
}