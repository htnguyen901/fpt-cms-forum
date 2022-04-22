namespace Events.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        CategoryDescription = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                        IdeaId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false),
                        StaffId = c.Int(nullable: false),
                        Role = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        IdeaId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Content = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                        CategoryId = c.Int(nullable: false),
                        SubmissionId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IdeaId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Submissions", t => t.SubmissionId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.SubmissionId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        Ideaid = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ideas", t => t.Ideaid, cascadeDelete: true)
                .Index(t => t.Ideaid);
            
            CreateTable(
                "dbo.Reactions",
                c => new
                    {
                        Reaction1 = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                        IdeaId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Reaction1)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Ideas", t => t.IdeaId, cascadeDelete: true)
                .Index(t => t.IdeaId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Submissions",
                c => new
                    {
                        SubmissionId = c.Int(nullable: false, identity: true),
                        SubmissionName = c.String(),
                        SubmissionDescription = c.String(),
                        ClosureDate = c.DateTime(nullable: false),
                        FinalClosureDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SubmissionId);
            
            CreateTable(
                "dbo.Views",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdeaId = c.Int(nullable: false),
                        LastVisitedDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Ideas", t => t.IdeaId, cascadeDelete: true)
                .Index(t => t.IdeaId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.IdeaComments",
                c => new
                    {
                        Idea_IdeaId = c.Int(nullable: false),
                        Comment_CommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idea_IdeaId, t.Comment_CommentId })
                .ForeignKey("dbo.Ideas", t => t.Idea_IdeaId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.Comment_CommentId, cascadeDelete: true)
                .Index(t => t.Idea_IdeaId)
                .Index(t => t.Comment_CommentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Views", "IdeaId", "dbo.Ideas");
            DropForeignKey("dbo.Views", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ideas", "SubmissionId", "dbo.Submissions");
            DropForeignKey("dbo.Reactions", "IdeaId", "dbo.Ideas");
            DropForeignKey("dbo.Reactions", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Files", "Ideaid", "dbo.Ideas");
            DropForeignKey("dbo.IdeaComments", "Comment_CommentId", "dbo.Comments");
            DropForeignKey("dbo.IdeaComments", "Idea_IdeaId", "dbo.Ideas");
            DropForeignKey("dbo.Ideas", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Ideas", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.IdeaComments", new[] { "Comment_CommentId" });
            DropIndex("dbo.IdeaComments", new[] { "Idea_IdeaId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Views", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Views", new[] { "IdeaId" });
            DropIndex("dbo.Reactions", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Reactions", new[] { "IdeaId" });
            DropIndex("dbo.Files", new[] { "Ideaid" });
            DropIndex("dbo.Ideas", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Ideas", new[] { "SubmissionId" });
            DropIndex("dbo.Ideas", new[] { "CategoryId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "ApplicationUser_Id" });
            DropTable("dbo.IdeaComments");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Departments");
            DropTable("dbo.Views");
            DropTable("dbo.Submissions");
            DropTable("dbo.Reactions");
            DropTable("dbo.Files");
            DropTable("dbo.Ideas");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
        }
    }
}
