using CourseAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseAuth.Repository
{
    public class UserRepo : IDisposable
    {
        AppDbContext _db = new AppDbContext();

        public User ValidateUser(string userName, string password)
        {
            return _db.Users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}