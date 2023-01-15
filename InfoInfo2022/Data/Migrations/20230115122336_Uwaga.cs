using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace info_2022.Data.Migrations
{
    public partial class Uwaga : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uwaga",
                columns: table => new
                {
                    IdUwaga = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TekstUwaga = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rozpatrzone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uwaga", x => x.IdUwaga);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Uwaga");
        }
    }
}
