namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDB3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        LikeNumber = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreatedDate = c.String(),
                        LikeNumber = c.Int(nullable: false),
                        UserId = c.Int(),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Photo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        PassWord = c.String(),
                        Name = c.String(),
                        Avatar = c.String(),
                        CoverPhoto = c.String(),
                        SchoolName = c.String(),
                        DoB = c.DateTime(nullable: false),
                        Email = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ListFriends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MemberOfListFriends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccessRequest = c.Boolean(nullable: false),
                        SeenMessage = c.Boolean(nullable: false),
                        TimeLastChat = c.DateTime(nullable: false),
                        ListFriendId = c.Int(nullable: false),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ListFriends", t => t.ListFriendId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.ListFriendId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        PostId = c.Int(nullable: false),
                        TextNoti = c.String(),
                        ClassIconName = c.String(),
                        NotificationState = c.Boolean(nullable: false),
                        NameOfUser = c.String(),
                        Avatar = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.SubComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        LikeNumber = c.Int(nullable: false),
                        UserId = c.Int(),
                        CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromUserId = c.Int(),
                        ToUserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FromUserId)
                .ForeignKey("dbo.Users", t => t.ToUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContactId = c.Int(),
                        UserId = c.Int(),
                        MessageSend = c.String(),
                        TimeSend = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ContactId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .Index(t => t.ContactId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "ToUserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Messages", "UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "ContactId", "dbo.Users");
            DropForeignKey("dbo.Contacts", "FromUserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.SubComments", "UserId", "dbo.Users");
            DropForeignKey("dbo.SubComments", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Posts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "PostId", "dbo.Posts");
            DropForeignKey("dbo.ListFriends", "UserId", "dbo.Users");
            DropForeignKey("dbo.MemberOfListFriends", "UserId", "dbo.Users");
            DropForeignKey("dbo.MemberOfListFriends", "ListFriendId", "dbo.ListFriends");
            DropForeignKey("dbo.Posts", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "ContactId" });
            DropIndex("dbo.Contacts", new[] { "ToUserId" });
            DropIndex("dbo.Contacts", new[] { "FromUserId" });
            DropIndex("dbo.SubComments", new[] { "CommentId" });
            DropIndex("dbo.SubComments", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "PostId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.MemberOfListFriends", new[] { "UserId" });
            DropIndex("dbo.MemberOfListFriends", new[] { "ListFriendId" });
            DropIndex("dbo.ListFriends", new[] { "UserId" });
            DropIndex("dbo.Posts", new[] { "SubjectId" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropTable("dbo.Messages");
            DropTable("dbo.Contacts");
            DropTable("dbo.SubComments");
            DropTable("dbo.Notifications");
            DropTable("dbo.MemberOfListFriends");
            DropTable("dbo.ListFriends");
            DropTable("dbo.Users");
            DropTable("dbo.Subjects");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
            DropTable("dbo.Admins");
        }
    }
}
