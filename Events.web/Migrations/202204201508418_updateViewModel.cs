namespace Events.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateViewModel : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Views");
            AddColumn("dbo.Views", "viewId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Views", "UserId", c => c.String());
            AddPrimaryKey("dbo.Views", "viewId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Views");
            AlterColumn("dbo.Views", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Views", "viewId");
            AddPrimaryKey("dbo.Views", "UserId");
        }
    }
}
