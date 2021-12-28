namespace SF.SGL.API.Infra.ConfiguracaoEntidades;

internal class ConfigEntidadeExecucaoMonitoramento : IEntityTypeConfiguration<EntidadeExecucaoMonitoramento>
{
    public void Configure(EntityTypeBuilder<EntidadeExecucaoMonitoramento> builder)
    {
        builder.ToTable("execucao_monitoramento");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .UseIdentityColumn()
            .HasColumnName("cd_execucao_monitoramento");

        builder
            .Property(e => e.Guid)
            .IsRequired()
            .IsConcurrencyToken()
            .HasColumnName("gd_guid");

        builder
            .Property(e => e.Data)
            .IsRequired()
            .HasColumnName("dt_execucao_monitoramento");

        builder
            .Property(e => e.Status)
            .IsRequired()
            .HasColumnName("nm_execucao_monitoramento_status");

        builder
            .Property(e => e.Mensagem)
            .HasMaxLength(100)
            .HasColumnName("nm_execucao_monitoramento_mensagem");

        builder.Property(e => e.MonitoramentoId)
            .HasColumnName("cd_monitoramento");

        builder
        .HasOne(e => e.EntidadeMonitoramento)
        .WithMany()
        .HasForeignKey(m => m.MonitoramentoId)
        .IsRequired();
    }
}
