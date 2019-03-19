namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createNotifi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        PostId = c.Int(nullable: false),
                        TextNoti = c.String(),
                        NotificationState = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "PostId", "dbo.Posts");
            DropIndex("dbo.Notifications", new[] { "PostId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropTable("dbo.Notifications");
        }
    }
}
