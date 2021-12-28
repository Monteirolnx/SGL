namespace SF.SGL.API.Infra.ConfiguracaoEntidades;

internal class ConfigEntidadeMonitoramento : IEntityTypeConfiguration<EntidadeMonitoramento>
{
    public void Configure(EntityTypeBuilder<EntidadeMonitoramento> builder)
    {
        builder.ToTable("monitoramento");

        builder
            .HasKey(m => m.Id);

        builder
            .Property(m => m.Id).UseIdentityColumn()
            .HasColumnName("cd_monitoramento");

        builder.Property(m => m.Guid)
            .IsRequired()
            .HasColumnName("gd_monitoramento");

        builder.Property(m => m.Nome)
            .HasMaxLength(100)
            .IsRequired()
            .HasColumnName("nm_monitoramento");

        builder.Property(m => m.Descricao)
            .HasMaxLength(255)
            .IsRequired()
            .HasColumnName("nm_monitoramento_descricao");

        builder.Property(m => m.Acao)
            .HasMaxLength(255)
            .IsRequired()
            .HasColumnName("nm_acao_contigencia");

        builder.Property(m => m.Contato)
            .IsRequired()
            .HasColumnName("nm_contato_responsavel");

        builder.Property(m => m.SistemaId)
          .HasColumnName("cd_sistema");

        builder
            .HasOne(m => m.EntidadeSistema)
            .WithMany()
            .HasForeignKey(m => m.SistemaId)
            .IsRequired();
    }
}
