namespace Events.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtokenPasstoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "tokenPass", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "tokenPass");
        }
    }
}
