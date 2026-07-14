using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Prodemos.Domain.Configuration;
public class ChampionshipConfiguration : IEntityTypeConfiguration<Championship>
{
    public void Configure(EntityTypeBuilder<Championship> builder)
    {
        builder.HasMany(c => c.Matches)
               .WithOne(m => m.Championship)
               .HasForeignKey(m => m.ChampionshipId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.UserPlays)
               .WithOne(up => up.Championship)
               .HasForeignKey(up => up.ChampionshipId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
