using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppSocial.Entities.SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Post_MaxContentLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LastSeenPostInFeedId",
                table: "SocialAppUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "nvarchar(350)",
                maxLength: 350,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SocialAppUsers_LastSeenPostInFeedId",
                table: "SocialAppUsers",
                column: "LastSeenPostInFeedId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialAppUsers_Posts_LastSeenPostInFeedId",
                table: "SocialAppUsers",
                column: "LastSeenPostInFeedId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialAppUsers_Posts_LastSeenPostInFeedId",
                table: "SocialAppUsers");

            migrationBuilder.DropIndex(
                name: "IX_SocialAppUsers_LastSeenPostInFeedId",
                table: "SocialAppUsers");

            migrationBuilder.DropColumn(
                name: "LastSeenPostInFeedId",
                table: "SocialAppUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(350)",
                oldMaxLength: 350);
        }
    }
}
