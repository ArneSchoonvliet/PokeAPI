using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL_Database.Migrations
{
    public partial class AddedClusteredIndexOnPokemonEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropPrimaryKey("PK_Pokemons", "Pokemons");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_PokedexId",
                table: "Pokemons",
                column: "PokedexId",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddPrimaryKey("PK_Pokemons", "Pokemons", "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // TODO
        }
    }
}
