using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseAuth.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string StudentName { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsRegular { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<CourseDetail> CourseDetails { get; set; }
    }

    public class CourseDetail
    {
        public int CourseDetailId { get; set; }
        public int CourseId { get; set; }
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }
    }

    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public decimal Fee { get; set; }
    }
    public class CourseRequest
    {
        public Course Course { get; set; }
        public byte[] ImageFile { get; set; }
        public string ImageFileName { get; set; }
    }
}