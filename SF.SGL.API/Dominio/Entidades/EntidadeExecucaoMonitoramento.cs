namespace SF.SGL.API.Dominio.Entidades;

public class EntidadeExecucaoMonitoramento
{
    public long Id { get; set; }

    public Guid Guid { get; set; }

    public bool Status { get; set; }

    public DateTime Data { get; set; }

    public string Mensagem { get; set; }

    public int MonitoramentoId { get; set; }

    [JsonIgnore]
    public EntidadeMonitoramento EntidadeMonitoramento { get; set; }
}
