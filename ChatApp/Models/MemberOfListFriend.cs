using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
    public class MemberOfListFriend : BaseEntity
    {
        public bool AccessRequest { get; set; }
        public bool SeenMessage { get; set; }
        public DateTime TimeLastChat { get; set; }

        [ForeignKey("ListFriend")]
        public int ListFriendId { get; set; }
        public virtual ListFriend ListFriend { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public MemberOfListFriend():base()
        {
            AccessRequest = false;
            TimeLastChat = DateTime.Now;
        }
    }
}