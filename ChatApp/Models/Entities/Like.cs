using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Entities
{
    public class Like : BaseEntity
    {
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }

        [ForeignKey("Comment")]
        public int? CommentId { get; set; }
        public virtual Comment Comment { get; set; }

        [ForeignKey("SubComment")]
        public int? SubCommentId { get; set; }
        public virtual SubComment SubComment { get; set; }

    }
}