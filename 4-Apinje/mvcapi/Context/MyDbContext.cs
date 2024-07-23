using Microsoft.EntityFrameworkCore;
using mvcapi.Models;

namespace mvcapi.Context;

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
{
    public DbSet<MyEntity> MyEntities { get; set; }
    public DbSet<Users> User { get; set; }

    /*
    Para que cada nombre sea único
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MyEntity>().HasIndex(c => c.Nombre).IsUnique();
    }
    */
}
