using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SF.SGL.API.Dominio.Entidades;

namespace SF.SGL.API.Infra.ConfiguracaoEntidades
{
    internal class ConfigEntidadeParametroExpurgo : IEntityTypeConfiguration<EntidadeParametroExpurgo>
    {
        public void Configure(EntityTypeBuilder<EntidadeParametroExpurgo> builder)
        {
            builder.ToTable("parametro_expurgo_log");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasColumnName("cd_parametro_expurgo_log");

            builder.Property(p => p.EntidadeSistemaId)
              .HasColumnName("cd_sistema");

            builder
                .HasOne(p => p.EntidadeSistema)
                .WithMany()
                .HasForeignKey(p => p.EntidadeSistemaId)
                .IsRequired();

            builder
                .Property(p => p.ParametroExpurgoLogAuditoria)
                .HasMaxLength(3)
                .IsRequired()
                .HasColumnName("pr_expurgo_log_auditoria");

            builder
                .Property(p => p.ParametroExpurgoLogOperacao)
                .HasMaxLength(3)
                .IsRequired()
                .HasColumnName("pr_expurgo_log_operacao");
        }
    }
}
