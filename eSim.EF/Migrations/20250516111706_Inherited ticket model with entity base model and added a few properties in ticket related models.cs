using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSim.EF.Migrations
{
    /// <inheritdoc />
    public partial class Inheritedticketmodelwithentitybasemodelandaddedafewpropertiesinticketrelatedmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "client",
                table: "TicketActivities",
                newName: "ActivityAt");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityId",
                schema: "client",
                table: "TicketAttachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "client",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                schema: "client",
                table: "Ticket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "client",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityId",
                schema: "client",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "client",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "client",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "client",
                table: "Ticket");

            migrationBuilder.RenameColumn(
                name: "ActivityAt",
                schema: "client",
                table: "TicketActivities",
                newName: "CreatedAt");
        }
    }
}
