using System.Text.Json.Serialization;

namespace SF.SGL.Dominio.Entidades
{
    public class EntidadeParametroExpurgo
    {
        public  int Id { get; set; }
        
        public  int ParametroExpurgoLogOperacao { get; set; }

        public  int ParametroExpurgoLogAuditoria { get; set; }

        public int EntidadeSistemaId { get; set; }

        [JsonIgnore]
        public  EntidadeSistema EntidadeSistema { get; set; }
    }
}
