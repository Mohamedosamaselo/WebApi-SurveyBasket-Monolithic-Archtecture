using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasketWebApi.Entities;

namespace SurveyBasketWebApi.Persistence.Configurations;

public class PollConfigurations : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.HasIndex(E => E.Title).IsUnique();

        builder.Property(E => E.Title).HasMaxLength(100).IsRequired();

        builder.Property(E => E.Summary).HasMaxLength(1000).IsRequired();
    }
}