using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Dto
{
    public class PostSaveDto
    {
        public int IdPostSave { get; set; }
        public string NameUser { get; set; }
        public string UrlPost { get; set; }
        public string Avatar { get; set; }
        public string SubjectName { get; set; }
        public string TimePost { get; set; }
        public string TextContent { get; set; }
        public string UrlPhoto { get; set; }
        public string Photo { get; set; }
    }
}