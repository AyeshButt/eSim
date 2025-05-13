using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSim.EF.Migrations
{
    /// <inheritdoc />
    public partial class ClientsAddKid_Secert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "master",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "master",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Kid",
                schema: "master",
                table: "Clients",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Secret",
                schema: "master",
                table: "Clients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "master",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Kid",
                schema: "master",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Secret",
                schema: "master",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "master",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
