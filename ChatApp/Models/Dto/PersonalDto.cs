using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Dto
{
    public class PersonalDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
        public string Avatar { get; set; }
        public string CoverPhoto { get; set; }
        public string SchoolName { get; set; }
        public DateTime DoB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string NewPassword { get; set; }
        public string MyPhotos { get; set; }
    }
}