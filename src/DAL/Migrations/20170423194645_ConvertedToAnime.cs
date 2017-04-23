using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_Database.Migrations
{
    public partial class ConvertedToAnime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.CreateTable(
                name: "UserAnimes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AnimeId = table.Column<int>(nullable: false),
                    SeenEpisodes = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserRating = table.Column<int>(nullable: false),
                    UserStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnimes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAnimes_AnimeId",
                table: "UserAnimes",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnimes_UserId",
                table: "UserAnimes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAnimes");

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Attack = table.Column<int>(nullable: false),
                    Defense = table.Column<int>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Hp = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PokedexId = table.Column<int>(nullable: false),
                    SpAttack = table.Column<int>(nullable: false),
                    SpDefense = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_PokedexId",
                table: "Pokemons",
                column: "PokedexId",
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }
    }
}
