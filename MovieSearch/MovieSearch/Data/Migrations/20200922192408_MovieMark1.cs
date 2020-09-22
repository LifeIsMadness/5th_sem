using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSearch.Data.Migrations
{
    public partial class MovieMark1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieMarks_AspNetUsers_ApplicationUserId1",
                table: "MovieMarks");

            migrationBuilder.DropIndex(
                name: "IX_MovieMarks_ApplicationUserId1",
                table: "MovieMarks");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "MovieMarks");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "MovieMarks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MovieMarks",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieMarks_AspNetUsers_ApplicationUserId",
                table: "MovieMarks");

            migrationBuilder.DropIndex(
                name: "IX_MovieMarks_ApplicationUserId",
                table: "MovieMarks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MovieMarks");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "MovieMarks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "MovieMarks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieMarks_ApplicationUserId1",
                table: "MovieMarks",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieMarks_AspNetUsers_ApplicationUserId1",
                table: "MovieMarks",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
