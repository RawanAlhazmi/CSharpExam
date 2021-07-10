using Microsoft.EntityFrameworkCore;
namespace Exam.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<ActivityT> Activities { get; set; }
        public DbSet<Join> Joins { get; set; }
    }
}
