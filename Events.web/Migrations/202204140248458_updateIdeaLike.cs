namespace Events.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateIdeaLike : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ideas", "LikeCount", c => c.Int(nullable: false));
            AddColumn("dbo.Ideas", "DislikeCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ideas", "DislikeCount");
            DropColumn("dbo.Ideas", "LikeCount");
        }
    }
}
