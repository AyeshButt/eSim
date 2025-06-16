using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSim.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddedEsimModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppliedEsimBundles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Iccid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BundleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedEsimBundles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Esims",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubscriberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Iccid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerRef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Physical = table.Column<bool>(type: "bit", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Esims", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppliedEsimBundles");

            migrationBuilder.DropTable(
                name: "Esims");
        }
    }
}
