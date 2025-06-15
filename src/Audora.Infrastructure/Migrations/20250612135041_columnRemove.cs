using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Audora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class columnRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewColumnForTestingFuck",
                table: "Podcasts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewColumnForTestingFuck",
                table: "Podcasts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
