namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDB : DbMigration
    {
		//tại sao không gen ra các class dto
        public override void Up()
        {
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
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        PassWord = c.String(),
                        Name = c.String(),
                        Avatar = c.String(),
                        PicUrl = c.String(),
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
            DropForeignKey("dbo.ListFriends", "UserId", "dbo.Users");
            DropForeignKey("dbo.MemberOfListFriends", "UserId", "dbo.Users");
            DropForeignKey("dbo.MemberOfListFriends", "ListFriendId", "dbo.ListFriends");
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "ContactId" });
            DropIndex("dbo.MemberOfListFriends", new[] { "UserId" });
            DropIndex("dbo.MemberOfListFriends", new[] { "ListFriendId" });
            DropIndex("dbo.ListFriends", new[] { "UserId" });
            DropIndex("dbo.Contacts", new[] { "ToUserId" });
            DropIndex("dbo.Contacts", new[] { "FromUserId" });
            DropTable("dbo.Messages");
            DropTable("dbo.MemberOfListFriends");
            DropTable("dbo.ListFriends");
            DropTable("dbo.Users");
            DropTable("dbo.Contacts");
        }
    }
}
