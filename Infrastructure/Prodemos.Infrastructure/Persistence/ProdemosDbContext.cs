using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prodemos.Domain;
using Prodemos.Domain.Configuration;

namespace Prodemos.Infrastructure.Persistence;
public class ProdemosDbContext : IdentityDbContext<User>
{
    public DbSet<Championship> Championships{ get; set; }
    public DbSet<Match> Matches{ get; set; }
    public DbSet<Team> Teams{ get; set; }
    public DbSet<UserGuest> UserGuests{ get; set; }
    public DbSet<UserPlay> UserPlays{ get; set; }


    public ProdemosDbContext(DbContextOptions<ProdemosDbContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().Property(x => x.Id).HasMaxLength(36);
        builder.Entity<User>().Property(x => x.NormalizedUserName).HasMaxLength(90);
        builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(36);
        builder.Entity<IdentityRole>().Property(x => x.NormalizedName).HasMaxLength(90);
        builder.ApplyConfigurationsFromAssembly(typeof(MatchConfiguration).Assembly);
    }
}
