using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portal.Domain.Models;

namespace Portal.EFInfrastructure
{
    public class Context : DbContext
    {
        public Context()
        {
        }

        public DbSet<Material> Materials { get; set; }

        public DbSet<ArticleMaterial> Articles { get; set; }

        public DbSet<BookMaterial> Books { get; set; }

        public DbSet<VideoMaterial> Videos { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseSkill> CourseSkills { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserSkill> UserSkills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.json");
            var config = configBuilder.Build();
            var connectionString = config.GetConnectionString("DatabaseConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder can't be null");
            }

            modelBuilder.Entity<Course>().
                Navigation(course => course.Skills).AutoInclude();
            modelBuilder.Entity<Course>().
                Navigation(course => course.Materials).AutoInclude();
            modelBuilder.Entity<Course>().
                Navigation(course => course.Owner).AutoInclude();
            modelBuilder.Entity<UserSkill>().
                Navigation(userSkill => userSkill.Owner).AutoInclude();
        }
    }
}
