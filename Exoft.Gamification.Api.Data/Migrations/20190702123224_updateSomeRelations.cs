using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Exoft.Gamification.Api.Data.Migrations
{
    public partial class updateSomeRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedTime",
                table: "Thanks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Events",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Thanks_FromUserId",
                table: "Thanks",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Thanks_ToUserId",
                table: "Thanks",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thanks_Users_FromUserId",
                table: "Thanks",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Thanks_Users_ToUserId",
                table: "Thanks",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Thanks_Users_FromUserId",
                table: "Thanks");

            migrationBuilder.DropForeignKey(
                name: "FK_Thanks_Users_ToUserId",
                table: "Thanks");

            migrationBuilder.DropIndex(
                name: "IX_Thanks_FromUserId",
                table: "Thanks");

            migrationBuilder.DropIndex(
                name: "IX_Thanks_ToUserId",
                table: "Thanks");

            migrationBuilder.DropColumn(
                name: "AddedTime",
                table: "Thanks");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Events",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
