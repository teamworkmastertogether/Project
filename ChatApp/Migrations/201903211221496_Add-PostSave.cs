namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostSave : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostSaves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostSaves", "UserId", "dbo.Users");
            DropForeignKey("dbo.PostSaves", "PostId", "dbo.Posts");
            DropIndex("dbo.PostSaves", new[] { "PostId" });
            DropIndex("dbo.PostSaves", new[] { "UserId" });
            DropTable("dbo.PostSaves");
        }
    }
}
