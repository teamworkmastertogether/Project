using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Entities
{
    public class ListFriend : BaseEntity
    {
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<MemberOfListFriend> MemberOfListFriends { get; set; }
    }
}