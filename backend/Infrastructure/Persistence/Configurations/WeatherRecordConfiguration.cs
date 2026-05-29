using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class WeatherRecordConfiguration : IEntityTypeConfiguration<WeatherRecord>
{
    public void Configure(EntityTypeBuilder<WeatherRecord> builder)
    {
        builder.ToTable("weather_records");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.NormalizedCity)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Temperature)
            .HasPrecision(5, 2);

        builder.Property(x => x.RecordedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.Latitude)
            .HasPrecision(9, 6)
            .IsRequired();

        builder.Property(x => x.Longitude)
            .HasPrecision(9, 6)
            .IsRequired();

        builder.HasIndex(x => x.NormalizedCity);

        builder.HasIndex(x => new
        {
            x.Latitude,
            x.Longitude
        });
    }
}