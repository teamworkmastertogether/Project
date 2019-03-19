using System.Data.Entity;

namespace ChatApp.Models.Entities
{
    public class ChatDbcontext : DbContext
    {
        public ChatDbcontext() : base("ChatDbConnectionstring")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MemberOfListFriend> MemberOfListFriends { get; set; }
        public DbSet<ListFriend> ListFriends { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
		public DbSet<Admin> Admins { get; set; }
    }
}