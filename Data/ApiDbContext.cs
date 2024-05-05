using cush.Models;
using Microsoft.EntityFrameworkCore;

namespace cush.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options):base(options) {  }

    public DbSet<Cush> Cushes { get; set; }



    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<Cush>();
        var utcNow = DateTime.UtcNow;
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = utcNow;
                entry.Entity.UpdatedAt = utcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = utcNow;
            }
        }
    }
}