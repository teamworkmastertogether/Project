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
        public string PicUrl { get; set; }
    }
}