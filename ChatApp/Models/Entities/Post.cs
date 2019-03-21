using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Entities
{
    public class Post : BaseEntity
    {
        public string Text { get; set; }
        public string CreatedDate { get; set; }

        public int LikeNumber { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}