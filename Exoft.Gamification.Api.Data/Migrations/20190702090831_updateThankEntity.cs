using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Exoft.Gamification.Api.Data.Migrations
{
    public partial class updateThankEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedTime",
                table: "Thanks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Thanks_FromUserId",
                table: "Thanks",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Thanks_Users_FromUserId",
                table: "Thanks",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thanks_Users_FromUserId",
                table: "Thanks");

            migrationBuilder.DropIndex(
                name: "IX_Thanks_FromUserId",
                table: "Thanks");

            migrationBuilder.DropColumn(
                name: "AddedTime",
                table: "Thanks");
        }
    }
}
