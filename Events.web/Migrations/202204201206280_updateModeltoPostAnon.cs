namespace Events.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModeltoPostAnon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "isAnon", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ideas", "isAnon", c => c.Boolean(nullable: false));
            DropColumn("dbo.Ideas", "DislikeCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ideas", "DislikeCount", c => c.Int(nullable: false));
            DropColumn("dbo.Ideas", "isAnon");
            DropColumn("dbo.Comments", "isAnon");
        }
    }
}
