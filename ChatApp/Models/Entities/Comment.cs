using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Entities
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public int LikeNumber { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<SubComment> SubComments { get; set; }
    }
}