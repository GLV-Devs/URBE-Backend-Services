using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppSocial.Entities.SQLServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Add_UsersWhoLikedThis_ModelConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialAppUserLike",
                columns: table => new
                {
                    UserWhoLikedThisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialAppUserLike", x => new { x.PostId, x.UserWhoLikedThisId });
                    table.ForeignKey(
                        name: "FK_SocialAppUserLike_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SocialAppUserLike_SocialAppUsers_UserWhoLikedThisId",
                        column: x => x.UserWhoLikedThisId,
                        principalTable: "SocialAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialAppUserLike_UserWhoLikedThisId",
                table: "SocialAppUserLike",
                column: "UserWhoLikedThisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialAppUserLike");

        }
    }
}
