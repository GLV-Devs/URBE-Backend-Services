using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppVehiculos.Entities.SQLiteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddedVehicleMake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehicleMake",
                table: "Reports",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_VehicleMake",
                table: "Reports",
                column: "VehicleMake");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_VehicleMake",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "VehicleMake",
                table: "Reports");
        }
    }
}
