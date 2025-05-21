using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eSim.EF.Migrations
{
    /// <inheritdoc />
    public partial class AspNetUsersTypeModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryEmail",
                schema: "master",
                table: "Clients",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetUsersType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsersType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsersType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Developer" },
                    { 2, "Superadmin" },
                    { 3, "Subadmin" },
                    { 4, "Client" },
                    { 5, "Subclient" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUsersType");

            migrationBuilder.DropColumn(
                name: "PrimaryEmail",
                schema: "master",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");
        }
    }
}
