using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models.ViewModels
{
	public class UserViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Avatar { get; set; }
		public string SchoolName { get; set; }
		public string Email { get; set; }
		public DateTime Dob { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
	}
}