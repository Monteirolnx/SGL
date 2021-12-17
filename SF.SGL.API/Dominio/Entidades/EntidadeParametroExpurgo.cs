namespace SF.SGL.API.Dominio.Entidades;

public class EntidadeParametroExpurgo
{
    public int Id { get; set; }

    public int ParametroExpurgoLogOperacao { get; set; }

    public int ParametroExpurgoLogAuditoria { get; set; }

    public int SistemaId { get; set; }

    [JsonIgnore]
    public EntidadeSistema EntidadeSistema { get; set; }
}
