using Microsoft.EntityFrameworkCore.Migrations;

namespace Exoft.Gamification.Api.Data.Migrations
{
    public partial class addCommentFieldToUserAchievement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "UserAchievement",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "UserAchievement");
        }
    }
}
