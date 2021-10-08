using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.SGL.Dominio.Entidades;

namespace SF.SGL.Infra.Data.ConfiguracaoEntidades
{
    internal class ConfigEntidadeSistema : IEntityTypeConfiguration<EntidadeSistema>
    {
        public void Configure(EntityTypeBuilder<EntidadeSistema> builder)
        {
            builder.ToTable("sistema");

            builder
                .HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("cd_sistema");

            builder.Property(s => s.Nome)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("nm_sistema");

            builder.Property(s => s.UrlServicoConsultaLog)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnName("nm_url_servico_consulta_log");

            builder.Property(s => s.UsuarioLogin)
                .HasMaxLength(10)
                .IsRequired()
                .HasColumnName("nm_usuario_servico_consulta_log");

            builder.Property(s => s.UsuarioSenha)
                .HasMaxLength(10)
                .IsRequired()
                .HasColumnName("nm_senha_servico_consulta_log");
        }
    }
}
