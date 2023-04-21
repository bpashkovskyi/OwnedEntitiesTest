using Microsoft.EntityFrameworkCore;

namespace OwnedEntitiesTest
{
    public class ParentEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<OwnedEntity> OwnedEntities { get; set; }
    }

    public class OwnedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Uncomment this Id to make test passed
        ////public Guid ParentEntityId { get; set; }
    }

    public class MyDbContext : DbContext
    {
        public DbSet<ParentEntity> ParentEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParentEntity>()
                .OwnsMany(parent => parent.OwnedEntities);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
        }
    }
}