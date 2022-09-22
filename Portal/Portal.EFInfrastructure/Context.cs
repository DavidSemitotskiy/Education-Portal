using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portal.Domain.Models;

namespace Portal.EFInfrastructure
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Material> Materials { get; set; }

        public DbSet<ArticleMaterial> Articles { get; set; }

        public DbSet<BookMaterial> Books { get; set; }

        public DbSet<VideoMaterial> Videos { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseSkill> CourseSkills { get; set; }

        public DbSet<UserSkill> UserSkills { get; set; }

        public DbSet<CourseState> CourseStates { get; set; }

        public DbSet<MaterialState> MaterialStates { get; set; }
    }
}
