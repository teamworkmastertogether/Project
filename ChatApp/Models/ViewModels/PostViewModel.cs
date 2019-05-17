using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models.ViewModels
{
	public class PostViewModel
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public string Photo { get; set; }
		public string CreatedDate { get; set; }

        public SubjectViewModel Subject { get; set; }
		public int SubjectId { get; set; }

        public UserViewModel User { get; set; }
        public int UserId { get; set; }
	}
}