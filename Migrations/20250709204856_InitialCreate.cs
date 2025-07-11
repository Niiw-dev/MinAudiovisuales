using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinAudiovisual.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devocionales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tema = table.Column<string>(type: "TEXT", nullable: false),
                    Versiculos = table.Column<string>(type: "TEXT", nullable: false),
                    Promesa = table.Column<string>(type: "TEXT", nullable: false),
                    Practica = table.Column<string>(type: "TEXT", nullable: false),
                    Documento = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devocionales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    NombreEvento = table.Column<string>(type: "TEXT", nullable: true),
                    FechaHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NombreResponsable = table.Column<string>(type: "TEXT", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: false),
                    Mejoras = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetallesServicio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ServicioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrdenCanciones = table.Column<string>(type: "TEXT", nullable: false),
                    HayAnuncios = table.Column<bool>(type: "INTEGER", nullable: false),
                    Anuncios = table.Column<string>(type: "TEXT", nullable: true),
                    Predica = table.Column<string>(type: "TEXT", nullable: false),
                    CancionMinistracion = table.Column<string>(type: "TEXT", nullable: false),
                    HaySantaCena = table.Column<bool>(type: "INTEGER", nullable: false),
                    CancionSantaCena = table.Column<string>(type: "TEXT", nullable: true),
                    ObservacionesAdicionales = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesServicio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesServicio_Servicios_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesServicio_ServicioId",
                table: "DetallesServicio",
                column: "ServicioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesServicio");

            migrationBuilder.DropTable(
                name: "Devocionales");

            migrationBuilder.DropTable(
                name: "Servicios");
        }
    }
}
