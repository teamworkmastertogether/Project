using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ChatApp.Models.Entities
{
    public class User : BaseEntity
    {
        // Tên tài khoản
        public string UserName { get; set; }
        // Mật khẩu
        public string PassWord { get; set; }
        // Tên người dùng
        public string Name { get; set; }
        // Ảnh đại diện
        public string Avatar { get; set; }
        // Ảnh cover
		public string CoverPhoto { get; set; }
        // Tên trường học
		public string SchoolName { get; set; }
        // Ngày sinh
		public DateTime DoB { get; set; }
        // Email
		public string Email { get; set; }
        // Địa chỉ
		public string Address { get; set; }
        // Số điện thoại 
		public string PhoneNumber { get; set; }
        // Còn hoạt động hay không
		public bool IsActive { get; set; }

        // Danh sách bạn bè
        public virtual ICollection<ListFriend> ListFriends { get; set; }
        // Danh sách tài khoản liên quan tới bạn bè
        public virtual ICollection<MemberOfListFriend> MemberOfListFriends { get; set; }
        // Thông báo
        public virtual ICollection<Notification> Notifications { get; set; }
        // Các bài viết đã lưu
        public virtual ICollection<PostSave> PostSaves { get; set; }

        public User():base()
        {
            Avatar = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_01.jpg";
            CoverPhoto = "http://3.bp.blogspot.com/-IzYETuZ48C8/T7MQmHucwCI/AAAAAAAABEQ/jBTkIk7ObKY/s1600/hinh-nen-dep-31.jpg";
			IsActive = true;
        }
    }
}