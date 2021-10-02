using Microsoft.EntityFrameworkCore.Migrations;

namespace SF.SGL.Infra.Data.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sistema",
                columns: table => new
                {
                    cd_sistema = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nm_sistema = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    nm_url_servico_consulta_log = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    nm_usuario_servico_consulta_log = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    nm_senha_servico_consulta_log = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sistema", x => x.cd_sistema);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sistema");
        }
    }
}
