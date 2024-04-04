using Microsoft.EntityFrameworkCore;
using StudentManager_BackEnd.Entity;
namespace Entity
{
    public class ContextDb : DbContext
    {
        public ContextDb() { }
        public ContextDb(DbContextOptions<ContextDb> options)
           : base(options)
        {
        }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
