using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Prodemos.Domain.Configuration;
public class UserGuestConfiguration : IEntityTypeConfiguration<UserGuest>
{
    public void Configure(EntityTypeBuilder<UserGuest> builder)
    {
        builder.Property(ug => ug.GuessStatus).HasConversion(
            ug => ug.ToString(),
            ug => (GuessStatus)Enum.Parse(typeof(GuessStatus), ug)
        );
    }
}
