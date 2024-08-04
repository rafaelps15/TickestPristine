using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tickest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_REGRA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_REGRA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_AREA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SetorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AREA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_TB_AREA_AreaId",
                        column: x => x.AreaId,
                        principalTable: "TB_AREA",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TB_CHAMADO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFechamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Prioridade = table.Column<int>(type: "int", nullable: false),
                    SolicitanteId = table.Column<int>(type: "int", nullable: false),
                    AtendenteId = table.Column<int>(type: "int", nullable: false),
                    AnalistaId = table.Column<int>(type: "int", nullable: true),
                    AreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CHAMADO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_CHAMADO_TB_AREA_AreaId",
                        column: x => x.AreaId,
                        principalTable: "TB_AREA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_CHAMADO_TB_USUARIO_AnalistaId",
                        column: x => x.AnalistaId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_CHAMADO_TB_USUARIO_AtendenteId",
                        column: x => x.AtendenteId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_CHAMADO_TB_USUARIO_SolicitanteId",
                        column: x => x.SolicitanteId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_SETOR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResponsavelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_SETOR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_SETOR_TB_USUARIO_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO_REGRA",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    RegraId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO_REGRA", x => new { x.UsuarioId, x.RegraId });
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_REGRA_TB_REGRA_RegraId",
                        column: x => x.RegraId,
                        principalTable: "TB_REGRA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_REGRA_TB_USUARIO_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_MENSAGEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ChamadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MENSAGEM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_MENSAGEM_TB_CHAMADO_ChamadoId",
                        column: x => x.ChamadoId,
                        principalTable: "TB_CHAMADO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_MENSAGEM_TB_USUARIO_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_AREA_SetorId",
                table: "TB_AREA",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CHAMADO_AnalistaId",
                table: "TB_CHAMADO",
                column: "AnalistaId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CHAMADO_AreaId",
                table: "TB_CHAMADO",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CHAMADO_AtendenteId",
                table: "TB_CHAMADO",
                column: "AtendenteId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CHAMADO_SolicitanteId",
                table: "TB_CHAMADO",
                column: "SolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MENSAGEM_ChamadoId",
                table: "TB_MENSAGEM",
                column: "ChamadoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MENSAGEM_UsuarioId",
                table: "TB_MENSAGEM",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_SETOR_ResponsavelId",
                table: "TB_SETOR",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_AreaId",
                table: "TB_USUARIO",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_REGRA_RegraId",
                table: "TB_USUARIO_REGRA",
                column: "RegraId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_AREA_TB_SETOR_SetorId",
                table: "TB_AREA",
                column: "SetorId",
                principalTable: "TB_SETOR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_AREA_TB_SETOR_SetorId",
                table: "TB_AREA");

            migrationBuilder.DropTable(
                name: "TB_MENSAGEM");

            migrationBuilder.DropTable(
                name: "TB_USUARIO_REGRA");

            migrationBuilder.DropTable(
                name: "TB_CHAMADO");

            migrationBuilder.DropTable(
                name: "TB_REGRA");

            migrationBuilder.DropTable(
                name: "TB_SETOR");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_AREA");
        }
    }
}
