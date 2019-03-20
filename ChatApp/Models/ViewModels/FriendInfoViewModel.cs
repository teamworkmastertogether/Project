using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models.ViewModels
{
	public class FriendInfoViewModel
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Avatar { get; set; }
	}
}