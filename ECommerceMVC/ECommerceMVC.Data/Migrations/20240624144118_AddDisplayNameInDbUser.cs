using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayNameInDbUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "db_User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "db_User");
        }
    }
}
