using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Audora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class playlistUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistEpisodes",
                table: "PlaylistEpisodes");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistEpisodes_PlaylistId",
                table: "PlaylistEpisodes");

            migrationBuilder.AddColumn<Guid>(
                name: "PlaylistId1",
                table: "PlaylistEpisodes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistEpisodes",
                table: "PlaylistEpisodes",
                columns: new[] { "PlaylistId", "EpisodeId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistEpisodes_EpisodeId",
                table: "PlaylistEpisodes",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistEpisodes_PlaylistId1",
                table: "PlaylistEpisodes",
                column: "PlaylistId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistEpisodes_Episodes_EpisodeId",
                table: "PlaylistEpisodes",
                column: "EpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistEpisodes_Playlists_PlaylistId1",
                table: "PlaylistEpisodes",
                column: "PlaylistId1",
                principalTable: "Playlists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistEpisodes_Episodes_EpisodeId",
                table: "PlaylistEpisodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistEpisodes_Playlists_PlaylistId1",
                table: "PlaylistEpisodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistEpisodes",
                table: "PlaylistEpisodes");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistEpisodes_EpisodeId",
                table: "PlaylistEpisodes");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistEpisodes_PlaylistId1",
                table: "PlaylistEpisodes");

            migrationBuilder.DropColumn(
                name: "PlaylistId1",
                table: "PlaylistEpisodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistEpisodes",
                table: "PlaylistEpisodes",
                columns: new[] { "EpisodeId", "PlaylistId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistEpisodes_PlaylistId",
                table: "PlaylistEpisodes",
                column: "PlaylistId");
        }
    }
}
