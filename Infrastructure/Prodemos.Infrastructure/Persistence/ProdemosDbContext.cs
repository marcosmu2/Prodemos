using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prodemos.Application.Services.Interfaces;
using Prodemos.Domain;
using Prodemos.Domain.Configuration;

namespace Prodemos.Infrastructure.Persistence;
public class ProdemosDbContext : IdentityDbContext<User>
{
    private readonly IAuthService _authService;
    public DbSet<Championship> Championships { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<UserGuest> UserGuests { get; set; }
    public DbSet<UserPlay> UserPlays { get; set; }


    public ProdemosDbContext(DbContextOptions<ProdemosDbContext> options, IAuthService authService) : base(options)
    {
        _authService = authService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().Property(x => x.Id).HasMaxLength(36);
        builder.Entity<User>().Property(x => x.NormalizedUserName).HasMaxLength(90);
        builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(36);
        builder.Entity<IdentityRole>().Property(x => x.NormalizedName).HasMaxLength(90);
        builder.ApplyConfigurationsFromAssembly(typeof(MatchConfiguration).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserEmail = _authService.GetSessionUserEmail();
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = currentUserEmail;
                entry.Entity.CreatedDate = now;
                entry.Entity.UpdatedBy = currentUserEmail;
                entry.Entity.UpdatedDate = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedBy = currentUserEmail;
                entry.Entity.UpdatedDate = now;
                entry.Property(nameof(BaseDomainModel.CreatedBy)).IsModified = false;
                entry.Property(nameof(BaseDomainModel.CreatedDate)).IsModified = false;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
