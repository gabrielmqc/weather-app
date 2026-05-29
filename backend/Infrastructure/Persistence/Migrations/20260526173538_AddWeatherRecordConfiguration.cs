using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWeatherRecordConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "weather_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    Longitude = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    Temperature = table.Column<double>(type: "double precision", precision: 5, scale: 2, nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather_records", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_weather_records_City",
                table: "weather_records",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_weather_records_Latitude_Longitude",
                table: "weather_records",
                columns: new[] { "Latitude", "Longitude" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weather_records");
        }
    }
}
