namespace SF.SGL.API.Infra.ConfiguracaoEntidades;

internal class ConfigEntidadeParametroExpurgo : IEntityTypeConfiguration<EntidadeParametroExpurgo>
{
    public void Configure(EntityTypeBuilder<EntidadeParametroExpurgo> builder)
    {
        builder.ToTable("parametro_expurgo_log");

        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Id).UseIdentityColumn()
            .HasColumnName("cd_parametro_expurgo_log");

        builder.Property(p => p.SistemaId)
          .HasColumnName("cd_sistema");

        builder
            .HasOne(p => p.EntidadeSistema)
            .WithMany()
            .HasForeignKey(p => p.SistemaId)
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
