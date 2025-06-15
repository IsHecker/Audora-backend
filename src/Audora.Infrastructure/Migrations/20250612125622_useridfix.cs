using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Audora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class useridfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ListenerId",
                table: "Reactions",
                column: "ListenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Podcasts_CreatorId",
                table: "Podcasts",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PodcastRatings_ListenerId",
                table: "PodcastRatings",
                column: "ListenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_ListenerId",
                table: "Playlists",
                column: "ListenerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaybackSessions_ListenerId",
                table: "PlaybackSessions",
                column: "ListenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_ListenerId",
                table: "Follows",
                column: "ListenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ListenerId",
                table: "Comments",
                column: "ListenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ListenerId",
                table: "Comments",
                column: "ListenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_ListenerId",
                table: "Follows",
                column: "ListenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaybackSessions_AspNetUsers_ListenerId",
                table: "PlaybackSessions",
                column: "ListenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_AspNetUsers_ListenerId",
                table: "Playlists",
                column: "ListenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PodcastRatings_AspNetUsers_ListenerId",
                table: "PodcastRatings",
                column: "ListenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Podcasts_AspNetUsers_CreatorId",
                table: "Podcasts",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_AspNetUsers_ListenerId",
                table: "Reactions",
                column: "ListenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ListenerId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_ListenerId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaybackSessions_AspNetUsers_ListenerId",
                table: "PlaybackSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_AspNetUsers_ListenerId",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_PodcastRatings_AspNetUsers_ListenerId",
                table: "PodcastRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_Podcasts_AspNetUsers_CreatorId",
                table: "Podcasts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_AspNetUsers_ListenerId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_ListenerId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Podcasts_CreatorId",
                table: "Podcasts");

            migrationBuilder.DropIndex(
                name: "IX_PodcastRatings_ListenerId",
                table: "PodcastRatings");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_ListenerId",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_PlaybackSessions_ListenerId",
                table: "PlaybackSessions");

            migrationBuilder.DropIndex(
                name: "IX_Follows_ListenerId",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ListenerId",
                table: "Comments");
        }
    }
}
