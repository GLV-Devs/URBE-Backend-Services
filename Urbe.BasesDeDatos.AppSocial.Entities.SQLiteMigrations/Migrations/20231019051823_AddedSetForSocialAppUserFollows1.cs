using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.BasesDeDatos.AppSocial.Entities.SQLiteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddedSetForSocialAppUserFollows1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialAppUserFollow");

            migrationBuilder.CreateTable(
                name: "SocialAppUserFollows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FollowerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FollowedId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialAppUserFollows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialAppUserFollows_SocialAppUsers_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "SocialAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialAppUserFollows_SocialAppUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "SocialAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialAppUserFollows_FollowedId",
                table: "SocialAppUserFollows",
                column: "FollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialAppUserFollows_FollowerId",
                table: "SocialAppUserFollows",
                column: "FollowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialAppUserFollows");

            migrationBuilder.CreateTable(
                name: "SocialAppUserFollow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FollowedId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FollowerId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialAppUserFollow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialAppUserFollow_SocialAppUsers_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "SocialAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialAppUserFollow_SocialAppUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "SocialAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialAppUserFollow_FollowedId",
                table: "SocialAppUserFollow",
                column: "FollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialAppUserFollow_FollowerId",
                table: "SocialAppUserFollow",
                column: "FollowerId");
        }
    }
}
