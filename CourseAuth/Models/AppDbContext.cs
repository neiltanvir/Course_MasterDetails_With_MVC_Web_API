using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CourseAuth.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("AppDbContext") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseDetail> CourseDetails { get; set; }
        public DbSet<Module> Modules { get; set; }
    }
}