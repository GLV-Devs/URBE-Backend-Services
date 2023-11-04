using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppSocial.Entities.SQLiteMigrations.Migrations;

/// <inheritdoc />
public partial class Base : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                RealName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                Pronouns = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                ProfileMessage = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                Settings = table.Column<ulong>(type: "INTEGER", nullable: false, defaultValue: 30ul),
                Email = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                FollowerCount = table.Column<int>(type: "INTEGER", nullable: false),
                ProfilePictureUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                UserName = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PendingMailConfirmations",
            columns: table => new
            {
                Id = table.Column<byte[]>(type: "BLOB", nullable: false),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                CreationDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PendingMailConfirmations", x => x.Id);
                table.ForeignKey(
                    name: "FK_PendingMailConfirmations_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Posts",
            columns: table => new
            {
                Id = table.Column<long>(type: "INTEGER", nullable: false),
                PosterId = table.Column<Guid>(type: "TEXT", nullable: false),
                Content = table.Column<string>(type: "TEXT", nullable: false),
                PosterThenUsername = table.Column<string>(type: "TEXT", nullable: false),
                DatePosted = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                InResponseToId = table.Column<long>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Posts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Posts_Posts_InResponseToId",
                    column: x => x.InResponseToId,
                    principalTable: "Posts",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Posts_Users_PosterId",
                    column: x => x.PosterId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SocialAppUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SocialAppUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_SocialAppUserClaims_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SocialAppUserFollow",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                FollowerId = table.Column<Guid>(type: "TEXT", nullable: false),
                FollowedId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SocialAppUserFollow", x => x.Id);
                table.ForeignKey(
                    name: "FK_SocialAppUserFollow_Users_FollowedId",
                    column: x => x.FollowedId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SocialAppUserFollow_Users_FollowerId",
                    column: x => x.FollowerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PendingMailConfirmations_UserId",
            table: "PendingMailConfirmations",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Posts_InResponseToId",
            table: "Posts",
            column: "InResponseToId");

        migrationBuilder.CreateIndex(
            name: "IX_Posts_PosterId",
            table: "Posts",
            column: "PosterId");

        migrationBuilder.CreateIndex(
            name: "IX_SocialAppUserClaims_UserId",
            table: "SocialAppUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_SocialAppUserFollow_FollowedId",
            table: "SocialAppUserFollow",
            column: "FollowedId");

        migrationBuilder.CreateIndex(
            name: "IX_SocialAppUserFollow_FollowerId",
            table: "SocialAppUserFollow",
            column: "FollowerId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_UserName",
            table: "Users",
            column: "UserName",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PendingMailConfirmations");

        migrationBuilder.DropTable(
            name: "Posts");

        migrationBuilder.DropTable(
            name: "SocialAppUserClaims");

        migrationBuilder.DropTable(
            name: "SocialAppUserFollow");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
