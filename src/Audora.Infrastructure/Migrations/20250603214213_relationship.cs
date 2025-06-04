using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Audora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EpisodeStats_PodcastId",
                table: "EpisodeStats",
                column: "PodcastId");

            migrationBuilder.AddForeignKey(
                name: "FK_EpisodeStats_Podcasts_PodcastId",
                table: "EpisodeStats",
                column: "PodcastId",
                principalTable: "Podcasts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EpisodeStats_Podcasts_PodcastId",
                table: "EpisodeStats");

            migrationBuilder.DropIndex(
                name: "IX_EpisodeStats_PodcastId",
                table: "EpisodeStats");
        }
    }
}
