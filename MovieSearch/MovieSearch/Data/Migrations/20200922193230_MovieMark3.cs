using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSearch.Data.Migrations
{
    public partial class MovieMark3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MovieMarks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_MovieMarks_UserId",
                table: "MovieMarks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieMarks_AspNetUsers_UserId",
                table: "MovieMarks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieMarks_AspNetUsers_UserId",
                table: "MovieMarks");

            migrationBuilder.DropIndex(
                name: "IX_MovieMarks_UserId",
                table: "MovieMarks");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "MovieMarks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "MovieMarks",
                type: "nvarchar(450)",
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
    }
}
