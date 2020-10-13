using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieSearch.Migrations
{
    public partial class UserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MoviesProfiles_MoviesProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MoviesProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MoviesProfileId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MoviesProfiles",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoviesProfiles_UserId",
                table: "MoviesProfiles",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesProfiles_AspNetUsers_UserId",
                table: "MoviesProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviesProfiles_AspNetUsers_UserId",
                table: "MoviesProfiles");

            migrationBuilder.DropIndex(
                name: "IX_MoviesProfiles_UserId",
                table: "MoviesProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MoviesProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MoviesProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MoviesProfileId",
                table: "AspNetUsers",
                column: "MoviesProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MoviesProfiles_MoviesProfileId",
                table: "AspNetUsers",
                column: "MoviesProfileId",
                principalTable: "MoviesProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
