using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvcapi.Models;

namespace mvcapi.Context;

public class MyDbContext(
    DbContextOptions<MyDbContext> options
) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<MyEntity> MyEntities { get; set; }
    public DbSet<Users> User { get; set; }
    public DbSet<Paragraph> Poem { get; set; }
}
