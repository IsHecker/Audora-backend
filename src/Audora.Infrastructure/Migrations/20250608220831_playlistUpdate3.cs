using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Audora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class playlistUpdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistEpisodes_Playlists_PlaylistId1",
                table: "PlaylistEpisodes");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistEpisodes_PlaylistId1",
                table: "PlaylistEpisodes");

            migrationBuilder.DropColumn(
                name: "PlaylistId1",
                table: "PlaylistEpisodes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlaylistId1",
                table: "PlaylistEpisodes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistEpisodes_PlaylistId1",
                table: "PlaylistEpisodes",
                column: "PlaylistId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistEpisodes_Playlists_PlaylistId1",
                table: "PlaylistEpisodes",
                column: "PlaylistId1",
                principalTable: "Playlists",
                principalColumn: "Id");
        }
    }
}
