using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasketWebApi.Entities;

namespace SurveyBasketWebApi.Persistence.Configurations;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(U => U.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(U => U.LastName)
                .HasMaxLength(100).IsRequired();

        builder.OwnsMany(x => x.RefreshTokens)
            .ToTable("RefreshTokens") // to change name of table
            .WithOwner()
            .HasForeignKey("UserId");
    }
}