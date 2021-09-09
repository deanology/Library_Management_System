using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_Management_System.Migrations
{
    public partial class addForeignKeyToModelClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "CheckOuts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "CheckIns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CheckOuts_BookId",
                table: "CheckOuts",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_BookId",
                table: "CheckIns",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckIns_Books_BookId",
                table: "CheckIns",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOuts_Books_BookId",
                table: "CheckOuts",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckIns_Books_BookId",
                table: "CheckIns");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOuts_Books_BookId",
                table: "CheckOuts");

            migrationBuilder.DropIndex(
                name: "IX_CheckOuts_BookId",
                table: "CheckOuts");

            migrationBuilder.DropIndex(
                name: "IX_CheckIns_BookId",
                table: "CheckIns");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "CheckOuts");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "CheckIns");
        }
    }
}
