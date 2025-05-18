using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eSim.EF.Migrations
{
    /// <inheritdoc />
    public partial class Renamedticketactivitiesandattachmentmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                schema: "client",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "client",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "IsVisibleToCustomer",
                schema: "client",
                table: "TicketAttachments");

            migrationBuilder.RenameColumn(
                name: "CommentType",
                schema: "client",
                table: "TicketAttachments",
                newName: "AttachmentType");

            migrationBuilder.RenameColumn(
                name: "ActivityBy",
                schema: "client",
                table: "TicketAttachments",
                newName: "Attachment");

            migrationBuilder.RenameColumn(
                name: "AttachmentType",
                schema: "client",
                table: "TicketActivities",
                newName: "CommentType");

            migrationBuilder.RenameColumn(
                name: "Attachment",
                schema: "client",
                table: "TicketActivities",
                newName: "ActivityBy");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                schema: "client",
                table: "TicketActivities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "client",
                table: "TicketActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleToCustomer",
                schema: "client",
                table: "TicketActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                schema: "client",
                table: "TicketActivities");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "client",
                table: "TicketActivities");

            migrationBuilder.DropColumn(
                name: "IsVisibleToCustomer",
                schema: "client",
                table: "TicketActivities");

            migrationBuilder.RenameColumn(
                name: "AttachmentType",
                schema: "client",
                table: "TicketAttachments",
                newName: "CommentType");

            migrationBuilder.RenameColumn(
                name: "Attachment",
                schema: "client",
                table: "TicketAttachments",
                newName: "ActivityBy");

            migrationBuilder.RenameColumn(
                name: "CommentType",
                schema: "client",
                table: "TicketActivities",
                newName: "AttachmentType");

            migrationBuilder.RenameColumn(
                name: "ActivityBy",
                schema: "client",
                table: "TicketActivities",
                newName: "Attachment");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                schema: "client",
                table: "TicketAttachments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "client",
                table: "TicketAttachments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleToCustomer",
                schema: "client",
                table: "TicketAttachments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
