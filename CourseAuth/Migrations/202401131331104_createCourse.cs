namespace CourseAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createCourse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseDetails",
                c => new
                    {
                        CourseDetailId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseDetailId)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        ModuleId = c.Int(nullable: false, identity: true),
                        ModuleName = c.String(),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ModuleId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseCode = c.String(),
                        StudentName = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        IsRegular = c.Boolean(nullable: false),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Roles = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseDetails", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseDetails", "ModuleId", "dbo.Modules");
            DropIndex("dbo.CourseDetails", new[] { "ModuleId" });
            DropIndex("dbo.CourseDetails", new[] { "CourseId" });
            DropTable("dbo.Users");
            DropTable("dbo.Courses");
            DropTable("dbo.Modules");
            DropTable("dbo.CourseDetails");
        }
    }
}
