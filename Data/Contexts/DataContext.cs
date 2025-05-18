using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ProfileEntity> UserProfiles { get; set; }
    public DbSet<AddressInfoEntity> AddressInfos { get; set; }
    public DbSet<AddressTypeEntity> AddressTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AddressTypeEntity>().HasData(
            new AddressTypeEntity { Id = 1, AddressType = "Home" },
            new AddressTypeEntity { Id = 2, AddressType = "Work" },
            new AddressTypeEntity { Id = 3, AddressType = "Other" }
        );
    }
}
