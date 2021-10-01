using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF_SGL_Dominio.Entidades;

namespace SF_SGL_Infra.ConfiguracaoEntidades.Sistema
{
    public class EntidadeSistemaConfig2 :IEntityTypeConfiguration<SistemaEntidade>
    {
        public void Configure(EntityTypeBuilder<SistemaEntidade> builder)
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
                .HasMaxLength(255)
                .IsRequired()
                .HasColumnName("nm_url_servico_consulta_log");

            builder.Property(s => s.UsuarioLogin)
                .HasMaxLength(200)
                .IsRequired()
                .HasColumnName("nm_usuario_servico_consulta_log");

            builder.Property(s => s.UsuarioSenha)
                .HasMaxLength(200)
                .IsRequired()
                .HasColumnName("nm_senha_servico_consulta_log");
        }
    }
}
