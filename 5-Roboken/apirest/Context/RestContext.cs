using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using apirest.Models;

namespace apirest.Context;

public class RestContext(
    DbContextOptions<RestContext> options
    ) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<PoemModel> Poem { get; set; }
}