using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.BasesDeDatos.AppSocial.Entities.SQLiteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Post_NullableInResponseTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "InResponseToId",
                table: "Posts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "InResponseToId",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
