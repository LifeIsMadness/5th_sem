using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSearch.Data.Migrations
{
    public partial class MovieMark2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieMarks_AspNetUsers_ApplicationUserId",
                table: "MovieMarks");

            migrationBuilder.DropIndex(
                name: "IX_MovieMarks_ApplicationUserId",
                table: "MovieMarks");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "MovieMarks");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "MovieMarks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieMarks_UserId1",
                table: "MovieMarks",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieMarks_AspNetUsers_UserId1",
                table: "MovieMarks",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieMarks_AspNetUsers_UserId1",
                table: "MovieMarks");

            migrationBuilder.DropIndex(
                name: "IX_MovieMarks_UserId1",
                table: "MovieMarks");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "MovieMarks");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "MovieMarks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieMarks_ApplicationUserId",
                table: "MovieMarks",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieMarks_AspNetUsers_ApplicationUserId",
                table: "MovieMarks",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
