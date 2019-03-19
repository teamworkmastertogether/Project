using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Dto
{
    public class SubCommentDto
    {
        public string UserName { get; set; }
        public string NameOfUser { get; set; }
        public string Text { get; set; }
        public int LikeNumber { get; set; }
        public string Avatar { get; set; }
        public int CommentId { get; set; }
        public int SubCommentId { get; set; }
    }
}