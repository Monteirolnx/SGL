using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SF.SGL.API.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            //migrationBuilder.CreateTable(
            //    name: "sistema",
            //    schema: "dbo",
            //    columns: table => new
            //    {
            //        cd_sistema = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        nm_sistema = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        nm_url_servico_consulta_log = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            //        nm_usuario_servico_consulta_log = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //        nm_senha_servico_consulta_log = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_sistema", x => x.cd_sistema);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "monitoramento",
            //    schema: "dbo",
            //    columns: table => new
            //    {
            //        cd_monitoramento = table.Column<int>(name: "cd_monitoramento ", type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        gd_monitoramento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        nm_monitoramento = table.Column<string>(name: "nm_monitoramento ", type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        nm_monitoramento_descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        nm_acao_contigencia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        nm_contato_responsavel = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        cd_sistema = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_monitoramento", x => x.cd_monitoramento);
            //        table.ForeignKey(
            //            name: "FK_monitoramento_sistema_cd_sistema",
            //            column: x => x.cd_sistema,
            //            principalSchema: "dbo",
            //            principalTable: "sistema",
            //            principalColumn: "cd_sistema",
            //            onDelete: ReferentialAction.Cascade);
            //    });

        //    migrationBuilder.CreateTable(
        //        name: "parametro_expurgo_log",
        //        schema: "dbo",
        //        columns: table => new
        //        {
        //            cd_parametro_expurgo_log = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            pr_expurgo_log_operacao = table.Column<int>(type: "int", maxLength: 3, nullable: false),
        //            pr_expurgo_log_auditoria = table.Column<int>(type: "int", maxLength: 3, nullable: false),
        //            cd_sistema = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_parametro_expurgo_log", x => x.cd_parametro_expurgo_log);
        //            table.ForeignKey(
        //                name: "FK_parametro_expurgo_log_sistema_cd_sistema",
        //                column: x => x.cd_sistema,
        //                principalSchema: "dbo",
        //                principalTable: "sistema",
        //                principalColumn: "cd_sistema",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_monitoramento_cd_sistema",
        //        schema: "dbo",
        //        table: "monitoramento",
        //        column: "cd_sistema");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_parametro_expurgo_log_cd_sistema",
        //        schema: "dbo",
        //        table: "parametro_expurgo_log",
        //        column: "cd_sistema");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "monitoramento",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "parametro_expurgo_log",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "sistema",
                schema: "dbo");
        }
    }
}
