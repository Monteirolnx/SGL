using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SF.SGL.API.Migrations
{
    public partial class SGL01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nm_monitoramento ",
                schema: "dbo",
                table: "monitoramento",
                newName: "nm_monitoramento");

            migrationBuilder.RenameColumn(
                name: "cd_monitoramento ",
                schema: "dbo",
                table: "monitoramento",
                newName: "cd_monitoramento");

            migrationBuilder.CreateTable(
                name: "execucao_monitoramento",
                schema: "dbo",
                columns: table => new
                {
                    cd_execucao_monitoramento = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nm_execucao_monitoramento_status = table.Column<bool>(type: "bit", nullable: false),
                    dt_execucao_monitoramento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nm_execucao_monitoramento_mensagem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    cd_monitoramento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_execucao_monitoramento", x => x.cd_execucao_monitoramento);
                    table.ForeignKey(
                        name: "FK_execucao_monitoramento_monitoramento_cd_monitoramento",
                        column: x => x.cd_monitoramento,
                        principalSchema: "dbo",
                        principalTable: "monitoramento",
                        principalColumn: "cd_monitoramento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_execucao_monitoramento_cd_monitoramento",
                schema: "dbo",
                table: "execucao_monitoramento",
                column: "cd_monitoramento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "execucao_monitoramento",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "nm_monitoramento",
                schema: "dbo",
                table: "monitoramento",
                newName: "nm_monitoramento ");

            migrationBuilder.RenameColumn(
                name: "cd_monitoramento",
                schema: "dbo",
                table: "monitoramento",
                newName: "cd_monitoramento ");
        }
    }
}
