using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppSocial.Entities.SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class UserSettingsTypeChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Settings",
                table: "SocialAppUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 30L,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)",
                oldDefaultValue: 30m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Settings",
                table: "SocialAppUsers",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 30m,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 30L);
        }
    }
}
