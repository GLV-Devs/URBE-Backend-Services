using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppSocial.Entities.SQLiteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddedSetForSocialAppUserFollows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialAppUserClaims_SocialAppUsers_UserId",
                table: "SocialAppUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialAppUserClaims",
                table: "SocialAppUserClaims");

            migrationBuilder.RenameTable(
                name: "SocialAppUserClaims",
                newName: "IdentityUserClaim<Guid>");

            migrationBuilder.RenameIndex(
                name: "IX_SocialAppUserClaims_UserId",
                table: "IdentityUserClaim<Guid>",
                newName: "IX_IdentityUserClaim<Guid>_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim<Guid>",
                table: "IdentityUserClaim<Guid>",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<Guid>_SocialAppUsers_UserId",
                table: "IdentityUserClaim<Guid>",
                column: "UserId",
                principalTable: "SocialAppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserClaim<Guid>_SocialAppUsers_UserId",
                table: "IdentityUserClaim<Guid>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim<Guid>",
                table: "IdentityUserClaim<Guid>");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim<Guid>",
                newName: "SocialAppUserClaims");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUserClaim<Guid>_UserId",
                table: "SocialAppUserClaims",
                newName: "IX_SocialAppUserClaims_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialAppUserClaims",
                table: "SocialAppUserClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAppUserClaims_SocialAppUsers_UserId",
                table: "SocialAppUserClaims",
                column: "UserId",
                principalTable: "SocialAppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
