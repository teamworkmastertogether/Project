namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removelikenumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Photo", c => c.String());
            DropColumn("dbo.Comments", "LikeNumber");
            DropColumn("dbo.Posts", "LikeNumber");
            DropColumn("dbo.SubComments", "LikeNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubComments", "LikeNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "LikeNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "LikeNumber", c => c.Int(nullable: false));
            DropColumn("dbo.Posts", "Photo");
        }
    }
}
