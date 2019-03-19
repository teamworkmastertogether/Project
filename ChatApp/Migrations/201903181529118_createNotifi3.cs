namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createNotifi3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "NameOfUser", c => c.String());
            AddColumn("dbo.Notifications", "Avatar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Avatar");
            DropColumn("dbo.Notifications", "NameOfUser");
        }
    }
}
