using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppSocial.Entities.SQLiteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class RenamedUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingMailConfirmations_Users_UserId",
                table: "PendingMailConfirmations");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_PosterId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialAppUserClaims_Users_UserId",
                table: "SocialAppUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialAppUserFollow_Users_FollowedId",
                table: "SocialAppUserFollow");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialAppUserFollow_Users_FollowerId",
                table: "SocialAppUserFollow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "SocialAppUsers");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserName",
                table: "SocialAppUsers",
                newName: "IX_SocialAppUsers_UserName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialAppUsers",
                table: "SocialAppUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingMailConfirmations_SocialAppUsers_UserId",
                table: "PendingMailConfirmations",
                column: "UserId",
                principalTable: "SocialAppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_SocialAppUsers_PosterId",
                table: "Posts",
                column: "PosterId",
                principalTable: "SocialAppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAppUserClaims_SocialAppUsers_UserId",
                table: "SocialAppUserClaims",
                column: "UserId",
                principalTable: "SocialAppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAppUserFollow_SocialAppUsers_FollowedId",
                table: "SocialAppUserFollow",
                column: "FollowedId",
                principalTable: "SocialAppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAppUserFollow_SocialAppUsers_FollowerId",
                table: "SocialAppUserFollow",
                column: "FollowerId",
                principalTable: "SocialAppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingMailConfirmations_SocialAppUsers_UserId",
                table: "PendingMailConfirmations");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_SocialAppUsers_PosterId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialAppUserClaims_SocialAppUsers_UserId",
                table: "SocialAppUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialAppUserFollow_SocialAppUsers_FollowedId",
                table: "SocialAppUserFollow");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialAppUserFollow_SocialAppUsers_FollowerId",
                table: "SocialAppUserFollow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialAppUsers",
                table: "SocialAppUsers");

            migrationBuilder.RenameTable(
                name: "SocialAppUsers",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_SocialAppUsers_UserName",
                table: "Users",
                newName: "IX_Users_UserName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingMailConfirmations_Users_UserId",
                table: "PendingMailConfirmations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_PosterId",
                table: "Posts",
                column: "PosterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAppUserClaims_Users_UserId",
                table: "SocialAppUserClaims",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAppUserFollow_Users_FollowedId",
                table: "SocialAppUserFollow",
                column: "FollowedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAppUserFollow_Users_FollowerId",
                table: "SocialAppUserFollow",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
