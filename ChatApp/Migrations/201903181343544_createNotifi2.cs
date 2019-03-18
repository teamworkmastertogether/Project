namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createNotifi2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "ClassIconName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "ClassIconName");
        }
    }
}
