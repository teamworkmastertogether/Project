using System.Data.Entity;

namespace ChatApp.Models
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
    }
}