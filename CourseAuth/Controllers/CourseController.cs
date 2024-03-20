using CourseAuth.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CourseAuth.Controllers
{
    public class CourseController : ApiController
    {
        private AppDbContext db = new AppDbContext();
        [Authorize(Roles = ("User,Admin"))]
        public System.Object GetCourses()
        {
            var result = db.Courses.ToList();
            return result.OrderBy(o => o.CourseId);
        }
        [Authorize(Roles = ("User,Admin"))]
        public IHttpActionResult GetCourseById(int id)
        {
            var course = (from a in db.Courses
                          where a.CourseId == id
                         select new
                         {
                             a.CourseId,
                             a.CourseCode,
                             a.StudentName,
                             a.IsRegular,
                             a.ImageUrl
                         }).FirstOrDefault();
            var courseDetails = (from a in db.CourseDetails
                                join b in db.Modules on a.ModuleId equals b.ModuleId
                                where a.CourseId == id
                                select new
                                {
                                    a.CourseId,
                                    //a.OrderedItemId,
                                    a.ModuleId,
                                    b.ModuleName,
                                    b.Fee
                                }).ToList();
            return Ok(new { course, courseDetails });
        }
        [Authorize(Roles = ("Admin"))]
        public IHttpActionResult DeleteCourse(int id)
        {
            var course = db.Courses.Find(id);
            var courseDetails = db.CourseDetails.Where(item => item.CourseId == id).ToList();
            foreach (var item in courseDetails)
            {
                db.CourseDetails.Remove(item);
            }
            db.Courses.Remove(course);
            db.SaveChanges();
            return Ok("Course and related items have been successfully deleted.");
        }
        [Authorize(Roles = ("Admin"))]
        public IHttpActionResult PostCourse(CourseRequest request)
        {
            if (request.Course == null)
            {
                return BadRequest("Course data is Missing");
            }
            Course obj = request.Course;
            byte[] imageFile = request.ImageFile;
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine("~/Images/", fileName);

                File.WriteAllBytes(HttpContext.Current.Server.MapPath(filePath), imageFile);
                obj.ImageUrl = filePath;
            }
            Course course = new Course
            {
                CourseCode = obj.CourseCode,
                StartDate = obj.StartDate,
                StudentName = obj.StudentName,
                IsRegular = obj.IsRegular,
                ImageUrl = obj.ImageUrl,
            };
            db.Courses.Add(course);
            db.SaveChanges();
            var courseObj = db.Courses.FirstOrDefault(x => x.CourseCode == obj.CourseCode);
            if (courseObj != null && obj.CourseDetails != null)
            {
                foreach (var item in obj.CourseDetails)
                {
                    CourseDetail courseDetail = new CourseDetail
                    {
                        CourseId = courseObj.CourseId,
                        ModuleId = item.ModuleId,
                    };
                    db.CourseDetails.Add(courseDetail);
                }
            }
            db.SaveChanges();
            return Ok("Course Saved Successfully");
        }
        [Authorize(Roles = ("Admin"))]
        public IHttpActionResult PutCourse(int id, CourseRequest request)
        {
            Course course = db.Courses.FirstOrDefault(x => x.CourseId == id);
            if (id != request.Course.CourseId)
            {
                return BadRequest();
            }
            if (course == null)
            {
                return NotFound();
            }
            if (request.Course == null)
            {
                return BadRequest("Course data is missing.");
            }
            Course obj = request.Course;
            byte[] imageFile = request.ImageFile;
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine("~/Images/", fileName);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(filePath), imageFile);
                obj.ImageUrl = filePath;
            }
            course.CourseCode = obj.CourseCode;
            course.StartDate = obj.StartDate;
            course.StudentName = obj.StudentName;
            course.IsRegular = obj.IsRegular;
            course.ImageUrl = obj.ImageUrl;
            var existingCourseDetails = db.CourseDetails.Where(x => x.CourseId == course.CourseId);
            db.CourseDetails.RemoveRange(existingCourseDetails);
            if (obj.CourseDetails != null)
            {
                foreach (var item in obj.CourseDetails)
                {
                    CourseDetail courseDetail = new CourseDetail
                    {
                        CourseId = course.CourseId,
                        ModuleId = item.ModuleId
                    };
                    db.CourseDetails.Add(courseDetail);
                }
            }
            db.SaveChanges();
            return Ok("Course and related items have been successfully updated.");
        }
    }
}
