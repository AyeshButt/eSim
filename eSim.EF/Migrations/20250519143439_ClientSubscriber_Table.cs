using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSim.EF.Migrations
{
    /// <inheritdoc />
    public partial class ClientSubscriber_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Settings",
            //    schema: "master",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        FixedCommission = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        PercentageCommission = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Settings", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                schema: "client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketStatus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "Open");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: "Close");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: "In-progress");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Status",
                value: "Waiting for reply");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketType",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "Bundle");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Payment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Settings",
            //    schema: "master");

            migrationBuilder.DropTable(
                name: "Subscribers",
                schema: "client");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketStatus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "open");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: "close");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: "in-progress");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Status",
                value: "waiting for reply");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketType",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "bundle");

            migrationBuilder.UpdateData(
                schema: "ref",
                table: "TicketType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "payment");
        }
    }
}
