using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSim.EF.Migrations
{
    /// <inheritdoc />
    public partial class RemovedInventoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryAvailableBundles");

            migrationBuilder.DropTable(
                name: "InventoryBundleAllowances");

            migrationBuilder.DropTable(
                name: "InventoryBundleCountries");

            migrationBuilder.DropTable(
                name: "InventoryBundles");

            migrationBuilder.DropTable(
                name: "InventoryBundleSpeeds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryAvailableBundles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailableId = table.Column<int>(type: "int", nullable: false),
                    Expiry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InventoryBundleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remaining = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAvailableBundles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryBundleAllowances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InventoryBundleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unlimited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBundleAllowances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryBundleCountries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryBundleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBundleCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryBundles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Autostart = table.Column<bool>(type: "bit", nullable: false),
                    Data = table.Column<int>(type: "int", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    DurationUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Unlimited = table.Column<bool>(type: "bit", nullable: false),
                    UseDms = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBundles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryBundleSpeeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryBundleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Speed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBundleSpeeds", x => x.Id);
                });
        }
    }
}
