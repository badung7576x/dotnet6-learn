using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

  public DbSet<AppUser> AppUser { get; set; }

  // Override when create database.
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    // Remove table prefix (AspNet) in database
    foreach (var entityType in builder.Model.GetEntityTypes())
    {
      var tableName = entityType.GetTableName();
      if (tableName.StartsWith("AspNet"))
      {
        entityType.SetTableName(tableName.Substring(6));
      }
    }
  }
}