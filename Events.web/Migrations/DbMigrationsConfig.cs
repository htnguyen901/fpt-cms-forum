namespace Events.web.Migrations
{
    using Events.web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class DbMigrationsConfig : DbMigrationsConfiguration<Events.web.Models.ApplicationDbContext>
    {
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }


        protected override void Seed(Events.web.Models.ApplicationDbContext context)
        {

            //Departments
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(new List<Department>()
                { 
                    new Department()
                    {
                        DepartmentId = 1,
                        DepartmentName = "Admin"
                    },

                    new Department()
                    {
                        DepartmentId = 2,
                        DepartmentName = "IT"
                    },

                    new Department()
                    {
                        DepartmentId = 3,
                        DepartmentName = "Business"
                    },

                    new Department()
                    {
                        DepartmentId = 4,
                        DepartmentName = "Law"
                    },

                    new Department()
                    {
                        DepartmentId = 5,
                        DepartmentName = "Finance"
                    },
                }
                    );
                context.SaveChanges();
            }

            //Category
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(new List<Category>()
                {
                    new Category()
                    {
                        CategoryName = "Financial Support",
                        CategoryDescription = "Add more funds to develop infrastructure"
                    },

                    new Category()
                    {
                        CategoryName = "Equipments",
                        CategoryDescription = "Better to upgrade equipments in campus"
                    },

                    new Category()
                    {
                        CategoryName = "Networking",
                        CategoryDescription = "Build connections among employees and other professionals"
                    }

                }
                    );
                context.SaveChanges();
            }

            //Admin User
            if (!context.Users.Any())
            {
                var adminEmail = "admin@gmail.com";
                var adminFullName = "Administrator";
                var adminUserName = adminEmail;
                var adminPassword = "Admin@1";
                //var adminDateTime = DateTime.Now;
                string adminRole = "Administrator";
                CreateAdminUser(context, adminEmail, adminUserName, adminFullName, adminPassword, adminRole);
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var role = new IdentityRole();
                roleManager.Create(new IdentityRole("QAManager"));
                roleManager.Create(new IdentityRole("QACoordinator"));
                roleManager.Create(new IdentityRole("Staff"));
            }

            //More App Users
            if (!context.Users.Any(u => u.Email == "staff@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var staff = new ApplicationUser
                {
                    FullName = "Ha Nguyen Staff",
                    UserName = "staff@gmail.com",
                    Email = "staff@gmail.com",
                    Role = "Staff",
                    DepartmentId = 2
                };

                var qacoor = new ApplicationUser
                {
                    FullName = "Hieu Nguyen Coor",
                    UserName = "qacoor@gmail.com",
                    Email = "qacoor@gmail.com",
                    Role = "QACoordinator",
                    DepartmentId = 2
                };

                manager.Create(staff, "Staff@1");
                manager.AddToRole(staff.Id, "Staff");

                manager.Create(qacoor, "Qacoor@1");
                manager.AddToRole(qacoor.Id, "QACoordinator");
            }
        }

        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUserName, string adminFullName, string adminPassword, string adminRole)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                FullName = adminFullName,
                Email = adminEmail,
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }
    }
}