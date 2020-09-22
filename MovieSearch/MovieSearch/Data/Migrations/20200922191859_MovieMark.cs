using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSearch.Data.Migrations
{
    public partial class MovieMark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieMark_AspNetUsers_ApplicationUserId1",
                table: "MovieMark");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieMark_Movies_MovieId",
                table: "MovieMark");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieMark",
                table: "MovieMark");

            migrationBuilder.RenameTable(
                name: "MovieMark",
                newName: "MovieMarks");

            migrationBuilder.RenameIndex(
                name: "IX_MovieMark_MovieId",
                table: "MovieMarks",
                newName: "IX_MovieMarks_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieMark_ApplicationUserId1",
                table: "MovieMarks",
                newName: "IX_MovieMarks_ApplicationUserId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieMarks",
                table: "MovieMarks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieMarks_AspNetUsers_ApplicationUserId1",
                table: "MovieMarks",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieMarks_Movies_MovieId",
                table: "MovieMarks",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieMarks_AspNetUsers_ApplicationUserId1",
                table: "MovieMarks");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieMarks_Movies_MovieId",
                table: "MovieMarks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieMarks",
                table: "MovieMarks");

            migrationBuilder.RenameTable(
                name: "MovieMarks",
                newName: "MovieMark");

            migrationBuilder.RenameIndex(
                name: "IX_MovieMarks_MovieId",
                table: "MovieMark",
                newName: "IX_MovieMark_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieMarks_ApplicationUserId1",
                table: "MovieMark",
                newName: "IX_MovieMark_ApplicationUserId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieMark",
                table: "MovieMark",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieMark_AspNetUsers_ApplicationUserId1",
                table: "MovieMark",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieMark_Movies_MovieId",
                table: "MovieMark",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
