using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppSocial.Entities.SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class CK_SocialAppUserFollow_NoSelfFollow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_SocialAppUserFollow_NoSelfFollow",
                table: "SocialAppUserFollows",
                sql: "FollowedId <> FollowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_SocialAppUserFollow_NoSelfFollow",
                table: "SocialAppUserFollows",
                sql: "constraint not_equal check (FollowedId <> FollowerId)");
        }
    }
}
