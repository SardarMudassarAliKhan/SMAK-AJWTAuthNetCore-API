using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMAK_AJWTAuthNetCore_Infra.Migrations
{
    /// <inheritdoc />
    public partial class M3040324 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginModels",
                table: "LoginModels");

            migrationBuilder.RenameTable(
                name: "LoginModels",
                newName: "LoggesInUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoggesInUsers",
                table: "LoggesInUsers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LoggesInUsers",
                table: "LoggesInUsers");

            migrationBuilder.RenameTable(
                name: "LoggesInUsers",
                newName: "LoginModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginModels",
                table: "LoginModels",
                column: "Id");
        }
    }
}
