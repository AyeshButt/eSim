using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eSim.EF.Migrations
{
    /// <inheritdoc />
    public partial class ProfileTableInSubscribers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                schema: "client",
                table: "Subscribers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

           

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                schema: "client",
                table: "Subscribers");

          

            
        }
    }
}
