using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eSim.EF.Migrations
{
    /// <inheritdoc />
    public partial class Addedticketrelatedmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ref");

            migrationBuilder.CreateTable(
                name: "Ticket",
                schema: "client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TRN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketActivities",
                schema: "client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketAttachments",
                schema: "client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CommentType = table.Column<int>(type: "int", nullable: false),
                    IsVisibleToCustomer = table.Column<bool>(type: "bit", nullable: false),
                    ActivityBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketAttachmentType",
                schema: "ref",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAttachmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketCommentType",
                schema: "ref",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCommentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatus",
                schema: "ref",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                schema: "ref",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "ref",
                table: "TicketAttachmentType",
                columns: new[] { "Id", "AttachmentType" },
                values: new object[,]
                {
                    { 1, "Internal" },
                    { 2, "External" }
                });

            migrationBuilder.InsertData(
                schema: "ref",
                table: "TicketCommentType",
                columns: new[] { "Id", "CommentType" },
                values: new object[,]
                {
                    { 1, "Customer" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                schema: "ref",
                table: "TicketStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "open" },
                    { 2, "close" },
                    { 3, "in-progress" },
                    { 4, "waiting for reply" }
                });

            migrationBuilder.InsertData(
                schema: "ref",
                table: "TicketType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "bundle" },
                    { 2, "payment" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticket",
                schema: "client");

            migrationBuilder.DropTable(
                name: "TicketActivities",
                schema: "client");

            migrationBuilder.DropTable(
                name: "TicketAttachments",
                schema: "client");

            migrationBuilder.DropTable(
                name: "TicketAttachmentType",
                schema: "ref");

            migrationBuilder.DropTable(
                name: "TicketCommentType",
                schema: "ref");

            migrationBuilder.DropTable(
                name: "TicketStatus",
                schema: "ref");

            migrationBuilder.DropTable(
                name: "TicketType",
                schema: "ref");
        }
    }
}
