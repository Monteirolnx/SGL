using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SF.SGL.API.Migrations
{
    public partial class SGL02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guid",
                schema: "dbo",
                table: "execucao_monitoramento",
                newName: "gd_guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "gd_guid",
                schema: "dbo",
                table: "execucao_monitoramento",
                newName: "Guid");
        }
    }
}
