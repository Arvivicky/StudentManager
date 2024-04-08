using Microsoft.EntityFrameworkCore;
using StudentManager_BackEnd.Entity;
using System.Data;
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
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //many-to-many
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>(
                    j => j
                        .HasOne(ur => ur.Role)
                        .WithMany()
                        .HasForeignKey(ur => ur.RoleId),
                    j => j
                        .HasOne(ur => ur.User)
                        .WithMany()
                        .HasForeignKey(ur => ur.UserId),
                    j =>
                    {
                        j.HasKey(ur => new { ur.UserId, ur.RoleId });
                    });


        }
    }
}
