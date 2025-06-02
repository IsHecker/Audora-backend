using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Audora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationshipsandnamings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowTarget",
                table: "Follows");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "Follows",
                newName: "ListenerId");

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "Follows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PodcastRatings_PodcastId",
                table: "PodcastRatings",
                column: "PodcastId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PodcastRatings_Podcasts_PodcastId",
                table: "PodcastRatings",
                column: "PodcastId",
                principalTable: "Podcasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PodcastRatings_Podcasts_PodcastId",
                table: "PodcastRatings");

            migrationBuilder.DropIndex(
                name: "IX_PodcastRatings_PodcastId",
                table: "PodcastRatings");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Follows");

            migrationBuilder.RenameColumn(
                name: "ListenerId",
                table: "Follows",
                newName: "FollowerId");

            migrationBuilder.AddColumn<byte>(
                name: "FollowTarget",
                table: "Follows",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
