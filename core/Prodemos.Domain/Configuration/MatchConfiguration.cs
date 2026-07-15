using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Prodemos.Domain.Configuration;
public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasOne(m => m.TeamA)
               .WithMany(t => t.MatchesAsTeamA)
               .HasForeignKey(m => m.TeamAId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.TeamB)
               .WithMany(t => t.MatchesAsTeamB)
               .HasForeignKey(m => m.TeamBId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(m => m.MatchStatus).HasConversion(
            m => m.ToString(),
            m => (MatchStatus)Enum.Parse(typeof(MatchStatus), m)
        );
    }
}
