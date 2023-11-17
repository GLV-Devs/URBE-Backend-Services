using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppSocial.Entities.SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Add_UsersWhoLikedThis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPostLikes",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    UserWhoLikedThisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPostLikes", x => new { x.PostId, x.UserWhoLikedThisId });
                    table.ForeignKey(
                        name: "FK_UserPostLikes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserPostLikes_SocialAppUsers_UserWhoLikedThisId",
                        column: x => x.UserWhoLikedThisId,
                        principalTable: "SocialAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPostLikes_UserWhoLikedThisId",
                table: "UserPostLikes",
                column: "UserWhoLikedThisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPostLikes");
        }
    }
}
