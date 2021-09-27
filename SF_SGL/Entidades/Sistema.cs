using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SF_SGL.Entidades
{
    public class Sistema:IEntity
    {
        [Column("cd_sistema")]
        public int Id { get; set; }

        [Required]
        [Column("nm_sistema")]
        public string Nome { get; set; }

        [Required]
        [Column("nm_url_servico_consulta_log")]
        public string UrlServicoConsultaLog { get; set; }
        
        [Required]
        [Column("nm_usuario_servico_consulta_log")]
        public string UsuarioLogin { get; set; }

        [Required]
        [Column("nm_senha_servico_consulta_log")]
        public string UsuarioSenha { get; set; }
    }
}
