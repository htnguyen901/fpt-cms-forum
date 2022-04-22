namespace Events.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAppUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DepartmentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DepartmentId");
        }
    }
}
