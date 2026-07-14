using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Prodemos.Domain.Configuration;
public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasMany(m => m.UserGuests)
               .WithOne(ug => ug.Match)
               .HasForeignKey(m => m.MatchId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.TeamA)
               .WithMany(ta => ta.Matches)
               .HasForeignKey(ta => ta.TeamAId)
               .IsRequired()
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(m => m.TeamB)
               .WithMany(tb => tb.Matches)
               .HasForeignKey(tb => tb.TeamBId)
               .IsRequired()
               .OnDelete(DeleteBehavior.SetNull);

        builder.Property(m => m.MatchStatus).HasConversion(
            m => m.ToString(),
            m => (MatchStatus)Enum.Parse(typeof(MatchStatus), m)
        );
    }
}
