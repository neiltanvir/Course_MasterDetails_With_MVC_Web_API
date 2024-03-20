namespace CourseAuth.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CourseAuth.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CourseAuth.Models.AppDbContext context)
        {
            context.Modules.AddOrUpdate(x => x.ModuleId,
                new Models.Module() { ModuleId = 1, ModuleName = "MVC", Fee = 18000 },
                new Models.Module() { ModuleId = 2, ModuleName = "Core", Fee = 20000 },
                new Models.Module() { ModuleId = 1, ModuleName = "API", Fee = 25000 });

            context.Users.AddOrUpdate(x => x.UserId,
                new Models.User() { UserId = 1, UserName = "Admin", Password = "1234", Email = "admin@gmail.com", Roles = "Admin" },
                new Models.User() { UserId = 2, UserName = "User", Password = "1234", Email = "User@gmail.com", Roles = "User" });
        }
    }
}
