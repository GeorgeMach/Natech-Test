using AwesomeCatApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AwesomeCatApi.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<CatEntity> Cats { get; set; }
        public DbSet<TagEntity> Tags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatEntity>()
                .HasMany(c => c.Tags)
                .WithMany(t => t.Cats)
                .UsingEntity(j => j.ToTable("CatTags"));
        }
    }
}
