using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Dto
{
    public class InforFriendDto 
    {
        public int? IdUser { get; set; }
        public string UrlProfile { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
		public string CoverPhoto { get; set; }
        public bool SeenMessage { get; set; }
	}
}