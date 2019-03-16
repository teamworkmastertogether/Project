using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ChatApp.Models.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
		public string CoverPhoto { get; set; }
		public string SchoolName { get; set; }
		public DateTime DoB { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }

        public virtual ICollection<ListFriend> ListFriends { get; set; }
        public virtual ICollection<MemberOfListFriend> MemberOfListFriends { get; set; }

        public User():base()
        {
            Avatar = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_01.jpg";
            CoverPhoto = "http://3.bp.blogspot.com/-IzYETuZ48C8/T7MQmHucwCI/AAAAAAAABEQ/jBTkIk7ObKY/s1600/hinh-nen-dep-31.jpg";
        }
    }
}