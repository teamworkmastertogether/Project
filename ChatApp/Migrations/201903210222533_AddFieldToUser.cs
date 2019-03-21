namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "MyPhotos", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "MyPhotos");
        }
    }
}
