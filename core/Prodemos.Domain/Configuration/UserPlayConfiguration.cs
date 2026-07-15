using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Prodemos.Domain.Configuration;
public class UserPlayConfiguration : IEntityTypeConfiguration<UserPlay>
{
    public void Configure(EntityTypeBuilder<UserPlay> builder)
    {
        builder.HasMany(up => up.UserGuests)
               .WithOne(ug => ug.UserPlay)
               .HasForeignKey(ug => ug.UserPlayId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
