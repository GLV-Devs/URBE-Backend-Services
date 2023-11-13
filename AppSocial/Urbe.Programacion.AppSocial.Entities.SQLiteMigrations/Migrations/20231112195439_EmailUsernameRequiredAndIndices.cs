using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Urbe.Programacion.AppSocial.Entities.SQLiteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class EmailUsernameRequiredAndIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PendingMailConfirmations_UserId",
                table: "PendingMailConfirmations");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "SocialAppUsers",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "SocialAppUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "SocialAppUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "PendingMailConfirmations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Validity",
                table: "PendingMailConfirmations",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_SocialAppUsers_Email",
                table: "SocialAppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialAppUsers_NormalizedEmail",
                table: "SocialAppUsers",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialAppUsers_NormalizedUserName",
                table: "SocialAppUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PendingMailConfirmations_Token",
                table: "PendingMailConfirmations",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PendingMailConfirmations_UserId",
                table: "PendingMailConfirmations",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SocialAppUsers_Email",
                table: "SocialAppUsers");

            migrationBuilder.DropIndex(
                name: "IX_SocialAppUsers_NormalizedEmail",
                table: "SocialAppUsers");

            migrationBuilder.DropIndex(
                name: "IX_SocialAppUsers_NormalizedUserName",
                table: "SocialAppUsers");

            migrationBuilder.DropIndex(
                name: "IX_PendingMailConfirmations_Token",
                table: "PendingMailConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_PendingMailConfirmations_UserId",
                table: "PendingMailConfirmations");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "PendingMailConfirmations");

            migrationBuilder.DropColumn(
                name: "Validity",
                table: "PendingMailConfirmations");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "SocialAppUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "SocialAppUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "SocialAppUsers",
                type: "TEXT",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_PendingMailConfirmations_UserId",
                table: "PendingMailConfirmations",
                column: "UserId");
        }
    }
}
