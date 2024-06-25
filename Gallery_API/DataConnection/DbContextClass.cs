using Microsoft.EntityFrameworkCore;
using Gallery_API.Model;
using Gallery_API.Models;

namespace Gallery_API.DataConnection
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(DbContextOptions<DbContextClass> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Login>().HasNoKey(); // Assuming Login entity exists in your models
            modelBuilder.Entity<UserDto>().HasNoKey();

            // You can add further configurations here if needed
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDto> UserDto { get; set; }

        public DbSet<Image> Images { get; set; }


    }
}