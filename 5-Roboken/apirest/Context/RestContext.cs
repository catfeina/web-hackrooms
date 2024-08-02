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
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<CommentModel> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<CommentModel>()
            .HasOne(c => c.Task)
            .WithMany(t => t.Commets)
            .HasForeignKey(c => c.TaskId);

        base.OnModelCreating(builder);
    }
}