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
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Message>()
        //    .HasRequired(c => c.User)
        //    .WithMany()
        //    .WillCascadeOnDelete(false);
        //}
    }
}