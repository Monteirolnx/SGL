using Microsoft.EntityFrameworkCore.Migrations;

namespace SF.SGL.Infra.Data.Migrations
{
    public partial class ParametroExpurgo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nm_usuario_servico_consulta_log",
                table: "sistema",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "nm_url_servico_consulta_log",
                table: "sistema",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "nm_senha_servico_consulta_log",
                table: "sistema",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateTable(
                name: "parametro_expurgo_log",
                columns: table => new
                {
                    cd_parametro_expurgo_log = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pr_expurgo_log_operacao = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    pr_expurgo_log_auditoria = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    cd_sistema = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parametro_expurgo_log", x => x.cd_parametro_expurgo_log);
                    table.ForeignKey(
                        name: "FK_parametro_expurgo_log_sistema_cd_sistema",
                        column: x => x.cd_sistema,
                        principalTable: "sistema",
                        principalColumn: "cd_sistema",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_parametro_expurgo_log_cd_sistema",
                table: "parametro_expurgo_log",
                column: "cd_sistema");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parametro_expurgo_log");

            migrationBuilder.AlterColumn<string>(
                name: "nm_usuario_servico_consulta_log",
                table: "sistema",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "nm_url_servico_consulta_log",
                table: "sistema",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "nm_senha_servico_consulta_log",
                table: "sistema",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
